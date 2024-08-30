using System.ComponentModel;

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace NewAppWizardComponents;


public partial class QForm
{
    public static LStr cpi_menu_only = new LStr("CpiMenuOnly", "To add this command, go to the QForm and use the context menu", "Для добавления этой команды перейдите в QForm и используйте контекстное меню");
    ////block api-calls
    public int[] ApiVer = new int[] { 11, 2, 300, 2 };
    [ApiFunction]
    public bool _api_get(AFileName arg)
    {
        return null != invoke(0, "_api_get", arg, null);
    }

    [ApiFunction]
    public bool _csharp_settings(ACSharpSettings arg)
    {
        return null != invoke(0, "_csharp_settings", arg, null);
    }

    [ApiFunction]
    public RDbTreeRet _db_tree(ADbTreeArg arg)
    {
        return (RDbTreeRet)invoke(0, "_db_tree", arg, typeof(RDbTreeRet));
    }

    [ApiFunction]
    public bool _localization_table_get(ASrcTargetPath arg)
    {
        return null != invoke(0, "_localization_table_get", arg, null);
    }

    [ApiFunction]
    public bool _matlab_settings(AMatlabSettings arg)
    {
        return null != invoke(0, "_matlab_settings", arg, null);
    }

    [ApiFunction]
    public bool _pyton_settings(APytonSettings arg)
    {
        return null != invoke(0, "_pyton_settings", arg, null);
    }

    [ApiFunction]
    public bool _vbnet_settings(AVBNetSettings arg)
    {
        return null != invoke(0, "_vbnet_settings", arg, null);
    }


    [ApiFunction]
    public RFieldId active_field_get()
    {
        return (RFieldId)invoke(0, "active_field_get", null, typeof(RFieldId));
    }

    [ApiFunction]
    public bool active_field_reset()
    {
        return null != invoke(0, "active_field_reset", null, null);
    }

    [ApiFunction]
    public bool active_field_set(AFieldId arg)
    {
        return null != invoke(0, "active_field_set", arg, null);
    }

    [ApiFunction]
    public RItemId assembled_tool_create(AAssembledTool arg)
    {
        return (RItemId)invoke(0, "assembled_tool_create", arg, typeof(RItemId));
    }

    [ApiFunction]
    public RAssembledTool assembled_tool_get(AItemId arg)
    {
        return (RAssembledTool)invoke(0, "assembled_tool_get", arg, typeof(RAssembledTool));
    }

    [ApiFunction]
    public bool assembled_tool_split(AItemId arg)
    {
        return null != invoke(0, "assembled_tool_split", arg, null);
    }

    [ApiFunction]
    public RItemList assembled_tools_get()
    {
        return (RItemList)invoke(0, "assembled_tools_get", null, typeof(RItemList));
    }

    [ApiFunction]
    public RSimulationResult async_calculate_tools(AObjectList arg)
    {
        return (RSimulationResult)invoke(0, "async_calculate_tools", arg, typeof(RSimulationResult));
    }

    [ApiFunction]
    public RSimulationResult async_calculate_tools_coupled()
    {
        return (RSimulationResult)invoke(0, "async_calculate_tools_coupled", null, typeof(RSimulationResult));
    }

    [ApiFunction]
    public bool async_execute_subroutines(ASubroutineCalculationMode arg)
    {
        return null != invoke(0, "async_execute_subroutines", arg, null);
    }

    [ApiFunction]
    public bool async_execute_tracking(ATrackingCalculationMode arg)
    {
        return null != invoke(0, "async_execute_tracking", arg, null);
    }

    [ApiFunction]
    public bool async_start_simulation(ASimulationParams arg)
    {
        return null != invoke(0, "async_start_simulation", arg, null);
    }

    [ApiFunction]
    public RAsyncState async_state()
    {
        return (RAsyncState)invoke(0, "async_state", null, typeof(RAsyncState));
    }

    [ApiFunction]
    public bool async_stop_simulation()
    {
        return null != invoke(0, "async_stop_simulation", null, null);
    }

    [ApiFunction]
    public RAsyncEvent async_wait(AAsyncWaitingParams arg)
    {
        return (RAsyncEvent)invoke(0, "async_wait", arg, typeof(RAsyncEvent));
    }

    [ApiFunction]
    public RFieldIdList available_fields_get(AFieldGroupId arg)
    {
        return (RFieldIdList)invoke(0, "available_fields_get", arg, typeof(RFieldIdList));
    }

    [ApiFunction]
    public RObjectAxis axis_calculate(AObjectId arg)
    {
        return (RObjectAxis)invoke(0, "axis_calculate", arg, typeof(RObjectAxis));
    }

    [ApiFunction]
    public bool axis_delete(AObjectAxisId arg)
    {
        return null != invoke(0, "axis_delete", arg, null);
    }

    [ApiFunction]
    public RObjectAxis axis_get(AObjectAxisId arg)
    {
        return (RObjectAxis)invoke(0, "axis_get", arg, typeof(RObjectAxis));
    }

    [ApiFunction]
    public bool axis_inherit(AObjectAxisId arg)
    {
        return null != invoke(0, "axis_inherit", arg, null);
    }

    [ApiFunction]
    public bool axis_set(AObjectAxisParams arg)
    {
        return null != invoke(0, "axis_set", arg, null);
    }

    [ApiFunction]
    public RObjectAxis axis_set_calculated(AObjectId arg)
    {
        return (RObjectAxis)invoke(0, "axis_set_calculated", arg, typeof(RObjectAxis));
    }

    [ApiFunction]
    public RCount billet_count_get()
    {
        return (RCount)invoke(0, "billet_count_get", null, typeof(RCount));
    }

    [ApiFunction]
    public bool billet_count_set(ACount arg)
    {
        return null != invoke(0, "billet_count_set", arg, null);
    }

    [ApiFunction]
    public RItemId billet_get_current()
    {
        return (RItemId)invoke(0, "billet_get_current", null, typeof(RItemId));
    }

    [ApiFunction]
    public RNullableRealValue billet_parameter_get(GBilletParameter arg)
    {
        return (RNullableRealValue)invoke(0, "billet_parameter_get", arg, typeof(RNullableRealValue));
    }

    [ApiFunction]
    public bool billet_parameter_set(ABilletParameter arg)
    {
        return null != invoke(0, "billet_parameter_set", arg, null);
    }

    [ApiFunction]
    public bool billet_set_current(AItemId arg)
    {
        return null != invoke(0, "billet_set_current", arg, null);
    }

    [ApiFunction]
    public RCount blow_count_get()
    {
        return (RCount)invoke(0, "blow_count_get", null, typeof(RCount));
    }

    [ApiFunction]
    public bool blow_count_set(ACount arg)
    {
        return null != invoke(0, "blow_count_set", arg, null);
    }

    [ApiFunction]
    public RItemId blow_get_current()
    {
        return (RItemId)invoke(0, "blow_get_current", null, typeof(RItemId));
    }

    [ApiFunction]
    public RNullableRealValue blow_parameter_get(GBlowParameter arg)
    {
        return (RNullableRealValue)invoke(0, "blow_parameter_get", arg, typeof(RNullableRealValue));
    }

    [ApiFunction]
    public bool blow_parameter_set(ABlowParameter arg)
    {
        return null != invoke(0, "blow_parameter_set", arg, null);
    }

    [ApiFunction]
    public bool blow_set_current(AItemId arg)
    {
        return null != invoke(0, "blow_set_current", arg, null);
    }

    [ApiFunction]
    public RItemId bound_cond_create(ABoundCondParams arg)
    {
        return (RItemId)invoke(0, "bound_cond_create", arg, typeof(RItemId));
    }

    [ApiFunction]
    public bool bound_cond_delete(AItemId arg)
    {
        return null != invoke(0, "bound_cond_delete", arg, null);
    }

    [ApiFunction]
    public bool bound_cond_set_body(AShapeBody arg)
    {
        return null != invoke(0, "bound_cond_set_body", arg, null);
    }

    [ApiFunction]
    public bool bound_cond_set_brick(AShapeBrick arg)
    {
        return null != invoke(0, "bound_cond_set_brick", arg, null);
    }

    [ApiFunction]
    public bool bound_cond_set_circle(AShapeCircle arg)
    {
        return null != invoke(0, "bound_cond_set_circle", arg, null);
    }

    [ApiFunction]
    public bool bound_cond_set_cone(AShapeCone arg)
    {
        return null != invoke(0, "bound_cond_set_cone", arg, null);
    }

    [ApiFunction]
    public bool bound_cond_set_cylinder(AShapeCylinder arg)
    {
        return null != invoke(0, "bound_cond_set_cylinder", arg, null);
    }

    [ApiFunction]
    public bool bound_cond_set_rect(AShapeRect arg)
    {
        return null != invoke(0, "bound_cond_set_rect", arg, null);
    }

    [ApiFunction]
    public bool bound_cond_set_sphere(AShapeSphere arg)
    {
        return null != invoke(0, "bound_cond_set_sphere", arg, null);
    }

    [ApiFunction]
    public bool bound_cond_set_sprayer_polar_array(AShapeSprayerPolarArray arg)
    {
        return null != invoke(0, "bound_cond_set_sprayer_polar_array", arg, null);
    }

    [ApiFunction]
    public bool bound_cond_set_sprayer_polar_array_db(AShapeSprayerPolarArrayDB arg)
    {
        return null != invoke(0, "bound_cond_set_sprayer_polar_array_db", arg, null);
    }

    [ApiFunction]
    public bool bound_cond_set_sprayer_rect_array(AShapeSprayerRectArray arg)
    {
        return null != invoke(0, "bound_cond_set_sprayer_rect_array", arg, null);
    }

    [ApiFunction]
    public bool bound_cond_set_sprayer_rect_array_db(AShapeSprayerRectArrayDB arg)
    {
        return null != invoke(0, "bound_cond_set_sprayer_rect_array_db", arg, null);
    }

    [ApiFunction]
    public bool bound_cond_set_surface_by_color(AShapeSurfaceByColor arg)
    {
        return null != invoke(0, "bound_cond_set_surface_by_color", arg, null);
    }

    [ApiFunction]
    public RBoundCondType bound_cond_type(AItemId arg)
    {
        return (RBoundCondType)invoke(0, "bound_cond_type", arg, typeof(RBoundCondType));
    }

    [ApiFunction]
    public RSimulationResult calculate_tools(AObjectList arg)
    {
        return (RSimulationResult)invoke(0, "calculate_tools", arg, typeof(RSimulationResult));
    }

    [ApiFunction]
    public RSimulationResult calculate_tools_coupled()
    {
        return (RSimulationResult)invoke(0, "calculate_tools_coupled", null, typeof(RSimulationResult));
    }

    [ApiFunction]
    public RCameraDirection camera_direction_get()
    {
        return (RCameraDirection)invoke(0, "camera_direction_get", null, typeof(RCameraDirection));
    }

    [ApiFunction]
    public bool camera_direction_set(ACameraDirection arg)
    {
        return null != invoke(0, "camera_direction_set", arg, null);
    }

    [ApiFunction]
    public RChart chart_get(AChartId arg)
    {
        return (RChart)invoke(0, "chart_get", arg, typeof(RChart));
    }

    [ApiFunction]
    public RChartValue chart_value_get(AChartValueId arg)
    {
        return (RChartValue)invoke(0, "chart_value_get", arg, typeof(RChartValue));
    }

    [ApiFunction]
    public RWebAddress client_server_get()
    {
        return (RWebAddress)invoke(0, "client_server_get", null, typeof(RWebAddress));
    }

    [ApiFunction]
    public bool client_server_set(AWebAddress arg)
    {
        return null != invoke(0, "client_server_set", arg, null);
    }

    [ApiFunction]
    public RExecutionStatus client_server_test_connection(AWebAddress arg)
    {
        return (RExecutionStatus)invoke(0, "client_server_test_connection", arg, typeof(RExecutionStatus));
    }

    [ApiFunction]
    public RContactArea contact_area(AObjectId arg)
    {
        return (RContactArea)invoke(0, "contact_area", arg, typeof(RContactArea));
    }

    [ApiFunction]
    public RField contact_field(AFieldContact arg)
    {
        return (RField)invoke(0, "contact_field", arg, typeof(RField));
    }

    [ApiFunction]
    public RDbArbitraryDriveRecords db_arbitrary_drive_get_records(GDbObjectPath arg)
    {
        return (RDbArbitraryDriveRecords)invoke(0, "db_arbitrary_drive_get_records", arg, typeof(RDbArbitraryDriveRecords));
    }

    [ApiFunction]
    public bool db_arbitrary_drive_set_records(ADbArbitraryDriveRecords arg)
    {
        return null != invoke(0, "db_arbitrary_drive_set_records", arg, null);
    }

    [ApiFunction]
    public RDbItem db_fetch_items(ADbFetchParams arg)
    {
        return (RDbItem)invoke(0, "db_fetch_items", arg, typeof(RDbItem));
    }

    [ApiFunction]
    public bool db_object_create(ADbObjectCreationParams arg)
    {
        return null != invoke(0, "db_object_create", arg, null);
    }

    [ApiFunction]
    public RBoolValue db_object_exists(APathName arg)
    {
        return (RBoolValue)invoke(0, "db_object_exists", arg, typeof(RBoolValue));
    }

    [ApiFunction]
    public bool db_object_export(ASrcTargetPath arg)
    {
        return null != invoke(0, "db_object_export", arg, null);
    }

    [ApiFunction]
    public bool db_object_import(ASrcTargetPath arg)
    {
        return null != invoke(0, "db_object_import", arg, null);
    }

    [ApiFunction]
    public bool db_object_save(ASrcTargetPath arg)
    {
        return null != invoke(0, "db_object_save", arg, null);
    }

    [ApiFunction]
    public bool db_objects_copy_to_project_file()
    {
        return null != invoke(0, "db_objects_copy_to_project_file", null, null);
    }

    [ApiFunction]
    public bool db_objects_save_all()
    {
        return null != invoke(0, "db_objects_save_all", null, null);
    }

    [ApiFunction]
    public RPropertyValue db_property_get(GDbProperty arg)
    {
        return (RPropertyValue)invoke(0, "db_property_get", arg, typeof(RPropertyValue));
    }

    [ApiFunction]
    public bool db_property_set(ADbProperty arg)
    {
        return null != invoke(0, "db_property_set", arg, null);
    }

    [ApiFunction]
    public RDbPropertyTable db_property_table_get(GDbProperty arg)
    {
        return (RDbPropertyTable)invoke(0, "db_property_table_get", arg, typeof(RDbPropertyTable));
    }

    [ApiFunction]
    public bool db_property_table_set(ADbPropertyTable arg)
    {
        return null != invoke(0, "db_property_table_set", arg, null);
    }

    [ApiFunction]
    public bool debug_log_begin(AFileName arg)
    {
        return null != invoke(0, "debug_log_begin", arg, null);
    }

    [ApiFunction]
    public bool debug_log_close()
    {
        return null != invoke(0, "debug_log_close", null, null);
    }

    [ApiFunction]
    public bool disable_assertion()
    {
        return null != invoke(0, "disable_assertion", null, null);
    }

    [ApiFunction]
    public bool do_batch(ABatchParams arg)
    {
        return null != invoke(0, "do_batch", arg, null);
    }

    [ApiFunction]
    public RItemId domain_create(ADomainParams arg)
    {
        return (RItemId)invoke(0, "domain_create", arg, typeof(RItemId));
    }

    [ApiFunction]
    public bool domain_delete(AItemId arg)
    {
        return null != invoke(0, "domain_delete", arg, null);
    }

    [ApiFunction]
    public bool domain_set_body(AShapeBody arg)
    {
        return null != invoke(0, "domain_set_body", arg, null);
    }

    [ApiFunction]
    public bool domain_set_brick(AShapeBrick arg)
    {
        return null != invoke(0, "domain_set_brick", arg, null);
    }

    [ApiFunction]
    public bool domain_set_circle(AShapeCircle arg)
    {
        return null != invoke(0, "domain_set_circle", arg, null);
    }

    [ApiFunction]
    public bool domain_set_cone(AShapeCone arg)
    {
        return null != invoke(0, "domain_set_cone", arg, null);
    }

    [ApiFunction]
    public bool domain_set_cylinder(AShapeCylinder arg)
    {
        return null != invoke(0, "domain_set_cylinder", arg, null);
    }

    [ApiFunction]
    public bool domain_set_rect(AShapeRect arg)
    {
        return null != invoke(0, "domain_set_rect", arg, null);
    }

    [ApiFunction]
    public bool domain_set_sphere(AShapeSphere arg)
    {
        return null != invoke(0, "domain_set_sphere", arg, null);
    }

    [ApiFunction]
    public bool domain_set_surface_by_color(AShapeSurfaceByColor arg)
    {
        return null != invoke(0, "domain_set_surface_by_color", arg, null);
    }

    [ApiFunction]
    public RDomainType domain_type(AItemId arg)
    {
        return (RDomainType)invoke(0, "domain_type", arg, typeof(RDomainType));
    }

    [ApiFunction]
    public RContours dxf_parse_contours(AFileName arg)
    {
        return (RContours)invoke(0, "dxf_parse_contours", arg, typeof(RContours));
    }

    [ApiFunction]
    public RSimulationResult execute_subroutines()
    {
        return (RSimulationResult)invoke(0, "execute_subroutines", null, typeof(RSimulationResult));
    }

    [ApiFunction]
    public RSimulationResult execute_subroutines_advanced(ASubroutineCalculationMode arg)
    {
        return (RSimulationResult)invoke(0, "execute_subroutines_advanced", arg, typeof(RSimulationResult));
    }

    [ApiFunction]
    public bool execute_tracking()
    {
        return null != invoke(0, "execute_tracking", null, null);
    }

    [ApiFunction]
    public RSimulationResult execute_tracking_advanced(ATrackingCalculationMode arg)
    {
        return (RSimulationResult)invoke(0, "execute_tracking_advanced", arg, typeof(RSimulationResult));
    }

    [ApiFunction]
    public bool export_bearing_contours(ABearingContoursExport arg)
    {
        return null != invoke(0, "export_bearing_contours", arg, null);
    }

    [ApiFunction]
    public bool export_field_isolines(AExportFieldIsolines arg)
    {
        return null != invoke(0, "export_field_isolines", arg, null);
    }

    [ApiFunction]
    public bool export_field_isosurface(AExportFieldIsosurface arg)
    {
        return null != invoke(0, "export_field_isosurface", arg, null);
    }

    [ApiFunction]
    public bool export_fields_at_tracking_points(AExportFieldsAtTrackingPoints arg)
    {
        return null != invoke(0, "export_fields_at_tracking_points", arg, null);
    }

    [ApiFunction]
    public bool export_mesh(AMeshExport arg)
    {
        return null != invoke(0, "export_mesh", arg, null);
    }

    [ApiFunction]
    public bool export_profile_section(AProfileSectionExport arg)
    {
        return null != invoke(0, "export_profile_section", arg, null);
    }

    [ApiFunction]
    public bool export_records(ARecordsExport arg)
    {
        return null != invoke(0, "export_records", arg, null);
    }

    [ApiFunction]
    public bool export_screenshot(AExportImage arg)
    {
        return null != invoke(0, "export_screenshot", arg, null);
    }

    [ApiFunction]
    public bool export_section_contour(AExportSectionContour arg)
    {
        return null != invoke(0, "export_section_contour", arg, null);
    }

    [ApiFunction]
    public bool export_section_mesh(AExportSection arg)
    {
        return null != invoke(0, "export_section_mesh", arg, null);
    }

    [ApiFunction]
    public bool export_video(AExportImage arg)
    {
        return null != invoke(0, "export_video", arg, null);
    }

    [ApiFunction]
    public RBearingZ extrusion_bearings_z()
    {
        return (RBearingZ)invoke(0, "extrusion_bearings_z", null, typeof(RBearingZ));
    }

    [ApiFunction]
    public RRealValue extrusion_ports_section_z()
    {
        return (RRealValue)invoke(0, "extrusion_ports_section_z", null, typeof(RRealValue));
    }

    [ApiFunction]
    public RCount extrusion_trace_count()
    {
        return (RCount)invoke(0, "extrusion_trace_count", null, typeof(RCount));
    }

    [ApiFunction]
    public RTrace extrusion_trace_get(ATraceId arg)
    {
        return (RTrace)invoke(0, "extrusion_trace_get", arg, typeof(RTrace));
    }

    [ApiFunction]
    public RRealValue field_at_mesh_point(AFieldAtMeshPoint arg)
    {
        return (RRealValue)invoke(0, "field_at_mesh_point", arg, typeof(RRealValue));
    }

    [ApiFunction]
    public RVectorValue field_at_mesh_point_vector(AFieldAtMeshPoint arg)
    {
        return (RVectorValue)invoke(0, "field_at_mesh_point_vector", arg, typeof(RVectorValue));
    }

    [ApiFunction]
    public RRealValue field_at_point(AFieldAtPoint arg)
    {
        return (RRealValue)invoke(0, "field_at_point", arg, typeof(RRealValue));
    }

    [ApiFunction]
    public RVectorValue field_at_point_vector(AFieldAtPoint arg)
    {
        return (RVectorValue)invoke(0, "field_at_point_vector", arg, typeof(RVectorValue));
    }

    [ApiFunction]
    public RRealValues field_at_tracking_line(AFieldAtTrackingObject arg)
    {
        return (RRealValues)invoke(0, "field_at_tracking_line", arg, typeof(RRealValues));
    }

    [ApiFunction]
    public RVectorValues field_at_tracking_line_vector(AFieldAtTrackingObject arg)
    {
        return (RVectorValues)invoke(0, "field_at_tracking_line_vector", arg, typeof(RVectorValues));
    }

    [ApiFunction]
    public RRealValue field_at_tracking_point(AFieldAtTrackingObject arg)
    {
        return (RRealValue)invoke(0, "field_at_tracking_point", arg, typeof(RRealValue));
    }

    [ApiFunction]
    public RVectorValue field_at_tracking_point_vector(AFieldAtTrackingObject arg)
    {
        return (RVectorValue)invoke(0, "field_at_tracking_point_vector", arg, typeof(RVectorValue));
    }

    [ApiFunction]
    public RField field_get(AFieldAtMesh arg)
    {
        return (RField)invoke(0, "field_get", arg, typeof(RField));
    }

    [ApiFunction]
    public RVectorField field_get_vector(AFieldAtMesh arg)
    {
        return (RVectorField)invoke(0, "field_get_vector", arg, typeof(RVectorField));
    }

    [ApiFunction]
    public RIsolineList field_isolines(AFieldIsosurface arg)
    {
        return (RIsolineList)invoke(0, "field_isolines", arg, typeof(RIsolineList));
    }

    [ApiFunction]
    public RIsosurfaceList field_isosurface(AFieldIsosurface arg)
    {
        return (RIsosurfaceList)invoke(0, "field_isosurface", arg, typeof(RIsosurfaceList));
    }

    [ApiFunction]
    public RFieldMinMax field_min_max(AFieldAtMinMax arg)
    {
        return (RFieldMinMax)invoke(0, "field_min_max", arg, typeof(RFieldMinMax));
    }

    [ApiFunction]
    public RFieldMode field_mode_get()
    {
        return (RFieldMode)invoke(0, "field_mode_get", null, typeof(RFieldMode));
    }

    [ApiFunction]
    public bool field_mode_set(AFieldMode arg)
    {
        return null != invoke(0, "field_mode_set", arg, null);
    }

    [ApiFunction]
    public RFieldPalette field_palette_get()
    {
        return (RFieldPalette)invoke(0, "field_palette_get", null, typeof(RFieldPalette));
    }

    [ApiFunction]
    public bool field_palette_set(AFieldPalette arg)
    {
        return null != invoke(0, "field_palette_set", arg, null);
    }

    [ApiFunction]
    public RFieldStat field_stat(AFieldStatAtMesh arg)
    {
        return (RFieldStat)invoke(0, "field_stat", arg, typeof(RFieldStat));
    }

    [ApiFunction]
    public RFieldStat field_stat_at_section(AFieldStatAtSection arg)
    {
        return (RFieldStat)invoke(0, "field_stat_at_section", arg, typeof(RFieldStat));
    }

    [ApiFunction]
    public RStringList file_dialog(AFileDlg arg)
    {
        return (RStringList)invoke(0, "file_dialog", arg, typeof(RStringList));
    }

    [ApiFunction]
    public bool geometry_convert_to_3d(AConvertTo3d arg)
    {
        return null != invoke(0, "geometry_convert_to_3d", arg, null);
    }

    [ApiFunction]
    public bool geometry_create_brick(ABrickObjectParams arg)
    {
        return null != invoke(0, "geometry_create_brick", arg, null);
    }

    [ApiFunction]
    public bool geometry_create_rect(ARectObjectParams arg)
    {
        return null != invoke(0, "geometry_create_rect", arg, null);
    }

    [ApiFunction]
    public bool geometry_create_sphere(ASphereObjectParams arg)
    {
        return null != invoke(0, "geometry_create_sphere", arg, null);
    }

    [ApiFunction]
    public bool geometry_create_tube(ATubeObjectParams arg)
    {
        return null != invoke(0, "geometry_create_tube", arg, null);
    }

    [ApiFunction]
    public bool geometry_generate_quad_mesh(AQuadMeshParams arg)
    {
        return null != invoke(0, "geometry_generate_quad_mesh", arg, null);
    }

    [ApiFunction]
    public RObjectList geometry_load(AFileName arg)
    {
        return (RObjectList)invoke(0, "geometry_load", arg, typeof(RObjectList));
    }

    [ApiFunction]
    public bool geometry_load_extruded_object(AExtrudedObject arg)
    {
        return null != invoke(0, "geometry_load_extruded_object", arg, null);
    }

    [ApiFunction]
    public bool geometry_load_revolved_object(ARevolvedObject arg)
    {
        return null != invoke(0, "geometry_load_revolved_object", arg, null);
    }

    [ApiFunction]
    public bool geometry_load_single_object(AFileObject arg)
    {
        return null != invoke(0, "geometry_load_single_object", arg, null);
    }

    [ApiFunction]
    public bool gravity_positioning(AGravityPositioning arg)
    {
        return null != invoke(0, "gravity_positioning", arg, null);
    }

    [ApiFunction]
    public RCount heating_cycle_count_get()
    {
        return (RCount)invoke(0, "heating_cycle_count_get", null, typeof(RCount));
    }

    [ApiFunction]
    public bool heating_cycle_count_set(ACount arg)
    {
        return null != invoke(0, "heating_cycle_count_set", arg, null);
    }

    [ApiFunction]
    public RItemId heating_cycle_get_current()
    {
        return (RItemId)invoke(0, "heating_cycle_get_current", null, typeof(RItemId));
    }

    [ApiFunction]
    public bool heating_cycle_set_current(AItemId arg)
    {
        return null != invoke(0, "heating_cycle_set_current", arg, null);
    }

    [ApiFunction]
    public RBoolValue is_windowless_mode()
    {
        return (RBoolValue)invoke(0, "is_windowless_mode", null, typeof(RBoolValue));
    }

    [ApiFunction]
    public RKeyNames key_names_get()
    {
        return (RKeyNames)invoke(0, "key_names_get", null, typeof(RKeyNames));
    }

    [ApiFunction]
    public bool key_send(ASendKey arg)
    {
        return null != invoke(0, "key_send", arg, null);
    }

    [ApiFunction]
    public RQFormLang language_get()
    {
        return (RQFormLang)invoke(0, "language_get", null, typeof(RQFormLang));
    }

    [ApiFunction]
    public bool language_set(AQFormLang arg)
    {
        return null != invoke(0, "language_set", arg, null);
    }

    [ApiFunction]
    public bool log_begin(ALogParams arg)
    {
        return null != invoke(0, "log_begin", arg, null);
    }

    [ApiFunction]
    public bool log_save(ALogFile arg)
    {
        return null != invoke(0, "log_save", arg, null);
    }

    [ApiFunction]
    public RMeshApex mesh_apex_get(AMeshApexId arg)
    {
        return (RMeshApex)invoke(0, "mesh_apex_get", arg, typeof(RMeshApex));
    }

    [ApiFunction]
    public RMeshCubics mesh_cubics_get(AMeshObjectId arg)
    {
        return (RMeshCubics)invoke(0, "mesh_cubics_get", arg, typeof(RMeshCubics));
    }

    [ApiFunction]
    public RMeshEdge mesh_edge_get(AMeshEdgeId arg)
    {
        return (RMeshEdge)invoke(0, "mesh_edge_get", arg, typeof(RMeshEdge));
    }

    [ApiFunction]
    public RMeshFace mesh_face_get(AMeshFaceId arg)
    {
        return (RMeshFace)invoke(0, "mesh_face_get", arg, typeof(RMeshFace));
    }

    [ApiFunction]
    public RFaceTypes mesh_face_types_get(AMeshObjectId arg)
    {
        return (RFaceTypes)invoke(0, "mesh_face_types_get", arg, typeof(RFaceTypes));
    }

    [ApiFunction]
    public RMeshCoords mesh_lap_points_get(AMeshObjectId arg)
    {
        return (RMeshCoords)invoke(0, "mesh_lap_points_get", arg, typeof(RMeshCoords));
    }

    [ApiFunction]
    public RMeshNodeOwners mesh_node_owners_get(AMeshObjectId arg)
    {
        return (RMeshNodeOwners)invoke(0, "mesh_node_owners_get", arg, typeof(RMeshNodeOwners));
    }

    [ApiFunction]
    public RMeshCoords mesh_nodes_get(AMeshObjectId arg)
    {
        return (RMeshCoords)invoke(0, "mesh_nodes_get", arg, typeof(RMeshCoords));
    }

    [ApiFunction]
    public RMeshPoint mesh_point_get(AObjectPoint arg)
    {
        return (RMeshPoint)invoke(0, "mesh_point_get", arg, typeof(RMeshPoint));
    }

    [ApiFunction]
    public RMeshProperties mesh_properties_get(AMeshObjectId arg)
    {
        return (RMeshProperties)invoke(0, "mesh_properties_get", arg, typeof(RMeshProperties));
    }

    [ApiFunction]
    public RMeshQuadrangles mesh_quadrangles_get(AMeshObjectId arg)
    {
        return (RMeshQuadrangles)invoke(0, "mesh_quadrangles_get", arg, typeof(RMeshQuadrangles));
    }

    [ApiFunction]
    public RMeshTetrahedrons mesh_thetrahedrons_get(AMeshObjectId arg)
    {
        return (RMeshTetrahedrons)invoke(0, "mesh_thetrahedrons_get", arg, typeof(RMeshTetrahedrons));
    }

    [ApiFunction]
    public RMeshTriangles mesh_triangles_get(AMeshObjectId arg)
    {
        return (RMeshTriangles)invoke(0, "mesh_triangles_get", arg, typeof(RMeshTriangles));
    }

    [ApiFunction]
    public bool mouse_click(AMouseClick arg)
    {
        return null != invoke(0, "mouse_click", arg, null);
    }

    [ApiFunction]
    public RMouseClick mouse_click_capture()
    {
        return (RMouseClick)invoke(0, "mouse_click_capture", null, typeof(RMouseClick));
    }

    [ApiFunction]
    public RMousePos mouse_pos_get()
    {
        return (RMousePos)invoke(0, "mouse_pos_get", null, typeof(RMousePos));
    }

    [ApiFunction]
    public bool mouse_pos_set(AMousePos arg)
    {
        return null != invoke(0, "mouse_pos_set", arg, null);
    }

    [ApiFunction]
    public RPressedDialogButton msg_box(AMsgBox arg)
    {
        return (RPressedDialogButton)invoke(0, "msg_box", arg, typeof(RPressedDialogButton));
    }

    [ApiFunction]
    public RMultiThreadingSettings multithreading_settings_get()
    {
        return (RMultiThreadingSettings)invoke(0, "multithreading_settings_get", null, typeof(RMultiThreadingSettings));
    }

    [ApiFunction]
    public bool multithreading_settings_set(AMultiThreadingSettings arg)
    {
        return null != invoke(0, "multithreading_settings_set", arg, null);
    }

    [ApiFunction]
    public bool object_apply_transform(AObjectTransform arg)
    {
        return null != invoke(0, "object_apply_transform", arg, null);
    }

    [ApiFunction]
    public RItemList object_axes_get(AObjectId arg)
    {
        return (RItemList)invoke(0, "object_axes_get", arg, typeof(RItemList));
    }

    [ApiFunction]
    public RItemList object_bound_conds_get(AObjectId arg)
    {
        return (RItemList)invoke(0, "object_bound_conds_get", arg, typeof(RItemList));
    }

    [ApiFunction]
    public RRealValue object_contact(AObjectContact arg)
    {
        return (RRealValue)invoke(0, "object_contact", arg, typeof(RRealValue));
    }

    [ApiFunction]
    public bool object_copy(AObjectConvert arg)
    {
        return null != invoke(0, "object_copy", arg, null);
    }

    [ApiFunction]
    public bool object_delete(AObjectId arg)
    {
        return null != invoke(0, "object_delete", arg, null);
    }

    [ApiFunction]
    public RBoolValue object_display_mode_get(GDisplayMode arg)
    {
        return (RBoolValue)invoke(0, "object_display_mode_get", arg, typeof(RBoolValue));
    }

    [ApiFunction]
    public bool object_display_mode_set(ADisplayMode arg)
    {
        return null != invoke(0, "object_display_mode_set", arg, null);
    }

    [ApiFunction]
    public RObjectName object_displayed_name(AObjectId arg)
    {
        return (RObjectName)invoke(0, "object_displayed_name", arg, typeof(RObjectName));
    }

    [ApiFunction]
    public RItemList object_domains_get(AObjectId arg)
    {
        return (RItemList)invoke(0, "object_domains_get", arg, typeof(RItemList));
    }

    [ApiFunction]
    public RBoolValue object_exists(AObjectId arg)
    {
        return (RBoolValue)invoke(0, "object_exists", arg, typeof(RBoolValue));
    }

    [ApiFunction]
    public RObjectId object_find_by_color(AFindByColor arg)
    {
        return (RObjectId)invoke(0, "object_find_by_color", arg, typeof(RObjectId));
    }

    [ApiFunction]
    public RObjectId object_find_by_surface_point(AFindByPoint arg)
    {
        return (RObjectId)invoke(0, "object_find_by_surface_point", arg, typeof(RObjectId));
    }

    [ApiFunction]
    public RObjectTransform object_get_transform(AObjectId arg)
    {
        return (RObjectTransform)invoke(0, "object_get_transform", arg, typeof(RObjectTransform));
    }

    [ApiFunction]
    public bool object_inherit(AObjectId arg)
    {
        return null != invoke(0, "object_inherit", arg, null);
    }

    [ApiFunction]
    public RBoolValue object_is_inherited(AObjectId arg)
    {
        return (RBoolValue)invoke(0, "object_is_inherited", arg, typeof(RBoolValue));
    }

    [ApiFunction]
    public bool object_move(AObjectMove arg)
    {
        return null != invoke(0, "object_move", arg, null);
    }

    [ApiFunction]
    public bool object_move_along_axis(AObjectMoveAxis arg)
    {
        return null != invoke(0, "object_move_along_axis", arg, null);
    }

    [ApiFunction]
    public bool object_rotate(AObjectRotate arg)
    {
        return null != invoke(0, "object_rotate", arg, null);
    }

    [ApiFunction]
    public bool object_rotate_around_axis(AObjectRotateAxis arg)
    {
        return null != invoke(0, "object_rotate_around_axis", arg, null);
    }

    [ApiFunction]
    public bool object_set_type_by_color(ATypeSetByColor arg)
    {
        return null != invoke(0, "object_set_type_by_color", arg, null);
    }

    [ApiFunction]
    public bool object_set_type_by_surface_point(ATypeSetByPoint arg)
    {
        return null != invoke(0, "object_set_type_by_surface_point", arg, null);
    }

    [ApiFunction]
    public bool object_type_set(AObjectConvert arg)
    {
        return null != invoke(0, "object_type_set", arg, null);
    }

    [ApiFunction]
    public bool object_type_set_in_direction(AObjectsInDirection arg)
    {
        return null != invoke(0, "object_type_set_in_direction", arg, null);
    }

    [ApiFunction]
    public RObjectList objects_find_by_color(AFindByColor arg)
    {
        return (RObjectList)invoke(0, "objects_find_by_color", arg, typeof(RObjectList));
    }

    [ApiFunction]
    public RObjectList objects_get_in_direction(APickDirection arg)
    {
        return (RObjectList)invoke(0, "objects_get_in_direction", arg, typeof(RObjectList));
    }

    [ApiFunction]
    public bool on_disconnect(AOnDisconnect arg)
    {
        return null != invoke(0, "on_disconnect", arg, null);
    }

    [ApiFunction]
    public RItemList operation_chains_get(AItemId arg)
    {
        return (RItemList)invoke(0, "operation_chains_get", arg, typeof(RItemList));
    }

    [ApiFunction]
    public ROperationChecks operation_check(AOptionalItemId arg)
    {
        return (ROperationChecks)invoke(0, "operation_check", arg, typeof(ROperationChecks));
    }

    [ApiFunction]
    public RItemId operation_copy(AOperationCopy arg)
    {
        return (RItemId)invoke(0, "operation_copy", arg, typeof(RItemId));
    }

    [ApiFunction]
    public bool operation_copy_from_parent(AOperationCopyFromParent arg)
    {
        return null != invoke(0, "operation_copy_from_parent", arg, null);
    }

    [ApiFunction]
    public RItemId operation_create(AOperationParams arg)
    {
        return (RItemId)invoke(0, "operation_create", arg, typeof(RItemId));
    }

    [ApiFunction]
    public bool operation_cut(AItemId arg)
    {
        return null != invoke(0, "operation_cut", arg, null);
    }

    [ApiFunction]
    public bool operation_delete(AItemId arg)
    {
        return null != invoke(0, "operation_delete", arg, null);
    }

    [ApiFunction]
    public RBoolValue operation_exists(AItemId arg)
    {
        return (RBoolValue)invoke(0, "operation_exists", arg, typeof(RBoolValue));
    }

    [ApiFunction]
    public ROperation operation_get(AItemId arg)
    {
        return (ROperation)invoke(0, "operation_get", arg, typeof(ROperation));
    }

    [ApiFunction]
    public RItemId operation_get_by_uid(AItemId arg)
    {
        return (RItemId)invoke(0, "operation_get_by_uid", arg, typeof(RItemId));
    }

    [ApiFunction]
    public RItemId operation_get_current()
    {
        return (RItemId)invoke(0, "operation_get_current", null, typeof(RItemId));
    }

    [ApiFunction]
    public ROperationGraph operation_graph_get(AItemId arg)
    {
        return (ROperationGraph)invoke(0, "operation_graph_get", arg, typeof(ROperationGraph));
    }

    [ApiFunction]
    public RItemId operation_insert(AOperationInsert arg)
    {
        return (RItemId)invoke(0, "operation_insert", arg, typeof(RItemId));
    }

    [ApiFunction]
    public RItemId operation_process_get(AItemId arg)
    {
        return (RItemId)invoke(0, "operation_process_get", arg, typeof(RItemId));
    }

    [ApiFunction]
    public bool operation_set_current(AItemId arg)
    {
        return null != invoke(0, "operation_set_current", arg, null);
    }

    [ApiFunction]
    public RItemId operation_set_first_in_chain()
    {
        return (RItemId)invoke(0, "operation_set_first_in_chain", null, typeof(RItemId));
    }

    [ApiFunction]
    public RItemId operation_set_last_in_chain()
    {
        return (RItemId)invoke(0, "operation_set_last_in_chain", null, typeof(RItemId));
    }

    [ApiFunction]
    public RItemId operation_set_next_in_chain()
    {
        return (RItemId)invoke(0, "operation_set_next_in_chain", null, typeof(RItemId));
    }

    [ApiFunction]
    public RItemId operation_set_prev_in_chain()
    {
        return (RItemId)invoke(0, "operation_set_prev_in_chain", null, typeof(RItemId));
    }

    [ApiFunction]
    public bool operation_template_set(ADbObjectPath arg)
    {
        return null != invoke(0, "operation_template_set", arg, null);
    }

    [ApiFunction]
    public ROperation operation_tree()
    {
        return (ROperation)invoke(0, "operation_tree", null, typeof(ROperation));
    }

    [ApiFunction]
    public RItemId operation_uid(AOptionalItemId arg)
    {
        return (RItemId)invoke(0, "operation_uid", arg, typeof(RItemId));
    }

    [ApiFunction]
    public RPanelPositions panel_positions_get()
    {
        return (RPanelPositions)invoke(0, "panel_positions_get", null, typeof(RPanelPositions));
    }

    [ApiFunction]
    public RPanelSizes panel_sizes_get()
    {
        return (RPanelSizes)invoke(0, "panel_sizes_get", null, typeof(RPanelSizes));
    }

    [ApiFunction]
    public bool panel_sizes_set(APanelSizes arg)
    {
        return null != invoke(0, "panel_sizes_set", arg, null);
    }

    [ApiFunction]
    public bool print(ATraceMsg arg)
    {
        return null != invoke(0, "print", arg, null);
    }

    [ApiFunction]
    public RItemId process_chain_get_current()
    {
        return (RItemId)invoke(0, "process_chain_get_current", null, typeof(RItemId));
    }

    [ApiFunction]
    public RItemList process_chain_get_current_operations()
    {
        return (RItemList)invoke(0, "process_chain_get_current_operations", null, typeof(RItemList));
    }

    [ApiFunction]
    public RItemList process_chain_get_operations(AItemId arg)
    {
        return (RItemList)invoke(0, "process_chain_get_operations", arg, typeof(RItemList));
    }

    [ApiFunction]
    public bool process_chain_set_current(AItemId arg)
    {
        return null != invoke(0, "process_chain_set_current", arg, null);
    }

    [ApiFunction]
    public RItemList processes_get()
    {
        return (RItemList)invoke(0, "processes_get", null, typeof(RItemList));
    }

    [ApiFunction]
    public RBoolValue project_ask_save()
    {
        return (RBoolValue)invoke(0, "project_ask_save", null, typeof(RBoolValue));
    }

    [ApiFunction]
    public bool project_export_as_script(AExportAsScriptOptions arg)
    {
        return null != invoke(0, "project_export_as_script", arg, null);
    }

    [ApiFunction]
    public RFileName project_file_get()
    {
        return (RFileName)invoke(0, "project_file_get", null, typeof(RFileName));
    }

    [ApiFunction]
    public bool project_new()
    {
        return null != invoke(0, "project_new", null, null);
    }

    [ApiFunction]
    public bool project_open(AFileName arg)
    {
        return null != invoke(0, "project_open", arg, null);
    }

    [ApiFunction]
    public bool project_open_as_copy(AProjectOpenAsCopy arg)
    {
        return null != invoke(0, "project_open_as_copy", arg, null);
    }

    [ApiFunction]
    public bool project_open_or_create(AFileName arg)
    {
        return null != invoke(0, "project_open_or_create", arg, null);
    }

    [ApiFunction]
    public RFileName project_path_get()
    {
        return (RFileName)invoke(0, "project_path_get", null, typeof(RFileName));
    }

    [ApiFunction]
    public bool project_save()
    {
        return null != invoke(0, "project_save", null, null);
    }

    [ApiFunction]
    public bool project_save_as(AFileName arg)
    {
        return null != invoke(0, "project_save_as", arg, null);
    }

    [ApiFunction]
    public bool project_save_as_template(APathName arg)
    {
        return null != invoke(0, "project_save_as_template", arg, null);
    }

    [ApiFunction]
    public RPropertyValue property_get(GProperty arg)
    {
        return (RPropertyValue)invoke(0, "property_get", arg, typeof(RPropertyValue));
    }

    [ApiFunction]
    public RArrayOfReal property_get_array_of_real(GPropertyPath arg)
    {
        return (RArrayOfReal)invoke(0, "property_get_array_of_real", arg, typeof(RArrayOfReal));
    }

    [ApiFunction]
    public RObjectId property_get_object(GPropertyPath arg)
    {
        return (RObjectId)invoke(0, "property_get_object", arg, typeof(RObjectId));
    }

    [ApiFunction]
    public bool property_set(AProperty arg)
    {
        return null != invoke(0, "property_set", arg, null);
    }

    [ApiFunction]
    public bool property_set_array_of_real(APropertyArrayOfReal arg)
    {
        return null != invoke(0, "property_set_array_of_real", arg, null);
    }

    [ApiFunction]
    public bool property_set_object(AObjectIdProperty arg)
    {
        return null != invoke(0, "property_set_object", arg, null);
    }

    [ApiFunction]
    public bool qform_attach_to(ASessionId arg)
    {
        return null != invoke(-6, "qform_attach_to", arg, null);
    }

    [ApiFunction]
    public RProcessId qform_process_id()
    {
        return (RProcessId)invoke(0, "qform_process_id", null, typeof(RProcessId));
    }

    [ApiFunction]
    public bool qform_reconnect()
    {
        return null != invoke(-4, "qform_reconnect", null, null);
    }

    [ApiFunction]
    public RQFormVer qform_version()
    {
        return (RQFormVer)invoke(0, "qform_version", null, typeof(RQFormVer));
    }

    [ApiFunction]
    public RWindowId qform_window_id()
    {
        return (RWindowId)invoke(0, "qform_window_id", null, typeof(RWindowId));
    }

    [ApiFunction]
    public RWindowPosition qform_window_pos_get()
    {
        return (RWindowPosition)invoke(0, "qform_window_pos_get", null, typeof(RWindowPosition));
    }

    [ApiFunction]
    public bool qform_window_pos_set(AWindowPosition arg)
    {
        return null != invoke(0, "qform_window_pos_set", arg, null);
    }

    [ApiFunction]
    public RRecord record_get()
    {
        return (RRecord)invoke(0, "record_get", null, typeof(RRecord));
    }

    [ApiFunction]
    public RRecord record_get_last()
    {
        return (RRecord)invoke(0, "record_get_last", null, typeof(RRecord));
    }

    [ApiFunction]
    public bool record_set(ARecord arg)
    {
        return null != invoke(0, "record_set", arg, null);
    }

    [ApiFunction]
    public bool results_truncate(ARecord arg)
    {
        return null != invoke(0, "results_truncate", arg, null);
    }

    [ApiFunction]
    public RSectionMeshList section_mesh_get(ASectionMeshPlane arg)
    {
        return (RSectionMeshList)invoke(0, "section_mesh_get", arg, typeof(RSectionMeshList));
    }

    [ApiFunction]
    public RItemId section_plane_create_3p(ASectionPlane3P arg)
    {
        return (RItemId)invoke(0, "section_plane_create_3p", arg, typeof(RItemId));
    }

    [ApiFunction]
    public RItemId section_plane_create_pn(ASectionPlanePN arg)
    {
        return (RItemId)invoke(0, "section_plane_create_pn", arg, typeof(RItemId));
    }

    [ApiFunction]
    public bool section_plane_delete(AItemId arg)
    {
        return null != invoke(0, "section_plane_delete", arg, null);
    }

    [ApiFunction]
    public bool section_plane_enable(ASectionPlaneEnabled arg)
    {
        return null != invoke(0, "section_plane_enable", arg, null);
    }

    [ApiFunction]
    public RSectionPlane section_plane_get(AItemId arg)
    {
        return (RSectionPlane)invoke(0, "section_plane_get", arg, typeof(RSectionPlane));
    }

    [ApiFunction]
    public bool section_plane_show(ASectionPlaneVisibility arg)
    {
        return null != invoke(0, "section_plane_show", arg, null);
    }

    [ApiFunction]
    public RItemList section_planes_get()
    {
        return (RItemList)invoke(0, "section_planes_get", null, typeof(RItemList));
    }

    [ApiFunction]
    public RObjectId selected_object_get()
    {
        return (RObjectId)invoke(0, "selected_object_get", null, typeof(RObjectId));
    }

    [ApiFunction]
    public RItemId session_id()
    {
        return (RItemId)invoke(0, "session_id", null, typeof(RItemId));
    }

    [ApiFunction]
    public RSession session_info()
    {
        return (RSession)invoke(0, "session_info", null, typeof(RSession));
    }

    [ApiFunction]
    public RSession session_info_by_id(ASessionId arg)
    {
        return (RSession)invoke(-7, "session_info_by_id", arg, typeof(RSession));
    }

    [ApiFunction]
    public RSessionList session_list()
    {
        return (RSessionList)invoke(-5, "session_list", null, typeof(RSessionList));
    }

    [ApiFunction]
    public RCount session_max_count()
    {
        return (RCount)invoke(0, "session_max_count", null, typeof(RCount));
    }

    [ApiFunction]
    public RObjectList simulation_objects_get()
    {
        return (RObjectList)invoke(0, "simulation_objects_get", null, typeof(RObjectList));
    }

    [ApiFunction]
    public bool sleep(ASleepTime arg)
    {
        return null != invoke(0, "sleep", arg, null);
    }

    [ApiFunction]
    public RItemId spring_between_tools_create(AOptionalItemId arg)
    {
        return (RItemId)invoke(0, "spring_between_tools_create", arg, typeof(RItemId));
    }

    [ApiFunction]
    public bool spring_between_tools_delete(AItemId arg)
    {
        return null != invoke(0, "spring_between_tools_delete", arg, null);
    }

    [ApiFunction]
    public RItemList springs_between_tools_get()
    {
        return (RItemList)invoke(0, "springs_between_tools_get", null, typeof(RItemList));
    }

    [ApiFunction]
    public RMainSimulationResult start_simulation()
    {
        return (RMainSimulationResult)invoke(0, "start_simulation", null, typeof(RMainSimulationResult));
    }

    [ApiFunction]
    public RMainSimulationResult start_simulation_advanced(ASimulationParams arg)
    {
        return (RMainSimulationResult)invoke(0, "start_simulation_advanced", arg, typeof(RMainSimulationResult));
    }

    [ApiFunction]
    public RSimulationState state_blow(AOptionalGlobalItemId arg)
    {
        return (RSimulationState)invoke(0, "state_blow", arg, typeof(RSimulationState));
    }

    [ApiFunction]
    public RExtrusionState state_extrusion(ASystemStateId arg)
    {
        return (RExtrusionState)invoke(0, "state_extrusion", arg, typeof(RExtrusionState));
    }

    [ApiFunction]
    public RMeshState state_mesh(AMeshStateId arg)
    {
        return (RMeshState)invoke(0, "state_mesh", arg, typeof(RMeshState));
    }

    [ApiFunction]
    public RSimulationState state_operation(AOptionalItemId arg)
    {
        return (RSimulationState)invoke(0, "state_operation", arg, typeof(RSimulationState));
    }

    [ApiFunction]
    public RSimulationState state_process_chain(AOptionalItemId arg)
    {
        return (RSimulationState)invoke(0, "state_process_chain", arg, typeof(RSimulationState));
    }

    [ApiFunction]
    public RSystemState state_system(ASystemStateId arg)
    {
        return (RSystemState)invoke(0, "state_system", arg, typeof(RSystemState));
    }

    [ApiFunction]
    public RToolState state_tool(AToolStateId arg)
    {
        return (RToolState)invoke(0, "state_tool", arg, typeof(RToolState));
    }

    [ApiFunction]
    public RWorkpieceState state_workpiece(AWorkpieceStateId arg)
    {
        return (RWorkpieceState)invoke(0, "state_workpiece", arg, typeof(RWorkpieceState));
    }

    [ApiFunction]
    public RItemId stop_cond_create(AStopCondParams arg)
    {
        return (RItemId)invoke(0, "stop_cond_create", arg, typeof(RItemId));
    }

    [ApiFunction]
    public RItemId stop_cond_create_distance(AStopCondDistance arg)
    {
        return (RItemId)invoke(0, "stop_cond_create_distance", arg, typeof(RItemId));
    }

    [ApiFunction]
    public RItemId stop_cond_create_final_pos(AStopCondFinPos arg)
    {
        return (RItemId)invoke(0, "stop_cond_create_final_pos", arg, typeof(RItemId));
    }

    [ApiFunction]
    public RItemId stop_cond_create_max_load(AStopCondMaxLoad arg)
    {
        return (RItemId)invoke(0, "stop_cond_create_max_load", arg, typeof(RItemId));
    }

    [ApiFunction]
    public RItemId stop_cond_create_profile_length(AStopCondProfileLength arg)
    {
        return (RItemId)invoke(0, "stop_cond_create_profile_length", arg, typeof(RItemId));
    }

    [ApiFunction]
    public RItemId stop_cond_create_rotation(AStopCondRotation arg)
    {
        return (RItemId)invoke(0, "stop_cond_create_rotation", arg, typeof(RItemId));
    }

    [ApiFunction]
    public RItemId stop_cond_create_stroke(AStopCondStroke arg)
    {
        return (RItemId)invoke(0, "stop_cond_create_stroke", arg, typeof(RItemId));
    }

    [ApiFunction]
    public RItemId stop_cond_create_time(AStopCondTime arg)
    {
        return (RItemId)invoke(0, "stop_cond_create_time", arg, typeof(RItemId));
    }

    [ApiFunction]
    public bool stop_cond_delete(AItemId arg)
    {
        return null != invoke(0, "stop_cond_delete", arg, null);
    }

    [ApiFunction]
    public RStopCond stop_cond_type(AItemId arg)
    {
        return (RStopCond)invoke(0, "stop_cond_type", arg, typeof(RStopCond));
    }

    [ApiFunction]
    public RItemId stop_condition_create_field_value(AStopCondFieldValue arg)
    {
        return (RItemId)invoke(0, "stop_condition_create_field_value", arg, typeof(RItemId));
    }

    [ApiFunction]
    public RItemList stop_conds_get()
    {
        return (RItemList)invoke(0, "stop_conds_get", null, typeof(RItemList));
    }

    [ApiFunction]
    public RItemId subroutine_create(ASubroutineCreate arg)
    {
        return (RItemId)invoke(0, "subroutine_create", arg, typeof(RItemId));
    }

    [ApiFunction]
    public bool subroutine_delete(AItemId arg)
    {
        return null != invoke(0, "subroutine_delete", arg, null);
    }

    [ApiFunction]
    public RSubroutine subroutine_get(AItemId arg)
    {
        return (RSubroutine)invoke(0, "subroutine_get", arg, typeof(RSubroutine));
    }

    [ApiFunction]
    public RNullableRealValue subroutine_parameter_get(GSubroutineParameter arg)
    {
        return (RNullableRealValue)invoke(0, "subroutine_parameter_get", arg, typeof(RNullableRealValue));
    }

    [ApiFunction]
    public bool subroutine_parameter_set(ASubroutineParameter arg)
    {
        return null != invoke(0, "subroutine_parameter_set", arg, null);
    }

    [ApiFunction]
    public RItemList subroutines_get()
    {
        return (RItemList)invoke(0, "subroutines_get", null, typeof(RItemList));
    }

    [ApiFunction]
    public RItemId sym_plane_create(ASymPlaneParams arg)
    {
        return (RItemId)invoke(0, "sym_plane_create", arg, typeof(RItemId));
    }

    [ApiFunction]
    public RItemId sym_plane_create_by_close_point(ASymPlaneByPoint arg)
    {
        return (RItemId)invoke(0, "sym_plane_create_by_close_point", arg, typeof(RItemId));
    }

    [ApiFunction]
    public RItemId sym_plane_create_by_face_color(ASymPlaneByColor arg)
    {
        return (RItemId)invoke(0, "sym_plane_create_by_face_color", arg, typeof(RItemId));
    }

    [ApiFunction]
    public bool sym_plane_delete(AItemId arg)
    {
        return null != invoke(0, "sym_plane_delete", arg, null);
    }

    [ApiFunction]
    public RUnitVector sym_plane_get(AItemId arg)
    {
        return (RUnitVector)invoke(0, "sym_plane_get", arg, typeof(RUnitVector));
    }

    [ApiFunction]
    public RItemList sym_planes_create_auto()
    {
        return (RItemList)invoke(0, "sym_planes_create_auto", null, typeof(RItemList));
    }

    [ApiFunction]
    public RItemList sym_planes_get()
    {
        return (RItemList)invoke(0, "sym_planes_get", null, typeof(RItemList));
    }

    [ApiFunction]
    public RUnitSystem system_of_units_get()
    {
        return (RUnitSystem)invoke(0, "system_of_units_get", null, typeof(RUnitSystem));
    }

    [ApiFunction]
    public bool system_of_units_set(AUnitSystem arg)
    {
        return null != invoke(0, "system_of_units_set", arg, null);
    }

    [ApiFunction]
    public RItemId tracking_contour_create(ANewTrackingContour arg)
    {
        return (RItemId)invoke(0, "tracking_contour_create", arg, typeof(RItemId));
    }

    [ApiFunction]
    public RItemId tracking_contours_create(AOptionalObjectItemId arg)
    {
        return (RItemId)invoke(0, "tracking_contours_create", arg, typeof(RItemId));
    }

    [ApiFunction]
    public RItemId tracking_group_create(ATrackingGroup arg)
    {
        return (RItemId)invoke(0, "tracking_group_create", arg, typeof(RItemId));
    }

    [ApiFunction]
    public RItemId tracking_line_create(ATrackingLineParams arg)
    {
        return (RItemId)invoke(0, "tracking_line_create", arg, typeof(RItemId));
    }

    [ApiFunction]
    public RTrackingLine tracking_line_get(AGlobalItemId arg)
    {
        return (RTrackingLine)invoke(0, "tracking_line_get", arg, typeof(RTrackingLine));
    }

    [ApiFunction]
    public RItemList tracking_lines_get()
    {
        return (RItemList)invoke(0, "tracking_lines_get", null, typeof(RItemList));
    }

    [ApiFunction]
    public RGlobalItemList tracking_lines_get_for_chain()
    {
        return (RGlobalItemList)invoke(0, "tracking_lines_get_for_chain", null, typeof(RGlobalItemList));
    }

    [ApiFunction]
    public RItemId tracking_point_create(ATrackingPointParams arg)
    {
        return (RItemId)invoke(0, "tracking_point_create", arg, typeof(RItemId));
    }

    [ApiFunction]
    public RMeshPoint tracking_point_get(AGlobalItemId arg)
    {
        return (RMeshPoint)invoke(0, "tracking_point_get", arg, typeof(RMeshPoint));
    }

    [ApiFunction]
    public RItemList tracking_points_get()
    {
        return (RItemList)invoke(0, "tracking_points_get", null, typeof(RItemList));
    }

    [ApiFunction]
    public RGlobalItemList tracking_points_get_for_chain()
    {
        return (RGlobalItemList)invoke(0, "tracking_points_get_for_chain", null, typeof(RGlobalItemList));
    }

    [ApiFunction]
    public bool view_back()
    {
        return null != invoke(0, "view_back", null, null);
    }

    [ApiFunction]
    public bool view_bottom()
    {
        return null != invoke(0, "view_bottom", null, null);
    }

    [ApiFunction]
    public bool view_front()
    {
        return null != invoke(0, "view_front", null, null);
    }

    [ApiFunction]
    public bool view_left()
    {
        return null != invoke(0, "view_left", null, null);
    }

    [ApiFunction]
    public bool view_on_bottom_90()
    {
        return null != invoke(0, "view_on_bottom_90", null, null);
    }

    [ApiFunction]
    public bool view_on_top_90()
    {
        return null != invoke(0, "view_on_top_90", null, null);
    }

    [ApiFunction]
    public RViewOverallDimensions view_overall_dimensions()
    {
        return (RViewOverallDimensions)invoke(0, "view_overall_dimensions", null, typeof(RViewOverallDimensions));
    }

    [ApiFunction]
    public bool view_right()
    {
        return null != invoke(0, "view_right", null, null);
    }

    [ApiFunction]
    public bool view_top()
    {
        return null != invoke(0, "view_top", null, null);
    }

    [ApiFunction]
    public RPathName work_dir_get()
    {
        return (RPathName)invoke(0, "work_dir_get", null, typeof(RPathName));
    }

    [ApiFunction]
    public bool work_dir_set(APathName arg)
    {
        return null != invoke(0, "work_dir_set", arg, null);
    }

    [ApiFunction]
    public bool zoom_to_fit()
    {
        return null != invoke(0, "zoom_to_fit", null, null);
    }

    [ApiFunction]
    public bool zoom_to_frame(AScale arg)
    {
        return null != invoke(0, "zoom_to_frame", arg, null);
    }

    ////end block
    public static void api_get(List<ApiEntry> lst)
    {
        lst.Add(new ApiEntry(0, "active_field_get", null, typeof(RFieldId), false, new LStr[] { new LStr("View", "View", "Вид") }));
        lst.Add(new ApiEntry(0, "active_field_reset", null, null, false, new LStr[] { new LStr("View", "View", "Вид") }));
        lst.Add(new ApiEntry(0, "active_field_set", typeof(AFieldId), null, true, new LStr[] { new LStr("View", "View", "Вид") }));
        lst.Add(new ApiEntry(0, "assembled_tool_create", typeof(AAssembledTool), typeof(RItemId), false, new LStr[] { new LStr("PageGeometry", "Geometry", "Геометрия") }));
        lst.Add(new ApiEntry(0, "assembled_tool_get", typeof(AItemId), typeof(RAssembledTool), false, new LStr[] { new LStr("PageGeometry", "Geometry", "Геометрия") }));
        lst.Add(new ApiEntry(0, "assembled_tool_split", typeof(AItemId), null, false, new LStr[] { new LStr("PageGeometry", "Geometry", "Геометрия") }));
        lst.Add(new ApiEntry(0, "assembled_tools_get", null, typeof(RItemList), false, new LStr[] { new LStr("PageGeometry", "Geometry", "Геометрия"), new LStr("Collections", "Collections", "Списки") }));
        lst.Add(new ApiEntry(0, "async_calculate_tools", typeof(AObjectList), typeof(RSimulationResult), false, new LStr[] { new LStr("PageSolve", "Simulation", "Расчет") }));
        lst.Add(new ApiEntry(0, "async_calculate_tools_coupled", null, typeof(RSimulationResult), false, new LStr[] { new LStr("PageSolve", "Simulation", "Расчет") }));
        lst.Add(new ApiEntry(0, "async_execute_subroutines", typeof(ASubroutineCalculationMode), null, false, new LStr[] { new LStr("OP_Scripts", "Subroutines", "Подпрограммы"), new LStr("PageSolve", "Simulation", "Расчет") }));
        lst.Add(new ApiEntry(0, "async_execute_tracking", typeof(ATrackingCalculationMode), null, false, new LStr[] { new LStr("PageSolve", "Simulation", "Расчет"), new LStr("TracingPoints", "Tracking points", "Трассируемые точки") }));
        lst.Add(new ApiEntry(0, "async_start_simulation", typeof(ASimulationParams), null, false, new LStr[] { new LStr("PageSolve", "Simulation", "Расчет") }));
        lst.Add(new ApiEntry(0, "async_state", null, typeof(RAsyncState), false, new LStr[] { new LStr("PageSolve", "Simulation", "Расчет") }));
        lst.Add(new ApiEntry(0, "async_stop_simulation", null, null, false, new LStr[] { new LStr("PageSolve", "Simulation", "Расчет") }));
        lst.Add(new ApiEntry(0, "async_wait", typeof(AAsyncWaitingParams), typeof(RAsyncEvent), false, new LStr[] { new LStr("PageSolve", "Simulation", "Расчет") }));
        lst.Add(new ApiEntry(0, "available_fields_get", typeof(AFieldGroupId), typeof(RFieldIdList), false, new LStr[] { new LStr("Fields", "Fields", "Поля") }));
        lst.Add(new ApiEntry(0, "axis_calculate", typeof(AObjectId), typeof(RObjectAxis), false, new LStr[] { new LStr("Axes", "Axes", "Оси") }));
        lst.Add(new ApiEntry(0, "axis_delete", typeof(AObjectAxisId), null, false, new LStr[] { new LStr("Axes", "Axes", "Оси") }));
        lst.Add(new ApiEntry(0, "axis_get", typeof(AObjectAxisId), typeof(RObjectAxis), false, new LStr[] { new LStr("Axes", "Axes", "Оси") }));
        lst.Add(new ApiEntry(0, "axis_inherit", typeof(AObjectAxisId), null, false, new LStr[] { new LStr("Axes", "Axes", "Оси") }));
        lst.Add(new ApiEntry(0, "axis_set", typeof(AObjectAxisParams), null, false, new LStr[] { new LStr("Axes", "Axes", "Оси") }));
        lst.Add(new ApiEntry(0, "axis_set_calculated", typeof(AObjectId), typeof(RObjectAxis), false, new LStr[] { new LStr("Axes", "Axes", "Оси") }));
        lst.Add(new ApiEntry(0, "billet_count_get", null, typeof(RCount), false, new LStr[] { new LStr("OP_Billet", "Billet", "Слиток") }));
        lst.Add(new ApiEntry(0, "billet_count_set", typeof(ACount), null, false, new LStr[] { new LStr("OP_Billet", "Billet", "Слиток") }));
        lst.Add(new ApiEntry(0, "billet_get_current", null, typeof(RItemId), false, new LStr[] { new LStr("OP_Billet", "Billet", "Слиток") }));
        lst.Add(new ApiEntry(0, "billet_parameter_get", typeof(GBilletParameter), typeof(RNullableRealValue), false, new LStr[] { new LStr("OP_Billet", "Billet", "Слиток"), new LStr("properties", "Properties", "Свойства") }));
        lst.Add(new ApiEntry(0, "billet_parameter_set", typeof(ABilletParameter), null, false, new LStr[] { new LStr("OP_Billet", "Billet", "Слиток"), new LStr("properties", "Properties", "Свойства") }));
        lst.Add(new ApiEntry(0, "billet_set_current", typeof(AItemId), null, false, new LStr[] { new LStr("OP_Billet", "Billet", "Слиток") }));
        lst.Add(new ApiEntry(0, "blow_count_get", null, typeof(RCount), false, new LStr[] { new LStr("OP_Blows", "Blows", "Удары") }));
        lst.Add(new ApiEntry(0, "blow_count_set", typeof(ACount), null, false, new LStr[] { new LStr("OP_Blows", "Blows", "Удары") }));
        lst.Add(new ApiEntry(0, "blow_get_current", null, typeof(RItemId), false, new LStr[] { new LStr("OP_Blows", "Blows", "Удары") }));
        lst.Add(new ApiEntry(0, "blow_parameter_get", typeof(GBlowParameter), typeof(RNullableRealValue), false, new LStr[] { new LStr("OP_Blows", "Blows", "Удары"), new LStr("properties", "Properties", "Свойства") }));
        lst.Add(new ApiEntry(0, "blow_parameter_set", typeof(ABlowParameter), null, false, new LStr[] { new LStr("OP_Blows", "Blows", "Удары"), new LStr("properties", "Properties", "Свойства") }));
        lst.Add(new ApiEntry(0, "blow_set_current", typeof(AItemId), null, false, new LStr[] { new LStr("OP_Blows", "Blows", "Удары") }));
        lst.Add(new ApiEntry(0, "bound_cond_create", typeof(ABoundCondParams), typeof(RItemId), false, new LStr[] { new LStr("Op_BConds", "Boundary conditions", "Граничные условия") }));
        lst.Add(new ApiEntry(0, "bound_cond_delete", typeof(AItemId), null, false, new LStr[] { new LStr("Op_BConds", "Boundary conditions", "Граничные условия") }));
        lst.Add(new ApiEntry(0, "bound_cond_set_body", typeof(AShapeBody), null, false, new LStr[] { new LStr("Op_BConds", "Boundary conditions", "Граничные условия") }));
        lst.Add(new ApiEntry(0, "bound_cond_set_brick", typeof(AShapeBrick), null, false, new LStr[] { new LStr("Op_BConds", "Boundary conditions", "Граничные условия") }));
        lst.Add(new ApiEntry(0, "bound_cond_set_circle", typeof(AShapeCircle), null, false, new LStr[] { new LStr("Op_BConds", "Boundary conditions", "Граничные условия") }));
        lst.Add(new ApiEntry(0, "bound_cond_set_cone", typeof(AShapeCone), null, false, new LStr[] { new LStr("Op_BConds", "Boundary conditions", "Граничные условия") }));
        lst.Add(new ApiEntry(0, "bound_cond_set_cylinder", typeof(AShapeCylinder), null, false, new LStr[] { new LStr("Op_BConds", "Boundary conditions", "Граничные условия") }));
        lst.Add(new ApiEntry(0, "bound_cond_set_rect", typeof(AShapeRect), null, false, new LStr[] { new LStr("Op_BConds", "Boundary conditions", "Граничные условия") }));
        lst.Add(new ApiEntry(0, "bound_cond_set_sphere", typeof(AShapeSphere), null, false, new LStr[] { new LStr("Op_BConds", "Boundary conditions", "Граничные условия") }));
        lst.Add(new ApiEntry(0, "bound_cond_set_sprayer_polar_array", typeof(AShapeSprayerPolarArray), null, false, new LStr[] { new LStr("Op_BConds", "Boundary conditions", "Граничные условия") }));
        lst.Add(new ApiEntry(0, "bound_cond_set_sprayer_polar_array_db", typeof(AShapeSprayerPolarArrayDB), null, false, new LStr[] { new LStr("Op_BConds", "Boundary conditions", "Граничные условия") }));
        lst.Add(new ApiEntry(0, "bound_cond_set_sprayer_rect_array", typeof(AShapeSprayerRectArray), null, false, new LStr[] { new LStr("Op_BConds", "Boundary conditions", "Граничные условия") }));
        lst.Add(new ApiEntry(0, "bound_cond_set_sprayer_rect_array_db", typeof(AShapeSprayerRectArrayDB), null, false, new LStr[] { new LStr("Op_BConds", "Boundary conditions", "Граничные условия") }));
        lst.Add(new ApiEntry(0, "bound_cond_set_surface_by_color", typeof(AShapeSurfaceByColor), null, false, new LStr[] { new LStr("Op_BConds", "Boundary conditions", "Граничные условия") }));
        lst.Add(new ApiEntry(0, "bound_cond_type", typeof(AItemId), typeof(RBoundCondType), false, new LStr[] { new LStr("Op_BConds", "Boundary conditions", "Граничные условия") }));
        lst.Add(new ApiEntry(0, "calculate_tools", typeof(AObjectList), typeof(RSimulationResult), false, new LStr[] { new LStr("PageSolve", "Simulation", "Расчет") }));
        lst.Add(new ApiEntry(0, "calculate_tools_coupled", null, typeof(RSimulationResult), false, new LStr[] { new LStr("PageSolve", "Simulation", "Расчет") }));
        lst.Add(new ApiEntry(0, "camera_direction_get", null, typeof(RCameraDirection), false, new LStr[] { new LStr("View", "View", "Вид") }));
        lst.Add(new ApiEntry(0, "camera_direction_set", typeof(ACameraDirection), null, false, new LStr[] { new LStr("View", "View", "Вид") }));
        lst.Add(new ApiEntry(0, "chart_get", typeof(AChartId), typeof(RChart), true, new LStr[] { new LStr("PlaybackResults", "Simulation results", "Результаты расчета") }));
        lst.Add(new ApiEntry(0, "chart_value_get", typeof(AChartValueId), typeof(RChartValue), true, new LStr[] { new LStr("PlaybackResults", "Simulation results", "Результаты расчета") }));
        lst.Add(new ApiEntry(0, "client_server_get", null, typeof(RWebAddress), false, new LStr[] { new LStr("QForm", "QForm", "QForm") }));
        lst.Add(new ApiEntry(0, "client_server_set", typeof(AWebAddress), null, false, new LStr[] { new LStr("QForm", "QForm", "QForm") }));
        lst.Add(new ApiEntry(0, "client_server_test_connection", typeof(AWebAddress), typeof(RExecutionStatus), false, new LStr[] { new LStr("QForm", "QForm", "QForm") }));
        lst.Add(new ApiEntry(0, "contact_area", typeof(AObjectId), typeof(RContactArea), false, new LStr[] { new LStr("Mesh", "Mesh", "Сетка") }));
        lst.Add(new ApiEntry(0, "contact_field", typeof(AFieldContact), typeof(RField), false, new LStr[] { new LStr("Mesh", "Mesh", "Сетка") }));
        lst.Add(new ApiEntry(0, "db_arbitrary_drive_get_records", typeof(GDbObjectPath), typeof(RDbArbitraryDriveRecords), false, new LStr[] { new LStr("MenuDataBase", "Database", "База данных"), new LStr("properties", "Properties", "Свойства") }));
        lst.Add(new ApiEntry(0, "db_arbitrary_drive_set_records", typeof(ADbArbitraryDriveRecords), null, false, new LStr[] { new LStr("MenuDataBase", "Database", "База данных"), new LStr("properties", "Properties", "Свойства") }));
        lst.Add(new ApiEntry(0, "db_fetch_items", typeof(ADbFetchParams), typeof(RDbItem), false, new LStr[] { new LStr("MenuDataBase", "Database", "База данных") }));
        lst.Add(new ApiEntry(0, "db_object_create", typeof(ADbObjectCreationParams), null, false, new LStr[] { new LStr("MenuDataBase", "Database", "База данных") }));
        lst.Add(new ApiEntry(0, "db_object_exists", typeof(APathName), typeof(RBoolValue), false, new LStr[] { new LStr("MenuDataBase", "Database", "База данных") }));
        lst.Add(new ApiEntry(0, "db_object_export", typeof(ASrcTargetPath), null, false, new LStr[] { new LStr("MenuDataBase", "Database", "База данных"), new LStr("MenuExp", "Export", "Экспорт") }));
        lst.Add(new ApiEntry(0, "db_object_import", typeof(ASrcTargetPath), null, false, new LStr[] { new LStr("MenuDataBase", "Database", "База данных"), new LStr("MenuExp", "Export", "Экспорт") }));
        lst.Add(new ApiEntry(0, "db_object_save", typeof(ASrcTargetPath), null, false, new LStr[] { new LStr("MenuDataBase", "Database", "База данных") }));
        lst.Add(new ApiEntry(0, "db_objects_copy_to_project_file", null, null, false, new LStr[] { new LStr("MenuDataBase", "Database", "База данных"), new LStr("OP_Project", "Project", "Проект") }));
        lst.Add(new ApiEntry(0, "db_objects_save_all", null, null, false, new LStr[] { new LStr("MenuDataBase", "Database", "База данных"), new LStr("OP_Project", "Project", "Проект") }));
        lst.Add(new ApiEntry(0, "db_property_get", typeof(GDbProperty), typeof(RPropertyValue), true, new LStr[] { new LStr("MenuDataBase", "Database", "База данных"), new LStr("properties", "Properties", "Свойства") }));
        lst.Add(new ApiEntry(0, "db_property_set", typeof(ADbProperty), null, true, new LStr[] { new LStr("MenuDataBase", "Database", "База данных"), new LStr("properties", "Properties", "Свойства") }));
        lst.Add(new ApiEntry(0, "db_property_table_get", typeof(GDbProperty), typeof(RDbPropertyTable), true, new LStr[] { new LStr("MenuDataBase", "Database", "База данных"), new LStr("properties", "Properties", "Свойства") }));
        lst.Add(new ApiEntry(0, "db_property_table_set", typeof(ADbPropertyTable), null, true, new LStr[] { new LStr("MenuDataBase", "Database", "База данных"), new LStr("properties", "Properties", "Свойства") }));
        lst.Add(new ApiEntry(0, "debug_log_begin", typeof(AFileName), null, false, new LStr[] { new LStr("QForm", "QForm", "QForm") }));
        lst.Add(new ApiEntry(0, "debug_log_close", null, null, false, new LStr[] { new LStr("QForm", "QForm", "QForm") }));
        lst.Add(new ApiEntry(0, "do_batch", typeof(ABatchParams), null, false, new LStr[] { new LStr("QForm", "QForm", "QForm") }));
        lst.Add(new ApiEntry(0, "domain_create", typeof(ADomainParams), typeof(RItemId), false, new LStr[] { new LStr("Domains", "Domains", "Области") }));
        lst.Add(new ApiEntry(0, "domain_delete", typeof(AItemId), null, false, new LStr[] { new LStr("Domains", "Domains", "Области") }));
        lst.Add(new ApiEntry(0, "domain_set_body", typeof(AShapeBody), null, false, new LStr[] { new LStr("Domains", "Domains", "Области") }));
        lst.Add(new ApiEntry(0, "domain_set_brick", typeof(AShapeBrick), null, false, new LStr[] { new LStr("Domains", "Domains", "Области") }));
        lst.Add(new ApiEntry(0, "domain_set_circle", typeof(AShapeCircle), null, false, new LStr[] { new LStr("Domains", "Domains", "Области") }));
        lst.Add(new ApiEntry(0, "domain_set_cone", typeof(AShapeCone), null, false, new LStr[] { new LStr("Domains", "Domains", "Области") }));
        lst.Add(new ApiEntry(0, "domain_set_cylinder", typeof(AShapeCylinder), null, false, new LStr[] { new LStr("Domains", "Domains", "Области") }));
        lst.Add(new ApiEntry(0, "domain_set_rect", typeof(AShapeRect), null, false, new LStr[] { new LStr("Domains", "Domains", "Области") }));
        lst.Add(new ApiEntry(0, "domain_set_sphere", typeof(AShapeSphere), null, false, new LStr[] { new LStr("Domains", "Domains", "Области") }));
        lst.Add(new ApiEntry(0, "domain_set_surface_by_color", typeof(AShapeSurfaceByColor), null, false, new LStr[] { new LStr("Domains", "Domains", "Области") }));
        lst.Add(new ApiEntry(0, "domain_type", typeof(AItemId), typeof(RDomainType), false, new LStr[] { new LStr("Domains", "Domains", "Области") }));
        lst.Add(new ApiEntry(0, "dxf_parse_contours", typeof(AFileName), typeof(RContours), false, new LStr[] { new LStr("Utils", "Utils", "Утилиты") }));
        lst.Add(new ApiEntry(0, "execute_subroutines", null, typeof(RSimulationResult), false, new LStr[] { new LStr("OP_Scripts", "Subroutines", "Подпрограммы"), new LStr("PageSolve", "Simulation", "Расчет") }));
        lst.Add(new ApiEntry(0, "execute_subroutines_advanced", typeof(ASubroutineCalculationMode), typeof(RSimulationResult), false, new LStr[] { new LStr("OP_Scripts", "Subroutines", "Подпрограммы"), new LStr("PageSolve", "Simulation", "Расчет") }));
        lst.Add(new ApiEntry(0, "execute_tracking", null, null, false, new LStr[] { new LStr("PageSolve", "Simulation", "Расчет"), new LStr("TracingPoints", "Tracking points", "Трассируемые точки") }));
        lst.Add(new ApiEntry(0, "execute_tracking_advanced", typeof(ATrackingCalculationMode), typeof(RSimulationResult), false, new LStr[] { new LStr("PageSolve", "Simulation", "Расчет"), new LStr("TracingPoints", "Tracking points", "Трассируемые точки") }));
        lst.Add(new ApiEntry(0, "export_bearing_contours", typeof(ABearingContoursExport), null, false, new LStr[] { new LStr("OP_ProcExtrusion", "Extrusion", "Прессование профилей"), new LStr("MenuExp", "Export", "Экспорт") }));
        lst.Add(new ApiEntry(0, "export_field_isolines", typeof(AExportFieldIsolines), null, true, new LStr[] { new LStr("MenuExp", "Export", "Экспорт") }));
        lst.Add(new ApiEntry(0, "export_field_isosurface", typeof(AExportFieldIsosurface), null, true, new LStr[] { new LStr("MenuExp", "Export", "Экспорт") }));
        lst.Add(new ApiEntry(0, "export_fields_at_tracking_points", typeof(AExportFieldsAtTrackingPoints), null, false, new LStr[] { new LStr("MenuExp", "Export", "Экспорт"), new LStr("TracingPoints", "Tracking points", "Трассируемые точки") }));
        lst.Add(new ApiEntry(0, "export_mesh", typeof(AMeshExport), null, false, new LStr[] { new LStr("MenuExp", "Export", "Экспорт") }));
        lst.Add(new ApiEntry(0, "export_profile_section", typeof(AProfileSectionExport), null, false, new LStr[] { new LStr("OP_ProcExtrusion", "Extrusion", "Прессование профилей"), new LStr("MenuExp", "Export", "Экспорт") }));
        lst.Add(new ApiEntry(0, "export_records", typeof(ARecordsExport), null, false, new LStr[] { new LStr("PlaybackResults", "Simulation results", "Результаты расчета"), new LStr("MenuExp", "Export", "Экспорт") }));
        lst.Add(new ApiEntry(0, "export_screenshot", typeof(AExportImage), null, false, new LStr[] { new LStr("MenuExp", "Export", "Экспорт") }));
        lst.Add(new ApiEntry(0, "export_section_contour", typeof(AExportSectionContour), null, false, new LStr[] { new LStr("MenuExp", "Export", "Экспорт") }));
        lst.Add(new ApiEntry(0, "export_section_mesh", typeof(AExportSection), null, false, new LStr[] { new LStr("MenuExp", "Export", "Экспорт") }));
        lst.Add(new ApiEntry(0, "export_video", typeof(AExportImage), null, false, new LStr[] { new LStr("MenuExp", "Export", "Экспорт") }));
        lst.Add(new ApiEntry(0, "extrusion_bearings_z", null, typeof(RBearingZ), false, new LStr[] { new LStr("OP_ProcExtrusion", "Extrusion", "Прессование профилей"), new LStr("PlaybackResults", "Simulation results", "Результаты расчета") }));
        lst.Add(new ApiEntry(0, "extrusion_ports_section_z", null, typeof(RRealValue), false, new LStr[] { new LStr("OP_ProcExtrusion", "Extrusion", "Прессование профилей"), new LStr("PlaybackResults", "Simulation results", "Результаты расчета") }));
        lst.Add(new ApiEntry(0, "extrusion_trace_count", null, typeof(RCount), false, new LStr[] { new LStr("OP_ProcExtrusion", "Extrusion", "Прессование профилей"), new LStr("TracingPoints", "Tracking points", "Трассируемые точки") }));
        lst.Add(new ApiEntry(0, "extrusion_trace_get", typeof(ATraceId), typeof(RTrace), false, new LStr[] { new LStr("OP_ProcExtrusion", "Extrusion", "Прессование профилей"), new LStr("TracingPoints", "Tracking points", "Трассируемые точки") }));
        lst.Add(new ApiEntry(0, "field_at_mesh_point", typeof(AFieldAtMeshPoint), typeof(RRealValue), true, new LStr[] { new LStr("Fields", "Fields", "Поля"), new LStr("Mesh", "Mesh", "Сетка") }));
        lst.Add(new ApiEntry(0, "field_at_mesh_point_vector", typeof(AFieldAtMeshPoint), typeof(RVectorValue), true, new LStr[] { new LStr("Fields", "Fields", "Поля"), new LStr("Mesh", "Mesh", "Сетка") }));
        lst.Add(new ApiEntry(0, "field_at_point", typeof(AFieldAtPoint), typeof(RRealValue), true, new LStr[] { new LStr("Fields", "Fields", "Поля") }));
        lst.Add(new ApiEntry(0, "field_at_point_vector", typeof(AFieldAtPoint), typeof(RVectorValue), true, new LStr[] { new LStr("Fields", "Fields", "Поля") }));
        lst.Add(new ApiEntry(0, "field_at_tracking_line", typeof(AFieldAtTrackingObject), typeof(RRealValues), true, new LStr[] { new LStr("Fields", "Fields", "Поля"), new LStr("TracingPoints", "Tracking points", "Трассируемые точки") }));
        lst.Add(new ApiEntry(0, "field_at_tracking_line_vector", typeof(AFieldAtTrackingObject), typeof(RVectorValues), true, new LStr[] { new LStr("Fields", "Fields", "Поля"), new LStr("TracingPoints", "Tracking points", "Трассируемые точки") }));
        lst.Add(new ApiEntry(0, "field_at_tracking_point", typeof(AFieldAtTrackingObject), typeof(RRealValue), true, new LStr[] { new LStr("Fields", "Fields", "Поля"), new LStr("TracingPoints", "Tracking points", "Трассируемые точки") }));
        lst.Add(new ApiEntry(0, "field_at_tracking_point_vector", typeof(AFieldAtTrackingObject), typeof(RVectorValue), true, new LStr[] { new LStr("Fields", "Fields", "Поля"), new LStr("TracingPoints", "Tracking points", "Трассируемые точки") }));
        lst.Add(new ApiEntry(0, "field_get", typeof(AFieldAtMesh), typeof(RField), true, new LStr[] { new LStr("Mesh", "Mesh", "Сетка"), new LStr("Fields", "Fields", "Поля") }));
        lst.Add(new ApiEntry(0, "field_get_vector", typeof(AFieldAtMesh), typeof(RVectorField), true, new LStr[] { new LStr("Mesh", "Mesh", "Сетка"), new LStr("Fields", "Fields", "Поля") }));
        lst.Add(new ApiEntry(0, "field_isolines", typeof(AFieldIsosurface), typeof(RIsolineList), true, new LStr[] { new LStr("Fields", "Fields", "Поля") }));
        lst.Add(new ApiEntry(0, "field_isosurface", typeof(AFieldIsosurface), typeof(RIsosurfaceList), true, new LStr[] { new LStr("Fields", "Fields", "Поля") }));
        lst.Add(new ApiEntry(0, "field_min_max", typeof(AFieldAtMinMax), typeof(RFieldMinMax), true, new LStr[] { new LStr("Fields", "Fields", "Поля") }));
        lst.Add(new ApiEntry(0, "field_mode_get", null, typeof(RFieldMode), false, new LStr[] { new LStr("View", "View", "Вид") }));
        lst.Add(new ApiEntry(0, "field_mode_set", typeof(AFieldMode), null, false, new LStr[] { new LStr("View", "View", "Вид") }));
        lst.Add(new ApiEntry(0, "field_palette_get", null, typeof(RFieldPalette), false, new LStr[] { new LStr("View", "View", "Вид") }));
        lst.Add(new ApiEntry(0, "field_palette_set", typeof(AFieldPalette), null, false, new LStr[] { new LStr("View", "View", "Вид") }));
        lst.Add(new ApiEntry(0, "field_stat", typeof(AFieldStatAtMesh), typeof(RFieldStat), true, new LStr[] { new LStr("Fields", "Fields", "Поля") }));
        lst.Add(new ApiEntry(0, "field_stat_at_section", typeof(AFieldStatAtSection), typeof(RFieldStat), true, new LStr[] { new LStr("Fields", "Fields", "Поля") }));
        lst.Add(new ApiEntry(0, "file_dialog", typeof(AFileDlg), typeof(RStringList), false, new LStr[] { new LStr("UserInterface", "User interface", "Интерфейс пользователя") }));
        lst.Add(new ApiEntry(0, "geometry_convert_to_3d", typeof(AConvertTo3d), null, false, new LStr[] { new LStr("PageGeometry", "Geometry", "Геометрия") }));
        lst.Add(new ApiEntry(0, "geometry_create_brick", typeof(ABrickObjectParams), null, false, new LStr[] { new LStr("PageGeometry", "Geometry", "Геометрия") }));
        lst.Add(new ApiEntry(0, "geometry_create_rect", typeof(ARectObjectParams), null, false, new LStr[] { new LStr("PageGeometry", "Geometry", "Геометрия") }));
        lst.Add(new ApiEntry(0, "geometry_create_sphere", typeof(ASphereObjectParams), null, false, new LStr[] { new LStr("PageGeometry", "Geometry", "Геометрия") }));
        lst.Add(new ApiEntry(0, "geometry_create_tube", typeof(ATubeObjectParams), null, false, new LStr[] { new LStr("PageGeometry", "Geometry", "Геометрия") }));
        lst.Add(new ApiEntry(0, "geometry_generate_quad_mesh", typeof(AQuadMeshParams), null, false, new LStr[] { new LStr("PageGeometry", "Geometry", "Геометрия") }));
        lst.Add(new ApiEntry(0, "geometry_load", typeof(AFileName), typeof(RObjectList), false, new LStr[] { new LStr("PageGeometry", "Geometry", "Геометрия") }));
        lst.Add(new ApiEntry(0, "geometry_load_extruded_object", typeof(AExtrudedObject), null, false, new LStr[] { new LStr("PageGeometry", "Geometry", "Геометрия") }));
        lst.Add(new ApiEntry(0, "geometry_load_revolved_object", typeof(ARevolvedObject), null, false, new LStr[] { new LStr("PageGeometry", "Geometry", "Геометрия") }));
        lst.Add(new ApiEntry(0, "geometry_load_single_object", typeof(AFileObject), null, false, new LStr[] { new LStr("PageGeometry", "Geometry", "Геометрия") }));
        lst.Add(new ApiEntry(0, "gravity_positioning", typeof(AGravityPositioning), null, false, new LStr[] { new LStr("Positioning", "Positioning", "Позиционирование") }));
        lst.Add(new ApiEntry(0, "heating_cycle_count_get", null, typeof(RCount), false, new LStr[] { new LStr("OP_CyclicHeating", "Cyclic tool heating", "Циклический нагрев инструмента") }));
        lst.Add(new ApiEntry(0, "heating_cycle_count_set", typeof(ACount), null, false, new LStr[] { new LStr("OP_CyclicHeating", "Cyclic tool heating", "Циклический нагрев инструмента") }));
        lst.Add(new ApiEntry(0, "heating_cycle_get_current", null, typeof(RItemId), false, new LStr[] { new LStr("OP_CyclicHeating", "Cyclic tool heating", "Циклический нагрев инструмента") }));
        lst.Add(new ApiEntry(0, "heating_cycle_set_current", typeof(AItemId), null, false, new LStr[] { new LStr("OP_CyclicHeating", "Cyclic tool heating", "Циклический нагрев инструмента") }));
        lst.Add(new ApiEntry(0, "is_windowless_mode", null, typeof(RBoolValue), false, new LStr[] { new LStr("QForm", "QForm", "QForm"), new LStr("UserInterface", "User interface", "Интерфейс пользователя") }));
        lst.Add(new ApiEntry(0, "key_names_get", null, typeof(RKeyNames), false, new LStr[] { new LStr("UserInterface", "User interface", "Интерфейс пользователя") }));
        lst.Add(new ApiEntry(0, "key_send", typeof(ASendKey), null, false, new LStr[] { new LStr("UserInterface", "User interface", "Интерфейс пользователя") }));
        lst.Add(new ApiEntry(0, "language_get", null, typeof(RQFormLang), false, new LStr[] { new LStr("QForm", "QForm", "QForm"), new LStr("UserInterface", "User interface", "Интерфейс пользователя") }));
        lst.Add(new ApiEntry(0, "language_set", typeof(AQFormLang), null, false, new LStr[] { new LStr("QForm", "QForm", "QForm"), new LStr("UserInterface", "User interface", "Интерфейс пользователя") }));
        lst.Add(new ApiEntry(0, "log_begin", typeof(ALogParams), null, false, new LStr[] { new LStr("QForm", "QForm", "QForm") }));
        lst.Add(new ApiEntry(0, "log_save", typeof(ALogFile), null, false, new LStr[] { new LStr("QForm", "QForm", "QForm") }));
        lst.Add(new ApiEntry(0, "mesh_apex_get", typeof(AMeshApexId), typeof(RMeshApex), false, new LStr[] { new LStr("Mesh", "Mesh", "Сетка") }));
        lst.Add(new ApiEntry(0, "mesh_cubics_get", typeof(AMeshObjectId), typeof(RMeshCubics), false, new LStr[] { new LStr("Mesh", "Mesh", "Сетка") }));
        lst.Add(new ApiEntry(0, "mesh_edge_get", typeof(AMeshEdgeId), typeof(RMeshEdge), false, new LStr[] { new LStr("Mesh", "Mesh", "Сетка") }));
        lst.Add(new ApiEntry(0, "mesh_face_get", typeof(AMeshFaceId), typeof(RMeshFace), false, new LStr[] { new LStr("Mesh", "Mesh", "Сетка") }));
        lst.Add(new ApiEntry(0, "mesh_face_types_get", typeof(AMeshObjectId), typeof(RFaceTypes), false, new LStr[] { new LStr("Mesh", "Mesh", "Сетка") }));
        lst.Add(new ApiEntry(0, "mesh_lap_points_get", typeof(AMeshObjectId), typeof(RMeshCoords), false, new LStr[] { new LStr("Mesh", "Mesh", "Сетка") }));
        lst.Add(new ApiEntry(0, "mesh_node_owners_get", typeof(AMeshObjectId), typeof(RMeshNodeOwners), false, new LStr[] { new LStr("Mesh", "Mesh", "Сетка") }));
        lst.Add(new ApiEntry(0, "mesh_nodes_get", typeof(AMeshObjectId), typeof(RMeshCoords), false, new LStr[] { new LStr("Mesh", "Mesh", "Сетка") }));
        lst.Add(new ApiEntry(0, "mesh_point_get", typeof(AObjectPoint), typeof(RMeshPoint), false, new LStr[] { new LStr("Mesh", "Mesh", "Сетка") }));
        lst.Add(new ApiEntry(0, "mesh_properties_get", typeof(AMeshObjectId), typeof(RMeshProperties), false, new LStr[] { new LStr("Mesh", "Mesh", "Сетка") }));
        lst.Add(new ApiEntry(0, "mesh_quadrangles_get", typeof(AMeshObjectId), typeof(RMeshQuadrangles), false, new LStr[] { new LStr("Mesh", "Mesh", "Сетка") }));
        lst.Add(new ApiEntry(0, "mesh_thetrahedrons_get", typeof(AMeshObjectId), typeof(RMeshTetrahedrons), false, new LStr[] { new LStr("Mesh", "Mesh", "Сетка") }));
        lst.Add(new ApiEntry(0, "mesh_triangles_get", typeof(AMeshObjectId), typeof(RMeshTriangles), false, new LStr[] { new LStr("Mesh", "Mesh", "Сетка") }));
        lst.Add(new ApiEntry(0, "mouse_click", typeof(AMouseClick), null, false, new LStr[] { new LStr("UserInterface", "User interface", "Интерфейс пользователя") }));
        lst.Add(new ApiEntry(0, "mouse_click_capture", null, typeof(RMouseClick), false, new LStr[] { new LStr("UserInterface", "User interface", "Интерфейс пользователя") }));
        lst.Add(new ApiEntry(0, "mouse_pos_get", null, typeof(RMousePos), false, new LStr[] { new LStr("UserInterface", "User interface", "Интерфейс пользователя") }));
        lst.Add(new ApiEntry(0, "mouse_pos_set", typeof(AMousePos), null, false, new LStr[] { new LStr("UserInterface", "User interface", "Интерфейс пользователя") }));
        lst.Add(new ApiEntry(0, "msg_box", typeof(AMsgBox), typeof(RPressedDialogButton), false, new LStr[] { new LStr("UserInterface", "User interface", "Интерфейс пользователя") }));
        lst.Add(new ApiEntry(0, "multithreading_settings_get", null, typeof(RMultiThreadingSettings), false, new LStr[] { new LStr("PageSolve", "Simulation", "Расчет"), new LStr("QForm", "QForm", "QForm") }));
        lst.Add(new ApiEntry(0, "multithreading_settings_set", typeof(AMultiThreadingSettings), null, false, new LStr[] { new LStr("PageSolve", "Simulation", "Расчет"), new LStr("QForm", "QForm", "QForm") }));
        lst.Add(new ApiEntry(0, "object_apply_transform", typeof(AObjectTransform), null, false, new LStr[] { new LStr("Positioning", "Positioning", "Позиционирование") }));
        lst.Add(new ApiEntry(0, "object_axes_get", typeof(AObjectId), typeof(RItemList), false, new LStr[] { new LStr("Axes", "Axes", "Оси"), new LStr("Collections", "Collections", "Списки") }));
        lst.Add(new ApiEntry(0, "object_bound_conds_get", typeof(AObjectId), typeof(RItemList), false, new LStr[] { new LStr("Op_BConds", "Boundary conditions", "Граничные условия"), new LStr("Collections", "Collections", "Списки") }));
        lst.Add(new ApiEntry(0, "object_contact", typeof(AObjectContact), typeof(RRealValue), false, new LStr[] { new LStr("Positioning", "Positioning", "Позиционирование") }));
        lst.Add(new ApiEntry(0, "object_copy", typeof(AObjectConvert), null, false, new LStr[] { new LStr("OP_Object", "Object", "Объект"), new LStr("PageGeometry", "Geometry", "Геометрия") }));
        lst.Add(new ApiEntry(0, "object_delete", typeof(AObjectId), null, false, new LStr[] { new LStr("OP_Object", "Object", "Объект") }));
        lst.Add(new ApiEntry(0, "object_display_mode_get", typeof(GDisplayMode), typeof(RBoolValue), false, new LStr[] { new LStr("View", "View", "Вид") }));
        lst.Add(new ApiEntry(0, "object_display_mode_set", typeof(ADisplayMode), null, false, new LStr[] { new LStr("View", "View", "Вид") }));
        lst.Add(new ApiEntry(0, "object_displayed_name", typeof(AObjectId), typeof(RObjectName), false, new LStr[] { new LStr("OP_Object", "Object", "Объект") }));
        lst.Add(new ApiEntry(0, "object_domains_get", typeof(AObjectId), typeof(RItemList), false, new LStr[] { new LStr("Domains", "Domains", "Области"), new LStr("Collections", "Collections", "Списки") }));
        lst.Add(new ApiEntry(0, "object_exists", typeof(AObjectId), typeof(RBoolValue), false, new LStr[] { new LStr("OP_Object", "Object", "Объект") }));
        lst.Add(new ApiEntry(0, "object_find_by_color", typeof(AFindByColor), typeof(RObjectId), false, new LStr[] { new LStr("OP_Object", "Object", "Объект") }));
        lst.Add(new ApiEntry(0, "object_find_by_surface_point", typeof(AFindByPoint), typeof(RObjectId), false, new LStr[] { new LStr("OP_Object", "Object", "Объект") }));
        lst.Add(new ApiEntry(0, "object_get_transform", typeof(AObjectId), typeof(RObjectTransform), false, new LStr[] { new LStr("Positioning", "Positioning", "Позиционирование") }));
        lst.Add(new ApiEntry(0, "object_inherit", typeof(AObjectId), null, false, new LStr[] { new LStr("OP_Object", "Object", "Объект"), new LStr("PageGeometry", "Geometry", "Геометрия") }));
        lst.Add(new ApiEntry(0, "object_is_inherited", typeof(AObjectId), typeof(RBoolValue), false, new LStr[] { new LStr("OP_Object", "Object", "Объект"), new LStr("PageGeometry", "Geometry", "Геометрия") }));
        lst.Add(new ApiEntry(0, "object_move", typeof(AObjectMove), null, false, new LStr[] { new LStr("Positioning", "Positioning", "Позиционирование") }));
        lst.Add(new ApiEntry(0, "object_move_along_axis", typeof(AObjectMoveAxis), null, false, new LStr[] { new LStr("Positioning", "Positioning", "Позиционирование") }));
        lst.Add(new ApiEntry(0, "object_rotate", typeof(AObjectRotate), null, false, new LStr[] { new LStr("Positioning", "Positioning", "Позиционирование") }));
        lst.Add(new ApiEntry(0, "object_rotate_around_axis", typeof(AObjectRotateAxis), null, false, new LStr[] { new LStr("Positioning", "Positioning", "Позиционирование") }));
        lst.Add(new ApiEntry(0, "object_set_type_by_color", typeof(ATypeSetByColor), null, false, new LStr[] { new LStr("OP_Object", "Object", "Объект") }));
        lst.Add(new ApiEntry(0, "object_set_type_by_surface_point", typeof(ATypeSetByPoint), null, false, new LStr[] { new LStr("OP_Object", "Object", "Объект") }));
        lst.Add(new ApiEntry(0, "object_type_set", typeof(AObjectConvert), null, false, new LStr[] { new LStr("OP_Object", "Object", "Объект") }));
        lst.Add(new ApiEntry(0, "object_type_set_in_direction", typeof(AObjectsInDirection), null, false, new LStr[] { new LStr("OP_Object", "Object", "Объект") }));
        lst.Add(new ApiEntry(0, "objects_find_by_color", typeof(AFindByColor), typeof(RObjectList), false, new LStr[] { new LStr("OP_Object", "Object", "Объект") }));
        lst.Add(new ApiEntry(0, "objects_get_in_direction", typeof(APickDirection), typeof(RObjectList), false, new LStr[] { new LStr("OP_Object", "Object", "Объект") }));
        lst.Add(new ApiEntry(0, "on_disconnect", typeof(AOnDisconnect), null, false, new LStr[] { new LStr("QForm", "QForm", "QForm") }));
        lst.Add(new ApiEntry(0, "operation_chains_get", typeof(AItemId), typeof(RItemList), false, new LStr[] { new LStr("OP_Oper", "Operation", "Операция") }));
        lst.Add(new ApiEntry(0, "operation_check", typeof(AOptionalItemId), typeof(ROperationChecks), false, new LStr[] { new LStr("OP_Oper", "Operation", "Операция") }));
        lst.Add(new ApiEntry(0, "operation_copy", typeof(AOperationCopy), typeof(RItemId), false, new LStr[] { new LStr("OP_Oper", "Operation", "Операция") }));
        lst.Add(new ApiEntry(0, "operation_copy_from_parent", typeof(AOperationCopyFromParent), null, false, new LStr[] { new LStr("OP_Oper", "Operation", "Операция") }));
        lst.Add(new ApiEntry(0, "operation_create", typeof(AOperationParams), typeof(RItemId), false, new LStr[] { new LStr("OP_Oper", "Operation", "Операция") }));
        lst.Add(new ApiEntry(0, "operation_cut", typeof(AItemId), null, false, new LStr[] { new LStr("OP_Oper", "Operation", "Операция") }));
        lst.Add(new ApiEntry(0, "operation_delete", typeof(AItemId), null, false, new LStr[] { new LStr("OP_Oper", "Operation", "Операция") }));
        lst.Add(new ApiEntry(0, "operation_exists", typeof(AItemId), typeof(RBoolValue), false, new LStr[] { new LStr("OP_Oper", "Operation", "Операция") }));
        lst.Add(new ApiEntry(0, "operation_get", typeof(AItemId), typeof(ROperation), false, new LStr[] { new LStr("OP_Oper", "Operation", "Операция") }));
        lst.Add(new ApiEntry(0, "operation_get_by_uid", typeof(AItemId), typeof(RItemId), false, new LStr[] { new LStr("OP_Oper", "Operation", "Операция") }));
        lst.Add(new ApiEntry(0, "operation_get_current", null, typeof(RItemId), false, new LStr[] { new LStr("OP_Oper", "Operation", "Операция") }));
        lst.Add(new ApiEntry(0, "operation_graph_get", typeof(AItemId), typeof(ROperationGraph), false, new LStr[] { new LStr("OP_Oper", "Operation", "Операция") }));
        lst.Add(new ApiEntry(0, "operation_insert", typeof(AOperationInsert), typeof(RItemId), false, new LStr[] { new LStr("OP_Oper", "Operation", "Операция") }));
        lst.Add(new ApiEntry(0, "operation_process_get", typeof(AItemId), typeof(RItemId), false, new LStr[] { new LStr("OP_Oper", "Operation", "Операция") }));
        lst.Add(new ApiEntry(0, "operation_set_current", typeof(AItemId), null, false, new LStr[] { new LStr("OP_Oper", "Operation", "Операция") }));
        lst.Add(new ApiEntry(0, "operation_set_first_in_chain", null, typeof(RItemId), false, new LStr[] { new LStr("OP_Oper", "Operation", "Операция") }));
        lst.Add(new ApiEntry(0, "operation_set_last_in_chain", null, typeof(RItemId), false, new LStr[] { new LStr("OP_Oper", "Operation", "Операция") }));
        lst.Add(new ApiEntry(0, "operation_set_next_in_chain", null, typeof(RItemId), false, new LStr[] { new LStr("OP_Oper", "Operation", "Операция") }));
        lst.Add(new ApiEntry(0, "operation_set_prev_in_chain", null, typeof(RItemId), false, new LStr[] { new LStr("OP_Oper", "Operation", "Операция") }));
        lst.Add(new ApiEntry(0, "operation_template_set", typeof(ADbObjectPath), null, false, new LStr[] { new LStr("OP_Oper", "Operation", "Операция") }));
        lst.Add(new ApiEntry(0, "operation_tree", null, typeof(ROperation), false, new LStr[] { new LStr("OP_Oper", "Operation", "Операция") }));
        lst.Add(new ApiEntry(0, "operation_uid", typeof(AOptionalItemId), typeof(RItemId), false, new LStr[] { new LStr("OP_Oper", "Operation", "Операция") }));
        lst.Add(new ApiEntry(0, "panel_positions_get", null, typeof(RPanelPositions), false, new LStr[] { new LStr("UserInterface", "User interface", "Интерфейс пользователя") }));
        lst.Add(new ApiEntry(0, "panel_sizes_get", null, typeof(RPanelSizes), false, new LStr[] { new LStr("UserInterface", "User interface", "Интерфейс пользователя") }));
        lst.Add(new ApiEntry(0, "panel_sizes_set", typeof(APanelSizes), null, false, new LStr[] { new LStr("UserInterface", "User interface", "Интерфейс пользователя") }));
        lst.Add(new ApiEntry(0, "print", typeof(ATraceMsg), null, false, new LStr[] { new LStr("DebugTools", "Debug tools", "Средства отладки"), new LStr("QForm", "QForm", "QForm"), new LStr("UserInterface", "User interface", "Интерфейс пользователя") }));
        lst.Add(new ApiEntry(0, "process_chain_get_current", null, typeof(RItemId), false, new LStr[] { new LStr("OP_Oper", "Operation", "Операция") }));
        lst.Add(new ApiEntry(0, "process_chain_get_current_operations", null, typeof(RItemList), false, new LStr[] { new LStr("OP_Oper", "Operation", "Операция") }));
        lst.Add(new ApiEntry(0, "process_chain_get_operations", typeof(AItemId), typeof(RItemList), false, new LStr[] { new LStr("OP_Oper", "Operation", "Операция") }));
        lst.Add(new ApiEntry(0, "process_chain_set_current", typeof(AItemId), null, false, new LStr[] { new LStr("OP_Oper", "Operation", "Операция") }));
        lst.Add(new ApiEntry(0, "processes_get", null, typeof(RItemList), false, new LStr[] { new LStr("OP_Oper", "Operation", "Операция"), new LStr("Collections", "Collections", "Списки") }));
        lst.Add(new ApiEntry(0, "project_ask_save", null, typeof(RBoolValue), false, new LStr[] { new LStr("OP_Project", "Project", "Проект") }));
        lst.Add(new ApiEntry(0, "project_export_as_script", typeof(AExportAsScriptOptions), null, false, new LStr[] { new LStr("OP_Project", "Project", "Проект"), new LStr("MenuExp", "Export", "Экспорт") }));
        lst.Add(new ApiEntry(0, "project_file_get", null, typeof(RFileName), false, new LStr[] { new LStr("OP_Project", "Project", "Проект") }));
        lst.Add(new ApiEntry(0, "project_new", null, null, false, new LStr[] { new LStr("OP_Project", "Project", "Проект") }));
        lst.Add(new ApiEntry(0, "project_open", typeof(AFileName), null, false, new LStr[] { new LStr("OP_Project", "Project", "Проект") }));
        lst.Add(new ApiEntry(0, "project_open_as_copy", typeof(AProjectOpenAsCopy), null, false, new LStr[] { new LStr("OP_Project", "Project", "Проект") }));
        lst.Add(new ApiEntry(0, "project_open_or_create", typeof(AFileName), null, false, new LStr[] { new LStr("OP_Project", "Project", "Проект") }));
        lst.Add(new ApiEntry(0, "project_path_get", null, typeof(RFileName), false, new LStr[] { new LStr("OP_Project", "Project", "Проект") }));
        lst.Add(new ApiEntry(0, "project_save", null, null, false, new LStr[] { new LStr("OP_Project", "Project", "Проект") }));
        lst.Add(new ApiEntry(0, "project_save_as", typeof(AFileName), null, false, new LStr[] { new LStr("OP_Project", "Project", "Проект") }));
        lst.Add(new ApiEntry(0, "project_save_as_template", typeof(APathName), null, false, new LStr[] { new LStr("OP_Project", "Project", "Проект"), new LStr("MenuDataBase", "Database", "База данных") }));
        lst.Add(new ApiEntry(0, "property_get", typeof(GProperty), typeof(RPropertyValue), true, new LStr[] { new LStr("properties", "Properties", "Свойства") }));
        lst.Add(new ApiEntry(0, "property_get_array_of_real", typeof(GPropertyPath), typeof(RArrayOfReal), true, new LStr[] { new LStr("properties", "Properties", "Свойства") }));
        lst.Add(new ApiEntry(0, "property_get_object", typeof(GPropertyPath), typeof(RObjectId), true, new LStr[] { new LStr("properties", "Properties", "Свойства") }));
        lst.Add(new ApiEntry(0, "property_set", typeof(AProperty), null, true, new LStr[] { new LStr("properties", "Properties", "Свойства") }));
        lst.Add(new ApiEntry(0, "property_set_array_of_real", typeof(APropertyArrayOfReal), null, true, new LStr[] { new LStr("properties", "Properties", "Свойства") }));
        lst.Add(new ApiEntry(0, "property_set_object", typeof(AObjectIdProperty), null, true, new LStr[] { new LStr("properties", "Properties", "Свойства") }));
        lst.Add(new ApiEntry(-6, "qform_attach_to", typeof(ASessionId), null, false, new LStr[] { new LStr("QForm", "QForm", "QForm") }));
        lst.Add(new ApiEntry(0, "qform_process_id", null, typeof(RProcessId), false, new LStr[] { new LStr("DebugTools", "Debug tools", "Средства отладки"), new LStr("QForm", "QForm", "QForm") }));
        lst.Add(new ApiEntry(-4, "qform_reconnect", null, null, false, new LStr[] { new LStr("QForm", "QForm", "QForm") }));
        lst.Add(new ApiEntry(0, "qform_version", null, typeof(RQFormVer), false, new LStr[] { new LStr("QForm", "QForm", "QForm") }));
        lst.Add(new ApiEntry(0, "qform_window_id", null, typeof(RWindowId), false, new LStr[] { new LStr("QForm", "QForm", "QForm") }));
        lst.Add(new ApiEntry(0, "qform_window_pos_get", null, typeof(RWindowPosition), false, new LStr[] { new LStr("QForm", "QForm", "QForm") }));
        lst.Add(new ApiEntry(0, "qform_window_pos_set", typeof(AWindowPosition), null, false, new LStr[] { new LStr("QForm", "QForm", "QForm") }));
        lst.Add(new ApiEntry(0, "record_get", null, typeof(RRecord), false, new LStr[] { new LStr("PlaybackResults", "Simulation results", "Результаты расчета") }));
        lst.Add(new ApiEntry(0, "record_get_last", null, typeof(RRecord), false, new LStr[] { new LStr("PlaybackResults", "Simulation results", "Результаты расчета") }));
        lst.Add(new ApiEntry(0, "record_set", typeof(ARecord), null, false, new LStr[] { new LStr("PlaybackResults", "Simulation results", "Результаты расчета") }));
        lst.Add(new ApiEntry(0, "results_truncate", typeof(ARecord), null, false, new LStr[] { new LStr("PlaybackResults", "Simulation results", "Результаты расчета") }));
        lst.Add(new ApiEntry(0, "section_mesh_get", typeof(ASectionMeshPlane), typeof(RSectionMeshList), false, new LStr[] { new LStr("Mesh", "Mesh", "Сетка") }));
        lst.Add(new ApiEntry(0, "section_plane_create_3p", typeof(ASectionPlane3P), typeof(RItemId), false, new LStr[] { new LStr("View", "View", "Вид") }));
        lst.Add(new ApiEntry(0, "section_plane_create_pn", typeof(ASectionPlanePN), typeof(RItemId), false, new LStr[] { new LStr("View", "View", "Вид") }));
        lst.Add(new ApiEntry(0, "section_plane_delete", typeof(AItemId), null, false, new LStr[] { new LStr("View", "View", "Вид") }));
        lst.Add(new ApiEntry(0, "section_plane_enable", typeof(ASectionPlaneEnabled), null, false, new LStr[] { new LStr("View", "View", "Вид") }));
        lst.Add(new ApiEntry(0, "section_plane_get", typeof(AItemId), typeof(RSectionPlane), false, new LStr[] { new LStr("View", "View", "Вид") }));
        lst.Add(new ApiEntry(0, "section_plane_show", typeof(ASectionPlaneVisibility), null, false, new LStr[] { new LStr("View", "View", "Вид") }));
        lst.Add(new ApiEntry(0, "section_planes_get", null, typeof(RItemList), false, new LStr[] { new LStr("View", "View", "Вид") }));
        lst.Add(new ApiEntry(0, "selected_object_get", null, typeof(RObjectId), false, new LStr[] { new LStr("OP_Object", "Object", "Объект"), new LStr("Collections", "Collections", "Списки") }));
        lst.Add(new ApiEntry(0, "session_id", null, typeof(RItemId), false, new LStr[] { new LStr("QForm", "QForm", "QForm") }));
        lst.Add(new ApiEntry(0, "session_info", null, typeof(RSession), false, new LStr[] { new LStr("QForm", "QForm", "QForm") }));
        lst.Add(new ApiEntry(-7, "session_info_by_id", typeof(ASessionId), typeof(RSession), false, new LStr[] { new LStr("QForm", "QForm", "QForm") }));
        lst.Add(new ApiEntry(-5, "session_list", null, typeof(RSessionList), false, new LStr[] { new LStr("QForm", "QForm", "QForm") }));
        lst.Add(new ApiEntry(0, "session_max_count", null, typeof(RCount), false, new LStr[] { new LStr("QForm", "QForm", "QForm") }));
        lst.Add(new ApiEntry(0, "simulation_objects_get", null, typeof(RObjectList), false, new LStr[] { new LStr("Collections", "Collections", "Списки") }));
        lst.Add(new ApiEntry(0, "sleep", typeof(ASleepTime), null, false, new LStr[] { new LStr("UserInterface", "User interface", "Интерфейс пользователя") }));
        lst.Add(new ApiEntry(0, "spring_between_tools_create", typeof(AOptionalItemId), typeof(RItemId), false, new LStr[] { new LStr("Op_SpringsBetweenTools", "Springs between tools", "Пружины между инструментами") }));
        lst.Add(new ApiEntry(0, "spring_between_tools_delete", typeof(AItemId), null, false, new LStr[] { new LStr("Op_SpringsBetweenTools", "Springs between tools", "Пружины между инструментами") }));
        lst.Add(new ApiEntry(0, "springs_between_tools_get", null, typeof(RItemList), false, new LStr[] { new LStr("Op_SpringsBetweenTools", "Springs between tools", "Пружины между инструментами"), new LStr("Collections", "Collections", "Списки") }));
        lst.Add(new ApiEntry(0, "start_simulation", null, typeof(RMainSimulationResult), false, new LStr[] { new LStr("PageSolve", "Simulation", "Расчет") }));
        lst.Add(new ApiEntry(0, "start_simulation_advanced", typeof(ASimulationParams), typeof(RMainSimulationResult), false, new LStr[] { new LStr("PageSolve", "Simulation", "Расчет") }));
        lst.Add(new ApiEntry(0, "state_blow", typeof(AOptionalGlobalItemId), typeof(RSimulationState), false, new LStr[] { new LStr("SlvState", "Simulation state", "Статус расчета") }));
        lst.Add(new ApiEntry(0, "state_extrusion", typeof(ASystemStateId), typeof(RExtrusionState), false, new LStr[] { new LStr("SlvState", "Simulation state", "Статус расчета") }));
        lst.Add(new ApiEntry(0, "state_mesh", typeof(AMeshStateId), typeof(RMeshState), false, new LStr[] { new LStr("SlvState", "Simulation state", "Статус расчета") }));
        lst.Add(new ApiEntry(0, "state_operation", typeof(AOptionalItemId), typeof(RSimulationState), false, new LStr[] { new LStr("SlvState", "Simulation state", "Статус расчета") }));
        lst.Add(new ApiEntry(0, "state_process_chain", typeof(AOptionalItemId), typeof(RSimulationState), false, new LStr[] { new LStr("SlvState", "Simulation state", "Статус расчета") }));
        lst.Add(new ApiEntry(0, "state_system", typeof(ASystemStateId), typeof(RSystemState), false, new LStr[] { new LStr("SlvState", "Simulation state", "Статус расчета") }));
        lst.Add(new ApiEntry(0, "state_tool", typeof(AToolStateId), typeof(RToolState), false, new LStr[] { new LStr("SlvState", "Simulation state", "Статус расчета") }));
        lst.Add(new ApiEntry(0, "state_workpiece", typeof(AWorkpieceStateId), typeof(RWorkpieceState), false, new LStr[] { new LStr("SlvState", "Simulation state", "Статус расчета") }));
        lst.Add(new ApiEntry(0, "stop_cond_create", typeof(AStopCondParams), typeof(RItemId), false, new LStr[] { new LStr("OP_StopConds", "Simulation stop conditions", "Условия остановки расчета") }));
        lst.Add(new ApiEntry(0, "stop_cond_create_distance", typeof(AStopCondDistance), typeof(RItemId), false, new LStr[] { new LStr("OP_StopConds", "Simulation stop conditions", "Условия остановки расчета") }));
        lst.Add(new ApiEntry(0, "stop_cond_create_final_pos", typeof(AStopCondFinPos), typeof(RItemId), false, new LStr[] { new LStr("OP_StopConds", "Simulation stop conditions", "Условия остановки расчета") }));
        lst.Add(new ApiEntry(0, "stop_cond_create_max_load", typeof(AStopCondMaxLoad), typeof(RItemId), false, new LStr[] { new LStr("OP_StopConds", "Simulation stop conditions", "Условия остановки расчета") }));
        lst.Add(new ApiEntry(0, "stop_cond_create_profile_length", typeof(AStopCondProfileLength), typeof(RItemId), false, new LStr[] { new LStr("OP_StopConds", "Simulation stop conditions", "Условия остановки расчета") }));
        lst.Add(new ApiEntry(0, "stop_cond_create_rotation", typeof(AStopCondRotation), typeof(RItemId), false, new LStr[] { new LStr("OP_StopConds", "Simulation stop conditions", "Условия остановки расчета") }));
        lst.Add(new ApiEntry(0, "stop_cond_create_stroke", typeof(AStopCondStroke), typeof(RItemId), false, new LStr[] { new LStr("OP_StopConds", "Simulation stop conditions", "Условия остановки расчета") }));
        lst.Add(new ApiEntry(0, "stop_cond_create_time", typeof(AStopCondTime), typeof(RItemId), false, new LStr[] { new LStr("OP_StopConds", "Simulation stop conditions", "Условия остановки расчета") }));
        lst.Add(new ApiEntry(0, "stop_cond_delete", typeof(AItemId), null, false, new LStr[] { new LStr("OP_StopConds", "Simulation stop conditions", "Условия остановки расчета") }));
        lst.Add(new ApiEntry(0, "stop_cond_type", typeof(AItemId), typeof(RStopCond), false, new LStr[] { new LStr("OP_StopConds", "Simulation stop conditions", "Условия остановки расчета") }));
        lst.Add(new ApiEntry(0, "stop_condition_create_field_value", typeof(AStopCondFieldValue), typeof(RItemId), false, new LStr[] { new LStr("OP_StopConds", "Simulation stop conditions", "Условия остановки расчета") }));
        lst.Add(new ApiEntry(0, "stop_conds_get", null, typeof(RItemList), false, new LStr[] { new LStr("OP_StopConds", "Simulation stop conditions", "Условия остановки расчета"), new LStr("Collections", "Collections", "Списки") }));
        lst.Add(new ApiEntry(0, "subroutine_create", typeof(ASubroutineCreate), typeof(RItemId), false, new LStr[] { new LStr("OP_Scripts", "Subroutines", "Подпрограммы") }));
        lst.Add(new ApiEntry(0, "subroutine_delete", typeof(AItemId), null, false, new LStr[] { new LStr("OP_Scripts", "Subroutines", "Подпрограммы") }));
        lst.Add(new ApiEntry(0, "subroutine_get", typeof(AItemId), typeof(RSubroutine), false, new LStr[] { new LStr("OP_Scripts", "Subroutines", "Подпрограммы") }));
        lst.Add(new ApiEntry(0, "subroutine_parameter_get", typeof(GSubroutineParameter), typeof(RNullableRealValue), false, new LStr[] { new LStr("OP_Scripts", "Subroutines", "Подпрограммы") }));
        lst.Add(new ApiEntry(0, "subroutine_parameter_set", typeof(ASubroutineParameter), null, false, new LStr[] { new LStr("OP_Scripts", "Subroutines", "Подпрограммы") }));
        lst.Add(new ApiEntry(0, "subroutines_get", null, typeof(RItemList), false, new LStr[] { new LStr("OP_Scripts", "Subroutines", "Подпрограммы"), new LStr("Collections", "Collections", "Списки") }));
        lst.Add(new ApiEntry(0, "sym_plane_create", typeof(ASymPlaneParams), typeof(RItemId), false, new LStr[] { new LStr("OP_Symplanes", "Symmetry planes", "Плоскости симметрии") }));
        lst.Add(new ApiEntry(0, "sym_plane_create_by_close_point", typeof(ASymPlaneByPoint), typeof(RItemId), false, new LStr[] { new LStr("OP_Symplanes", "Symmetry planes", "Плоскости симметрии") }));
        lst.Add(new ApiEntry(0, "sym_plane_create_by_face_color", typeof(ASymPlaneByColor), typeof(RItemId), false, new LStr[] { new LStr("OP_Symplanes", "Symmetry planes", "Плоскости симметрии") }));
        lst.Add(new ApiEntry(0, "sym_plane_delete", typeof(AItemId), null, false, new LStr[] { new LStr("OP_Symplanes", "Symmetry planes", "Плоскости симметрии") }));
        lst.Add(new ApiEntry(0, "sym_plane_get", typeof(AItemId), typeof(RUnitVector), false, new LStr[] { new LStr("OP_Symplanes", "Symmetry planes", "Плоскости симметрии") }));
        lst.Add(new ApiEntry(0, "sym_planes_create_auto", null, typeof(RItemList), false, new LStr[] { new LStr("OP_Symplanes", "Symmetry planes", "Плоскости симметрии") }));
        lst.Add(new ApiEntry(0, "sym_planes_get", null, typeof(RItemList), false, new LStr[] { new LStr("OP_Symplanes", "Symmetry planes", "Плоскости симметрии"), new LStr("Collections", "Collections", "Списки") }));
        lst.Add(new ApiEntry(0, "system_of_units_get", null, typeof(RUnitSystem), false, new LStr[] { new LStr("QForm", "QForm", "QForm"), new LStr("UserInterface", "User interface", "Интерфейс пользователя") }));
        lst.Add(new ApiEntry(0, "system_of_units_set", typeof(AUnitSystem), null, false, new LStr[] { new LStr("QForm", "QForm", "QForm"), new LStr("UserInterface", "User interface", "Интерфейс пользователя") }));
        lst.Add(new ApiEntry(0, "tracking_contour_create", typeof(ANewTrackingContour), typeof(RItemId), false, new LStr[] { new LStr("TracingPoints", "Tracking points", "Трассируемые точки") }));
        lst.Add(new ApiEntry(0, "tracking_contours_create", typeof(AOptionalObjectItemId), typeof(RItemId), false, new LStr[] { new LStr("TracingPoints", "Tracking points", "Трассируемые точки") }));
        lst.Add(new ApiEntry(0, "tracking_group_create", typeof(ATrackingGroup), typeof(RItemId), false, new LStr[] { new LStr("TracingPoints", "Tracking points", "Трассируемые точки") }));
        lst.Add(new ApiEntry(0, "tracking_line_create", typeof(ATrackingLineParams), typeof(RItemId), false, new LStr[] { new LStr("TracingPoints", "Tracking points", "Трассируемые точки") }));
        lst.Add(new ApiEntry(0, "tracking_line_get", typeof(AGlobalItemId), typeof(RTrackingLine), false, new LStr[] { new LStr("TracingPoints", "Tracking points", "Трассируемые точки") }));
        lst.Add(new ApiEntry(0, "tracking_lines_get", null, typeof(RItemList), false, new LStr[] { new LStr("Collections", "Collections", "Списки"), new LStr("TracingPoints", "Tracking points", "Трассируемые точки") }));
        lst.Add(new ApiEntry(0, "tracking_lines_get_for_chain", null, typeof(RGlobalItemList), false, new LStr[] { new LStr("Collections", "Collections", "Списки"), new LStr("TracingPoints", "Tracking points", "Трассируемые точки") }));
        lst.Add(new ApiEntry(0, "tracking_point_create", typeof(ATrackingPointParams), typeof(RItemId), false, new LStr[] { new LStr("TracingPoints", "Tracking points", "Трассируемые точки") }));
        lst.Add(new ApiEntry(0, "tracking_point_get", typeof(AGlobalItemId), typeof(RMeshPoint), false, new LStr[] { new LStr("TracingPoints", "Tracking points", "Трассируемые точки") }));
        lst.Add(new ApiEntry(0, "tracking_points_get", null, typeof(RItemList), false, new LStr[] { new LStr("Collections", "Collections", "Списки"), new LStr("TracingPoints", "Tracking points", "Трассируемые точки") }));
        lst.Add(new ApiEntry(0, "tracking_points_get_for_chain", null, typeof(RGlobalItemList), false, new LStr[] { new LStr("Collections", "Collections", "Списки"), new LStr("TracingPoints", "Tracking points", "Трассируемые точки") }));
        lst.Add(new ApiEntry(0, "view_back", null, null, false, new LStr[] { new LStr("View", "View", "Вид") }));
        lst.Add(new ApiEntry(0, "view_bottom", null, null, false, new LStr[] { new LStr("View", "View", "Вид") }));
        lst.Add(new ApiEntry(0, "view_front", null, null, false, new LStr[] { new LStr("View", "View", "Вид") }));
        lst.Add(new ApiEntry(0, "view_left", null, null, false, new LStr[] { new LStr("View", "View", "Вид") }));
        lst.Add(new ApiEntry(0, "view_on_bottom_90", null, null, false, new LStr[] { new LStr("View", "View", "Вид") }));
        lst.Add(new ApiEntry(0, "view_on_top_90", null, null, false, new LStr[] { new LStr("View", "View", "Вид") }));
        lst.Add(new ApiEntry(0, "view_overall_dimensions", null, typeof(RViewOverallDimensions), false, new LStr[] { new LStr("View", "View", "Вид") }));
        lst.Add(new ApiEntry(0, "view_right", null, null, false, new LStr[] { new LStr("View", "View", "Вид") }));
        lst.Add(new ApiEntry(0, "view_top", null, null, false, new LStr[] { new LStr("View", "View", "Вид") }));
        lst.Add(new ApiEntry(0, "work_dir_get", null, typeof(RPathName), false, new LStr[] { new LStr("QForm", "QForm", "QForm") }));
        lst.Add(new ApiEntry(0, "work_dir_set", typeof(APathName), null, false, new LStr[] { new LStr("QForm", "QForm", "QForm") }));
        lst.Add(new ApiEntry(0, "zoom_to_fit", null, null, false, new LStr[] { new LStr("View", "View", "Вид") }));
        lst.Add(new ApiEntry(0, "zoom_to_frame", typeof(AScale), null, false, new LStr[] { new LStr("View", "View", "Вид") }));
    }
}

////block api-types
public class GBilletParameter
{
    [Category("Mandatory")]
    public int billet { get; set; }
    [Category("Mandatory")]
    public BilletParam param { get; set; }
}

public class GBlowParameter
{
    [Category("Mandatory")]
    public int blow { get; set; }
    [Category("Mandatory")]
    public BlowParam param { get; set; }
    [Category("Mandatory")]
    public int stop_condition { get; set; }
}

public class GDbObjectPath
{
    [Category("Mandatory")]
    public string db_path { get; set; }
}

public class GDbProperty
{
    [Category("Mandatory")]
    public string db_path { get; set; }
    [Category("Mandatory")]
    public string prop_path { get; set; }
}

public class GDisplayMode
{
    [Category("Mandatory")]
    public ObjectType type { get; set; }
    [Category("Mandatory")]
    public int id { get; set; }
    [Category("Mandatory")]
    public DisplayModes mode { get; set; }
    public GDisplayMode()
    {
        type = ObjectType.Nothing;
    }
}

public class GProperty
{
    [Category("Mandatory")]
    public ObjectType object_type { get; set; }
    [Category("Mandatory")]
    public int object_id { get; set; }
    [Category("Mandatory")]
    public string path { get; set; }
    [Category("Mandatory")]
    public PropertyType property_type { get; set; }
    public GProperty()
    {
        object_type = ObjectType.Operation;
        property_type = PropertyType.Value;
    }
}

public class GPropertyPath
{
    [Category("Mandatory")]
    public ObjectType object_type { get; set; }
    [Category("Mandatory")]
    public int object_id { get; set; }
    [Category("Mandatory")]
    public string path { get; set; }
    public GPropertyPath()
    {
        object_type = ObjectType.Operation;
    }
}

public class GSubroutineParameter
{
    [Category("Mandatory")]
    public int id { get; set; }
    [Category("Mandatory")]
    public string parameter { get; set; }
}

public class AFileName
{
    [Category("Mandatory")]
    public string file { get; set; }
}

public class ACSharpSettings
{
    [Category("Mandatory")]
    public string connection_type { get; set; }
    [Category("Mandatory")]
    public bool alt_connection { get; set; }
    [Category("Mandatory")]
    public string import_dir { get; set; }
    [Category("Mandatory")]
    public string class_name { get; set; }
    [Category("Mandatory")]
    public bool use_static { get; set; }
    [Category("Mandatory")]
    public bool use_qform_exceptions { get; set; }
}

public class AVBNetSettings
{
    [Category("Mandatory")]
    public string connection_type { get; set; }
    [Category("Mandatory")]
    public bool alt_connection { get; set; }
    [Category("Mandatory")]
    public string import_dir { get; set; }
    [Category("Mandatory")]
    public string class_name { get; set; }
    [Category("Mandatory")]
    public bool use_static { get; set; }
    [Category("Mandatory")]
    public bool use_qform_exceptions { get; set; }
}

public class ADbTreeArg
{
    [Category("Mandatory")]
    public int db_type { get; set; }
}

public class ASrcTargetPath
{
    [Category("Mandatory")]
    public string source_path { get; set; }
    [Category("Optional")]
    public string target_path { get; set; }
}

public class APytonSettings
{
    [Category("Mandatory")]
    public string script_type { get; set; }
    [Category("Mandatory")]
    public string connection_type { get; set; }
    [Category("Mandatory")]
    public bool alt_connection { get; set; }
    [Category("Mandatory")]
    public string import_dir { get; set; }
    [Category("Mandatory")]
    public bool use_qform_exceptions { get; set; }
}

public class AMatlabSettings
{
    [Category("Mandatory")]
    public string script_type { get; set; }
    [Category("Mandatory")]
    public string connection_type { get; set; }
    [Category("Mandatory")]
    public bool alt_connection { get; set; }
    [Category("Mandatory")]
    public bool use_qform_exceptions { get; set; }
}

public class AFieldId
{
    [Category("Private")]
    public string field { get; set; }
    [Category("Private")]
    public FieldSource field_source { get; set; }
    [Category("Private")]
    public int field_type { get; set; }
    [Category("Private")]
    public int field_target { get; set; }
    [Category("Private")]
    public int source_object { get; set; }
    [Category("Private")]
    public int source_operation { get; set; }
    [Category("Private")]
    public double field_min { get; set; }
    [Category("Private")]
    public double field_max { get; set; }
    [Category("Private")]
    public int units { get; set; }
    public AFieldId()
    {
        source_object = -1;
        source_operation = -1;
    }
}

public class AAssembledTool
{
    [Category("Optional")]
    public int id { get; set; }
    List<int> x_parts = new List<int>();
    [Category("Mandatory")]
    public List<int> parts { get { return x_parts; } }
    public AAssembledTool()
    {
        id = -1;
    }
}

public class AItemId
{
    [Category("Mandatory")]
    public int id { get; set; }
}

public class AObjectList
{
    List<AObjectId> x_objects = new List<AObjectId>();
    [Category("Mandatory")]
    public List<AObjectId> objects { get { return x_objects; } }
}

public class AObjectId
{
    [Category("Mandatory")]
    public ObjectType type { get; set; }
    [Category("Mandatory")]
    public int id { get; set; }
    public AObjectId()
    {
        type = ObjectType.Nothing;
    }
}

public class ASubroutineCalculationMode
{
    [Category("Optional")]
    public CalculationUnit calculation_mode { get; set; }
}

public class ATrackingCalculationMode
{
    [Category("Optional")]
    public CalculationUnit calculation_mode { get; set; }
    [Category("Optional")]
    public bool forward { get; set; }
    [Category("Optional")]
    public bool backward { get; set; }
}

public class ASimulationParams
{
    [Category("Optional")]
    public int start_from_record { get; set; }
    [Category("Optional")]
    public bool remesh_tools { get; set; }
    [Category("Optional")]
    public int stop_at_record { get; set; }
    [Category("Optional")]
    public int max_records { get; set; }
    [Category("Optional")]
    public double stop_at_process_time { get; set; }
    [Category("Optional")]
    public double max_process_time { get; set; }
    [Category("Optional")]
    public int max_calculation_time { get; set; }
    [Category("Optional")]
    public CalculationMode calculation_mode { get; set; }
    public ASimulationParams()
    {
        start_from_record = -1;
        calculation_mode = CalculationMode.Chain;
    }
}

public class AAsyncWaitingParams
{
    [Category("Optional")]
    public int timeout { get; set; }
    [Category("Optional")]
    public bool with_simulation_stage_events { get; set; }
    [Category("Optional")]
    public bool with_diagnostic_events { get; set; }
    [Category("Optional")]
    public bool with_iteration_events { get; set; }
    public AAsyncWaitingParams()
    {
        timeout = 60;
    }
}

public class AFieldGroupId
{
    [Category("Mandatory")]
    public FieldGroup group { get; set; }
    public AFieldGroupId()
    {
        group = FieldGroup.Workpiece;
    }
}

public class AObjectAxisId
{
    [Category("Mandatory")]
    public ObjectType object_type { get; set; }
    [Category("Mandatory")]
    public int object_id { get; set; }
    [Category("Mandatory")]
    public int axis { get; set; }
    public AObjectAxisId()
    {
        axis = 1;
    }
}

public class AObjectAxisParams
{
    [Category("Mandatory")]
    public ObjectType object_type { get; set; }
    [Category("Mandatory")]
    public int object_id { get; set; }
    [Category("Mandatory")]
    public int axis { get; set; }
    [Category("Mandatory")]
    public double point1_x { get; set; }
    [Category("Mandatory")]
    public double point1_y { get; set; }
    [Category("Mandatory")]
    public double point1_z { get; set; }
    [Category("Mandatory")]
    public double point2_x { get; set; }
    [Category("Mandatory")]
    public double point2_y { get; set; }
    [Category("Mandatory")]
    public double point2_z { get; set; }
    public AObjectAxisParams()
    {
        axis = 1;
    }
}

public class ACount
{
    [Category("Mandatory")]
    public int count { get; set; }
}

public class ABilletParameter
{
    [Category("Mandatory")]
    public int billet { get; set; }
    [Category("Mandatory")]
    public BilletParam param { get; set; }
    [Category("Mandatory")]
    public double value { get; set; }
}

public class ABlowParameter
{
    [Category("Mandatory")]
    public int blow { get; set; }
    [Category("Mandatory")]
    public BlowParam param { get; set; }
    [Category("Mandatory")]
    public int stop_condition { get; set; }
    [Category("Mandatory")]
    public double value { get; set; }
}

public class ABoundCondParams
{
    [Category("Optional")]
    public int id { get; set; }
    [Category("Mandatory")]
    public BCond type { get; set; }
    [Category("Mandatory")]
    public ObjectType object_type { get; set; }
    [Category("Mandatory")]
    public int object_id { get; set; }
    public ABoundCondParams()
    {
        id = -1;
    }
}

public class AShapeBody
{
    [Category("Mandatory")]
    public int id { get; set; }
    [Category("Mandatory")]
    public ObjectType source_body_type { get; set; }
    [Category("Mandatory")]
    public int source_body_id { get; set; }
}

public class AShapeBrick
{
    [Category("Mandatory")]
    public int id { get; set; }
    [Category("Mandatory")]
    public double size1_x { get; set; }
    [Category("Mandatory")]
    public double size1_y { get; set; }
    [Category("Mandatory")]
    public double size1_z { get; set; }
    [Category("Mandatory")]
    public double size2_x { get; set; }
    [Category("Mandatory")]
    public double size2_y { get; set; }
    [Category("Mandatory")]
    public double size2_z { get; set; }
}

public class AShapeCircle
{
    [Category("Mandatory")]
    public int id { get; set; }
    [Category("Mandatory")]
    public double r { get; set; }
    [Category("Optional")]
    public double inner_r { get; set; }
}

public class AShapeCone
{
    [Category("Mandatory")]
    public int id { get; set; }
    [Category("Mandatory")]
    public double angle { get; set; }
    [Category("Mandatory")]
    public double d { get; set; }
    [Category("Mandatory")]
    public double h { get; set; }
}

public class AShapeCylinder
{
    [Category("Mandatory")]
    public int id { get; set; }
    [Category("Mandatory")]
    public double r { get; set; }
    [Category("Optional")]
    public double inner_r { get; set; }
    [Category("Mandatory")]
    public double h1 { get; set; }
    [Category("Mandatory")]
    public double h2 { get; set; }
}

public class AShapeRect
{
    [Category("Mandatory")]
    public int id { get; set; }
    [Category("Mandatory")]
    public double size1_x { get; set; }
    [Category("Mandatory")]
    public double size1_z { get; set; }
    [Category("Mandatory")]
    public double size2_x { get; set; }
    [Category("Mandatory")]
    public double size2_z { get; set; }
}

public class AShapeSphere
{
    [Category("Mandatory")]
    public int id { get; set; }
    [Category("Mandatory")]
    public double r { get; set; }
    [Category("Optional")]
    public double inner_r { get; set; }
}

public class AShapeSprayerPolarArray
{
    [Category("Mandatory")]
    public int id { get; set; }
    [Category("Mandatory")]
    public double angle { get; set; }
    [Category("Mandatory")]
    public double d { get; set; }
    [Category("Mandatory")]
    public double h { get; set; }
    [Category("Mandatory")]
    public double SectorRadius { get; set; }
    [Category("Mandatory")]
    public double SectorAngle { get; set; }
    [Category("Mandatory")]
    public int CountConesInSector { get; set; }
    [Category("Mandatory")]
    public int CountLayers { get; set; }
    [Category("Mandatory")]
    public double DistanceBetweenLayers { get; set; }
    [Category("Mandatory")]
    public double AngleBetweenConeAxisAndLayerPlane { get; set; }
    [Category("Optional")]
    public double TwistAngle { get; set; }
}

public class AShapeSprayerPolarArrayDB
{
    [Category("Mandatory")]
    public int id { get; set; }
    [Category("Mandatory")]
    public double SectorRadius { get; set; }
    [Category("Mandatory")]
    public double SectorAngle { get; set; }
    [Category("Mandatory")]
    public int CountConesInSector { get; set; }
    [Category("Mandatory")]
    public int CountLayers { get; set; }
    [Category("Mandatory")]
    public double DistanceBetweenLayers { get; set; }
    [Category("Mandatory")]
    public double AngleBetweenConeAxisAndLayerPlane { get; set; }
    [Category("Optional")]
    public double TwistAngle { get; set; }
}

public class AShapeSprayerRectArray
{
    [Category("Mandatory")]
    public int id { get; set; }
    [Category("Mandatory")]
    public double angle { get; set; }
    [Category("Mandatory")]
    public double d { get; set; }
    [Category("Mandatory")]
    public double h { get; set; }
    [Category("Mandatory")]
    public int CountConesDir1 { get; set; }
    [Category("Mandatory")]
    public int CountConesDir2 { get; set; }
    [Category("Mandatory")]
    public double DistanceDir1 { get; set; }
    [Category("Mandatory")]
    public double DistanceDir2 { get; set; }
    [Category("Optional")]
    public double TwistAngle { get; set; }
}

public class AShapeSprayerRectArrayDB
{
    [Category("Mandatory")]
    public int id { get; set; }
    [Category("Mandatory")]
    public int CountConesDir1 { get; set; }
    [Category("Mandatory")]
    public int CountConesDir2 { get; set; }
    [Category("Mandatory")]
    public double DistanceDir1 { get; set; }
    [Category("Mandatory")]
    public double DistanceDir2 { get; set; }
    [Category("Optional")]
    public double TwistAngle { get; set; }
}

public class AShapeSurfaceByColor
{
    [Category("Mandatory")]
    public int id { get; set; }
    [Category("Mandatory")]
    public int color_R { get; set; }
    [Category("Mandatory")]
    public int color_G { get; set; }
    [Category("Mandatory")]
    public int color_B { get; set; }
}

public class ACameraDirection
{
    [Category("Mandatory")]
    public double forward_dir_x { get; set; }
    [Category("Mandatory")]
    public double forward_dir_y { get; set; }
    [Category("Mandatory")]
    public double forward_dir_z { get; set; }
    [Category("Mandatory")]
    public double up_dir_x { get; set; }
    [Category("Mandatory")]
    public double up_dir_y { get; set; }
    [Category("Mandatory")]
    public double up_dir_z { get; set; }
}

public class AChartId
{
    [Category("Private")]
    public ObjectType arg_object_type { get; set; }
    [Category("Private")]
    public int arg_object_id { get; set; }
    [Category("Private")]
    public int arg_subobject { get; set; }
    [Category("Private")]
    public int arg_id { get; set; }
    [Category("Private")]
    public ObjectType func_object_type { get; set; }
    [Category("Private")]
    public int func_object_id { get; set; }
    [Category("Private")]
    public int func_subobject { get; set; }
    [Category("Private")]
    public int func_id { get; set; }
}

public class AChartValueId
{
    [Category("Private")]
    public ObjectType arg_object_type { get; set; }
    [Category("Private")]
    public int arg_object_id { get; set; }
    [Category("Private")]
    public int arg_subobject { get; set; }
    [Category("Private")]
    public int arg_id { get; set; }
    [Category("Private")]
    public ObjectType func_object_type { get; set; }
    [Category("Private")]
    public int func_object_id { get; set; }
    [Category("Private")]
    public int func_subobject { get; set; }
    [Category("Private")]
    public int func_id { get; set; }
    [Category("Optional")]
    public double record { get; set; }
    public AChartValueId()
    {
        record = -1;
    }
}

public class AWebAddress
{
    [Category("Mandatory")]
    public string url { get; set; }
}

public class AFieldContact
{
    [Category("Mandatory")]
    public ObjectType type { get; set; }
    [Category("Mandatory")]
    public int id { get; set; }
    [Category("Mandatory")]
    public bool in_elements { get; set; }
    public AFieldContact()
    {
        type = ObjectType.Nothing;
    }
}

public class ADbObjectPath
{
    [Category("Mandatory")]
    public string db_path { get; set; }
}

public class ADbArbitraryDriveRecords
{
    [Category("Mandatory")]
    public string db_path { get; set; }
    List<ADbArbitraryDriveRecord> x_records = new List<ADbArbitraryDriveRecord>();
    [Category("Mandatory")]
    public List<ADbArbitraryDriveRecord> records { get { return x_records; } }
}

public class ADbArbitraryDriveRecord
{
    [Category("Mandatory")]
    public double t { get; set; }
    [Category("Mandatory")]
    public double x { get; set; }
    [Category("Mandatory")]
    public double y { get; set; }
    [Category("Mandatory")]
    public double z { get; set; }
    [Category("Mandatory")]
    public double rx { get; set; }
    [Category("Mandatory")]
    public double ry { get; set; }
    [Category("Mandatory")]
    public double rz { get; set; }
    [Category("Mandatory")]
    public string units { get; set; }
}

public class ADbFetchParams
{
    [Category("Mandatory")]
    public string db_name { get; set; }
    [Category("Optional")]
    public DbStandart db_standart { get; set; }
}

public class ADbObjectCreationParams
{
    [Category("Mandatory")]
    public string path { get; set; }
    [Category("Mandatory")]
    public DriveType drive_type { get; set; }
    public ADbObjectCreationParams()
    {
        drive_type = DriveType.Unspecified;
    }
}

public class APathName
{
    [Category("Mandatory")]
    public string path { get; set; }
}

public class ADbProperty
{
    [Category("Mandatory")]
    public string db_path { get; set; }
    [Category("Mandatory")]
    public string prop_path { get; set; }
    [Category("Mandatory")]
    public string value { get; set; }
}

public class ADbPropertyTable
{
    [Category("Mandatory")]
    public string db_path { get; set; }
    [Category("Mandatory")]
    public string prop_path { get; set; }
    List<double> x_row_arg_values = new List<double>();
    [Category("Mandatory")]
    public List<double> row_arg_values { get { return x_row_arg_values; } }
    List<double> x_column_arg_values = new List<double>();
    [Category("Mandatory")]
    public List<double> column_arg_values { get { return x_column_arg_values; } }
    List<double> x_layer_arg_values = new List<double>();
    [Category("Mandatory")]
    public List<double> layer_arg_values { get { return x_layer_arg_values; } }
    [Category("Mandatory")]
    public DbTableArg row_arg { get; set; }
    [Category("Mandatory")]
    public DbTableArg column_arg { get; set; }
    [Category("Mandatory")]
    public DbTableArg layer_arg { get; set; }
    List<double> x_values = new List<double>();
    [Category("Mandatory")]
    public List<double> values { get { return x_values; } }
    public ADbPropertyTable()
    {
        row_arg = DbTableArg.Nothing;
        column_arg = DbTableArg.Nothing;
        layer_arg = DbTableArg.Nothing;
    }
}

public class ABatchParams
{
    [Category("Mandatory")]
    public string file { get; set; }
    [Category("Optional")]
    public string log_file { get; set; }
    [Category("Optional")]
    public LogFormat log_format { get; set; }
    [Category("Optional")]
    public bool log_input { get; set; }
    [Category("Optional")]
    public bool log_output { get; set; }
    public ABatchParams()
    {
        log_format = LogFormat.FromFileExtension;
    }
}

public class ADomainParams
{
    [Category("Optional")]
    public int id { get; set; }
    [Category("Mandatory")]
    public Domain type { get; set; }
    [Category("Mandatory")]
    public ObjectType object_type { get; set; }
    [Category("Mandatory")]
    public int object_id { get; set; }
    public ADomainParams()
    {
        id = -1;
    }
}

public class ABearingContoursExport
{
    [Category("Mandatory")]
    public string file { get; set; }
    [Category("Mandatory")]
    public BearingContoursFormat format { get; set; }
    public ABearingContoursExport()
    {
        format = BearingContoursFormat.FromFileExtension;
    }
}

public class AExportFieldIsolines
{
    [Category("Mandatory")]
    public ObjectType object_type { get; set; }
    [Category("Mandatory")]
    public int object_id { get; set; }
    [Category("Mandatory")]
    public int mesh_index { get; set; }
    [Category("Private")]
    public string field { get; set; }
    [Category("Private")]
    public FieldSource field_source { get; set; }
    [Category("Private")]
    public int source_object { get; set; }
    [Category("Private")]
    public int source_operation { get; set; }
    [Category("Mandatory")]
    public double field_value { get; set; }
    [Category("Mandatory")]
    public string file { get; set; }
    [Category("Optional")]
    public IsolinesFormat format { get; set; }
    public AExportFieldIsolines()
    {
        object_type = ObjectType.Nothing;
        source_object = -1;
        source_operation = -1;
    }
}

public class AExportFieldIsosurface
{
    [Category("Mandatory")]
    public ObjectType object_type { get; set; }
    [Category("Mandatory")]
    public int object_id { get; set; }
    [Category("Mandatory")]
    public int mesh_index { get; set; }
    [Category("Private")]
    public string field { get; set; }
    [Category("Private")]
    public FieldSource field_source { get; set; }
    [Category("Private")]
    public int source_object { get; set; }
    [Category("Private")]
    public int source_operation { get; set; }
    [Category("Mandatory")]
    public double field_value { get; set; }
    [Category("Mandatory")]
    public string file { get; set; }
    [Category("Optional")]
    public SectionFormat format { get; set; }
    [Category("Optional")]
    public LengthUnit mesh_units { get; set; }
    public AExportFieldIsosurface()
    {
        object_type = ObjectType.Nothing;
        source_object = -1;
        source_operation = -1;
    }
}

public class AExportFieldsAtTrackingPoints
{
    [Category("Mandatory")]
    public string file { get; set; }
    [Category("Mandatory")]
    public TrackingFieldsFormat file_format { get; set; }
    [Category("Mandatory")]
    public ValuesOnSheet values_on_sheet { get; set; }
    [Category("Mandatory")]
    public bool with_workpiece_points { get; set; }
    [Category("Mandatory")]
    public bool with_tool_points { get; set; }
    [Category("Mandatory")]
    public int start_operation { get; set; }
    [Category("Mandatory")]
    public int end_operation { get; set; }
    [Category("Mandatory")]
    public int start_blow { get; set; }
    [Category("Mandatory")]
    public int end_blow { get; set; }
    public AExportFieldsAtTrackingPoints()
    {
        file_format = TrackingFieldsFormat.FromFileExtension;
        values_on_sheet = ValuesOnSheet.at_point_blow;
        with_workpiece_points = true;
        start_operation = -1;
        end_operation = -1;
        start_blow = -1;
        end_blow = -1;
    }
}

public class AMeshExport
{
    [Category("Mandatory")]
    public ObjectType object_type { get; set; }
    [Category("Mandatory")]
    public int object_id { get; set; }
    [Category("Mandatory")]
    public string file { get; set; }
    [Category("Mandatory")]
    public MeshFormat format { get; set; }
    [Category("Mandatory")]
    public int mesh_index { get; set; }
    [Category("Mandatory")]
    public bool surface_only { get; set; }
    public AMeshExport()
    {
        object_type = ObjectType.Nothing;
        format = MeshFormat.FromFileExtension;
    }
}

public class AProfileSectionExport
{
    [Category("Mandatory")]
    public string file { get; set; }
    [Category("Mandatory")]
    public ProfileSectionFormat format { get; set; }
    public AProfileSectionExport()
    {
        format = ProfileSectionFormat.FromFileExtension;
    }
}

public class ARecordsExport
{
    [Category("Mandatory")]
    public RecordsExportMode mode { get; set; }
    [Category("Optional")]
    public string output_directory { get; set; }
    public ARecordsExport()
    {
        mode = RecordsExportMode.ExportCurrent;
    }
}

public class AExportImage
{
    [Category("Mandatory")]
    public string file { get; set; }
    [Category("Optional")]
    public int width { get; set; }
    [Category("Optional")]
    public int height { get; set; }
    [Category("Optional")]
    public bool keep_aspect_ratio { get; set; }
    [Category("Optional")]
    public bool display_operation_name { get; set; }
    [Category("Optional")]
    public bool display_blow_number { get; set; }
    [Category("Optional")]
    public bool display_record_number { get; set; }
    [Category("Optional")]
    public bool display_time { get; set; }
    [Category("Optional")]
    public bool display_time_step { get; set; }
    [Category("Optional")]
    public bool zoom_to_fit { get; set; }
    [Category("Optional")]
    public bool display_legend { get; set; }
    [Category("Optional")]
    public int legend_width { get; set; }
    public AExportImage()
    {
        width = 0;
        height = 0;
    }
}

public class AExportSectionContour
{
    [Category("Mandatory")]
    public double point_x { get; set; }
    [Category("Mandatory")]
    public double point_y { get; set; }
    [Category("Mandatory")]
    public double point_z { get; set; }
    [Category("Mandatory")]
    public double normal_x { get; set; }
    [Category("Mandatory")]
    public double normal_y { get; set; }
    [Category("Mandatory")]
    public double normal_z { get; set; }
    [Category("Optional")]
    public ObjectType object_type { get; set; }
    [Category("Optional")]
    public int object_id { get; set; }
    [Category("Optional")]
    public int mesh_index { get; set; }
    [Category("Optional")]
    public bool u_vector_defined { get; set; }
    [Category("Optional")]
    public double u_vector_x { get; set; }
    [Category("Optional")]
    public double u_vector_y { get; set; }
    [Category("Optional")]
    public double u_vector_z { get; set; }
    [Category("Mandatory")]
    public string file { get; set; }
    [Category("Optional")]
    public SectionContourFormat format { get; set; }
    [Category("Optional")]
    public LengthUnit mesh_units { get; set; }
}

public class AExportSection
{
    [Category("Mandatory")]
    public double point_x { get; set; }
    [Category("Mandatory")]
    public double point_y { get; set; }
    [Category("Mandatory")]
    public double point_z { get; set; }
    [Category("Mandatory")]
    public double normal_x { get; set; }
    [Category("Mandatory")]
    public double normal_y { get; set; }
    [Category("Mandatory")]
    public double normal_z { get; set; }
    [Category("Mandatory")]
    public ObjectType object_type { get; set; }
    [Category("Mandatory")]
    public int object_id { get; set; }
    [Category("Optional")]
    public int mesh_index { get; set; }
    [Category("Optional")]
    public bool u_vector_defined { get; set; }
    [Category("Optional")]
    public double u_vector_x { get; set; }
    [Category("Optional")]
    public double u_vector_y { get; set; }
    [Category("Optional")]
    public double u_vector_z { get; set; }
    [Category("Mandatory")]
    public string file { get; set; }
    [Category("Optional")]
    public SectionFormat format { get; set; }
    [Category("Optional")]
    public LengthUnit mesh_units { get; set; }
}

public class ATraceId
{
    [Category("Mandatory")]
    public PickTraceBy pick_by { get; set; }
    [Category("Mandatory")]
    public int number { get; set; }
}

public class AFieldAtMeshPoint
{
    [Category("Mandatory")]
    public ObjectType object_type { get; set; }
    [Category("Mandatory")]
    public int object_id { get; set; }
    [Category("Mandatory")]
    public int mesh_index { get; set; }
    [Category("Private")]
    public string field { get; set; }
    [Category("Private")]
    public FieldSource field_source { get; set; }
    [Category("Private")]
    public int source_object { get; set; }
    [Category("Private")]
    public int source_operation { get; set; }
    List<int> x_node = new List<int>();
    [Category("Mandatory")]
    public List<int> node { get { return x_node; } }
    List<double> x_node_weight = new List<double>();
    [Category("Mandatory")]
    public List<double> node_weight { get { return x_node_weight; } }
    [Category("Mandatory")]
    public bool on_surface { get; set; }
    public AFieldAtMeshPoint()
    {
        object_type = ObjectType.Nothing;
        source_object = -1;
        source_operation = -1;
    }
}

public class AFieldAtPoint
{
    [Category("Mandatory")]
    public ObjectType object_type { get; set; }
    [Category("Mandatory")]
    public int object_id { get; set; }
    [Category("Mandatory")]
    public int mesh_index { get; set; }
    [Category("Private")]
    public string field { get; set; }
    [Category("Private")]
    public FieldSource field_source { get; set; }
    [Category("Private")]
    public int source_object { get; set; }
    [Category("Private")]
    public int source_operation { get; set; }
    [Category("Mandatory")]
    public double x { get; set; }
    [Category("Mandatory")]
    public double y { get; set; }
    [Category("Mandatory")]
    public double z { get; set; }
    [Category("Mandatory")]
    public bool on_surface { get; set; }
    public AFieldAtPoint()
    {
        object_type = ObjectType.Nothing;
        source_object = -1;
        source_operation = -1;
    }
}

public class AFieldAtTrackingObject
{
    [Category("Mandatory")]
    public int object_id { get; set; }
    [Category("Mandatory")]
    public int object_operation { get; set; }
    [Category("Mandatory")]
    public int mesh_index { get; set; }
    [Category("Private")]
    public string field { get; set; }
    [Category("Private")]
    public FieldSource field_source { get; set; }
    [Category("Private")]
    public int source_object { get; set; }
    [Category("Private")]
    public int source_operation { get; set; }
    public AFieldAtTrackingObject()
    {
        object_operation = -1;
        source_object = -1;
        source_operation = -1;
    }
}

public class AFieldAtMesh
{
    [Category("Mandatory")]
    public ObjectType object_type { get; set; }
    [Category("Mandatory")]
    public int object_id { get; set; }
    [Category("Mandatory")]
    public int mesh_index { get; set; }
    [Category("Private")]
    public string field { get; set; }
    [Category("Private")]
    public FieldSource field_source { get; set; }
    [Category("Private")]
    public int source_object { get; set; }
    [Category("Private")]
    public int source_operation { get; set; }
    public AFieldAtMesh()
    {
        object_type = ObjectType.Nothing;
        source_object = -1;
        source_operation = -1;
    }
}

public class AFieldIsosurface
{
    [Category("Mandatory")]
    public ObjectType object_type { get; set; }
    [Category("Mandatory")]
    public int object_id { get; set; }
    [Category("Mandatory")]
    public int mesh_index { get; set; }
    [Category("Private")]
    public string field { get; set; }
    [Category("Private")]
    public FieldSource field_source { get; set; }
    [Category("Private")]
    public int source_object { get; set; }
    [Category("Private")]
    public int source_operation { get; set; }
    [Category("Mandatory")]
    public double field_value { get; set; }
    public AFieldIsosurface()
    {
        object_type = ObjectType.Nothing;
        source_object = -1;
        source_operation = -1;
    }
}

public class AFieldAtMinMax
{
    [Category("Mandatory")]
    public ObjectType object_type { get; set; }
    [Category("Mandatory")]
    public int object_id { get; set; }
    [Category("Mandatory")]
    public int mesh_index { get; set; }
    [Category("Private")]
    public string field { get; set; }
    [Category("Private")]
    public FieldSource field_source { get; set; }
    [Category("Private")]
    public int source_object { get; set; }
    [Category("Private")]
    public int source_operation { get; set; }
    [Category("Mandatory")]
    public bool on_surface { get; set; }
    public AFieldAtMinMax()
    {
        object_type = ObjectType.Nothing;
        source_object = -1;
        source_operation = -1;
    }
}

public class AFieldMode
{
    [Category("Mandatory")]
    public FillMode mode { get; set; }
}

public class AFieldPalette
{
    [Category("Mandatory")]
    public Colormap palette { get; set; }
    [Category("Mandatory")]
    public bool inverse { get; set; }
}

public class AFieldStatAtMesh
{
    [Category("Mandatory")]
    public ObjectType object_type { get; set; }
    [Category("Mandatory")]
    public int object_id { get; set; }
    [Category("Mandatory")]
    public int mesh_index { get; set; }
    [Category("Private")]
    public string field { get; set; }
    [Category("Private")]
    public FieldSource field_source { get; set; }
    [Category("Private")]
    public int source_object { get; set; }
    [Category("Private")]
    public int source_operation { get; set; }
    [Category("Mandatory")]
    public int interval_count { get; set; }
    [Category("Mandatory")]
    public HistogramBy histogram_by { get; set; }
    [Category("Mandatory")]
    public double percentile_1_level { get; set; }
    [Category("Mandatory")]
    public double percentile_2_level { get; set; }
    public AFieldStatAtMesh()
    {
        object_type = ObjectType.Nothing;
        source_object = -1;
        source_operation = -1;
        interval_count = 25;
        percentile_1_level = 5;
        percentile_2_level = 95;
    }
}

public class AFieldStatAtSection
{
    [Category("Mandatory")]
    public ObjectType object_type { get; set; }
    [Category("Mandatory")]
    public int object_id { get; set; }
    [Category("Mandatory")]
    public int mesh_index { get; set; }
    [Category("Private")]
    public string field { get; set; }
    [Category("Private")]
    public FieldSource field_source { get; set; }
    [Category("Private")]
    public int source_object { get; set; }
    [Category("Private")]
    public int source_operation { get; set; }
    [Category("Mandatory")]
    public double section_point_x { get; set; }
    [Category("Mandatory")]
    public double section_point_y { get; set; }
    [Category("Mandatory")]
    public double section_point_z { get; set; }
    [Category("Mandatory")]
    public double section_normal_x { get; set; }
    [Category("Mandatory")]
    public double section_normal_y { get; set; }
    [Category("Mandatory")]
    public double section_normal_z { get; set; }
    [Category("Mandatory")]
    public int interval_count { get; set; }
    [Category("Mandatory")]
    public double percentile_1_level { get; set; }
    [Category("Mandatory")]
    public double percentile_2_level { get; set; }
    public AFieldStatAtSection()
    {
        object_type = ObjectType.Nothing;
        source_object = -1;
        source_operation = -1;
        interval_count = 25;
        percentile_1_level = 5;
        percentile_2_level = 95;
    }
}

public class AFileDlg
{
    [Category("Mandatory")]
    public bool open_mode { get; set; }
    [Category("Optional")]
    public string file_name { get; set; }
    [Category("Optional")]
    public string file_ext { get; set; }
    List<string> x_filters = new List<string>();
    [Category("Optional")]
    public List<string> filters { get { return x_filters; } }
    [Category("Optional")]
    public int current_filter { get; set; }
    [Category("Optional")]
    public string current_folder { get; set; }
    [Category("Optional")]
    public bool allow_multiple_files { get; set; }
    [Category("Optional")]
    public string settings_key { get; set; }
    public AFileDlg()
    {
        open_mode = true;
    }
}

public class AConvertTo3d
{
    [Category("Mandatory")]
    public ObjectType object_type { get; set; }
    [Category("Mandatory")]
    public int object_id { get; set; }
    [Category("Mandatory")]
    public double thickness { get; set; }
    [Category("Optional")]
    public int layer_count { get; set; }
    [Category("Optional")]
    public double dir_x { get; set; }
    [Category("Optional")]
    public double dir_y { get; set; }
    [Category("Optional")]
    public double dir_z { get; set; }
}

public class ABrickObjectParams
{
    [Category("Mandatory")]
    public ObjectType object_type { get; set; }
    [Category("Mandatory")]
    public int object_id { get; set; }
    [Category("Optional")]
    public double offset_x { get; set; }
    [Category("Optional")]
    public double offset_y { get; set; }
    [Category("Optional")]
    public double offset_z { get; set; }
    [Category("Mandatory")]
    public double size_x { get; set; }
    [Category("Mandatory")]
    public double size_y { get; set; }
    [Category("Mandatory")]
    public double size_z { get; set; }
    [Category("Mandatory")]
    public bool hexahedral_mesh { get; set; }
    [Category("Mandatory")]
    public bool based_on_hexahedrons { get; set; }
    [Category("Mandatory")]
    public int elem_count_x { get; set; }
    [Category("Mandatory")]
    public int elem_count_y { get; set; }
    [Category("Mandatory")]
    public int elem_count_z { get; set; }
    public ABrickObjectParams()
    {
        object_type = ObjectType.Nothing;
    }
}

public class ARectObjectParams
{
    [Category("Mandatory")]
    public ObjectType object_type { get; set; }
    [Category("Mandatory")]
    public int object_id { get; set; }
    [Category("Optional")]
    public double offset_x { get; set; }
    [Category("Optional")]
    public double offset_y { get; set; }
    [Category("Optional")]
    public double offset_z { get; set; }
    [Category("Mandatory")]
    public double size_x { get; set; }
    [Category("Mandatory")]
    public double size_z { get; set; }
    [Category("Mandatory")]
    public bool hexahedral_mesh { get; set; }
    [Category("Mandatory")]
    public int elem_count_x { get; set; }
    [Category("Mandatory")]
    public int elem_count_z { get; set; }
    public ARectObjectParams()
    {
        object_type = ObjectType.Nothing;
    }
}

public class ASphereObjectParams
{
    [Category("Mandatory")]
    public ObjectType object_type { get; set; }
    [Category("Mandatory")]
    public int object_id { get; set; }
    [Category("Optional")]
    public double offset_x { get; set; }
    [Category("Optional")]
    public double offset_y { get; set; }
    [Category("Optional")]
    public double offset_z { get; set; }
    [Category("Mandatory")]
    public double r { get; set; }
    [Category("Mandatory")]
    public double inner_r { get; set; }
    public ASphereObjectParams()
    {
        object_type = ObjectType.Nothing;
    }
}

public class ATubeObjectParams
{
    [Category("Mandatory")]
    public ObjectType object_type { get; set; }
    [Category("Mandatory")]
    public int object_id { get; set; }
    [Category("Optional")]
    public double offset_x { get; set; }
    [Category("Optional")]
    public double offset_y { get; set; }
    [Category("Optional")]
    public double offset_z { get; set; }
    [Category("Mandatory")]
    public double r { get; set; }
    [Category("Mandatory")]
    public double inner_r { get; set; }
    [Category("Mandatory")]
    public double h { get; set; }
    [Category("Mandatory")]
    public double sector_angle { get; set; }
    [Category("Mandatory")]
    public double sector_start_angle { get; set; }
    [Category("Mandatory")]
    public bool hexahedral_mesh { get; set; }
    [Category("Mandatory")]
    public bool based_on_hexahedrons { get; set; }
    [Category("Mandatory")]
    public int elem_count_axial { get; set; }
    [Category("Mandatory")]
    public int elem_count_radial { get; set; }
    [Category("Mandatory")]
    public int elem_count_tangential { get; set; }
    public ATubeObjectParams()
    {
        object_type = ObjectType.Nothing;
    }
}

public class AQuadMeshParams
{
    [Category("Mandatory")]
    public ObjectType object_type { get; set; }
    [Category("Mandatory")]
    public int object_id { get; set; }
    [Category("Mandatory")]
    public double penalty_angle_min { get; set; }
    [Category("Mandatory")]
    public double penalty_angle_max { get; set; }
    [Category("Mandatory")]
    public int max_split_level { get; set; }
    [Category("Mandatory")]
    public int min_initial_side { get; set; }
    [Category("Mandatory")]
    public int min_closing_level { get; set; }
    [Category("Mandatory")]
    public bool can_be_simplified { get; set; }
    [Category("Mandatory")]
    public double duglas_peucker_eps { get; set; }
    public AQuadMeshParams()
    {
        penalty_angle_min = 30;
        penalty_angle_max = 145;
        max_split_level = 3;
        min_initial_side = 10;
        min_closing_level = 12;
        can_be_simplified = true;
        duglas_peucker_eps = 1e-05;
    }
}

public class AExtrudedObject
{
    [Category("Mandatory")]
    public string file { get; set; }
    [Category("Mandatory")]
    public ObjectType object_type { get; set; }
    [Category("Mandatory")]
    public int object_id { get; set; }
    [Category("Mandatory")]
    public double depth { get; set; }
    [Category("Mandatory")]
    public double mesh_adapt_koef { get; set; }
    public AExtrudedObject()
    {
        mesh_adapt_koef = 1;
    }
}

public class ARevolvedObject
{
    [Category("Mandatory")]
    public string file { get; set; }
    [Category("Mandatory")]
    public ObjectType object_type { get; set; }
    [Category("Mandatory")]
    public int object_id { get; set; }
    [Category("Mandatory")]
    public double angle { get; set; }
    [Category("Optional")]
    public bool use_x_axis { get; set; }
    [Category("Mandatory")]
    public double mesh_adapt_koef { get; set; }
    public ARevolvedObject()
    {
        mesh_adapt_koef = 1;
    }
}

public class AFileObject
{
    [Category("Mandatory")]
    public string file { get; set; }
    [Category("Mandatory")]
    public ObjectType object_type { get; set; }
    [Category("Mandatory")]
    public int object_id { get; set; }
}

public class AGravityPositioning
{
    [Category("Mandatory")]
    public ObjectType moving_object_type { get; set; }
    [Category("Mandatory")]
    public int moving_object_id { get; set; }
    [Category("Mandatory")]
    public ObjectType target_object_type { get; set; }
    [Category("Mandatory")]
    public int target_object_id { get; set; }
}

public class ASendKey
{
    [Category("Mandatory")]
    public string key { get; set; }
    [Category("Optional")]
    public bool ctrl { get; set; }
    [Category("Optional")]
    public bool alt { get; set; }
    [Category("Optional")]
    public bool shift { get; set; }
}

public class AQFormLang
{
    [Category("Mandatory")]
    public Language language { get; set; }
}

public class ALogParams
{
    [Category("Optional")]
    public bool log_input { get; set; }
    [Category("Optional")]
    public bool log_output { get; set; }
    public ALogParams()
    {
        log_output = true;
    }
}

public class ALogFile
{
    [Category("Mandatory")]
    public string file { get; set; }
    [Category("Optional")]
    public LogFormat log_format { get; set; }
    public ALogFile()
    {
        log_format = LogFormat.FromFileExtension;
    }
}

public class AMeshApexId
{
    [Category("Mandatory")]
    public ObjectType object_type { get; set; }
    [Category("Mandatory")]
    public int object_id { get; set; }
    [Category("Mandatory")]
    public int mesh_index { get; set; }
    [Category("Mandatory")]
    public int apex { get; set; }
    public AMeshApexId()
    {
        object_type = ObjectType.Nothing;
    }
}

public class AMeshObjectId
{
    [Category("Mandatory")]
    public ObjectType object_type { get; set; }
    [Category("Mandatory")]
    public int object_id { get; set; }
    [Category("Mandatory")]
    public int mesh_index { get; set; }
    public AMeshObjectId()
    {
        object_type = ObjectType.Nothing;
    }
}

public class AMeshEdgeId
{
    [Category("Mandatory")]
    public ObjectType object_type { get; set; }
    [Category("Mandatory")]
    public int object_id { get; set; }
    [Category("Mandatory")]
    public int mesh_index { get; set; }
    [Category("Mandatory")]
    public int edge { get; set; }
    public AMeshEdgeId()
    {
        object_type = ObjectType.Nothing;
    }
}

public class AMeshFaceId
{
    [Category("Mandatory")]
    public ObjectType object_type { get; set; }
    [Category("Mandatory")]
    public int object_id { get; set; }
    [Category("Mandatory")]
    public int mesh_index { get; set; }
    [Category("Mandatory")]
    public int face { get; set; }
    public AMeshFaceId()
    {
        object_type = ObjectType.Nothing;
    }
}

public class AObjectPoint
{
    [Category("Mandatory")]
    public ObjectType object_type { get; set; }
    [Category("Mandatory")]
    public int object_id { get; set; }
    [Category("Mandatory")]
    public int mesh_index { get; set; }
    [Category("Mandatory")]
    public double point_x { get; set; }
    [Category("Mandatory")]
    public double point_y { get; set; }
    [Category("Mandatory")]
    public double point_z { get; set; }
    [Category("Mandatory")]
    public bool on_surface { get; set; }
    public AObjectPoint()
    {
        object_type = ObjectType.Nothing;
    }
}

public class AMouseClick
{
    [Category("Mandatory")]
    public MouseButton button { get; set; }
    [Category("Mandatory")]
    public bool use_current_pos { get; set; }
    [Category("Optional")]
    public int coord_x { get; set; }
    [Category("Optional")]
    public int coord_y { get; set; }
    [Category("Optional")]
    public bool ctrl { get; set; }
    [Category("Optional")]
    public bool alt { get; set; }
    [Category("Optional")]
    public bool shift { get; set; }
    public AMouseClick()
    {
        use_current_pos = true;
    }
}

public class AMousePos
{
    [Category("Mandatory")]
    public int coord_x { get; set; }
    [Category("Mandatory")]
    public int coord_y { get; set; }
}

public class AMsgBox
{
    [Category("Mandatory")]
    public string msg { get; set; }
    [Category("Optional")]
    public bool button_ok { get; set; }
    [Category("Optional")]
    public bool button_cancel { get; set; }
    [Category("Optional")]
    public bool button_yes { get; set; }
    [Category("Optional")]
    public bool button_no { get; set; }
    [Category("Optional")]
    public bool button_retry { get; set; }
    [Category("Optional")]
    public bool button_continue { get; set; }
    [Category("Optional")]
    public bool button_close { get; set; }
    [Category("Optional")]
    public bool button_save_as { get; set; }
    public AMsgBox()
    {
        button_ok = true;
    }
}

public class AMultiThreadingSettings
{
    [Category("Mandatory")]
    public int computation_thread_count { get; set; }
    [Category("Mandatory")]
    public bool computation_reproducibility { get; set; }
}

public class AObjectTransform
{
    [Category("Mandatory")]
    public ObjectType object_type { get; set; }
    [Category("Mandatory")]
    public int object_id { get; set; }
    List<double> x_matrix4x4 = new List<double>();
    [Category("Mandatory")]
    public List<double> matrix4x4 { get { return x_matrix4x4; } }
}

public class AObjectContact
{
    [Category("Mandatory")]
    public ObjectType moving_object_type { get; set; }
    [Category("Mandatory")]
    public int moving_object_id { get; set; }
    [Category("Mandatory")]
    public ObjectType target_object_type { get; set; }
    [Category("Mandatory")]
    public int target_object_id { get; set; }
    [Category("Mandatory")]
    public Direction direction { get; set; }
    [Category("Mandatory")]
    public bool reverse_direction { get; set; }
    [Category("Optional")]
    public bool move_dependent_objects { get; set; }
    List<AObjectId> x_move_additional_objects = new List<AObjectId>();
    [Category("Optional")]
    public List<AObjectId> move_additional_objects { get { return x_move_additional_objects; } }
}

public class AObjectConvert
{
    [Category("Mandatory")]
    public ObjectType object_type { get; set; }
    [Category("Mandatory")]
    public int object_id { get; set; }
    [Category("Mandatory")]
    public ObjectType new_type { get; set; }
    [Category("Mandatory")]
    public int new_id { get; set; }
}

public class ADisplayMode
{
    [Category("Mandatory")]
    public ObjectType type { get; set; }
    [Category("Mandatory")]
    public int id { get; set; }
    [Category("Mandatory")]
    public DisplayModes mode { get; set; }
    [Category("Mandatory")]
    public bool value { get; set; }
    public ADisplayMode()
    {
        type = ObjectType.Nothing;
        value = true;
    }
}

public class AFindByColor
{
    [Category("Mandatory")]
    public int color_R { get; set; }
    [Category("Mandatory")]
    public int color_G { get; set; }
    [Category("Mandatory")]
    public int color_B { get; set; }
    [Category("Mandatory")]
    public bool pick_by_body_color { get; set; }
    [Category("Mandatory")]
    public bool pick_by_face_color { get; set; }
}

public class AFindByPoint
{
    [Category("Mandatory")]
    public double point_x { get; set; }
    [Category("Mandatory")]
    public double point_y { get; set; }
    [Category("Mandatory")]
    public double point_z { get; set; }
}

public class AObjectMove
{
    [Category("Mandatory")]
    public ObjectType object_type { get; set; }
    [Category("Mandatory")]
    public int object_id { get; set; }
    [Category("Mandatory")]
    public double vector_x { get; set; }
    [Category("Mandatory")]
    public double vector_y { get; set; }
    [Category("Mandatory")]
    public double vector_z { get; set; }
    [Category("Optional")]
    public bool move_dependent_objects { get; set; }
}

public class AObjectMoveAxis
{
    [Category("Mandatory")]
    public ObjectType object_type { get; set; }
    [Category("Mandatory")]
    public int object_id { get; set; }
    [Category("Mandatory")]
    public int axis { get; set; }
    [Category("Mandatory")]
    public double distance { get; set; }
    [Category("Optional")]
    public bool move_dependent_objects { get; set; }
}

public class AObjectRotate
{
    [Category("Mandatory")]
    public ObjectType object_type { get; set; }
    [Category("Mandatory")]
    public int object_id { get; set; }
    [Category("Mandatory")]
    public double point_x { get; set; }
    [Category("Mandatory")]
    public double point_y { get; set; }
    [Category("Mandatory")]
    public double point_z { get; set; }
    [Category("Mandatory")]
    public double vector_x { get; set; }
    [Category("Mandatory")]
    public double vector_y { get; set; }
    [Category("Mandatory")]
    public double vector_z { get; set; }
    [Category("Mandatory")]
    public double angle { get; set; }
    [Category("Optional")]
    public bool rotate_dependent_objects { get; set; }
}

public class AObjectRotateAxis
{
    [Category("Mandatory")]
    public ObjectType object_type { get; set; }
    [Category("Mandatory")]
    public int object_id { get; set; }
    [Category("Mandatory")]
    public int axis { get; set; }
    [Category("Mandatory")]
    public double angle { get; set; }
    [Category("Optional")]
    public bool rotate_dependent_objects { get; set; }
}

public class ATypeSetByColor
{
    [Category("Mandatory")]
    public ObjectType type { get; set; }
    [Category("Mandatory")]
    public int id { get; set; }
    [Category("Mandatory")]
    public int color_R { get; set; }
    [Category("Mandatory")]
    public int color_G { get; set; }
    [Category("Mandatory")]
    public int color_B { get; set; }
    [Category("Mandatory")]
    public bool pick_by_body_color { get; set; }
    [Category("Mandatory")]
    public bool pick_by_face_color { get; set; }
    public ATypeSetByColor()
    {
        type = ObjectType.Nothing;
    }
}

public class ATypeSetByPoint
{
    [Category("Mandatory")]
    public ObjectType type { get; set; }
    [Category("Mandatory")]
    public int id { get; set; }
    [Category("Mandatory")]
    public double point_x { get; set; }
    [Category("Mandatory")]
    public double point_y { get; set; }
    [Category("Mandatory")]
    public double point_z { get; set; }
}

public class AObjectsInDirection
{
    [Category("Mandatory")]
    public double vector_x { get; set; }
    [Category("Mandatory")]
    public double vector_y { get; set; }
    [Category("Mandatory")]
    public double vector_z { get; set; }
    [Category("Optional")]
    public bool pick_specified_type { get; set; }
    [Category("Optional")]
    public ObjectType pick_type { get; set; }
    List<AObjectId> x_objects = new List<AObjectId>();
    [Category("Mandatory")]
    public List<AObjectId> objects { get { return x_objects; } }
    public AObjectsInDirection()
    {
        pick_specified_type = true;
        pick_type = ObjectType.ImportedObject;
    }
}

public class APickDirection
{
    [Category("Mandatory")]
    public double vector_x { get; set; }
    [Category("Mandatory")]
    public double vector_y { get; set; }
    [Category("Mandatory")]
    public double vector_z { get; set; }
    [Category("Optional")]
    public bool pick_specified_type { get; set; }
    [Category("Optional")]
    public ObjectType pick_type { get; set; }
    public APickDirection()
    {
        pick_specified_type = true;
        pick_type = ObjectType.ImportedObject;
    }
}

public class AOnDisconnect
{
    [Category("Mandatory")]
    public bool exit { get; set; }
}

public class AOptionalItemId
{
    [Category("Optional")]
    public int id { get; set; }
    public AOptionalItemId()
    {
        id = -1;
    }
}

public class AOperationCopy
{
    [Category("Optional")]
    public int id { get; set; }
    [Category("Optional")]
    public int source { get; set; }
    [Category("Optional")]
    public string name { get; set; }
    [Category("Optional")]
    public string process_name { get; set; }
    [Category("Optional")]
    public bool make_copy_active { get; set; }
    public AOperationCopy()
    {
        id = -1;
        source = -1;
    }
}

public class AOperationCopyFromParent
{
    [Category("Mandatory")]
    public int id { get; set; }
    [Category("Optional")]
    public bool copy_bound_conds { get; set; }
    [Category("Optional")]
    public bool copy_tools { get; set; }
    [Category("Optional")]
    public bool copy_workpiece { get; set; }
    [Category("Optional")]
    public bool inherit_results { get; set; }
    public AOperationCopyFromParent()
    {
        id = -1;
    }
}

public class AOperationParams
{
    [Category("Optional")]
    public int id { get; set; }
    [Category("Optional")]
    public string name { get; set; }
    [Category("Optional")]
    public int parent { get; set; }
    [Category("Optional")]
    public OperationCreationMode creation_mode { get; set; }
    [Category("Optional")]
    public string process_name { get; set; }
    public AOperationParams()
    {
        id = -1;
    }
}

public class AOperationInsert
{
    [Category("Optional")]
    public int id { get; set; }
    [Category("Optional")]
    public string name { get; set; }
    List<int> x_childs = new List<int>();
    [Category("Mandatory")]
    public List<int> childs { get { return x_childs; } }
    public AOperationInsert()
    {
        id = -1;
    }
}

public class APanelSizes
{
    [Category("Optional")]
    public int view_height { get; set; }
    [Category("Optional")]
    public int view_width { get; set; }
    [Category("Optional")]
    public int left_panel_width { get; set; }
    [Category("Optional")]
    public int right_panel_width { get; set; }
    [Category("Optional")]
    public int bottom_panel_height { get; set; }
}

public class ATraceMsg
{
    [Category("Mandatory")]
    public string msg { get; set; }
    [Category("Optional")]
    public MessageType type { get; set; }
}

public class AExportAsScriptOptions
{
    [Category("Mandatory")]
    public string file { get; set; }
    [Category("Mandatory")]
    public OperationSubset operation_subset { get; set; }
    [Category("Optional")]
    public int id { get; set; }
    public AExportAsScriptOptions()
    {
        id = -1;
    }
}

public class AProjectOpenAsCopy
{
    [Category("Mandatory")]
    public string source_path { get; set; }
    [Category("Optional")]
    public string target_path { get; set; }
    [Category("Optional")]
    public bool copy_simulation_results { get; set; }
}

public class AProperty
{
    [Category("Mandatory")]
    public ObjectType object_type { get; set; }
    [Category("Mandatory")]
    public int object_id { get; set; }
    [Category("Mandatory")]
    public string path { get; set; }
    [Category("Mandatory")]
    public PropertyType property_type { get; set; }
    [Category("Mandatory")]
    public string value { get; set; }
    public AProperty()
    {
        object_type = ObjectType.Operation;
        property_type = PropertyType.Value;
    }
}

public class APropertyPath
{
    [Category("Mandatory")]
    public ObjectType object_type { get; set; }
    [Category("Mandatory")]
    public int object_id { get; set; }
    [Category("Mandatory")]
    public string path { get; set; }
    public APropertyPath()
    {
        object_type = ObjectType.Operation;
    }
}

public class APropertyArrayOfReal
{
    [Category("Mandatory")]
    public ObjectType object_type { get; set; }
    [Category("Mandatory")]
    public int object_id { get; set; }
    [Category("Mandatory")]
    public string path { get; set; }
    List<double> x_values = new List<double>();
    [Category("Mandatory")]
    public List<double> values { get { return x_values; } }
    public APropertyArrayOfReal()
    {
        object_type = ObjectType.Operation;
    }
}

public class AObjectIdProperty
{
    [Category("Mandatory")]
    public ObjectType object_type { get; set; }
    [Category("Mandatory")]
    public int object_id { get; set; }
    [Category("Mandatory")]
    public string path { get; set; }
    [Category("Mandatory")]
    public ObjectType value_type { get; set; }
    [Category("Mandatory")]
    public int value_id { get; set; }
    public AObjectIdProperty()
    {
        object_type = ObjectType.Operation;
        value_type = ObjectType.Nothing;
    }
}

public class ASessionId
{
    [Category("Mandatory")]
    public int session_id { get; set; }
}

public class AWindowPosition
{
    [Category("Optional")]
    public int x { get; set; }
    [Category("Optional")]
    public int y { get; set; }
    [Category("Optional")]
    public int width { get; set; }
    [Category("Optional")]
    public int height { get; set; }
    [Category("Mandatory")]
    public bool maximized { get; set; }
}

public class ARecord
{
    [Category("Mandatory")]
    public double record { get; set; }
}

public class ASectionMeshPlane
{
    [Category("Mandatory")]
    public double point_x { get; set; }
    [Category("Mandatory")]
    public double point_y { get; set; }
    [Category("Mandatory")]
    public double point_z { get; set; }
    [Category("Mandatory")]
    public double normal_x { get; set; }
    [Category("Mandatory")]
    public double normal_y { get; set; }
    [Category("Mandatory")]
    public double normal_z { get; set; }
    [Category("Mandatory")]
    public ObjectType object_type { get; set; }
    [Category("Mandatory")]
    public int object_id { get; set; }
    [Category("Optional")]
    public int mesh_index { get; set; }
    [Category("Optional")]
    public bool u_vector_defined { get; set; }
    [Category("Optional")]
    public double u_vector_x { get; set; }
    [Category("Optional")]
    public double u_vector_y { get; set; }
    [Category("Optional")]
    public double u_vector_z { get; set; }
}

public class ASectionPlane3P
{
    [Category("Mandatory")]
    public double x1 { get; set; }
    [Category("Mandatory")]
    public double y1 { get; set; }
    [Category("Mandatory")]
    public double z1 { get; set; }
    [Category("Mandatory")]
    public double x2 { get; set; }
    [Category("Mandatory")]
    public double y2 { get; set; }
    [Category("Mandatory")]
    public double z2 { get; set; }
    [Category("Mandatory")]
    public double x3 { get; set; }
    [Category("Mandatory")]
    public double y3 { get; set; }
    [Category("Mandatory")]
    public double z3 { get; set; }
    [Category("Mandatory")]
    public bool slice_mode { get; set; }
    [Category("Optional")]
    public int id { get; set; }
    [Category("Optional")]
    public double offset { get; set; }
    [Category("Optional")]
    public bool reverse_cut { get; set; }
    public ASectionPlane3P()
    {
        id = -1;
        offset = 0;
    }
}

public class ASectionPlanePN
{
    [Category("Mandatory")]
    public double point_x { get; set; }
    [Category("Mandatory")]
    public double point_y { get; set; }
    [Category("Mandatory")]
    public double point_z { get; set; }
    [Category("Mandatory")]
    public double normal_x { get; set; }
    [Category("Mandatory")]
    public double normal_y { get; set; }
    [Category("Mandatory")]
    public double normal_z { get; set; }
    [Category("Mandatory")]
    public bool slice_mode { get; set; }
    [Category("Optional")]
    public int id { get; set; }
    [Category("Optional")]
    public double offset { get; set; }
    [Category("Optional")]
    public bool reverse_cut { get; set; }
    public ASectionPlanePN()
    {
        id = -1;
        offset = 0;
    }
}

public class ASectionPlaneEnabled
{
    [Category("Mandatory")]
    public int id { get; set; }
    [Category("Mandatory")]
    public bool enabled { get; set; }
}

public class ASectionPlaneVisibility
{
    [Category("Mandatory")]
    public int id { get; set; }
    [Category("Mandatory")]
    public bool visible { get; set; }
}

public class ASleepTime
{
    [Category("Mandatory")]
    public double seconds { get; set; }
}

public class AOptionalGlobalItemId
{
    [Category("Optional")]
    public int id { get; set; }
    [Category("Optional")]
    public int operation { get; set; }
    public AOptionalGlobalItemId()
    {
        id = -1;
        operation = -1;
    }
}

public class ASystemStateId
{
    [Category("Optional")]
    public double record { get; set; }
    public ASystemStateId()
    {
        record = -1;
    }
}

public class AMeshStateId
{
    [Category("Optional")]
    public double record { get; set; }
    [Category("Mandatory")]
    public int id { get; set; }
    [Category("Mandatory")]
    public ObjectType type { get; set; }
    [Category("Mandatory")]
    public int mesh_index { get; set; }
    public AMeshStateId()
    {
        record = -1;
    }
}

public class AToolStateId
{
    [Category("Optional")]
    public double record { get; set; }
    [Category("Mandatory")]
    public int id { get; set; }
    [Category("Mandatory")]
    public ObjectType type { get; set; }
    public AToolStateId()
    {
        record = -1;
    }
}

public class AWorkpieceStateId
{
    [Category("Optional")]
    public double record { get; set; }
    [Category("Mandatory")]
    public int object_id { get; set; }
    public AWorkpieceStateId()
    {
        record = -1;
    }
}

public class AStopCondParams
{
    [Category("Optional")]
    public int id { get; set; }
    [Category("Mandatory")]
    public StopCondType type { get; set; }
    public AStopCondParams()
    {
        id = -1;
    }
}

public class AStopCondDistance
{
    [Category("Optional")]
    public int id { get; set; }
    [Category("Mandatory")]
    public double distance { get; set; }
    [Category("Mandatory")]
    public ObjectType object_1_type { get; set; }
    [Category("Mandatory")]
    public int object_1_id { get; set; }
    [Category("Mandatory")]
    public ObjectType object_2_type { get; set; }
    [Category("Mandatory")]
    public int object_2_id { get; set; }
    public AStopCondDistance()
    {
        id = -1;
        object_1_type = ObjectType.Nothing;
        object_2_type = ObjectType.Nothing;
    }
}

public class AStopCondFinPos
{
    [Category("Optional")]
    public int id { get; set; }
    [Category("Mandatory")]
    public ObjectType tool_type { get; set; }
    [Category("Mandatory")]
    public int tool_number { get; set; }
    public AStopCondFinPos()
    {
        id = -1;
        tool_type = ObjectType.Nothing;
    }
}

public class AStopCondMaxLoad
{
    [Category("Optional")]
    public int id { get; set; }
    [Category("Mandatory")]
    public ObjectType tool_type { get; set; }
    [Category("Mandatory")]
    public int tool_number { get; set; }
    [Category("Mandatory")]
    public double max_load { get; set; }
    public AStopCondMaxLoad()
    {
        id = -1;
        tool_type = ObjectType.Nothing;
    }
}

public class AStopCondProfileLength
{
    [Category("Optional")]
    public int id { get; set; }
    [Category("Mandatory")]
    public double profile_length { get; set; }
    public AStopCondProfileLength()
    {
        id = -1;
    }
}

public class AStopCondRotation
{
    [Category("Optional")]
    public int id { get; set; }
    [Category("Mandatory")]
    public ObjectType tool_type { get; set; }
    [Category("Mandatory")]
    public int tool_number { get; set; }
    [Category("Mandatory")]
    public double angle { get; set; }
    [Category("Mandatory")]
    public int axis { get; set; }
    public AStopCondRotation()
    {
        id = -1;
        tool_type = ObjectType.Nothing;
    }
}

public class AStopCondStroke
{
    [Category("Optional")]
    public int id { get; set; }
    [Category("Mandatory")]
    public ObjectType tool_type { get; set; }
    [Category("Mandatory")]
    public int tool_number { get; set; }
    [Category("Mandatory")]
    public double stroke { get; set; }
    public AStopCondStroke()
    {
        id = -1;
        tool_type = ObjectType.Nothing;
    }
}

public class AStopCondTime
{
    [Category("Optional")]
    public int id { get; set; }
    [Category("Mandatory")]
    public double time { get; set; }
    public AStopCondTime()
    {
        id = -1;
    }
}

public class AStopCondFieldValue
{
    [Category("Optional")]
    public int id { get; set; }
    [Category("Mandatory")]
    public ObjectType object_type { get; set; }
    [Category("Mandatory")]
    public int object_id { get; set; }
    [Category("Mandatory")]
    public double bound_value { get; set; }
    [Category("Mandatory")]
    public SolidBodyNodeValueCondition_ValueType bound_type { get; set; }
    [Category("Mandatory")]
    public SolidBodyNodeValueCondition_ValueRegionInBody region_in_object { get; set; }
    public AStopCondFieldValue()
    {
        id = -1;
        object_type = ObjectType.Nothing;
    }
}

public class ASubroutineCreate
{
    [Category("Optional")]
    public int id { get; set; }
    [Category("Mandatory")]
    public SubRoutineType type { get; set; }
    [Category("Mandatory")]
    public string file { get; set; }
    [Category("Mandatory")]
    public bool solve_in_process { get; set; }
    public ASubroutineCreate()
    {
        id = -1;
        type = SubRoutineType.lua;
    }
}

public class ASubroutineParameter
{
    [Category("Mandatory")]
    public int id { get; set; }
    [Category("Mandatory")]
    public string parameter { get; set; }
    [Category("Mandatory")]
    public double value { get; set; }
}

public class ASymPlaneParams
{
    [Category("Optional")]
    public int id { get; set; }
    [Category("Mandatory")]
    public double point_x { get; set; }
    [Category("Mandatory")]
    public double point_y { get; set; }
    [Category("Mandatory")]
    public double point_z { get; set; }
    [Category("Mandatory")]
    public double normal_x { get; set; }
    [Category("Mandatory")]
    public double normal_y { get; set; }
    [Category("Mandatory")]
    public double normal_z { get; set; }
    public ASymPlaneParams()
    {
        id = -1;
    }
}

public class ASymPlaneByPoint
{
    [Category("Optional")]
    public int id { get; set; }
    [Category("Mandatory")]
    public double point_x { get; set; }
    [Category("Mandatory")]
    public double point_y { get; set; }
    [Category("Mandatory")]
    public double point_z { get; set; }
    [Category("Optional")]
    public ObjectType object_type { get; set; }
    [Category("Optional")]
    public int object_id { get; set; }
    public ASymPlaneByPoint()
    {
        id = -1;
    }
}

public class ASymPlaneByColor
{
    [Category("Optional")]
    public int id { get; set; }
    [Category("Mandatory")]
    public int color_R { get; set; }
    [Category("Mandatory")]
    public int color_G { get; set; }
    [Category("Mandatory")]
    public int color_B { get; set; }
    [Category("Optional")]
    public ObjectType object_type { get; set; }
    [Category("Optional")]
    public int object_id { get; set; }
    public ASymPlaneByColor()
    {
        id = -1;
    }
}

public class AUnitSystem
{
    [Category("Mandatory")]
    public SystemOfUnits system { get; set; }
    public AUnitSystem()
    {
        system = SystemOfUnits.SI;
    }
}

public class ANewTrackingContour
{
    [Category("Optional")]
    public int index { get; set; }
    [Category("Mandatory")]
    public int tracking_contours_id { get; set; }
    List<double> x_point_x = new List<double>();
    [Category("Mandatory")]
    public List<double> point_x { get { return x_point_x; } }
    List<double> x_point_y = new List<double>();
    [Category("Mandatory")]
    public List<double> point_y { get { return x_point_y; } }
    List<double> x_point_z = new List<double>();
    [Category("Mandatory")]
    public List<double> point_z { get { return x_point_z; } }
    public ANewTrackingContour()
    {
        index = -1;
    }
}

public class AOptionalObjectItemId
{
    [Category("Optional")]
    public int id { get; set; }
    [Category("Mandatory")]
    public ObjectType object_type { get; set; }
    [Category("Mandatory")]
    public int object_id { get; set; }
    public AOptionalObjectItemId()
    {
        id = -1;
        object_type = ObjectType.Nothing;
    }
}

public class ATrackingGroup
{
    [Category("Optional")]
    public int id { get; set; }
    [Category("Mandatory")]
    public string name { get; set; }
}

public class ATrackingLineParams
{
    [Category("Optional")]
    public int id { get; set; }
    [Category("Mandatory")]
    public ObjectType object_type { get; set; }
    [Category("Mandatory")]
    public int object_id { get; set; }
    [Category("Mandatory")]
    public double point1_x { get; set; }
    [Category("Mandatory")]
    public double point1_y { get; set; }
    [Category("Mandatory")]
    public double point1_z { get; set; }
    [Category("Mandatory")]
    public double point2_x { get; set; }
    [Category("Mandatory")]
    public double point2_y { get; set; }
    [Category("Mandatory")]
    public double point2_z { get; set; }
    [Category("Mandatory")]
    public bool on_surface { get; set; }
    public ATrackingLineParams()
    {
        id = -1;
    }
}

public class AGlobalItemId
{
    [Category("Mandatory")]
    public int id { get; set; }
    [Category("Optional")]
    public int operation { get; set; }
    public AGlobalItemId()
    {
        operation = -1;
    }
}

public class ATrackingPointParams
{
    [Category("Optional")]
    public int id { get; set; }
    [Category("Mandatory")]
    public ObjectType object_type { get; set; }
    [Category("Mandatory")]
    public int object_id { get; set; }
    [Category("Mandatory")]
    public double point_x { get; set; }
    [Category("Mandatory")]
    public double point_y { get; set; }
    [Category("Mandatory")]
    public double point_z { get; set; }
    [Category("Mandatory")]
    public bool on_surface { get; set; }
    [Category("Optional")]
    public int group { get; set; }
    public ATrackingPointParams()
    {
        id = -1;
    }
}

public class AScale
{
    [Category("Mandatory")]
    public double scale { get; set; }
    public AScale()
    {
        scale = 1;
    }
}

public class RDbTreeRet : Ret
{
    public List<RDbTreeRet> childs { get; set; }
    public string name { get; set; }
    public string vname { get; set; }
    public int type { get; set; }
    public int bid { get; set; }
}

public class RFieldId : Ret
{
    public string name { get; set; }
    public string field { get; set; }
    public FieldSource field_source { get; set; }
    public int field_type { get; set; }
    public int field_target { get; set; }
    public int source_object { get; set; }
    public int source_operation { get; set; }
    public double field_min { get; set; }
    public double field_max { get; set; }
    public int units { get; set; }
}

public class RItemId : Ret
{
    public int id { get; set; }
}

public class RAssembledTool : Ret
{
    public int id { get; set; }
    public List<int> parts { get; set; }
}

public class RItemList : Ret
{
    public List<int> objects { get; set; }
}

public class RSimulationResult : Ret
{
    public StatusCode status { get; set; }
}

public class RAsyncState : Ret
{
    public bool working { get; set; }
    public SimulationStage simulation_stage { get; set; }
    public int stage_time { get; set; }
    public int stage_counter { get; set; }
    public double record { get; set; }
    public int operation { get; set; }
    public int blow { get; set; }
}

public class RAsyncEvent : Ret
{
    public bool working { get; set; }
    public AsyncEventType type { get; set; }
    public CalculationUnit finished_calculation_unit { get; set; }
    public SimulationStage simulation_stage { get; set; }
    public int stage_time { get; set; }
    public int stage_counter { get; set; }
    public double record { get; set; }
    public double progress { get; set; }
    public double process_time { get; set; }
    public string diagnostic_msg { get; set; }
    public MessageType diagnostic_msg_type { get; set; }
    public int diagnostic_msg_code { get; set; }
    public int iteration { get; set; }
    public double iteration_velocity_norm { get; set; }
    public double iteration_mean_stress_norm { get; set; }
    public int iteration_separated_nodes { get; set; }
    public int iteration_sticked_nodes { get; set; }
    public int operation { get; set; }
    public int blow { get; set; }
    public bool backward { get; set; }
    public string units { get; set; }
}

public class RFieldIdList : Ret
{
    public List<RFieldId> fields { get; set; }
}

public class RObjectAxis : Ret
{
    public bool defined { get; set; }
    public bool inherited { get; set; }
    public double point1_x { get; set; }
    public double point1_y { get; set; }
    public double point1_z { get; set; }
    public double point2_x { get; set; }
    public double point2_y { get; set; }
    public double point2_z { get; set; }
    public string units { get; set; }
}

public class RCount : Ret
{
    public int count { get; set; }
}

public class RNullableRealValue : Ret
{
    public double value { get; set; }
    public bool is_null { get; set; }
    public string units { get; set; }
}

public class RBoundCondType : Ret
{
    public BCond type { get; set; }
}

public class RCameraDirection : Ret
{
    public double forward_dir_x { get; set; }
    public double forward_dir_y { get; set; }
    public double forward_dir_z { get; set; }
    public double up_dir_x { get; set; }
    public double up_dir_y { get; set; }
    public double up_dir_z { get; set; }
}

public class RChart : Ret
{
    public List<double> arg_value { get; set; }
    public List<bool> arg_has_value { get; set; }
    public List<double> func_value { get; set; }
    public List<bool> func_has_value { get; set; }
    public string func_units { get; set; }
    public string arg_units { get; set; }
    public string arg_name { get; set; }
    public string func_name { get; set; }
}

public class RChartValue : Ret
{
    public double arg_value { get; set; }
    public double func_value { get; set; }
    public bool arg_has_value { get; set; }
    public bool func_has_value { get; set; }
    public double record { get; set; }
    public string func_units { get; set; }
    public string arg_units { get; set; }
    public string arg_name { get; set; }
    public string func_name { get; set; }
}

public class RWebAddress : Ret
{
    public string url { get; set; }
}

public class RExecutionStatus : Ret
{
    public bool successful { get; set; }
    public string msg { get; set; }
}

public class RContactArea : Ret
{
    public double contact_area { get; set; }
    public double total_area { get; set; }
    public string units { get; set; }
}

public class RField : Ret
{
    public List<double> values { get; set; }
    public List<bool> has_data { get; set; }
    public bool only_on_surface { get; set; }
    public bool in_elements { get; set; }
    public string units { get; set; }
}

public class RDbArbitraryDriveRecords : Ret
{
    public List<RDbArbitraryDriveRecord> records { get; set; }
}

public class RDbArbitraryDriveRecord : Ret
{
    public double t { get; set; }
    public double x { get; set; }
    public double y { get; set; }
    public double z { get; set; }
    public double rx { get; set; }
    public double ry { get; set; }
    public double rz { get; set; }
    public string units { get; set; }
}

public class RDbItem : Ret
{
    public string name { get; set; }
    public string db_path { get; set; }
    public List<RDbItem> childs { get; set; }
}

public class RBoolValue : Ret
{
    public bool value { get; set; }
}

public class RPropertyValue : Ret
{
    public PropertyType property_type { get; set; }
    public string value { get; set; }
    public string units { get; set; }
}

public class RDbPropertyTable : Ret
{
    public string db_path { get; set; }
    public string prop_path { get; set; }
    public List<double> row_arg_values { get; set; }
    public List<double> column_arg_values { get; set; }
    public List<double> layer_arg_values { get; set; }
    public DbTableArg row_arg { get; set; }
    public DbTableArg column_arg { get; set; }
    public DbTableArg layer_arg { get; set; }
    public List<double> values { get; set; }
    public string units { get; set; }
}

public class RDomainType : Ret
{
    public Domain type { get; set; }
}

public class RContours : Ret
{
    public List<RContour> contours { get; set; }
}

public class RContour : Ret
{
    public string name { get; set; }
    public List<double> point_x { get; set; }
    public List<double> point_y { get; set; }
    public List<double> point_z { get; set; }
    public bool closed { get; set; }
    public string units { get; set; }
}

public class RBearingZ : Ret
{
    public double z_min { get; set; }
    public double z_max { get; set; }
    public string units { get; set; }
}

public class RRealValue : Ret
{
    public double value { get; set; }
    public string units { get; set; }
}

public class RTrace : Ret
{
    public int number { get; set; }
    public List<RTraceSubroutineField> subroutine_fields { get; set; }
    public List<RTracePoint> points { get; set; }
    public string units { get; set; }
}

public class RTraceSubroutineField : Ret
{
    public string subroutine { get; set; }
    public int subroutine_id { get; set; }
    public string field { get; set; }
    public string units { get; set; }
    public int value_index { get; set; }
}

public class RTracePoint : Ret
{
    public double time { get; set; }
    public double point_path { get; set; }
    public double point_x { get; set; }
    public double point_y { get; set; }
    public double point_z { get; set; }
    public double velocity { get; set; }
    public double velocity_x { get; set; }
    public double velocity_y { get; set; }
    public double velocity_z { get; set; }
    public double temperature { get; set; }
    public double mean_stress { get; set; }
    public double effective_stress { get; set; }
    public double plastic_strain { get; set; }
    public double strain_rate { get; set; }
    public List<double> subroutine_fields { get; set; }
    public int node_1 { get; set; }
    public int node_2 { get; set; }
    public int node_3 { get; set; }
    public int node_4 { get; set; }
    public double node_1_weigth { get; set; }
    public double node_2_weigth { get; set; }
    public double node_3_weigth { get; set; }
    public double node_4_weigth { get; set; }
}

public class RVectorValue : Ret
{
    public double x { get; set; }
    public double y { get; set; }
    public double z { get; set; }
    public string units { get; set; }
}

public class RRealValues : Ret
{
    public List<double> values { get; set; }
    public string units { get; set; }
}

public class RVectorValues : Ret
{
    public List<double> x { get; set; }
    public List<double> y { get; set; }
    public List<double> z { get; set; }
    public string units { get; set; }
}

public class RVectorField : Ret
{
    public List<double> x { get; set; }
    public List<double> y { get; set; }
    public List<double> z { get; set; }
    public List<bool> has_data { get; set; }
    public bool only_on_surface { get; set; }
    public string units { get; set; }
}

public class RIsolineList : Ret
{
    public List<RIsoline> isolines { get; set; }
}

public class RIsoline : Ret
{
    public List<double> coord_x { get; set; }
    public List<double> coord_z { get; set; }
    public List<int> source_node_1 { get; set; }
    public List<int> source_node_2 { get; set; }
    public List<double> intersection_t { get; set; }
    public bool closed { get; set; }
}

public class RIsosurfaceList : Ret
{
    public List<RIsosurface> isosurfaces { get; set; }
}

public class RIsosurface : Ret
{
    public List<double> coord_x { get; set; }
    public List<double> coord_y { get; set; }
    public List<double> coord_z { get; set; }
    public List<int> source_node_1 { get; set; }
    public List<int> source_node_2 { get; set; }
    public List<double> intersection_t { get; set; }
    public List<int> triangle_node_1 { get; set; }
    public List<int> triangle_node_2 { get; set; }
    public List<int> triangle_node_3 { get; set; }
    public string units { get; set; }
}

public class RFieldMinMax : Ret
{
    public double min_value { get; set; }
    public double min_x { get; set; }
    public double min_y { get; set; }
    public double min_z { get; set; }
    public int min_node { get; set; }
    public double max_value { get; set; }
    public double max_x { get; set; }
    public double max_y { get; set; }
    public double max_z { get; set; }
    public int max_node { get; set; }
    public bool has_values { get; set; }
    public string units { get; set; }
}

public class RFieldMode : Ret
{
    public FillMode mode { get; set; }
}

public class RFieldPalette : Ret
{
    public Colormap palette { get; set; }
    public bool inverse { get; set; }
}

public class RFieldStat : Ret
{
    public double area { get; set; }
    public double volume { get; set; }
    public double min_value { get; set; }
    public double max_value { get; set; }
    public double mean_value { get; set; }
    public double standart_deviation { get; set; }
    public double median { get; set; }
    public double coefficient_of_skewness { get; set; }
    public double excess_kurtosis { get; set; }
    public double percentile_1 { get; set; }
    public double percentile_2 { get; set; }
    public List<double> histogram_field { get; set; }
    public List<double> histogram_level { get; set; }
    public string units { get; set; }
}

public class RStringList : Ret
{
    public List<string> items { get; set; }
}

public class RObjectList : Ret
{
    public List<RObjectId> objects { get; set; }
}

public class RObjectId : Ret
{
    public ObjectType type { get; set; }
    public int id { get; set; }
}

public class RKeyNames : Ret
{
    public List<string> names { get; set; }
}

public class RQFormLang : Ret
{
    public Language language { get; set; }
}

public class RMeshApex : Ret
{
    public int node { get; set; }
    public List<RMeshApexEdge> edges { get; set; }
}

public class RMeshApexEdge : Ret
{
    public int edge { get; set; }
    public bool is_start { get; set; }
}

public class RMeshCubics : Ret
{
    public List<int> node_1 { get; set; }
    public List<int> node_2 { get; set; }
    public List<int> node_3 { get; set; }
    public List<int> node_4 { get; set; }
    public List<int> node_5 { get; set; }
    public List<int> node_6 { get; set; }
    public List<int> node_7 { get; set; }
    public List<int> node_8 { get; set; }
}

public class RMeshEdge : Ret
{
    public List<int> nodes { get; set; }
    public int apex_start { get; set; }
    public int apex_end { get; set; }
    public int adjacent_face_1 { get; set; }
    public int adjacent_face_2 { get; set; }
    public int adjacent_face_1_bound { get; set; }
    public int adjacent_face_2_bound { get; set; }
}

public class RMeshFace : Ret
{
    public FaceType type { get; set; }
    public List<int> triangles { get; set; }
    public List<int> quadrangles { get; set; }
    public List<int> inner_nodes { get; set; }
    public List<int> bound_nodes { get; set; }
    public List<RMeshFaceBound> bounds { get; set; }
}

public class RMeshFaceBound : Ret
{
    public List<int> nodes { get; set; }
    public List<int> edges { get; set; }
    public List<bool> edge_directions { get; set; }
}

public class RFaceTypes : Ret
{
    public List<FaceType> type { get; set; }
}

public class RMeshCoords : Ret
{
    public List<double> x { get; set; }
    public List<double> y { get; set; }
    public List<double> z { get; set; }
    public string units { get; set; }
}

public class RMeshNodeOwners : Ret
{
    public List<MeshNodeOwnerType> owner_type { get; set; }
    public List<int> owner_id { get; set; }
}

public class RMeshPoint : Ret
{
    public double x { get; set; }
    public double y { get; set; }
    public double z { get; set; }
    public int mesh_index { get; set; }
    public bool on_surface { get; set; }
    public List<int> node { get; set; }
    public List<double> node_weight { get; set; }
    public string units { get; set; }
    public bool not_defined { get; set; }
}

public class RMeshProperties : Ret
{
    public int dim { get; set; }
    public int element_count_volumetric { get; set; }
    public int element_count_surface { get; set; }
    public int node_count { get; set; }
    public int node_count_internal { get; set; }
    public int node_count_surface { get; set; }
    public int triangle_count { get; set; }
    public int triangle_quad_count { get; set; }
    public int quadrangle_count { get; set; }
    public int tetrahedron_count { get; set; }
    public int cubic_count { get; set; }
    public int face_count { get; set; }
    public int edge_count { get; set; }
    public int apex_count { get; set; }
}

public class RMeshQuadrangles : Ret
{
    public List<int> node_1 { get; set; }
    public List<int> node_2 { get; set; }
    public List<int> node_3 { get; set; }
    public List<int> node_4 { get; set; }
}

public class RMeshTetrahedrons : Ret
{
    public List<int> node_1 { get; set; }
    public List<int> node_2 { get; set; }
    public List<int> node_3 { get; set; }
    public List<int> node_4 { get; set; }
}

public class RMeshTriangles : Ret
{
    public List<int> node_1 { get; set; }
    public List<int> node_2 { get; set; }
    public List<int> node_3 { get; set; }
}

public class RMouseClick : Ret
{
    public MouseButton button { get; set; }
    public int coord_x { get; set; }
    public int coord_y { get; set; }
    public bool ctrl { get; set; }
    public bool alt { get; set; }
    public bool shift { get; set; }
}

public class RMousePos : Ret
{
    public int coord_x { get; set; }
    public int coord_y { get; set; }
}

public class RPressedDialogButton : Ret
{
    public DialogButton button { get; set; }
}

public class RMultiThreadingSettings : Ret
{
    public int computation_thread_count { get; set; }
    public int computation_thread_max_count { get; set; }
    public bool computation_reproducibility { get; set; }
}

public class RObjectName : Ret
{
    public string name { get; set; }
}

public class RObjectTransform : Ret
{
    public ObjectType object_type { get; set; }
    public int object_id { get; set; }
    public List<double> matrix4x4 { get; set; }
}

public class ROperationChecks : Ret
{
    public List<string> errors { get; set; }
    public List<string> warnings { get; set; }
}

public class ROperation : Ret
{
    public int id { get; set; }
    public OperationType type { get; set; }
    public string name { get; set; }
    public string dsc { get; set; }
    public List<ROperation> childs { get; set; }
}

public class ROperationGraph : Ret
{
    public bool parent_defined { get; set; }
    public int parent { get; set; }
    public List<int> childs { get; set; }
}

public class RPanelPositions : Ret
{
    public int frame_x { get; set; }
    public int frame_y { get; set; }
    public int view_x { get; set; }
    public int view_y { get; set; }
    public int view_width { get; set; }
    public int view_height { get; set; }
    public int left_panel_x { get; set; }
    public int left_panel_y { get; set; }
    public int left_panel_width { get; set; }
    public int left_panel_height { get; set; }
    public int right_panel_x { get; set; }
    public int right_panel_y { get; set; }
    public int right_panel_width { get; set; }
    public int right_panel_height { get; set; }
    public int bottom_panel_x { get; set; }
    public int bottom_panel_y { get; set; }
    public int bottom_panel_width { get; set; }
    public int bottom_panel_height { get; set; }
}

public class RPanelSizes : Ret
{
    public int view_height { get; set; }
    public int view_width { get; set; }
    public int left_panel_width { get; set; }
    public int right_panel_width { get; set; }
    public int bottom_panel_height { get; set; }
}

public class RFileName : Ret
{
    public string file { get; set; }
}

public class RArrayOfReal : Ret
{
    public List<double> values { get; set; }
    public string units { get; set; }
}

public class RProcessId : Ret
{
    public int pid { get; set; }
}

public class RQFormVer : Ret
{
    public int v1 { get; set; }
    public int v2 { get; set; }
    public int v3 { get; set; }
    public int v4 { get; set; }
    public bool is_cloud { get; set; }
    public bool is_viewer { get; set; }
    public string configuration { get; set; }
}

public class RWindowId : Ret
{
    public int hwnd { get; set; }
}

public class RWindowPosition : Ret
{
    public int x { get; set; }
    public int y { get; set; }
    public int width { get; set; }
    public int height { get; set; }
    public bool maximized { get; set; }
}

public class RRecord : Ret
{
    public double record { get; set; }
}

public class RSectionMeshList : Ret
{
    public List<RSectionMesh> meshes { get; set; }
    public double area { get; set; }
    public string units { get; set; }
}

public class RSectionMeshNodes : Ret
{
    public List<double> coord_x { get; set; }
    public List<double> coord_y { get; set; }
    public List<double> coord_z { get; set; }
    public List<double> coord_u { get; set; }
    public List<double> coord_v { get; set; }
    public List<int> source_node_1 { get; set; }
    public List<int> source_node_2 { get; set; }
    public List<double> intersection_t { get; set; }
    public double u_vector_x { get; set; }
    public double u_vector_y { get; set; }
    public double u_vector_z { get; set; }
    public double v_vector_x { get; set; }
    public double v_vector_y { get; set; }
    public double v_vector_z { get; set; }
    public double uv_origin_x { get; set; }
    public double uv_origin_y { get; set; }
    public double uv_origin_z { get; set; }
    public string units { get; set; }
}

public class RSectionMeshTriangles : Ret
{
    public List<int> node_1 { get; set; }
    public List<int> node_2 { get; set; }
    public List<int> node_3 { get; set; }
}

public class RSectionMeshBound : Ret
{
    public List<int> nodes { get; set; }
    public List<bool> nodes_is_on_edge { get; set; }
    public double length { get; set; }
    public double area { get; set; }
    public string units { get; set; }
}

public class RSectionMesh : Ret
{
    public RSectionMeshNodes nodes { get; set; }
    public RSectionMeshTriangles triangles { get; set; }
    public List<RSectionMeshBound> bounds { get; set; }
    public double area { get; set; }
    public string units { get; set; }
}

public class RSectionPlane : Ret
{
    public double point_x { get; set; }
    public double point_y { get; set; }
    public double point_z { get; set; }
    public double normal_x { get; set; }
    public double normal_y { get; set; }
    public double normal_z { get; set; }
    public bool defined { get; set; }
    public string units { get; set; }
}

public class RSession : Ret
{
    public int session_id { get; set; }
    public int process_id { get; set; }
    public bool is_api_connected { get; set; }
    public bool is_started_by_api { get; set; }
    public string last_api_connected_app { get; set; }
    public int last_api_connected_app_pid { get; set; }
    public int window_id { get; set; }
    public double last_window_activation_time { get; set; }
    public double last_window_deactivation_time { get; set; }
    public bool is_visible { get; set; }
    public bool is_simulation_running { get; set; }
    public string opened_project { get; set; }
    public string qform_dir { get; set; }
}

public class RSessionList : Ret
{
    public List<RSession> sessions { get; set; }
}

public class RMainSimulationResult : Ret
{
    public StatusCode status { get; set; }
    public CalculationUnit finished_calculation_unit { get; set; }
}

public class RSimulationState : Ret
{
    public bool simulation_completed { get; set; }
    public int process_chain { get; set; }
    public int operation_index_in_chain { get; set; }
    public int operation { get; set; }
    public int blow { get; set; }
    public int blow_record_count { get; set; }
    public double blow_progress { get; set; }
    public string units { get; set; }
}

public class RExtrusionState : Ret
{
    public double force { get; set; }
    public double fill_time { get; set; }
    public double euler_min_z { get; set; }
    public string units { get; set; }
}

public class RMeshState : Ret
{
    public int volumetric_elements { get; set; }
    public int surface_elements { get; set; }
    public int internal_nodes { get; set; }
    public int surface_nodes { get; set; }
    public int total_nodes { get; set; }
    public int dim { get; set; }
    public double record { get; set; }
}

public class RSystemState : Ret
{
    public double record { get; set; }
    public double time { get; set; }
    public double process_time { get; set; }
    public double time_step { get; set; }
    public string units { get; set; }
    public int iteration_count { get; set; }
    public double calculation_time { get; set; }
    public double total_calculation_time { get; set; }
    public double progress { get; set; }
}

public class RToolState : Ret
{
    public double record { get; set; }
    public double load { get; set; }
    public double energy { get; set; }
    public double displacement { get; set; }
    public double power { get; set; }
    public double torque_1 { get; set; }
    public double torque_2 { get; set; }
    public double work { get; set; }
    public double velocity { get; set; }
    public string units { get; set; }
}

public class RWorkpieceState : Ret
{
    public double record { get; set; }
    public double volume { get; set; }
    public string units { get; set; }
    public bool with_laps { get; set; }
}

public class RStopCond : Ret
{
    public StopCondType type { get; set; }
}

public class RSubroutine : Ret
{
    public List<int> used_subroutines { get; set; }
    public List<string> parameters { get; set; }
    public List<string> input_fields { get; set; }
    public List<string> output_fields { get; set; }
    public SubRoutineType type { get; set; }
    public string file { get; set; }
    public bool solve_in_process { get; set; }
}

public class RUnitVector : Ret
{
    public double vector_x { get; set; }
    public double vector_y { get; set; }
    public double vector_z { get; set; }
    public double point_x { get; set; }
    public double point_y { get; set; }
    public double point_z { get; set; }
    public string units { get; set; }
}

public class RUnitSystem : Ret
{
    public SystemOfUnits system { get; set; }
}

public class RTrackingLine : Ret
{
    public List<RMeshPoint> points { get; set; }
}

public class RGlobalItemList : Ret
{
    public List<RGlobalItemId> objects { get; set; }
}

public class RGlobalItemId : Ret
{
    public int id { get; set; }
    public int operation { get; set; }
}

public class RViewOverallDimensions : Ret
{
    public double height { get; set; }
    public double width { get; set; }
    public string units { get; set; }
}

public class RPathName : Ret
{
    public string path { get; set; }
}

public enum BilletParam
{
    BilletTemperature = 8,
    TemperatureTaper = 9,
    VelocityValue = 10,
    ProfileVelocity = 11,
    BilletLength = 12,
    BilletToBilletPause = 13,
    MaxStroke = 14,
    ButtEndLength = 15,
}

public enum BlowParam
{
    StopConditionValue = 1,
    EnergyShare = 2,
    CoolingInAir = 3,
    CoolingInTool = 4,
    Feed = 5,
    VerticalMovement = 7,
    Rotation = 6,
    RollingToolMotion = 16,
    UpperToolRotationSpeed = 17,
    LowerToolRotationSpeed = 18,
    SideToolRotationSpeed = 19,
    UpperToolCrosswiseMovement = 20,
}

public enum ObjectType
{
    Nothing = 0,
    Workpiece = 100,
    Tool = 150,
    AssembledTool = 50,
    SymmetryPlane = 800,
    BoundaryCondition = 450,
    Domain = 650,
    StopCondition = 500,
    Subroutine = 900,
    Operation = 600,
    Process = 601,
    ImportedObject = 10,
    MainRoll = 201,
    RollMandrell = 202,
    AxialRoll = 203,
    GuideRoll = 204,
    RollPlate = 205,
    RollEdger = 231,
    PressureRoll = 232,
    ClippingSurface = 401,
    TurningSurface = 402,
    TracingPoint = 701,
    TracingLines = 702,
    TracingBox = 703,
    TracingSurfLines = 704,
    TracingContours = 705,
    ExtrWorkpiece = 301,
    ExtrToolMesh = 302,
    ExtrTool = 303,
    ExtrDieholder = 304,
    ExtrMandrell = 305,
    ExtrDieplace = 306,
    ExtrBacker = 307,
    ExtrBolster = 308,
    ExtrSpreader = 309,
    ExtrSeparator = 310,
    ElectroPusher = 351,
    ElectroAnvil = 352,
    ElectroClamp = 353,
    CrossRollingTool = 851,
    CrossRollingMandrelTool = 852,
    CrossRollingLinearTool = 853,
    CrossRollingFreeRotatedTool = 854,
    Blow = 550,
    Billet = 551,
    Pass = 552,
    Internal = 750,
    SpringBetweenTools = 950,
}

public enum DisplayModes
{
    visible = 1,
    show_mesh = 4,
    transparent = 16384,
    show_laps = 65536,
    show_geometric_mesh = 1024,
}

public enum PropertyType
{
    Selector = 0,
    Value = 1,
    DbObject = 2,
    Automatic = 5,
    Special = 7,
    Inherited = 4,
    ConstSpeed = 11,
    FreeRotation = 12,
}

public enum FieldSource
{
    MainSimulation = 2,
    ToolSimulation = 4,
    ToolCoupledSimulation = 5,
    Subroutine = 6,
    PhaseTransformations = 11,
    Sprayer = 12,
    Hardness = 13,
    Concentration = 14,
    Inductor = 15,
}

public enum CalculationUnit
{
    Nothing = 0,
    Chain = 1,
    Operation = 2,
    Blow = 4,
    Billet = 12,
    Pass = 20,
    Records = 32,
    ProcessTime = 64,
    CalculationTime = 128,
}

public enum CalculationMode
{
    Chain = 0,
    Operation = 2,
    Blow = 4,
    Billet = 12,
    Pass = 20,
}

public enum FieldGroup
{
    Workpiece = 2,
    Tool = 1,
    TrackingPoints = 4,
}

public enum BCond
{
    Nil = 0,
    Load = 1,
    Velocity = 2,
    Temperature = 42,
    HeatFlow = 6,
    HeatFlow_s = 7,
    HeatFlow_v = 8,
    Fixing = 9,
    Normal = 10,
    Bearing = 11,
    Ring = 12,
    Fit = 14,
    Friction = 16,
    Env = 17,
    Fastener = 22,
    Pressure = 27,
    Manipulator = 28,
    Rotation = 29,
    Pusher = 31,
    Sprayer = 32,
    SprayerRectArray = 33,
    SprayerPolarArray = 34,
    SprayerDB = 38,
    SprayerRectArrayDB = 39,
    SprayerPolarArrayDB = 40,
    BodyContour = 35,
    ConstantTemperatureContact = 36,
    Inductor = 37,
    SurfTempPointCloud = 43,
    Dilation = 47,
    Auxiliary = 23,
    ProfileCoolingTable = 48,
}

public enum DbStandart
{
    Default = -1,
    File = 0,
    DIN = 1,
    AISI = 2,
    GOST = 3,
    BS = 4,
    JIS = 5,
    GB = 6,
    ISO = 7,
}

public enum DriveType
{
    Unspecified = 0,
    Fixed = 2,
    Free = 12,
    MechanicalPresse = 4,
    Hydraulic = 5,
    LoadHolder = 7,
    Universal = 6,
    Tabular = 11,
    ScrewPress = 9,
    Hammer = 8,
    UniversalV2 = 15,
    Arbitrary = 16,
}

public enum DbTableArg
{
    Nothing = -1,
    Strain = 18,
    StrainRate = 19,
    Temperature = 10,
    Time = 7,
    Pressure = 38,
    CharacteristicTime = 138,
    Angle = 15,
    Stroke = 57,
    Distance = 131,
    Displacement = 2,
    Thickness = 51,
    Diameter = 52,
    CoolingRate = 48,
    ColumnNumber = 81,
    StressTriaxiality = 133,
    LodeStressParameter = 134,
    NormalizedMaxPrincipalStress = 135,
    MinorTrueStrain = 150,
    DwellTemperature = 204,
    InitialGrainSize = 203,
    DeformationTemperature = 202,
    LiquidFluxDensity = 163,
    SurfaceTemperature = 164,
}

public enum LogFormat
{
    FromFileExtension = 0,
    XML = 520,
    SExpr = 518,
}

public enum Domain
{
    Nil = 0,
    Friction = 16,
    MeshAdaptation = 41,
    MeshAdaptationTool = 18,
    MeshAdaptationWorkpiece = 19,
    GeometricalMeshAdaptation = 30,
}

public enum BearingContoursFormat
{
    FromFileExtension = 0,
    DXF = 504,
}

public enum IsolinesFormat
{
    FromFileExtension = 0,
    DXF = 504,
}

public enum SectionFormat
{
    FromFileExtension = 0,
    STL = 503,
}

public enum LengthUnit
{
    Auto = 0,
    m = 155,
    mm = 156,
    cm = 157,
    inch = 158,
}

public enum TrackingFieldsFormat
{
    FromFileExtension = 0,
    XLS = 509,
    NPZ = 523,
}

public enum ValuesOnSheet
{
    at_current_time = 5,
    at_time_blow = 1,
    at_time_operation = 2,
    at_time_chain = 3,
    at_point_blow = 4,
    at_point_operation = 6,
    at_point_chain = 7,
}

public enum MeshFormat
{
    FromFileExtension = 0,
    CSV3D = 10,
    CSV2D = 9,
    DXF = 504,
    STL = 503,
    _3MF = 519,
    XLS = 509,
    XLSX = 510,
}

public enum ProfileSectionFormat
{
    FromFileExtension = 0,
    CSV2D = 9,
    DXF = 504,
}

public enum RecordsExportMode
{
    ExportCurrent = 0,
    ExportFromCurrent = 1,
    ExportToCurrent = 2,
}

public enum SectionContourFormat
{
    FromFileExtension = 0,
    DXF = 504,
}

public enum PickTraceBy
{
    TraceNumber = 0,
    NodeNumber = 1,
    SurfaceElementNumber = 2,
}

public enum FillMode
{
    Gradient = 1,
    Discrete = 8,
    Isolines = 2,
    IsolinesMarks = 4,
}

public enum Colormap
{
    Auto = 0,
    Jet = 1,
    Temp = 2,
    Grey = 3,
    Legacy = 4,
    Rainbow = 5,
    Turbo = 6,
    Viridis = 7,
    Plasma = 8,
    Copper = 9,
    RdYlBu = 10,
    Spectral = 11,
    Coolwarm = 12,
    Seismic = 13,
}

public enum HistogramBy
{
    ByNodes = 0,
    ByElements = 1,
}

public enum Language
{
    russian = 0,
    german = 1,
    spanish = 2,
    italian = 3,
    japanese = 4,
    thai = 5,
    chinese = 6,
    chinese_t = 7,
    polish = 8,
    portuguese = 9,
    turkish = 10,
    english = 11,
}

public enum MouseButton
{
    Right = 0,
    Middle = 1,
    Left = 2,
}

public enum Direction
{
    X = 1,
    Y = 2,
    Z = 3,
}

public enum OperationCreationMode
{
    CreateAsNewProcess = 0,
    AddToCurrentChain = 1,
    InsertAfterParent = 2,
}

public enum MessageType
{
    info = 0,
    error = 101,
    warning = 119,
}

public enum OperationSubset
{
    Project = 0,
    Process = 1,
    OperationChain = 2,
    Operation = 3,
}

public enum StopCondType
{
    Distance = 1,
    Time = 2,
    ToolStroke = 3,
    ToolRotationAxis1 = 4,
    ToolRotationAxis2 = 5,
    MaxLoad = 6,
    FinalPosition = 7,
    FieldValue = 8,
    SolverTime = 9,
    ProfileLength = 10,
}

public enum SolidBodyNodeValueCondition_ValueType
{
    MIN_VALUE = 0,
    MAX_VALUE = 1,
}

public enum SolidBodyNodeValueCondition_ValueRegionInBody
{
    ANY_NODE = 0,
    DEFINED_BODY_VOLUME_PERCENT = 1,
    DEFINED_VOLUME_OF_DEFORMATION_ZONE_PERCENT = 2,
}

public enum SubRoutineType
{
    lua = 0,
    wear = 1,
    strain_tensor = 2,
    stress_tensor = 3,
    stress_tensor_tool = 31,
    strain_tensor_tool = 32,
    strain_rate_tensor = 11,
    press_wp = 4,
    press_tool = 15,
    distance_to_surface = 5,
    global_displacement = 6,
    strain_tensor_lagrangian = 12,
    fatigue = 21,
    fatigueDB = 25,
    debug_q = 1000,
    debug_tl = 1005,
    PressCenter = 1008,
    debug_qwp = 1010,
    debug_wp = 901,
    debug_1 = 1001,
    debug_2 = 1002,
    FLDdiagram = 101,
    yield_tensor = 201,
    surface_normal = 1003,
    surface_normal_tool = 1007,
    gartfield = 1004,
    min_rad = 1011,
    slippage = 3101,
    slippage_tool = 3102,
    normal_velocity = 2002,
    track = 3001,
    forgingratio = 3005,
    track3 = 3003,
    outcrop = 3006,
    adaptation_tool = 52,
    adaptation_wp = 51,
    temperature = 61,
    ort = 3002,
    thickness = 55,
    cylindric = 10001,
    cyl_velocity = 10002,
    gtn_component = 10011,
    cylindric_tool = 10005,
    wp_damage = 20000,
    jmak = 20200,
    extr_jmak = 20210,
    extr_streaking_lines = 20201,
    extr_tool_analysis = 21000,
    extr_fill_analysis = 21001,
    extr_overheating = 21002,
    extr_flow_time = 21003,
    extr_seam_quality = 21005,
    test_cubic = 30000,
    velocity_gradient = 22000,
    extr_skin = 21006,
    extr_funnel = 21007,
    extr_seam_quality_ext = 21008,
}

public enum SystemOfUnits
{
    metric = 0,
    english = 1,
    SI = 2,
}

public enum StatusCode
{
    Ok = 0,
    Canceled = 1,
    Failed = 2,
    Working = 3,
    Idle = 4,
}

public enum SimulationStage
{
    Idle = 0,
    Working = 1,
    Preliminary = 2,
    Cooling = 3,
    Thermal = 4,
    Forwarding = 5,
    Remeshing = 6,
    Plastic = 7,
    Cutting = 8,
    Meshing = 9,
    Tool = 10,
    Diffusion = 11,
    GravitationalPos = 12,
}

public enum AsyncEventType
{
    Idle = 0,
    Done = 1,
    NextCalculationUnit = 2,
    NextRecord = 3,
    NextStage = 4,
    Iteration = 5,
    Timeout = 6,
    Error = 7,
    Message = 8,
    Canceled = 9,
}

public enum FaceType
{
    Unspecified = 0,
    SymPlane = 50,
    ExtrContainer = 10,
    ExtrDie = 12,
    ExtrProfile = 4,
    ExtrBearing = 3,
    ExtrPrechamber = 13,
    ExtrProfileFree = 5,
    ExtrRam = 11,
    ExtrToolToContainer = 17,
    ExtrCase = 24,
    ExtrPressureRing = 28,
    ExtrBolsterInner = 26,
    ExtrDieHolder = 14,
}

public enum MeshNodeOwnerType
{
    Face = 2,
    Edge = 1,
    Apex = 4,
    Volume = 8,
}

public enum DialogButton
{
    Ok = 111,
    Cancel = 99,
    Yes = 121,
    No = 110,
    Apply = 97,
    Done = 100,
    Close = 122,
    Retry = 114,
    Skip = 107,
    Ignore = 105,
    Continue = 116,
    SaveAs = 115,
}

public enum OperationType
{
    none = -1,
    electric = 7,
    deformation = 8,
    thermo = 9,
    extrusion = 10,
    ring_rolling = 11,
    wheel_rolling = 13,
    rolling = 14,
    cross_rolling = 15,
    sheet_bulk_forming = 16,
    reverse_rolling = 17,
    cyclic_heating = 18,
    extr_profile_cooling = 19,
}

public enum DFProperty
{
    constant = 1,
    table = 2,
    lua = 3,
}

public enum FitType
{
    clearance = 1,
    interference = 2,
}

public enum MeshAdaptType
{
    none = 0,
    adapt = 1,
    meshsize = 2,
}

public enum PointCloudCoordinateUnits
{
    m = 1,
    mm = 2,
    inch = 3,
}

public enum PointCloudTemperatureUnits
{
    K = 1,
    C = 2,
    F = 3,
}

public enum POSITIONING_TYPE
{
    bring_with_retract = 0,
    bring_without_retract = 1,
    none = 2,
    retract_to_contact = 3,
    bring_with_retract_sync = 4,
}

public enum AxisType
{
    point = 0,
    direction = 1,
}

public enum HEAT_EXCHANGE
{
    winkler = 1,
    joint = 2,
    const_temp = 3,
    none = 0,
}

public enum AxisBlockType
{
    p = 0,
    v = 1,
    x = 2,
    y = 3,
    z = 4,
}

public enum AxisInputMode
{
    manual = 0,
    automatic = 1,
}

public enum ClipType
{
    inner = 0,
    outer = 1,
}

public enum FieldType
{
    elem = 16,
    scalar = 1,
}

public enum ProtectionMode
{
    no = 0,
    close_src = 1,
    encrypted = 2,
}

public enum Scope
{
    public_scope = 0,
    private_scope = 1,
}

public enum ContourHierarchy
{
    outer = 0,
    inner = 1,
}

public enum ContourClipping
{
    trimming = 0,
    clipping = 1,
}

public enum BoxAxis
{
    axis_z = 0,
    axis_y = 1,
    axis_x = 2,
}

public enum StopConditionSolverTimeType
{
    PROCESS = 0,
    OPERATION = 1,
    BLOW = 2,
}

public enum SSTMode
{
    nil = -1,
    source = 0,
    result = 3,
    preparation = 2,
    mesh_preparation = 1,
}

public enum TracingBoxType
{
    line = 0,
    point = 1,
}

public enum Current
{
    direct = 1,
    alternating = 2,
}

public enum ElectricPowerType
{
    current = 1,
    voltage = 2,
    power = 3,
}

public enum SolveMethod
{
    implicit_method = 0,
    explicit_method = 1,
}

public enum ThermoMethod
{
    fe = 0,
    voronoi = 1,
}

public enum DEF_TYPE
{
    none = -1,
    flat = 0,
    axis = 1,
    _3d = 2,
}

public enum MicrostructureModuleTypes
{
    qform = 0,
    gmt = 1,
}

public enum ThermalProcessingType
{
    heating = 1,
    tempering = 2,
    quenching = 3,
}

public enum RRVStabilization
{
    yes = 1,
    no = -1,
    automatic = 0,
}

public enum RRMRollVelocityType
{
    angular = 0,
    linear = 1,
}

public enum RRGuideRollForceType
{
    percent = 0,
    formula = 1,
}

public enum RRFinDiam
{
    max = 0,
    level = 1,
}

public enum RRStrategy
{
    diam_height = 0,
    feed = 1,
    mandrell_roll = 2,
    mandrell_ring = 3,
}

public enum RRAxialHDispType
{
    hauto = 0,
    diam = 1,
    htime = 2,
    from_diam = 3,
}

public enum RRAxialVDispType
{
    top = 0,
    sync = 1,
    ratio = 2,
}

public enum RRWheelStrat
{
    rolls = 0,
    shaft = 1,
}

public enum RollingPusherVelocityType
{
    automatic = 0,
    manual = 1,
}

public enum RollingDirectionType
{
    automatic = 0,
    plus_x = 1,
    plus_y = 2,
    plus_z = 3,
    minus_x = 4,
    minus_y = 5,
    minus_z = 6,
}

public enum ExtrProcType
{
    flow = 1,
    billet = 2,
    steady = 3,
}

public enum ExtrVelocityType
{
    ram = 1,
    profile = 2,
    ran_var = 3,
}

public enum ExtrPrestageType
{
    equal = 0,
    velocity = 1,
    time = 2,
}

public enum HardnessEstimationMethod
{
    volume_fractions = 0,
    characteristic_time = 1,
}

public enum UltimateStrengthEstimationMethod
{
    volume_fractions = 0,
    characteristic_time = 1,
}

public enum ToolJoinDeform
{
    simple = 1,
    coupled = 2,
    stressed_state = 3,
}

public enum HeatTreatmentModuleTypes
{
    qform = 0,
    gmt = 1,
}

public enum HardnessScales
{
    HV = 1,
    HB = 2,
    HBW = 3,
    HRA = 4,
    HRB = 5,
    HRC = 6,
    HRD = 7,
    HRE = 8,
    HRF = 9,
    HRG = 10,
    HRH = 11,
    HRK = 12,
}

public enum ProfileDistortionTimeStepType
{
    constant_time_step = 0,
    var_time_step_const_layer_thickness = 1,
}

////end block
