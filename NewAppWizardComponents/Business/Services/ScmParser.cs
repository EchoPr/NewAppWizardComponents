using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Services.Maps;

namespace NewAppWizardComponents;
public class ScmParser
{
    private List<ApiEntry> _apiEntries = new List<ApiEntry>();

    public ScmParser(List<ApiEntry> apiEntries)
    {
        _apiEntries = apiEntries;
    }
    public List<ApiEntry> load_scm(string filePath)
    {
        List<ApiEntry> lst = new List<ApiEntry>();
        try
        {
            Scm scm = new Scm();
            scm.open_file(filePath);
            for (uint id = 1; ; id++)
            {
                ScmEntry fe = scm.get();
                if (fe == null)
                    break;

                string cmd = fe.bscm.value_str_get();

                if (string.IsNullOrEmpty(cmd))
                    break;

                ApiEntry ae = api_entry(cmd);

                CodeEntry ce = null;
                if (ae != null)
                {
                    ce = new CodeEntry(id);
                    init(ce, ae);
                    if (fe.comment != null)
                        ce.comment = fe.comment;
                }

                if (ce != null)
                {
                    if (fe.bscm.childs != null)
                    {
                        foreach (var p in fe.bscm.childs)
                        {
                            string s = p.value_str_get();
                            if (s == "cmd_id")
                            {
                                if (p.childs != null && p.childs.Count > 0)
                                    ce.cmd_id = p.childs[0].value_str_get();
                            }
                        }
                    }
                }

                if (ce != null && ce.arg != null)
                {
                    fe.bscm.load(ce.arg);
                }


                ae.arg_value = ce.arg;
                ae.ret_value = ce.res;

                if (ae != null)
                    lst.Add(ae);

            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to read file\nError : {ex.Message}");    
        }
        return lst;
    }

    private ApiEntry api_entry(string name)
    {
        foreach (ApiEntry e in _apiEntries)
        {
            if (e.Name == name)
                return e.Clone();
        }
        return null;
    }

    private void init(CodeEntry ce, ApiEntry entry)
    {
        ce.cmd = entry.Name;
        ce.service_cmd = entry.service_cmd;
        ce.code_func = entry.code_func;
        //            ce.arg = clone(propertyGridArgs.SelectedObject);
        AProperty pa = null;
        if (entry.arg_type != null)
        {
            ce.arg = Activator.CreateInstance(entry.arg_type);
            pa = ce.arg as AProperty;
        }

        if (entry.ret_type != null)
            ce.res = Activator.CreateInstance(entry.ret_type);
    }


}
