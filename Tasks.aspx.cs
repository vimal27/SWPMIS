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


public partial class Tasks : System.Web.UI.Page
{
    /// <summary>
    /// Declarations Part For Variables,Strings,SqlConnections,etc
    /// </summary>
    IFormatProvider culture = new CultureInfo("en-US", true);
    string TaskIDHided;
    bool taskChanged = false;
    string ProjectID;
    string status_update, ProjectAlloted, Rights;
    StringBuilder htmlTable = new StringBuilder();
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString());
    SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DCSconn"].ToString());
    SqlConnection con2 = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ToString());
    SqlCommand com_insert = new SqlCommand();
    SqlCommand com_update = new SqlCommand();
    SqlCommand com_check = new SqlCommand();
    SqlCommand com = new SqlCommand();
    SqlCommand com_task = new SqlCommand();
    SqlCommand com_scope = new SqlCommand();
    ProductionCalculation pcData = new ProductionCalculation();
    clsDataControl objData = new clsDataControl();
    SqlDataReader dr;
    bool isInsert;
    int inserVal = 1;
    string Color11 = string.Empty;
    string request_id = string.Empty;
    string updateMode = string.Empty;
    string highbook0, highbook1, highbook2, highbook3, highbook4, Math1, Journals, books1, Q2ID, Others;

    /// <summary>
    /// Page_s the load.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "sesdftFilters", "setFilters();", true);
            ProjectID = (Convert.ToString(Request.QueryString["projectid"]));
            if (Request.UrlReferrer == null)
            {
                Response.Redirect("Login");

            }
            //bindProject();
            if (!IsPostBack)
            {
                bindProject();
                bindYearOfRecords();
                bindMonthOfRecords();
                ddl_FilterYear.SelectedValue = Convert.ToString(DateTime.Today.Year);
                ddl_FilterMonth.SelectedValue = Convert.ToString(DateTime.Today.Month);
                ddl_FilterYear_SelectedIndexChanged(sender, e);
                tasks_gridbind();
            }
            Rights = Convert.ToString(Session["Rights"]);
            if (Rights == "Administrator" || Rights == "Team Leader")
            {
                btn_AddRating.Enabled = true;
                btn_AddQualityRating.Enabled = true;
            }
            else
            {
                btn_AddQualityRating.Enabled = false;
                btn_AddRating.Enabled = false;
            }
            txt_RequiredDate.Attributes.Add("readonly", "readonly");
            txt_RequestDate.Attributes.Add("readonly", "readonly");
            if (txt_TaskName.Text == "")
            {
                txt_TaskName.Focus();
            }
            if (Session["Userid"] == null) Response.Redirect("Login");
            else if (Session["Userid"].ToString() == "") Response.Redirect("Login");
            if (!Page.IsPostBack)
            {
                ddl_Project.Focus();
                this.block_Register.Visible = false;
                generateID();
                this.Page.Title = "Tasks";
                btn_Allot.Attributes.Add("onclick", "return validate()");

                if (Session["sessiontype"] != "PM" && Convert.ToString(Session["sessiontype"]).ToLower() != "leader" && Convert.ToString(Session["sessiontype"]).ToLower() != "admin")
                {
                    btn_Allot.Enabled = false;
                    bindProject();
                }
                else
                {
                    btn_Allot.Enabled = true;
                }

            }

        }
        catch (Exception)
        {
        }


    }

    /// <summary>
    /// Generates the UniqueID For Each Tasks.
    /// </summary>
    protected void generateID()
    {
        try
        {
            Int32 count;
            SqlCommand cmd = new SqlCommand("select Top 1(Right(requestid,4))as count from tbl_taskmaster where requestid like 'T" + "%' order by requestid desc", con);
            if (con.State == ConnectionState.Open)
                con.Close();
            if (con.State == ConnectionState.Closed)
                con.Open();
            count = Convert.ToInt32(cmd.ExecuteScalar());
            if (count > 0)
            {
                count++;
            }
            else
            {
                count = 0001;
            }
            request_id = "T";
            string id = String.Format("{0:D4}", count);
            request_id += id;
            count += 1;

            txt_RequestNo.Text = request_id;
            hdn_RequestNo.Value = request_id;
            con1.Close();
            txt_RequestNo.Attributes.Add("readonly", "readonly");
        }
        catch (Exception)
        {

        }
        //finally
        //{
        //    con.Close();
        //}
    }

    /// <summary>
    /// Binds the project Based on user log in.
    /// </summary>
    protected void bindProject()
    {
        try
        {
            string Rights = Convert.ToString(Session["Rights"]);
            ddl_Project.Items.Clear();
            con.Close();
            con.Open();

            SqlDataAdapter sda = new SqlDataAdapter("Select projectid,projectname,projectreq from tbl_projectReq where projectstatus!='Hold' and projectstatus!='Closed' and projectstatus!='Completed' and status!=0 and projectid!='NA'", con);
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
                string[] SplittedProjectAlloted = ProjectAlloted.Split(',');
                com_check.Connection = con;
                com_check.CommandText = "Select projectid,projectname,projectreq from tbl_projectReq where projectstatus!='Hold' and projectstatus!='Closed' and projectstatus!='Completed' and status!=0 and projectid!='NA' and (";
                foreach (string Splitted in SplittedProjectAlloted)
                {
                    com_check.CommandText += "allotedteamid like '%" + Splitted + "%' or ";
                }
                com_check.CommandText = com_check.CommandText.Substring(0, com_check.CommandText.Length - 3);
                com_check.CommandText += ")";
                sda = new SqlDataAdapter(com_check.CommandText, con);
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



    }

    /// <summary>
    /// Binds the stage Based on project.
    /// </summary>
    protected void bindStage()
    {
        try
        {
            ddl_Stage.Items.Clear();
            con.Close();
            con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("Select Stage,slno from tbl_MstStageMaster where projectid='" + ddl_Project.SelectedValue + "' and Delstage!=1", con);
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
    /// Binds the scope based on project.
    /// </summary>
    protected void bindScope()
    {
        try
        {
            ddl_Scope.Items.Clear();
            con.Close();
            con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("Select Scope[scopereq],ID,Scope from tbl_scope where projectid='" + ddl_Project.SelectedValue + "' and scopestatus!=0", con);
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

    /// <summary>
    /// Binds the users based on task alloted to.
    /// </summary>
    protected void bindUsers()
    {
        try
        {
            ddl_Users.Items.Clear();
            con.Close();
            con.Open();
            SqlCommand cmd = new SqlCommand("select userid,username from tbl_teamAllotmentMaster where teamid='" + ddl_Team.SelectedValue + "' and status=1", con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                string userid = dr["userid"].ToString();
                string username = dr["username"].ToString();
                string[] SplittedUserID = userid.ToString().Split(',');
                string[] SplittedUserName = username.ToString().Split(',');
                Dictionary<string, string> users = new Dictionary<string, string>();
                for (int i = 0; i < SplittedUserID.Length; i++)
                {
                    users.Add(SplittedUserID[i], SplittedUserID[i] + " - " + SplittedUserName[i]);
                    ddl_Users.DataSource = users;
                    ddl_Users.DataTextField = "Value";
                    ddl_Users.DataValueField = "Key";
                    ddl_Users.DataBind();
                }
            }
            dr.Close();
            con.Close();
            ddl_Users.Items.Insert(0, new ListItem("Select", "N/A"));
        }
        catch (Exception)
        {


        }

    }

    /// <summary>
    /// Binds the team based on project alloted to team
    /// </summary>
    protected void bindTeam()
    {
        try
        {
            ddl_Team.Items.Clear();
            con.Close();
            con.Open();
            SqlCommand cmd = new SqlCommand("select allotedteamid,allotedteamname from tbl_ProjectReq where projectid='" + ddl_Project.SelectedValue + "'", con);
            SqlDataReader dr = cmd.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                string teamid = dr["allotedteamid"].ToString();
                string teamname = dr["allotedteamname"].ToString();
                string[] splittedteamid = teamid.Split(',');
                string[] splittedteamname = teamname.Split(',');
                Dictionary<string, string> teams = new Dictionary<string, string>();


                for (int i = 0; i < splittedteamid.Length; i++)
                {
                    teams.Add(splittedteamid[i], splittedteamname[i]);
                    ddl_Team.DataSource = teams;
                    ddl_Team.DataTextField = "Value";
                    ddl_Team.DataValueField = "Key";
                    ddl_Team.DataBind();
                }
            }
            dr.Close();
            con.Close();
            ddl_Team.Items.Insert(0, new ListItem("Select", "N/A"));
        }
        catch (Exception)
        {


        }
    }

    /// <summary>
    /// Ddl_project_selected index changed event to bindstage,scope,team based on selected project.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void ddl_Project_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_Project.SelectedValue != "NA" && ddl_Project.SelectedValue != "N/A")
        {
            btn_NewStage.Visible = true;
            btn_NewScope.Visible = true;
        }
        else
        {
            btn_NewStage.Visible = false;
            btn_NewScope.Visible = false;
        }
        bindStage();
        bindScope();
        bindTeam();
    }
    protected void ddl_Scope_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// Insert New Stage IF Needed
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void btn_Stage_Click(object sender, EventArgs e)
    {
        com_insert.Connection = con;
        con.Close();
        con.Open();
        string ifExists = objData.GetSingleData("select Stage from tbl_MstStageMaster where Stage='" + txt_Stage.Text + "' and projectid='" + ddl_Project.SelectedValue + "'");
        if (ifExists != "0")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "mismatch", "swal('Stage Already Exists','','warning')", true);
        }
        else
        {
            com_insert.CommandText = "insert into tbl_MstStageMaster(Stage,status,projectid,projectname,Delstage) values(@Stage,'1',@projectid,@projectname,'0')";
            com_insert.Parameters.Add("@Stage", txt_Stage.Text.ToString());
            com_insert.Parameters.Add("@projectid", ddl_Project.SelectedValue.ToString());
            com_insert.Parameters.Add("@projectname", ddl_Project.SelectedItem.Text.ToString());
            com_insert.Connection = con;
            con.Close();
            con.Open();
            com_insert.ExecuteNonQuery();
            bindStage();
        }
        txt_Stage.Text = string.Empty;
    }

    /// <summary>
    /// Cancel Adding new stage
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void btn_CancelStage_Click(object sender, EventArgs e)
    {
        txt_Stage.Text = string.Empty;
    }

    /// <summary>
    /// Add New Scope If Needed
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void btn_Scope_Click(object sender, EventArgs e)
    {
        com_insert.Connection = con;
        con.Close();
        con.Open();
        string ifExistsScope = objData.GetSingleData("select Scope from tbl_scope where scope='" + txt_Scope.Text + "' and projectid='" + ddl_Project.SelectedValue + "'");
        if (ifExistsScope != "0")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "mismatch", "swal('Scope Already Exists','','warning')", true);
        }
        else
        {
            com_insert.CommandText = "insert into tbl_scope(Scope,projectid,Description,scopestatus) values(@scope,@projectid,@description,'1')";
            com_insert.Parameters.Add("@Stage", txt_Stage.Text.ToString());
            com_insert.Parameters.Add("@scope", txt_Scope.Text.ToString());
            com_insert.Parameters.Add("@projectid", ddl_Project.SelectedValue.ToString());
            com_insert.Parameters.Add("@description", txt_Description.Text.ToString());
            com_insert.Connection = con;
            con.Close();
            con.Open();
            com_insert.ExecuteNonQuery();
            bindScope();
        }
        txt_Scope.Text = string.Empty;
        txt_Description.Text = string.Empty;
    }

    /// <summary>
    /// Cancel Adding New Scope
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void btn_CancelScope_Click(object sender, EventArgs e)
    {
        txt_Scope.Text = string.Empty;
        txt_Description.Text = string.Empty;
    }

    /// <summary>
    /// Allot Task Button Click To Insert Task Or Update
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void btn_Allot_Click(object sender, EventArgs e)
    {
        try
        {
            //ScriptManager.RegisterStartupScript(this, GetType(), "RemoveClickedProperty", "isSubmitted=false;", true);
            //btn_Allot.Attributes.Add("onclick", "if (Page_ClientValidate()) {this.value=\"Processing...\";this.disabled=true;" + btn_Allot.Page.ClientScript.GetPostBackEventReference(btn_Allot, "").ToString() + "}");
            CultureInfo provider = CultureInfo.InvariantCulture;
            DateTime requestdt = DateTime.ParseExact(txt_RequestDate.Text, "dd/MM/yyyy", provider);
            DateTime requiredt = DateTime.ParseExact(txt_RequiredDate.Text, "dd/MM/yyyy", provider);
            string[] EstimatedTime = txt_EstimatedHours.Text.ToString().Split(':');
            if (requestdt.Date > requiredt.Date)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "mismatch", "isSubmitted=false;swal('Required Date must not be less than Request Date');isSubmitted=false;", true);
                //ScriptManager.RegisterStartupScript(Page, GetType(), "Alert", "alert('Required Date must not be less than Request Date');", true);
                return;
            }
            else if (Convert.ToInt32(EstimatedTime[1]) > 59)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "mismatch", "isSubmitted=false;swal('Estimated Hours in Invalid Format!');isSubmitted=false;", true);
                //ScriptManager.RegisterStartupScript(Page, GetType(), "myscript", "alert('Endtime is Invalid')", true);
                txt_EstimatedHours.Text = "";
            }
            else
            {
                int inserValCon = Convert.ToInt32(ViewState["Insert"]);
                if (inserValCon == 2)
                {
                    con.Close();
                    con.Open();
                    com_update.Connection = con;
                    com_update.CommandText = "update tbl_taskmaster set requestid=@requestid,taskname=@taskname,taskdescription=@taskdescription,projectid=@projectid,stageid=@stageid,scopeid=@scopeid,teamid=@teamid,userid=@userid,requestdate=@requestdate,requireddate=@requireddate,estimatedtime=@estimatedtime where requestid = '" + txt_RequestNo.Text + "' and id ='" + hdn_TaskNo.Value + "'";
                    com_update.Parameters.Add("@requestid", Convert.ToString(txt_RequestNo.Text));
                    com_update.Parameters.Add("@taskname", Convert.ToString(txt_TaskName.Text));
                    com_update.Parameters.Add("@taskdescription", Convert.ToString(txt_TaskDescription.Text));
                    com_update.Parameters.Add("@projectid", Convert.ToString(ddl_Project.SelectedValue));
                    com_update.Parameters.Add("@stageid", Convert.ToString(ddl_Stage.SelectedValue));
                    com_update.Parameters.Add("@scopeid", Convert.ToString(ddl_Scope.SelectedValue));
                    com_update.Parameters.Add("@teamid", Convert.ToString(ddl_Team.SelectedValue));
                    com_update.Parameters.Add("@userid", Convert.ToString(ddl_Users.SelectedValue));
                    if (txt_RequestDate.Text != "")
                    {
                        com_update.Parameters.Add("@requestdate", Convert.ToString(DateTime.ParseExact(txt_RequestDate.Text.Trim(), "dd/MM/yyyy", null).ToString("MM/dd/yyyy")));
                    }
                    if (txt_RequiredDate.Text != "")
                    {
                        com_update.Parameters.Add("@requireddate", Convert.ToString(DateTime.ParseExact(txt_RequiredDate.Text.Trim(), "dd/MM/yyyy", null).ToString("MM/dd/yyyy")));
                    }
                    com_update.Parameters.Add("@estimatedtime", Convert.ToString(txt_EstimatedHours.Text));
                    con.Close();
                    con.Open();
                    com_update.ExecuteNonQuery();
                    Clear();
                    ScriptManager.RegisterStartupScript(this, GetType(), "mismatchedInsert", "isSubmitted=false;swal('Task Updated Successfully','','success');setDatatable();", true);
                    tasks_gridbind();
                    block_Register.Visible = false;
                    block_Grid.Visible = true;
                    lbl_SeparatorMonthYear.Visible = true;
                    ddl_FilterMonth.Visible = true;
                    ddl_FilterYear.Visible = true;
                    //upl_Filter.Update();
                }
                else
                {
                    com_insert.Connection = con;
                    con.Close();
                    con.Open();
                    com_insert.CommandText = "insert into tbl_taskmaster(requestid,taskname,taskdescription,projectid,stageid,scopeid,teamid,userid,requestdate,requireddate,estimatedtime,status,taskstatus) values(@requestid,@taskname,@taskdescription,@projectid,@stageid,@scopeid,@teamid,@userid,@requestdate,@requireddate,@estimatedtime,1,'" + "Yet To Start" + "')";
                    com_insert.Parameters.Add("@requestid", Convert.ToString(txt_RequestNo.Text));
                    com_insert.Parameters.Add("@taskname", Convert.ToString(txt_TaskName.Text));
                    com_insert.Parameters.Add("@taskdescription", Convert.ToString(txt_TaskDescription.Text));
                    com_insert.Parameters.Add("@projectid", Convert.ToString(ddl_Project.SelectedValue));
                    com_insert.Parameters.Add("@stageid", Convert.ToString(ddl_Stage.SelectedValue));
                    com_insert.Parameters.Add("@scopeid", Convert.ToString(ddl_Scope.SelectedValue));
                    com_insert.Parameters.Add("@teamid", Convert.ToString(ddl_Team.SelectedValue));
                    com_insert.Parameters.Add("@userid", Convert.ToString(ddl_Users.SelectedValue));
                    if (txt_RequestDate.Text != "")
                    {
                        com_insert.Parameters.Add("@requestdate", Convert.ToString(DateTime.ParseExact(txt_RequestDate.Text.Trim(), "dd/MM/yyyy", null).ToString("MM/dd/yyyy")));
                    }
                    if (txt_RequiredDate.Text != "")
                    {
                        com_insert.Parameters.Add("@requireddate", Convert.ToString(DateTime.ParseExact(txt_RequiredDate.Text.Trim(), "dd/MM/yyyy", null).ToString("MM/dd/yyyy")));
                    }
                    com_insert.Parameters.Add("@estimatedtime", Convert.ToString(txt_EstimatedHours.Text));
                    com_insert.ExecuteNonQuery();
                    Clear();
                    ScriptManager.RegisterStartupScript(this, GetType(), "mismatchedRemove", "isSubmitted=false;swal('Task Alloted Successfully','','success');setDatatable();isSubmitted=false;", true);
                    tasks_gridbind();
                    block_Register.Visible = false;
                    block_Grid.Visible = true;
                    lbl_SeparatorMonthYear.Visible = true;
                    ddl_FilterMonth.Visible = true;
                    ddl_FilterYear.Visible = true;
                    //upl_Filter.Update();
                }
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "mismatchasdfas", "isSubmitted=false;", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "mismatchasdfasdf", "isSubmitted=false;", true);
        }
        finally
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "mismatchdfgedrfg", "isSubmitted=false;", true);
            con.Close();
        }
    }

    /// <summary>
    /// Clear this instance.
    /// </summary>
    protected void Clear()
    {
        ddl_Project.SelectedIndex = 0;
        ddl_Stage.SelectedIndex = 0;
        ddl_Team.SelectedIndex = 0;
        ddl_Scope.SelectedIndex = 0;
        ddl_Users.SelectedIndex = 0;
        txt_TaskName.Text = string.Empty;
        txt_TaskDescription.Text = string.Empty;
        txt_RequestDate.Text = string.Empty;
        txt_RequiredDate.Text = string.Empty;
        txt_EstimatedHours.Text = string.Empty;
    }

    /// <summary>
    /// Reset Button Click to call clear function
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void btn_Reset_Click(object sender, EventArgs e)
    {
        Clear();
    }

    /// <summary>
    /// back button to redirect self
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void btn_Back_Click(object sender, EventArgs e)
    {
        try
        {
            this.block_Register.Visible = false;
            this.block_Grid.Visible = true;
            lbl_SeparatorMonthYear.Visible = true;
            ddl_FilterMonth.Visible = true;
            ddl_FilterYear.Visible = true;
            //upl_Filter.Update();
            //ScriptManager.RegisterStartupScript(this, GetType(), "setDatatable", "setDatatable();", true);
            ddl_FilterMonth_SelectedIndexChanged(null, null);
            //Response.Redirect(Request.Url.AbsoluteUri);
        }
        catch (Exception)
        {

        }
    }

    /// <summary>
    /// Export to Excel Function
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void btn_Export_Click(object sender, EventArgs e)
    {
        try
        {
            SqlDataAdapter da = new SqlDataAdapter();
            String Rights = Session["Rights"].ToString();
            com_task.Connection = con;
            con.Close();
            con.Open();
            com_task.CommandText = "select e.projectreq as Project,c.Scope as Scope,b.Stage as Stage,a.taskname[Task],d.userid+' - '+d.username as AllotedUser,CONVERT(VARCHAR(20),a.requestdate,103) AS RequestDate,CONVERT(VARCHAR(20),a.requireddate,103) AS RequiredDate,(select isNull(convert(Varchar(10),max(CurrentDate)),'TBD') from prmsProductionHour_Backup g where g.task=a.id)[CompletedDate],a.taskstatus as TaskStatus from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid inner join tbl_ProjectReq e on e.projectid=a.projectid ";
            com_task.CommandText += " inner join tbl_teams f on f.TeamID=a.teamid where ";
            if (ddl_FilterProject.SelectedIndex > 0 && ddl_FilterProject.SelectedValue != "-Select-")
                ProjectID = ddl_FilterProject.SelectedValue;
            if (!string.IsNullOrEmpty(ProjectID))
                com_task.CommandText += " a.projectid='" + ProjectID + "'";
            else
                com_task.CommandText += " ((Month(a.requireddate)='" + ddl_FilterMonth.SelectedValue + "' and YEAR(a.requireddate)='" + ddl_FilterYear.SelectedValue + "') or (Month(a.requestdate)='" + ddl_FilterMonth.SelectedValue + "' and YEAR(a.requestdate)='" + ddl_FilterYear.SelectedValue + "'))";
            com_task.CommandText += " order by a.id desc";
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
                string[] SplittedProjectAlloted = ProjectAlloted.Split(',');
                com_check.Connection = con;
                com_task.CommandText = "select e.projectreq[Project ID],c.Scope as Scope,b.Stage as Stage,a.taskname[Task],d.userid+' - '+d.username as AllotedUser,CONVERT(VARCHAR(20),a.requestdate,103) AS requestdate,CONVERT(VARCHAR(20),a.requireddate,103) AS requireddate,(select isNull(convert(Varchar(10),max(CurrentDate)),'TBD') from prmsProductionHour_Backup g where g.task=a.id)[CompletedDate],a.taskstatus[Task Status] from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid  inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_teams f on f.TeamID=a.teamid inner join tbl_ProjectReq e on e.projectid=a.projectid ";

                com_task.CommandText += "  and (";

                foreach (string Splitted in SplittedProjectAlloted)
                {
                    com_task.CommandText += "e.allotedteamid like '%" + Splitted + "%' or ";
                }
                com_task.CommandText = com_task.CommandText.Substring(0, com_task.CommandText.Length - 3);
                com_task.CommandText += ") and e.projectid!='NA' where ";
                if (ddl_FilterProject.SelectedIndex > 0 && ddl_FilterProject.SelectedValue != "-Select-")
                    ProjectID = ddl_FilterProject.SelectedValue;
                if (!string.IsNullOrEmpty(ProjectID))
                    com_task.CommandText += " and a.projectid='" + ProjectID + "'";
                else
                    com_task.CommandText += " ((Month(a.requireddate)='" + ddl_FilterMonth.SelectedValue + "' and YEAR(a.requireddate)='" + ddl_FilterYear.SelectedValue + "') or (Month(a.requestdate)='" + ddl_FilterMonth.SelectedValue + "' and YEAR(a.requestdate)='" + ddl_FilterYear.SelectedValue + "'))";
                com_task.CommandText += " order by a.id desc";
            }
            da = new SqlDataAdapter(com_task.CommandText, con);
            DataTable dts = new DataTable();
            da.Fill(dts);
            if (dts.Rows.Count > 0)
            {
                List<string> HeaderNames = new List<string>();
                HeaderNames.Add("Project");
                HeaderNames.Add("Scope");
                HeaderNames.Add("Stage");
                HeaderNames.Add("Task");
                HeaderNames.Add("Alloted User");
                HeaderNames.Add("Request Date");
                HeaderNames.Add("Required Date");
                HeaderNames.Add("Completed Date");
                HeaderNames.Add("Task Status");
                List<string> ExcelReport = pcData.generateExcelReport(dts, "TaskDetails", "GenericReports", "Task Details", 9, HeaderNames);
                FileInfo file = new FileInfo(ExcelReport[2]);
                Response.Clear();
                Response.AddHeader("Content-Length", file.Length.ToString(CultureInfo.InvariantCulture));
                Response.AddHeader("Content-Disposition", "attachment;filename=\"" + ("TaskDetails.xls") + "\"");
                Response.ContentType = "application/octet-stream";
                Response.Flush();
                Response.TransmitFile(ExcelReport[0] + ("TaskDetails" + DateTime.Now.ToShortDateString().Replace("/", "") + ".xls"));
                Response.End();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "mismatch", "swal('" + ex.Message + "')", true);
        }
    }

    /// <summary>
    /// Gridview Bind With All tasks
    /// </summary>
    protected void tasks_gridbind()
    {
        try
        {
            con.Close();
            ph_Filters.Visible = true;
            (ph_Filters.Controls[0] as DropDownList).Enabled = true;
            (ph_Filters.Controls[2] as DropDownList).Enabled = true;
            (ph_Filters.Controls[3] as DropDownList).Enabled = true;
            SqlDataAdapter da = new SqlDataAdapter();
            string Rights = Convert.ToString(Session["Rights"]);
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
                string[] SplittedProjectAlloted = ProjectAlloted.Split(',');
                com_check.Connection = con;
                com_task.CommandText = "select a.id,a.requestid,a.taskstatus,a.taskname,a.taskdescription,a.projectid,a.stageid,a.scopeid,a.teamid,a.userid,CONVERT(VARCHAR(20),a.requestdate,103) AS requestdate,CONVERT(VARCHAR(20),a.requireddate,103) AS requireddate,a.estimatedtime,b.Stage as Stage,c.Scope as Scope,f.Teamname as AllotedTeam,d.username as Username,e.projectreq as Project,a.Production from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid  inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_teams f on f.TeamID=a.teamid inner join tbl_ProjectReq e on e.projectid=a.projectid ";
                if (!string.IsNullOrEmpty(ProjectID))
                    com_task.CommandText += " and a.projectid='" + ProjectID + "'";
                com_task.CommandText += "  and (";
                foreach (string Splitted in SplittedProjectAlloted)
                {
                    com_task.CommandText += "e.allotedteamid like '%" + Splitted + "%' or ";
                }
                com_task.CommandText = com_task.CommandText.Substring(0, com_task.CommandText.Length - 3);
                string teamUsers = objData.GetSingleData("select ''''+REPLACE(userid,',',''',''')+''''[users] from tbl_teamAllotmentMaster where teamleader='" + Convert.ToString(Session["UserID"]) + "'");
                com_task.CommandText += ") and e.projectid!='NA' where a.userID in(" + teamUsers + ")";
                if (ddl_FilterProject.SelectedIndex > 0 && ddl_FilterProject.SelectedValue != "-Select-")
                    ProjectID = ddl_FilterProject.SelectedValue;
                if (!string.IsNullOrEmpty(ProjectID))
                    com_task.CommandText += " and a.projectid='" + ProjectID + "'";
                else
                    com_task.CommandText += " and  ((Month(a.requireddate)='" + ddl_FilterMonth.SelectedValue + "' and YEAR(a.requireddate)='" + ddl_FilterYear.SelectedValue + "') or (Month(a.requestdate)='" + ddl_FilterMonth.SelectedValue + "' and YEAR(a.requestdate)='" + ddl_FilterYear.SelectedValue + "'))";
                com_task.CommandText += " order by a.id desc";
                da = new SqlDataAdapter(com_task.CommandText, con);

                DataSet ds = new DataSet();
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    grd_Tasks.DataSource = ds;
                    grd_Tasks.DataBind();
                }
                else
                {
                    ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                    grd_Tasks.DataSource = ds;
                    grd_Tasks.DataBind();
                    int columncount = grd_Tasks.Rows[0].Cells.Count;
                    grd_Tasks.Rows[0].Cells.Clear();
                    grd_Tasks.Rows[0].Cells.Add(new TableCell());
                    grd_Tasks.Rows[0].Cells[0].ColumnSpan = columncount;
                    grd_Tasks.Rows[0].Cells[0].Text = "<center>----- No Records Found -----</center>";
                }
            }
            else
            {

                com_task.Connection = con;
                con.Close();
                con.Open();
                com_task.CommandText = "select a.id,a.requestid,a.taskstatus,a.taskname,a.taskdescription,a.projectid,a.stageid,a.scopeid,a.teamid,a.userid,CONVERT(VARCHAR(20),a.requestdate,103) AS requestdate,CONVERT(VARCHAR(20),a.requireddate,103) AS requireddate,a.estimatedtime,b.Stage as Stage,c.Scope as Scope,f.Teamname as AllotedTeam,d.username as Username,e.projectreq as Project,a.Production from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid and e.projectid!='NA' inner join tbl_teams f on f.TeamID=a.teamid where ((Month(a.requireddate)='" + ddl_FilterMonth.SelectedValue + "' and YEAR(a.requireddate)='" + ddl_FilterYear.SelectedValue + "') or (Month(a.requestdate)='" + ddl_FilterMonth.SelectedValue + "' and YEAR(a.requestdate)='" + ddl_FilterYear.SelectedValue + "')) order by id desc";
                if (ddl_FilterProject.SelectedIndex > 0 && ddl_FilterProject.SelectedValue != "-Select-")
                    ProjectID = ddl_FilterProject.SelectedValue;
                if (!string.IsNullOrEmpty(ProjectID))
                {
                    com_task.CommandText = "select a.id,a.requestid,a.taskstatus,a.taskname,a.taskdescription,a.projectid,a.stageid,a.scopeid,a.teamid,a.userid,CONVERT(VARCHAR(20),a.requestdate,103) AS requestdate,CONVERT(VARCHAR(20),a.requireddate,103) AS requireddate,a.estimatedtime,b.Stage as Stage,c.Scope as Scope,f.Teamname as AllotedTeam,d.username as Username,e.projectreq as Project,a.Production from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid and e.projectid!='NA' and a.projectid='" + ProjectID + "' inner join tbl_teams f on f.TeamID=a.teamid where a.projectid='" + ddl_FilterProject.SelectedValue + "' order by id desc";
                }
                da.SelectCommand = com_task;
                DataSet ds = new DataSet();
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    grd_Tasks.DataSource = ds;
                    grd_Tasks.DataBind();
                }
                else
                {
                    ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                    grd_Tasks.DataSource = ds;
                    grd_Tasks.DataBind();
                    int columncount = grd_Tasks.Rows[0].Cells.Count;
                    grd_Tasks.Rows[0].Cells.Clear();
                    grd_Tasks.Rows[0].Cells.Add(new TableCell());
                    grd_Tasks.Rows[0].Cells[0].ColumnSpan = columncount;
                    grd_Tasks.Rows[0].Cells[0].Text = "<center>----- No Records Found -----</center>";
                }
            }
            ddl_FilterProject.Visible = true;
            if (taskChanged == false)
                bindProjectFilter();
        }
        catch (Exception)
        {
        }
    }

    /// <summary>
    /// Task status Change
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void grd_Tasks_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            String id = (string)grd_Tasks.DataKeys[e.RowIndex].Values[1].ToString();
            con.Open();
            com_scope.Connection = con;
            com_scope.CommandText = "select taskstatus from tbl_taskmaster where  id ='" + id + "'";
            SqlDataReader dr = com_scope.ExecuteReader();
            dr.Read();
            string State = dr[0].ToString();
            dr.Close();
            com_scope.CommandText = "insert into tbl_TaskHolds(holdeddate,previousstate,taskid) values (GETDATE(),'" + State + "','" + id + "')";
            //com_scope.CommandText = "delete from tbl_taskmaster where requestid='" + id + "'";
            com_scope.ExecuteNonQuery();
            com_scope.Connection = con;
            com_scope.CommandText = "update tbl_taskmaster set status=0,taskstatus='Hold' where  id ='" + id + "'";
            //com_scope.CommandText = "delete from tbl_taskmaster where requestid='" + id + "'";
            com_scope.ExecuteNonQuery();
            tasks_gridbind();
            ScriptManager.RegisterStartupScript(this, GetType(), "setDatatable", "setDatatable();", true);
        }
        catch (Exception)
        {
        }
    }
    protected void grd_Tasks_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

    }

    protected void grd_Tasks_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            grd_Tasks.EditIndex = -1;
            tasks_gridbind();
        }
        catch (Exception)
        {

            //throw;
        }
    }
    //protected void OnPaging(object sender, GridViewPageEventArgs e)
    //{

    //}

    /// <summary>
    /// New Task Add
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void btn_newTask_Click(object sender, System.EventArgs e)
    {
        Clear();
        btn_Allot.Text = "Allot";
        ScriptManager.RegisterStartupScript(this, GetType(), "sesdftFilters", "setFilters();", true);
        ph_Filters.Visible = false;
        (ph_Filters.Controls[0] as DropDownList).Enabled = false;
        (ph_Filters.Controls[2] as DropDownList).Enabled = false;
        (ph_Filters.Controls[3] as DropDownList).Enabled = false;
        generateID();
        inserVal = 3;
        ViewState["Insert"] = inserVal;
        try
        {
            this.block_Grid.Visible = false;
            lbl_SeparatorMonthYear.Visible = false;
            ddl_FilterMonth.Visible = false;
            ddl_FilterYear.Visible = false;
            this.block_Register.Visible = true;
            //upl_Filter.Update();
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
    protected void grd_Tasks_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            inserVal = 2;
            GridViewRow row = grd_Tasks.SelectedRow;
            hdn_RequestNo.Value = ((HiddenField)row.FindControl("lbl_RequestNo")).Value;
            hdn_TaskNo.Value = ((HiddenField)row.FindControl("hdn_taskid")).Value;
            txt_TaskName.Text = ((Label)row.FindControl("lbl_Task")).Text;
            txt_TaskDescription.Text = ((HiddenField)row.FindControl("hdn_TaskDescription")).Value;
            txt_RequestNo.Text = ((HiddenField)row.FindControl("lbl_RequestNo")).Value;
            ddl_Project.SelectedValue = ((HiddenField)row.FindControl("hdn_projectid")).Value;
            PostBackTrigger.Equals("true", "true");
            bindStage();
            bindScope();
            bindTeam();
            ddl_Stage.SelectedValue = ((HiddenField)row.FindControl("hdn_StageID")).Value;
            ddl_Scope.SelectedValue = ((HiddenField)row.FindControl("hdn_ScopeID")).Value;
            ddl_Team.SelectedValue = ((HiddenField)row.FindControl("hdn_TeamID")).Value;
            bindUsers();
            ddl_Users.SelectedValue = ((HiddenField)row.FindControl("hdn_UserID")).Value;
            txt_RequestDate.Text = ((Label)row.FindControl("lbl_RequestDate")).Text;
            string RequiredDateLimitStart = Convert.ToString(DateTime.ParseExact(txt_RequestDate.Text.Trim(), "dd/MM/yyyy", null).ToString("MM/dd/yyyy"));
            txt_RequiredDate.Text = ((Label)row.FindControl("lbl_RequiredDate")).Text;
            calext_RequiredDate.StartDate = DateTime.ParseExact(RequiredDateLimitStart, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            if (((HiddenField)row.FindControl("hdn_EstimatedHours")).Value != "")
            {
                txt_EstimatedHours.Text = ((HiddenField)row.FindControl("hdn_EstimatedHours")).Value;
            }
            this.block_Grid.Visible = false;
            lbl_SeparatorMonthYear.Visible = false;
            ddl_FilterMonth.Visible = false;
            ddl_FilterYear.Visible = false;
            this.block_Register.Visible = true;
            ViewState["Insert"] = inserVal;
            btn_Allot.Text = "Update";
            //upl_Filter.Update();
        }
        catch (Exception)
        {
            Response.Redirect(Request.Url.AbsoluteUri);
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
            grd_Tasks.PageIndex = e.NewPageIndex;
            this.tasks_gridbind();
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
    protected void OnPageIndexChangingScope(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grd_Tasks.PageIndex = e.NewPageIndex;
            this.bindScope();
        }
        catch (Exception)
        {

        }
    }
    protected void grd_Tasks_RowEditing(object sender, GridViewEditEventArgs e)
    {
    }

    /// <summary>
    /// dropdown selected index changed to select users based on selected team
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void ddl_Team_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindUsers();
    }

    //View task Details
    protected void SelectCurrentData(object sender, EventArgs e)
    {
        string TotalTime = string.Empty;
        grd_TaskHolds.Visible = false;
        title_TaskHolds.Visible = false;
        string TaskID;
        this.block_Grid.Visible = false;
        this.block_Register.Visible = false;
        this.block_View.Visible = true;
        LinkButton btn = (LinkButton)(sender);
        string requestid = btn.CommandArgument.ToString();
        DataTable dt = null;
        dt = objData.Getdata("select SUM(DATEDIFF(MINUTE,'00:00:00', b.TotalTime))/60 AS hh,SUM(DATEDIFF(MINUTE,'00:00:00', b.TotalTime))%60 as mm from PrmsProductionHour_Backup b where b.task='" + requestid + "'");
        TotalTime = dt.Rows[0]["hh"].ToString() + ':' + dt.Rows[0]["mm"].ToString();
        ViewState["RatingForTask"] = requestid;
        con.Close();
        con.Open();
        com_task.Connection = con;
        com_task.CommandText = "select a.requestid,a.id,a.taskname,a.taskdescription,a.projectid,a.stageid,a.scopeid,a.teamid,a.userid,CONVERT(VARCHAR(20),a.requestdate,103) AS requestdate,CONVERT(VARCHAR(20),a.requireddate,103) AS requireddate,a.estimatedtime,b.Stage as Stage,c.Scope+' - '+c.Description as Scope,f.Teamname as AllotedTeam,d.username as Username,e.projectreq as Project,a.RatingOfTask,a.QualityRating,a.taskstatus from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid inner join tbl_ProjectReq e on e.projectid=a.projectid inner join tbl_teams f on f.TeamID=a.teamid where a.id='" + requestid + "'";
        SqlDataReader dr = com_task.ExecuteReader();
        while (dr.Read())
        {
            TitleOfPage.InnerText = dr["requestid"].ToString() + " : " + dr["Project"];
            lbl_TaskIDTextView.InnerText = dr["requestid"].ToString();
            TaskID = dr["id"].ToString();
            lbl_TaskNameTextView.InnerText = dr["taskname"].ToString();
            lbl_TaskDescriptionTextView.InnerText = dr["taskdescription"].ToString();
            lbl_ProjectTextView.InnerText = dr["Project"].ToString();
            lbl_StageTextView.InnerText = dr["Stage"].ToString();
            lbl_ScopeTextView.InnerText = dr["Scope"].ToString();
            lbl_AllotedToTextView.InnerText = dr["Username"].ToString();
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
            if (dr["taskstatus"].ToString().ToLower() == "completed")
            {
                btn_AddQualityRating.Enabled = true;
                btn_AddRating.Enabled = true;
                string RatingSelected = dr["RatingOfTask"].ToString();
                string QualityRatingSelected = dr["QualityRating"].ToString();
                ScriptManager.RegisterStartupScript(this, GetType(), "mismatch", "$('#star-" + RatingSelected + "').attr('checked','checked');$('#qualitystar-" + QualityRatingSelected + "').attr('checked','checked')", true);
            }
            else
            {
                btn_AddQualityRating.Enabled = false;
                btn_AddRating.Enabled = false;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mismatch", "$('#star-" + 1 + "').attr('disabled','disabled');$('#star-" + 2 + "').attr('disabled','disabled');$('#star-" + 3 + "').attr('disabled','disabled');$('#star-" + 4 + "').attr('disabled','disabled');$('#star-" + 5 + "').attr('disabled','disabled');$('#qualitystar-" + 1 + "').attr('disabled','disabled');$('#qualitystar-" + 2 + "').attr('disabled','disabled');$('#qualitystar-" + 3 + "').attr('disabled','disabled');$('#qualitystar-" + 4 + "').attr('disabled','disabled');$('#qualitystar-" + 5 + "').attr('disabled','disabled');$('#star-" + 1 + "').css('cursor','not-allowed');$('#star-" + 2 + "').css('cursor','not-allowed');$('#star-" + 3 + "').css('cursor','not-allowed');$('#star-" + 4 + "').css('cursor','not-allowed');$('#star-" + 5 + "').css('cursor','not-allowed');$('#qualitystar-" + 1 + "').css('cursor','not-allowed');$('#qualitystar-" + 2 + "').css('cursor','not-allowed');$('#qualitystar-" + 3 + "').css('cursor','not-allowed');$('#qualitystar-" + 4 + "').css('cursor','not-allowed');$('#qualitystar-" + 5 + "').css('cursor','not-allowed');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mismatch", "", true);
            }
            //ScriptManager.RegisterStartupScript(this, GetType(), "mismatch", "$('#qualitystar-" + QualityRatingSelected + "').attr('checked','checked')", true);
        }
        dr.Close();
        com_task.CommandText = "select Convert(varchar(10),CurrentDate,103)[CurrentDate],a.EmpNo+' - '+a.EmpName as Employee,RIGHT(a.StartTime,5)[StartTime],RIGHT(a.EndTime,5)[EndTime],a.Break1 as BreakTime,a.TotalTime,a.Remarks,a.statusoftask from PrmsProductionHour_Backup a inner join tbl_MstStageMaster b on b.slno = a.Stage inner join tbl_scope c on c.ID = a.Scope inner join tbl_ProjectReq d on d.projectid=a.projectid where a.ProjectID!='NA' and a.task='" + requestid + "' order by a.CurrentDate";
        SqlDataAdapter da = new SqlDataAdapter(com_task.CommandText, con);
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
        com_task.CommandText = "select closeddate,* from tbl_TaskHolds where taskid='" + requestid + "'";
        con.Close();
        con.Open();
        SqlDataReader dr2 = com_task.ExecuteReader();
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
        com_task.CommandText = "select id,taskid,holdeddate,wipdate,closeddate from tbl_TaskHolds where taskid='" + requestid + "'";
        SqlDataAdapter da2 = new SqlDataAdapter(com_task.CommandText, con);
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
        //lbl_SeparatorMonthYear.Visible = false;
        //ddl_FilterMonth.Visible = false;
        //ddl_FilterYear.Visible = false;
        ddl_FilterMonth.Visible = false;
        ddl_FilterYear.Visible = false;
        lbl_SeparatorMonthYear.Visible = false;
        //upl_Filter.Update();upl_Filter

    }

    /// <summary>
    /// back button to redirect tasks
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void btn_BackButtonClick(object sender, EventArgs e)
    {
        this.block_Grid.Visible = true;
        lbl_SeparatorMonthYear.Visible = true;
        ddl_FilterMonth.Visible = true;
        ddl_FilterYear.Visible = true;
        this.block_View.Visible = false;
        this.block_Register.Visible = false;
        //upl_Filter.Update();
        this.TitleOfPage.InnerText = "Tasks";
        //ScriptManager.RegisterStartupScript(this, GetType(), "setDatatable", "setDatatable();", true);
        ddl_FilterMonth_SelectedIndexChanged(this, null);
    }


    /// <summary>
    /// Row Databound To Differentiate Task Status
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void grd_Tasks_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // Retrieve the underlying data item. In this example
            // the underlying data item is a DataRowView object. 
            DataRowView rowView = (DataRowView)e.Row.DataItem;
            // Retrieve the state value for the current row. 
            String status = rowView["taskstatus"].ToString();
            var lk_Edit = (LinkButton)e.Row.FindControl("lnk_Edit");
            var lk_Delete = (LinkButton)e.Row.FindControl("lnk_Delete");
            string Rights = Convert.ToString(Session["Rights"]);
            //format color of the as below 
            if (status == "WIP")
            {
                (e.Row.FindControl("lbl_CompletedDate") as Label).Text = "TBD";
                (e.Row.FindControl("lnk_TaskStatus") as LinkButton).CssClass = "btn btn-info";
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
                    (e.Row.FindControl("lbl_Production") as Label).Text = "TBD";
                    (e.Row.FindControl("lbl_Production") as Label).Style.Add("margin-left", "10px");
                }
            }
            if (status == "Hold")
            {
                (e.Row.FindControl("lbl_CompletedDate") as Label).Text = "TBD";
                (e.Row.FindControl("lnk_TaskStatus") as LinkButton).CssClass = "btn btn-warning";
                (e.Row.FindControl("lnk_TaskStatus") as LinkButton).Style.Add("cursor", "pointer");
                hdn_TaskNo.Value = (e.Row.FindControl("lnk_TaskStatus") as LinkButton).CommandArgument;
                lk_Edit.Visible = false;
                lk_Delete.Visible = false;
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
                    (e.Row.FindControl("lbl_Production") as Label).Text = "TBD";
                    (e.Row.FindControl("lbl_Production") as Label).Style.Add("margin-left", "10px");
                }
            }

            if (status == "Yet To Start")
            {
                (e.Row.FindControl("lbl_CompletedDate") as Label).Text = "TBD";
                (e.Row.FindControl("lnk_TaskStatus") as LinkButton).CssClass = "btn btn-default";
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
                    (e.Row.FindControl("lbl_Production") as Label).Text = "TBD";
                    (e.Row.FindControl("lbl_Production") as Label).Style.Add("margin-left", "10px");
                }
            }
            if (status == "Completed")
            {
                com_check.Connection = con;
                con.Close();
                con.Open();
                com_check.CommandText = "select max(CurrentDate) from prmsProductionHour_Backup where task='" + (e.Row.FindControl("hdn_taskid") as HiddenField).Value + "'";
                SqlDataReader dr = com_check.ExecuteReader();
                dr.Read();
                String CompletedDate = dr[0].ToString();
                (e.Row.FindControl("lbl_CompletedDate") as Label).Text = CompletedDate.ToString().Substring(0, CompletedDate.Length - 9);
                (e.Row.FindControl("lnk_TaskStatus") as LinkButton).CssClass = "btn btn-success";
                lk_Edit.Visible = false;
                lk_Delete.Visible = false;
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
            if (status == "Closed")
            {
                (e.Row.FindControl("lbl_CompletedDate") as Label).Text = "TBD";
                (e.Row.FindControl("lnk_TaskStatus") as LinkButton).CssClass = "btn btn-danger";
                lk_Delete.Visible = false;
                lk_Edit.Visible = false;
                int ProductionToCalculate = 0;
                string Production = (e.Row.FindControl("lbl_Production") as Label).Text.ToString();
                if (Production != "")
                {
                    ProductionToCalculate = Convert.ToInt32(Production);
                }
                if (ProductionToCalculate == 0 || ProductionToCalculate < 0)
                {
                    HtmlGenericControl div0 = (HtmlGenericControl)e.Row.FindControl("blk_ProgressBar");
                    div0.Attributes["class"] = "progress-bar progress-bar-info progress-bar-striped";
                    (e.Row.FindControl("lbl_Production") as Label).Text = "NA";
                    (e.Row.FindControl("lbl_Production") as Label).Style.Add("margin-left", "10px");
                }
            }
        }

    }

    /// <summary>
    /// Task Status CLicked to perform action based on taskstatus
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void lnk_TaskStatus_Click(object sender, EventArgs e)
    {
        LinkButton lnk_TaskStatus = (LinkButton)sender;
        GridViewRow row = (GridViewRow)lnk_TaskStatus.NamingContainer;
        LinkButton lnk_Status = (LinkButton)row.FindControl("lnk_TaskStatus");

        ViewState["selectedTaskID"] = lnk_TaskStatus.CommandArgument;

        if (lnk_Status.Text == "Hold")
        {
            popup_HoldState.Show();
        }
        else
        {

        }
        ScriptManager.RegisterStartupScript(this, GetType(), "setDatatable", "setDatatable();", true);
    }

    /// <summary>
    /// Task Holds details
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void btn_WIPProject_Click(object sender, EventArgs e)
    {
        try
        {
            string HoldID = "";
            string PreviousState = "";
            String id = ViewState["selectedTaskID"].ToString();
            com.Connection = con;
            con.Open();
            com.CommandText = "select TOP 1 id,previousstate from tbl_TaskHolds where taskid='" + id + "' order by id desc";
            SqlDataReader dr = com.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                HoldID = dr[0].ToString();
                PreviousState = dr[1].ToString();
            }
            dr.Close();
            if (ddl_TaskStateChange.SelectedValue == "WIP")
            {
                com.CommandText = "update tbl_TaskHolds set wipdate=GETDATE() where id='" + HoldID + "'";
                com.ExecuteNonQuery();
                if (PreviousState == "")
                {
                    PreviousState = "Yet To Start";
                }
                com.CommandText = "update tbl_taskmaster set taskstatus='" + PreviousState + "',status=1 where id='" + id + "'";
                com.ExecuteNonQuery();
            }
            else
            {
                com.CommandText = "update tbl_taskmaster set taskstatus='Closed',status=0 where id='" + id + "'";
                com.ExecuteNonQuery();
                com.CommandText = "update tbl_TaskHolds set closeddate=GETDATE() where id='" + HoldID + "'";
                com.ExecuteNonQuery();
            }
            tasks_gridbind();
            ScriptManager.RegisterStartupScript(this, GetType(), "setDatatable", "setDatatable();", true);
        }
        catch (Exception)
        {

        }
    }
    /// <summary>
    /// Close Popup and Set Datatable
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_Exit_Click(object sender, EventArgs e)
    {
        popup_HoldState.Hide();
        //ScriptManager.RegisterStartupScript(this, GetType(), "setDatatable", "setDatatable();", true);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "setDatatable", "setDatatable(); window.location='" + Request.ApplicationPath + "/tasks';", true);
    }
    /// <summary>
    /// Show Task Hold POPUP
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void ddl_TaskStateChange_SelectedIndexChanged(object sender, EventArgs e)
    {
        popup_HoldState.Show();
    }

    /// <summary>
    /// Currently Not In Use
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void btn_Search_Click(object sender, EventArgs e)
    {
        tasks_gridbind();
    }
    /// <summary>
    /// Users DropDown To bind grid that alloted for selected user
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void ddl_Users_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["Insert"].ToString() == "2")
            {
                string userid = string.Empty;
                if (hdn_TaskNo.Value != "")
                {
                    SqlCommand cmd = new SqlCommand("select userid from tbl_taskmaster where id = " + hdn_TaskNo.Value + "", con);
                    userid = Convert.ToString(cmd.ExecuteScalar());
                    if (userid != ddl_Users.SelectedValue)
                    {

                        btn_Allot.Text = "Re Allot";
                    }
                    else
                    {
                        btn_Allot.Text = "Update";
                    }
                }

            }
        }
        catch (Exception)
        {

            //throw;
        }
    }
    /// <summary>
    /// Request Date Text Changed Event to Limit The Required Date Textbox
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void txt_RequestDate_TextChanged(object sender, EventArgs e)
    {
        txt_RequiredDate.Text = txt_RequestDate.Text;
        string RequiredDateLimitStart = Convert.ToString(DateTime.ParseExact(txt_RequiredDate.Text.Trim(), "dd/MM/yyyy", null).ToString("MM/dd/yyyy"));
        calext_RequiredDate.StartDate = DateTime.ParseExact(RequiredDateLimitStart, "MM/dd/yyyy", CultureInfo.InvariantCulture);
        //Convert.ToDateTime();
    }
    /// <summary>
    /// Clear All
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void btn_ClearAll_Click(object sender, EventArgs e)
    {
        tasks_gridbind();
    }
    /// <summary>
    /// Add Coding Standard Rating For Each Tasks
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void btn_AddRating_Click(object sender, EventArgs e)
    {

        string RatingBy = Convert.ToString(Session["Userid"]);
        string TaskID = Convert.ToString(ViewState["RatingForTask"]);
        string ifCompleted = objData.GetSingleData("select taskstatus from tbl_Taskmaster where id='" + TaskID + "'");
        if (ifCompleted.ToLower() == "completed")
        {
            string Rating = hdn_ScoredRating.Value;
            objData.InsertOrUpdateData("update tbl_Taskmaster set RatingOfTask='" + Rating + "',RatingBy='" + RatingBy + "',RatingOn=GetDate() where id='" + TaskID + "'");
            string RatingSelected = objData.GetSingleData("select RatingOfTask from tbl_Taskmaster where id='" + TaskID + "'");
            string QualityRatingSelected = objData.GetSingleData("select QualityRating from tbl_Taskmaster where id='" + TaskID + "'");
            ScriptManager.RegisterStartupScript(this, GetType(), "mismatch", "$('#star-" + RatingSelected + "').attr('checked','checked');$('#qualitystar-" + QualityRatingSelected + "').attr('checked','checked')", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "mismatch", "swal('Rating Updated Successfully')", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "mismatch", "swal('Task Not Completed')", true);
        }
    }
    /// <summary>
    /// Add Quality Rating For Each Tasks
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_AddQualityRating_Click(object sender, EventArgs e)
    {
        string RatingBy = Convert.ToString(Session["Userid"]);
        string TaskID = Convert.ToString(ViewState["RatingForTask"]);
        string ifCompleted = objData.GetSingleData("select taskstatus from tbl_Taskmaster where id='" + TaskID + "'");
        if (ifCompleted.ToLower() == "completed")
        {
            string Rating = hdn_ScoredQualityRating.Value;
            objData.InsertOrUpdateData("update tbl_Taskmaster set QualityRating='" + Rating + "',QualityRatingBy='" + RatingBy + "',QualityRatingOn=GetDate() where id='" + TaskID + "'");
            string QualityRatingSelected = objData.GetSingleData("select QualityRating from tbl_Taskmaster where id='" + TaskID + "'");
            string CodingStandardRatingSelected = objData.GetSingleData("select RatingOfTask from tbl_Taskmaster where id='" + TaskID + "'");
            ScriptManager.RegisterStartupScript(this, GetType(), "mismatch", "$('#qualitystar-" + QualityRatingSelected + "').attr('checked','checked');$('#star-" + CodingStandardRatingSelected + "').attr('checked','checked')", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "mismatch", "swal('Rating Updated Successfully')", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "mismatch", "swal('Task Not Completed')", true);
        }
    }
    /// <summary>
    /// Projects For Gridview Records to Show
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddl_FilterProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        taskChanged = true;
        tasks_gridbind();
        upl_All.Update();
        ScriptManager.RegisterStartupScript(this, GetType(), "setDataTable", "setDatatable();", true);
    }
    /// <summary>
    /// Month For Gridview Records to Show
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void ddl_FilterMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddl_FilterProject.SelectedIndex = -1;
        tasks_gridbind();
        upl_All.Update();
        ScriptManager.RegisterStartupScript(this, GetType(), "setDataTable", "setDatatable();", true);
    }
    /// <summary>
    /// Year For Gridview Records to Show
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void ddl_FilterYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddl_FilterProject.SelectedIndex = -1;
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
        tasks_gridbind();
        upl_All.Update();
        ScriptManager.RegisterStartupScript(this, GetType(), "setDataTable", "setDatatable();", true);
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
    /// <summary>
    /// Bind Projects for Filter
    /// </summary>
    private void bindProjectFilter()
    {
        SqlDataAdapter da = new SqlDataAdapter();
        DataTable dt = new DataTable();
        string Rights = Convert.ToString(Session["Rights"]);
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
            string[] SplittedProjectAlloted = ProjectAlloted.Split(',');
            com_check.Connection = con;
            com_task.CommandText = "select distinct a.projectid,e.projectreq as Project from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid  inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_teams f on f.TeamID=a.teamid inner join tbl_ProjectReq e on e.projectid=a.projectid ";
            if (!string.IsNullOrEmpty(ProjectID))
                com_task.CommandText += " and a.projectid='" + ProjectID + "'";
            com_task.CommandText += "  and (";
            foreach (string Splitted in SplittedProjectAlloted)
            {
                com_task.CommandText += "e.allotedteamid like '%" + Splitted + "%' or ";
            }
            com_task.CommandText = com_task.CommandText.Substring(0, com_task.CommandText.Length - 3);
            string teamUsers = objData.GetSingleData("select ''''+REPLACE(userid,',',''',''')+''''[users] from tbl_teamAllotmentMaster where teamleader='" + Convert.ToString(Session["UserID"]) + "'");
            com_task.CommandText += ") and e.projectid!='NA' where a.userID in(" + teamUsers + ")";
            //if (ddl_FilterProject.SelectedIndex > 0 && ddl_FilterProject.SelectedValue != "-Select-")
            //    ProjectID = ddl_FilterProject.SelectedValue;
            //if (!string.IsNullOrEmpty(ProjectID))
            //    com_task.CommandText += " and a.projectid='" + ProjectID + "'";
            //else
            //    com_task.CommandText += " and  ((Month(a.requireddate)='" + ddl_FilterMonth.SelectedValue + "' and YEAR(a.requireddate)='" + ddl_FilterYear.SelectedValue + "') or (Month(a.requestdate)='" + ddl_FilterMonth.SelectedValue + "' and YEAR(a.requestdate)='" + ddl_FilterYear.SelectedValue + "'))";
            da = new SqlDataAdapter(com_task.CommandText, con);
            da.Fill(dt);
        }
        else
        {
            com_task.Connection = con;
            con.Close();
            con.Open();
            com_task.CommandText = "select distinct a.projectid,e.projectreq as Project from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid and e.projectid!='NA' inner join tbl_teams f on f.TeamID=a.teamid";
            //if (ddl_FilterProject.SelectedIndex > 0 && ddl_FilterProject.SelectedValue != "-Select-")
            //    ProjectID = ddl_FilterProject.SelectedValue;
            //if (!string.IsNullOrEmpty(ProjectID))
            //{
            //    com_task.CommandText = "select a.id,a.requestid,a.taskstatus,a.taskname,a.taskdescription,a.projectid,a.stageid,a.scopeid,a.teamid,a.userid,CONVERT(VARCHAR(20),a.requestdate,103) AS requestdate,CONVERT(VARCHAR(20),a.requireddate,103) AS requireddate,a.estimatedtime,b.Stage as Stage,c.Scope as Scope,f.Teamname as AllotedTeam,d.username as Username,e.projectreq as Project,a.Production from tbl_taskmaster a inner join tbl_MstStageMaster b on b.slno = a.stageid inner join tbl_scope c on c.ID = a.scopeid inner join tbl_usermaster d on d.userid = a.userid  inner join tbl_ProjectReq e on e.projectid=a.projectid and e.projectid!='NA' and a.projectid='" + ProjectID + "' inner join tbl_teams f on f.TeamID=a.teamid where a.projectid='" + ddl_FilterProject.SelectedValue + "' order by id desc";
            //}
            da.SelectCommand = com_task;
            da.Fill(dt);
        }
        ddl_FilterProject.DataSource = dt;
        ddl_FilterProject.DataTextField = "project";
        ddl_FilterProject.DataValueField = "projectid";
        ddl_FilterProject.DataBind();
        ddl_FilterProject.Items.Insert(0, new ListItem("-Select Project-", "-Select-"));
    }
}