namespace NewAppWizardComponents;
public sealed partial class MessageBox : ContentDialog
{
    public MessageBox(string content)
    {
        this.InitializeComponent();
        this.Content = content;
    }

    private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
    }

}
