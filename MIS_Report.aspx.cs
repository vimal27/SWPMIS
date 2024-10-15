
/// <summary>
/// Requried Namespaces
/// </summary>
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Services;
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

public partial class MIS_Report : System.Web.UI.Page
{
	/// <summary>
	/// Declarations Part For Variables,Strings,SqlConnections,etc
	/// </summary>
    string selectedUser, ProjectAlloted;
    string[] SplittedProjectAlloted;
    string Users;
    string[] TeamsSplitted;
    string TeamsAlloted;
    DateTime StartTime, EndTime;
    string JoinedWords = string.Empty;
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString());
    SqlConnection con2 = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString());
    SqlCommand com_check = new SqlCommand();
    SqlCommand com = new SqlCommand();
    string DateConverted = string.Empty;

	/// <summary>
	/// Page_s the load.
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">E.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["Userid"] == null) Response.Redirect("Login");
            else if (Session["Userid"].ToString() == "") Response.Redirect("Login");
            if (!IsPostBack)
            {
                string Rights = Convert.ToString(Session["Rights"]);
                getDate();
                
                
                txt_AsOnDate.Attributes.Add("readonly", "readonly");
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
                    //lbl_CurrentEmployeeID.Text = Session["UserName"].ToString() + " - " + Session["Userid"].ToString();
                }
            }
        }
        catch (Exception)
        {

        }
    }

	/// <summary>
	/// Gets the Current date For From Date And To Date TextBox.
	/// </summary>
    protected void getDate()
    {
        DateTime dt = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-GB"));
        DateConverted = dt.ToString("dd/MM/yyyy");
        txt_AsOnDate.Text = DateConverted;
    }

	/// <summary>
	/// Reset Fields
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">E.</param>
    protected void btn_Cancel_Click(object sender, EventArgs e)
    {
        getDate();

        block_Grid.Visible = false;
    }

	/// <summary>
	/// Bind Grid If TL Login
	/// </summary>
    protected void loadQuery()
    {
        com_check.CommandText = "select a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[Date],d.projectreq as Project,c.Scope as Scope,b.Stage,(select e.taskname from tbl_taskmaster e where e.id=a.task)[Task],convert(Varchar(20),a.statusoftask)+'%' as StatusOfTask,a.StartTime,a.EndTime,a.Break1[Break],a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid and d.projectid!='NA' where (";
        foreach (string Splitted in SplittedProjectAlloted)
        {
            com_check.CommandText += "allotedteamid like '%" + Splitted + "%' or ";
        }
        com_check.CommandText = com_check.CommandText.Substring(0, com_check.CommandText.Length - 3);
        com_check.CommandText += ")";
    }

	/// <summary>
	/// Bind Grid If TL Login
	/// </summary>
    protected void loadQueryGrid()
    {
        com_check.CommandText = "select a.CurrentDate,a.ProjectID,a.slno,a.EmpNo,a.Stage as StageID,a.task as TaskID,a.Scope as ScopeID,a.ProjectID as Project,a.CurrentDate as Date,a.StartTime as TimeStart,a.EndTime as TimeEnd,a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[Date],d.projectreq as Project,c.Scope as Scope,b.Stage,(select e.taskname from tbl_taskmaster e where e.id=a.task)[Task],convert(Varchar(20),a.statusoftask)+'%' as StatusOfTask,a.StartTime,a.EndTime,a.Break1[Break],a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid and d.projectid!='NA' where (";
        foreach (string Splitted in SplittedProjectAlloted)
        {
            com_check.CommandText += "allotedteamid like '%" + Splitted + "%' or ";
        }
        com_check.CommandText = com_check.CommandText.Substring(0, com_check.CommandText.Length - 3);
        com_check.CommandText += ")";
    }

	/// <summary>
	/// Gridview Bind If Report Button IS Clicked
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">E.</param>
    protected void btn_Export_Click(object sender, EventArgs e)
    {
        try
        {
            block_Grid.Visible = true;
            string datenow = DateTime.Now.ToString("yyyy/MM/dd");
			string sqlstr = @"select ROW_NUMBER() OVER(ORDER BY t.sno) AS Sno,t.Team,t.receiveddate,t.projectname,t.typeproject,t.allotedteamname,t.startdate,t.duedate,replace(replace(convert(varchar(15),convert(date,t.completeddate,105),106),'01 Jan 1900',''),' ','-')[enddate],t.remarks,t.projectstatus,t.ExtendReason from (
                          select ROW_NUMBER() OVER(ORDER BY typeproject) AS Sno,b.Completeddate,b.typeproject Team,replace(Convert(varchar(15),CONVERT(date,b.receiveddate,106),106),' ','-')receiveddate,
                            b.projectname,CASE WHEN b.typeproject = 'External' THEN b.receivedfrom ELSE b.typeproject END AS typeproject,b.allotedteamname,replace(Convert(varchar(15),CONVERT(date,b.receiveddate,106),106),' ','-')startdate,
                            (CASE WHEN ISDATE (b.duedate) = 1 THEN replace(Convert(varchar(15),convert(datetime, cast([b].duedate as varchar(15))),106),' ','-')END)duedate,
                            (CASE WHEN ISDATE (b.completeddate) = 1 THEN convert(date,completeddate,105)END)enddate,
                            stuff((select '<p><b>'+replace(Convert(varchar(15),convert(datetime, cast(pr.date as varchar(15))),106),' ','-') +' </b>: ' + pr.remark+' </p> ' from tbl_ProjectRemarks as pr where pr.project = b.projectid and pr.status=1 order by pr.date asc for xml path(''), type).value('.', 'varchar(max)'), 1, 0, '') as remarks,
                            b.projectstatus,b.extendreason from tbl_ProjectReq b right outer join tbl_ProjectStatusMaster a on a.project = b.projectid right outer JOIN(SELECT MAX(id) as maxID, project FROM tbl_ProjectStatusMaster where dateofchange <= '"+datenow+"' GROUP BY project) as d ON  a.project = d.project and a.id = d.maxID WHERE a.dateofchange <= '"+datenow+"' and projectid!= 'NA' and b.projectstatus not in ('Completed') union  select ROW_NUMBER() OVER(ORDER BY typeproject) AS Sno,b.Completeddate,b.typeproject Team,replace(Convert(varchar(15),CONVERT(date,b.receiveddate,106),106),' ','-')receiveddate, b.projectname,CASE WHEN b.typeproject = 'External' THEN b.receivedfrom ELSE b.typeproject END AS typeproject,b.allotedteamname,replace(Convert(varchar(15),CONVERT(date,b.receiveddate,106),106),' ','-')startdate, (CASE WHEN ISDATE (b.duedate) = 1 THEN replace(Convert(varchar(15),convert(datetime, cast([b].duedate as varchar(15))),106),' ','-')END)duedate, (CASE WHEN ISDATE (b.completeddate) = 1 THEN convert(date,completeddate,105)END)enddate, stuff((select '<p><b>'+replace(Convert(varchar(15),convert(datetime, cast(pr.date as varchar(15))),106),' ','-') +' </b>: ' + pr.remark+' </p> ' from tbl_ProjectRemarks as pr where pr.project = b.projectid and pr.status=1 order by pr.date asc for xml path(''), type).value('.', 'varchar(max)'), 1, 0, '') as remarks, b.projectstatus,b.extendreason from tbl_ProjectReq b right outer join tbl_ProjectStatusMaster a on a.project = b.projectid right outer JOIN(SELECT MAX(id) as maxID, project FROM tbl_ProjectStatusMaster  GROUP BY project) as d ON  a.project = d.project  and a.id = d.maxID WHERE a.dateofchange <= '"+datenow+"' and projectid!= 'NA' and b.projectstatus ='Completed') as t";
            SqlDataAdapter da = new SqlDataAdapter(sqlstr, con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            generateReport(dt, "Daily Status Report");
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "Script", "swal('" + ex.Message + "')", true);
        }
    }

    /// <summary>
    /// Generating Excel Report With MIS Report
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="filename"></param>
    void generateReport(DataTable dt, string filename)
    {
        try
        {
            string teamVerify = "";
            int toSpan = 1;
            string filePath = HttpContext.Current.Server.MapPath(".") + "\\Reports\\";
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath + "\\Reports\\");
            string excelpath = filePath + ("StatusReport" + DateTime.Now.ToShortDateString().Replace("/", "") + ".xls");
            if (File.Exists(excelpath))
                File.Delete(excelpath);
            FileInfo file = new FileInfo(excelpath);
            string reportName = "Daily Status Report as on : " + DateTime.Now.ToString("dd/MM/yyyy");
            StringBuilder sb = new StringBuilder();
            sb.Append("<html>");
            sb.Append("<head>");
            sb.Append("<body>");
            sb.Append("<table border='1' style='font-family:Calibri; font-size:14px;'>");
            sb.Append("<tr><td colspan='12' style='text-align:center;vertical-align:middle;font-weight:bold;background-color: yellow;'>" + reportName + "</td></tr>");
            sb.Append("<tr><td style='text-align:center;vertical-align:middle;font-weight:bold'>S.No</td><td style='text-align:center;font-weight:bold'>Team</ td><td style='text-align:center;font-weight:bold'>Received Date</td><td style='text-align:center;font-weight:bold'>Project Name</td><td style='text-align:center;font-weight:bold'>Received From</td><td style='text-align:center;font-weight:bold'>Responsible</td><td style='text-align:center;font-weight:bold'>Start Date</td><td style='text-align:center;font-weight:bold'>Due Date</td><td style='text-align:center;font-weight:bold'>End Date</td><td style='text-align:center;font-weight:bold'>Reason For Extend</td><td style='text-align:center;font-weight:bold'>Remarks</td><td style='text-align:center;font-weight:bold'>Current Status</td></tr>");
            for (int i1 = 0; i1 < dt.Rows.Count; i1++)
            {
                string teamCheck = dt.Rows[i1]["Team"].ToString().ToUpper();
                if (teamCheck == teamVerify)
                {
                    toSpan += 1;
                }
                else
                {
                    toSpan = 1;
                }
                teamCheck = dt.Rows[i1]["Team"].ToString().ToUpper();
                sb.Append("<tr>");
                sb.Append("<td style='text-align:center;vertical-align:middle;'>" + dt.Rows[i1]["sno"].ToString().ToUpper() + "</td>");
                if (teamCheck == teamVerify)
                {
					sb.Append("<td style='text-align:center;vertical-align:middle;' rowspan>" + dt.Rows[i1]["Team"].ToString().ToUpper() + "</td>");
                    //sb.Append("<td align='center' style='text-align:center;vertical-align:middle;'>\"</td>");
                }
                else
                {
                    sb.Append("<td style='text-align:center;vertical-align:middle;' rowspan>" + dt.Rows[i1]["Team"].ToString().ToUpper() + "</td>");
                }
                teamVerify = teamCheck;
                sb.Append("<td style='text-align:center;vertical-align:middle;'>" + dt.Rows[i1]["receiveddate"].ToString() + "</td>");
                sb.Append("<td style='text-align:left;vertical-align:middle;'>" + dt.Rows[i1]["projectname"].ToString().ToUpper() + "</td>");
                sb.Append("<td style='text-align:center;vertical-align:middle;'>" + dt.Rows[i1]["typeproject"].ToString().ToUpper() + "</td>");
                sb.Append("<td style='text-align:center;vertical-align:middle;'>" + dt.Rows[i1]["allotedteamname"].ToString().ToUpper() + "</td>");
                sb.Append("<td style='text-align:center;vertical-align:middle;'>" + dt.Rows[i1]["startdate"].ToString().ToUpper() + "</td>");
                sb.Append("<td style='text-align:center;vertical-align:middle;'>" + dt.Rows[i1]["duedate"].ToString().ToUpper() + "</td>");
                sb.Append("<td style='text-align:center;vertical-align:middle;'>" + dt.Rows[i1]["enddate"].ToString().ToUpper() + "</td>");
                sb.Append("<td style='text-align:left;vertical-align:middle;'>" + dt.Rows[i1]["extendreason"].ToString().ToUpper() + "</td>");

                sb.Append("<td style='text-align:left;vertical-align:middle;'>" + dt.Rows[i1]["remarks"].ToString().ToUpper() + "</td>");

                sb.Append("<td style='text-align:center;vertical-align:middle;'>" + dt.Rows[i1]["projectstatus"].ToString().ToUpper() + "</td>");
                sb.Append("</tr>");
            }
            sb.Append("</table>");
            sb.Append("</body>");
            sb.Append("</head>");
            sb.Append("</html>");
            File.WriteAllText(excelpath, sb.ToString());
            Response.Clear();
            Response.AddHeader("Content-Length", file.Length.ToString(CultureInfo.InvariantCulture));
            Response.AddHeader("Content-Disposition", "attachment;filename=\"" + ("MIS_Report.xls") + "\"");
            Response.ContentType = "application/octet-stream";
            Response.Flush();
            Response.TransmitFile(filePath+("StatusReport" + DateTime.Now.ToShortDateString().Replace("/", "") + ".xls"));
            Response.End();
        }
        catch (Exception e)
        {

        }
    }
    /// <summary>
    /// Currently Not Used
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grd_DPR.PageIndex = e.NewPageIndex;
            
        }
        catch (Exception)
        {

        }
    }
    protected void txt_AsOnDate_TextChanged(object sender, EventArgs e)
    {
        
    }
    protected void ddl_EmployeeID_SelectedIndexChanged(object sender, EventArgs e)
    {
        
    }

}