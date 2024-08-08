using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml.Controls;
using Newtonsoft.Json;
using Windows.Services.Maps;
using Windows.UI.Core;

namespace NewAppWizardComponents;

public partial class MainPageVM : INotifyPropertyChanged
{
    private List<ApiEntry> _methods;
    public List<ApiEntry> Methods
    {
        get => _methods;
        set
        {
            _methods = value;
            OnPropertyChanged();
            FilterMethodsList(null);
        }
    }

    private ObservableCollection<ApiEntry> _filteredMethods;
    public ObservableCollection<ApiEntry> FilteredMethods
    {
        get => _filteredMethods;
        set
        {
            _filteredMethods = value;
            OnPropertyChanged();
        }
    }

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

    private ObservableCollection<TreeViewItemModel> _treeViewItems;
    public ObservableCollection<TreeViewItemModel> TreeViewItems
    {
        get => _treeViewItems;
        set
        {
            _treeViewItems = value;
            OnPropertyChanged();
        }
    }

    private Visibility _treeMethodsVisibility = Visibility.Visible;
    public Visibility TreeMethodsVisibility
    {
        get => _treeMethodsVisibility;
        set
        {
            if (_treeMethodsVisibility != value)
            {
                _treeMethodsVisibility = value;
                OnPropertyChanged();
            }
        }
    }

    private Visibility _listMethodsVisibility = Visibility.Collapsed;
    public Visibility ListMethodsVisibility
    {
        get => _listMethodsVisibility;
        set
        {
            if (_listMethodsVisibility != value)
            {
                _listMethodsVisibility = value;
                OnPropertyChanged();
            }
        }
    }

    // Объеденить это дело во что-то нормальное

    private Visibility _inputVisibility = Visibility.Collapsed;
    public Visibility InputVisibility
    {
        get => _inputVisibility;
        set
        {
            if (_inputVisibility != value)
            {
                _inputVisibility = value;
                OnPropertyChanged();
            }
        }
    }

    private Visibility _outputVisibility = Visibility.Collapsed;
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

    private Visibility _docViewVisibility = Visibility.Collapsed;
    public Visibility DocViewVisibility
    {
        get => _docViewVisibility;
        set
        {
            _docViewVisibility = value;
            OnPropertyChanged();
        }
    }

    public List<Method> Docs {  get; set; }

    private Method _selectedMethod = null;
    public Method SelectedMethod
    {
        get => _selectedMethod;
        set
        {
            _selectedMethod = value;
            OnPropertyChanged();
        }
    }


    public int lastSelectedBlock { get; set; }

    private string? _currentSession = null;
    public string? CurrentSession
    {
        get => _currentSession;
        set
        {
            _currentSession = value;
            OnPropertyChanged();
        }
    }

    private Visibility _statusBarVisibility = Visibility.Collapsed;
    public Visibility StatusBarVisibility
    {
        get => _statusBarVisibility;
        set
        {
            _statusBarVisibility = value;
            OnPropertyChanged();
        }
    }

    public QFormManager qformManager;
    private ProjectManager _projectManager;

    public event PropertyChangedEventHandler PropertyChanged;
    public event EventHandler<CodeGenerationMode> AddedNewCodeBlock;
    public event EventHandler ClearedCodeBlocks;

    public MainPageVM()
    {
        CodeBlocks = new ObservableCollection<ApiEntry>();
        FilteredMethods = new ObservableCollection<ApiEntry>();
        ReadAllMethodParameters();
        PopulateTreeViewItems();

        _projectManager = new ProjectManager(Methods);
        qformManager = new QFormManager();
    }

    public void AddToCodeBlocks(ApiEntry method, bool loadingProject = false, CodeGenerationMode generationMode = CodeGenerationMode.ObjectInit)
    {
        if (method != null)
        {
            _codeBlocks.Add(loadingProject ? method : method.Clone());
            AddedNewCodeBlock?.Invoke(this, generationMode);
        }
    }

    public void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void UpdateDocsVisibility(ApiEntry entry)
    {
        var docMethod = Docs.Find(m => m.Name == entry.Name);
        SelectedMethod = docMethod;

        SetVisibility();

    }

    private void SetVisibility()
    {
        DocViewVisibility = Visibility.Visible;

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

    public void OnSessionInfoChanged(SessionInfo sessionInfo)
    {
        CurrentSession = sessionInfo.ToString();
        StatusBarVisibility = Visibility.Visible;
    }

    private void ReadAllMethodParameters()
    {
        Methods = new List<ApiEntry>();
        QForm.api_get(Methods);
        FilteredMethods = new ObservableCollection<ApiEntry>(Methods);
        PopulateTreeViewItems();

        string jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets/api_new.json");
        var allMethods = JsonConvert.DeserializeObject<AllMethods>(File.ReadAllText(jsonFilePath));
        Docs = allMethods.Functions;
    }

    private void PopulateTreeViewItems()
    {
        var categoryDict = new Dictionary<string, TreeViewItemModel>();

        foreach (var method in Methods)
        {
            foreach (var group in method.groups)
            {
                if (!categoryDict.ContainsKey(group.ru))
                {
                    categoryDict[group.ru] = new TreeViewItemModel(group.ru, method);
                }
                categoryDict[group.ru].Children.Add(new TreeViewItemModel(method.Name, method));
            }
        }

        TreeViewItems = new ObservableCollection<TreeViewItemModel>(
            categoryDict.Values.OrderBy(c => c.Name)
        );

        foreach (var category in TreeViewItems)
        {
            var sortedChildren = category.Children.OrderBy(c => c.Name).ToList();
            category.Children.Clear();
            foreach (var child in sortedChildren)
            {
                category.Children.Add(child);
            }
        }
    }

    [RelayCommand]
    private void ClearCodeBlocks()
    {
        _ClearCodeBlocks();
    }

    private void _ClearCodeBlocks()
    {
        _codeBlocks?.Clear();
        ClearedCodeBlocks?.Invoke(this, EventArgs.Empty);
    }

    public void OnModifingApiEntryChanged(ApiEntry entry)
    {
    }

    public void FilterMethodsListTextChanged(object sender, TextChangedEventArgs e)
    {
        FilterMethodsList(sender as TextBox);

    }

    private void FilterMethodsList(TextBox? viewFilter)
    {
        string filterText;

        if (viewFilter == null) 
            filterText = "";
        else 
            filterText = viewFilter.Text?.ToLowerInvariant();
        if (string.IsNullOrEmpty(filterText))
        {
            FilteredMethods = new ObservableCollection<ApiEntry>(Methods);
        }
        else
        {
            var filtered = Methods.Where(m => m.Name.ToLowerInvariant().Contains(filterText)).ToList();
            FilteredMethods = new ObservableCollection<ApiEntry>(filtered);
        }
    }

    public void FilterMethodsTreeTextChanged(object sender, TextChangedEventArgs e)
    {
        FilterMethodsTree(sender as TextBox);
    }

    private void FilterMethodsTree(TextBox? viewFilter)
    {
        string filterText = viewFilter?.Text?.ToLowerInvariant() ?? string.Empty;

        foreach (var item in TreeViewItems)
        {
            SetVisibility(item, filterText);
        }
    }

    private bool SetVisibility(TreeViewItemModel item, string filterText)
    {
        bool isVisible = item.Name.ToLowerInvariant().Contains(filterText);

        foreach (var child in item.Children)
        {
            bool childIsVisible = SetVisibility(child, filterText);
            isVisible |= childIsVisible;
        }

        item.IsVisible = isVisible;

        return isVisible;
    }

    public void SaveCodeLines(string codeLines, string progLang)
    {
        _projectManager.SaveCode(codeLines, progLang);
    }

    public async Task LoadApiEntriesFromScm()
    {
        try
        {
            List<ApiEntry> readData = await _projectManager.LoadCode();

            _ClearCodeBlocks();



            foreach (ApiEntry entry in readData)
            {
                AddToCodeBlocks(entry, loadingProject: true, generationMode: CodeGenerationMode.StepByStep);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
    }

    internal void ChangeApiFunctionsVisibility(int selectedIndex)
    {
        TreeMethodsVisibility = selectedIndex == 0 ? Visibility.Visible : Visibility.Collapsed;
        ListMethodsVisibility = selectedIndex == 1 ? Visibility.Visible : Visibility.Collapsed;
    }
}


public class TreeViewItemModel : INotifyPropertyChanged
{
    public string Name { get; set; }
    public ObservableCollection<TreeViewItemModel> Children { get; set; }
    public ApiEntry ApiEntry { get; set; }

    private bool _isVisible;
    public bool IsVisible
    {
        get => _isVisible;
        set
        {
            if (_isVisible != value)
            {
                _isVisible = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ViewVisibility));
            }
        }
    }

    public Visibility ViewVisibility => IsVisible ? Visibility.Visible : Visibility.Collapsed;

    public TreeViewItemModel(string name, ApiEntry apiEntry)
    {
        Name = name;
        Children = new ObservableCollection<TreeViewItemModel>();
        ApiEntry = apiEntry;
        IsVisible = true; // Default to visible
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
