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
using System.Collections.Generic;
using System.Linq;

public partial class CodingStandardRating : System.Web.UI.Page
{
    /// <summary>
    /// Declarations Part For Variables,Strings,SqlConnections,etc
    /// </summary>
    List<string> lst_Users = new List<string>();
    string selectedUser, ProjectAlloted;
    string RatingType = string.Empty;
    string[] SplittedProjectAlloted;
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString());
    SqlConnection con2 = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString());
    SqlCommand com_check = new SqlCommand();
    SqlCommand com = new SqlCommand();
    string DateConverted = string.Empty;
    StringBuilder toBuildRadioButtonList = new StringBuilder();
    ProductionCalculation pcData = new ProductionCalculation();
    clsDataControl objData = new clsDataControl();
    DataTable dt = new DataTable();
    string CurrentTeamLeader = string.Empty;
    string Rights = string.Empty;
    /// <summary>
    /// Page_s the load.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            btn_AddRating.Visible = false;
            txt_DateOfRating.Attributes.Add("readonly", "readonly");
            calext_DateOfRating.StartDate = DateTime.Today.AddDays(-1);
            calext_DateOfRating.EndDate = DateTime.Today;
            //Dynamic Controls Addition Function
            BindRecord();
            if (!IsPostBack)
            {
                txt_DateOfRating.Text = DateTime.Today.ToString("dd/MM/yyyy");
                calext_ToDate.StartDate = DateTime.Today;
                rbl_RatingType.SelectedValue = "Coding Standard Rating";
                //Bind Gridview of Records
                bindGrid();
                Rights = Convert.ToString(Session["Rights"]);

                //DropDown Bind For TL and Admin
                if (Rights == "Team Leader" || Rights == "Administrator")
                {
                    icon_EmployeeDdl.Visible = true;
                    bindUsers();
                }
                //Today Date function
                getDate();

                //TextBox Datepicker Readonly
                txt_FromDate.Attributes.Add("readonly", "readonly");
                txt_ToDate.Attributes.Add("readonly", "readonly");

                //Block Restricted Dates
                calext_FromDate.EndDate = DateTime.Now;
                calext_ToDate.EndDate = DateTime.Now;

                //If Session Expired Then Logout
                if (Session["Userid"] == null) Response.Redirect("Login");
                else if (Session["Userid"].ToString() == "") Response.Redirect("Login");
                Rights = Convert.ToString(Session["Rights"]);

                //New Rating Option For TL and Admin
                if (Rights == "Administrator")
                {
                    //btn_AddRating.Visible = true;
                }
                else if (Rights == "Team Leader")
                {
                    // btn_AddRating.Visible = true;
                }
                else
                {
                    //btn_AddRating.Visible = false;
                    icon_EmployeeDdl.Visible = false;
                    lbl_CurrentEmployeeID.Text = Session["UserName"].ToString() + " - " + Session["Userid"].ToString();
                }
            }
        }
        catch (Exception)
        {

        }
    }
    /// <summary>
    /// Redirects to Add Today Coding Standard Rating Form
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void btn_AddRating_Click(object sender, EventArgs e)
    {
        rbl_RatingType.SelectedValue = "Coding Standard Rating";
        txt_DateOfRating.Text = DateTime.Today.ToString("dd/MM/yyyy");
        block_History.Visible = false;
        block_NewRating.Visible = true;
        string CurrentTeamLeader = string.Empty;

    }
    /// <summary>
    /// Bind DropDowns and Labels Dynamically based on User Access
    /// </summary>
    /// <param name="CurrentTeamLeader">Current team leader.</param>
    void BindRecord()
    {
        string CurrentTeamLeader = string.Empty;
        string Rights = Convert.ToString(Session["Rights"]);
        switch (Rights)
        {
            case "Team Leader":
                CurrentTeamLeader = Convert.ToString(Session["Userid"]);
                break;
            case "Administrator":
                CurrentTeamLeader = "";
                break;
            default:
                CurrentTeamLeader = "Normal User";
                break;
        }
        if (Rights == "Team Leader" || Rights == "Administrator")
        {
            DataTable dt = new DataTable();
            List<string> lst_Users = new List<string>();
            if (Rights == "Team Leader")
            {
                string TeamUsers = objData.GetSingleData("select replace(''''+userid+'''',',',''',''') from tbl_teamAllotmentMaster where teamleader like ('%" + CurrentTeamLeader + "%')");
                dt = objData.Getdata("select userid from tbl_usermaster where userid in(" + TeamUsers + ") and status=1 and userid!='admin'");
            }
            else
                if (Rights == "Administrator")
                {
                    dt = objData.Getdata("select userid from tbl_usermaster where status=1 and userid!='admin'");
                }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lst_Users.Add(dt.Rows[i]["userid"].ToString());
            }
            Label lbl_CurrentDate = new Label();
            lbl_CurrentDate.Text = "Rating For" + DateTime.Today.ToString("dd/MM/yyyy");
            lbl_CurrentDate.CssClass = "alert alert-info";
            foreach (string User in lst_Users)
            {
                string userid = User;
                string username = objData.GetSingleData("select username from tbl_usermaster where userid='" + User + "'");
                Label lbl_Employee = new Label();
                lbl_Employee.ID = "lbl_Emp_" + userid;
                lbl_Employee.Text = username + " - " + userid;
                lbl_Employee.Style.Add("float", "left");
                lbl_Employee.Style.Add("font-weight", "bold");
                lbl_Employee.CssClass = "label-title";
                RadioButtonList rbl_Status = new RadioButtonList();
                rbl_Status.ID = "rbl_Status_" + userid;
                rbl_Status.Style.Add("float", "left");
                rbl_Status.RepeatDirection = RepeatDirection.Horizontal;
                rbl_Status.Items.Clear();
                var item2 = new ListItem("1", "1");
                rbl_Status.Items.Add(item2);
                var item3 = new ListItem("2", "2");
                rbl_Status.Items.Add(item3);
                var item4 = new ListItem("3", "3");
                rbl_Status.Items.Add(item4);
                var item5 = new ListItem("4", "4");
                rbl_Status.Items.Add(item5);
                var item6 = new ListItem("5", "5");
                rbl_Status.Items.Add(item6);
                var item7 = new ListItem("A", "Absent");
                rbl_Status.Items.Add(item7);
                Label lbl_RatingDesc = new Label();
                lbl_RatingDesc.Text = "Description";
                lbl_RatingDesc.Style.Add("float", "left");
                lbl_RatingDesc.Style.Add("font-weight", "bold");
                lbl_RatingDesc.CssClass = "label-title";
                lbl_RatingDesc.ID = "txt_RatingDesc_" + userid;
                TextBox txt_RatingDesc = new TextBox();
                txt_RatingDesc.TextMode = TextBoxMode.MultiLine;
                txt_RatingDesc.CssClass = "form-control";
                pnl_RadioButtonList.Controls.Add(lbl_Employee);
                pnl_RadioButtonList.Controls.Add(rbl_Status);
                pnl_RadioButtonList.Controls.Add(lbl_RatingDesc);
                pnl_RadioButtonList.Controls.Add(txt_RatingDesc);
                pnl_RadioButtonList.Controls.Add(new LiteralControl("<br />"));
                pnl_RadioButtonList.Controls.Add(new LiteralControl("<br />"));
            }
        }
    }
    /// <summary>
    /// Update Rating For Current Date
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void btn_UpdateRating_Click(object sender, EventArgs e)
    {
        string SelectedType = rbl_RatingType.SelectedValue;
        InsertRecord();
        block_History.Visible = true;
        block_NewRating.Visible = false;
        bindGrid();
        pnl_RadioButtonList.Controls.Clear();
        rbl_RatingType.SelectedValue = SelectedType;
        ScriptManager.RegisterStartupScript(this, GetType(), "mismatch", "swal('Rating Updated Successfully','','success');", true);
    }
    /// <summary>
    /// Inserts the record in Database.
    /// </summary>
    void InsertRecord()
    {
        RatingType = rbl_RatingType.SelectedValue.ToString();
        DateTime convertedDateOfRating = DateTime.ParseExact(txt_DateOfRating.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        string DateOfRating = convertedDateOfRating.ToString("yyyy/MM/dd");
        string CurrentTeamLeader = string.Empty;
        string Rights = Convert.ToString(Session["Rights"]);
        DataTable dt_Users = new DataTable();
        List<string> lst_Users = new List<string>();
        List<string> lst_Values = new List<string>();
        List<string> lst_RatingDesc = new List<string>();
        Dictionary<List<string>, List<string>> dct_ListValues = new Dictionary<List<string>, List<string>>();
        Dictionary<string, string> dct_StringValues = new Dictionary<string, string>();
        Dictionary<string, string> dct_StringRatingDesc = new Dictionary<string, string>();
        string gradeSelected = string.Empty;
        string gradeDescription = string.Empty;
        string value = string.Empty;
        string value2 = string.Empty;
        if (Rights == "Team Leader")
        {
            CurrentTeamLeader = Convert.ToString(Session["Userid"]);
            string TeamUsers = objData.GetSingleData("select replace(''''+userid+'''',',',''',''') from tbl_teamAllotmentMaster where teamleader like ('%" + CurrentTeamLeader + "%')");
            dt_Users = objData.Getdata("select userid from tbl_usermaster where userid in(" + TeamUsers + ") and status=1");
            for (int j = 0; j < dt_Users.Rows.Count; j++)
            {
                lst_Users.Add(Convert.ToString(dt_Users.Rows[j]["userid"]));
            }
        }
        else if (Rights == "Administrator")
        {
            dt_Users = objData.Getdata("select userid from tbl_usermaster where status=1 and userid not in('admin')");
            for (int j = 0; j < dt_Users.Rows.Count; j++)
            {
                lst_Users.Add(Convert.ToString(dt_Users.Rows[j]["userid"]));
            }
        }
        foreach (Control ctr in pnl_RadioButtonList.Controls)
        {
            if (ctr is RadioButtonList)
            {
                value = ((RadioButtonList)ctr).SelectedValue;
                lst_Values.Add(value);
            }
            if (ctr is TextBox && ctr.ID != "txt_DateOfRating" && ctr.ID != "rbl_RatingType")
            {
                value2 = ((TextBox)ctr).Text;
                lst_RatingDesc.Add(value2);
            }
        }

        //Zip Method To Zip Users And Values
        var UsersAndValues = lst_Users.Zip(lst_Values, (U, V) => new { Users = U, Values = V });
        foreach (var UV in UsersAndValues)
        {
            dct_StringValues.Add(UV.Users, UV.Values);
        }

        //Zip Method To Zip Users And Rating Description 
        var UsersAndDesc = lst_Users.Zip(lst_RatingDesc, (U, V) => new { Users = U, Rating = V });
        foreach (var UV in UsersAndDesc)
        {
            dct_StringRatingDesc.Add(UV.Users, UV.Rating);
        }

        string StringToDebug = string.Empty;

        foreach (KeyValuePair<string, string> kvp in dct_StringValues)
        {
            StringToDebug += string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
        }
        foreach (string Users in lst_Users)
        {
            gradeSelected = dct_StringValues[Users];
            gradeDescription = dct_StringRatingDesc[Users];
            string Date = DateTime.Today.ToString("dd/MM/yyyy");
            string GradeBy = Convert.ToString(Session["Userid"]);
            string UserName = objData.GetSingleData("select username from tbl_usermaster where userid='" + Users + "'");
            if (gradeSelected != "")
            {
                com_check.Connection = con;
                con.Close();
                con.Open();
                if (RatingType == "Coding Standard Rating")
                {
                    string ifExists = objData.GetSingleData("select userid from tbl_CodingStandardRating where userid='" + Users + "' and insertdate='" + convertedDateOfRating + "'");
                    if (ifExists == "0")
                    {
                        com_check.CommandText = "insert into tbl_CodingStandardRating(userid,username,gradeSelected,Description,gradeBy,insertDate) values('" + Users + "','" + UserName + "','" + gradeSelected + "','" + gradeDescription + "','" + GradeBy + "','" + convertedDateOfRating + "')";
                    }
                    else
                    {
                        com_check.CommandText = "update tbl_CodingStandardRating set userid='" + Users + "',username='" + UserName + "',gradeSelected='" + gradeSelected + "',Description='" + gradeDescription + "',gradeBy='" + GradeBy + "',insertDate='" + convertedDateOfRating + "' where userid='" + Users + "' and insertdate='" + convertedDateOfRating + "'";
                    }
                }
                else if (RatingType == "Quality Rating")
                {
                    string ifExists = objData.GetSingleData("select userid from tbl_QualityRating where userid='" + Users + "' and insertdate='" + convertedDateOfRating + "'");
                    if (ifExists == "0")
                    {
                        com_check.CommandText = "insert into tbl_QualityRating(userid,username,gradeSelected,Description,gradeBy,insertDate) values('" + Users + "','" + UserName + "','" + gradeSelected + "','" + gradeDescription + "','" + GradeBy + "','" + convertedDateOfRating + "')";
                    }
                    else
                    {
                        com_check.CommandText = "update tbl_QualityRating set userid='" + Users + "',username='" + UserName + "',gradeSelected='" + gradeSelected + "',Description='" + gradeDescription + "',gradeBy='" + GradeBy + "',insertDate='" + convertedDateOfRating + "' where userid='" + Users + "' and insertdate='" + convertedDateOfRating + "'";
                    }
                }
                com_check.ExecuteNonQuery();
            }
        }
    }
    /// <summary>
    /// Redirects to History Of Ratings Page
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void btn_BackToHistory_Click(object sender, EventArgs e)
    {
        block_History.Visible = true;
        block_NewRating.Visible = false;
        TitleOfPage.InnerText = "Coding Standard / Quality Rating (Applicable Since 16th April 2016)";
        bindGrid();
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
        ddl_EmployeeID.SelectedIndex = 0;
        bindGrid();
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
    /// Export To Excel Function
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void btn_Export_Click(object sender, EventArgs e)
    {
        try
        {
            List<string> ExcelReport = new List<string>();
            SqlDataAdapter da = new SqlDataAdapter();
            string CurrentDate = DateTime.Now.ToString();
            string Rights = Convert.ToString(Session["Rights"]);
            DateTime convertedStartTime = DateTime.ParseExact(txt_FromDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime convertedEndTime = DateTime.ParseExact(txt_ToDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string dates = "Track.date between '" + convertedStartTime.ToString("yyyy-MM-dd") + "' and '" + convertedEndTime.ToString("yyyy-MM-dd") + "'";
            con.Close();
            da = new SqlDataAdapter("select convert(varchar(10),convert(date,Track.date),103),Track.Project,Track.Task,Track.username,Track.gradeSelected,Track.gradeby,Track.RatingType from (select a.id,(select c.projectreq from tbl_ProjectReq c where c.projectid=a.projectid)[Project],a.taskname[Task],a.userid,isNull(convert(varchar(10),a.RatingOn),'NA')[date],(select b.userid +' - ' +b.username from tbl_usermaster b where b.userid=a.userid)[username],a.RatingOfTask[gradeSelected],(select b.userid+ ' - ' +b.username from tbl_usermaster b where a.RatingBy=b.userid)[gradeby],'Coding Standard'[RatingType] from tbl_Taskmaster a union select a.id,(select c.projectreq from tbl_ProjectReq c where c.projectid=a.projectid)[Project],a.taskname[Task],a.userid,isNull(convert(varchar(10),a.QualityRatingOn),'NA')[date],(select b.userid +' - ' +b.username from tbl_usermaster b where b.userid=a.userid)[username],a.QualityRating[gradeSelected],(select b.userid+ ' - ' +b.username from tbl_usermaster b where a.QualityRatingBy=b.userid)[gradeby],'Quality'[RatingType] from tbl_Taskmaster a )Track where Track.gradeby is not null and " + dates + " and Track.userid='" + Session["Userid"] + "' order by Track.RatingType asc,Track.date desc", con);
            //da = new SqlDataAdapter("select id,insertdate[date],userid+' - '+username[username],gradeSelected,Description,(select b.username from tbl_usermaster b where a.gradeby=b.userid)gradeby from tbl_CodingStandardRating a where " + dates + " and userid='" + Session["Userid"].ToString() + "' order by insertDate desc", con);
            if (Rights == "Administrator")
            {
                da = new SqlDataAdapter("select convert(varchar(10),convert(date,Track.date),103),Track.Project,Track.Task,Track.username,Track.gradeSelected,Track.gradeby,Track.RatingType from (select a.id,(select c.projectreq from tbl_ProjectReq c where c.projectid=a.projectid)[Project],a.taskname[Task],a.userid,isNull(convert(varchar(10),a.RatingOn),'NA')[date],(select b.userid +' - ' +b.username from tbl_usermaster b where b.userid=a.userid)[username],a.RatingOfTask[gradeSelected],(select b.userid+ ' - ' +b.username from tbl_usermaster b where a.RatingBy=b.userid)[gradeby],'Coding Standard'[RatingType] from tbl_Taskmaster a union select a.id,(select c.projectreq from tbl_ProjectReq c where c.projectid=a.projectid)[Project],a.taskname[Task],a.userid,isNull(convert(varchar(10),a.QualityRatingOn),'NA')[date],(select b.userid +' - ' +b.username from tbl_usermaster b where b.userid=a.userid)[username],a.QualityRating[gradeSelected],(select b.userid+ ' - ' +b.username from tbl_usermaster b where a.QualityRatingBy=b.userid)[gradeby],'Quality'[RatingType] from tbl_Taskmaster a )Track where Track.gradeby is not null and " + dates + " order by Track.RatingType asc,Track.date desc", con);
                //da = new SqlDataAdapter("select id,insertdate[date],userid+' - '+username[username],gradeSelected,Description,(select b.username from tbl_usermaster b where a.gradeby=b.userid)gradeby from tbl_CodingStandardRating a where " + dates + " order by insertDate desc", con);
                if (ddl_EmployeeID.SelectedIndex != 0)
                {
                    da = new SqlDataAdapter("select convert(varchar(10),convert(date,Track.date),103),Track.Project,Track.Task,Track.username,Track.gradeSelected,Track.gradeby,Track.RatingType from (select a.id,(select c.projectreq from tbl_ProjectReq c where c.projectid=a.projectid)[Project],a.taskname[Task],a.userid,isNull(convert(varchar(10),a.RatingOn),'NA')[date],(select b.userid +' - ' +b.username from tbl_usermaster b where b.userid=a.userid)[username],a.RatingOfTask[gradeSelected],(select b.userid+ ' - ' +b.username from tbl_usermaster b where a.RatingBy=b.userid)[gradeby],'Coding Standard'[RatingType] from tbl_Taskmaster a union select a.id,(select c.projectreq from tbl_ProjectReq c where c.projectid=a.projectid)[Project],a.taskname[Task],a.userid,isNull(convert(varchar(10),a.QualityRatingOn),'NA')[date],(select b.userid +' - ' +b.username from tbl_usermaster b where b.userid=a.userid)[username],a.QualityRating[gradeSelected],(select b.userid+ ' - ' +b.username from tbl_usermaster b where a.QualityRatingBy=b.userid)[gradeby],'Quality'[RatingType] from tbl_Taskmaster a )Track where Track.gradeby is not null and " + dates + " and Track.userid='" + ddl_EmployeeID.SelectedValue.ToString() + "' order by Track.RatingType asc,Track.date desc", con);
                    //da = new SqlDataAdapter("select id,insertdate[date],userid+' - '+username[username],gradeSelected,Description,(select b.username from tbl_usermaster b where a.gradeby=b.userid)gradeby from tbl_CodingStandardRating a where " + dates + " and userid='" + ddl_EmployeeID.SelectedValue.ToString() + "' order by insertDate desc", con);
                }
            }
            else if (Rights == "Team Leader")
            {
                string TeamUsers = objData.GetSingleData("select replace(''''+userid+'''',',',''',''') from tbl_teamAllotmentMaster where teamleader like ('%" + Session["Userid"].ToString() + "%')");
                da = new SqlDataAdapter("select convert(varchar(10),convert(date,Track.date),103),Track.Project,Track.Task,Track.username,Track.gradeSelected,Track.gradeby,Track.RatingType from (select a.id,(select c.projectreq from tbl_ProjectReq c where c.projectid=a.projectid)[Project],a.taskname[Task],a.userid,isNull(convert(varchar(10),a.RatingOn),'NA')[date],(select b.userid +' - ' +b.username from tbl_usermaster b where b.userid=a.userid)[username],a.RatingOfTask[gradeSelected],(select b.userid+ ' - ' +b.username from tbl_usermaster b where a.RatingBy=b.userid)[gradeby],'Coding Standard'[RatingType] from tbl_Taskmaster a union select a.id,(select c.projectreq from tbl_ProjectReq c where c.projectid=a.projectid)[Project],a.taskname[Task],a.userid,isNull(convert(varchar(10),a.QualityRatingOn),'NA')[date],(select b.userid +' - ' +b.username from tbl_usermaster b where b.userid=a.userid)[username],a.QualityRating[gradeSelected],(select b.userid+ ' - ' +b.username from tbl_usermaster b where a.QualityRatingBy=b.userid)[gradeby],'Quality'[RatingType] from tbl_Taskmaster a )Track where Track.gradeby is not null and " + dates + " and Track.userid in(" + TeamUsers + ")  order by Track.RatingType asc,Track.date desc", con);
                //da = new SqlDataAdapter("select id,insertdate[date],userid+' - '+username[username],gradeSelected,Description,(select b.username from tbl_usermaster b where a.gradeby=b.userid)gradeby from tbl_CodingStandardRating a where userid in(" + TeamUsers + ") and " + dates + " order by insertDate desc", con);
                if (ddl_EmployeeID.SelectedIndex != 0)
                {
                    da = new SqlDataAdapter("select convert(varchar(10),convert(date,Track.date),103),Track.Project,Track.Task,Track.username,Track.gradeSelected,Track.gradeby,Track.RatingType from (select a.id,(select c.projectreq from tbl_ProjectReq c where c.projectid=a.projectid)[Project],a.taskname[Task],a.userid,isNull(convert(varchar(10),a.RatingOn),'NA')[date],(select b.userid +' - ' +b.username from tbl_usermaster b where b.userid=a.userid)[username],a.RatingOfTask[gradeSelected],(select b.userid+ ' - ' +b.username from tbl_usermaster b where a.RatingBy=b.userid)[gradeby],'Coding Standard'[RatingType] from tbl_Taskmaster a union select a.id,(select c.projectreq from tbl_ProjectReq c where c.projectid=a.projectid)[Project],a.taskname[Task],a.userid,isNull(convert(varchar(10),a.QualityRatingOn),'NA')[date],(select b.userid +' - ' +b.username from tbl_usermaster b where b.userid=a.userid)[username],a.QualityRating[gradeSelected],(select b.userid+ ' - ' +b.username from tbl_usermaster b where a.QualityRatingBy=b.userid)[gradeby],'Quality'[RatingType] from tbl_Taskmaster a )Track where Track.gradeby is not null and " + dates + " and Track.userid='" + ddl_EmployeeID.SelectedValue.ToString() + "' order by Track.RatingType asc,Track.date desc", con);
                    //da = new SqlDataAdapter("select id,insertdate[date],userid+' - '+username[username],gradeSelected,Description,(select b.username from tbl_usermaster b where a.gradeby=b.userid)gradeby from tbl_CodingStandardRating a where " + dates + " and userid='" + ddl_EmployeeID.SelectedValue.ToString() + "' order by insertDate desc", con);
                }
            }
            DataTable dts = new DataTable();
            if (Rights == "Administrator")
            {
                objData.DynamicParameters.Add("@StartDate", convertedStartTime.ToString("yyyy-MM-dd"));
                objData.DynamicParameters.Add("@EndDate", convertedEndTime.ToString("yyyy-MM-dd"));
                dts = objData.GetDetails("SP_ProductionReport", true, true);
            }
            else
            {
                da.Fill(dts);
            }
            if (dts.Rows.Count > 0)
            {
                List<string> HeaderNames = new List<string>();
                HeaderNames.Add("Date");
                HeaderNames.Add("Project");
                HeaderNames.Add("Task");
                HeaderNames.Add("Task By");
                HeaderNames.Add("Grade");
                HeaderNames.Add("Grade Given By");
                HeaderNames.Add("Rating For");
                if (Rights == "Administrator")
                {
                    HeaderNames.Clear();
                    HeaderNames.Add("Employee");
                    HeaderNames.Add("Productivity in %");
                    HeaderNames.Add("Coding standard rating in %");
                    HeaderNames.Add("Quality rating in %");
                }
                ExcelReport = pcData.generateExcelReport(dts, "RatingReport", "GenericReports", "Coding Standard / Quality Rating Report", Rights == "Administrator" ? 4 : 7, HeaderNames);
                FileInfo file = new FileInfo(ExcelReport[2]);
                Response.Clear();
                Response.AddHeader("Content-Length", file.Length.ToString(CultureInfo.InvariantCulture));
                Response.AddHeader("Content-Disposition", "attachment;filename=\"" + ("RatingReport.xls") + "\"");
                Response.ContentType = "application/octet-stream";
                Response.Flush();
                Response.TransmitFile(ExcelReport[0] + ("RatingReport" + DateTime.Now.ToShortDateString().Replace("/", "") + ".xls"));
                Response.End();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "mismatch", "swal('No Records Found','','warning');", true);
            }
            btn_Report_Click(null, null);
        }

        catch (Exception ex)
        {

        }
    }
    /// <summary>
    /// Report Button Click function to call gridview bind
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void btn_Report_Click(object sender, EventArgs e)
    {
        try
        {
            bindGrid();
        }
        catch (Exception ex)
        {
        }
    }

    /// <summary>
    /// Binds the gridview.
    /// </summary>
    protected void bindGrid()
    {
        try
        {
            SqlDataAdapter da = new SqlDataAdapter();
            RatingType = rbl_RatingType.SelectedValue.ToString();
            string CurrentDate = DateTime.Now.ToString();
            string Rights = Convert.ToString(Session["Rights"]);
            DateTime convertedStartTime = DateTime.ParseExact(txt_FromDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime convertedEndTime = DateTime.ParseExact(txt_ToDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string dates = "Track.date between '" + convertedStartTime.ToString("yyyy-MM-dd") + "' and '" + convertedEndTime.ToString("yyyy-MM-dd") + "'";
            con.Close();
            da = new SqlDataAdapter("select Track.id,convert(varchar(10),convert(date,Track.date),103)[date],Track.Project,Track.Task,Track.username,Track.gradeSelected,Track.gradeby,Track.RatingType from (select a.id,(select c.projectreq from tbl_ProjectReq c where c.projectid=a.projectid)[Project],a.taskname[Task],a.userid,isNull(convert(varchar(10),a.RatingOn),'NA')[date],(select b.userid +' - ' +b.username from tbl_usermaster b where b.userid=a.userid)[username],a.RatingOfTask[gradeSelected],(select b.userid+ ' - ' +b.username from tbl_usermaster b where a.RatingBy=b.userid)[gradeby],'Coding Standard'[RatingType] from tbl_Taskmaster a union select a.id,(select c.projectreq from tbl_ProjectReq c where c.projectid=a.projectid)[Project],a.taskname[Task],a.userid,isNull(convert(varchar(10),a.QualityRatingOn),'NA')[date],(select b.userid +' - ' +b.username from tbl_usermaster b where b.userid=a.userid)[username],a.QualityRating[gradeSelected],(select b.userid+ ' - ' +b.username from tbl_usermaster b where a.QualityRatingBy=b.userid)[gradeby],'Quality'[RatingType] from tbl_Taskmaster a )Track where Track.gradeby is not null and " + dates + " and Track.userid='" + Session["Userid"] + "' order by Track.RatingType asc,Track.date desc", con);
            //da = new SqlDataAdapter("select id,insertdate[date],userid+' - '+username[username],gradeSelected,Description,(select b.username from tbl_usermaster b where a.gradeby=b.userid)gradeby from tbl_CodingStandardRating a where " + dates + " and userid='" + Session["Userid"].ToString() + "' order by insertDate desc", con);
            if (Rights == "Administrator")
            {
                da = new SqlDataAdapter("select Track.id,convert(varchar(10),convert(date,Track.date),103)[date],Track.Project,Track.Task,Track.username,Track.gradeSelected,Track.gradeby,Track.RatingType from (select a.id,(select c.projectreq from tbl_ProjectReq c where c.projectid=a.projectid)[Project],a.taskname[Task],a.userid,isNull(convert(varchar(10),a.RatingOn),'NA')[date],(select b.userid +' - ' +b.username from tbl_usermaster b where b.userid=a.userid)[username],a.RatingOfTask[gradeSelected],(select b.userid+ ' - ' +b.username from tbl_usermaster b where a.RatingBy=b.userid)[gradeby],'Coding Standard'[RatingType] from tbl_Taskmaster a union select a.id,(select c.projectreq from tbl_ProjectReq c where c.projectid=a.projectid)[Project],a.taskname[Task],a.userid,isNull(convert(varchar(10),a.QualityRatingOn),'NA')[date],(select b.userid +' - ' +b.username from tbl_usermaster b where b.userid=a.userid)[username],a.QualityRating[gradeSelected],(select b.userid+ ' - ' +b.username from tbl_usermaster b where a.QualityRatingBy=b.userid)[gradeby],'Quality'[RatingType] from tbl_Taskmaster a )Track where Track.gradeby is not null and " + dates + " order by Track.RatingType asc,Track.date desc", con);
                //da = new SqlDataAdapter("select id,insertdate[date],userid+' - '+username[username],gradeSelected,Description,(select b.username from tbl_usermaster b where a.gradeby=b.userid)gradeby from tbl_CodingStandardRating a where " + dates + " order by insertDate desc", con);
                if (ddl_EmployeeID.SelectedIndex != 0)
                {
                    da = new SqlDataAdapter("select Track.id,convert(varchar(10),convert(date,Track.date),103)[date],Track.Project,Track.Task,Track.username,Track.gradeSelected,Track.gradeby,Track.RatingType from (select a.id,(select c.projectreq from tbl_ProjectReq c where c.projectid=a.projectid)[Project],a.taskname[Task],a.userid,isNull(convert(varchar(10),a.RatingOn),'NA')[date],(select b.userid +' - ' +b.username from tbl_usermaster b where b.userid=a.userid)[username],a.RatingOfTask[gradeSelected],(select b.userid+ ' - ' +b.username from tbl_usermaster b where a.RatingBy=b.userid)[gradeby],'Coding Standard'[RatingType] from tbl_Taskmaster a union select a.id,(select c.projectreq from tbl_ProjectReq c where c.projectid=a.projectid)[Project],a.taskname[Task],a.userid,isNull(convert(varchar(10),a.QualityRatingOn),'NA')[date],(select b.userid +' - ' +b.username from tbl_usermaster b where b.userid=a.userid)[username],a.QualityRating[gradeSelected],(select b.userid+ ' - ' +b.username from tbl_usermaster b where a.QualityRatingBy=b.userid)[gradeby],'Quality'[RatingType] from tbl_Taskmaster a )Track where Track.gradeby is not null and " + dates + " and Track.userid='" + ddl_EmployeeID.SelectedValue.ToString() + "' order by Track.RatingType asc,Track.date desc", con);
                    //da = new SqlDataAdapter("select id,insertdate[date],userid+' - '+username[username],gradeSelected,Description,(select b.username from tbl_usermaster b where a.gradeby=b.userid)gradeby from tbl_CodingStandardRating a where " + dates + " and userid='" + ddl_EmployeeID.SelectedValue.ToString() + "' order by insertDate desc", con);
                }
            }
            else if (Rights == "Team Leader")
            {
                string TeamUsers = objData.GetSingleData("select replace(''''+userid+'''',',',''',''') from tbl_teamAllotmentMaster where teamleader like ('%" + Session["Userid"].ToString() + "%')");
                da = new SqlDataAdapter("select Track.id,convert(varchar(10),convert(date,Track.date),103)[date],Track.Project,Track.Task,Track.username,Track.gradeSelected,Track.gradeby,Track.RatingType from (select a.id,(select c.projectreq from tbl_ProjectReq c where c.projectid=a.projectid)[Project],a.taskname[Task],a.userid,isNull(convert(varchar(10),a.RatingOn),'NA')[date],(select b.userid +' - ' +b.username from tbl_usermaster b where b.userid=a.userid)[username],a.RatingOfTask[gradeSelected],(select b.userid+ ' - ' +b.username from tbl_usermaster b where a.RatingBy=b.userid)[gradeby],'Coding Standard'[RatingType] from tbl_Taskmaster a union select a.id,(select c.projectreq from tbl_ProjectReq c where c.projectid=a.projectid)[Project],a.taskname[Task],a.userid,isNull(convert(varchar(10),a.QualityRatingOn),'NA')[date],(select b.userid +' - ' +b.username from tbl_usermaster b where b.userid=a.userid)[username],a.QualityRating[gradeSelected],(select b.userid+ ' - ' +b.username from tbl_usermaster b where a.QualityRatingBy=b.userid)[gradeby],'Quality'[RatingType] from tbl_Taskmaster a )Track where Track.gradeby is not null and " + dates + " and Track.userid in(" + TeamUsers + ")  order by Track.RatingType asc,Track.date desc", con);
                //da = new SqlDataAdapter("select id,insertdate[date],userid+' - '+username[username],gradeSelected,Description,(select b.username from tbl_usermaster b where a.gradeby=b.userid)gradeby from tbl_CodingStandardRating a where userid in(" + TeamUsers + ") and " + dates + " order by insertDate desc", con);
                if (ddl_EmployeeID.SelectedIndex != 0)
                {
                    da = new SqlDataAdapter("select Track.id,convert(varchar(10),convert(date,Track.date),103)[date],Track.Project,Track.Task,Track.username,Track.gradeSelected,Track.gradeby,Track.RatingType from (select a.id,(select c.projectreq from tbl_ProjectReq c where c.projectid=a.projectid)[Project],a.taskname[Task],a.userid,isNull(convert(varchar(10),a.RatingOn),'NA')[date],(select b.userid +' - ' +b.username from tbl_usermaster b where b.userid=a.userid)[username],a.RatingOfTask[gradeSelected],(select b.userid+ ' - ' +b.username from tbl_usermaster b where a.RatingBy=b.userid)[gradeby],'Coding Standard'[RatingType] from tbl_Taskmaster a union select a.id,(select c.projectreq from tbl_ProjectReq c where c.projectid=a.projectid)[Project],a.taskname[Task],a.userid,isNull(convert(varchar(10),a.QualityRatingOn),'NA')[date],(select b.userid +' - ' +b.username from tbl_usermaster b where b.userid=a.userid)[username],a.QualityRating[gradeSelected],(select b.userid+ ' - ' +b.username from tbl_usermaster b where a.QualityRatingBy=b.userid)[gradeby],'Quality'[RatingType] from tbl_Taskmaster a )Track where Track.gradeby is not null and " + dates + " and Track.userid='" + ddl_EmployeeID.SelectedValue.ToString() + "' order by Track.RatingType asc,Track.date desc", con);
                    //da = new SqlDataAdapter("select id,insertdate[date],userid+' - '+username[username],gradeSelected,Description,(select b.username from tbl_usermaster b where a.gradeby=b.userid)gradeby from tbl_CodingStandardRating a where " + dates + " and userid='" + ddl_EmployeeID.SelectedValue.ToString() + "' order by insertDate desc", con);
                }
            }
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                grd_CodingRating.DataSource = ds;
                grd_CodingRating.DataBind();
            }
            else
            {
                ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                grd_CodingRating.DataSource = ds;
                grd_CodingRating.DataBind();
                int columncount = grd_CodingRating.Rows[0].Cells.Count;
                grd_CodingRating.Rows[0].Cells.Clear();
                grd_CodingRating.Rows[0].Cells.Add(new TableCell());
                grd_CodingRating.Rows[0].Cells[0].ColumnSpan = columncount;
                grd_CodingRating.Rows[0].Cells[0].Text = "<center>----- No Records Found -----</center>";
            }
        }
        catch (Exception)
        {
        }
    }
    /// <summary>
    /// From Date Textbox Change Event
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void txt_FromDate_TextChanged(object sender, EventArgs e)
    {
        txt_ToDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
        string ToDateLimitStart = Convert.ToString(DateTime.ParseExact(txt_FromDate.Text.Trim(), "dd/MM/yyyy", null).ToString("MM/dd/yyyy"));
        calext_ToDate.StartDate = DateTime.ParseExact(ToDateLimitStart, "MM/dd/yyyy", CultureInfo.InvariantCulture);
        //Convert.ToDateTime();
        bindGrid();
    }

    /// <summary>
    /// To Date Textbox Change Event
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void txt_ToDate_TextChanged(object sender, EventArgs e)
    {
        bindGrid();
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
    /// <summary>
    /// Ddl_employeeId_selected index changed event to bind reports for selected employee.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void ddl_EmployeeID_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindGrid();
    }
}