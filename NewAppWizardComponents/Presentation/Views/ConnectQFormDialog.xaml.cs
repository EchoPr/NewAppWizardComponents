

using System.Diagnostics;

namespace NewAppWizardComponents;
public sealed partial class ConnectQFormDialog : ContentDialog
{
    private ConnectQFormDialogVM _conQFormVM;

    public ConnectQFormDialog(MainPageVM mainPageVM)
    {
        this.InitializeComponent();

        _conQFormVM = new ConnectQFormDialogVM(mainPageVM);
        this.DataContext = _conQFormVM;
    }

    private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        args.Cancel = true;

        SessionInfo selectedSession = _conQFormVM.SelectedItem;

        if (selectedSession == null)
        {
            DisplayErrorMessage( "No sessions selected");
            return;
        }

        var connectionResult = _conQFormVM.ConnectQFormSession(selectedSession);

        if (connectionResult.Item1) sender.Hide();
        else
        {
            if (connectionResult.Item2 != null)
            {
                DisplayErrorMessage(connectionResult.Item2);
            }
        }
    }

    private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
    }

    private void DisplayErrorMessage(string content)
    {
        ErrorsBufferTextBlock.Visibility = Visibility.Visible;
        ErrorsBufferTextBlock.Text = content;
    }

}

