using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using Uno.Extensions;

namespace NewAppWizardComponents;
public partial class VBAApiSettingsDialogVM
{   
    
    public ObservableCollection<string> ComLibraries { get; private set; }

    private string registryKeyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";


    public VBAApiSettingsDialogVM()
    {
        ComLibraries = new ObservableCollection<string>();

        LoadComLibraries("QFormApiCom");
    }

    public void LoadComLibraries(string prefix)
    {
        

        LoadLibrariesFromRegistry(registryKeyPath, RegistryView.Registry64, prefix);
        LoadLibrariesFromRegistry(registryKeyPath, RegistryView.Registry32, prefix);
    }



    private void LoadLibrariesFromRegistry(string registryKeyPath, RegistryView registryView, string prefix)
    {

        using var baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, registryView);
        using var typeLibKey = baseKey.OpenSubKey(registryKeyPath);
        if (typeLibKey != null)
        {
            var subKeyNames = typeLibKey.GetSubKeyNames();
            foreach (var name in subKeyNames)
            {
                if (name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                {
                    ComLibraries.Add($"QForm API {name.Split(" ")[1]}");
                }
            }
        }

    }

    public void UninstallQFormApi(string ver)
    {
        string uninstallerPath = "";

        using var baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
        using var typeLibKey = baseKey.OpenSubKey(registryKeyPath);
        if (typeLibKey != null)
        {
            var subKeyNames = typeLibKey.GetSubKeyNames();
            foreach (var name in subKeyNames)
            {
                if (name == $"QFormApiCom {ver}")
                {
                    uninstallerPath = typeLibKey.OpenSubKey(name).GetValue("UninstallPath").ToString();
                    break;
                }
            }
        }

        var splitIndex = uninstallerPath.IndexOf(".exe", StringComparison.OrdinalIgnoreCase) + 4;
        var filePath = uninstallerPath.Substring(0, splitIndex).Trim();
        var arguments = uninstallerPath.Substring(splitIndex).Trim();

        var processInfo = new ProcessStartInfo
        {
            FileName = filePath,
            Arguments = arguments,
            UseShellExecute = false
        };

        
        var process = Process.Start(processInfo);

        process.WaitForExit();

        ComLibraries.Clear();
        LoadComLibraries("QFormApiCom");
    }
}
