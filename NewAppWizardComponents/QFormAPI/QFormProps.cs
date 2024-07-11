using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NewAppWizardComponents;

public class QFormPropPrefix
{
    public string key { get; set; }
    public string en { get; set; }
    public string ru { get; set; }
}

public enum DBXItemType
{
    IT_IGNORE = -1,
    IT_FILE = 0,
    IT_DIR,
    IT_STD_ROOT,
    IT_USR_ROOT,
    IT_DOC_ROOT,
    IT_BUILT_IN_ROOT,
    IT_ATT_ROOT,
    IT_DEV_ROOT,
    IT_NET_ROOT,
};

//public class DBXItem
//{
//    public bool expanded = false;
//    public string name { get; set; }
//    public string vname { get; set; }
//    public int type { get; set; }
//    public int bid { get; set; }
//    public List<DBXItem> childs { get; set; }

//    public string caption()
//    {
//        if (vname != null)
//            return vname;

//        return name;
//    }

//    public string pname()
//    {
//        DBXItemType tt = (DBXItemType)type;
//        switch (tt)
//        {
//            case DBXItemType.IT_IGNORE:
//                return null;

//            case DBXItemType.IT_BUILT_IN_ROOT: return "qform";
//            case DBXItemType.IT_STD_ROOT: return "std";
//            case DBXItemType.IT_USR_ROOT: return "usr";
//            case DBXItemType.IT_DOC_ROOT: return "doc";
//            case DBXItemType.IT_NET_ROOT: return "net";
//        }

//        if (type == 0 && bid > 0)
//        {
//            return bid.ToString();
//        }

//        return name;
//    }

//    public static DBXItem
//        load(int db_type)
//    {
//        string str = System.IO.File.ReadAllText(@"D:\QForm\_build\API\db-" + db_type + ".json", Encoding.UTF8);
//        return JsonSerializer.Deserialize<DBXItem>(str);
//    }

//    public override string ToString()
//    {
//        return vname;
//    }
//}

public class QFormProp
{
    public static LangFile langFile = null;

    public string name { get; set; }
    public string name_ru { get; set; }
    public string name_en { get; set; }
    public List<QFormPropPrefix> prefix { get; set; }
    public string type { get; set; }
    public string path { get; set; }
    public string si_unit { get; set; }
    public string en_unit { get; set; }
    public int db_type { get; set; }

    public override string ToString()
    {
        if (langFile == null)
        {
            langFile = new LangFile();
            langFile.load(null);
        }

        return langFile.name(this);
    }

    public static LangFile lang_file()
    {
        if (langFile == null)
        {
            langFile = new LangFile();
            langFile.load(null);
        }
        return langFile;
    }


    public string unit()
    {
        if (null == si_unit)
            return "";

        if (si_unit.Equals(en_unit))
            return "[" + si_unit + "]";

        return "[" + si_unit + "] [" + en_unit + "]";
    }

}
public class QFormProps
{
    public Dictionary<string, List<QFormProp>> props;

    public void
        parse()
    {
        string str = System.IO.File.ReadAllText(@"D:\QForm\_build\API\props.json", Encoding.UTF8);
        props = JsonSerializer.Deserialize<Dictionary<string, List<QFormProp>>>(str);

        QFormProp pr;
        string s;
        foreach (var kv in props)
        {
            foreach (QFormProp p in kv.Value)
            {
                if (p.prefix != null)
                {
                    pr = p;
                }

                if (p.si_unit != null)
                    s = p.si_unit;
            }
        }

    }
}
