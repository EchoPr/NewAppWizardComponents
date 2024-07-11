using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewAppWizardComponents;

public
partial
class QForm
{
    public static string exe_directory()
    {
        Process p = Process.GetCurrentProcess();
        string name = p.MainModule.FileName;
        string[] ss = name.Split(new Char[] { '\\' });
        name = ss[0];
        for (int i = 1; i < ss.Length - 1; i++)
        {
            name += "\\" + ss[i];
        }
        return name;
    }

    public static string app_directory()
    {
        Process p = Process.GetCurrentProcess();
        string name = p.MainModule.FileName;
        string[] ss = name.Split(new Char[] { '\\' });
        name = ss[0];
        for (int i = 1; i < ss.Length - 2; i++)
        {
            name += "\\" + ss[i];
        }
        return name;
    }

}
