using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace NewAppWizardComponents;

public class LangFile
{
    enum Lang
    {
        Eng,
        Russian,
        Other
    }

    Lang language = Lang.Eng;
		string lang_name;
    Dictionary<string, string> table = new Dictionary<string, string>();

    string reg_lang()
    {
        return (string)Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Software\QuantorForm\QForm", "Language", "english");
    }

		public string help_lang()
		{
			return map_lang(lang_name);
		}

		static public string map_lang(string qf_name)
		{
			qf_name = qf_name.ToLower();
			switch(qf_name)
			{
				case "german":		return "DE";
				case "english":		return "EN";
				case "spanish":		return "ES";
				case "french":		return "FR";
				case "italian":		return "IT";
				case "korean":		return "KO";
				case "japanese":	return "JA";
				case "thai":		return "TH";
				case "chinese":		return "ZH-HANS";
				case "chinese_t":	return "ZH-HANT";
				case "polish":		return "PO";
				case "portuguese":	return "PT";
				case "russian":		return "RU";
				case "turkish":		return "TR";
			}
			return qf_name;
		}

    string
        lstr(string key, string en, string ru)
    {
        if (language == Lang.Eng)
            return en;

        if (language == Lang.Russian)
            return ru;

        string s;
        if (table.TryGetValue(key, out s))
            return s;

        return en;
    }

    public string
        str(NewAppWizardComponents.LStr s)
    {
        return lstr(s.key, s.en, s.ru);
    }


    public string
        name(QFormProp prop)
    {
        string s = "";
        if (prop.prefix != null)
        {
            foreach (QFormPropPrefix px in prop.prefix)
            {
                s += lstr(px.key, px.en, px.ru) + "\\";
            }
        }

        if (string.IsNullOrEmpty(prop.name))
            s += prop.path;
        else
            s += lstr(prop.name, prop.name_en, prop.name_ru);
        return s;
    }

    public bool
        load(string lang)
    {
        if (lang == null)
            lang = reg_lang();

			lang_name = lang;

        if (lang.Equals("english", StringComparison.OrdinalIgnoreCase))
        {
            language = Lang.Eng;
            return true;
        }

        if (lang.Equals("russian", StringComparison.OrdinalIgnoreCase))
        {
            language = Lang.Russian;
            return true;
        }

        language = Lang.Other;
        string name = NewAppWizardComponents.QForm.app_directory();
        name += "\\lang\\qform." + lang + ".lang";
        byte[] data = System.IO.File.ReadAllBytes(name);
        return parse(data);
    }

    bool
        parse(byte[] data)
    {
			if (data.Length < 4)
				return false;

        int pos = 0;
        int sz = BitConverter.ToInt32(data, pos);
        pos += sizeof(Int32);

        if (sz <= 0)
            return false;

        int fsz = data.Length - pos;
        int msz = (sizeof(byte) * 2 + sizeof(short) * 2) * sz;
        if (msz > fsz)
            return false;

        List<string> keys = new List<string>();
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < sz; i++)
        {
            int c = data[pos];
            pos++;
            sb.Clear();
            for (int j = 0; j < c; j++)
                sb.Append((Char)data[pos + j]);
            keys.Add(sb.ToString());
            pos += c + 1; // and zero
        }

        for (int i = 0; i < sz; i++)
        {
            int c = BitConverter.ToInt16(data, pos);
            pos += sizeof(Int16);

            sb.Clear();
            for (int j = 0; j < c; j++)
            {
                sb.Append(BitConverter.ToChar(data, pos));
                pos += sizeof(Char);
            }

            string k = keys[i];
            string v = sb.ToString();
            table[k] = v;

            pos += sizeof(Char); // and zero
        }

        return true;
    }
}
