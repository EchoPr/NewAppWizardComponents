
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace NewAppWizardComponents;
public sealed partial class PythonApiSettingsDialog : ContentDialog
{
    public List<ViewCodeSample> codeSamples;
    public SnippetConfig snippetConfig;

    public PythonApiSettingsDialog(SnippetConfig initialConfig = null)
    {
        this.InitializeComponent();

        this.Loaded += (s, e) =>
        {
            if (initialConfig == null)
            {
                PythonFileRadioButton.IsChecked = true;
                StartFromAppWindowRadioButton.IsChecked = true;
                UseQFormAPIFromInstallationRadioButton.IsChecked = true;

                snippetConfig = new SnippetConfig();
            }
            else
            {
                RestoreDialogState(initialConfig);
                PrimaryButtonText = "Save";
            }
        };
    }

    private void UpdateConfig()
    {
        snippetConfig.ScriptType = PythonFileRadioButton.IsChecked == true ? "Python" : "Notebook";
        snippetConfig.QFormInteractionType = DetermineInteractionType();
        snippetConfig.ConnectToExisting = ConnectToExistingWindowCheckBox.IsChecked == true;
        snippetConfig.QFormReferenceType = DetermineReferenceType();
    }

    private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        if (!ValidateInputs())
        {
            args.Cancel = true;
            ErrorTextBlock.Visibility = Visibility.Visible;
            return;
        }

        UpdateConfig();

        this.PrimaryButtonText = "Save";
        ErrorTextBlock.Visibility = Visibility.Collapsed;
    }

    private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        
    }

    private PythonQFormInteractionType DetermineInteractionType()
    {
        if (NewQFormWindowRadioButton.IsChecked == true)
            return PythonQFormInteractionType.script_starts;
        if (ConnectToExistingQFormRadioButton.IsChecked == true)
            return PythonQFormInteractionType.script_connect;
        if (StartFromAppWindowRadioButton.IsChecked == true)
            return ConnectToExistingWindowCheckBox.IsChecked == true
                ? PythonQFormInteractionType.qform_starts_or_connect
                : PythonQFormInteractionType.qform_starts;

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

    public void RestoreDialogState(SnippetConfig config)
    {
        snippetConfig = config;
        ScriptTypePanel.Visibility = Visibility.Collapsed;

        PythonFileRadioButton.IsChecked = snippetConfig.ScriptType == "Python";
        NotebookRadioButton.IsChecked = snippetConfig.ScriptType == "Notebook";
        NewQFormWindowRadioButton.IsChecked = snippetConfig.QFormInteractionType == PythonQFormInteractionType.script_starts;
        StartFromAppWindowRadioButton.IsChecked = snippetConfig.QFormInteractionType == PythonQFormInteractionType.qform_starts ||
                                                  snippetConfig.QFormInteractionType == PythonQFormInteractionType.qform_starts_or_connect;
        ConnectToExistingQFormRadioButton.IsChecked = snippetConfig.QFormInteractionType == PythonQFormInteractionType.script_connect;
        ConnectToExistingWindowCheckBox.IsChecked = snippetConfig.ConnectToExisting;
        UseQFormAPIFromInstallationRadioButton.IsChecked = snippetConfig.QFormReferenceType == PythonQFormReference.default_folder;
        UseCopyOfQFormAPIRadioButton.IsChecked = snippetConfig.QFormReferenceType == PythonQFormReference.local_folder;

        //NotebookRadioButton_Checked(null, null);
        //StartFromAppWindowRadioButton_Checked(null, null);
    }

    public List<ViewCodeSample> GenerateAPISnippet(PythonQFormInteractionType interactionType, PythonQFormReference referenceType)
    {
        PythonCodeGenerator generator = new PythonCodeGenerator();
        codeSamples = generator.GenerateApiSnippet(snippetConfig.QFormInteractionType, snippetConfig.QFormReferenceType);

        return codeSamples;
    }

    public SnippetConfig GetConfig() => snippetConfig;

}

public class SnippetConfig
{
    public string ScriptType { get; set; }
    public PythonQFormInteractionType QFormInteractionType { get; set; }
    public bool ConnectToExisting { get; set; }
    public PythonQFormReference QFormReferenceType { get; set; }
}
