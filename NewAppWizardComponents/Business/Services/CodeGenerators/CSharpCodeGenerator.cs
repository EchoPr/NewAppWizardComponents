using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Documents;

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

        if (entry.arg_type != null)
        {
            codeEntries.Add(new ViewCodeSample($"{entry.arg_type.Name[1..]} ", ViewCodeSampleType.Type));
            codeEntries.Add(new ViewCodeSample($"arg{num} ", ViewCodeSampleType.Default));
            codeEntries.Add(new ViewCodeSample("= new", ViewCodeSampleType.Keyword));
            codeEntries.Add(new ViewCodeSample("() {\n", ViewCodeSampleType.Brackets));

            PropertyInfo[] properties = entry.arg_type.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType)) continue;

                var value = property.GetValue(entry.arg_value);

                codeEntries.Add(new ViewCodeSample($"\t{property.Name} ", ViewCodeSampleType.Default));
                codeEntries.Add(new ViewCodeSample($"= ", ViewCodeSampleType.Keyword));

                if (property.PropertyType.IsEnum)
                    codeEntries.Add(new ViewCodeSample($"{property.PropertyType.Name}.", ViewCodeSampleType.Value));

                codeEntries.Add(new ViewCodeSample($"{value}", ViewCodeSampleType.Value));
                codeEntries.Add(new ViewCodeSample($",\n", ViewCodeSampleType.Default));
            }

            codeEntries.Add(new ViewCodeSample("}", ViewCodeSampleType.Brackets));
            codeEntries.Add(new ViewCodeSample(";\n", ViewCodeSampleType.Default));
        }

        if (entry.ret_type != null)
        {
            codeEntries.Add(new ViewCodeSample($"{entry.ret_type.Name[1..]} ", ViewCodeSampleType.Type));
            codeEntries.Add(new ViewCodeSample($"ret{num} ", ViewCodeSampleType.Default));
            codeEntries.Add(new ViewCodeSample($"= ", ViewCodeSampleType.Keyword));

        }

        codeEntries.Add(new ViewCodeSample("qform.", ViewCodeSampleType.Default));
        codeEntries.Add(new ViewCodeSample($"{entry.Name}", ViewCodeSampleType.Method));
        codeEntries.Add(new ViewCodeSample("(", ViewCodeSampleType.Brackets));


        if (entry.arg_type != null)
        {
            codeEntries.Add(new ViewCodeSample($"arg{num}", ViewCodeSampleType.Default));
        }

        codeEntries.Add(new ViewCodeSample(")", ViewCodeSampleType.Brackets));
        codeEntries.Add(new ViewCodeSample(";", ViewCodeSampleType.Default));

        return codeEntries;
    }

    public List<ViewCodeSample> GenerateCodeEntryStepByStep(ApiEntry entry, int num)
    {
        var codeEntries = new List<ViewCodeSample>();

        if (entry.arg_type != null)
        {
            codeEntries.Add(new ViewCodeSample($"{entry.arg_type.Name[1..]} ", ViewCodeSampleType.Type));
            codeEntries.Add(new ViewCodeSample($"arg{num} ", ViewCodeSampleType.Default));
            codeEntries.Add(new ViewCodeSample("= new ", ViewCodeSampleType.Keyword));
            codeEntries.Add(new ViewCodeSample($"{entry.arg_type.Name[1..]}", ViewCodeSampleType.Type));
            codeEntries.Add(new ViewCodeSample("()", ViewCodeSampleType.Brackets));
            codeEntries.Add(new ViewCodeSample(";\n", ViewCodeSampleType.Default));

            PropertyInfo[] properties = entry.arg_type.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                var value = property.GetValue(entry.arg_value);

                if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                {
                    var val = value as IList;
                    var elementType = property.PropertyType.GetGenericArguments()[0];

                    if (elementType == typeof(int) || elementType == typeof(double) ||
                        elementType == typeof(bool) || elementType == typeof(string))
                    {
                        foreach (var v in val)
                        {
                            codeEntries.Add(new ViewCodeSample($"arg{num}.{property.Name}.", ViewCodeSampleType.Default));
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
                            codeEntries.Add(new ViewCodeSample($"arg{num}_object{i + 1}", ViewCodeSampleType.Default));
                            codeEntries.Add(new ViewCodeSample("= new ", ViewCodeSampleType.Keyword));
                            codeEntries.Add(new ViewCodeSample($"{objectName}", ViewCodeSampleType.Type));
                            codeEntries.Add(new ViewCodeSample("()", ViewCodeSampleType.Brackets));
                            codeEntries.Add(new ViewCodeSample(";\n", ViewCodeSampleType.Default));

                            PropertyInfo[] innerProperties = val[i].GetType().GetProperties();
                            foreach (PropertyInfo innerProperty in innerProperties)
                            {
                                codeEntries.Add(new ViewCodeSample($"arg{num}_object{i + 1}.{innerProperty.Name} ", ViewCodeSampleType.Default));
                                codeEntries.Add(new ViewCodeSample("= ", ViewCodeSampleType.Keyword));

                                if (innerProperty.PropertyType.IsEnum)
                                    codeEntries.Add(new ViewCodeSample($"{innerProperty.PropertyType.Name}.", ViewCodeSampleType.Value));

                                codeEntries.Add(new ViewCodeSample($"{innerProperty.GetValue(val[i])}", ViewCodeSampleType.Value));
                                codeEntries.Add(new ViewCodeSample(";\n", ViewCodeSampleType.Default));
                            }
                        }
                    }

                }
                else
                {
                    codeEntries.Add(new ViewCodeSample($"arg{num}.", ViewCodeSampleType.Default));
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
            codeEntries.Add(new ViewCodeSample($"ret{num} ", ViewCodeSampleType.Default));
            codeEntries.Add(new ViewCodeSample("= ", ViewCodeSampleType.Keyword));
        }

        codeEntries.Add(new ViewCodeSample("qform.", ViewCodeSampleType.Default));
        codeEntries.Add(new ViewCodeSample($"{entry.Name}", ViewCodeSampleType.Method));
        codeEntries.Add(new ViewCodeSample("(", ViewCodeSampleType.Brackets));

        if (entry.arg_type != null) { codeEntries.Add(new ViewCodeSample($"arg{num}", ViewCodeSampleType.Default)); }

        codeEntries.Add(new ViewCodeSample(")", ViewCodeSampleType.Brackets));
        codeEntries.Add(new ViewCodeSample(";\n", ViewCodeSampleType.Default)); 
        
        return codeEntries;
    }
}
