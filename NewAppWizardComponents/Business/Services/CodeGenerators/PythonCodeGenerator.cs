using System;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Uno.Extensions;
using Windows.UI.ViewManagement;

namespace NewAppWizardComponents;
public class PythonCodeGenerator : ICodeGenerator
{
    public List<ViewCodeSample> GenerateCodeEntry(ApiEntry entry, int num, CodeGenerationMode mode = CodeGenerationMode.StepByStep)
    {
        var codeEntries = new List<ViewCodeSample>();

        if (entry.comment?.Count > 0)
        {
            foreach (var comment in entry.comment)
            {
                codeEntries.Add(new ViewCodeSample($"# {comment}\n", ViewCodeSampleType.Comment));
            }
        }

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
                            if (elementType == typeof(string))
                                codeEntries.Add(new ViewCodeSample($"\"{v}\"", ViewCodeSampleType.Comment));
                            else
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

                    if (property.PropertyType == typeof(string))
                        codeEntries.Add(new ViewCodeSample($"\"{value}\"\n", ViewCodeSampleType.Comment));
                    else
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

    public List<ViewCodeSample> GenerateApiSnippet(ApiEntry entry, string qformBaseDir)
    {
        var snippetConfig = entry.arg_value as APytonSettings;

        var codeEntries = new List<ViewCodeSample>();

        codeEntries.Add(new ViewCodeSample("qform_base_dir ", ViewCodeSampleType.Keyword));
        codeEntries.Add(new ViewCodeSample("= ", ViewCodeSampleType.Keyword));
        codeEntries.Add(new ViewCodeSample($"r\"{qformBaseDir}\"\n\n", ViewCodeSampleType.Comment));



        if (snippetConfig.import_dir == APIQFormReference.default_folder.ToString())
        {
            codeEntries.Add(new ViewCodeSample("import ", ViewCodeSampleType.Keyword));
            codeEntries.Add(new ViewCodeSample("sys\n\n", ViewCodeSampleType.Default));

            codeEntries.Add(new ViewCodeSample("sys.path.", ViewCodeSampleType.Default));
            codeEntries.Add(new ViewCodeSample("append", ViewCodeSampleType.Method));
            codeEntries.Add(new ViewCodeSample("(", ViewCodeSampleType.Brackets));
            codeEntries.Add(new ViewCodeSample("qform_base_dir + ", ViewCodeSampleType.Default));
            codeEntries.Add(new ViewCodeSample("r'\\API\\App\\Python'", ViewCodeSampleType.Comment));
            codeEntries.Add(new ViewCodeSample(")\n", ViewCodeSampleType.Brackets));
        }


        codeEntries.Add(new ViewCodeSample("from ", ViewCodeSampleType.Keyword));
        codeEntries.Add(new ViewCodeSample("QFormAPI ", ViewCodeSampleType.Default));
        codeEntries.Add(new ViewCodeSample("import ", ViewCodeSampleType.Keyword));
        codeEntries.Add(new ViewCodeSample("*\n\n", ViewCodeSampleType.Default));


        codeEntries.Add(new ViewCodeSample("qform ", ViewCodeSampleType.Default));
        codeEntries.Add(new ViewCodeSample("= ", ViewCodeSampleType.Keyword));
        codeEntries.Add(new ViewCodeSample("QForm", ViewCodeSampleType.Type));
        codeEntries.Add(new ViewCodeSample("()\n\n", ViewCodeSampleType.Brackets));

        if (!snippetConfig.use_qform_exceptions)
        {
            codeEntries.Add(new ViewCodeSample("qform.", ViewCodeSampleType.Default));
            codeEntries.Add(new ViewCodeSample("exceptions_disable", ViewCodeSampleType.Method));
            codeEntries.Add(new ViewCodeSample("()\n", ViewCodeSampleType.Brackets));

            codeEntries.Add(new ViewCodeSample("try", ViewCodeSampleType.Keyword));
            codeEntries.Add(new ViewCodeSample(":\n", ViewCodeSampleType.Default));
        }

        string prefix = snippetConfig.use_qform_exceptions ? "" : "\t";

        switch (snippetConfig.connection_type)
        {
            case nameof(APIQFormInteractionType.script_starts):
                codeEntries.Add(new ViewCodeSample($"{prefix}qform.", ViewCodeSampleType.Default));
                codeEntries.Add(new ViewCodeSample("qform_dir_set", ViewCodeSampleType.Method));
                codeEntries.Add(new ViewCodeSample("(", ViewCodeSampleType.Brackets));
                codeEntries.Add(new ViewCodeSample("qform_base_dir + ", ViewCodeSampleType.Default));
                codeEntries.Add(new ViewCodeSample("r'\\x64'", ViewCodeSampleType.Comment));
                codeEntries.Add(new ViewCodeSample(")\n", ViewCodeSampleType.Brackets));

                if (snippetConfig.use_qform_exceptions)

                    codeEntries.Add(new ViewCodeSample($"if not ", ViewCodeSampleType.Keyword));

                codeEntries.Add(new ViewCodeSample($"{prefix}qform.", ViewCodeSampleType.Default));
                codeEntries.Add(new ViewCodeSample("qform_start", ViewCodeSampleType.Method));
                codeEntries.Add(new ViewCodeSample("()", ViewCodeSampleType.Brackets));
                codeEntries.Add(new ViewCodeSample($"{(snippetConfig.use_qform_exceptions ? ":" : "")}\n", ViewCodeSampleType.Default));

                if (snippetConfig.use_qform_exceptions)
                {
                    codeEntries.Add(new ViewCodeSample("\tprint", ViewCodeSampleType.Method));
                    codeEntries.Add(new ViewCodeSample("(", ViewCodeSampleType.Brackets));
                    codeEntries.Add(new ViewCodeSample($"qform.", ViewCodeSampleType.Default));
                    codeEntries.Add(new ViewCodeSample("last_error", ViewCodeSampleType.Method));
                    codeEntries.Add(new ViewCodeSample("())\n", ViewCodeSampleType.Brackets));
                }
                break;
            case nameof(APIQFormInteractionType.qform_starts):
                if (snippetConfig.alt_connection)
                {
                    codeEntries.Add(new ViewCodeSample($"{prefix}if ", ViewCodeSampleType.Keyword));
                    codeEntries.Add(new ViewCodeSample("qform.", ViewCodeSampleType.Default));
                    codeEntries.Add(new ViewCodeSample("is_started_by_qform", ViewCodeSampleType.Method));
                    codeEntries.Add(new ViewCodeSample("()", ViewCodeSampleType.Brackets));
                    codeEntries.Add(new ViewCodeSample(":\n\t", ViewCodeSampleType.Default));

                    if (snippetConfig.use_qform_exceptions)

                        codeEntries.Add(new ViewCodeSample($"if not ", ViewCodeSampleType.Keyword));

                    codeEntries.Add(new ViewCodeSample($"{prefix}qform.", ViewCodeSampleType.Default));
                    codeEntries.Add(new ViewCodeSample("qform_attach", ViewCodeSampleType.Method));
                    codeEntries.Add(new ViewCodeSample("()", ViewCodeSampleType.Brackets));
                    codeEntries.Add(new ViewCodeSample($"{(snippetConfig.use_qform_exceptions ? ":" : "")}\n", ViewCodeSampleType.Default));

                    if (snippetConfig.use_qform_exceptions)
                    {
                        codeEntries.Add(new ViewCodeSample("\t\tprint", ViewCodeSampleType.Method));
                        codeEntries.Add(new ViewCodeSample("(", ViewCodeSampleType.Brackets));
                        codeEntries.Add(new ViewCodeSample($"qform.", ViewCodeSampleType.Default));
                        codeEntries.Add(new ViewCodeSample("last_error", ViewCodeSampleType.Method));
                        codeEntries.Add(new ViewCodeSample("())\n", ViewCodeSampleType.Brackets));
                    }

                    codeEntries.Add(new ViewCodeSample($"{prefix}else", ViewCodeSampleType.Keyword));
                    codeEntries.Add(new ViewCodeSample(":\n", ViewCodeSampleType.Default));

                    AddConnectionSnippet(codeEntries, prefix + "\t", snippetConfig.use_qform_exceptions);
                    break;
                }

                if (snippetConfig.use_qform_exceptions)

                    codeEntries.Add(new ViewCodeSample($"if not ", ViewCodeSampleType.Keyword));

                codeEntries.Add(new ViewCodeSample($"{prefix}qform.", ViewCodeSampleType.Default));
                codeEntries.Add(new ViewCodeSample("qform_attach", ViewCodeSampleType.Method));
                codeEntries.Add(new ViewCodeSample("()", ViewCodeSampleType.Brackets));
                codeEntries.Add(new ViewCodeSample($"{(snippetConfig.use_qform_exceptions ? ":" : "")}\n", ViewCodeSampleType.Default));

                if (snippetConfig.use_qform_exceptions)
                {
                    codeEntries.Add(new ViewCodeSample("\tprint", ViewCodeSampleType.Method));
                    codeEntries.Add(new ViewCodeSample("(", ViewCodeSampleType.Brackets));
                    codeEntries.Add(new ViewCodeSample($"qform.", ViewCodeSampleType.Default));
                    codeEntries.Add(new ViewCodeSample("last_error", ViewCodeSampleType.Method));
                    codeEntries.Add(new ViewCodeSample("())\n", ViewCodeSampleType.Brackets));
                }

                break;

            case nameof(APIQFormInteractionType.script_connect):
                AddConnectionSnippet(codeEntries, prefix, snippetConfig.use_qform_exceptions);
                break;
        }

        if (!snippetConfig.use_qform_exceptions)
        {
            codeEntries.Add(new ViewCodeSample("except ", ViewCodeSampleType.Keyword));
            codeEntries.Add(new ViewCodeSample("Exception ", ViewCodeSampleType.Type));
            codeEntries.Add(new ViewCodeSample("as ", ViewCodeSampleType.Keyword));
            codeEntries.Add(new ViewCodeSample("ex:\n", ViewCodeSampleType.Default));

            codeEntries.Add(new ViewCodeSample("\tprint", ViewCodeSampleType.Method));
            codeEntries.Add(new ViewCodeSample("(", ViewCodeSampleType.Brackets));
            codeEntries.Add(new ViewCodeSample("ex", ViewCodeSampleType.Default));
            codeEntries.Add(new ViewCodeSample(")", ViewCodeSampleType.Brackets));
        }

        return codeEntries;
    }

    private void AddConnectionSnippet(List<ViewCodeSample> codeEntries, string prefix, bool errorHandling)
    {
        codeEntries.Add(new ViewCodeSample($"{prefix}qform.", ViewCodeSampleType.Default));
        codeEntries.Add(new ViewCodeSample("qform_dir_set", ViewCodeSampleType.Method));
        codeEntries.Add(new ViewCodeSample("(", ViewCodeSampleType.Brackets));
        codeEntries.Add(new ViewCodeSample("qform_base_dir + ", ViewCodeSampleType.Default));
        codeEntries.Add(new ViewCodeSample("r'\\x64'", ViewCodeSampleType.Comment));
        codeEntries.Add(new ViewCodeSample(")\n", ViewCodeSampleType.Brackets));

        codeEntries.Add(new ViewCodeSample($"{prefix}qform_session_id", ViewCodeSampleType.Default));
        codeEntries.Add(new ViewCodeSample(" = ", ViewCodeSampleType.Keyword));
        codeEntries.Add(new ViewCodeSample("SessionId", ViewCodeSampleType.Type));
        codeEntries.Add(new ViewCodeSample("()\n", ViewCodeSampleType.Brackets));

        codeEntries.Add(new ViewCodeSample($"{prefix}qform_session_id.", ViewCodeSampleType.Default));
        codeEntries.Add(new ViewCodeSample("session_id", ViewCodeSampleType.Method));
        codeEntries.Add(new ViewCodeSample(" = ", ViewCodeSampleType.Keyword));
        codeEntries.Add(new ViewCodeSample("0\n", ViewCodeSampleType.Default));

        if (errorHandling)
            codeEntries.Add(new ViewCodeSample($"{prefix}if not ", ViewCodeSampleType.Keyword));

        codeEntries.Add(new ViewCodeSample($"{(errorHandling ? "" : prefix)}qform.", ViewCodeSampleType.Default));
        codeEntries.Add(new ViewCodeSample("qform_attach_to", ViewCodeSampleType.Method));
        codeEntries.Add(new ViewCodeSample("(", ViewCodeSampleType.Brackets));
        codeEntries.Add(new ViewCodeSample("qform_session_id", ViewCodeSampleType.Default));
        codeEntries.Add(new ViewCodeSample(")", ViewCodeSampleType.Brackets));
        codeEntries.Add(new ViewCodeSample($"{(errorHandling ? ":" : "")}\n", ViewCodeSampleType.Default));

        if (errorHandling)
        {
            codeEntries.Add(new ViewCodeSample($"{prefix}\tprint", ViewCodeSampleType.Method));
            codeEntries.Add(new ViewCodeSample("(", ViewCodeSampleType.Brackets));
            codeEntries.Add(new ViewCodeSample($"qform.", ViewCodeSampleType.Default));
            codeEntries.Add(new ViewCodeSample("last_error", ViewCodeSampleType.Method));
            codeEntries.Add(new ViewCodeSample("())\n", ViewCodeSampleType.Brackets));
        }
    }
}
