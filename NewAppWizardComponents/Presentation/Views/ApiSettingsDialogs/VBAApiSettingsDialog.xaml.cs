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
            _excelHelper.CreateNewProject(file.Path);
        }

        //_excelHelper.CreateNewProject("NewWorkBook");


    }

    private async void EditExistingProject_Click(object sender, RoutedEventArgs e)
    {
        StorageFile file = await mainPageVM.projectManager.SelectFile(".xlsm", ".xlsx");

        if (file != null)
        {
            _excelHelper.EditExistingProject(file.Path);
        }
    }
}

public class ExcelHelper
{
    public MainPageVM mainPageVM;

    private string logFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "QFormAppWizard", "Logs");
    
    public ExcelHelper(MainPageVM vm)
    {
        mainPageVM = vm;
        Directory.CreateDirectory(logFolderPath);
    }

    public void CreateNewProject(string fileName)
    {
        StringBuilder logBuilder = new StringBuilder();
        logBuilder.AppendLine($"=== Project Creation Log - {DateTime.Now} ===");

        SetMacrosAccessibility(true);
        Excel.Application excelApp = new Excel.Application();

        try
        {
            Excel.Workbook workbook = excelApp.Workbooks.Add();
            LogWithTimestamp(logBuilder, "New Excel workbook created.");

            // Удаление существующего модуля QFormSvc, если он есть
            RemoveExistingModule(workbook, "QFormSvc");

            // Добавление нового модуля QFormSvc
            Microsoft.Vbe.Interop.VBComponent vbaModule = workbook.VBProject.VBComponents.Add(Microsoft.Vbe.Interop.vbext_ComponentType.vbext_ct_StdModule);
            vbaModule.Name = "QFormSvc";
            LogWithTimestamp(logBuilder, "New module 'QFormSvc' added to workbook.");

            vbaModule.CodeModule.AddFromString(GetVBASnippet(excelApp.OperatingSystem.Contains("64") ? "64" : "86"));
            LogWithTimestamp(logBuilder, "VBA code added to module 'QFormSvc'.");

            System.Threading.Thread.Sleep(1000);

            // Удаление всех существующих ссылок на QForm
            RemoveExistingReferences(workbook);

            excelApp.Run("AddReference");
            LogWithTimestamp(logBuilder, "Procedure 'AddReference' executed.");

            workbook.SaveAs(fileName, Excel.XlFileFormat.xlOpenXMLWorkbookMacroEnabled);
            LogWithTimestamp(logBuilder, $"Workbook saved as '{fileName}' with macro-enabled format.");

            workbook.Close(true);
            LogWithTimestamp(logBuilder, "Workbook closed.");
        }
        catch (COMException ex)
        {
            LogWithTimestamp(logBuilder, "Error creating new Excel project: " + ex.Message);
        }
        finally
        {
            excelApp.Quit();
            Marshal.ReleaseComObject(excelApp);
        }

        SetMacrosAccessibility(false);

        logBuilder.AppendLine("=======================================");
        string logFilePath = Path.Combine(logFolderPath, "LogAppWizard.txt");
        LogToFile(logFilePath, logBuilder.ToString());
    }

    public void EditExistingProject(string fileName)
    {
        StringBuilder logBuilder = new StringBuilder();
        logBuilder.AppendLine($"=== Project Editing Log - {DateTime.Now} ===");

        SetMacrosAccessibility(true);
        Excel.Application excelApp = new Excel.Application();

        try
        {
            Excel.Workbook workbook = excelApp.Workbooks.Open(fileName, ReadOnly: false);
            LogWithTimestamp(logBuilder, $"Workbook '{fileName}' opened.");

            // Удаление существующего модуля QFormSvc, если он есть
            RemoveExistingModule(workbook, "QFormSvc");

            // Добавление нового модуля QFormSvc
            Microsoft.Vbe.Interop.VBComponent vbaModule = workbook.VBProject.VBComponents.Add(Microsoft.Vbe.Interop.vbext_ComponentType.vbext_ct_StdModule);
            vbaModule.Name = "QFormSvc";
            LogWithTimestamp(logBuilder, "New module 'QFormSvc' added to workbook.");

            vbaModule.CodeModule.AddFromString(GetVBASnippet(excelApp.OperatingSystem.Contains("64") ? "64" : "86"));
            LogWithTimestamp(logBuilder, "VBA code added to module 'QFormSvc'.");

            System.Threading.Thread.Sleep(1000);

            // Удаление всех существующих ссылок на QForm
            RemoveExistingReferences(workbook);

            excelApp.Run("AddReference");
            LogWithTimestamp(logBuilder, "Procedure 'AddReference' executed.");

            workbook.Save();
            LogWithTimestamp(logBuilder, $"Workbook '{fileName}' saved.");

            workbook.Close(true);
            LogWithTimestamp(logBuilder, "Workbook closed.");

            // Установка файла с возможностью редактирования
            FileInfo fileInfo = new FileInfo(fileName);
            fileInfo.IsReadOnly = false;
            LogWithTimestamp(logBuilder, $"File '{fileName}' set to be writable.");
        }
        catch (COMException ex)
        {
            LogWithTimestamp(logBuilder, "Error editing Excel project: " + ex.Message);
        }
        finally
        {
            excelApp.Quit();
            Marshal.ReleaseComObject(excelApp);
        }

        SetMacrosAccessibility(false);

        logBuilder.AppendLine("=======================================");
        string logFilePath = Path.Combine(logFolderPath, "LogAppWizard.txt");
        LogToFile(logFilePath, logBuilder.ToString());
    }

    private void RemoveExistingModule(Excel.Workbook workbook, string moduleName)
    {
        foreach (Microsoft.Vbe.Interop.VBComponent vbaComponent in workbook.VBProject.VBComponents)
        {
            if (vbaComponent.Name == moduleName)
            {
                workbook.VBProject.VBComponents.Remove(vbaComponent);
                LogWithTimestamp(new StringBuilder(), $"Existing module '{moduleName}' removed.");
                break;
            }
        }
    }

    private void RemoveExistingReferences(Excel.Workbook workbook)
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
                    LogWithTimestamp(new StringBuilder(), $"Existing reference to QForm removed.");
                }
                catch (COMException ex)
                {
                    LogWithTimestamp(new StringBuilder(), $"Error removing reference: {ex.Message}");
                }
            }
        }
    }


    private void LogWithTimestamp(StringBuilder logBuilder, string message)
    {
        logBuilder.AppendLine($"{DateTime.Now}: {message}");
    }


    private void LogToFile(string logFilePath, string message)
    {
        using (StreamWriter sw = new StreamWriter(logFilePath, true))
        {
            sw.WriteLine(message);
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
    Set Ref = ThisWorkbook.VBProject.References
    On Error Resume Next
    Ref.AddFromFile ""{Path.Combine(mainPageVM.qformManager.QFormBaseDir, $"..\\QFormApiCom_{mainPageVM.qformManager.qformVersion}\\x{bitDepth}\\QFormAPI.tlb")}""
    
    If Err.Number <> 0 Then
        WriteErrorToFile Err.Description
    Else
        WriteErrorToFile ""Library added successfully.""
    End If

    On Error GoTo 0
End Sub

Sub WriteErrorToFile(msg As String)
    Dim fileNum As Integer
    fileNum = FreeFile
    Open ""{Path.Combine(logFolderPath, "LogVBA.txt")}"" For Append As #fileNum
    Print #fileNum, Now & "" - "" & msg
    Close #fileNum
End Sub
";
    }

}
