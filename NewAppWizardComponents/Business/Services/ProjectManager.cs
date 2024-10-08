using System.Diagnostics;
using System.IO.Compression;
using System.Text;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage.Pickers;
using Windows.Web.Syndication;
using WinRT.Interop; 

namespace NewAppWizardComponents;
public class ProjectManager
{
    private FileSavePicker _savePicker;
    private FileOpenPicker _loadPicker;
    private FolderPicker _folderPicker;
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
        _folderPicker = new FolderPicker(); 
        _windowHandler = WindowNative.GetWindowHandle(App.MainWindow);

        InitializeWithWindow.Initialize(_savePicker, _windowHandler);
        _savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;

        InitializeWithWindow.Initialize(_loadPicker, _windowHandler);
        _loadPicker.ViewMode = PickerViewMode.Thumbnail;
        _loadPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;

        InitializeWithWindow.Initialize(_folderPicker, _windowHandler);
        _folderPicker.SuggestedStartLocation = PickerLocationId.Desktop;
        _folderPicker.FileTypeFilter.Add("*");

        apiMethods
            .Add(new ApiEntry(0, "_pyton_settings", typeof(APytonSettings), null, false, null, true));

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

    public async Task<StorageFile> SaveFile(params Tuple<string, string>[] extensions)
    {
        foreach (var extension in extensions)
        {
            _savePicker.FileTypeChoices.Add(extension.Item1, [extension.Item2]);
        }
        

        StorageFile file = await _savePicker.PickSaveFileAsync();

        _savePicker.FileTypeChoices.Clear();

        return file;
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

        _loadPicker.FileTypeFilter.Clear();
        return readData;
    }

    public async Task<StorageFile> SelectFile(params string[] extensions)
    {
        foreach (var extension in extensions)
        {
            _loadPicker.FileTypeFilter.Add(extension);
        }
        var file = await _loadPicker.PickSingleFileAsync();

        _loadPicker.FileTypeFilter.Clear();
        return file;
    }

    public async Task<StorageFolder> SelectFolder()
    {
        return await _folderPicker.PickSingleFolderAsync();
    }
}
