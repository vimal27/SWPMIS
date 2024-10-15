//Required Namespaces
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
using System.Web.Services;

public partial class Dashboard : System.Web.UI.Page
{
    /// <summary>
    /// Declarations Part For Variables,Strings,SqlConnections,etc
    /// </summary>
    int pc_WIP, pc_Completed, pc_Hold, pc_Closed, pc_YetToStart, pc_Total;
    StringBuilder MonthReport = new StringBuilder();
    string report = "";
    DateTime requestdate, requireddate, wipdate, holdeddate, enddate, startdate;
    double actuals;
    TimeSpan planned, actual, completed;
    double NoOfPlannedDays;
    double NoOfActualDays = 0;
    double NoOfCompletedDays;
    DataTable dt = new DataTable();
    public string dateHeader;
    string[] colors = new string[] { "navy", "maroon", "orange" };
    string var_TaskCompleted, Status;
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString());
    SqlConnection con2 = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString());
    SqlCommand com_Users = new SqlCommand();
    SqlCommand com_Tasks = new SqlCommand();
    SqlCommand com_HoldedTasks = new SqlCommand();
    clsDataControl objData = new clsDataControl();
    /// <summary>
    /// Page Load Function
    /// </summary>   
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            block_Project.Visible = false;
            this.block_View.Visible = false;
            WIP_gridbind();
            com_Users.Connection = con;
            con.Close();
            con.Open();
            string Rights = Convert.ToString(Session["Rights"]);
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

            if (Rights != "Administrator" && Rights != "Team Leader")
            {
                block_Project.Visible = false;
                chart_CodingStandardRating.Attributes["class"] = "col-sm-12 col-xs-12";
            }
            else
            {
                chart_CodingStandardRating.Attributes["class"] = "col-sm-12 col-xs-12";
            }
            com_Users.Connection = con;

            //Task Completed Value to fill BarChart Monthwise
            if (Rights == "Administrator")
            {
                com_Users.Connection = con;
                con.Close();
                con.Open();
                com_Users.CommandText = "select COUNT(distinct projectid)count from tbl_ProjectReq where projectstatus='Completed'";
                title_Completed.InnerText = "Completed";
                SqlDataReader dr = com_Users.ExecuteReader();
                dr.Read();
                lbl_ProjectCompleted.Text = dr[0].ToString();
                dr.Close();
                com_Users.CommandText = "select COUNT(*)as count from tbl_taskmaster where taskstatus='Completed'";
                SqlDataReader dr2 = com_Users.ExecuteReader();
                dr2.Read();
                lbl_Completed.Text = dr2[0].ToString();
                dr2.Close();
            }

            //If Team Leader Pie Chart and Bar chart values
            else if (Rights == "Team Leader")
            {
                com_Users.Connection = con;
                con.Close();
                con.Open();
                com_Users.CommandText = "select COUNT(distinct projectid)count from tbl_ProjectReq where projectstatus='Completed' and allotedteamid in(select TeamID from tbl_teamAllotmentMaster where teamleader like ('%" + Session["Userid"] + "%'))";
                title_Completed.InnerText = "Completed";
                SqlDataReader dr = com_Users.ExecuteReader();
                dr.Read();
                lbl_ProjectCompleted.Text = dr[0].ToString();
                dr.Close();
                com_Users.CommandText = "select COUNT(a.taskstatus)as count from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid  inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid inner join tbl_teams f on f.TeamID=a.teamid  and (" + teams + ") where a.taskstatus in('Completed') and a.userid in(" + teamUsers + ")";
                SqlDataReader dr2 = com_Users.ExecuteReader();
                dr2.Read();
                lbl_Completed.Text = dr2[0].ToString();
                dr2.Close();
            }
            else
            {
                //If Users Pie chart and Barchart Data
                com_Users.Connection = con;
                con.Close();
                con.Open();
                com_Users.CommandText = "select COUNT(*)as count from tbl_taskmaster where taskstatus='Completed' and userid='" + Session["Userid"] + "'";
                SqlDataReader dr = com_Users.ExecuteReader();
                dr.Read();
                lbl_Completed.Text = dr[0].ToString();
                dr.Close();
            }

            //Projects Hold
            if (Rights == "Administrator")
            {
                com_Users.CommandText = "select COUNT(distinct projectid)count from tbl_ProjectReq where projectstatus='Hold'";
                title_Hold.InnerText = "Hold";
                SqlDataReader dr3 = com_Users.ExecuteReader();
                dr3.Read();
                lbl_ProjectHold.Text = dr3[0].ToString();
                dr3.Close();
                com_Users.CommandText = "select count(*)as count from tbl_taskmaster a inner join tbl_ProjectReq b on b.projectid=a.projectid where taskstatus='Hold'";
                SqlDataReader dr4 = com_Users.ExecuteReader();
                dr4.Read();
                lbl_Hold.Text = dr4[0].ToString();
                dr4.Close();

            }
            else if (Rights == "Team Leader")
            {
                com_Users.CommandText = "select COUNT(distinct projectid)count from tbl_ProjectReq where projectstatus='Hold' and allotedteamid in(select TeamID from tbl_teamAllotmentMaster where teamleader like ('%" + Session["Userid"] + "%'))";
                title_Hold.InnerText = "Hold";
                SqlDataReader dr3 = com_Users.ExecuteReader();
                dr3.Read();
                lbl_ProjectHold.Text = dr3[0].ToString();
                dr3.Close();
                com_Users.CommandText = "select count(a.id)as count from tbl_taskmaster a inner join tbl_ProjectReq e on e.projectid=a.projectid where a.taskstatus='Hold' and (" + teams + ") and a.userid in(" + teamUsers + ")";
                SqlDataReader dr4 = com_Users.ExecuteReader();
                dr4.Read();
                lbl_Hold.Text = dr4[0].ToString();
                dr4.Close();
            }
            else
            {
                com_Users.CommandText = "select count(*)as count from tbl_taskmaster a inner join tbl_ProjectReq b on b.projectid=a.projectid where taskstatus='Hold' and userid='" + Session["userid"] + "'";
                SqlDataReader dr3 = com_Users.ExecuteReader();
                dr3.Read();
                lbl_Hold.Text = dr3[0].ToString();
                dr3.Close();
            }


            //Projects Yet To Start
            if (Rights == "Administrator")
            {
                com_Users.CommandText = "select COUNT(distinct projectid)count from tbl_ProjectReq where projectstatus='Yet To Start'";
                title_Hold.InnerText = "Hold";
                SqlDataReader dr5 = com_Users.ExecuteReader();
                dr5.Read();
                lbl_ProjectYetToStart.Text = dr5[0].ToString();
                dr5.Close();
                com_Users.CommandText = "select COUNT(*)as count from tbl_taskmaster a inner join tbl_ProjectReq b on b.projectid=a.projectid  where taskstatus='Yet To Start'";


                SqlDataReader dr6 = com_Users.ExecuteReader();
                dr6.Read();
                lbl_YetToStart.Text = dr6[0].ToString();
                dr6.Close();
            }
            else if (Rights == "Team Leader")
            {
                com_Users.CommandText = "select COUNT(distinct projectid)count from tbl_ProjectReq where projectstatus='Yet To Start' and allotedteamid in(select TeamID from tbl_teamAllotmentMaster where teamleader like ('%" + Session["Userid"] + "%'))";
                title_Hold.InnerText = "Hold";
                SqlDataReader dr5 = com_Users.ExecuteReader();
                dr5.Read();
                lbl_ProjectYetToStart.Text = dr5[0].ToString();
                dr5.Close();
                com_Users.CommandText = "select COUNT(a.taskstatus)as count from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid  inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid inner join tbl_teams f on f.TeamID=a.teamid  and (" + teams + ") where a.taskstatus in('Yet To Start') and a.userid in(" + teamUsers + ")";
                SqlDataReader dr6 = com_Users.ExecuteReader();
                dr6.Read();
                lbl_YetToStart.Text = dr6[0].ToString();
                dr6.Close();
            }
            else
            {
                com_Users.CommandText = "select COUNT(*)as count from tbl_taskmaster a inner join tbl_ProjectReq b on b.projectid=a.projectid and b.projectid!='NA' where taskstatus='Yet To Start' and userid='" + Session["Userid"] + "'";
                SqlDataReader dr5 = com_Users.ExecuteReader();
                dr5.Read();
                lbl_YetToStart.Text = dr5[0].ToString();
                dr5.Close();
            }


            //Projects Closed
            if (Rights == "Administrator")
            {
                com_Users.CommandText = "select COUNT(distinct projectid)count from tbl_ProjectReq where projectstatus='Closed'";
                title_Cancelled.InnerText = "Closed";
                SqlDataReader dr4 = com_Users.ExecuteReader();
                dr4.Read();
                lbl_ProjectClosed.Text = dr4[0].ToString();
                dr4.Close();
                com_Users.CommandText = "select COUNT(*)as count from tbl_taskmaster a inner join tbl_ProjectReq b on b.projectid=a.projectid where taskstatus='Closed'";
                SqlDataReader dr5 = com_Users.ExecuteReader();
                dr5.Read();
                lbl_Cancelled.Text = dr5[0].ToString();
                dr5.Close();
            }
            else if (Rights == "Team Leader")
            {
                com_Users.CommandText = "select COUNT(*)as count from tbl_taskmaster a inner join tbl_ProjectReq b on b.projectid=a.projectid where a.taskstatus='Closed' and b.allotedteamid in(select TeamID from tbl_teamAllotmentMaster where teamleader like ('%" + Session["Userid"] + "%'))";
                title_Cancelled.InnerText = "Closed";
                SqlDataReader dr4 = com_Users.ExecuteReader();
                dr4.Read();
                lbl_Cancelled.Text = dr4[0].ToString();
                dr4.Close();
                com_Users.CommandText = "select COUNT(*)as count from tbl_taskmaster a inner join tbl_ProjectReq e on e.projectid=a.projectid where a.taskstatus='Closed' and (" + teams + ") and a.userid in(" + teamUsers + ")";
                SqlDataReader dr6 = com_Users.ExecuteReader();
                dr6.Read();
                lbl_Cancelled.Text = dr6[0].ToString();
                dr6.Close();
            }
            else
            {
                com_Users.CommandText = "select COUNT(*)as count from tbl_taskmaster a inner join tbl_ProjectReq b on b.projectid=a.projectid and b.projectstatus='Closed' where taskstatus='Closed' and userid='" + Session["Userid"] + "'";
                SqlDataReader dr4 = com_Users.ExecuteReader();
                dr4.Read();
                lbl_Cancelled.Text = dr4[0].ToString();
                dr4.Close();
            }

            //Task WIP
            if (Rights == "Administrator")
            {
                com_Users.CommandText = "select COUNT(distinct projectid)count from tbl_ProjectReq where projectstatus='WIP'";
                title_WIP.InnerText = "WIP";
                SqlDataReader dr2 = com_Users.ExecuteReader();
                dr2.Read();
                lbl_ProjectWIP.Text = dr2[0].ToString();
                dr2.Close();
                com_Users.CommandText = "select count(*) from tbl_taskmaster where taskstatus='WIP' and projectid!='NA'";
                SqlDataReader dr3 = com_Users.ExecuteReader();
                dr3.Read();
                lbl_WIP.Text = dr3[0].ToString();
            }
            else if (Rights == "Team Leader")
            {
                //com_Users.CommandText = "select COUNT(distinct projectid)count from tbl_ProjectReq where projectstatus='WIP' and allotedteamid like '%'+(select TeamID from tbl_teamAllotmentMaster where teamleader like ('%" + Session["Userid"] + "%'))+'%' and a.userid in(" + teamUsers + ")";
                //title_WIP.InnerText = "WIP";
                //SqlDataReader dr2 = com_Users.ExecuteReader();
                //dr2.Read();
                //lbl_ProjectWIP.Text = dr2[0].ToString();
                //dr2.Close();
                com_Users.CommandText = "select count(a.id) from tbl_taskmaster a inner join tbl_Projectreq e  on a.projectid=e.projectid where  a.taskstatus='WIP' and (" + teams + ") and a.userid in (" + teamUsers + ")";
                SqlDataReader dr3 = com_Users.ExecuteReader();
                dr3.Read();
                lbl_WIP.Text = dr3[0].ToString();
            }
            else
            {
                com_Users.CommandText = "select count(*) from tbl_taskmaster where taskstatus='WIP' and userid='" + Session["Userid"] + "' and projectid in(select b.projectid from tbl_ProjectReq b where b.projectid=projectid)";
                SqlDataReader dr2 = com_Users.ExecuteReader();
                dr2.Read();
                lbl_WIP.Text = dr2[0].ToString();
            }

            if (Session["Userid"] == null) Response.Redirect("Login");
            else if (Session["Userid"].ToString() == "") Response.Redirect("Login");
            if (!IsPostBack)
            {
                bindMonthOfRecords();
                bindYearOfRecords();
                bindEmployeesForChart();
                ddl_FromYear.SelectedValue = Convert.ToString(DateTime.Now.Year);
                ddl_FromMonth.SelectedValue = (DateTime.Now.Month).ToString("D2");
                getUsers();
                ScriptManager.RegisterStartupScript(this, GetType(), "mismatch", "loadLineChart()", true);
            }
            // generateRatingChart();
        }
        catch (Exception)
        {

        }
    }

    /// <summary>
    /// For Gantt chart Data Load
    /// </summary>
    protected void getUsers()
    {
        System.Text.StringBuilder sb_Notifications = new System.Text.StringBuilder();

        string Rights = Convert.ToString(Session["Rights"]);
        com_Users.Connection = con;
        con.Close();
        con.Open();

        //Login Check
        if (Rights == "Administrator")
        {
            com_Users.CommandText = "select a.userid,b.EmpName,b.Scope,c.Scope as ScopeName,b.EmpName,b.Stage,a.taskname,d.Stage as StageName,replace(CONVERT(VARCHAR(10), max(DATEADD(month, -1, a.requestdate)), 102), '.', ',') as requestdate,max(b.statusoftask)[Status],replace(CONVERT(VARCHAR(10), max(DATEADD(month, -1, a.requireddate)), 102), '.', ',') as requireddate,a.projectid,replace((CONVERT(varchar(10),DATEADD(month,-1,CONVERT(datetime,Convert(varchar(10),min(b.StartTime)),104)),102)),'.',',') as StartTime,replace((CONVERT(varchar(10),DATEADD(month,-1,CONVERT(datetime,Convert(varchar(10),max(b.EndTime)),104)),102)),'.',',') as EndTime,a.teamid,a.id from tbl_taskmaster a inner join PrmsProductionHour_Backup b on b.Scope=a.scopeid and b.Stage=a.stageid and b.EmpNo=a.userid and a.id=b.task inner join tbl_scope c on c.ID=b.Scope inner join tbl_MstStageMaster d on d.slno=b.Stage where a.projectid!='NA' and a.requireddate >= dateadd(day,datediff(day,0,GetDate())- 60,0) group by b.Scope,b.EmpName,b.Stage,d.Stage,a.userid,a.requestdate,c.Scope,a.requireddate,a.projectid,a.taskname,a.id,a.teamid order by a.id desc";
        }
        else if (Rights == "Team Leader")
        {
            com_Users.CommandText = "select a.userid,b.EmpName,b.Scope,c.Scope as ScopeName,b.EmpName,b.Stage,a.taskname,d.Stage as StageName,replace(CONVERT(VARCHAR(10), max(DATEADD(month, -1, a.requestdate)), 102), '.', ',') as requestdate,max(b.statusoftask)[Status],replace(CONVERT(VARCHAR(10), max(DATEADD(month, -1, a.requireddate)), 102), '.', ',') as requireddate,a.projectid,replace((CONVERT(varchar(10),DATEADD(month,-1,CONVERT(datetime,Convert(varchar(10),min(b.StartTime)),104)),102)),'.',',') as StartTime,replace((CONVERT(varchar(10),DATEADD(month,-1,CONVERT(datetime,Convert(varchar(10),max(b.EndTime)),104)),102)),'.',',') as EndTime,a.teamid,a.id from tbl_taskmaster a inner join PrmsProductionHour_Backup b on b.Scope=a.scopeid and b.Stage=a.stageid and b.EmpNo=a.userid and a.id=b.task and b.ProjectID!='NA' inner join tbl_scope c on c.ID=b.Scope inner join tbl_MstStageMaster d on d.slno=b.Stage where a.teamid in(select TeamID from tbl_teamAllotmentMaster where teamleader like ('%" + Session["Userid"] + "%')) and a.projectid!='NA' and a.requireddate >= dateadd(day,datediff(day,0,GetDate())- 60,0)   group by b.Scope,b.EmpName,b.Stage,d.Stage,a.userid,a.requestdate,c.Scope,a.requireddate,a.teamid,a.projectid,a.taskname,a.id order by a.id desc";
        }
        else
        {
            com_Users.CommandText = "select a.userid,b.EmpName,b.Scope,c.Scope as ScopeName,b.EmpName,b.Stage,a.taskname,d.Stage as StageName,replace(CONVERT(VARCHAR(10), max(DATEADD(month, -1, a.requestdate)), 102), '.', ',') as requestdate,max(b.statusoftask)[Status],replace(CONVERT(VARCHAR(10), max(DATEADD(month, -1, a.requireddate)), 102), '.', ',') as requireddate,a.projectid,replace((CONVERT(varchar(10),DATEADD(month,-1,CONVERT(datetime,Convert(varchar(10),min(b.StartTime)),104)),102)),'.',',') as StartTime,replace((CONVERT(varchar(10),DATEADD(month,-1,CONVERT(datetime,Convert(varchar(10),max(b.EndTime)),104)),102)),'.',',') as EndTime,a.teamid,a.id from tbl_taskmaster a inner join PrmsProductionHour_Backup b on b.Scope=a.scopeid and b.Stage=a.stageid and a.id=b.task and b.EmpNo=a.userid and b.ProjectID!='NA' inner join tbl_scope c on c.ID=b.Scope inner join tbl_MstStageMaster d on d.slno=b.Stage where a.userid ='" + Session["Userid"] + "' and a.projectid!='NA' and a.requireddate >= dateadd(day,datediff(day,0,GetDate())- 60,0)  group by b.Scope,b.EmpName,b.Stage,d.Stage,a.userid,a.requestdate,c.Scope,a.requireddate,a.projectid,a.taskname,a.id,a.teamid order by a.id desc";
        }
        SqlDataAdapter da = new SqlDataAdapter(com_Users.CommandText, con);
        DataTable dt = new DataTable();
        da.Fill(dt);

        //Html Table For Status Of Task Panel On Dashboard
        for (int rowid = 0; rowid < dt.Rows.Count; rowid++)
        {
            //Session["EmpNo"] = dt.Rows[rowid]["userid"].ToString();
            //Session["ProjectID"] = dt.Rows[rowid]["ProjectID"].ToString();
            //Session["Stage"] = dt.Rows[rowid]["Stage"].ToString();
            //Session["Status"] = dt.Rows[rowid]["Status"].ToString();
            //Session["Tasks"] = dt.Rows[rowid]["id"].ToString();
            //Session["CountTasks"] = dt.Rows.Count;
            //Session["ClickedTask"] = dt.Rows[rowid]["taskname"].ToString();
            //var_TaskCompleted = dt.Rows[rowid]["Status"].ToString();
            //sb_Notifications.Append("<a href='dashboard.aspx?mode=" + dt.Rows[rowid]["id"].ToString() + "'>");
            //sb_Notifications.Append("<li id='" + dt.Rows[rowid]["id"].ToString() + "' runat='server'>");
            //sb_Notifications.Append("<img src='assets/img/profile/original/profile-img.png' alt='profile-photo' class='profile-img pull-left'><div class='message-block'>");
            //sb_Notifications.Append("<div><span class='username' runat='server' id='username'></span>" + dt.Rows[rowid]["EmpName"].ToString() + "(" + dt.Rows[rowid]["userid"].ToString() + ")</div>");
            //sb_Notifications.Append("<div class='message' id='con_Stage' runat='server'><b>Task : </b>" + dt.Rows[rowid]["taskname"].ToString() + "</br><b>Task Completed : </b>" + dt.Rows[rowid]["Status"].ToString() + "%</div><div style='width:100%;'><div class='progress'>");
            //sb_Notifications.Append("<div class='progress-bar progress-bar-success' id='fill_success' runat='server' style='width:" + var_TaskCompleted + "% ;'><span class='sr-only'></span></div>");
            //sb_Notifications.Append("<div class='progress-bar progress-bar-warning progress-bar-striped' runat='server' id='fill_warning' style='width: 0%'><span class='sr-only'></span></div>");
            //sb_Notifications.Append("<div class='progress-bar progress-bar-warning' runat='server' id='fill_warning' style='width: 0%'><span class='sr-only'></span></div></div></div></div></li></a>");
            //this.ul_Message.InnerHtml = sb_Notifications.ToString();
            //string mode = Request.QueryString.Get("mode");
            //if (mode == "" || mode == null)
            //{
            //}
            //else
            //{
            //    lbl_TaskID.Text = mode;
            //    getSelectedUsers();
            //}

        }
    }

    /// <summary>
    /// Gantt Chart Data If Click Full gantt Chart on Panel On Dashboard
    /// </summary>
    protected void getAllUsers()
    {
        System.Text.StringBuilder sb_Notifications = new System.Text.StringBuilder();
        string Rights = Convert.ToString(Session["Rights"]);
        com_Users.Connection = con;
        con.Close();
        con.Open();
        if (Rights == "Administrator")
        {
            com_Users.CommandText = "select top 50 a.userid,b.EmpName,b.Scope,c.Scope as ScopeName,b.EmpName,b.Stage,a.taskname,d.Stage as StageName,replace(CONVERT(VARCHAR(10), max(DATEADD(month, -1, a.requestdate)), 102), '.', ',') as requestdate,max(b.statusoftask)[Status],replace(CONVERT(VARCHAR(10), max(DATEADD(month, -1, a.requireddate)), 102), '.', ',') as requireddate,a.projectid,replace((CONVERT(varchar(10),DATEADD(month,-1,CONVERT(datetime,Convert(varchar(10),min(b.StartTime)),104)),102)),'.',',') as StartTime,replace((CONVERT(varchar(10),DATEADD(month,-1,CONVERT(datetime,Convert(varchar(10),max(b.EndTime)),104)),102)),'.',',') as EndTime,a.teamid,a.id from tbl_taskmaster a inner join PrmsProductionHour_Backup b on b.Scope=a.scopeid and b.Stage=a.stageid and b.EmpNo=a.userid and a.id=b.task inner join tbl_scope c on c.ID=b.Scope inner join tbl_MstStageMaster d on d.slno=b.Stage where b.projectid!='NA' group by b.Scope,b.EmpName,b.Stage,d.Stage,a.userid,a.requestdate,c.Scope,a.requireddate,a.projectid,a.taskname,a.id,a.teamid order by a.id desc";
        }
        else if (Rights == "Team Leader")
        {
            com_Users.CommandText = "select top 50 a.userid,b.EmpName,b.Scope,c.Scope as ScopeName,b.EmpName,b.Stage,a.taskname,d.Stage as StageName,replace(CONVERT(VARCHAR(10), max(DATEADD(month, -1, a.requestdate)), 102), '.', ',') as requestdate,max(b.statusoftask)[Status],replace(CONVERT(VARCHAR(10), max(DATEADD(month, -1, a.requireddate)), 102), '.', ',') as requireddate,a.projectid,replace((CONVERT(varchar(10),DATEADD(month,-1,CONVERT(datetime,Convert(varchar(10),min(b.StartTime)),104)),102)),'.',',') as StartTime,replace((CONVERT(varchar(10),DATEADD(month,-1,CONVERT(datetime,Convert(varchar(10),max(b.EndTime)),104)),102)),'.',',') as EndTime,a.teamid,a.id from tbl_taskmaster a inner join PrmsProductionHour_Backup b on b.Scope=a.scopeid and b.Stage=a.stageid and b.EmpNo=a.userid and a.id=b.task and b.ProjectID!='NA' inner join tbl_scope c on c.ID=b.Scope inner join tbl_MstStageMaster d on d.slno=b.Stage where a.teamid in(select TeamID from tbl_teamAllotmentMaster where teamleader like ('%" + Session["Userid"] + "%')) and a.projectid!='NA' group by b.Scope,b.EmpName,b.Stage,d.Stage,a.userid,a.requestdate,c.Scope,a.requireddate,a.teamid,a.projectid,a.taskname,a.id order by a.id desc";
        }
        else
        {
            com_Users.CommandText = "select top 50 a.userid,b.EmpName,b.Scope,c.Scope as ScopeName,b.EmpName,b.Stage,a.taskname,d.Stage as StageName,replace(CONVERT(VARCHAR(10), max(DATEADD(month, -1, a.requestdate)), 102), '.', ',') as requestdate,max(b.statusoftask)[Status],replace(CONVERT(VARCHAR(10), max(DATEADD(month, -1, a.requireddate)), 102), '.', ',') as requireddate,a.projectid,replace((CONVERT(varchar(10),DATEADD(month,-1,CONVERT(datetime,Convert(varchar(10),min(b.StartTime)),104)),102)),'.',',') as StartTime,replace((CONVERT(varchar(10),DATEADD(month,-1,CONVERT(datetime,Convert(varchar(10),max(b.EndTime)),104)),102)),'.',',') as EndTime,a.teamid,a.id from tbl_taskmaster a inner join PrmsProductionHour_Backup b on b.Scope=a.scopeid and b.Stage=a.stageid and a.id=b.task and b.EmpNo=a.userid and b.ProjectID!='NA' inner join tbl_scope c on c.ID=b.Scope inner join tbl_MstStageMaster d on d.slno=b.Stage where a.userid ='" + Session["Userid"] + "' and a.projectid!='NA' group by b.Scope,b.EmpName,b.Stage,d.Stage,a.userid,a.requestdate,c.Scope,a.requireddate,a.projectid,a.taskname,a.id,a.teamid order by a.id desc";
        }
        SqlDataAdapter da2 = new SqlDataAdapter(com_Users.CommandText, con);
        DataTable dt2 = new DataTable();
        da2.Fill(dt2);

        //Convert TO JSON Data
        var Test = Jsontableconvert(dt2);

        //JSon Data File export as Javascript File For Gantt Chart
        var FileName = Server.MapPath("~/assets/gantt_Data/" + Session["ProjectID"] + Session["Tasks"] + ".js");
        File.WriteAllText(@FileName, Test);
        Response.Redirect("ganttchart");
    }
    ///// <summary>
    ///// Gantt chart If Any of task click at panel on Dashboard
    ///// </summary>
    //protected void getSelectedUsers()
    //{
    //    System.Text.StringBuilder sb_Notifications = new System.Text.StringBuilder();

    //    string Rights = Convert.ToString(Session["Rights"]);
    //    com_Users.Connection = con;
    //    con.Close();
    //    con.Open();
    //    if (Rights == "Administrator")
    //    {
    //        com_Users.CommandText = "select a.userid,b.EmpName,b.Scope,c.Scope as ScopeName,b.EmpName,b.Stage,a.taskname,d.Stage as StageName,replace(CONVERT(VARCHAR(10), max(DATEADD(month, -1, a.requestdate)), 102), '.', ',') as requestdate,max(b.statusoftask)[Status],replace(CONVERT(VARCHAR(10), max(DATEADD(month, -1, a.requireddate)), 102), '.', ',') as requireddate,a.projectid,replace((CONVERT(varchar(10),DATEADD(month,-1,CONVERT(datetime,Convert(varchar(10),min(b.StartTime)),104)),102)),'.',',') as StartTime,replace((CONVERT(varchar(10),DATEADD(month,-1,CONVERT(datetime,Convert(varchar(10),max(b.EndTime)),104)),102)),'.',',') as EndTime,a.teamid,a.id from tbl_taskmaster a inner join PrmsProductionHour_Backup b on b.Scope=a.scopeid and b.Stage=a.stageid and b.EmpNo=a.userid and a.id=b.task inner join tbl_scope c on c.ID=b.Scope inner join tbl_MstStageMaster d on d.slno=b.Stage where b.projectid!='NA' and a.id='" + lbl_TaskID.Text + "' group by b.Scope,b.EmpName,b.Stage,d.Stage,a.userid,a.requestdate,c.Scope,a.requireddate,a.projectid,a.taskname,a.id,a.teamid order by a.id desc";
    //    }
    //    else if (Rights == "Team Leader")
    //    {
    //        com_Users.CommandText = "select a.userid,b.EmpName,b.Scope,c.Scope as ScopeName,b.EmpName,b.Stage,a.taskname,d.Stage as StageName,replace(CONVERT(VARCHAR(10), max(DATEADD(month, -1, a.requestdate)), 102), '.', ',') as requestdate,max(b.statusoftask)[Status],replace(CONVERT(VARCHAR(10), max(DATEADD(month, -1, a.requireddate)), 102), '.', ',') as requireddate,a.projectid,replace((CONVERT(varchar(10),DATEADD(month,-1,CONVERT(datetime,Convert(varchar(10),min(b.StartTime)),104)),102)),'.',',') as StartTime,replace((CONVERT(varchar(10),DATEADD(month,-1,CONVERT(datetime,Convert(varchar(10),max(b.EndTime)),104)),102)),'.',',') as EndTime,a.teamid,a.id from tbl_taskmaster a inner join PrmsProductionHour_Backup b on b.Scope=a.scopeid and b.Stage=a.stageid and b.EmpNo=a.userid and a.id=b.task and b.ProjectID!='NA' inner join tbl_scope c on c.ID=b.Scope inner join tbl_MstStageMaster d on d.slno=b.Stage where a.teamid in(select TeamID from tbl_teamAllotmentMaster where teamleader like ('%" + Session["Userid"] + "%')) and a.projectid!='NA' and a.id='" + lbl_TaskID.Text + "' group by b.Scope,b.EmpName,b.Stage,d.Stage,a.userid,a.requestdate,c.Scope,a.requireddate,a.teamid,a.projectid,a.taskname,a.id order by a.id desc";
    //    }
    //    else
    //    {
    //        com_Users.CommandText = "select a.userid,b.EmpName,b.Scope,c.Scope as ScopeName,b.EmpName,b.Stage,a.taskname,d.Stage as StageName,replace(CONVERT(VARCHAR(10), max(DATEADD(month, -1, a.requestdate)), 102), '.', ',') as requestdate,max(b.statusoftask)[Status],replace(CONVERT(VARCHAR(10), max(DATEADD(month, -1, a.requireddate)), 102), '.', ',') as requireddate,a.projectid,replace((CONVERT(varchar(10),DATEADD(month,-1,CONVERT(datetime,Convert(varchar(10),min(b.StartTime)),104)),102)),'.',',') as StartTime,replace((CONVERT(varchar(10),DATEADD(month,-1,CONVERT(datetime,Convert(varchar(10),max(b.EndTime)),104)),102)),'.',',') as EndTime,a.teamid,a.id from tbl_taskmaster a inner join PrmsProductionHour_Backup b on b.Scope=a.scopeid and b.Stage=a.stageid and a.id=b.task and b.EmpNo=a.userid and b.ProjectID!='NA' inner join tbl_scope c on c.ID=b.Scope inner join tbl_MstStageMaster d on d.slno=b.Stage where a.userid ='" + Session["Userid"] + "' and a.projectid!='NA' and a.id='" + lbl_TaskID.Text + "' group by b.Scope,b.EmpName,b.Stage,d.Stage,a.userid,a.requestdate,c.Scope,a.requireddate,a.projectid,a.taskname,a.id,a.teamid order by a.id desc";
    //    }
    //    SqlDataAdapter da2 = new SqlDataAdapter(com_Users.CommandText, con);
    //    DataTable dt2 = new DataTable();
    //    da2.Fill(dt2);

    //    //Json Convertion
    //    var Test = Jsontableconvert(dt2);

    //    //File Export to Js
    //    var FileName = Server.MapPath("~/assets/gantt_Data/" + Session["ProjectID"] + Session["Tasks"] + ".js");
    //    File.WriteAllText(@FileName, Test);
    //    Response.Redirect("ganttchart");

    //}

    /// <summary>
    /// Function To Convert datatable to Js(Not Used Now)
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public string ConvertDataTabletoString(DataTable dt)
    {

        System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        Dictionary<string, object> row;
        foreach (DataRow dr in dt.Rows)
        {
            row = new Dictionary<string, object>();
            foreach (DataColumn col in dt.Columns)
            {
                row.Add(col.ColumnName, dr[col]);
            }
            rows.Add(row);
        }
        return serializer.Serialize(rows);
    }
    /// <summary>
    /// Function To Convert datatable to Js
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public string Jsontableconvert(DataTable dt)
    {
        StringBuilder JsonString = new StringBuilder();
        JsonString.AppendLine("var gantt = [");
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            JsonString.AppendLine("{");
            JsonString.AppendLine("id: \"" + dt.Rows[i].ItemArray[1] + "\", name: \"<b>Task : </b>" + dt.Rows[i].ItemArray[6] + "</br>[<b>" + dt.Rows[i].ItemArray[1] + "(" + dt.Rows[i].ItemArray[0] + ")</b>]" + "\", series: [");
            JsonString.AppendLine("{ name: \"Planned\", start: new Date(" + dt.Rows[i].ItemArray[8].ToString() + "), end: new Date(" + dt.Rows[i].ItemArray[10].ToString() + "), color: \"#ff8\" },");
            com_HoldedTasks.Connection = con2;
            con2.Close();
            con2.Open();
            com_HoldedTasks.CommandText = "select replace(CONVERT(VARCHAR(10), DATEADD(month, -1, holdeddate), 102), '.', ',') as holdeddate,replace(CONVERT(VARCHAR(10), DATEADD(month, -1, wipdate), 102), '.', ',') as wipdate from tbl_TaskHolds where taskid='" + dt.Rows[i].ItemArray[15] + "' order by holdeddate asc";
            SqlDataReader dr = com_HoldedTasks.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    requestdate = Convert.ToDateTime(dt.Rows[i].ItemArray[8].ToString());
                    requireddate = Convert.ToDateTime(dt.Rows[i].ItemArray[10].ToString());
                    enddate = Convert.ToDateTime(dt.Rows[i].ItemArray[13].ToString());
                    startdate = Convert.ToDateTime(dt.Rows[i].ItemArray[12].ToString());
                    if (dr[1].ToString() != "" && dr[1].ToString() != null)
                    {
                        wipdate = Convert.ToDateTime(dr[1].ToString());
                        holdeddate = Convert.ToDateTime(dr[0].ToString());
                        actual = wipdate - holdeddate;
                        if (NoOfActualDays != 0)
                        {
                            NoOfActualDays = Convert.ToInt32(actual.TotalDays + 1) + NoOfActualDays;
                        }
                        else
                        {
                            NoOfActualDays = Convert.ToInt32(actual.TotalDays + 1);
                        }
                        JsonString.AppendLine("{ name: \"Holded\", start: new Date(" + dr[0].ToString() + "), end: new Date(" + dr[1].ToString() + "), color: \"#eee\" },");
                    }
                }
                planned = requireddate - requestdate;
                NoOfPlannedDays = Convert.ToInt32(planned.TotalDays + 1);
                actuals = NoOfPlannedDays - NoOfActualDays;
                completed = enddate - startdate;
                NoOfCompletedDays = Convert.ToInt32(completed.TotalDays + 1);
                JsonString.Length--;
            }
            else
            {
                JsonString.Length--;
            }
            con2.Close();
            if (NoOfActualDays != 0)
            {
                if (dt.Rows[i].ItemArray[9].ToString() == "100" && NoOfPlannedDays >= NoOfCompletedDays - NoOfActualDays)
                {
                    JsonString.AppendLine("{ name: \"Actual\", start: new Date(" + dt.Rows[i].ItemArray[12].ToString() + "), end: new Date(" + dt.Rows[i].ItemArray[13].ToString() + "), color: \"rgb(128, 245, 132)\" },");
                }
                else if (dt.Rows[i].ItemArray[9].ToString() != "100" && NoOfPlannedDays >= NoOfCompletedDays - NoOfActualDays)
                {
                    JsonString.AppendLine("{ name: \"Actual\", start: new Date(" + dt.Rows[i].ItemArray[12].ToString() + "), end: new Date(" + dt.Rows[i].ItemArray[13].ToString() + "), color: \"rgb(136, 232, 245)\" }");
                }
                else if (dt.Rows[i].ItemArray[9].ToString() != "100" && NoOfPlannedDays <= NoOfCompletedDays - NoOfActualDays)
                {
                    JsonString.AppendLine("{ name: \"Actual\", start: new Date(" + dt.Rows[i].ItemArray[12].ToString() + "), end: new Date(" + dt.Rows[i].ItemArray[13].ToString() + "), color: \"rgb(230, 33, 33)\" }");
                }
                else if (dt.Rows[i].ItemArray[9].ToString() == "100" && NoOfPlannedDays <= NoOfCompletedDays - NoOfActualDays)
                {
                    JsonString.AppendLine("{ name: \"Actual\", start: new Date(" + dt.Rows[i].ItemArray[12].ToString() + "), end: new Date(" + dt.Rows[i].ItemArray[13].ToString() + "), color: \"rgb(255, 156, 124)\" }");
                }
            }
            else
            {
                if (dt.Rows[i].ItemArray[9].ToString() == "100" && Convert.ToDateTime(dt.Rows[i].ItemArray[10].ToString().Trim()).Date >= Convert.ToDateTime(dt.Rows[i].ItemArray[13].ToString().Trim()).Date)
                {
                    JsonString.AppendLine("{ name: \"Actual\", start: new Date(" + dt.Rows[i].ItemArray[12].ToString() + "), end: new Date(" + dt.Rows[i].ItemArray[13].ToString() + "), color: \"rgb(128, 245, 132)\" },");
                }
                else if (Convert.ToDateTime(dt.Rows[i].ItemArray[10].ToString().Trim()).Date < Convert.ToDateTime(dt.Rows[i].ItemArray[13].ToString().Trim()).Date && dt.Rows[i].ItemArray[9].ToString() != "100")
                {
                    JsonString.AppendLine("{ name: \"Actual\", start: new Date(" + dt.Rows[i].ItemArray[12].ToString() + "), end: new Date(" + dt.Rows[i].ItemArray[13].ToString() + "), color: \"rgb(230, 33, 33)\" }");
                }
                else if (Convert.ToDateTime(dt.Rows[i].ItemArray[10].ToString().Trim()).Date < Convert.ToDateTime(dt.Rows[i].ItemArray[13].ToString().Trim()).Date && dt.Rows[i].ItemArray[9].ToString() == "100")
                {
                    JsonString.AppendLine("{ name: \"Actual\", start: new Date(" + dt.Rows[i].ItemArray[12].ToString() + "), end: new Date(" + dt.Rows[i].ItemArray[13].ToString() + "), color: \"rgb(255, 156, 124)\" }");
                }
                else
                {
                    JsonString.AppendLine("{ name: \"Actual\", start: new Date(" + dt.Rows[i].ItemArray[12].ToString() + "), end: new Date(" + dt.Rows[i].ItemArray[13].ToString() + "), color: \"rgb(136, 232, 245)\" }");
                }
            }
            JsonString.AppendLine("]");
            JsonString.AppendLine("},");
        }
        JsonString.Length--;
        JsonString.AppendLine("];");
        return JsonString.ToString();

    }

    /// <summary>
    /// Task Details Of Current Week On Placed at Bottom of dashboard
    /// </summary>
    protected void WIP_gridbind()
    {
        try
        {
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
            con.Close();
            SqlDataAdapter da = new SqlDataAdapter();
            string Rights = Convert.ToString(Session["Rights"]);
            if (Rights == "Team Leader")
                da = new SqlDataAdapter("select a.id,a.requestid,a.taskstatus,a.taskname,a.taskdescription,a.projectid,a.stageid,a.scopeid,a.teamid,a.userid,CONVERT(VARCHAR(20),a.requestdate,103) AS requestdate,CONVERT(VARCHAR(20),a.requireddate,103) AS requireddate,b.Stage as Stage,c.Scope as Scope,f.Teamname as AllotedTeam,d.username as Username,e.projectname as Project from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid  inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid and e.projectstatus in('Yet To Start','WIP') inner join tbl_teams f on f.TeamID=a.teamid  and ("+teams+") where a.taskstatus in('Yet To Start','WIP')  and a.userid in(" + teamUsers + ") and a.requireddate>=cast(GETDATE() as DATE) order by id desc", con);
            else if (Rights == "Administrator")
                da = new SqlDataAdapter("select a.id,a.requestid,a.taskstatus,a.taskname,a.taskdescription,a.projectid,a.stageid,a.scopeid,a.teamid,a.userid,CONVERT(VARCHAR(20),a.requestdate,103) AS requestdate,CONVERT(VARCHAR(20),a.requireddate,103) AS requireddate,b.Stage as Stage,c.Scope as Scope,f.Teamname as AllotedTeam,d.username as Username,e.projectname as Project from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid and e.projectstatus in('Yet To Start','WIP') inner join tbl_teams f on f.TeamID=a.teamid  where a.taskstatus in('Yet To Start','WIP') and a.requireddate>=cast(GETDATE() as DATE) order by id desc", con);
            else
                da = new SqlDataAdapter("select a.id,a.requestid,a.taskstatus,a.taskname,a.taskdescription,a.projectid,a.stageid,a.scopeid,a.teamid,a.userid,CONVERT(VARCHAR(20),a.requestdate,103) AS requestdate,CONVERT(VARCHAR(20),a.requireddate,103) AS requireddate,b.Stage as Stage,c.Scope as Scope,f.Teamname as AllotedTeam,d.username as Username,e.projectname as Project from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid and e.projectstatus in ('Yet To Start','WIP')inner join tbl_teams f on f.TeamID=a.teamid  where a.taskstatus in('Yet To Start','WIP') and a.requireddate>=cast(GETDATE() as DATE) and a.userid='" + Session["Userid"] + "' order by id desc", con);
            //if (txt_SearchTasks.Value != "")
            //{
            //    da = new SqlDataAdapter("select a.id, a.requestid,a.taskstatus,a.taskname,a.taskdescription,a.projectid,a.stageid,a.scopeid,a.teamid,a.userid,CONVERT(VARCHAR(20),a.requestdate,103) AS requestdate,CONVERT(VARCHAR(20),a.requireddate,103) AS requireddate,b.Stage as Stage,c.Scope as Scope,f.Teamname as AllotedTeam,d.username as Username,e.projectreq as Project from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid  inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid  inner join tbl_teams f on f.TeamID=a.teamid  where e.projectreq like '%" + txt_SearchTasks.Value + "%' or b.Stage like '%" + txt_SearchTasks.Value + "%' or c.Scope+' - '+c.Description like '%" + txt_SearchTasks.Value + "%' or taskname like '%" + txt_SearchTasks.Value + "%' or taskdescription like '%" + txt_SearchTasks.Value + "%' or d.username like  '%" + txt_SearchTasks.Value + "%' or requestdate like  '%" + txt_SearchTasks.Value + "%' or requireddate like  '%" + txt_SearchTasks.Value + "%' or taskstatus like  '%" + txt_SearchTasks.Value + "%'", con);

            //}
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
        catch (Exception)
        {
        }
    }

    /// <summary>
    /// Currently Not In Use
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            //Not Used Now
            grd_WIP.PageIndex = e.NewPageIndex;
            this.WIP_gridbind();
        }
        catch (Exception)
        {

        }
    }

    /// <summary>
    /// Currently Not in Use
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
    /// To Redirect to DPR Page If Task Current Week task is clicked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grd_WIP_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = grd_WIP.SelectedRow;
        string taskid = ((HiddenField)row.FindControl("hdn_taskid")).Value;
        Session["TaskID"] = taskid;
        Response.Redirect("timetrackingsheet");
    }
    /// <summary>
    /// To View The Current week Task Details That is Placed at bottom of Dashboard
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void SelectCurrentData(object sender, EventArgs e)
    {
        string TotalTime = string.Empty;
        this.block_View.Visible = true;
        this.block_all.Visible = false;
        LinkButton btn = (LinkButton)(sender);
        string id = btn.CommandArgument;
        DataTable dt = null;
        dt = objData.Getdata("select SUM(DATEDIFF(MINUTE,'00:00:00', b.TotalTime))/60 AS hh,SUM(DATEDIFF(MINUTE,'00:00:00', b.TotalTime))%60 as mm from PrmsProductionHour_Backup b where b.task='" + id + "'");
        con.Close();
        con.Open();
        com_Tasks.Connection = con;
        com_Tasks.CommandText = "select a.requestid,a.taskname,a.taskdescription,a.projectid,a.stageid,a.scopeid,a.teamid,a.userid,CONVERT(VARCHAR(20),a.requestdate,103) AS requestdate,CONVERT(VARCHAR(20),a.requireddate,103) AS requireddate,b.Stage as Stage,c.Scope+' - '+c.Description as Scope,f.Teamname as AllotedTeam,d.username as Username,e.projectreq as Project,a.estimatedtime,a.RatingOfTask,a.QualityRating from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid inner join tbl_ProjectReq e on e.projectid=a.projectid inner join tbl_teams f on f.TeamID=a.teamid where a.id='" + id + "'";
        SqlDataReader dr = com_Tasks.ExecuteReader();
        while (dr.Read())
        {
            TitleOfPage.InnerText = dr["requestid"].ToString() + " : " + dr["Project"];
            lbl_TaskTextView.InnerText = dr["taskname"].ToString();
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
            string QualityRatingSelected = dr["RatingOfTask"].ToString();
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
    }

    /// <summary>
    /// If Full Gantt Chart on Panel at is Clicked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnk_All_Click(object sender, EventArgs e)
    {
        ViewState["ALL"] = "Y";
        getAllUsers();
    }

    /// <summary>
    /// Backbutton To Only show neccessary blocks and redirect to dashboard
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_BackButton_Click(object sender, EventArgs e)
    {
        this.block_View.Visible = false;
        this.block_all.Visible = true;
        this.TitleOfPage.InnerText = "Dashboard";
    }

    /// <summary>
    /// RowDatabound To Differentiate Task Status
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grd_WIP_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // Retrieve the underlying data item. In this example
            // the underlying data item is a DataRowView object. 
            DataRowView rowView = (DataRowView)e.Row.DataItem;

            // Retrieve the state value for the current row. 
            String status = rowView["taskstatus"].ToString();

            //Format symbols of WIP Tasks
            if (status == "WIP")
            {
                (e.Row.FindControl("lnk_TaskStatus") as LinkButton).CssClass = "icon glyphicon glyphicon-play fa-2x";
                (e.Row.FindControl("lnk_TaskStatus") as LinkButton).Style.Add("color", "#22A7F0");
            }

            //Format symbols of Yet To Start Tasks
            if (status == "Yet To Start")
            {
                (e.Row.FindControl("lnk_TaskStatus") as LinkButton).CssClass = "icon glyphicon glyphicon-inbox fa-2x";
                (e.Row.FindControl("lnk_TaskStatus") as LinkButton).Style.Add("color", "#9FA400");
            }

        }
    }

    /// <summary>
    /// Generate Coding Standard and Quality Rating Values in Chart
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void generateRatingChart()
    {
        DataTable dt_Users = new DataTable();
        DataTable dt_EachUser = new DataTable();
        DataTable dt_Dates = new DataTable();
        string Rights = Convert.ToString(Session["Rights"]);
        List<DataRow> lst_Users = new List<DataRow>();
        string UsersToBind = string.Empty;
        string FinalQualityString = string.Empty;
        string FinalQualityDates = string.Empty;
        string FinalCodingStandardString = string.Empty;
        string FinalCodingStandardDates = string.Empty;
        StringBuilder sb_New = new StringBuilder();
        //To Filter Users Related To User Access
        //bindUsers(ref dt_Users, Rights, ref lst_Users);
        //foreach (DataRow Users in lst_Users)
        //{
        //    sb_New.Append("'" + Users[0] + "'");
        //    sb_New.Append(",");
        //}
        sb_New.Append("'" + ddl_EmpNo.SelectedItem.Text + "'");
        UsersToBind = sb_New.ToString();
        #region Quality Rating Chart
        #region Dates For Chart's Y Axis
        //Dates For Chart's Y Axis
        string Query = string.Empty;
        Query = "select distinct RequiredDate from tbl_Taskmaster where (QualityRatingOn is not null or RatingOn is not null) and userid in('" + ddl_EmpNo.SelectedValue + "') and month(Requireddate)='" + ddl_FromMonth.SelectedValue + "' and year(Requireddate)=year('" + ddl_FromYear.SelectedValue + "')";
        if (Rights == "Team Leader")
        {
            Query += "and (QualityRatingBy='" + Convert.ToString(Session["Userid"]) + "' and QualityRating is not null) or (Ratingby='" + Convert.ToString(Session["Userid"]) + "' and RatingOfTask is not null)";
        }
        dt_Dates = objData.Getdata(Query);
        sb_New.Clear();
        sb_New.Append("[");
        foreach (DataRow dr in dt_Dates.Rows)
        {
            DateTime dt_Date = Convert.ToDateTime(dr[0]);
            string str_Date = dt_Date.ToString("dd-MMM-yyyy");
            sb_New.Append("'" + str_Date + "',");
        }
        sb_New.Length--;
        sb_New.Append("]");
        FinalQualityDates = sb_New.ToString();
        Session["DatesLineChart"] = FinalQualityDates;
        #endregion

        #region Rating values with Users For Chart's X Axis
        sb_New.Clear();
        sb_New.Append("[");

        int count = 0;
        string crossed = "false";
        //Coding Standard Rating
        sb_New.Append("{name: 'Coding Standard Rating',");

        //sb_New.Append(Convert.ToString(objData.GetSingleData("select username from tbl_usermaster where userid='" + ddl_EmpNo.SelectedValue + "'")) + "',");
        foreach (DataRow dr in dt_Dates.Rows)
        {
            Query = "select Avg(isNull(NullIf(RatingOfTask,''),0))[CodingStandardRating] from tbl_taskmaster where Requireddate='" + dr[0] + "' and userid='" + ddl_EmpNo.SelectedValue + "' and RatingOfTask is not null ";
            if (Rights == "Team Leader")
            {
                Query += "and RatingBy='" + Convert.ToString(Session["Userid"]) + "'";
            }
            Query += "group by Requireddate";

            string CurrentValue = objData.GetSingleData(Query);
            if (CurrentValue != "0" && CurrentValue != "")
            {
                if (count == 0)
                {
                    sb_New.Append("data:[");
                    crossed = "true";
                }
                count++;
                sb_New.Append(CurrentValue + ",");
            }
        }
        sb_New.Length--;
        sb_New.Append("]");
        sb_New.Append("},");

        //Quality Rating
        count = 0;
        sb_New.Append("{name: 'Quality',");

        //sb_New.Append(Convert.ToString(objData.GetSingleData("select username from tbl_usermaster where userid='" + ddl_EmpNo.SelectedValue + "'")) + "',");
        sb_New.Append("color:'orange',");
        foreach (DataRow dr in dt_Dates.Rows)
        {
            Query = "select Avg(isNull(NullIf(QualityRating,''),0))[QualityRating] from tbl_taskmaster where Requireddate='" + dr[0] + "' and userid='" + ddl_EmpNo.SelectedValue + "' and QualityRating is not null ";
            if (Rights == "Team Leader")
            {
                Query += "and QualityRatingBy='" + Convert.ToString(Session["Userid"]) + "'";
            }
            Query += "group by Requireddate";
            string CurrentValue = objData.GetSingleData(Query);
            if (CurrentValue != "0" && CurrentValue != "")
            {
                if (count == 0)
                {
                    sb_New.Append("data:[");
                    crossed = "true";
                }
                count++;
                sb_New.Append(CurrentValue + ",");

            }
        }
        sb_New.Length--;
        if (crossed != "true")
        {
            sb_New.Append(",data:[]");
        }
        else
        {
            sb_New.Append("]");
        }
        sb_New.Append("},");
        sb_New.Length--;
        sb_New.Append("]");
        #endregion

        #region Final Quality Rating Values
        FinalQualityString = sb_New.ToString();
        Session["RatingLineChart"] = FinalQualityString;
        #endregion
        #endregion
    }
    [WebMethod]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public static List<object> generateRatingChart1(string EmpID, string Month, string Year)
    {
        List<object> Chart = new List<object>();
        clsDataControl objData = new clsDataControl();
        DataTable dt_Users = new DataTable();
        DataTable dt_EachUser = new DataTable();
        DataTable dt_Dates = new DataTable();
        string Rights = Convert.ToString(HttpContext.Current.Session["Rights"]);
        List<DataRow> lst_Users = new List<DataRow>();
        List<int> Rating = new List<int>();
        string UsersToBind = string.Empty;
        string FinalQualityString = string.Empty;
        string FinalQualityDates = string.Empty;
        string FinalCodingStandardString = string.Empty;
        string FinalCodingStandardDates = string.Empty;
        ScriptManager.RegisterClientScriptBlock((Page)(HttpContext.Current.Handler), typeof(Page), "mismatch", "console.log($('#ddl_EmpID').val())", true);
        #region Coding Standard/Quality Rating Chart
        #region Dates For Chart's Y Axis
        //Dates For Chart's Y Axis
        string Query = string.Empty;
        Query = "select distinct RequiredDate,Avg(isNull(NullIf(RatingOfTask,''),0))[CodingStandardRating],Avg(isNull(NullIf(QualityRating,''),0))[QualityRating] from tbl_Taskmaster where (QualityRatingOn is not null or RatingOn is not null) and userid in('" + EmpID + "') and month(Requireddate)='" + Month + "' and year(Requireddate)=year('" + Year + "')";
        if (Rights == "Team Leader")
        {
            Query += "and ((QualityRatingBy='" + Convert.ToString(HttpContext.Current.Session["Userid"]) + "' and QualityRating is not null) or (Ratingby='" + Convert.ToString(HttpContext.Current.Session["Userid"]) + "' and RatingOfTask is not null))";
        }
        Query += " group by RequiredDate";
        dt_Dates = objData.Getdata(Query);

        List<string> StartDate = new List<string>();
        foreach (DataRow dr in dt_Dates.Rows)
        {
            DateTime dt_Date = Convert.ToDateTime(dr[0]);
            string str_Date = dt_Date.ToString("dd-MMM-yyyy");
            StartDate.Add(str_Date);

        }
        Chart.Add(StartDate);
        List<int> Quality = new List<int>();
        foreach (DataRow dr in dt_Dates.Rows)
        {
            Quality.Add(Convert.ToInt32(dr[1]));
        }

        Chart.Add(Quality);
        foreach (DataRow dr in dt_Dates.Rows)
        {

            Rating.Add(Convert.ToInt32(dr[2]));

        }
        Chart.Add(Rating);
        #endregion

        #endregion
        return Chart;
        //return "";
    }

    /// <summary>
    /// Bind Users Based on User Role
    /// </summary>
    /// <param name="dt_Users"></param>
    /// <param name="Rights"></param>
    /// <param name="lst_Users"></param>
    private void bindUsers(ref DataTable dt_Users, string Rights, ref List<DataRow> lst_Users)
    {
        switch (Rights.ToLower())
        {
            case "administrator":
                dt_Users = objData.Getdata("select userid,userid+' - '+username[USER] from tbl_usermaster where status=1 and rights='User'");
                lst_Users = dt_Users.AsEnumerable().ToList();
                break;
            case "team leader":
                string TeamUsers = objData.GetSingleData("select replace(''''+userid+'''',',',''',''') from tbl_teamAllotmentMaster where teamleader like ('%" + Convert.ToString(Session["Userid"]) + "%')");
                dt_Users = objData.Getdata("select userid,userid+' - '+username[USER] from tbl_usermaster where status=1 and rights='User' and userid in(" + TeamUsers + ")");
                lst_Users = dt_Users.AsEnumerable().ToList();
                break;
            case "user":
                dt_Users = objData.Getdata("select userid,userid+' - '+username[USER] from tbl_usermaster where userid='" + Convert.ToString(Session["Userid"]) + "'");
                lst_Users = dt_Users.AsEnumerable().ToList();
                break;
        }
    }

    /// <summary>
    /// Month For Line Chart Records to Show
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void ddl_FromMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "mismatch", "loadLineChart()", true);
    }
    /// <summary>
    /// Year For Line Chart Records to Show
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void ddl_FromYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        int YearSelected = Convert.ToInt32(ddl_FromYear.SelectedValue);
        int CurrentMonth = Convert.ToInt32(DateTime.Today.Month);
        if (YearSelected != DateTime.Today.Year)
        {
            bindMonthOfRecords();
        }
        else
        {
            for (int Month = 12; Month > CurrentMonth; Month--)
            {
                ddl_FromMonth.Items.RemoveAt(Month - 1);
            }
        }
        ScriptManager.RegisterStartupScript(this, GetType(), "mismatch", "loadLineChart()", true);
        //BindGrid();
    }
    /// <summary>
    /// Binds the year Dropdown to Select Line Chart Records MonthWise
    /// </summary>
    protected void bindYearOfRecords()
    {
        try
        {
            var years = new Dictionary<int, string>();
            int year = Convert.ToInt32(DateTime.Today.Year);
            ddl_FromYear.Items.Clear();
            ddl_FromYear.Items.Add(Convert.ToString(year - 1));
            ddl_FromYear.Items.Add(Convert.ToString(year));
            con.Close();
        }
        catch (Exception)
        {
        }
    }
    /// <summary>
    /// Binds the Month Dropdown to Select Line Chart Records MonthWise
    /// </summary>
    protected void bindMonthOfRecords()
    {
        try
        {
            ddl_FromMonth.Items.Clear();
            ddl_FromMonth.Items.Insert(0, new ListItem("Jan", "1"));
            ddl_FromMonth.Items.Insert(1, new ListItem("Feb", "2"));
            ddl_FromMonth.Items.Insert(2, new ListItem("Mar", "3"));
            ddl_FromMonth.Items.Insert(3, new ListItem("Apr", "4"));
            ddl_FromMonth.Items.Insert(4, new ListItem("May", "5"));
            ddl_FromMonth.Items.Insert(5, new ListItem("Jun", "6"));
            ddl_FromMonth.Items.Insert(6, new ListItem("Jul", "7"));
            ddl_FromMonth.Items.Insert(7, new ListItem("Aug", "8"));
            ddl_FromMonth.Items.Insert(8, new ListItem("Sep", "9"));
            ddl_FromMonth.Items.Insert(9, new ListItem("Oct", "10"));
            ddl_FromMonth.Items.Insert(10, new ListItem("Nov", "11"));
            ddl_FromMonth.Items.Insert(11, new ListItem("Dec", "12"));
            con.Close();
        }
        catch (Exception)
        {
        }
    }
    /// <summary>
    /// Bind DropDown For Line Chart To Filter Employees
    /// </summary>
    protected void bindEmployeesForChart()
    {
        try
        {
            ddl_EmpNo.Items.Clear();
            con.Close();
            con.Open();
            DataTable dt = new DataTable();
            List<DataRow> lst = new List<DataRow>();
            string Rights = Convert.ToString(Session["Rights"]);
            bindUsers(ref dt, Rights, ref lst);
            ddl_EmpNo.DataSource = dt;
            ddl_EmpNo.DataTextField = "USER";
            ddl_EmpNo.DataValueField = "userid";
            ddl_EmpNo.DataBind();
            con.Close();
        }
        catch (Exception)
        {
        }
    }
    /// <summary>
    /// Drop Down Selected Index Changed Event To Filter Employees
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddl_EmpNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "mismatch", "loadLineChart()", true);
    }
}