using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using System.Reflection.Metadata;

namespace NewAppWizardComponents;

public class MainPageVM : INotifyPropertyChanged
{
    private List<Method> _methods;
    public List<Method> Methods
    {
        get => _methods;
        set
        {
            _methods = value;
            OnPropertyChanged();
        }
    }

    private Visibility _inputVisibility;
    public Visibility InputVisibility
    {
        get => _inputVisibility;
        private set
        {
            if (_inputVisibility != value)
            {
                _inputVisibility = value;
                OnPropertyChanged();
            }
        }
    }

    private Visibility _outputVisibility;
    public Visibility OutputVisibility
    {
        get => _outputVisibility;
        private set
        {
            if (_outputVisibility != value)
            {
                _outputVisibility = value;
                OnPropertyChanged();
            }
        }
    }

    private Method _selectedMethod = null;
    public Method SelectedMethod
    {
        get => _selectedMethod;
        set
        {
            _selectedMethod = value;
            SetVisibility();
            OnPropertyChanged();
            OnPropertyChanged(nameof(SelectedMethodInputParameters));
            OnPropertyChanged(nameof(SelectedMethodOutputParameters));
        }
    }

    private void SetVisibility()
    {
        if (SelectedMethod != null)
        {
            InputVisibility = SelectedMethod.Input.Count == 0 ? Visibility.Collapsed : Visibility.Visible;
            OutputVisibility = SelectedMethod.Output.Count == 0 ? Visibility.Collapsed : Visibility.Visible;
        }
        else
        {
            InputVisibility = Visibility.Collapsed;
            OutputVisibility = Visibility.Collapsed;
        }
    }

    public List<Parameters> SelectedMethodInputParameters => SelectedMethod.Input;
    public List<Parameters> SelectedMethodOutputParameters => SelectedMethod.Output;


    public MainPageVM()
    {
        ReadAllMethodParameters();
    }

    private void ReadAllMethodParameters()
    {
        string jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets/api_new.json");
        var allMethods = JsonConvert.DeserializeObject<AllMethods>(File.ReadAllText(jsonFilePath));
        Methods = allMethods.Functions;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
