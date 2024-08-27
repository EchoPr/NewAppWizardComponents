using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using Uno.Extensions;
using Excel = Microsoft.Office.Interop.Excel;

namespace NewAppWizardComponents;
public partial class VBAApiSettingsDialogVM
{   
    
    public ObservableCollection<string> ComLibraries { get; private set; }

    private string registryKeyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";


    public VBAApiSettingsDialogVM()
    {
        ComLibraries = new ObservableCollection<string>();

        LoadComLibraries("QFormApiCom");
    }

    public void LoadComLibraries(string prefix)
    {
        

        LoadLibrariesFromRegistry(registryKeyPath, RegistryView.Registry64, prefix);
        LoadLibrariesFromRegistry(registryKeyPath, RegistryView.Registry32, prefix);
    }



    private void LoadLibrariesFromRegistry(string registryKeyPath, RegistryView registryView, string prefix)
    {

        using var baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, registryView);
        using var typeLibKey = baseKey.OpenSubKey(registryKeyPath);
        if (typeLibKey != null)
        {
            var subKeyNames = typeLibKey.GetSubKeyNames();
            foreach (var name in subKeyNames)
            {
                if (name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                {
                    ComLibraries.Add($"QForm API {name.Split(" ")[1]}");
                }
            }
        }

    }

    public void UninstallQFormApi(string ver)
    {
        string uninstallerPath = "";

        using var baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
        using var typeLibKey = baseKey.OpenSubKey(registryKeyPath);
        if (typeLibKey != null)
        {
            var subKeyNames = typeLibKey.GetSubKeyNames();
            foreach (var name in subKeyNames)
            {
                if (name == $"QFormApiCom {ver}")
                {
                    uninstallerPath = typeLibKey.OpenSubKey(name).GetValue("UninstallPath").ToString();
                    break;
                }
            }
        }

        var splitIndex = uninstallerPath.IndexOf(".exe", StringComparison.OrdinalIgnoreCase) + 4;
        var filePath = uninstallerPath.Substring(0, splitIndex).Trim();
        var arguments = uninstallerPath.Substring(splitIndex).Trim();

        var processInfo = new ProcessStartInfo
        {
            FileName = filePath,
            Arguments = arguments,
            UseShellExecute = false
        };

        
        var process = Process.Start(processInfo);

        process.WaitForExit();

        ComLibraries.Clear();
        LoadComLibraries("QFormApiCom");
    }

//    private void ffsda()
//    {
//        // Путь к существующему файлу Excel
//        string excelFilePath = @"C:\Path\To\Your\ExcelFile.xlsx";

//        // Создание экземпляра приложения Excel
//        Excel.Application excelApp = new Excel.Application();
//        excelApp.Visible = true;  // Опционально, чтобы видеть, что происходит

//        try
//        {
//            // Открытие существующей книги Excel
//            Excel.Workbook workbook = excelApp.Workbooks.Open(excelFilePath);

//            // Получение доступа к коллекции модулей VBA в книге
//            Excel.VBComponent vbaModule = workbook.VBProject.VBComponents.Add(Excel.vbext_ComponentType.vbext_ct_StdModule);

//            // VBA-код, который добавит ссылку на библиотеку
//            string vbaCode = @"
//Sub AddReference()
//    Dim Ref As Object
//    Set Ref = ThisWorkbook.VBProject.References
//    On Error Resume Next
//    Ref.AddFromFile ""C:\Path\To\YourLibrary.dll"" ' Укажите путь к вашей библиотеке
//    If Err.Number <> 0 Then
//        MsgBox ""Library not found or could not be added.""
//    Else
//        MsgBox ""Library added successfully.""
//    End If
//    On Error GoTo 0
//End Sub
//";

//            // Добавление кода в модуль VBA
//            vbaModule.CodeModule.AddFromString(vbaCode);

//            // Запуск макроса
//            excelApp.Run("AddReference");

//            // Опционально: Сохранение книги и закрытие Excel
//            workbook.Close(true); // Сохраняем изменения и закрываем книгу
//        }
//        catch (COMException ex)
//        {
//            Console.WriteLine("Произошла ошибка при работе с Excel: " + ex.Message);
//        }
//        finally
//        {
//            // Закрытие приложения Excel
//            excelApp.Quit();
//            Marshal.ReleaseComObject(excelApp);
//        }
//    }
}
