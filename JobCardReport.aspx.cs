//Required Namespaces
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


public partial class JobCardReport : System.Web.UI.Page
{
	/// <summary>
    /// Declarations Part For Variables,Strings,SqlConnections,etc
	/// </summary>
    string selectedUser, ProjectAlloted;
    string[] SplittedProjectAlloted;
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString());
    SqlConnection con2 = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString());
    SqlCommand com_check = new SqlCommand();
    SqlCommand com = new SqlCommand();
    string DateConverted = string.Empty;
	ProductionCalculation pcData = new ProductionCalculation();
    clsDataControl objData = new clsDataControl();

	/// <summary>
    /// Page Load Function
	/// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
			//Redirect To Login Page if Session is Expired
            if (Session["Userid"] == null) Response.Redirect("Login");
            else if (Session["Userid"].ToString() == "") Response.Redirect("Login");
            if (!IsPostBack)
            {
				//Projects Binding Depends On Appropriate Access
                bindProject();
                string Rights = Convert.ToString(Session["Rights"]);
                if (Rights == "Team Leader" || Rights == "Administrator")
                {
					//Users Binding Depends On Appropriate Access
                    bindUsers();
                }		

				//Current Date is Inserted On from Date and To Date
                getDate();
                bindGrid();

				//TextBoxes are Readonly To Avoid Invalid Characters from users
                txt_FromDate.Attributes.Add("readonly", "readonly");
                txt_ToDate.Attributes.Add("readonly", "readonly");
                calext_FromDate.EndDate = DateTime.Now;
                calext_ToDate.EndDate = DateTime.Now;

                Rights = Convert.ToString(Session["Rights"]);
                if (Rights == "Administrator")
                {

                }
                else if (Rights == "Team Leader")
                {

                }
                else
                {
					//If Userid is Logged on employee detail is statically inserted on label
                    lbl_CurrentEmployeeID.Text = Session["UserName"].ToString() + " - " + Session["Userid"].ToString();
                }
            }
        }
        catch (Exception)
        {

        }
    }

	/// <summary>
    /// Function to insert Current Date on from Date and to Date
	/// </summary>
    protected void getDate()
    {
        DateTime dt = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-GB"));
        DateConverted = dt.ToString("dd/MM/yyyy");
        txt_FromDate.Text = DateConverted;
        txt_ToDate.Text = DateConverted;
    }

	/// <summary>
    /// Function to reset form if cancel button ISet clicked
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
    protected void btn_Cancel_Click(object sender, EventArgs e)
    {
        getDate();
        bindGrid();
        block_Grid.Visible = false;
    }

	/// <summary>
    /// Export To Excel Function Based On Different Roles and Different Conditions
	/// </summary>
    protected void btn_Export_Click(object sender, EventArgs e)
    {
        try
        {
            string CurrentDate = DateTime.Now.ToString();
            string Rights = Convert.ToString(Session["Rights"]);
            DateTime convertedStartTime = DateTime.ParseExact(txt_FromDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime convertedEndTime = DateTime.ParseExact(txt_ToDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string dates = "currentdate between '" + convertedStartTime + "' and '" + convertedEndTime + "'";
            SqlDataAdapter da = new SqlDataAdapter("select a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[Date],d.projectreq as Project,c.Scope as Scope,b.Stage,(select e.taskname from tbl_taskmaster e where e.id=a.task)[Task],convert(Varchar(20),a.statusoftask)+'%' as StatusOfTask,a.StartTime,a.EndTime,a.Break1[Break],a.meetingtime,a.meetingremarks,a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid where a.EmpNo='" + Session["Userid"] + "' and " + dates + " order by CurrentDate desc", con);
            if (Rights != "Administrator" && Rights != "Team Leader")
            {
                da = new SqlDataAdapter("select a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[Date],d.projectreq as Project,c.Scope as Scope,b.Stage,(select e.taskname from tbl_taskmaster e where e.id=a.task)[Task],convert(Varchar(20),a.statusoftask)+'%' as StatusOfTask,a.StartTime,a.EndTime,a.Break1[Break],a.meetingtime,a.meetingremarks,a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid  where a.EmpNo='" + Session["Userid"] + "' and " + dates + " order by CurrentDate desc", con);
            }
            if (ddl_Project.SelectedIndex != 0)
            {
                da = new SqlDataAdapter("select a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[Date],d.projectreq as Project,c.Scope as Scope,b.Stage,(select e.taskname from tbl_taskmaster e where e.id=a.task)[Task],convert(Varchar(20),a.statusoftask)+'%' as StatusOfTask,a.StartTime,a.EndTime,a.Break1[Break],a.meetingtime,a.meetingremarks,a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid  where a.EmpNo='" + Session["Userid"] + "' and " + dates + " and a.projectid like '%" + ddl_Project.SelectedValue + "%'", con);
                if (Rights != "Administrator" && Rights != "Team Leader")
                {
                    da = new SqlDataAdapter("select a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[Date],d.projectreq as Project,c.Scope as Scope,b.Stage,(select e.taskname from tbl_taskmaster e where e.id=a.task)[Task],convert(Varchar(20),a.statusoftask)+'%' as StatusOfTask,a.StartTime,a.EndTime,a.Break1[Break],a.meetingtime,a.meetingremarks,a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid  where a.EmpNo='" + Session["Userid"] + "' and " + dates + " and a.projectid like '%" + ddl_Project.SelectedValue + "%' order by CurrentDate desc", con);
                }
            }
            if (ddl_Scope.SelectedIndex != 0)
            {
                da = new SqlDataAdapter("select a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[Date],d.projectreq as Project,c.Scope as Scope,b.Stage,(select e.taskname from tbl_taskmaster e where e.id=a.task)[Task],convert(Varchar(20),a.statusoftask)+'%' as StatusOfTask,a.StartTime,a.EndTime,a.Break1[Break],a.meetingtime,a.meetingremarks,a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid  where a.EmpNo='" + Session["Userid"] + "' and " + dates + " and a.projectid like '%" + ddl_Project.SelectedValue + "%' and a.scope like '%" + ddl_Scope.SelectedValue + "%'", con);
                if (Rights != "Administrator" && Rights != "Team Leader")
                {
					icon_EmployeeDdl.Visible = false;
                    da = new SqlDataAdapter("select a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[Date],d.projectreq as Project,c.Scope as Scope,b.Stage,(select e.taskname from tbl_taskmaster e where e.id=a.task)[Task],convert(Varchar(20),a.statusoftask)+'%' as StatusOfTask,a.StartTime,a.EndTime,a.Break1[Break],a.meetingtime,a.meetingremarks,a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid  where a.EmpNo='" + Session["Userid"] + "' and " + dates + " and a.projectid like '%" + ddl_Project.SelectedValue + "%' and a.scope like '%" + ddl_Scope.SelectedValue + "%' order by CurrentDate desc", con);
                }
            }
            if (Rights == "Administrator")
            {
                da = new SqlDataAdapter("select a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[Date],d.projectreq as Project,c.Scope as Scope,b.Stage,(select e.taskname from tbl_taskmaster e where e.id=a.task)[Task],convert(Varchar(20),a.statusoftask)+'%' as StatusOfTask,a.StartTime,a.EndTime,a.Break1[Break],a.meetingtime,a.meetingremarks,a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid  where " + dates + " order by  CurrentDate desc", con);
                if (ddl_EmployeeID.SelectedIndex != 0)
                {
                    da = new SqlDataAdapter("select a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[Date],d.projectreq as Project,c.Scope as Scope,b.Stage,(select e.taskname from tbl_taskmaster e where e.id=a.task)[Task],convert(Varchar(20),a.statusoftask)+'%' as StatusOfTask,a.StartTime,a.EndTime,a.Break1[Break],a.meetingtime,a.meetingremarks,a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid  where a.EmpNo='" + ddl_EmployeeID.SelectedValue + "' and " + dates + " order by  CurrentDate desc", con);
                }
                if (ddl_Project.SelectedIndex != 0)
                {
                    da = new SqlDataAdapter("select a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[Date],d.projectreq as Project,c.Scope as Scope,b.Stage,(select e.taskname from tbl_taskmaster e where e.id=a.task)[Task],convert(Varchar(20),a.statusoftask)+'%' as StatusOfTask,a.StartTime,a.EndTime,a.Break1[Break],a.meetingtime,a.meetingremarks,a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid  where a.EmpNo='" + ddl_EmployeeID.SelectedValue + "' and d.projectid='" + ddl_Project.SelectedValue + "' and " + dates + " order by  CurrentDate desc", con);
                    if (ddl_EmployeeID.SelectedIndex == 0)
                    {
                        da = new SqlDataAdapter("select a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[Date],d.projectreq as Project,c.Scope as Scope,b.Stage,(select e.taskname from tbl_taskmaster e where e.id=a.task)[Task],convert(Varchar(20),a.statusoftask)+'%' as StatusOfTask,a.StartTime,a.EndTime,a.Break1[Break],a.meetingtime,a.meetingremarks,a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid  where d.projectid='" + ddl_Project.SelectedValue + "' and " + dates + " order by  CurrentDate desc", con);
                    }
                }
                if (ddl_Scope.SelectedIndex != 0)
                {
                    da = new SqlDataAdapter("select a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[Date],d.projectreq as Project,c.Scope as Scope,b.Stage,(select e.taskname from tbl_taskmaster e where e.id=a.task)[Task],convert(Varchar(20),a.statusoftask)+'%' as StatusOfTask,a.StartTime,a.EndTime,a.Break1[Break],a.meetingtime,a.meetingremarks,a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid  where a.EmpNo='" + ddl_EmployeeID.SelectedValue + "' and d.projectid='" + ddl_Project.SelectedValue + "' and a.scope='" + ddl_Scope.SelectedValue + "' and " + dates + " order by  CurrentDate desc", con);
                    if (ddl_EmployeeID.SelectedIndex == 0)
                    {
                        da = new SqlDataAdapter("select a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[Date],d.projectreq as Project,c.Scope as Scope,b.Stage,(select e.taskname from tbl_taskmaster e where e.id=a.task)[Task],convert(Varchar(20),a.statusoftask)+'%' as StatusOfTask,a.StartTime,a.EndTime,a.Break1[Break],a.meetingtime,a.meetingremarks,a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid  where d.projectid='" + ddl_Project.SelectedValue + "' and a.scope='" + ddl_Scope.SelectedValue + "' and " + dates + " order by  CurrentDate desc", con);
                    }
                }
            }
            else if (Rights == "Team Leader")
            {
                con2.Close();
                con2.Open();
                com_check.Connection = con2;
                com_check.CommandText = "select TeamID from tbl_teamAllotmentMaster where teamleader like  ('%" + Session["Userid"] + "%')";
                SqlDataReader dr = com_check.ExecuteReader();
                if (dr.HasRows)
                {
                    StringBuilder sb_ProjectAlloted = new StringBuilder();
                    while (dr.Read())
                    {
                        sb_ProjectAlloted.Append(dr["TeamID"] + ",");
                        ProjectAlloted = sb_ProjectAlloted.ToString();
                    }
                    ProjectAlloted = ProjectAlloted.TrimEnd(',');
                }
                dr.Close();
                con2.Close();
                SplittedProjectAlloted = ProjectAlloted.Split(',');
                com_check.Connection = con;
                com_check.CommandText = "select a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[Date],d.projectreq as Project,c.Scope as Scope,b.Stage,(select e.taskname from tbl_taskmaster e where e.id=a.task)[Task],convert(Varchar(20),a.statusoftask)+'%' as StatusOfTask,a.StartTime,a.EndTime,a.Break1[Break],a.meetingtime,a.meetingremarks,a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid  where (";
                foreach (string Splitted in SplittedProjectAlloted)
                {
                    com_check.CommandText += "allotedteamid like '%" + Splitted + "%' or ";
                }
                com_check.CommandText = com_check.CommandText.Substring(0, com_check.CommandText.Length - 3);
                com_check.CommandText += ") and  " + dates + " order by  CurrentDate desc";
                da = new SqlDataAdapter(com_check.CommandText, con);
                if (ddl_EmployeeID.SelectedIndex != 0)
                {
                    loadQuery();
                    com_check.CommandText += "and a.EmpNo='" + ddl_EmployeeID.SelectedValue + "'  and " + dates + "";
                    da = new SqlDataAdapter(com_check.CommandText, con);
                }
                if (ddl_Project.SelectedIndex != 0)
                {
                    loadQuery();
                    com_check.CommandText += "and a.EmpNo='" + ddl_EmployeeID.SelectedValue + "' and d.projectid='" + ddl_Project.SelectedValue + "' and " + dates + "";
                    da = new SqlDataAdapter(com_check.CommandText, con);
                    if (ddl_EmployeeID.SelectedIndex == 0)
                    {
                        loadQuery();
                        com_check.CommandText += "and d.projectid='" + ddl_Project.SelectedValue + "' and " + dates + "";
                        da = new SqlDataAdapter(com_check.CommandText, con);
                    }
                }
                if (ddl_Scope.SelectedIndex != 0)
                {
                    loadQuery();
                    com_check.CommandText += "and " + dates + " and a.EmpNo='" + ddl_EmployeeID.SelectedValue + "' and d.projectid='" + ddl_Project.SelectedValue + "' and a.scope='" + ddl_Scope.SelectedValue + "'";
                    da = new SqlDataAdapter(com_check.CommandText, con);
                    if (ddl_EmployeeID.SelectedIndex == 0)
                    {
                        loadQuery();
                        com_check.CommandText += "and d.projectid='" + ddl_Project.SelectedValue + "' and a.scope='" + ddl_Scope.SelectedValue + "' and " + dates + "";
                        da = new SqlDataAdapter(com_check.CommandText, con);
                    }
                }
            }
            DataTable dts = new DataTable();
            if (con.State == ConnectionState.Closed) con.Open();
            da.SelectCommand.Connection = con;
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
				HeaderNames.Add("Meeting Time");
				HeaderNames.Add("Meeting Remarks");
				HeaderNames.Add("Total Time");
				HeaderNames.Add("Remarks");
				List<string> ExcelReport=pcData.generateExcelReport(dts,"JobCardReport","GenericReports","Job Card Report",15,HeaderNames);
				FileInfo file = new FileInfo(ExcelReport[2]);
				Response.Clear();
				Response.AddHeader("Content-Length", file.Length.ToString(CultureInfo.InvariantCulture));
				Response.AddHeader("Content-Disposition", "attachment;filename=\"" + ("JobCardReport.xls") + "\"");
				Response.ContentType = "application/octet-stream";
				Response.Flush();
				Response.TransmitFile(ExcelReport[0]+("JobCardReport" + DateTime.Now.ToShortDateString().Replace("/", "") + ".xls"));
				Response.End();
            }
        }
        catch (Exception)
        {

        }
    }

	/// <summary>
    /// Bind Gridview if Teamleader Login with his Team members
	/// </summary>
    protected void loadQuery()
    {
        com_check.CommandText = "select a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[Date],d.projectreq as Project,c.Scope as Scope,b.Stage,(select e.taskname from tbl_taskmaster e where e.id=a.task)[Task],convert(Varchar(20),a.statusoftask)+'%' as StatusOfTask,a.StartTime,a.EndTime,a.Break1[Break],a.meetingtime,a.meetingremarks,a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid  where (";
        foreach (string Splitted in SplittedProjectAlloted)
        {
            com_check.CommandText += "allotedteamid like '%" + Splitted + "%' or ";
        }
        com_check.CommandText = com_check.CommandText.Substring(0, com_check.CommandText.Length - 3);
        com_check.CommandText += ")";
    }

	/// <summary>
    /// Bind Gridview if Teamleader Login with his Team members
	/// </summary>
    protected void loadQueryGrid()
    {
        com_check.CommandText = "select a.CurrentDate,a.ProjectID,a.slno,a.EmpNo,a.Stage as StageID,a.task as TaskID,a.Scope as ScopeID,a.ProjectID as Project,a.CurrentDate as Date,a.StartTime as TimeStart,a.EndTime as TimeEnd,a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[Date],d.projectreq as Project,c.Scope as Scope,b.Stage,(select e.taskname from tbl_taskmaster e where e.id=a.task)[Task],convert(Varchar(20),a.statusoftask)+'%' as StatusOfTask,a.StartTime,a.EndTime,a.Break1[Break],a.meetingtime,a.meetingremarks,a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid  where (";
        foreach (string Splitted in SplittedProjectAlloted)
        {
            com_check.CommandText += "allotedteamid like '%" + Splitted + "%' or ";
        }
        com_check.CommandText = com_check.CommandText.Substring(0, com_check.CommandText.Length - 3);
        com_check.CommandText += ")";
    }

    /// <summary>
    /// Bind Gridview if Report Button is Clicked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
    /// Gridview Bind Function
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
            SqlDataAdapter da = new SqlDataAdapter("select a.ProjectID,a.CurrentDate,a.slno,a.EmpNo,a.Stage as StageID,a.task as TaskID,a.Scope as ScopeID,a.ProjectID as Project,a.CurrentDate as Date,a.StartTime as TimeStart,a.EndTime as TimeEnd,a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[CurrentDate],d.projectreq as ProjectID,c.Scope as Scope,b.Stage,(select e.taskname from tbl_taskmaster e where e.id=a.task)[Task],a.statusoftask[StatusOfTask],a.StartTime,a.EndTime,a.Break1[Break],a.meetingtime,a.meetingremarks,a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid  where a.EmpNo='" + Session["Userid"] + "' and " + dates + " order by a.CurrentDate desc", con);
            if (Rights != "Administrator" && Rights != "Team Leader")
            {
                da = new SqlDataAdapter("select a.ProjectID,a.CurrentDate,a.slno,a.EmpNo,a.Stage as StageID,a.task as TaskID,a.Scope as ScopeID,a.ProjectID as Project,a.CurrentDate as Date,a.StartTime as TimeStart,a.EndTime as TimeEnd,a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[CurrentDate],d.projectreq as ProjectID,c.Scope as Scope,b.Stage,(select e.taskname from tbl_taskmaster e where e.id=a.task)[Task],a.statusoftask[StatusOfTask],a.StartTime,a.EndTime,a.Break1[Break],a.meetingtime,a.meetingremarks,a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid  where a.EmpNo='" + Session["Userid"] + "' and " + dates + " order by a.CurrentDate desc", con);
            }
            if (ddl_Project.SelectedIndex != 0)
            {
                da = new SqlDataAdapter("select a.ProjectID,a.CurrentDate,a.slno,a.EmpNo,a.Stage as StageID,a.task as TaskID,a.Scope as ScopeID,a.ProjectID as Project,a.CurrentDate as Date,a.StartTime as TimeStart,a.EndTime as TimeEnd,a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[CurrentDate],d.projectreq as ProjectID,c.Scope as Scope,b.Stage,(select e.taskname from tbl_taskmaster e where e.id=a.task)[Task],a.statusoftask[StatusOfTask],a.StartTime,a.EndTime,a.Break1[Break],a.meetingtime,a.meetingremarks,a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid  where a.EmpNo='" + Session["Userid"] + "' and " + dates + " and a.projectid like '%" + ddl_Project.SelectedValue + "%' order by a.CurrentDate desc", con);
                if (Rights != "Administrator" && Rights != "Team Leader")
                {
                    da = new SqlDataAdapter("select a.ProjectID,a.CurrentDate,a.slno,a.EmpNo,a.Stage as StageID,a.task as TaskID,a.Scope as ScopeID,a.ProjectID as Project,a.CurrentDate as Date,a.StartTime as TimeStart,a.EndTime as TimeEnd,a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[CurrentDate],d.projectreq as ProjectID,c.Scope as Scope,b.Stage,(select e.taskname from tbl_taskmaster e where e.id=a.task)[Task],a.statusoftask[StatusOfTask],a.StartTime,a.EndTime,a.Break1[Break],a.meetingtime,a.meetingremarks,a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid  where a.EmpNo='" + Session["Userid"] + "' and " + dates + " and a.projectid like '%" + ddl_Project.SelectedValue + "%' order by a.CurrentDate desc", con);
                }
            }
            if (ddl_Scope.SelectedIndex != 0)
            {
                da = new SqlDataAdapter("select a.ProjectID,a.CurrentDate,a.slno,a.EmpNo,a.Stage as StageID,a.task as TaskID,a.Scope as ScopeID,a.ProjectID as Project,a.CurrentDate as Date,a.StartTime as TimeStart,a.EndTime as TimeEnd,a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[CurrentDate],d.projectreq as ProjectID,c.Scope as Scope,b.Stage,(select e.taskname from tbl_taskmaster e where e.id=a.task)[Task],a.statusoftask[StatusOfTask],a.StartTime,a.EndTime,a.Break1[Break],a.meetingtime,a.meetingremarks,a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid  where a.EmpNo='" + Session["Userid"] + "' and " + dates + " and a.projectid like '%" + ddl_Project.SelectedValue + "%' and a.scope like '%" + ddl_Scope.SelectedValue + "%'  order by a.CurrentDate desc", con);
                if (Rights != "Administrator" && Rights != "Team Leader")
                {
                    da = new SqlDataAdapter("select a.ProjectID,a.CurrentDate,a.slno,a.EmpNo,a.Stage as StageID,a.task as TaskID,a.Scope as ScopeID,a.ProjectID as Project,a.CurrentDate as Date,a.StartTime as TimeStart,a.EndTime as TimeEnd,a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[CurrentDate],d.projectreq as ProjectID,c.Scope as Scope,b.Stage,(select e.taskname from tbl_taskmaster e where e.id=a.task)[Task],a.statusoftask[StatusOfTask],a.StartTime,a.EndTime,a.Break1[Break],a.meetingtime,a.meetingremarks,a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid  where a.EmpNo='" + Session["Userid"] + "' and " + dates + " and a.projectid like '%" + ddl_Project.SelectedValue + "%' and a.scope like '%" + ddl_Scope.SelectedValue + "%' order by a.Currentdate desc", con);
                }
            }
            if (Rights == "Administrator")
            {
                da = new SqlDataAdapter("select a.ProjectID,a.CurrentDate,a.slno,a.EmpNo,a.Stage as StageID,a.task as TaskID,a.Scope as ScopeID,a.ProjectID as Project,a.CurrentDate as Date,a.StartTime as TimeStart,a.EndTime as TimeEnd,a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[Date],d.projectreq as Project,c.Scope as Scope,b.Stage,(select e.taskname from tbl_taskmaster e where e.id=a.task)[Task],convert(Varchar(20),a.statusoftask)+'%' as StatusOfTask,a.StartTime,a.EndTime,a.Break1[Break],a.meetingtime,a.meetingremarks,a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid  where " + dates + " order by  currentdate desc", con);
                if (ddl_EmployeeID.SelectedIndex != 0)
                {
                    da = new SqlDataAdapter("select a.ProjectID,a.CurrentDate,a.slno,a.EmpNo,a.Stage as StageID,a.task as TaskID,a.Scope as ScopeID,a.ProjectID as Project,a.CurrentDate as Date,a.StartTime as TimeStart,a.EndTime as TimeEnd,a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[Date],d.projectreq as Project,c.Scope as Scope,b.Stage,(select e.taskname from tbl_taskmaster e where e.id=a.task)[Task],convert(Varchar(20),a.statusoftask)+'%' as StatusOfTask,a.StartTime,a.EndTime,a.Break1[Break],a.meetingtime,a.meetingremarks,a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid  where a.EmpNo='" + ddl_EmployeeID.SelectedValue + "' and " + dates + " order by  currentdate desc", con);
                }
                if (ddl_Project.SelectedIndex != 0)
                {
                    da = new SqlDataAdapter("select a.ProjectID,a.CurrentDate,a.slno,a.EmpNo,a.Stage as StageID,a.task as TaskID,a.Scope as ScopeID,a.ProjectID as Project,a.CurrentDate as Date,a.StartTime as TimeStart,a.EndTime as TimeEnd,a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[Date],d.projectreq as Project,c.Scope as Scope,b.Stage,(select e.taskname from tbl_taskmaster e where e.id=a.task)[Task],convert(Varchar(20),a.statusoftask)+'%' as StatusOfTask,a.StartTime,a.EndTime,a.Break1[Break],a.meetingtime,a.meetingremarks,a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid  where a.EmpNo='" + ddl_EmployeeID.SelectedValue + "' and d.projectid='" + ddl_Project.SelectedValue + "' and " + dates + " order by  currentdate desc", con);
                    if (ddl_EmployeeID.SelectedIndex == 0)
                    {
                        da.SelectCommand.CommandText = "";
                        da = new SqlDataAdapter("select a.ProjectID,a.CurrentDate,a.slno,a.EmpNo,a.Stage as StageID,a.task as TaskID,a.Scope as ScopeID,a.ProjectID as Project,a.CurrentDate as Date,a.StartTime as TimeStart,a.EndTime as TimeEnd,a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[Date],d.projectreq as Project,c.Scope as Scope,b.Stage,(select e.taskname from tbl_taskmaster e where e.id=a.task)[Task],convert(Varchar(20),a.statusoftask)+'%' as StatusOfTask,a.StartTime,a.EndTime,a.Break1[Break],a.meetingtime,a.meetingremarks,a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid  where d.projectid='" + ddl_Project.SelectedValue + "' and " + dates + " order by  currentdate desc", con);
                    }
                }
                if (ddl_Scope.SelectedIndex != 0)
                {
                    da = new SqlDataAdapter("select a.ProjectID,a.CurrentDate,a.slno,a.EmpNo,a.Stage as StageID,a.task as TaskID,a.Scope as ScopeID,a.ProjectID as Project,a.CurrentDate as Date,a.StartTime as TimeStart,a.EndTime as TimeEnd,a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[Date],d.projectreq as Project,c.Scope as Scope,b.Stage,(select e.taskname from tbl_taskmaster e where e.id=a.task)[Task],convert(Varchar(20),a.statusoftask)+'%' as StatusOfTask,a.StartTime,a.EndTime,a.Break1[Break],a.meetingtime,a.meetingremarks,a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid  where a.EmpNo='" + ddl_EmployeeID.SelectedValue + "' and d.projectid='" + ddl_Project.SelectedValue + "' and a.scope='" + ddl_Scope.SelectedValue + "' and " + dates + " order by  currentdate desc", con);
                    if (ddl_EmployeeID.SelectedIndex == 0)
                    {
                        da = new SqlDataAdapter("select a.ProjectID,a.CurrentDate,a.slno,a.EmpNo,a.Stage as StageID,a.task as TaskID,a.Scope as ScopeID,a.ProjectID as Project,a.CurrentDate as Date,a.StartTime as TimeStart,a.EndTime as TimeEnd,a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[Date],d.projectreq as Project,c.Scope as Scope,b.Stage,(select e.taskname from tbl_taskmaster e where e.id=a.task)[Task],convert(Varchar(20),a.statusoftask)+'%' as StatusOfTask,a.StartTime,a.EndTime,a.Break1[Break],a.meetingtime,a.meetingremarks,a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid  where d.projectid='" + ddl_Project.SelectedValue + "' and a.scope='" + ddl_Scope.SelectedValue + "' and " + dates + " order by  currentdate desc", con);
                    }
                }
            }
            if (Rights == "Team Leader")
            {
                con2.Close();
                con2.Open();
                com_check.Connection = con2;
                com_check.CommandText = "select TeamID from tbl_teamAllotmentMaster where teamleader like  ('%" + Session["Userid"] + "%')";
                SqlDataReader dr = com_check.ExecuteReader();
                if (dr.HasRows)
                {
                    StringBuilder sb_ProjectAlloted = new StringBuilder();
                    while (dr.Read())
                    {
                        sb_ProjectAlloted.Append(dr["TeamID"] + ",");
                        ProjectAlloted = sb_ProjectAlloted.ToString();
                    }
                    ProjectAlloted = ProjectAlloted.TrimEnd(',');
                }
                dr.Close();
                con2.Close();
                SplittedProjectAlloted = ProjectAlloted.Split(',');
                com_check.Connection = con;
                com_check.CommandText = "select a.ProjectID,a.CurrentDate,a.slno,a.EmpNo,a.Stage as StageID,a.task as TaskID,a.Scope as ScopeID,a.ProjectID as Project,a.CurrentDate as Date,a.StartTime as TimeStart,a.EndTime as TimeEnd,a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[Date],d.projectreq as Project,c.Scope as Scope,b.Stage,(select e.taskname from tbl_taskmaster e where e.id=a.task)[Task],convert(Varchar(20),a.statusoftask)+'%' as StatusOfTask,a.StartTime,a.EndTime,a.Break1[Break],a.meetingtime,a.meetingremarks,a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid  where (";
                foreach (string Splitted in SplittedProjectAlloted)
                {
                    com_check.CommandText += "allotedteamid like '%" + Splitted + "%' or ";
                }
                com_check.CommandText = com_check.CommandText.Substring(0, com_check.CommandText.Length - 3);
                com_check.CommandText += ") and  " + dates + " order by  currentdate desc";
                da = new SqlDataAdapter(com_check.CommandText, con);
                if (ddl_EmployeeID.SelectedIndex != 0)
                {
                    loadQueryGrid();
                    com_check.CommandText += "and a.EmpNo='" + ddl_EmployeeID.SelectedValue + "'  and " + dates + "";
                    da = new SqlDataAdapter(com_check.CommandText, con);
                }
                if (ddl_Project.SelectedIndex != 0)
                {
                    loadQueryGrid();
                    com_check.CommandText += "and a.EmpNo='" + ddl_EmployeeID.SelectedValue + "' and d.projectid='" + ddl_Project.SelectedValue + "' and " + dates + "";
                    da = new SqlDataAdapter(com_check.CommandText, con);
                    if (ddl_EmployeeID.SelectedIndex == 0)
                    {
                        loadQueryGrid();
                        com_check.CommandText += "and d.projectid='" + ddl_Project.SelectedValue + "' and " + dates + "";
                        da = new SqlDataAdapter(com_check.CommandText, con);
                    }
                }
                if (ddl_Scope.SelectedIndex != 0)
                {
                    loadQueryGrid();
                    com_check.CommandText += "and " + dates + " and a.EmpNo='" + ddl_EmployeeID.SelectedValue + "' and d.projectid='" + ddl_Project.SelectedValue + "' and a.scope='" + ddl_Scope.SelectedValue + "'";
                    da = new SqlDataAdapter(com_check.CommandText, con);
                    if (ddl_EmployeeID.SelectedIndex == 0)
                    {
                        loadQueryGrid();
                        com_check.CommandText += "and d.projectid='" + ddl_Project.SelectedValue + "' and a.scope='" + ddl_Scope.SelectedValue + "' and " + dates + "";
                        da = new SqlDataAdapter(com_check.CommandText, con);
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
    /// Currently not used
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
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
    /// On From Date Text Box Change Event
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
    protected void txt_FromDate_TextChanged(object sender, EventArgs e)
    {
        bindGrid();
    }
	/// <summary>
    /// On To Date Text Box Change Event
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
    protected void txt_ToDate_TextChanged(object sender, EventArgs e)
    {
        bindGrid();
    }

	/// <summary>
    /// Bind Users Based on Login access
	/// </summary>
    protected void bindUsers()
    {
        string Rights = Convert.ToString(Session["Rights"]);
        ddl_EmployeeID.Visible = true;
        if (Rights == "Team Leader")
        {
            ddl_EmployeeID.Visible = true;
			icon_EmployeeDdl.Visible = true;
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
                        string isActive = objData.GetSingleData("select userid from tbl_usermaster where status=1 and userid='"+splittedUserID[i]+"'");
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
            if (selectedUser != "N/A")
            {
                ddl_EmployeeID.SelectedValue = selectedUser;
            }
        }
        else if (Rights == "Administrator")
        {
			icon_EmployeeDdl.Visible = true;
            ddl_EmployeeID.Visible = true;
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
    /// Binding Projects based on login access
	/// </summary>
    protected void bindProject()
    {
        try
        {
            ddl_Project.Items.Clear();
            con.Close();
            con.Open();
            string Rights = Convert.ToString(Session["Rights"]);
            SqlDataAdapter sda = new SqlDataAdapter("Select projectid,projectname,projectreq from tbl_projectReq where projectstatus!='Hold' and projectstatus!='Closed' and projectstatus!='Completed' and status!=0", con);
            if (ddl_EmployeeID.SelectedIndex != 0)
            {
                sda = new SqlDataAdapter("select distinct a.projectid,a.projectname,a.projectreq,b.projectid from tbl_projectReq a inner join tbl_taskmaster b on a.projectid=b.projectid  where  a.projectstatus!='Hold' and a.projectstatus!='Closed' and a.projectstatus!='Completed' or a.projectid='NA' and a.status!=0 and b.userid='" + ddl_EmployeeID.SelectedValue + "' group by a.projectid,a.projectname,a.projectreq,b.projectid,b.userid", con);
            }
            if (Rights == "Team Leader")
            {
                sda = new SqlDataAdapter("Select distinct projectid,projectname,projectreq from tbl_projectReq where allotedteamid in(select TeamID from tbl_teamAllotmentMaster where teamleader like ('%" + Session["Userid"] + "%')) and projectstatus!='Hold' and projectstatus!='Closed' and projectstatus!='Completed'  and status!=0 or a.projectid='NA'", con);
                if (ddl_EmployeeID.SelectedIndex != 0)
                {
                    sda = new SqlDataAdapter("select distinct a.projectid,a.projectname,a.projectreq,b.projectid from tbl_projectReq a inner join tbl_taskmaster b on a.projectid=b.projectid  where a.allotedteamid in(select TeamID from tbl_teamAllotmentMaster where teamleader like ('%" + Session["Userid"] + "%')) and a.projectstatus!='Hold' and a.projectstatus!='Closed' and a.projectstatus!='Completed' and a.status!=0 and b.userid='" + ddl_EmployeeID.SelectedValue + "' or a.projectid='NA'  group by a.projectid,a.projectname,a.projectreq,b.projectid,b.userid", con);
                }
            }
            if (Rights == "User")
            {
                sda = new SqlDataAdapter("select distinct a.projectid,a.projectname,a.projectreq,b.projectid from tbl_projectReq a inner join tbl_taskmaster b on a.projectid=b.projectid  where a.allotedteamid in(select TeamID from tbl_teamAllotmentMaster where  a.projectstatus!='Hold' and a.projectstatus!='Closed' and a.projectstatus!='Completed') and a.status!=0 and b.userid='" + Session["Userid"] + "' or a.projectid='NA'  group by a.projectid,a.projectname,a.projectreq,b.projectid,b.userid", con);
            }
            DataSet ds = new DataSet();
            sda.Fill(ds);
            ddl_Project.DataSource = ds;
            ddl_Project.DataTextField = "projectreq";
            ddl_Project.DataValueField = "projectid";
            ddl_Project.DataBind();
            con.Close();
        }
        catch (Exception)
        {


        }
        ddl_Project.Items.Insert(0, new ListItem("Select", "N/A"));
        bindScope();
    }

	/// <summary>
    /// Binding Scopes Based on projects selected
	/// </summary>
    protected void bindScope()
    {
        try
        {
            ddl_Scope.Items.Clear();
            con.Close();
            con.Open();
            string Rights = Convert.ToString(Session["Rights"]);
            SqlDataAdapter sda = new SqlDataAdapter("Select ID,Scope,Description from tbl_scope where scopestatus!=0 and projectid='" + ddl_Project.SelectedValue + "'", con);
            if (ddl_EmployeeID.SelectedIndex != 0)
            {
                sda = new SqlDataAdapter("Select a.ID,a.Scope,a.Description,b.userid from tbl_scope a inner join tbl_taskmaster b on b.scopeid=a.ID or a.projectid='NA' where a.scopestatus!=0 and a.projectid='" + ddl_Project.SelectedValue + "' and b.userid='" + ddl_EmployeeID.SelectedValue + "' group by a.ID,a.Scope,a.Description,a.scopestatus,a.projectid,b.userid", con);
            }
            if (Rights == "User")
            {
                sda = new SqlDataAdapter("Select a.ID,a.Scope,a.Description,b.userid from tbl_scope a inner join tbl_taskmaster b on b.scopeid=a.ID or a.projectid='NA' where a.scopestatus!=0 and a.projectid='" + ddl_Project.SelectedValue + "' and b.userid='" + Session["Userid"] + "' group by a.ID,a.Scope,a.Description,a.scopestatus,a.projectid,b.userid", con);
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
    /// Drop Down Selected index changed event to bind values depends OnPageIndexChanging selected employee
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
    protected void ddl_EmployeeID_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindGrid();
        bindProject();
    }
	/// <summary>
    /// Drop Down Selected index changed event to bind values depends OnPageIndexChanging selected Project
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
    protected void ddl_Project_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindGrid();
        if (Session["Rights"].ToString() != "User")
        {
            if (ddl_EmployeeID.SelectedIndex == 0)
            {
                bindUsers();
            }
        }
        bindScope();
    }
	/// <summary>
    /// Drop Down Selected index changed event to bind values depends OnPageIndexChanging selected scopes
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
    protected void ddl_Scope_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindGrid();
    }
}