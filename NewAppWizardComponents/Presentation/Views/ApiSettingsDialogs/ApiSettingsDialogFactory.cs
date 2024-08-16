using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewAppWizardComponents;
public class ApiSettingsDialogFactory
{
    public static ContentDialog GetDialog(string language, MainPageVM vm, ApiEntry entry = null)
    {
        switch (language)
        {
            case "Python":
                return new PythonApiSettingsDialog(vm, entry);
        }

        
        throw new ArgumentException($"Unsupported language: {language}");
    }
}
