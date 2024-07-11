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

    private ApiEntry _selectedMethod;
    public ApiEntry SelectedMethod
    {
        get => _selectedMethod;
        set
        {
            if (_selectedMethod != value)
            {
                _selectedMethod = value;
                OnPropertyChanged();
                AddToCodeBlocks(_selectedMethod);
            }
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

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void ReadAllMethodParameters()
    {
        Methods = new List<ApiEntry>();
        QForm.api_get(Methods);
    }

    private void AddToCodeBlocks(ApiEntry method)
    {
        _codeBlocks.Add(method);
        AddedNewCodeBlock?.Invoke(this, EventArgs.Empty);
    }

    [RelayCommand]
    private void ClearCodeBlocks()
    {
        _codeBlocks?.Clear();
        ClearedCodeBlocks?.Invoke(this, EventArgs.Empty);
    }
}
