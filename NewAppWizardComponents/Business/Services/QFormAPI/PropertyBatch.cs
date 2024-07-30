using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewAppWizardComponents;

class PropertyAcc
{
    public const int ACC_VALUE = 0;
    public const int ACC_DBX_VALUE = 1;
    public const int ACC_DBX_TABLE = 2;
    public const int ACC_CHART = 3;
    public const int ACC_FIELD = 4;
    public const int ACC_DBX_SAVE = 5;
		public const int ACC_FIELD_TP	= 6;
		public const int ACC_FIELD_TL	= 7;
		public const int ACC_FIELD_MP	= 8;
		public const int ACC_FIELD_P	= 9;
		public const int ACC_FIELD_AVT = 10;
		public const int ACC_FIELD_MMAX = 11;
    public const int ACC_STAT = 12;
    public const int ACC_STAT_SECT = 13;
		public const int ACC_ISO = 14;
		public const int ACC_ISO_EXP = 15;
		public const int ACC_CHART_VAL = 16;
    public const int ACC_ISOL = 17;
    public const int ACC_ISOL_EXP = 18;
		public const int ACC_OBJECT = 19;
		public const int ACC_REAL_ARRAY = 20;
    public const int ACC_ARB_DRIVE = 21;

    public const int FT_NIL = 0;
    public const int FT_SCALAR = 1 << 0;
    public const int FT_VECTOR = 1 << 1;
    public const int FT_STD = 1 << 2;
    public const int FT_DEBUG = 1 << 3;
    public const int FT_ELEM = 1 << 4;

		public const int PROP_VALUE			= 1;
		public const int PROP_RESOURCE		= 2;
		public const int PROP_INHERITED		= 4;
		public const int PROP_AUTO			= 5;

    public NewAppWizardComponents.ObjectType object_type { get; set; }
    public int object_number { get; set; }
    public int object_part { get; set; }
    public int object_func { get; set; }
    public NewAppWizardComponents.ObjectType object2_type { get; set; }
    public int object2_number { get; set; }
    public int object2_part { get; set; }
    public int object2_func { get; set; }
    public int acc_type { get; set; }
    public string path { get; set; }
    public string type { get; set; }
    public int prop_type { get; set; }
    public string db_path { get; set; }
		public string db_src_path { get; set; }
		public int db_type { get; set; }
    public string name { get; set; }
    public string field_name { get; set; }
    public string value { get; set; }
    public string enum_name { get; set; }
    public string enum_value { get; set; }
    public string units { get; set; }

    public int field_stg { get; set; }
    public int field_type { get; set; }
		public int field_target { get; set; }
		public int field_src { get; set; }
    public int field_src_op { get; set; }
		public int field_units { get; set; }
		public double field_min { get; set; }
		public double field_max { get; set; }

		public NewAppWizardComponents.DbTableArg arg_rows        { get; set; }
    public NewAppWizardComponents.DbTableArg arg_cols        { get; set; }
    public NewAppWizardComponents.DbTableArg arg_layers      { get; set; }
    public string rows_units    { get; set; }
    public string cols_units    { get; set; }
    public string layers_units  { get; set; }
    public string rows_name     { get; set; }
    public string cols_name     { get; set; }
    public string layers_name   { get; set; }


}
class PropertyBatch
{
    public List<PropertyAcc> properties { get; set; }

    public string name { get; set; }
    public bool setter { get; set; }
    public int seq { get; set;  }
		public string wiz_start_time { get; set; }

	}
