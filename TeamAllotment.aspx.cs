//Requried Namespaces
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

public partial class TeamAllotment : System.Web.UI.Page
{
    /// <summary>
    /// Declarations Part For Variables,Strings,SqlConnections,etc
    /// </summary>
    string[] TeamsSplitted;
    string TeamsAlloted;
    string JoinedWords = string.Empty;
    string Teams = string.Empty;
    string userId = String.Empty;
    string userName = String.Empty;
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString());
    SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DCSconn"].ToString());
    SqlCommand com = new SqlCommand();

    /// <summary>
    /// Page Load Function
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.UrlReferrer == null)
        {
            Response.Redirect("Login");
        }
        ddl_TeamName.Focus();
        if (Session["Userid"] == null) Response.Redirect("Login");
        else if (Session["Userid"].ToString() == "") Response.Redirect("Login");
        if (!Page.IsPostBack)
        {
            ViewState["userid"] = Session["userid"];
            btn_Add.Enabled = true;
            bindTeam();
        }
    }

    /// <summary>
    /// Bind Team members based on Selected Team
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddl_TeamName_SelectedIndexChanged(object sender, EventArgs e)
    {
        btn_Add.Enabled = true;
        block_SelectMembers.Visible = true;
        block_SelectedMembers.Visible = false;
        chk_TeamMembers.Items.Clear();
        try
        {
            com.Connection = con;
            com.CommandText = "select userid,username from tbl_usermaster where status=1";
            con.Close();
            con.Open();
            SqlDataReader dr2 = this.com.ExecuteReader();
            if (dr2.HasRows)
            {
                while (dr2.Read())
                {
                    ListItem item = new ListItem();
                    item.Text = dr2["username"].ToString();
                    item.Value = dr2["userid"].ToString();
                    chk_TeamMembers.Items.Add(item);
                }
                dr2.Close();
                con.Close();
            }
            chk_SelectAll.Visible = true;
            com.Connection = con;
            com.CommandText = "select UserID,Username from tbl_teamAllotmentMaster where TeamID='" + ddl_TeamName.SelectedValue + "'";
            con.Open();
            SqlDataReader dr = this.com.ExecuteReader();

            if (dr.HasRows)
            {
                btn_Add.Text = "Update";
                while (dr.Read())
                {
                    userId = dr["UserID"].ToString();
                    userName = dr["Username"].ToString();
                    string[] splitteduserId = userId.Split(',');
                    for (int i = 0; i < splitteduserId.Length; i++)
                    {
                        chk_TeamMembers.Items.FindByValue(splitteduserId[i]).Selected = true;
                    }
                }

                dr.Close();


            }

            else
            {
                btn_Add.Text = "Add";
                dr.Close();
                chk_TeamMembers.Items.Clear();
                com.CommandText = "select userid,username from tbl_usermaster where status=1";
                SqlDataReader dr3 = this.com.ExecuteReader();

                if (dr3.HasRows)
                {
                    while (dr3.Read())
                    {
                        ListItem item = new ListItem();
                        item.Text = dr3["username"].ToString();
                        item.Value = dr3["userid"].ToString();
                        chk_TeamMembers.Items.Add(item);


                    }
                    con.Close();
                }
            }
            com.Connection = con;
            com.CommandText = "select teamleader from tbl_teamAllotmentMaster where TeamID='" + ddl_TeamName.SelectedValue + "'";
            con.Close();
            con.Open();
            SqlDataReader dr8 = this.com.ExecuteReader();

            if (dr8.HasRows)
            {
                btn_Add.Text = "Update";
                while (dr8.Read())
                {
                    ddl_TeamLeader.SelectedValue = dr8["teamleader"].ToString();
                }
            }

        }
        catch (Exception)
        {

        }
        bindteamleader();
    }

    /// <summary>
    /// Bind Teams
    /// </summary>
    protected void bindTeam()
    {
        try
        {
            con.Open();
            com.Connection = con;
            string Rights = Convert.ToString(Session["Rights"]);
            com.CommandText = "Select TeamID,TeamID +'-'+ Teamname[Teamname1],Teamname from tbl_teams where status=1 and delTeam!=1";
            if (Rights == "Team Leader")
            {
                com.CommandText = "Select TeamID,TeamID +'-'+ Teamname[Teamname1],Teamname from tbl_teams where status=1 and delTeam!=1 and TeamID in (select TeamID from tbl_teamAllotmentMaster where teamleader like('%"+Convert.ToString(ViewState["userid"])+"%'))";
                ddl_TeamName.DataSource = com.ExecuteReader();
                ddl_TeamName.Items.Clear();
                ddl_TeamName.DataTextField = "Teamname";
                ddl_TeamName.DataValueField = "TeamID";
                ddl_TeamName.DataBind();
                ddl_TeamName.Items.Insert(0, new ListItem("Select", "NA"));
                bindteamleader();
                ddl_TeamName_SelectedIndexChanged(this, EventArgs.Empty);
            }
            else
            {
                ddl_TeamName.DataSource = com.ExecuteReader();
                ddl_TeamName.Items.Clear();
                ddl_TeamName.DataTextField = "Teamname";
                ddl_TeamName.DataValueField = "TeamID";
                ddl_TeamName.DataBind();
                ddl_TeamName.Items.Insert(0, new ListItem("Select", "NA"));
                con.Close();
                Page.Form.Attributes.Add("enctype", "multipart/form-data");
            }
        }
        catch (Exception)
        {
        }
    }

    /// <summary>
    /// Bind Teamleader Based On Selected Team
    /// </summary>
    protected void bindteamleader()
    {
        try
        {
            con.Close();
            con.Open();
            com.Connection = con;
            string Rights = Convert.ToString(Session["Rights"]);
            com.CommandText = "Select userid,userid+' - '+username as UserName from tbl_usermaster where status=1 and rights='Team Leader'";
            if (Rights == "Team Leader")
            {
                com.CommandText = "Select userid,userid+' - '+username as UserName from tbl_usermaster where userid='"+Session["Userid"]+"'";
                ddl_TeamLeader.DataSource = com.ExecuteReader();
                ddl_TeamLeader.Items.Clear();
                ddl_TeamLeader.DataTextField = "UserName";
                ddl_TeamLeader.DataValueField = "userid";
                ddl_TeamLeader.DataBind();
                ddl_TeamLeader.Items.Insert(0, new ListItem("Select", "NA"));
            }
            else
            {
                ddl_TeamLeader.DataSource = com.ExecuteReader();
                ddl_TeamLeader.Items.Clear();
                ddl_TeamLeader.DataTextField = "UserName";
                ddl_TeamLeader.DataValueField = "userid";
                ddl_TeamLeader.DataBind();
                ddl_TeamLeader.Items.Insert(0, new ListItem("Select", "NA"));
                con.Close();
                Page.Form.Attributes.Add("enctype", "multipart/form-data");
            }
        }
        catch (Exception)
        {
        }
    }

    /// <summary>
    /// Insert Or Update Team details
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_Add_Click(object sender, EventArgs e)
    {
        if (btn_Add.Text == "Update")
        {
            con.Close();
            con.Open();
            string checkUserid = string.Empty;
            string checkUsername = string.Empty;
            foreach (ListItem item in chk_TeamMembers.Items)
            {
                if (item.Selected == true)
                {
                    com.Connection = con;
                    con.Close();
                    con.Open();
                    SqlCommand cmd = new SqlCommand("select teamid from tbl_usermaster where userid='" + item.Value + "'", con);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        Teams = reader["teamid"].ToString();
                        TeamsSplitted = Teams.Split(',');
                        foreach (string splitted in TeamsSplitted)
                        {
                            StringBuilder sb_Team = new StringBuilder();
                            sb_Team.Append("\'" + splitted + "\',");
                            JoinedWords = sb_Team.ToString();
                        }
                        JoinedWords = JoinedWords.TrimEnd(',');
                    }
                    reader.Close();
                    com.CommandText = "update tbl_usermaster set teamid= '"+ddl_TeamName.SelectedValue+"' where userid='" + ddl_TeamLeader.SelectedValue + "'";
                    com.ExecuteNonQuery();
                    checkUserid += "," + item.Value;
                    checkUsername += "," + item;
                    SqlCommand cmd2 = new SqlCommand("select teamid from tbl_usermaster where userid='" + item.Value + "'", con);
                    SqlDataReader reader2 = cmd.ExecuteReader();
                    if (reader2.HasRows)
                    {
                        reader2.Read();
                        Teams = reader2["teamid"].ToString();
                        TeamsSplitted = Teams.Split(',');
                        foreach (string splitted in TeamsSplitted)
                        {
                            StringBuilder sb_Team = new StringBuilder();
                            sb_Team.Append("\'" + splitted + "\',");
                            TeamsAlloted = sb_Team.ToString();
                        }
                        TeamsAlloted = TeamsAlloted.TrimEnd(',');
                    }
                    reader.Close();
                    if (TeamsAlloted.Contains(item.Value) == false)
                    {
                    //    checkUserid = checkUserid.Replace("," + item.Value, "");
                    //    checkUsername = checkUsername.Replace("," + item, "");
                    }
                }
            }
            checkUsername = checkUsername.TrimStart(',');
            checkUserid = checkUserid.TrimStart(',');
            com.Connection = con;
            con.Close();
            con.Open();
            com.CommandText = "update tbl_teamAllotmentMaster set UserID='" + checkUserid + "',status=1,UpdatedDate=GETDATE(),Username='" + checkUsername + "',teamleader='" + ddl_TeamLeader.SelectedValue + "' where TeamID='" + ddl_TeamName.SelectedValue + "'";
            StringBuilder sb = new StringBuilder();
            sb.Append("<b>Team Name : </b>" + ddl_TeamName.SelectedItem.Text + "</br>");
            sb.AppendLine();
            sb.Append("<b>Team Leader Name </b>: " + ddl_TeamLeader.SelectedItem.Text + "</br>");
            sb.Append(Environment.NewLine);
            sb.Append("<b>Team Members : </b>" + checkUsername + "</br>");
            string TeamDetails = sb.ToString();
            com.ExecuteNonQuery();
            lbl_SelectedMembers.InnerHtml = TeamDetails;
            com.CommandText = "select teamleader from tbl_teamAllotmentMaster where status=1";
            SqlDataReader dr = this.com.ExecuteReader();
            var leaders = new List<string>();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    leaders.Add(dr["teamleader"].ToString());
                }
            }
            dr.Close();
            foreach (string item in leaders)
            {
                com.CommandText = "update tbl_usermaster set rights='Team Leader' where userid='" + item + "'";
                com.ExecuteNonQuery();
            }
            block_SelectMembers.Visible = false;
            block_SelectedMembers.Visible = true;
            btn_Add.Enabled = false;
        }
        else
        {
            string checkUserid = string.Empty;
            string checkUsername = string.Empty;
            foreach (ListItem item in chk_TeamMembers.Items)
            {
                if (item.Selected == true)
                {
                    com.Connection = con;
                    con.Close();
                    con.Open();
                    SqlCommand cmd = new SqlCommand("select teamid from tbl_usermaster where userid='" + item.Value + "'", con);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        Teams = reader["teamid"].ToString();
                        TeamsSplitted = Teams.Split(',');
                        foreach (string splitted in TeamsSplitted)
                        {
                            StringBuilder sb_Team = new StringBuilder();
                            sb_Team.Append("\'" + splitted + "\',");
                            JoinedWords = sb_Team.ToString();
                        }
                        JoinedWords = JoinedWords.TrimEnd(',');
                    }
                    reader.Close();
                    com.CommandText = "update tbl_usermaster set teamid= '" + ddl_TeamName.SelectedValue + "' where userid='" + ddl_TeamLeader.SelectedValue + "'";
                    com.ExecuteNonQuery();
                    checkUserid += "," + item.Value;
                    checkUsername += "," + item;
                }
            }
            checkUsername = checkUsername.TrimStart(',');
            checkUserid = checkUserid.TrimStart(',');
            com.Connection = con;
            con.Close();
            con.Open();
            com.CommandText = "insert into tbl_teamAllotmentMaster(TeamID,UserID,status,InsertedDate,UpdatedDate,Username,teamleader) values('" + ddl_TeamName.SelectedValue + "','" + checkUserid + "','1',GETDATE() ,'','" + checkUsername + "','" + ddl_TeamLeader.SelectedValue + "')";
            StringBuilder sb = new StringBuilder();
            sb.Append("<b>Team Name : </b>" + ddl_TeamName.SelectedItem.Text + "</br>");
            sb.AppendLine();
            sb.Append("<b>Team Leader Name </b>: " + ddl_TeamLeader.SelectedItem.Text + "</br>");
            sb.Append(Environment.NewLine);
            sb.Append("<b>Team Members : </b>" + checkUsername + "</br>");
            string TeamDetails = sb.ToString();
            lbl_SelectedMembers.InnerHtml = TeamDetails;
            com.ExecuteNonQuery();
            com.CommandText = "select teamleader from tbl_teamAllotmentMaster where status=1";
            SqlDataReader dr = this.com.ExecuteReader();
            var leaders = new List<string>();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    leaders.Add(dr["teamleader"].ToString());
                }
            }
            dr.Close();
            foreach (string item in leaders)
            {
                com.CommandText = "update tbl_usermaster set rights='Team Leader' where userid='" + item + "'";
                com.ExecuteNonQuery();
            }
            block_SelectMembers.Visible = false;
            block_SelectedMembers.Visible = true;
            btn_Add.Enabled = false;
        }
    }

    /// <summary>
    /// Clear Form Fields
    /// </summary>
    protected void Clear()
    {
        ddl_TeamName.SelectedIndex = 0;
        ddl_TeamLeader.SelectedIndex = 0;
        chk_TeamMembers.Items.Clear();
        btn_Add.Text = "Add";
    }

    /// <summary>
    /// Reset Form On Reset button Click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_Reset_Click(object sender, EventArgs e)
    {
        ddl_TeamName.SelectedIndex = 0;
        ddl_TeamLeader.SelectedIndex = 0;
        chk_SelectAll.Visible = false;
        chk_TeamMembers.Items.Clear();
        btn_Add.Text = "Add";
        block_SelectedMembers.Visible = false;
        block_SelectMembers.Visible = true;
        btn_Add.Enabled = true;
    }

    /// <summary>
    /// Select all Button In Checkbox list
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void chk_SelectAllClicked(object sender, EventArgs e)
    {
        if (chk_SelectAll.Checked == true)
        {
            foreach (ListItem item in chk_TeamMembers.Items)
            {
                item.Selected = true;
            }
        }
        else
        {
            foreach (ListItem item in chk_TeamMembers.Items)
            {
                item.Selected = false;
            }
        }
    }

    /// <summary>
    /// Redirect to Dashboard If back button is Clicked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_Back_Click(object sender, EventArgs e)
    {
        Response.Redirect("Dashboard");
    }
}