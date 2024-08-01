using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace NewAppWizardComponents;

	[AttributeUsage(AttributeTargets.Method)]
	public class ApiFunction : Attribute
	{
	}

	[AttributeUsage(AttributeTargets.Method)]
	public class ServiceFunction : Attribute
	{
	}

	[AttributeUsage(AttributeTargets.Method)]
	public class ServiceDenyInvoke : Attribute
	{
	}

	[AttributeUsage(AttributeTargets.Method)]
	public class ServiceQFormDir : Attribute
	{
	}

public enum QFormStatus
	{
    NotConnected,
    StartedByApi,
    Attached,
    Detached,
    ClosedByUser,
    ClosedByApi,
    ClosedByException,
    ConnectionLost,
    ConnectionError,
}

public class _QFormStatus
{
    public QFormStatus status { get; set; }
}

public class _QFormAttach
	{
    public string qform_pid { get; set; }
    public string out_pipe { get; set; }
    public string out_sem { get; set; }
    public string in_pipe { get; set; }
    public string in_sem { get; set; }
	}

public class CodeEntry
{
    static ulong uid_seq = 0;
    public CodeEntry(uint Id)
    {
        id = Id;
        uid_seq++;
        eid = uid_seq.ToString();
    }

    public uint id;
    public string eid;
    public string cmd;
    public string cmd_id;
    public int service_cmd = 0;
    public System.Reflection.MethodInfo code_func = null;
    public List<string> comment = new List<string>();
    public Object arg;
    public string arg_json;
    public Object res;
    public Ret status;
    public string html;
    public bool changed = false;
    public bool New = true;
    public Type property_value_type = null;
    public bool property_is_bool = false;
    public string property_name = null;
    public string property_units = null;
    public int property_db_type = -1;
}


////del no-private
public struct LStr : IComparable<LStr>, IEquatable<LStr>
{
    public string key;
    public string en;
    public string ru;
    public string str;
    public LStr(string K, string E, string R)
    {
        key = K;
        en = E;
        ru = R;
        str = null;
    }
    public int CompareTo(LStr other) { return str.CompareTo(other.str); }
    public bool Equals(LStr other) { return key == other.key; }
}

public class ApiEntry : IComparable<ApiEntry>
{
    private string _name;
    public string Name { get => this.ToString(); set => _name = value; }

    public int service_cmd;
    public LStr[] groups;
    public Type arg_type;
    public Type ret_type;
    public bool menu_only;
    public bool instruction = false;
    public string signature;
    public System.Reflection.MethodInfo code_func = null;

    public object arg_value;
    public object ret_value;

    public ApiEntry(int svc_cmd, string EntryName, Type a, Type r, bool mnu, LStr[] G) {
        service_cmd = svc_cmd; menu_only = mnu; _name = EntryName; arg_type = a; ret_type = r; groups = G;
        arg_value = arg_type == null ? null : Activator.CreateInstance(this.arg_type);
        ret_value = ret_type == null ? null : Activator.CreateInstance(this.ret_type);
    }
    public ApiEntry(int svc_cmd, string EntryName, LStr G) { 
        service_cmd = svc_cmd; menu_only = false; _name = EntryName; arg_type = null; ret_type = null; groups = new[] { G }; 
    }

    public override string ToString() { return _name + (menu_only ? "*" : ""); }
    public int CompareTo(ApiEntry other) { return Name.CompareTo(other.Name); }

    public ApiEntry Clone() => new ApiEntry(service_cmd, _name, arg_type, ret_type, menu_only, groups);
}
public class Ret
{
    public string invocationResultStatus { get; set; }
}

////end del
public
////del merged
partial
////end del
class QForm : IDisposable
{
    const int       MSG_EXIT            = -1;
    const int       MSG_DETACH          = -2;
    const int       MSG_START           = -8;
    const int       MSG_START_HIDDEN    = -9;
    const int       MSG_STATUS          = -3;
    const int       MSG_RECONNECT       = -4;
    const int       MSG_CONNECT         = -6;
    const int       MSG_ATTACH          = -10;

    const string    m_err_qform_path_not_set        = "Path to QForm is not set";
    const string    m_err_qform_is_already_started  = "QForm is already started";
    const string    m_attach_error                  = "Failed to attach QForm";

    Process         m_qfcon;
    bool            m_throw     = true;
    string          m_qform_path;
    string          m_last_error;
    string          m_last_error_func;
    string          m_instance_name = null;
    BinaryReader    m_R;
    BinaryWriter    m_W;
    bool            m_disposed = false;

    public QForm(string instane_name = null)
		{
        m_instance_name = instane_name;
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!m_disposed)
			{
				if (disposing)
				{
                qform_detach();
                qfcon_clear();
				}
				m_disposed = true;
			}
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

    public class CmdInfo
    {
        public string arg;
        public string res;
    }

    [ServiceFunction]
    [ServiceDenyInvoke]
    public void
        exceptions_enable(bool Enable = true)
    {
        m_throw = Enable;
    }

    [ServiceFunction]
    [ServiceDenyInvoke]
    public void
        exceptions_disable(bool Disable = true)
    {
        m_throw = !Disable;
    }

    [ServiceFunction]
    public bool
        exceptions_enabled()
    {
        return m_throw;
    }

    [ServiceFunction]
    public void
        last_error_clear()
    {
        m_last_error = null;
        m_last_error_func = null;
    }

    [ServiceFunction]
    public string
        last_error()
    {
        return m_last_error;
    }

    [ServiceFunction]
    public string
        last_error_at_function()
    {
        return m_last_error_func;
    }

    [ServiceFunction]
    public string
        instance_name()
    {
        return m_instance_name;
    }

    [ServiceFunction]
    public QFormStatus
        qform_status()
		{
        if (m_qfcon == null)
            return QFormStatus.NotConnected;

        if (m_qfcon.HasExited)
			{
            return (QFormStatus)m_qfcon.ExitCode;
        }

        try
        {
            object ret = invoke_private(MSG_STATUS, "qform_status", null, typeof(_QFormStatus));
            _QFormStatus st = ret as _QFormStatus;
            if (st != null)
                return st.status;
        }
        catch (Exception)
        {
        }

        return QFormStatus.ConnectionLost;
		}

    [ServiceFunction]
    public bool qform_is_running()
		{
        var st = qform_status();
        return st == QFormStatus.StartedByApi || st == QFormStatus.Attached;
		}

    [ServiceFunction]
    public bool qform_is_started_by_api()
    {
        return qform_status() == QFormStatus.StartedByApi;
    }

    [ServiceFunction]
    public bool qform_is_attached()
    {
        return qform_status() == QFormStatus.Attached;
    }

    [ServiceFunction]
    public bool qform_is_detached()
    {
        return qform_status() == QFormStatus.Detached;
    }

    [ServiceFunction]
    public bool qform_is_closed_by_user()
		{
        return qform_status() == QFormStatus.ClosedByUser;
		}

    string qfcon_args(bool attach, bool no_window, bool svc_mode)
		{
        string args = "-bscm";

        if (!string.IsNullOrEmpty(m_instance_name))
			{
            string inst = Uri.EscapeDataString(m_instance_name);
            args += " -inst " + inst;
			}

        if (attach)
            args += " -attach";

        if (svc_mode)
            args += " -svc";

        if (no_window)
            args += " -nowindow";

        using (Process curr = Process.GetCurrentProcess())
        {
            args += " -parent " + curr.Id;
        }

        return args;
		}

    [ServiceFunction]
    [ServiceQFormDir]
    public void qform_dir_set(string path_to_qform)
    {
        m_qform_path = path_to_qform;
    }

    [ServiceFunction]
    public string qform_dir()
		{
        if (is_started_by_qform())
            return Environment.GetEnvironmentVariable("CTL_MASTER_DIR");
        return m_qform_path;
    }

    [ServiceFunction]
		public bool qform_start_or_attach(bool no_window = false)
    {
        if (is_started_by_qform())
        {
            return qform_attach();
        }
        else
        {
            return qform_start(no_window);
        }
    }

    [ServiceFunction]
		public bool is_started_by_qform()
		{
        string pid = Environment.GetEnvironmentVariable("CTL_MASTER_PID");
        return !string.IsNullOrEmpty(pid);
		}

    [ServiceFunction]
		public bool qform_attach()
    {
        string pid = Environment.GetEnvironmentVariable("CTL_MASTER_PID");

        if (!string.IsNullOrEmpty(pid))
        {
            string op = Environment.GetEnvironmentVariable("CTL_SLAVE_OUT");
            string osem = Environment.GetEnvironmentVariable("CTL_SLAVE_OUT_SEM");
            string ip = Environment.GetEnvironmentVariable("CTL_SLAVE_IN");
            string isem = Environment.GetEnvironmentVariable("CTL_SLAVE_IN_SEM");
            string exedir = Environment.GetEnvironmentVariable("CTL_MASTER_DIR");
            try
				{
                attach(exedir, pid, op, osem, ip, isem);
                m_instance_name = null;
                return true;
				}
            catch (Exception ex)
				{
                return bool_error(ex.Message, "qform_attach");
				}
        }
        return bool_error("This function is only available when the application is started from the QForm Applications menu", "qform_attach");
    }

    [ServiceFunction]
    public void qform_close_or_detach()
    {
        var st = qform_status();
        switch (st)
			{
            case QFormStatus.Attached:
                qform_detach();
                break;

            case QFormStatus.StartedByApi:
                qform_close();
                break;
			}
    }

    bool io_init()
    {
        var istm = m_qfcon.StandardInput.BaseStream;
        var ostm = m_qfcon.StandardOutput.BaseStream;
        m_R = new BinaryReader(ostm);
        m_W = new BinaryWriter(istm);
        return true;
    }

    bool
        qfcon_try_start(string caller_func)
		{
        try
			{
            qfcon_start();
            return true;
			}
        catch (Exception ex)
			{
            error(ex.Message, caller_func);
            return false;
			}
		}

    void
        qfcon_clear()
		{
			close_io_handles();
		}

    Process
        qfcon_new()
		{
        qfcon_clear();
        return new Process();
		}

    void
        qfcon_start()
		{
        try
        {
            m_qfcon.Start();
            io_init();
        }
        catch (Exception)
        {
            m_qfcon = null;
            throw mk_err("Unable to start connection service (QFormCon.exe)");
        }
    }

    bool
        qfcon_is_running()
		{
        if (m_qfcon == null)
            return false;

        if (m_qfcon.HasExited)
            return false;

        return true;
		}

    void
        start_svc()
    {
        if (qfcon_is_running())
            return;

        string q_dir = qform_dir();
        if (string.IsNullOrEmpty(q_dir))
            throw new Exception(m_err_qform_path_not_set);

        m_qfcon = new Process();
        m_qfcon.StartInfo.UseShellExecute = false;
			m_qfcon.StartInfo.CreateNoWindow = true;
        m_qfcon.StartInfo.RedirectStandardOutput = true;
        m_qfcon.StartInfo.RedirectStandardInput = true;

        string exe = q_dir + @"\QFormCon.exe";

        m_qfcon.StartInfo.FileName = exe;

        m_qfcon.StartInfo.Arguments = qfcon_args(false, false, true);
        qfcon_start();
    }

    void attach(string qform_exe_dir, string qform_pid, string out_pipe, string out_sem, string in_pipe, string in_sem)
    {
        if (qfcon_is_running())
			{
            if (qform_is_running())
                throw mk_err(m_err_qform_is_already_started, "attach");

            _QFormAttach arg = new _QFormAttach();
            arg.qform_pid   = qform_pid;
            arg.out_pipe    = out_pipe;
            arg.out_sem     = out_sem;
            arg.in_pipe     = in_pipe;
            arg.in_sem      = in_sem;
            invoke_private(MSG_ATTACH, "attach", arg, null);
            return;
			}

			m_qfcon = qfcon_new();

        m_qfcon.StartInfo.UseShellExecute = false;
        m_qfcon.StartInfo.CreateNoWindow = true;
        m_qfcon.StartInfo.Arguments = qfcon_args(true, false, false);
        m_qfcon.StartInfo.RedirectStandardOutput = true;
        m_qfcon.StartInfo.RedirectStandardInput = true;
        m_qfcon.StartInfo.FileName = qform_exe_dir + @"\QFormCon.exe";
        m_qfcon.StartInfo.CreateNoWindow = true;
        qfcon_start();

        var st = qform_status();
        if (st != QFormStatus.Attached)
            throw mk_err(m_attach_error, "attach");
    }

    [ServiceFunction]
    public bool qform_start(bool no_window = false)
    {
        if (qfcon_is_running())
        {
            if (qform_is_running())
                return bool_error(m_err_qform_is_already_started, "qform_start");

            try
				{
                int svc = no_window ? MSG_START_HIDDEN : MSG_START;
                invoke_private(svc, "start", null, null);
                return true;
            }
            catch (Exception ex)
				{
                return bool_error(ex.Message, "qform_start");
				}
        }

        string qdir = qform_dir();
        if (string.IsNullOrEmpty(qdir))
            return bool_error(m_err_qform_path_not_set, "qform_start");

        m_qfcon = qfcon_new();
        m_qfcon.StartInfo.UseShellExecute = false;
        m_qfcon.StartInfo.CreateNoWindow = true;
        m_qfcon.StartInfo.RedirectStandardOutput = true;
        m_qfcon.StartInfo.RedirectStandardInput = true;
        m_qfcon.StartInfo.FileName = qdir + @"\QFormCon.exe";
        m_qfcon.StartInfo.Arguments = qfcon_args(false, no_window, false);

        return qfcon_try_start("qform_start");
    }

		void disconnect(int msg)
		{
			if (m_qfcon == null)
			{
				close_io_handles();
				return;
			}

			if (m_qfcon.HasExited)
			{
				close_io_handles();
				return;
			}

			if (m_W != null)
			{
				try
				{
					m_W.Write(msg);
					m_W.Flush();

					if (msg == MSG_EXIT)
					{
						if (m_qfcon != null)
						{
							m_qfcon.WaitForExit();
						}
					}
				}
				catch (Exception)
				{
				}
			}

        close_io_handles();
		}

		void
			close_io_handles()
		{
			if (m_R != null)
				m_R.Dispose();

			if (m_W != null)
				m_W.Dispose();

			m_R = null;
			m_W = null;

			if (m_qfcon != null)
			{
				if (!m_qfcon.HasExited)
				{
					try
					{
						m_qfcon.Close();
					}
					catch (Exception)
					{
					}
				}
				m_qfcon.Dispose();
				m_qfcon = null;
			}
		}

		void close_lost()
    {
        close_io_handles();
    }

    [ServiceFunction]
    public void qform_close()
    {
			disconnect(MSG_EXIT);
		}

    [ServiceFunction]
		public void qform_detach()
    {
        disconnect(MSG_DETACH);
		}

    Object
        error(string msg, string func = null)
    {
        m_last_error = msg;
        m_last_error_func = func;

        if (m_throw)
            throw new Exception(m_last_error);
        return null;
    }

    bool
        bool_error(string msg, string func = null)
		{
        m_last_error = msg;
        m_last_error_func = func;

        if (m_throw)
            throw new Exception(m_last_error);
        return false;
		}

    Exception
        mk_err(string msg, string func = null)
    {
        return new Exception(msg);
    }

    Object
        okay()
    {
        return this;
    }

    public Object invoke(int service_cmd, string cmd, Object arg, Type ret_type, CmdInfo nfo = null)
    {
        try
        {
            return invoke_private(service_cmd, cmd, arg, ret_type, nfo);
        }
        catch (Exception ex)
        {
            return error(ex.Message, cmd);
        }
    }

		Object invoke_private(int service_cmd, string cmd, Object arg, Type ret_type, CmdInfo nfo = null)
    {
        if (!qfcon_is_running())
        {
            if (service_cmd < 0)
                start_svc();
            else
                throw mk_err("QForm is not started", cmd);
        }

        BScm msg = new BScm();
        if (arg != null)
            msg.store(arg);

        if (service_cmd < 0) // defined
            msg.value_int_set(service_cmd);
        else
            msg.value_str_set(cmd);

        int msz = msg.byte_size();

////del no-private
#if DEBUG
        using (MemoryStream dbgS = new MemoryStream())
			{
				using (BinaryWriter dbgW = new BinaryWriter(dbgS))
				{
					msg.write(dbgW);
					var szw = dbgS.Position;
				}
			}
#endif
////end del
			try
        {
            if (service_cmd < 0)
            {
                if (arg != null)
                {
                    int hdr = -(1000 + msz);
                    m_W.Write(hdr);
                    msg.write(m_W);
                }
                else
                {
                    m_W.Write(service_cmd);
                }
            }
            else
            {
                m_W.Write(msz);
                msg.write(m_W);
            }
            m_W.Flush();
        }
        catch (Exception ex)
        {
				string emsg = ex.Message;
            throw mk_err("QForm connection error", cmd);
        }

        BScm response = new BScm();
			try
			{
				response.read(m_R);
			}
			catch (Exception ex)
			{
            close_lost();
				string emsg = ex.Message;
            throw mk_err("QForm connection lost", cmd);
			}

			if (nfo != null)
        {
            StringBuilder sb = new StringBuilder();
            if (service_cmd < 0)
                msg.value_str_set(cmd);
            nfo.arg = msg.print(sb);
            nfo.res = response.print(sb);
        }

        string sr = response.value_str_get();
        if (sr != "ok")
            throw mk_err(sr, cmd);

        if (ret_type == null)
            return okay();

        Object ret = Activator.CreateInstance(ret_type);
        response.load(ret);
        return ret;
    }
////paste api-calls
}

////paste bscm
////paste api-types
