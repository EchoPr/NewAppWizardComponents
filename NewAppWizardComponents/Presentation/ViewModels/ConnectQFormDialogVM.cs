using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NewAppWizardComponents;
public class ConnectQFormDialogVM
{
    private ObservableCollection<SessionInfo> _visualSessions;
    public ObservableCollection<SessionInfo> VisualSessions
    {
        get => _visualSessions;
        set
        {
            _visualSessions = value;
            OnPropertyChanged();
        }
    }

    private SessionInfo? _selectedItem = null;
    public SessionInfo SelectedItem
    {
        get => _selectedItem;
        set
        {
            if (_selectedItem != value)
            {
                _selectedItem = value;
                OnPropertyChanged();
            }
        }
    }

    private RSessionList _availableSessions;
    private QFormManager _qformManager;

    public MainPageVM _mainPageVM;

    public event PropertyChangedEventHandler PropertyChanged;

    public ConnectQFormDialogVM(MainPageVM mainPageVM)
    {
        _mainPageVM = mainPageVM;
        _qformManager = mainPageVM.qformManager;
        VisualSessions = new ObservableCollection<SessionInfo>();
        LoadAvailableSessions();
    }

    public void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    public Tuple<bool, string?> ConnectQFormSession(SessionInfo sessionInfo)
    {
        int sid = sessionInfo.Id;

        if (sid < 0)
        {
            try
            {
                _qformManager.StartQForm();
            }
            catch (Exception e)
            {
                return new Tuple<bool, string?>(false, e.Message);
            }
        }
        else
        {
            ASessionId sessionArgs = new ASessionId();
            sessionArgs.session_id = sid;
            try
            {
                _qformManager.AttachToSession(sessionArgs);
            }
            catch (Exception e)
            {
                return new Tuple<bool, string?>(false, e.Message);
            }
        }

        return new Tuple<bool, string?>(true, null);
    }

    private void LoadAvailableSessions() 
    {

        VisualSessions.Add(new SessionInfo("New QForm session", -1));
        _availableSessions = _qformManager.GetAvailableSessions();

        Debug.WriteLine(VisualSessions.Count);
        if (_availableSessions.sessions == null)
            return;

        _availableSessions.sessions.Sort((s1, s2) => s1.session_id.CompareTo(s2.session_id));
        _availableSessions.sessions.ForEach(s => { if (!s.is_api_connected) VisualSessions.Add(new SessionInfo(s.ToString(), s.session_id)); });

    }
}
