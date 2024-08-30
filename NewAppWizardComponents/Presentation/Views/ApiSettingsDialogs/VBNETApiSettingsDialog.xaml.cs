using System.Diagnostics;
using System.IO.Compression;
using System.Xml.Linq;

namespace NewAppWizardComponents;
public sealed partial class VBNETApiSettingsDialog : ContentDialog
{
    public MainPageVM mainPageVM;
    private ApiEntry _snippetEntry;
    private Dictionary<string, ApiEntry> langSnippets;

    public VBNETApiSettingsDialog(MainPageVM vm, ApiEntry? entry = null, Dictionary<string, ApiEntry> _languageSnippets = null)
    {
        this.InitializeComponent();

        this.Loaded += (s, e) =>
        {
            if (entry != null)
            {
                _snippetEntry = entry;
                RestoreDialogState();
                this.PrimaryButtonText = "Save";
                this.PrimaryButtonClick += ContentDialog_PrimaryButtonClick;
            }
            else
            {
                StartFromAppWindowRadioButton.IsChecked = true;

                _snippetEntry = new ApiEntry(0, "_vbnet_settings", typeof(AVBNetSettings), null, false, null, true);
                UpdateConfig();
            }
        };

        mainPageVM = vm;
        langSnippets = _languageSnippets;
    }

    private void UpdateConfig()
    {
        var snippetConfig = (_snippetEntry.arg_value as AVBNetSettings);

        snippetConfig.connection_type = DetermineInteractionType().ToString();
        snippetConfig.alt_connection = ConnectToExistingWindowCheckBox.IsChecked == true;
        snippetConfig.class_name = string.IsNullOrEmpty(ClassNameTextBox.Text) ? "NewClass" : ClassNameTextBox.Text;
        snippetConfig.use_static = UseStaticName.IsChecked == true;
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

    private void AddScriptToWorkspace(object sender, RoutedEventArgs e)
    {
        if (!ValidateInputs())
        {
            DisplayError("Please fill in all required fields");
            return;
        }

        UpdateConfig();

        this.PrimaryButtonText = "Save";
        ErrorTextBlock.Visibility = Visibility.Collapsed;

        langSnippets["VB.Net"] = _snippetEntry;
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
        var snippetConfig = (_snippetEntry.arg_value as AVBNetSettings);

        NewQFormWindowRadioButton.IsChecked = snippetConfig.connection_type == APIQFormInteractionType.script_starts.ToString();
        StartFromAppWindowRadioButton.IsChecked = snippetConfig.connection_type == APIQFormInteractionType.qform_starts.ToString();
        ConnectToExistingQFormRadioButton.IsChecked = snippetConfig.connection_type == APIQFormInteractionType.script_connect.ToString();
        ConnectToExistingWindowCheckBox.IsChecked = snippetConfig.alt_connection;

        ClassNameTextBox.Text = snippetConfig.class_name;
        UseStaticName.IsChecked = snippetConfig.use_static;

        EHTryCatch.IsChecked = snippetConfig.use_qform_exceptions == false;
        EHQFormExceptions.IsChecked = snippetConfig.use_qform_exceptions == true;
    }

    public ApiEntry GetEntry() => _snippetEntry;

    private void AddApiReferenceToProject(string path)
    {
        string baseDir = mainPageVM.qformManager.QFormBaseDir;
        string qformApi = "x64\\QFormApiNet.dll";

        string apiFile = Path.Combine(baseDir, qformApi);

        string vbprojFilePath = Directory.GetFiles(path, "*.vbproj", SearchOption.TopDirectoryOnly).FirstOrDefault();
        if (vbprojFilePath == null)
            throw new Exception("There is no .vbproj file in the selected folder");

        XDocument vbprojXml = XDocument.Load(vbprojFilePath);
        XElement root = vbprojXml.Root;
        if (root == null)
            throw new Exception("Invalid .vbproj file.");

        XNamespace ns = root.GetDefaultNamespace();
        XElement itemGroup = root.Element(ns + "ItemGroup");
        if (itemGroup == null)
        {
            itemGroup = new XElement(ns + "ItemGroup");
            root.Add(itemGroup);
        }

        var existingReference = itemGroup.Elements(ns + "Reference")
                .FirstOrDefault(e => e.Attribute("Include")?.Value == "QFormAPINet");
        existingReference?.Remove();

        // Add new DLL reference
        XElement reference = new XElement(ns + "Reference",
                new XAttribute("Include", "QFormAPINet"),
                new XElement(ns + "HintPath", apiFile)
            );
        itemGroup.Add(reference);
        vbprojXml.Save(vbprojFilePath);
    }

    private bool isDotnetFramework(XDocument vbproj)
    {
        var targetFrameworkElement = vbproj.Descendants("TargetFramework").FirstOrDefault();

        if (targetFrameworkElement != null)
        {
            string targetFramework = targetFrameworkElement.Value;

            if (targetFramework.StartsWith("netcoreapp") || targetFramework.StartsWith("net"))
            {
                return false;
            }
            else if (targetFramework.StartsWith("net") && targetFramework.Length == 4)
            {
                return true;
            }
        }
        else
        {
            if (vbproj.ToString().Contains("TargetFrameworkVersion"))
            {
                return true;
            }
        }

        throw new Exception("Unknown vbproj type");
    }


    private async void AddClassToProject(object sender, RoutedEventArgs e)
    {
        if (!ValidateInputs())
        {
            DisplayError("Please fill in all required fields");
            return;
        }

        UpdateConfig();

        StorageFile file = await mainPageVM.projectManager.SaveFile(new Tuple<string, string>("VB.Net", ".vb"));

        if (file == null) return;

        
        try
        {
            AddApiReferenceToProject((await file.GetParentAsync()).Path);
        }
        catch (Exception ex)
        {
            DisplayError(ex.Message);

            if (File.Exists(file.Path))
                File.Delete(file.Path);

            return;
        }

        ICodeGenerator generator = CodeGeneratorFactory.GetGenerator("VB.Net");
        List<ViewCodeSample> generatedCode = generator.GenerateApiSnippet(_snippetEntry, mainPageVM.qformManager.QFormBaseDir);

        using (StreamWriter sw = new StreamWriter(file.Path, append: false))
        {
            sw.WriteLine(generatedCode.Aggregate("", (acc, val) => acc + val.content));
        }


        ErrorTextBlock.Visibility = Visibility.Collapsed;
    }
}
