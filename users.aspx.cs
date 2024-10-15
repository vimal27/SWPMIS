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
using System.Security.Cryptography;

public partial class Users : System.Web.UI.Page
{
    /// <summary>
    /// Declarations Part For Variables,Strings,SqlConnections,etc
    /// </summary>
    string StatusSearch;
    string description;
    string var_Status = string.Empty;
    string status_update;
    StringBuilder htmlTable = new StringBuilder();
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString());
    SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DCSconn"].ToString());
    SqlCommand com_insert = new SqlCommand();
    SqlCommand com_update = new SqlCommand();
    SqlCommand com_check = new SqlCommand();
    SqlCommand com_checkStatus = new SqlCommand();
    SqlCommand com_userView = new SqlCommand();
    SqlCommand com_scope2 = new SqlCommand();
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
            if (Request.UrlReferrer == null)
            {
                Response.Redirect("Login");
            }
            else
            {
                calext_JoinDate.EndDate = DateTime.Now;
                if (Session["Userid"] != "admin")
                {

                }
                txt_JoinDate.Attributes.Add("readonly", "readonly");
                users_gridbind();
                txt_UserID.Focus();
                if (Session["Userid"] == null) Response.Redirect("Login");
                else if (Session["Userid"].ToString() == "") Response.Redirect("Login");
                if (!IsPostBack)
                {
                    users_gridbind();
                    btn_Save.Attributes.Add("onClientClick", "return validate()");
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

        }
        catch (Exception)
        {

        }
    }

    /// <summary>
    /// Encrypt Password after creating User with Encryption Key -> $321SeCiVrEsLaTiGiDZiPaL
    /// </summary>
    /// <param name="clearText"></param>
    /// <returns></returns>
    private string Encrypt(string clearText)
    {
        string EncryptionKey = "$321SeCiVrEsLaTiGiDZiPaL";
        byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                clearText = Convert.ToBase64String(ms.ToArray());
            }
        }
        return clearText;
    }

    /// <summary>
    /// Decrypt Password If Need Updations of data
    /// </summary>
    /// <param name="cipherText"></param>
    /// <returns></returns>
    private string Decrypt(string cipherText)
    {
        string EncryptionKey = "$321SeCiVrEsLaTiGiDZiPaL";
        byte[] cipherBytes = Convert.FromBase64String(cipherText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipherBytes, 0, cipherBytes.Length);
                    cs.Close();
                }
                cipherText = Encoding.Unicode.GetString(ms.ToArray());
            }
        }
        return cipherText;
    }

    /// <summary>
    /// Clear form Fields If Clicking Back button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void back(object sender, EventArgs e)
    {
        try
        {
            txt_UserID.Value = "";
            txt_UserName.Value = "";
            txt_Password.Value = "";
            txt_JoinDate.Text = "";
            rbl_Status.SelectedIndex = 0;
            ddl_UserRole.SelectedIndex = 0;
        }
        catch (Exception)
        {

        }
    }

    /// <summary>
    /// Add New User or Update existing User
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_Save_Click(object sender, EventArgs e)
    {

        try
        {
            int inserValCon = Convert.ToInt32(ViewState["Insert"]);
            btn_Save.Attributes.Add("onclick", "return validate()");
            if (inserValCon != 3)
            {
                con.Close();
                con.Open();
                com_update.Connection = con;
                com_update.CommandText = "update tbl_usermaster set  rights=@rights,status=@status where sno = '" + hdn_Sno.Value + "'";
                //com_update.Parameters.Add("@username", Convert.ToString(txt_UserName.Value));
                //com_update.Parameters.Add("@userid", Convert.ToString(txt_UserID.Value));
                //if (txt_JoinDate.Text != "")
                //{
                //    DateTime date = DateTime.ParseExact(txt_JoinDate.Text, "dd/MM/yyyy", null);
                //    com_update.Parameters.Add("@joindate", date);
                //}
                //else
                //{
                //    txt_JoinDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                //    com_update.Parameters.Add("@joindate", DateTime.ParseExact(txt_JoinDate.Text, "dd/MM/yyyy", null));
                //}
                //com_update.Parameters.Add("@password", Encrypt(Convert.ToString(txt_Password.Value)));

                com_update.Parameters.Add("@status", rbl_Status.SelectedValue);
                com_update.Parameters.Add("@rights", ddl_UserRole.SelectedValue);
                con.Close();
                con.Open();
                com_update.ExecuteNonQuery();
                Response.Redirect("Users");
            }
            else if (inserValCon == 3)
            {
                calext_JoinDate.Enabled = true;

                //if (txt_Password.Text == "")
                //{
                //    Response.Write("<script>alert('Please enter password!.')</script>");
                //}
                //else
                //{
                con.Close();
                con.Open();
                com_insert.Parameters.Add("@username", Convert.ToString(txt_UserName.Value));
                com_insert.Parameters.Add("@userid", Convert.ToString(txt_UserID.Value));
                if (txt_JoinDate.Text != "")
                {
                    DateTime date = DateTime.ParseExact(txt_JoinDate.Text, "dd/MM/yyyy", null);
                    com_insert.Parameters.AddWithValue("@joindate", date);
                }
                else
                {
                    txt_JoinDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    com_insert.Parameters.AddWithValue("@joindate", DateTime.ParseExact(txt_JoinDate.Text, "dd/MM/yyyy", null));
                }
                com_insert.Parameters.Add("@password", Encrypt(Convert.ToString(txt_Password.Value.ToLower())));
                com_insert.Parameters.AddWithValue("@status", rbl_Status.SelectedValue);
                com_insert.Parameters.AddWithValue("@rights", ddl_UserRole.SelectedValue);
                Int32 catCheck, projCheck;
                com_check.Connection = con;
                com_check.CommandText = "select Count(userid) from tbl_usermaster where userid like '" + txt_UserID.Value + "'";
                catCheck = Convert.ToInt32(com_check.ExecuteScalar());

                com_check.Connection = con;
                com_check.CommandText = "select userid from tbl_usermaster where userid like '" + txt_UserID.Value + "' and Deluser!=1";
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

                    com_insert.CommandText = "insert into tbl_usermaster(username,userid,joindate,rights,password,status,Deluser) values(@username,@userid,@joindate,@rights,@password,@status,0)";




                    com_insert.ExecuteNonQuery();
                    //if (catCheck == 0)
                    //{
                    //    com_insert.Connection = con;
                    //    com_insert.CommandText = "insert into category1(Projectid,Projectname,receiveddate)values('" + Projectid.Value + "','" + Projectname.Value + "','" + ReceivedDate.Text + "')";
                    //    com_insert.ExecuteNonQuery();
                    //}
                    trans1.Commit();

                    txt_UserName.Value = "";
                    txt_UserID.Value = "";
                    txt_Password.Value = "";

                    //ViewState["insert"] = "";
                    Response.Redirect("Users");
                }
                else
                {
                    lbl_Error.Visible = true;
                    lbl_Error.Text = "User ID already exists";
                    txt_UserID.Focus();
                }

                //}//Session["username"] = txt_UserName.Value;
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
    /// Clear Form Fields Call back function
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
    /// On Back Button Click Call Clear method
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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

    /// <summary>
    /// clear method to clear form fields
    /// </summary>
    protected void clear()
    {
        Response.Redirect(Request.Url.AbsoluteUri);

    }

    /// <summary>
    /// Export to excel Function
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_Export_Click(object sender, EventArgs e)
    {
        try
        {
            SqlDataAdapter da = new SqlDataAdapter("select userid[User ID],username[User Name],rights[Role],(case when Status = 1 then 'Active' else 'In Active' end)[Status]  from tbl_usermaster where Deluser!=1", con);
            DataTable dts = new DataTable();
            da.Fill(dts);
            if (dts.Rows.Count > 0)
            {
				List<string> HeaderNames=new List<string>();
				HeaderNames.Add("User ID");
				HeaderNames.Add("User Name");
				HeaderNames.Add("Role");
				HeaderNames.Add("Status");
				List<string> ExcelReport=pcData.generateExcelReport(dts,"UsersDetails","GenericReports","Users Details",4,HeaderNames);
				FileInfo file = new FileInfo(ExcelReport[2]);
				Response.Clear();
				Response.AddHeader("Content-Length", file.Length.ToString(CultureInfo.InvariantCulture));
				Response.AddHeader("Content-Disposition", "attachment;filename=\"" + ("UsersDetails.xls") + "\"");
				Response.ContentType = "application/octet-stream";
				Response.Flush();
				Response.TransmitFile(ExcelReport[0]+("UsersDetails" + DateTime.Now.ToShortDateString().Replace("/", "") + ".xls"));
				Response.End();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "Script", "alert('" + ex.Message + "')", true);
        }
    }


    /// <summary>
    /// Gridview Bind for Users details
    /// </summary>
    protected void users_gridbind()
    {
        try
        {
            con.Close();
            SqlDataAdapter da = new SqlDataAdapter("select CONVERT(VARCHAR(20),joindate,103)[joindate],CONVERT(VARCHAR(20),releiveddate,103)[releiveddate],*,(case when Status = 1 then 'Active' else 'In Active' end)[status1] from tbl_usermaster where Deluser!=1 order by status desc", con);
            DataSet ds = new DataSet();

            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                grd_Users.DataSource = ds;
                grd_Users.DataBind();

            }

            else
            {
                ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                grd_Users.DataSource = ds;
                grd_Users.DataBind();
                int columncount = grd_Users.Rows[0].Cells.Count;
                grd_Users.Rows[0].Cells.Clear();
                grd_Users.Rows[0].Cells.Add(new TableCell());
                grd_Users.Rows[0].Cells[0].ColumnSpan = columncount;
                grd_Users.Rows[0].Cells[0].Text = "<center>----- No Records Found -----</center>";
            }
        }
        catch (Exception)
        {
        }
    }


    /// <summary>
    /// Users Inactive/Delete Function
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grd_Users_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                int id = (int)grd_Users.DataKeys[e.RowIndex].Value;
                con.Open();
                com_checkStatus.Connection = con;
                com_checkStatus.CommandText = "select status from tbl_usermaster where sno='" + id + "'";
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
                    com_scope.CommandText = "update tbl_usermaster set status=0 where sno='" + id + "'";
                    com_scope.ExecuteNonQuery();
                    users_gridbind();
                }
                else
                {
                    com_scope.Connection = con;
                    com_scope.CommandText = "update tbl_usermaster set Deluser=1 where sno='" + id + "'";
                    com_scope.ExecuteNonQuery();
                    users_gridbind();
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
    protected void grd_Users_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

    }

    protected void grd_Users_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {

    }

    /// <summary>
    /// To Redirect Create New User Form
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_NewUsers_Click(object sender, System.EventArgs e)
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
    /// Update Users
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grd_Users_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_UserID.Attributes.Add("readonly", "readonly");
            txt_UserName.Attributes.Add("readonly", "readonly");
            calext_JoinDate.Enabled = false;
            txt_UserID.Attributes.Add("readonly", "readonly");
            txt_Password.Visible = false;
            lbl_Password.Visible = false;
            req_Password.Visible = false;

            GridViewRow row = grd_Users.SelectedRow;
            hdn_Sno.Value = ((HiddenField)row.FindControl("hdn_Sno")).Value;
            hdn_UserID.Value = ((Label)row.FindControl("lbl_UserID")).Text;
            txt_UserID.Value = ((Label)row.FindControl("lbl_UserID")).Text;
            txt_UserName.Value = ((Label)row.FindControl("lbl_UserName")).Text;
            ddl_UserRole.SelectedValue = ((HiddenField)row.FindControl("hdn_Rights")).Value;
            //ReceivedDate.Text = ((Label)row.FindControl("lbl_Receiveddate")).Text;
            string status = ((HiddenField)row.FindControl("hdn_Status")).Value;
            if (status == "True")
            {
                rbl_Status.SelectedValue = "1";
            }
            else
            {
                rbl_Status.SelectedValue = "0";
            }
            txt_JoinDate.Text = ((HiddenField)row.FindControl("hdn_JoinDate")).Value;





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
    /// View Users Details
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void SelectCurrentData(object sender, EventArgs e)
    {
        this.block_Grid.Visible = false;
        this.block_Register.Visible = false;
        this.block_View.Visible = true;
        LinkButton btn = (LinkButton)(sender);
        string userid = btn.CommandArgument;
        con.Close();
        con.Open();
        com_userView.Connection = con;
        com_userView.CommandText = "select userid[User ID],rights,username[User Name],(case when Status = 1 then 'Active' else 'In Active' end)[status1] from tbl_usermaster where userid='" + userid + "'";
        SqlDataReader dr = com_userView.ExecuteReader();
        while (dr.Read())
        {
            string usernametitle = dr["User ID"].ToString() + " - " + dr["User Name"].ToString();
            TitleOfPage.InnerText = usernametitle;
            lbl_UserIDTextView.InnerText = dr["User ID"].ToString();
            lbl_UserNameTextView.InnerText = dr["User Name"].ToString();
            lbl_RoleTextView.InnerText = dr["rights"].ToString();
            lbl_StatusTextView.InnerText = dr["status1"].ToString();
        }

    }

    /// <summary>
    /// Currently not in Use
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grd_Users.PageIndex = e.NewPageIndex;
            this.users_gridbind();
        }
        catch (Exception)
        {

        }
    }



    /// <summary>
    /// back button Click to clear form fields
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_BackButtonClick(object sender, EventArgs e)
    {
        this.block_Grid.Visible = true;
        this.block_View.Visible = false;
        this.block_Register.Visible = false;
        this.TitleOfPage.InnerText = "Users";
    }

    /// <summary>
    /// Currently Not in Use
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_Search_Click(object sender, EventArgs e)
    {
        users_gridbind();
    }
}
