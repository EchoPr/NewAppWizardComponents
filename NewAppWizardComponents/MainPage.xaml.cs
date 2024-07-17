using System.Collections;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Windows.ApplicationModel.Activation;
using Windows.UI;
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
        CreateViewCodeBlock(CodeBlocks, mainPageVM.CodeBlocks.Last(), mainPageVM.CodeBlocks.Count);
        //ShowInputParameters();
        //ShowOutputParameters();
    }

    public void CreateViewCodeBlock(StackPanel codeContainer, ApiEntry entry, int num)
    {
        var newCodeLines = new TextBlock();

        if (entry.arg_type != null)
        {
            newCodeLines.Inlines.Add(new Run { Text = $"{entry.arg_type.Name[1..]} ", Foreground = codeTypeBrush });
            newCodeLines.Inlines.Add(new Run { Text = $"arg{num} ", Foreground = codeDefaultBrush });
            newCodeLines.Inlines.Add(new Run { Text = "= new", Foreground = codeKeywordBrush });
            newCodeLines.Inlines.Add(new Run { Text = "() {\n", Foreground = codeBracketsBrush });

            var instance = Activator.CreateInstance(entry.arg_type);
            PropertyInfo[] properties = entry.arg_type.GetProperties();
            entry.arg_value = instance;

            foreach (PropertyInfo property in properties)
            {
                if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType)) continue;

                var value = property.GetValue(instance);

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

        codeContainer.Children.Add(newBlock);
    }

    public void ClearCodeBlocks(object sender, EventArgs e)
    {
        CodeBlocks.Children.Clear();
    }

    public void RefreshCodeBlocks()
    {
        // Only visual blocks
        CodeBlocks.Children.Clear();

        for(int i = 0; i < mainPageVM.CodeBlocks.Count; i++)
        {
            CreateViewCodeBlock(CodeBlocks, mainPageVM.CodeBlocks[i], i);
        }
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

            for (int i = 0; i < properties.Length; i++)
            {
                dest.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                var property = properties[i];
                var propertyNameTextBlock = new TextBlock { Text = property.Name, Style = (Style)Resources["ParameterName"] };
                var propertyNameVisual = new Border { Style = (Style)Resources["ParameterNameBorder"] };
                propertyNameVisual.Child = propertyNameTextBlock;
                var propertyValueControl = CreateControlForProperty(property, property.PropertyType.IsValueType ? Activator.CreateInstance(property.PropertyType) : null);

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
        InputParametersGrid.RowDefinitions.Clear();

        OutputParametersGrid.Children.Clear();
        OutputParametersGrid.RowDefinitions.Clear();
    }

    private FrameworkElement CreateControlForProperty(PropertyInfo property, object value)
    {
        if (property.PropertyType == typeof(bool))
        {
            var comboBox = new ComboBox();
            comboBox.Items.Add("True");
            comboBox.Items.Add("False");
            comboBox.SelectedIndex = 0;
            comboBox.Tag = property;

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
            comboBox.SelectedIndex = 0;
            comboBox.Tag = property;

            comboBox.Style = (Style)Resources["ParameterValueComboBox"];

            return comboBox;
        }
        else
        {
            var textBox = new TextBox();
            textBox.Text = value?.ToString();
            textBox.Tag = property;

            textBox.Style = (Style)Resources["ParameterValueTextBox"];

            return textBox;
        }
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

            if (entry.arg_type != null) ShowParameters(entry, ParameterTypes.Input);
            if (entry.ret_type != null) ShowParameters(entry, ParameterTypes.Output);
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

        }

    }

}

public enum ParameterTypes
{
    Input = 0,
    Output = 1,
}
