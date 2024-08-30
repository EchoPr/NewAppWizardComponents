using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Documents;
using Windows.UI.ViewManagement;

namespace NewAppWizardComponents;
public class SexprCodeGenerator : ICodeGenerator
{
    public List<ViewCodeSample> GenerateApiSnippet(ApiEntry entry, string qformBaseDir)
    {
        throw new NotImplementedException();
    }

    public List<ViewCodeSample> GenerateCodeEntry(ApiEntry entry, int num, CodeGenerationMode mode = CodeGenerationMode.StepByStep)
    {
        var codeEntries = new List<ViewCodeSample>();

        if (entry.comment?.Count > 0)
        {
            foreach (var comment in entry.comment)
            {
                codeEntries.Add(new ViewCodeSample($"; {comment}\n", ViewCodeSampleType.Comment));
            }
        }

        if (entry.arg_type != null)
        {
            codeEntries.Add(new ViewCodeSample("(", ViewCodeSampleType.Brackets));
            codeEntries.Add(new ViewCodeSample($"{entry.Name}\n", ViewCodeSampleType.Method));
           

            PropertyInfo[] properties = entry.arg_type.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                var value = property.GetValue(entry.arg_value);

                if (property.PropertyType.IsGenericType &&
                    property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    var val = value as IList;
                    var elementType = property.PropertyType.GetGenericArguments()[0];

                    if (val.Count != 0)
                    {
                        codeEntries.Add(new ViewCodeSample("\t(", ViewCodeSampleType.Brackets));
                        codeEntries.Add(new ViewCodeSample($"{property.Name} ", ViewCodeSampleType.Default));

                        if (elementType == typeof(int) || elementType == typeof(double) ||
                        elementType == typeof(bool) || elementType == typeof(string))
                        {
                            foreach (var v in val)
                            {
                                if (elementType == typeof(string))
                                    codeEntries.Add(new ViewCodeSample($"\"{v}\" ", ViewCodeSampleType.Comment));
                                else
                                    codeEntries.Add(new ViewCodeSample($"{v} ", ViewCodeSampleType.Value));
                            }
                        }
                        else
                        {
                            for (int i = 0; i < val.Count; i++)
                            {
                                string objectName = val[i].GetType().Name[1..];
                                codeEntries.Add(new ViewCodeSample("(", ViewCodeSampleType.Brackets));
                                codeEntries.Add(new ViewCodeSample($"{objectName}", ViewCodeSampleType.Type));

                                PropertyInfo[] innerProperties = val[i].GetType().GetProperties();
                                foreach (PropertyInfo innerProperty in innerProperties)
                                {
                                    codeEntries.Add(new ViewCodeSample(" (", ViewCodeSampleType.Brackets));
                                    codeEntries.Add(new ViewCodeSample($"{innerProperty.Name} ", ViewCodeSampleType.Default));

                                    if (innerProperty.PropertyType == typeof(string))
                                        codeEntries.Add(new ViewCodeSample($"\"{innerProperty.GetValue(val[i])}\"", ViewCodeSampleType.Comment));
                                    else
                                        codeEntries.Add(new ViewCodeSample($"{innerProperty.GetValue(val[i])}", ViewCodeSampleType.Value));

                                    codeEntries.Add(new ViewCodeSample(")", ViewCodeSampleType.Brackets));
                                }
                                codeEntries.Add(new ViewCodeSample(")", ViewCodeSampleType.Brackets));
                            }
                        }

                        codeEntries.Add(new ViewCodeSample(")\n", ViewCodeSampleType.Brackets));
                    }     

                }
                else
                {
                    codeEntries.Add(new ViewCodeSample("\t(", ViewCodeSampleType.Brackets));
                    codeEntries.Add(new ViewCodeSample($"{property.Name} ", ViewCodeSampleType.Default));

                    if (property.PropertyType == typeof(string))
                        codeEntries.Add(new ViewCodeSample($"\"{value}\"", ViewCodeSampleType.Comment));
                    else
                        codeEntries.Add(new ViewCodeSample($"{value}", ViewCodeSampleType.Value));
                    codeEntries.Add(new ViewCodeSample(")\n", ViewCodeSampleType.Brackets));
                }
            }
            codeEntries.Add(new ViewCodeSample(")\n", ViewCodeSampleType.Brackets));
        }
        else
        {
            codeEntries.Add(new ViewCodeSample("(", ViewCodeSampleType.Brackets));
            codeEntries.Add(new ViewCodeSample($"{entry.Name}", ViewCodeSampleType.Method));
            codeEntries.Add(new ViewCodeSample(")\n", ViewCodeSampleType.Brackets));
        }
        return codeEntries;
    }
}
