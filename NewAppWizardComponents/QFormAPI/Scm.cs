using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewAppWizardComponents;

	public class ScmEntry
	{
		public List<string>	comment;
		public BScm			bscm;
	}

	public class Scm
	{
		string	m_s;
		int		m_p;
		int		m_e;

		public void
			open_file(string file)
		{
			string text = File.ReadAllText(file, Encoding.UTF8);
			open_str(text);
		}
		public void
			open_str(string text)
		{
			m_s = text;
			m_p = 0;
			m_e = text.Length;
		}

		bool
			at_end()
		{
			return m_p >= m_e;
		}

		public ScmEntry
			get()
		{
			ScmEntry e = new ScmEntry();
			ss();
			if (at_end())
				return null;
			while (at(";", true))
				parse_comment(e);

			if (at("(", true))
			{
				e.bscm = new BScm();
				parse_entry(e.bscm);
				return e;
			}

			string sym = parse_sym();
			if (string.IsNullOrEmpty(sym))
				return null;

			e.bscm = new BScm();
			e.bscm.value_str_set(sym);

			return e;
		}

		BScm
			add(BScm E)
		{
			if (E.childs == null)
				E.childs = new List<BScm>();
			BScm c = new BScm();
			E.childs.Add(c);
			return c;
		}

		void
			parse_entry(BScm E)
		{
			string sym = parse_sym();
			E.value_str_set(sym);
			int nested_lists = 0;
			for (;;)
			{
				ss();

				char c = cur();

				if (c == 0)
					throw new Exception("Unexpected EOF");

				if (at(")", true))
					break;

				if (at("[", true))
				{
					nested_lists++;
					continue;
				}

				if (at("]", true))
				{
					nested_lists--;
					if (nested_lists < 0)
						throw new Exception("Syntax Error");
					continue;
				}

				if (at("(", true))
				{
					BScm ce = add(E);
					parse_entry(ce);
					continue;
				}

				if (at("\"", true))
				{
					string str = parse_string();
					BScm ce = add(E);
					ce.value_str_set(str);
					continue;
				}

				if (symchar(c))
				{
					string str = parse_sym();
					BScm ce = add(E);
					ce.value_str_set(str);
					continue;
				}
			}

			if (nested_lists > 0)
				throw new Exception("Syntax Error");
		}

		void
			want(string pref, bool adv)
		{
			if (!at(pref, adv))
				throw new Exception("Error: '" + pref + "' expected");
		}

		static bool oneof(char c, string chars)
		{
			for (int i = 0; i < chars.Length; i++)
			{
				if (chars[i] == c)
					return true;
			}
			return false;
		}

		string
			parse_string()
		{
			StringBuilder sb = new StringBuilder();
			while (m_p < m_e)
			{
				char c = cur();
				if (c == '\"')
					break;
				if (c == '\\')
				{
					c = next();
					switch (c)
					{
						case '\"': sb.Append(c); break;
						case '\\': sb.Append(c); break;
						case '0': sb.Append("\0"); break;
						case 'a': sb.Append("\a"); break;
						case 'b': sb.Append("\b"); break;
						case 'f': sb.Append("\f"); break;
						case 'n': sb.Append("\n"); break;
						case 'r': sb.Append("\r"); break;
						case 't': sb.Append("\t"); break;
						case 'v': sb.Append("\v"); break;
					}
				}
				else
				{
					sb.Append(c);
				}
				m_p++;
			}
			if (at("\"", true))
				return sb.ToString();

			throw new Exception("Unexpected EOF");
		}

		static bool
			symchar(char c)
		{
			return Char.IsLetterOrDigit(c) || oneof(c, "@$+-_.,/");
		}

		string
			parse_sym()
		{
			int s = m_p;
			int n = 0;
			for (; m_p < m_e; ++n, ++m_p)
			{
				char c = cur();
				if (symchar(c))
					continue;
				break;
			}
			if (n == 0)
				throw new Exception("Error symbol expected");
			return m_s.Substring(s, n);
		}

		void
			parse_comment(ScmEntry E)
		{
			int s = m_p;
			int n = 0;
			for(; m_p < m_e; m_p++, n++)
			{
				char c = cur();
				if (c == '\n' || c == '\r')
					break;
			}

			ss();

			string cm = m_s.Substring(s, n);
			cm = cm.Trim();
			if (string.IsNullOrEmpty(cm))
				return;

			if (E.comment == null)
				E.comment = new List<string>();

			E.comment.Add(cm);
		}

		bool
			at(string pref, bool adv)
		{
			if (pref.Length > avl())
				return false;

			for (int i = 0; i < pref.Length; i++)
			{
				if (pref[i] != m_s[i+m_p])
					return false;
			}

			if (adv)
				m_p += pref.Length;

			return true;
		}

		int
			avl()
		{
			return m_e - m_p;
		}

		char
			cur()
		{
			if (m_p < m_e)
				return m_s[m_p];

			return (char)0;
		}

		char
			next()
		{
			if (m_p >= m_e)
				throw new Exception("Unexpected EOF");
			++m_p;
			return m_s[m_p];
		}

		void
			ss()
		{
			for (; m_p < m_e; m_p++)
			{
				char c = cur();
				if (!Char.IsWhiteSpace(c))
					break;
			}
		}
	}
