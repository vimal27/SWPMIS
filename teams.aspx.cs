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

public partial class Teams : System.Web.UI.Page
{
    /// <summary>
    /// Declarations Part For Variables,Strings,SqlConnections,etc
    /// </summary>
    string description, StatusSearch;
    string var_Status = string.Empty;
    string status_update;
    StringBuilder htmlTable = new StringBuilder();
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString());
    SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DCSconn"].ToString());
    SqlCommand com_insert = new SqlCommand();
    SqlCommand com_update = new SqlCommand();
    SqlCommand com_check = new SqlCommand();
    SqlCommand com_checkStatus = new SqlCommand();
    SqlCommand com_Sch = new SqlCommand();
    SqlCommand com_team = new SqlCommand();
    SqlCommand com_scope = new SqlCommand();
	ProductionCalculation pcData = new ProductionCalculation();
    SqlDataReader dr;
    bool isInsert;
    int inserVal = 1;
    string Color11 = string.Empty;
    string project_id = string.Empty;
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
            //Block Direct URL Write
            if (Request.UrlReferrer == null)
            {
                Response.Redirect("Login");
            }
            teams_gridbind();
            txt_TeamID.Focus();
            //Redirects To Login Page If Session is Expired
            if (Session["Userid"] == null) Response.Redirect("Login");
            else if (Session["Userid"].ToString() == "") Response.Redirect("Login");
            if (!Page.IsPostBack)
            {
                this.block_Register.Visible = false;
                con.Close();
                con.Open();


                if (Session["sessiontype"] != "PM" && Convert.ToString(Session["sessiontype"]).ToLower() != "leader" && Convert.ToString(Session["sessiontype"]).ToLower() != "admin")
                {
                    btn_Save.Enabled = false;
                }
                else
                {
                    btn_Save.Enabled = true;
                }

            }
        }
        catch (Exception)
        {

        }
    }
    /// <summary>
    /// Clear If Back Function is Called
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void back(object sender, EventArgs e)
    {
        try
        {
            txt_TeamID.Value = "";
            txt_TeamName.Value = "";
        }
        catch (Exception)
        {

        }
    }
    /// <summary>
    /// Insert New Team or Update Team
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_Save_Click(object sender, EventArgs e)
    {

        try
        {

            int inserValCon = Convert.ToInt32(ViewState["Insert"]);
            if (inserValCon != 3)
            {
                con.Close();
                con.Open();
                com_update.Connection = con;
                com_update.CommandText = "update tbl_teams set Teamname=@teamname,TeamID=@teamid,status=@status where ID = '" + hdn_ID.Value + "'";
                com_update.Parameters.Add("@teamname", Convert.ToString(txt_TeamName.Value));
                com_update.Parameters.Add("@teamid", Convert.ToString(txt_TeamID.Value));
                if (rbl_Status.SelectedValue == "0")
                {
                    status_update = "0";
                    //DateTime date2 = DateTime.ParseExact(txt_releivedDate.Text, "dd/MM/yyyy", null);
                    //com_update.Parameters.Add("@releiveddate", date2);
                }
                else
                {
                    status_update = "1";
                    //com_update.Parameters.Add("@releiveddate", "");
                }
                com_update.Parameters.Add("@status", Convert.ToString(status_update));
                con.Close();
                con.Open();
                com_update.ExecuteNonQuery();
                Response.Redirect("Teams");
            }
            else if (inserValCon == 3)
            {
                if (txt_TeamID.Value == "")
                {
                    lbl_Error.Visible = true;
                    lbl_Error.Text = "TeamID is required";
                }
                //if (txt_password.Text == "")
                //{
                //    Response.Write("<script>alert('Please enter password!.')</script>");
                //}
                //else
                //{
                con.Close();
                con.Open();
                com_insert.Parameters.Add("@teamname", Convert.ToString(txt_TeamName.Value));
                com_insert.Parameters.Add("@teamid", Convert.ToString(txt_TeamID.Value));
                //com_insert.Parameters.Add("@releiveddate", Convert.ToDateTime(txt_releivedDate.Text));
                Int32 catCheck, projCheck;
                com_check.Connection = con;
                com_check.CommandText = "select Count(TeamID) from tbl_teams where TeamID like '" + txt_TeamID.Value + "'";
                catCheck = Convert.ToInt32(com_check.ExecuteScalar());

                com_check.Connection = con;
                com_check.CommandText = "select TeamID,Teamname from tbl_teams where TeamID like '" + txt_TeamID.Value + "' or Teamname like '" + txt_TeamName.Value + "' and Delteam!=1";
                dr = com_check.ExecuteReader();
                dr.Read();

                if (dr.HasRows == false)
                {
                    //string message = "";
                    //foreach (ListItem item in DropDownCheckBoxes1.Items)
                    //{
                    //    if (item.Selected)
                    //    {
                    //        message += item.Text + ",";
                    //    }
                    //}
                    //String messageb = message.TrimEnd(',');
                    dr.Close();
                    SqlTransaction trans1 = con.BeginTransaction();
                    com_insert.Transaction = trans1;
                    com_insert.Connection = con;
                    if (rbl_Status.SelectedValue != "0")
                    {
                        if (txt_TeamID.Visible == true)
                        {

                            com_insert.CommandText = "insert into tbl_teams(Teamname,TeamID,status,Delteam) values(@teamname,@teamid,1,0)";

                        }
                    }
                    else
                    {
                        if (txt_TeamID.Visible == true)
                        {

                            com_insert.CommandText = "insert into tbl_teams(Teamname,TeamID,status,Delteam) values(@teamname,@teamid,0,0)";
                        }

                    }



                    com_insert.ExecuteNonQuery();
                    //if (catCheck == 0)
                    //{
                    //    com_insert.Connection = con;
                    //    com_insert.CommandText = "insert into category1(Projectid,Projectname,receiveddate)values('" + Projectid.Value + "','" + Projectname.Value + "','" + ReceivedDate.Text + "')";
                    //    com_insert.ExecuteNonQuery();
                    //}
                    trans1.Commit();

                    txt_TeamName.Value = "";
                    txt_TeamID.Value = "";
                    //ViewState["insert"] = "";
                    Response.Redirect("Teams");
                }
                else
                {
                    lbl_Error.Visible = true;
                    lbl_Error.Text = "This TeamID and/or Team Name Already Exist";
                    txt_TeamID.Focus();
                }

                //}//Session["username"] = txt_userName.Value;
            }
        }
        catch (SqlException eee)
        {
            lbl_Error.Visible = true;
            lbl_Error.Text = "Connection Error ! Please try later" + eee.Errors;
        }
        finally
        {
            con.Close();
        }
    }

    /// <summary>
    /// Reset Form fields When Reset Button is Clicked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_Reset_Click(object sender, EventArgs e)
    {
        try
        {
            back(sender, e);
        }
        catch (Exception)
        {

        }
    }

    /// <summary>
    /// Back Button Click Function To call clear
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Btn_Back_Click(object sender, EventArgs e)
    {
        try
        {
            this.block_Register.Visible = false;
            this.block_Grid.Visible = true;
            Clear();
        }
        catch (Exception)
        {

        }
    }

    /// <summary>
    /// Clear All Form Elements
    /// </summary>
    protected void Clear()
    {
        Response.Redirect(Request.Url.AbsoluteUri);
    }

    /// <summary>
    /// Export to Excel Function
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_Export_Click(object sender, EventArgs e)
    {
        try
        {
            SqlDataAdapter da = new SqlDataAdapter("select a.TeamID,a.Teamname[Team Name],(select (select c.username from tbl_usermaster c where c.userid=b.teamleader)[teamleader] from tbl_teamAllotmentMaster b where b.teamid=a.teamid)[TeamLeader],(case when a.Status = 1 then 'Active' else 'In Active' end)[Status] from tbl_teams a where a.Delteam!=1", con);
            DataTable dts = new DataTable();
            da.Fill(dts);
            if (dts.Rows.Count > 0)
            {
				List<string> HeaderNames=new List<string>();
				HeaderNames.Add("Team ID");
				HeaderNames.Add("Team Name");
				HeaderNames.Add("Team Leader");
				HeaderNames.Add("Status");
				List<string> ExcelReport=pcData.generateExcelReport(dts,"TeamsDetails","GenericReports","Teams Details",4,HeaderNames);
				FileInfo file = new FileInfo(ExcelReport[2]);
				Response.Clear();
				Response.AddHeader("Content-Length", file.Length.ToString(CultureInfo.InvariantCulture));
				Response.AddHeader("Content-Disposition", "attachment;filename=\"" + ("TeamsDetails.xls") + "\"");
				Response.ContentType = "application/octet-stream";
				Response.Flush();
				Response.TransmitFile(ExcelReport[0]+("TeamsDetails" + DateTime.Now.ToShortDateString().Replace("/", "") + ".xls"));
				Response.End();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "Script", "alert('" + ex.Message + "')", true);
        }
    }


    /// <summary>
    /// Gridview With Team Details Bind Function
    /// </summary>
    protected void teams_gridbind()
    {
        try
        {
            con.Close();
            SqlDataAdapter da = new SqlDataAdapter("select *,(case when Status = 1 then 'Active' else 'In Active' end)[status1] from tbl_teams where Delteam!=1 order by status desc", con);

            DataSet ds = new DataSet();

            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {

                grd_Teams.DataSource = ds;
                grd_Teams.DataBind();

            }

            else
            {
                ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                grd_Teams.DataSource = ds;
                grd_Teams.DataBind();
                int columncount = grd_Teams.Rows[0].Cells.Count;
                grd_Teams.Rows[0].Cells.Clear();
                grd_Teams.Rows[0].Cells.Add(new TableCell());
                grd_Teams.Rows[0].Cells[0].ColumnSpan = columncount;
                grd_Teams.Rows[0].Cells[0].Text = "<center>----- No Records Found -----</center>";
            }
        }
        catch (Exception)
        {
        }
    }

    /// <summary>
    /// Teams Gridview Hold/Closed Function 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grd_Teams_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                int id = (int)grd_Teams.DataKeys[e.RowIndex].Value;
                con.Open();
                com_checkStatus.Connection = con;
                com_checkStatus.CommandText = "select status from tbl_teams where ID='" + id + "'";
                SqlDataReader reader = com_checkStatus.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    var_Status = reader["status"].ToString();
                }
                con.Close();
                con.Open();
                if (var_Status != "False")
                {
                    com_scope.Connection = con;
                    com_scope.CommandText = "update tbl_teams set status=0 where ID='" + id + "'";
                    com_scope.ExecuteNonQuery();
                    teams_gridbind();
                }
                else
                {
                    com_scope.Connection = con;
                    com_scope.CommandText = "update tbl_teams set Delteam=1 where ID='" + id + "'";
                    com_scope.ExecuteNonQuery();
                    teams_gridbind();
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
    /// Currently Not in Use
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grd_Teams_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        //try
        //{
        //    int id = Convert.ToInt32(grd_Teams.DataKeys[e.RowIndex].Value);
        //    GridViewRow row = (GridViewRow)grd_Teams.Rows[e.RowIndex];
        //    Label lblID = (Label)grd_Teams.Rows[e.RowIndex].FindControl("slno");
        //    TextBox Scope = (TextBox)grd_Teams.Rows[e.RowIndex].FindControl("txt_Scope");
        //    TextBox Description = (TextBox)grd_Teams.Rows[e.RowIndex].FindControl("txt_Description");
        //    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString());
        //    con.Open();
        //    SqlCommand cmd = new SqlCommand("update tbl_scope set scope='" + Scope.Text + "',Description='" + Description.Text + "' where ID='" + id + "'", con);
        //    cmd.ExecuteNonQuery();
        //    con.Close();
        //    grd_Scope.EditIndex = -1;
        //    project_gridbind();
        //}
        //catch (Exception)
        //{
        //}
    }

    /// <summary>
    /// Currently not In use
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grd_Teams_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        //try
        //{
        //    grd_Teams.EditIndex = -1;
        //    project_gridbind();
        //}
        //catch (Exception)
        //{

        //    //throw;
        //}
    }
    //protected void OnPaging(object sender, GridViewPageEventArgs e)
    //{

    //}

    /// <summary>
    /// Add New Team
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_NewTeams_Click(object sender, System.EventArgs e)
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
    /// Update Existing team
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grd_Teams_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_TeamID.Attributes.Add("readonly", "readonly");

            GridViewRow row = grd_Teams.SelectedRow;
            hdn_ID.Value = ((HiddenField)row.FindControl("hdn_ID")).Value;
            hdn_TeamID.Value = ((Label)row.FindControl("lbl_TeamID")).Text;
            txt_TeamID.Value = ((Label)row.FindControl("lbl_TeamID")).Text;
            txt_TeamName.Value = ((Label)row.FindControl("lbl_TeamName")).Text;
            string Status = ((HiddenField)row.FindControl("hdn_Status")).Value;
            //rbl_Status.SelectedValue = ((HiddenField)row.FindControl("hdn_Status")).Value;
            //txt_releivedDate.Text = ((HiddenField)row.FindControl("hdn_releiveddate")).Value;
            if (Status == "True")
            {
                rbl_Status.SelectedValue = "1";
            }
            else
            {
                rbl_Status.SelectedValue = "0";
            }
            inserVal = 2;
            this.block_Grid.Visible = false;
            this.block_Register.Visible = true;
            ViewState["update"] = inserVal;
            btn_Save.Text = "Update";
        }
        catch (Exception)
        {
            Response.Redirect(Request.Url.AbsoluteUri);
        }

    }

    /// <summary>
    /// Currently Not in Use
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grd_Teams.PageIndex = e.NewPageIndex;
            this.teams_gridbind();
        }
        catch (Exception)
        {

        }
    }

    /// <summary>
    /// View Selected Team Details
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void SelectCurrentData(object sender, EventArgs e)
    {
        this.block_Grid.Visible = false;
        this.block_Register.Visible = false;
        this.block_View.Visible = true;
        LinkButton btn = (LinkButton)(sender);
        string teamid = btn.CommandArgument;
        con.Close();
        con.Open();
        com_team.Connection = con;
        com_team.CommandText = "select *,(case when Status = 1 then 'Active' else 'In Active' end)[status1] from tbl_teams where teamid='" + teamid + "'";
        SqlDataReader dr = com_team.ExecuteReader();
        while (dr.Read())
        {
            TitleOfPage.InnerText = dr["teamname"].ToString();
            lbl_TeamIDTextView.InnerText = dr["teamid"].ToString();
            lbl_TeamNameTextView.InnerText = dr["teamname"].ToString();
            lbl_StatusTextView.InnerText = dr["status1"].ToString();
        }
        dr.Close();
        com_team.CommandText = "select (select b.userid+' - '+b.username from tbl_usermaster b where b.userid=teamleader)[TeamleaderName],Username from tbl_teamAllotmentMaster where teamid='" + teamid + "'";
        SqlDataReader dr2 = com_team.ExecuteReader();
        if (dr2.HasRows)
        {
            while (dr2.Read())
            {
                lbl_TeamLeaderView.Visible = true;
                lbl_TeamMembersTextView.Visible = true;
                lbl_TeamLeaderTextView.Visible = true;
                lbl_TeamMembersTextView.Visible = true;
                lbl_TeamLeaderTextView.InnerText = dr2["TeamleaderName"].ToString();
                lbl_TeamMembersTextView.InnerText = dr2["Username"].ToString();
            }
        }
        else
        {
            lbl_TeamLeaderTextView.InnerText = "Not Alloted";
            lbl_TeamMembersTextView.InnerText = "Not Alloted";
        }
    }


    /// <summary>
    /// Redirect Teams When back Button Clicks
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_BackButtonClick(object sender, EventArgs e)
    {
        this.block_Grid.Visible = true;
        this.block_View.Visible = false;
        this.block_Register.Visible = false;
        this.TitleOfPage.InnerText = "Teams";
        Response.Redirect(Request.Url.AbsoluteUri);
    }

}
