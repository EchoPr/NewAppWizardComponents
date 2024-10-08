using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewAppWizardComponents;
public class CodeGeneratorFactory
{
    private static readonly Dictionary<string, ICodeGenerator> Generators = new Dictionary<string, ICodeGenerator>
    {
        { "C#", new CSharpCodeGenerator() },
        { "Python", new PythonCodeGenerator() },
        { "VB.Net", new VBNETCodeGenerator() },
        { "VBA", new VBACodeGenerator() },
        { "MATLAB", new MATLABCodeGenerator() },
        { "S-expr", new SexprCodeGenerator() },
        { "XML", new XMLCodeGenerator() },
    };

    public static ICodeGenerator GetGenerator(string language)
    {
        if (Generators.TryGetValue(language, out var generator))
        {
            return generator;
        }
        throw new ArgumentException($"Unsupported language: {language}");
    }
}

