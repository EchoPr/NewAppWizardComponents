
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace NewAppWizardComponents;
public sealed partial class VBAApiSettingsDialog : ContentDialog
{
    public MainPageVM mainPageVM;
    public VBAApiSettingsDialogVM vbaVM;
    private ApiEntry _snippetEntry;

    public VBAApiSettingsDialog(MainPageVM vm, ApiEntry? entry = null)
    {
        this.InitializeComponent();

        vbaVM = new VBAApiSettingsDialogVM();
        this.DataContext = vbaVM;

        mainPageVM = vm;
    }

    

    private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        
    }

    private void ContentDialog_Loaded(object sender, RoutedEventArgs e)
    {
        InstallButton.Content = $"Install QForm API {mainPageVM.qformManager.qformVersion}";
    }

    private void InstallButton_Click(object sender, RoutedEventArgs e)
    {
        var process = Process.Start(Path.Combine(mainPageVM.qformManager.QFormBaseDir, "API\\App\\Excel\\QFormApiCom.exe"));

        process.WaitForExit();
        vbaVM.ComLibraries.Clear();
        vbaVM.LoadComLibraries("QFormApiCom");
    }

    private void UninstallButton_Click(object sender, RoutedEventArgs e)
    {
        string ver = ApiVersions.SelectedValue.ToString().Split(" ")[2];

        vbaVM.UninstallQFormApi(ver);

    }

    private void ApiVersions_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        UninstallButton.IsEnabled = true;
    }
}
