using System.Diagnostics;

namespace NewAppWizardComponents;
public sealed partial class CSharpApiSettingsDialog : ContentDialog
{
    public MainPageVM mainPageVM;
    private ApiEntry _snippetEntry;

    public CSharpApiSettingsDialog(MainPageVM vm, ApiEntry? entry = null)
    {
        this.InitializeComponent();

        this.Loaded += (s, e) =>
        {
            if (entry != null)
            {
                _snippetEntry = entry;
                RestoreDialogState();
                this.PrimaryButtonText = "Save";
            }
            else
            {
                StartFromAppWindowRadioButton.IsChecked = true;
                UseQFormAPIFromInstallationRadioButton.IsChecked = true;

                _snippetEntry = new ApiEntry(0, "_csharp_settings", typeof(ACSharpSettings), null, false, null, true);
            }
        };

        mainPageVM = vm;
    }

    private void UpdateConfig()
    {
        var snippetConfig = (_snippetEntry.arg_value as ACSharpSettings);
        
        snippetConfig.connection_type = DetermineInteractionType().ToString();
        snippetConfig.alt_connection = ConnectToExistingWindowCheckBox.IsChecked == true;
        snippetConfig.import_dir = DetermineReferenceType().ToString();
    }

    private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        if (!ValidateInputs())
        {
            args.Cancel = true;
            DisplayError("Please fill in all required fields");
            return;
        }

        UpdateConfig();

        this.PrimaryButtonText = "Save";
        ErrorTextBlock.Visibility = Visibility.Collapsed;
    }

    private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {

    }

    private void DisplayError(string error)
    {
        ErrorTextBlock.Visibility = Visibility.Visible;
        ErrorTextBlock.Text = error;
    }

    private APIQFormInteractionType DetermineInteractionType()
    {
        if (NewQFormWindowRadioButton.IsChecked == true)
            return APIQFormInteractionType.script_starts;
        if (ConnectToExistingQFormRadioButton.IsChecked == true)
            return APIQFormInteractionType.script_connect;
        if (StartFromAppWindowRadioButton.IsChecked == true)
            return APIQFormInteractionType.qform_starts;

        return APIQFormInteractionType.script_starts; // Значение по умолчанию
    }

    private APIQFormReference DetermineReferenceType()
    {
        return UseCopyOfQFormAPIRadioButton.IsChecked == true
            ? APIQFormReference.local_folder
            : APIQFormReference.default_folder;
    }

    private bool ValidateInputs()
    {
        if (NewQFormWindowRadioButton.IsChecked == false && StartFromAppWindowRadioButton.IsChecked == false &&
            ConnectToExistingQFormRadioButton.IsChecked == false)
            return false;

        if (UseQFormAPIFromInstallationRadioButton.IsChecked == false && UseCopyOfQFormAPIRadioButton.IsChecked == false)
            return false;

        return true;
    }

    private void SetFieldsAvailability(bool value)
    {
        StartFromAppWindowRadioButton.IsEnabled = value;
        UseCopyOfQFormAPIRadioButton.IsEnabled = value;
        SelectAPICopyButton.IsEnabled = value;
    }

    private void StartFromAppWindowRadioButton_Checked(object sender, RoutedEventArgs e)
    {
        if (ConnectToExistingWindowCheckBox != null)
            ConnectToExistingWindowCheckBox.IsEnabled = true;
    }
    private void StartFromAppWindowRadioButton_Unchecked(object sender, RoutedEventArgs e)
    {
        ConnectToExistingWindowCheckBox.IsEnabled = false;
    }

    public void RestoreDialogState()
    {
        var snippetConfig = (_snippetEntry.arg_value as ACSharpSettings);

        NewQFormWindowRadioButton.IsChecked = snippetConfig.connection_type == APIQFormInteractionType.script_starts.ToString();
        StartFromAppWindowRadioButton.IsChecked = snippetConfig.connection_type == APIQFormInteractionType.qform_starts.ToString();
        ConnectToExistingQFormRadioButton.IsChecked = snippetConfig.connection_type == APIQFormInteractionType.script_connect.ToString();
        ConnectToExistingWindowCheckBox.IsChecked = snippetConfig.alt_connection;

        UseQFormAPIFromInstallationRadioButton.IsChecked = snippetConfig.import_dir == APIQFormReference.default_folder.ToString();
        UseCopyOfQFormAPIRadioButton.IsChecked = snippetConfig.import_dir == APIQFormReference.local_folder.ToString();
    }

    public ApiEntry GetEntry() => _snippetEntry;

    private async void SelectAPICopyButton_Click(object sender, RoutedEventArgs e)
    {
        //string baseDir = mainPageVM.qformManager.QFormBaseDir;
        //string qformApi = "API\\App\\Python\\QFormAPI.py";
        //string apiFile = Path.Combine(baseDir, qformApi);

        //StorageFolder folder = await mainPageVM.projectManager.SelectFolder();

        //if (folder != null)
        //{
        //    var task = mainPageVM.projectManager.CopyFile(apiFile, Path.Combine(folder.Path, "QFormAPI.py"));
        //    if (!string.IsNullOrEmpty(task.Result))
        //    {
        //        Debug.WriteLine(task.Result);
        //        DisplayError(task.Result);
        //    }
        //}


    }

    private async void SelectAPIdllButton_Click(object sender, RoutedEventArgs e)
    {

    }
}
