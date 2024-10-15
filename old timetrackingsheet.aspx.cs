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
using System.Globalization;
using System.Linq;
using System.Collections.Generic;

public partial class TimeTrackingSheet : System.Web.UI.Page
{
    string str_Command, TaskID;
    string EmpName, UserID, UserName;
    DateTime StartDate, EndDate;
    int inserVal = 1;
    SqlCommand com = new SqlCommand();
    SqlCommand com_DPR = new SqlCommand();
    SqlCommand com_GetUsers = new SqlCommand();
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString());
    SqlConnection con2 = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString());
    clsDataControl dataCls = new clsDataControl();
    int nooferr, nooferr1, break1, pageloadset;
    string Todaydate, EmpRights, smc;

    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            txt_StartTime.Attributes.Add("onkeyup", "no_alpha()");
            if (Session["Rights"].ToString() != "Team Leader" && Session["Rights"].ToString() != "Administrator")
            {
                //txt_Date_CalendarExtender.StartDate = DateTime.Now.AddDays(-2);
            }
            txt_Date_CalendarExtender.EndDate = DateTime.Now;
            txt_Date.Attributes.Add("readonly", "readonly");
            rbl_BreakTime.Attributes.Add("readonly", "readonly");
            this.Page.Title = "PMIS Time Tracking Sheet";
            //ScriptManager.GetCurrent(this).RegisterPostBackControl(btn_excelView);
            if (Session["Userid"] == null)
            {
                Response.Redirect("Login.aspx");
            }
            else if (Session["Userid"].ToString() == "")
            {
                Response.Redirect("Login.aspx");
            }
            lbl_Error.Visible = false;
            if (pageloadset == 1) clear();
            Page.MaintainScrollPositionOnPostBack = true;
            if (!Page.IsPostBack)
            {
                clear();
                btn_CalcTime.Visible = false;
                txt_Date.Text = DateTime.Today.ToString("dd/MM/yyyy");
                //txt_StartTime_MaskedEditExtender.MaskType = AjaxControlToolkit.MaskedEditType.DateTime;
                //MaskedEditExtender2.MaskType = AjaxControlToolkit.MaskedEditType.DateTime;
                string Rights = Convert.ToString(Session["Rights"]);

                if (Rights == "Administrator" || Rights == "Team Leader")
                {
                    btn_SaveStatus.Enabled = false;
                    ddl_EmpNo.Focus();
                    txt_StartTime.Attributes.Remove("readonly");
                    txt_EndTime.Attributes.Remove("readonly");
                    txt_StartTime.Enabled = true;
                    txt_EmpNo.Visible = false;
                    ddl_EmpNo.Enabled = true;
                    //txt_StartTime_MaskedEditExtender.AutoComplete = false;
                    //MaskedEditExtender2.AutoComplete = false;
                }
                if (Rights == "User")
                {
                    span_EmpNo.Visible = false;
                    ddl_EmpNo.Enabled = false;
                    txt_EmpNo.Visible = true;
                    ddl_EmpNo.Visible = false;
                    btn_Edit.Visible = false;
                    bindProjects();
                }
                btn_Edit.Visible = false;

                if ((Session["Rights"].ToString() != "Administrator") && (Session["Rights"].ToString() != "Team Leader") && (Convert.ToString(Session["Userid"]) != "R138"))
                    ddl_EmpNo.Enabled = false;
                txt_EmpNo_TextChanged(sender, e);
                EmpName = txt_EmpNo.Text;
                dpr_gridbind();
                pageloadset = 0;
                Loaddetails();

                if (ddl_EmpNo.Enabled == true)
                {
                    bindEmployees();
                }
                else
                {
                    bindEmployeeName();
                }
                if (Session["TaskID"] != "")
                {
                    btn_SaveStatus.Enabled = true;
                    bindTaskData();
                    Session["TaskID"] = "";
                }
            }

        }
        catch (Exception)
        {
        }

    }
    protected void bindEmployeeName()
    {
        try
        {
            string Rights = Session["Rights"].ToString();
            string userid = ddl_EmpNo.SelectedValue.ToString();
            if (txt_EmpNo.Text == "")
            {
                txt_EmpNo.Text = ddl_EmpNo.SelectedValue;
            }
            if (Rights != "Administrator" && Rights != "Team Leader")
            {
                com.Connection = con;
                con.Open();
                com.CommandText = "select username from tbl_usermaster where userid='" + txt_EmpNo.Text + "'";
            }
            else
            {
                com.Connection = con;
                con.Open();
                com.CommandText = "select username from tbl_usermaster where userid='" + ddl_EmpNo.SelectedValue + "'";
            }
            con.Close();
            con.Open();
            SqlDataReader dr = com.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    hdn_EmpName.Value = dr.GetString(0);
                }
            }
        }
        catch (Exception)
        {

        }
    }

    protected void bindEmployees()
    {
        try
        {
            string Rights = Convert.ToString(Session["Rights"]);
            com_GetUsers.Connection = con;
            con.Close();
            con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("select distinct(a.userid)+ ' | ' + a.username as Username,a.username as name,a.userid from tbl_usermaster a where status=1 and Deluser!=1 order by userid asc", con); ;
            DataSet ds = new DataSet();
            sda.Fill(ds);
            ddl_EmpNo.DataSource = ds;
            ddl_EmpNo.DataTextField = "Username";
            ddl_EmpNo.DataValueField = "userid";
            ddl_EmpNo.DataBind();
            ddl_EmpNo.Items.Insert(0, new ListItem("Select", "N/A"));

            con.Close();
            if (Rights == "Team Leader")
            {
                com_GetUsers.Connection = con;
                con.Close();
                con.Open();
                com_GetUsers.CommandText = "select TOP 1 UserID,Username from tbl_teamAllotmentMaster where TeamID in(select TeamID from tbl_teamAllotmentMaster where teamleader like ('%" + Session["Userid"] + "%')) order by InsertedDate,UpdatedDate desc";
                SqlDataReader dr = com_GetUsers.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    UserID = dr[0].ToString();
                    UserName = dr[1].ToString();
                    string[] splitteduserId = UserID.Split(',');
                    string[] splitteduserName = UserName.Split(',');
                    Dictionary<string, string> users = new Dictionary<string, string>();
                    for (int i = 0; i < splitteduserId.Length; i++)
                    {
                        users.Add(splitteduserId[i], splitteduserId[i] + " - " + splitteduserName[i]);
                        ddl_EmpNo.DataSource = users;
                        ddl_EmpNo.DataTextField = "Value";
                        ddl_EmpNo.DataValueField = "Key";
                        ddl_EmpNo.DataBind();
                        ddl_EmpNo.Items.Insert(0, new ListItem("Select", "N/A"));
                    }
                }
                dr.Close();
            }
        }
        catch (Exception)
        {

        }

    }
    void bindProjects()
    {

        try
        {
            if (ddl_EmpNo.Enabled == true)
            {
                DataTable dtProjects = dataCls.Getdata("select distinct(a.projectid),a.projectname,b.userid from tbl_ProjectReq a inner Join  tbl_taskmaster b on b.userid='" + ddl_EmpNo.SelectedValue + "' and b.projectid=a.projectid and b.taskstatus!='Completed' or a.projectname='No-Project' and b.userid='" + ddl_EmpNo.SelectedValue + "' where a.status=1 and b.status=1");
                ddl_ProjectID.DataSource = dtProjects;
                ddl_ProjectID.DataTextField = "Projectname";
                ddl_ProjectID.DataValueField = "projectid";
                ddl_ProjectID.DataBind();
                ddl_ProjectID.Items.Insert(0, new ListItem("Select", "N/A"));
                if (ddl_ProjectID.Items.Count == 1)
                {
                    DataTable dtProjectsDefault = dataCls.Getdata("select projectid,projectname from tbl_ProjectReq where projectname='No-Project'");
                    ddl_ProjectID.DataSource = dtProjectsDefault;
                    ddl_ProjectID.DataTextField = "Projectname";
                    ddl_ProjectID.DataValueField = "projectid";
                    ddl_ProjectID.DataBind();

                }
                con.Close();
            }
            else
            {
                hdn_EmpNo.Value = Session["Userid"].ToString();
                DataTable dtProjects = dataCls.Getdata("select distinct(a.projectid),a.projectname,b.userid from tbl_ProjectReq a inner Join  tbl_taskmaster b on b.userid='" + hdn_EmpNo.Value + "' and b.projectid=a.projectid and b.taskstatus!='Completed' or a.projectname='No-Project' and b.userid='" + hdn_EmpNo.Value + "' where a.status=1 and b.status=1");
                ddl_ProjectID.DataSource = dtProjects;
                ddl_ProjectID.DataTextField = "Projectname";
                ddl_ProjectID.DataValueField = "projectid";
                ddl_ProjectID.DataBind();
                ddl_ProjectID.Items.Insert(0, new ListItem("Select", "N/A"));
                if (ddl_ProjectID.Items.Count == 1)
                {
                    DataTable dtProjectsDefault = dataCls.Getdata("select projectid,projectname from tbl_ProjectReq where projectname='No-Project'");
                    ddl_ProjectID.DataSource = dtProjectsDefault;
                    ddl_ProjectID.DataTextField = "Projectname";
                    ddl_ProjectID.DataValueField = "projectid";
                    ddl_ProjectID.DataBind();
                    ddl_ProjectID.Items.Insert(0, new ListItem("Select", "N/A"));
                }
                con.Close();
            }
        }
        catch
        {
        }
    }
    protected void bindScope()
    {
        try
        {
            ddl_Scope.Items.Clear();
            con.Close();
            con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("select b.Scope as ScopeName,b.Scope,b.ID,a.scopeid from tbl_scope b inner join tbl_taskmaster a on a.scopeid=b.ID and a.taskstatus!='Completed' inner join tbl_MstStageMaster c on c.slno=a.stageid where a.projectid='" + ddl_ProjectID.SelectedValue + "' and a.stageid='" + ddl_Stage.SelectedValue + "' group by Description,Scope,b.ID,scopeid", con);
            if (ViewState["UpdationCompleted"].ToString() == "Y")
            {
                sda = new SqlDataAdapter("select b.Scope as ScopeName,b.Scope,b.ID,a.scopeid from tbl_scope b inner join tbl_taskmaster a on a.scopeid=b.ID inner join tbl_MstStageMaster c on c.slno=a.stageid where a.projectid='" + ddl_ProjectID.SelectedValue + "' and a.stageid='" + ddl_Stage.SelectedValue + "' group by Description,Scope,b.ID,scopeid", con);
            }
            DataSet ds = new DataSet();
            sda.Fill(ds);
            ddl_Scope.DataSource = ds;
            ddl_Scope.DataTextField = "ScopeName";
            ddl_Scope.DataValueField = "ID";
            ddl_Scope.DataBind();
            con.Close();
            bindTask();

        }
        catch (Exception)
        {


        }

        ddl_Scope.Items.Insert(0, new ListItem("Select", "N/A"));
    }
    protected void bindTask()
    {
        try
        {
            ddl_Task.Items.Clear();
            if (ddl_ProjectID.SelectedValue == "NA")
            {
                ddl_Task.Items.Insert(0, new ListItem("Select", "N/A"));
                ddl_Task.Items.Insert(1, new ListItem("Others", "-1"));
            }
            else
            {
                ddl_Task.Items.Clear();
                con.Close();
                con.Open();
                string Empno = string.Empty;
                if (ddl_EmpNo.Visible == true) { Empno = ddl_EmpNo.SelectedValue; } else if (txt_EmpNo.Visible == true) { Empno = txt_EmpNo.Text; }
                SqlDataAdapter sda = new SqlDataAdapter("select b.taskname,b.requestid,b.id from  tbl_taskmaster b  where b.taskstatus!='Completed' and b.projectid='" + ddl_ProjectID.SelectedValue + "' and b.stageid='" + ddl_Stage.SelectedValue + "' and b.scopeid='" + ddl_Scope.SelectedValue + "' and userid ='" + Empno + "'", con);
                if (ViewState["UpdationCompleted"].ToString() == "Y")
                {
                    sda = new SqlDataAdapter("select b.taskname,b.requestid,b.id from  tbl_taskmaster b  where b.projectid='" + ddl_ProjectID.SelectedValue + "' and b.stageid='" + ddl_Stage.SelectedValue + "' and b.scopeid='" + ddl_Scope.SelectedValue + "' and userid ='" + Empno + "'", con);
                }
                DataSet ds = new DataSet();
                sda.Fill(ds);
                ddl_Task.DataSource = ds;
                ddl_Task.DataTextField = "taskname";
                ddl_Task.DataValueField = "id";
                ddl_Task.DataBind();
                con.Close();
                dpr_gridbind();
            }
        }
        catch (Exception)
        {


        }
        if (ddl_ProjectID.SelectedValue != "NA")
        {
            ddl_Task.Items.Insert(0, new ListItem("Select", "N/A"));
        }
    }
    protected void bindCompletedProjects()
    {
        try
        {
            if (ddl_EmpNo.Enabled == true)
            {
                DataTable dtProjects = dataCls.Getdata("select distinct(a.projectid),a.projectname,b.userid from tbl_ProjectReq a inner Join  tbl_taskmaster b on b.userid='" + ddl_EmpNo.SelectedValue + "' and b.projectid=a.projectid or a.projectname='No-Project' and b.userid='" + ddl_EmpNo.SelectedValue + "' where a.status=1 and b.status=1");
                ddl_ProjectID.DataSource = dtProjects;
                ddl_ProjectID.DataTextField = "Projectname";
                ddl_ProjectID.DataValueField = "projectid";
                ddl_ProjectID.DataBind();
                ddl_ProjectID.Items.Insert(0, new ListItem("Select", "N/A"));
                if (ddl_ProjectID.Items.Count == 1)
                {
                    DataTable dtProjectsDefault = dataCls.Getdata("select projectid,projectname from tbl_ProjectReq where projectname='No-Project'");
                    ddl_ProjectID.DataSource = dtProjectsDefault;
                    ddl_ProjectID.DataTextField = "Projectname";
                    ddl_ProjectID.DataValueField = "projectid";
                    ddl_ProjectID.DataBind();

                }
                con.Close();

            }
            else
            {
                hdn_EmpNo.Value = Session["Userid"].ToString();
                DataTable dtProjects = dataCls.Getdata("select distinct(a.projectid),a.projectname,b.userid from tbl_ProjectReq a inner Join  tbl_taskmaster b on b.userid='" + hdn_EmpNo.Value + "' and b.projectid=a.projectid  or a.projectname='No-Project' and b.userid='" + hdn_EmpNo.Value + "' where a.status=1 and b.status=1");
                ddl_ProjectID.DataSource = dtProjects;
                ddl_ProjectID.DataTextField = "Projectname";
                ddl_ProjectID.DataValueField = "projectid";
                ddl_ProjectID.DataBind();
                ddl_ProjectID.Items.Insert(0, new ListItem("Select", "N/A"));
                if (ddl_ProjectID.Items.Count == 1)
                {
                    DataTable dtProjectsDefault = dataCls.Getdata("select projectid,projectname from tbl_ProjectReq where projectname='No-Project'");
                    ddl_ProjectID.DataSource = dtProjectsDefault;
                    ddl_ProjectID.DataTextField = "Projectname";
                    ddl_ProjectID.DataValueField = "projectid";
                    ddl_ProjectID.DataBind();
                    ddl_ProjectID.Items.Insert(0, new ListItem("Select", "N/A"));
                }
                con.Close();
            }
        }
        catch
        {
        }
    }
    public string getProjectID()
    {
        string[] splitter_Grade = ddl_ProjectID.SelectedItem.Text.Split(new Char[] { '|' });
        return splitter_Grade[0].Trim();
    }

    public void txt_EmpNo_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string UserID = (Session["Rights"].ToString() != "Administrator" && Session["Userid"].ToString() != "R138") ? Session["Userid"].ToString() : txt_EmpNo.Text;
            string sqlUser = "SELECT userid,username,jobdesi,TeamName,rights FROM tbl_usermaster where UserID='" + UserID + "'";
            DataTable dtUsers = dataCls.Getdata(sqlUser);
            if (dtUsers != null && dtUsers.Rows.Count > 0)
            {
                txt_EmpNo.Text = dtUsers.Rows[0]["userid"].ToString();
                hdn_EmpNo.Value = dtUsers.Rows[0]["userid"].ToString();
                hdn_EmpName.Value = dtUsers.Rows[0]["username"].ToString();
                EmpRights = dtUsers.Rows[0]["rights"].ToString();
                lbl_TeamName.Text = dtUsers.Rows[0]["TeamName"].ToString();
                display();

                Session["EmpIDSec"] = txt_EmpNo.Text;
            }
            else
            {
                hdn_EmpName.Value = "";
                clear();
                txt_EmpNo.Focus();
            }
            clear();
        }
        catch (Exception)
        {
        }
    }

    void display()
    {
        try
        {
            ////txt_Date.Text = DateTime.Today.ToString("dd/MM/yyyy");
            //Todaydate = DateTime.ParseExact(txt_Date.Text, "dd/MM/yyyy", null).ToString("dd/MM/yyyy");
            //string CheckAm = "";
            //CheckAm = Convert.ToString(DateTime.Now.ToString("tt"));

        }
        catch (Exception)
        {
        }
    }


    void TrackSheet_Grid()
    {
        string projectid_split1 = ddl_ProjectID.SelectedItem.Text;
        string[] splitter1 = projectid_split1.Split(new Char[] { '|' });
        string projectidonly = splitter1[0];
        string Projectname = "";
        if (splitter1.Length > 1)
            Projectname = splitter1[1];
        DataTable dd = new DataTable();
        SqlDataAdapter ds_ada = new SqlDataAdapter("select a.Projectid,a.EmpName,a.NoofCorrection,a.NoofCorrection1,sum(convert(int,isnull(a.NoOfPage,'0')))[NoOfPage],CONVERT(char(10),a.CurrentDate,103)[CurrentDate] from PrmsProductionHour_Backup a where a.ProjectID ='" + ddl_ProjectID.SelectedValue + "' and a.ProjectID<>'NA' GROUP BY a.projectid,a.EmpName,a.NoofCorrection,a.NoofCorrection1,a.CurrentDate union select (ProjectId +'|'+ ProjectName),'',ChapterID,'','','',0,'' from Project_details where ChapterID not in (select distinct from  PrmsProductionHour_Backup a WHERE a.ProjectID ='" + ddl_ProjectID.SelectedValue + "') and ProjectId ='" + projectidonly.Trim() + "' and ProjectName='" + Projectname.Trim() + "'", con);
        ds_ada.Fill(dd);
        con.Close();

    }

    private void Loaddetails()
    {
        DataTable dtDPR = dataCls.Getdata("Select slno,Empno,Empname,projectid,Grade,Stage,ActivitySubStage,Starttime,isnull(holdtime,0)[holdTime],Shift from PrmsProductionHour_Backup where endtime is null and (popstatus = 0 or popstatus is null) and empno ='" + txt_EmpNo.Text.Trim() + "' order by slno desc");
        if (dtDPR != null)
        {
            if (dtDPR.Rows.Count > 0)
            {
                ViewState["holdtime"] = Convert.ToString(dtDPR.Rows[0]["holdTime"]);
                txt_EmpNo.Text = Convert.ToString(dtDPR.Rows[0]["Empno"]);
                hdn_EmpName.Value = Convert.ToString(dtDPR.Rows[0]["Empname"]);
                ListItem shift = ddl_Shift.Items.FindByText(Convert.ToString(dtDPR.Rows[0]["Shift"]));
                if (shift != null && shift.Value != "") ddl_Shift.SelectedIndex = ddl_Shift.Items.IndexOf(shift);

                ListItem pid = ddl_ProjectID.Items.FindByText(Convert.ToString(dtDPR.Rows[0]["projectid"]));
                try
                {
                    if (pid != null && pid.Value != "")
                    {
                        ddl_ProjectID.SelectedIndex = ddl_ProjectID.Items.IndexOf(pid);

                        if (pid.Text.ToLower() == "non production")
                        {

                            ddl_Stage.Items.Clear();
                            ddl_Stage.Items.Insert(0, "Non Production");
                            ddl_Stage.SelectedIndex = 0;

                        }
                        else
                        {
                            loadstage();
                            ListItem stg = ddl_Stage.Items.FindByText(Convert.ToString(dtDPR.Rows[0]["Stage"]));
                            if (stg != null && stg.Value != "") ddl_Stage.SelectedIndex = ddl_Stage.Items.IndexOf(stg);
                            Loadchapter();

                        }
                    }
                }
                catch (Exception)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "", "alert('Please Contact your Admin for DPR End Time Process')", true);
                }
                if (ddl_EmpNo.Enabled == false)
                {
                    txt_StartTime.Text = Convert.ToString(dtDPR.Rows[0]["Starttime"]);
                }
                if (txt_StartTime.Text == "")
                {
                    ddl_ProjectID.Enabled = true;
                    ddl_Stage.Enabled = true;
                    ddl_Shift.Enabled = true;
                }
                else if (txt_StartTime.Text != "")
                {
                    ddl_ProjectID.Enabled = false;
                    ddl_Stage.Enabled = false;
                    ddl_Shift.Enabled = false;

                }
            }
        }
        DataTable dtDPR1 = dataCls.Getdata("select isnull(popstatus,0)[popstatus],starttime,slno from PrmsProductionHour_Backup where empno ='" + txt_EmpNo.Text + "'  order by slno desc");
    }

    private void DropdownlistLoad(DropDownList dp, string displayfield, string valuefield, string Sqlquery)
    {
        if (Sqlquery != null)
        {
            dp.DataSource = dataCls.Getdata(Sqlquery);
            dp.DataTextField = displayfield;
            dp.DataValueField = valuefield;
            dp.DataBind();
        }
    }

    void clear()
    {
        ddl_ProjectID.SelectedIndex = 0;
        ddl_EmpNo.SelectedIndex = -1;
        hdn_EmpName.Value = String.Empty;
        ddl_CompletedTask.SelectedIndex = 0;
        ddl_Stage.SelectedIndex = 0;
        ddl_Scope.SelectedIndex = 0;
        txt_Remarks.Text = "";
        txt_EndTime.Text = "";
        txt_TotalTime.Text = "";
        txt_StartTime.Text = "";
        ddl_ProjectID.Enabled = true;
        ddl_Stage.Enabled = true;
        ddl_Shift.SelectedIndex = 0;
        ddl_Shift.Enabled = true;
    }


    public void loadstage()
    {
        ddl_Stage.Items.Clear();
        ddl_Stage.Items.Insert(0, "Select");
        //SqlDataAdapter sqlada = new SqlDataAdapter("select distinct(Stage) from project_details where Batch='" + DropGrade.SelectedValue + "' and Projectid='" + getProjectID() + "'", con);
        //SqlDataAdapter sqlada = new SqlDataAdapter("select Track.Stage from (select distinct(a.Stage),(select c.OIPOrder from tbl_stage_master c where c.ProjectId=a.ProjectId and c.Stage=a.Stage)[order1] from project_details a where a.Batch='" + DropGrade.SelectedValue + "' and a.Projectid='" + getProjectID() + "')Track order by Track.Order1", con);
        SqlDataAdapter sqlada = new SqlDataAdapter("select max(a.Stage)[Stage] from project_details a, tbl_stage_master c where a.ProjectId=c.ProjectId and a.Stage=c.Stage and a.Projectid like'%" + getProjectID() + "%' group by a.Stage", con);
        DataSet ds = new DataSet();
        sqlada.Fill(ds);
        ddl_Stage.DataTextField = "Stage";
        ddl_Stage.DataValueField = "Stage";
        ddl_Stage.DataSource = ds;
        ddl_Stage.DataBind();
        ds.Clear();
    }

    public void Loadchapter()
    {
        SqlDataAdapter sqlada;
        if (Convert.ToString(Session["sessiontype"]).ToLower() != "admin")
            sqlada = new SqlDataAdapter("select distinct(Chapterid),Stage,Projectname from Project_details where Stage='" + ddl_Stage.SelectedValue + "' and ProjectID='" + getProjectID().Trim() + "' and  Chapterid not in (select ChapterId from tbl_TSDCS_Details b where b.ProjectId='" + getProjectID().Trim() + "' and DispatchId in (select DispatchId from tbl_TSDCS where ProjectId='" + getProjectID().Trim() + "' and DeleteStatus='0' and stage='" + ddl_Stage.SelectedValue + "'))", con);
        else
            sqlada = new SqlDataAdapter("select distinct(Chapterid),Stage,Projectname from Project_details where Stage='" + ddl_Stage.SelectedValue + "' and ProjectID='" + getProjectID().Trim() + "' and  Chapterid not in (select ChapterId from tbl_TSDCS_Details b where b.ProjectId='" + getProjectID().Trim() + "' and DispatchId in (select DispatchId from tbl_TSDCS where ProjectId='" + getProjectID().Trim() + "' and DeleteStatus='0' and stage='" + ddl_Stage.SelectedValue + "'))", con);

        DataSet ds = new DataSet();
        sqlada.Fill(ds);
        ds.Clear();
    }



    public void cmdreport_Click(object sender, ImageClickEventArgs e)
    {
        display();
    }



    public void udsave()
    {
        string Activity;
        if ((txt_TotalTime.Text.Trim() == ""))
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "Message", "javascript:Test();", true);
            return;
        }
        else if ((txt_TotalTime.Text.IndexOf('-')) >= 0)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "Message", "javascript:Test();", true);
            return;
        }
        string projectidtemp;
        string[] projectidtemp1;
        string projectidtemp2;
        string projectnametemp;
        if (ddl_ProjectID.SelectedItem.Text != "No" && ddl_ProjectID.SelectedItem.Text.ToLower() != "non production")
        {
            projectidtemp = ddl_ProjectID.SelectedItem.Text;
            projectidtemp1 = projectidtemp.Split(new Char[] { '|' });
            projectidtemp2 = projectidtemp1[0];
            projectnametemp = projectidtemp1[1];
        }
        else
        {
            projectidtemp = ddl_ProjectID.SelectedItem.Text;
            projectidtemp2 = projectidtemp;
            projectnametemp = projectidtemp;
        }
        if (ddl_Stage.SelectedItem.Text == "Select")
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "Message", "alert('please Select name');", true);
            return;
        }
        pageloadset = 1;
        try
        {
            int noofpage1;
            noofpage1 = 0;
        }
        catch
        {
            nooferr1 = 1;
        }
        if (nooferr1 != 0)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "Message", "javascript:popup1();", true);
            return;
        }
        string breakTime = "0";
        breakTime = "0";
        dataCls.DynamicParameters.Add("@pagenodetails", "0");
        dataCls.DynamicParameters.Add("@EnterDate", DateTime.Now.Date.ToString("MM/dd/yyyy"));
        dataCls.DynamicParameters.Add("@Break1", breakTime);
        dataCls.DynamicParameters.Add("@Remarks", txt_Remarks.Text.Replace("'", "''"));
        dataCls.DynamicParameters.Add("@TeamName", lbl_TeamName.Text);
        dataCls.DynamicParameters.Add("@StartTime", txt_StartTime.Text);
        dataCls.DynamicParameters.Add("@EndTime", txt_EndTime.Text);
        dataCls.DynamicParameters.Add("@TotalTime", txt_TotalTime.Text);
        dataCls.InsertOrUpdateData(@"update PrmsProductionHour_Backup set StartTime=@StartTime,EndTime=@EndTime,TotalTime=@TotalTime,pagenodetails=@pagenodetails,EnterDate=@EnterDate,Break1=@Break1,Remarks=@Remarks,
        TeamName=@TeamName where slno=@slno", false, true);
        dataCls.DynamicParameters.Clear();
        ViewState["holdtime"] = "0";
        if (nooferr == 0 && nooferr1 == 0)
        {
            if (lbl_StartTime.InnerText == "") lbl_StartTime.InnerText = txt_StartTime.Text;
            bindDPR();
            clear();
            ddl_ProjectID.Focus();
            nooferr = 0;
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "Message", "alert('Please Check Time . . . . ');", true);
            return;
        }
        Session["TotalTime"] = "0:00";
    }

    public bool udValidation()
    {
        string projectidtemp;

        string projectidtemp2;
        string projectnametemp;
        {
            projectidtemp = ddl_ProjectID.SelectedItem.Text;
            projectidtemp2 = projectidtemp;
            projectnametemp = projectidtemp;
        }
        if (ddl_Stage.SelectedValue == "Select")
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "Message", "alert('please Select name');", true);
            return false;
        }
        pageloadset = 1;
        try
        {
            int noofpage1;
            noofpage1 = 0;
        }
        catch
        {
            nooferr1 = 1;
        }
        if (nooferr1 != 0)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "Message", "javascript:popup1();", true);
            return false;
        }
        return true;
    }

    public void Edit_Click(object sender, ImageClickEventArgs e)
    {

    }



    public void ddl_Stage_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            dpr_gridbind();
            if (ddl_ProjectID.SelectedItem.Text == "No-Project")
            {
            }
            else
            {
                bindScope();
                if (ddl_ProjectID.SelectedItem.Text.ToLower().Contains("non production"))
                {
                }
                Loadchapter();
            }
            ddl_Scope.Focus();
        }
        catch (Exception)
        {

        }
    }
    public void ddl_Scope_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindTask();
            dpr_gridbind();
            ddl_Task.Focus();
        }
        catch (Exception)
        {

        }
    }

    public void LinkButton1_Click(object sender, EventArgs e)
    {

    }

    public void btn_excelView_Click(object sender, EventArgs e)
    {
        if (ddl_ProjectID.SelectedItem.Text != "Select")
        {
            SqlDataReader dr;
            String Query;
            String Query1;
            String Query2;
            String Query3;
            Response.Clear();
            string projectid_split1 = ddl_ProjectID.SelectedItem.Text;
            string[] splitter1 = projectid_split1.Split(new Char[] { '|' });
            string projectidonly = splitter1[0];
            string Projectname = splitter1[1];
            if (con.State == ConnectionState.Closed) con.Open();
            string Rights = Convert.ToString(Session["Rights"]);
            if (Rights == "Administrator")
            {
                Query3 = "select Stage from PrmsProductionHour_Backup where projectid like '" + projectid_split1 + "%' and Stage like '" + ddl_Stage.SelectedValue + "%'  group by Stage";
                Query2 = "select Grade from PrmsProductionHour_Backup where projectid like '" + projectid_split1 + "%' and Stage like '" + ddl_Stage.SelectedValue + "%' group by Grade";
                Query1 = "select Projectid from PrmsProductionHour_Backup where projectid like '" + projectid_split1 + "%' and Stage like '" + ddl_Stage.SelectedValue + "%' group by Projectid";
                Query = "select a.EmpName,sum(convert(int,isnull(a.NoOfPage,'0')))[NoOfPage],CONVERT(char(10),a.CurrentDate,103)[CurrentDate] from PrmsProductionHour_Backup a where a.ProjectID ='" + ddl_ProjectID.SelectedValue + "' and a.ProjectID<>'NA' GROUP BY a.EmpName,a.CurrentDate union select '',ChapterID,'',0,'' from Project_details where ProjectId ='" + projectidonly.Trim() + "' and ProjectName='" + Projectname.Trim() + "'";
            }
            else
            {
                Query3 = "select Stage from PrmsProductionHour_Backup where projectid like '" + projectid_split1 + "%' and Stage like '" + ddl_Stage.SelectedValue + "%'  group by Stage";
                Query2 = "select Grade from PrmsProductionHour_Backup where projectid like '" + projectid_split1 + "%' and Stage like '" + ddl_Stage.SelectedValue + "%' group by Grade";
                Query1 = "select Projectid from PrmsProductionHour_Backup where projectid like '" + projectid_split1 + "%' and Stage like '" + ddl_Stage.SelectedValue + "%' group by Projectid";
                Query = "select a.EmpName,sum(convert(int,isnull(a.NoOfPage,'0')))[NoOfPage],CONVERT(char(10),a.CurrentDate,103)[CurrentDate] from PrmsProductionHour_Backup a where a.ProjectID ='" + ddl_ProjectID.SelectedValue + "' and a.ProjectID<>'NA' GROUP BY a.EmpName,a.CurrentDate union select '',ChapterID,'',0,'' from Project_details where ProjectId ='" + projectidonly.Trim() + "' and ProjectName='" + Projectname.Trim() + "'";
            }
            SqlCommand cmd = new SqlCommand(Query.ToString(), con);
            cmd.CommandType = CommandType.Text;
            dr = cmd.ExecuteReader();
            string projectid_split = ddl_ProjectID.SelectedItem.Text;
            string[] splitter = projectid_split.Split(new Char[] { '|' });
            string projectidonly1 = splitter[0];
            //*************************************
            string excelname = "ExcelReport.xls";
            string Excelpath = "";
            string sql = "";
            Excel.Range DeuxCellules;
            Excel.Application xla = new Excel.Application();
            Excel.Workbook xlw = (Excel.Workbook)xla.Workbooks.Add(1);
            Excel.Worksheet xls = (Excel.Worksheet)xla.ActiveWorkbook.Sheets[1];
            sql = Query;
            xls = (Excel.Worksheet)xla.ActiveWorkbook.Sheets.Add(System.Reflection.Missing.Value, xla.ActiveWorkbook.Sheets[xla.ActiveWorkbook.Sheets.Count], System.Reflection.Missing.Value, System.Reflection.Missing.Value);
            xls.Cells[1, 1] = "Project ID :" + ddl_ProjectID.SelectedValue;
            xls.Cells[3, 1] = "Stage :" + ddl_Stage.SelectedValue;
            xls.QueryTables[1].Refresh(false);
            xls.UsedRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            xls.UsedRange.Font.Name = "Verdana";
            xls.UsedRange.Columns.AutoFit();
            DeuxCellules = (Excel.Range)xls.get_Range("A4", xls.Cells[1, xls.UsedRange.Columns.Count]);
            DeuxCellules.Font.Size = 10;
            DeuxCellules.Font.Bold = true;
            DeuxCellules = (Excel.Range)xls.get_Range("A2", xls.Cells[1, xls.UsedRange.Columns.Count]);
            DeuxCellules.Font.Size = 11;
            DeuxCellules.Font.Bold = true;
            DeuxCellules = (Excel.Range)xls.get_Range("A3", xls.Cells[3, xls.UsedRange.Columns.Count]);
            DeuxCellules.Font.Size = 11;
            DeuxCellules.Font.Bold = true;
            DeuxCellules = (Excel.Range)xls.get_Range("A1", xls.Cells[1, xls.UsedRange.Columns.Count]);
            DeuxCellules.MergeCells = true;
            DeuxCellules.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
            DeuxCellules.Font.Size = 16;
            DeuxCellules.Font.Bold = true;
            xls.UsedRange.Columns.AutoFit();
            xla.ActiveWorkbook.SaveCopyAs(Server.MapPath(".") + "\\Reports\\" + excelname);
            xla.ActiveWorkbook.Close(false, "", false);
            xla.Quit();
            Excelpath = "\\ts\\Reports\\" + excelname;
            Response.Redirect(Excelpath);
        }
    }

    public void reportgrd_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

        TrackSheet_Grid();
    }

    public void btn_Edit_Click(object sender, EventArgs e)
    {
        bindDPR();
        //txt_StartTime_MaskedEditExtender.MaskType = AjaxControlToolkit.MaskedEditType.None;
        //MaskedEditExtender2.MaskType = AjaxControlToolkit.MaskedEditType.None;
        txt_EndTime.Enabled = true;
        txt_StartTime.Enabled = true;
    }

    void bindDPR()
    {
        SqlDataAdapter adaa = new SqlDataAdapter("select slno,EmpNo,EmpName,Shift,CONVERT(varchar, currentdate,103) EnterDate,ProjectID,Grade Batch,Stage,ActivitySubStage,(select sum(cast(ppv.MeasurementValue as int))noofpage from PrmsProductionValue ppv where ppv.DPRID=pp.slno)NoOfPage,StartTime,EndTime,left(TotalTime,10)as [Total Time],[Break1],Remarks from PrmsProductionHour_Backup pp where CurrentDate = '" + DateTime.ParseExact(txt_Date.Text, "dd/MM/yyyy", null).ToString("MM/dd/yyyy") + "' and Empno='" + txt_EmpNo.Text + "' order by currentdate desc", con);
        DataSet ds = new DataSet();
        adaa.Fill(ds);
        con.Close();

    }



    public void grid_PriviousData_ItemDataBound(object sender, DataGridItemEventArgs e)
    {

    }



    public void grid_PriviousData_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        DataSet dd = new DataSet();

        SqlDataAdapter sqladada = new SqlDataAdapter("SELECT slno,EmpNo,EmpName,Shift,CONVERT(varchar, currentdate,103) EnterDate,ProjectID,Grade Batch,Stage,ActivitySubStage,(select sum(cast(ppv.MeasurementValue as int))noofpage from PrmsProductionValue ppv where ppv.DPRID=pp.slno)NoOfPage,StartTime,EndTime,left(TotalTime,10)as [Total Time],[Break1],Remarks FROM PrmsProductionHour_Backup pp where empNo = '" + txt_EmpNo.Text + "' and CurrentDate ='" + DateTime.ParseExact(txt_Date.Text, "dd/MM/yyyy", null).ToString("MM/dd/yyyy") + "' order by currentdate desc", con);
        sqladada.Fill(dd);

    }

    public void txt_EndTime_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string[] splittedTime = txt_EndTime.Text.ToString().Split(':');
            if (Convert.ToInt32(splittedTime[0]) > 23 || Convert.ToInt32(splittedTime[1]) > 59)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "myscript", "alert('Endtime is Invalid')", true);
                txt_EndTime.Text = "";
            }
            else
            {
                if (txt_EndTime.Text == "" || txt_EndTime.Text == "__:__")
                {

                }
                else if (txt_StartTime.Text == "" || txt_StartTime.Text == "__:__")
                {

                }
                else
                {
                    //DateTime startdate = Convert.ToDateTime(txt_StartTime.Text);
                    //DateTime enddate = Convert.ToDateTime(txt_EndTime.Text);
                    //if (startdate <= enddate)
                    calculateTime();
                    //else
                    //{
                    //    ScriptManager.RegisterStartupScript(Page, GetType(), "myscript", "alert('start sate should be less than end date!')", true);
                    //    return;
                    //}

                }
                dpr_gridbind();
                txt_EndTime.Enabled = true;
            }
            txt_MeetingTime.Enabled = true;
        }
        catch (Exception)
        {
        }
        txt_MeetingTime.Focus();
    }
    public void txt_Date_TextChanged(object sender, EventArgs e)
    {
        dpr_gridbind();
    }
    protected void txt_StartTime_TextChanged1(object sender, EventArgs e)
    {
        try
        {
            string[] splittedTime = txt_StartTime.Text.ToString().Split(':');
            if (Convert.ToInt32(splittedTime[0]) > 23 || Convert.ToInt32(splittedTime[1]) > 59)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "myscript", "alert('Start Time in Invalid')", true);
                txt_StartTime.Text = "";
            }
            else
            {
                txt_EndTime.Enabled = true;
                if ((txt_StartTime.Text != "") && (txt_StartTime.Text != "__:__"))
                {

                }
                if ((txt_EndTime.Text != "") && (txt_EndTime.Text != "__:__"))
                {
                    //DateTime startdate = Convert.ToDateTime(txt_StartTime.Text);
                    //DateTime enddate = Convert.ToDateTime(txt_EndTime.Text);
                    //if (startdate <= enddate)  
                    calculateTime();
                    //else
                    //{
                    //    ScriptManager.RegisterStartupScript(Page, GetType(), "myscript", "alert('start sate should be less than end date!')", true);
                    //    return;
                    //}
                }
                //StartDate = DateTime.ParseExact(txt_StartTime.Text.ToString().Substring(0, 10), "dd/MM/yyyy", CultureInfo.InvariantCulture);

                if ((txt_EndTime.Text != "") && (txt_EndTime.Text != "__:__"))
                {
                    txt_EndTime.Text = txt_StartTime.Text;
                    calculateTime();
                }
                dpr_gridbind();
                txt_EndTime.Focus();
            }
        }
        catch (Exception)
        {
        }
    }

    public void txt_EndTime_TextChanged1(object sender, EventArgs e)
    {
        try
        {
            string[] splittedTime = txt_EndTime.Text.ToString().Split(':');
            if (Convert.ToInt32(splittedTime[0]) > 23 || Convert.ToInt32(splittedTime[1]) > 59)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "myscript", "alert('Endtime is Invalid')", true);
            }
            else
            {
                calculateTime();
                if ((txt_TotalTime.Text.Trim() == ""))
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "myscript", "alert('Check the time')", true);
                    return;
                }
                else if ((txt_TotalTime.Text.IndexOf('-')) >= 0)
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "myscript", "alert('Check the time')", true);
                    return;
                }
                dpr_gridbind();
                rbl_BreakTime.Focus();
            }
        }
        catch (Exception)
        {
        }
    }
    protected void txt_MeetingTime_TextChanged(object sender, EventArgs e)
    {
        string breakTime = "0";
        if (rbl_BreakTime.SelectedValue == "15") breakTime = "15";
        else if (rbl_BreakTime.SelectedValue == "20") breakTime = "20";
        else if (rbl_BreakTime.SelectedValue == "30") breakTime = "30";
        else
            breakTime = "0";
        if (con.State == ConnectionState.Closed) con.Open();
        if (Convert.ToString(ViewState["holdtime"]) != "0")
            breakTime = Convert.ToString(Convert.ToInt32(breakTime) + Convert.ToInt32(ViewState["holdtime"]));
        txt_TotalTime.Text = dataCls.Subtract(txt_Date.Text + " " + txt_StartTime.Text, txt_Date.Text + " " + txt_EndTime.Text, breakTime, "00:00");
        if (txt_MeetingTime.Text != "" && txt_MeetingTime.Text != "__:__")
        {
            txt_TotalTime.Text = dataCls.SubtractNew(txt_Date.Text + " " + txt_StartTime.Text, txt_Date.Text + " " + txt_EndTime.Text, breakTime, txt_MeetingTime.Text);
        }
        txt_MeetingRemarks.Focus();
    }
    protected void LinkNC_Click(object sender, EventArgs e)
    {
        Session["ProjectID"] = ddl_ProjectID.Text;
        Session["ddl_Stage"] = ddl_Stage.Text;
        Session["Ncpage"] = "false";
        Response.Redirect("NCReport_New.aspx");
    }


    protected void calculateTime()
    {
        string breakTime = "0";
        if (rbl_BreakTime.SelectedValue == "15") breakTime = "15";
        else if (rbl_BreakTime.SelectedValue == "20") breakTime = "20";
        else if (rbl_BreakTime.SelectedValue == "30") breakTime = "30";
        else
            breakTime = "0";
        if (con.State == ConnectionState.Closed) con.Open();
        if (Convert.ToString(ViewState["holdtime"]) != "0")
            breakTime = Convert.ToString(Convert.ToInt32(breakTime) + Convert.ToInt32(ViewState["holdtime"]));
        txt_TotalTime.Text = dataCls.Subtract(txt_Date.Text + " " + txt_StartTime.Text, txt_Date.Text + " " + txt_EndTime.Text, breakTime, "00:00");
        if (txt_MeetingTime.Text != "" && txt_MeetingTime.Text != "__:__")
        {
            txt_TotalTime.Text = dataCls.Subtract(txt_Date.Text + " " + txt_StartTime.Text, txt_Date.Text + " " + txt_EndTime.Text, breakTime, txt_MeetingTime.Text);
        }
    }

    protected void btn_CkeckTime_Click(object sender, EventArgs e)
    {
        calculateTime();
    }


    protected void btnsave_Click(object sender, EventArgs e)
    {
        string sqlStr = "IF NOT EXISTS (select slno from PrmsProductionValue where DPRID=@DPRID and MeasurementID=@MeasurementID )insert into PrmsProductionValue (DPRID,MeasurementID,MeasurementName,MeasurementValue) values (@DPRID,@MeasurementID,@MeasurementName,@MeasurementValue)";

        dataCls.InsertOrUpdateData(sqlStr, false, true);
        dataCls.DynamicParameters.Clear();
    }
    protected void grid_Measurement_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void grid_Measurement_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[0].Visible = false;
    }

    //protected void btn_Hold_Click(object sender, EventArgs e)
    //{
    //    dpr_gridbind();
    //    if (getProjectID() != "" && hdn_DPRID.Value != "0")
    //    {

    //        dataCls.DynamicParameters.Add("@Empno", txt_EmpNo.Text.Trim());
    //        dataCls.DynamicParameters.Add("@EmpName", hdn_EmpName.Value.Trim());
    //        dataCls.DynamicParameters.Add("@Shift", ddl_Shift.SelectedValue);
    //        dataCls.DynamicParameters.Add("@ProjectID", "Non Production");
    //        dataCls.DynamicParameters.Add("@Stage", "Non Production");

    //        dataCls.DynamicParameters.Add("@CurrentDate", DateTime.ParseExact(txt_Date.Text, "dd/MM/yyyy", null).ToString("MM/dd/yyyy"));
    //        dataCls.DynamicParameters.Add("@projectidtemp", getProjectID());
    //        dataCls.DynamicParameters.Add("@projectnametemp", ddl_ProjectID.SelectedItem.Text.Split(new Char[] { '|' })[1]);
    //        dataCls.DynamicParameters.Add("@popstatus", true);

    //        dataCls.DynamicParameters.Add("@DPRID", hdn_DPRID.Value);
    //        bool isinserted = dataCls.InsertOrUpdateData("if not exists(select slno from PrmsProductionHour_Backup where Empno=@Empno and EmpName=@EmpName and Shift=@Shift and ProjectID=@ProjectID and Stage=@Stage and StartTime=@StartTime and CurrentDate=@CurrentDate and projectidtemp=@projectidtemp and projectnametemp=@projectnametemp and popstatus=@popstatus)Insert into PrmsProductionHour_Backup(Empno,EmpName,Shift,ProjectID,Stage,StartTime,CurrentDate,projectidtemp,projectnametemp,popstatus)values(@Empno,@EmpName,@Shift,@ProjectID,@Stage,@StartTime,@CurrentDate,@projectidtemp,@projectnametemp,@popstatus)", false, true);
    //        if (isinserted)
    //        {
    //            string lastInsertedHoldID = dataCls.GetSingleData("SELECT IDENT_CURRENT('PrmsProductionHour_Backup')");
    //            btn_EndTime.Enabled = false;

    //        }
    //        btn_Hold.Visible = false;

    //    }
    //}
    protected void btn_release_Click(object sender, EventArgs e)
    {


        dataCls.DynamicParameters.Add("@Empno", txt_EmpNo.Text.Trim());
        dataCls.DynamicParameters.Add("@Shift", ddl_Shift.SelectedValue);
        dataCls.DynamicParameters.Add("@ProjectID", "Non Production");
        dataCls.DynamicParameters.Add("@Stage", "Non Production");

        dataCls.DynamicParameters.Add("@CurrentDate", DateTime.ParseExact(txt_Date.Text, "dd/MM/yyyy", null).ToString("MM/dd/yyyy"));
        bool isinserted1 = dataCls.InsertOrUpdateData("if exists(select slno from PrmsProductionHour_Backup where Empno=@Empno and Shift=@Shift and CurrentDate=@CurrentDate and ProjectID=@ProjectID and Stage=@Stage and StartTime=@StartTime and EndTime is NULL)update PrmsProductionHour_Backup set Holdtime = (isnull(Holdtime,0) + @TotalTimeMIN) where slno=@id1", false, true);
        if (isinserted1)
        {

            dataCls.DynamicParameters.Add("@EndTime", dataCls.GetCurrentDateTime());


            dataCls.DynamicParameters.Add("@popstatus", false);

            bool isinserted = dataCls.InsertOrUpdateData("update PrmsProductionHour_Backup set EndTime=@EndTime,Grade=@Grade,Remarks=@Remarks,TotalTime=@TotalTime,popstatus =@popstatus where slno=@ID", false, true);
        }
    }
    protected void btn_CalcTime_Click(object sender, EventArgs e)
    {
        calculateTime();
    }
    protected void btn_QAEntry_Click(object sender, EventArgs e)
    {
        string Querys = string.Empty;
        if (ddl_ProjectID.SelectedItem.Text == "Select")
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "Message", "alert('please select the Projectid');", true);
            return;
        }
        Querys = string.Format("SELECT slno,EmpNo,EmpName,Shift,CONVERT(varchar, currentdate,103) EnterDate,ProjectID,Grade Batch,Stage,ActivitySubStage,(select sum(cast(ppv.MeasurementValue as int))noofpage from PrmsProductionValue ppv where ppv.DPRID=pp.slno)NoOfPage,StartTime,EndTime,left(TotalTime,10)as [Total Time],[Break1],Remarks FROM PrmsProductionHour_Backup pp where projectidtemp='{0}' and ActivitySubStage in (select distinct(Name) from tbl_SubStage where ProjectID='{0}' and FinalStage='1' and Status='1') order by slno desc", getProjectID());
        if (ddl_Stage.SelectedValue.Trim() != "Select")
        {
            Querys = string.Format("SELECT slno,EmpNo,EmpName,Shift,CONVERT(varchar, currentdate,103) EnterDate,ProjectID,Grade Batch,Stage,ActivitySubStage,(select sum(cast(ppv.MeasurementValue as int))noofpage from PrmsProductionValue ppv where ppv.DPRID=pp.slno)NoOfPage,StartTime,EndTime,left(TotalTime,10)as [Total Time],[Break1],Remarks FROM PrmsProductionHour_Backup pp where projectidtemp='{0}' and ActivitySubStage in (select distinct(Name) from tbl_SubStage where ProjectID='{0}' and FinalStage='1' and Status='1') and Stage='{2}' order by slno desc", getProjectID(), ddl_Stage.SelectedValue.Trim());
        }


    }
    protected void btn_WithoutQAEntry_Click(object sender, EventArgs e)
    {
        string Querys = string.Empty;
        if (ddl_ProjectID.SelectedItem.Text == "Select")
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "Message", "alert('please select the Projectid');", true);
            return;
        }
        Querys = string.Format("SELECT slno,EmpNo,EmpName,Shift,CONVERT(varchar, currentdate,103) EnterDate,ProjectID,Grade Batch,Stage,ActivitySubStage,(select sum(cast(ppv.MeasurementValue as int))noofpage from PrmsProductionValue ppv where ppv.DPRID=pp.slno)NoOfPage,StartTime,EndTime,left(TotalTime,10)as [Total Time],[Break1],Remarks FROM PrmsProductionHour_Backup pp where projectidtemp='{0}' and ActivitySubStage not in (select distinct(Name) from tbl_SubStage where ProjectID='{0}' and FinalStage='1' and Status='1') order by slno desc", getProjectID());
        if (ddl_Stage.SelectedValue.Trim() != "Select")
        {
            Querys = string.Format("SELECT slno,EmpNo,EmpName,Shift,CONVERT(varchar, currentdate,103) EnterDate,ProjectID,Grade Batch,Stage,ActivitySubStage,(select sum(cast(ppv.MeasurementValue as int))noofpage from PrmsProductionValue ppv where ppv.DPRID=pp.slno)NoOfPage,StartTime,EndTime,left(TotalTime,10)as [Total Time],[Break1],Remarks FROM PrmsProductionHour_Backup pp where projectidtemp='{0}' and ActivitySubStage not in (select distinct(Name) from tbl_SubStage where ProjectID='{0}' and FinalStage='1' and Status='1') and Stage='{2}'  order by slno desc", getProjectID(), ddl_Stage.SelectedValue.Trim());
        }


    }
    protected void ddl_Shift_SelectedIndexChanged(object sender, EventArgs e)
    {
        txt_Date.Focus();
        dpr_gridbind();
    }

    protected void btn_Rnonproduction_Click(object sender, EventArgs e)
    {
        string Querys = string.Empty;
        if (ddl_ProjectID.SelectedItem.Text == "Select")
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "Message", "alert('please select the Projectid');", true);
            return;
        }
        Querys = string.Format("SELECT slno,EmpNo,EmpName,Shift,CONVERT(varchar, currentdate,103) EnterDate,ProjectID,Grade Batch,Stage,ActivitySubStage,(select sum(cast(ppv.MeasurementValue as int))noofpage from PrmsProductionValue ppv where ppv.DPRID=pp.slno)NoOfPage,StartTime,EndTime,left(TotalTime,10)as [Total Time],[Break1],Remarks FROM PrmsProductionHour_Backup pp where projectidtemp='{0}' and ActivitySubStage not in (select distinct(Name) from tbl_SubStage where ProjectID='{0}' and FinalStage='1' and Status='1') and order by slno desc", getProjectID());
        if (ddl_Stage.SelectedValue.Trim() != "Select")
        {
            Querys = string.Format("SELECT slno,EmpNo,EmpName,Shift,CONVERT(varchar, currentdate,103) EnterDate,ProjectID,Grade Batch,Stage,ActivitySubStage,(select sum(cast(ppv.MeasurementValue as int))noofpage from PrmsProductionValue ppv where ppv.DPRID=pp.slno)NoOfPage,StartTime,EndTime,left(TotalTime,10)as [Total Time],[Break1],Remarks FROM PrmsProductionHour_Backup pp where projectidtemp='{0}' and ActivitySubStage not in (select distinct(Name) from tbl_SubStage where ProjectID='{0}' and FinalStage='1' and Status='1') and Stage='{2}' order by slno desc", getProjectID(), ddl_Stage.SelectedValue.Trim());
        }
    }
    protected void btn_SaveStatus_Click(object sender, EventArgs e)
    {
        try
        {
            string Rights = Session["Rights"].ToString();
            btn_SaveStatus.Attributes.Add("OnClientClick", "return validate()");
            if (btn_SaveStatus.Text == "Save")
            {
                string TotalTime = txt_TotalTime.Text;
                if (TotalTime.Contains("-"))
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Message", "alert('Start Time must not be greater than End Time');", true);
                    txt_StartTime.Focus();
                }
                else
                {
                    if (ddl_ProjectID.SelectedValue != "NA")
                    {
                        if (ddl_CompletedTask.SelectedValue != "100")
                        {
                            com.Connection = con;
                            con.Close();
                            con.Open();
                            com.CommandText = "update tbl_ProjectReq set projectstatus='WIP' where projectid='" + ddl_ProjectID.SelectedValue + "'";
                            com.ExecuteNonQuery();
                            com.CommandText = "update tbl_taskmaster set taskstatus='WIP' where id='" + ddl_Task.SelectedValue + "'";
                            com.ExecuteNonQuery();
                        }
                        if (ddl_CompletedTask.SelectedValue == "100")
                        {
                            com.Connection = con;
                            con.Close();
                            con.Open();
                            com.CommandText = "update tbl_ProjectReq set projectstatus='WIP' where projectid='" + ddl_ProjectID.SelectedValue + "'";
                            com.ExecuteNonQuery();
                            com.CommandText = "update tbl_taskmaster set taskstatus='Completed' where id='" + ddl_Task.SelectedValue + "'";
                            com.ExecuteNonQuery();
                        }
                    }
                    ViewState["EmpNo"] = "";
                    SqlCommand cmd;
                    cmd = new SqlCommand("if not exists(select slno from PrmsProductionHour_Backup where Empno=@Empno and EmpName=@EmpName and ProjectID=@ProjectID and Stage=@Stage and Scope=@Scope and StartTime=@StartTime and CurrentDate=@CurrentDate and Shift=@Shift and statusoftask=@statusoftask)Insert into PrmsProductionHour_Backup(Empno,EmpName,Shift,CurrentDate,ProjectID,Stage,Scope,task,statusoftask,StartTime,EndTime,Break1,meetingtime,meetingremarks,TotalTime,Remarks)values(@Empno,@EmpName,@Shift,@CurrentDate,@ProjectID,@Stage,@Scope,@task,@statusoftask,@StartTime,@EndTime,@Breaktime,@meetingtime,@meetingremarks,@Totaltime,@Remarks)");
                    cmd.Connection = con;
                    cmd.Parameters.Clear();
                    if (Rights == "Administrator" || Rights == "Team Leader")
                    {
                        cmd.Parameters.AddWithValue("@Empno", ddl_EmpNo.SelectedValue.Trim());
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Empno", txt_EmpNo.Text.Trim());
                    }
                    cmd.Parameters.AddWithValue("@EmpName", hdn_EmpName.Value.Trim());
                    cmd.Parameters.AddWithValue("@ProjectID", ddl_ProjectID.SelectedValue);
                    cmd.Parameters.AddWithValue("@Stage", ddl_Stage.SelectedValue);
                    cmd.Parameters.AddWithValue("@Scope", ddl_Scope.SelectedValue);
                    cmd.Parameters.AddWithValue("@task", ddl_Task.SelectedValue);
                    cmd.Parameters.AddWithValue("@statusoftask", ddl_CompletedTask.SelectedValue);
                    cmd.Parameters.AddWithValue("@StartTime", txt_Date.Text + " " + txt_StartTime.Text.Trim());
                    cmd.Parameters.AddWithValue("@EndTime", txt_Date.Text + " " + txt_EndTime.Text.Trim());
                    cmd.Parameters.AddWithValue("@Shift", ddl_Shift.SelectedItem.Text.Trim());
                    cmd.Parameters.AddWithValue("@CurrentDate", DateTime.ParseExact(txt_Date.Text.Trim(), "dd/MM/yyyy", null).ToString("MM/dd/yyyy"));
                    cmd.Parameters.AddWithValue("@Breaktime", rbl_BreakTime.SelectedValue.Trim());
                    cmd.Parameters.AddWithValue("@meetingtime", txt_MeetingTime.Text);
                    cmd.Parameters.AddWithValue("@meetingremarks", txt_MeetingRemarks.Text);
                    cmd.Parameters.AddWithValue("@Totaltime", txt_TotalTime.Text);
                    cmd.Parameters.AddWithValue("@Remarks", txt_Remarks.Text);
                    con.Close();
                    con.Open();
                    cmd.ExecuteNonQuery();
                    dpr_gridbind();
                    clear();
                    ScriptManager.RegisterStartupScript(Page, GetType(), "myscript", "alert('Status Updated')", true);
                }
            }

            else
            {
                if (ddl_ProjectID.SelectedValue != "NA")
                {
                    if (ddl_CompletedTask.SelectedValue != "100")
                    {
                        com.Connection = con;
                        con.Close();
                        con.Open();
                        com.CommandText = "update tbl_ProjectReq set projectstatus='WIP' where projectid='" + ddl_ProjectID.SelectedValue + "'";
                        com.ExecuteNonQuery();
                        com.CommandText = "update tbl_taskmaster set taskstatus='WIP' where id='" + ddl_Task.SelectedValue + "'";
                        com.ExecuteNonQuery();
                    }
                    if (ddl_CompletedTask.SelectedValue == "100")
                    {
                        com.Connection = con;
                        con.Close();
                        con.Open();
                        com.CommandText = "update tbl_ProjectReq set projectstatus='WIP' where projectid='" + ddl_ProjectID.SelectedValue + "'";
                        com.ExecuteNonQuery();
                        com.CommandText = "update tbl_taskmaster set taskstatus='Completed' where id='" + ddl_Task.SelectedValue + "'";
                        com.ExecuteNonQuery();
                    }
                }
                string TotalTime = txt_TotalTime.Text;
                if (TotalTime[0].ToString() == "-")
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Message", "alert('Start Time must not be greater than End Time');", true);
                    txt_StartTime.Focus();
                }
                else
                {
                    SqlCommand cmd;
                    cmd = new SqlCommand("update PrmsProductionHour_Backup set EmpName=@EmpName,Shift=@Shift,CurrentDate=@CurrentDate,ProjectID=@ProjectID,Stage=@Stage,Scope=@Scope,task=@task,statusoftask=@statusoftask,StartTime=@StartTime,EndTime=@EndTime,Break1=@Breaktime,meetingtime=@meetingtime,meetingremarks=@meetingremarks,TotalTime=@Totaltime,Remarks=@Remarks where slno=@Slno");
                    cmd.Connection = con;
                    cmd.Parameters.Clear();
                    if (Rights == "Administrator" || Rights == "TeamLeader")
                    {
                        cmd.Parameters.AddWithValue("@Empno", ddl_EmpNo.SelectedValue.Trim());
                        ViewState["EmpNo"] = ddl_EmpNo.SelectedValue.Trim();
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Empno", txt_EmpNo.Text.Trim());
                    }
                    cmd.Parameters.AddWithValue("@Slno", hdn_Slno.Value.Trim());
                    cmd.Parameters.AddWithValue("@EmpName", hdn_EmpName.Value.Trim());
                    cmd.Parameters.AddWithValue("@ProjectID", ddl_ProjectID.SelectedValue);
                    cmd.Parameters.AddWithValue("@Stage", ddl_Stage.SelectedValue);
                    cmd.Parameters.AddWithValue("@Scope", ddl_Scope.SelectedValue);
                    cmd.Parameters.AddWithValue("@task", ddl_Task.SelectedValue);
                    cmd.Parameters.AddWithValue("@statusoftask", ddl_CompletedTask.SelectedValue);
                    cmd.Parameters.AddWithValue("@StartTime", txt_Date.Text + " " + txt_StartTime.Text.Trim());
                    cmd.Parameters.AddWithValue("@EndTime", txt_Date.Text + " " + txt_EndTime.Text.Trim());
                    cmd.Parameters.AddWithValue("@Shift", ddl_Shift.SelectedItem.Text.Trim());
                    cmd.Parameters.AddWithValue("@CurrentDate", DateTime.ParseExact(txt_Date.Text.Trim(), "dd/MM/yyyy", null).ToString("MM/dd/yyyy"));
                    cmd.Parameters.AddWithValue("@Breaktime", rbl_BreakTime.SelectedValue.Trim());
                    cmd.Parameters.AddWithValue("@meetingtime", txt_MeetingTime.Text);
                    cmd.Parameters.AddWithValue("@meetingremarks", txt_MeetingRemarks.Text);
                    cmd.Parameters.AddWithValue("@Totaltime", txt_TotalTime.Text);
                    cmd.Parameters.AddWithValue("@Remarks", txt_Remarks.Text);
                    con.Close();
                    con.Open();
                    cmd.ExecuteNonQuery();
                    dpr_gridbind();
                    btn_SaveStatus.Text = "Save";
                    Response.Redirect(Request.Url.AbsoluteUri);
                    if (EmpRights == "Administrator")
                    {
                        ddl_EmpNo.SelectedValue = ViewState["EmpNo"].ToString();
                    }
                    ScriptManager.RegisterStartupScript(Page, GetType(), "myscript", "alert('Status Updated')", true);
                }
            }
        }
        catch (Exception)
        {
            Response.Redirect(Request.Url.AbsoluteUri);
        }
    }
    protected void Edit_Click(object sender, EventArgs e)
    {
        string Activity = "";

        btn_SaveStatus.Visible = true;
        string projectidtempE;
        string[] projectidtempE1;
        string projectidtempE2;
        string projectnametempE;
        if (ddl_ProjectID.SelectedItem.Text != "No-Project" && ddl_ProjectID.SelectedItem.Text != "Non Production")
        {
            projectidtempE = ddl_ProjectID.SelectedItem.Text;
            projectidtempE1 = projectidtempE.Split(new Char[] { '|' });
            projectidtempE2 = projectidtempE1[0];
            projectnametempE = projectidtempE1[1];
        }
        else
        {
            projectidtempE = ddl_ProjectID.SelectedItem.Text;
            projectidtempE2 = projectidtempE;
            projectnametempE = projectidtempE;
        }
        pageloadset = 1;
        calculateTime();
        if (nooferr == 0)
        {
            string sql_qry;
            con.Close();
            con.Open();
            string Substage = string.Empty;
            string breakTime = "0";
            //if (Break15.Checked == true) breakTime = "15";
            //else if (Break30.Checked == true) breakTime = "30";
            //else if (Break20.Checked == true) breakTime = "20";
            //else 
            breakTime = "0";

            //sql_qry = "update PrmsProductionHour_Backup set empno = '" + txt_EmpNo.Text + "',EmpName = '" + hdn_EmpName.Value + "',Shift='" + ddl_Shift.SelectedValue + "',Designation = '" + txtDesignation.Text + "',ProjectID = '" + ddl_ProjectID.SelectedItem.Text + "',Projectname='" + tbxProjectname.Text + "',stage='" + ddl_Stage.SelectedItem.Text + "', ActivitySubStage='" + Substage + "',TotalTime = '" + txt_TotalTime.Text + "',StartTime ='" + txt_StartTime.Text + "',EndTime = '" + txt_EndTime.Text + "',[Break1] ='" + breakTime + "',remarks='" + txt_Remarks.Text.Replace("'", "''") + "',teamname='" + lbl_TeamName.Text + "',projectidtemp='" + projectidtempE2 + "',projectnametemp='" + projectnametempE + "' where slno ='" + txtslno1.Text + "'";
            //SqlCommand com2 = new SqlCommand(sql_qry, con);
            //com2.ExecuteNonQuery();
            //con.Close();

            nooferr = 0;
            ddl_ProjectID.Focus();

            clear();
            txt_StartTime.Text = "";
            if (lbl_StartTime.InnerText == "") lbl_StartTime.InnerText = txt_StartTime.Text;
            bindDPR();
            clear();
            ddl_ProjectID.Focus();

            //LinkButton1.Text = "View";

            nooferr = 0;
        }
        else Response.Write("<body onload='javascript:popup()'>");
    }
    protected void bindStage()
    {
        try
        {
            if (ddl_ProjectID.SelectedValue == "NA")
            {
                bindDefaultStage();
                bindDefaultScope();
            }
            else
            {
                ddl_Stage.Items.Clear();
                ddl_Scope.Items.Clear();
                con.Close();
                con.Open();
                if (ddl_EmpNo.Enabled == true)
                {
                    SqlDataAdapter sda = new SqlDataAdapter("select b.Stage,b.slno,a.stageid from tbl_MstStageMaster b inner join tbl_taskmaster a on a.stageid=b.slno and a.taskstatus!='Completed' where a.projectid='" + ddl_ProjectID.SelectedItem.Value + "' and a.userid='" + ddl_EmpNo.SelectedValue + "' and a.status=1 group by Stage,slno,stageid", con);
                    if (ViewState["UpdationCompleted"].ToString() == "Y")
                    {
                        sda = new SqlDataAdapter("select b.Stage,b.slno,a.stageid from tbl_MstStageMaster b inner join tbl_taskmaster a on a.stageid=b.slno where a.projectid='" + ddl_ProjectID.SelectedItem.Value + "' and a.userid='" + ddl_EmpNo.SelectedValue + "' and a.status=1 group by Stage,slno,stageid", con);
                    }
                    DataSet ds = new DataSet();
                    sda.Fill(ds);
                    ddl_Stage.DataSource = ds;
                    ddl_Stage.DataTextField = "Stage";
                    ddl_Stage.DataValueField = "slno";
                    ddl_Stage.DataBind();
                    con.Close();
                }
                else
                {
                    SqlDataAdapter sda = new SqlDataAdapter("select b.Stage,b.slno,a.stageid from tbl_MstStageMaster b inner join tbl_taskmaster a on a.stageid=b.slno and a.taskstatus!='Completed' where a.projectid='" + ddl_ProjectID.SelectedItem.Value + "' and a.userid='" + txt_EmpNo.Text + "' and a.status=1 group by Stage,slno,stageid", con);
                    if (ViewState["UpdationCompleted"].ToString() == "Y")
                    {
                        sda = new SqlDataAdapter("select b.Stage,b.slno,a.stageid from tbl_MstStageMaster b inner join tbl_taskmaster a on a.stageid=b.slno  where a.projectid='" + ddl_ProjectID.SelectedItem.Value + "' and a.userid='" + txt_EmpNo.Text + "' and a.status=1 group by Stage,slno,stageid", con);
                    }
                    DataSet ds = new DataSet();
                    sda.Fill(ds);
                    ddl_Stage.DataSource = ds;
                    ddl_Stage.DataTextField = "Stage";
                    ddl_Stage.DataValueField = "slno";
                    ddl_Stage.DataBind();
                    con.Close();
                }

            }
        }
        catch (Exception)
        {


        }

        ddl_Stage.Items.Insert(0, new ListItem("Select", "N/A"));
    }
    protected void bindDefaultStage()
    {
        try
        {
            ddl_Scope.Items.Clear();
            ddl_Stage.Items.Clear();
            con.Close();
            con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("Select Stage,slno from tbl_MstStageMaster where projectid='" + ddl_ProjectID.SelectedValue + "'", con);
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
    }
    protected void bindDefaultScope()
    {
        try
        {
            ddl_Scope.Items.Clear();
            con.Close();
            con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("Select Scope[scopereq],ID,Scope from tbl_scope where projectid='" + ddl_ProjectID.SelectedValue + "'", con);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            ddl_Scope.DataSource = ds;
            ddl_Scope.DataTextField = "Scopereq";
            ddl_Scope.DataValueField = "ID";
            ddl_Scope.DataBind();
            con.Close();
        }
        catch (Exception)
        {


        }

        ddl_Scope.Items.Insert(0, new ListItem("Select", "N/A"));
    }
    protected void ddl_ProjectID_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["UpdationCompleted"] = "N";
        bindStage();
        ddl_Stage.Focus();
    }
    protected void rbl_BreakTime_SelectedIndexChanged1(object sender, EventArgs e)
    {
        try
        {
            calculateTime();
            dpr_gridbind();
            txt_MeetingTime.Focus();
        }
        catch (Exception)
        {
        }
    }
    protected void ddl_EmpNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            btn_SaveStatus.Enabled = true;
            EmpName = ddl_EmpNo.SelectedValue;
            dpr_gridbind();
            bindEmployeeName();
            bindProjects();
            ddl_ProjectID.Focus();
        }
        catch (Exception)
        {

        }
    }
    protected void dpr_gridbind()
    {
        try
        {
            DateTime CurrentDate = DateTime.ParseExact(txt_Date.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            con.Close();
            string Rights = Convert.ToString(Session["Rights"]);
            if (Rights != "Administrator" && Rights != "Team Leader")
            {
                EmpName = Session["userid"].ToString();
            }
            else
            {
                EmpName = ddl_EmpNo.SelectedValue;
            }
            str_Command = "select a.slno,a.EmpNo,a.Stage as StageID,a.task as TaskID,a.Scope as ScopeID,a.ProjectID as Project,a.CurrentDate as Date,a.StartTime as TimeStart,a.EndTime as TimeEnd,a.EmpNo+' - '+a.EmpName as Employee,a.Shift,Convert(varchar(10),CurrentDate,103)[CurrentDate],d.projectreq as ProjectID,b.Stage,c.Scope as Scope,(select e.taskname from tbl_taskmaster e where e.id=a.task and e.taskstatus!='Hold')[Task],a.statusoftask,a.StartTime,a.EndTime,a.Break1 as BreakTime,a.meetingtime,a.meetingremarks,a.TotalTime,a.Remarks from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid where a.EmpNo='" + EmpName + "' and a.CurrentDate='" + CurrentDate + "' order by a.slno";
            SqlDataAdapter da = new SqlDataAdapter(str_Command, con);
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
    protected void grd_DPR_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                int id = (int)grd_DPR.DataKeys[e.RowIndex].Values[0];
                string projectID = (string)grd_DPR.DataKeys[e.RowIndex].Values[1].ToString();
                string taskID = (string)grd_DPR.DataKeys[e.RowIndex].Values[2].ToString();
                string EmpNo = (string)grd_DPR.DataKeys[e.RowIndex].Values[3].ToString();
                con.Open();
                com_DPR.Connection = con;
                com_DPR.CommandText = "delete from PrmsProductionHour_Backup where slno='" + id + "'";
                com_DPR.ExecuteNonQuery();
                com_DPR.CommandText = "select ProjectID from PrmsProductionHour_Backup where projectid='" + projectID + "'";
                SqlDataReader dr = com_DPR.ExecuteReader();
                if (dr.HasRows)
                {
                    dpr_gridbind();
                }
                else
                {
                    dr.Close();
                    com_DPR.CommandText = "update PrmsProductionHour_Backup set projectstatus='Yet To Start' where ProjectID='" + projectID + "'";
                    com_DPR.ExecuteNonQuery();
                }
                con.Close();
                con.Open();
                com_DPR.CommandText = "select task from PrmsProductionHour_Backup where task='" + taskID + "' and EmpNo='" + EmpNo + "'";
                SqlDataReader dr2 = com_DPR.ExecuteReader();
                if (dr2.HasRows)
                {
                    dpr_gridbind();
                }
                else
                {
                    dr2.Close();
                    com_DPR.CommandText = "update tbl_taskmaster set taskstatus='Yet To Start' where id='" + taskID + "' and userid='" + EmpNo + "'";
                    com_DPR.ExecuteNonQuery();
                    dpr_gridbind();
                }

            }
            else
            {

            }
        }
        catch (Exception)
        {
        }
    }

    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grd_DPR.PageIndex = e.NewPageIndex;
            this.dpr_gridbind();
        }
        catch (Exception)
        {

        }
    }
    protected void grd_DPR_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_MeetingTime.Enabled = true;
            ViewState["UpdationCompleted"] = "Y";
            GridViewRow row = grd_DPR.SelectedRow;
            if (EmpRights == "Administrator")
            {
                ddl_EmpNo.SelectedValue = ((HiddenField)row.FindControl("hdn_EmployeeID")).Value;
            }
            ddl_Shift.SelectedValue = ((Label)row.FindControl("lbl_Shift")).Text;
            txt_Date.Text = ((Label)row.FindControl("lbl_Date")).Text;
            hdn_Slno.Value = ((HiddenField)row.FindControl("hdn_Slno")).Value;
            bindCompletedProjects();
            ddl_ProjectID.SelectedValue = ((HiddenField)row.FindControl("hdn_ProjectID")).Value;
            bindStage();
            ddl_Stage.SelectedValue = ((HiddenField)row.FindControl("hdn_StageID")).Value;
            if (ddl_ProjectID.SelectedItem.Text != "No-Project")
            {
                bindScope();
            }
            ddl_Scope.SelectedValue = ((HiddenField)row.FindControl("hdn_ScopeID")).Value;
            bindTask();
            
            ddl_Task.SelectedValue = ((HiddenField)row.FindControl("hdn_TaskID")).Value;
            ddl_CompletedTask.SelectedValue = ((HiddenField)row.FindControl("hdn_StatusOfTask")).Value;
            string StartTime = ((HiddenField)row.FindControl("hdn_StartTime")).Value;
            StartTime = StartTime.Substring(11);
            txt_StartTime.Text = StartTime;
            string EndTime = ((HiddenField)row.FindControl("hdn_EndTime")).Value;
            EndTime = EndTime.Substring(11);
            txt_EndTime.Text = EndTime;
            if (((Label)row.FindControl("lbl_Break")).Text != "0")
            {
                rbl_BreakTime.SelectedValue = ((Label)row.FindControl("lbl_Break")).Text;
            }
            if (((Label)row.FindControl("lbl_MeetingTime")).Text != "" && ((Label)row.FindControl("lbl_MeetingTime")).Text != "__:__" && ((Label)row.FindControl("lbl_MeetingTime")).Text != "null")
            {
                txt_MeetingTime.Text = ((Label)row.FindControl("lbl_MeetingTime")).Text;
            }
            if (((Label)row.FindControl("lbl_MeetingRemarks")).Text != "" && ((Label)row.FindControl("lbl_MeetingRemarks")).Text != "")
            {
                txt_MeetingRemarks.Text = ((Label)row.FindControl("lbl_MeetingRemarks")).Text;
            }
            txt_TotalTime.Text = ((Label)row.FindControl("lbl_TotalTime")).Text;
            txt_Remarks.Text = ((Label)row.FindControl("lbl_Remarks")).Text;
            btn_SaveStatus.Enabled = true;
            //rbl_Status.SelectedValue = ((HiddenField)row.FindControl("hdn_Status")).Value;
            //txt_releivedDate.Text = ((HiddenField)row.FindControl("hdn_releiveddate")).Value;
            inserVal = 2;
            this.block_Grid.Visible = false;
            this.block_Register.Visible = true;
            ViewState["update"] = inserVal;
            btn_SaveStatus.Text = "Update";
        }
        catch (Exception)
        {
            Response.Redirect(Request.Url.AbsoluteUri);
        }

    }

    protected void btn_Back_Click(object sender, EventArgs e)
    {
        Response.Redirect("Dashboard.aspx");
    }
    protected void btn_Reset_Click(object sender, EventArgs e)
    {
        clear();
        dpr_gridbind();
    }
    protected void ddl_CompletedTask_SelectedIndexChanged(object sender, EventArgs e)
    {
        dpr_gridbind();
        ddl_Shift.Focus();
    }
    protected void ddl_Task_SelectedIndexChanged(object sender, EventArgs e)
    {
        dpr_gridbind();
        ddl_CompletedTask.Focus();
    }
    protected void bindTaskData()
    {
        TaskID = Session["TaskID"].ToString();
        con2.Close();
        con2.Open();
        SqlCommand cmd = new SqlCommand("select id,userid,projectid,stageid,scopeid from tbl_taskmaster where id='" + TaskID + "'", con2);
        SqlDataReader dr_Task = cmd.ExecuteReader();
        if (dr_Task.HasRows)
        {
            while (dr_Task.Read())
            {
                bindEmployees();
                ddl_EmpNo.SelectedValue = dr_Task["userid"].ToString();
                ddl_EmpNo_SelectedIndexChanged(this, EventArgs.Empty);
                bindProjects();
                ddl_ProjectID.SelectedValue = dr_Task["projectid"].ToString();
                ViewState["UpdationCompleted"] = "N";
                bindStage();
                bindScope();
                ddl_Stage.SelectedValue = dr_Task["stageid"].ToString();
                ddl_Scope.SelectedValue = dr_Task["scopeid"].ToString();
                bindTask();
                ddl_Task.SelectedValue = dr_Task["id"].ToString();
            }
        }
    }
    protected void grd_DPR_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // Retrieve the underlying data item. In this example
            // the underlying data item is a DataRowView object. 
            DataRowView rowView = (DataRowView)e.Row.DataItem;
            // Retrieve the state value for the current row. 
            String Task = rowView["Task"].ToString();
            if (Task == "")
            {
                (e.Row.FindControl("lbl_Task") as Label).Text = "Others";
            }
        }
                
    }
}