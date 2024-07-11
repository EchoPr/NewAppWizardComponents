using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NewAppWizardComponents;

	[ComVisible(false)]
	public abstract class ListBased
	{
		public abstract object list();
		public abstract void list_set(object o);
		public abstract Type list_type();
	}

	[ComVisible(false)]
	public class ListBasedT<T> : ListBased
	{
		protected List<T> L = new List<T>();
		public override object list() { return L; }
		public override void list_set(object o) { L = (List<T>)o; }
		public override Type list_type() { return typeof(List<T>); }
	}

	[ComVisible(false)]

////block bscm
public class BScm
{
    const byte _Shift = 128;

    public enum BType
    {
        Nil = 1,
        True = 2,
        False = 3,
        Str = 10,
        Wcs = 11,
        Bool = 20,
        Int = 21,
        Double = 22,
        ArrayBool = 40,
        ArrayInt = 41,
        ArrayDouble = 42,

    }

		class Value
    {
        public virtual BType type() { return BType.Nil; }
        public virtual void write(BinaryWriter W) {}
        public virtual void read(BinaryReader R) { }
        public virtual string name() { return null; }
        public virtual object val() { return null; }
        public virtual int byte_size() { return 0; }
    }

		class ValueT<T> : Value
    {
        public T v;
        public override object val() { return v; }
        public override string ToString()
        {
            return v.ToString();
        }
    }

		class ArrayT<T> : Value
    {
        public List<T> v;
        public override object val() { return v; }
        protected int read_size(BinaryReader R)
        {
            int n = R.ReadInt32();
            v = new List<T>();
            v.Capacity = n;
            return n;
        }
        public override string ToString()
        {
            int max_vals = 10;
            if (v.Count < max_vals)
                max_vals = v.Count;
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            for (int i = 0; i < max_vals; i++)
            {
                if (i > 0)
                    sb.Append(" ");
                sb.Append(v[i].ToString());
            }
            if (v.Count > max_vals)
                sb.AppendFormat("... {0} items", v.Count);
            sb.Append("]");
            return sb.ToString();
        }
    }

		class ValueInt : ValueT<int>
    {
        public ValueInt(int w = 0) { v = w; }
        public override BType type() { return BType.Int; }
        public override void write(BinaryWriter W) { W.Write(v);  }
        public override void read(BinaryReader R) { v = R.ReadInt32();  }
        public override int byte_size() { return sizeof(int); }
    }

		class ValueReal : ValueT<double>
    {
        public ValueReal(double w = 0) { v = w; }
        public override BType type() { return BType.Double; }
        public override void write(BinaryWriter W) { W.Write(v); }
        public override void read(BinaryReader R) { v = R.ReadDouble(); }
        public override int byte_size() { return sizeof(double); }
    }

		class ValueString : ValueT<string>
    {
        public ValueString(string w = null) { v = w; }
        public override string name() { return v; }
        public override BType type() { return BType.Str; }
        public override void write(BinaryWriter W)
        {
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(v);
            W.Write(bs.Length);
            W.Write(bs);
        }
        public override int byte_size() { return sizeof(int) + System.Text.Encoding.UTF8.GetByteCount(v); }
    }

		class ValueWcs : ValueString
    {
        public override void read(BinaryReader R)
        {
            int n = R.ReadInt32();
            byte[] bs = R.ReadBytes(n * 2);
            v = System.Text.Encoding.Unicode.GetString(bs);
        }
    }

		class ValueStr : ValueString
    {
        public override void read(BinaryReader R)
        {
            int n = R.ReadInt32();
            byte[] cs = R.ReadBytes(n);
            v = System.Text.Encoding.UTF8.GetString(cs);
        }
    }

		class ValueBool : ValueT<bool>
    {
        public ValueBool(bool V) { v = V;  }
        public override BType type() { return v ? BType.True : BType.False; }
        public override void write(BinaryWriter W) {}
        public override void read(BinaryReader R) {}
        public override int byte_size() { return 0; }
    }

		class ArrayBool : ArrayT<bool>
    {
        public ArrayBool(List<bool> w = null) { v = w; }
        public override BType type() { return BType.ArrayBool; }
        public override void write(BinaryWriter W)
        {
            W.Write(v.Count);
            foreach (var p in v)
                W.Write(p);
        }
        public override void read(BinaryReader R)
        {
            int n = read_size(R);
            for (int i = 0; i < n; i++)
                v.Add(R.ReadBoolean());
        }
        public override int byte_size() { return sizeof(int) + sizeof(bool)*v.Count; }
    }

		class ArrayInt : ArrayT<int>
    {
        public ArrayInt(List<int> w = null) { v = w; }
        public override BType type() { return BType.ArrayInt; }
        public override void write(BinaryWriter W)
        {
            W.Write(v.Count);
            foreach (var p in v)
                W.Write(p);
        }
        public override void read(BinaryReader R)
        {
            int n = read_size(R);
            for (int i = 0; i < n; i++)
                v.Add(R.ReadInt32());
        }
        public override int byte_size() { return sizeof(int) + sizeof(int) * v.Count; }
    }

		class ArrayReal : ArrayT<double>
    {
        public ArrayReal(List<double> w = null) { v = w; }
        public override BType type() { return BType.ArrayDouble; }
        public override void write(BinaryWriter W)
        {
            W.Write(v.Count);
            foreach (var p in v)
                W.Write(p);
        }
        public override void read(BinaryReader R)
        {
            int n = read_size(R);
            for (int i = 0; i < n; i++)
                v.Add(R.ReadDouble());
        }
        public override int byte_size() { return sizeof(int) + sizeof(double) * v.Count; }
    }

    Value value;
    public List<BScm> childs;
    public void read(BinaryReader R)
    {
        byte b = R.ReadByte();
        string dbg;
        if (b > _Shift)
        {
            b -= _Shift;
            dbg = read_value(R, (BType)b);
            int n = R.ReadInt32();
            childs = new List<BScm>(n);
            for (int i = 0; i < n; i++)
            {
                BScm c = new BScm();
                childs.Add(c);
                c.read(R);
            }
        }
        else
        {
            dbg = read_value(R, (BType)b);
        }
    }
    public void write(BinaryWriter W)
    {
        byte t;
        if (value == null)
            t = (byte)BType.Nil;
        else
            t = (byte)value.type();

        if (childs != null)
        {
            t += _Shift;
            W.Write(t);
            write_value(W);
            W.Write(childs.Count);
            foreach (var c in childs)
                c.write(W);
        }
        else
        {
            W.Write(t);
            write_value(W);
        }
    }

    void write_value(BinaryWriter W)
    {
        if (value != null)
            value.write(W);
    }

    Value create_value(BType t)
    {
        switch (t)
        {
            case BType.Nil:          return null;
            case BType.True:         return new ValueBool(true);
            case BType.False:        return new ValueBool(false);
            case BType.Str:          return new ValueStr();
            case BType.Wcs:          return new ValueWcs();
            case BType.Int:          return new ValueInt();
            case BType.Double:       return new ValueReal();
            case BType.ArrayBool:    return new ArrayBool();
            case BType.ArrayInt:     return new ArrayInt();
            case BType.ArrayDouble:  return new ArrayReal();
        }
        throw new Exception("Bad object format");
    }

    string read_value(BinaryReader R, BType t)
    {
        value = create_value(t);
        if (value != null)
        {
            value.read(R);
#if DEBUG
            return value.ToString();
#endif
        }
        return null;
    }

    void
        add(System.Reflection.PropertyInfo nfo, Value v)
    {
        BScm key = new BScm();
        childs.Add(key);
        key.value = new ValueString(nfo.Name);
        key.childs = new List<BScm>();
        BScm val = new BScm();
        key.childs.Add(val);
        val.value = v;
    }

    BScm
        prop_add(string name)
    {
        BScm key = add();
        key.value = new ValueString(name);
        key.childs = new List<BScm>();
        return key;
    }

    BScm
        add()
    {
        BScm key = new BScm();
        childs.Add(key);
        return key;
    }

    public void
        store(Object obj)
    {
        Type t = obj.GetType();
        childs = new List<BScm>();
        foreach(var p in t.GetProperties())
        {
            object v = p.GetValue(obj);
            if (v == null)
					continue;
////del no-com
				if (v is ListBased lb)
					v = lb.list();
////end del no-com
            if (v is bool b)
                add(p, new ValueBool(b));
            else if (v is int i)
                add(p, new ValueInt(i));
            else if (v is double d)
                add(p, new ValueReal(d));
            else if (v is string s)
                add(p, new ValueString(s));
            else if (v is List<int> ai)
                add(p, new ArrayInt(ai));
            else if (v is List<bool> ab)
                add(p, new ArrayBool(ab));
            else if (v is List<double> ad)
                add(p, new ArrayReal(ad));
            else if (p.PropertyType.IsEnum)
                add(p, new ValueString(v.ToString()));
            else if (v is List<string> ls)
            {
                if (ls.Count > 0)
                {
                    BScm key = prop_add(p.Name);
                    foreach (string ss in ls)
                    {
                        BScm val = key.add();
                        val.value_str_set(ss);
                    }
                }
            }
            else if (v is System.Collections.IList lst)
            {
                if (lst.Count > 0)
                {
						Type pt = p.PropertyType;
						Type itm_type = pt.GetGenericArguments()[0];
						BScm key = prop_add(p.Name);

						if (itm_type.IsEnum)
						{
							foreach (Object o in lst)
							{
								BScm val = key.add();
								val.value_str_set(o.ToString());
							}
						}
						else
						{
							foreach (Object o in lst)
							{
								BScm val = key.add();
								val.store(o);
							}
						}
					}
            }
        }

        if (childs.Count == 0)
            childs = null;
    }

    public string
        value_str_get()
    {
        if (value == null)
            return null;
        return value.name();
    }

    public void
        value_str_set(string str)
    {
        value = new ValueString(str);
    }

    public void
        value_int_set(int val)
    {
        value = new ValueInt(val);
    }

		public int
			value_int_get()
		{
			if (value == null)
				throw new System.NullReferenceException("Unable to convert null to int");

			if (value is ValueInt)
				return ((ValueInt)value).v;

			if (value is ValueString)
				return int.Parse(value.ToString());

			throw new System.Exception("Unable to convert object to int");
		}

		public double
			value_real_get()
		{
			if (value == null)
				throw new System.NullReferenceException("Unable to convert null to double");

			if (value is ValueReal)
				return ((ValueReal)value).v;

			if (value is ValueString)
				return stod(value.ToString());

			if (value is ValueInt)
				return ((ValueInt)value).v;

			throw new System.Exception("Unable to convert object to double");
		}

		System.Reflection.PropertyInfo
        find(System.Reflection.PropertyInfo[] props, string name, ref int size)
    {
        for (int i = 0; i < size; i++)
        {
            if (props[i].Name == name)
            {
                var p = props[i];
                --size;
                props[i] = props[size];
                return p;
            }
        }
        return null;
    }

		void load_list(object lstobj, Type pt)
		{
			if (lstobj is List<int>)
			{
				var lst = (List<int>)lstobj;
				foreach (var cs in childs)
					lst.Add(cs.value_int_get());
				return;
			}

			if (lstobj is List<double>)
			{
				var lst = (List<double>)lstobj;
				foreach (var cs in childs)
					lst.Add(cs.value_real_get());
				return;
			}

			if (lstobj is List<string>)
			{
				var lst = (List<string>)lstobj;
				foreach (var cs in childs)
					lst.Add(cs.value_str_get());
				return;
			}

			Type itm_type = pt.GetGenericArguments()[0];
			if (!itm_type.IsValueType)
			{
				var lst = lstobj as System.Collections.IList;
				foreach (var cs in childs)
				{
					var cobj = Activator.CreateInstance(itm_type);
					lst.Add(cobj);
					cs.load(cobj);
				}
				return;
			}
		}

		object load_list(Type pt)
		{
			Type itm_type = pt.GetGenericArguments()[0];

			if (itm_type == typeof(string))
			{
				var lst = Activator.CreateInstance(pt) as List<string>;
				foreach (var cs in childs)
					lst.Add(cs.value_str_get());
				return lst;
			}

			if (itm_type.IsEnum)
			{
				var lst = Activator.CreateInstance(pt) as System.Collections.IList;
				var emap = new Dictionary<string, object>();
				var evals = itm_type.GetEnumValues();
				foreach (var ev in evals)
					emap.Add(itm_type.GetEnumName(ev), ev);

				foreach (var cs in childs)
				{
					string ename = cs.value_str_get();
					object ev;
					if (emap.TryGetValue(ename, out ev))
						lst.Add(ev);
					else
						lst.Add(evals.GetValue(0));
				}
				return lst;
			}

			if (!itm_type.IsValueType)
			{
				var lst = Activator.CreateInstance(pt) as System.Collections.IList;
				foreach (var cs in childs)
				{
					var cobj = Activator.CreateInstance(itm_type);
					lst.Add(cobj);
					cs.load(cobj);
				}
				return lst;
			}
			return null;
		}

		void
        load(Object obj, System.Reflection.PropertyInfo p)
    {
        if (childs == null)
            return;

        BScm bval = childs[0];
        Type pt = p.PropertyType;
        if (pt.IsEnum)
        {
            string ename = bval.value_str_get();
            foreach(var ev in pt.GetEnumValues())
            {
                if (pt.GetEnumName(ev) == ename)
                {
                    p.SetValue(obj, ev);
                    break;
                }
            }
            return;
        }

        if (pt.IsGenericType && pt.GetGenericTypeDefinition() == typeof(List<>))
        {
				if (p.CanWrite)
				{
					object lst = load_list(pt);
					if (lst != null)
					{
						p.SetValue(obj, lst);
						return;
					}
				}
				else
				{
					object lst = p.GetValue(obj);
					load_list(lst, pt);
					return;
				}
        }

////del no-com
			object pv = p.GetValue(obj);
			if (pv is ListBased lb)
			{
				object lst = load_list(lb.list_type());
				if (lst != null)
				{
					lb.list_set(lst);
				}
				else
				{
					object bvl = bval.value.val();
					lb.list_set(bvl);
				}
				return;
			}
////end del no-com
			if (pt.IsClass)
			{
				Type obj_t = obj.GetType();
				if (obj_t.Namespace == pt.Namespace)
				{
					object ov = Activator.CreateInstance(pt);
					load(ov);
					p.SetValue(obj, ov);
					return;
				}
			}
			object bv = bval.value.val();
			if (bv != null && pt != bv.GetType())
				bv = convert(bv, pt);
			p.SetValue(obj, bv);
    }

		double stod(string s)
		{
			return double.Parse(s.Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture);
		}

		bool stob(string s)
		{
			s = s.Trim();
			s = s.ToLower();
			return s == "true";
		}

		object convert(object val, Type type)
		{
			string s = val.ToString();

			if (type == typeof(int))
				return int.Parse(s);

			if (type == typeof(double))
				return stod(s);

			if (type == typeof(bool))
				return stob(s);

			return null;
		}

    public void
        load(Object obj)
    {
        Type t = obj.GetType();
        var props = t.GetProperties();
        int prop_count = props.Length;
        foreach(BScm c in childs)
        {
            string nam = c.value_str_get();
            if (nam == null)
                continue;
            var p = find(props, nam, ref prop_count);
            if (p != null)
                c.load(obj, p);
        }
    }

    public int
        byte_size()
    {
        int sz = 1;
        if (value != null)
            sz += value.byte_size();

        if (childs != null)
        {
            sz += sizeof(int);
            foreach (var ch in childs)
                sz += ch.byte_size();
        }

        return sz;
    }

    public string
        print(StringBuilder sb)
    {
        sb.Clear();
        do_print(sb);

        return sb.ToString();
    }

    void
        do_print(StringBuilder sb)
    {
        if (value != null)
            sb.Append(value.ToString());
        else
            sb.Append("()");

        if (childs == null)
            return;

        foreach (var c in childs)
        {
            if (c.childs != null)
            {
                sb.Append(" (");
                c.do_print(sb);
                sb.Append(")");
            }
            else
            {
                sb.Append(" ");
                c.do_print(sb);
            }
        }
    }
}
////end block
