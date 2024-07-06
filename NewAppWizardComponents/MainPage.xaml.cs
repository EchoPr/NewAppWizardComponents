using System.Diagnostics;
using Microsoft.UI.Xaml.Controls;
using Windows.UI;

namespace NewAppWizardComponents;

public sealed partial class MainPage : Page
{
    public MainPage()
    {
        this.InitializeComponent();

        MainPageVM mainPageVM = new MainPageVM();
        this.DataContext = mainPageVM;

        //for (int i = 0; i < 3; i++)
        //{
        //    RowDefinition rowDefinition = new RowDefinition();
        //    MethodArgumentsTable.RowDefinitions.Add(rowDefinition);
        //}

        //for (int i = 0; i < 3; i++)
        //{
        //    TextBlock textBlock = new TextBlock
        //    {
        //        Text = mainPageVM.allMethods.Functions[i].Name,
        //        VerticalAlignment = VerticalAlignment.Center,
        //        HorizontalAlignment = HorizontalAlignment.Center
        //    };

        //    // Установка строки для элемента
        //    Grid.SetRow(textBlock, i);

        //    // Добавление элемента в Grid
        //    MethodArgumentsTable.Children.Add(textBlock);
        //}
    }
}
