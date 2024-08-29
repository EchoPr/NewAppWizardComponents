using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewAppWizardComponents;
public class ApiSettingsDialogFactory
{
    public static ContentDialog GetDialog(string language, MainPageVM vm, ApiEntry? entry, Dictionary<string, ApiEntry> _languageSnippets)
    {
        switch (language)
        {
            case "Python":
                return new PythonApiSettingsDialog(vm, entry, _languageSnippets);
            case "C#":
                return new CSharpApiSettingsDialog(vm, entry, _languageSnippets);
            case "VBA":
                return new VBAApiSettingsDialog(vm, entry);
        }

        
        throw new ArgumentException($"Unsupported language: {language}");
    }
}

public enum PythonScriptType
{
    pyfile,
    notebook
}

public enum APIQFormInteractionType
{
    script_starts,
    qform_starts,
    qform_starts_or_connect,
    script_connect
}

public enum APIQFormReference
{
    default_folder,
    local_folder
}
