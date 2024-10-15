//Required namespaces
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

public partial class Login : System.Web.UI.Page
{
    /// <summary>
    /// Declarations Part For Variables,Strings,SqlConnections,etc
    /// </summary>
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString());
    SqlCommand com = new SqlCommand();
    string var_Username;
    string var_Password;
    string var_Rights;
    SqlDataReader dr;
    clsDataControl objData = new clsDataControl();
    /// <summary>
    /// Page Load Function
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        txt_UserName.Focus();
        Response.Buffer = true;
        Response.Expires = 0;
        Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
        Response.CacheControl = "no-cache";
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now);
        FormsAuthentication.SignOut();
        if (!string.IsNullOrEmpty(Convert.ToString(HttpContext.Current.Request.QueryString["empid"])) && !string.IsNullOrEmpty(Convert.ToString(HttpContext.Current.Request.QueryString["loginid"])))
        {
            txt_UserName.Value = ASCIIEncoding.ASCII.GetString(Convert.FromBase64String(Request.QueryString["empid"]));
            Btn_Login_Click(sender, e);
        }
        else
        {
            //Storing Login Details On Sessions
            Session["UserName"] = "";
            Session["Rights"] = "";
            Session["ProjectID"] = "";
            Session["DropGrade"] = "";
            Session["DropStage"] = "";
            Session["dropchapter"] = "";
            Session["DropActive"] = "";
            Session["Ncpage"] = "false";
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
    /// Login Details Check
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Btn_Login_Click(object sender, EventArgs e)
    {
        try
        {
            if (txt_UserName.Value == "")
            {
                lbl_Error.Visible = true;
                lbl_Error.Text = "UserID is required";

            }
            else
                if (txt_Password.Value == "")
                {
                    lbl_Error.Visible = true;
                    lbl_Error.Text = "Password is required";
                }
            if ((txt_UserName.Value != ""))
            {
                string ifAdminExists = objData.GetSingleData("select userid from tbl_usermaster where userid in('admin','Admin','ADMIN','administrator','Administrator','ADMINISTRATOR')");
                if (ifAdminExists == "0")
                {
                    objData.InsertOrUpdateData("insert into tbl_usermaster(userid,username,password,rights,desi,Deluser,status,joindate,EmpID) values('admin','Administrator','" + Encrypt("pass123$") + "','Administrator','Administrator','0','1',GETDATE(),'admin')");
                }
                Session["Userid"] = "";
                Session["UserName"] = "";
                Session["master"] = "";
                Session["caller"] = "";
                Session["admin"] = "";
                con.Close();
                con.Open();
                com.Connection = con;
                com.CommandText = "select userid,username,password,rights from tbl_usermaster where UserID = '" + txt_UserName.Value + "' and password='" + Encrypt(txt_Password.Value) + "'";
                dr = com.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {

                        var_Username = (Convert.ToString(dr["userid"]));
                        var_Password = Decrypt((Convert.ToString(dr["password"]))).ToLower();
                        var_Rights = (Convert.ToString(dr["rights"]));
                        Session["Userid"] = Convert.ToString(dr["userid"]);
                        //Session["Teamid"] = Convert.ToString(dr[""]);
                        Session["UserName"] = Convert.ToString(dr["username"]);
                        Session["Rights"] = Convert.ToString(dr["rights"]);
                        Response.Cookies.Clear();
                        //                        if (var_Username.ToLower() == txt_UserName.Value.ToLower() && var_Password == txt_Password.Value.ToLower())
                        if (var_Username.ToLower() == txt_UserName.Value.ToLower())
                        {
                            if (Session["Rights"].ToString().Trim() == "Project Manager")
                            {
                                Session["sessiontype"] = "PM";
                                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, "1", DateTime.Now, DateTime.Now.AddMinutes(10), false, "1");
                                string sMyCookie = FormsAuthentication.Encrypt(ticket);
                                HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, sMyCookie);
                                Response.Cookies.Add(cookie);
                                txt_UserName.Value = string.Empty;
                                txt_Password.Value = string.Empty;
                                Response.Redirect("Dashboard");
                            }

                            else if (Session["Rights"].ToString().Trim() == "Administrator")
                            {
                                Session["sessiontype"] = "Admin";
                                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, "1", DateTime.Now, DateTime.Now.AddMinutes(10), false, "1");
                                string sMyCookie = FormsAuthentication.Encrypt(ticket);
                                HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, sMyCookie);
                                Response.Cookies.Add(cookie);
                                txt_UserName.Value = string.Empty;
                                txt_Password.Value = string.Empty;
                                Response.Redirect("Dashboard");
                                Response.Redirect("ErrorReport");
                            }
                            else if (Session["Rights"].ToString().Trim() == "Team Leader")
                            {
                                Session["sessiontype"] = "Leader";
                                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, "2", DateTime.Now, DateTime.Now.AddMinutes(10), false, "2");
                                string sMyCookie = FormsAuthentication.Encrypt(ticket);
                                HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, sMyCookie);
                                Response.Cookies.Add(cookie);
                                txt_UserName.Value = string.Empty;
                                txt_Password.Value = string.Empty;
                                Response.Redirect("Dashboard");
                                Response.Redirect("ErrorReport");
                            }
                            else if (Session["Rights"].ToString().Trim() == "Bay Leader")
                            {
                                Session["sessiontype"] = "BL";
                                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, "2", DateTime.Now, DateTime.Now.AddMinutes(10), false, "2");
                                string sMyCookie = FormsAuthentication.Encrypt(ticket);
                                HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, sMyCookie);
                                Response.Cookies.Add(cookie);
                                txt_UserName.Value = string.Empty;
                                txt_Password.Value = string.Empty;
                                Response.Redirect("Dashboard");
                                Response.Redirect("ErrorReport");
                            }
                            else
                            {
                                Session["sessiontype"] = "Member";
                                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, "2", DateTime.Now, DateTime.Now.AddMinutes(10), false, "2");
                                string sMyCookie = FormsAuthentication.Encrypt(ticket);
                                HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, sMyCookie);
                                Response.Cookies.Add(cookie);
                                txt_UserName.Value = string.Empty;
                                txt_Password.Value = string.Empty;
                                Response.Redirect("Dashboard");
                                Response.Redirect("ErrorReport");
                            }
                        }
                        else
                        {
                            //ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Password is Incorrect');", true);
                            lbl_Error.Visible = true;
                            lbl_Error.Text = "Password is Incorrect";
                            txt_Password.Focus();
                        }
                    }
                }
                else
                {
                    //ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('UserID is Incorrect');", true);
                    lbl_Error.Visible = true;
                    lbl_Error.Text = "UserID or Password is Incorrect";
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "swal(" + ex + ");", true);
        }
    }
}