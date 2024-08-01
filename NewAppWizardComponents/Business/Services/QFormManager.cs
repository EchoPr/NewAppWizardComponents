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

    public QFormManager(string qformDir = "C:\\QForm\\11.0.2\\x64")
    {
        _qformDir = qformDir;
        _qform.qform_dir_set(_qformDir);
    }

    public void attachQForm() 
    {
        if (_qform.is_started_by_qform())
        {
            _qform.qform_attach();
        }
    }

    public void invokeMethod(ApiEntry apiEntry)
    {
        if (apiEntry == null)
            return;

        if (apiEntry.code_func == null)
        {
            if (apiEntry.service_cmd == 0)
            {
                if (!_qform.qform_is_running())
                {
                    //ConnectForm frm = new ConnectForm(qform);
                    //frm.StartPosition = FormStartPosition.CenterParent;
                    //frm.ShowDialog(this);

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

        object arg = apiEntry.arg_value;
        Type? ret_type = null;
        ret_type = apiEntry.ret_type;

        var info = new QForm.CmdInfo();
        try
        {

            Ret rv = apiEntry.ret_value as Ret;
            if (rv == null)
                rv = new Ret();
            rv._status = "\u231B";
            apiEntry.ret_value = rv;

            object ret;
            if (apiEntry.code_func != null)
                ret = _CodeFunctionInvoke(apiEntry, arg, ret_type);
            else
                ret = _qform.invoke(apiEntry.service_cmd, apiEntry.Name, arg, ret_type, info);

            if (ret_type != null)
            {
                Ret r = ret as Ret;
                r._status = "Ok";
                apiEntry.ret_value = ret;
            }
            else
            {
                Ret st = new Ret();
                apiEntry.ret_value = new Ret { _status = "Ok" };
                apiEntry.ret_type = typeof(Ret);
            }
        }
        catch (Exception ex)
        {
            if (ret_type != null)
            {
                object ret = Activator.CreateInstance(ret_type);
                Ret r = ret as Ret;
                r._status = ex.Message;
                apiEntry.ret_value = ret;
            }
            else
            {
                Ret st = new Ret();
                apiEntry.ret_value = new Ret { _status = "Ok" };
                apiEntry.ret_type = typeof(Ret);
            }
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
