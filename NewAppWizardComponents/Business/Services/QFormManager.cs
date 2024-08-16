using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.VoiceCommands;

namespace NewAppWizardComponents;
public class QFormManager
{
    QForm _qform = new QForm();

    string? _qformDir = null;

    public event EventHandler<string> ErrorMessageRequested;
    public event EventHandler InvokationResultsReceived;
    public event EventHandler InvocationStarted;
    public event EventHandler InvocationEnded;

    private string qformBaseDir = "C:\\QForm\\11.0.2";
    public string QFormBaseDir { get => qformBaseDir; set => qformBaseDir = value; }

    public QFormManager()
    {
        SetBaseDir(qformBaseDir);
    }

    public void SetBaseDir(string dir)
    {
        qformBaseDir = dir;
        _qform.qform_dir_set(Path.Combine(qformBaseDir, "x64"));
    }

    public void attachQForm() 
    {
        if (_qform.is_started_by_qform())
        {
            _qform.qform_attach();
        }
    }

    public async Task invokeMethod(ApiEntry apiEntry)
    {
        if (apiEntry == null)
            return;

        if (apiEntry.code_func == null)
        {
            if (apiEntry.service_cmd == 0)
            {
                if (!_qform.qform_is_running())
                {
                    ErrorMessageRequested?.Invoke(this, "There will be able to connect QForm session");
                    return;
                }
            }
        }
        else
        {
            if (_HasAttribute(apiEntry.code_func, typeof(ServiceDenyInvoke)))
            {
                ErrorMessageRequested?.Invoke(this, "This function is not available in the Application Wizard");
                return;
            }
        }

        InvocationStarted?.Invoke(this, null);
        object arg = apiEntry.arg_value;
        Type ret_type = apiEntry.ret_type;

        var info = new QForm.CmdInfo();
        try
        {
            Ret rv = apiEntry.ret_value as Ret;
            if (rv == null)
                rv = new Ret();
            rv.invocationResultStatus = "\u231B";
            apiEntry.ret_value = rv;

            object ret;
            if (apiEntry.code_func != null)
                ret = await Task.Run(() => _CodeFunctionInvoke(apiEntry, arg, ret_type));
            else
                ret = await Task.Run(() => _qform.invoke(apiEntry.service_cmd, apiEntry.Name, arg, ret_type, info));

            if (ret.GetType() == typeof(Tuple<string>)) throw new Exception((ret as Tuple<string>).Item1);

            if (ret_type != null)
            {
                if (ret is Ret r)
                {
                    r.invocationResultStatus = "Ok";
                    apiEntry.ret_value = ret;
                }
                else
                {
                    apiEntry.ret_value = new Ret { invocationResultStatus = "Ok" };
                    apiEntry.ret_type = typeof(Ret);
                }
                InvocationEnded?.Invoke(this, null);
            }
            else
            {
                apiEntry.ret_value = new Ret { invocationResultStatus = "Ok" };
                apiEntry.ret_type = typeof(Ret);
                InvocationEnded?.Invoke(this, null);
            }
        }
        catch (Exception ex)
        {
            if (ret_type != null)
            {
                object ret = Activator.CreateInstance(ret_type);
                if (ret is Ret r)
                {
                    r.invocationResultStatus = ex.Message;
                    apiEntry.ret_value = ret;
                }
            }
            else
            {
                apiEntry.ret_value = new Ret { invocationResultStatus = ex.Message };
                apiEntry.ret_type = typeof(Ret);
            }
            InvocationEnded?.Invoke(this, null);
        }

        InvokationResultsReceived?.Invoke(this, null);
    }


    public RSessionList GetAvailableSessions()
    {
        return _qform.session_list();
    }

    public void StartQForm()
    {
        _qform.qform_start();
    }

    public void DetachQForm()
    {
        _qform.qform_detach();
    }

    public void AttachToSession(ASessionId sessionId) 
    {
        _qform.qform_attach_to(sessionId);
    }

    private bool _HasAttribute(MethodInfo m, Type t)
    {
        foreach (var a in m.CustomAttributes)
        {
            if (a.AttributeType == t)
                return true;
        }
        return false;
    }

    private object _CodeFunctionInvoke(ApiEntry e, object arg, Type ret_type)
    {
        object[] pars = null;
        if (arg != null)
        {
            CodeArg ca = arg as CodeArg;
            pars = new object[] { ca.value_get() };
        }

        object ret = null;
        try
        {
            ret = e.code_func.Invoke(_qform, pars);
        }
        catch (TargetInvocationException ex)
        {
            throw ex.InnerException;
        }

        if (ret_type != null)
        {
            object robj = Activator.CreateInstance(ret_type);
            CodeRet cr = robj as CodeRet;
            cr.value_set(ret);

            return cr;
        }
        return null;
    }
}
