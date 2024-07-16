using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

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

    private ObservableCollection<UIElement> _inputParameters;
    public ObservableCollection<UIElement> InputParameters
    {
        get => _inputParameters;
        set
        {
            _inputParameters = value;
            OnPropertyChanged();
        }
    }

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
            _codeBlocks.Add(method);
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

    
}
