using System;
using System.Collections;
using System.Reflection;
using Windows.UI.ViewManagement;

namespace NewAppWizardComponents;
public class CSharpCodeGenerator : ICodeGenerator
{
    public List<ViewCodeSample> GenerateCodeEntry(ApiEntry entry, int num, CodeGenerationMode mode)
    {
        return mode == CodeGenerationMode.ObjectInit ? GenerateCodeEntryObjectInit(entry, num) : GenerateCodeEntryStepByStep(entry, num);
    }

    public List<ViewCodeSample> GenerateCodeEntryObjectInit(ApiEntry entry, int num)
    {
        var codeEntries = new List<ViewCodeSample>();

        if (entry.comment?.Count > 0)
        {
            foreach (var comment in entry.comment)
            {
                codeEntries.Add(new ViewCodeSample($"// {comment}\n", ViewCodeSampleType.Comment));
            }
        }

        if (entry.arg_type != null)
        {
            codeEntries.Add(new ViewCodeSample($"{entry.arg_type.Name[1..]} ", ViewCodeSampleType.Type));
            codeEntries.Add(new ViewCodeSample($"arg{num + 1} ", ViewCodeSampleType.Default));
            codeEntries.Add(new ViewCodeSample("= new", ViewCodeSampleType.Keyword));
            codeEntries.Add(new ViewCodeSample("() {\n", ViewCodeSampleType.Brackets));

            PropertyInfo[] properties = entry.arg_type.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (property.PropertyType.IsGenericType &&
                    property.PropertyType.GetGenericTypeDefinition() == typeof(List<>)) continue;

                var value = property.GetValue(entry.arg_value);

                codeEntries.Add(new ViewCodeSample($"\t{property.Name} ", ViewCodeSampleType.Default));
                codeEntries.Add(new ViewCodeSample($"= ", ViewCodeSampleType.Keyword));

                if (property.PropertyType.IsEnum)
                    codeEntries.Add(new ViewCodeSample($"{property.PropertyType.Name}.", ViewCodeSampleType.Value));

                if (property.PropertyType == typeof(String))
                    codeEntries.Add(new ViewCodeSample($"\"{value}\"", ViewCodeSampleType.Comment));
                else
                    codeEntries.Add(new ViewCodeSample($"{value}", ViewCodeSampleType.Value));

                codeEntries.Add(new ViewCodeSample($",\n", ViewCodeSampleType.Default));
            }

            codeEntries.Add(new ViewCodeSample("}", ViewCodeSampleType.Brackets));
            codeEntries.Add(new ViewCodeSample(";\n", ViewCodeSampleType.Default));
        }

        if (entry.ret_type != null)
        {
            codeEntries.Add(new ViewCodeSample($"{entry.ret_type.Name[1..]} ", ViewCodeSampleType.Type));
            codeEntries.Add(new ViewCodeSample($"ret{num + 1} ", ViewCodeSampleType.Default));
            codeEntries.Add(new ViewCodeSample($"= ", ViewCodeSampleType.Keyword));

        }

        codeEntries.Add(new ViewCodeSample("qform.", ViewCodeSampleType.Default));
        codeEntries.Add(new ViewCodeSample($"{entry.Name}", ViewCodeSampleType.Method));
        codeEntries.Add(new ViewCodeSample("(", ViewCodeSampleType.Brackets));


        if (entry.arg_type != null)
        {
            codeEntries.Add(new ViewCodeSample($"arg{num + 1}", ViewCodeSampleType.Default));
        }

        codeEntries.Add(new ViewCodeSample(")", ViewCodeSampleType.Brackets));
        codeEntries.Add(new ViewCodeSample(";", ViewCodeSampleType.Default));

        return codeEntries;
    }

    public List<ViewCodeSample> GenerateCodeEntryStepByStep(ApiEntry entry, int num)
    {
        var codeEntries = new List<ViewCodeSample>();

        if (entry.comment?.Count > 0)
        {
            foreach (var comment in entry.comment)
            {
                codeEntries.Add(new ViewCodeSample($"// {comment}\n", ViewCodeSampleType.Comment));
            }
        }

        if (entry.arg_type != null)
        {
            codeEntries.Add(new ViewCodeSample($"{entry.arg_type.Name[1..]} ", ViewCodeSampleType.Type));
            codeEntries.Add(new ViewCodeSample($"arg{num + 1} ", ViewCodeSampleType.Default));
            codeEntries.Add(new ViewCodeSample("= new ", ViewCodeSampleType.Keyword));
            codeEntries.Add(new ViewCodeSample($"{entry.arg_type.Name[1..]}", ViewCodeSampleType.Type));
            codeEntries.Add(new ViewCodeSample("()", ViewCodeSampleType.Brackets));
            codeEntries.Add(new ViewCodeSample(";\n", ViewCodeSampleType.Default));

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
                        elementType == typeof(bool) || elementType == typeof(String))
                    {
                        foreach (var v in val)
                        {
                            codeEntries.Add(new ViewCodeSample($"arg{num + 1}.{property.Name}.", ViewCodeSampleType.Default));
                            codeEntries.Add(new ViewCodeSample("Add", ViewCodeSampleType.Method));
                            codeEntries.Add(new ViewCodeSample("(", ViewCodeSampleType.Brackets));
                            codeEntries.Add(new ViewCodeSample($"{v}", ViewCodeSampleType.Value));
                            codeEntries.Add(new ViewCodeSample(")", ViewCodeSampleType.Brackets));
                            codeEntries.Add(new ViewCodeSample(";\n", ViewCodeSampleType.Default));
                        }
                    }
                    else
                    {
                        for (int i = 0; i < val.Count; i++)
                        {
                            string objectName = val[i].GetType().Name[1..];
                            codeEntries.Add(new ViewCodeSample($"{objectName} ", ViewCodeSampleType.Type));
                            codeEntries.Add(new ViewCodeSample($"arg{num + 1}_object{i + 1}", ViewCodeSampleType.Default));
                            codeEntries.Add(new ViewCodeSample("= new ", ViewCodeSampleType.Keyword));
                            codeEntries.Add(new ViewCodeSample($"{objectName}", ViewCodeSampleType.Type));
                            codeEntries.Add(new ViewCodeSample("()", ViewCodeSampleType.Brackets));
                            codeEntries.Add(new ViewCodeSample(";\n", ViewCodeSampleType.Default));

                            PropertyInfo[] innerProperties = val[i].GetType().GetProperties();
                            foreach (PropertyInfo innerProperty in innerProperties)
                            {
                                codeEntries.Add(new ViewCodeSample($"arg{num + 1}_object{i + 1}.{innerProperty.Name} ", ViewCodeSampleType.Default));
                                codeEntries.Add(new ViewCodeSample("= ", ViewCodeSampleType.Keyword));

                                if (innerProperty.PropertyType.IsEnum)
                                    codeEntries.Add(new ViewCodeSample($"{innerProperty.PropertyType.Name}.", ViewCodeSampleType.Value));

                                codeEntries.Add(new ViewCodeSample($"{innerProperty.GetValue(val[i])}", ViewCodeSampleType.Value));
                                codeEntries.Add(new ViewCodeSample(";\n", ViewCodeSampleType.Default));
                            }

                            codeEntries.Add(new ViewCodeSample($"arg{num + 1}.{property.Name}.", ViewCodeSampleType.Default));
                            codeEntries.Add(new ViewCodeSample("Add", ViewCodeSampleType.Method));
                            codeEntries.Add(new ViewCodeSample("(", ViewCodeSampleType.Brackets));
                            codeEntries.Add(new ViewCodeSample($"arg{num + 1}_object{i + 1}", ViewCodeSampleType.Default));
                            codeEntries.Add(new ViewCodeSample(");\n", ViewCodeSampleType.Brackets));
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

                    codeEntries.Add(new ViewCodeSample($"{value}", ViewCodeSampleType.Value));
                    codeEntries.Add(new ViewCodeSample(";\n", ViewCodeSampleType.Default));
                }
            }
        }

        if (entry.ret_type != null)
        {
            codeEntries.Add(new ViewCodeSample($"{entry.ret_type.Name[1..]} ", ViewCodeSampleType.Type));
            codeEntries.Add(new ViewCodeSample($"ret{num + 1} ", ViewCodeSampleType.Default));
            codeEntries.Add(new ViewCodeSample("= ", ViewCodeSampleType.Keyword));
        }

        codeEntries.Add(new ViewCodeSample("qform.", ViewCodeSampleType.Default));
        codeEntries.Add(new ViewCodeSample($"{entry.Name}", ViewCodeSampleType.Method));
        codeEntries.Add(new ViewCodeSample("(", ViewCodeSampleType.Brackets));

        if (entry.arg_type != null) { codeEntries.Add(new ViewCodeSample($"arg{num + 1}", ViewCodeSampleType.Default)); }

        codeEntries.Add(new ViewCodeSample(")", ViewCodeSampleType.Brackets));
        codeEntries.Add(new ViewCodeSample(";\n", ViewCodeSampleType.Default)); 
        
        return codeEntries;
    }

    public List<ViewCodeSample> GenerateApiSnippet(ApiEntry entry, string qformBaseDir)
    {
        var snippetConfig = entry.arg_value as ACSharpSettings;

        var codeEntries = new List<ViewCodeSample>();

        codeEntries.Add(new ViewCodeSample("using ", ViewCodeSampleType.Keyword));
        codeEntries.Add(new ViewCodeSample("QFormAPI;\n\n", ViewCodeSampleType.Default));

        if (snippetConfig.use_static) codeEntries.Add(new ViewCodeSample("static ", ViewCodeSampleType.Keyword));
        codeEntries.Add(new ViewCodeSample("class ", ViewCodeSampleType.Keyword));
        codeEntries.Add(new ViewCodeSample($"{snippetConfig.class_name} ", ViewCodeSampleType.Default));
        codeEntries.Add(new ViewCodeSample("{\n", ViewCodeSampleType.Brackets));

        if (snippetConfig.use_static) codeEntries.Add(new ViewCodeSample("    static ", ViewCodeSampleType.Keyword));
        codeEntries.Add(new ViewCodeSample("string ", ViewCodeSampleType.Keyword));
        codeEntries.Add(new ViewCodeSample("qform_base_dir ", ViewCodeSampleType.Default));
        codeEntries.Add(new ViewCodeSample("= ", ViewCodeSampleType.Keyword));
        codeEntries.Add(new ViewCodeSample($"@\"{qformBaseDir}\"", ViewCodeSampleType.Comment));
        codeEntries.Add(new ViewCodeSample(";\n", ViewCodeSampleType.Default));

        if (snippetConfig.use_static) codeEntries.Add(new ViewCodeSample("    static ", ViewCodeSampleType.Keyword));
        codeEntries.Add(new ViewCodeSample("QForm ", ViewCodeSampleType.Type));
        codeEntries.Add(new ViewCodeSample("qform ", ViewCodeSampleType.Default));
        codeEntries.Add(new ViewCodeSample("= ", ViewCodeSampleType.Keyword));
        codeEntries.Add(new ViewCodeSample("new ", ViewCodeSampleType.Keyword));
        codeEntries.Add(new ViewCodeSample("QForm", ViewCodeSampleType.Type));
        codeEntries.Add(new ViewCodeSample("()", ViewCodeSampleType.Brackets));
        codeEntries.Add(new ViewCodeSample(";\n\n", ViewCodeSampleType.Default));

        codeEntries.Add(new ViewCodeSample("    public ", ViewCodeSampleType.Keyword));
        if (snippetConfig.use_static) codeEntries.Add(new ViewCodeSample("static ", ViewCodeSampleType.Keyword));
        codeEntries.Add(new ViewCodeSample("void ", ViewCodeSampleType.Keyword));
        codeEntries.Add(new ViewCodeSample("Execute", ViewCodeSampleType.Method));
        codeEntries.Add(new ViewCodeSample("(){\n", ViewCodeSampleType.Brackets));

        switch (snippetConfig.connection_type)
        {
            case nameof(APIQFormInteractionType.script_starts):
                codeEntries.Add(new ViewCodeSample("\t\tqform.", ViewCodeSampleType.Default));
                codeEntries.Add(new ViewCodeSample("qform_dir_set", ViewCodeSampleType.Method));
                codeEntries.Add(new ViewCodeSample("(", ViewCodeSampleType.Brackets));
                codeEntries.Add(new ViewCodeSample("qform_base_dir + ", ViewCodeSampleType.Default));
                codeEntries.Add(new ViewCodeSample("@\"\\x64\"", ViewCodeSampleType.Comment));
                codeEntries.Add(new ViewCodeSample(")", ViewCodeSampleType.Brackets));
                codeEntries.Add(new ViewCodeSample(";\n", ViewCodeSampleType.Default));

                codeEntries.Add(new ViewCodeSample("\t\tqform.", ViewCodeSampleType.Default));
                codeEntries.Add(new ViewCodeSample("qform_start", ViewCodeSampleType.Method));
                codeEntries.Add(new ViewCodeSample("()", ViewCodeSampleType.Brackets));
                codeEntries.Add(new ViewCodeSample(";\n", ViewCodeSampleType.Default));

                break;

            case nameof(APIQFormInteractionType.qform_starts):
                if (snippetConfig.alt_connection)
                {
                    codeEntries.Add(new ViewCodeSample("\tif", ViewCodeSampleType.Keyword));
                    codeEntries.Add(new ViewCodeSample(" (", ViewCodeSampleType.Brackets));
                    codeEntries.Add(new ViewCodeSample("qform.", ViewCodeSampleType.Default));
                    codeEntries.Add(new ViewCodeSample("is_started_by_qform", ViewCodeSampleType.Method));
                    codeEntries.Add(new ViewCodeSample("())", ViewCodeSampleType.Brackets));
                    codeEntries.Add(new ViewCodeSample("{\n", ViewCodeSampleType.Brackets));

                    codeEntries.Add(new ViewCodeSample("\t    qform.", ViewCodeSampleType.Default));
                    codeEntries.Add(new ViewCodeSample("qform_attach", ViewCodeSampleType.Method));
                    codeEntries.Add(new ViewCodeSample("()", ViewCodeSampleType.Brackets));
                    codeEntries.Add(new ViewCodeSample(";\n", ViewCodeSampleType.Default));

                    codeEntries.Add(new ViewCodeSample("\t} ", ViewCodeSampleType.Brackets));
                    codeEntries.Add(new ViewCodeSample("else ", ViewCodeSampleType.Keyword));
                    codeEntries.Add(new ViewCodeSample("\t{\n", ViewCodeSampleType.Brackets));

                    codeEntries.Add(new ViewCodeSample("\t    qform.", ViewCodeSampleType.Default));
                    codeEntries.Add(new ViewCodeSample("qform_dir_set", ViewCodeSampleType.Method));
                    codeEntries.Add(new ViewCodeSample("(", ViewCodeSampleType.Brackets));
                    codeEntries.Add(new ViewCodeSample("qform_base_dir + ", ViewCodeSampleType.Default));
                    codeEntries.Add(new ViewCodeSample("@\"\\x64\"", ViewCodeSampleType.Comment));
                    codeEntries.Add(new ViewCodeSample(")", ViewCodeSampleType.Brackets));
                    codeEntries.Add(new ViewCodeSample(";\n", ViewCodeSampleType.Default));

                    codeEntries.Add(new ViewCodeSample("\t    SessionId ", ViewCodeSampleType.Default));
                    codeEntries.Add(new ViewCodeSample("qform_session_id ", ViewCodeSampleType.Default));
                    codeEntries.Add(new ViewCodeSample("= ", ViewCodeSampleType.Keyword));
                    codeEntries.Add(new ViewCodeSample("new ", ViewCodeSampleType.Keyword));
                    codeEntries.Add(new ViewCodeSample("SessionId", ViewCodeSampleType.Type));
                    codeEntries.Add(new ViewCodeSample("()", ViewCodeSampleType.Brackets));
                    codeEntries.Add(new ViewCodeSample(";\n", ViewCodeSampleType.Default));

                    codeEntries.Add(new ViewCodeSample("\t    qform_session_id.", ViewCodeSampleType.Default));
                    codeEntries.Add(new ViewCodeSample("session_id", ViewCodeSampleType.Default));
                    codeEntries.Add(new ViewCodeSample(" = ", ViewCodeSampleType.Keyword));
                    codeEntries.Add(new ViewCodeSample("0", ViewCodeSampleType.Value));
                    codeEntries.Add(new ViewCodeSample(";\n", ViewCodeSampleType.Default));

                    codeEntries.Add(new ViewCodeSample("\t    qform.", ViewCodeSampleType.Default));
                    codeEntries.Add(new ViewCodeSample("qform_attach_to", ViewCodeSampleType.Method));
                    codeEntries.Add(new ViewCodeSample("(", ViewCodeSampleType.Brackets));
                    codeEntries.Add(new ViewCodeSample("qform_session_id", ViewCodeSampleType.Default));
                    codeEntries.Add(new ViewCodeSample(")", ViewCodeSampleType.Brackets));
                    codeEntries.Add(new ViewCodeSample(";\n", ViewCodeSampleType.Default));

                    codeEntries.Add(new ViewCodeSample("\t}\n", ViewCodeSampleType.Brackets));

                    break;
                }

                codeEntries.Add(new ViewCodeSample("\tqform.", ViewCodeSampleType.Default));
                codeEntries.Add(new ViewCodeSample("qform_attach", ViewCodeSampleType.Method));
                codeEntries.Add(new ViewCodeSample("()", ViewCodeSampleType.Brackets));
                codeEntries.Add(new ViewCodeSample(";\n", ViewCodeSampleType.Default));

                break;

            case nameof(APIQFormInteractionType.script_connect):
                codeEntries.Add(new ViewCodeSample("\tqform.", ViewCodeSampleType.Default));
                codeEntries.Add(new ViewCodeSample("qform_dir_set", ViewCodeSampleType.Method));
                codeEntries.Add(new ViewCodeSample("(", ViewCodeSampleType.Brackets));
                codeEntries.Add(new ViewCodeSample("qform_base_dir + ", ViewCodeSampleType.Default));
                codeEntries.Add(new ViewCodeSample("@\"\\x64\"", ViewCodeSampleType.Comment));
                codeEntries.Add(new ViewCodeSample(")", ViewCodeSampleType.Brackets));
                codeEntries.Add(new ViewCodeSample(";\n", ViewCodeSampleType.Default));

                codeEntries.Add(new ViewCodeSample("\tSessionId ", ViewCodeSampleType.Default));
                codeEntries.Add(new ViewCodeSample("qform_session_id ", ViewCodeSampleType.Default));
                codeEntries.Add(new ViewCodeSample("= ", ViewCodeSampleType.Keyword));
                codeEntries.Add(new ViewCodeSample("new ", ViewCodeSampleType.Keyword));
                codeEntries.Add(new ViewCodeSample("SessionId", ViewCodeSampleType.Type));
                codeEntries.Add(new ViewCodeSample("()", ViewCodeSampleType.Brackets));
                codeEntries.Add(new ViewCodeSample(";\n", ViewCodeSampleType.Default));

                codeEntries.Add(new ViewCodeSample("\tqform_session_id.", ViewCodeSampleType.Default));
                codeEntries.Add(new ViewCodeSample("session_id", ViewCodeSampleType.Default));
                codeEntries.Add(new ViewCodeSample(" = ", ViewCodeSampleType.Keyword));
                codeEntries.Add(new ViewCodeSample("0", ViewCodeSampleType.Value));
                codeEntries.Add(new ViewCodeSample(";\n", ViewCodeSampleType.Default));

                codeEntries.Add(new ViewCodeSample("\tqform.", ViewCodeSampleType.Default));
                codeEntries.Add(new ViewCodeSample("qform_attach_to", ViewCodeSampleType.Method));
                codeEntries.Add(new ViewCodeSample("(", ViewCodeSampleType.Brackets));
                codeEntries.Add(new ViewCodeSample("qform_session_id", ViewCodeSampleType.Default));
                codeEntries.Add(new ViewCodeSample(")", ViewCodeSampleType.Brackets));
                codeEntries.Add(new ViewCodeSample(";\n", ViewCodeSampleType.Default));

                break;
        }


        codeEntries.Add(new ViewCodeSample("    }\n}\n", ViewCodeSampleType.Brackets));

        return codeEntries;
    }
}
