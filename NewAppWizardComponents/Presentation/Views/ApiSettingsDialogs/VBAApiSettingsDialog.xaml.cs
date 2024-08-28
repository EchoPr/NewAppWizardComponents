using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using WinRT.Interop;

using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using System.Text;
using Newtonsoft.Json.Linq;
using QFormAPI;
using Uno.Extensions;
using Windows.Web.Syndication;

namespace NewAppWizardComponents;
public sealed partial class VBAApiSettingsDialog : ContentDialog
{
    public MainPageVM mainPageVM;
    public VBAApiSettingsDialogVM vbaVM;
    private ApiEntry _snippetEntry;

    private ExcelHelper _excelHelper;

    public VBAApiSettingsDialog(MainPageVM vm, ApiEntry? entry = null)
    {
        this.InitializeComponent();

        vbaVM = new VBAApiSettingsDialogVM();
        this.DataContext = vbaVM;

        mainPageVM = vm;

        _excelHelper = new ExcelHelper(mainPageVM);
    }

    

    private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        _excelHelper.SetMacrosAccessibility(false);
    }

    private void ContentDialog_Loaded(object sender, RoutedEventArgs e)
    {
        InstallButton.Content = $"Install QForm API {mainPageVM.qformManager.qformVersion}";

        if (ApiVersions.Items.Count == 0)
        {
            SetAccessibilityButtons(false);
        }
    }

    private void SetAccessibilityButtons(bool val)
    {
        AdditionWay.Children.ForEach(item => { if (item is CheckBox i) i.IsEnabled = val; });
        ErrorHandling.Children.ForEach(item => { if (item is RadioButton i) i.IsEnabled = val; });
        ProjectActions.Children.ForEach(item => { if (item is Button i) i.IsEnabled = val; });
    }

    private void InstallButton_Click(object sender, RoutedEventArgs e)
    {
        var process = Process.Start(Path.Combine(mainPageVM.qformManager.QFormBaseDir, "API\\App\\Excel\\QFormApiCom.exe"));

        process.WaitForExit();
        vbaVM.ComLibraries.Clear();
        vbaVM.LoadComLibraries("QFormApiCom");
        if (ApiVersions.Items.Count != 0)
        {
            SetAccessibilityButtons(true);
        }
    }

    private void UninstallButton_Click(object sender, RoutedEventArgs e)
    {
        string ver = ApiVersions.SelectedValue.ToString().Split(" ")[2];

        vbaVM.UninstallQFormApi(ver);

        if (ApiVersions.Items.Count == 0)
        {
            SetAccessibilityButtons(false);
        }
        UninstallButton.IsEnabled = false;

    }

    private void ApiVersions_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        UninstallButton.IsEnabled = true;
    }

    private async void CreateNewProject_Click(object sender, RoutedEventArgs e)
    {
        StorageFile file = await mainPageVM.projectManager.SaveFile(new Tuple<string, string>("Excel", ".xlsm"));

        if (file != null)
        {
            FileInfo fileInfo = new FileInfo(file.Path);
            fileInfo.IsReadOnly = false;

            string res = _excelHelper.CreateNewProject(file.Path);
            StatusText.Text = res;
        }

        //_excelHelper.CreateNewProject("NewWorkBook");


    }

    private async void EditExistingProject_Click(object sender, RoutedEventArgs e)
    {
        StorageFile file = await mainPageVM.projectManager.SelectFile(".xlsm", ".xlsx");

        if (file != null)
        {
            string res = _excelHelper.EditExistingProject(file.Path);
            StatusText.Text = res;
        }
    }
}

public class ExcelHelper
{
    public MainPageVM mainPageVM;

    private string resultFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "QFormAppWizard", "result.txt");

    public ExcelHelper(MainPageVM vm)
    {
        mainPageVM = vm;
    }

    public string CreateNewProject(string fileName)
    {
        string result = "";

        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }

        SetMacrosAccessibility(true);
        Type xType = Type.GetTypeFromProgID("Excel.Application");
        if (xType == null)
        {
            return "Excel could not be found.";
        }

        dynamic excelApp = null;
        dynamic workbooks = null;
        dynamic workbook = null;

        try
        {
            excelApp = Activator.CreateInstance(xType);
            excelApp.Visible = false;
            workbooks = excelApp.Workbooks;
            workbook = workbooks.Add();

            // Удаление существующего модуля QFormSvc, если он есть
            RemoveExistingModule(workbook, "QFormSvc");

            // Добавление нового модуля QFormSvc
            dynamic vbaModule = workbook.VBProject.VBComponents.Add(1); // 1 for vbext_ComponentType.vbext_ct_StdModule
            vbaModule.Name = "QFormSvc";

            vbaModule.CodeModule.AddFromString(GetVBASnippet(excelApp.OperatingSystem.Contains("64") ? "64" : "86"));

            System.Threading.Thread.Sleep(1000);

            // Удаление всех существующих ссылок на QForm
            RemoveExistingReferences(workbook);

            // Запуск процедуры и получение результата
            excelApp.Run("AddReference");

            if (File.Exists(resultFilePath))
            {
                result = File.ReadAllText(resultFilePath);

                File.WriteAllText(resultFilePath, string.Empty);
            }

            workbook.SaveAs(fileName, 52); // 52 for xlOpenXMLWorkbookMacroEnabled

            workbook.Close(true);
        }
        catch (COMException ex)
        {
            return "Error creating new Excel project: " + ex.Message;
        }
        finally
        {
            // Освобождаем объекты в обратном порядке
            if (workbook != null) Marshal.ReleaseComObject(workbook);
            if (workbooks != null) Marshal.ReleaseComObject(workbooks);
            if (excelApp != null)
            {
                excelApp.Quit();
                Marshal.ReleaseComObject(excelApp);
            }
        }

        SetMacrosAccessibility(false);
        return result;
    }

    public string EditExistingProject(string fileName)
    {
        string result = "";

        SetMacrosAccessibility(true);
        Type xType = Type.GetTypeFromProgID("Excel.Application");
        if (xType == null)
        {
            return "Excel could not be found";
        }

        dynamic excelApp = null;
        dynamic workbooks = null;
        dynamic workbook = null;

        try
        {
            if (IsFileLocked(new FileInfo(fileName)))
            {
                return "The file is already opened by another process or user.";
            }

            excelApp = Activator.CreateInstance(xType);
            excelApp.Visible = false;
            workbooks = excelApp.Workbooks;
            workbook = workbooks.Open(fileName, ReadOnly: false);

            // Удаление существующего модуля QFormSvc, если он есть
            RemoveExistingModule(workbook, "QFormSvc");

            // Добавление нового модуля QFormSvc
            dynamic vbaModule = workbook.VBProject.VBComponents.Add(1); // 1 for vbext_ComponentType.vbext_ct_StdModule
            vbaModule.Name = "QFormSvc";

            vbaModule.CodeModule.AddFromString(GetVBASnippet(excelApp.OperatingSystem.Contains("64") ? "64" : "86"));

            System.Threading.Thread.Sleep(1000);

            // Удаление всех существующих ссылок на QForm
            RemoveExistingReferences(workbook);

            excelApp.Run("AddReference");

            if (File.Exists(resultFilePath))
            {
                result = File.ReadAllText(resultFilePath);

                File.WriteAllText(resultFilePath, string.Empty);
            }

            workbook.Save();

            workbook.Close(true);

            // Установка файла с возможностью редактирования
            FileInfo fileInfo = new FileInfo(fileName);
            fileInfo.IsReadOnly = false;
        }
        catch (COMException ex)
        {
            return "Error editing Excel project: " + ex.Message;
        }
        finally
        {
            // Освобождаем объекты в обратном порядке
            if (workbook != null) Marshal.ReleaseComObject(workbook);
            if (workbooks != null) Marshal.ReleaseComObject(workbooks);
            if (excelApp != null)
            {
                excelApp.Quit();
                Marshal.ReleaseComObject(excelApp);
            }
        }

        SetMacrosAccessibility(false);

        return result;
    }

    private bool IsFileLocked(FileInfo file)
    {
        FileStream? stream = null;

        try
        {
            // Открываем файл с режимом только для чтения и эксклюзивным доступом
            stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
        }
        catch (IOException)
        {
            // Если возникает ошибка ввода-вывода, значит файл заблокирован
            return true;
        }
        finally
        {
            // Закрываем поток если файл был открыт
            stream?.Close();
        }

        // Если исключений не было, файл не заблокирован
        return false;
    }

    private void RemoveExistingModule(dynamic workbook, string moduleName)
    {
        foreach (dynamic vbaComponent in workbook.VBProject.VBComponents)
        {
            if (vbaComponent.Name == moduleName)
            {
                workbook.VBProject.VBComponents.Remove(vbaComponent);
                break;
            }
        }
    }

    private void RemoveExistingReferences(dynamic workbook)
    {
        var refCollection = workbook.VBProject.References;
        for (int i = refCollection.Count; i >= 1; i--)
        {
            var reference = refCollection.Item(i);
            if (reference.Description.Contains("QForm"))
            {
                try
                {
                    refCollection.Remove(reference);
                }
                catch (COMException ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }
    }


    public void SetMacrosAccessibility(bool value)
    {
        string? officeVersion = GetInstalledOfficeVersion();

        if (officeVersion == null)
            throw new Exception("MS Excel is not installed");

        string registryKeyPath = $@"Software\Microsoft\Office\{officeVersion}\Excel\Security";

        try
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(registryKeyPath, true))
            {
                if (key != null)
                {
                    key.SetValue("AccessVBOM", value ? 1 : 0, RegistryValueKind.DWord);
                }
                else
                {
                    throw new Exception("Не удалось найти указанный ключ реестра.");
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Ошибка при попытке изменения реестра: {ex.Message}");
        }
    }

    private string? GetInstalledOfficeVersion()
    {
        string[] officeVersions = { "16.0", "15.0", "14.0", "12.0", "11.0" };
        string baseRegistryPath = @"Software\Microsoft\Office\";

        foreach (string version in officeVersions)
        {
            string registryPath = $@"{baseRegistryPath}{version}\Excel";
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryPath) ??
                                    Registry.CurrentUser.OpenSubKey(registryPath))
            {
                if (key != null)
                {
                    return version;
                }
            }
        }

        return null;
    }
    private string GetVBASnippet(string bitDepth)
    {
        return $@"
Sub AddReference()
    Dim Ref As Object
    Dim filePath As String
    filePath = Environ(""LocalAppData"") & ""\QFormAppWizard\result.txt""
    
    If Dir(Environ(""LocalAppData"") & ""\QFormAppWizard"", vbDirectory) = """" Then
        MkDir Environ(""LocalAppData"") & ""\QFormAppWizard""
    End If
    
    Set Ref = ThisWorkbook.VBProject.References
    On Error Resume Next
     Ref.AddFromFile ""{Path.Combine(mainPageVM.qformManager.QFormBaseDir, $"..\\QFormApiCom_{mainPageVM.qformManager.qformVersion}\\x{bitDepth}\\QFormAPI.tlb")}""

    Dim resultText As String
    If Err.Number <> 0 Then
        resultText = ""Error: Library not found or could not be added.""
    Else
        resultText = ""Success""
    End If
    
    Dim fileNum As Integer
    fileNum = FreeFile
    Open filePath For Output As #fileNum
    Print #fileNum, resultText
    Close #fileNum
    
    On Error GoTo 0
End Sub
";
    }

}
