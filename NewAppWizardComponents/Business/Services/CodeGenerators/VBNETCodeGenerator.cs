using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Documents;

namespace NewAppWizardComponents;
public class VBNETCodeGenerator : ICodeGenerator
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
                codeEntries.Add(new ViewCodeSample($"' {comment}\n", ViewCodeSampleType.Comment));
            }
        }

        if (entry.arg_type != null)
        {
            codeEntries.Add(new ViewCodeSample($"Dim ", ViewCodeSampleType.Keyword));
            codeEntries.Add(new ViewCodeSample($"arg{num + 1} ", ViewCodeSampleType.Default));
            codeEntries.Add(new ViewCodeSample($"As New ", ViewCodeSampleType.Keyword));
            codeEntries.Add(new ViewCodeSample($"QFormAPI.{entry.arg_type.Name[1..]}\n", ViewCodeSampleType.Type));
            codeEntries.Add(new ViewCodeSample($"With ", ViewCodeSampleType.Keyword));
            codeEntries.Add(new ViewCodeSample($"arg{num + 1}\n", ViewCodeSampleType.Default));

            PropertyInfo[] properties = entry.arg_type.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(List<>)) continue;

                var value = property.GetValue(entry.arg_value);

                codeEntries.Add(new ViewCodeSample($"\t.{property.Name} ", ViewCodeSampleType.Default));
                codeEntries.Add(new ViewCodeSample($"= ", ViewCodeSampleType.Keyword));

                if (property.PropertyType.IsEnum)
                    codeEntries.Add(new ViewCodeSample($"QFormAPI.{property.PropertyType.Name}.", ViewCodeSampleType.Value));

                if (property.PropertyType == typeof(string))
                    codeEntries.Add(new ViewCodeSample($"\"{value}\"\n", ViewCodeSampleType.Comment));
                else
                    codeEntries.Add(new ViewCodeSample($"{value}\n", ViewCodeSampleType.Value));
            }

            codeEntries.Add(new ViewCodeSample($"End With\n", ViewCodeSampleType.Keyword));
        }

        if (entry.ret_type != null)
        {
            codeEntries.Add(new ViewCodeSample($"Dim ", ViewCodeSampleType.Keyword));
            codeEntries.Add(new ViewCodeSample($"ret{num + 1} ", ViewCodeSampleType.Default));
            codeEntries.Add(new ViewCodeSample($"As ", ViewCodeSampleType.Keyword));
            codeEntries.Add(new ViewCodeSample($"QFormAPI.{entry.ret_type.Name[1..]} ", ViewCodeSampleType.Type));
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

        return codeEntries;
    }

    public List<ViewCodeSample> GenerateCodeEntryStepByStep(ApiEntry entry, int num)
    {
        var codeEntries = new List<ViewCodeSample>();

        if (entry.comment?.Count > 0)
        {
            foreach (var comment in entry.comment)
            {
                codeEntries.Add(new ViewCodeSample($"' {comment}\n", ViewCodeSampleType.Comment));
            }
        }

        if (entry.arg_type != null)
        {
            codeEntries.Add(new ViewCodeSample($"Dim ", ViewCodeSampleType.Keyword));
            codeEntries.Add(new ViewCodeSample($"arg{num + 1} ", ViewCodeSampleType.Default));
            codeEntries.Add(new ViewCodeSample($"As New ", ViewCodeSampleType.Keyword));
            codeEntries.Add(new ViewCodeSample($"QFormAPI.{entry.arg_type.Name[1..]}\n", ViewCodeSampleType.Type));

            PropertyInfo[] properties = entry.arg_type.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                var value = property.GetValue(entry.arg_value);

                if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    var val = value as IList;
                    var elementType = property.PropertyType.GetGenericArguments()[0];

                    if (elementType == typeof(int) || elementType == typeof(double) ||
                        elementType == typeof(bool) || elementType == typeof(string))
                    {
                        foreach (var v in val)
                        {
                            codeEntries.Add(new ViewCodeSample($"arg{num + 1}.{property.Name}.", ViewCodeSampleType.Default));
                            codeEntries.Add(new ViewCodeSample("Add", ViewCodeSampleType.Method));
                            codeEntries.Add(new ViewCodeSample("(", ViewCodeSampleType.Brackets));
                            if (property.PropertyType == typeof(string))
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

                            codeEntries.Add(new ViewCodeSample("Dim ", ViewCodeSampleType.Keyword));
                            codeEntries.Add(new ViewCodeSample($"arg{num + 1}_object{i + 1} ", ViewCodeSampleType.Default));
                            codeEntries.Add(new ViewCodeSample($"As New ", ViewCodeSampleType.Keyword));
                            codeEntries.Add(new ViewCodeSample($"QFormAPI.{objectName}\n", ViewCodeSampleType.Type));

                            PropertyInfo[] innerProperties = val[i].GetType().GetProperties();
                            foreach (PropertyInfo innerProperty in innerProperties)
                            {
                                codeEntries.Add(new ViewCodeSample($"arg{num + 1}_object{i + 1}.{innerProperty.Name} ", ViewCodeSampleType.Default));
                                codeEntries.Add(new ViewCodeSample("= ", ViewCodeSampleType.Keyword));

                                if (innerProperty.PropertyType.IsEnum)
                                    codeEntries.Add(new ViewCodeSample($"QFormAPI.{innerProperty.PropertyType.Name}.", ViewCodeSampleType.Value));

                                codeEntries.Add(new ViewCodeSample($"{innerProperty.GetValue(val[i])}\n", ViewCodeSampleType.Value));
                            }

                            codeEntries.Add(new ViewCodeSample($"arg{num + 1}.{property.Name}.", ViewCodeSampleType.Default));
                            codeEntries.Add(new ViewCodeSample("Add", ViewCodeSampleType.Method));
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

                    if (property.PropertyType.IsEnum)
                        codeEntries.Add(new ViewCodeSample($"QFormAPI.{property.PropertyType.Name}.", ViewCodeSampleType.Value));

                    codeEntries.Add(new ViewCodeSample("= ", ViewCodeSampleType.Keyword));

                    if (property.PropertyType == typeof(string))
                        codeEntries.Add(new ViewCodeSample($"\"{value}\"\n", ViewCodeSampleType.Comment));
                    else
                        codeEntries.Add(new ViewCodeSample($"{value}\n", ViewCodeSampleType.Value));
                }
            }
        }

        if (entry.ret_type != null)
        {
            codeEntries.Add(new ViewCodeSample($"Dim ", ViewCodeSampleType.Keyword));
            codeEntries.Add(new ViewCodeSample($"ret{num + 1} ", ViewCodeSampleType.Default));
            codeEntries.Add(new ViewCodeSample($"As ", ViewCodeSampleType.Keyword));
            codeEntries.Add(new ViewCodeSample($"QFormAPI.{entry.ret_type.Name[1..]} ", ViewCodeSampleType.Type));
            codeEntries.Add(new ViewCodeSample($"= ", ViewCodeSampleType.Keyword));

        }

        codeEntries.Add(new ViewCodeSample("qform.", ViewCodeSampleType.Default));
        codeEntries.Add(new ViewCodeSample($"{entry.Name}", ViewCodeSampleType.Method));
        codeEntries.Add(new ViewCodeSample("(", ViewCodeSampleType.Brackets));


        if (entry.arg_type != null)
        {
            codeEntries.Add(new ViewCodeSample($"arg{num + 1}", ViewCodeSampleType.Default));
        }

        codeEntries.Add(new ViewCodeSample(")\n", ViewCodeSampleType.Brackets));

        return codeEntries;
    }

    public List<ViewCodeSample> GenerateApiSnippet(ApiEntry entry, string qformBaseDir)
    {
        var snippetConfig = entry.arg_value as AVBNetSettings;

        var codeEntries = new List<ViewCodeSample>();

        codeEntries.Add(new ViewCodeSample("Imports ", ViewCodeSampleType.Keyword));
        codeEntries.Add(new ViewCodeSample("QFormAPI\n\n", ViewCodeSampleType.Default));

        if (snippetConfig.use_static) codeEntries.Add(new ViewCodeSample("Shared ", ViewCodeSampleType.Keyword));
        codeEntries.Add(new ViewCodeSample("Class ", ViewCodeSampleType.Keyword));
        codeEntries.Add(new ViewCodeSample($"{snippetConfig.class_name} ", ViewCodeSampleType.Default));
        codeEntries.Add(new ViewCodeSample("\n", ViewCodeSampleType.Brackets));

        if (snippetConfig.use_static) codeEntries.Add(new ViewCodeSample("\tShared ", ViewCodeSampleType.Keyword));
        codeEntries.Add(new ViewCodeSample("\tReadOnly ", ViewCodeSampleType.Keyword));
        codeEntries.Add(new ViewCodeSample("qform_base_dir ", ViewCodeSampleType.Default));
        codeEntries.Add(new ViewCodeSample("As ", ViewCodeSampleType.Keyword));
        codeEntries.Add(new ViewCodeSample("String ", ViewCodeSampleType.Type));
        codeEntries.Add(new ViewCodeSample("= ", ViewCodeSampleType.Keyword));
        codeEntries.Add(new ViewCodeSample($"\"{qformBaseDir}\"", ViewCodeSampleType.Comment));
        codeEntries.Add(new ViewCodeSample("\n", ViewCodeSampleType.Default));

        if (snippetConfig.use_static) codeEntries.Add(new ViewCodeSample("\tShared ", ViewCodeSampleType.Keyword));
        codeEntries.Add(new ViewCodeSample("\tDim ", ViewCodeSampleType.Keyword));
        codeEntries.Add(new ViewCodeSample("qform", ViewCodeSampleType.Default));
        codeEntries.Add(new ViewCodeSample(" As ", ViewCodeSampleType.Default));
        codeEntries.Add(new ViewCodeSample("New ", ViewCodeSampleType.Keyword));
        codeEntries.Add(new ViewCodeSample("QForm", ViewCodeSampleType.Type));
        codeEntries.Add(new ViewCodeSample("()\n\n", ViewCodeSampleType.Brackets));

        codeEntries.Add(new ViewCodeSample("\tPublic ", ViewCodeSampleType.Keyword));
        if (snippetConfig.use_static) codeEntries.Add(new ViewCodeSample("Shared ", ViewCodeSampleType.Keyword));
        codeEntries.Add(new ViewCodeSample("Sub Execute()\n", ViewCodeSampleType.Keyword));

        switch (snippetConfig.connection_type)
        {
            case nameof(APIQFormInteractionType.script_starts):
                codeEntries.Add(new ViewCodeSample("\t\tqform.qform_dir_set(", ViewCodeSampleType.Default));
                codeEntries.Add(new ViewCodeSample("qform_base_dir", ViewCodeSampleType.Default));
                codeEntries.Add(new ViewCodeSample(" + ", ViewCodeSampleType.Default));
                codeEntries.Add(new ViewCodeSample("\"\\x64\"", ViewCodeSampleType.Comment));
                codeEntries.Add(new ViewCodeSample(")\n", ViewCodeSampleType.Default));

                if (snippetConfig.use_qform_exceptions)
                {
                    codeEntries.Add(new ViewCodeSample("\t\tIf Not ", ViewCodeSampleType.Keyword));
                    codeEntries.Add(new ViewCodeSample("qform.qform_start", ViewCodeSampleType.Method));
                    codeEntries.Add(new ViewCodeSample("()", ViewCodeSampleType.Brackets));
                    codeEntries.Add(new ViewCodeSample(" Then\n", ViewCodeSampleType.Keyword));
                    codeEntries.Add(new ViewCodeSample("\t\t\tConsole.WriteLine(", ViewCodeSampleType.Default));
                    codeEntries.Add(new ViewCodeSample("qform.last_exception()", ViewCodeSampleType.Method));
                    codeEntries.Add(new ViewCodeSample(")\n", ViewCodeSampleType.Default));
                    codeEntries.Add(new ViewCodeSample("\t\tEnd If\n", ViewCodeSampleType.Keyword));
                }
                else
                {
                    codeEntries.Add(new ViewCodeSample("\t\tTry\n", ViewCodeSampleType.Keyword));
                    codeEntries.Add(new ViewCodeSample("\t\t\tqform.qform_start", ViewCodeSampleType.Method));
                    codeEntries.Add(new ViewCodeSample("()\n", ViewCodeSampleType.Brackets));
                    codeEntries.Add(new ViewCodeSample("\t\tCatch ex As ", ViewCodeSampleType.Keyword));
                    codeEntries.Add(new ViewCodeSample("Exception\n", ViewCodeSampleType.Default));
                    codeEntries.Add(new ViewCodeSample("\t\t\tConsole.WriteLine(", ViewCodeSampleType.Default));
                    codeEntries.Add(new ViewCodeSample("ex.Message", ViewCodeSampleType.Method));
                    codeEntries.Add(new ViewCodeSample(")\n", ViewCodeSampleType.Default));
                    codeEntries.Add(new ViewCodeSample("\t\tEnd Try\n", ViewCodeSampleType.Keyword));
                }

                break;

            case nameof(APIQFormInteractionType.qform_starts):
                if (snippetConfig.alt_connection)
                {
                    codeEntries.Add(new ViewCodeSample("\t\tIf ", ViewCodeSampleType.Keyword));
                    codeEntries.Add(new ViewCodeSample("qform.is_started_by_qform", ViewCodeSampleType.Method));
                    codeEntries.Add(new ViewCodeSample("() ", ViewCodeSampleType.Brackets));
                    codeEntries.Add(new ViewCodeSample("Then\n", ViewCodeSampleType.Keyword));

                    if (snippetConfig.use_qform_exceptions)
                    {
                        codeEntries.Add(new ViewCodeSample("\t\t\tIf Not ", ViewCodeSampleType.Keyword));
                        codeEntries.Add(new ViewCodeSample("qform.qform_attach", ViewCodeSampleType.Method));
                        codeEntries.Add(new ViewCodeSample("() ", ViewCodeSampleType.Brackets));
                        codeEntries.Add(new ViewCodeSample("Then\n", ViewCodeSampleType.Keyword));
                        codeEntries.Add(new ViewCodeSample("\t\t\t\tConsole.WriteLine(", ViewCodeSampleType.Default));
                        codeEntries.Add(new ViewCodeSample("qform.last_exception()", ViewCodeSampleType.Method));
                        codeEntries.Add(new ViewCodeSample(")\n", ViewCodeSampleType.Default));
                        codeEntries.Add(new ViewCodeSample("\t\t\tEnd If\n", ViewCodeSampleType.Keyword));
                    }
                    else
                    {
                        codeEntries.Add(new ViewCodeSample("\t\t\tTry\n", ViewCodeSampleType.Keyword));
                        codeEntries.Add(new ViewCodeSample("\t\t\t\tqform.qform_attach", ViewCodeSampleType.Method));
                        codeEntries.Add(new ViewCodeSample("()\n", ViewCodeSampleType.Brackets));
                        codeEntries.Add(new ViewCodeSample("\t\t\tCatch ex As ", ViewCodeSampleType.Keyword));
                        codeEntries.Add(new ViewCodeSample("Exception\n", ViewCodeSampleType.Default));
                        codeEntries.Add(new ViewCodeSample("\t\t\t\tConsole.WriteLine(", ViewCodeSampleType.Default));
                        codeEntries.Add(new ViewCodeSample("ex.Message", ViewCodeSampleType.Method));
                        codeEntries.Add(new ViewCodeSample(")\n", ViewCodeSampleType.Default));
                        codeEntries.Add(new ViewCodeSample("\t\t\tEnd Try\n", ViewCodeSampleType.Keyword));
                    }

                    codeEntries.Add(new ViewCodeSample("\t\tElse\n", ViewCodeSampleType.Keyword));
                    codeEntries.Add(new ViewCodeSample("\t\t\tqform.qform_dir_set(", ViewCodeSampleType.Default));
                    codeEntries.Add(new ViewCodeSample("qform_base_dir", ViewCodeSampleType.Default));
                    codeEntries.Add(new ViewCodeSample(" + ", ViewCodeSampleType.Default));
                    codeEntries.Add(new ViewCodeSample("\"\\x64\"", ViewCodeSampleType.Comment));
                    codeEntries.Add(new ViewCodeSample(")\n", ViewCodeSampleType.Default));

                    codeEntries.Add(new ViewCodeSample("\t\t\tDim ", ViewCodeSampleType.Keyword));
                    codeEntries.Add(new ViewCodeSample("qform_session_id", ViewCodeSampleType.Default));
                    codeEntries.Add(new ViewCodeSample(" As New ", ViewCodeSampleType.Keyword));
                    codeEntries.Add(new ViewCodeSample("SessionId()\n", ViewCodeSampleType.Default));
                    codeEntries.Add(new ViewCodeSample("\t\t\tqform_session_id.session_id", ViewCodeSampleType.Default));
                    codeEntries.Add(new ViewCodeSample(" = 0\n", ViewCodeSampleType.Default));

                    if (snippetConfig.use_qform_exceptions)
                    {
                        codeEntries.Add(new ViewCodeSample("\t\t\tIf Not ", ViewCodeSampleType.Keyword));
                        codeEntries.Add(new ViewCodeSample("qform.qform_attach_to", ViewCodeSampleType.Method));
                        codeEntries.Add(new ViewCodeSample("(", ViewCodeSampleType.Brackets));
                        codeEntries.Add(new ViewCodeSample("qform_session_id", ViewCodeSampleType.Default));
                        codeEntries.Add(new ViewCodeSample(") ", ViewCodeSampleType.Brackets));
                        codeEntries.Add(new ViewCodeSample("Then\n", ViewCodeSampleType.Keyword));
                        codeEntries.Add(new ViewCodeSample("\t\t\t\tConsole.WriteLine(", ViewCodeSampleType.Default));
                        codeEntries.Add(new ViewCodeSample("qform.last_exception()", ViewCodeSampleType.Method));
                        codeEntries.Add(new ViewCodeSample(")\n", ViewCodeSampleType.Default));
                        codeEntries.Add(new ViewCodeSample("\t\t\tEnd If\n", ViewCodeSampleType.Keyword));
                    }
                    else
                    {
                        codeEntries.Add(new ViewCodeSample("\t\t\tTry\n", ViewCodeSampleType.Keyword));
                        codeEntries.Add(new ViewCodeSample("\t\t\t\tqform.qform_attach_to(", ViewCodeSampleType.Method));
                        codeEntries.Add(new ViewCodeSample("qform_session_id", ViewCodeSampleType.Default));
                        codeEntries.Add(new ViewCodeSample(")\n", ViewCodeSampleType.Brackets));
                        codeEntries.Add(new ViewCodeSample("\t\t\tCatch ", ViewCodeSampleType.Keyword));
                        codeEntries.Add(new ViewCodeSample("ex ", ViewCodeSampleType.Default));
                        codeEntries.Add(new ViewCodeSample("As ", ViewCodeSampleType.Keyword));
                        codeEntries.Add(new ViewCodeSample("Exception\n", ViewCodeSampleType.Type));
                        codeEntries.Add(new ViewCodeSample("\t\t\t\tConsole.WriteLine(", ViewCodeSampleType.Default));
                        codeEntries.Add(new ViewCodeSample("ex.Message", ViewCodeSampleType.Method));
                        codeEntries.Add(new ViewCodeSample(")\n", ViewCodeSampleType.Default));
                        codeEntries.Add(new ViewCodeSample("\t\t\tEnd Try\n", ViewCodeSampleType.Keyword));
                    }

                    codeEntries.Add(new ViewCodeSample("\t\tEnd If\n", ViewCodeSampleType.Keyword));

                }
                else
                {
                    if (snippetConfig.use_qform_exceptions)
                    {
                        codeEntries.Add(new ViewCodeSample("\t\tIf Not ", ViewCodeSampleType.Keyword));
                        codeEntries.Add(new ViewCodeSample("qform.qform_attach", ViewCodeSampleType.Method));
                        codeEntries.Add(new ViewCodeSample("() ", ViewCodeSampleType.Brackets));
                        codeEntries.Add(new ViewCodeSample("Then\n", ViewCodeSampleType.Keyword));
                        codeEntries.Add(new ViewCodeSample("\t\t\tConsole.WriteLine(", ViewCodeSampleType.Default));
                        codeEntries.Add(new ViewCodeSample("qform.last_exception()", ViewCodeSampleType.Method));
                        codeEntries.Add(new ViewCodeSample(")\n", ViewCodeSampleType.Default));
                        codeEntries.Add(new ViewCodeSample("\t\tEnd If\n", ViewCodeSampleType.Keyword));
                    }
                    else
                    {
                        codeEntries.Add(new ViewCodeSample("\t\tTry\n", ViewCodeSampleType.Keyword));
                        codeEntries.Add(new ViewCodeSample("\t\t\tqform.qform_attach", ViewCodeSampleType.Method));
                        codeEntries.Add(new ViewCodeSample("()\n", ViewCodeSampleType.Brackets));
                        codeEntries.Add(new ViewCodeSample("\t\tCatch ", ViewCodeSampleType.Keyword));
                        codeEntries.Add(new ViewCodeSample("ex ", ViewCodeSampleType.Default));
                        codeEntries.Add(new ViewCodeSample("As ", ViewCodeSampleType.Keyword));
                        codeEntries.Add(new ViewCodeSample("Exception\n", ViewCodeSampleType.Type));
                        codeEntries.Add(new ViewCodeSample("\t\t\tConsole.WriteLine(", ViewCodeSampleType.Default));
                        codeEntries.Add(new ViewCodeSample("ex.Message", ViewCodeSampleType.Method));
                        codeEntries.Add(new ViewCodeSample(")\n", ViewCodeSampleType.Default));
                        codeEntries.Add(new ViewCodeSample("\t\tEnd Try\n", ViewCodeSampleType.Keyword));
                    }
                }

                break;

            case nameof(APIQFormInteractionType.script_connect):
                codeEntries.Add(new ViewCodeSample("\t\tqform.qform_dir_set(", ViewCodeSampleType.Default));
                codeEntries.Add(new ViewCodeSample("qform_base_dir", ViewCodeSampleType.Default));
                codeEntries.Add(new ViewCodeSample(" + ", ViewCodeSampleType.Default));
                codeEntries.Add(new ViewCodeSample("\"\\x64\"", ViewCodeSampleType.Comment));
                codeEntries.Add(new ViewCodeSample(")\n", ViewCodeSampleType.Default));

                codeEntries.Add(new ViewCodeSample("\t\tDim ", ViewCodeSampleType.Keyword));
                codeEntries.Add(new ViewCodeSample("qform_session_id", ViewCodeSampleType.Default));
                codeEntries.Add(new ViewCodeSample(" As New ", ViewCodeSampleType.Keyword));
                codeEntries.Add(new ViewCodeSample("SessionId()\n", ViewCodeSampleType.Default));
                codeEntries.Add(new ViewCodeSample("\t\tqform_session_id.session_id", ViewCodeSampleType.Default));
                codeEntries.Add(new ViewCodeSample(" = 0\n", ViewCodeSampleType.Default));

                if (snippetConfig.use_qform_exceptions)
                {
                    codeEntries.Add(new ViewCodeSample("\t\tIf Not ", ViewCodeSampleType.Keyword));
                    codeEntries.Add(new ViewCodeSample("qform.qform_attach_to(", ViewCodeSampleType.Method));
                    codeEntries.Add(new ViewCodeSample("qform_session_id", ViewCodeSampleType.Default));
                    codeEntries.Add(new ViewCodeSample(") ", ViewCodeSampleType.Brackets));
                    codeEntries.Add(new ViewCodeSample("Then\n", ViewCodeSampleType.Keyword));
                    codeEntries.Add(new ViewCodeSample("\t\t\tConsole.WriteLine(", ViewCodeSampleType.Default));
                    codeEntries.Add(new ViewCodeSample("qform.last_exception()", ViewCodeSampleType.Method));
                    codeEntries.Add(new ViewCodeSample(")\n", ViewCodeSampleType.Default));
                    codeEntries.Add(new ViewCodeSample("\t\tEnd If\n", ViewCodeSampleType.Keyword));
                }
                else
                {
                    codeEntries.Add(new ViewCodeSample("\t\tTry\n", ViewCodeSampleType.Keyword));
                    codeEntries.Add(new ViewCodeSample("\t\t\tqform.qform_attach_to(", ViewCodeSampleType.Method));
                    codeEntries.Add(new ViewCodeSample("qform_session_id", ViewCodeSampleType.Default));
                    codeEntries.Add(new ViewCodeSample(")\n", ViewCodeSampleType.Brackets));
                    codeEntries.Add(new ViewCodeSample("\t\tCatch ", ViewCodeSampleType.Keyword));
                    codeEntries.Add(new ViewCodeSample("ex ", ViewCodeSampleType.Default));
                    codeEntries.Add(new ViewCodeSample("As ", ViewCodeSampleType.Keyword));
                    codeEntries.Add(new ViewCodeSample("Exception\n", ViewCodeSampleType.Type));
                    codeEntries.Add(new ViewCodeSample("\t\t\tConsole.WriteLine(", ViewCodeSampleType.Default));
                    codeEntries.Add(new ViewCodeSample("ex.Message", ViewCodeSampleType.Method));
                    codeEntries.Add(new ViewCodeSample(")\n", ViewCodeSampleType.Default));
                    codeEntries.Add(new ViewCodeSample("\t\tEnd Try\n", ViewCodeSampleType.Keyword));
                }

                break;
        }

        codeEntries.Add(new ViewCodeSample("\tEnd Sub\n", ViewCodeSampleType.Keyword));
        codeEntries.Add(new ViewCodeSample("End Class", ViewCodeSampleType.Keyword));

        return codeEntries;
    }

}
