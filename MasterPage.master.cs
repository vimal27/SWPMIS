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
using System.Security.Cryptography;
using System.IO;
using System.Text;
public partial class MasterPage : System.Web.UI.MasterPage
{
    /// <summary>
    /// Declarations Part For Variables,Strings,SqlConnections,etc
    /// </summary>
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString());
    SqlConnection con2 = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString());
    SqlCommand com = new SqlCommand();
    SqlCommand com_PasswordChange = new SqlCommand();
    clsDataControl objData = new clsDataControl();
    SqlDataReader dr;
    string CurrentPassword;
    string TeamUsers = string.Empty;
    string TeamLeader = string.Empty;
    string CountOfDprNonEntry = string.Empty;

    /// <summary>
    /// Page Load Function
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        clsDataControl objData = new clsDataControl();
        string Rights = Convert.ToString(Session["Rights"]);
        lnk_Milestones.Visible = false;

        //Pages That Are Blocked for users
        if (Rights != "Administrator" && Rights != "Team Leader")
        {
            DCSReport.Visible = false;
            lnk_Milestones.Visible = false;
            MRM_Reports.Visible = false;
            Projects.Visible = false;
            Teams.Visible = false;
            string CurrentUser = Convert.ToString(Session["Userid"]);
            //Place to Show New Tasks and Holded Tasks On Notification
            span_HoldedTasks.InnerText = objData.GetSingleData("select count(*)as count from tbl_taskmaster a inner join tbl_ProjectReq b on b.projectid=a.projectid where a.taskstatus='Hold' and a.userid ='" + CurrentUser + "'");
            span_NewTasks.InnerText = objData.GetSingleData("select count(*)as count from tbl_taskmaster a inner join tbl_ProjectReq b on b.projectid=a.projectid where a.taskstatus='Yet To Start' and a.userid ='" + CurrentUser + "'");
            span_WIPTasks.InnerText = objData.GetSingleData("select count(*)as count from tbl_taskmaster a inner join tbl_ProjectReq b on b.projectid=a.projectid where a.taskstatus='WIP' and a.userid ='" + CurrentUser + "'");
            span_ActiveTasks.InnerText = objData.GetSingleData("select count(*)as count from tbl_taskmaster a inner join tbl_ProjectReq b on b.projectid=a.projectid where a.taskstatus in ('Yet To Start','WIP') and a.userid ='" + CurrentUser + "'");
        }
        //Pages That Are Blocked for TeamLeaders
        if (Rights == "Team Leader")
        {
            submenu_Users.Visible = false;
            submenu_Teams.Visible = false;
            string TeamLeader = Convert.ToString(Session["Userid"]);
            string TeamUsers = objData.GetSingleData("select replace(''''+userid+'''',',',''',''') from tbl_teamAllotmentMaster where teamleader like ('%" + TeamLeader + "%')");
            //Place to Show New Tasks and Holded Tasks On Notification
            span_HoldedTasks.InnerText = objData.GetSingleData("select count(a.id)as count from tbl_taskmaster a inner join tbl_ProjectReq b on b.projectid=a.projectid where a.taskstatus='Hold' and b.allotedteamid in(select TeamID from tbl_teamAllotmentMaster where teamleader like ('%" + Session["Userid"] + "%'))");
            span_NewTasks.InnerText = objData.GetSingleData("select COUNT(*)as count from tbl_taskmaster a inner join tbl_ProjectReq b on b.projectid=a.projectid where a.taskstatus='Yet To Start' and b.allotedteamid in(select TeamID from tbl_teamAllotmentMaster where teamleader like ('%" + Session["Userid"] + "%'))");
            span_WIPTasks.InnerText = objData.GetSingleData("select count(a.id) from tbl_taskmaster a inner join tbl_Projectreq b  on a.projectid=b.projectid where  a.taskstatus='WIP' and b.allotedteamid in(select TeamID from tbl_teamAllotmentMaster where teamleader like ('%" + Session["Userid"] + "%'))");
            span_ActiveTasks.InnerText = objData.GetSingleData("select count(a.id) from tbl_taskmaster a inner join tbl_Projectreq b  on a.projectid=b.projectid where  a.taskstatus in('Yet To Start','WIP') and b.allotedteamid in(select TeamID from tbl_teamAllotmentMaster where teamleader like ('%" + Session["Userid"] + "%'))");
        }
        else if (Rights == "Administrator")
        {
            //Place to Show New Tasks and Holded Tasks On Notification
            span_HoldedTasks.InnerText = objData.GetSingleData("select count(*)as count from tbl_taskmaster a inner join tbl_ProjectReq b on b.projectid=a.projectid where taskstatus='Hold'");
            span_NewTasks.InnerText = objData.GetSingleData("select count(*)as count from tbl_taskmaster a inner join tbl_ProjectReq b on b.projectid=a.projectid where taskstatus='Yet To Start'");
            span_WIPTasks.InnerText = objData.GetSingleData("select count(*)as count from tbl_taskmaster a inner join tbl_ProjectReq b on b.projectid=a.projectid where taskstatus='WIP'");
            span_ActiveTasks.InnerText = objData.GetSingleData("select count(*)as count from tbl_taskmaster a inner join tbl_ProjectReq b on b.projectid=a.projectid where taskstatus in ('WIP','Yet To Start')");
        }
        //Dpr_Non_Entry();
        int TotalActive = Int32.Parse(span_NewTasks.InnerText) + Int32.Parse(span_HoldedTasks.InnerText) + Int32.Parse(span_WIPTasks.InnerText);
        span_TotalActive.InnerText = Convert.ToString(TotalActive);
        lnk_TotalActive.InnerHtml = Convert.ToString("<i class='fa fa-star-half-o'></i>" + TotalActive);
        lbl_UserNameHeader.Text = lblusernamedetails.Text = Convert.ToString(Session["UserName"]);

        if (Session["userid"] == "" || Session["userid"] == null || Session["UserName"] == "")
        {
            Response.Redirect("Login");
        }
        if (ScriptManager.GetCurrent(Page) == null)
        {

            //Page.Form.Controls.AddAt(0, new ScriptManager());
        }
    }
    /// <summary>
    /// Encryption For Login Password with Encrypt key : $321SeCiVrEsLaTiGiDZiPaL
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
    /// Decryption For Login Password
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
    /// Password change Function if Change password button is Clicked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_ChangePassword_Click(object sender, EventArgs e)
    {
        con.Close();
        con.Open();
        com.Connection = con;
        com.CommandText = "select password,userid from tbl_usermaster where UserID='" + Session["Userid"] + "'";
        dr = com.ExecuteReader();
        dr.Read();
        if (dr[1].ToString() == Session["Userid"].ToString())
        {
            CurrentPassword = (Convert.ToString(dr[0]));
            if (CurrentPassword != Encrypt(txt_CurrentPassword.Text))
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "Alert", "alert('Password Incorrect, Please! Try Again..');", true);
            }
            else if (txt_NewPassword.Text != txt_ConfirmPassword.Text)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "Alert", "alert('Password Mismatched, Please! Try Again..');", true);
            }
            else
            {
                com_PasswordChange.Connection = con2;
                con.Close();
                con2.Open();
                com_PasswordChange.CommandText = "update tbl_usermaster set password='" + Encrypt(txt_ConfirmPassword.Text) + "' where UserID='" + Session["Userid"] + "'";
                com_PasswordChange.ExecuteNonQuery();
                ScriptManager.RegisterStartupScript(Page, GetType(), "Alert", "alert('Password Changed Successfully');", true);
                con2.Close();
                Response.Redirect("Login");
            }
        }
        dr.Close();
    }

    /// <summary>
    /// Cancel button clicked on change password popup
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_Cancel_Click(object sender, EventArgs e)
    {
        txt_CurrentPassword.Text = string.Empty;
        txt_NewPassword.Text = string.Empty;
        txt_ConfirmPassword.Text = string.Empty;
    }

    /// <summary>
    /// Logout function if Logout button is clicked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_Logout_Click(object sender, EventArgs e)
    {
        Session.RemoveAll();
        Response.Redirect("Login");
    }

    /// <summary>
    /// Logout function
    /// </summary>
    protected void Logout()
    {
        Session.RemoveAll();
        Response.Redirect("Login");
    }

}
