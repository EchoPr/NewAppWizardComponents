using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Microsoft.UI.Xaml.Input;
using Windows.UI;
using System.Windows;
using System.Linq;

namespace NewAppWizardComponents;

public sealed partial class EditCollectionDialog : ContentDialog
{
    private bool _isResizing = false;
    private double _initialPosition;
    private ColumnDefinition _firstMovedColumn;
    private ColumnDefinition _secondMovedColumn;

    public ApiEntry apiEntry;
    public PropertyInfo property;

    public Type elementType;
    public IList collection;

    private Grid _lastSelectedBlock = null;
    private readonly Brush _defaultBackground = new SolidColorBrush(Windows.UI.Color.FromArgb(0, 0, 0, 0));
    private readonly Brush _selectedBackground = new SolidColorBrush(Windows.UI.Color.FromArgb(50, 0, 120, 210));

    public EditCollectionDialog(ApiEntry apiEntry_, PropertyInfo property_)
	{
		this.InitializeComponent();

        apiEntry = apiEntry_;
        property = property_;

        elementType = property.PropertyType.GetGenericArguments()[0];
        collection = property.GetValue(apiEntry.arg_value) as IList;

        LoadRowDefinitions();
        LoadElements();
	}

    private void LoadRowDefinitions()
    {
        for(int i = 0; i < elementType.GetProperties().Length; i++)
        {
            ElementValues.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        }
    }

    private void LoadElements()
    {
        for (int i = 0; i < collection.Count; i++)
        {
            CreateVisualElement(collection[i], i);
        }
    }

    private void CreateVisualElement(object v, int i)
    {
        Border index = new Border
        {
            Child = new TextBlock
            {
                Text = (i + 1).ToString(),
                Style = (Style)Resources["CollectionElementTextBlock"],

            },
            Style = (Style)Resources["CollectionElementBorder"]
        };
        Border value = new Border
        {
            Child = new TextBlock
            {
                Text = v.GetType().Name,
                Style = (Style)Resources["CollectionElementTextBlock"]
            },
            Style = (Style)Resources["CollectionElementBorder"]
        };


        Grid resGrid = new Grid();
        resGrid.Tag = new Tuple<object, int>(v, i);
        resGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
        resGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        
        index.Child.GotFocus += (s, e) => CollectionElementGotFocus(resGrid);
        value.Child.GotFocus += (s, e) => CollectionElementGotFocus(resGrid);

        Grid.SetRow(index, 0);
        Grid.SetColumn(index, 0);
        Grid.SetRow(value, 0);
        Grid.SetColumn(value, 1);

        resGrid.Children.Add(index);
        resGrid.Children.Add(value);

        CollectionElements.Children.Add(resGrid);
    }

    private void CollectionElementGotFocus(Grid selectedBlock)
    {
        if (_lastSelectedBlock != null)
        {
            _lastSelectedBlock.Background = _defaultBackground;
        }

        selectedBlock.Background = _selectedBackground;
        _lastSelectedBlock = selectedBlock;

        ShowElementParameters(selectedBlock);
    }

    private void ShowElementParameters(Grid selectedBlock)
    {
        var parameters = (selectedBlock.Tag as Tuple<object, int>).Item1;
        PropertyInfo[] properties = parameters.GetType().GetProperties();

        for (int i = 0; i < properties.Length; i++)
        {
            var property = properties[i];
            var propertyNameTextBlock = new TextBlock { Text = property.Name, Style = (Style)Resources["ParameterName"] };
            var propertyNameVisual = new Border { Style = (Style)Resources["ParameterNameBorder"] };
            propertyNameVisual.Child = propertyNameTextBlock;

            var propertyValueControl = CreateControlForElementProperty(
                    property,
                    property.GetValue((selectedBlock.Tag as Tuple<object, int>).Item1),
                    (selectedBlock.Tag as Tuple<object, int>).Item1,
                    selectedBlock
                );

            Grid.SetRow(propertyNameVisual, i);
            Grid.SetColumn(propertyNameVisual, 0);
            ElementValues.Children.Add(propertyNameVisual);

            Grid.SetRow(propertyValueControl, i);
            Grid.SetColumn(propertyValueControl, 1);
            ElementValues.Children.Add(propertyValueControl);
        }
    }

    private FrameworkElement CreateControlForElementProperty(PropertyInfo property, object? value, object entry, Grid container)
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
                property.SetValue(entry, Convert.ToBoolean(comboBox.SelectedIndex));
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

            comboBox.SelectedItem = property.GetValue(entry)?.ToString();
            comboBox.Tag = property;

            comboBox.SelectionChanged += (s, e) =>
            {
                var selectedValue = comboBox.SelectedItem.ToString();
                var enumValue = Enum.Parse(property.PropertyType, selectedValue);
                property.SetValue(entry, enumValue);
            };

            comboBox.Style = (Style)Resources["ParameterValueComboBox"];

            return comboBox;
        }

        else
        {
            var textBox = new TextBox();
            textBox.Text = value?.ToString();
            textBox.Tag = property;

            textBox.TextChanged += (sender, e) => OnValueChanged(property, textBox.Text, entry, textBox, container);

            textBox.Style = (Style)Resources["ParameterValueTextBox"];

            return textBox;
        }
    }

    private void OnValueChanged(PropertyInfo property, string newValue, object entry, TextBox visualBlock, Grid container)
    {
        try
        {
            var targetType = property.PropertyType;

            if (targetType == typeof(int))
            {
                property.SetValue(entry, int.Parse(newValue));
            }
            else if (targetType == typeof(double))
            {
                property.SetValue(entry, double.Parse(newValue));
            }
            else if (targetType == typeof(string))
            {
                property.SetValue(entry, newValue);
            }
            else
            {
                Debug.WriteLine("A property of a non-built-in type was changed, but this could not have any consequences.");
            }

            visualBlock.Background = (SolidColorBrush)Resources["Transparent"];
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Invalid value {newValue} for property {property.Name}");
            visualBlock.Background = (SolidColorBrush)Resources["FailStatus"];
        }
    }

    private void AddButtonClick(object sender, RoutedEventArgs e)
    {
        collection.Add(Activator.CreateInstance(elementType));

        CreateVisualElement(collection[collection.Count - 1], collection.Count - 1);
    }
    private void RemoveButtonClick(object sender, RoutedEventArgs e)
    {

    }

    private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
	{

	}

	private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
	{

	}

    private void Separator_PointerPressed(object sender, PointerRoutedEventArgs e)
    {
        _isResizing = true;
        _initialPosition = e.GetCurrentPoint(MainDialogGrid).Position.X;
        var separator = sender as Border;
        int columnIndex = int.Parse(separator.Tag.ToString());
        _firstMovedColumn = MainDialogGrid.ColumnDefinitions[columnIndex - 1];
        _secondMovedColumn = MainDialogGrid.ColumnDefinitions[columnIndex + 1];
        separator.CapturePointer(e.Pointer);
    }

    private void Separator_PointerMoved(object sender, PointerRoutedEventArgs e)
    {
        if (!_isResizing)
            return;

        double currentPosition = e.GetCurrentPoint(MainDialogGrid).Position.X;
        double delta = currentPosition - _initialPosition;

        double newFirstWidth = _firstMovedColumn.ActualWidth + delta;
        double newSecondWidth = _secondMovedColumn.ActualWidth - delta;
        if (newFirstWidth > 0 && newFirstWidth < this.Width && newSecondWidth > 0 && newSecondWidth < this.Width)
        {
            _firstMovedColumn.Width = new GridLength(_firstMovedColumn.ActualWidth + delta);
            _secondMovedColumn.Width = new GridLength(_secondMovedColumn.ActualWidth - delta);
            _initialPosition = currentPosition;
        }
    }

    private void Separator_PointerReleased(object sender, PointerRoutedEventArgs e)
    {
        _isResizing = false;
        (sender as UIElement).ReleasePointerCapture(e.Pointer);
    }
}
