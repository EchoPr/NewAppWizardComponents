using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewAppWizardComponents;
public class SessionInfo
{
    public int Id { get; set; }

    private string _name;
    public string Name { get => this.ToString(); set { _name = value; } }

    public SessionInfo(string n, int id)
    {
        _name = n;
        Id = id;
    }
    public override string ToString() => _name;
}
