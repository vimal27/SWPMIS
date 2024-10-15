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


public partial class Milestones : System.Web.UI.Page
{
	/// <summary>
    /// Declarations Part For Variables,Strings,SqlConnections,etc
	/// </summary>
    IFormatProvider culture = new CultureInfo("en-US", true);
    string TaskIDHided;
    string status_update, ProjectAlloted;
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
    SqlDataReader dr;
    bool isInsert;
    int inserVal = 1;
    string Color11 = string.Empty;
    string request_id = string.Empty;
    string updateMode = string.Empty;
    string highbook0, highbook1, highbook2, highbook3, highbook4, Math1, Journals, books1, Q2ID, Others;

	/// <summary>
    /// Page Load Function
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Request.UrlReferrer == null)
            {
                Response.Redirect("Login");

            }
            //bindProject();
            if (!IsPostBack)
            {
                bindProject();
                milestones_gridbind();
				generateID();
            }
            txt_RequiredDate.Attributes.Add("readonly", "readonly");
            txt_RequestDate.Attributes.Add("readonly", "readonly");
            txt_MilestoneName.Focus();

            if (Session["Userid"] == null) Response.Redirect("Login");
            else if (Session["Userid"].ToString() == "") Response.Redirect("Login");
            if (!Page.IsPostBack)
            {
                ddl_Project.Focus();
                this.block_Register.Visible = false;
                this.Page.Title = "Task Allotment";
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
    /// Generate Unique ID On each Milestones
	/// </summary>
    protected void generateID()
    {
        try
        {
            Int32 count;
            SqlCommand cmd = new SqlCommand("select Top 1(Right(requestid,4))as count from tbl_Milestones where requestid like 'M" + "%' order by requestid desc", con);
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
            request_id = "M";
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
    /// Function To Projects Bind With Different User access
	/// </summary>
    protected void bindProject()
    {
        try
        {
            string Rights = Convert.ToString(Session["Rights"]);
            ddl_Project.Items.Clear();
            con.Close();
            con.Open();

            SqlDataAdapter sda = new SqlDataAdapter("Select projectid,projectname,projectreq from tbl_projectReq where projectstatus!='Hold' and projectstatus!='Closed' and projectstatus!='Completed' and status!=0", con);
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
                com_check.CommandText = "Select projectid,projectname,projectreq from tbl_projectReq where projectstatus!='Hold' and projectstatus!='Closed' and projectstatus!='Completed' and status!=0 and (";
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
    /// Function To Stages Bind With Different Projects
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
    /// Function To Scopes Bind With Different Projects
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
    /// Bind Stage,Scope On Project Changed
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
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
        
    }
    protected void ddl_Scope_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
	/// <summary>
    /// New Stage add on milestone page If Necessary
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
    protected void btn_Stage_Click(object sender, EventArgs e)
    {
        com_insert.Connection = con;
        con.Close();
        con.Open();
        com_insert.CommandText = "insert into tbl_MstStageMaster(Stage,status,projectid,projectname,Delstage) values('" + txt_Stage.Text + "','1','" + ddl_Project.SelectedValue + "','" + ddl_Project.SelectedItem.Text + "','0')";
        com_insert.ExecuteNonQuery();
        bindStage();
        txt_Stage.Text = string.Empty;
    }
	/// <summary>
    /// Cancel New Stage
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
    protected void btn_CancelStage_Click(object sender, EventArgs e)
    {
        txt_Stage.Text = string.Empty;
    }

	/// <summary>
    /// New Scope add on milestone page If Necessary
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
    protected void btn_Scope_Click(object sender, EventArgs e)
    {
        com_insert.Connection = con;
        con.Close();
        con.Open();
        com_insert.CommandText = "insert into tbl_scope(Scope,projectid,Description,scopestatus) values('" + txt_Scope.Text + "','" + ddl_Project.SelectedValue + "','" + txt_Description.Text + "','1')";
        com_insert.ExecuteNonQuery();
        bindScope();
        txt_Scope.Text = string.Empty;
        txt_Description.Text = string.Empty;
    }

	/// <summary>
    /// Cancel New Scope
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
    protected void btn_CancelScope_Click(object sender, EventArgs e)
    {
        txt_Scope.Text = string.Empty;
        txt_Description.Text = string.Empty;
    }

	/// <summary>
    /// Allot Milestone on allot button clicked
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
    protected void btn_Allot_Click(object sender, EventArgs e)
    {
        try
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            DateTime requestdt = DateTime.ParseExact(txt_RequestDate.Text, "dd/MM/yyyy", provider);
            DateTime requiredt = DateTime.ParseExact(txt_RequiredDate.Text, "dd/MM/yyyy", provider);
            if (requestdt.Date > requiredt.Date)
            {
				ScriptManager.RegisterStartupScript(this,GetType(),"mismatch","swal('Required Date must not be less than Request Date')",true);
                //ScriptManager.RegisterStartupScript(Page, GetType(), "Alert", "alert('Required Date must not be less than Request Date');", true);
                return;
            }
            else
            {
                int inserValCon = Convert.ToInt32(ViewState["Insert"]);
                if (inserValCon == 2)
                {
                    con.Close();
                    con.Open();
                    com_update.Connection = con;
                    com_update.CommandText = "update tbl_Milestones set  requestid=@requestid,name=@name,description=@description,project=@project,stage=@stage,scope=@scope,requestdate=@requestdate,requireddate=@requireddate,updateddate=GETDATE() where requestid = '" + txt_RequestNo.Text + "' and id ='" + hdn_TaskNo.Value + "'";
                    com_update.Parameters.Add("@requestid", Convert.ToString(txt_RequestNo.Text));
                    com_update.Parameters.Add("@name", Convert.ToString(txt_MilestoneName.Text));
                    com_update.Parameters.Add("@description", Convert.ToString(txt_MilestoneDescription.Text));
                    com_update.Parameters.Add("@project", Convert.ToString(ddl_Project.SelectedValue));
                    com_update.Parameters.Add("@stage", Convert.ToString(ddl_Stage.SelectedValue));
                    com_update.Parameters.Add("@scope", Convert.ToString(ddl_Scope.SelectedValue));
					if (txt_RequestDate.Text != "")
                    {
                        com_update.Parameters.Add("@requestdate", Convert.ToString(DateTime.ParseExact(txt_RequestDate.Text.Trim(), "dd/MM/yyyy", null).ToString("MM/dd/yyyy")));
                    }
                    if (txt_RequiredDate.Text != "")
                    {
                        com_update.Parameters.Add("@requireddate", Convert.ToString(DateTime.ParseExact(txt_RequiredDate.Text.Trim(), "dd/MM/yyyy", null).ToString("MM/dd/yyyy")));
                    }
                    con.Close();
                    con.Open();
                    com_update.ExecuteNonQuery();
					ScriptManager.RegisterStartupScript(this,GetType(),"mismatch","swal('Task Updated Successfully','','success')", true);
                    Clear();
                    milestones_gridbind();
                    block_Register.Visible = false;
                    block_Grid.Visible = true;
                }
                else
                {
                    com_insert.Connection = con;
                    con.Close();
                    con.Open();   
                    com_insert.Parameters.AddWithValue("@requestid", Convert.ToString(txt_RequestNo.Text));
                    com_insert.Parameters.AddWithValue("@name", Convert.ToString(txt_MilestoneName.Text));
                    com_insert.Parameters.AddWithValue("@description", Convert.ToString(txt_MilestoneDescription.Text));
                    com_insert.Parameters.AddWithValue("@project", Convert.ToString(ddl_Project.SelectedValue));
                    com_insert.Parameters.AddWithValue("@stage", Convert.ToString(ddl_Stage.SelectedValue));
                    com_insert.Parameters.AddWithValue("@scope", Convert.ToString(ddl_Scope.SelectedValue));
                    com_insert.CommandText = "insert into tbl_Milestones(requestid,name,description,project,stage,scope,requestdate,requireddate,status,milestonestatus,createddate) values(@requestid,@name,@description,@project,@stage,@scope,@requestdate,@requireddate,'1','Yet To Start',GETDATE())";
                    if (txt_RequestDate.Text != "")
                    {
                        com_insert.Parameters.AddWithValue("@requestdate", Convert.ToString(DateTime.ParseExact(txt_RequestDate.Text.Trim(), "dd/MM/yyyy", null).ToString("MM/dd/yyyy")));
                    }
                    if (txt_RequiredDate.Text != "")
                    {
                        com_insert.Parameters.AddWithValue("@requireddate", Convert.ToString(DateTime.ParseExact(txt_RequiredDate.Text.Trim(), "dd/MM/yyyy", null).ToString("MM/dd/yyyy")));
                    }
					
                    com_insert.ExecuteNonQuery();

                    Clear();
					ScriptManager.RegisterStartupScript(this,GetType(),"mismatch","swal('Milestones Added Successfully','','success')", true);
                    milestones_gridbind();
                    block_Register.Visible = false;
                    block_Grid.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "mismatch", "swal(ex)", true);

        }
        finally
        {
            con.Close();
        }
    }

	/// <summary>
    /// Clear Form Contents After submitting Form
	/// </summary>
    protected void Clear()
    {
        ddl_Project.SelectedIndex = 0;
        ddl_Stage.SelectedIndex = 0;
        txt_MilestoneName.Text = string.Empty;
        txt_MilestoneDescription.Text = string.Empty;
        ddl_Scope.SelectedIndex = 0;
        txt_RequestDate.Text = string.Empty;
        txt_RequiredDate.Text = string.Empty;
    }

	/// <summary>
    /// Reset Form
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
    protected void btn_Reset_Click(object sender, EventArgs e)
    {
        Clear();
    }

	/// <summary>
    /// Redirect If Back button Is Clicked
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
    protected void btn_Back_Click(object sender, EventArgs e)
    {
        try
        {
            this.block_Register.Visible = false;
            this.block_Grid.Visible = true;
            Response.Redirect(Request.Url.AbsoluteUri);
        }
        catch (Exception)
        {

        }
    }

	/// <summary>
    /// Export To Excel function
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
    protected void btn_Export_Click(object sender, EventArgs e)
    {
        try
		{
			SqlDataAdapter da = new SqlDataAdapter("select a.id,a.requestid,a.name,a.description,a.project,a.scope,a.stage,a.status,a.milestonestatus,a.requestdate,b.stage as stagename,c.scope as scopename,e.projectreq,a.requireddate from tbl_Milestones a inner join tbl_MstStageMaster b on b.slno = a.stage  inner join tbl_scope c on c.ID = a.scope inner join tbl_ProjectReq e on e.projectid=a.project order by a.id desc", con);
            DataTable dts = new DataTable();
            da.Fill(dts);
            if (dts.Rows.Count > 0)
            {
                Response.ClearContent();
                string excelname = "MilestoneDetails.xls";
                StringWriter sw = new StringWriter(); ;
                HtmlTextWriter htm = new HtmlTextWriter(sw);
                DataGrid dgGrid = new DataGrid();
                dgGrid.HeaderStyle.ForeColor = Color.Black;
                dgGrid.HeaderStyle.BackColor = Color.LightBlue;
                dgGrid.ControlStyle.BackColor = Color.Linen;
                dgGrid.DataSource = dts;
                dgGrid.DataBind();
                dgGrid.RenderControl(htm);

                Response.AddHeader("Content-Disposition", "attachment; filename=" + excelname + "");
                Response.ContentType = "application/excel";
                this.EnableViewState = false;
                Response.Write(sw.ToString());
                Response.End();
            }
        }
        catch (Exception ex)
        {
			ScriptManager.RegisterStartupScript(this,GetType(),"mismatch","swal('" + ex.Message + "')", true);
        }
    }

	/// <summary>
    /// Binding Gridview with Created Milestones
	/// </summary>
    protected void milestones_gridbind()
    {
        try
        {
            con.Close();
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
                com_task.CommandText = "select a.id,a.requestid,a.name,a.description,a.project,a.scope,a.stage,a.status,a.milestonestatus,a.requestdate,a.requireddate from tbl_Milestones a inner join tbl_MstStageMaster b on b.slno = a.stage  inner join tbl_scope c on c.ID = a.scope inner join tbl_ProjectReq e on e.projectid=a.project and (";
                foreach (string Splitted in SplittedProjectAlloted)
                {
                    com_task.CommandText += "e.allotedteamid like '%" + Splitted + "%' or ";
                }
                com_task.CommandText = com_task.CommandText.Substring(0, com_task.CommandText.Length - 3);
                com_task.CommandText += ") order by a.id desc";
                da = new SqlDataAdapter(com_task.CommandText, con);
                
                DataSet ds = new DataSet();
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    grd_Milestones.DataSource = ds;
                    grd_Milestones.DataBind();
                }

                else
                {
                    ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                    grd_Milestones.DataSource = ds;
                    grd_Milestones.DataBind();
                    int columncount = grd_Milestones.Rows[0].Cells.Count;
                    grd_Milestones.Rows[0].Cells.Clear();
                    grd_Milestones.Rows[0].Cells.Add(new TableCell());
                    grd_Milestones.Rows[0].Cells[0].ColumnSpan = columncount;
                    grd_Milestones.Rows[0].Cells[0].Text = "<center>----- No Records Found -----</center>";
                }
            }
            else
            {
                da = new SqlDataAdapter("select a.id,a.requestid,a.name,a.description,a.project,a.scope,a.stage,a.status,a.milestonestatus,a.requestdate,b.stage as stagename,c.scope as scopename,e.projectreq,a.requireddate from tbl_Milestones a inner join tbl_MstStageMaster b on b.slno = a.stage  inner join tbl_scope c on c.ID = a.scope inner join tbl_ProjectReq e on e.projectid=a.project order by a.id desc", con);
                
                DataSet ds = new DataSet();

                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    grd_Milestones.DataSource = ds;
                    grd_Milestones.DataBind();

                }

                else
                {
                    ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                    grd_Milestones.DataSource = ds;
                    grd_Milestones.DataBind();
                    int columncount = grd_Milestones.Rows[0].Cells.Count;
                    grd_Milestones.Rows[0].Cells.Clear();
                    grd_Milestones.Rows[0].Cells.Add(new TableCell());
                    grd_Milestones.Rows[0].Cells[0].ColumnSpan = columncount;
                    grd_Milestones.Rows[0].Cells[0].Text = "<center>----- No Records Found -----</center>";
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

	/// <summary>
    /// Milestones Delete Function
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
    protected void grd_Milestones_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            String id = (string)grd_Milestones.DataKeys[e.RowIndex].Values[1].ToString();
            con.Open();
            com_scope.Connection = con;
            com_scope.CommandText = "select taskstatus from tbl_taskmaster where  id ='" + id + "'";
            SqlDataReader dr = com_scope.ExecuteReader();
            dr.Read();
            string State = dr[0].ToString();
            dr.Close();
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                com_scope.CommandText = "insert into tbl_TaskHolds(holdeddate,previousstate,taskid) values (GETDATE(),'" + State + "','" + id + "')";
                //com_scope.CommandText = "delete from tbl_taskmaster where requestid='" + id + "'";
                com_scope.ExecuteNonQuery();
                com_scope.Connection = con;
                com_scope.CommandText = "update tbl_taskmaster set status=0,taskstatus='Hold' where  id ='" + id + "'";
                //com_scope.CommandText = "delete from tbl_taskmaster where requestid='" + id + "'";
                com_scope.ExecuteNonQuery();

                milestones_gridbind();
            }
            else
            {

            }
        }
        catch (Exception)
        {
        }
    }
    protected void grd_Milestones_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

    }

    protected void grd_Milestones_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
      
    } 

	/// <summary>
    /// Add New Milestones if Button is clicked
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
    protected void btn_newMilestone_Click(object sender, System.EventArgs e)
    {
        inserVal = 3;
        ViewState["Insert"] = inserVal;
        try
        {
            this.block_Grid.Visible = false;
            this.block_Register.Visible = true;
        }
        catch (Exception)
        {
        }
    }

	/// <summary>
    /// Update selected Milestone on gridview Milestone
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
    protected void grd_Milestones_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            inserVal = 2;
            GridViewRow row = grd_Milestones.SelectedRow;
            hdn_RequestNo.Value = ((HiddenField)row.FindControl("lbl_RequestNo")).Value;
            hdn_TaskNo.Value = ((HiddenField)row.FindControl("hdn_taskid")).Value;
            txt_MilestoneName.Text = ((Label)row.FindControl("lbl_Task")).Text;
            txt_MilestoneDescription.Text = ((HiddenField)row.FindControl("hdn_MilestoneDescription")).Value;
            txt_RequestNo.Text = ((HiddenField)row.FindControl("lbl_RequestNo")).Value;
            ddl_Project.SelectedValue = ((HiddenField)row.FindControl("hdn_projectid")).Value;
            PostBackTrigger.Equals("true", "true");
            bindStage();
			bindScope();
            ddl_Stage.SelectedValue = ((HiddenField)row.FindControl("hdn_StageID")).Value;
            ddl_Scope.SelectedValue = ((HiddenField)row.FindControl("hdn_ScopeID")).Value;
            
            
            txt_RequestDate.Text = ((Label)row.FindControl("lbl_RequestDate")).Text;
            txt_RequiredDate.Text = ((Label)row.FindControl("lbl_RequiredDate")).Text;
            this.block_Grid.Visible = false;
            this.block_Register.Visible = true;
            ViewState["Insert"] = inserVal;
            btn_Allot.Text = "Update";
        }
        catch (Exception)
        {
            Response.Redirect(Request.Url.AbsoluteUri);
        }
    }
    
    //protected void btn_BackButtonClick(object sender, EventArgs e)
    //{
    //    this.block_Grid.Visible = true;
    //    this.block_View.Visible = false;
    //    this.block_Register.Visible = false;
    //    this.TitleOfPage.InnerText = "Task Master";
    //}
    //protected void SelectCurrentData(object sender, EventArgs e)
    //{
    //    this.block_Grid.Visible = false;
    //    this.block_Register.Visible = false;
    //    this.block_View.Visible = true;
    //    LinkButton btn = (LinkButton)(sender);
    //    string projectid = btn.CommandArgument;
    //    con.Close();
    //    con.Open();
    //    com_projectView.Connection = con;
    //    com_projectView.CommandText = "select requestid,projectid,stageid,scopeid,userid,requestdate,requireddate from tbl_taskmaster where requestid='" + projectid + "'";
    //    SqlDataReader dr = com_projectView.ExecuteReader();
    //    while (dr.Read())
    //    {
    //        TitleOfPage.InnerText = dr["projectreq"].ToString();
    //        lbl_ProjectidTextView.InnerText = dr["projectid"].ToString();
    //        lbl_ProjectnameTextView.InnerText = dr["projectname"].ToString();
    //        lbl_allotedteamTextView.InnerText = dr["allotedteam2"].ToString();
    //        lbl_receiveddateTextView.InnerText = dr["receiveddate"].ToString();
    //        lbl_manualidTextView.InnerText = dr["manualid"].ToString();
    //        lbl_ProjectdescTextView.InnerText = dr["projectDesc"].ToString();
    //        if (dr["remarks"] == "")
    //        {
    //            lbl_remarksTextView.InnerText = "N/A";
    //        }
    //        else
    //        {
    //            lbl_remarksTextView.InnerText = dr["remarks"].ToString();
    //        }
    //    }

    //}
    protected void grd_Milestones_RowEditing(object sender, GridViewEditEventArgs e)
    {
        //GridViewRow row = grd_Milestones.Rows[e.NewEditIndex];
        //// string serialNo = row.Cells[1].ToString();
        //// int slno = Convert.ToInt32(grd_Milestones.SelectedDataKey[e.NewEditIndex].ToString());
        //// int empId = Convert.ToInt32(grd_Milestones.DataKeys[e.RowIndex].Value); 
        //this.block_Grid.Visible = false;
        //this.block_Register.Visible = false;
        //this.block_View.Visible = true;
        //grd_Milestones.EditIndex = e.NewEditIndex;

        //Bind Grid Than Find Control 

        //GridViewRow row = grd_Milestones.SelectedRow;

        //txt_clientProjectId.Value = ((Label)row.FindControl("lbl_Clientprojectid")).Text;
        //Projectname.Value = ((Label)row.FindControl("lbl_Projectname")).Text;
        //ReceivedDate.Text = ((Label)row.FindControl("lbl_Receiveddate")).Text;
        //tbx_Desc.Text = ((HiddenField)row.FindControl("hdn_Description")).Value;
        //tbx_Remarks.Text = ((HiddenField)row.FindControl("hdn_Remark")).Value;


    }
   	
	/// <summary>
    /// View Milestone Details
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
    protected void SelectCurrentData(object sender, EventArgs e)
    {
        string TaskID;
        this.block_Grid.Visible = false;
        this.block_Register.Visible = false;
        this.block_View.Visible = true;
        LinkButton btn = (LinkButton)(sender);
        string requestid = btn.CommandArgument.ToString();
        con.Close();
		con.Open ();
        com_task.Connection = con;
		com_task.CommandText = "select a.id,a.requestid,a.name,a.description,a.project,a.scope,a.stage,a.status,a.milestonestatus,a.requestdate,b.stage as stagename,c.scope as scopename,e.projectreq,a.requireddate from tbl_Milestones a inner join tbl_MstStageMaster b on b.slno = a.stage  inner join tbl_scope c on c.ID = a.scope inner join tbl_ProjectReq e on e.projectid=a.project where a.id='" + requestid + "' order by a.id desc";
        SqlDataReader dr = com_task.ExecuteReader();
        while (dr.Read())
        {
            TitleOfPage.InnerText = dr["requestid"].ToString() + " : " + dr["Project"];
            lbl_TaskIDTextView.InnerText = dr["requestid"].ToString();
            TaskID = dr["id"].ToString();
            lbl_TaskNameTextView.InnerText = dr["name"].ToString();
            lbl_TaskDescriptionTextView.InnerText = dr["description"].ToString();
            lbl_ProjectTextView.InnerText = dr["projectreq"].ToString();
            lbl_StageTextView.InnerText = dr["stagename"].ToString();
            lbl_ScopeTextView.InnerText = dr["scopename"].ToString();
            lbl_RequestDateTextView.InnerText = dr["requestdate"].ToString();
            lbl_RequiredDateTextView.InnerText = dr["requireddate"].ToString();
        }
        dr.Close();
        
    }

	/// <summary>
    /// Reset Page If Back button is clicked
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
    protected void btn_BackButtonClick(object sender, EventArgs e)
    {
        this.block_Grid.Visible = true;
        this.block_View.Visible = false;
        this.block_Register.Visible = false;
        this.TitleOfPage.InnerText = "Tasks";
    }


	/// <summary>
    /// Row Databound Function To Differentiate Project Status
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
    protected void grd_Milestones_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // Retrieve the underlying data item. In this example
            // the underlying data item is a DataRowView object. 
            DataRowView rowView = (DataRowView)e.Row.DataItem;
            // Retrieve the state value for the current row. 
            String status = rowView["milestonestatus"].ToString();
            var lk_Edit = (LinkButton)e.Row.FindControl("lnk_Edit");
            var lk_Delete = (LinkButton)e.Row.FindControl("lnk_Delete");
            string Rights = Convert.ToString(Session["Rights"]);
            //format color of the as below 
            if (status == "WIP")
            {
                (e.Row.FindControl("lbl_CompletedDate") as Label).Text = "TBD";
                (e.Row.FindControl("lnk_TaskStatus") as LinkButton).CssClass = "btn btn-info";
            }
            if (status == "Hold")
            {
                (e.Row.FindControl("lbl_CompletedDate") as Label).Text = "TBD";
                (e.Row.FindControl("lnk_TaskStatus") as LinkButton).CssClass = "btn btn-warning";
                (e.Row.FindControl("lnk_TaskStatus") as LinkButton).Style.Add("cursor", "pointer");
                hdn_TaskNo.Value = (e.Row.FindControl("lnk_TaskStatus") as LinkButton).CommandArgument;
                lk_Edit.Visible = false;
                lk_Delete.Visible = false;
            }

            if (status == "Yet To Start")
            {
                (e.Row.FindControl("lbl_CompletedDate") as Label).Text = "TBD";
                (e.Row.FindControl("lnk_TaskStatus") as LinkButton).CssClass = "btn btn-default";
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
            }
            if (status == "Closed")
            {
                (e.Row.FindControl("lbl_CompletedDate") as Label).Text = "TBD";
                (e.Row.FindControl("lnk_TaskStatus") as LinkButton).CssClass = "btn btn-danger";
                lk_Delete.Visible = false;
                lk_Edit.Visible = false;
            }
        }

    }

	/// <summary>
    /// Release Hold Status
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
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
    }

	/// <summary>
    /// Change Hold To WIP,Closed Or Yet tostart 
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
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
                com.CommandText = "update tbl_taskmaster set taskstatus='Closed',closeddate=GETDATE() where id='" + id + "'";
                com.ExecuteNonQuery();
                com.CommandText = "update tbl_TaskHolds set closeddate=GETDATE() where id='" + HoldID + "'";
                com.ExecuteNonQuery();
            }
            milestones_gridbind();
        }
        catch (Exception)
        {

        }
    }

	/// <summary>
    /// Show Popup OF Holded Milestone
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
    protected void ddl_TaskStateChange_SelectedIndexChanged(object sender, EventArgs e)
    {
        popup_HoldState.Show();
    }

	/// <summary>
    /// Search Currently Not in use
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
    protected void btn_Search_Click(object sender, EventArgs e)
    {
        milestones_gridbind();
    }

	/// <summary>
	/// Milestones based on users
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
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
                    
                }

            }
        }
        catch (Exception)
        {

            //throw;
        }
    }

	/// <summary>
    /// Request date Textbox changed event to block required Date textbox function
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
    protected void txt_RequestDate_TextChanged(object sender, EventArgs e)
    {     
        txt_RequiredDate.Text = txt_RequestDate.Text;
        string RequiredDateLimitStart = Convert.ToString(DateTime.ParseExact(txt_RequiredDate.Text.Trim(), "dd/MM/yyyy", null).ToString("MM/dd/yyyy"));
        calext_RequiredDate.StartDate = DateTime.ParseExact(RequiredDateLimitStart, "MM/dd/yyyy", CultureInfo.InvariantCulture);
    }
    protected void btn_ClearAll_Click(object sender, EventArgs e)
    {
        milestones_gridbind();
    }
}