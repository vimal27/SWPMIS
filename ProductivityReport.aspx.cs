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

public partial class ProductivityReport : System.Web.UI.Page
{
    /// <summary>
    /// Declarations Part For Variables,Strings,SqlConnections,etc
    /// </summary>
    List<string> TaskDetails = new List<string>();
    string selectedUser, ProjectAlloted;
    DataTable dt = new DataTable();
    DataRow dr = null;
    string[] SplittedProjectAlloted;
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString());
    SqlConnection con2 = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString());
    SqlCommand com_check = new SqlCommand();
    SqlCommand com = new SqlCommand();
    string DateConverted = string.Empty;
    clsDataControl objData = new clsDataControl();
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
                ViewState["isReportClick"] = 0;
                Session["CurrentRequestDate"] = "01/01/2015";
                getDate();
                txt_FromDate.Attributes.Add("readonly", "readonly");
                txt_ToDate.Attributes.Add("readonly", "readonly");
                calext_FromDate.EndDate = DateTime.Now;
                calext_ToDate.EndDate = DateTime.Now;
                Session["BarChartData"] = "";
                Session["PercentageChart"] = 0;
                string Rights = Convert.ToString(Session["Rights"]);
                if (Rights == "Team Leader" || Rights == "Administrator")
                {
                    icon_EmployeeDdl.Visible = true;
                    bindUsers();
                    Session["BarChartData"] = pcData.generateBarChart(ddl_EmployeeID.SelectedValue, "01/01/2015", txt_ToDate.Text);
                    if (Rights == "Team Leader")
                    {
                        Session["BarChartData"] = pcData.generateBarChart(ddl_EmployeeID.SelectedValue, "01/01/2015", txt_ToDate.Text, true, Session["Userid"].ToString());
                    }
                }
                else
                {
                    icon_EmployeeDdl.Visible = false;
                    lbl_CurrentEmployeeID.Text = Session["UserName"].ToString() + " - " + Session["Userid"].ToString();
                    BindTaskDetails("01/01/2015", DateTime.Now.ToString("dd/MM/yyyy"), Session["Userid"].ToString());
                    Session["BarChartData"] = pcData.generateBarChart(Convert.ToString(Session["Userid"]), "01/01/2015", txt_ToDate.Text);
                }
                if (Session["Userid"] == null)
                    Response.Redirect("Login");
                else if (Session["Userid"].ToString() == "")
                    Response.Redirect("Login");
            }
        }
        catch (Exception ex)
        {

        }
    }

    /// <summary>
    /// Gets the Currentdate For From Date and To Date Textbox.
    /// </summary>
    protected void getDate()
    {
        DateTime dt = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-GB"));
        DateConverted = dt.ToString("dd/MM/yyyy");
        txt_FromDate.Text = DateConverted;
        txt_ToDate.Text = DateConverted;
    }

    /// <summary>
    /// Reset If cancel Click
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void btn_Cancel_Click(object sender, EventArgs e)
    {
        getDate();
    }

    /// <summary>
    /// Binding Report Function
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void btn_Report_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["isReportClick"] = 1;
            if (Session["Rights"].ToString() == "Administrator" || Session["Rights"].ToString() == "Team Leader")
            {
                BindTaskDetails(Convert.ToString(txt_FromDate.Text), Convert.ToString(txt_ToDate.Text), ddl_EmployeeID.SelectedValue.ToString());
                Session["BarChartData"] = pcData.generateBarChart(ddl_EmployeeID.SelectedValue, txt_FromDate.Text, txt_ToDate.Text);
                if (Session["Rights"].ToString() == "Team Leader" && ddl_EmployeeID.SelectedValue == "N/A")
                {
                    BindTaskDetails(Convert.ToString(txt_FromDate.Text), Convert.ToString(txt_ToDate.Text), ddl_EmployeeID.SelectedValue.ToString(), true, Session["Userid"].ToString());
                    Session["BarChartData"] = pcData.generateBarChart(ddl_EmployeeID.SelectedValue, txt_FromDate.Text, txt_ToDate.Text, true, Session["Userid"].ToString());
                }
            }
            else
            {
                BindTaskDetails(Convert.ToString(txt_FromDate.Text), Convert.ToString(txt_ToDate.Text), Convert.ToString(Session["Userid"]));
                Session["BarChartData"] = pcData.generateBarChart(Convert.ToString(Session["Userid"]), txt_FromDate.Text, txt_ToDate.Text);
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "viewProductionChart", "setChart();", true);
        }
        catch (Exception ex)
        {
            // BindTaskDetails(Convert.ToString(txt_FromDate.Text), Convert.ToString(txt_ToDate.Text), Session["Userid"].ToString());
        }
    }
    
    /// <summary>
    /// Export Productivity Report
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void btn_Export_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime dt_FromDate = DateTime.ParseExact(Convert.ToString(txt_FromDate.Text), "dd/MM/yyyy", null);
            DateTime dt_ToDate = DateTime.ParseExact(Convert.ToString(txt_ToDate.Text), "dd/MM/yyyy", null);
            DataTable dt = new DataTable();
            ViewState["isReportClick"] = 1;
            if (Session["Rights"].ToString() == "Administrator" || Session["Rights"].ToString() == "Team Leader")
            {
                dt = objData.Getdata("select (select f.projectid+' - '+f.projectname[projectid] from tbl_ProjectReq f where f.projectid=pvt.projectid)[projectid],(isNull(pvt.total,0))[Total],isNull(early,0)[Early],isNull(ontime,0)[OnTime],isNull(extend,0)[Extend],(convert(decimal(20),(convert(decimal(20,0),(ISNULL(pvt.early,0)))/convert(decimal(20,0),ISNULL(pvt.total,0)))*100))[Early2],(convert(decimal(20),(convert(decimal(20,0),(ISNULL(pvt.ontime,0)))/convert(decimal(20,0),ISNULL(pvt.total,0)))*100))[OnTime2],(convert(decimal(20),(convert(decimal(20,0),(ISNULL(pvt.extend,0)))/convert(decimal(20,0),ISNULL(pvt.total,0)))*100))[Extend2] from(select temp.projectid,temp.early[earlydel],temp.deliverystatus from(select count(distinct b.task)[early],b.projectid,'early'[deliverystatus] from tbl_Taskmaster a,PrmsProductionHour_Backup b where a.id=b.task and a.taskstatus='Completed' and a.Production>100  and b.CurrentDate between '" + dt_FromDate.ToString("yyyy-MM-dd") + "' and '" + dt_ToDate.ToString("yyyy-MM-dd") + "' and b.statusoftask=100  group by b.projectid  Union select count(distinct b.task)[ontime],b.projectid,'ontime'[deliverystatus] from tbl_Taskmaster a,PrmsProductionHour_Backup b where a.id=b.task and a.taskstatus='Completed' and a.Production=100  and b.CurrentDate between '" + dt_FromDate.ToString("yyyy-MM-dd") + "' and '" + dt_ToDate.ToString("yyyy-MM-dd") + "' and b.statusoftask=100  group by b.projectid  Union select count(distinct b.task)[extend],b.projectid,'extend'[deliverystatus] from tbl_Taskmaster a,PrmsProductionHour_Backup b where a.id=b.task and a.taskstatus='Completed' and a.Production<100  and b.CurrentDate between '" + dt_FromDate.ToString("yyyy-MM-dd") + "' and '" + dt_ToDate.ToString("yyyy-MM-dd") + "' and b.statusoftask=100  group by b.projectid  Union select count(distinct b.task)[total],b.projectid,'total'[deliverystatus] from tbl_Taskmaster a,PrmsProductionHour_Backup b where a.id=b.task and a.taskstatus='Completed' and b.CurrentDate between '" + dt_FromDate.ToString("yyyy-MM-dd") + "' and '" + dt_ToDate.ToString("yyyy-MM-dd") + "' and b.statusoftask=100  group by b.projectid) as temp) as e pivot(max(earlydel) for deliverystatus in([total],[early],[ontime],[extend])) as pvt");
                if (Session["Rights"].ToString() == "Team Leader")
                {
                    string TeamLeader = Convert.ToString(Session["Userid"]);
                    string TeamUsers = objData.GetSingleData("select replace(''''+userid+'''',',',''',''') from tbl_teamAllotmentMaster where teamleader like ('%" + TeamLeader + "%')");
                    dt = objData.Getdata("select (select f.projectid+' - '+f.projectname[projectid] from tbl_ProjectReq f where f.projectid=pvt.projectid)[projectid],(isNull(pvt.total,0))[Total],isNull(early,0)[Early],isNull(ontime,0)[OnTime],isNull(extend,0)[Extend],(convert(decimal(20),(convert(decimal(20,0),(ISNULL(pvt.early,0)))/convert(decimal(20,0),ISNULL(pvt.total,0)))*100))[Early2],(convert(decimal(20),(convert(decimal(20,0),(ISNULL(pvt.ontime,0)))/convert(decimal(20,0),ISNULL(pvt.total,0)))*100))[OnTime2],(convert(decimal(20),(convert(decimal(20,0),(ISNULL(pvt.extend,0)))/convert(decimal(20,0),ISNULL(pvt.total,0)))*100))[Extend2] from(select temp.projectid,temp.early[earlydel],temp.deliverystatus from(select count(distinct b.task)[early],b.projectid,'early'[deliverystatus] from tbl_Taskmaster a,PrmsProductionHour_Backup b where a.id=b.task and a.taskstatus='Completed' and a.Production>100  and b.CurrentDate between '" + dt_FromDate.ToString("yyyy-MM-dd") + "' and '" + dt_ToDate.ToString("yyyy-MM-dd") + "' and b.statusoftask=100  and a.userid in(" + TeamUsers + ")  group by b.projectid  Union select count(distinct b.task)[ontime],b.projectid,'ontime'[deliverystatus] from tbl_Taskmaster a,PrmsProductionHour_Backup b where a.id=b.task and a.taskstatus='Completed' and a.Production=100  and b.CurrentDate between '" + dt_FromDate.ToString("yyyy-MM-dd") + "' and '" + dt_ToDate.ToString("yyyy-MM-dd") + "' and b.statusoftask=100   and a.userid in(" + TeamUsers + ")  group by b.projectid  Union select count(distinct b.task)[extend],b.projectid,'extend'[deliverystatus] from tbl_Taskmaster a,PrmsProductionHour_Backup b where a.id=b.task and a.taskstatus='Completed' and a.Production<100  and b.CurrentDate between '" + dt_FromDate.ToString("yyyy-MM-dd") + "' and '" + dt_ToDate.ToString("yyyy-MM-dd") + "' and b.statusoftask=100  and a.userid in(" + TeamUsers + ")   group by b.projectid  Union select count(distinct b.task)[total],b.projectid,'total'[deliverystatus] from tbl_Taskmaster a,PrmsProductionHour_Backup b where a.id=b.task and a.taskstatus='Completed' and b.CurrentDate between '" + dt_FromDate.ToString("yyyy-MM-dd") + "' and '" + dt_ToDate.ToString("yyyy-MM-dd") + "' and b.statusoftask=100  and a.userid in(" + TeamUsers + ")   group by b.projectid) as temp ) as e pivot(max(earlydel) for deliverystatus in([total],[early],[ontime],[extend])) as pvt");
                }
            }
            else
            {
                string CurrentUser = Convert.ToString(Session["Userid"]);
                dt = objData.Getdata("select (select f.projectid+' - '+f.projectname[projectid] from tbl_ProjectReq f where f.projectid=pvt.projectid)[projectid],(isNull(pvt.total,0))[Total],isNull(early,0)[Early],isNull(ontime,0)[OnTime],isNull(extend,0)[Extend],(convert(decimal(20),(convert(decimal(20,0),(ISNULL(pvt.early,0)))/convert(decimal(20,0),ISNULL(pvt.total,0)))*100))[Early2],(convert(decimal(20),(convert(decimal(20,0),(ISNULL(pvt.ontime,0)))/convert(decimal(20,0),ISNULL(pvt.total,0)))*100))[OnTime2],(convert(decimal(20),(convert(decimal(20,0),(ISNULL(pvt.extend,0)))/convert(decimal(20,0),ISNULL(pvt.total,0)))*100))[Extend2] from(select temp.projectid,temp.early[earlydel],temp.deliverystatus from(select count(distinct b.task)[early],b.projectid,'early'[deliverystatus] from tbl_Taskmaster a,PrmsProductionHour_Backup b where a.id=b.task and a.taskstatus='Completed' and a.Production>100  and b.CurrentDate between '" + dt_FromDate.ToString("yyyy-MM-dd") + "' and '" + dt_ToDate.ToString("yyyy-MM-dd") + "' and b.statusoftask=100 and a.userid='" + CurrentUser + "'   group by b.projectid  Union select count(distinct b.task)[ontime],b.projectid,'ontime'[deliverystatus] from tbl_Taskmaster a,PrmsProductionHour_Backup b where a.id=b.task and a.taskstatus='Completed' and a.Production=100  and b.CurrentDate between '" + dt_FromDate.ToString("yyyy-MM-dd") + "' and '" + dt_ToDate.ToString("yyyy-MM-dd") + "' and b.statusoftask=100 and a.userid='" + CurrentUser + "'   group by b.projectid  Union select count(distinct b.task)[extend],b.projectid,'extend'[deliverystatus] from tbl_Taskmaster a,PrmsProductionHour_Backup b where a.id=b.task and a.taskstatus='Completed' and a.Production<100  and b.CurrentDate between '" + dt_FromDate.ToString("yyyy-MM-dd") + "' and '" + dt_ToDate.ToString("yyyy-MM-dd") + "' and b.statusoftask=100 and a.userid='" + CurrentUser + "'   group by b.projectid  Union select count(distinct b.task)[total],b.projectid,'total'[deliverystatus] from tbl_Taskmaster a,PrmsProductionHour_Backup b where a.id=b.task and a.taskstatus='Completed' and b.CurrentDate between '" + dt_FromDate.ToString("yyyy-MM-dd") + "' and '" + dt_ToDate.ToString("yyyy-MM-dd") + "' and b.statusoftask=100 and a.userid='" + CurrentUser + "'  group by b.projectid) as temp ) as e  pivot(max(earlydel) for deliverystatus in([total],[early],[ontime],[extend])) as pvt");
            }
            int sumOfTasks = Convert.ToInt32(dt.Compute("SUM(Total)", string.Empty));
            int sumOfAheadCounts = Convert.ToInt32(dt.Compute("SUM(Early)", string.Empty));
            int sumOfOnTimeCounts = Convert.ToInt32(dt.Compute("SUM(OnTime)", string.Empty));
            int sumOfDelayCounts = Convert.ToInt32(dt.Compute("SUM(Extend)", string.Empty));
            int sumOfAheadRate = Convert.ToInt32(dt.Compute("(SUM(Early)/Sum(Total))*100", string.Empty));
            int sumOfOnTimeRate = Convert.ToInt32(dt.Compute("(SUM(OnTime)/Sum(Total))*100", string.Empty));
            int sumOfDelayRate = Convert.ToInt32(dt.Compute("(SUM(Extend)/Sum(Total))*100", string.Empty));
            if (dt.Rows.Count > 0)
            {
                List<string> HeaderNames = new List<string>();
                HeaderNames.Add("Project");
                HeaderNames.Add("Tasks");
                HeaderNames.Add("Ahead");
                HeaderNames.Add("On-time");
                HeaderNames.Add("Delay");
                HeaderNames.Add("Ahead");
                HeaderNames.Add("On-time");
                HeaderNames.Add("Delay");
                string filePath = HttpContext.Current.Server.MapPath(".") + "\\GenericReports\\";
                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath + "\\GenericReports\\");
                string excelpath = filePath + ("ProductivityReport" + DateTime.Now.ToShortDateString().Replace("/", "") + ".xls");
                if (File.Exists(excelpath))
                    File.Delete(excelpath);
                FileInfo file = new FileInfo(excelpath);
                string reportName = "Productivity Report as on : " + DateTime.Now.ToString("dd/MM/yyyy");
                StringBuilder sb = new StringBuilder();
                sb.Append("<html>");
                sb.Append("<head>");
                sb.Append("<body>");
                sb.Append("<table border='1' style='font-family:Calibri; font-size:14px;'>");
                sb.Append("<tr><td colspan='8' style='text-align:center;vertical-align:middle;font-weight:bold;background-color: #ddebf7;'>" + reportName + "</td></tr>");
                sb.Append("<tr><td style='background-color: #ddebf7;'></td><td style='background-color: #ddebf7;'></td><td colspan='3' style='text-align:center;vertical-align:middle;font-weight:bold;background-color: #ddebf7;'>Delivery (in Counts)</td><td colspan='3' style='text-align:center;vertical-align:middle;font-weight:bold;background-color: #ddebf7;'>Delivery (in %)</td></tr>");
                sb.Append("<tr>");
                foreach (string header in HeaderNames)
                {
                    sb.Append("<td style='text-align:center;vertical-align:middle;font-weight:bold;background-color: #ddebf7;'>" + header + "</td>");
                }
                sb.Append("</tr>");
                foreach (DataRow row in dt.Rows)
                {
                    sb.Append("<tr>");
                    foreach (DataColumn column in dt.Columns)
                    {
                        sb.Append("<td style='text-align:center;vertical-align:middle;'>" + row[column].ToString().ToUpper() + "</td>");
                    }
                    sb.Append("</tr>");
                }
                sb.Append("<tr>");
                sb.Append("<td style='text-align:center;vertical-align:middle;font-weight:bold'>Sum Of Rows</td>");
                sb.Append("<td style='text-align:center;vertical-align:middle;font-weight:bold''>" + sumOfTasks + "</td>");
                sb.Append("<td style='text-align:center;vertical-align:middle;font-weight:bold''>" + sumOfAheadCounts + "</td>");
                sb.Append("<td style='text-align:center;vertical-align:middle;font-weight:bold''>" + sumOfOnTimeCounts + "</td>");
                sb.Append("<td style='text-align:center;vertical-align:middle;font-weight:bold''>" + sumOfDelayCounts + "</td>");
                sb.Append("<td style='text-align:center;vertical-align:middle;font-weight:bold''>" + sumOfAheadRate + "</td>");
                sb.Append("<td style='text-align:center;vertical-align:middle;font-weight:bold''>" + sumOfOnTimeRate + "</td>");
                sb.Append("<td style='text-align:center;vertical-align:middle;font-weight:bold''>" + sumOfDelayRate + "</td>");
                sb.Append("</tr>");
                sb.Append("</table>");
                sb.Append("</body>");
                sb.Append("</head>");
                sb.Append("</html>");
                File.WriteAllText(excelpath, sb.ToString());
                Response.Clear();
                Response.AddHeader("Content-Length", file.Length.ToString(CultureInfo.InvariantCulture));
                Response.AddHeader("Content-Disposition", "attachment;filename=\"" + ("ProductivityReport.xls") + "\"");
                Response.ContentType = "application/octet-stream";
                Response.Flush();
                Response.TransmitFile(filePath + ("ProductivityReport" + DateTime.Now.ToShortDateString().Replace("/", "") + ".xls"));
                Response.End();
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "viewProductionChart", "setChart();", true);
        }
        catch (Exception ex)
        {
            //BindTaskDetails(Convert.ToString(txt_FromDate.Text), Convert.ToString(txt_ToDate.Text), Session["Userid"].ToString());
        }
    }
    
    /// <summary>
    /// Redirect To Status With All Selected Task Details
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void RedirectToStatus_6(object sender, EventArgs e)
    {
        Session["CurrentModeSelected"] = 6;
        if (ViewState["isReportClick"].ToString() == "1")
        {
            Session["CurrentRequestDate"] = DateTime.ParseExact(txt_FromDate.Text, "dd/MM/yyyy", null);
        }
        Session["CurrentRequiredDate"] = DateTime.ParseExact(txt_ToDate.Text, "dd/MM/yyyy", null);
        if (Session["Rights"].ToString() == "Administrator" || Session["Rights"].ToString() == "Team Leader")
        {
            Session["CurrentRequestDate"] = DateTime.ParseExact(txt_FromDate.Text, "dd/MM/yyyy", null);
            Session["CurrentEmployeeSelected"] = ddl_EmployeeID.SelectedValue == "N/A" ? "" : ddl_EmployeeID.SelectedValue;
        }
        else
        {
            Session["CurrentEmployeeSelected"] = Session["Userid"].ToString();
        }
        Response.Redirect("Status_6");
    }
    /// <summary>
    /// Redirect To Status With All Selected Task Details
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void RedirectToStatus_7(object sender, EventArgs e)
    {
        Session["CurrentModeSelected"] = 7;
        if (ViewState["isReportClick"].ToString() == "1")
        {
            Session["CurrentRequestDate"] = DateTime.ParseExact(txt_FromDate.Text, "dd/MM/yyyy", null);
        }
        Session["CurrentRequiredDate"] = DateTime.ParseExact(txt_ToDate.Text, "dd/MM/yyyy", null);
        if (Session["Rights"].ToString() == "Administrator" || Session["Rights"].ToString() == "Team Leader")
        {
            Session["CurrentRequestDate"] = DateTime.ParseExact(txt_FromDate.Text, "dd/MM/yyyy", null);
            Session["CurrentEmployeeSelected"] = ddl_EmployeeID.SelectedValue == "N/A" ? "" : ddl_EmployeeID.SelectedValue;
        }
        else
        {
            Session["CurrentEmployeeSelected"] = Session["Userid"].ToString();
        }
        Response.Redirect("Status_7");
    }
    /// <summary>
    /// Redirect To Status With All Selected Task Details
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void RedirectToStatus_8(object sender, EventArgs e)
    {
        Session["CurrentModeSelected"] = 8;
        if (ViewState["isReportClick"].ToString() == "1")
        {
            Session["CurrentRequestDate"] = DateTime.ParseExact(txt_FromDate.Text, "dd/MM/yyyy", null);
        }
        Session["CurrentRequiredDate"] = DateTime.ParseExact(txt_ToDate.Text, "dd/MM/yyyy", null);
        if (Session["Rights"].ToString() == "Administrator" || Session["Rights"].ToString() == "Team Leader")
        {
            Session["CurrentRequestDate"] = DateTime.ParseExact(txt_FromDate.Text, "dd/MM/yyyy", null);
            Session["CurrentEmployeeSelected"] = ddl_EmployeeID.SelectedValue == "N/A" ? "" : ddl_EmployeeID.SelectedValue;
        }
        else
        {
            Session["CurrentEmployeeSelected"] = Session["Userid"].ToString();
        }
        Response.Redirect("Status_8");
    }
    /// <summary>
    /// Binds the task details.
    /// </summary>
    /// <param name="FromDate">From date.</param>
    /// <param name="ToDate">To date.</param>
    /// <param name="EmpID">Emp I.</param>
    /// <param name="isTL">If set to <c>true</c> is T.</param>
    /// <param name="userIDTL">User IDT.</param>
    private void BindTaskDetails(string FromDate, string ToDate, string EmpID, bool isTL = false, string userIDTL = "")
    {
        TaskDetails = pcData.getMonthWiseProduction(FromDate, ToDate, EmpID);
        if (isTL)
        {
            TaskDetails = pcData.getMonthWiseProduction(FromDate, ToDate, EmpID, true, Session["Userid"].ToString());
        }
        lbl_OnEarly.Text = TaskDetails[0];
        lbl_OnTime.Text = TaskDetails[1];
        lbl_Extended.Text = TaskDetails[2];
        Session["PercentageChart"] = TaskDetails[3];

        dt.Columns.Clear();
        dt.Rows.Clear();
        //define the columns
        dt.Columns.Add(new DataColumn("Slno", typeof(string)));
        dt.Columns.Add(new DataColumn("On Early Delivery", typeof(string)));
        dt.Columns.Add(new DataColumn("On Time Delivery", typeof(string)));
        dt.Columns.Add(new DataColumn("Extended Delivery", typeof(string)));
        dt.Columns.Add(new DataColumn("Average Production", typeof(string)));


        //create new row
        dr = dt.NewRow();


        //add values to each rows
        dr["Slno"] = 1;
        dr["On Early Delivery"] = TaskDetails[0];
        dr["On Time Delivery"] = TaskDetails[1];
        dr["Extended Delivery"] = TaskDetails[2];
        dr["Average Production"] = TaskDetails[3];


        //add the row to DataTable
        dt.Rows.Add(dr);
        ScriptManager.RegisterStartupScript(this, GetType(), "viewProductionChart", "setChart();", true);
    }

    /// <summary>
    /// Binds the users Based On User access.
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
}