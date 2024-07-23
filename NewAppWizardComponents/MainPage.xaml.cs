
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Windows.UI.Core;


namespace NewAppWizardComponents;

public sealed partial class MainPage : Page
{
    public MainPageVM mainPageVM;
    
    private CoreDispatcher dispatcher;

    SolidColorBrush? codeTypeBrush;
    SolidColorBrush? codeDefaultBrush;
    SolidColorBrush? codeKeywordBrush;
    SolidColorBrush? codeBracketsBrush;
    SolidColorBrush? codeValueBrush;
    SolidColorBrush? codeMethodBrush;

    private Border? _lastSelectedBlock = null;
    private readonly Brush _defaultBackground = new SolidColorBrush(Windows.UI.Color.FromArgb(0, 0, 0, 0));
    private readonly Brush _selectedBackground = new SolidColorBrush(Windows.UI.Color.FromArgb(50, 0, 120, 210));

    private bool _isResizing = false;
    private double _initialPosition;
    private RowDefinition _firstMovedGrip;
    private RowDefinition _secondMovedGrip;

    public MainPage()
    {
        this.InitializeComponent();

        mainPageVM = new MainPageVM();
        this.DataContext = mainPageVM;

       

        codeTypeBrush = Resources["CodeType"] as SolidColorBrush;
        codeDefaultBrush = Resources["CodeDefault"] as SolidColorBrush;
        codeKeywordBrush = Resources["CodeKeyword"] as SolidColorBrush;
        codeBracketsBrush = Resources["CodeBrackets"] as SolidColorBrush;
        codeValueBrush = Resources["CodeValue"] as SolidColorBrush;
        codeMethodBrush = Resources["CodeMethod"] as SolidColorBrush;

        if (codeTypeBrush == null || codeDefaultBrush == null || codeKeywordBrush == null || codeBracketsBrush == null || codeValueBrush == null || codeMethodBrush == null)
        {
            throw new InvalidOperationException("One or more brushes was not found!");
        }

        mainPageVM.AddedNewCodeBlock += AddNewCodeBlock;
        mainPageVM.ClearedCodeBlocks += ClearCodeBlocks;

    }

    public void AddNewCodeBlock(object sender, EventArgs e) {
        Border newBlock = CreateObjectInitializedBlock(CodeBlocks, mainPageVM.CodeBlocks.Last(), mainPageVM.CodeBlocks.Count, false);
        CodeBlocks.Children.Add(newBlock);
        ScrollToCodeBlock(newBlock);
    }

    public Border CreateObjectInitializedBlock(StackPanel codeContainer, ApiEntry entry, int num, bool editingMode)
    {
        var newCodeLines = new TextBlock();

        if (entry.arg_type != null)
        {
            newCodeLines.Inlines.Add(new Run { Text = $"{entry.arg_type.Name[1..]} ", Foreground = codeTypeBrush });
            newCodeLines.Inlines.Add(new Run { Text = $"arg{num} ", Foreground = codeDefaultBrush });
            newCodeLines.Inlines.Add(new Run { Text = "= new", Foreground = codeKeywordBrush });
            newCodeLines.Inlines.Add(new Run { Text = "() {\n", Foreground = codeBracketsBrush });

            PropertyInfo[] properties = entry.arg_type.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType)) continue;

                var value = property.GetValue(entry.arg_value);

                newCodeLines.Inlines.Add(new Run { Text = $"\t{property.Name} ", Foreground = codeDefaultBrush });
                newCodeLines.Inlines.Add(new Run { Text = $"= ", Foreground = codeKeywordBrush });
                newCodeLines.Inlines.Add(new Run { Text = $"{value}", Foreground = codeValueBrush });
                newCodeLines.Inlines.Add(new Run { Text = $",\n", Foreground = codeDefaultBrush });
            }

            newCodeLines.Inlines.Add(new Run { Text = "}", Foreground = codeBracketsBrush });
            newCodeLines.Inlines.Add(new Run { Text = ";\n", Foreground = codeDefaultBrush });

        }

        if (entry.ret_type != null)
        {
            newCodeLines.Inlines.Add(new Run { Text = $"{entry.ret_type.Name[1..]} ", Foreground = codeTypeBrush });
            newCodeLines.Inlines.Add(new Run { Text = $"ret{num} ", Foreground = codeDefaultBrush });
            newCodeLines.Inlines.Add(new Run { Text = $"= ", Foreground = codeKeywordBrush });
        }

        newCodeLines.Inlines.Add(new Run { Text = "qform.", Foreground = codeDefaultBrush });
        newCodeLines.Inlines.Add(new Run { Text = $"{entry.Name}", Foreground = codeMethodBrush });
        newCodeLines.Inlines.Add(new Run { Text = "(", Foreground = codeBracketsBrush });

        if (entry.arg_type != null) { newCodeLines.Inlines.Add(new Run { Text = $"arg{num}", Foreground = codeDefaultBrush }); }

        newCodeLines.Inlines.Add(new Run { Text = ")", Foreground = codeBracketsBrush });
        newCodeLines.Inlines.Add(new Run { Text = ";", Foreground = codeDefaultBrush });

        newCodeLines.Style = (Style)Resources["CodeBlock"];
        newCodeLines.AllowFocusOnInteraction = true;
        newCodeLines.GotFocus += CodeBlockGotFocus;


        var newBlock = new Border();

        newBlock.Child = newCodeLines;
        newBlock.Style = (Style)Resources["CodeBlockBorder"];
        if (num == 1) { newBlock.Margin = new Thickness(0, 0, 0, 0); }

        newBlock.Tag = new ExpandedEntry(entry, num);

        SetCodeBlockSelection(newBlock);
        if (!editingMode)
            ShowParameters(entry);

        return newBlock;
    }
    private Border CreateStepByStepInitializedBlock(bool editingMode)
    {
        var newCodeLines = new TextBlock();

        var tag = _lastSelectedBlock.Tag as ExpandedEntry;
        ApiEntry entry = tag.apiEntry;
        int id = tag.index;

        if (entry.arg_type != null)
        {
            newCodeLines.Inlines.Add(new Run { Text = $"{entry.arg_type.Name[1..]} ", Foreground = codeTypeBrush });
            newCodeLines.Inlines.Add(new Run { Text = $"arg{id} ", Foreground = codeDefaultBrush });
            newCodeLines.Inlines.Add(new Run { Text = "= new ", Foreground = codeKeywordBrush });
            newCodeLines.Inlines.Add(new Run { Text = $"{entry.arg_type.Name[1..]}", Foreground = codeTypeBrush });
            newCodeLines.Inlines.Add(new Run { Text = "()", Foreground = codeBracketsBrush });
            newCodeLines.Inlines.Add(new Run { Text = ";\n", Foreground = codeDefaultBrush });

            PropertyInfo[] properties = entry.arg_type.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                var value = property.GetValue(entry.arg_value);

                if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                {
                    var val = value as IList;
                    var elementType = property.PropertyType.GetGenericArguments()[0];

                    if (elementType == typeof(int) || elementType == typeof(double) ||
                        elementType == typeof(bool) || elementType == typeof(string)) 
                    {
                        foreach(var v in val)
                        {
                            newCodeLines.Inlines.Add(new Run { Text = $"arg{id}.{property.Name}.", Foreground = codeDefaultBrush});
                            newCodeLines.Inlines.Add(new Run { Text = "Add", Foreground = codeMethodBrush });
                            newCodeLines.Inlines.Add(new Run { Text = "(", Foreground = codeBracketsBrush });
                            newCodeLines.Inlines.Add(new Run { Text = $"{v}", Foreground = codeValueBrush });
                            newCodeLines.Inlines.Add(new Run { Text = ")", Foreground = codeBracketsBrush });
                            newCodeLines.Inlines.Add(new Run { Text = ";\n", Foreground = codeDefaultBrush });
                        }
                    }
                    else
                    {
                        for (int i = 0; i < val.Count; i++)
                        {
                            string objectName = val[i].GetType().Name[1..];
                            newCodeLines.Inlines.Add(new Run { Text = $"{objectName} ", Foreground = codeTypeBrush });
                            newCodeLines.Inlines.Add(new Run { Text = $"arg{id + 1}_object{i + 1}", Foreground = codeDefaultBrush });
                            newCodeLines.Inlines.Add(new Run { Text = "= new ", Foreground = codeKeywordBrush });
                            newCodeLines.Inlines.Add(new Run { Text = $"{objectName}", Foreground = codeTypeBrush });
                            newCodeLines.Inlines.Add(new Run { Text = "()", Foreground = codeBracketsBrush });
                            newCodeLines.Inlines.Add(new Run { Text = ";\n", Foreground = codeDefaultBrush });

                            PropertyInfo[] innerProperties = val[i].GetType().GetProperties();
                            foreach( PropertyInfo innerProperty in innerProperties)
                            {
                                newCodeLines.Inlines.Add(new Run { Text = $"{objectName}.", Foreground = codeDefaultBrush });
                                newCodeLines.Inlines.Add(new Run { Text = $"{innerProperty.Name} ", Foreground = codeDefaultBrush });
                                newCodeLines.Inlines.Add(new Run { Text = $"= ", Foreground = codeKeywordBrush });
                                newCodeLines.Inlines.Add(new Run { Text = $"{innerProperty.GetValue(val[i])}", Foreground = codeValueBrush });
                                newCodeLines.Inlines.Add(new Run { Text = $";\n", Foreground = codeDefaultBrush });
                            }
                        }
                    }

                }
                else
                {
                    newCodeLines.Inlines.Add(new Run { Text = $"arg{id}.", Foreground = codeDefaultBrush });
                    newCodeLines.Inlines.Add(new Run { Text = $"{property.Name} ", Foreground = codeDefaultBrush });
                    newCodeLines.Inlines.Add(new Run { Text = $"= ", Foreground = codeKeywordBrush });
                    newCodeLines.Inlines.Add(new Run { Text = $"{value}", Foreground = codeValueBrush });
                    newCodeLines.Inlines.Add(new Run { Text = $";\n", Foreground = codeDefaultBrush });

                }
            }
        }

        if (entry.ret_type != null)
        {
            newCodeLines.Inlines.Add(new Run { Text = $"{entry.ret_type.Name[1..]} ", Foreground = codeTypeBrush });
            newCodeLines.Inlines.Add(new Run { Text = $"ret{id} ", Foreground = codeDefaultBrush });
            newCodeLines.Inlines.Add(new Run { Text = $"= ", Foreground = codeKeywordBrush });
        }

        newCodeLines.Inlines.Add(new Run { Text = "qform.", Foreground = codeDefaultBrush });
        newCodeLines.Inlines.Add(new Run { Text = $"{entry.Name}", Foreground = codeMethodBrush });
        newCodeLines.Inlines.Add(new Run { Text = "(", Foreground = codeBracketsBrush });

        if (entry.arg_type != null) { newCodeLines.Inlines.Add(new Run { Text = $"arg{id}", Foreground = codeDefaultBrush }); }

        newCodeLines.Inlines.Add(new Run { Text = ")", Foreground = codeBracketsBrush });
        newCodeLines.Inlines.Add(new Run { Text = ";", Foreground = codeDefaultBrush });

        newCodeLines.Style = (Style)Resources["CodeBlock"];
        newCodeLines.AllowFocusOnInteraction = true;
        newCodeLines.GotFocus += CodeBlockGotFocus;

        var newBlock = new Border();

        newBlock.Child = newCodeLines;
        newBlock.Style = (Style)Resources["CodeBlockBorder"];
        if (id == 1) { newBlock.Margin = new Thickness(0, 0, 0, 0); }

        newBlock.Tag = new ExpandedEntry(entry, id, true);

        SetCodeBlockSelection(newBlock);
        if (!editingMode)
            ShowParameters(entry);

        return newBlock;
    }

    public void ClearCodeBlocks(object sender, EventArgs e)
    {
        CodeBlocks.Children.Clear();
        ClearShownParameters();
    }

    private void ShowParameters(ApiEntry entry, ParameterTypes type)
    {
        Type parameters;
        Grid dest;

        if (type == ParameterTypes.Input)
        {
            parameters = entry.arg_type;
            dest = InputParametersGrid;
        }
        else
        {
            parameters = entry.ret_type;
            dest = OutputParametersGrid;
        }

        if (parameters != null)
        {
            PropertyInfo[] properties = parameters.GetProperties();

            var rows = dest.RowDefinitions.Count;
            for (int i = 0; i < properties.Length - rows; i++)
            {
                dest.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            }

            for (int i = 0; i < properties.Length; i++)
            { 

                var property = properties[i];
                var propertyNameTextBlock = new TextBlock { Text = property.Name, Style = (Style)Resources["ParameterName"] };
                var propertyNameVisual = new Border { Style = (Style)Resources["ParameterNameBorder"] };
                propertyNameVisual.Child = propertyNameTextBlock;

                var propertyValueControl = CreateControlForProperty(
                    property, 
                    property.GetValue(type == ParameterTypes.Input ? entry.arg_value : entry.ret_value), 
                    entry, 
                    type == ParameterTypes.Output
                );

                Grid.SetRow(propertyNameVisual, i);
                Grid.SetColumn(propertyNameVisual, 0);
                dest.Children.Add(propertyNameVisual);

                Grid.SetRow(propertyValueControl, i);
                Grid.SetColumn(propertyValueControl, 1);
                dest.Children.Add(propertyValueControl);
            }
        }
    }

    private void ClearShownParameters()
    {
        InputParametersGrid.Children.Clear();
        OutputParametersGrid.Children.Clear();
    }

    private FrameworkElement CreateControlForProperty(PropertyInfo property, object value, ApiEntry entry, bool isResult)
    {
        // ComboBox True/False
        if (property.PropertyType == typeof(bool))
        {
            var comboBox = new ComboBox();

            comboBox.Items.Add("False");
            comboBox.Items.Add("True");

            comboBox.SelectedIndex = Convert.ToInt32(value);
            comboBox.Tag = property;

            
            comboBox.SelectionChanged += (s, e) =>
            {
                if (isResult) property.SetValue(entry.ret_value, Convert.ToBoolean(comboBox.SelectedIndex));
                else property.SetValue(entry.arg_value, Convert.ToBoolean(comboBox.SelectedIndex));

                EditCodeBlock();
            };

            comboBox.Style = (Style)Resources["ParameterValueComboBox"];

            return comboBox;
        }
        // ComboBox ObjectTypeEnum
        else if (property.PropertyType.IsEnum)
        {
            var comboBox = new ComboBox();
            foreach (var enumValue in Enum.GetValues(property.PropertyType))
            {
                comboBox.Items.Add(enumValue.ToString());
            }
            if (isResult)
            {
                comboBox.SelectedItem = property.GetValue(entry.ret_value)?.ToString();
            }
            else
                comboBox.SelectedItem = property.GetValue(entry.arg_value)?.ToString();
            comboBox.Tag = property;

            comboBox.SelectionChanged += (s, e) =>
            {
                var selectedValue = comboBox.SelectedItem.ToString();
                var enumValue = Enum.Parse(property.PropertyType, selectedValue);
                
                if (isResult) property.SetValue(entry.ret_value, enumValue);
                else property.SetValue(entry.arg_value, enumValue);

                EditCodeBlock();
            };

            comboBox.Style = (Style)Resources["ParameterValueComboBox"];

            return comboBox;
        }
        //Button Collection
        else if (property.PropertyType.GetInterfaces().Contains(typeof(IList)))
        {
            var button = new Button();
            button.Content = "View items";
            button.Style = (Style)Resources["EditCollectionButton"];

            button.Click += (s, e) => OnChangeCollection(entry, property, isResult);

            return button;
        }
        //TextBox ChangebleProperty
        else
        {
            var textBox = new TextBox();
            textBox.Text = value?.ToString();
            textBox.Tag = property;

             
            textBox.TextChanged += (sender, e) => OnValueChanged(property, textBox.Text, entry, textBox, isResult);

            textBox.Style = (Style)Resources["ParameterValueTextBox"];

            return textBox;
        }
    }

    private void OnValueChanged(System.Reflection.PropertyInfo property, string newValue, ApiEntry entry, Control visualBlock, bool isResult)
    {
        try
        {
            var targetType = property.PropertyType;

            if (targetType == typeof(int))
            {
                property.SetValue(isResult ? entry.ret_value : entry.arg_value, int.Parse(newValue));
            }
            else if (targetType == typeof(double))
            {
                property.SetValue(isResult ? entry.ret_value : entry.arg_value, double.Parse(newValue));
            }
            else if (targetType == typeof(string))
            {
                property.SetValue(isResult ? entry.ret_value : entry.arg_value, newValue);
            }
            else
            {
                Debug.WriteLine("A property of a non-built-in type was changed, but this could not have any consequences.");
            }

            EditCodeBlock();
            visualBlock.Background = (SolidColorBrush)Resources["Transparent"];
        }
        catch
        {
            visualBlock.Background = (SolidColorBrush)Resources["FailStatus"];
        }
    }

    void EditCodeBlock()
    {
        var meta = _lastSelectedBlock.Tag as ExpandedEntry;

        Border updatedBlock;
        if (meta.isConnectedBlockSequentialInitialized)
        {

            updatedBlock = CreateStepByStepInitializedBlock(true);
        }
        else
        {
            updatedBlock = CreateObjectInitializedBlock(CodeBlocks, meta.apiEntry, meta.index, true);
        }

        CodeBlocks.Children[meta.index - 1] = updatedBlock;

    }

    private async void OnChangeCollection(ApiEntry entry, PropertyInfo property, bool isResult)
    {
        var dialog = new EditCollectionDialog(entry, property, isResult);
        dialog.XamlRoot = this.XamlRoot;
        var result = await dialog.ShowAsync();

        if (result == ContentDialogResult.Primary)
        {
            if (!isResult)
                CodeBlocks.Children[CodeBlocks.Children.Count - 1] = CreateStepByStepInitializedBlock(true);
        }
        else if (result == ContentDialogResult.Secondary)
        {
        }
    }


    private void SetCodeBlockSelection(Border newBlock)
    {
        if (_lastSelectedBlock != null)
        {
            _lastSelectedBlock.Background = _defaultBackground;
        }

        _lastSelectedBlock = newBlock;
        _lastSelectedBlock.Background = _selectedBackground;
    }

    private async void ScrollToCodeBlock(Border block)
    {
        await Task.Delay(100);

        double posY = block.TransformToVisual(CodeView.Content as UIElement).TransformPoint(new Windows.Foundation.Point()).Y;
        CodeView.ChangeView(null, posY, null);
    }

    private void CodeBlockGotFocus(object sender, RoutedEventArgs e)
    {
        var selectedBlock = (Border)VisualTreeHelper.GetParent((TextBlock)sender);
        if (selectedBlock == null) return;
        
        SetCodeBlockSelection(selectedBlock);
        ScrollToCodeBlock(selectedBlock);

        var entry = (selectedBlock.Tag as ExpandedEntry).apiEntry;
        ShowParameters(entry);
        
    }

    private void ShowParameters(ApiEntry entry) 
    {
        ClearShownParameters();

        if (entry.arg_type != null) ShowParameters(entry, ParameterTypes.Input);
        if (entry.ret_type != null) ShowParameters(entry, ParameterTypes.Output);
    }

    public void OnItemClick(object sender, ItemClickEventArgs e)
    {
        var clickedItem = e.ClickedItem as ApiEntry;

        if (!clickedItem.menu_only)
        {

            mainPageVM.AddToCodeBlocks(clickedItem);
        }
        else
        {
            ShowMessageBox("To add this method use the context menu in QForm");
        }

    }
    private async void ShowMessageBox(string content) {
        var dialog = new MessageBox(content);
        dialog.XamlRoot = this.XamlRoot;

        await dialog.ShowAsync();
    }

    private void Separator_PointerPressed(object sender, PointerRoutedEventArgs e)
    {
        _isResizing = true;
        _initialPosition = e.GetCurrentPoint(MethodParametersGrid).Position.Y;
        var separator = sender as Border;
        int columnIndex = int.Parse(separator.Tag.ToString());
        _firstMovedGrip = MethodParametersGrid.RowDefinitions[columnIndex - 1];
        _secondMovedGrip = MethodParametersGrid.RowDefinitions[columnIndex + 1];
        separator.CapturePointer(e.Pointer);
    }

    private void Separator_PointerMoved(object sender, PointerRoutedEventArgs e)
    {
        if (!_isResizing)
            return;

        double currentPosition = e.GetCurrentPoint(MethodParametersGrid).Position.Y;
        double delta = currentPosition - _initialPosition;

        double newFirstHeight = _firstMovedGrip.ActualHeight + delta;
        double newSecondHeight = _secondMovedGrip.ActualHeight - delta;

        

        if (newFirstHeight > 16  && newSecondHeight > 16 )
        {
            _firstMovedGrip.Height = new GridLength(newFirstHeight);
            _secondMovedGrip.Height = new GridLength(newSecondHeight);
            _initialPosition = currentPosition;
        }
    }

    private void Separator_PointerReleased(object sender, PointerRoutedEventArgs e)
    {
        _isResizing = false;
        (sender as UIElement).ReleasePointerCapture(e.Pointer);
    }

    private void TreeView_ItemInvoked(TreeView sender, TreeViewItemInvokedEventArgs args)
    {
        var clickedItem = (args.InvokedItem as TreeViewItemModel);

        if (clickedItem.Children.Count != 0) return;

        if (!clickedItem.apiEntry.menu_only)
        {

            mainPageVM.AddToCodeBlocks(clickedItem.apiEntry);
        }
        else
        {
            ShowMessageBox("To add this method use the context menu in QForm");
        }
    }
}

public enum ParameterTypes
{
    Input = 0,
    Output = 1,
}
