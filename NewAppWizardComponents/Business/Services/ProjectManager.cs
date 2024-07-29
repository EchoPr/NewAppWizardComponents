using System.Diagnostics;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage.Pickers;
using WinRT.Interop; 

namespace NewAppWizardComponents;
class ProjectManager
{
    private FileSavePicker _savePicker;
    private FileOpenPicker _loadPicker;
    private nint _windowHandler;

    private ScmParser _scmParser;

    private Dictionary<string, List<string>> _fileTypes = new Dictionary<string, List<string>>()
    {
        { "C#", new List<string> { ".cs" } },
        { "Python", new List<string> { ".py" } },
        { "VB.Net", new List<string> { ".vb" } },
        { "VBA", new List<string> { ".vba" } },
        { "MATLAB", new List<string> { ".m" } },
        { "S-expr", new List<string> { ".scm", ".lisp" } },
        { "XML", new List<string> { ".xml" } }
    };

    public ProjectManager(List<ApiEntry> apiMethods)
    {
        _savePicker = new FileSavePicker();
        _loadPicker = new FileOpenPicker();
        _windowHandler = WindowNative.GetWindowHandle(App.MainWindow);

        InitializeWithWindow.Initialize(_savePicker, _windowHandler);
        _savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;

        InitializeWithWindow.Initialize(_loadPicker, _windowHandler);
        _loadPicker.ViewMode = PickerViewMode.Thumbnail;
        _loadPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;

        _scmParser = new ScmParser(apiMethods);
    }

    public async void SaveCode(string content, string progLang)
    {
        _savePicker.FileTypeChoices.Add(progLang, _fileTypes[progLang]);

        StorageFile file = await _savePicker.PickSaveFileAsync();
        if (file != null) 
        {
            CachedFileManager.DeferUpdates(file);
            using (var stream = await file.OpenStreamForWriteAsync())
            {
                using (var tw = new StreamWriter(stream))
                {
                    tw.WriteLine(content);
                }
            }
        }

        _savePicker.FileTypeChoices.Clear();
    }

    public async Task<List<ApiEntry>> LoadCode()
    {
        _loadPicker.FileTypeFilter.Add(".scm");

        List<ApiEntry> readData = new List<ApiEntry>();

        var file = await _loadPicker.PickSingleFileAsync();

        if (file != null)
        {
            readData = _scmParser.load_scm(file.Path);
        }
        else
        {
            Debug.WriteLine("Operation cancelled.");
        }

        return readData;
    }
}
