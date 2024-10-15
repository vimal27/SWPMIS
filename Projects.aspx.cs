// <summary>
// Required Namespaces
// </summary>
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


public partial class ProjectReq : System.Web.UI.Page
{
    // <summary>
    // Declarations Part For Variables,Strings,SqlConnections,etc
    // </summary>
    string teamid, teamname, ProjectID, DueDate, ConvertedDueDate, ConvertedCompletedDate;
    int result;
    string projectid = string.Empty;
    string ProjectAlloted, ProjectIDResult;
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString());
    SqlConnection con2 = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString());
    SqlCommand com_insert = new SqlCommand();
    SqlCommand com_update = new SqlCommand();
    SqlCommand com_modal = new SqlCommand();
    SqlCommand com_check = new SqlCommand();
    SqlCommand com = new SqlCommand();
    SqlCommand com_scope2 = new SqlCommand();
    SqlCommand com_projectView = new SqlCommand();
    SqlCommand com_scope = new SqlCommand();
    clsDataControl objData = new clsDataControl();
    ProductionCalculation pcData = new ProductionCalculation();
    int inserVal = 1;
    SqlDataReader dr;
    String project_id;

    // <summary>
    // Page_s the load.
    // </summary>
    // <param name="sender">Sender.</param>
    // <param name="e">E.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Request.UrlReferrer == null)
            {
                Response.Redirect("Login");
            }
            else
            {
                if (txt_ClientProjectID.Value == "")
                {
                    txt_ClientProjectID.Focus();
                }
                btn_Save.Attributes.Add("OnClientClick", "return validate()");
                if (Session["Userid"] == null) Response.Redirect("Login");
                else if (Session["Userid"].ToString() == "") Response.Redirect("Login");
                txt_ReceivedDate.Attributes.Add("readonly", "readonly");
                txt_DueDate.Attributes.Add("readonly", "readonly");
                txt_RemarkDate.Attributes.Add("readonly", "readonly");
                ddl_ProjectID.Visible = false;
                if (!Page.IsPostBack)
                {
                    txt_ClientProjectID.Value = txt_ProjectID.Value;
                    block_ProjectHistory.Visible = false;
                    bindSubTypes();
                    ViewState["userid"] = Convert.ToString(Session["userid"]);
                    project_GridBind();
                    bindScope();
                    BindCoOrdinators();
                    AjaxControlToolkit.Utility.SetFocusOnLoad(txt_Scope);
                    txt_ClientProjectID.Focus();
                    this.block_Register.Visible = false;
                    loadID();
                    bindTeam();
                    con.Close();
                    con.Open();
                    string confirmValue = Request.Form["confirm_value"];
                    com_check.Connection = con2;
                    con2.Open();

                    com_scope2.Connection = con;
                    com_scope2.CommandText = "update tbl_scope set scopestatus=0 where projectid='" + hdn_ProjectID.Value + "'";
                    com_scope2.ExecuteNonQuery();
                    bindScope();
                    BindCoOrdinators();
                    txt_ClientProjectID.Focus();
                    this.Page.Title = "PMIS";
                    if (Session["sessiontype"] != "PM" && Convert.ToString(Session["sessiontype"]).ToLower() != "leader" && Convert.ToString(Session["sessiontype"]).ToLower() != "admin")
                    {
                        btn_Save.Enabled = false;
                        btn_Reset.Enabled = false;
                        ddl_ProjectID.Visible = false;
                        txt_ProjectID.Visible = true;
                        Db_ProjectBind();
                    }
                    else
                    {
                        btn_Save.Enabled = true;
                        btn_Reset.Enabled = true;
                        ddl_ProjectID.Visible = false;
                        txt_ProjectID.Visible = true;
                    }
                }
            }
        }
        catch (Exception)
        {

        }
    }

    // <summary>
    // Binds the team based on user access.
    // </summary>
    protected void bindTeam()
    {
        try
        {
            string Rights = Session["Rights"].ToString();
            chk_Team.Items.Clear();
            con.Close();
            con.Open();
            com.Connection = con;
            com_check.Connection = con2;
            con2.Close();
            con2.Open();
            com.CommandText = "Select a.TeamID,a.Teamname from tbl_teams a where a.status=1";
            if (Rights == "Team Leader")
            {
                com_check.CommandText = "select a.TeamID,a.Teamname,b.TeamID,b.teamleader from tbl_teams a inner join tbl_teamAllotmentMaster b on a.status=1 and a.TeamID=b.TeamID where b.TeamID in(select TeamID from tbl_teamAllotmentMaster where teamleader like ('%" + Session["Userid"] + "%'))";
                SqlDataReader dr2 = this.com.ExecuteReader();
                if (dr2.HasRows)
                {
                    while (dr2.Read())
                    {
                        ListItem item = new ListItem();
                        item.Text = dr2["Teamname"].ToString();
                        item.Value = dr2["TeamID"].ToString();
                        chk_Team.Items.Add(item);
                    }
                    dr2.Close();
                    con.Close();
                }
            }
            else
            {
                SqlDataReader dr4 = this.com.ExecuteReader();
                if (dr4.HasRows)
                {
                    while (dr4.Read())
                    {
                        ListItem item = new ListItem();
                        item.Text = dr4["Teamname"].ToString();
                        item.Value = dr4["TeamID"].ToString();
                        chk_Team.Items.Add(item);
                    }
                    dr4.Close();
                    con.Close();
                }
            }
        }
        catch (Exception)
        {
        }
    }

    // <summary>
    // Loads the Unique Project ID on each Projects.
    // </summary>
    protected void loadID()
    {
        try
        {
            Int32 count;
            SqlCommand cmd = new SqlCommand("select Top 1(Right(projectid,4))as count from tbl_ProjectReq where projectid like 'EL" + DateTime.Now.ToString("yyyyMM") + "%' order by projectid desc", con);
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
            project_id = "EL";
            project_id += DateTime.Now.ToString("yyyyMM");
            string id = String.Format("{0:D4}", count);
            project_id += id;
            count += 1;

            txt_ProjectID.Value = project_id;
            hdn_ProjectID.Value = project_id;
            con.Close();
            txt_ProjectID.Attributes.Add("readonly", "readonly");
        }
        catch (Exception)
        {

        }
    }

    // <summary>
    // Binds the scope.
    // </summary>
    protected void bindScope()
    {
        try
        {
            con.Close();
            SqlDataAdapter da = new SqlDataAdapter("select ID,Scope,Description from tbl_scope where projectid='" + hdn_ProjectID.Value + "' and scopestatus=1", con);
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

    // <summary>
    // Scope Cancelling Edit Function On Gridview
    // </summary>
    // <param name="sender">Sender.</param>
    // <param name="e">E.</param>
    protected void grd_Scope_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            grd_Scope.EditIndex = -1;
            bindScope();
        }
        catch (Exception)
        {
        }
    }

    // <summary>
    // Scope Delete Function
    // </summary>
    // <param name="sender">Sender.</param>
    // <param name="e">E.</param>
    protected void grd_Scope_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                int id = (int)grd_Scope.DataKeys[e.RowIndex].Value;
                com_scope.Connection = con;
                con.Open();
                com_scope.CommandText = "select requestid from tbl_taskmaster where scopeid='" + id + "'";
                SqlDataReader dr = com_scope.ExecuteReader();
                if (dr.HasRows)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Scope not deleted due to currently used in Tasks');", true);
                }
                else
                {
                    con.Close();
                    con.Open();
                    com_scope.Connection = con;
                    com_scope.CommandText = "update tbl_scope set scopestatus=0 where ID='" + id + "'";
                    com_scope.ExecuteNonQuery();
                    bindScope();
                }
            }
        }
        catch (Exception)
        {
        }
    }

    // <summary>
    // Scope Updation Function
    // </summary>
    // <param name="sender">Sender.</param>
    // <param name="e">E.</param>
    protected void grd_Scope_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            int id = Convert.ToInt32(grd_Scope.DataKeys[e.RowIndex].Value);
            TextBox Scope = (TextBox)grd_Scope.Rows[e.RowIndex].FindControl("txt_Scope");
            TextBox Description = (TextBox)grd_Scope.Rows[e.RowIndex].FindControl("txt_Description");
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString());
            con.Open();
            SqlCommand cmd = new SqlCommand("update tbl_scope set scope='" + Scope.Text + "',Description='" + Description.Text + "' where ID='" + id + "'", con);
            cmd.ExecuteNonQuery();
            con.Close();
            grd_Scope.EditIndex = -1;
            bindScope();
        }
        catch (Exception)
        {
        }
    }

    // <summary>
    // Scope Editing Function
    // </summary>
    // <param name="sender">Sender.</param>
    // <param name="e">E.</param>
    protected void grd_Scope_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            grd_Scope.EditIndex = e.NewEditIndex;
            bindScope();
        }
        catch (Exception)
        {

        }
    }

    // <summary>
    // Binds the gridview for projects.
    // </summary>
    private void bindGrid()
    {
        try
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("select projectname,Convert(varchar(10),receiveddate,101)[receiveddate],Convert(varchar(10),duedate,101)[duedate],ProjectDesc,ScopeService,allotedteamid[Alloted Team],allotedteamname[Alloted Team Name] from tbl_ProjectReq where projectid ='" + ddl_ProjectID.SelectedItem.Text + "'", con);
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    txt_ProjectName.Value = Convert.ToString(dr[0]);
                    txt_ReceivedDate.Text = Convert.ToString(dr[1]);
                    txt_DueDate.Text = Convert.ToString(dr[2]);
                    tbx_Desc.Text = Convert.ToString(dr[3]);
                    txt_Scope.Text = Convert.ToString(dr[4]);
                }
            }
            dr.Close();
        }
        catch (Exception)
        {

        }
    }

    // <summary>
    // Back the specified sender and e.
    // </summary>
    // <param name="sender">Sender.</param>
    // <param name="e">E.</param>
    protected void back(object sender, EventArgs e)
    {
        try
        {
            txt_ProjectName.Value = "";
            tbx_Remarks.Text = "";
            txt_ReceivedDate.Text = "";
            txt_DueDate.Text = "";
            txt_ClientProjectID.Value = "";
            tbx_Desc.Text = "";
            txt_Scope.Text = "";
            ddl_ProjectType.SelectedIndex = 0;
            ddl_ProjectSubType.SelectedIndex = 0;
            txt_ReceivedFrom.Text = "";
            bindScope();
            BindCoOrdinators();
            bindTeam();
            btn_Save.Text = "Save";
            loadID();
            lbl_Already.Visible = false;
        }
        catch (Exception)
        {

        }
    }

    // <summary>
    // Save Button Click to Insert New Project
    // </summary>
    // <param name="sender">Sender.</param>
    // <param name="e">E.</param>
    protected void btn_Save_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["NotSelect"] = "0";
            if (ddl_ProjectSubType.Visible == true)
            {
                if (ddl_ProjectSubType.SelectedValue == "Select")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "mismatch", "swal('Please Select SubCategory of Project');", true);
                    ViewState["NotSelect"] = "1";
                }
            }
            if (ViewState["NotSelect"].ToString() != "1")
            {
                btn_Save.Attributes.Add("OnClick", "return validate()");
                int inserValCon = Convert.ToInt32(ViewState["Insert"]);
                if (btn_Save.Text == "Update")
                {
                    teamid = string.Empty;
                    teamname = string.Empty;
                    foreach (ListItem item in chk_Team.Items)
                    {
                        if (item.Selected == true)
                        {
                            teamid += "," + item.Value;
                            teamname += "," + item;
                        }
                    }
                    teamid = teamid.TrimStart(',');
                    teamname = teamname.TrimStart(',');
                    con.Close();
                    con.Open();
                    com_update.Connection = con;
                    string isReAlloted = objData.GetSingleData("select isNull(isReAllot,0) from tbl_ProjectReq where projectid='" + Convert.ToString(txt_ProjectID.Value) + "'");
                    if (isReAlloted == "0" || isReAlloted == "False")
                    {
                        com_update.CommandText = "update tbl_projectreq set projectname=@projectname,receivedfrom=@receivedfrom,typeproject=@typeproject,manualid=@manualid,receiveddate=@receiveddate,duedate=@duedate,ProjectDesc=@ProjectDesc,Scopeservice=@Scopeservice,projectreq=@projectreq,remarks=@remarks,allotedteamid=@allotedteamid,allotedteamname=@allotedteamname where projectid = '" + txt_ProjectID.Value + "'";
                    }
                    else
                    {
                        com_update.CommandText = "update tbl_projectreq set projectname=@projectname,receivedfrom=@receivedfrom,typeproject=@typeproject,manualid=@manualid,ProjectDesc=@ProjectDesc,Scopeservice=@Scopeservice,projectreq=@projectreq,remarks=@remarks,allotedteamid=@allotedteamid,allotedteamname=@allotedteamname where projectid = '" + txt_ProjectID.Value + "'";
                    }
                    com_update.Parameters.Add("@projectname", Convert.ToString(txt_ProjectName.Value));
                    if (txt_ProjectID.Visible == true) com_update.Parameters.Add("@Projectid", Convert.ToString(txt_ProjectID.Value));
                    else com_update.Parameters.Add("@Projectid", Convert.ToString(ddl_ProjectID.SelectedItem.Text));
                    if (ddl_ProjectType.SelectedItem.Value != "Internal")
                    {
                        com_update.Parameters.Add("@typeproject", Convert.ToString(ddl_ProjectType.SelectedItem.Value));
                        if (ddl_ProjectType.SelectedValue == "External")
                        {
                            com_update.Parameters.Add("@receivedfrom", Convert.ToString(txt_ReceivedFrom.Text));
                        }
                        else
                        {
                            com_update.Parameters.Add("@receivedfrom", "");
                        }
                    }
                    //else if(ddl_ProjectType.SelectedValue)
                    else
                    {
                        com_update.Parameters.Add("@receivedfrom", "");
                        com_update.Parameters.Add("@typeproject", Convert.ToString(ddl_ProjectSubType.SelectedItem.Value));
                    }

                    com_update.Parameters.Add("@manualid", Convert.ToString(txt_ClientProjectID.Value));

                    if (txt_ReceivedDate.Text != "")
                    {

                        com_update.Parameters.Add("@receiveddate", Convert.ToString(DateTime.ParseExact(txt_ReceivedDate.Text.Trim(), "dd/MM/yyyy", null).ToString("MM/dd/yyyy")));
                    }
                    string DueDate = "TBD";
                    if (txt_DueDate.Text != "")
                    {
                        DueDate = Convert.ToString(DateTime.ParseExact(txt_DueDate.Text.Trim(), "dd/MM/yyyy", null).ToString("MM/dd/yyyy"));
                    }
                    com_update.Parameters.Add("@duedate", DueDate);
                    com_update.Parameters.Add("@ProjectDesc", Convert.ToString(tbx_Desc.Text));
                    com_update.Parameters.Add("@Scopeservice", Convert.ToString(tbx_Desc.Text));
                    com_update.Parameters.Add("@allotedteamid", teamid);
                    com_update.Parameters.Add("@allotedteamname", teamname);
                    if (txt_ClientProjectID.Value != "")
                    {
                        com_update.Parameters.Add("@projectreq", Convert.ToString(txt_ProjectID.Value + " - " + txt_ProjectName.Value + " (" + txt_ClientProjectID.Value + ") "));
                    }
                    else
                    {
                        com_update.Parameters.Add("@projectreq", Convert.ToString(txt_ProjectID.Value + " - " + txt_ProjectName.Value));
                    }
                    com_update.Parameters.Add("@remarks", Convert.ToString(tbx_Remarks.Text));
                    con.Close();
                    con.Open();
                    com_update.ExecuteNonQuery();
                    if (String.IsNullOrEmpty(txt_DueDate.Text.ToString()))
                    {
                        com_update.CommandText = "update tbl_ProjectHistory set receiveddate='" + Convert.ToString(DateTime.ParseExact(txt_ReceivedDate.Text.Trim(), "dd/MM/yyyy", null).ToString("MM/dd/yyyy")) + "' where projectid='" + txt_ProjectID.Value + "' and id in(select max(id) from tbl_ProjectHistory where projectid='" + txt_ProjectID.Value + "')";
                    }
                    else
                    {
                        com_update.CommandText = "update tbl_ProjectHistory set receiveddate='" + Convert.ToString(DateTime.ParseExact(txt_ReceivedDate.Text.Trim(), "dd/MM/yyyy", null).ToString("MM/dd/yyyy")) + "',duedate='" + Convert.ToString(DateTime.ParseExact(txt_DueDate.Text.Trim(), "dd/MM/yyyy", null).ToString("MM/dd/yyyy")) + "' where projectid='" + txt_ProjectID.Value + "' and id in(select max(id) from tbl_ProjectHistory where projectid='" + txt_ProjectID.Value + "')";
                    }
                    com_update.ExecuteNonQuery();
                    project_GridBind();
                    ScriptManager.RegisterStartupScript(this, GetType(), "mismatch", "swal('Project Updated Successfully','','success');setDatatable();", true);
                    back(sender, e);
                    block_Register.Visible = false;
                    block_Grid.Visible = true;
                }
                else
                {
                    teamid = string.Empty;
                    teamname = string.Empty;
                    foreach (ListItem item in chk_Team.Items)
                    {
                        if (item.Selected == true)
                        {
                            teamid += "," + item.Value;
                            teamname += "," + item;
                        }
                    }
                    teamid = teamid.TrimStart(',');
                    teamname = teamname.TrimStart(',');
                    lbl_Already.Text = "";
                    con.Close();
                    con.Open();
                    Int32 catCheck, projCheck;
                    com_check.Connection = con;
                    com_check.CommandText = "select Count(projectid) from category1 where projectid like '" + txt_ProjectID.Value + "'";
                    catCheck = Convert.ToInt32(com_check.ExecuteScalar());
                    string checkProjectExists = objData.GetSingleData("select manualid from tbl_ProjectReq where manualid='" + txt_ClientProjectID.Value + "'");
                    com_check.Connection = con;
                    com_check.CommandText = "select projectid from tbl_projectreq where projectid like '" + txt_ProjectID.Value + "'";
                    dr = com_check.ExecuteReader();
                    dr.Read();
                    if ((dr.HasRows == false || txt_ClientProjectID.Value == "") && (checkProjectExists == "0"))
                    {
                        dr.Close();
                        SqlTransaction trans1 = con.BeginTransaction();
                        com_insert.Transaction = trans1;
                        com_insert.Connection = con;
                        if (txt_ProjectID.Visible == true)
                        {
                            com_insert.CommandText = "insert into tbl_projectreq(projectname,Projectid,typeproject,manualid,receiveddate,duedate,ProjectDesc,Scopeservice,projectreq,allotedteamid,allotedteamname,remarks,bool,status,projectstatus,receivedfrom) values(@projectname,@Projectid,@typeproject,@manualid,@receiveddate,@duedate,@ProjectDesc,@Scopeservice,@projectreq,@allotedteamid,@allotedteamname,@remarks,0,1,'Yet To Start',@receivedfrom)";
                        }
                        else if (ddl_ProjectID.Visible == true)
                        {
                            com_insert.CommandText = "update tbl_projectreq set projectname=@projectname,typeproject=@typeproject,manualid=@manualid,receiveddate=@receiveddate,duedate=@duedate,ProjectDesc=@ProjectDesc,Scopeservice=@Scopeservice,projectreq=@projectreq,allotedteamid=@allotedteamid,allotedteamname=@allotedteamname,remarks=@remarks where projectid = '" + ddl_ProjectID.SelectedItem.Text + "'";
                        }
                        com_insert.Parameters.Add("@projectname", Convert.ToString(txt_ProjectName.Value));
                        if (txt_ProjectID.Visible == true) com_insert.Parameters.Add("@Projectid", Convert.ToString(txt_ProjectID.Value));
                        else com_insert.Parameters.Add("@Projectid", Convert.ToString(ddl_ProjectID.SelectedItem.Text));
                        if (ddl_ProjectType.SelectedItem.Value != "Internal")
                        {
                            com_insert.Parameters.Add("@typeproject", Convert.ToString(ddl_ProjectType.SelectedItem.Value));
                            if (ddl_ProjectType.SelectedItem.Value == "External")
                            {
                                com_insert.Parameters.Add("@receivedfrom", Convert.ToString(txt_ReceivedFrom.Text));
                            }
                            else
                            {
                                com_insert.Parameters.Add("@receivedfrom", "");
                            }
                        }
                        else
                        {
                            com_insert.Parameters.Add("@receivedfrom", "");
                            com_insert.Parameters.Add("@typeproject", Convert.ToString(ddl_ProjectSubType.SelectedItem.Value));
                        }
                        com_insert.Parameters.Add("@manualid", Convert.ToString(txt_ClientProjectID.Value));
                        if (txt_ReceivedDate.Text != "")
                        {
                            com_insert.Parameters.Add("@receiveddate", Convert.ToString(DateTime.ParseExact(txt_ReceivedDate.Text.Trim(), "dd/MM/yyyy", null).ToString("MM/dd/yyyy")));
                        }
                        string DueDate = "TBD";
                        if (txt_DueDate.Text != "")
                        {
                            DueDate = Convert.ToString(DateTime.ParseExact(txt_DueDate.Text.Trim(), "dd/MM/yyyy", null).ToString("MM/dd/yyyy"));
                        }
                        com_insert.Parameters.Add("@duedate", DueDate);
                        com_insert.Parameters.Add("@ProjectDesc", Convert.ToString(tbx_Desc.Text));
                        com_insert.Parameters.Add("@Scopeservice", Convert.ToString(tbx_Desc.Text));
                        if (txt_ClientProjectID.Value != "")
                        {
                            com_insert.Parameters.Add("@projectreq", Convert.ToString(txt_ProjectID.Value + " - " + txt_ProjectName.Value + " (" + txt_ClientProjectID.Value + ") "));
                        }
                        else
                        {
                            com_insert.Parameters.Add("@projectreq", Convert.ToString(txt_ProjectID.Value + " - " + txt_ProjectName.Value));
                        }
                        com_insert.Parameters.Add("@allotedteamid", teamid);
                        com_insert.Parameters.Add("@allotedteamname", teamname);
                        com_insert.Parameters.Add("@remarks", Convert.ToString(tbx_Remarks.Text));
                        con.Close();
                        con.Open();
                        com_insert.ExecuteNonQuery();
                        string currentProject = txt_ProjectID.Value;
                        Session["CurrentProject"] = currentProject;
                        com_projectView.Connection = con;
                        com_projectView.CommandText = "insert into tbl_ProjectStatusMaster(project,currentstate,changedstate,dateofchange) values('" + Convert.ToString(txt_ProjectID.Value) + "','Projet Not Created','Yet To Start',GETDATE())";
                        com_projectView.ExecuteNonQuery();
                        txt_ProjectName.Value = "";
                        txt_ProjectID.Value = "";
                        ddl_ProjectType.SelectedValue = "";
                        txt_ReceivedDate.Text = "";
                        txt_DueDate.Text = "";
                        tbx_Desc.Text = "";
                        chk_Team.SelectedIndex = 0;
                        tbx_Remarks.Text = "";
                        project_GridBind();
                        ScriptManager.RegisterStartupScript(this, GetType(), "mismatch", "swal('Project Inserted Successfully','','success');setDatatable();", true);
                        back(sender, e);
                        block_Register.Visible = false;
                        block_Grid.Visible = true;
                    }
                    else
                    {
                        lbl_Already.Visible = true;
                        lbl_Already.Text = "This Project ID Already Exist";
                        txt_ProjectID.Focus();
                        bindScope();
                        BindCoOrdinators();
                    }
                }
            }
        }
        catch (SqlException eee)
        {
            lbl_Already.Visible = true;
        }
        finally
        {
            con.Close();
        }
    }

    // <summary>
    // Reset Form Fields
    // </summary>
    // <param name="sender">Sender.</param>
    // <param name="e">E.</param>
    protected void btn_Reset_Click(object sender, EventArgs e)
    {
        try
        {
            back(sender, e);
            if (txt_ProjectID.Visible == true)
            {
                ddl_ProjectID.Visible = false;
                txt_ProjectName.Visible = true;
                Db_ProjectBind();
            }
            else if (ddl_ProjectID.Visible == true)
            {
                txt_ProjectID.Visible = false;
                ddl_ProjectID.Visible = true;
            }
            com_scope2.Connection = con;
            com_scope2.CommandText = "update tbl_scope set scopestatus=0 where projectid='" + hdn_ProjectID.Value + "'";
            com_scope2.ExecuteNonQuery();
            bindScope();
        }
        catch (Exception)
        {

        }
    }


    // <summary>
    // Redirect Projects From gridview
    // </summary>
    // <param name="sender">Sender.</param>
    // <param name="e">E.</param>
    protected void btn_Back_Click(object sender, EventArgs e)
    {
        try
        {
            this.block_Register.Visible = false;
            this.block_Grid.Visible = true;
            clear();
        }
        catch (Exception)
        {

        }
    }

    // <summary>
    // Clear this instance.
    // </summary>
    protected void clear()
    {
        Response.Redirect(Request.Url.AbsoluteUri);
    }

    // <summary>
    // Export To Excel
    // </summary>
    // <param name="sender">Sender.</param>
    // <param name="e">E.</param>
    protected void btn_Export_Click(object sender, EventArgs e)
    {
        try
        {
            string Rights = Convert.ToString(Session["Rights"]);
            SqlDataAdapter da = new SqlDataAdapter("select Projectid[Request ID],manualid[Project ID],Projectname[Project Name],typeproject[Project Type],allotedteamname[Alloted Team],CONVERT(VARCHAR(20),receiveddate,103)[Received Date],case when (duedate is null or duedate='') then 'TBD' else Convert(Varchar(10),duedate) end as DueDate,case when (completeddate is null or completeddate='') then 'TBD' else Convert(Varchar(10),completeddate) end as CompletedDate,projectstatus[Project Status] from tbl_ProjectReq order by projectid desc", con);
            if (Rights == "Team Leader")
            {
                da = new SqlDataAdapter("select Projectid[Request ID],manualid[Project ID],Projectname[Project Name],typeproject[Project Type],allotedteamname[Alloted Team],CONVERT(VARCHAR(20),receiveddate,103)[Received Date],case when (duedate is null or duedate='') then 'TBD' else Convert(Varchar(10),duedate) end as DueDate,case when (completeddate is null or completeddate='') then 'TBD' else Convert(Varchar(10),completeddate) end as CompletedDate,projectstatus[Project Status] from tbl_ProjectReq where allotedteamid in(select TeamID from tbl_teamAllotmentMaster where teamleader like ('%" + Session["Userid"] + "%')) order by projectid desc", con);
            }
            DataTable dts = new DataTable();
            da.Fill(dts);
            if (dts.Rows.Count > 0)
            {
                List<string> HeaderNames = new List<string>();
                HeaderNames.Add("Request ID");
                HeaderNames.Add("Project ID");
                HeaderNames.Add("Project Name");
                HeaderNames.Add("Project Type");
                HeaderNames.Add("Alloted Team");
                HeaderNames.Add("Received Date");
                HeaderNames.Add("Due Date");
                HeaderNames.Add("Completed Date");
                HeaderNames.Add("Project Status");
                List<string> ExcelReport = pcData.generateExcelReport(dts, "ProjectDetails", "GenericReports", "Project Details", 9, HeaderNames);
                FileInfo file = new FileInfo(ExcelReport[2]);
                Response.Clear();
                Response.AddHeader("Content-Length", file.Length.ToString(CultureInfo.InvariantCulture));
                Response.AddHeader("Content-Disposition", "attachment;filename=\"" + ("ProjectDetails.xls") + "\"");
                Response.ContentType = "application/octet-stream";
                Response.Flush();
                Response.TransmitFile(ExcelReport[0] + ("ProjectDetails" + DateTime.Now.ToShortDateString().Replace("/", "") + ".xls"));
                Response.End();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "Script", "alert('" + ex.Message + "')", true);
        }
    }

    // <summary>
    // Cacel Scope Add
    // </summary>
    // <param name="sender">Sender.</param>
    // <param name="e">E.</param>
    protected void btn_CancelScope_Click(object sender, EventArgs e)
    {
        try
        {
            txt_Description.Text = string.Empty;
            txt_Scope.Text = string.Empty;
            bindScope();
        }
        catch (Exception)
        {

        }
    }

    // <summary>
    // Currently not in use
    // </summary>
    private void Db_ProjectBind()
    {
        try
        {
            con.Close();
            con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("select distinct(a.ProjectId) as ProjectId from tbl_projectReq a where a.bool=0 order by projectid asc", con);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            ddl_ProjectID.DataSource = ds;
            ddl_ProjectID.DataTextField = "Projectid";
            ddl_ProjectID.DataBind();
            ddl_ProjectID.Items.Insert(0, "Select");
        }
        catch (Exception)
        {

        }

    }

    // <summary>
    // Currently Not In Use.
    // </summary>
    // <param name="sender">Sender.</param>
    // <param name="e">E.</param>
    protected void ddl_ProjectID_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindGrid();
        }
        catch (Exception)
        {
        }
    }

    // <summary>
    // Insert New Scope
    // </summary>
    // <param name="sender">Sender.</param>
    // <param name="e">E.</param>
    protected void btn_Scope_Click(object sender, EventArgs e)
    {
        try
        {
            con.Open();
            com_scope.Connection = con;
            if (txt_Scope.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Enter Valid Name For Scope');", true);
            }
            else if (txt_Description.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Enter Description For Scope');", true);
            }
            else
            {
                com_scope.CommandText = "select Scope from tbl_scope where projectid='" + hdn_ProjectID.Value + "' and scope='" + txt_Scope.Text + "' and scopestatus!=0";
                SqlDataReader dr = com_scope.ExecuteReader();
                if (dr.HasRows)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Scope already exists');", true);
                }
                else
                {
                    dr.Close();
                    com_scope.CommandText = "insert into tbl_scope(projectid,Scope,Description,scopestatus) values(@projectid,@scope,@description,'1')";
                    com_scope.Parameters.Add("@projectid", txt_ProjectID.Value.ToString());
                    com_scope.Parameters.Add("@scope", txt_Scope.Text.ToString());
                    com_scope.Parameters.Add("@description", txt_Description.Text.ToString());
                    com_scope.Connection = con;
                    con.Close();
                    con.Open();
                    com_scope.ExecuteNonQuery();

                    bindScope();
                    txt_Description.Text = "";
                    txt_Scope.Text = "";
                }
                bindScope();
            }
        }
        catch (Exception)
        {
        }
    }

    // <summary>
    // Gridview bind For Projects
    // </summary>
    protected void project_GridBind()
    {
        try
        {
            string Rights = Convert.ToString(Session["Rights"]);
            IFormatProvider culture = new CultureInfo("en-US", true);
            con.Close();
            con.Open();
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
                com_modal.Connection = con;
                com_modal.CommandText = "select case when exists(select projectid from tbl_ProjectHistory where projectid=a.projectid) then CONVERT(VARCHAR(20),b.receiveddate,103) else CONVERT(VARCHAR(20),a.receiveddate,103) end as receiveddate2,case when exists(select projectid from tbl_ProjectHistory where projectid=a.projectid) then CONVERT(VARCHAR(20),b.duedate,103) else replace(substring(a.duedate, 4,2) + '/' + substring(a.duedate, 1, 2)  + '/' + substring(a.duedate, 7, 4),'/TB/','TBD') end AS DueDate2,a.* from tbl_ProjectReq a left outer join tbl_ProjectHistory b on b.projectid=a.projectid and b.id in (select max(id) from tbl_ProjectHistory where projectid=a.projectid)  where (";
                foreach (string Splitted in SplittedProjectAlloted)
                {
                    com_modal.CommandText += "allotedteamid like '%" + Splitted + "%' or ";
                }
                com_modal.CommandText = com_modal.CommandText.Substring(0, com_modal.CommandText.Length - 3);
                com_modal.CommandText += ") order by a.projectid desc";
                SqlDataAdapter da = new SqlDataAdapter(com_modal.CommandText, con);
                com_modal.CommandText = com_modal.CommandText.Substring(0, com_modal.CommandText.Length - 24);

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
            else
            {
                SqlDataAdapter da = new SqlDataAdapter("", con);
                da = new SqlDataAdapter("select case when exists(select projectid from tbl_ProjectHistory where projectid=a.projectid) then CONVERT(VARCHAR(20),b.receiveddate,103) else CONVERT(VARCHAR(20),a.receiveddate,103) end as receiveddate2,case when exists(select projectid from tbl_ProjectHistory where projectid=a.projectid) then CONVERT(VARCHAR(20),b.duedate,103) else replace(substring(a.duedate, 4,2) + '/' + substring(a.duedate, 1, 2)  + '/' + substring(a.duedate, 7, 4),'/TB/','TBD') end AS DueDate2,a.* from tbl_ProjectReq a left outer join tbl_ProjectHistory b on b.projectid=a.projectid and b.id in (select max(id) from tbl_ProjectHistory where projectid=a.projectid) order by a.projectid desc", con);
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

    // <summary>
    // Add New Project Remarks (control Specially for teamleaders and admin)
    // </summary>
    // <param name="sender">Sender.</param>
    // <param name="e">E.</param>
    protected void Btn_AddProjectRemarks_Click(object sender, EventArgs e)
    {
        try
        {
            if (txt_RemarkDate.Text != "" && tbx_ProjectRemarks.Text != "")
            {
                com_insert.Connection = con;
                con.Close();
                con.Open();
                com_insert.Connection = con;
                com_insert.CommandText = "insert into tbl_ProjectRemarks(project,remark,date,requestby,status) values(@project,'<p>'+@remark,@remarkdate,@requestby,1)";
                com_insert.Parameters.Clear();
                com_insert.Parameters.AddWithValue("@project", lbl_ProjectIDTextView.InnerText);
                com_insert.Parameters.AddWithValue("@remark", tbx_ProjectRemarks.Text.Replace("\r\n", "</p><p>"));
                com_insert.Parameters.AddWithValue("@remarkdate", Convert.ToString(DateTime.ParseExact(txt_RemarkDate.Text.Trim(), "dd/MM/yyyy", null).ToString("MM/dd/yyyy")));
                com_insert.Parameters.AddWithValue("@requestby", Session["Userid"].ToString());
                com_insert.ExecuteNonQuery();
                tbx_ProjectRemarks.Text = "";
                txt_RemarkDate.Text = "";
                ScriptManager.RegisterStartupScript(this, GetType(), "mismatch", "swal('Project Remarks Added Successfully');", true);
                projectremarks_GridBind();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "mismatch", "swal('Please Enter Datas in Required Columns');", true);
                projectremarks_GridBind();
            }
        }
        catch (Exception ex)
        {

        }
    }

    // <summary>
    // Show Project Remarks
    // </summary>
    protected void projectremarks_GridBind()
    {
        try
        {
            string Rights = Convert.ToString(Session["Rights"]);
            IFormatProvider culture = new CultureInfo("en-US", true);
            con.Close();
            con.Open();
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
                com_modal.Connection = con;
                com_modal.CommandText = "select a.id,a.project,a.remark,(select c.username from tbl_usermaster c where c.userid=a.requestby)[requestby],a.date from tbl_ProjectRemarks a inner join tbl_ProjectReq b on b.projectid=a.project  where (";
                foreach (string Splitted in SplittedProjectAlloted)
                {
                    com_modal.CommandText += "b.allotedteamid like '%" + Splitted + "%' or ";
                }
                com_modal.CommandText = com_modal.CommandText.Substring(0, com_modal.CommandText.Length - 3);
                com_modal.CommandText += ") and a.project='" + lbl_ProjectIDTextView.InnerText + "' and a.status=1 order by date desc";
                SqlDataAdapter da = new SqlDataAdapter(com_modal.CommandText, con);
                com_modal.CommandText = com_modal.CommandText.Substring(0, com_modal.CommandText.Length - 24);

                DataSet ds = new DataSet();
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {

                    grd_AddRemark.DataSource = ds;
                    grd_AddRemark.DataBind();
                }
                else
                {
                    ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                    grd_AddRemark.DataSource = ds;
                    grd_AddRemark.DataBind();
                    int columncount = grd_AddRemark.Rows[0].Cells.Count;
                    grd_AddRemark.Rows[0].Cells.Clear();
                    grd_AddRemark.Rows[0].Cells.Add(new TableCell());
                    grd_AddRemark.Rows[0].Cells[0].ColumnSpan = columncount;
                    grd_AddRemark.Rows[0].Cells[0].Text = "<center>----- No Records Found -----</center>";
                }
            }
            else
            {
                SqlDataAdapter da = new SqlDataAdapter("", con);
                da = new SqlDataAdapter("select a.id,a.project,a.remark,(select c.username from tbl_usermaster c where c.userid=a.requestby)[requestby],a.date from tbl_ProjectRemarks a inner join tbl_ProjectReq b on b.projectid=a.project where a.project='" + lbl_ProjectIDTextView.InnerText + "' and a.status=1 order by a.date desc", con);
                DataSet ds = new DataSet();

                da.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    grd_AddRemark.DataSource = ds;
                    grd_AddRemark.DataBind();
                }
                else
                {
                    ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                    grd_AddRemark.DataSource = ds;
                    grd_AddRemark.DataBind();
                    int columncount = grd_AddRemark.Rows[0].Cells.Count;
                    grd_AddRemark.Rows[0].Cells.Clear();
                    grd_AddRemark.Rows[0].Cells.Add(new TableCell());
                    grd_AddRemark.Rows[0].Cells[0].ColumnSpan = columncount;
                    grd_AddRemark.Rows[0].Cells[0].Text = "<center>----- No Records Found -----</center>";
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "Alert", "swal('" + ex.ToString().Substring(0, 60) + "');", true);
        }
    }


    // <summary>
    // Delete Project Remarks
    // </summary>
    // <param name="sender">Sender.</param>
    // <param name="e">E.</param>
    protected void grd_AddRemark_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {

                string id = (string)grd_AddRemark.DataKeys[e.RowIndex].Value.ToString();
                com_scope.Connection = con;
                con.Open();
                com_scope.CommandText = "update tbl_ProjectRemarks set status=0 where id='" + id + "' and requestby='" + Session["userid"].ToString() + "'";
                int num_records = com_scope.ExecuteNonQuery();
                if (num_records == 0)
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Alert", "swal('You are not allowed to delete this remark');", true);
                }
                projectremarks_GridBind();
            }
            else
            {

            }
        }
        catch (Exception ex)
        {

        }
    }

    // <summary>
    // Delete Projects
    // </summary>
    // <param name="sender">Sender.</param>
    // <param name="e">E.</param>
    protected void grd_Projects_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                string id = (string)grd_Projects.DataKeys[e.RowIndex].Value;
                com_check.Connection = con2;
                con2.Open();
                com_check.CommandText = "Select projectid from tbl_taskmaster where projectid= '" + id + "' and status=1";
                SqlDataReader dr = this.com_check.ExecuteReader();
                if (dr.HasRows)
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Alert", "alert('Project You Selected Was Used in Tasks');", true);
                }
                else
                {
                    com_scope.Connection = con;
                    con.Open();
                    com_scope.CommandText = "update tbl_ProjectReq set status=0 where projectid='" + id + "'";
                    com_scope.ExecuteNonQuery();
                    project_GridBind();
                }
            }
            else
            {

            }
            ScriptManager.RegisterStartupScript(this, GetType(), "setDatatable", "setDatatable();", true);
        }
        catch (Exception)
        {
        }
    }
    protected void grd_Projects_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
        }
        catch (Exception)
        {
        }
    }

    protected void grd_Projects_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            grd_Projects.EditIndex = -1;
            project_GridBind();
        }
        catch (Exception)
        {


        }
    }

    protected void btn_NewProject_Click(object sender, System.EventArgs e)
    {
        inserVal = 3;
        ViewState["Insert"] = inserVal;
        try
        {
            this.block_Grid.Visible = false;
            this.block_Register.Visible = true;
            com.CommandText = "delete from tbl_Coordinator where projectid not in(select projectid from tbl_ProjectReq)";
            com.Connection = con;
            con.Close();
            con.Open();
            com.ExecuteNonQuery();
            bindScope();
            BindCoOrdinators();
        }
        catch (Exception)
        {
        }
    }


    // <summary>
    // Projects Updation Function
    // </summary>
    // <param name="sender">Sender.</param>
    // <param name="e">E.</param>
    protected void grd_Projects_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            inserVal = 2;
            GridViewRow row = grd_Projects.SelectedRow;
            hdn_ProjectID.Value = ((Label)row.FindControl("lbl_ProjectID")).Text;
            txt_ProjectID.Value = ((Label)row.FindControl("lbl_ProjectID")).Text;
            txt_ClientProjectID.Value = ((Label)row.FindControl("lbl_ClientProjectID")).Text;
            txt_ProjectName.Value = ((Label)row.FindControl("lbl_ProjectName")).Text;
            if (((Label)row.FindControl("lbl_ProjectType")).Text == "External" || ((Label)row.FindControl("lbl_ProjectType")).Text == "CB" || ((Label)row.FindControl("lbl_ProjectType")).Text == "" || ((Label)row.FindControl("lbl_ProjectType")).Text == "NULL")
            {
                icon_SubType.Visible = false;
                ddl_ProjectSubType.Visible = false;
                lbl_ProjectSubType.Visible = false;
                ddl_ProjectType.SelectedValue = ((Label)row.FindControl("lbl_ProjectType")).Text;
                if (((Label)row.FindControl("lbl_ProjectType")).Text == "External")
                {
                    icon_ReceivedFrom.Visible = true;
                    txt_ReceivedFrom.Visible = true;
                    lbl_ReceivedFrom.Visible = true;
                    txt_ReceivedFrom.Text = ((HiddenField)row.FindControl("hdn_ReceivedFrom")).Value;
                }
            }
            else if (((Label)row.FindControl("lbl_ProjectType")).Text == "Internal")
            {
                ddl_ProjectType.SelectedValue = ((Label)row.FindControl("lbl_ProjectType")).Text;
                icon_SubType.Visible = true;
                ddl_ProjectSubType.Visible = true;
                lbl_ProjectSubType.Visible = true;
                ddl_ProjectSubType.SelectedIndex = 0;
            }
            else
            {
                ddl_ProjectType.SelectedValue = "Internal";
                icon_SubType.Visible = true;
                ddl_ProjectSubType.Visible = true;
                lbl_ProjectSubType.Visible = true;
                ddl_ProjectSubType.SelectedValue = ((Label)row.FindControl("lbl_ProjectType")).Text;
            }
            com.Connection = con;
            com.CommandText = "select allotedteamid from tbl_ProjectReq where projectid='" + txt_ProjectID.Value + "'";
            con.Open();
            SqlDataReader dr = this.com.ExecuteReader();
            string db_teamid;
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    db_teamid = dr["allotedteamid"].ToString();
                    string[] splittedteamid = db_teamid.Split(',');
                    for (int i = 0; i < splittedteamid.Length; i++)
                    {
                        try
                        {
                            chk_Team.Items.FindByValue(splittedteamid[i]).Selected = true;
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
                dr.Close();
            }
            txt_ReceivedDate.Text = ((Label)row.FindControl("lbl_ReceivedDate")).Text;
            if (((Label)row.FindControl("lbl_DueDate")).Text != "TBD")
            {
                txt_DueDate.Text = ((Label)row.FindControl("lbl_DueDate")).Text;
            }
            tbx_Desc.Text = ((HiddenField)row.FindControl("hdn_Description")).Value;
            tbx_Remarks.Text = ((HiddenField)row.FindControl("hdn_Remark")).Value;
            this.block_Grid.Visible = false;
            this.block_Register.Visible = true;
            ViewState["update"] = inserVal;
            btn_Save.Text = "Update";
            bindScope();
            BindCoOrdinators();
        }
        catch (Exception)
        {
            Response.Redirect(Request.Url.AbsoluteUri);
        }
    }

    // <summary>
    // Currently Not in Use
    // </summary>
    // <param name="sender">Sender.</param>
    // <param name="e">E.</param>
    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grd_Projects.PageIndex = e.NewPageIndex;
            this.project_GridBind();
        }
        catch (Exception)
        {

        }
    }

    // <summary>
    // Scope Gridview PageIndexChanging
    // </summary>
    // <param name="sender">Sender.</param>
    // <param name="e">E.</param>
    protected void OnPageIndexChangingScope(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grd_Scope.PageIndex = e.NewPageIndex;
            this.bindScope();
        }
        catch (Exception)
        {

        }
    }

    // <summary>
    // Back Button Click Function
    // </summary>
    // <param name="sender">Sender.</param>
    // <param name="e">E.</param>
    protected void btn_BackButtonClick(object sender, EventArgs e)
    {
        this.block_Grid.Visible = true;
        this.block_View.Visible = false;
        this.block_Register.Visible = false;
        this.TitleOfPage.InnerText = "Project Master";
        ScriptManager.RegisterStartupScript(this, GetType(), "setDatatable", "setDatatable();", true);
        grd_ProjectHistory.DataSource = null;
        grd_ProjectHistory.DataBind();
        block_ProjectHistory.Visible = false;
    }
    /// <summary>
    /// Export DPR'S For the Project
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void btn_ExportDPRs_Click(object sender, EventArgs e)
    {
        string projectid = Convert.ToString(lbl_ProjectIDTextView.InnerText);
        DataTable dt_DPRS = objData.Getdata("select a.EmpNo+' - '+a.EmpName[EmpName],a.Shift,convert(varchar(10),a.CurrentDate,103)[currentdate],(select b.projectname from tbl_ProjectReq b where b.projectid='" + projectid + "')[Projectid],(select c.scope from tbl_scope c where c.ID=a.scope)[scope],(select c.stage from tbl_MstStageMaster c where c.slno=a.stage)[stage],(select d.taskname from tbl_taskmaster d where d.id=a.task)[task],RIGHT(a.StartTime,5)[StartTime],RIGHT(a.EndTime,5)[EndTime],a.Break1 as BreakTime,a.TotalTime,a.Statusoftask[statusoftask],a.Remarks from prmsProductionHour_Backup a where a.projectid='" + projectid + "' order by a.currentdate desc");
        if (dt_DPRS.Rows.Count > 0)
        {
            List<string> HeaderNames = new List<string>();
            HeaderNames.Add("Employee");
            HeaderNames.Add("Shift");
            HeaderNames.Add("Date");
            HeaderNames.Add("Project");
            HeaderNames.Add("Scope");
            HeaderNames.Add("Stage");
            HeaderNames.Add("Task");
            HeaderNames.Add("Start Time");
            HeaderNames.Add("End Time");
            HeaderNames.Add("Break");
            HeaderNames.Add("Total Time");
            HeaderNames.Add("% Completed");
            HeaderNames.Add("Remarks");
            List<string> ExcelReport = pcData.generateExcelReport(dt_DPRS, "ProjectDPRS", "GenericReports", "Project DPRS", 13, HeaderNames);
            FileInfo file = new FileInfo(ExcelReport[2]);
            Response.Clear();
            Response.AddHeader("Content-Length", file.Length.ToString(CultureInfo.InvariantCulture));
            Response.AddHeader("Content-Disposition", "attachment;filename=\"" + ("ProjectDPRS.xls") + "\"");
            Response.ContentType = "application/octet-stream";
            Response.Flush();
            Response.TransmitFile(ExcelReport[0] + ("ProjectDPRS" + DateTime.Now.ToShortDateString().Replace("/", "") + ".xls"));
            Response.End();
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "swal('No Data Found','','warning')", true);
        }
    }
    // <summary>
    // View Projects Details
    // </summary>
    // <param name="sender">Sender.</param>
    // <param name="e">E.</param>
    protected void SelectCurrentData(object sender, EventArgs e)
    {
        this.block_Grid.Visible = false;
        this.block_Register.Visible = false;
        this.block_View.Visible = true;
        LinkButton btn = (LinkButton)(sender);
        projectid = btn.CommandArgument;
        con.Close();
        con.Open();
        com_projectView.Connection = con;
        com_projectView.CommandText = "select case when exists(select projectid from tbl_ProjectHistory where projectid=a.projectid) then CONVERT(VARCHAR(20),b.receiveddate,103) else CONVERT(VARCHAR(20),a.receiveddate,103) end as receiveddate,case when exists(select projectid from tbl_ProjectHistory where projectid=a.projectid) then CONVERT(VARCHAR(20),b.duedate,103) else replace(substring(a.duedate, 4,2) + '/' + substring(a.duedate, 1, 2)  + '/' + substring(a.duedate, 7, 4),'/TB/','TBD') end AS DueDate2,a.allotedteamname[Alloted Team],a.* from tbl_ProjectReq a left outer join tbl_ProjectHistory b on b.projectid=a.projectid and b.id in (select max(id) from tbl_ProjectHistory where projectid=a.projectid) where a.projectid='" + projectid + "'";
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
            if (dr["remarks"].ToString() == "")
            {
                lbl_RemarksTextView.InnerText = "N/A";
            }
            else
            {
                lbl_RemarksTextView.InnerText = dr["remarks"].ToString();
            }
        }
        projectremarks_GridBind();
        string isReOpened = objData.GetSingleData("select projectid from tbl_ProjectHistory where projectid='" + projectid + "'");
        if (isReOpened != "0" || isReOpened == "False")
            projectHistoryBind();
    }

    // <summary>
    // Proejcts Editing Function Not in Use
    // </summary>
    // <param name="sender">Sender.</param>
    // <param name="e">E.</param>
    protected void grd_Projects_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridViewRow row = grd_Projects.Rows[e.NewEditIndex];
        // string serialNo = row.Cells[1].ToString();
        // int slno = Convert.ToInt32(grd_Projects.SelectedDataKey[e.NewEditIndex].ToString());
        // int empId = Convert.ToInt32(grd_Projects.DataKeys[e.RowIndex].Value); 
        this.block_Grid.Visible = false;
        this.block_Register.Visible = false;
        this.block_View.Visible = true;
        grd_Projects.EditIndex = e.NewEditIndex;

        //Bind Grid Than Find Control 

        //GridViewRow row = grd_Projects.SelectedRow;

        //txt_ClientProjectID.Value = ((Label)row.FindControl("lbl_ClientProjectID")).Text;
        //Projectname.Value = ((Label)row.FindControl("lbl_ProjectName")).Text;
        //ReceivedDate.Text = ((Label)row.FindControl("lbl_ReceivedDate")).Text;
        //tbx_Desc.Text = ((HiddenField)row.FindControl("hdn_Description")).Value;
        //tbx_Remarks.Text = ((HiddenField)row.FindControl("hdn_Remark")).Value;


    }


    // <summary>
    // Team select to Allot Project
    // </summary>
    // <param name="sender">Sender.</param>
    // <param name="e">E.</param>
    protected void chk_Team_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindScope();
        BindCoOrdinators();
    }

    // <summary>
    // To Change Project Status To Hold,Closed.
    // </summary>
    // <param name="sender">Sender.</param>
    // <param name="e">E.</param>
    protected void DeleteProject(object sender, EventArgs e)
    {
        LinkButton lnkbtn = (LinkButton)sender;
        string projectID = lnkbtn.CommandArgument;
        //popup_Delete.Show();

    }

    // <summary>
    // Status of Project Change
    // </summary>
    // <param name="sender">Sender.</param>
    // <param name="e">E.</param>
    protected void btn_DeleteProject_Click(object sender, EventArgs e)
    {
        com_modal.Connection = con;
        con.Open();
        com_modal.CommandText = "update tbl_ProjectReq set reason='" + txt_Reason.Text + "',projectstatus='" + ddl_DelStatus.SelectedValue + "',status='0' where projectid='" + ViewState["selectedProjectID"] + "'";
        com_modal.ExecuteNonQuery();
        if (ddl_DelStatus.SelectedValue == "Hold")
        {
            string status;
            com_modal.CommandText = "insert into tbl_ProjectHolds(reason,holdeddate,projectid) values(@Reason,GETDATE(),@projectid)";
            com_modal.Parameters.Add("@Reason", txt_Reason.Text.ToString());
            com_modal.Parameters.Add("@projectid", ViewState["selectedProjectID"]);
            com_modal.Connection = con;
            con.Close();
            con.Open();
            com_modal.ExecuteNonQuery();
            com_modal.Connection = con;
            com_modal.CommandText = "select statusoftask from PrmsProductionHour_backup where projectid='" + ViewState["selectedProjectID"] + "'";
            con.Close();
            con.Open();
            SqlDataReader dr = com_modal.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                status = "WIP";
            }
            else
            {
                status = "Yet To Start";
            }
            dr.Close();
            com_modal.CommandText = "insert into tbl_ProjectStatusMaster(project,currentstate,changedstate,dateofchange) values(@projectid,@status,'Hold',GETDATE())";
            com_modal.Parameters.Clear();
            com_modal.Parameters.Add("@projectid", ViewState["selectedProjectID"]);
            com_modal.Parameters.Add("@status", status);
            com_modal.Connection = con;
            con.Close();
            con.Open();
            com_modal.ExecuteNonQuery();
            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "swal('Project Holded Successfully','','success')", true);
        }
        if (ddl_DelStatus.SelectedValue == "Closed")
        {
            string status;
            com_modal.CommandText = "insert into tbl_ProjectHolds(reason,closeddate,projectid) values(@Reason,GETDATE(),@projectid)";
            com_modal.Parameters.Clear();
            com_modal.Parameters.Add("@projectid", ViewState["selectedProjectID"]);
            com_modal.Parameters.Add("@Reason", txt_Reason.Text);
            com_modal.Connection = con;
            con.Close();
            con.Open();
            com_modal.ExecuteNonQuery();
            com_modal.Connection = con;
            com_modal.CommandText = "select statusoftask from PrmsProductionHour_backup where projectid='" + ViewState["selectedProjectID"] + "'";
            SqlDataReader dr = com_modal.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                status = "WIP";
            }
            else
            {
                status = "Yet To Start";
            }
            dr.Close();
            com_modal.CommandText = "insert into tbl_ProjectStatusMaster(project,currentstate,changedstate,dateofchange) values(@projectid,@status,'Closed',GETDATE())";
            com_modal.Parameters.Clear();
            com_modal.Parameters.Add("@projectid", ViewState["selectedProjectID"]);
            com_modal.Parameters.Add("@status", status);
            com_modal.Connection = con;
            con.Close();
            con.Open();
            com_modal.ExecuteNonQuery();
            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "swal('Project Closed Successfully','','success')", true);
        }
        //com_modal.CommandText = "update tbl_taskmaster set taskstatus='" + ddl_DelStatus.SelectedValue + "' where projectid='" + ViewState["selectedProjectID"] + "'";
        //com_modal.ExecuteNonQuery();
        project_GridBind();
        txt_Reason.Text = string.Empty;
        ddl_DelStatus.SelectedIndex = 0;
        ScriptManager.RegisterStartupScript(this, GetType(), "setDatatable", "setDatatable();", true);
    }

    // <summary>
    // Release Project Hold State
    // </summary>
    // <param name="sender">Sender.</param>
    // <param name="e">E.</param>
    protected void btn_ReleaseProject_Click(object sender, EventArgs e)
    {
        string HoldID = "";
        string status = "";
        com_modal.Connection = con;
        con.Open();
        if (ddl_RelStatus.SelectedValue == "WIP")
        {
            com_modal.Connection = con;
            com_modal.CommandText = "select statusoftask from PrmsProductionHour_backup where projectid='" + ViewState["selectedProjectID"] + "'";
            SqlDataReader read = com_modal.ExecuteReader();
            if (read.HasRows)
            {
                read.Read();
                status = "WIP";
            }
            else
            {
                status = "Yet To Start";
            }
            read.Close();
            com_modal.CommandText = "update tbl_ProjectReq set projectstatus='" + ddl_RelStatus.SelectedValue + "',status='1' where projectid='" + ViewState["selectedProjectID"] + "'";
            com_modal.ExecuteNonQuery();
            com_modal.CommandText = "select TOP 1 id from tbl_ProjectHolds where projectid='" + ViewState["selectedProjectID"] + "' order by id desc";
            SqlDataReader dr = com_modal.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                HoldID = dr[0].ToString();
            }
            dr.Close();
            com_modal.CommandText = "update tbl_ProjectHolds set wipdate=GETDATE() where id='" + HoldID + "'";
            com_modal.ExecuteNonQuery();
            com_modal.CommandText = "insert into tbl_ProjectStatusMaster(project,currentstate,changedstate,dateofchange) values('" + ViewState["selectedProjectID"] + "','Hold','" + status + "',GETDATE())";

            com_modal.ExecuteNonQuery();
            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "swal('Project State Changed to WIP Successfully','','success')", true);
        }
        else
        {
            com_modal.CommandText = "update tbl_ProjectReq set projectstatus='" + ddl_RelStatus.SelectedValue + "' where projectid='" + ViewState["selectedProjectID"] + "'";
            com_modal.ExecuteNonQuery();
            com_modal.CommandText = "select TOP 1 id from tbl_ProjectHolds where projectid='" + ViewState["selectedProjectID"] + "' order by id desc";
            SqlDataReader dr = com_modal.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                HoldID = dr[0].ToString();
            }
            dr.Close();
            com_modal.CommandText = "update tbl_ProjectHolds set closeddate=GETDATE() where id='" + HoldID + "'";
            com_modal.ExecuteNonQuery();
            com_modal.CommandText = "insert into tbl_ProjectStatusMaster(project,currentstate,changedstate,dateofchange) values(@projectid,'Hold','Closed',GETDATE())";
            com_modal.Parameters.Add("@projectid", ViewState["selectedProjectID"]);
            com_modal.Connection = con;
            con.Close();
            con.Open();
            com_modal.ExecuteNonQuery();
            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "swal('Project Closed Successfully','','success')", true);
        }
        //com_modal.CommandText = "update tbl_taskmaster set taskstatus='" + ddl_DelStatus.SelectedValue + "' where projectid='" + ViewState["selectedProjectID"] + "'";
        //com_modal.ExecuteNonQuery();
        project_GridBind();
        txt_Reason.Text = string.Empty;
        ddl_DelStatus.SelectedIndex = 0;
        ScriptManager.RegisterStartupScript(this, GetType(), "setDatatable", "setDatatable();", true);
    }

    // <summary>
    // Project Hold/Closed Function
    // </summary>
    // <param name="sender">Sender.</param>
    // <param name="e">E.</param>
    protected void lnk_Delete_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)(sender);
        ViewState["selectedProjectID"] = btn.CommandArgument;
        popup_Delete.Show();
        ScriptManager.RegisterStartupScript(this, GetType(), "setDatatable", "setDatatable();", true);
    }

    // <summary>
    // Release Project Hold Function
    // </summary>
    // <param name="sender">Sender.</param>
    // <param name="e">E.</param>
    protected void lnk_Release_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)(sender);
        ViewState["selectedProjectID"] = btn.CommandArgument;
        popup_Release.Show();
    }

    // <summary>
    // Close Panel of Project Hold popup
    // </summary>
    // <param name="sender">Sender.</param>
    // <param name="e">E.</param>
    protected void btn_Close(object sender, EventArgs e)
    {
        txt_Reason.Text = string.Empty;
        popup_Delete.Hide();
        ScriptManager.RegisterStartupScript(this, GetType(), "setDatatable", "setDatatable();", true);
    }

    // <summary>
    // RowDatabound to Differentiate Project Status
    // </summary>
    // <param name="sender">Sender.</param>
    // <param name="e">E.</param>
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
            var lk_TaskDetails = (LinkButton)e.Row.FindControl("lnk_TaskDetails");
            string Rights = Convert.ToString(Session["Rights"]);
            //format color of the as below
            if (status == "Closed")
            {
                (e.Row.FindControl("lbl_CompletedDate") as Label).Text = "TBD";
                (e.Row.FindControl("lnk_ProjectStatus") as LinkButton).CssClass = "btn btn-danger";
                lk_Delete.Enabled = false;
                lk_Delete.Style.Add("opacity", "0.5");
                lk_Edit.Enabled = false;
                lk_Edit.Style.Add("opacity", "0.5");
                lk_TaskDetails.Enabled = false;
                lk_TaskDetails.Style.Add("opacity", "0.5");
            }

            if (status == "Yet To Start")
            {
                (e.Row.FindControl("lbl_CompletedDate") as Label).Text = "TBD";
                (e.Row.FindControl("lnk_ProjectStatus") as LinkButton).CssClass = "btn btn-default";
            }

            if (status == "Hold")
            {
                (e.Row.FindControl("lbl_CompletedDate") as Label).Text = "TBD";
                (e.Row.FindControl("lnk_ProjectStatus") as LinkButton).CssClass = "btn btn-warning";
                lk_Delete.Enabled = false;
                lk_Delete.Style.Add("opacity", "0.5");
                lk_Edit.Enabled = false;
                lk_Edit.Style.Add("opacity", "0.5");
                lk_TaskDetails.Enabled = false;
                lk_TaskDetails.Style.Add("opacity", "0.5");
                (e.Row.FindControl("lnk_ProjectStatus") as LinkButton).Style.Add("cursor", "pointer");
            }

            if (status == "WIP")
            {
                (e.Row.FindControl("lbl_CompletedDate") as Label).Text = "TBD";
                (e.Row.FindControl("lnk_ProjectStatus") as LinkButton).CssClass = "btn btn-info";
                if ((e.Row.FindControl("lbl_ClientProjectID") as Label).Text.ToString() != "NA")
                {
                    (e.Row.FindControl("lnk_ProjectStatus") as LinkButton).Style.Add("cursor", "pointer");
                }
                else
                {
                    (e.Row.FindControl("lnk_ProjectStatus") as LinkButton).Enabled = false;
                    (e.Row.FindControl("lnk_Delete") as LinkButton).Enabled = false;
                    lk_Delete.Style.Add("opacity", "0.5");
                }
            }

            if (status == "Completed")
            {
                String CompletedDate = string.Empty;
                com_check.Connection = con;
                con.Close();
                con.Open();
                com_check.CommandText = "";
                string isReOpened = objData.GetSingleData("select completeddate from tbl_ProjectHistory where projectid='" + (e.Row.FindControl("lbl_ProjectID") as Label).Text + "' and id in(select max(id) from tbl_ProjectHistory where projectid='" + (e.Row.FindControl("lbl_ProjectID") as Label).Text + "')");
                if (isReOpened == "0")
                {
                    CompletedDate = objData.GetSingleData("select completeddate from tbl_ProjectReq where projectid='" + (e.Row.FindControl("lbl_ProjectID") as Label).Text + "'");
                }
                else
                {
                    DateTime dt = Convert.ToDateTime(isReOpened);
                    CompletedDate = dt.ToString("dd/MM/yyyy");
                }
                (e.Row.FindControl("lbl_CompletedDate") as Label).Text = CompletedDate;
                (e.Row.FindControl("lnk_ProjectStatus") as LinkButton).CssClass = "btn btn-success";
                (e.Row.FindControl("lnk_ProjectStatus") as LinkButton).Style.Add("cursor", "pointer");
                lk_Delete.Enabled = false;
                lk_Delete.Style.Add("opacity", "0.5");
            }
        }
    }

    // <summary>
    // Currently Not In Use
    // </summary>
    // <param name="sender">Sender.</param>
    // <param name="e">E.</param>
    protected void lnkbtn_Search_Click(object sender, EventArgs e)
    {
        project_GridBind();
    }

    // <summary>
    // Project received Date change function to limit the project duedate
    // </summary>
    // <param name="sender">Sender.</param>
    // <param name="e">E.</param>
    protected void txt_ReceivedDate_TextChanged(object sender, EventArgs e)
    {
        txt_DueDate.Text = "";
        string DueDateLimitStart = Convert.ToString(DateTime.ParseExact(txt_ReceivedDate.Text.Trim(), "dd/MM/yyyy", null).ToString("MM/dd/yyyy"));
        txt_DueDateCalendarExtender.StartDate = Convert.ToDateTime(DueDateLimitStart);
        bindScope();
        BindCoOrdinators();
    }
    // <summary>
    // Date Compare While Re Open Project
    // </summary>
    // <param name="sender"></param>
    // <param name="e"></param>
    protected void txt_ReceivedDateReOpen_TextChanged(object sender, EventArgs e)
    {
        txt_DueDateReOpen.Text = txt_ReceivedDateReOpen.Text;
        string DueDateLimitStart = Convert.ToString(DateTime.ParseExact(txt_ReceivedDateReOpen.Text.Trim(), "dd/MM/yyyy", null).ToString("MM/dd/yyyy"));
        calext_DueDateReOpen.StartDate = Convert.ToDateTime(DueDateLimitStart);
        popup_CompletedState.Show();
        ScriptManager.RegisterStartupScript(this, GetType(), "setDatatable", "setDatatable();", true);
    }
    // <summary>
    // Popup show if completed date is changed
    // </summary>
    // <param name="sender">Sender.</param>
    // <param name="e">E.</param>
    protected void txt_CompletedDate_TextChanged(object sender, EventArgs e)
    {
        if (txt_CompletedDate.Text != "")
        {
            string DueDate = string.Empty;
            popup_CompletedState.Show();
            string isReAlloted = objData.GetSingleData("select isNull(isReAllot,0) from tbl_ProjectReq where projectid='" + ViewState["selectedProjectID"].ToString() + "'");
            if (isReAlloted == "0" || isReAlloted == "False")
            {
                DueDate = objData.GetSingleData("select duedate from tbl_ProjectReq where projectid='" + ViewState["selectedProjectID"].ToString() + "'");
            }
            else
            {
                DueDate = objData.GetSingleData("select duedate from tbl_ProjectHistory where projectid='" + ViewState["selectedProjectID"].ToString() + "' and id in(select max(id) from tbl_ProjectHistory where projectid='" + ViewState["selectedProjectID"].ToString() + "')");
            }
            if (txt_CompletedDate.Text != "")
            {
                ConvertedCompletedDate = DateTime.ParseExact(txt_CompletedDate.Text.ToString(), "dd/MM/yyyy", null).ToString("MM/dd/yyyy");
            }
            result = 0;
            if (DueDate != "TBD")
            {
                if (isReAlloted == "0" || isReAlloted == "False")
                {
                    ConvertedDueDate = DateTime.ParseExact(DueDate, "MM/dd/yyyy", null).ToString("MM/dd/yyyy");
                }
                else
                {
                    ConvertedDueDate = DueDate;
                }
                result = DateTime.Compare(Convert.ToDateTime(ConvertedCompletedDate).Date, Convert.ToDateTime(ConvertedDueDate).Date);
            }
            if (result > 0)
            {
                lbl_ReasonForExtend.Visible = true;
                txt_ReasonForExtend.Visible = true;
            }
            else
            {
                lbl_ReasonForExtend.Visible = false;
                txt_ReasonForExtend.Visible = false;
            }
            icon_CompletedDate.Visible = true;
            txt_CompletedDate.Visible = true;
            lbl_CompletedDate.Visible = true;
        }
        ScriptManager.RegisterStartupScript(this, GetType(), "setDatatable", "setDatatable();", true);
    }
    // <summary>
    // Exit Click From PopUp
    // </summary>
    // <param name="sender"></param>
    // <param name="e"></param>
    protected void btn_Exit_Click(object sender, EventArgs e)
    {
        popup_CompletedState.Hide();
        txt_ReceivedDateReOpen.Text = string.Empty;
        txt_DueDateReOpen.Text = string.Empty;
        ScriptManager.RegisterStartupScript(this, GetType(), "setDatatable", "setDatatable();", true);
    }
    // <summary>
    // On Project status Click To Call Appropriate function based on project status
    // </summary>
    // <param name="sender">Sender.</param>
    // <param name="e">E.</param>
    protected void lnk_ProjectStatus_Click(object sender, EventArgs e)
    {
        txt_CompletedDate.Attributes.Add("readonly", "readonly");
        txt_CompletedDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
        //calext_CompletedDate.StartDate = DateTime.Now.AddDays(-7);
        LinkButton lnk_ProjectStatus = (LinkButton)sender;
        GridViewRow row = (GridViewRow)lnk_ProjectStatus.NamingContainer;
        LinkButton lnk_Status = (LinkButton)row.FindControl("lnk_ProjectStatus");
        ViewState["selectedProjectID"] = lnk_ProjectStatus.CommandArgument;
        if (lnk_Status.Text == "WIP" || lnk_Status.Text == "Completed")
        {
            if (lnk_Status.Text == "Completed")
            {
                ddl_ProjectState.Items.Clear();
                ddl_ProjectState.Items.Add("ReOpen");
                ddl_ProjectState.Items[0].Value = "WIP";
                string DateCompleted = objData.GetSingleData("select completeddate from tbl_ProjectHistory where projectid='" + ViewState["selectedProjectID"].ToString() + "' and id in(select max(id) from tbl_ProjectHistory where projectid='" + ViewState["selectedProjectID"].ToString() + "')");
                DateTime dt_DateCompleted;
                if (DateCompleted == "0" || DateCompleted == "")
                {
                    DateCompleted = objData.GetSingleData("select completeddate from tbl_ProjectReq where projectid='" + ViewState["selectedProjectID"].ToString() + "'");
                    dt_DateCompleted = Convert.ToDateTime(Convert.ToString(DateTime.ParseExact(DateCompleted.ToString().Trim(), "dd/MM/yyyy", null).ToString("MM/dd/yyyy"))).Date;
                }
                else
                {
                    dt_DateCompleted = Convert.ToDateTime(DateCompleted);
                }
                calext_ReceivedDateReOpen.StartDate = dt_DateCompleted;
                calext_DueDateReOpen.StartDate = dt_DateCompleted;
                txt_ReceivedDateReOpen.Attributes.Add("readonly", "readonly");
                txt_DueDateReOpen.Attributes.Add("readonly", "readonly");
                calext_CompletedDate.Enabled = false;
                pnl_CompletedState.Style.Add("height", "70%");
                title_OnCompleted.InnerText = "Re Open Project?";
                block_ReOpenProject.Visible = true;
                pnl_CompletedState.Style.Add("border-color", "rgba(0, 255, 255, 0.47)");
            }
            else
            {
                ddl_ProjectState.Items.Clear();
                ddl_ProjectState.Items.Add("Completed");
                pnl_CompletedState.Style.Add("height", "48%");
                calext_CompletedDate.Enabled = true;
                title_OnCompleted.InnerText = "Confirm Completed?";
                block_ReOpenProject.Visible = false;
                pnl_CompletedState.Style.Add("border-color", "rgba(0, 236, 6, 0.419608)");
            }
            txt_ReasonForExtend.Text = "";
            popup_CompletedState.Show();
            ddl_ProjectState_SelectedIndexChanged(sender, e);
        }
        else if (lnk_Status.Text == "Hold")
        {
            popup_Release.Show();
        }
        ScriptManager.RegisterStartupScript(this, GetType(), "setDatatable", "setDatatable();", true);
    }

    // <summary>
    // show popup of change project status
    // </summary>
    // <param name="sender">Sender.</param>
    // <param name="e">E.</param>
    protected void ddl_ProjectState_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_ProjectState.SelectedValue == "WIP")
        {
            icon_CompletedDate.Visible = false;
            txt_CompletedDate.Visible = false;
            lbl_CompletedDate.Visible = false;
            lbl_ReasonForExtend.Visible = false;
            txt_ReasonForExtend.Visible = false;
            popup_CompletedState.Show();
        }
        else
        {
            string isReAlloted = objData.GetSingleData("select isNull(isReAllot,0) from tbl_ProjectReq where projectid='" + ViewState["selectedProjectID"].ToString() + "'");
            if (isReAlloted == "0" || isReAlloted == "False")
            {
                DueDate = objData.GetSingleData("select duedate from tbl_ProjectReq where projectid='" + ViewState["selectedProjectID"].ToString() + "'");
            }
            else
            {
                DueDate = objData.GetSingleData("select duedate from tbl_ProjectHistory where projectid='" + ViewState["selectedProjectID"].ToString() + "' and id in(select max(id) from tbl_ProjectHistory where projectid='" + ViewState["selectedProjectID"].ToString() + "')");
            }
            result = 0;
            if (txt_CompletedDate.Text != "")
            {
                ConvertedCompletedDate = DateTime.ParseExact(txt_CompletedDate.Text.ToString(), "dd/MM/yyyy", null).ToString("MM/dd/yyyy");
            }
            if (DueDate != "TBD")
            {
                if (isReAlloted == "0" || isReAlloted == "False")
                {
                    ConvertedDueDate = DateTime.ParseExact(DueDate, "MM/dd/yyyy", null).ToString("MM/dd/yyyy");
                }
                else
                {
                    ConvertedDueDate = DueDate;
                }
                result = DateTime.Compare(Convert.ToDateTime(ConvertedCompletedDate).Date, Convert.ToDateTime(ConvertedDueDate).Date);
            }
            if (result > 0)
            {
                lbl_ReasonForExtend.Visible = true;
                txt_ReasonForExtend.Visible = true;
            }
            else
            {
                lbl_ReasonForExtend.Visible = false;
                txt_ReasonForExtend.Visible = false;
            }
            icon_CompletedDate.Visible = true;
            txt_CompletedDate.Visible = true;
            lbl_CompletedDate.Visible = true;
            popup_CompletedState.Show();
        }
        ScriptManager.RegisterStartupScript(this, GetType(), "setDatatable", "setDatatable();", true);
    }

    // <summary>
    // Complete the project on complete button click
    // </summary>
    // <param name="sender">Sender.</param>
    // <param name="e">E.</param>
    protected void btn_CompleteProject_Click(object sender, EventArgs e)
    {
        string projectstatus = "";
        ProjectID = ViewState["selectedProjectID"].ToString();
        com_check.Connection = con;
        con.Close();
        con.Open();
        com_check.CommandText = "select projectid from tbl_taskmaster where taskstatus in('WIP','Yet To Start','Hold') and projectid='" + ProjectID + "'";
        SqlDataReader dr = com_check.ExecuteReader();
        if (dr.HasRows)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "swal('Finish all tasks before completing project');", true);
        }
        else
        {
            dr.Close();
            com_insert.Connection = con;
            con.Close();
            con.Open();
            if (ddl_ProjectState.SelectedValue == "Completed")
            {
                string isRevert = objData.GetSingleData("select isNull(isReAllot,0) from tbl_ProjectReq where projectid='" + ProjectID + "'");
                if (isRevert == "0" || isRevert == "False")
                {
                    com_insert.CommandText = "update tbl_ProjectReq set projectstatus='" + ddl_ProjectState.SelectedValue + "',completeddate='" + txt_CompletedDate.Text + "',extendreason='" + txt_ReasonForExtend.Text + "' where projectid='" + ProjectID + "'";
                    com_insert.ExecuteNonQuery();
                }
                else
                {
                    com_insert.CommandText = "update tbl_ProjectHistory set completeddate='" + Convert.ToString(DateTime.ParseExact(txt_CompletedDate.Text.Trim(), "dd/MM/yyyy", null).ToString("MM/dd/yyyy")) + "',extendreason='" + txt_ReasonForExtend.Text + "' where projectid='" + ProjectID + "' and id in(select max(id) from tbl_ProjectHistory where projectid='" + ProjectID + "')";
                    com_insert.ExecuteNonQuery();
                    com_insert.CommandText = "update tbl_ProjectReq set projectstatus='" + ddl_ProjectState.SelectedValue + "',isReAllot=0 where projectid='" + ProjectID + "'";
                    com_insert.ExecuteNonQuery();
                }
                com_insert.CommandText = "insert into tbl_ProjectStatusMaster(project,currentstate,changedstate,dateofchange) values(@projectid,'WIP','Completed',GETDATE())";
                com_insert.Parameters.Add("@projectid", ProjectID);
                com_insert.Connection = con;
                con.Close();
                con.Open();
                com_insert.ExecuteNonQuery();
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "swal('Project Completed Successfully','','success')", true);
            }
            if (ddl_ProjectState.SelectedValue == "WIP")
            {
                com_insert.Connection = con;
                com_insert.CommandText = "select projectstatus from tbl_ProjectReq where projectid='" + ProjectID + "'";
                SqlDataReader read = com_insert.ExecuteReader();
                if (read.HasRows)
                {
                    read.Read();
                    projectstatus = read[0].ToString();
                }
                read.Close();
                com_insert.CommandText = "update tbl_ProjectReq set projectstatus='" + ddl_ProjectState.SelectedValue + "',isReAllot=1 where projectid='" + ProjectID + "'";
                com_insert.ExecuteNonQuery();
                com_insert.CommandText = "insert into tbl_ProjectHistory(projectid,receiveddate,duedate,description,ReOpenDate,ReOpenBy) values(@ProjectID,'" + Convert.ToString(DateTime.ParseExact(txt_ReceivedDateReOpen.Text.Trim(), "dd/MM/yyyy", null).ToString("MM/dd/yyyy")) + "','" + Convert.ToString(DateTime.ParseExact(txt_DueDateReOpen.Text.Trim(), "dd/MM/yyyy", null).ToString("MM/dd/yyyy")) + "','" + txt_DescriptionReOpen.Text + "',GETDATE(),'" + Session["Userid"].ToString() + "')";
                com_insert.Parameters.Add("@projectid", ProjectID);
                com_insert.Connection = con;
                con.Close();
                con.Open();
                com_insert.ExecuteNonQuery();
                if (projectstatus == "Completed")
                {
                    com_insert.CommandText = "insert into tbl_ProjectStatusMaster(project,currentstate,changedstate,dateofchange) values('" + ProjectID + "','Completed','WIP',GETDATE())";
                    com_insert.ExecuteNonQuery();
                }
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "swal('Project Reopened Successfully','','success')", true);
            }
            project_GridBind();
        }
        ScriptManager.RegisterStartupScript(this, GetType(), "setDatatable", "setDatatable();", true);
    }

    // <summary>
    // On Project Type Selected Index changed for internal/external/CB
    // </summary>
    // <param name="sender">Sender.</param>
    // <param name="e">E.</param>
    protected void ddl_ProjectType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_ProjectType.SelectedItem.Value == "Internal")
        {
            icon_SubType.Visible = true;
            ddl_ProjectSubType.Visible = true;
            lbl_ProjectSubType.Visible = true;
            icon_ReceivedFrom.Visible = false;
            txt_ReceivedFrom.Visible = false;
            lbl_ReceivedFrom.Visible = false;
            bindSubTypes();
        }
        else if (ddl_ProjectType.SelectedItem.Value == "External")
        {
            icon_SubType.Visible = false;
            ddl_ProjectSubType.Visible = false;
            lbl_ProjectSubType.Visible = false;
            icon_ReceivedFrom.Visible = true;
            txt_ReceivedFrom.Visible = true;
            lbl_ReceivedFrom.Visible = true;
        }
        else
        {
            icon_SubType.Visible = false;
            ddl_ProjectSubType.Visible = false;
            lbl_ProjectSubType.Visible = false;
            icon_ReceivedFrom.Visible = false;
            txt_ReceivedFrom.Visible = false;
            lbl_ReceivedFrom.Visible = false;
        }
        bindScope();
        BindCoOrdinators();
    }

    // <summary>
    // Bind Subtypes if Project type is internal
    // </summary>
    protected void bindSubTypes()
    {
        con.Close();

        SqlDataAdapter da = new SqlDataAdapter("select ID,types from tbl_ProjectSubType", con);
        DataSet ds = new DataSet();
        da.Fill(ds);
        ddl_ProjectSubType.DataSource = ds;
        ddl_ProjectSubType.DataValueField = "types";
        ddl_ProjectSubType.DataTextField = "types";
        ddl_ProjectSubType.DataBind();
        ddl_ProjectSubType.Items.Insert(0, "Select");
    }
    /// <summary>
    /// Project History With Project ReOpen Details
    /// </summary>
    protected void projectHistoryBind()
    {
        try
        {
            block_ProjectHistory.Visible = true;
            string Rights = Convert.ToString(Session["Rights"]);
            IFormatProvider culture = new CultureInfo("en-US", true);
            con.Close();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter("", con);
            da = new SqlDataAdapter("select sno[id],projectreq,projectdesc,convert(varchar(10),receiveddate,103)[receiveddate],isnull(convert(varchar(10),duedate,103),'NA')[duedate],isNull(convert(varchar(10),completeddate,103),'NA')[completeddate],'NA'[ReOpenDate],'NA'[ReOpenBy],isNull(ExtendReason,'NA')[extendreason] from tbl_ProjectReq where projectid='" + projectid + "'", con);
            DataSet ds = new DataSet();
            da.Fill(ds);
            SqlDataAdapter da2 = new SqlDataAdapter("select id,(select projectreq from tbl_ProjectReq b where projectid='" + projectid + "')[projectreq],description[projectdesc],convert(varchar(10),receiveddate,103)[receiveddate],convert(varchar(10),duedate,103)[duedate],convert(varchar(10),completeddate,103)[completeddate],convert(varchar(10),ReOpenDate,103)[ReOpenDate],(select username from tbl_usermaster b where ReOpenBy=b.userid)[ReOpenBy],isNull(extendreason,'NA')[extendreason] from tbl_ProjectHistory where projectid='" + projectid + "' order by id", con);
            da2.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                grd_ProjectHistory.DataSource = ds;
                grd_ProjectHistory.DataBind();
            }
            else
            {
                ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                grd_ProjectHistory.DataSource = ds;
                grd_ProjectHistory.DataBind();
                int columncount = grd_ProjectHistory.Rows[0].Cells.Count;
                grd_ProjectHistory.Rows[0].Cells.Clear();
                grd_ProjectHistory.Rows[0].Cells.Add(new TableCell());
                grd_ProjectHistory.Rows[0].Cells[0].ColumnSpan = columncount;
                grd_ProjectHistory.Rows[0].Cells[0].Text = "<center>----- No Records Found -----</center>";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void lnk_TaskDetails_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)(sender);
        string Projectid = btn.CommandArgument;
        Response.Redirect("Tasks?projectid=" + Projectid);
    }
    #region Co-Ordinators
    /// <summary>
    /// Add New Co - Ordinator
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_AddCoordinator_Click(object sender, EventArgs e)
    {
        string ifExists = objData.GetSingleData("select name from tbl_CoOrdinator where projectid='" + txt_ProjectID.Value + "' and status=1 and name='" + txt_CoordinatorName.Text + "'");
        if (ifExists != "0")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "swal('Co-Ordinator Name Already Exists');", true);
        }
        else
        {
            string query = "insert into tbl_CoOrdinator(name,insertedDate,insertedBy,projectid,status) values (@name,GETDATE(),@insertedBy,@projectid,'1')";
            objData.DynamicParameters.Add("@name", Convert.ToString(txt_CoordinatorName.Text));
            objData.DynamicParameters.Add("@insertedBy", Convert.ToString(Session["Userid"]));
            objData.DynamicParameters.Add("@projectid", Convert.ToString(txt_ProjectID.Value));
            bool result = objData.InsertOrUpdateData(query, false, true);
            txt_CoordinatorName.Text = string.Empty;
        }
        BindCoOrdinators();
        bindScope();
    }
    /// <summary>
    /// Close Popup
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_ClosePopup_Click(object sender, EventArgs e)
    {

    }
    /// <summary>
    /// Binding Gridview For Co Ordinators
    /// </summary>
    protected void BindCoOrdinators()
    {
        try
        {
            con.Close();
            SqlDataAdapter da = new SqlDataAdapter("select ID,name from tbl_Coordinator where projectid='" + txt_ProjectID.Value + "' and status=1", con);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                grd_Coordinator.DataSource = ds;
                grd_Coordinator.DataBind();
            }
            else
            {
                ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                grd_Coordinator.DataSource = ds;
                grd_Coordinator.DataBind();
                int columncount = grd_Coordinator.Rows[0].Cells.Count;
                grd_Coordinator.Rows[0].Cells.Clear();
                grd_Coordinator.Rows[0].Cells.Add(new TableCell());
                grd_Coordinator.Rows[0].Cells[0].ColumnSpan = columncount;
                grd_Coordinator.Rows[0].Cells[0].Text = "<center>----- No Records Found -----</center>";
            }
            bindScope();
        }
        catch (Exception)
        {

        }
    }
    /// <summary>
    /// Cancelling Edit Gridview Event
    /// </summary>
    protected void grd_Coordinator_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            grd_Coordinator.EditIndex = -1;
            BindCoOrdinators();
        }
        catch (Exception)
        {
        }
    }
    /// <summary>
    /// Editing Coordinator
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grd_Coordinator_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            grd_Coordinator.EditIndex = e.NewEditIndex;
            BindCoOrdinators();
        }
        catch (Exception)
        {

        }
    }
    /// <summary>
    /// Updating Coordinatior Name
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grd_Coordinator_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            int id = Convert.ToInt32(grd_Coordinator.DataKeys[e.RowIndex].Value);
            TextBox Coordinator = (TextBox)grd_Coordinator.Rows[e.RowIndex].FindControl("txt_Coordinator");
            string ifExists = objData.GetSingleData("select name from tbl_Coordinator where projectid='"+txt_ProjectID.Value+"' and name='"+Coordinator.Text+"' and name not in(select name from tbl_Coordinator where id='"+id+"')");
            if (ifExists != "0")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "swal('Co-Ordinator Name Already Exists');", true);
            }
            else
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString());
                con.Open();
                SqlCommand cmd = new SqlCommand("update tbl_CoOrdinator set name='" + Coordinator.Text + "',updateddate=GETDATE(),updatedby='" + Convert.ToString(Session["Userid"]) + "' where ID='" + id + "'", con);
                cmd.ExecuteNonQuery();
                con.Close();
                grd_Coordinator.EditIndex = -1;
            }
            BindCoOrdinators();
        }
        catch (Exception)
        {
        }
    }
    /// <summary>
    /// Remove Coordinator
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grd_Coordinator_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            string confirmValue = string.Empty;
            string confirm_Value = Request.Form["confirm_value"];
            if (confirm_Value.Contains(","))
            {
                string[] values = confirm_Value.Split(',');
                confirmValue = values[values.Length - 1];
            }
            else
            {
                confirmValue = confirm_Value;
            }
            if (confirmValue == "Yes")
            {
                int id = (int)grd_Coordinator.DataKeys[e.RowIndex].Value;
                com_scope.Connection = con;
                con.Close();
                con.Open();
                com_scope.Connection = con;
                com_scope.CommandText = "update tbl_CoOrdinator set status=0,deleteddate=GETDATE(),deletedby='" + Convert.ToString(Session["Userid"]) + "' where ID='" + id + "'";
                com_scope.ExecuteNonQuery();
                BindCoOrdinators();
            }
        }
        catch (Exception)
        {
        }
    }
    /// <summary>
    /// Paging For Coordinator Gridview
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void OnPageIndexChangingCoordinator(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grd_Coordinator.PageIndex = e.NewPageIndex;
            this.BindCoOrdinators();
        }
        catch (Exception)
        {
        }
    }
    /// <summary>
    /// Delete Button Clicked On Gridview
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnk_DeleteCoordinator_Click(object sender, EventArgs e)
    {

    }
    #endregion
}
