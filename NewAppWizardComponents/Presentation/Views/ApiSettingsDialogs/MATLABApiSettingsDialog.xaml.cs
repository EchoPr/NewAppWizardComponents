
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

        return APIQFormInteractionType.script_starts; // Значение по умолчанию
    }

    

    private bool ValidateInputs()
    {

        if (NewQFormWindowRadioButton.IsChecked == false &&
            ConnectToExistingQFormRadioButton.IsChecked == false)
            return false;

        return true;
    }

    public void RestoreDialogState()
    {
        var snippetConfig = (_snippetEntry.arg_value as AMatlabSettings);

        NewQFormWindowRadioButton.IsChecked = snippetConfig.connection_type == APIQFormInteractionType.script_starts.ToString();
        ConnectToExistingQFormRadioButton.IsChecked = snippetConfig.connection_type == APIQFormInteractionType.script_connect.ToString();

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

        StorageFile file = await mainPageVM.projectManager.SaveFile(new Tuple<string, string>("MATLAB", ".m"));

        if (file == null) return;

        ICodeGenerator generator = CodeGeneratorFactory.GetGenerator("MATLAB");
        List<ViewCodeSample> generatedCode = generator.GenerateApiSnippet(_snippetEntry, mainPageVM.qformManager.QFormBaseDir);

        using (StreamWriter sw = new StreamWriter(file.Path, append: false))
        {
            sw.WriteLine(generatedCode.Aggregate("", (acc, val) => acc + val.content));
        }


        ErrorTextBlock.Visibility = Visibility.Collapsed;
    }
}
