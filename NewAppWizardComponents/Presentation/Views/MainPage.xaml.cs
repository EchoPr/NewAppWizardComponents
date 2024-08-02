
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Windows.Graphics.Printing.PrintTicket;
using Windows.Storage.Pickers;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Core.Preview;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace NewAppWizardComponents;

public sealed partial class MainPage : Page
{
    public MainPageVM mainPageVM;

    SolidColorBrush? codeTypeBrush;
    SolidColorBrush? codeDefaultBrush;
    SolidColorBrush? codeKeywordBrush;
    SolidColorBrush? codeBracketsBrush;
    SolidColorBrush? codeValueBrush;
    SolidColorBrush? codeMethodBrush;

    private List<Border> _selectedBlocks = new List<Border>();

    private bool _isResizing = false;
    private double _initialPosition;
    private RowDefinition _firstMovedGrip;
    private RowDefinition _secondMovedGrip;

    private bool _isShiftPressed;

    public MainPage()
    {
        this.InitializeComponent();

        mainPageVM = new MainPageVM();
        this.DataContext = mainPageVM;

        InitializeBrushes(); 

        this.KeyDown += MainPage_KeyDown;
        this.KeyUp += MainPage_KeyUp;
        mainPageVM.AddedNewCodeBlock += AddNewCodeBlock;
        mainPageVM.ClearedCodeBlocks += ClearCodeBlocks;
        mainPageVM.qformManager.ErrorMessageRequested += OnMessageBoxRequested;
        mainPageVM.qformManager.InvokationResultsReceived += OnInvokationResultsReceived;
        mainPageVM.qformManager.InvocationStarted += OnInvocationStarted;
        mainPageVM.qformManager.InvocationSuccess += OnInvocationSuccess;
        mainPageVM.qformManager.InvocationFailed += OnInvocationFailed;
    }

    private void OnInvocationFailed(object? sender, EventArgs e)
    {
        //InvokeProgressBar.Value = 0;
        InvokeProgressBar.ShowError = true;
    }

    private void OnInvocationSuccess(object? sender, EventArgs e)
    {
        InvokeProgressBar.IsIndeterminate = false;
        InvokeProgressBar.ShowError = false;
        InvokeProgressBar.Value = 100;
    }

    private void OnInvocationStarted(object? sender, EventArgs e)
    {
        InvokeProgressBar.Value = 0;
        InvokeProgressBar.IsIndeterminate = true;
        InvokeProgressBar.ShowError = false;
    }

    private void OnSessionInfoChanged(object? sender, SessionInfo e)
    {
        
    }

    private void OnInvokationResultsReceived(object? sender, EventArgs e)
    {
        ShowParameters((_selectedBlocks.Last().Tag as ExpandedEntry)?.apiEntry);
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        mainPageVM.qformManager.attachQForm();
    }

    private void InitializeBrushes()
    {
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
    }

    public void AddNewCodeBlock(object sender, CodeGenerationMode mode)
    {
        _AddNewCodeBlock(mainPageVM.CodeBlocks.Last(), mainPageVM.CodeBlocks.Count, mode, false);
    }
    private void _AddNewCodeBlock(ApiEntry entry, int entryNumber, CodeGenerationMode generationMode, bool isEditig)
    {
        Border newBlock = CreateViewCodeBlock(entry, entryNumber, generationMode, isEditig);
        CodeBlocks.Children.Add(newBlock);
        ScrollToCodeBlock(newBlock);
    }

    private Border CreateViewCodeBlock(ApiEntry entry, int entryNumber, CodeGenerationMode generationMode, bool isEditig)
    {
        string selectedLanguage = ((ComboBoxItem)LanguageComboBox.SelectedValue).Content.ToString();
        ICodeGenerator generator = CodeGeneratorFactory.GetGenerator(selectedLanguage);
        var generatedCode = generator.GenerateCodeEntry(entry, entryNumber, generationMode);

        var newCodeLines = new TextBlock();

        foreach (ViewCodeSample sample in generatedCode)
        {
            newCodeLines.Inlines.Add(new Run { Text = sample.content, Foreground = GetBrush(sample.type) });
        }

        newCodeLines.Style = (Style)Resources["CodeBlock"];
        newCodeLines.AllowFocusOnInteraction = true;
        newCodeLines.GotFocus += CodeBlockGotFocus;

        newCodeLines.KeyDown += (s, e) =>
        {
            if (e.Key == VirtualKey.Shift)
            {
                _isShiftPressed = true;
            }
        };

        newCodeLines.KeyUp += (s, e) =>
        {
            if (e.Key == VirtualKey.Shift)
            {
                _isShiftPressed = false;
            }
        };

        var newBlock = new Border();

        newBlock.Child = newCodeLines;
        newBlock.Style = (Style)Resources["CodeBlockBorder"];
        if (entryNumber == 1) { newBlock.Margin = new Thickness(0, 0, 0, 0); }

        newBlock.Tag = new ExpandedEntry(entry, entryNumber, generationMode == CodeGenerationMode.StepByStep);

        SetCodeBlockSelection(newBlock, multipleSelection: false);
        if (!isEditig)
            ShowParameters(entry);

        return newBlock;
    }

    private Brush? GetBrush(ViewCodeSampleType type)
    {
        return type switch
        {
            ViewCodeSampleType.Type => codeTypeBrush,
            ViewCodeSampleType.Default => codeDefaultBrush,
            ViewCodeSampleType.Keyword => codeKeywordBrush,
            ViewCodeSampleType.Value => codeValueBrush,
            ViewCodeSampleType.Method => codeMethodBrush,
            ViewCodeSampleType.Brackets => codeBracketsBrush,
            _ => codeDefaultBrush,
        };
    }

    public void ClearCodeBlocks(object sender, EventArgs e)
    {
        _ClearCodeBlocks();
    }

    private void _ClearCodeBlocks()
    {
        CodeBlocks.Children.Clear();
        ClearShownParameters();
        _selectedBlocks.Clear();
    }

    private void ShowParameters(ApiEntry entry, ParameterTypes type)
    {
        Type parameters;
        Grid dest;
        int start;

        if (type == ParameterTypes.Input)
        {
            parameters = entry.arg_type;
            dest = InputParametersGrid;
        }
        else
        {
            parameters = entry.ret_type;
            dest = OutputParametersGrid;

            Border statusNameBlock = CreatePropertyVisibleName("Status", true);

            var statusValueTextBlock = new TextBlock { Text = (entry.ret_value as Ret)?.invocationResultStatus ?? "", Style = (Style)Resources["ParameterName"] };
            var statusValueBlock = new Border { Style = (Style)Resources["ParameterNameBorder"], BorderThickness = new Thickness(1, 0, 0, 1) };
            statusValueBlock.Child = statusValueTextBlock;

            var propertyNameToolTip = new ToolTip { Content = statusValueTextBlock.Text, VerticalOffset = -40 };
            ToolTipService.SetToolTip(statusValueBlock, propertyNameToolTip);

            AddOnParametersGrid(dest, statusNameBlock, 0, 0);
            AddOnParametersGrid(dest, statusValueBlock, 0, 1);
        }

        if (parameters != null)
        {
            var factor = Convert.ToInt32(type == ParameterTypes.Output);
            PropertyInfo[] properties = parameters.GetProperties();


            var rows = dest.RowDefinitions.Count;
            for (int i = 0; i < properties.Length - rows + factor; i++)
            {
                dest.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            }

            for (int i = 0; i < properties.Length - factor; i++)
            {
                var property = properties[i];

                Border propertyNameBlock = CreatePropertyVisibleName(property.Name);

                var propertyValueControl = CreateControlForProperty(
                    property,
                    property.GetValue(type == ParameterTypes.Input ? entry.arg_value : entry.ret_value),
                    entry,
                    type == ParameterTypes.Output
                );

                AddOnParametersGrid(dest, propertyNameBlock, i + factor, 0);
                AddOnParametersGrid(dest, propertyValueControl, i + factor, 1);
            }
        }
    }

    private void ClearShownParameters()
    {
        InputParametersGrid.Children.Clear();
        OutputParametersGrid.Children.Clear();
    }

    private void AddOnParametersGrid(Grid grid, FrameworkElement block, int row, int col)
    {
        Grid.SetRow(block, row);
        Grid.SetColumn(block, col);
        grid.Children.Add(block);
    }

    private Border CreatePropertyVisibleName(string name, bool highlighted = false) 
    {
        var propertyTextBlock = new TextBlock { Text = name, Style = (Style)Resources["ParameterName"] };
        if (highlighted) propertyTextBlock.FontFamily = (FontFamily)Resources["SourceCodePro-Medium"];

        var propertyBlockVisual = new Border { Style = (Style)Resources["ParameterNameBorder"] };
        propertyBlockVisual.Child = propertyTextBlock;

        var propertyNameToolTip = new ToolTip { Content = propertyTextBlock.Text, VerticalOffset = -40 };
        ToolTipService.SetToolTip(propertyBlockVisual, propertyNameToolTip);

        return propertyBlockVisual;
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
        Debug.WriteLine("Action Was In EditCodeBlock");
        var meta = _selectedBlocks.Last().Tag as ExpandedEntry;

        Border updatedBlock;
        updatedBlock = CreateViewCodeBlock(
            meta.apiEntry,
            meta.index,
            meta.isConnectedBlockSequentialInitialized ? CodeGenerationMode.StepByStep : CodeGenerationMode.ObjectInit,
            true
        );

        CodeBlocks.Children[meta.index - 1] = updatedBlock;
    }

    private async void OnChangeCollection(ApiEntry entry, PropertyInfo property, bool isResult)
    {
        var dialog = new EditCollectionDialog(entry, property, isResult);
        dialog.XamlRoot = this.XamlRoot;
        var result = await dialog.ShowAsync();

        var meta = _selectedBlocks.Last().Tag as ExpandedEntry;

        if (result == ContentDialogResult.Primary)
        {
            if (!isResult)
                CodeBlocks.Children[(_selectedBlocks.Last().Tag as ExpandedEntry).index - 1] = CreateViewCodeBlock(meta.apiEntry, meta.index, CodeGenerationMode.StepByStep, true);
        }
        else if (result == ContentDialogResult.Secondary)
        {
        }
    }


    private void SetCodeBlockSelection(Border newBlock, bool multipleSelection)
    {
        foreach (Border block in _selectedBlocks)
        {
            block.Background = (Brush)Resources[multipleSelection ? "SelectedBlockSecondary" : "Transparent"];
        }

        if (!multipleSelection)
            _selectedBlocks.Clear();

        newBlock.Background = (Brush)Resources["SelectedBlockPrimary"];
        if (!_selectedBlocks.Contains(newBlock)) _selectedBlocks.Add(newBlock);

    }

    private async void ScrollToCodeBlock(Border block)
    {
        await Task.Delay(100);

        double posY = block.TransformToVisual(CodeView.Content as UIElement).TransformPoint(new Windows.Foundation.Point()).Y;
        CodeView.ChangeView(null, posY, null);
    }

    private void CodeBlockGotFocus(object sender, RoutedEventArgs e)
    {
        var shiftPressed = _isShiftPressed;

        var selectedBlock = (Border)VisualTreeHelper.GetParent((TextBlock)sender);
        if (selectedBlock == null) return;

        SetCodeBlockSelection(selectedBlock, shiftPressed);
        ScrollToCodeBlock(selectedBlock);

        var entry = (selectedBlock.Tag as ExpandedEntry).apiEntry;
        ShowParameters(entry);

    }

    private void MainPage_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == VirtualKey.Shift)
        {
            _isShiftPressed = true;
        }
    }

    private void MainPage_KeyUp(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == VirtualKey.Shift)
        {
            _isShiftPressed = false;
        }
    }

    private void ShowParameters(ApiEntry? entry)
    {
        if (entry == null)
        {
            Debug.WriteLine("[ShowParameters] entry wa null");
            return;
        }

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

    public void OnMessageBoxRequested(object sender, string message)
    {
        ShowConnectQFormDialog();
    }
    private async void ShowMessageBox(string content)
    {
        var dialog = new MessageBox(content);
        dialog.XamlRoot = this.XamlRoot;

        await dialog.ShowAsync();
    }

    private async void ShowConnectQFormDialog() 
    {
        var dialog = new ConnectQFormDialog(mainPageVM);
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



        if (newFirstHeight > 16 && newSecondHeight > 16)
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

        if (!clickedItem.ApiEntry.menu_only)
        {

            mainPageVM.AddToCodeBlocks(clickedItem.ApiEntry);
        }
        else
        {
            ShowMessageBox("To add this method use the context menu in QForm");
        }
    }
    private void LanguageSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (CodeBlocks == null) return;

        for (int i = 0; i < CodeBlocks.Children.Count; i++)
        {
            Border cb = CodeBlocks.Children[i] as Border;
            var meta = cb.Tag as ExpandedEntry;
            CodeBlocks.Children[i] = CreateViewCodeBlock(
                meta.apiEntry,
                meta.index,
                meta.isConnectedBlockSequentialInitialized ? CodeGenerationMode.StepByStep : CodeGenerationMode.ObjectInit,
                true
            );
        }
    }

    private void SaveSelectedBlocks(object sender, RoutedEventArgs e)
    {
        SaveCode(_selectedBlocks.OrderBy(b => (b.Tag as ExpandedEntry).index).Select(b => (b.Child as TextBlock).Text));
    }

    private void SaveProject(object sender, RoutedEventArgs e)
    {
        string selectedLanguage = "S-expr";

        ICodeGenerator codeGenerator = CodeGeneratorFactory.GetGenerator(selectedLanguage);

        List<string> generatedCodeList = new List<string>();
        foreach (Border codeBlock in CodeBlocks.Children)
        {
            var blockMetadata = codeBlock.Tag as ExpandedEntry;

            CodeGenerationMode generationMode = blockMetadata.isConnectedBlockSequentialInitialized
                ? CodeGenerationMode.StepByStep
                : CodeGenerationMode.ObjectInit;

            var generatedEntries = codeGenerator.GenerateCodeEntry(blockMetadata.apiEntry, blockMetadata.index, generationMode);
            string generatedCode = string.Join(null, generatedEntries.Select(entry => entry.content));
            generatedCodeList.Add(generatedCode);
        }

        SaveCode(generatedCodeList, "S-expr");
    }

    private void SaveCode(IEnumerable collection, string? lang = null)
    {
        string data = "";

        foreach (var line in collection)
        {
            data += line;
        }

        lang ??= ((ComboBoxItem)LanguageComboBox.SelectedItem).Content.ToString();
        mainPageVM.SaveCodeLines(data, lang);
    }

    private async void LoadProject(object sender, RoutedEventArgs e)
    {
        await mainPageVM.LoadApiEntriesFromScm();
    }

    private async void InvokeButtonCommand(object sender, RoutedEventArgs e)
    {
        if (_selectedBlocks.Count == 0)
        {
            ShowMessageBox("There are no selected bloks to invoke");
            return;
        }

        var lastSelectedBlock = _selectedBlocks.Last();
        var apiEntry = (lastSelectedBlock.Tag as ExpandedEntry).apiEntry;

        Debug.WriteLine("Start");
        await mainPageVM.qformManager.invokeMethod(apiEntry);
        Debug.WriteLine("End");
    }

    private void MenuConnectQForm(object sender, RoutedEventArgs e)
    {
        if (mainPageVM.CurrentSession != null)
        {
            ShowMessageBox($"Already connected to: {mainPageVM.CurrentSession}\nDiscoonect or end current session to connect new");
            return;
        }

        ShowConnectQFormDialog();
    }

    private void MenuDisconnectQForm(object sender, RoutedEventArgs e)
    {
        mainPageVM.qformManager.DetachQForm();
        mainPageVM.CurrentSession = null;
        mainPageVM.StatusBarVisibility = Visibility.Collapsed;
    }
}

public enum ParameterTypes
{
    Input = 0,
    Output = 1,
}

public class CodeArg
{
    public virtual object value_get() { return null; }
}

public class CodeRet : Ret
{
    public virtual void value_set(object o) { }
}

    
