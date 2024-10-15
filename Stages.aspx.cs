/// <summary>
/// Required namespaces
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
using System.Text;

public partial class Stages : System.Web.UI.Page
{
	/// <summary>
	/// Declarations Part For Variables,Strings,SqlConnections,etc
	/// </summary>
    String projectid, projectname, var_Status, ProjectAlloted;
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString());
    SqlConnection con2 = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString());
    SqlCommand com_stage = new SqlCommand();
    SqlCommand com_check = new SqlCommand();
    SqlCommand com2 = new SqlCommand();

	/// <summary>
	/// Page_s the load.
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">E.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Request.UrlReferrer == null)
            {
                Response.Redirect("Login");
            }

            if (Session["Userid"] == null) Response.Redirect("Login");
            else if (Session["Userid"].ToString() == "") Response.Redirect("Login");
            if (Session["CurrentProject"] != "")
            {
                if (!IsPostBack)
                {
                    bindProject();
                    ddl_Project.SelectedValue = Convert.ToString(Session["CurrentProject"]);
                    ddl_Project_SelectedIndexChanged(this, EventArgs.Empty);
                }
            }

            if (!IsPostBack)
            {
                ViewState["userid"] = Session["userid"];
                ddl_Project.Focus();
                bindProject();
            }
        }
        catch (Exception)
        {
        }
    }

	/// <summary>
	/// Project Dropdownselectedindexchanged event for bindgrid based on project
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">E.</param>
    protected void ddl_Project_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddl_Project.SelectedValue != "" || ddl_Project.SelectedValue!="N/A")
            {
                bindGrid();
            }
            projectid = ddl_Project.Text;
            projectname = ddl_Project.SelectedItem.Text;
            if (ddl_Project.SelectedIndex != 0)
            {
                lbl_ProjectID_Name.Text = projectname;
            }
            else
            {
                lbl_ProjectID_Name.Text = "";
            }
        }
        catch (Exception)
        {
        }
    }

	/// <summary>
	/// Binds the project based on user access.
	/// </summary>
    protected void bindProject()
    {
        try
        {
            ddl_Project.Items.Clear();
            con.Close();
            con.Open();
            string Rights = Convert.ToString(Session["Rights"]);
            SqlDataAdapter sda = new SqlDataAdapter("Select projectid,projectname,projectreq from tbl_projectReq where status=1 and projectstatus in('Yet To Start','WIP')", con);
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
                com_stage.Connection = con;
                com_stage.CommandText = "Select projectid,projectname,projectreq from tbl_projectReq where status=1 and projectstatus in('Yet To Start','WIP') and (";
                foreach (string Splitted in SplittedProjectAlloted)
                {
                    com_stage.CommandText += "allotedteamid like '%" + Splitted + "%' or ";
                }
                com_stage.CommandText = com_stage.CommandText.Substring(0, com_stage.CommandText.Length - 3);
                com_stage.CommandText += ")";
                sda = new SqlDataAdapter(com_stage.CommandText, con);
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
	/// Binds the grid for stages.
	/// </summary>
    protected void bindGrid()
    {
        try
        {
            con.Close();
            SqlDataAdapter da = new SqlDataAdapter("select slno,projectid,Stage[name],Convert(int,Status)[Status],(case when Status = 1 then 'Active' else 'In Active' end)[status1],estimatedtime from tbl_MstStageMaster where projectid='" + ddl_Project.SelectedValue + "' and Delstage!=1", con);

            DataSet ds = new DataSet();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                grd_Scope.DataSource = ds;
                grd_Scope.DataBind();
            }
            else
            {
                ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                grd_Scope.DataSource = ds;
                grd_Scope.DataBind();
                int columncount = grd_Scope.Rows[0].Cells.Count;
                grd_Scope.Rows[0].Cells.Clear();
                grd_Scope.Rows[0].Cells.Add(new TableCell());
                grd_Scope.Rows[0].Cells[0].ColumnSpan = columncount;
                grd_Scope.Rows[0].Cells[0].Text = "<center>----- No Records Found -----</center>";
            }
        }
        catch (Exception)
        {
        }
    }

	/// <summary>
	/// Clear this instance.
	/// </summary>
    void Clear()
    {
        try
        {
            txt_AddStage.Text = "";
            //txt_EstimatedTime.Text = string.Empty;
            ddl_Project.SelectedValue = "N/A";
            lbl_ProjectID_Name.Text = "";
            bindGrid();
        }
        catch (Exception)
        {
        }
    }

	/// <summary>
	/// Reset Form on Reset button Click
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">E.</param>
    protected void btn_Reset_Click(object sender, EventArgs e)
    {
        try
        {
            Clear();
        }
        catch (Exception)
        {

        }
    }

	/// <summary>
	/// New Stage Add Function
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">E.</param>
    protected void btn_Add_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddl_Project.SelectedValue == "N/A")
            {
				ScriptManager.RegisterStartupScript(this,GetType(),"mismatch","swal('Project is Mandatory');", true);
                bindGrid();
            }

            else
                if (txt_AddStage.Text == "")
                {
					ScriptManager.RegisterStartupScript(this,GetType(),"mismatch","swal('Stage Name is Mandatory');", true);
                    bindGrid();
                    txt_AddStage.Focus();
                }
                else
                {
                    con.Open();
                    com_stage.Connection = con;
                    string a = ddl_Project.SelectedValue.ToString();
                    string b = ddl_Project.SelectedItem.Text;
                    com_stage.CommandText = "select Stage from tbl_MstStageMaster where projectid='" + ddl_Project.SelectedItem.Value + "' and Stage='" + txt_AddStage.Text + "' and Delstage!=1";
                    SqlDataReader dr = com_stage.ExecuteReader();
                    if (dr.HasRows)
                    {
						ScriptManager.RegisterStartupScript(this,GetType(),"mismatch","swal('Stage already exists');", true);
                    }
                    else
                    {
                        dr.Close();
                        com_stage.CommandText = "insert into tbl_MstStageMaster(Stage,status,projectid,projectname,Delstage) values(@Stage,'1',@projectid,@projectname,0)";
                        com_stage.Parameters.Add("@Stage", txt_AddStage.Text.ToString());
                        com_stage.Parameters.Add("@projectid", ddl_Project.SelectedValue.ToString());
                        com_stage.Parameters.Add("@projectname", ddl_Project.SelectedItem.Text.ToString());
                        com_stage.Connection = con;
					con.Close ();
					con.Open ();
					com_stage.ExecuteNonQuery();
                        txt_AddStage.Text = string.Empty;
                        bindGrid();
                        if (!IsPostBack)
                        {
                            PostBackTrigger.Equals("true", "true");
                            bindProject();
                        }

                        //txt_EstimatedTime.Text = string.Empty;
                        txt_AddStage.Text = "";
                        txt_AddStage.Focus();
                    }
                }
        }
        catch (Exception)
        {
        }
    }

	/// <summary>
	/// Stage Delete Function Not In Use
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">E.</param>
    protected void grd_stage_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        //int id = (int)grd_stage.DataKeys[e.RowIndex].Value;
        //con.Open();
        //com_stage.Connection = con;
        //com_stage.CommandText = "delete from tbl_MstStageMaster where slno='" + id + "'";
        //com_stage.ExecuteNonQuery();
        //bindGrid();
    }
    protected void grd_stage_RowEditing(object sender, GridViewEditEventArgs e)
    {
        //grd_stage.EditIndex = e.NewEditIndex;
        //bindGrid();
    }
    protected void grd_stage_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {

    }
    protected void grd_stage_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        //try
        //{
        //    int id = Convert.ToInt32(grd_stage.DataKeys[e.RowIndex].Value);
        //    //GridViewRow row = (GridViewRow)grd_stage.Rows[e.RowIndex];
        //    //Label lblID = (Label)grd_stage.Rows[e.RowIndex].FindControl("slno");
        //    TextBox Stage = (TextBox)grd_stage.Rows[e.RowIndex].FindControl("txt_Stage");
        //    RadioButtonList rbl1 = grd_stage.Rows[e.RowIndex].FindControl("rbl_Status") as RadioButtonList;
        //    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString());
        //    con.Open();
        //    SqlCommand cmd = new SqlCommand("update tbl_MstStageMaster set stage='" + Stage.Text + "',status='" + rbl1.SelectedValue + "' where slno='" + id + "'", con);
        //    cmd.ExecuteNonQuery();
        //    con.Close();
        //    grd_stage.EditIndex = -1;
        //    bindGrid();
        //    //string sql = "update tbl_MstStageMaster set stage='" + stage.Text +"',status='" + rbl1.SelectedValue + "' where slno='" +
        //    //id + "'";
        //    //SqlCommand cmd = new SqlCommand(sql);
        //    //cmd.CommandType = CommandType.Text;
        //    //cmd.Connection = con;
        //    //cmd.ExecuteNonQuery();
        //    //grd_stage.EditIndex = -1;
        //    //bindGrid();
        //}
        //catch (Exception)
        //{
        //    Response.Write(ex.Message);
        //    bindGrid();
        //}
    }

	/// <summary>
	/// Cancel Edit Stage Add
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">E.</param>
    protected void grd_Scope_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            grd_Scope.EditIndex = -1;
            bindGrid();
        }
        catch (Exception)
        {

            //throw;
        }

    }

	/// <summary>
	/// Stage Delete Function
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">E.</param>
    protected void grd_Scope_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                int id = (int)grd_Scope.DataKeys[e.RowIndex].Value;
                con.Open();
                com_stage.Connection = con;
                com_stage.CommandText = "select status from tbl_MstStageMaster where slno='" + id + "'";
                SqlDataReader reader = com_stage.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    var_Status = reader["status"].ToString();
                }
                con.Close();
                con.Open();
                if (var_Status != "False")
                {
                    com_stage.Connection = con;
                    com_stage.CommandText = "update tbl_MstStageMaster set status=0 where slno='" + id + "'";
                    com_stage.ExecuteNonQuery();
                    bindGrid();
                }
                else
                {
                    com_stage.Connection = con;
                    com_stage.CommandText = "select requestid from tbl_taskmaster where stageid='" + id + "'";
                    SqlDataReader dr = com_stage.ExecuteReader();
                    if (dr.HasRows)
                    {
						ScriptManager.RegisterStartupScript(this,GetType(),"mismatch","swal(Stage not deleted due to currently used in Tasks');", true);
                    }
                    else
                    {
                        dr.Close();
                        com_stage.Connection = con;
                        com_stage.CommandText = "update tbl_MstStageMaster set Delstage=1 where slno='" + id + "'";
                        com_stage.ExecuteNonQuery();
                        bindGrid();
                    }
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

	/// <summary>
	/// Stage Update
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">E.</param>
    protected void grd_Scope_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            int id = Convert.ToInt32(grd_Scope.DataKeys[e.RowIndex].Value);
            //GridViewRow row = (GridViewRow)grd_stage.Rows[e.RowIndex];
            //Label lblID = (Label)grd_stage.Rows[e.RowIndex].FindControl("slno");
            TextBox Stage = (TextBox)grd_Scope.Rows[e.RowIndex].FindControl("txt_Stage");
            //TextBox Allotedtime = (TextBox)grd_Scope.Rows[e.RowIndex].FindControl("txt_EstimatedTime");
            RadioButtonList rbl1 = grd_Scope.Rows[e.RowIndex].FindControl("rbl_Status") as RadioButtonList;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString());
            con.Open();
            SqlCommand cmd = new SqlCommand("update tbl_MstStageMaster set stage='" + Stage.Text + "',status='" + rbl1.SelectedValue + "' where slno='" + id + "'", con);
            cmd.ExecuteNonQuery();
            con.Close();
            grd_Scope.EditIndex = -1;
            bindGrid();
            txt_AddStage.Text = "";
            //txt_EstimatedTime.Text = string.Empty;
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
    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grd_Scope.PageIndex = e.NewPageIndex;
            this.bindGrid();
        }
        catch (Exception)
        {

        }
    }

	/// <summary>
	/// Gridview Stage Row Edit
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">E.</param>
    protected void grd_Scope_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            grd_Scope.EditIndex = e.NewEditIndex;
            bindGrid();
        }
        catch (Exception)
        {
        }
    }

	/// <summary>
	/// Redirect Projects If Back Button Clicked
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">E.</param>
    protected void btn_Back_Click(object sender, EventArgs e)
    {
        Response.Redirect("Projects");
    }
}

