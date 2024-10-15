/// <summary>
/// Required Namespaces
/// </summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Configuration;
using System.Globalization;
using System.Text;
using System.Drawing;
using System.Web.UI.HtmlControls;

public partial class YetToStart : System.Web.UI.Page
{
    /// <summary>
    /// Declarations Part For Variables,Strings,SqlConnections,etc
    /// </summary>
    string Status1 = string.Empty;
    DataTable dt = new DataTable();
    public string dateHeader, ProjectAlloted, ProjectID;
    string[] colors = new string[] { "navy", "maroon", "orange" };
    string var_TaskCompleted, Status, ProjectStatus;
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString());
    SqlConnection con2 = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString());
    SqlCommand com_Users = new SqlCommand();
    SqlCommand com_Tasks = new SqlCommand();
    SqlCommand com_insert = new SqlCommand();
    SqlCommand com_Modal = new SqlCommand();
    SqlCommand com_projectView = new SqlCommand();
    SqlCommand com_Check = new SqlCommand();
    clsDataControl objData = new clsDataControl();
    ProductionCalculation pcData = new ProductionCalculation();
    /// <summary>
    /// Page_s the load.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Status = Request.QueryString.Get("Status");
            //if (Status.Substring(0, 1) == "p")
            //{
            //    project_gridbind();
            //}
            // else
            // {
            lbl_SeparatorMonthYear.Visible = false;
            ddl_FilterMonth.Visible = false;
            ddl_FilterYear.Visible = false;
            bindYearOfRecords();
            bindMonthOfRecords();
            ddl_FilterYear.SelectedValue = Convert.ToString(DateTime.Today.Year);
            ddl_FilterMonth.SelectedValue = Convert.ToString(DateTime.Today.Month);
            ddl_FilterYear_SelectedIndexChanged(sender, e);
            BindGrid();
            //}
        }
    }

    /// <summary>
    /// Binds the grid of Selected Project Status / TaskStatus.
    /// </summary>
    protected void BindGrid()
    {
        try
        {
            com_Tasks.Connection = con;
            con.Close();
            con.Open();
            ViewState["Status"] = "";
            int count = Request.Url.Segments.Count();
            Status1 = Request.Url.Segments[count - 1].ToString();
            Status = Status1.Substring(Status1.ToString().Length - 1);
            string teamUsers = string.Empty;
            string teams = string.Empty;
            if (Convert.ToString(Session["Rights"]) == "Team Leader")
            {
                teamUsers = objData.GetSingleData("select ''''+REPLACE(userid,',',''',''')+''''[users] from tbl_teamAllotmentMaster where teamleader='" + Convert.ToString(Session["UserID"]) + "'");
                DataTable dt_Teams = objData.Getdata("select TeamID from tbl_teamAllotmentMaster where teamleader like ('%" + Session["Userid"] + "%')");

                foreach (DataRow row in dt_Teams.Rows)
                {
                    teams += "e.allotedteamid like '%" + row[0] + "%' or ";
                }
                teams = teams.Substring(0, teams.Length - 3);
            }
            SqlDataAdapter da = new SqlDataAdapter();
            if (Status != "null")
            {
                if (Status == "1")
                {
                    lbl_SeparatorMonthYear.Visible = false;
                    ddl_FilterMonth.Visible = false;
                    ddl_FilterYear.Visible = false;
                    grd_Projects.Visible = false;
                    ViewState["Status"] = "Yet To Start";
                    con.Close();
                    string Rights = Convert.ToString(Session["Rights"]);
                    if (Rights == "Team Leader")
                        da = new SqlDataAdapter("select a.id,a.requestid,a.taskstatus,a.taskname,a.taskdescription,a.projectid,a.stageid,a.scopeid,'TBD' as Production,a.teamid,a.userid,CONVERT(VARCHAR(20),a.requestdate,103) AS requestdate,CONVERT(VARCHAR(20),a.requireddate,103) AS requireddate,b.Stage as Stage,c.Scope as Scope,f.Teamname as AllotedTeam,d.username as Username,e.projectname as Project from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid  inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid inner join tbl_teams f on f.TeamID=a.teamid  and (" + teams + ") where a.taskstatus in('Yet To Start') and a.userid in(" + teamUsers + ") order by a.id desc", con);
                    else if (Rights == "Administrator")
                        da = new SqlDataAdapter("select a.id,a.requestid,a.taskstatus,a.taskname,a.taskdescription,a.projectid,a.stageid,a.scopeid,'TBD' as Production,a.teamid,a.userid,CONVERT(VARCHAR(20),a.requestdate,103) AS requestdate,CONVERT(VARCHAR(20),a.requireddate,103) AS requireddate,b.Stage as Stage,c.Scope as Scope,f.Teamname as AllotedTeam,d.username as Username,e.projectname as Project from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid inner join tbl_teams f on f.TeamID=a.teamid  where a.taskstatus in('Yet To Start') order by a.id desc", con);
                    else
                        da = new SqlDataAdapter("select a.id,a.requestid,a.taskstatus,a.taskname,a.taskdescription,a.projectid,a.stageid,a.scopeid,'TBD' as Production,a.teamid,a.userid,CONVERT(VARCHAR(20),a.requestdate,103) AS requestdate,CONVERT(VARCHAR(20),a.requireddate,103) AS requireddate,b.Stage as Stage,c.Scope as Scope,f.Teamname as AllotedTeam,d.username as Username,e.projectname as Project from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid inner join tbl_teams f on f.TeamID=a.teamid  where a.taskstatus in('Yet To Start') and a.userid='" + Session["Userid"] + "' order by a.id desc", con);
                }
                else if (Status == "2")
                {
                    lbl_SeparatorMonthYear.Visible = false;
                    ddl_FilterMonth.Visible = false;
                    ddl_FilterYear.Visible = false;
                    grd_Projects.Visible = false;
                    Status = Request.QueryString.Get("Status");
                    ViewState["Status"] = "WIP";
                    con.Close();
                    string Rights = Convert.ToString(Session["Rights"]);
                    if (Rights == "Team Leader")
                        da = new SqlDataAdapter("select a.id,a.requestid,a.taskstatus,a.taskname,a.taskdescription,a.projectid,a.stageid,a.scopeid,'TBD' as Production,a.teamid,a.userid,CONVERT(VARCHAR(20),a.requestdate,103) AS requestdate,CONVERT(VARCHAR(20),a.requireddate,103) AS requireddate,b.Stage as Stage,c.Scope as Scope,f.Teamname as AllotedTeam,d.username as Username,e.projectname as Project from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid  inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid inner join tbl_teams f on f.TeamID=a.teamid and (" + teams + ") where a.taskstatus in('WIP') and a.userid in(" + teamUsers + ") order by a.id desc", con);
                    else if (Rights == "Administrator")
                        da = new SqlDataAdapter("select a.id,a.requestid,a.taskstatus,a.taskname,a.taskdescription,a.projectid,a.stageid,a.scopeid,'TBD' as Production,a.teamid,a.userid,CONVERT(VARCHAR(20),a.requestdate,103) AS requestdate,CONVERT(VARCHAR(20),a.requireddate,103) AS requireddate,b.Stage as Stage,c.Scope as Scope,f.Teamname as AllotedTeam,d.username as Username,e.projectname as Project from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid  inner join tbl_teams f on f.TeamID=a.teamid  where a.taskstatus in('WIP') order by a.id desc", con);
                    else
                        da = new SqlDataAdapter("select a.id,a.requestid,a.taskstatus,a.taskname,a.taskdescription,a.projectid,a.stageid,a.scopeid,'TBD' as Production,a.teamid,a.userid,CONVERT(VARCHAR(20),a.requestdate,103) AS requestdate,CONVERT(VARCHAR(20),a.requireddate,103) AS requireddate,b.Stage as Stage,c.Scope as Scope,f.Teamname as AllotedTeam,d.username as Username,e.projectname as Project from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid inner join tbl_teams f on f.TeamID=a.teamid  where a.taskstatus in('WIP') and a.userid='" + Session["Userid"] + "' order by a.id desc", con);
                }
                else if (Status == "3")
                {
                    lbl_SeparatorMonthYear.Visible = false;
                    ddl_FilterMonth.Visible = false;
                    ddl_FilterYear.Visible = false;
                    grd_Projects.Visible = false;
                    Status = Request.QueryString.Get("Status");
                    ViewState["Status"] = "Hold";
                    con.Close();
                    string Rights = Convert.ToString(Session["Rights"]);
                    if (Rights == "Team Leader")
                        da = new SqlDataAdapter("select a.id,a.requestid,a.taskstatus,a.taskname,a.taskdescription,a.projectid,a.stageid,a.scopeid,'TBD' as Production,a.teamid,a.userid,CONVERT(VARCHAR(20),a.requestdate,103) AS requestdate,CONVERT(VARCHAR(20),a.requireddate,103) AS requireddate,b.Stage as Stage,c.Scope as Scope,f.Teamname as AllotedTeam,d.username as Username,e.projectname as Project from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid  inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid  inner join tbl_teams f on f.TeamID=a.teamid  and (" + teams + ") where a.taskstatus in('Hold')  and a.userid in(" + teamUsers + ") order by a.id desc", con);
                    else if (Rights == "Administrator")
                        da = new SqlDataAdapter("select a.id,a.requestid,a.taskstatus,a.taskname,a.taskdescription,a.projectid,a.stageid,a.scopeid,'TBD' as Production,a.teamid,a.userid,CONVERT(VARCHAR(20),a.requestdate,103) AS requestdate,CONVERT(VARCHAR(20),a.requireddate,103) AS requireddate,b.Stage as Stage,c.Scope as Scope,f.Teamname as AllotedTeam,d.username as Username,e.projectname as Project from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid  inner join tbl_teams f on f.TeamID=a.teamid  where a.taskstatus in('Hold') order by a.id desc", con);
                    else
                        da = new SqlDataAdapter("select a.id,a.requestid,a.taskstatus,a.taskname,a.taskdescription,a.projectid,a.stageid,a.scopeid,'TBD' as Production,a.teamid,a.userid,CONVERT(VARCHAR(20),a.requestdate,103) AS requestdate,CONVERT(VARCHAR(20),a.requireddate,103) AS requireddate,b.Stage as Stage,c.Scope as Scope,f.Teamname as AllotedTeam,d.username as Username,e.projectname as Project from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid inner join tbl_teams f on f.TeamID=a.teamid  where a.taskstatus in('Hold') and a.userid='" + Session["Userid"] + "' order by a.id desc", con);
                }
                else if (Status == "5")
                {
                    lbl_SeparatorMonthYear.Visible = true;
                    ddl_FilterMonth.Visible = true;
                    ddl_FilterYear.Visible = true;
                    grd_Projects.Visible = true;
                    Status = Request.QueryString.Get("Status");
                    ViewState["Status"] = "Completed";
                    con.Close();
                    string Rights = Convert.ToString(Session["Rights"]);
                    if (Rights == "Team Leader")
                        da = new SqlDataAdapter("select a.id,a.requestid,a.taskstatus,a.taskname,a.taskdescription,a.projectid,a.stageid,a.scopeid,a.Production,a.teamid,a.userid,CONVERT(VARCHAR(20),a.requestdate,103) AS requestdate,CONVERT(VARCHAR(20),a.requireddate,103) AS requireddate,b.Stage as Stage,c.Scope as Scope,f.Teamname as AllotedTeam,d.username as Username,e.projectname as Project from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid  inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid  inner join tbl_teams f on f.TeamID=a.teamid  and (" + teams + ") where a.taskstatus in('Completed')   and a.userid in(" + teamUsers + ") and ((Month(a.requireddate)='" + ddl_FilterMonth.SelectedValue + "' and YEAR(a.requireddate)='" + ddl_FilterYear.SelectedValue + "') or (Month(a.requestdate)='" + ddl_FilterMonth.SelectedValue + "' and YEAR(a.requestdate)='" + ddl_FilterYear.SelectedValue + "')) order by a.id desc", con);
                    else if (Rights == "Administrator")
                        da = new SqlDataAdapter("select a.id,a.requestid,a.taskstatus,a.taskname,a.taskdescription,a.projectid,a.stageid,a.scopeid,a.Production,a.teamid,a.userid,CONVERT(VARCHAR(20),a.requestdate,103) AS requestdate,CONVERT(VARCHAR(20),a.requireddate,103) AS requireddate,b.Stage as Stage,c.Scope as Scope,f.Teamname as AllotedTeam,d.username as Username,e.projectname as Project from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid  inner join tbl_teams f on f.TeamID=a.teamid  where a.taskstatus in('Completed') and ((Month(a.requireddate)='" + ddl_FilterMonth.SelectedValue + "' and YEAR(a.requireddate)='" + ddl_FilterYear.SelectedValue + "') or (Month(a.requestdate)='" + ddl_FilterMonth.SelectedValue + "' and YEAR(a.requestdate)='" + ddl_FilterYear.SelectedValue + "')) order by a.id desc", con);
                    else
                        da = new SqlDataAdapter("select a.id,a.requestid,a.taskstatus,a.taskname,a.taskdescription,a.projectid,a.stageid,a.scopeid,a.Production,a.teamid,a.userid,CONVERT(VARCHAR(20),a.requestdate,103) AS requestdate,CONVERT(VARCHAR(20),a.requireddate,103) AS requireddate,b.Stage as Stage,c.Scope as Scope,f.Teamname as AllotedTeam,d.username as Username,e.projectname as Project from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid inner join tbl_teams f on f.TeamID=a.teamid  where a.taskstatus in('Completed') and a.userid='" + Session["Userid"] + "' and ((Month(a.requireddate)='" + ddl_FilterMonth.SelectedValue + "' and YEAR(a.requireddate)='" + ddl_FilterYear.SelectedValue + "') or (Month(a.requestdate)='" + ddl_FilterMonth.SelectedValue + "' and YEAR(a.requestdate)='" + ddl_FilterYear.SelectedValue + "')) order by a.id desc", con);
                }
                else if (Status == "4")
                {
                    lbl_SeparatorMonthYear.Visible = false;
                    ddl_FilterMonth.Visible = false;
                    ddl_FilterYear.Visible = false;
                    grd_Projects.Visible = false;
                    Status = Request.QueryString.Get("Status");
                    ViewState["Status"] = "Closed";
                    con.Close();
                    string Rights = Convert.ToString(Session["Rights"]);
                    if (Rights == "Team Leader")
                        da = new SqlDataAdapter("select a.id,a.requestid,a.taskstatus,a.taskname,a.taskdescription,a.projectid,a.stageid,a.scopeid,'TBD' as Production,a.teamid,a.userid,CONVERT(VARCHAR(20),a.requestdate,103) AS requestdate,CONVERT(VARCHAR(20),a.requireddate,103) AS requireddate,b.Stage as Stage,c.Scope as Scope,f.Teamname as AllotedTeam,d.username as Username,e.projectname as Project from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid  inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid  inner join tbl_teams f on f.TeamID=a.teamid  and (" + teams + ") where a.taskstatus in('Closed')  and a.userid in(" + teamUsers + ") order by a.id desc", con);
                    else if (Rights == "Administrator")
                        da = new SqlDataAdapter("select a.id,a.requestid,a.taskstatus,a.taskname,a.taskdescription,a.projectid,a.stageid,a.scopeid,'TBD' as Production,a.teamid,a.userid,CONVERT(VARCHAR(20),a.requestdate,103) AS requestdate,CONVERT(VARCHAR(20),a.requireddate,103) AS requireddate,b.Stage as Stage,c.Scope as Scope,f.Teamname as AllotedTeam,d.username as Username,e.projectname as Project from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid inner join tbl_teams f on f.TeamID=a.teamid  where a.taskstatus in('Closed') order by a.id desc", con);
                    else
                        da = new SqlDataAdapter("select a.id,a.requestid,a.taskstatus,a.taskname,a.taskdescription,a.projectid,a.stageid,a.scopeid,'TBD' as Production,a.teamid,a.userid,CONVERT(VARCHAR(20),a.requestdate,103) AS requestdate,CONVERT(VARCHAR(20),a.requireddate,103) AS requireddate,b.Stage as Stage,c.Scope as Scope,f.Teamname as AllotedTeam,d.username as Username,e.projectname as Project from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid inner join tbl_teams f on f.TeamID=a.teamid  where a.taskstatus in('Closed') and a.userid='" + Session["Userid"] + "' order by a.id desc", con);
                }
                else if (Status == "7")
                {
                    lbl_SeparatorMonthYear.Visible = false;
                    ddl_FilterMonth.Visible = false;
                    ddl_FilterYear.Visible = false;
                    grd_Projects.Visible = false;
                    Status = Request.QueryString.Get("Status");
                    ViewState["Status"] = "On Time Delivery";
                    string Rights = Convert.ToString(Session["Rights"]);
                    if (Rights == "Team Leader")
                    {
                        string TeamUsers = objData.GetSingleData("select replace(''''+userid+'''',',',''',''') as Users from tbl_TeamAllotmentMaster where TeamLeader='" + Session["UserId"].ToString() + "'");
                        com_Tasks.CommandText = "select a.id,a.requestid,a.taskstatus,a.taskname,a.taskdescription,a.projectid,a.stageid,a.scopeid,a.Production,a.teamid,a.userid,CONVERT(VARCHAR(20),a.requestdate,103) AS requestdate,CONVERT(VARCHAR(20),a.requireddate,103) AS requireddate,b.Stage as Stage,c.Scope as Scope,f.Teamname as AllotedTeam,d.username as Username,e.projectname as Project from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid  inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid  inner join tbl_teams f on f.TeamID=a.teamid  and a.Production=100 and a.requireddate between '" + Session["CurrentRequestDate"] + "' and  '" + Session["CurrentRequiredDate"] + "' and a.userid in (" + TeamUsers + ") ";
                        if (Session["CurrentEmployeeSelected"] != "")
                        {
                            com_Tasks.CommandText += "and a.userid like '%" + Session["CurrentEmployeeSelected"] + "%'";
                        }
                        com_Tasks.CommandText += "order by a.id desc";
                        da.SelectCommand = com_Tasks;
                    }
                    else if (Rights == "Administrator")
                        da = new SqlDataAdapter("select a.id,a.requestid,a.taskstatus,a.taskname,a.taskdescription,a.projectid,a.stageid,a.scopeid,a.Production,a.teamid,a.userid,CONVERT(VARCHAR(20),a.requestdate,103) AS requestdate,CONVERT(VARCHAR(20),a.requireddate,103) AS requireddate,b.Stage as Stage,c.Scope as Scope,f.Teamname as AllotedTeam,d.username as Username,e.projectname as Project from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid inner join tbl_teams f on f.TeamID=a.teamid  where a.Production=100 and a.requireddate between '" + Session["CurrentRequestDate"] + "' and  '" + Session["CurrentRequiredDate"] + "' and a.userid like '%" + Session["CurrentEmployeeSelected"] + "%' order by a.id desc", con);
                    else
                        da = new SqlDataAdapter("select a.id,a.requestid,a.taskstatus,a.taskname,a.taskdescription,a.projectid,a.stageid,a.scopeid,a.Production,a.teamid,a.userid,CONVERT(VARCHAR(20),a.requestdate,103) AS requestdate,CONVERT(VARCHAR(20),a.requireddate,103) AS requireddate,b.Stage as Stage,c.Scope as Scope,f.Teamname as AllotedTeam,d.username as Username,e.projectname as Project from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid inner join tbl_teams f on f.TeamID=a.teamid  where a.userid='" + Session["Userid"] + "' and a.Production=100 and a.requireddate between '" + Session["CurrentRequestDate"] + "' and  '" + Session["CurrentRequiredDate"] + "' and a.userid like '%" + Session["CurrentEmployeeSelected"] + "%' order by a.id desc", con);
                }
                else if (Status == "6")
                {
                    lbl_SeparatorMonthYear.Visible = false;
                    ddl_FilterMonth.Visible = false;
                    ddl_FilterYear.Visible = false;
                    grd_Projects.Visible = false;
                    Status = Request.QueryString.Get("Status");
                    ViewState["Status"] = "On Early Delivery";
                    string Rights = Convert.ToString(Session["Rights"]);
                    if (Rights == "Team Leader")
                    {
                        string TeamUsers = objData.GetSingleData("select replace(''''+userid+'''',',',''',''') as Users from tbl_TeamAllotmentMaster where TeamLeader='" + Session["UserId"].ToString() + "'");
                        com_Tasks.CommandText = "select a.id,a.requestid,a.taskstatus,a.taskname,a.taskdescription,a.projectid,a.stageid,a.scopeid,a.Production,a.teamid,a.userid,CONVERT(VARCHAR(20),a.requestdate,103) AS requestdate,CONVERT(VARCHAR(20),a.requireddate,103) AS requireddate,b.Stage as Stage,c.Scope as Scope,f.Teamname as AllotedTeam,d.username as Username,e.projectname as Project from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid  inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid  inner join tbl_teams f on f.TeamID=a.teamid  and a.Production>100 and a.requireddate between '" + Session["CurrentRequestDate"] + "' and  '" + Session["CurrentRequiredDate"] + "' and a.userid in (" + TeamUsers + ")";
                        if (Session["CurrentEmployeeSelected"] != "")
                        {
                            com_Tasks.CommandText += "and a.userid like '%" + Session["CurrentEmployeeSelected"] + "%'";
                        }
                        com_Tasks.CommandText += "order by a.id desc";
                        da.SelectCommand = com_Tasks;
                    }
                    else if (Rights == "Administrator")
                        da = new SqlDataAdapter("select a.id,a.requestid,a.taskstatus,a.taskname,a.taskdescription,a.projectid,a.stageid,a.scopeid,a.Production,a.teamid,a.userid,CONVERT(VARCHAR(20),a.requestdate,103) AS requestdate,CONVERT(VARCHAR(20),a.requireddate,103) AS requireddate,b.Stage as Stage,c.Scope as Scope,f.Teamname as AllotedTeam,d.username as Username,e.projectname as Project from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid inner join tbl_teams f on f.TeamID=a.teamid  where  a.Production>100 and a.requireddate between '" + Session["CurrentRequestDate"] + "' and  '" + Session["CurrentRequiredDate"] + "' and a.userid like '%" + Session["CurrentEmployeeSelected"] + "%' order by a.id desc", con);
                    else
                        da = new SqlDataAdapter("select a.id,a.requestid,a.taskstatus,a.taskname,a.taskdescription,a.projectid,a.stageid,a.scopeid,a.Production,a.teamid,a.userid,CONVERT(VARCHAR(20),a.requestdate,103) AS requestdate,CONVERT(VARCHAR(20),a.requireddate,103) AS requireddate,b.Stage as Stage,c.Scope as Scope,f.Teamname as AllotedTeam,d.username as Username,e.projectname as Project from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid inner join tbl_teams f on f.TeamID=a.teamid  where  a.userid='" + Session["Userid"] + "' and a.Production>100 and a.requireddate between '" + Session["CurrentRequestDate"] + "' and  '" + Session["CurrentRequiredDate"] + "' and a.userid like '%" + Session["CurrentEmployeeSelected"] + "%' order by a.id desc", con);
                }
                else if (Status == "8")
                {
                    lbl_SeparatorMonthYear.Visible = false;
                    ddl_FilterMonth.Visible = false;
                    ddl_FilterYear.Visible = false;
                    grd_Projects.Visible = false;
                    Status = Request.QueryString.Get("Status");
                    ViewState["Status"] = "Extended Delivery";
                    string Rights = Convert.ToString(Session["Rights"]);
                    if (Rights == "Team Leader")
                    {
                        string TeamUsers = objData.GetSingleData("select replace(''''+userid+'''',',',''',''') as Users from tbl_TeamAllotmentMaster where TeamLeader='" + Session["UserId"].ToString() + "'");
                        com_Tasks.CommandText = "select a.id,a.requestid,a.taskstatus,a.taskname,a.taskdescription,a.projectid,a.stageid,a.scopeid,a.Production,a.teamid,a.userid,CONVERT(VARCHAR(20),a.requestdate,103) AS requestdate,CONVERT(VARCHAR(20),a.requireddate,103) AS requireddate,b.Stage as Stage,c.Scope as Scope,f.Teamname as AllotedTeam,d.username as Username,e.projectname as Project from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid  inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid  inner join tbl_teams f on f.TeamID=a.teamid  and a.Production<100 and a.requireddate between '" + Session["CurrentRequestDate"] + "' and  '" + Session["CurrentRequiredDate"] + "' and a.userid in (" + TeamUsers + ")";
                        if (Session["CurrentEmployeeSelected"] != "")
                        {
                            com_Tasks.CommandText += "and a.userid like '%" + Session["CurrentEmployeeSelected"] + "%'";
                        }
                        com_Tasks.CommandText += "order by a.id desc";
                        da.SelectCommand = com_Tasks;
                    }
                    else if (Rights == "Administrator")
                        da = new SqlDataAdapter("select a.id,a.requestid,a.taskstatus,a.taskname,a.taskdescription,a.projectid,a.stageid,a.scopeid,a.Production,a.teamid,a.userid,CONVERT(VARCHAR(20),a.requestdate,103) AS requestdate,CONVERT(VARCHAR(20),a.requireddate,103) AS requireddate,b.Stage as Stage,c.Scope as Scope,f.Teamname as AllotedTeam,d.username as Username,e.projectname as Project from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid inner join tbl_teams f on f.TeamID=a.teamid  where a.Production<100 and a.requireddate between '" + Session["CurrentRequestDate"] + "' and  '" + Session["CurrentRequiredDate"] + "' and a.userid like '%" + Session["CurrentEmployeeSelected"] + "%' order by a.id desc", con);
                    else
                        da = new SqlDataAdapter("select a.id,a.requestid,a.taskstatus,a.taskname,a.taskdescription,a.projectid,a.stageid,a.scopeid,a.Production,a.teamid,a.userid,CONVERT(VARCHAR(20),a.requestdate,103) AS requestdate,CONVERT(VARCHAR(20),a.requireddate,103) AS requireddate,b.Stage as Stage,c.Scope as Scope,f.Teamname as AllotedTeam,d.username as Username,e.projectname as Project from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid inner join tbl_teams f on f.TeamID=a.teamid  where a.Production<100 and a.requireddate between '" + Session["CurrentRequestDate"] + "' and  '" + Session["CurrentRequiredDate"] + "' and a.userid like '%" + Session["CurrentEmployeeSelected"] + "%' and a.userid='" + Session["Userid"] + "' order by a.id desc", con);
                }
                else if (Status == "A")
                {
                    lbl_SeparatorMonthYear.Visible = false;
                    ddl_FilterMonth.Visible = false;
                    ddl_FilterYear.Visible = false;
                    grd_Projects.Visible = false;
                    ViewState["Status"] = "Active Tasks";
                    con.Close();
                    string Rights = Convert.ToString(Session["Rights"]);
                    if (Rights == "Team Leader")
                        da = new SqlDataAdapter("select a.id,a.requestid,a.taskstatus,a.taskname,a.taskdescription,a.projectid,a.stageid,a.scopeid,'TBD' as Production,a.teamid,a.userid,CONVERT(VARCHAR(20),a.requestdate,103) AS requestdate,CONVERT(VARCHAR(20),a.requireddate,103) AS requireddate,b.Stage as Stage,c.Scope as Scope,f.Teamname as AllotedTeam,d.username as Username,e.projectname as Project from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid  inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid inner join tbl_teams f on f.TeamID=a.teamid  and e.allotedteamid like '%'+(select TeamID from tbl_teamAllotmentMaster where teamleader like('%" + Session["Userid"] + "%'))+'%' where a.taskstatus in('Yet To Start','WIP') and a.userid in(" + teamUsers + ") order by a.id desc", con);
                    else if (Rights == "Administrator")
                        da = new SqlDataAdapter("select a.id,a.requestid,a.taskstatus,a.taskname,a.taskdescription,a.projectid,a.stageid,a.scopeid,'TBD' as Production,a.teamid,a.userid,CONVERT(VARCHAR(20),a.requestdate,103) AS requestdate,CONVERT(VARCHAR(20),a.requireddate,103) AS requireddate,b.Stage as Stage,c.Scope as Scope,f.Teamname as AllotedTeam,d.username as Username,e.projectname as Project from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid inner join tbl_teams f on f.TeamID=a.teamid  where a.taskstatus in('Yet To Start','WIP') order by a.id desc", con);
                    else
                        da = new SqlDataAdapter("select a.id,a.requestid,a.taskstatus,a.taskname,a.taskdescription,a.projectid,a.stageid,a.scopeid,'TBD' as Production,a.teamid,a.userid,CONVERT(VARCHAR(20),a.requestdate,103) AS requestdate,CONVERT(VARCHAR(20),a.requireddate,103) AS requireddate,b.Stage as Stage,c.Scope as Scope,f.Teamname as AllotedTeam,d.username as Username,e.projectname as Project from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid inner join tbl_teams f on f.TeamID=a.teamid  where a.taskstatus in('Yet To Start','WIP') and a.userid='" + Session["Userid"] + "' order by a.id desc", con);
                }
                if (ViewState["Status"] != "")
                {
                    this.TitleOfPage.InnerText = ViewState["Status"].ToString();
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        grd_WIP.DataSource = ds;
                        grd_WIP.DataBind();

                    }

                    else
                    {
                        ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                        grd_WIP.DataSource = ds;
                        grd_WIP.DataBind();
                        int columncount = grd_WIP.Rows[0].Cells.Count;
                        grd_WIP.Rows[0].Cells.Clear();
                        grd_WIP.Rows[0].Cells.Add(new TableCell());
                        grd_WIP.Rows[0].Cells[0].ColumnSpan = columncount;
                        grd_WIP.Rows[0].Cells[0].Text = "<center>----- No Records Found -----</center>";
                    }
                }

            }
        }
        catch (Exception)
        {
        }
    }
    /// <summary>
    /// Export to Excel
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_ExportToExcel_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Status"] = "";
            int count = Request.Url.Segments.Count();
            Status1 = Request.Url.Segments[count - 1].ToString();
            Status = Status1.Substring(Status1.ToString().Length - 1);
            string teamUsers = string.Empty;
            string teams = string.Empty;
            if (Convert.ToString(Session["Rights"]) == "Team Leader")
            {
                teamUsers = objData.GetSingleData("select ''''+REPLACE(userid,',',''',''')+''''[users] from tbl_teamAllotmentMaster where teamleader='" + Convert.ToString(Session["UserID"]) + "'");
                DataTable dt_Teams = objData.Getdata("select TeamID from tbl_teamAllotmentMaster where teamleader like ('%" + Session["Userid"] + "%')");

                foreach (DataRow row in dt_Teams.Rows)
                {
                    teams += "e.allotedteamid like '%" + row[0] + "%' or ";
                }
                teams = teams.Substring(0, teams.Length - 3);
            }
            SqlDataAdapter da = new SqlDataAdapter();
            if (Status != "null")
            {
                if (Status == "1")
                {
                    grd_Projects.Visible = false;
                    ViewState["Status"] = "Yet To Start";
                    con.Close();
                    string Rights = Convert.ToString(Session["Rights"]);
                    if (Rights == "Team Leader")
                        da = new SqlDataAdapter("select d.userid+' - '+d.username[username],e.projectname as Project,b.Stage as Stage,c.Scope as Scope,a.taskname as Task,CONVERT(VARCHAR(20),a.requestdate,103) [Request Date],CONVERT(VARCHAR(20),a.requireddate,103) [Required Date],'TBD' as Productivity from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid  inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid inner join tbl_teams f on f.TeamID=a.teamid  and ("+teams+") where a.taskstatus in('Yet To Start') and a.userid in(" + teamUsers + ") order by a.id desc", con);
                    else if (Rights == "Administrator")
                        da = new SqlDataAdapter("select d.userid+' - '+d.username[username],e.projectname as Project,b.Stage as Stage,c.Scope as Scope,a.taskname as Task,CONVERT(VARCHAR(20),a.requestdate,103) [Request Date],CONVERT(VARCHAR(20),a.requireddate,103) [Required Date],'TBD' as Productivity from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid inner join tbl_teams f on f.TeamID=a.teamid  where a.taskstatus in('Yet To Start') order by a.id desc", con);
                    else
                        da = new SqlDataAdapter("select d.userid+' - '+d.username[username],e.projectname as Project,b.Stage as Stage,c.Scope as Scope,a.taskname as Task,CONVERT(VARCHAR(20),a.requestdate,103) [Request Date],CONVERT(VARCHAR(20),a.requireddate,103) [Required Date],'TBD' as Productivity from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid inner join tbl_teams f on f.TeamID=a.teamid  where a.taskstatus in('Yet To Start') and a.userid='" + Session["Userid"] + "' order by a.id desc", con);
                }
                else if (Status == "2")
                {
                    grd_Projects.Visible = false;
                    Status = Request.QueryString.Get("Status");
                    ViewState["Status"] = "WIP";
                    con.Close();
                    string Rights = Convert.ToString(Session["Rights"]);
                    if (Rights == "Team Leader")
                        da = new SqlDataAdapter("select d.userid+' - '+d.username[username],e.projectname as Project,b.Stage as Stage,c.Scope as Scope,a.taskname as Task,CONVERT(VARCHAR(20),a.requestdate,103) [Request Date],CONVERT(VARCHAR(20),a.requireddate,103) [Required Date],'TBD' as Productivity from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid  inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid inner join tbl_teams f on f.TeamID=a.teamid  and (" + teams + ") where a.taskstatus in('WIP') and a.userid in(" + teamUsers + ") order by a.id desc", con);
                    else if (Rights == "Administrator")
                        da = new SqlDataAdapter("select d.userid+' - '+d.username[username],e.projectname as Project,b.Stage as Stage,c.Scope as Scope,a.taskname as Task,CONVERT(VARCHAR(20),a.requestdate,103) [Request Date],CONVERT(VARCHAR(20),a.requireddate,103) [Required Date],'TBD' as Productivity from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid  inner join tbl_teams f on f.TeamID=a.teamid  where a.taskstatus in('WIP') order by a.id desc", con);
                    else
                        da = new SqlDataAdapter("select d.userid+' - '+d.username[username],e.projectname as Project,b.Stage as Stage,c.Scope as Scope,a.taskname as Task,CONVERT(VARCHAR(20),a.requestdate,103) [Request Date],CONVERT(VARCHAR(20),a.requireddate,103) [Required Date],'TBD' as Productivity from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid inner join tbl_teams f on f.TeamID=a.teamid  where a.taskstatus in('WIP') and a.userid='" + Session["Userid"] + "' order by a.id desc", con);
                }
                else if (Status == "3")
                {
                    grd_Projects.Visible = false;
                    Status = Request.QueryString.Get("Status");
                    ViewState["Status"] = "Hold";
                    con.Close();
                    string Rights = Convert.ToString(Session["Rights"]);
                    if (Rights == "Team Leader")
                        da = new SqlDataAdapter("select d.userid+' - '+d.username[username],e.projectname as Project,b.Stage as Stage,c.Scope as Scope,a.taskname as Task,CONVERT(VARCHAR(20),a.requestdate,103) [Request Date],CONVERT(VARCHAR(20),a.requireddate,103) [Required Date],'TBD' as Productivity from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid  inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid  inner join tbl_teams f on f.TeamID=a.teamid  and (" + teams + ") where a.taskstatus in('Hold') and a.userid in(" + teamUsers + ") order by a.id desc", con);
                    else if (Rights == "Administrator")
                        da = new SqlDataAdapter("select d.userid+' - '+d.username[username],e.projectname as Project,b.Stage as Stage,c.Scope as Scope,a.taskname as Task,CONVERT(VARCHAR(20),a.requestdate,103) [Request Date],CONVERT(VARCHAR(20),a.requireddate,103) [Required Date],'TBD' as Productivity from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid  inner join tbl_teams f on f.TeamID=a.teamid  where a.taskstatus in('Hold') order by a.id desc", con);
                    else
                        da = new SqlDataAdapter("select d.userid+' - '+d.username[username],e.projectname as Project,b.Stage as Stage,c.Scope as Scope,a.taskname as Task,CONVERT(VARCHAR(20),a.requestdate,103) [Request Date],CONVERT(VARCHAR(20),a.requireddate,103) [Required Date],'TBD' as Productivity from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid inner join tbl_teams f on f.TeamID=a.teamid  where a.taskstatus in('Hold') and a.userid='" + Session["Userid"] + "' order by a.id desc", con);
                }
                else if (Status == "5")
                {
                    grd_Projects.Visible = false;
                    Status = Request.QueryString.Get("Status");
                    ViewState["Status"] = "Completed";
                    con.Close();
                    string Rights = Convert.ToString(Session["Rights"]);
                    if (Rights == "Team Leader")
                        da = new SqlDataAdapter("select d.userid+' - '+d.username[username],e.projectname as Project,b.Stage as Stage,c.Scope as Scope,a.taskname as Task,CONVERT(VARCHAR(20),a.requestdate,103) [Request Date],CONVERT(VARCHAR(20),a.requireddate,103) [Required Date],a.Production as Productivity from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid  inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid  inner join tbl_teams f on f.TeamID=a.teamid  and (" + teams + ") where a.taskstatus in('Completed') and a.userid in(" + teamUsers + ") and ((Month(a.requireddate)='" + ddl_FilterMonth.SelectedValue + "' and YEAR(a.requireddate)='" + ddl_FilterYear.SelectedValue + "') or (Month(a.requestdate)='" + ddl_FilterMonth.SelectedValue + "' and YEAR(a.requestdate)='" + ddl_FilterYear.SelectedValue + "')) order by a.id desc", con);
                    else if (Rights == "Administrator")
                        da = new SqlDataAdapter("select d.userid+' - '+d.username[username],e.projectname as Project,b.Stage as Stage,c.Scope as Scope,a.taskname as Task,CONVERT(VARCHAR(20),a.requestdate,103) [Request Date],CONVERT(VARCHAR(20),a.requireddate,103) [Required Date],a.Production as Productivity from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid  inner join tbl_teams f on f.TeamID=a.teamid  where a.taskstatus in('Completed') and ((Month(a.requireddate)='" + ddl_FilterMonth.SelectedValue + "' and YEAR(a.requireddate)='" + ddl_FilterYear.SelectedValue + "') or (Month(a.requestdate)='" + ddl_FilterMonth.SelectedValue + "' and YEAR(a.requestdate)='" + ddl_FilterYear.SelectedValue + "')) order by a.id desc", con);
                    else
                        da = new SqlDataAdapter("select d.userid+' - '+d.username[username],e.projectname as Project,b.Stage as Stage,c.Scope as Scope,a.taskname as Task,CONVERT(VARCHAR(20),a.requestdate,103) [Request Date],CONVERT(VARCHAR(20),a.requireddate,103) [Required Date],a.Production as Productivity from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid inner join tbl_teams f on f.TeamID=a.teamid  where a.taskstatus in('Completed') and a.userid='" + Session["Userid"] + "' and ((Month(a.requireddate)='" + ddl_FilterMonth.SelectedValue + "' and YEAR(a.requireddate)='" + ddl_FilterYear.SelectedValue + "') or (Month(a.requestdate)='" + ddl_FilterMonth.SelectedValue + "' and YEAR(a.requestdate)='" + ddl_FilterYear.SelectedValue + "')) order by a.id desc", con);
                }
                else if (Status == "4")
                {
                    grd_Projects.Visible = false;
                    Status = Request.QueryString.Get("Status");
                    ViewState["Status"] = "Closed";
                    con.Close();
                    //string Rights = Convert.ToString(Session["Rights"]);
                    //if (Rights == "Team Leader")
                    //    da = new SqlDataAdapter("select d.userid+' - '+d.username[username],a.id,a.requestid,a.taskstatus,a.taskname,a.taskdescription,a.projectid,a.stageid,a.scopeid,'TBD' as Productivity,a.teamid,a.userid,CONVERT(VARCHAR(20),a.requestdate,103) AS requestdate,CONVERT(VARCHAR(20),a.requireddate,103) AS requireddate,b.Stage as Stage,c.Scope as Scope,f.Teamname as AllotedTeam,d.username as Username,e.projectname as Project from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid  inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid  inner join tbl_teams f on f.TeamID=a.teamid  and e.allotedteamid like '%'+(select TeamID from tbl_teamAllotmentMaster where teamleader like('%" + Session["Userid"] + "%'))+'%' where a.taskstatus in('Closed') and a.userid in(" + teamUsers + ") order by a.id desc", con);
                    //else if (Rights == "Administrator")
                    //    da = new SqlDataAdapter("select d.userid+' - '+d.username[username],a.id,a.requestid,a.taskstatus,a.taskname,a.taskdescription,a.projectid,a.stageid,a.scopeid,'TBD' as Productivity,a.teamid,a.userid,CONVERT(VARCHAR(20),a.requestdate,103) AS requestdate,CONVERT(VARCHAR(20),a.requireddate,103) AS requireddate,b.Stage as Stage,c.Scope as Scope,f.Teamname as AllotedTeam,d.username as Username,e.projectname as Project from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid inner join tbl_teams f on f.TeamID=a.teamid  where a.taskstatus in('Closed') order by a.id desc", con);
                    //else
                    //    da = new SqlDataAdapter("select d.userid+' - '+d.username[username],a.id,a.requestid,a.taskstatus,a.taskname,a.taskdescription,a.projectid,a.stageid,a.scopeid,'TBD' as Productivity,a.teamid,a.userid,CONVERT(VARCHAR(20),a.requestdate,103) AS requestdate,CONVERT(VARCHAR(20),a.requireddate,103) AS requireddate,b.Stage as Stage,c.Scope as Scope,f.Teamname as AllotedTeam,d.username as Username,e.projectname as Project from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid inner join tbl_teams f on f.TeamID=a.teamid  where a.taskstatus in('Closed') and a.userid='" + Session["Userid"] + "' order by a.id desc", con);
                    string Rights = Convert.ToString(Session["Rights"]);
                    if (Rights == "Team Leader")
                        da = new SqlDataAdapter("select d.userid+' - '+d.username[username],e.projectname as Project,b.Stage as Stage,c.Scope as Scope,a.taskname as Task,CONVERT(VARCHAR(20),a.requestdate,103) [Request Date],CONVERT(VARCHAR(20),a.requireddate,103) [Required Date],'TBD' as Productivity from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid  inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid  inner join tbl_teams f on f.TeamID=a.teamid  and (" + teams + ") where a.taskstatus in('Closed') and a.userid in(" + teamUsers + ") order by a.id desc", con);
                    else if (Rights == "Administrator")
                        da = new SqlDataAdapter("select d.userid+' - '+d.username[username],e.projectname as Project,b.Stage as Stage,c.Scope as Scope,a.taskname as Task,CONVERT(VARCHAR(20),a.requestdate,103) [Request Date],CONVERT(VARCHAR(20),a.requireddate,103) [Required Date],'TBD' as Productivity from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid  inner join tbl_teams f on f.TeamID=a.teamid  where a.taskstatus in('Closed') order by a.id desc", con);
                    else
                        da = new SqlDataAdapter("select d.userid+' - '+d.username[username],e.projectname as Project,b.Stage as Stage,c.Scope as Scope,a.taskname as Task,CONVERT(VARCHAR(20),a.requestdate,103) [Request Date],CONVERT(VARCHAR(20),a.requireddate,103) [Required Date],'TBD' as Productivity from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid inner join tbl_teams f on f.TeamID=a.teamid  where a.taskstatus in('Closed') and a.userid='" + Session["Userid"] + "' order by a.id desc", con);
                }
                else if (Status == "7")
                {
                    grd_Projects.Visible = false;
                    Status = Request.QueryString.Get("Status");
                    ViewState["Status"] = "On Time Delivery";
                    con.Close();
                    string Rights = Convert.ToString(Session["Rights"]);
                    if (Rights == "Team Leader")
                        da = new SqlDataAdapter("select d.userid+' - '+d.username[username],e.projectname as Project,b.Stage as Stage,c.Scope as Scope,a.taskname as Task,CONVERT(VARCHAR(20),a.requestdate,103) [Request Date],CONVERT(VARCHAR(20),a.requireddate,103) [Required Date],a.Production as Productivity from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid  inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid  inner join tbl_teams f on f.TeamID=a.teamid  and (" + teams + ") and a.Production=100 and a.requireddate between '" + Session["CurrentRequestDate"] + "' and  '" + Session["CurrentRequiredDate"] + "' and a.userid like '%" + Session["CurrentEmployeeSelected"] + "%' and a.requireddate between '" + Session["CurrentRequestDate"] + "' and  '" + Session["CurrentRequiredDate"] + "' and a.userid like '%" + Session["CurrentEmployeeSelected"] + "%' order by a.id desc", con);
                    else if (Rights == "Administrator")
                        da = new SqlDataAdapter("select d.userid+' - '+d.username[username],e.projectname as Project,b.Stage as Stage,c.Scope as Scope,a.taskname as Task,CONVERT(VARCHAR(20),a.requestdate,103) [Request Date],CONVERT(VARCHAR(20),a.requireddate,103) [Required Date],a.Production as Productivity from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid inner join tbl_teams f on f.TeamID=a.teamid  where a.Production=100 and a.requireddate between '" + Session["CurrentRequestDate"] + "' and  '" + Session["CurrentRequiredDate"] + "' and a.userid like '%" + Session["CurrentEmployeeSelected"] + "%' order by a.id desc", con);
                    else
                        da = new SqlDataAdapter("select d.userid+' - '+d.username[username],e.projectname as Project,b.Stage as Stage,c.Scope as Scope,a.taskname as Task,CONVERT(VARCHAR(20),a.requestdate,103) [Request Date],CONVERT(VARCHAR(20),a.requireddate,103) [Required Date],a.Production as Productivity from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid inner join tbl_teams f on f.TeamID=a.teamid  where a.userid='" + Session["Userid"] + "' and a.Production=100 and a.requireddate between '" + Session["CurrentRequestDate"] + "' and  '" + Session["CurrentRequiredDate"] + "' and a.userid like '%" + Session["CurrentEmployeeSelected"] + "%' order by a.id desc", con);
                }
                else if (Status == "6")
                {
                    grd_Projects.Visible = false;
                    Status = Request.QueryString.Get("Status");
                    ViewState["Status"] = "On Early Delivery";
                    con.Close();
                    string Rights = Convert.ToString(Session["Rights"]);
                    if (Rights == "Team Leader")
                        da = new SqlDataAdapter("select d.userid+' - '+d.username[username],e.projectname as Project,b.Stage as Stage,c.Scope as Scope,a.taskname as Task,CONVERT(VARCHAR(20),a.requestdate,103) [Request Date],CONVERT(VARCHAR(20),a.requireddate,103) [Required Date],a.Production as Productivity from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid  inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid  inner join tbl_teams f on f.TeamID=a.teamid  and (" + teams + ") where a.Production>100 and a.requireddate between '" + Session["CurrentRequestDate"] + "' and  '" + Session["CurrentRequiredDate"] + "' and a.userid like '%" + Session["CurrentEmployeeSelected"] + "%' order by a.id desc", con);
                    else if (Rights == "Administrator")
                        da = new SqlDataAdapter("select d.userid+' - '+d.username[username],e.projectname as Project,b.Stage as Stage,c.Scope as Scope,a.taskname as Task,CONVERT(VARCHAR(20),a.requestdate,103) [Request Date],CONVERT(VARCHAR(20),a.requireddate,103) [Required Date],a.Production as Productivity from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid inner join tbl_teams f on f.TeamID=a.teamid  where  a.Production>100 and a.requireddate between '" + Session["CurrentRequestDate"] + "' and  '" + Session["CurrentRequiredDate"] + "' and a.userid like '%" + Session["CurrentEmployeeSelected"] + "%' order by a.id desc", con);
                    else
                        da = new SqlDataAdapter("select d.userid+' - '+d.username[username],e.projectname as Project,b.Stage as Stage,c.Scope as Scope,a.taskname as Task,CONVERT(VARCHAR(20),a.requestdate,103) [Request Date],CONVERT(VARCHAR(20),a.requireddate,103) [Required Date],a.Production as Productivity from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid inner join tbl_teams f on f.TeamID=a.teamid  where  a.userid='" + Session["Userid"] + "' and a.Production>100 and a.requireddate between '" + Session["CurrentRequestDate"] + "' and  '" + Session["CurrentRequiredDate"] + "' and a.userid like '%" + Session["CurrentEmployeeSelected"] + "%' order by a.id desc", con);
                }
                else if (Status == "8")
                {
                    grd_Projects.Visible = false;
                    Status = Request.QueryString.Get("Status");
                    ViewState["Status"] = "Extended Delivery";
                    con.Close();
                    string Rights = Convert.ToString(Session["Rights"]);
                    if (Rights == "Team Leader")
                        da = new SqlDataAdapter("select d.userid+' - '+d.username[username],e.projectname as Project,b.Stage as Stage,c.Scope as Scope,a.taskname as Task,CONVERT(VARCHAR(20),a.requestdate,103) [Request Date],CONVERT(VARCHAR(20),a.requireddate,103) [Required Date],a.Production as Productivity from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid  inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid  inner join tbl_teams f on f.TeamID=a.teamid  and (" + teams + ") where  a.Production<100 and a.requireddate between '" + Session["CurrentRequestDate"] + "' and  '" + Session["CurrentRequiredDate"] + "' and a.userid like '%" + Session["CurrentEmployeeSelected"] + "%' order by a.id desc", con);
                    else if (Rights == "Administrator")
                        da = new SqlDataAdapter("select d.userid+' - '+d.username[username],e.projectname as Project,b.Stage as Stage,c.Scope as Scope,a.taskname as Task,CONVERT(VARCHAR(20),a.requestdate,103) [Request Date],CONVERT(VARCHAR(20),a.requireddate,103) [Required Date],a.Production as Productivity from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid inner join tbl_teams f on f.TeamID=a.teamid  where a.Production<100 and a.requireddate between '" + Session["CurrentRequestDate"] + "' and  '" + Session["CurrentRequiredDate"] + "' and a.userid like '%" + Session["CurrentEmployeeSelected"] + "%' order by a.id desc", con);
                    else
                        da = new SqlDataAdapter("select d.userid+' - '+d.username[username],e.projectname as Project,b.Stage as Stage,c.Scope as Scope,a.taskname as Task,CONVERT(VARCHAR(20),a.requestdate,103) [Request Date],CONVERT(VARCHAR(20),a.requireddate,103) [Required Date],a.Production as Productivity from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid inner join tbl_teams f on f.TeamID=a.teamid  where a.Production<100 and a.requireddate between '" + Session["CurrentRequestDate"] + "' and  '" + Session["CurrentRequiredDate"] + "' and a.userid like '%" + Session["CurrentEmployeeSelected"] + "%' and a.userid='" + Session["Userid"] + "' order by a.id desc", con);
                }
                else if (Status == "A")
                {
                    grd_Projects.Visible = false;
                    Status = Request.QueryString.Get("Status");
                    ViewState["Status"] = "Active Tasks";
                    con.Close();
                    string Rights = Convert.ToString(Session["Rights"]);
                    if (Rights == "Team Leader")
                        da = new SqlDataAdapter("select d.userid+' - '+d.username[username],e.projectname as Project,b.Stage as Stage,c.Scope as Scope,a.taskname as Task,CONVERT(VARCHAR(20),a.requestdate,103) [Request Date],CONVERT(VARCHAR(20),a.requireddate,103) [Required Date],'TBD' as Productivity from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid  inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid inner join tbl_teams f on f.TeamID=a.teamid  and (" + teams + ") where a.taskstatus in('Yet To Start','WIP') and a.userid in(" + teamUsers + ") order by a.id desc", con);
                    else if (Rights == "Administrator")
                        da = new SqlDataAdapter("select d.userid+' - '+d.username[username],e.projectname as Project,b.Stage as Stage,c.Scope as Scope,a.taskname as Task,CONVERT(VARCHAR(20),a.requestdate,103) [Request Date],CONVERT(VARCHAR(20),a.requireddate,103) [Required Date],'TBD' as Productivity from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid  inner join tbl_teams f on f.TeamID=a.teamid  where a.taskstatus in('Yet To Start','WIP') order by a.id desc", con);
                    else
                        da = new SqlDataAdapter("select d.userid+' - '+d.username[username],e.projectname as Project,b.Stage as Stage,c.Scope as Scope,a.taskname as Task,CONVERT(VARCHAR(20),a.requestdate,103) [Request Date],CONVERT(VARCHAR(20),a.requireddate,103) [Required Date],'TBD' as Productivity from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid inner join tbl_teams f on f.TeamID=a.teamid  where a.taskstatus in('Yet To Start','WIP') and a.userid='" + Session["Userid"] + "' order by a.id desc", con);
                }
                da.Fill(dt);
                if (ViewState["Status"] != "")
                {
                    List<string> HeaderNames = new List<string>();
                    HeaderNames.Add("Task By");
                    HeaderNames.Add("Project");
                    HeaderNames.Add("Stage");
                    HeaderNames.Add("Scope");
                    HeaderNames.Add("Task");
                    HeaderNames.Add("Request Date");
                    HeaderNames.Add("Required Date");
                    HeaderNames.Add("Productivity");
                    List<string> ExcelReport = pcData.generateExcelReport(dt, "StatusReport", "GenericReports", "Status Report", 8, HeaderNames);
                    FileInfo file = new FileInfo(ExcelReport[2]);
                    Response.Clear();
                    Response.AddHeader("Content-Length", file.Length.ToString(CultureInfo.InvariantCulture));
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + ("StatusReport.xls") + "\"");
                    Response.ContentType = "application/octet-stream";
                    Response.Flush();
                    Response.TransmitFile(ExcelReport[0] + ("StatusReport" + DateTime.Now.ToShortDateString().Replace("/", "") + ".xls"));
                    Response.End();
                }
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
            grd_WIP.PageIndex = e.NewPageIndex;
            this.BindGrid();
        }
        catch (Exception)
        {

        }
    }

    /// <summary>
    /// Scope Paging Support
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void OnPageIndexChangingScope(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grd_WIP.PageIndex = e.NewPageIndex;
            //this.bindScope();
        }
        catch (Exception)
        {

        }
    }

    /// <summary>
    /// Task Update Function
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void grd_WIP_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = grd_WIP.SelectedRow;
        string taskid = ((HiddenField)row.FindControl("hdn_taskid")).Value;
        Session["TaskID"] = taskid;
        Response.Redirect("timetrackingsheet");
    }

    /// <summary>
    /// Task View details Function
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void SelectCurrentData(object sender, EventArgs e)
    {
        lbl_SeparatorMonthYear.Visible = false;
        ddl_FilterMonth.Visible = false;
        ddl_FilterYear.Visible = false;
        string TaskID;
        this.block_View.Visible = true;
        this.block_Grid.Visible = false;
        LinkButton btn = (LinkButton)(sender);
        string id = btn.CommandArgument;
        DataTable dt = null;
        dt = objData.Getdata("select SUM(DATEDIFF(MINUTE,'00:00:00', b.TotalTime))/60 AS hh,SUM(DATEDIFF(MINUTE,'00:00:00', b.TotalTime))%60 as mm from PrmsProductionHour_Backup b where b.task='" + id + "'");
        string TotalTime = dt.Rows[0]["hh"].ToString() + ':' + dt.Rows[0]["mm"].ToString();
        con.Close();
        con.Open();
        com_Tasks.Connection = con;
        com_Tasks.CommandText = "select a.requestid,a.taskname,a.taskdescription,a.projectid,a.stageid,a.scopeid,a.Production,a.teamid,a.userid,CONVERT(VARCHAR(20),a.requestdate,103) AS requestdate,CONVERT(VARCHAR(20),a.requireddate,103) AS requireddate,b.Stage as Stage,c.Scope+' - '+c.Description as Scope,f.Teamname as AllotedTeam,d.username as Username,e.projectreq as Project,a.estimatedtime,a.RatingOfTask,a.QualityRating from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid inner join tbl_ProjectReq e on e.projectid=a.projectid inner join tbl_teams f on f.TeamID=a.teamid where a.id='" + id + "'";
        SqlDataReader dr = com_Tasks.ExecuteReader();
        while (dr.Read())
        {
            TitleOfPage.InnerText = dr["requestid"].ToString() + " : " + dr["Project"];
            lbl_TaskByTextView.InnerText = dr["username"].ToString();
            lbl_TaskTextView.InnerText = dr["taskname"].ToString();
            lbl_TaskDescriptionTextView.InnerText = dr["taskdescription"].ToString();
            lbl_ProjectTextView.InnerText = dr["Project"].ToString();
            lbl_StageTextView.InnerText = dr["Stage"].ToString();
            lbl_ScopeTextView.InnerText = dr["Scope"].ToString();
            lbl_RequestDateTextView.InnerText = dr["requestdate"].ToString();
            lbl_RequiredDateTextView.InnerText = dr["requireddate"].ToString();
            if (dr["estimatedtime"].ToString() == "" || dr["estimatedtime"].ToString() == "__:__")
            {
                lbl_EstimatedHoursTextView.InnerText = "NA";
                TotalTime = "NA";
            }
            else
            {
                lbl_EstimatedHoursTextView.InnerText = dr["estimatedtime"].ToString();
            }
            if (TotalTime != "NA")
            {
                if (TotalTime == ":")
                {
                    TotalTime = "00:00";
                }
            }
            lbl_TimeTakenTextView.InnerText = TotalTime;
            string RatingSelected = dr["RatingOfTask"].ToString();
            string QualityRatingSelected = dr["QualityRating"].ToString();
            ScriptManager.RegisterStartupScript(this, GetType(), "mismatch", "$('#star-" + RatingSelected + "').attr('checked','checked');$('#qualitystar-" + QualityRatingSelected + "').attr('checked','checked');", true);
        }
        dr.Close();
        com_Tasks.CommandText = "select Convert(varchar(10),CurrentDate,103)[CurrentDate],a.EmpNo+' - '+a.EmpName as Employee,RIGHT(a.StartTime,5)[StartTime],RIGHT(a.EndTime,5)[EndTime],a.Break1 as BreakTime,a.TotalTime,a.Remarks,a.statusoftask from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid where a.ProjectID!='NA' and a.task='" + id + "' order by a.CurrentDate";
        SqlDataAdapter da = new SqlDataAdapter(com_Tasks.CommandText, con);
        DataSet ds = new DataSet();
        da.Fill(ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            grd_TaskDetails.DataSource = ds;
            grd_TaskDetails.DataBind();
        }

        else
        {
            ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
            grd_TaskDetails.DataSource = ds;
            grd_TaskDetails.DataBind();
            int columncount = grd_TaskDetails.Rows[0].Cells.Count;
            grd_TaskDetails.Rows[0].Cells.Clear();
            grd_TaskDetails.Rows[0].Cells.Add(new TableCell());
            grd_TaskDetails.Rows[0].Cells[0].ColumnSpan = columncount;
            grd_TaskDetails.Rows[0].Cells[0].Text = "<center>----- No Records Found -----</center>";
        }
        btn_BackToDashboard.Visible = false;
        com_Tasks.CommandText = "select closeddate,* from tbl_TaskHolds where taskid='" + id + "'";
        con.Close();
        con.Open();
        SqlDataReader dr2 = com_Tasks.ExecuteReader();
        if (dr2.HasRows)
        {
            dr2.Read();
            string ClosedDate = dr2[0].ToString();
            if (ClosedDate != "")
            {
                lbl_ClosedDetails.Text = "<b>Task Closed On </b>" + ClosedDate.ToString().Remove(ClosedDate.IndexOf(' '));
            }
            title_TaskHolds.Visible = true;
            grd_TaskHolds.Visible = true;
        }
        dr2.Close();
        com_Tasks.CommandText = "select id,taskid,holdeddate,wipdate,closeddate from tbl_TaskHolds where taskid='" + id + "'";
        SqlDataAdapter da2 = new SqlDataAdapter(com_Tasks.CommandText, con);
        DataSet ds2 = new DataSet();
        da2.Fill(ds2);
        if (ds2.Tables[0].Rows.Count > 0)
        {
            grd_TaskHolds.DataSource = ds2;
            grd_TaskHolds.DataBind();
        }

        else
        {
            ds2.Tables[0].Rows.Add(ds2.Tables[0].NewRow());
            grd_TaskHolds.DataSource = ds2;
            grd_TaskHolds.DataBind();
            int columncount = grd_TaskHolds.Rows[0].Cells.Count;
            grd_TaskHolds.Rows[0].Cells.Clear();
            grd_TaskHolds.Rows[0].Cells.Add(new TableCell());
            grd_TaskHolds.Rows[0].Cells[0].ColumnSpan = columncount;
            grd_TaskHolds.Rows[0].Cells[0].Text = "<center>----- No Records Found -----</center>";
        }
    }

    /// <summary>
    /// Show aLl tasks
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void lnk_All_Click(object sender, EventArgs e)
    {
        ViewState["ALL"] = "Y";
    }

    /// <summary>
    /// Redirect To Dashboard On Back Button On Dashboard gridview
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void btn_BackToDashboard_Click(object sender, EventArgs e)
    {
        Response.Redirect("Dashboard");
    }

    /// <summary>
    /// Change Title On Back button Click
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void btn_BackButton_Click(object sender, EventArgs e)
    {
        btn_BackToDashboard.Visible = true;
        this.block_View.Visible = false;
        this.block_Grid.Visible = true;
        int count = Request.Url.Segments.Count();
        Status1 = Request.Url.Segments[count - 1].ToString();
        Status = Status1.Substring(Status1.ToString().Length - 1);
        if (Status == "5")
        {
            lbl_SeparatorMonthYear.Visible = true;
            ddl_FilterMonth.Visible = true;
            ddl_FilterYear.Visible = true;
        }
        else
        {
            lbl_SeparatorMonthYear.Visible = false;
            ddl_FilterMonth.Visible = false;
            ddl_FilterYear.Visible = false;
        }
        this.TitleOfPage.InnerText = ViewState["Status"].ToString();
    }

    /// <summary>
    /// Row Databound To Differentiate Project status / Task Status
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void grd_WIP_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // Retrieve the underlying data item. In this example
            // the underlying data item is a DataRowView object. 
            DataRowView rowView = (DataRowView)e.Row.DataItem;

            // Retrieve the state value for the current row. 
            String status = rowView["taskstatus"].ToString();
            //format color of the as below 
            if (status == "WIP")
            {
                (e.Row.FindControl("lnk_TaskStatus") as LinkButton).CssClass = "icon glyphicon glyphicon-play fa-2x";
                (e.Row.FindControl("lnk_TaskStatus") as LinkButton).Style.Add("color", "#22A7F0");
            }
            if (status == "Yet To Start")
            {
                (e.Row.FindControl("lnk_TaskStatus") as LinkButton).CssClass = "icon glyphicon glyphicon-inbox fa-2x";
                (e.Row.FindControl("lnk_TaskStatus") as LinkButton).Style.Add("color", "#9FA400");
            }
            if (status == "Completed")
            {
                int ProductionToCalculate = 0;
                string Production = (e.Row.FindControl("lbl_Production") as Label).Text.ToString();
                if (Production != "")
                {
                    ProductionToCalculate = Convert.ToInt32(Production);
                }
                if (ProductionToCalculate == 0 || ProductionToCalculate < 0)
                {
                    HtmlGenericControl div0 = (HtmlGenericControl)e.Row.FindControl("blk_ProgressBar");
                    div0.Attributes["class"] = "progress-bar progress-bar-info progress-bar-striped active";
                    (e.Row.FindControl("lbl_Production") as Label).Text = "NA";
                }
                else if (ProductionToCalculate == 100)
                {
                    HtmlGenericControl div = (HtmlGenericControl)e.Row.FindControl("blk_ProgressBar");
                    div.Attributes["class"] = "progress-bar progress-bar-success progress-bar-striped active";
                    div.Style.Add("width", ProductionToCalculate.ToString() + "px");
                }
                else if (ProductionToCalculate < 100 && ProductionToCalculate > 49)
                {
                    HtmlGenericControl div2 = (HtmlGenericControl)e.Row.FindControl("blk_ProgressBar");
                    div2.Attributes["class"] = "progress-bar progress-bar-warning progress-bar-striped active";
                    div2.Style.Add("width", ProductionToCalculate.ToString() + "px");
                }
                else if (ProductionToCalculate < 50)
                {
                    HtmlGenericControl div4 = (HtmlGenericControl)e.Row.FindControl("blk_ProgressBar");
                    div4.Attributes["class"] = "progress-bar progress-bar-danger progress-bar-striped active";
                    div4.Style.Add("width", ProductionToCalculate.ToString() + "px");
                }
                else if (ProductionToCalculate > 100)
                {
                    HtmlGenericControl div3 = (HtmlGenericControl)e.Row.FindControl("blk_ProgressBar");
                    div3.Attributes["class"] = "progress-bar progress-bar-primary progress-bar-striped active";
                    div3.Style.Add("width", ProductionToCalculate.ToString() + "px");
                }
                string qualityRating = objData.GetSingleData("select QualityRating from tbl_taskmaster where id='" + (e.Row.FindControl("hdn_taskid") as HiddenField).Value + "'");
                string CodingStandardRating = objData.GetSingleData("select RatingOfTask from tbl_taskmaster where id='" + (e.Row.FindControl("hdn_taskid") as HiddenField).Value + "'");
                if (qualityRating == "0" || string.IsNullOrEmpty(qualityRating))
                {
                    (e.Row.FindControl("icon_QualityRating") as HtmlGenericControl).InnerText = "TBD";
                }
                else
                {
                    (e.Row.FindControl("icon_QualityRating") as HtmlGenericControl).InnerText = qualityRating;
                }
                if (CodingStandardRating == "0" || string.IsNullOrEmpty(qualityRating))
                {
                    (e.Row.FindControl("icon_CodingStandardRating") as HtmlGenericControl).InnerText = "TBD";
                }
                else
                {
                    (e.Row.FindControl("icon_CodingStandardRating") as HtmlGenericControl).InnerText = CodingStandardRating;
                }
            }
        }
    }

    /// <summary>
    /// Bind Projects Gridview of Selected project status
    /// </summary>
    protected void project_gridbind()
    {
        try
        {
            ViewState["ProjectStatus"] = "";
            Status = Request.QueryString.Get("Status");
            string Rights = Convert.ToString(Session["Rights"]);
            IFormatProvider culture = new CultureInfo("en-US", true);
            con.Close();
            con.Open();
            if (Rights == "Team Leader")
            {
                con2.Close();
                con2.Open();
                com_Check.Connection = con2;
                com_Check.CommandText = "select TeamID from tbl_teamAllotmentMaster where teamleader like  ('%" + Session["Userid"] + "%')";
                SqlDataReader dr = com_Check.ExecuteReader();
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
                string[] SplittedProjectAlloted = ProjectAlloted.Split(',');
                com_Modal.Connection = con;
                if (Status == "projects_yettostart")
                {
                    ViewState["ProjectStatus"] = "Yet To Start";
                    com_Modal.CommandText = "select CONVERT(VARCHAR(20),receiveddate,103) AS receiveddate,replace(substring(duedate, 4,2) + '/' + substring(duedate, 1, 2)  + '/' + substring(duedate, 7, 4),'/TB/','DPD') AS DueDate2,* from tbl_ProjectReq  where projectstatus='Yet To Start' and (";
                }
                else if (Status == "projects_WIP")
                {
                    ViewState["ProjectStatus"] = "WIP";
                    com_Modal.CommandText = "select CONVERT(VARCHAR(20),receiveddate,103) AS receiveddate,replace(substring(duedate, 4,2) + '/' + substring(duedate, 1, 2)  + '/' + substring(duedate, 7, 4),'/TB/','DPD') AS DueDate2,* from tbl_ProjectReq  where projectstatus='WIP' and (";
                }
                else if (Status == "projects_Hold")
                {
                    ViewState["ProjectStatus"] = "Hold";
                    com_Modal.CommandText = "select CONVERT(VARCHAR(20),receiveddate,103) AS receiveddate,replace(substring(duedate, 4,2) + '/' + substring(duedate, 1, 2)  + '/' + substring(duedate, 7, 4),'/TB/','DPD') AS DueDate2,* from tbl_ProjectReq  where projectstatus='Hold' and (";
                }
                else if (Status == "projects_Completed")
                {
                    ViewState["ProjectStatus"] = "Completed";
                    com_Modal.CommandText = "select CONVERT(VARCHAR(20),receiveddate,103) AS receiveddate,replace(substring(duedate, 4,2) + '/' + substring(duedate, 1, 2)  + '/' + substring(duedate, 7, 4),'/TB/','DPD') AS DueDate2,* from tbl_ProjectReq  where projectstatus='Completed' and (";
                }
                else if (Status == "projects_Closed")
                {
                    ViewState["ProjectStatus"] = "Closed";
                    com_Modal.CommandText = "select CONVERT(VARCHAR(20),receiveddate,103) AS receiveddate,replace(substring(duedate, 4,2) + '/' + substring(duedate, 1, 2)  + '/' + substring(duedate, 7, 4),'/TB/','DPD') AS DueDate2,* from tbl_ProjectReq  where projectstatus='Closed' and (";
                }
                foreach (string Splitted in SplittedProjectAlloted)
                {
                    com_Modal.CommandText += "allotedteamid like '%" + Splitted + "%' or ";
                }
                com_Modal.CommandText = com_Modal.CommandText.Substring(0, com_Modal.CommandText.Length - 3);
                com_Modal.CommandText += ") order by projectid desc";
                SqlDataAdapter da = new SqlDataAdapter(com_Modal.CommandText, con);
                com_Modal.CommandText = com_Modal.CommandText.Substring(0, com_Modal.CommandText.Length - 24);
                DataSet ds = new DataSet();
                this.TitleOfPage.InnerText = ViewState["ProjectStatus"].ToString();
                da.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {

                    grd_Projects.DataSource = ds;
                    grd_Projects.DataBind();
                }
                else
                {
                    ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                    grd_Projects.DataSource = ds;
                    grd_Projects.DataBind();
                    int columncount = grd_Projects.Rows[0].Cells.Count;
                    grd_Projects.Rows[0].Cells.Clear();
                    grd_Projects.Rows[0].Cells.Add(new TableCell());
                    grd_Projects.Rows[0].Cells[0].ColumnSpan = columncount;
                    grd_Projects.Rows[0].Cells[0].Text = "<center>----- No Records Found -----</center>";
                }
            }
            else
            {
                this.TitleOfPage.InnerText = Status;
                ViewState["ProjectStatus"] = "";
                con.Close();
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter("", con);
                if (Status == "projects_yettostart")
                {
                    ViewState["ProjectStatus"] = "Yet To Start";
                    da = new SqlDataAdapter("select CONVERT(VARCHAR(20),receiveddate,103) AS receiveddate,replace(substring(duedate, 4,2) + '/' + substring(duedate, 1, 2)  + '/' + substring(duedate, 7, 4),'/TB/','DPD') AS DueDate2,* from tbl_ProjectReq where projectstatus='Yet To Start' order by projectid desc", con);
                }
                if (Status == "projects_WIP")
                {
                    ViewState["ProjectStatus"] = "WIP";
                    da = new SqlDataAdapter("select CONVERT(VARCHAR(20),receiveddate,103) AS receiveddate,replace(substring(duedate, 4,2) + '/' + substring(duedate, 1, 2)  + '/' + substring(duedate, 7, 4),'/TB/','DPD') AS DueDate2,* from tbl_ProjectReq where projectstatus='WIP' order by projectid desc", con);
                }
                if (Status == "projects_Hold")
                {
                    ViewState["ProjectStatus"] = "Hold";
                    da = new SqlDataAdapter("select CONVERT(VARCHAR(20),receiveddate,103) AS receiveddate,replace(substring(duedate, 4,2) + '/' + substring(duedate, 1, 2)  + '/' + substring(duedate, 7, 4),'/TB/','DPD') AS DueDate2,* from tbl_ProjectReq where projectstatus='Hold' order by projectid desc", con);
                }
                if (Status == "projects_Completed")
                {
                    ViewState["ProjectStatus"] = "Completed";
                    da = new SqlDataAdapter("select CONVERT(VARCHAR(20),receiveddate,103) AS receiveddate,replace(substring(duedate, 4,2) + '/' + substring(duedate, 1, 2)  + '/' + substring(duedate, 7, 4),'/TB/','DPD') AS DueDate2,* from tbl_ProjectReq where projectstatus='Completed' order by projectid desc", con);
                }
                if (Status == "projects_Closed")
                {
                    ViewState["ProjectStatus"] = "Closed";
                    da = new SqlDataAdapter("select CONVERT(VARCHAR(20),receiveddate,103) AS receiveddate,replace(substring(duedate, 4,2) + '/' + substring(duedate, 1, 2)  + '/' + substring(duedate, 7, 4),'/TB/','DPD') AS DueDate2,* from tbl_ProjectReq where projectstatus='Closed' order by projectid desc", con);
                }
                this.TitleOfPage.InnerText = ViewState["ProjectStatus"].ToString();
                DataSet ds = new DataSet();
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    grd_Projects.DataSource = ds;
                    grd_Projects.DataBind();
                }
                else
                {
                    ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                    grd_Projects.DataSource = ds;
                    grd_Projects.DataBind();
                    int columncount = grd_Projects.Rows[0].Cells.Count;
                    grd_Projects.Rows[0].Cells.Clear();
                    grd_Projects.Rows[0].Cells.Add(new TableCell());
                    grd_Projects.Rows[0].Cells[0].ColumnSpan = columncount;
                    grd_Projects.Rows[0].Cells[0].Text = "<center>----- No Records Found -----</center>";
                }
            }
        }
        catch (Exception)
        {
        }
    }

    /// <summary>
    /// Currently Not In Use
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void OnPageIndexChangingProject(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grd_Projects.PageIndex = e.NewPageIndex;
            this.project_gridbind();
        }
        catch (Exception)
        {

        }
    }

    /// <summary>
    /// Back button to redirect to dashboard and change title
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void btn_ProjectBackButton_Click(object sender, EventArgs e)
    {
        btn_BackToDashboard.Visible = true;
        grd_Projects.Visible = true;
        this.block_ProjectsView.Visible = false;
        this.TitleOfPage.InnerText = ViewState["ProjectStatus"].ToString();
    }

    /// <summary>
    /// View Project Details
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void SelectCurrentProjectData(object sender, EventArgs e)
    {
        this.block_Grid.Visible = false;
        this.block_ProjectsView.Visible = true;
        LinkButton btn = (LinkButton)(sender);
        string projectid = btn.CommandArgument;
        con.Close();
        con.Open();
        com_projectView.Connection = con;
        com_projectView.CommandText = "select Convert(varchar(10),receiveddate,103)[receiveddate],replace(substring(duedate, 4,2) + '/' + substring(duedate, 1, 2)  + '/' + substring(duedate, 7, 4),'/TB/','DPD') AS DueDate2,allotedteamname[Alloted Team],* from tbl_ProjectReq where projectid='" + projectid + "'";
        SqlDataReader dr = com_projectView.ExecuteReader();
        while (dr.Read())
        {
            TitleOfPage.InnerText = dr["projectreq"].ToString();
            lbl_ProjectIDTextView.InnerText = dr["projectid"].ToString();
            lbl_ProjectNameTextView.InnerText = dr["projectname"].ToString();
            if (dr["typeproject"].ToString() == "")
            {
                lbl_ProjectTypeTextView.InnerText = "N/A";
            }
            else
            {
                lbl_ProjectTypeTextView.InnerText = dr["typeproject"].ToString();
            }
            lbl_AllotedTeamTextView.InnerText = dr["allotedteamname"].ToString();
            lbl_ReceivedDateTextView.InnerText = dr["receiveddate"].ToString();
            lbl_DueDateTextView.InnerText = dr["DueDate2"].ToString();
            if (dr["manualid"].ToString() != "")
            {
                lbl_ManualIDTextView.InnerText = dr["manualid"].ToString();
            }
            else
            {
                lbl_ManualIDTextView.InnerText = "N/A";
            }
            lbl_ProjectDescTextView.InnerText = dr["projectDesc"].ToString();
            if (dr["remarks"] == "")
            {
                lbl_RemarksTextView.InnerText = "N/A";
            }
            else
            {
                lbl_RemarksTextView.InnerText = dr["remarks"].ToString();
            }
        }
        grd_Projects.Visible = false;
        btn_BackToDashboard.Visible = false;
    }

    /// <summary>
    /// Delete Project
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void DeleteProject(object sender, EventArgs e)
    {
        LinkButton lnkbtn = (LinkButton)sender;
        string projectID = lnkbtn.CommandArgument;
        //popup_Delete.Show();

    }

    /// <summary>
    /// Change Project State
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void btn_DeleteProject_Click(object sender, EventArgs e)
    {
        com_Modal.Connection = con;
        con.Open();
        com_Modal.CommandText = "update tbl_ProjectReq set reason='" + txt_Reason.Text + "',projectstatus='" + ddl_DelStatus.SelectedValue + "',status='0' where projectid='" + ViewState["selectedProjectID"] + "'";
        com_Modal.ExecuteNonQuery();
        com_Modal.CommandText = "update tbl_taskmaster set taskstatus='" + ddl_DelStatus.SelectedValue + "' where projectid='" + ViewState["selectedProjectID"] + "'";
        com_Modal.ExecuteNonQuery();
        project_gridbind();
        txt_Reason.Text = string.Empty;
        ddl_DelStatus.SelectedIndex = 0;
    }

    /// <summary>
    /// Show popup window for change project status
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void lnk_Delete_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)(sender);
        ViewState["selectedProjectID"] = btn.CommandArgument;
        popup_Delete.Show();
    }

    /// <summary>
    /// Close popup window for change project status
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void btn_Close(object sender, EventArgs e)
    {
        txt_Reason.Text = string.Empty;
        popup_Delete.Hide();
    }
    /// <summary>
    /// projects databound function to differentiate project status
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void grd_Projects_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        ViewState["WIP"] = "N";
        ViewState["CompletedDate"] = "";
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // Retrieve the underlying data item. In this example
            // the underlying data item is a DataRowView object.
            DataRowView rowView = (DataRowView)e.Row.DataItem;

            // Retrieve the state value for the current row.
            String status = rowView["projectstatus"].ToString();
            var lk_Edit = (LinkButton)e.Row.FindControl("lnk_Edit");
            var lk_Delete = (LinkButton)e.Row.FindControl("lnk_Delete");
            string Rights = Convert.ToString(Session["Rights"]);
            //format color of the as below
            if (status == "Closed")
            {
                (e.Row.FindControl("lbl_CompletedDate") as Label).Text = "DPD";
                (e.Row.FindControl("lnk_ProjectStatus") as LinkButton).CssClass = "btn btn-danger";
                lk_Delete.Enabled = false;
                lk_Delete.Style.Add("opacity", "0.5");
                //lk_Edit.Enabled = false;
                //lk_Edit.Style.Add("opacity", "0.5");
            }

            if (status == "Yet To Start")
            {
                (e.Row.FindControl("lbl_CompletedDate") as Label).Text = "DPD";
                (e.Row.FindControl("lnk_ProjectStatus") as LinkButton).CssClass = "btn btn-default";

            }

            if (status == "Hold")
            {
                (e.Row.FindControl("lbl_CompletedDate") as Label).Text = "DPD";
                (e.Row.FindControl("lnk_ProjectStatus") as LinkButton).CssClass = "btn btn-warning";
                lk_Delete.Enabled = false;
                lk_Delete.Style.Add("opacity", "0.5");
                //lk_Edit.Enabled = false;
                //lk_Edit.Style.Add("opacity", "0.5");
            }

            if (status == "WIP")
            {
                (e.Row.FindControl("lbl_CompletedDate") as Label).Text = "DPD";
                (e.Row.FindControl("lnk_ProjectStatus") as LinkButton).CssClass = "btn btn-info";
                (e.Row.FindControl("lnk_ProjectStatus") as LinkButton).Style.Add("cursor", "pointer");
            }

            if (status == "Completed")
            {
                com_Check.Connection = con;
                con.Close();
                con.Open();
                com_Check.CommandText = "select completeddate from tbl_ProjectReq where projectid='" + (e.Row.FindControl("lbl_ProjectID") as Label).Text + "'";
                SqlDataReader dr = com_Check.ExecuteReader();
                dr.Read();
                String CompletedDate = dr[0].ToString();
                (e.Row.FindControl("lbl_CompletedDate") as Label).Text = CompletedDate;
                (e.Row.FindControl("lnk_ProjectStatus") as LinkButton).CssClass = "btn btn-success";
                (e.Row.FindControl("lnk_ProjectStatus") as LinkButton).Style.Add("cursor", "pointer");
                lk_Delete.Enabled = false;
                lk_Delete.Style.Add("opacity", "0.5");

                //lk_Edit.Enabled = false;
                //lk_Edit.Style.Add("opacity", "0.5");
            }
        }
    }

    /// <summary>
    /// Project status Click to perform action based on project status
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void lnk_ProjectStatus_Click(object sender, EventArgs e)
    {
        txt_CompletedDate.Attributes.Add("readonly", "readonly");
        txt_CompletedDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
        calext_CompletedDate.StartDate = DateTime.Now.AddDays(-7);
        LinkButton lnk_ProjectStatus = (LinkButton)sender;
        GridViewRow row = (GridViewRow)lnk_ProjectStatus.NamingContainer;
        LinkButton lnk_Status = (LinkButton)row.FindControl("lnk_ProjectStatus");
        ViewState["selectedProjectID"] = lnk_ProjectStatus.CommandArgument;
        if (lnk_Status.Text == "WIP" || lnk_Status.Text == "Completed")
        {
            popup_CompletedState.Show();
        }
        else
        {

        }
    }

    /// <summary>
    /// Complete Project CLick to complete the project
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void btn_CompleteProject_Click(object sender, EventArgs e)
    {
        ProjectID = ViewState["selectedProjectID"].ToString();
        com_Check.Connection = con;
        con.Close();
        con.Open();
        com_Check.CommandText = "select projectid from tbl_taskmaster where taskstatus in('WIP','Yet To Start','Hold') and projectid='" + ProjectID + "'";
        SqlDataReader dr = com_Check.ExecuteReader();
        if (dr.HasRows)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Still Some Tasks not completed in this Project.Hence Operation not Completed');", true);
        }
        else
        {
            dr.Close();
            com_insert.Connection = con;
            con.Close();
            con.Open();
            if (ddl_ProjectState.SelectedValue == "Completed")
            {
                com_insert.CommandText = "update tbl_ProjectReq set projectstatus='" + ddl_ProjectState.SelectedValue + "',completeddate='" + txt_CompletedDate.Text + "' where projectid='" + ProjectID + "'";
                com_insert.ExecuteNonQuery();
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Project Completed Successfully');", true);
            }
            if (ddl_ProjectState.SelectedValue == "WIP")
            {
                com_insert.CommandText = "update tbl_ProjectReq set projectstatus='" + ddl_ProjectState.SelectedValue + "',completeddate='' where projectid='" + ProjectID + "'";
                com_insert.ExecuteNonQuery();
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Project Updated To WIP State Successfully');", true);
            }
            project_gridbind();
        }
    }

    /// <summary>
    /// Project state change
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void ddl_ProjectState_SelectedIndexChanged(object sender, EventArgs e)
    {
        popup_CompletedState.Show();
        if (ddl_ProjectState.SelectedValue == "WIP")
        {
            txt_CompletedDate.Visible = false;
            lbl_CompletedDate.Visible = false;
        }
        else
        {
            txt_CompletedDate.Visible = true;
            lbl_CompletedDate.Visible = true;
        }
    }

    /// <summary>
    /// Project Completed Date Change
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void txt_CompletedDate_TextChanged(object sender, EventArgs e)
    {
        popup_CompletedState.Show();
    }
    /// <summary>
    /// Month For Gridview Records to Show
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void ddl_FilterMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGrid();
    }
    /// <summary>
    /// Year For Gridview Records to Show
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void ddl_FilterYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        int YearSelected = Convert.ToInt32(ddl_FilterYear.SelectedValue);
        int CurrentMonth = Convert.ToInt32(DateTime.Today.Month);
        if (YearSelected != DateTime.Today.Year)
        {
            bindMonthOfRecords();
        }
        else
        {
            for (int Month = 12; Month > CurrentMonth; Month--)
            {
                ddl_FilterMonth.Items.RemoveAt(Month - 1);
            }
        }
        BindGrid();
    }
    /// <summary>
    /// Binds the year Dropdown to Select Gridview Records MonthWise
    /// </summary>
    protected void bindYearOfRecords()
    {
        try
        {
            var years = new Dictionary<int, string>();
            int year = Convert.ToInt32(DateTime.Today.Year);
            ddl_FilterYear.Items.Clear();
            ddl_FilterYear.Items.Add(Convert.ToString(year - 1));
            ddl_FilterYear.Items.Add(Convert.ToString(year));
            ddl_FilterYear.Items.Add(Convert.ToString(year + 1));
            con.Close();
        }
        catch (Exception)
        {
        }
    }
    /// <summary>
    /// Binds the Month Dropdown to Select Gridview Records MonthWise
    /// </summary>
    protected void bindMonthOfRecords()
    {
        try
        {
            ddl_FilterMonth.Items.Clear();
            ddl_FilterMonth.Items.Insert(0, new ListItem("Jan", "1"));
            ddl_FilterMonth.Items.Insert(1, new ListItem("Feb", "2"));
            ddl_FilterMonth.Items.Insert(2, new ListItem("Mar", "3"));
            ddl_FilterMonth.Items.Insert(3, new ListItem("Apr", "4"));
            ddl_FilterMonth.Items.Insert(4, new ListItem("May", "5"));
            ddl_FilterMonth.Items.Insert(5, new ListItem("Jun", "6"));
            ddl_FilterMonth.Items.Insert(6, new ListItem("Jul", "7"));
            ddl_FilterMonth.Items.Insert(7, new ListItem("Aug", "8"));
            ddl_FilterMonth.Items.Insert(8, new ListItem("Sep", "9"));
            ddl_FilterMonth.Items.Insert(9, new ListItem("Oct", "10"));
            ddl_FilterMonth.Items.Insert(10, new ListItem("Nov", "11"));
            ddl_FilterMonth.Items.Insert(11, new ListItem("Dec", "12"));
            con.Close();
        }
        catch (Exception)
        {
        }
    }

}