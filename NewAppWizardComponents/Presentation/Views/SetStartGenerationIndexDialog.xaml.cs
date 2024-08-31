using System.Diagnostics;

namespace NewAppWizardComponents;
public sealed partial class SetStartGenerationIndexDialog : ContentDialog
{
    MainPageVM mainPageVM;
    public SetStartGenerationIndexDialog(MainPageVM vm)
    {
        this.InitializeComponent();

        mainPageVM = vm;

        SGindex.Text = mainPageVM.startGenerationIndex.ToString();
    }

    private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        try
        {
            Convert.ToInt32(SGindex.Text);
            mainPageVM.startGenerationIndex = Convert.ToInt32(SGindex.Text);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }

    }

    private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
    }

    private void SGindex_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            Convert.ToInt32(SGindex.Text);

            SGindex.Background = (SolidColorBrush)Resources["Transparent"];
        }
        catch
        {
            SGindex.Background = (SolidColorBrush)Resources["FailStatus"];
        }
    }
}
