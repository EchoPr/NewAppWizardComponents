
using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Uno.Extensions;
using Uno.Extensions.Specialized;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.DataTransfer;
using Windows.Security.Cryptography.Core;
using Windows.System;
using Windows.UI.Core;


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
    SolidColorBrush? codeCommentBrush;

    private List<Border> _selectedBlocks = new List<Border>();

    private ApiEntry _lastClickedMethod = null;

    private bool _isResizing = false;
    private double _initialPosition;
    // RowDefinition or ColumnDefinition
    private object _firstMovedGrip;
    private object _secondMovedGrip;
    //
    private GripSeparatorType? usingSeparator = null;
    private Grid resizingGrid;

    private bool _isCtrlPressed;
    private Dictionary<string, ApiEntry> _languageSnippets = new Dictionary<string, ApiEntry>();

    private string currentSelectedLanguage = "C#";

    private Dictionary<string, string> _wizardTitles = new()
    {
        { "C#", "New C# Class" },
        { "Python", "New Python Script" },
        { "VB.Net", "New VB.Net Class" },
        { "VBA", "Excel Project" },
        { "MATLAB", "New MATLAB Script" },
        { "S-expr", "New S-expr Script" },
        { "XML", "Wizard" },
    };


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
        mainPageVM.qformManager.InvocationStarted += (s, e) => { InvokeProgressRing.IsActive = true; };
        mainPageVM.qformManager.InvocationEnded += (s, e) => { InvokeProgressRing.IsActive = false; };

#if !HAS_UNO
        Clipboard.ContentChanged += (s, e) =>
        {
            try
            {
                string data = ClipboardHelper.GetDataFromClipboard();
                ReadBatchEntriesFromJson(data);
            }
            catch (Exception ex)
            {
                //Debug.WriteLine(ex);
            }
        };
#else
        Clipboard.ContentChanged += async (s, e) =>
        {
            string dataName = "QForm.CPI.PropertyDefW";
            DataPackageView dataPackageView = Clipboard.GetContent();
            if (dataPackageView.Contains(dataName))
            {
                string data = await dataPackageView.GetTextAsync(dataName);
                ReadBatchEntriesFromJson(data);
            }
        };
#endif
    }

    private void ReadBatchEntriesFromJson(string data)
    {
        PropertyBatch pb = JsonSerializer.Deserialize(data, typeof(PropertyBatch)) as PropertyBatch;

        List<ApiEntry> entries = mainPageVM.qformManager.GetPropertyEntry(pb);

        foreach (ApiEntry entry in entries)
            mainPageVM.AddToCodeBlocks(entry, mainPageVM.CodeBlocks.Count, originalEntry: true);
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
        codeCommentBrush = Resources["CodeComment"] as SolidColorBrush;

        if (codeTypeBrush == null || codeDefaultBrush == null || codeKeywordBrush == null || codeBracketsBrush == null || codeValueBrush == null || codeMethodBrush == null || codeCommentBrush == null)
        {
            throw new InvalidOperationException("One or more brushes was not found!");
        }
    }

    public void AddNewCodeBlock(object sender, ExpandedEntry args)
    {
       OnAddCodeBlock(sender, args);
    }

    public void OnAddCodeBlock(object sender, ExpandedEntry args, bool needScroll = true)
    {
        Border newBlock = CreateViewCodeBlock(
           args.apiEntry,
           args.index,
           args.isConnectedBlockSequentialInitialized ? CodeGenerationMode.StepByStep : CodeGenerationMode.ObjectInit,
           isEditig: false
       );

        CodeBlocks.Children.Insert(args.index - mainPageVM.startGenerationIndex, newBlock);
        if (needScroll) ScrollToCodeBlock(newBlock);
    }

    private Border CreateViewCodeBlock(ApiEntry entry, int entryNumber, CodeGenerationMode generationMode, bool isEditig)
    {
        
        ICodeGenerator generator = CodeGeneratorFactory.GetGenerator(currentSelectedLanguage);
        var generatedCode = entry.is_snippet
                            ? generator.GenerateApiSnippet(entry, mainPageVM.qformManager.QFormBaseDir)
                            : generator.GenerateCodeEntry(
                                entry, 
                                entryNumber - Convert.ToInt32(_languageSnippets[currentSelectedLanguage] != null), 
                                generationMode);

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
            if (e.Key == VirtualKey.Control)
            {
                _isCtrlPressed = true;
            }
        };

        newCodeLines.KeyUp += (s, e) =>
        {
            if (e.Key == VirtualKey.Control)
            {
                _isCtrlPressed = false;
            }
        };

        if (entry.is_snippet)
        {
            for (int i = 0; i < CodeBlocks.Children.Count; i++)
            {
                var meta = (CodeBlocks.Children[i] as Border).Tag as ExpandedEntry;
                (CodeBlocks.Children[i] as Border).Tag = new ExpandedEntry(meta.apiEntry, meta.index + 1, meta.isConnectedBlockSequentialInitialized);
            }
        }

        var newBlock = new Border();

        newBlock.Child = newCodeLines;
        newBlock.Style = (Style)Resources["CodeBlockBorder"];

        newBlock.Tag = new ExpandedEntry(entry, entryNumber - mainPageVM.startGenerationIndex, generationMode == CodeGenerationMode.StepByStep);


        if (!entry.is_snippet && !isEditig)
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
            ViewCodeSampleType.Comment => codeCommentBrush,
            _ => codeDefaultBrush,
        };
    }

    public void ClearCodeBlocks(object sender, EventArgs e)
    {
        _ClearViewCodeBlocks();

        _selectedBlocks.Clear();
        _languageSnippets.Clear();
    }

    private void _ClearViewCodeBlocks()
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
        if (highlighted) propertyTextBlock.FontFamily = new FontFamily("SegoeUI");

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
        //Debug.WriteLine("Action Was In EditCodeBlock");
        var meta = _selectedBlocks.Last().Tag as ExpandedEntry;

        Border updatedBlock;
        updatedBlock = CreateViewCodeBlock(
            meta.apiEntry,
            meta.index + mainPageVM.startGenerationIndex,
            meta.isConnectedBlockSequentialInitialized ? CodeGenerationMode.StepByStep : CodeGenerationMode.ObjectInit,
            true
        );

        CodeBlocks.Children[meta.index] = updatedBlock;
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
                CodeBlocks.Children[(_selectedBlocks.Last().Tag as ExpandedEntry).index] = CreateViewCodeBlock(meta.apiEntry, meta.index, CodeGenerationMode.StepByStep, true);
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
        if (!_selectedBlocks.Contains(newBlock))
            _selectedBlocks.Add(newBlock);

    }

    private async void ScrollToCodeBlock(Border block, bool fromGotFocused = false)
    {
        await Task.Delay(100);

        double posY = block.TransformToVisual(CodeView.Content as UIElement).TransformPoint(new Windows.Foundation.Point()).Y;
        CodeView.ChangeView(null, posY, null);
        if (!fromGotFocused) SetCodeBlockSelection(block, false);
    }

    private void CodeBlockGotFocus(object sender, RoutedEventArgs e)
    {
        var ctrlPressed = _isCtrlPressed;

        var selectedBlock = (Border)VisualTreeHelper.GetParent((TextBlock)sender);
        if (selectedBlock == null) return;

        SetCodeBlockSelection(selectedBlock, ctrlPressed);
        ScrollToCodeBlock(selectedBlock, true); 

        var entry = (selectedBlock.Tag as ExpandedEntry).apiEntry;
        if (!ctrlPressed && !entry.is_snippet)
        {
            ShowParameters(entry);
        }
        else
        {
            ClearShownParameters();
        }

        InvokeButton.IsEnabled = !ctrlPressed;

    }

    private void MainPage_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == VirtualKey.Control)
        {
            _isCtrlPressed = true;
        }
    }

    private void MainPage_KeyUp(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == VirtualKey.Control)
        {
            _isCtrlPressed = false;
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

        var separator = sender as Border;
        resizingGrid = separator.Parent as Grid;


        string separatorInfo = separator.Tag.ToString();
        int separatorIndex = int.Parse(separatorInfo[1].ToString());
        usingSeparator = separatorInfo[0] == 'H' ? GripSeparatorType.Horizontal : GripSeparatorType.Vertical;

        _initialPosition = usingSeparator == GripSeparatorType.Horizontal
            ? e.GetCurrentPoint(resizingGrid).Position.Y
            : e.GetCurrentPoint(resizingGrid).Position.X;

        if (usingSeparator == GripSeparatorType.Horizontal)
        {
            _firstMovedGrip = resizingGrid.RowDefinitions[separatorIndex - 1];
            _secondMovedGrip = resizingGrid.RowDefinitions[separatorIndex + 1];
        }
        else
        {
            _firstMovedGrip = resizingGrid.ColumnDefinitions[separatorIndex - 1];
            _secondMovedGrip = resizingGrid.ColumnDefinitions[separatorIndex + 1];
        }

        separator.CapturePointer(e.Pointer);
    }

    private void Separator_PointerMoved(object sender, PointerRoutedEventArgs e)
    {
        if (!_isResizing)
            return;

        double currentPosition = usingSeparator == GripSeparatorType.Horizontal
            ? e.GetCurrentPoint(resizingGrid).Position.Y
            : e.GetCurrentPoint(resizingGrid).Position.X;

        double delta = currentPosition - _initialPosition;

        double newFirstDim;
        double newSecondDim;

        if (usingSeparator == GripSeparatorType.Horizontal)
        {
            newFirstDim = (_firstMovedGrip as RowDefinition).ActualHeight + delta;
            newSecondDim = (_secondMovedGrip as RowDefinition).ActualHeight - delta;
        }
        else
        {
            newFirstDim = (_firstMovedGrip as ColumnDefinition).ActualWidth + delta;
            newSecondDim = (_secondMovedGrip as ColumnDefinition).ActualWidth - delta;
        }

        if (newFirstDim > 16 && newSecondDim > 16)
        {
            if (usingSeparator == GripSeparatorType.Horizontal)
            {
                (_firstMovedGrip as RowDefinition).Height = new GridLength(newFirstDim, GridUnitType.Pixel);
                (_secondMovedGrip as RowDefinition).Height = new GridLength(newSecondDim, GridUnitType.Pixel);
            }
            else
            {
                (_firstMovedGrip as ColumnDefinition).Width = new GridLength(newFirstDim, GridUnitType.Pixel);
                (_secondMovedGrip as ColumnDefinition).Width = new GridLength(newSecondDim, GridUnitType.Pixel);
            }

            _initialPosition = currentPosition;
        }
    }

    private void Separator_PointerReleased(object sender, PointerRoutedEventArgs e)
    {
        _isResizing = false;

        if (usingSeparator == GripSeparatorType.Horizontal)
        {
            double totalHeight = resizingGrid.ActualHeight;
            double firstStar = (_firstMovedGrip as RowDefinition).ActualHeight / totalHeight;
            double secondStar = (_secondMovedGrip as RowDefinition).ActualHeight / totalHeight;

            (_firstMovedGrip as RowDefinition).Height = new GridLength(firstStar, GridUnitType.Star);
            (_secondMovedGrip as RowDefinition).Height = new GridLength(secondStar, GridUnitType.Star);
        }
        else
        {
            double totalWidth = resizingGrid.ActualWidth;
            double firstStar = (_firstMovedGrip as ColumnDefinition).ActualWidth / totalWidth;
            double secondStar = (_secondMovedGrip as ColumnDefinition).ActualWidth / totalWidth;

            Debug.WriteLine($"{totalWidth} {firstStar} {secondStar} | {resizingGrid.Name}");

            (_firstMovedGrip as ColumnDefinition).Width = new GridLength(firstStar, GridUnitType.Star);
            (_secondMovedGrip as ColumnDefinition).Width = new GridLength(secondStar, GridUnitType.Star);
        }

        (sender as UIElement).ReleasePointerCapture(e.Pointer);
    }

    private void OnListMethodDoubleClick(object sender, DoubleTappedRoutedEventArgs e)
    {

        if (_lastClickedMethod == null) return;

        if (!_lastClickedMethod.menu_only)
        {
            mainPageVM.AddToCodeBlocks(_lastClickedMethod, mainPageVM.CodeBlocks.Count);
            SetCodeBlockSelection(CodeBlocks.Children.Last() as Border, multipleSelection: false);
            InvokeButton.IsEnabled = true;
        }
        else
        {

            ShowMessageBox("To add this method use the context menu in QForm");
        }
    }

    public void OnListMethodClick(object sender, ItemClickEventArgs e)
    {
        _lastClickedMethod = e.ClickedItem as ApiEntry;
        if (e.ClickedItem is ApiEntry entry && !entry.menu_only)
        {
            AddToWorksapceButton.IsEnabled = true;
        }
        else
        {
            AddToWorksapceButton.IsEnabled = false;
        }

        mainPageVM.UpdateDocsVisibility(_lastClickedMethod);
    }

    private void OnTreeMethodDoubleClick(object sender, DoubleTappedRoutedEventArgs e)
    {
        if (_lastClickedMethod == null) return;

        if (!_lastClickedMethod.menu_only)
        {
            mainPageVM.AddToCodeBlocks(_lastClickedMethod, mainPageVM.CodeBlocks.Count);
            SetCodeBlockSelection(CodeBlocks.Children.Last() as Border, multipleSelection: false);
            InvokeButton.IsEnabled = true;
        }
        else
        {

            ShowMessageBox("To add this method use the context menu in QForm");
        }
    }

    private void OnTreeMethodClick(TreeView sender, TreeViewItemInvokedEventArgs args)
    {
        if (args.InvokedItem is TreeViewItemModel item && item.Children.Count == 0)
        {
            _lastClickedMethod = item.ApiEntry;
            if (!item.ApiEntry.menu_only)
            {
                AddToWorksapceButton.IsEnabled = true;
            }
            else
            {
                AddToWorksapceButton.IsEnabled = false;
            }

            mainPageVM.UpdateDocsVisibility(item.ApiEntry);
        }
        else
        {
            AddToWorksapceButton.IsEnabled = false;
        }
    }

    private void LanguageSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedLang = (e.AddedItems[0] as ComboBoxItem).Content as string;
        LanguageSelectionChange(selectedLang);

        if (this.IsLoaded)
        {
            foreach (var item in WizardMenuFlyout.Items)
            {
                item.Visibility = (item.Tag as string) == selectedLang || item.Tag == null ? Visibility.Visible : Visibility.Collapsed;
            }

            WizardSplitButton.Content = _wizardTitles[selectedLang];

            WizardSplitButton.Visibility = selectedLang == "XML" || selectedLang == "S-expr" ? Visibility.Collapsed : Visibility.Visible;
        }


    }

    private void LanguageSelectionChange(string lang, List<bool>? genModes = null) 
    {
        if (CodeBlocks == null || lang == null) return;

        CodeBlocksBefore.Children.Clear();
        CodeBlocksAfter.Children.Clear();

        if (lang == "XML")
        {
            TextBlock textBlockBefore = new TextBlock();
            textBlockBefore.Inlines.Add(new Run { Text = "<", Foreground = GetBrush(ViewCodeSampleType.Brackets) });
            textBlockBefore.Inlines.Add(new Run { Text = "qform_batch", Foreground = GetBrush(ViewCodeSampleType.Default) });
            textBlockBefore.Inlines.Add(new Run { Text = ">", Foreground = GetBrush(ViewCodeSampleType.Brackets) });

            CodeBlocksBefore.Children.Add(textBlockBefore);

            TextBlock textBlockAfter = new TextBlock();
            textBlockAfter.Inlines.Add(new Run { Text = "</", Foreground = GetBrush(ViewCodeSampleType.Brackets) });
            textBlockAfter.Inlines.Add(new Run { Text = "qform_batch", Foreground = GetBrush(ViewCodeSampleType.Default) });
            textBlockAfter.Inlines.Add(new Run { Text = ">", Foreground = GetBrush(ViewCodeSampleType.Brackets) });

            CodeBlocksAfter.Children.Add(textBlockAfter);
        }
        else
        {
            CodeBlocksBefore.Children.Clear();
            CodeBlocksAfter.Children.Clear();
        }


        if (currentSelectedLanguage != null && _languageSnippets.ContainsKey(currentSelectedLanguage))
        {
            var index = mainPageVM.CodeBlocks.IndexOf(_languageSnippets[currentSelectedLanguage]);
            if (index != -1)
            {
                mainPageVM.CodeBlocks.RemoveAt(index);
            }
        }

        currentSelectedLanguage = lang;
        if (_languageSnippets.ContainsKey(currentSelectedLanguage))
        {
            var snippet = _languageSnippets[currentSelectedLanguage];
            mainPageVM.CodeBlocks.Insert(0, snippet);
        }

        if (genModes == null)
        {
            ClearShownParameters();
            genModes = new List<bool>();
            foreach (Border b in CodeBlocks.Children)
            {
                genModes.Add((b.Tag as ExpandedEntry).isConnectedBlockSequentialInitialized);
            }
        }

        _ClearViewCodeBlocks();
        var blocksCount = mainPageVM.CodeBlocks.Count;
        for (int i = 0; i < blocksCount; i++)
        {
            // Don't try this at home!
            OnAddCodeBlock(null, new ExpandedEntry(mainPageVM.CodeBlocks[i], i +  mainPageVM.startGenerationIndex, true), false);
        }
    }

    private void SaveSelectedBlocks(object sender, RoutedEventArgs e)
    {
        SaveCode(_selectedBlocks.OrderBy(b => (b.Tag as ExpandedEntry).index).Select(b => (b.Child as TextBlock).Text));
    }

    private void SaveProject(object sender, RoutedEventArgs e)
    {
        string currentSelectedLanguage = "S-expr";

        ICodeGenerator codeGenerator = CodeGeneratorFactory.GetGenerator(currentSelectedLanguage);

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

    private void SaveCodeAs(object sender, RoutedEventArgs e)
    {
        mainPageVM.SaveCodeLines(
            CodeBlocks.Children.Aggregate("", (acc, val) => acc + ((val as Border).Child as TextBlock).Text + "\n\n"),
            currentSelectedLanguage
        );
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

    private void AddMethodToWorkspace(object sender, RoutedEventArgs e)
    {
        mainPageVM.AddToCodeBlocks(_lastClickedMethod, mainPageVM.CodeBlocks.Count);
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
    }

    private void TabBar_SelectionChanged(TabBar sender, TabBarSelectionChangedEventArgs args)
    {
        mainPageVM.ChangeApiFunctionsVisibility(sender.SelectedIndex);
    }

    private void ApiWizardClickSplitButton(SplitButton sender, SplitButtonClickEventArgs args)
    {
        ApiWizardClick();
    }

    private void ApiWizardClickMenuFlyout(object sender, RoutedEventArgs e)
    {
        ApiWizardClick();
    }

    private async void CopyQFormApiPyMenuFlyout(object sender, RoutedEventArgs e)
    {
        string baseDir = mainPageVM.qformManager.QFormBaseDir;
        string qformApi = "API\\App\\Python\\QFormAPI.py";
        string apiFile = Path.Combine(baseDir, qformApi);

        StorageFolder folder = await mainPageVM.projectManager.SelectFolder();

        if (folder != null)
        {
            try
            {
                File.Copy(apiFile, Path.Combine(folder.Path, "QFormAPI.py"));
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message);
            }
        }
    }

    private async void ApiWizardClick()
    {
        ApiEntry? curLangSnippet = _languageSnippets.TryGetValue(currentSelectedLanguage, out var value) ? value : null;

        ContentDialog snippetDialog = ApiSettingsDialogFactory.GetDialog(
            currentSelectedLanguage, 
            mainPageVM,
            curLangSnippet,
            _languageSnippets
        );

        snippetDialog.XamlRoot = this.XamlRoot;
        var result = await snippetDialog.ShowAsync();

        LanguageSelectionChange(currentSelectedLanguage);
    }

    //private void ProcessOkWizardResult(ContentDialog snippetDialog)
    //{
    //    ApiEntry snippet;

    //    if (snippetDialog is PythonApiSettingsDialog dialogPy)
    //        snippet = dialogPy.GetEntry();
    //    else if (snippetDialog is CSharpApiSettingsDialog dialogCs)
    //        snippet = dialogCs.GetEntry();
    //    else
    //        throw new NotImplementedException();


    //    _languageSnippets[currentSelectedLanguage] = snippet;


    //}

    private async void AddCSApiReferenceToProject(object sender, RoutedEventArgs e)
    {
        StorageFolder folder = await mainPageVM.projectManager.SelectFolder();

        string baseDir = mainPageVM.qformManager.QFormBaseDir;
        string qformApi = "x64\\QFormApiNet.dll";

        string apiFile = Path.Combine(baseDir, qformApi);

        string csprojFilePath = Directory.GetFiles(folder.Path, "*.csproj", SearchOption.TopDirectoryOnly).FirstOrDefault();
        if (csprojFilePath == null)
            throw new Exception("There is no .csproj file in the selected folder");

        XDocument csprojXml = XDocument.Load(csprojFilePath);
        XElement root = csprojXml.Root;
        if (root == null)
            throw new Exception("Invalid .csproj file.");

        XNamespace ns = root.GetDefaultNamespace();
        XElement itemGroup = root.Element(ns + "ItemGroup");
        if (itemGroup == null)
        {
            itemGroup = new XElement(ns + "ItemGroup");
            root.Add(itemGroup);
        }

        // Remove existing DLL references
        var existingReference = itemGroup.Elements(ns + "Reference")
            .FirstOrDefault(e => e.Attribute("Include")?.Value == "QFormAPINet");
        existingReference?.Remove();

        // Add new DLL reference
        XElement reference = new XElement(ns + "Reference",
                new XAttribute("Include", "QFormAPINet"),
                new XElement(ns + "HintPath", apiFile)
            );
        itemGroup.Add(reference);
        csprojXml.Save(csprojFilePath);

    }

    private bool isDotnetFramework(XDocument csproj)
    {
        var targetFrameworkElement = csproj.Descendants("TargetFramework").FirstOrDefault();

        if (targetFrameworkElement != null)
        {
            string targetFramework = targetFrameworkElement.Value;

            if (targetFramework.StartsWith("netcoreapp") || targetFramework.StartsWith("net"))
            {
                return false;
            }
            else if (targetFramework.StartsWith("net") && targetFramework.Length == 4)
            {
                return true;
            }
        }
        else
        {
            if (csproj.ToString().Contains("TargetFrameworkVersion"))
            {
                return true;
            }
        }

        throw new Exception("Unknown csproj type");
    }

    private async void AddCSClassToProject(object sender, RoutedEventArgs e)
    {
        StorageFolder folder = await mainPageVM.projectManager.SelectFolder();

        if (folder == null) return;

        string baseDir = mainPageVM.qformManager.QFormBaseDir;
        string qformApi = "API\\App\\C#\\QForm.cs";

        string apiFile = Path.Combine(baseDir, qformApi);

        string csprojFilePath = Directory.GetFiles(folder.Path, "*.csproj", SearchOption.TopDirectoryOnly).FirstOrDefault();
        if (csprojFilePath != null)
        {
            XDocument csprojXml = XDocument.Load(csprojFilePath);
            XElement root = csprojXml.Root;
            if (root == null)
                throw new Exception("Invalid .csproj file.");

            XNamespace ns = root.GetDefaultNamespace();
            XElement itemGroup = root.Element(ns + "ItemGroup");
            if (itemGroup == null)
            {
                itemGroup = new XElement(ns + "ItemGroup");
                root.Add(itemGroup);
            }

            if (isDotnetFramework(csprojXml))
            {
                var existingCompile = itemGroup.Elements(ns + "Compile")
                .FirstOrDefault(e => e.Attribute("Include")?.Value == "QFormApi.cs");
                existingCompile?.Remove();

                XElement compile = new XElement(ns + "Compile",
                        new XAttribute("Include", "QFormApi.cs")
                    );
                itemGroup.Add(compile);
                csprojXml.Save(csprojFilePath);
            }
        }

        File.Copy(apiFile, Path.Combine(folder.Path, "QFormApi.cs"), true);
    }

    private void ClearSelected(object sender, RoutedEventArgs e)
    {
        foreach (var block in _selectedBlocks)
        {
            mainPageVM.CodeBlocks.Remove((block.Tag as ExpandedEntry).apiEntry);
        }


        LanguageSelectionChange(currentSelectedLanguage);
        
        ClearShownParameters();
        _selectedBlocks.Clear();
    }
    
    private void CopyCode(object sender, RoutedEventArgs e)
    {
        var dp = new DataPackage();
        dp.SetText(CodeBlocks.Children.Aggregate("", (acc, val) => acc + ((val as Border).Child as TextBlock).Text + "\n\n"));
        Clipboard.SetContent(dp);
    }

    private void CopySelected(object sender, RoutedEventArgs e)
    {
        var dp = new DataPackage();
        dp.SetText(_selectedBlocks.OrderBy(e => (e.Tag as ExpandedEntry).index).Aggregate("", (acc, val) => acc + (val.Child as TextBlock).Text + "\n\n"));
        Clipboard.SetContent(dp);
    }

    private void CodeBlockUp(object sender, RoutedEventArgs e)
    {
        if (_selectedBlocks.Count == 0) return;

        var entry = _selectedBlocks.Last().Tag as ExpandedEntry;
        int index = entry.index;

        if (!(entry.index == 0 || entry.index == 1 && mainPageVM.CodeBlocks.First().is_snippet))
        {
            ApiEntry tmp = mainPageVM.CodeBlocks[index];
            mainPageVM.CodeBlocks[index] = mainPageVM.CodeBlocks[index - 1];
            mainPageVM.CodeBlocks[index - 1] = tmp;

            List<bool> genModes = new List<bool>();
            foreach (Border b in CodeBlocks.Children)
            {
                genModes.Add((b.Tag as ExpandedEntry).isConnectedBlockSequentialInitialized);
            }

            var t = genModes[index];
            genModes[index] = genModes[index - 1];
            genModes[index - 1] = t;

            LanguageSelectionChange(currentSelectedLanguage, genModes);
            ScrollToCodeBlock(CodeBlocks.Children[index - 1] as Border);
            ShowParameters(mainPageVM.CodeBlocks[index - 1]);

        }
    }

    private void CodeBlockDown(object sender, RoutedEventArgs e)
    {
        if (_selectedBlocks.Count == 0) return;

        var entry = _selectedBlocks.Last().Tag as ExpandedEntry;
        int index = entry.index;

        if (!(entry.index == CodeBlocks.Children.Count - 1 || entry.index ==  CodeBlocks.Children.Count - 2 && mainPageVM.CodeBlocks.Last().is_snippet))
        {
            ApiEntry tmp = mainPageVM.CodeBlocks[index];
            mainPageVM.CodeBlocks[index] = mainPageVM.CodeBlocks[index + 1];
            mainPageVM.CodeBlocks[index + 1] = tmp;

            List<bool> genModes = new List<bool>();
            foreach (Border b in CodeBlocks.Children)
            {
                genModes.Add((b.Tag as ExpandedEntry).isConnectedBlockSequentialInitialized);
            }

            var t = genModes[index];
            genModes[index] = genModes[index + 1];
            genModes[index + 1] = t;

            LanguageSelectionChange(currentSelectedLanguage, genModes);
            ScrollToCodeBlock(CodeBlocks.Children[index + 1] as Border);
            ShowParameters(mainPageVM.CodeBlocks[index + 1]);

        }
    }

    private async void SetStartIndex(object sender, RoutedEventArgs e)
    {
        var dialog = new SetStartGenerationIndexDialog(mainPageVM);
        dialog.XamlRoot = this.XamlRoot;

        var res = await dialog.ShowAsync();

        if (res == ContentDialogResult.Primary)
        {
            LanguageSelectionChange(currentSelectedLanguage);
        }
    }

    private void Border_PointerEntered(object sender, PointerRoutedEventArgs e)
    {
    }

    private void Border_PointerExited(object sender, PointerRoutedEventArgs e)
    {

    }

    private void CollapseAll_Click(object sender, RoutedEventArgs e)
    {
        mainPageVM.PopulateTreeViewItems(FilterMethodsTree.Text, false);

        
    }


}

public class CodeArg
{
    public virtual object value_get() { return null; }
}

public class CodeRet : Ret
{
    public virtual void value_set(object o) { }
}

#if !HAS_UNO
class ClipboardHelper
{
    const string FORMAT_NAME = "QForm.CPI.PropertyDefW";

    [DllImport("user32.dll", SetLastError = true)]
    static extern bool OpenClipboard(IntPtr hWndNewOwner);

    [DllImport("user32.dll", SetLastError = true)]
    static extern bool CloseClipboard();

    [DllImport("user32.dll")]
    static extern IntPtr GetClipboardData(uint uFormat);

    [DllImport("user32.dll", SetLastError = true)]
    static extern uint RegisterClipboardFormat(string lpszFormat);

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern IntPtr GlobalLock(IntPtr hMem);

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool GlobalUnlock(IntPtr hMem);

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern uint GlobalSize(IntPtr hMem);

    public static string GetDataFromClipboard()
    {
        // Регистрируем пользовательский формат
        uint format = RegisterClipboardFormat(FORMAT_NAME);
        if (format == 0)
        {
            throw new InvalidOperationException("Failed to register clipboard format.");
        }

        // Открываем буфер обмена
        if (!OpenClipboard(IntPtr.Zero))
        {
            throw new InvalidOperationException("Failed to open clipboard.");
        }

        string result = null;

        try
        {
            // Получаем данные с форматом "Foo"
            IntPtr handle = GetClipboardData(format);
            if (handle == IntPtr.Zero)
            {
                throw new InvalidOperationException("Failed to get clipboard data.");
            }

            // Блокируем глобальную память, чтобы получить доступ к данным
            IntPtr pointer = GlobalLock(handle);
            if (pointer == IntPtr.Zero)
            {
                throw new InvalidOperationException("Failed to lock global memory.");
            }

            try
            {
                // Определяем размер данных
                uint size = GlobalSize(handle);
                byte[] buffer = new byte[size];

                // Копируем данные из памяти в буфер
                Marshal.Copy(pointer, buffer, 0, (int)size);

                // Преобразуем данные в строку (предполагается, что это UTF-8)
                result = Encoding.Unicode.GetString(buffer);
            }
            finally
            {
                // Разблокируем память
                GlobalUnlock(handle);
            }
        }
        finally
        {
            // Закрываем буфер обмена
            CloseClipboard();
        }

        return result;
    }
}
#endif

public enum ParameterTypes
{
    Input = 0,
    Output = 1,
}

public enum GripSeparatorType
{
    Horizontal,
    Vertical
}

