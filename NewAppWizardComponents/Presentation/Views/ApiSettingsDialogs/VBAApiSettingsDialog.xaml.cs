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
        AdditionWay.Children.ForEach(item => { if (item is CheckBox i && i.Tag.ToString() != "NoEdit") i.IsEnabled = val; });
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

            string res = _excelHelper.CreateNewProject(file.Path, (bool)ConnectionTab.IsChecked, (bool)SampleTab.IsChecked);
            StatusText.Text = res;
        }
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

    private void ContentDialog_Closed(ContentDialog sender, ContentDialogClosedEventArgs args)
    {
        _excelHelper.Dispose();
    }
}

public class ExcelHelper : IDisposable
{
    public MainPageVM mainPageVM;

    private string resultFilePath;

    public ExcelHelper(MainPageVM vm)
    {
        mainPageVM = vm;
        resultFilePath = Path.Combine(mainPageVM.qformManager.qformTempDir, Process.GetCurrentProcess().Id.ToString() + ".txt");
    }

    public void Dispose()
    {
        if (File.Exists(resultFilePath))
        {
            File.Delete(resultFilePath);
        }
    }

    public string CreateNewProject(string fileName, bool addConnectionTab = false, bool addSampleTab = false)
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

            if (addConnectionTab || addSampleTab)
            {
                Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Sheets[1];
                worksheet.Name = "QForm configuration";

                int startRow = 1; // Начальная строка для таблицы
                int startCol = 1; // Начальная колонка для таблицы

                Excel.Range headerTableRange = (Excel.Range)worksheet.Cells[1, 1];
                headerTableRange.Value = "QForm Parameters";
                headerTableRange.Font.Bold = true;
                worksheet.Range["A1:C1"].Merge();
                headerTableRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                worksheet.Range["C1:D1"].Merge();
                worksheet.Range["C2:D2"].Merge();
                worksheet.Range["C3:D3"].Merge();
                worksheet.Range["C4:D4"].Merge();
                worksheet.Range["C5:D5"].Merge();

                string[] tableHeaders = { "Parameter", "Value", "Description", "" };
                for (int i = 0; i < tableHeaders.Length; i++)
                {
                    ((Excel.Range)worksheet.Cells[startRow + 1, startCol + i]).Value = tableHeaders[i];
                    ((Excel.Range)worksheet.Cells[startRow + 1, startCol + i]).Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                    ((Excel.Range)worksheet.Cells[startRow + 1, startCol + i]).Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlMedium;
                }

                string[,] tableData = {
                { "Path to QForm", Path.Combine(mainPageVM.qformManager.QFormBaseDir, "x64"), "Required", "" },
                { "Instance Name", "New Instance", "Optional. Used for reconnect", "" },
                { "Disable exceptions", "1", "VBA has poor exception handling support", "" }
            };

                for (int row = 0; row < tableData.GetLength(0); row++)
                {
                    for (int col = 0; col < tableData.GetLength(1); col++)
                    {
                        ((Excel.Range)worksheet.Cells[startRow + 2 + row, startCol + col]).Value = tableData[row, col];
                        ((Excel.Range)worksheet.Cells[startRow + 2 + row, startCol + col]).Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                        ((Excel.Range)worksheet.Cells[startRow + 2 + row, startCol + col]).Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;
                    }
                }

                if (addConnectionTab) 
                    AddConnectionTab(workbook, worksheet);
                if (addSampleTab)
                    AddSampleTab(workbook, worksheet, addConnectionTab);

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
            // Освобождение объектов в обратном порядке
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

    public string EditExistingProject(string fileName, bool addConnectionTab = false, bool addSampleTab = false)
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

            if (addConnectionTab || addSampleTab)
            {
                Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Sheets[1];
                worksheet.Name = "QForm configuration";

                int startRow = 1; // Начальная строка для таблицы
                int startCol = 1; // Начальная колонка для таблицы

                Excel.Range headerTableRange = (Excel.Range)worksheet.Cells[1, 1];
                headerTableRange.Value = "QForm Parameters";
                headerTableRange.Font.Bold = true;
                worksheet.Range["A1:C1"].Merge();
                headerTableRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                string[] tableHeaders = { "Parameter", "Value", "Description" };
                for (int i = 0; i < tableHeaders.Length; i++)
                {
                    ((Excel.Range)worksheet.Cells[startRow + 1, startCol + i]).Value = tableHeaders[i];
                    ((Excel.Range)worksheet.Cells[startRow + 1, startCol + i]).Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                    ((Excel.Range)worksheet.Cells[startRow + 1, startCol + i]).Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlMedium;
                }

                string[,] tableData = {
                { "Path to QForm", Path.Combine(mainPageVM.qformManager.QFormBaseDir, "x64"), "Required" },
                { "Instance Name", "New Instance", "Optional. Used for reconnect" },
                { "Disable exceptions", "1", "VBA has poor exception handling support" }
            };

                for (int row = 0; row < tableData.GetLength(0); row++)
                {
                    for (int col = 0; col < tableData.GetLength(1); col++)
                    {
                        ((Excel.Range)worksheet.Cells[startRow + 2 + row, startCol + col]).Value = tableData[row, col];
                        ((Excel.Range)worksheet.Cells[startRow + 2 + row, startCol + col]).Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                        ((Excel.Range)worksheet.Cells[startRow + 2 + row, startCol + col]).Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;
                    }
                }

                if (addConnectionTab)
                    AddConnectionTab(workbook, worksheet);
                if (addSampleTab)
                    AddSampleTab(workbook, worksheet, addConnectionTab);

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

    private void AddConnectionTab(dynamic workbook, Excel.Worksheet worksheet)
    {
        RemoveExistingModule(workbook, "QFormConnection");

        dynamic vbaModule = workbook.VBProject.VBComponents.Add(1); // 1 for vbext_ComponentType.vbext_ct_StdModule
        vbaModule.Name = "QFormConnection";

        vbaModule.CodeModule.AddFromString(VBAMacroses.ConnectionTab);

        string[] connectionButtonsNames = ["Start New QForm", "Close QForm", "Disconnect QForm", "Connect QForm"];
        string[] connectionButtonsBinds = ["QFormStart", "QFormReconnect", "QFormDetach", "QFormAttach"];
        int conButStartRow = 8;

        Excel.Range headerConnectionButtonsRange = (Excel.Range)worksheet.Cells[conButStartRow, 1];
        headerConnectionButtonsRange.Value = "QForm Connection";
        headerConnectionButtonsRange.Font.Bold = true;
        worksheet.Range[$"A{conButStartRow}:E{conButStartRow}"].Merge();
        headerConnectionButtonsRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

        Excel.Range sessionIdHeader = (Excel.Range)worksheet.Cells[conButStartRow + 1, 5];
        sessionIdHeader.Value = "Session ID";
        sessionIdHeader.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

        Excel.Range sessionId = (Excel.Range)worksheet.Cells[conButStartRow + 2, 5];
        sessionId.Value = "0";

        for (int i = 1; i < 1 + 4; i++)
        {
            Excel.Range cell = (Excel.Range)worksheet.Cells[conButStartRow + 2, i];

            ((Excel.Range)worksheet.Columns[i]).ColumnWidth = 40; 
            ((Excel.Range)worksheet.Rows[conButStartRow + 2]).RowHeight = 60;     

            double buttonWidth = Convert.ToInt32(cell.Width) - 20;
            double buttonHeight = Convert.ToInt32(cell.Height) - 20; 

            double leftPosition = Convert.ToInt32(cell.Left) + 10; 
            double topPosition = Convert.ToInt32(cell.Top) + 10;  

            Excel.Shape button = worksheet.Shapes.AddFormControl(
                Excel.XlFormControl.xlButtonControl,
                Convert.ToInt32(leftPosition),
                Convert.ToInt32(topPosition),
                Convert.ToInt32(buttonWidth),
                Convert.ToInt32(buttonHeight)
            );

            button.TextFrame.Characters().Text = connectionButtonsNames[i - 1];
            button.OnAction = connectionButtonsBinds[i - 1];
        }
    }

    private void AddSampleTab(dynamic workbook, Excel.Worksheet worksheet, bool connectionAdded)
    {
        RemoveExistingModule(workbook, "QFormSamples");

        dynamic vbaModule = workbook.VBProject.VBComponents.Add(1); // 1 for vbext_ComponentType.vbext_ct_StdModule
        vbaModule.Name = "QFormSamples";

        vbaModule.CodeModule.AddFromString(VBAMacroses.SampleTab);

        string[] sampleButtonsNames = ["Test With Reconnect", "Test Without Reconnect", "Reset State By Runtime"];
        string[] sampleButtonsBinds = ["TestWithReconnect", "TestWithoutReconnect", "ResetStateByRuntimeError"];
        string[] sampleDescriptions = ["Description 1", "Description 2", "Description 3"];
        int conButStartRow = connectionAdded ? 13 : 8;

        Excel.Range headerConnectionButtonsRange = (Excel.Range)worksheet.Cells[conButStartRow, 1];
        headerConnectionButtonsRange.Value = "Samples";
        headerConnectionButtonsRange.Font.Bold = true;
        worksheet.Range[$"A{conButStartRow}:B{conButStartRow}"].Merge();
        headerConnectionButtonsRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

        Excel.Range sampleCommandHeader = (Excel.Range)worksheet.Cells[conButStartRow + 1, 1];
        sampleCommandHeader.Value = "Command";
        sampleCommandHeader.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

        Excel.Range sampleDscHeader = (Excel.Range)worksheet.Cells[conButStartRow + 1, 2];
        sampleDscHeader.Value = "Description";
        sampleDscHeader.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;


        for (int i = 1; i < 1 + 3; i++)
        {
            Excel.Range cell = (Excel.Range)worksheet.Cells[conButStartRow + 1 + i, 1];

            ((Excel.Range)worksheet.Columns[i]).ColumnWidth = 40;
            ((Excel.Range)worksheet.Rows[conButStartRow + 1 + i]).RowHeight = 60;

            double buttonWidth = Convert.ToInt32(cell.Width) - 20;
            double buttonHeight = Convert.ToInt32(cell.Height) - 20;

            double leftPosition = Convert.ToInt32(cell.Left) + 10;
            double topPosition = Convert.ToInt32(cell.Top) + 10;

            Excel.Shape button = worksheet.Shapes.AddFormControl(
                Excel.XlFormControl.xlButtonControl,
                Convert.ToInt32(leftPosition),
                Convert.ToInt32(topPosition),
                Convert.ToInt32(buttonWidth),
                Convert.ToInt32(buttonHeight)
            );

            button.TextFrame.Characters().Text = sampleButtonsNames[i - 1];
            button.OnAction = sampleButtonsBinds[i - 1];

            Excel.Range sampleDsc = (Excel.Range)worksheet.Cells[conButStartRow + 1 + i, 2];
            sampleDsc.Value = sampleDescriptions[i - 1];
            sampleDsc.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
        }
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
' THIS IS GENERATED FILE. DO NOT EDIT IT!

Sub AddReference()
    Dim Ref As Object
    Dim filePath As String
    filePath = ""{resultFilePath}""

    Set Ref = ThisWorkbook.VBProject.References
    On Error Resume Next
     Ref.AddFromFile ""{Path.Combine(mainPageVM.qformManager.QFormBaseDir, $"..\\QFormApiCom_{mainPageVM.qformManager.qformVersion}\\x{bitDepth}\\QFormAPI.tlb")}""

    Dim resultText As String
    If Err.Number <> 0 Then
        resultText = ""Error: "" & Err.Description
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

public static class VBAMacroses
{
    public static string ConnectionTab = @"
Option Explicit
Global qform As QFormAPI.qform

Sub QFormInit()
    Dim QFormExeDir As String
    Dim QFormInstanceName As String
    Dim ExceptionsEnable As Boolean
    
    QFormExeDir = Range(""B3"").Value
    QFormInstanceName = Range(""B4"").Value
    ExceptionsEnable = True
    If Range(""B5"").Value = 1 Then
        ExceptionsEnable = False
    End If
    
    If qform Is Nothing Then
        Dim mgr As New QFormAPI.QFormMgr
        Set qform = mgr.instance(QFormInstanceName)
        qform.exceptions_enable (ExceptionsEnable)
        qform.qform_dir_set (QFormExeDir)
    End If
End Sub

Sub QFormStart()
    QFormInit
    
    If qform.exceptions_enabled Then
        qform.qform_start
    Else
        If Not qform.qform_start Then
            MsgBox qform.last_error
        End If
    End If
End Sub

Function QFormReconnect() As Boolean
    QFormInit
    QFormReconnect = True
    If qform.exceptions_enabled Then
        qform.qform_reconnect
    Else
        If Not qform.qform_reconnect Then
            MsgBox qform.last_error
            QFormReconnect = False
        End If
    End If
End Function

Sub QFormAttach()
    QFormInit
    
    Dim sid As New QFormAPI.SessionId
    sid.session_id = Range(""E10"").Value ' QForm window number
    
    If qform.exceptions_enabled Then
        qform.qform_attach_to sid
    Else
        If Not qform.qform_attach_to(sid) Then
            MsgBox qform.last_error
        End If
    End If

End Sub

Sub QFormDetach()
    QFormInit
    qform.qform_detach
End Sub

Sub QFormClose()
    QFormInit
    qform.qform_close
End Sub
";
    public static string SampleTab = @"
Option Explicit

Sub TestWithReconnect()
    If Not QFormReconnect Then Exit Sub
    
    Dim ret1 As QFormAPI.ProcessId
    Set ret1 = qform.qform_process_id()

    MsgBox ""QForm process id:"" & ret1.pid
End Sub

Sub TestWithoutReconnect()
    Dim ret1 As QFormAPI.ProcessId
    Set ret1 = qform.qform_process_id()

    MsgBox ""QForm process id:"" & ret1.pid
End Sub

Sub ResetStateByRuntimeError()

    Dim q As QFormAPI.qform
    q.project_save ' null reference error
End Sub

";
}
