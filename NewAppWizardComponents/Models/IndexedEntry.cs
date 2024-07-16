using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewAppWizardComponents;

public class IndexedEntry
{
    public ApiEntry apiEntry { get; set; }
    public int index { get; set; }

    public IndexedEntry(ApiEntry entry, int id)
    {
        apiEntry = entry;
        index = id;
    }
}
