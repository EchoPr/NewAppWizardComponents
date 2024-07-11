using System.Collections;
using System.Diagnostics;
using System.Reflection;
using Microsoft.UI.Xaml.Controls;
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
        if (!mainPageVM.CodeBlocks.Last().menu_only)
        {
            var tb = BuldViewCodeBlock(mainPageVM.CodeBlocks.Last(), mainPageVM.CodeBlocks.Count);
            CodeBlocks.Children.Add(tb);
        }
        else
        {
            mainPageVM.CodeBlocks.RemoveAt(mainPageVM.CodeBlocks.Count - 1);
        }
    }

    public TextBlock BuldViewCodeBlock(ApiEntry entry, int num)
    {
        var result = new TextBlock();

        if (entry.arg_type != null)
        {
            result.Inlines.Add(new Run { Text = $"{entry.arg_type.Name[1..]} ", Foreground = codeTypeBrush });
            result.Inlines.Add(new Run { Text = $"arg{num} ", Foreground = codeDefaultBrush });
            result.Inlines.Add(new Run { Text = "= new", Foreground = codeKeywordBrush });
            result.Inlines.Add(new Run { Text = "() {\n", Foreground = codeBracketsBrush });

            var instance = Activator.CreateInstance(entry.arg_type);
            PropertyInfo[] properties = entry.arg_type.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType)) continue;

                var value = property.GetValue(instance);

                result.Inlines.Add(new Run { Text = $"\t{property.Name} ", Foreground = codeDefaultBrush });
                result.Inlines.Add(new Run { Text = $"= ", Foreground = codeKeywordBrush });
                result.Inlines.Add(new Run { Text = $"{value} ", Foreground = codeValueBrush });
                result.Inlines.Add(new Run { Text = $",\n", Foreground = codeDefaultBrush });
            }

            result.Inlines.Add(new Run { Text = "}", Foreground = codeBracketsBrush });
            result.Inlines.Add(new Run { Text = ";\n", Foreground = codeDefaultBrush });

        }

        if (entry.ret_type != null)
        {
            result.Inlines.Add(new Run { Text = $"{entry.ret_type.Name[1..]} ", Foreground = codeTypeBrush });
            result.Inlines.Add(new Run { Text = $"ret{num} ", Foreground = codeDefaultBrush });
            result.Inlines.Add(new Run { Text = $"= ", Foreground = codeKeywordBrush });
        }

        result.Inlines.Add(new Run { Text = "qform.", Foreground = codeDefaultBrush });
        result.Inlines.Add(new Run { Text = $"{entry.Name}", Foreground = codeMethodBrush });
        result.Inlines.Add(new Run { Text = "(", Foreground = codeBracketsBrush });

        if (entry.arg_type != null) { result.Inlines.Add(new Run { Text = $"arg{num}", Foreground = codeDefaultBrush }); }

        result.Inlines.Add(new Run { Text = ")", Foreground = codeBracketsBrush });
        result.Inlines.Add(new Run { Text = ";\n", Foreground = codeDefaultBrush });


        result.Style = (Style)Resources["CodeBlock"];

        return result;
    }

    public void ClearCodeBlocks(object sender, EventArgs e)
    {
        CodeBlocks.Children.Clear();
    }
}
