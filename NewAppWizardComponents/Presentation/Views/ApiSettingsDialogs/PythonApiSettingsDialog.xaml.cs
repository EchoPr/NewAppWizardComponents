
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace NewAppWizardComponents;
public sealed partial class PythonApiSettingsDialog : ContentDialog
{
    public List<ViewCodeSample> codeSamples;

    public MainPageVM mainPageVM;
    private ApiEntry _snippetEntry;

    public PythonApiSettingsDialog(MainPageVM vm, ApiEntry? entry = null)
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
                PythonFileRadioButton.IsChecked = true;
                StartFromAppWindowRadioButton.IsChecked = true;
                UseQFormAPIFromInstallationRadioButton.IsChecked = true;

                _snippetEntry = new ApiEntry(0, "_pyton_settings", typeof(APytonSettings), null, false, null, true);
            }
        };

        mainPageVM = vm;
    }

    private void UpdateConfig()
    {
        var snippetConfig = (_snippetEntry.arg_value as APytonSettings);
        snippetConfig.script_type = (PythonFileRadioButton.IsChecked == true
                                                                        ? PythonScriptType.pyfile 
                                                                        : PythonScriptType.notebook).ToString();
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

    private PythonQFormInteractionType DetermineInteractionType()
    {
        if (NewQFormWindowRadioButton.IsChecked == true)
            return PythonQFormInteractionType.script_starts;
        if (ConnectToExistingQFormRadioButton.IsChecked == true)
            return PythonQFormInteractionType.script_connect;
        if (StartFromAppWindowRadioButton.IsChecked == true)
            return PythonQFormInteractionType.qform_starts;

        return PythonQFormInteractionType.script_starts; // Значение по умолчанию
    }

    private PythonQFormReference DetermineReferenceType()
    {
        return UseCopyOfQFormAPIRadioButton.IsChecked == true
            ? PythonQFormReference.local_folder
            : PythonQFormReference.default_folder;
    }

    private bool ValidateInputs()
    {
        // Пример валидации данных. Дополните при необходимости.
        if (PythonFileRadioButton.IsChecked == false && NotebookRadioButton.IsChecked == false)
            return false;

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

    private void PythonFileRadioButton_Checked(object sender, RoutedEventArgs e)
    {
        SetFieldsAvailability(true);
    }

    private void NotebookRadioButton_Checked(object sender, RoutedEventArgs e)
    {
        SetFieldsAvailability(false);
        StartFromAppWindowRadioButton.IsChecked = false;
        UseCopyOfQFormAPIRadioButton.IsChecked = false;
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
        var snippetConfig = (_snippetEntry.arg_value as APytonSettings);

        ScriptTypePanel.Visibility = Visibility.Collapsed;

        PythonFileRadioButton.IsChecked = snippetConfig.script_type == PythonScriptType.pyfile.ToString();
        NotebookRadioButton.IsChecked = snippetConfig.script_type == PythonScriptType.notebook.ToString();

        NewQFormWindowRadioButton.IsChecked = snippetConfig.connection_type == PythonQFormInteractionType.script_starts.ToString();
        StartFromAppWindowRadioButton.IsChecked = snippetConfig.connection_type == PythonQFormInteractionType.qform_starts.ToString();
        ConnectToExistingQFormRadioButton.IsChecked = snippetConfig.connection_type == PythonQFormInteractionType.script_connect.ToString();
        ConnectToExistingWindowCheckBox.IsChecked = snippetConfig.alt_connection;

        UseQFormAPIFromInstallationRadioButton.IsChecked = snippetConfig.import_dir == PythonQFormReference.default_folder.ToString();
        UseCopyOfQFormAPIRadioButton.IsChecked = snippetConfig.import_dir == PythonQFormReference.local_folder.ToString();
    }

    public ApiEntry GetEntry() => _snippetEntry;

    private async void SelectAPICopyButton_Click(object sender, RoutedEventArgs e)
    {
        string baseDir = mainPageVM.qformManager.QFormBaseDir;
        string qformApi = "API\\App\\Python\\QFormAPI.py";
        string apiFile = Path.Combine(baseDir, qformApi);

        StorageFolder folder = await mainPageVM.projectManager.SelectFolder();

        if (folder != null)
        {
            var task = mainPageVM.projectManager.CopyFile(apiFile, Path.Combine(folder.Path, "QFormAPI.py"));
            if (!string.IsNullOrEmpty(task.Result))
            {
                Debug.WriteLine(task.Result);
                DisplayError(task.Result);
            }
        }
    }
}
