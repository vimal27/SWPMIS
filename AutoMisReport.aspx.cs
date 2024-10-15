//Required Namespaces
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AutoMisReport : System.Web.UI.Page
{
    /// <summary>
    /// Declaration Part
    /// </summary>
    public string con = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
    /// <summary>
    /// Page Load Function
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
		DateTime baseDate = DateTime.Today;
        string datenow = DateTime.Now.ToString("yyyy/MM/dd");
		var thisMonthStart = baseDate.AddDays(1 - baseDate.Day);
		string StartOfMonth=thisMonthStart.ToString("yyyy/MM/dd");
		ScriptManager.RegisterStartupScript(this, GetType(), "mismatch", "swal('"+StartOfMonth.ToString()+"','','success')", true);
		string sqlstr = @" select ROW_NUMBER() OVER(ORDER BY t.sno) AS Sno,t.Team,t.receiveddate,t.projectname,t.typeproject,t.allotedteamname,t.startdate,t.duedate,replace(replace(convert(varchar(15),convert(date,t.completeddate,105),106),'01 Jan 1900',''),' ','-')[enddate],t.remarks,t.projectstatus,t.ExtendReason from (
                          select ROW_NUMBER() OVER(ORDER BY typeproject) AS Sno,b.Completeddate,b.typeproject Team,replace(Convert(varchar(15),CONVERT(date,b.receiveddate,106),106),' ','-')receiveddate,
                            b.projectname,CASE WHEN b.typeproject = 'External' THEN b.receivedfrom ELSE b.typeproject END AS typeproject,b.allotedteamname,replace(Convert(varchar(15),CONVERT(date,b.receiveddate,106),106),' ','-')startdate,
                            (CASE WHEN ISDATE (b.duedate) = 1 THEN replace(Convert(varchar(15),convert(datetime, cast([b].duedate as varchar(15))),106),' ','-')END)duedate,
                            (CASE WHEN ISDATE (b.completeddate) = 1 THEN convert(date,completeddate,105)END)enddate,
                            stuff((select '<p><b>'+replace(Convert(varchar(15),convert(datetime, cast(pr.date as varchar(15))),106),' ','-') +' </b>: ' + pr.remark+' </p> ' from tbl_ProjectRemarks as pr where pr.project = b.projectid and pr.status=1 order by pr.date asc for xml path(''), type).value('.', 'varchar(max)'), 1, 0, '') as remarks,
                            b.projectstatus,b.extendreason from tbl_ProjectReq b right outer join tbl_ProjectStatusMaster a on a.project = b.projectid right outer JOIN(SELECT MAX(id) as maxID, project FROM tbl_ProjectStatusMaster where dateofchange <= '"+datenow+"' GROUP BY project) as d ON  a.project = d.project and a.id = d.maxID WHERE a.dateofchange <= '"+datenow+"' and projectid!= 'NA' and b.projectstatus not in ('Completed') union  select ROW_NUMBER() OVER(ORDER BY typeproject) AS Sno,b.Completeddate,b.typeproject Team,replace(Convert(varchar(15),CONVERT(date,b.receiveddate,106),106),' ','-')receiveddate, b.projectname,CASE WHEN b.typeproject = 'External' THEN b.receivedfrom ELSE b.typeproject END AS typeproject,b.allotedteamname,replace(Convert(varchar(15),CONVERT(date,b.receiveddate,106),106),' ','-')startdate, (CASE WHEN ISDATE (b.duedate) = 1 THEN replace(Convert(varchar(15),convert(datetime, cast([b].duedate as varchar(15))),106),' ','-')END)duedate, (CASE WHEN ISDATE (b.completeddate) = 1 THEN convert(date,completeddate,105)END)enddate, stuff((select '<p><b>'+replace(Convert(varchar(15),convert(datetime, cast(pr.date as varchar(15))),106),' ','-') +' </b>: ' + pr.remark+' </p> ' from tbl_ProjectRemarks as pr where pr.project = b.projectid and pr.status=1 order by pr.date asc for xml path(''), type).value('.', 'varchar(max)'), 1, 0, '') as remarks, b.projectstatus,b.extendreason from tbl_ProjectReq b right outer join tbl_ProjectStatusMaster a on a.project = b.projectid right outer JOIN(SELECT MAX(id) as maxID, project FROM tbl_ProjectStatusMaster  GROUP BY project) as d ON  a.project = d.project  and a.id = d.maxID WHERE a.dateofchange <= '"+datenow+"' and projectid!= 'NA' and b.projectstatus ='Completed' and  convert(date,isNull(b.completeddate,'1993-01-27'),105) between CONVERT(VARCHAR(25),DATEADD(dd,-(DAY(GETDATE())-1),GETDATE()),101) and GETDATE()) as t where t.projectstatus!='Completed'";
        SqlDataAdapter da = new SqlDataAdapter(sqlstr, con);
        DataTable dt = new DataTable();
        da.Fill(dt);
        generateReport(dt, "Daily Status Report");
    }
    /// <summary>
    /// Generating Excel Report With MIS Report
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="filename"></param>
    void generateReport(DataTable dt, string filename)
    {
        try
        {
            string teamVerify = "";
            int toSpan = 1;
            string filePath = HttpContext.Current.Server.MapPath(".") + "\\Reports\\";
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath + "\\Reports\\");
            string excelpath = filePath + ("StatusReport" + DateTime.Now.ToShortDateString().Replace("/", "") + ".xls");
            if (File.Exists(excelpath))
                File.Delete(excelpath);
            FileInfo file = new FileInfo(excelpath);
            string reportName = "Daily Status Report as on : " + DateTime.Now.ToString("dd/MM/yyyy");
            StringBuilder sb = new StringBuilder();
            sb.Append("<html>");
            sb.Append("<head>");
            sb.Append("<body>");
            sb.Append("<table border='1' style='font-family:Calibri; font-size:14px;'>");
            sb.Append("<tr><td colspan='12' style='text-align:center;vertical-align:middle;font-weight:bold;background-color: yellow;'>" + reportName + "</td></tr>");
            sb.Append("<tr><td style='text-align:center;vertical-align:middle;font-weight:bold'>S.No</td><td style='text-align:center;font-weight:bold'>Team</td><td style='text-align:center;font-weight:bold'>Received Date</td><td style='text-align:center;font-weight:bold'>Project Name</td><td style='text-align:center;font-weight:bold'>Received From</td><td style='text-align:center;font-weight:bold'>Responsible</td><td style='text-align:center;font-weight:bold'>Start Date</td><td style='text-align:center;font-weight:bold'>Due Date</td><td style='text-align:center;font-weight:bold'>End Date</td><td style='text-align:center;font-weight:bold'>Reason For Extend</td><td style='text-align:center;font-weight:bold'>Remarks</td><td style='text-align:center;font-weight:bold'>Current Status</td></tr>");
            for (int i1 = 0; i1 < dt.Rows.Count; i1++)
            {
                string teamCheck = dt.Rows[i1]["Team"].ToString().ToUpper();
                if (teamCheck == teamVerify)
                {
                    toSpan += 1;
                }
                else
                {
                    toSpan = 1;
                }
                teamCheck = dt.Rows[i1]["Team"].ToString().ToUpper();
                sb.Append("<tr>");
                sb.Append("<td style='text-align:center;vertical-align:middle;'>" + dt.Rows[i1]["sno"].ToString().ToUpper() + "</td>");
                sb.Append("<td style='text-align:center;vertical-align:middle;'>" + dt.Rows[i1]["Team"].ToString().ToUpper() + "</td>");
                teamVerify = teamCheck;
                sb.Append("<td style='text-align:center;vertical-align:middle;'>" + dt.Rows[i1]["receiveddate"].ToString() + "</td>");
                sb.Append("<td style='text-align:left;vertical-align:middle;'>" + dt.Rows[i1]["projectname"].ToString().ToUpper() + "</td>");
                sb.Append("<td style='text-align:center;vertical-align:middle;'>" + dt.Rows[i1]["typeproject"].ToString().ToUpper() + "</td>");
                sb.Append("<td style='text-align:center;vertical-align:middle;'>" + dt.Rows[i1]["allotedteamname"].ToString().ToUpper() + "</td>");
                sb.Append("<td style='text-align:center;vertical-align:middle;'>" + dt.Rows[i1]["startdate"].ToString().ToUpper() + "</td>");
                sb.Append("<td style='text-align:center;vertical-align:middle;'>" + dt.Rows[i1]["duedate"].ToString().ToUpper() + "</td>");
                sb.Append("<td style='text-align:center;vertical-align:middle;'>" + dt.Rows[i1]["enddate"].ToString().ToUpper() + "</td>");
                sb.Append("<td style='text-align:left;vertical-align:middle;'>" + dt.Rows[i1]["extendreason"].ToString().ToUpper() + "</td>");
                sb.Append("<td style='text-align:left;vertical-align:middle;'>" + dt.Rows[i1]["remarks"].ToString().ToUpper() + "</td>");
                sb.Append("<td style='text-align:center;vertical-align:middle;'>" + dt.Rows[i1]["projectstatus"].ToString().ToUpper() + "</td>");
                sb.Append("</tr>");
            }
            sb.Append("</table>");
            sb.Append("</body>");
            sb.Append("</head>");
            sb.Append("</html>");
            File.WriteAllText(excelpath, sb.ToString());
         	SendEmail(excelpath);
        }
        catch (Exception e)
        {

        }
    }
    /// <summary>
    /// Automail Function
    /// </summary>
    /// <param name="excelpath"></param>
    public void SendEmail(string excelpath)
    {
        string _Mailstatus = string.Empty;
        string subject = "Team Status Report";
		string Message = @"<table style=\""font-family:calibre;background-color: lightgrey;solid navy;\""><tr><td>Please find the attachment status sheet for today.This is an automated email please do not reply...</td></tr><tr><td>Regards,</td></tr><tr><td>Software Team</td></tr></table>";
        string MailTo = "vimal.bws@gmail.com";
        string MailCc = ConfigurationManager.AppSettings["cc_mailid"].Trim();
        string MailBCc = "";
        try
        {
            System.Net.Mail.MailMessage MyMail = new System.Net.Mail.MailMessage();
            MyMail.From = new MailAddress(ConfigurationManager.AppSettings["smtplogin"].Trim());
            MyMail.To.Add(MailTo);
            MyMail.Subject = subject;
            MyMail.Body = Message;
            MyMail.CC.Add(MailCc);
            MyMail.BodyEncoding = System.Text.Encoding.UTF8;
            MyMail.IsBodyHtml = true;
            MyMail.Priority = System.Net.Mail.MailPriority.High;
            MyMail.Attachments.Add(new Attachment(excelpath));
            SmtpClient mysmtpmail = new SmtpClient();
            NetworkCredential basicCredential =
            new NetworkCredential(ConfigurationManager.AppSettings["smtplogin"].Trim(), ConfigurationManager.AppSettings["smtppassword"].Trim());
            mysmtpmail.Host = ConfigurationManager.AppSettings["Smtpserver"].Trim();
            mysmtpmail.Port = Convert.ToInt32(ConfigurationManager.AppSettings["Smtpport"].Trim());
            mysmtpmail.UseDefaultCredentials = false;
            mysmtpmail.Credentials = basicCredential;
            mysmtpmail.Send(MyMail);
            _Mailstatus = "Success";
        }
        catch (Exception ee)
        {
            _Mailstatus = "Failure : " + ee.Message.ToString();
        }

        SqlConnection conn = new SqlConnection(con);
        if (conn.State != ConnectionState.Open)
        {
            conn.Close();
            conn.Open();
        }

        SqlCommand cmd = new SqlCommand(@"insert into tbl_log_mail(Category,Refno,MailFrom,MailTo,MailCc,MailSub,MailContent,EmailDatatime,Mailstatus,MailBCC,MailAttachments)values(  
                                                    @Category,@Refno,@MailFrom,@MailTo,@MailCc,@MailSub,@MailContent,getdate(),@Mailstatus,@MailBCC,@MailAttachments)", conn);
        cmd.CommandType = CommandType.Text;
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@Category", "Daily Status Report");
        cmd.Parameters.AddWithValue("@Refno", DateTime.Now.ToString());
        cmd.Parameters.AddWithValue("@MailFrom", "vimal.bws@gmail.com");
        cmd.Parameters.AddWithValue("@MailTo", MailTo);
        cmd.Parameters.AddWithValue("@MailCc", MailCc);
        cmd.Parameters.AddWithValue("@MailSub", "Daily Status Report");
        cmd.Parameters.AddWithValue("@MailContent", Message);
        cmd.Parameters.AddWithValue("@Mailstatus", _Mailstatus);
        cmd.Parameters.AddWithValue("@MailBCC", MailBCc);
        cmd.Parameters.AddWithValue("@MailAttachments", excelpath);
        cmd.ExecuteNonQuery();
    }
}