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

    public event PropertyChangedEventHandler PropertyChanged;
    public event EventHandler AddedNewCodeBlock;
    public event EventHandler ClearedCodeBlocks;

    public MainPageVM()
    {
        CodeBlocks = new ObservableCollection<ApiEntry>();
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
        PopulateTreeViewItems(); // Populate and sort TreeView items after loading data
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

        // Create TreeView collection and sort it by category keys
        TreeViewItems = new ObservableCollection<TreeViewItemModel>(
            categoryDict.Values.OrderBy(c => c.Name)
        );

        // Sort children of each category by name
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
}


public class TreeViewItemModel
{
    public string Name { get; set; }
    public ObservableCollection<TreeViewItemModel> Children { get; set; }
    public ApiEntry apiEntry { get; set; }

    public TreeViewItemModel(string name, ApiEntry apiEntry)
    {
        Name = name;
        Children = new ObservableCollection<TreeViewItemModel>();
        this.apiEntry = apiEntry;
    }
}
