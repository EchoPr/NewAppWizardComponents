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

    public int lastSelectedBlock { get; set; }

    private ProjectManager _projectManager;

    public event PropertyChangedEventHandler PropertyChanged;
    public event EventHandler AddedNewCodeBlock;
    public event EventHandler ClearedCodeBlocks;

    public MainPageVM()
    {
        _projectManager = new ProjectManager();

        CodeBlocks = new ObservableCollection<ApiEntry>();
        FilteredMethods = new ObservableCollection<ApiEntry>();
        ReadAllMethodParameters();
        PopulateTreeViewItems();
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
        FilteredMethods = new ObservableCollection<ApiEntry>(Methods);
        PopulateTreeViewItems(); 
    }

    private void PopulateTreeViewItems()
    {
        var categoryDict = new Dictionary<string, TreeViewItemModel>();

        foreach (var method in Methods)
        {
            foreach (var group in method.groups)
            {
                if (!categoryDict.ContainsKey(group.key))
                {
                    categoryDict[group.key] = new TreeViewItemModel(group.key, method);
                }
                categoryDict[group.key].Children.Add(new TreeViewItemModel(method.Name, method));
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
