using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewAppWizardComponents;

public class ExpandedEntry
{
    public ApiEntry apiEntry { get; set; }
    public int index { get; set; }
    public bool isConnectedBlockSequentialInitialized = false;

    public ExpandedEntry(ApiEntry entry, int id)
    {
        apiEntry = entry;
        index = id;
    }

    public ExpandedEntry(ApiEntry entry, int id, bool seqInited) : this(entry, id)
    {
        isConnectedBlockSequentialInitialized = seqInited;
    }
}
