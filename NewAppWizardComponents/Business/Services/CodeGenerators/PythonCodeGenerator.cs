using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Documents;

namespace NewAppWizardComponents;
public class PythonCodeGenerator : ICodeGenerator
{
    public List<ViewCodeSample> GenerateCodeEntry(ApiEntry entry, int num, CodeGenerationMode mode = CodeGenerationMode.StepByStep)
    {
        var codeEntries = new List<ViewCodeSample>();

        if (entry.arg_type != null)
        {
            codeEntries.Add(new ViewCodeSample($"arg{num + 1} ", ViewCodeSampleType.Default));
            codeEntries.Add(new ViewCodeSample("= ", ViewCodeSampleType.Keyword));
            codeEntries.Add(new ViewCodeSample($"{entry.arg_type.Name[1..]}", ViewCodeSampleType.Type));
            codeEntries.Add(new ViewCodeSample("()\n", ViewCodeSampleType.Brackets));

            PropertyInfo[] properties = entry.arg_type.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                var value = property.GetValue(entry.arg_value);

                if (property.PropertyType.IsGenericType &&
                    property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    var val = value as IList;
                    var elementType = property.PropertyType.GetGenericArguments()[0];

                    if (elementType == typeof(int) || elementType == typeof(double) ||
                        elementType == typeof(bool) || elementType == typeof(string))
                    {
                        foreach (var v in val)
                        {
                            codeEntries.Add(new ViewCodeSample($"arg{num + 1}.{property.Name}.", ViewCodeSampleType.Default));
                            codeEntries.Add(new ViewCodeSample("append", ViewCodeSampleType.Method));
                            codeEntries.Add(new ViewCodeSample("(", ViewCodeSampleType.Brackets));
                            codeEntries.Add(new ViewCodeSample($"{v}", ViewCodeSampleType.Value));
                            codeEntries.Add(new ViewCodeSample(")\n", ViewCodeSampleType.Brackets));
                        }
                    }
                    else
                    {
                        for (int i = 0; i < val.Count; i++)
                        {
                            string objectName = val[i].GetType().Name[1..];
                            codeEntries.Add(new ViewCodeSample($"arg{num + 1}_object{i + 1}", ViewCodeSampleType.Default));
                            codeEntries.Add(new ViewCodeSample("= ", ViewCodeSampleType.Keyword));
                            codeEntries.Add(new ViewCodeSample($"{objectName}", ViewCodeSampleType.Type));
                            codeEntries.Add(new ViewCodeSample("()\n", ViewCodeSampleType.Brackets));

                            PropertyInfo[] innerProperties = val[i].GetType().GetProperties();
                            foreach (PropertyInfo innerProperty in innerProperties)
                            {
                                codeEntries.Add(new ViewCodeSample($"arg{num + 1}_object{i + 1}.{innerProperty.Name} ", ViewCodeSampleType.Default));
                                codeEntries.Add(new ViewCodeSample("= ", ViewCodeSampleType.Keyword));

                                if (innerProperty.PropertyType.IsEnum)
                                    codeEntries.Add(new ViewCodeSample($"{innerProperty.PropertyType.Name}.", ViewCodeSampleType.Value));

                                codeEntries.Add(new ViewCodeSample($"{innerProperty.GetValue(val[i])}\n", ViewCodeSampleType.Value));
                            }

                            codeEntries.Add(new ViewCodeSample($"arg{num + 1}.{property.Name}.", ViewCodeSampleType.Default));
                            codeEntries.Add(new ViewCodeSample("append", ViewCodeSampleType.Method));
                            codeEntries.Add(new ViewCodeSample("(", ViewCodeSampleType.Brackets));
                            codeEntries.Add(new ViewCodeSample($"arg{num + 1}_object{i + 1}", ViewCodeSampleType.Default));
                            codeEntries.Add(new ViewCodeSample(")\n", ViewCodeSampleType.Brackets));
                        }
                    }

                }
                else
                {
                    codeEntries.Add(new ViewCodeSample($"arg{num + 1}.", ViewCodeSampleType.Default));
                    codeEntries.Add(new ViewCodeSample($"{property.Name} ", ViewCodeSampleType.Default));
                    codeEntries.Add(new ViewCodeSample("= ", ViewCodeSampleType.Keyword));

                    if (property.PropertyType.IsEnum)
                        codeEntries.Add(new ViewCodeSample($"{property.PropertyType.Name}.", ViewCodeSampleType.Value));

                    codeEntries.Add(new ViewCodeSample($"{value}\n", ViewCodeSampleType.Value));
                }
            }
        }

        if (entry.ret_type != null)
        {
            codeEntries.Add(new ViewCodeSample($"ret{num + 1}:", ViewCodeSampleType.Default));
            codeEntries.Add(new ViewCodeSample($"{entry.ret_type.Name[1..]} ", ViewCodeSampleType.Type));
            codeEntries.Add(new ViewCodeSample("= ", ViewCodeSampleType.Keyword));
        }

        codeEntries.Add(new ViewCodeSample("qform.", ViewCodeSampleType.Default));
        codeEntries.Add(new ViewCodeSample($"{entry.Name}", ViewCodeSampleType.Method));
        codeEntries.Add(new ViewCodeSample("(", ViewCodeSampleType.Brackets));

        if (entry.arg_type != null) { codeEntries.Add(new ViewCodeSample($"arg{num + 1}", ViewCodeSampleType.Default)); }

        codeEntries.Add(new ViewCodeSample(")", ViewCodeSampleType.Brackets));

        return codeEntries;
    }

    public List<ViewCodeSample> GenerateApiSnippet(ApiEntry entry)
    {
        var snippetConfig = entry.arg_value as APytonSettings;

        var codeEntries = new List<ViewCodeSample>();

        codeEntries.Add(new ViewCodeSample("qform_base_dir ", ViewCodeSampleType.Keyword));
        codeEntries.Add(new ViewCodeSample("= ", ViewCodeSampleType.Keyword));
        codeEntries.Add(new ViewCodeSample("r'C:\\QForm UK\\11.0.2\\'\n\n", ViewCodeSampleType.Default));



        if (snippetConfig.import_dir == PythonQFormReference.default_folder.ToString())
        {
            codeEntries.Add(new ViewCodeSample("import ", ViewCodeSampleType.Keyword));
            codeEntries.Add(new ViewCodeSample("sys\n\n", ViewCodeSampleType.Default));

            codeEntries.Add(new ViewCodeSample("sys.path.", ViewCodeSampleType.Default));
            codeEntries.Add(new ViewCodeSample("append", ViewCodeSampleType.Method));
            codeEntries.Add(new ViewCodeSample("(", ViewCodeSampleType.Brackets));
            codeEntries.Add(new ViewCodeSample("qform_base_dir + r'\\API\\App\\Python'", ViewCodeSampleType.Default));
            codeEntries.Add(new ViewCodeSample(")\n", ViewCodeSampleType.Brackets));
        }


        codeEntries.Add(new ViewCodeSample("from ", ViewCodeSampleType.Keyword));
        codeEntries.Add(new ViewCodeSample("QFormAPI ", ViewCodeSampleType.Default));
        codeEntries.Add(new ViewCodeSample("import ", ViewCodeSampleType.Keyword));
        codeEntries.Add(new ViewCodeSample("*\n\n", ViewCodeSampleType.Default));


        codeEntries.Add(new ViewCodeSample("qform ", ViewCodeSampleType.Default));
        codeEntries.Add(new ViewCodeSample("= ", ViewCodeSampleType.Keyword));
        codeEntries.Add(new ViewCodeSample("QForm", ViewCodeSampleType.Type));
        codeEntries.Add(new ViewCodeSample("()\n", ViewCodeSampleType.Brackets));


        switch (snippetConfig.connection_type)
        {
            case nameof(PythonQFormInteractionType.script_starts):
                codeEntries.Add(new ViewCodeSample("qform", ViewCodeSampleType.Default));
                codeEntries.Add(new ViewCodeSample(".qform_start", ViewCodeSampleType.Method));
                codeEntries.Add(new ViewCodeSample("()", ViewCodeSampleType.Brackets));
                codeEntries.Add(new ViewCodeSample("\n", ViewCodeSampleType.Default));
                break;
            case nameof(PythonQFormInteractionType.qform_starts):
                if (snippetConfig.alt_connection)
                {
                    codeEntries.Add(new ViewCodeSample("if ", ViewCodeSampleType.Keyword));
                    codeEntries.Add(new ViewCodeSample("qform", ViewCodeSampleType.Default));
                    codeEntries.Add(new ViewCodeSample(".is_started_by_qform", ViewCodeSampleType.Method));
                    codeEntries.Add(new ViewCodeSample("()", ViewCodeSampleType.Brackets));
                    codeEntries.Add(new ViewCodeSample(":\n", ViewCodeSampleType.Default));

                    codeEntries.Add(new ViewCodeSample("    qform", ViewCodeSampleType.Default));
                    codeEntries.Add(new ViewCodeSample(".qform_attach", ViewCodeSampleType.Method));
                    codeEntries.Add(new ViewCodeSample("()", ViewCodeSampleType.Brackets));
                    codeEntries.Add(new ViewCodeSample("\n", ViewCodeSampleType.Default));

                    codeEntries.Add(new ViewCodeSample("else", ViewCodeSampleType.Keyword));
                    codeEntries.Add(new ViewCodeSample(":\n", ViewCodeSampleType.Default));

                    AddConnectionSnippet(codeEntries, isNested: true);
                    break;
                }

                codeEntries.Add(new ViewCodeSample("qform", ViewCodeSampleType.Default));
                codeEntries.Add(new ViewCodeSample(".qform_attach", ViewCodeSampleType.Method));
                codeEntries.Add(new ViewCodeSample("()", ViewCodeSampleType.Brackets));
                codeEntries.Add(new ViewCodeSample("\n", ViewCodeSampleType.Default));
                break;

            case nameof(PythonQFormInteractionType.script_connect):
                AddConnectionSnippet(codeEntries, isNested: false);
                break;
        }

        return codeEntries;
    }

    private void AddConnectionSnippet(List<ViewCodeSample> codeEntries, bool isNested)
    {
        codeEntries.Add(new ViewCodeSample($"{(isNested ? "    " : "")}qform", ViewCodeSampleType.Default));
        codeEntries.Add(new ViewCodeSample(".qform_dir_set", ViewCodeSampleType.Method));
        codeEntries.Add(new ViewCodeSample("(", ViewCodeSampleType.Brackets));
        codeEntries.Add(new ViewCodeSample("qform_base_dir + r'\\x64'", ViewCodeSampleType.Default));
        codeEntries.Add(new ViewCodeSample(")", ViewCodeSampleType.Brackets));
        codeEntries.Add(new ViewCodeSample("\n", ViewCodeSampleType.Default));

        codeEntries.Add(new ViewCodeSample($"{(isNested ? "    " : "")}qform_session_id", ViewCodeSampleType.Default));
        codeEntries.Add(new ViewCodeSample(" = ", ViewCodeSampleType.Keyword));
        codeEntries.Add(new ViewCodeSample("SessionId", ViewCodeSampleType.Type));
        codeEntries.Add(new ViewCodeSample("()", ViewCodeSampleType.Brackets));
        codeEntries.Add(new ViewCodeSample("\n", ViewCodeSampleType.Default));

        codeEntries.Add(new ViewCodeSample($"{(isNested ? "    " : "")}qform_session_id", ViewCodeSampleType.Default));
        codeEntries.Add(new ViewCodeSample(".session_id", ViewCodeSampleType.Default));
        codeEntries.Add(new ViewCodeSample(" = ", ViewCodeSampleType.Keyword));
        codeEntries.Add(new ViewCodeSample("0", ViewCodeSampleType.Default));
        codeEntries.Add(new ViewCodeSample("\n", ViewCodeSampleType.Default));

        codeEntries.Add(new ViewCodeSample($"{(isNested ? "    " : "")}qform", ViewCodeSampleType.Default));
        codeEntries.Add(new ViewCodeSample(".qform_attach_to", ViewCodeSampleType.Method));
        codeEntries.Add(new ViewCodeSample("(", ViewCodeSampleType.Brackets));
        codeEntries.Add(new ViewCodeSample("qform_session_id", ViewCodeSampleType.Default));
        codeEntries.Add(new ViewCodeSample(")", ViewCodeSampleType.Brackets));
    }
}


public enum PythonScriptType
{
    pyfile,
    notebook
}

public enum PythonQFormInteractionType
{
    script_starts,
    qform_starts,
    qform_starts_or_connect,
    script_connect
}

public enum PythonQFormReference
{
    default_folder,
    local_folder
}
