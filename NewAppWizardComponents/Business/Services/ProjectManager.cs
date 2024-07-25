using Microsoft.UI.Xaml.Controls;
using Windows.Storage.Pickers;
using WinRT.Interop; 

namespace NewAppWizardComponents;
class ProjectManager
{
    private FileSavePicker _savePicker;
    private nint _windowHandler;

    private Dictionary<string, List<string>> _fileTypes = new Dictionary<string, List<string>>()
    {
        { "C#", new List<string> { ".cs" } },
        { "Python", new List<string> { ".py" } },
        { "VB.Net", new List<string> { ".vb" } },
        { "VBA", new List<string> { ".vba" } },
        { "MATLAB", new List<string> { ".m" } },
        { "S-expr", new List<string> { ".sexp", ".lisp" } },
        { "XML", new List<string> { ".xml" } }
    };

    public ProjectManager()
    {
        _savePicker = new FileSavePicker();
        _windowHandler = WindowNative.GetWindowHandle(App.MainWindow);

        InitializeWithWindow.Initialize(_savePicker, _windowHandler);
        _savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
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
}
