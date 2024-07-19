using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.UI.Xaml.Controls;
using Windows.Services.Maps;

namespace NewAppWizardComponents;

public partial class MainPageVM : INotifyPropertyChanged
{
    private List<ApiEntry> _methods;
    public List<ApiEntry> Methods { get => _methods; set => _methods = value; }

    private ObservableCollection<ApiEntry> _codeBlocks;
    public ObservableCollection<ApiEntry> CodeBlocks
    {
        get => _codeBlocks;
        set
        {
            _codeBlocks = value;
            OnPropertyChanged();
        }
    }

    public int lastSelectedBlock { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;
    public event EventHandler AddedNewCodeBlock;
    public event EventHandler ClearedCodeBlocks;

    public MainPageVM()
    {
        CodeBlocks = new ObservableCollection<ApiEntry>();
        ReadAllMethodParameters();

    }
    public void AddToCodeBlocks(ApiEntry method)
    {
        if (method != null)
        {
            _codeBlocks.Add(method.Clone());
            AddedNewCodeBlock?.Invoke(this, EventArgs.Empty);
        }
    }

    public void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void ReadAllMethodParameters()
    {
        Methods = new List<ApiEntry>();
        QForm.api_get(Methods);
    }

    [RelayCommand]
    private void ClearCodeBlocks()
    {
        _codeBlocks?.Clear();
        ClearedCodeBlocks?.Invoke(this, EventArgs.Empty);
    }

    //public void OpenEditCollectionWindow(ApiEntry entry) {
        

    //    var dialog = new EditCollectionDialog();
    //    dialog.XamlRoot = this.XamlRoot;
    //    var result = await dialog.ShowAsync();

    //    if (result == ContentDialogResult.Primary)
    //    {
    //        Debug.WriteLine("Pressed OK!");
    //    }
    //    else if (result == ContentDialogResult.Secondary)
    //    {
    //        Debug.WriteLine("Pressed Cancel!");
    //    }
    //}

    public void OnModifingApiEntryChanged(ApiEntry entry)
    {

    }

}
