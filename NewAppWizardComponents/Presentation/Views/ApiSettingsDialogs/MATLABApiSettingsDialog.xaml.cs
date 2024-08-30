
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace NewAppWizardComponents;
public sealed partial class MATLABApiSettingsDialog : ContentDialog
{
    public MainPageVM mainPageVM;
    private ApiEntry _snippetEntry;
    private Dictionary<string, ApiEntry> langSnippets;

    public MATLABApiSettingsDialog(MainPageVM vm, ApiEntry? entry = null, Dictionary<string, ApiEntry> _languageSnippets = null)
    {
        this.InitializeComponent();

        this.Loaded += (s, e) =>
        {
            if (entry != null)
            {
                _snippetEntry = entry;
                RestoreDialogState();
                this.PrimaryButtonText = "Save";
                PrimaryButtonClick += ContentDialog_PrimaryButtonClick;
            }
            else
            {
                StartFromAppWindowRadioButton.IsChecked = true;

                _snippetEntry = new ApiEntry(0, "_matlab_settings", typeof(AMatlabSettings), null, false, null, true);
                UpdateConfig();
            }
        };

        mainPageVM = vm;
        langSnippets = _languageSnippets;
    }

    private void UpdateConfig()
    {
        var snippetConfig = (_snippetEntry.arg_value as AMatlabSettings);
        snippetConfig.connection_type = DetermineInteractionType().ToString();
        snippetConfig.alt_connection = ConnectToExistingWindowCheckBox.IsChecked == true;
        snippetConfig.use_qform_exceptions = EHQFormExceptions.IsChecked == true;
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

    private void AddSnippetToWorkspace(object sender, RoutedEventArgs args)
    {
        if (!ValidateInputs())
        {
            DisplayError("Please fill in all required fields");
            return;
        }

        UpdateConfig();

        this.PrimaryButtonText = "Save";
        ErrorTextBlock.Visibility = Visibility.Collapsed;

        langSnippets["MATLAB"] = _snippetEntry;

        this.Hide();
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

    

    private bool ValidateInputs()
    {

        if (NewQFormWindowRadioButton.IsChecked == false && StartFromAppWindowRadioButton.IsChecked == false &&
            ConnectToExistingQFormRadioButton.IsChecked == false)
            return false;

        

        return true;
    }

    private void SetFieldsAvailability(bool value)
    {
        StartFromAppWindowRadioButton.IsEnabled = value;
    }

    private void MATLABFileRadioButton_Checked(object sender, RoutedEventArgs e)
    {
        SetFieldsAvailability(true);
    }

    private void NotebookRadioButton_Checked(object sender, RoutedEventArgs e)
    {
        SetFieldsAvailability(false);
        StartFromAppWindowRadioButton.IsChecked = false;
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
        var snippetConfig = (_snippetEntry.arg_value as AMatlabSettings);

        NewQFormWindowRadioButton.IsChecked = snippetConfig.connection_type == APIQFormInteractionType.script_starts.ToString();
        StartFromAppWindowRadioButton.IsChecked = snippetConfig.connection_type == APIQFormInteractionType.qform_starts.ToString();
        ConnectToExistingQFormRadioButton.IsChecked = snippetConfig.connection_type == APIQFormInteractionType.script_connect.ToString();
        ConnectToExistingWindowCheckBox.IsChecked = snippetConfig.alt_connection;

        EHTryCatch.IsChecked = snippetConfig.use_qform_exceptions == false;
        EHQFormExceptions.IsChecked = snippetConfig.use_qform_exceptions == true;
    }

    public ApiEntry GetEntry() => _snippetEntry;

    private void CopyToBuffer(object sender, RoutedEventArgs e)
    {
        if (!ValidateInputs())
        {
            DisplayError("Please fill in all required fields");
            return;
        }

        UpdateConfig();

        ICodeGenerator generator = CodeGeneratorFactory.GetGenerator("MATLAB");
        List<ViewCodeSample> generatedCode = generator.GenerateApiSnippet(_snippetEntry, mainPageVM.qformManager.QFormBaseDir);

        var dp = new DataPackage();
        dp.SetText(generatedCode.Aggregate("", (acc, val) => acc + val.content));
        Clipboard.SetContent(dp);
    }

    private async void CreateScript(object sender, RoutedEventArgs e)
    {
        if (!ValidateInputs())
        {
            DisplayError("Please fill in all required fields");
            return;
        }

        UpdateConfig();

        StorageFile file = await mainPageVM.projectManager.SaveFile(new Tuple<string, string>("MATLAB", ".py"));

        if (file == null) return;

        string baseDir = mainPageVM.qformManager.QFormBaseDir;
        string qformApi = "API\\App\\MATLAB\\QFormAPI.py";
        string apiFile = Path.Combine(baseDir, qformApi);

        if (file != null)
        {
            try
            {
                File.Copy(apiFile, Path.Combine((await file.GetParentAsync()).Path, "QFormAPI.py"));
            }
            catch (Exception ex)
            {
                DisplayError(ex.Message);

                if (File.Exists(file.Path))
                    File.Delete(file.Path);

                return;
            }
        }


        ICodeGenerator generator = CodeGeneratorFactory.GetGenerator("MATLAB");
        List<ViewCodeSample> generatedCode = generator.GenerateApiSnippet(_snippetEntry, mainPageVM.qformManager.QFormBaseDir);

        using (StreamWriter sw = new StreamWriter(file.Path, append: false))
        {
            sw.WriteLine(generatedCode.Aggregate("", (acc, val) => acc + val.content));
        }


        ErrorTextBlock.Visibility = Visibility.Collapsed;
    }
}
