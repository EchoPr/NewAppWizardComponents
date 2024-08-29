using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.VoiceCommands;

namespace NewAppWizardComponents;
public class QFormManager
{
    QForm _qform = new QForm();

    string? _qformDir = null;

    public event EventHandler<string> ErrorMessageRequested;
    public event EventHandler InvokationResultsReceived;
    public event EventHandler InvocationStarted;
    public event EventHandler InvocationEnded;

    private string qformBaseDir = "C:\\QForm\\11.0.2";
    public string QFormBaseDir { get => qformBaseDir; set => qformBaseDir = value; }
    public string qformVersion = "11.0.2";
    public string qformTempDir;



    public QFormManager()
    {
        SetBaseDir(qformBaseDir);
    }

    public void SetBaseDir(string dir)
    {
        qformBaseDir = dir;
        _qform.qform_dir_set(Path.Combine(qformBaseDir, "x64"));
    }

    public void attachQForm() 
    {
        if (_qform.is_started_by_qform())
        {
            _qform.qform_attach();
        }
    }

    public async Task invokeMethod(ApiEntry apiEntry)
    {
        if (apiEntry == null)
            return;

        if (apiEntry.code_func == null)
        {
            if (apiEntry.service_cmd == 0)
            {
                if (!_qform.qform_is_running())
                {
                    ErrorMessageRequested?.Invoke(this, "There will be able to connect QForm session");
                    return;
                }
            }
        }
        else
        {
            if (_HasAttribute(apiEntry.code_func, typeof(ServiceDenyInvoke)))
            {
                ErrorMessageRequested?.Invoke(this, "This function is not available in the Application Wizard");
                return;
            }
        }

        InvocationStarted?.Invoke(this, null);
        object arg = apiEntry.arg_value;
        Type ret_type = apiEntry.ret_type;

        var info = new QForm.CmdInfo();
        try
        {
            Ret rv = apiEntry.ret_value as Ret;
            if (rv == null)
                rv = new Ret();
            rv.invocationResultStatus = "\u231B";
            apiEntry.ret_value = rv;

            object ret;
            if (apiEntry.code_func != null)
                ret = await Task.Run(() => _CodeFunctionInvoke(apiEntry, arg, ret_type));
            else
                ret = await Task.Run(() => _qform.invoke(apiEntry.service_cmd, apiEntry.Name, arg, ret_type, info));

            if (ret.GetType() == typeof(Tuple<string>)) throw new Exception((ret as Tuple<string>).Item1);

            if (ret_type != null)
            {
                if (ret is Ret r)
                {
                    r.invocationResultStatus = "Ok";
                    apiEntry.ret_value = ret;
                }
                else
                {
                    apiEntry.ret_value = new Ret { invocationResultStatus = "Ok" };
                    apiEntry.ret_type = typeof(Ret);
                }
                InvocationEnded?.Invoke(this, null);
            }
            else
            {
                apiEntry.ret_value = new Ret { invocationResultStatus = "Ok" };
                apiEntry.ret_type = typeof(Ret);
                InvocationEnded?.Invoke(this, null);
            }
        }
        catch (Exception ex)
        {
            if (ret_type != null)
            {
                object ret = Activator.CreateInstance(ret_type);
                if (ret is Ret r)
                {
                    r.invocationResultStatus = ex.Message;
                    apiEntry.ret_value = ret;
                }
            }
            else
            {
                apiEntry.ret_value = new Ret { invocationResultStatus = ex.Message };
                apiEntry.ret_type = typeof(Ret);
            }
            InvocationEnded?.Invoke(this, null);
        }

        InvokationResultsReceived?.Invoke(this, null);
    }


    public RSessionList GetAvailableSessions()
    {
        return _qform.session_list();
    }

    public void StartQForm()
    {
        _qform.qform_start();
    }

    public void DetachQForm()
    {
        _qform.qform_detach();
    }

    public void AttachToSession(ASessionId sessionId) 
    {
        _qform.qform_attach_to(sessionId);
    }
    public List<ApiEntry> GetPropertyEntry (PropertyBatch pb)
    {

        List<ApiEntry> res = new List<ApiEntry>();

        foreach (PropertyAcc acc in pb.properties)
        {
            CodeEntry ce = new CodeEntry(1);

            string com = acc.name;
            if (acc.units != null && acc.units.Length > 0)
                com += " [" + acc.units + "]";

            ce.comment.Add(com);
            if (!String.IsNullOrEmpty(acc.enum_name))
            {
                if (acc.enum_name == "bool")
                    ce.property_is_bool = true;
                else
                    ce.property_value_type = Type.GetType("" + acc.enum_name);
            }

            if (acc.acc_type == PropertyAcc.ACC_VALUE)
            {
                ce.property_name = acc.name;
                ce.property_units = acc.units;
                ce.property_db_type = acc.db_type;
            }

            switch (acc.acc_type)
            {
                case PropertyAcc.ACC_FIELD_AVT:
                    {
                        AFieldId fa = new AFieldId();
                        fa.field = acc.field_name;
                        fa.field_source = (FieldSource)acc.field_stg;
                        fa.source_object = acc.field_src;
                        fa.source_operation = acc.field_src_op;
                        fa.field_min = acc.field_min;
                        fa.field_max = acc.field_max;
                        fa.units = acc.field_units;
                        fa.field_type = acc.field_type;
                        fa.field_target = acc.field_target;
                        ce.arg = fa;
                        ce.cmd = "active_field_set";
                        break;
                    }
                case PropertyAcc.ACC_FIELD:
                    {
                        AFieldAtMesh fa = new AFieldAtMesh();
                        fa.field = acc.field_name;
                        fa.field_source = (FieldSource)acc.field_stg;
                        fa.mesh_index = 0;
                        fa.source_object = acc.field_src;
                        fa.source_operation = acc.field_src_op;
                        ce.arg = fa;
                        if (0 != (acc.field_type & PropertyAcc.FT_VECTOR))
                        {
                            ce.cmd = "field_get_vector";
                            ce.res = new RVectorField();
                        }
                        else
                        {
                            ce.cmd = "field_get";
                            ce.res = new RField();
                        }
                        break;
                    }
                case PropertyAcc.ACC_FIELD_MMAX:
                    {
                        AFieldAtMinMax fa = new AFieldAtMinMax();
                        fa.field = acc.field_name;
                        fa.field_source = (FieldSource)acc.field_stg;
                        fa.mesh_index = 0;
                        fa.source_object = acc.field_src;
                        fa.source_operation = acc.field_src_op;
                        ce.arg = fa;
                        ce.cmd = "field_min_max";
                        ce.res = new RFieldMinMax();
                        break;
                    }
                case PropertyAcc.ACC_STAT:
                    {
                        AFieldStatAtMesh fa = new AFieldStatAtMesh();
                        fa.field = acc.field_name;
                        fa.field_source = (FieldSource)acc.field_stg;
                        fa.mesh_index = 0;
                        fa.source_object = acc.field_src;
                        fa.source_operation = acc.field_src_op;
                        ce.arg = fa;
                        ce.cmd = "field_stat";
                        ce.res = new RFieldStat();
                        break;
                    }
                case PropertyAcc.ACC_STAT_SECT:
                    {
                        AFieldStatAtSection fa = new AFieldStatAtSection();
                        fa.field = acc.field_name;
                        fa.field_source = (FieldSource)acc.field_stg;
                        fa.mesh_index = 0;
                        fa.source_object = acc.field_src;
                        fa.source_operation = acc.field_src_op;
                        ce.arg = fa;
                        ce.cmd = "field_stat_at_section";
                        ce.res = new RFieldStat();
                        break;
                    }
                case PropertyAcc.ACC_ISO:
                    {
                        AFieldIsosurface fa = new AFieldIsosurface();
                        fa.field = acc.field_name;
                        fa.field_source = (FieldSource)acc.field_stg;
                        fa.mesh_index = 0;
                        fa.source_object = acc.field_src;
                        fa.source_operation = acc.field_src_op;
                        ce.arg = fa;
                        ce.cmd = "field_isosurface";
                        ce.res = new RIsosurfaceList();
                        break;
                    }

                case PropertyAcc.ACC_ISO_EXP:
                    {
                        AExportFieldIsosurface fa = new AExportFieldIsosurface();
                        fa.field = acc.field_name;
                        fa.field_source = (FieldSource)acc.field_stg;
                        fa.mesh_index = 0;
                        fa.source_object = acc.field_src;
                        fa.source_operation = acc.field_src_op;
                        ce.arg = fa;
                        ce.cmd = "export_field_isosurface";
                        ce.res = null;
                        break;
                    }

                case PropertyAcc.ACC_ISOL:
                    {
                        AFieldIsosurface fa = new AFieldIsosurface();
                        fa.field = acc.field_name;
                        fa.field_source = (FieldSource)acc.field_stg;
                        fa.mesh_index = 0;
                        fa.source_object = acc.field_src;
                        fa.source_operation = acc.field_src_op;
                        ce.arg = fa;
                        ce.cmd = "field_isolines";
                        ce.res = new RIsolineList();
                        break;
                    }

                case PropertyAcc.ACC_ISOL_EXP:
                    {
                        AExportFieldIsolines fa = new AExportFieldIsolines();
                        fa.field = acc.field_name;
                        fa.field_source = (FieldSource)acc.field_stg;
                        fa.mesh_index = 0;
                        fa.source_object = acc.field_src;
                        fa.source_operation = acc.field_src_op;
                        ce.arg = fa;
                        ce.cmd = "export_field_isolines";
                        ce.res = null;
                        break;
                    }

                case PropertyAcc.ACC_FIELD_TP:
                case PropertyAcc.ACC_FIELD_TL:
                    {
                        AFieldAtTrackingObject fa = new AFieldAtTrackingObject();
                        fa.field = acc.field_name;
                        fa.field_source = (FieldSource)acc.field_stg;
                        fa.mesh_index = 0;
                        fa.source_object = acc.field_src;
                        fa.source_operation = acc.field_src_op;
                        ce.arg = fa;
                        bool at_pt = PropertyAcc.ACC_FIELD_TP == acc.acc_type;
                        if (0 != (acc.field_type & PropertyAcc.FT_VECTOR))
                        {
                            if (at_pt)
                            {
                                ce.cmd = "field_at_tracking_point_vector";
                                ce.res = new RVectorValue();
                            }
                            else
                            {
                                ce.cmd = "field_at_tracking_line_vector";
                                ce.res = new RVectorValues();
                            }
                        }
                        else
                        {
                            if (at_pt)
                            {
                                ce.cmd = "field_at_tracking_point";
                                ce.res = new RRealValue();
                            }
                            else
                            {
                                ce.cmd = "field_at_tracking_line";
                                ce.res = new RRealValues();
                            }
                        }
                        break;
                    }
                case PropertyAcc.ACC_FIELD_MP:
                    {
                        AFieldAtMeshPoint fa = new AFieldAtMeshPoint();
                        fa.field = acc.field_name;
                        fa.field_source = (FieldSource)acc.field_stg;
                        fa.mesh_index = 0;
                        fa.source_object = acc.field_src;
                        fa.source_operation = acc.field_src_op;
                        ce.arg = fa;
                        if (0 != (acc.field_type & PropertyAcc.FT_VECTOR))
                        {
                            ce.cmd = "field_at_mesh_point_vector";
                            ce.res = new RVectorValue();
                        }
                        else
                        {
                            ce.cmd = "field_at_mesh_point";
                            ce.res = new RRealValue();
                        }
                        break;
                    }
                case PropertyAcc.ACC_FIELD_P:
                    {
                        AFieldAtPoint fa = new AFieldAtPoint();
                        fa.field = acc.field_name;
                        fa.field_source = (FieldSource)acc.field_stg;
                        fa.mesh_index = 0;
                        fa.source_object = acc.field_src;
                        fa.source_operation = acc.field_src_op;
                        ce.arg = fa;
                        if (0 != (acc.field_type & PropertyAcc.FT_VECTOR))
                        {
                            ce.cmd = "field_at_point_vector";
                            ce.res = new RVectorValue();
                        }
                        else
                        {
                            ce.cmd = "field_at_point";
                            ce.res = new RRealValue();
                        }
                        break;
                    }

                case PropertyAcc.ACC_CHART:
                    {
                        AChartId ca = new AChartId();
                        ca.arg_object_type = acc.object_type;
                        ca.arg_object_id = acc.object_number;
                        ca.arg_subobject = acc.object_part;
                        ca.arg_id = acc.object_func;

                        ca.func_object_type = acc.object2_type;
                        ca.func_object_id = acc.object2_number;
                        ca.func_subobject = acc.object2_part;
                        ca.func_id = acc.object2_func;

                        ce.cmd = "chart_get";
                        ce.arg = ca;
                        ce.res = new RChart();
                    }
                    break;
                case PropertyAcc.ACC_CHART_VAL:
                    {
                        AChartValueId ca = new AChartValueId();
                        ca.arg_object_type = acc.object_type;
                        ca.arg_object_id = acc.object_number;
                        ca.arg_subobject = acc.object_part;
                        ca.arg_id = acc.object_func;

                        ca.func_object_type = acc.object2_type;
                        ca.func_object_id = acc.object2_number;
                        ca.func_subobject = acc.object2_part;
                        ca.func_id = acc.object2_func;

                        ce.cmd = "chart_value_get";
                        ce.arg = ca;
                        ce.res = new RChartValue();
                    }
                    break;

                case PropertyAcc.ACC_ARB_DRIVE:
                    {
                        if (pb.setter)
                        {
                            ADbArbitraryDriveRecords pa = new ADbArbitraryDriveRecords();
                            pa.db_path = acc.db_path;
                            ce.arg = pa;
                            ce.cmd = "db_arbitrary_drive_set_records";
                        }
                        else
                        {
                            ADbObjectPath pa = new ADbObjectPath();
                            pa.db_path = acc.db_path;
                            ce.arg = pa;
                            ce.res = new RDbArbitraryDriveRecords();
                            ce.cmd = "db_arbitrary_drive_get_records";
                        }
                        break;
                    }

                case PropertyAcc.ACC_OBJECT:
                    {
                        if (pb.setter)
                        {
                            AObjectIdProperty pa = new AObjectIdProperty();
                            pa.object_id = acc.object_number;
                            pa.object_type = acc.object_type;
                            pa.path = acc.path;
                            ce.arg = pa;
                            pa.value_type = acc.object2_type;
                            pa.value_id = acc.object2_number;
                            ce.cmd = "property_set_object";
                        }
                        else
                        {
                            APropertyPath pa = new APropertyPath();
                            pa.object_id = acc.object_number;
                            pa.object_type = acc.object_type;
                            pa.path = acc.path;
                            ce.arg = pa;
                            ce.cmd = "property_get_object";
                            ce.res = new RObjectId();
                        }
                        break;
                    }

                case PropertyAcc.ACC_REAL_ARRAY:
                    {
                        if (pb.setter)
                        {
                            APropertyArrayOfReal pa = new APropertyArrayOfReal();
                            pa.object_id = acc.object_number;
                            pa.object_type = acc.object_type;
                            pa.path = acc.path;
                            ce.arg = pa;
                            ce.cmd = "property_set_array_of_real";
                        }
                        else
                        {
                            APropertyPath pa = new APropertyPath();
                            pa.object_id = acc.object_number;
                            pa.object_type = acc.object_type;
                            pa.path = acc.path;
                            ce.arg = pa;
                            ce.cmd = "property_get_array_of_real";
                            ce.res = new RArrayOfReal();
                        }
                        break;
                    }

                case PropertyAcc.ACC_VALUE:
                    {
                        AProperty pa = new AProperty();
                        pa.object_id = acc.object_number;
                        pa.object_type = acc.object_type;
                        pa.path = acc.path;
                        pa.property_type = (PropertyType)acc.prop_type;
                        ce.arg = pa;
                        if (pb.setter)
                        {
                            if (acc.enum_value != null && acc.enum_value.Length > 0)
                                pa.value = acc.enum_value;
                            else
                                pa.value = acc.value;
                            ce.cmd = "property_set";
                        }
                        else
                        {
                            ce.cmd = "property_get";
                            ce.res = new RPropertyValue();
                        }
                    }
                    break;

                case PropertyAcc.ACC_DBX_VALUE:
                    {
                        ADbProperty dpa = new ADbProperty();
                        ce.arg = dpa;
                        dpa.db_path = acc.db_path;
                        dpa.prop_path = acc.path;

                        if (pb.setter)
                        {
                            if (acc.enum_value != null && acc.enum_value.Length > 0)
                                dpa.value = acc.enum_value;
                            else
                                dpa.value = acc.value;
                            ce.cmd = "db_property_set";
                        }
                        else
                        {
                            ce.cmd = "db_property_get";
                            ce.res = new RPropertyValue();
                        }
                    }
                    break;

                case PropertyAcc.ACC_DBX_SAVE:
                    {
                        ASrcTargetPath dpa = new ASrcTargetPath();
                        ce.arg = dpa;
                        dpa.source_path = acc.db_src_path;
                        dpa.target_path = acc.db_path;
                        ce.cmd = "db_object_save";
                    }
                    break;

                case PropertyAcc.ACC_DBX_TABLE:
                    {
                        if (acc.arg_rows != DbTableArg.Nothing)
                            ce.comment.Add("row_args : " + acc.rows_name);
                        if (acc.arg_cols != DbTableArg.Nothing)
                            ce.comment.Add("column_args : " + acc.cols_name);
                        if (acc.arg_layers != DbTableArg.Nothing)
                            ce.comment.Add("layer_args : " + acc.layers_name);

                        if (pb.setter)
                        {
                            var dpa = new ADbPropertyTable();
                            ce.arg = dpa;
                            dpa.db_path = acc.db_path;
                            dpa.prop_path = acc.path;
                            dpa.column_arg = acc.arg_cols;
                            dpa.row_arg = acc.arg_rows;
                            dpa.layer_arg = acc.arg_layers;
                            ce.cmd = "db_property_table_set";
                        }
                        else
                        {
                            ADbProperty dpa = new ADbProperty();
                            ce.arg = dpa;
                            dpa.db_path = acc.db_path;
                            dpa.prop_path = acc.path;
                            ce.cmd = "db_property_table_get";
                            ce.res = new RDbPropertyTable();
                        }

                    }
                    break;
            }

            ApiEntry apiEntry = new ApiEntry(
                0,
                ce.cmd,
                ce.arg.GetType(),
                null,
                false,
                Array.Empty<LStr>()
            );

            apiEntry.arg_value = ce.arg;
            
            apiEntry.comment = ce.comment;

            res.Add( apiEntry );

        }

        return res;
    }

    private bool _HasAttribute(MethodInfo m, Type t)
    {
        foreach (var a in m.CustomAttributes)
        {
            if (a.AttributeType == t)
                return true;
        }
        return false;
    }

    private object _CodeFunctionInvoke(ApiEntry e, object arg, Type ret_type)
    {
        object[] pars = null;
        if (arg != null)
        {
            CodeArg ca = arg as CodeArg;
            pars = new object[] { ca.value_get() };
        }

        object ret = null;
        try
        {
            ret = e.code_func.Invoke(_qform, pars);
        }
        catch (TargetInvocationException ex)
        {
            throw ex.InnerException;
        }

        if (ret_type != null)
        {
            object robj = Activator.CreateInstance(ret_type);
            CodeRet cr = robj as CodeRet;
            cr.value_set(ret);

            return cr;
        }
        return null;
    }

}
