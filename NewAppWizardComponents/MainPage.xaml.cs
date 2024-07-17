using System.Collections;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using Microsoft.UI.Xaml.Documents;
using Windows.UI;

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

    private Border _lastSelectedBlock = null;
    private readonly Brush _defaultBackground = new SolidColorBrush(Windows.UI.Color.FromArgb(0, 0, 0, 0));
    private readonly Brush _selectedBackground = new SolidColorBrush(Windows.UI.Color.FromArgb(50, 0, 120, 210));

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
        CodeBlocks.Children.Add(CreateViewCodeBlock(CodeBlocks, mainPageVM.CodeBlocks.Last(), mainPageVM.CodeBlocks.Count)); 
    }

    public Border CreateViewCodeBlock(StackPanel codeContainer, ApiEntry entry, int num)
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

        newBlock.Tag = new IndexedEntry(entry, num);

        return newBlock;
    }

    public void ClearCodeBlocks(object sender, EventArgs e)
    {
        CodeBlocks.Children.Clear();
        ClearShownParameters();
    }

    private void ShowParameters(ApiEntry entry, ParameterTypes type, Border container)
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
                    type == ParameterTypes.Output,
                    container
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

    private FrameworkElement CreateControlForProperty(PropertyInfo property, object value, ApiEntry entry, bool isResult, Border container)
    {
        if (property.PropertyType == typeof(bool))
        {
            var comboBox = new ComboBox();

            comboBox.Items.Add("False");
            comboBox.Items.Add("True");

            comboBox.SelectedIndex = Convert.ToInt32(value);
            comboBox.Tag = property;

            comboBox.SelectionChanged += (s, e) =>
            {
                property.SetValue(entry.arg_value, Convert.ToBoolean(comboBox.SelectedIndex));

                EditCodeBlock(container);
            };

            comboBox.Style = (Style)Resources["ParameterValueComboBox"];

            return comboBox;
        }
        else if (property.PropertyType.IsEnum)
        {
            var comboBox = new ComboBox();
            foreach (var enumValue in Enum.GetValues(property.PropertyType))
            {
                comboBox.Items.Add(enumValue.ToString());
            }
            comboBox.SelectedItem = property.GetValue(entry.arg_value)?.ToString();
            comboBox.Tag = property;

            comboBox.SelectionChanged += (s, e) =>
            {
                var selectedValue = comboBox.SelectedItem.ToString();
                var enumValue = Enum.Parse(property.PropertyType, selectedValue);
                property.SetValue(entry.arg_value, enumValue);

                EditCodeBlock(container);
            };

            comboBox.Style = (Style)Resources["ParameterValueComboBox"];

            return comboBox;
        }
        else
        {
            var textBox = new TextBox();
            textBox.Text = value?.ToString();
            textBox.Tag = property;

            if (isResult) 
                textBox.IsReadOnly = true;
            else 
                textBox.TextChanged += (sender, e) => OnValueChanged(property, textBox.Text, entry, textBox, container);

            textBox.Style = (Style)Resources["ParameterValueTextBox"];

            return textBox;
        }
    }

    private void OnValueChanged(System.Reflection.PropertyInfo property, string newValue, ApiEntry entry, Control visualBlock, Border container)
    {
        try
        {
            var targetType = property.PropertyType;

            if (targetType == typeof(int))
            {
                property.SetValue(entry.arg_value, int.Parse(newValue));
            }
            else if (targetType == typeof(double))
            {
                property.SetValue(entry.arg_value, double.Parse(newValue));
            }
            else if (targetType == typeof(string))
            {
                property.SetValue(entry.arg_value, newValue);
            }
            else
            {
                Debug.WriteLine("A property of a non-built-in type was changed, but this could not have any consequences.");
            }

            EditCodeBlock(container);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Invalid value {newValue} for property {property.Name}");
            visualBlock.Background = (SolidColorBrush)Resources["FailStatus"];
        }
    }

    void EditCodeBlock(Border container)
    {
        var meta = container.Tag as IndexedEntry;
        var editedBlock = CreateViewCodeBlock(CodeBlocks, meta.apiEntry, meta.index);

        _lastSelectedBlock = editedBlock;
        _lastSelectedBlock.Background = _selectedBackground;

        CodeBlocks.Children[meta.index - 1] = editedBlock;
    }

    private void CodeBlockGotFocus(object sender, RoutedEventArgs e)
    {
        if (_lastSelectedBlock != null)
        {
            _lastSelectedBlock.Background = _defaultBackground;
        }

        var selectedBlock = (Border)VisualTreeHelper.GetParent((TextBlock)sender);
        if (selectedBlock != null)
        {
            selectedBlock.Background = _selectedBackground;
            _lastSelectedBlock = selectedBlock;

            var entry = (selectedBlock.Tag as IndexedEntry).apiEntry;

            ClearShownParameters();

            if (entry.arg_type != null) ShowParameters(entry, ParameterTypes.Input, selectedBlock);
            if (entry.ret_type != null) ShowParameters(entry, ParameterTypes.Output, selectedBlock);
        }

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
            // Imptlement flyout or smth idk
        }

    }

}

public enum ParameterTypes
{
    Input = 0,
    Output = 1,
}
