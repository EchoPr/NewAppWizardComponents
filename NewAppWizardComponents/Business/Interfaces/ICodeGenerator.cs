using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewAppWizardComponents;
public interface ICodeGenerator
{
    public List<ViewCodeSample> GenerateCodeEntry(ApiEntry entry, int entrySerialNumber, CodeGenerationMode mode);
}

public enum CodeGenerationMode
{
    ObjectInit,
    StepByStep
}
