using System.Diagnostics;
using System.Xml.Linq;

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

    private async void SelectAPIButton_Click(object sender, RoutedEventArgs e)
    {
        bool isDllCopy = Convert.ToBoolean(UseQFormAPIFromInstallationRadioButton.IsChecked);

        string baseDir = mainPageVM.qformManager.QFormBaseDir;
        string qformApi = isDllCopy
                            ? "x64\\QFormApiNet.dll"
                            : "API\\App\\C#\\QForm.cs";


        string apiFile = Path.Combine(baseDir, qformApi);
        StorageFolder folder = await mainPageVM.projectManager.SelectFolder();

        if (folder != null)
        {
            try
            {

                string csprojFilePath = Directory.GetFiles(folder.Path, "*.csproj", SearchOption.TopDirectoryOnly)[0];
                if (csprojFilePath == null)
                    throw new Exception("There is no .cproj file in selectted folder");

                XDocument csprojXml = XDocument.Load(csprojFilePath);

                XElement root = csprojXml.Root;

                if (root == null)
                    throw new Exception("Invalid .csproj file.");

                XNamespace ns = root.GetDefaultNamespace();

                if (isDllCopy)
                {
                    XElement itemGroup = root.Element(ns + "ItemGroup");
                    if (itemGroup == null)
                    {
                        itemGroup = new XElement(ns + "ItemGroup");
                        root.Add(itemGroup);
                    }

                    XElement reference = new XElement(ns + "Reference",
                            new XAttribute("Include", "QFormAPINet"),
                            new XElement(ns + "HintPath", apiFile)
                        );

                    itemGroup.Add(reference);

                    csprojXml.Save(csprojFilePath);
                }
                else
                {

                    if (isDotnetFramework(csprojXml))
                    {
                        XElement itemGroup = new XElement(
                            ns + "ItemGroup",
                            new XElement(ns + "Compile", new XAttribute("Include", "QFormApi.cs"))
                        );
                       
                        root.Add(itemGroup);
                        csprojXml.Save(csprojFilePath);
                    }
                    File.Copy(apiFile, Path.Combine(folder.Path, "QFormAPI.cs"));
                }
            }catch (Exception ex)
            {
                DisplayError(ex.Message);
            }
        }
    }

    private bool isDotnetFramework(XDocument csproj)
    {
        var targetFrameworkElement = csproj.Descendants("TargetFramework").FirstOrDefault();

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
            if (csproj.ToString().Contains("TargetFrameworkVersion"))
            {
                return true;
            }
        }

        throw new Exception("Unknown csproj type");
    }

    private void RadioButton_Checked(object sender, RoutedEventArgs e)
    {
        if (UseQFormAPIFromInstallationRadioButton.IsChecked == true)
        {
            SelectAPIdllButton.IsEnabled = true;
            SelectAPICopyButton.IsEnabled = false;
        }
        else if (UseCopyOfQFormAPIRadioButton.IsChecked == true)
        {
            SelectAPIdllButton.IsEnabled = false;
            SelectAPICopyButton.IsEnabled = true;
        }
    }

}
