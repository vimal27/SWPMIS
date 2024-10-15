/// <summary>
/// Required Namespaces
/// </summary>
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;

public partial class Non_ProductionReport : System.Web.UI.Page
{
	/// <summary>
	/// Declarations Part For Variables,Strings,SqlConnections,etc
	/// </summary>
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString());
    SqlCommand com = new SqlCommand();
    string DateConverted = string.Empty;
	clsDataControl objData = new clsDataControl ();
	ProductionCalculation pcData = new ProductionCalculation();

	/// <summary>
	/// Page_s the load.
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">E.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                bindScope();
                bindStage();
                string Rights = Convert.ToString(Session["Rights"]);
                if (Rights == "Team Leader" || Rights == "Administrator")
                {
					icon_EmployeeDdl.Visible=true;
                    bindUsers();
                }
                getDate();
                bindGrid();
                txt_FromDate.Attributes.Add("readonly", "readonly");
                txt_ToDate.Attributes.Add("readonly", "readonly");
                calext_FromDate.EndDate = DateTime.Now;
                calext_ToDate.EndDate = DateTime.Now;
                if (Session["Userid"] == null) Response.Redirect("Login");
                else if (Session["Userid"].ToString() == "") Response.Redirect("Login");
                Rights = Convert.ToString(Session["Rights"]);
                if (Rights == "Administrator")
                {

                }
                else if (Rights == "Team Leader")
                {

                }
                else
                {
					icon_EmployeeDdl.Visible=false;
                    lbl_CurrentEmployeeID.Text = Session["UserName"].ToString() + " - " + Session["Userid"].ToString();
                }
            }
        }
        catch (Exception)
        {

        }
    }

	/// <summary>
	/// Gets the Currentdate for From Date and To Date Textbox.
	/// </summary>
    protected void getDate()
    {
        DateTime dt = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-GB"));
        DateConverted = dt.ToString("dd/MM/yyyy");
        txt_FromDate.Text = DateConverted;
        txt_ToDate.Text = DateConverted;
    }

	/// <summary>
	/// Reset Form Fields
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">E.</param>
    protected void btn_Cancel_Click(object sender, EventArgs e)
    {
        getDate();
    }

	/// <summary>
	/// Export to excel
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">E.</param>
    protected void btn_Export_Click(object sender, EventArgs e)
    {
        try
        {
            string CurrentDate = DateTime.Now.ToString();
            string Rights = Convert.ToString(Session["Rights"]);
            DateTime convertedStartTime = DateTime.ParseExact(txt_FromDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime convertedEndTime = DateTime.ParseExact(txt_ToDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string dates = "convert(date,currentdate) between '" + convertedStartTime + "' and '" + convertedEndTime + "'";
            SqlDataAdapter da = new SqlDataAdapter("select a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[Date],d.projectreq as Project,c.Scope as Scope,b.Stage,REPLACE(task,-1,'Others') as Task,convert(Varchar(20),a.statusoftask)+'%' as StatusOfTask,a.StartTime,a.EndTime,a.Break1[Break],a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid where a.EmpNo='" + Session["Userid"] + "' and  " + dates + " and a.projectid='NA' order by CurrentDate", con);
            if (ddl_Scope.SelectedIndex != 0)
            {
                da = new SqlDataAdapter("select a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[Date],d.projectreq as Project,c.Scope as Scope,b.Stage,REPLACE(task,-1,'Others') as Task,convert(Varchar(20),a.statusoftask)+'%' as StatusOfTask,a.StartTime,a.EndTime,a.Break1[Break],a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid where a.EmpNo='" + Session["Userid"] + "' and  " + dates + " and a.projectid='NA' and a.projectid like '%NA%' and a.scope like '%" + ddl_Scope.SelectedValue + "%' order by CurrentDate", con);
            }
            if (Rights == "Administrator")
            {
                da = new SqlDataAdapter("select a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[Date],d.projectreq as Project,c.Scope as Scope,b.Stage,REPLACE(task,-1,'Others') as Task,convert(Varchar(20),a.statusoftask)+'%' as StatusOfTask,a.StartTime,a.EndTime,a.Break1[Break],a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid where  " + dates + " and a.projectid='NA' order by  currentdate asc", con);
                if (ddl_EmployeeID.SelectedIndex != 0)
                {
                    da = new SqlDataAdapter("select a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[Date],d.projectreq as Project,c.Scope as Scope,b.Stage,REPLACE(task,-1,'Others') as Task,convert(Varchar(20),a.statusoftask)+'%' as StatusOfTask,a.StartTime,a.EndTime,a.Break1[Break],a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid where a.EmpNo='" + ddl_EmployeeID.SelectedValue + "' and  " + dates + " and a.projectid='NA' order by  currentdate asc", con);
                }
                if (ddl_Stage.SelectedIndex != 0)
                {
                    da = new SqlDataAdapter("select a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[Date],d.projectreq as Project,c.Scope as Scope,b.Stage,REPLACE(task,-1,'Others') as Task,convert(Varchar(20),a.statusoftask)+'%' as StatusOfTask,a.StartTime,a.EndTime,a.Break1[Break],a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid where a.EmpNo='" + ddl_EmployeeID.SelectedValue + "' and b.slno='" + ddl_Stage.SelectedValue + "' and  " + dates + " and a.projectid='NA' order by  currentdate asc", con);
                    if (ddl_EmployeeID.SelectedIndex == 0)
                    {
                        da = new SqlDataAdapter("select a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[Date],d.projectreq as Project,c.Scope as Scope,b.Stage,REPLACE(task,-1,'Others') as Task,convert(Varchar(20),a.statusoftask)+'%' as StatusOfTask,a.StartTime,a.EndTime,a.Break1[Break],a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid where b.slno='" + ddl_Stage.SelectedValue + "' and  " + dates + " and a.projectid!='NA' order by  currentdate asc", con);
                    }
                }
                if (ddl_Scope.SelectedIndex != 0)
                {
                    da = new SqlDataAdapter("select a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[Date],d.projectreq as Project,c.Scope as Scope,b.Stage,REPLACE(task,-1,'Others') as Task,convert(Varchar(20),a.statusoftask)+'%' as StatusOfTask,a.StartTime,a.EndTime,a.Break1[Break],a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid where a.EmpNo='" + ddl_EmployeeID.SelectedValue + "' and d.projectid='NA' and a.scope='" + ddl_Scope.SelectedValue + "' and  " + dates + " and a.projectid='NA' order by  currentdate asc", con);
                    if (ddl_EmployeeID.SelectedIndex == 0)
                    {
                        da = new SqlDataAdapter("select a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[Date],d.projectreq as Project,c.Scope as Scope,b.Stage,REPLACE(task,-1,'Others') as Task,convert(Varchar(20),a.statusoftask)+'%' as StatusOfTask,a.StartTime,a.EndTime,a.Break1[Break],a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid where d.projectid='NA' and a.scope='" + ddl_Scope.SelectedValue + "' and  " + dates + " and a.projectid='NA' order by  currentdate asc", con);
                    }
                }
            }
            if (Rights == "Team Leader")
            {
				string	CurrentTeamLeader=Convert.ToString(Session["Userid"]);
				string teamUsers=objData.GetSingleData("select convert(varchar(max),replace(''''+UserID+'''',',',''','''))[teamusers] from tbl_teamAllotmentMaster  where teamleader like ('"+CurrentTeamLeader+"')");
                da = new SqlDataAdapter("select a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[Date],d.projectreq as Project,c.Scope as Scope,b.Stage,REPLACE(task,-1,'Others') as Task,convert(Varchar(20),a.statusoftask)+'%' as StatusOfTask,a.StartTime,a.EndTime,a.Break1[Break],a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid where a.EmpNo in("+teamUsers+") and   " + dates + " and a.projectid='NA' order by  currentdate asc", con);
                if (ddl_EmployeeID.SelectedIndex != 0)
                {
                    da = new SqlDataAdapter("select a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[Date],d.projectreq as Project,c.Scope as Scope,b.Stage,REPLACE(task,-1,'Others') as Task,convert(Varchar(20),a.statusoftask)+'%' as StatusOfTask,a.StartTime,a.EndTime,a.Break1[Break],a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid where a.EmpNo='" + ddl_EmployeeID.SelectedValue + "' and  " + dates + " and a.EmpNo in("+teamUsers+") and a.projectid='NA' order by  currentdate asc", con);
                }
                if (ddl_Stage.SelectedIndex != 0)
                {
                    da = new SqlDataAdapter("select a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[Date],d.projectreq as Project,c.Scope as Scope,b.Stage,REPLACE(task,-1,'Others') as Task,convert(Varchar(20),a.statusoftask)+'%' as StatusOfTask,a.StartTime,a.EndTime,a.Break1[Break],a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid where a.EmpNo='" + ddl_EmployeeID.SelectedValue + "' and b.slno='" + ddl_Stage.SelectedValue + "' and  " + dates + " and a.projectid='NA' order by  currentdate asc", con);
                    if (ddl_EmployeeID.SelectedIndex == 0)
                    {
                        da = new SqlDataAdapter("select a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[Date],d.projectreq as Project,c.Scope as Scope,b.Stage,REPLACE(task,-1,'Others') as Task,convert(Varchar(20),a.statusoftask)+'%' as StatusOfTask,a.StartTime,a.EndTime,a.Break1[Break],a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid where b.slno='" + ddl_Stage.SelectedValue + "' and  " + dates + " and a.projectid!='NA' order by  currentdate asc", con);
                    }
                }
                if (ddl_Scope.SelectedIndex != 0)
                {
                    da = new SqlDataAdapter("select a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[Date],d.projectreq as Project,c.Scope as Scope,b.Stage,REPLACE(task,-1,'Others') as Task,convert(Varchar(20),a.statusoftask)+'%' as StatusOfTask,a.StartTime,a.EndTime,a.Break1[Break],a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid where a.EmpNo='" + ddl_EmployeeID.SelectedValue + "' and d.projectid='NA' and a.scope='" + ddl_Scope.SelectedValue + "' and  " + dates + " and a.EmpNo in("+teamUsers+") and a.projectid='NA' order by  currentdate asc", con);
                    if (ddl_EmployeeID.SelectedIndex == 0)
                    {
                        da = new SqlDataAdapter("select a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[Date],d.projectreq as Project,c.Scope as Scope,b.Stage,REPLACE(task,-1,'Others') as Task,convert(Varchar(20),a.statusoftask)+'%' as StatusOfTask,a.StartTime,a.EndTime,a.Break1[Break],a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid where d.projectid='NA' and a.scope='" + ddl_Scope.SelectedValue + "' and  " + dates + " and a.EmpNo in("+teamUsers+") and a.projectid='NA' order by  currentdate asc", con);
                    }
                }
            }
            DataTable dts = new DataTable();
            da.Fill(dts);
            if (dts.Rows.Count > 0)
            {
				List<string> HeaderNames=new List<string>();
				HeaderNames.Add("Employee");
				HeaderNames.Add("Shift");
				HeaderNames.Add("Date");
				HeaderNames.Add("Project");
				HeaderNames.Add("Scope");
				HeaderNames.Add("Stage");
				HeaderNames.Add("Task");
				HeaderNames.Add("Status Of Task");
				HeaderNames.Add("Start Time");
				HeaderNames.Add("End Time");
				HeaderNames.Add("Break");
				HeaderNames.Add("Total Time");
				HeaderNames.Add("Remarks");
				List<string> ExcelReport=pcData.generateExcelReport(dts,"NonProductionReport","GenericReports","Non-Production Report",13,HeaderNames);
				FileInfo file = new FileInfo(ExcelReport[2]);
				Response.Clear();
				Response.AddHeader("Content-Length", file.Length.ToString(CultureInfo.InvariantCulture));
				Response.AddHeader("Content-Disposition", "attachment;filename=\"" + ("NonProductionReport.xls") + "\"");
				Response.ContentType = "application/octet-stream";
				Response.Flush();
				Response.TransmitFile(ExcelReport[0]+("NonProductionReport" + DateTime.Now.ToShortDateString().Replace("/", "") + ".xls"));
				Response.End();
            }
        }
        catch (Exception)
        { 

        }
    }

	/// <summary>
	/// Report Button click Function
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">E.</param>
    protected void btn_Report_Click(object sender, EventArgs e)
    {
        try
        {
            bindGrid();
            block_Grid.Visible = true;
            
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "Script", "alert('" + ex.Message + "')", true);
        }
    }

	/// <summary>
	/// Gridview bind function
	/// </summary>
    protected void bindGrid()
    {
        try
        {
			string CurrentDate = DateTime.Now.ToString();
			string Rights = Convert.ToString(Session["Rights"]);
			DateTime convertedStartTime = DateTime.ParseExact(txt_FromDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
			DateTime convertedEndTime = DateTime.ParseExact(txt_ToDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
			string dates = "currentdate between '" + convertedStartTime + "' and '" + convertedEndTime + "'";
            con.Close();
            SqlDataAdapter da = new SqlDataAdapter("select a.slno,a.EmpNo,a.Stage as StageID,a.task as TaskID,a.Scope as ScopeID,a.ProjectID as Project,a.CurrentDate as Date,a.StartTime as TimeStart,a.EndTime as TimeEnd,a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[CurrentDate],d.projectreq as ProjectID,c.Scope as Scope,b.Stage,REPLACE(task,-1,'Others') as Task,a.statusoftask,a.StartTime,a.EndTime,a.Break1 as BreakTime,a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid where a.EmpNo='" + Session["Userid"] + "' and  " + dates + " and a.projectid='NA' order by CurrentDate", con);
            if (ddl_Scope.SelectedIndex != 0)
            {
                da = new SqlDataAdapter("select a.slno,a.EmpNo,a.Stage as StageID,a.task as TaskID,a.Scope as ScopeID,a.ProjectID as Project,a.CurrentDate as Date,a.StartTime as TimeStart,a.EndTime as TimeEnd,a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[CurrentDate],d.projectreq as ProjectID,c.Scope as Scope,b.Stage,REPLACE(task,-1,'Others') as Task,a.statusoftask,a.StartTime,a.EndTime,a.Break1 as BreakTime,a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid where a.EmpNo='" + Session["Userid"] + "' and  " + dates + " and a.projectid='NA' and a.projectid like '%NA%' and a.scope like '%" + ddl_Scope.SelectedValue + "%' order by CurrentDate", con);
            }
            if (Rights == "Administrator")
            {
                da = new SqlDataAdapter("select a.slno,a.EmpNo,a.Stage as StageID,a.task as TaskID,a.Scope as ScopeID,a.ProjectID as Project,a.CurrentDate as Date,a.StartTime as TimeStart,a.EndTime as TimeEnd,a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[CurrentDate],d.projectreq as ProjectID,c.Scope as Scope,b.Stage,REPLACE(task,-1,'Others') as Task,a.statusoftask,a.StartTime,a.EndTime,a.Break1 as BreakTime,a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid where  " + dates + " and a.projectid='NA' order by  currentdate asc", con);
                if (ddl_EmployeeID.SelectedIndex != 0)
                {
                    da = new SqlDataAdapter("select a.slno,a.EmpNo,a.Stage as StageID,a.task as TaskID,a.Scope as ScopeID,a.ProjectID as Project,a.CurrentDate as Date,a.StartTime as TimeStart,a.EndTime as TimeEnd,a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[CurrentDate],d.projectreq as ProjectID,c.Scope as Scope,b.Stage,REPLACE(task,-1,'Others') as Task,a.statusoftask,a.StartTime,a.EndTime,a.Break1 as BreakTime,a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid where a.EmpNo='" + ddl_EmployeeID.SelectedValue + "' and  " + dates + " and a.projectid='NA' order by  currentdate asc", con);
                }
                if (ddl_Stage.SelectedIndex != 0)
                {
                    da = new SqlDataAdapter("select a.slno,a.EmpNo,a.Stage as StageID,a.task as TaskID,a.Scope as ScopeID,a.ProjectID as Project,a.CurrentDate as Date,a.StartTime as TimeStart,a.EndTime as TimeEnd,a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[CurrentDate],d.projectreq as ProjectID,c.Scope as Scope,b.Stage,REPLACE(task,-1,'Others') as Task,a.statusoftask,a.StartTime,a.EndTime,a.Break1 as BreakTime,a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid where a.EmpNo='" + ddl_EmployeeID.SelectedValue + "' and b.slno='" + ddl_Stage.SelectedValue + "' and  " + dates + " and a.projectid='NA' order by  currentdate asc", con);
                    if (ddl_EmployeeID.SelectedIndex == 0)
                    {
                        da = new SqlDataAdapter("select a.slno,a.EmpNo,a.Stage as StageID,a.task as TaskID,a.Scope as ScopeID,a.ProjectID as Project,a.CurrentDate as Date,a.StartTime as TimeStart,a.EndTime as TimeEnd,a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[CurrentDate],d.projectreq as ProjectID,c.Scope as Scope,b.Stage,REPLACE(task,-1,'Others') as Task,a.statusoftask,a.StartTime,a.EndTime,a.Break1 as BreakTime,a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid where b.slno='" + ddl_Stage.SelectedValue + "' and  " + dates + " and a.projectid!='NA' order by  currentdate asc", con);
                    }
                }
                if (ddl_Scope.SelectedIndex != 0)
                {
                    da = new SqlDataAdapter("select a.slno,a.EmpNo,a.Stage as StageID,a.task as TaskID,a.Scope as ScopeID,a.ProjectID as Project,a.CurrentDate as Date,a.StartTime as TimeStart,a.EndTime as TimeEnd,a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[CurrentDate],d.projectreq as ProjectID,c.Scope as Scope,b.Stage,REPLACE(task,-1,'Others') as Task,a.statusoftask,a.StartTime,a.EndTime,a.Break1 as BreakTime,a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid where a.EmpNo='" + ddl_EmployeeID.SelectedValue + "' and d.projectid='NA' and a.scope='" + ddl_Scope.SelectedValue + "' and  " + dates + " and a.projectid='NA' order by  currentdate asc", con);
                    if (ddl_EmployeeID.SelectedIndex == 0)
                    {
                        da = new SqlDataAdapter("select a.slno,a.EmpNo,a.Stage as StageID,a.task as TaskID,a.Scope as ScopeID,a.ProjectID as Project,a.CurrentDate as Date,a.StartTime as TimeStart,a.EndTime as TimeEnd,a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[CurrentDate],d.projectreq as ProjectID,c.Scope as Scope,b.Stage,REPLACE(task,-1,'Others') as Task,a.statusoftask,a.StartTime,a.EndTime,a.Break1 as BreakTime,a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid where d.projectid='NA' and a.scope='" + ddl_Scope.SelectedValue + "' and  " + dates + " and a.projectid='NA' order by  currentdate asc", con);
                    }
                }
                if (ddl_Stage.SelectedIndex != 0 && ddl_Scope.SelectedIndex != 0)
                {
                    da = new SqlDataAdapter("select a.slno,a.EmpNo,a.Stage as StageID,a.task as TaskID,a.Scope as ScopeID,a.ProjectID as Project,a.CurrentDate as Date,a.StartTime as TimeStart,a.EndTime as TimeEnd,a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[CurrentDate],d.projectreq as ProjectID,c.Scope as Scope,b.Stage,REPLACE(task,-1,'Others') as Task,a.statusoftask,a.StartTime,a.EndTime,a.Break1 as BreakTime,a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid where a.EmpNo='" + ddl_EmployeeID.SelectedValue + "' and d.projectid='NA' and a.scope='" + ddl_Scope.SelectedValue + "' and b.slno='"+ddl_Stage.SelectedValue+"' and  " + dates + " and a.projectid='NA' order by  currentdate asc", con);
                    if (ddl_EmployeeID.SelectedIndex == 0)
                    {
                        da = new SqlDataAdapter("select a.slno,a.EmpNo,a.Stage as StageID,a.task as TaskID,a.Scope as ScopeID,a.ProjectID as Project,a.CurrentDate as Date,a.StartTime as TimeStart,a.EndTime as TimeEnd,a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[CurrentDate],d.projectreq as ProjectID,c.Scope as Scope,b.Stage,REPLACE(task,-1,'Others') as Task,a.statusoftask,a.StartTime,a.EndTime,a.Break1 as BreakTime,a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid where d.projectid='NA' and a.scope='" + ddl_Scope.SelectedValue + "' and b.slno='" + ddl_Stage.SelectedValue + "' and  " + dates + " and a.projectid='NA' order by  currentdate asc", con);
                    }
                }
            }
            if (Rights == "Team Leader")
            {
				string	CurrentTeamLeader=Convert.ToString(Session["Userid"]);
				string teamUsers=objData.GetSingleData("select convert(varchar(max),replace(''''+UserID+'''',',',''','''))[teamusers] from tbl_teamAllotmentMaster  where teamleader like ('"+CurrentTeamLeader+"')");
                da = new SqlDataAdapter("select a.slno,a.EmpNo,a.Stage as StageID,a.task as TaskID,a.Scope as ScopeID,a.ProjectID as Project,a.CurrentDate as Date,a.StartTime as TimeStart,a.EndTime as TimeEnd,a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[CurrentDate],d.projectreq as ProjectID,c.Scope as Scope,b.Stage,REPLACE(task,-1,'Others') as Task,a.statusoftask,a.StartTime,a.EndTime,a.Break1 as BreakTime,a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid where a.EmpNo in("+teamUsers+") and   " + dates + " and a.projectid='NA' order by  currentdate asc", con);
                if (ddl_EmployeeID.SelectedIndex != 0)
                {
                    da = new SqlDataAdapter("select a.slno,a.EmpNo,a.Stage as StageID,a.task as TaskID,a.Scope as ScopeID,a.ProjectID as Project,a.CurrentDate as Date,a.StartTime as TimeStart,a.EndTime as TimeEnd,a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[CurrentDate],d.projectreq as ProjectID,c.Scope as Scope,b.Stage,REPLACE(task,-1,'Others') as Task,a.statusoftask,a.StartTime,a.EndTime,a.Break1 as BreakTime,a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid where a.EmpNo='" + ddl_EmployeeID.SelectedValue + "' and  " + dates + " and a.EmpNo in("+teamUsers+") and a.projectid='NA' order by  currentdate asc", con);
                }
                if (ddl_Stage.SelectedIndex != 0)
                {
                    da = new SqlDataAdapter("select a.slno,a.EmpNo,a.Stage as StageID,a.task as TaskID,a.Scope as ScopeID,a.ProjectID as Project,a.CurrentDate as Date,a.StartTime as TimeStart,a.EndTime as TimeEnd,a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[CurrentDate],d.projectreq as ProjectID,c.Scope as Scope,b.Stage,REPLACE(task,-1,'Others') as Task,a.statusoftask,a.StartTime,a.EndTime,a.Break1 as BreakTime,a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid where a.EmpNo='" + ddl_EmployeeID.SelectedValue + "' and b.slno='" + ddl_Stage.SelectedValue + "' and  " + dates + " and a.projectid='NA' order by  currentdate asc", con);
                    if (ddl_EmployeeID.SelectedIndex == 0)
                    {
                        da = new SqlDataAdapter("select a.slno,a.EmpNo,a.Stage as StageID,a.task as TaskID,a.Scope as ScopeID,a.ProjectID as Project,a.CurrentDate as Date,a.StartTime as TimeStart,a.EndTime as TimeEnd,a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[CurrentDate],d.projectreq as ProjectID,c.Scope as Scope,b.Stage,REPLACE(task,-1,'Others') as Task,a.statusoftask,a.StartTime,a.EndTime,a.Break1 as BreakTime,a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid where b.slno='" + ddl_Stage.SelectedValue + "' and  " + dates + " and a.projectid!='NA' order by  currentdate asc", con);
                    }
                }
                if (ddl_Scope.SelectedIndex != 0)
                {
                    da = new SqlDataAdapter("select a.slno,a.EmpNo,a.Stage as StageID,a.task as TaskID,a.Scope as ScopeID,a.ProjectID as Project,a.CurrentDate as Date,a.StartTime as TimeStart,a.EndTime as TimeEnd,a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[CurrentDate],d.projectreq as ProjectID,c.Scope as Scope,b.Stage,REPLACE(task,-1,'Others') as Task,a.statusoftask,a.StartTime,a.EndTime,a.Break1 as BreakTime,a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid where a.EmpNo='" + ddl_EmployeeID.SelectedValue + "' and d.projectid='NA' and a.scope='" + ddl_Scope.SelectedValue + "' and  " + dates + " and a.EmpNo in("+teamUsers+") and a.projectid='NA' order by  currentdate asc", con);
                    if (ddl_EmployeeID.SelectedIndex == 0)
                    {
                        da = new SqlDataAdapter("select a.slno,a.EmpNo,a.Stage as StageID,a.task as TaskID,a.Scope as ScopeID,a.ProjectID as Project,a.CurrentDate as Date,a.StartTime as TimeStart,a.EndTime as TimeEnd,a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[CurrentDate],d.projectreq as ProjectID,c.Scope as Scope,b.Stage,REPLACE(task,-1,'Others') as Task,a.statusoftask,a.StartTime,a.EndTime,a.Break1 as BreakTime,a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid where d.projectid='NA' and a.scope='" + ddl_Scope.SelectedValue + "' and  " + dates + " and a.EmpNo in("+teamUsers+") and a.projectid='NA' order by  currentdate asc", con);
                    }
                }
                if (ddl_Stage.SelectedIndex != 0 && ddl_Scope.SelectedIndex != 0)
                {
                    da = new SqlDataAdapter("select a.slno,a.EmpNo,a.Stage as StageID,a.task as TaskID,a.Scope as ScopeID,a.ProjectID as Project,a.CurrentDate as Date,a.StartTime as TimeStart,a.EndTime as TimeEnd,a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[CurrentDate],d.projectreq as ProjectID,c.Scope as Scope,b.Stage,REPLACE(task,-1,'Others') as Task,a.statusoftask,a.StartTime,a.EndTime,a.Break1 as BreakTime,a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid where a.EmpNo='" + ddl_EmployeeID.SelectedValue + "' and d.projectid='NA' and a.scope='" + ddl_Scope.SelectedValue + "' and b.slno='" + ddl_Stage.SelectedValue + "' and  " + dates + " and a.projectid='NA' order by  currentdate asc", con);
                    if (ddl_EmployeeID.SelectedIndex == 0)
                    {
                        da = new SqlDataAdapter("select a.slno,a.EmpNo,a.Stage as StageID,a.task as TaskID,a.Scope as ScopeID,a.ProjectID as Project,a.CurrentDate as Date,a.StartTime as TimeStart,a.EndTime as TimeEnd,a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[CurrentDate],d.projectreq as ProjectID,c.Scope as Scope,b.Stage,(select e.taskname from tbl_taskmaster e where e.requestid=a.task)[Task],a.statusoftask,a.StartTime,a.EndTime,a.Break1 as BreakTime,a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid where d.projectid='NA' and a.scope='" + ddl_Scope.SelectedValue + "' and b.slno='" + ddl_Stage.SelectedValue + "' and  " + dates + " and a.projectid='NA' order by  currentdate asc", con);
                    }
                }
            }
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                grd_DPR.DataSource = ds;
                grd_DPR.DataBind();

            }

            else
            {
                ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                grd_DPR.DataSource = ds;
                grd_DPR.DataBind();
                int columncount = grd_DPR.Rows[0].Cells.Count;
                grd_DPR.Rows[0].Cells.Clear();
                grd_DPR.Rows[0].Cells.Add(new TableCell());
                grd_DPR.Rows[0].Cells[0].ColumnSpan = columncount;
                grd_DPR.Rows[0].Cells[0].Text = "<center>----- No Records Found -----</center>";
            }
        }
        catch (Exception)
        {
        }
    }

	/// <summary>
	/// Currently Not in Use
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">E.</param>
    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grd_DPR.PageIndex = e.NewPageIndex;
            bindGrid();
        }
        catch (Exception)
        {

        }
    }

	/// <summary>
	/// From Date Text Box changed event
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">E.</param>
    protected void txt_FromDate_TextChanged(object sender, EventArgs e)
    {
        bindGrid();
    }

	/// <summary>
	/// To Date Text Box changed event
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">E.</param>
    protected void txt_ToDate_TextChanged(object sender, EventArgs e)
    {
        bindGrid();
    }

	/// <summary>
	/// Bind Users Depends On Login Access
	/// </summary>
    protected void bindUsers()
    {
        ddl_EmployeeID.Visible = true;
        string Rights = Convert.ToString(Session["Rights"]);
        if (Rights == "Team Leader")
        {
            ddl_EmployeeID.Visible = true;
            com.Connection = con;
            con.Close();
            con.Open();
            com.CommandText = "select TOP 1 UserID,Username from tbl_teamAllotmentMaster where TeamID in(select TeamID from tbl_teamAllotmentMaster where teamleader like ('%" + Session["Userid"] + "%')) order by InsertedDate,UpdatedDate desc";
            SqlDataReader dr = com.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    string UserID = dr[0].ToString();
                    string UserName = dr[1].ToString();
                    string[] splittedUserName = UserName.Split(',');
                    string[] splittedUserID = UserID.Split(',');
                    Dictionary<string, string> users = new Dictionary<string, string>();
                    for (int i = 0; i < splittedUserID.Length; i++)
                    {
                        string isActive = objData.GetSingleData("select userid from tbl_usermaster where status=1 and userid='" + splittedUserID[i] + "'");
                        if (isActive != "0")
                        {
                            users.Add(splittedUserID[i], splittedUserID[i] + " - " + splittedUserName[i]);
                            ddl_EmployeeID.DataSource = users;
                            ddl_EmployeeID.DataTextField = "Value";
                            ddl_EmployeeID.DataValueField = "Key";
                            ddl_EmployeeID.DataBind();
                        }
                    }
                }
            }
            dr.Close();
            con.Close();
        }
        else if (Rights == "Administrator")
        {
            ddl_EmployeeID.Items.Clear();
            con.Close();
            con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("select distinct(a.userid)+ ' | ' + a.username as Username,a.username as name,a.userid from tbl_usermaster a where status=1 and Deluser!=1 order by userid asc", con);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            ddl_EmployeeID.DataSource = ds;
            ddl_EmployeeID.DataTextField = "Username";
            ddl_EmployeeID.DataValueField = "userid";
            ddl_EmployeeID.DataBind();
            con.Close();
        }
        ddl_EmployeeID.Items.Insert(0, new ListItem("Select", "N/A"));
    }

	/// <summary>
	/// Bind Reports Based on selected Employee
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">E.</param>
    protected void ddl_EmployeeID_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindGrid();
    }

	/// <summary>
	/// Bind Stage Based on selected Employee
	/// </summary>
    protected void bindStage()
    {
        try
        {
            ddl_Stage.Items.Clear();
            con.Close();
            con.Open();
            string Rights = Convert.ToString(Session["Rights"]);
            SqlDataAdapter sda = new SqlDataAdapter("Select slno,Stage from tbl_MstStageMaster where Delstage!=1 and projectid='NA'", con);
            if (ddl_EmployeeID.SelectedIndex != 0)
            {
                sda = new SqlDataAdapter("Select a.slno,a.Stage,b.userid from tbl_MstStageMaster a inner join tbl_taskmaster b on b.stageid=a.slno where a.Delstage!=1 and a.projectid='NA' and b.userid='" + ddl_EmployeeID.SelectedValue + "' group by a.slno,a.Stage,a.Delstatus,a.projectid,b.userid", con);
            }
            if (Rights == "User")
            {
                sda = new SqlDataAdapter("Select a.slno,a.Stage,b.userid from tbl_MstStageMaster a inner join tbl_taskmaster b on b.stageid=a.ID where a.Delstage!=1 and a.projectid='NA' and b.userid='" + Session["Userid"] + "' group by a.slno,a.Stage,a.Delstatus,a.projectid,b.userid", con);
            }
            DataSet ds = new DataSet();
            sda.Fill(ds);
            ddl_Stage.DataSource = ds;
            ddl_Stage.DataTextField = "Stage";
            ddl_Stage.DataValueField = "slno";
            ddl_Stage.DataBind();
            con.Close();
        }
        catch (Exception)
        {


        }

        ddl_Stage.Items.Insert(0, new ListItem("Select", "N/A"));

    }

	/// <summary>
	/// Bind scope Based on selected Employee
	/// </summary>
    protected void bindScope()
    {
        try
        {
            ddl_Scope.Items.Clear();
            con.Close();
            con.Open();
            string Rights = Convert.ToString(Session["Rights"]);
            SqlDataAdapter sda = new SqlDataAdapter("Select ID,Scope,Description from tbl_scope where scopestatus!=0 and projectid='NA'", con);
            if (ddl_EmployeeID.SelectedIndex != 0)
            {
                sda = new SqlDataAdapter("Select a.ID,a.Scope,a.Description,b.userid from tbl_scope a inner join tbl_taskmaster b on b.scopeid=a.ID where a.scopestatus!=0 and a.projectid='NA' and b.userid='" + ddl_EmployeeID.SelectedValue + "' group by a.ID,a.Scope,a.Description,a.scopestatus,a.projectid,b.userid", con);
            }
            if (Rights == "User")
            {
                sda = new SqlDataAdapter("Select a.ID,a.Scope,a.Description,b.userid from tbl_scope a inner join tbl_taskmaster b on b.scopeid=a.ID where a.scopestatus!=0 and a.projectid='NA' and b.userid='" + Session["Userid"] + "' group by a.ID,a.Scope,a.Description,a.scopestatus,a.projectid,b.userid", con);
            }
            DataSet ds = new DataSet();
            sda.Fill(ds);
            ddl_Scope.DataSource = ds;
            ddl_Scope.DataTextField = "Scope";
            ddl_Scope.DataValueField = "ID";
            ddl_Scope.DataBind();
            con.Close();
        }
        catch (Exception)
        {


        }

        ddl_Scope.Items.Insert(0, new ListItem("Select", "N/A"));

    }

	/// <summary>
	/// Bind Reports Based on selected scope
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">E.</param>
    protected void ddl_Scope_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindGrid();
    }

	/// <summary>
	/// Bind Reports Based on selected stage
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">E.</param>
    protected void ddl_Stage_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindGrid();
    }
}