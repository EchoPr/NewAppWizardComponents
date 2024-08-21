using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewAppWizardComponents;
public interface ICodeGenerator
{
    public List<ViewCodeSample> GenerateCodeEntry(ApiEntry entry, int entrySerialNumber, CodeGenerationMode mode);

    public List<ViewCodeSample> GenerateApiSnippet(ApiEntry entry) { return new List<ViewCodeSample>(); }
}

public enum CodeGenerationMode
{
    ObjectInit,
    StepByStep,
    Regen
}
