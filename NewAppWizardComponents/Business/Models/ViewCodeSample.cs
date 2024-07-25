using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewAppWizardComponents;
public class ViewCodeSample
{
    public string content;
    public ViewCodeSampleType type;

    public ViewCodeSample(string content_, ViewCodeSampleType type_)
    {
        content = content_;
        type = type_;
    }
}

public enum ViewCodeSampleType
{
    Type,
    Default,
    Keyword,
    Brackets,
    Value,
    Method
}
