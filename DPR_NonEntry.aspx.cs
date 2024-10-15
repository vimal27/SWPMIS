
//Required Namespaces
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

public partial class DPR_NonEntry : System.Web.UI.Page
{
    /// <summary>
    /// Declarations Part For Variables,Strings,SqlConnections,etc
    /// </summary>
    ProductionCalculation pcData = new ProductionCalculation();
    bool isExport = false;
    SqlDataAdapter sda = new SqlDataAdapter();
    DataSet ds = new DataSet();
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
    string UserIDSelected = string.Empty;
    /// <summary>
    /// Page Load Function
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            UserIDSelected = (Convert.ToString(Request.QueryString["EmpID"]));
            //Function to Redirect Login Page If Session Is Expired
            if (Session["Userid"] == null) Response.Redirect("Login");
            else if (Session["Userid"].ToString() == "") Response.Redirect("Login");
            if (!IsPostBack)
            {
                ViewState["View"] = "false";
                string Rights = Convert.ToString(Session["Rights"]);
                //Function To Insert Current Date on Textbox FromDate and ToDate
                getDate();
                if (ViewState["View"] == "true")
                {
                    bindGrid();
                }
                if (!string.IsNullOrEmpty(UserIDSelected))
                {
                    btn_Report_Click(null, null);
                }
                //TextBoxes are Readonly To Avoid Invalid Characters from users
                txt_FromDate.Attributes.Add("readonly", "readonly");
                txt_ToDate.Attributes.Add("readonly", "readonly");
                calext_FromDate.EndDate = DateTime.Now;
                calext_ToDate.EndDate = DateTime.Now;
                Rights = Convert.ToString(Session["Rights"]);
            }
        }
        catch (Exception)
        {

        }
    }
    /// <summary>
    /// Function To Insert Current Date on Textbox FromDate and ToDate
    /// </summary>
    protected void getDate()
    {
        DateTime dt = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-GB"));
        DateConverted = dt.ToString("dd/MM/yyyy");
        txt_FromDate.Text = DateConverted;
        txt_ToDate.Text = DateConverted;
        if (!string.IsNullOrEmpty(UserIDSelected))
        {
            txt_FromDate.Text = DateTime.Today.AddDays(-1).ToString("dd/MM/yyyy");
            txt_FromDate_TextChanged(null, null);
        }
    }
    /// <summary>
    /// Reset If Cancel Button Is Clicked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_Cancel_Click(object sender, EventArgs e)
    {
        getDate();
        bindGrid();
        block_Grid.Visible = false;
        ViewState["View"] = "false";
        btn_Report.Enabled = true;
    }

    /// <summary>
    /// IF In TeamLeader Login Gridview Bind Function with his Team Members
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
    /// IF In TeamLeader Login Gridview Bind Function with his Team Members
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
    /// GridView Bind If Report Button Is Clicked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_Report_Click(object sender, EventArgs e)
    {
        try
        {
            bindGrid();
            btn_Report.Enabled = false;
            block_Grid.Visible = true;
            ViewState["View"] = "true";
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
            if (con.State == ConnectionState.Closed) con.Open();
            com.Parameters.Clear();
            DateTime datetimeNow = DateTime.Now;
            TimeSpan timeNow = new TimeSpan(10, 0, 0);
            TimeSpan getdays;

            //WebService named TMS-DPREntrySWPMIS Is Used to bind with attendance details
            SWPMIS_Service.LoginWebService myClient = new SWPMIS_Service.LoginWebService();
            string Rights = Session["Rights"].ToString();
            string MinDateConverted = DateTime.ParseExact(txt_FromDate.Text.ToString(), "dd/MM/yyyy", null).ToString("MM/dd/yyyy");
            string MaxDateConverted = DateTime.ParseExact(txt_ToDate.Text.ToString(), "dd/MM/yyyy", null).ToString("MM/dd/yyyy");
            StartTime = DateTime.ParseExact(txt_FromDate.Text.ToString(), "dd/MM/yyyy", null);
            EndTime = DateTime.ParseExact(txt_ToDate.Text.ToString(), "dd/MM/yyyy", null);
            int result2 = DateTime.Compare(StartTime, EndTime);

            //While Loop To Bind Dpr-Non Entry with from date to to date
            while (Convert.ToDateTime(MaxDateConverted).Subtract(Convert.ToDateTime(MinDateConverted)).Days != 0)
            {
                string result = myClient.TMS_DPR_ENTRY(StartTime.ToString());
                if (result == "") result = "'0'";

                if (result.Length > 3)
                    result = "'" + result.Substring(0, result.Length - 2);

                if (MinDateConverted != "")
                    getdays = datetimeNow - Convert.ToDateTime(StartTime.ToString()).Date;
                else
                {
                    getdays = new TimeSpan();
                }
                string a = Convert.ToString(StartTime.ToString().Split(' ')[0]);

                if (datetimeNow.TimeOfDay <= timeNow && getdays.Days == 1)
                {
                    com.CommandText = @"select ROW_NUMBER() over(order by sno)as slno,convert(varchar(10),convert(datetime,'" + a + "'),103) as CurrentDate,userid,username from tbl_usermaster where status in ('1') and userid not in (select EmpNo from PrmsProductionHour_Backup where CurrentDate='" + a + "') and userid not in ('L1040','admin','L1053','c0358','L1044','L1170','C0023')";
                }
                else
                {
                    com.CommandText = @"select ROW_NUMBER() over(order by sno)as slno,convert(varchar(10),convert(datetime,'" + a + "'),103) as CurrentDate,userid,username from [SWPMIS].dbo.tbl_usermaster where status in ('1') and userid not in (select EmpNo from [SWPMIS].dbo.PrmsProductionHour_Backup where currentdate='" + a + "') and userid not in ('L1040','admin','L1053','c0358','L1044','L1170','C0023') and userid in (" + result + ")";
                }
                if (Rights == "Team Leader")
                {
                    string users = "";
                    com_check.Connection = con2;
                    con2.Close();
                    con2.Open();
                    com_check.CommandText = "select userid from tbl_teamAllotmentMaster where teamleader='" + Session["Userid"].ToString() + "'";
                    SqlDataReader read = com_check.ExecuteReader();
                    if (read.HasRows)
                    {
                        read.Read();
                        users = "'" + read[0].ToString().Replace(",", "\',\'") + "'";
                    }
                    com.CommandText += " and userid in(" + users + ")";
                }
                if (Rights == "User")
                {
                    com.CommandText += " and userid ='" + Session["Userid"].ToString() + "'";
                }
                com.Connection = con;
                sda.SelectCommand = (com);
                sda.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    grd_DPR.DataSource = ds;
                    grd_DPR.DataBind();
                }
                else
                {
                    grd_DPR.DataSource = ds;
                    grd_DPR.DataBind();
                }
                StartTime = StartTime.Date.AddDays(1);
                MinDateConverted = StartTime.ToString();
            }
            if (isExport == true)
            {
                ExportasExcel(ds);
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btn_Export_Click(object sender, EventArgs e)
    {
        isExport = true;
        bindGrid();
    }
    private void ExportasExcel(DataSet ds)
    {
        DataTable dt;
        dt = ds.Tables[0];
        dt.Columns.RemoveAt(0);
        List<string> lst_Headers = new List<string>();
        lst_Headers.Add("Employee ID");
        lst_Headers.Add("Employee Name");
        lst_Headers.Add("Date");
        List<string> ExcelReport = pcData.generateExcelReport(dt, "DprNonEntryReport", "GenericReports", "DPR Non Entry Report", 3, lst_Headers);
        FileInfo file = new FileInfo(ExcelReport[2]);
        Response.Clear();
        Response.AddHeader("Content-Length", file.Length.ToString(CultureInfo.InvariantCulture));
        Response.AddHeader("Content-Disposition", "attachment;filename=\"" + ("DprNonEntryReport.xls") + "\"");
        Response.ContentType = "application/octet-stream";
        Response.Flush();
        Response.TransmitFile(ExcelReport[0] + ("DprNonEntryReport" + DateTime.Now.ToShortDateString().Replace("/", "") + ".xls"));
        Response.End();
    }
    /// <summary>
    /// PageIndexchanging Currently not in use
    /// </summary>
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
    /// From Date TextBox Change Event To Bind Gridview
    /// </summary>
    protected void txt_FromDate_TextChanged(object sender, EventArgs e)
    {
        if (ViewState["View"].ToString() == "true")
        {
            bindGrid();
        }
    }

    /// <summary>
    /// To Date TextBox Change Event To Bind Gridview
    /// </summary>
    protected void txt_ToDate_TextChanged(object sender, EventArgs e)
    {
        if (ViewState["View"].ToString() == "true")
        {
            bindGrid();
        }
    }
}