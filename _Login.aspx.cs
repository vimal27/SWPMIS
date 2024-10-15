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
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString());
    SqlCommand com = new SqlCommand();
    string var_Username;
    string var_Password;
    string var_Rights;
    SqlDataReader dr;
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
    private string Encrypt(string clearText)
    {
        //Encryption For Login Password
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
    private string Decrypt(string cipherText)
    {
        //Decryption For Login Password
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


    protected void Btn_Login_Click(object sender, EventArgs e)
    {
        try
        {
            //Login Details Check
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
            if ((txt_UserName.Value != "") && (txt_Password.Value != ""))
            {
                Session["Userid"] = "";
                Session["UserName"] = "";
                Session["master"] = "";
                Session["caller"] = "";
                Session["admin"] = "";
                con.Close();
                con.Open();
                com.Connection = con;
                com.CommandText = "select userid,username,password,rights from tbl_usermaster where UserID = '" + txt_UserName.Value + "'";
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

                        if (var_Username.ToLower() == txt_UserName.Value.ToLower() && var_Password == txt_Password.Value.ToLower())
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
                            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Password is Incorrect');", true);
                            txt_Password.Focus();
                        }
                    }

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('UserID is Incorrect');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert("+ex+");", true);
        }
    }
}