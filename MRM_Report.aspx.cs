/// <summary>
/// Required Namespaces
/// </summary>
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
using NPOI.HSSF.UserModel;
using NPOI.SS.Util;
using NPOI.SS.UserModel;
using NPOI.HSSF.Util;
using System.Linq;

public partial class MRM_Report : System.Web.UI.Page
{
    #region Data Memeber

    /// <summary>
    /// Declarations Part For Variables,Strings,SqlConnections,etc
    /// </summary>
    //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TMS"].ToString());
    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ToString());
    SqlCommand com = new SqlCommand();
    string DateConverted = string.Empty;
    SqlCommand cmd = new SqlCommand();
    SqlDataAdapter da = new SqlDataAdapter();
    HSSFWorkbook hssfworkbook = new HSSFWorkbook();
    string fromdate, todate, fromdate_Month, fromdate_Year, todate_Month, todate_Year;
    DateTime d_fromdate, d_todate;
    string new_month, timess, change_month, times, sun, ss, currentMonth, day, mnth, yy, s1, s2, s3, s4, s5, s6, s11, s12 = "";
    int new_request, change_note, new_com, change_com, man_powers, sunday, dd, mm, yyyy, end_year, end_month, mm_num, sundays, diff_month, working_days;
    double total_hours, Abstraction, Accounts, CB, CE, Coding, Conversion, CRM, Design, Digital_Media, HR, L2_Conversion, L2_TP, Marketting, MIS, RandD, RCM, Software, Support, TCL, TCT, TP, Vendor, IEEEJournals, L2IEEE, L2Conversion, L2Design, L2Support, L3RCM;
    int[] total_req = new int[100];
    int[] total_com = new int[100];
    int[] new_req = new int[100];
    int[] change_req = new int[100];
    int[] man_pow = new int[100];
    double[] total_hrs = new double[100];
    double[] Abstraction1 = new double[100];
    double[] Accounts1 = new double[100];
    double[] CB1 = new double[100];
    double[] CE1 = new double[100];
    double[] Coding1 = new double[100];
    double[] Conversion1 = new double[100];
    double[] CRM1 = new double[100];
    double[] Design1 = new double[100];
    double[] Digital_Media1 = new double[100];
    double[] HR1 = new double[100];
    double[] L2_Conversion1 = new double[100];
    double[] L2_TP1 = new double[100];
    double[] Marketting1 = new double[100];
    double[] MIS1 = new double[100];
    double[] RandD1 = new double[100];
    double[] RCM1 = new double[100];
    double[] Software1 = new double[100];
    double[] Support1 = new double[100];
    double[] TCL1 = new double[100];
    double[] TCT1 = new double[100];
    double[] TP1 = new double[100];
    double[] Vendor1 = new double[100];
    double[] IEEEJournals1 = new double[100];
    double[] L2IEEE1 = new double[100];
    double[] L2Conversion1 = new double[100];
    double[] L2Design1 = new double[100];
    double[] L2Support1 = new double[100];
    double[] L3RCM1 = new double[100];
    double[] tot_manshift = new double[100];
    int[] sun_day = new int[100];
    double[] man_hours = new double[100];
    double[] s = new double[100];
    double s7 = 0.0;
    double leave = 0.0;
    double[] leaves = new double[100];
    double[] Ex_time = new double[100];
    double[] avail_manshift = new double[100];
    double[] avail_manhours = new double[100];
    double[] meet_time = new double[100];
    double e_time = 0.0;
    double meet = 0.0;
    double new_training = 0.0;
    double[] pass_man_hours = new double[100];
    double[] new_proj_training = new double[100];
    double[] new_proj = new double[100];
    double[] downl = new double[100];
    double down = 0.0;
    double[] analysis = new double[100];
    double anal = 0.0;
    double[] troubleshooting = new double[100];
    double trouble = 0.0;
    double[] communication = new double[100];
    double comm = 0.0;
    double[] on_duty = new double[100];
    double onduty = 0.0;
    double[] documentation = new double[100];
    double document = 0.0;
    int[] production = new int[100];
    double shut = 0.0;
    double[] shutdown = new double[100];
    double no_inp = 0.0;
    double[] no_input = new double[100];
    double rnd = 0.0;
    double[] rnd_sample = new double[100];
    double new_tech = 0.0;
    double[] new_technology = new double[100];
    double gene = 0.0;
    double[] general = new double[100];
    int[] non_production = new int[100];
    int[] prod_and_nonprod = new int[100];
    string[] x = new string[3];
    int[] y = new int[3];
    #endregion

    /// <summary>
    /// Page_s the load.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            txt_FromDate.Attributes.Add("readonly", "readonly");
            txt_ToDate.Attributes.Add("readonly", "readonly");
            CultureInfo provider = CultureInfo.InvariantCulture;
            d_fromdate = DateTime.ParseExact(txt_FromDate.Text, "d/M/yyyy", provider);

            d_todate = DateTime.ParseExact(txt_ToDate.Text, "d/M/yyyy", provider);

            fromdate_Month = txt_FromDate.Text.ToString().Substring(txt_FromDate.ToString().Length - 4);

            fromdate_Year = txt_FromDate.Text.ToString().Substring(txt_FromDate.ToString().Length - 4);
            todate_Month = txt_FromDate.Text.ToString().Substring(txt_FromDate.ToString().Length - 4);
            todate_Year = txt_FromDate.Text.ToString().Substring(txt_FromDate.ToString().Length - 4);


            //d_fromdate = Convert.ToDateTime (fromdate);

            //d_todate = Convert.ToDateTime (todate);
            if (!IsPostBack)
            {
                string Rights = Convert.ToString(Session["Rights"]);
                getDate();

                calext_FromDate.EndDate = DateTime.Now;
                calext_ToDate.EndDate = DateTime.Now;

                if (Session["Userid"] == null) Response.Redirect("Login");
                else if (Session["Userid"].ToString() == "") Response.Redirect("Login");
                Rights = Convert.ToString(Session["Rights"]);
                if (Rights == "Administrator")
                {

                }
                else if (Rights == "Team Leader")
                {

                }
                else
                {
                }
            }
        }
        catch (Exception)
        {

        }
    }

    /// <summary>
    /// Gets the Current date.
    /// </summary>
    protected void getDate()
    {
        DateTime dt = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-GB"));
        DateConverted = dt.ToString("dd/MM/yyyy");
        txt_FromDate.Text = DateConverted;
        txt_ToDate.Text = DateConverted;
        calext_ToDate.StartDate = DateTime.Today;
    }

    /// <summary>
    /// Calculation this instance.
    /// </summary>
    protected void Calculation()
    {
        end_year = Convert.ToInt32(d_fromdate.Year);
        end_month = Convert.ToInt32(d_fromdate.Month);
        mm_num = 0;
        sundays = 0;
        diff_month = 0;

        if (ConnectionState.Closed == conn.State) conn.Open();
        cmd.Connection = conn;
        cmd.CommandText = "SELECT DATEDIFF(MONTH,'" + d_fromdate.Year + "-" + d_fromdate.Month + "-01" + "','" + d_todate.Year + "-" + d_todate.Month + "-" + DateTime.DaysInMonth(Convert.ToInt32(d_fromdate.Year), Convert.ToInt32(d_todate.Month)) + "') + 1";
        da = new SqlDataAdapter(cmd);
        DataTable dts = new DataTable();
        da.Fill(dts);
        if (dts.Rows.Count > 0)
        {
            s1 = dts.Rows[0].ItemArray[0].ToString();
            diff_month = Convert.ToInt32(s1);
        }


        for (int n = 1; n <= diff_month; n++)
        {
            sundays = 0;
            DateTime endOfMonth = new DateTime(Convert.ToInt32(end_year), Convert.ToInt32(end_month), DateTime.DaysInMonth(Convert.ToInt32(end_year), Convert.ToInt32(end_month)));
            DateTime datevalue = (Convert.ToDateTime(endOfMonth.ToString()));

            day = datevalue.Day.ToString();
            mnth = datevalue.Month.ToString();
            yy = datevalue.Year.ToString();
            dd = Convert.ToInt32(day);
            mm = Convert.ToInt32(mnth);
            yyyy = Convert.ToInt32(yy);

            currentMonth = string.Empty;
            if (n == 0)
                currentMonth = d_fromdate.Month.ToString();
            else
                currentMonth = Convert.ToString(Convert.ToInt32(currentMonth + n));
            if (currentMonth == "13")
                currentMonth = "1";

            if (mm > 0 && mm < 10)
            {
                new_month = "Requestdate between" + " " + "'" + yyyy + "-0" + mm + "-01" + "'" + " " + " and " + " " + "'" + yyyy + "-0" + mm + "-" + dd + "'";
                change_month = "requireddate between" + " " + "'" + yyyy + "-0" + mm + "-01" + "'" + " " + " and " + " " + "'" + yyyy + "-0" + mm + "-" + dd + "'";
                times = "currentdate between" + " " + "'" + yyyy + "-0" + mm + "-01" + "'" + " " + " and " + " " + "'" + yyyy + "-0" + mm + "-" + dd + "'";
                timess = "a.shiftdate between" + " " + "'" + yyyy + "-0" + mm + "-01" + "'" + " " + " and " + " " + "'" + yyyy + "-0" + mm + "-" + dd + "'";
            }
            else
            {
                new_month = "Requestdate between" + " " + "'" + yyyy + "-" + mm + "-01" + "'" + " " + " and " + " " + "'" + yyyy + "-" + mm + "-" + dd + "'";
                change_month = "requireddate between" + " " + "'" + yyyy + "-" + mm + "-01" + "'" + " " + " and " + " " + "'" + yyyy + "-" + mm + "-" + dd + "'";
                times = "currentdate between" + " " + "'" + yyyy + "-" + mm + "-01" + "'" + " " + " and " + " " + "'" + yyyy + "-" + mm + "-" + dd + "'";
                timess = "a.shiftdate between" + " " + "'" + yyyy + "-" + mm + "-01" + "'" + " " + " and " + " " + "'" + yyyy + "-" + mm + "-" + dd + "'";
            }

            if (ConnectionState.Closed == conn.State) conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT count(*) from tbl_taskmaster where " + new_month;
            da = new SqlDataAdapter(cmd);
            DataTable dt1 = new DataTable();
            da.Fill(dt1);
            if (dt1.Rows.Count > 0)
            {
                s1 = dt1.Rows[0].ItemArray[0].ToString();
                new_request = Convert.ToInt32(s1);

            }

            dt1.Clear();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT count(*) from tbl_taskmaster where " + change_month;
            da = new SqlDataAdapter(cmd);
            DataTable dt2 = new DataTable();
            da.Fill(dt2);
            if (dt2.Rows.Count > 0)
            {
                s2 = dt2.Rows[0].ItemArray[0].ToString();
                change_note = Convert.ToInt32(s2);
            }

            dt2.Clear();
            cmd.Connection = conn;
            da = new SqlDataAdapter(cmd);
            DataTable dt3 = new DataTable();
            da.Fill(dt3);
            if (dt3.Rows.Count > 0)
            {
                s3 = dt3.Rows[0].ItemArray[0].ToString();
                new_com = Convert.ToInt32(s3);
            }

            dt3.Clear();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT count(*) from tbl_taskmaster where taskstatus='Completed' and " + change_month;
            da = new SqlDataAdapter(cmd);
            DataTable dt4 = new DataTable();
            da.Fill(dt4);
            if (dt4.Rows.Count > 0)
            {
                s4 = dt4.Rows[0].ItemArray[0].ToString();
                change_com = Convert.ToInt32(s4);
            }

            dt4.Clear();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT count(*) from tbl_usermaster where status='1'";
            da = new SqlDataAdapter(cmd);
            DataTable dt5 = new DataTable();
            da.Fill(dt5);
            if (dt5.Rows.Count > 0)
            {
                s5 = dt5.Rows[0].ItemArray[0].ToString();
                man_powers = Convert.ToInt32(s5);
            }

            dt5.Clear();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT SUM(DATEDIFF(MINUTE,'00:00:00', TotalTime))/60,SUM(DATEDIFF(MINUTE,'00:00:00', TotalTime))%60 FROM PrmsProductionHour_Backup where " + times;
            da = new SqlDataAdapter(cmd);
            DataTable dt6 = new DataTable();
            da.Fill(dt6);
            if (dt6.Rows.Count > 0)
            {
                s6 = dt6.Rows[0].ItemArray[0].ToString();
                s11 = dt6.Rows[0].ItemArray[1].ToString();
                string ad = s6 + "." + s11;
                if (s6 == "" && s11 == "")
                {
                    total_hours = 0;
                }
                else
                {
                    total_hours = Convert.ToDouble(ad);
                }
            }

            dt6.Clear();
            cmd.Connection = conn;
            cmd.CommandText = "select types from tbl_ProjectSubType union select distinct typeproject from tbl_ProjectReq";
            da = new SqlDataAdapter(cmd);
            DataTable dt7 = new DataTable();
            da.Fill(dt7);
            if (dt7.Rows.Count > 0)
            {
                for (int i = 0; i < dt7.Rows.Count; i++)
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT SUM(DATEDIFF(MINUTE,'00:00:00', a.TotalTime))/60,SUM(DATEDIFF(MINUTE,'00:00:00', a.TotalTime))%60 FROM PrmsProductionHour_Backup a left outer join tbl_ProjectReq b on b.projectid=a.ProjectID  where b.typeproject='" + dt7.Rows[i].ItemArray[0].ToString() + "' and b.projectid!='NA' and a.projectid!='NA' and " + times;
                    da = new SqlDataAdapter(cmd);
                    DataTable dt8 = new DataTable();
                    da.Fill(dt8);
                    if (dt8.Rows.Count > 0)
                    {
                        ss = dt8.Rows[0].ItemArray[0].ToString();
                        s12 = dt8.Rows[0].ItemArray[1].ToString();
                        string add = ss + "." + s12;
                        if (ss == "" && s12 == "")
                        {
                            s[i] = 0;
                        }
                        else
                        {
                            s[i] = Convert.ToDouble(add);
                            s7 += s[i];
                        }
                    }
                    Abstraction = s[0];
                    Accounts = s[1];
                    CB = s[2];
                    CE = s[3];
                    Coding = s[4];
                    Conversion = s[5];
                    CRM = s[6];
                    Design = s[7];
                    Digital_Media = s[8];
                    HR = s[9];
                    L2_TP = s[10];
                    Marketting = s[11];
                    MIS = s[12];
                    RandD = s[13];
                    RCM = s[14];
                    Software = s[15];
                    Support = s[16];
                    TCL = s[17];
                    TCT = s[18];
                    TP = s[19];
                    Vendor = s[20];
                    IEEEJournals = s[21];
                    L2IEEE = s[22];
                    L2Conversion = s[23];
                    L2Design = s[24];
                    L2Support = s[25];
                    L3RCM = s[26];
                }
                for (int j = 1; j <= DateTime.DaysInMonth(end_year, end_month); j++)
                {
                    DateTime thisDay = new DateTime(end_year, end_month, j);
                    if (thisDay.DayOfWeek == DayOfWeek.Sunday)
                    {
                        sundays += 1;
                    }
                }
                sunday = sundays;
                sun = Convert.ToString(sunday);
                int days = Convert.ToInt32(sun);
                int _day = Convert.ToInt32(day);
                working_days = _day - days;

                /*if (ConnectionState.Closed == con.State) con.Open();
				cmd.Connection = con;
				cmd.CommandText = "SELECT COUNT(*) as apsenties from tbl_ShiftTiming a join tbl_Master_employee b on a.empid=b.empid where b.dcode='24' and b.empstat='A' and a.attstatus='AA' and " + timess;
				da = new SqlDataAdapter(cmd);
				DataTable dt = new DataTable();
				da.Fill(dt);
				if (dt.Rows.Count > 0)
				{
					string ll = dt.Rows[0].ItemArray[0].ToString();
					if (ll == "")
					{
						leave = 0;
					}
					else
					{
						leave = Convert.ToInt32(ll);
					}
				}

				dt.Clear();
				cmd.CommandText = "SELECT SUM(DATEDIFF(MINUTE,'00:00:00', a.Extendsecondoffhrs))/60,SUM(DATEDIFF(MINUTE,'00:00:00', a.Extendsecondoffhrs))%60 FROM tbl_ShiftTiming a join tbl_Master_employee b on a.empid=b.empid where b.dcode='24' and b.empstat='A' and " + timess;
				da = new SqlDataAdapter(cmd);
				DataTable dt = new DataTable();
				da.Fill(dt);
				if (dt.Rows.Count > 0)
				{
					string tt = dt.Rows[0].ItemArray[0].ToString();
					string t1 = dt.Rows[0].ItemArray[1].ToString();
					string ta = tt + "." + t1;
					if (tt == "" && t1 == "")
					{
						e_time = 0;
					}
					else
					{
						e_time = Convert.ToDouble(ta);
					}
				}
				*/
                if (ConnectionState.Open == conn.State) conn.Close();

                //dt.Clear();
                /*cmd.Connection = conn;
				cmd.CommandText = "SELECT SUM(DATEDIFF(MINUTE,'00:00:00', totaltakentime))/60,SUM(DATEDIFF(MINUTE,'00:00:00', totaltakentime))%60 FROM DPR_Test WHERE meeting_category='Working Project' and " + times;
				da = new SqlDataAdapter(cmd);
				DataTable dt = new DataTable();
				da.Fill(dt);
				if (dt.Rows.Count > 0)
				{
					string mt = dt.Rows[0].ItemArray[0].ToString();
					string ma = dt.Rows[0].ItemArray[1].ToString();
					string ms = mt + "." + ma;
					if (mt == "" && ma == "")
					{
						meet = 0;
					}
					else
					{
						meet = Convert.ToDouble(ms);
					}
				}*/

                //dt.Clear();


                /*cmd.Connection = con;
				cmd.CommandText = "SELECT SUM(DATEDIFF(MINUTE,'00:00:00', totaltakentime))/60,SUM(DATEDIFF(MINUTE,'00:00:00', totaltakentime))%60 FROM DPR_Test WHERE meeting_category='New Project' and " + times;
				da = new SqlDataAdapter(cmd);
				dt = new DataTable();
				da.Fill(dt);
				if (dt.Rows.Count > 0)
				{
					string mt = dt.Rows[0].ItemArray[0].ToString();
					string ma = dt.Rows[0].ItemArray[1].ToString();
					string ms = mt + "." + ma;
					if (mt == "" && ma == "")
					{
						new_training = 0;
					}
					else
					{
						new_training = Convert.ToDouble(ms);
					}
				}

				dt.Clear();
				*/

                /*
				cmd.Connection = con;
				cmd.CommandText = "SELECT SUM(DATEDIFF(MINUTE,'00:00:00', totaltakentime))/60,SUM(DATEDIFF(MINUTE,'00:00:00', totaltakentime))%60 FROM DPR_Test WHERE task_type='Downloading/Uploading' and " + times;
				da = new SqlDataAdapter(cmd);
				dt = new DataTable();
				da.Fill(dt);
				if (dt.Rows.Count > 0)
				{
					string mt = dt.Rows[0].ItemArray[0].ToString();
					string ma = dt.Rows[0].ItemArray[1].ToString();
					string ms = mt + "." + ma;
					if (mt == "" && ma == "")
					{
						down = 0;
					}
					else
					{
						down = Convert.ToDouble(ms);
					}
				}

				dt.Clear();
				*/

                /*
				cmd.Connection = con;
				cmd.CommandText = "SELECT SUM(DATEDIFF(MINUTE,'00:00:00', totaltakentime))/60,SUM(DATEDIFF(MINUTE,'00:00:00', totaltakentime))%60 FROM DPR_Test WHERE (task_type='Input Analysis' or task_type='Code Analysis') and " + times;
				da = new SqlDataAdapter(cmd);
				dt = new DataTable();
				da.Fill(dt);
				if (dt.Rows.Count > 0)
				{
					string mt = dt.Rows[0].ItemArray[0].ToString();
					string ma = dt.Rows[0].ItemArray[1].ToString();
					string ms = mt + "." + ma;
					if (mt == "" && ma == "")
					{
						anal = 0;
					}
					else
					{
						anal = Convert.ToDouble(ms);
					}
				}
*/
                //dt.Clear();


                /*
				cmd.Connection = con;
				cmd.CommandText = "SELECT SUM(DATEDIFF(MINUTE,'00:00:00', totaltakentime))/60,SUM(DATEDIFF(MINUTE,'00:00:00', totaltakentime))%60 FROM DPR_Test WHERE task_type='TroubleShooting' and " + times;
				da = new SqlDataAdapter(cmd);
				dt = new DataTable();
				da.Fill(dt);
				if (dt.Rows.Count > 0)
				{
					string mt = dt.Rows[0].ItemArray[0].ToString();
					string ma = dt.Rows[0].ItemArray[1].ToString();
					string ms = mt + "." + ma;
					if (mt == "" && ma == "")
					{
						trouble = 0;
					}
					else
					{
						trouble = Convert.ToDouble(ms);
					}
				}

				dt.Clear();
*/
                /*
				cmd.Connection = con;
				cmd.CommandText = "SELECT SUM(DATEDIFF(MINUTE,'00:00:00', totaltakentime))/60,SUM(DATEDIFF(MINUTE,'00:00:00', totaltakentime))%60 FROM DPR_Test WHERE task_type='Communication' and " + times;
				da = new SqlDataAdapter(cmd);
				dt = new DataTable();
				da.Fill(dt);
				if (dt.Rows.Count > 0)
				{
					string mt = dt.Rows[0].ItemArray[0].ToString();
					string ma = dt.Rows[0].ItemArray[1].ToString();
					string ms = mt + "." + ma;
					if (mt == "" && ma == "")
					{
						comm = 0;
					}
					else
					{
						comm = Convert.ToDouble(ms);
					}
				}

				dt.Clear();
				*/

                /*
				cmd.Connection = con;
				cmd.CommandText = "SELECT SUM(DATEDIFF(MINUTE,'00:00:00', totaltakentime))/60,SUM(DATEDIFF(MINUTE,'00:00:00', totaltakentime))%60 FROM DPR_Test WHERE task_type='On-Duty' and " + times;
				da = new SqlDataAdapter(cmd);
				dt = new DataTable();
				da.Fill(dt);
				if (dt.Rows.Count > 0)
				{
					string mt = dt.Rows[0].ItemArray[0].ToString();
					string ma = dt.Rows[0].ItemArray[1].ToString();
					string ms = mt + "." + ma;
					if (mt == "" && ma == "")
					{
						onduty = 0;
					}
					else
					{
						onduty = Convert.ToDouble(ms);
					}
				}

				dt.Clear();

*/


                /*
				cmd.Connection = con;
				cmd.CommandText = "SELECT SUM(DATEDIFF(MINUTE,'00:00:00', totaltakentime))/60,SUM(DATEDIFF(MINUTE,'00:00:00', totaltakentime))%60 FROM DPR_Test WHERE task_type='Documentation' and " + times;
				da = new SqlDataAdapter(cmd);
				dt = new DataTable();
				da.Fill(dt);
				if (dt.Rows.Count > 0)
				{
					string mt = dt.Rows[0].ItemArray[0].ToString();
					string ma = dt.Rows[0].ItemArray[1].ToString();
					string ms = mt + "." + ma;
					if (mt == "" && ma == "")
					{
						document = 0;
					}
					else
					{
						document = Convert.ToDouble(ms);
					}
				}

				dt.Clear();
				*/


                /*
				cmd.Connection = con;
				cmd.CommandText = "SELECT SUM(DATEDIFF(MINUTE,'00:00:00', totaltakentime))/60,SUM(DATEDIFF(MINUTE,'00:00:00', totaltakentime))%60 FROM DPR_Test WHERE (non_production='System Breakdown' or non_production='Power Shutdown' or non_production='Internet/Intranet conectivity Failure' or non_production='Others') and " + times;
				da = new SqlDataAdapter(cmd);
				dt = new DataTable();
				da.Fill(dt);
				if (dt.Rows.Count > 0)
				{
					string mt = dt.Rows[0].ItemArray[0].ToString();
					string ma = dt.Rows[0].ItemArray[1].ToString();
					string ms = mt + "." + ma;
					if (mt == "" && ma == "")
					{
						shut = 0;
					}
					else
					{
						shut = Convert.ToDouble(ms);
					}
				}

				dt.Clear();


*/

                /*
				cmd.Connection = con;
				cmd.CommandText = "SELECT SUM(DATEDIFF(MINUTE,'00:00:00', totaltakentime))/60,SUM(DATEDIFF(MINUTE,'00:00:00', totaltakentime))%60 FROM DPR_Test WHERE non_production='No Input' and " + times;
				da = new SqlDataAdapter(cmd);
				dt = new DataTable();
				da.Fill(dt);
				if (dt.Rows.Count > 0)
				{
					string mt = dt.Rows[0].ItemArray[0].ToString();
					string ma = dt.Rows[0].ItemArray[1].ToString();
					string ms = mt + "." + ma;
					if (mt == "" && ma == "")
					{
						no_inp = 0;
					}
					else
					{
						no_inp = Convert.ToDouble(ms);
					}
				}

				dt.Clear();

*/

                /*
				cmd.Connection = con;


				cmd.CommandText = "SELECT SUM(DATEDIFF(MINUTE,'00:00:00', totaltakentime))/60,SUM(DATEDIFF(MINUTE,'00:00:00', totaltakentime))%60 FROM DPR_Test WHERE non_production='RND - Sample Analysis and Creation' and " + times;
				da = new SqlDataAdapter(cmd);
				dt = new DataTable();
				da.Fill(dt);
				if (dt.Rows.Count > 0)
				{
					string mt = dt.Rows[0].ItemArray[0].ToString();
					string ma = dt.Rows[0].ItemArray[1].ToString();
					string ms = mt + "." + ma;
					if (mt == "" && ma == "")
					{
						rnd = 0;
					}
					else
					{
						rnd = Convert.ToDouble(ms);
					}
				}

				dt.Clear();

*/

                /*
				cmd.Connection = con;
				cmd.CommandText = "SELECT SUM(DATEDIFF(MINUTE,'00:00:00', totaltakentime))/60,SUM(DATEDIFF(MINUTE,'00:00:00', totaltakentime))%60 FROM DPR_Test WHERE meeting_category='New Technology' and " + times;
				da = new SqlDataAdapter(cmd);
				dt = new DataTable();
				da.Fill(dt);
				if (dt.Rows.Count > 0)
				{
					string mt = dt.Rows[0].ItemArray[0].ToString();
					string ma = dt.Rows[0].ItemArray[1].ToString();
					string ms = mt + "." + ma;
					if (mt == "" && ma == "")
					{
						new_tech = 0;
					}
					else
					{
						new_tech = Convert.ToDouble(ms);
					}
				}

				dt.Clear();
				*/


                /*
				cmd.Connection = con;
				cmd.CommandText = "SELECT SUM(DATEDIFF(MINUTE,'00:00:00', totaltakentime))/60,SUM(DATEDIFF(MINUTE,'00:00:00', totaltakentime))%60 FROM DPR_Test WHERE meeting_category='General' and " + times;
				da = new SqlDataAdapter(cmd);
				dt = new DataTable();
				da.Fill(dt);
				if (dt.Rows.Count > 0)
				{
					string mt = dt.Rows[0].ItemArray[0].ToString();
					string ma = dt.Rows[0].ItemArray[1].ToString();
					string ms = mt + "." + ma;
					if (mt == "" && ma == "")
					{
						gene = 0;
					}
					else
					{
						gene = Convert.ToDouble(ms);
					}
				}
				*/
            }
            dt7.Clear();
            if (end_month == 12)
            {
                end_month = 1;
                end_year++;
            }
            else
            {
                end_month++;
            }

            total_req[n] = new_request + change_note;
            total_com[n] = new_com + change_com;
            new_req[n] = new_request;
            change_req[n] = change_note;
            man_pow[n] = man_powers;
            Abstraction1[n] = Abstraction;
            Accounts1[n] = Accounts;
            CB1[n] = CB;
            CE1[n] = CE;
            Coding1[n] = Coding;
            Conversion1[n] = Conversion;
            CRM1[n] = CRM;
            Design1[n] = Design;
            Digital_Media1[n] = Digital_Media;
            HR1[n] = HR;
            L2_Conversion1[n] = L2_Conversion;
            L2_TP1[n] = L2_TP;
            Marketting1[n] = Marketting;
            MIS1[n] = MIS;
            RandD1[n] = RandD;
            RCM1[n] = RCM;
            Software1[n] = Software;
            Support1[n] = Support;
            TCL1[n] = TCL;
            TCT1[n] = TCT;
            TP1[n] = TP;
            Vendor1[n] = Vendor;
            IEEEJournals1[n] = IEEEJournals;
            L2IEEE1[n] = L2IEEE;
            L2Conversion1[n] = L2Conversion;
            L2Design1[n] = L2Design;
            L2Support1[n] = L2Support;
            L3RCM1[n] = L3RCM;
            sun_day[n] = working_days;
            leaves[n] = leave;
            Ex_time[n] = e_time;
            meet_time[n] = meet;
            new_proj[n] = new_training;
            downl[n] = down;
            analysis[n] = anal;
            troubleshooting[n] = trouble;
            communication[n] = comm;
            on_duty[n] = onduty;
            documentation[n] = document;
            shutdown[n] = shut;
            no_input[n] = no_inp;
            rnd_sample[n] = rnd;
            new_technology[n] = new_tech;
            general[n] = gene;
            total_hrs[n] = Abstraction1[n] + Accounts1[n] + CB1[n] + CE1[n] + Coding1[n] + Conversion1[n] + CRM1[n] + Design1[n] + Digital_Media1[n] + HR1[n] + L2_Conversion1[n] + L2_TP1[n] + Marketting1[n] + MIS1[n] + RandD1[n] + RCM1[n] + Software1[n] + Support1[n] + TCL1[n] + TCT1[n] + TP1[n] + Vendor1[n] + IEEEJournals1[n] + IEEEJournals1[n] + L2IEEE1[n] + L2Conversion1[n] + L2Design1[n] + L2Support1[n] + L3RCM1[n];
        }
        if (ConnectionState.Open == conn.State) conn.Close();
    }

    /// <summary>
    /// Reset Function if Cancel is Clicked
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void btn_Cancel_Click(object sender, EventArgs e)
    {
        getDate();
    }
    protected void btn_Export_Click(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// Bind Gridview
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">E.</param>
    protected void btn_Report_Click(object sender, EventArgs e)
    {
        //Calculation();
        if (txt_FromDate.Text == "")
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "Script", "swal('Start Date is Required')", true);
        }
        else if (txt_ToDate.Text == "")
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "Script", "swal('End Date is Required')", true);
        }
        else
        {
            end_year = Convert.ToInt32(d_fromdate.Year);
            end_month = Convert.ToInt32(d_fromdate.Month);
            mm_num = 0;
            sundays = 0;
            diff_month = 0;

            // Get Month different to Selected month
            if (ConnectionState.Closed == conn.State) conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT DATEDIFF(MONTH,'" + d_fromdate.Year + "-" + d_fromdate.Month + "-01" + "','" + d_todate.Year + "-" + d_todate.Month + "-" + DateTime.DaysInMonth(Convert.ToInt32(d_fromdate.Year), Convert.ToInt32(d_todate.Month)) + "') + 1";
            da = new SqlDataAdapter(cmd);
            DataTable dts = new DataTable();
            da.Fill(dts);
            if (dts.Rows.Count > 0)
            {
                s1 = dts.Rows[0].ItemArray[0].ToString();
                diff_month = Convert.ToInt32(s1);
            }

            var Header = Convert.ToString(d_fromdate.ToString("MMM")) + "" + Convert.ToInt32(d_fromdate.Year) + "' to '" + Convert.ToString(d_todate.ToString("MMM")) + "" + Convert.ToInt32(d_todate.Year);
            int cols = diff_month + 2;
            int colsp = diff_month + 4;
            mm_num = d_fromdate.Month;
            int e_year = d_fromdate.Year;

            //create a excel book

            ISheet excelsheet = hssfworkbook.CreateSheet("PPM Consolidate");

            // Set the Styles of header rows and columns
            IFont headerFont = hssfworkbook.CreateFont();
            ICellStyle headerFontStyle = hssfworkbook.CreateCellStyle();
            headerFont.Boldweight = (short)FontBoldWeight.Bold;
            headerFont.FontHeight = 14 * 14;
            headerFontStyle.FillForegroundColor = HSSFColor.LightGreen.Index;
            headerFontStyle.FillPattern = FillPattern.SolidForeground;
            headerFontStyle.Alignment = HorizontalAlignment.Center;
            headerFontStyle.VerticalAlignment = VerticalAlignment.Center;
            headerFontStyle.WrapText = true;
            headerFontStyle.SetFont(headerFont);
            // Set the border styles of header rows and columns
            headerFontStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            headerFontStyle.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
            headerFontStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            headerFontStyle.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
            headerFontStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            headerFontStyle.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
            headerFontStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            headerFontStyle.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;

            // Set the styles of content rows and columns
            IFont lowFont = hssfworkbook.CreateFont();
            ICellStyle lowCellStyle = hssfworkbook.CreateCellStyle();
            lowFont.FontHeight = 14 * 14;
            lowCellStyle.Alignment = HorizontalAlignment.Center;
            lowCellStyle.WrapText = true;
            lowCellStyle.Alignment = HorizontalAlignment.Center;
            lowCellStyle.VerticalAlignment = VerticalAlignment.Center;
            lowCellStyle.SetFont(lowFont);
            // Set the border styles of content rows and columns
            lowCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            lowCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            lowCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            lowCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            lowCellStyle.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
            lowCellStyle.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
            lowCellStyle.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
            lowCellStyle.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;

            // set the Styles of colspan and rowspan fields

            IFont spanFont = hssfworkbook.CreateFont();
            ICellStyle spanColumnCellStyle = hssfworkbook.CreateCellStyle();
            spanFont.FontHeight = 14 * 14;
            spanColumnCellStyle.Alignment = HorizontalAlignment.Center;
            spanColumnCellStyle.WrapText = true;
            spanColumnCellStyle.Alignment = HorizontalAlignment.Center;
            spanColumnCellStyle.VerticalAlignment = VerticalAlignment.Center;
            // Set the border styles of colspan and rowspan fields
            spanColumnCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            spanColumnCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            spanColumnCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            spanColumnCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            spanColumnCellStyle.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
            spanColumnCellStyle.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
            spanColumnCellStyle.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
            spanColumnCellStyle.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
            spanColumnCellStyle.SetFont(spanFont);

            // Set the different style rotation format text vertical alignment

            IFont rotateFont = hssfworkbook.CreateFont();
            ICellStyle rotateFontStyle = hssfworkbook.CreateCellStyle();
            rotateFont.Boldweight = (short)FontBoldWeight.Bold;
            rotateFont.FontHeight = 15 * 15;
            rotateFontStyle.Alignment = HorizontalAlignment.Center;
            rotateFontStyle.VerticalAlignment = VerticalAlignment.Center;
            rotateFontStyle.SetFont(rotateFont);
            rotateFontStyle.Rotation = 90;
            rotateFontStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            rotateFontStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            rotateFontStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            rotateFontStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            rotateFontStyle.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
            rotateFontStyle.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
            rotateFontStyle.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
            rotateFontStyle.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;

            ICell cell;
            excelsheet.DisplayGridlines = true;
            IRow row;

            // Create a row(0)

            row = excelsheet.CreateRow(0);
            row.Height = 800;

            // set the image of column to selected range

            cell = row.CreateCell(2);
            excelsheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 1));
            var patriarch = excelsheet.CreateDrawingPatriarch();
            HSSFClientAnchor anchor;
            anchor = new HSSFClientAnchor(0, 0, 0, 0, 0, 0, 1, 0);
            anchor.AnchorType = 2;
            string path = Server.MapPath("~/");
            path = path + @"assets\img\logo.png";
            var picture = patriarch.CreatePicture(anchor, LoadImage(path, hssfworkbook));
            picture.Resize();

            cell = row.CreateCell(2);
            cell.CellStyle = lowCellStyle;
            excelsheet.AddMergedRegion(new CellRangeAddress(0, 0, 2, 11));
            cell.SetCellValue("PPM Consolidated Report - '" + Header + "'");
            cell = row.CreateCell(11);

            // Create a row(1)

            row = excelsheet.CreateRow(1);
            row.Height = 500;

            cell = row.CreateCell(0);
            cell.CellStyle = headerFontStyle;
            excelsheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 0));
            cell.SetCellValue("Dep.");

            cell = row.CreateCell(1);
            cell.CellStyle = headerFontStyle;
            excelsheet.AddMergedRegion(new CellRangeAddress(1, 1, 1, 1));
            cell.SetCellValue("Month");

            cell = row.CreateCell(2);
            cell.CellStyle = headerFontStyle;
            excelsheet.AddMergedRegion(new CellRangeAddress(1, 1, 2, 2));
            cell.SetCellValue("Team Name");

            cell = row.CreateCell(3);
            cell.CellStyle = headerFontStyle;
            excelsheet.AddMergedRegion(new CellRangeAddress(1, 1, 3, 3));
            cell.SetCellValue("No of Requirements");

            cell = row.CreateCell(4);
            cell.CellStyle = headerFontStyle;
            excelsheet.AddMergedRegion(new CellRangeAddress(1, 1, 4, 4));
            cell.SetCellValue("Target");

            cell = row.CreateCell(5);
            cell.CellStyle = headerFontStyle;
            excelsheet.AddMergedRegion(new CellRangeAddress(1, 1, 5, 5));
            cell.SetCellValue("Hours");

            cell = row.CreateCell(6);
            cell.CellStyle = headerFontStyle;
            excelsheet.AddMergedRegion(new CellRangeAddress(1, 1, 6, 6));
            cell.SetCellValue("Productivity");

            cell = row.CreateCell(7);
            cell.CellStyle = headerFontStyle;
            excelsheet.AddMergedRegion(new CellRangeAddress(1, 1, 7, 7));
            cell.SetCellValue("Internal \n NC's");

            cell = row.CreateCell(8);
            cell.CellStyle = headerFontStyle;
            excelsheet.AddMergedRegion(new CellRangeAddress(1, 1, 8, 8));
            cell.SetCellValue("External \n Quality \n %");

            cell = row.CreateCell(9);
            cell.CellStyle = headerFontStyle;
            excelsheet.AddMergedRegion(new CellRangeAddress(1, 1, 9, 9));
            cell.SetCellValue("Delivery \n Performance \n (On-time)");

            cell = row.CreateCell(10);
            cell.CellStyle = headerFontStyle;
            excelsheet.AddMergedRegion(new CellRangeAddress(1, 1, 10, 10));
            cell.SetCellValue("Resource \n Ulitization \n for \n Production");

            cell = row.CreateCell(11);
            cell.CellStyle = headerFontStyle;
            excelsheet.AddMergedRegion(new CellRangeAddress(1, 1, 11, 11));
            cell.SetCellValue("Customer \n Complaints");

            // Set column width
            excelsheet.SetColumnWidth(0, 2000);
            excelsheet.SetColumnWidth(1, 2500);
            excelsheet.SetColumnWidth(2, 5000);
            excelsheet.SetColumnWidth(3, 5000);
            excelsheet.SetColumnWidth(4, 3500);
            excelsheet.SetColumnWidth(5, 3500);
            excelsheet.SetColumnWidth(6, 4000);
            excelsheet.SetColumnWidth(7, 3500);
            excelsheet.SetColumnWidth(8, 3500);
            excelsheet.SetColumnWidth(9, 3500);
            excelsheet.SetColumnWidth(10, 3500);
            excelsheet.SetColumnWidth(11, 3500);

            int total_requirement = 0;
            Double total_hours = 0;
            int countrow = 2;
            int rowscount = 0;
            Int32 currentMonth;

            for (int mm_count = 0; mm_count < diff_month; mm_count++)
            {
                sundays = 0;
                DateTime endOfMonth = new DateTime(Convert.ToInt32(end_year), Convert.ToInt32(end_month), DateTime.DaysInMonth(Convert.ToInt32(end_year), Convert.ToInt32(end_month)));
                DateTime datevalue = (Convert.ToDateTime(endOfMonth.ToString()));

                day = datevalue.Day.ToString();
                mnth = datevalue.Month.ToString();
                yy = datevalue.Year.ToString();
                dd = Convert.ToInt32(day);
                mm = Convert.ToInt32(mnth);
                yyyy = Convert.ToInt32(yy);

                if (mm_count == 0)
                    currentMonth = Convert.ToInt32(d_fromdate.Month);
                else
                    currentMonth = mm_count + Convert.ToInt32(d_fromdate.Month);
                if (currentMonth == 13) currentMonth = 1;

                if (mm > 0 && mm < 10)
                {
                    new_month = "Requestdate between" + " " + "'" + yyyy + "-0" + mm + "-01" + "'" + " " + " and " + " " + "'" + yyyy + "-0" + mm + "-" + dd + "'";
                    change_month = "requireddate between" + " " + "'" + yyyy + "-0" + mm + "-01" + "'" + " " + " and " + " " + "'" + yyyy + "-0" + mm + "-" + dd + "'";
                    times = "currentdate between" + " " + "'" + yyyy + "-0" + mm + "-01" + "'" + " " + " and " + " " + "'" + yyyy + "-0" + mm + "-" + dd + "'";
                }
                else
                {
                    new_month = "Requestdate between" + " " + "'" + yyyy + "-" + mm + "-01" + "'" + " " + " and " + " " + "'" + yyyy + "-" + mm + "-" + dd + "'";
                    change_month = "requireddate between" + " " + "'" + yyyy + "-" + mm + "-01" + "'" + " " + " and " + " " + "'" + yyyy + "-" + mm + "-" + dd + "'";
                    times = "currentdate between" + " " + "'" + yyyy + "-" + mm + "-01" + "'" + " " + " and " + " " + "'" + yyyy + "-" + mm + "-" + dd + "'";
                }

                // get query department wise time and requirementtask

                if (conn.State == ConnectionState.Closed) conn.Open();
                cmd.Parameters.Clear();
                cmd = new SqlCommand(@"select pr.typeproject[typeproject],COUNT(phb.ProjectID)[projecttype], isnull(SUM(DATEDIFF(MI,CAST('00:00' AS TIME),phb.TotalTime))/60,0) as hour, isnull(SUM(DATEDIFF(MI,CAST('00:00' AS TIME),phb.TotalTime))%60,0) as minute from tbl_ProjectSubType pst right join tbl_ProjectReq pr on pst.types=pr.typeproject left join PrmsProductionHour_Backup phb on phb.ProjectID=pr.projectid and phb." + times + " and phb.statusoftask=100 where pr.typeproject!='No-Project'  group by pr.typeproject", conn);
                da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (rowscount == 0) rowscount = dt.Rows.Count;

                // get query time on delivery task in new requirement
                int sumOfTasks = dt.AsEnumerable().Sum(x => x.Field<int>("projecttype"));
                cmd.Connection = conn;
                cmd.CommandText = "SELECT count(*) from tbl_taskmaster where taskstatus='Completed' and " + new_month;
                da = new SqlDataAdapter(cmd);
                DataTable dt3 = new DataTable();
                da.Fill(dt3);
                if (dt3.Rows.Count > 0)
                {
                    new_com = 0;
                    s3 = dt3.Rows[0].ItemArray[0].ToString();
                    new_com = Convert.ToInt32(s3);
                }

                // get query time on delivery task in change note requirement

                dt3.Clear();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT count(*) from tbl_taskmaster where taskstatus='Completed' and " + change_month;
                da = new SqlDataAdapter(cmd);
                DataTable dt4 = new DataTable();
                da.Fill(dt4);
                if (dt4.Rows.Count > 0)
                {
                    change_com = 0;
                    s4 = dt4.Rows[0].ItemArray[0].ToString();
                    change_com = Convert.ToInt32(s4);
                }
                change_com = sumOfTasks;
                // create a excel cells  to start

                if (mm_count == 0)
                {
                    row = excelsheet.CreateRow(2);
                    cell = row.CreateCell(0);
                    cell.CellStyle = rotateFontStyle;
                    excelsheet.AddMergedRegion(new CellRangeAddress(countrow, rowscount * diff_month + 1, 0, 0));
                    cell.SetCellValue("Software");
                }

                cell = row.CreateCell(1);
                cell.CellStyle = spanColumnCellStyle;
                if (mm_count == 0)
                    excelsheet.AddMergedRegion(new CellRangeAddress(countrow, rowscount + 1, 1, 1));
                else
                    excelsheet.AddMergedRegion(new CellRangeAddress(countrow, rowscount + countrow - mm_count - 1, 1, 1));
                if (mm_count == 0)
                    cell.SetCellValue(Convert.ToString(d_fromdate.ToString("MMM")) + "'" + Convert.ToInt32(d_fromdate.Year));
                else
                {
                    if (mm == 1)
                        cell.SetCellValue("Jan'" + Convert.ToInt32(yy));
                    else if (mm == 2)
                        cell.SetCellValue("Feb'" + Convert.ToInt32(yy));
                    else if (mm == 3)
                        cell.SetCellValue("Mar'" + Convert.ToInt32(yy));
                    else if (mm == 4)
                        cell.SetCellValue("Apr'" + Convert.ToInt32(yy));
                    else if (mm == 5)
                        cell.SetCellValue("May'" + Convert.ToInt32(yy));
                    else if (mm == 6)
                        cell.SetCellValue("Jun'" + Convert.ToInt32(yy));
                    else if (mm == 7)
                        cell.SetCellValue("Jul'" + Convert.ToInt32(yy));
                    else if (mm == 8)
                        cell.SetCellValue("Aug'" + Convert.ToInt32(yy));
                    else if (mm == 9)
                        cell.SetCellValue("Sep'" + Convert.ToInt32(yy));
                    else if (mm == 10)
                        cell.SetCellValue("Oct'" + Convert.ToInt32(yy));
                    else if (mm == 11)
                        cell.SetCellValue("Nov'" + Convert.ToInt32(yy));
                    else if (mm == 12)
                        cell.SetCellValue("Dec'" + Convert.ToInt32(yy));
                }
                total_requirement = 0;
                total_hours = 0;
                for (int teamcount = 0; teamcount < dt.Rows.Count; teamcount++)
                {
                    cell = row.CreateCell(2);
                    cell.CellStyle = lowCellStyle;
                    //  excelsheet.AddMergedRegion(new CellRangeAddress(countrow, countrow, 2, 2));
                    cell.SetCellValue(dt.Rows[teamcount]["typeproject"].ToString());

                    total_requirement = Convert.ToInt32(dt.Rows[teamcount]["projecttype"]);

                    cell = row.CreateCell(3);
                    cell.CellStyle = lowCellStyle;
                    cell.SetCellValue(total_requirement);

                    cell = row.CreateCell(4);
                    cell.CellStyle = lowCellStyle;
                    // excelsheet.AddMergedRegion(new CellRangeAddress(countrow, countrow, 4, 4));
                    cell.SetCellValue("");

                    string gettime = dt.Rows[teamcount]["hour"].ToString() + '.' + dt.Rows[teamcount]["minute"].ToString();
                    total_hours += Convert.ToDouble(gettime);

                    cell = row.CreateCell(5);
                    cell.CellStyle = lowCellStyle;
                    //  excelsheet.AddMergedRegion(new CellRangeAddress(countrow, countrow, 5, 5));
                    cell.SetCellValue(Convert.ToDouble(gettime));

                    cell = row.CreateCell(6);
                    cell.CellStyle = lowCellStyle;
                    // excelsheet.AddMergedRegion(new CellRangeAddress(countrow, countrow, 6, 6));
                    cell.SetCellValue("");

                    cell = row.CreateCell(7);
                    cell.CellStyle = lowCellStyle;
                    //  excelsheet.AddMergedRegion(new CellRangeAddress(countrow, countrow, 7, 7));
                    cell.SetCellValue("");

                    cell = row.CreateCell(8);
                    cell.CellStyle = lowCellStyle;
                    //   excelsheet.AddMergedRegion(new CellRangeAddress(countrow, countrow, 8, 8));
                    cell.SetCellValue("");

                    cell = row.CreateCell(11);
                    cell.CellStyle = lowCellStyle;
                    //  excelsheet.AddMergedRegion(new CellRangeAddress(countrow, countrow, 11, 11));
                    cell.SetCellValue("");
                    countrow = countrow + 1;
                    row = excelsheet.CreateRow(countrow);




                    //IRow a = excelsheet.GetRow(countrow - rowscount + mm_count);

                    //if (mm_count == 0)
                    //    excelsheet.AddMergedRegion(new CellRangeAddress(countrow - rowscount, rowscount + 1, 3, 3));
                    //else
                    //    excelsheet.AddMergedRegion(new CellRangeAddress(countrow - rowscount + mm_count, countrow - 1, 3, 3));
                    //cell.SetCellValue(total_requirement);
                }

                //IRow a = excelsheet.GetRow(countrow - rowscount + mm_count);
                //cell = a.CreateCell(3);
                //cell.CellStyle = spanColumnCellStyle;
                //if (mm_count == 0)
                //    excelsheet.AddMergedRegion(new CellRangeAddress(countrow - rowscount, rowscount + 1, 3, 3));
                //else
                //    excelsheet.AddMergedRegion(new CellRangeAddress(countrow - rowscount + mm_count, countrow - 1, 3, 3));
                //cell.SetCellValue(total_requirement);

                IRow b = excelsheet.GetRow(countrow - rowscount + mm_count);
                cell = b.CreateCell(9);
                cell.CellStyle = spanColumnCellStyle;
                if (mm_count == 0)
                    excelsheet.AddMergedRegion(new CellRangeAddress(countrow - rowscount, rowscount + 1, 9, 9));
                else
                    excelsheet.AddMergedRegion(new CellRangeAddress(countrow - rowscount + mm_count, countrow - 1, 9, 9));
                cell.SetCellValue(change_com);

                IRow c = excelsheet.GetRow(countrow - rowscount + mm_count);
                cell = c.CreateCell(10);
                cell.CellStyle = spanColumnCellStyle;
                if (mm_count == 0)
                    excelsheet.AddMergedRegion(new CellRangeAddress(countrow - rowscount, rowscount + 1, 10, 10));
                else
                    excelsheet.AddMergedRegion(new CellRangeAddress(countrow - rowscount + mm_count, countrow - 1, 10, 10));
                cell.SetCellValue(total_hours + " hrs");

                rowscount = rowscount + 1;

                //above to create a excel cells  to End

                //to set a next month for next loop
                if (end_month == 12)
                {
                    end_month = 1;
                    end_year++;
                }
                else
                {
                    end_month++;
                }
            }

            // To Generate the Resources Reports

            resource_ulitization(diff_month, mm, yy);
            NCR_report(Header, diff_month, mm, yy);
            quality_objective_Report(Header);
            Customer_Feedback_Report(Header);

            // To Save the excel
            string excelPath = Server.MapPath(".") + "\\Reports\\";
            string excelname = Header + "_MRM Reports_Software.xls";
            excelPath = excelPath + excelname;
            FileStream file = new FileStream(excelPath, FileMode.Create);
            hssfworkbook.Write(file);
            file.Close();

            // To download the excel its user end

            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=\"" + excelname + "\"");
            Response.TransmitFile(HttpContext.Current.Server.MapPath(".") + "\\Reports\\" + excelname);
            Response.End();
        }
    }
    //Load Logo Image
    public static int LoadImage(string path, HSSFWorkbook wb)
    {
        FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
        byte[] buffer = new byte[file.Length];
        file.Read(buffer, 0, (int)file.Length);
        return wb.AddPicture(buffer, PictureType.JPEG);
    }

    //Resource Utilization
    protected void resource_ulitization(int diff_month, int mm, string year)
    {
        Calculation();
        ISheet excelsheet = hssfworkbook.CreateSheet("Resource Utilization");
        // Set the Styles of header rows and columns
        IFont headerFont = hssfworkbook.CreateFont();
        ICellStyle headerFontStyle = hssfworkbook.CreateCellStyle();
        headerFont.Boldweight = (short)FontBoldWeight.Bold;
        headerFont.FontHeight = 14 * 14;
        headerFontStyle.FillForegroundColor = HSSFColor.LightGreen.Index;
        headerFontStyle.FillPattern = FillPattern.SolidForeground;
        headerFontStyle.Alignment = HorizontalAlignment.Center;
        headerFontStyle.VerticalAlignment = VerticalAlignment.Center;
        headerFontStyle.WrapText = true;
        headerFontStyle.SetFont(headerFont);
        // Set the border styles of header rows and columns
        headerFontStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
        headerFontStyle.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
        headerFontStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
        headerFontStyle.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
        headerFontStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
        headerFontStyle.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
        headerFontStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
        headerFontStyle.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;

        // Set the styles of content rows and columns
        IFont lowFont = hssfworkbook.CreateFont();
        ICellStyle lowCellStyle = hssfworkbook.CreateCellStyle();
        lowFont.FontHeight = 14 * 14;
        lowCellStyle.Alignment = HorizontalAlignment.Center;
        lowCellStyle.WrapText = true;
        lowCellStyle.Alignment = HorizontalAlignment.Center;
        lowCellStyle.VerticalAlignment = VerticalAlignment.Center;
        lowCellStyle.SetFont(lowFont);
        // Set the border styles of content rows and columns
        lowCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
        lowCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
        lowCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
        lowCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
        lowCellStyle.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
        lowCellStyle.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
        lowCellStyle.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
        lowCellStyle.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;

        // Set the styles of orange Main rows and columns
        IFont orangeFont = hssfworkbook.CreateFont();
        ICellStyle orangeCellStyle = hssfworkbook.CreateCellStyle();
        orangeFont.FontHeight = 14 * 14;
        orangeCellStyle.Alignment = HorizontalAlignment.Center;
        orangeCellStyle.WrapText = true;
        orangeCellStyle.Alignment = HorizontalAlignment.Center;
        orangeCellStyle.VerticalAlignment = VerticalAlignment.Center;
        orangeCellStyle.SetFont(lowFont);
        //set orange order styles of content rows and columns
        orangeCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
        orangeCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
        orangeCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
        orangeCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
        orangeCellStyle.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
        orangeCellStyle.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
        orangeCellStyle.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
        orangeCellStyle.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
        orangeCellStyle.FillForegroundColor = HSSFColor.LightOrange.Index;
        orangeCellStyle.FillPattern = FillPattern.SolidForeground;

        // Set the styles of content rows and columns
        IFont topFont = hssfworkbook.CreateFont();
        ICellStyle topCellStyle = hssfworkbook.CreateCellStyle();
        topFont.Boldweight = (short)FontBoldWeight.Bold;
        topFont.FontHeight = 14 * 14;
        topCellStyle.Alignment = HorizontalAlignment.Center;
        topCellStyle.WrapText = true;
        topCellStyle.VerticalAlignment = VerticalAlignment.Center;
        topCellStyle.SetFont(topFont);
        //styles of content rows and columns
        topCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
        topCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
        topCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
        topCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
        topCellStyle.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
        topCellStyle.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
        topCellStyle.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
        topCellStyle.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
        // set the Styles of colspan and rowspan fields

        IFont spanFont = hssfworkbook.CreateFont();
        ICellStyle spanColumnCellStyle = hssfworkbook.CreateCellStyle();
        spanFont.FontHeight = 14 * 14;
        spanColumnCellStyle.Alignment = HorizontalAlignment.Center;
        spanColumnCellStyle.WrapText = true;
        spanColumnCellStyle.Alignment = HorizontalAlignment.Center;
        spanColumnCellStyle.VerticalAlignment = VerticalAlignment.Center;
        // Set the border styles of colspan and rowspan fields
        spanColumnCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
        spanColumnCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
        spanColumnCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
        spanColumnCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
        spanColumnCellStyle.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
        spanColumnCellStyle.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
        spanColumnCellStyle.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
        spanColumnCellStyle.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
        spanColumnCellStyle.SetFont(spanFont);
        spanColumnCellStyle.FillForegroundColor = HSSFColor.LightYellow.Index;
        spanColumnCellStyle.FillPattern = FillPattern.SolidForeground;

        // Set the different style rotation format text vertical alignment

        IFont rotateFont = hssfworkbook.CreateFont();
        ICellStyle rotateFontStyle = hssfworkbook.CreateCellStyle();
        rotateFont.Boldweight = (short)FontBoldWeight.Bold;
        rotateFont.FontHeight = 15 * 15;
        rotateFontStyle.Alignment = HorizontalAlignment.Center;
        rotateFontStyle.VerticalAlignment = VerticalAlignment.Center;
        rotateFontStyle.SetFont(rotateFont);
        rotateFontStyle.Rotation = 90;
        rotateFontStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
        rotateFontStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
        rotateFontStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
        rotateFontStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
        rotateFontStyle.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
        rotateFontStyle.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
        rotateFontStyle.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
        rotateFontStyle.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;

        ICell cell;
        excelsheet.DisplayGridlines = true;
        IRow row;

        // Create a row(0)

        row = excelsheet.CreateRow(0);
        row.Height = 500;

        // set the image of column to selected range

        //cell = row.CreateCell(0);
        //cell.CellStyle = orangeCellStyle;
        //excelsheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 0));
        //var patriarch = excelsheet.CreateDrawingPatriarch();
        //HSSFClientAnchor anchor;
        //anchor = new HSSFClientAnchor(0, 0, 0, 0, 0, 0, 0, 0);
        //anchor.AnchorType = 1;
        //string path = Server.MapPath("~/");
        //path = path + @"assets\img\logo.png";
        //var picture = patriarch.CreatePicture(anchor, LoadImage(path, hssfworkbook));
        //picture.Resize();
        cell = row.CreateCell(0);
        // cell.CellStyle = lowCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(0, 1, 0, 0));
        var patriarch = excelsheet.CreateDrawingPatriarch();
        HSSFClientAnchor anchor;
        anchor = new HSSFClientAnchor(0, 0, 0, 0, 0, 0, 0, 55);
        anchor.AnchorType = 2;
        string path = Server.MapPath("~/") + @"assets/img/logo.png";
        var picture = patriarch.CreatePicture(anchor, LoadImage(path, hssfworkbook));
        picture.Resize();



        cell = row.CreateCell(1);
        cell.CellStyle = orangeCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(0, 0, 1, 1 + diff_month));
        cell.SetCellValue("Resource Utilization");
        cell = row.CreateCell(11);

        excelsheet.SetColumnWidth(0, 6000);
        excelsheet.SetColumnWidth(1, 10000);

        // Create a row(1)

        row = excelsheet.CreateRow(1);
        row.Height = 500;

        cell = row.CreateCell(0);
        cell.CellStyle = spanColumnCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 0));
        cell.SetCellValue("Activities");

        cell = row.CreateCell(1);
        cell.CellStyle = spanColumnCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(1, 1, diff_month, diff_month));
        cell.SetCellValue("Guidelines to be followed");
        for (int i = 0; i < diff_month; i++)
        {
            int k = i + 2;
            cell = row.CreateCell(k);
            cell.CellStyle = spanColumnCellStyle;
            excelsheet.AddMergedRegion(new CellRangeAddress(1, 1, k, k));
            if (i == 0) mm = mm - diff_month + 1;
            if (mm == 1)
                cell.SetCellValue("Jan'" + Convert.ToInt32(yy));
            else if (mm == 2)
                cell.SetCellValue("Feb'" + Convert.ToInt32(yy));
            else if (mm == 3)
                cell.SetCellValue("Mar'" + Convert.ToInt32(yy));
            else if (mm == 4)
                cell.SetCellValue("Apr'" + Convert.ToInt32(yy));
            else if (mm == 5)
                cell.SetCellValue("May'" + Convert.ToInt32(yy));
            else if (mm == 6)
                cell.SetCellValue("Jun'" + Convert.ToInt32(yy));
            else if (mm == 7)
                cell.SetCellValue("Jul'" + Convert.ToInt32(yy));
            else if (mm == 8)
                cell.SetCellValue("Aug'" + Convert.ToInt32(yy));
            else if (mm == 9)
                cell.SetCellValue("Sep'" + Convert.ToInt32(yy));
            else if (mm == 10)
                cell.SetCellValue("Oct'" + Convert.ToInt32(yy));
            else if (mm == 11)
                cell.SetCellValue("Nov'" + Convert.ToInt32(yy));
            else if (mm == 12)
                cell.SetCellValue("Dec'" + Convert.ToInt32(yy));
            mm++;
        }
        // Create a row(2)
        row = excelsheet.CreateRow(2);
        row.Height = 500;

        cell = row.CreateCell(0);
        cell.CellStyle = spanColumnCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 0));
        cell.SetCellValue("Total Manshift");

        cell = row.CreateCell(1);
        cell.CellStyle = headerFontStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(1, 1, diff_month, diff_month));
        cell.SetCellValue("Number of working days for the Month X \n Number of employees in the team");

        for (int i = 1; i <= diff_month; i++)
        {
            int k = i + 1;
            cell = row.CreateCell(k);
            cell.CellStyle = lowCellStyle;
            excelsheet.AddMergedRegion(new CellRangeAddress(1, 1, k, k));
            tot_manshift[i] = sun_day[i] * man_pow[i];
            cell.SetCellValue(tot_manshift[i]);
        }
        // Create a row(3)
        row = excelsheet.CreateRow(3);
        row.Height = 500;

        cell = row.CreateCell(0);
        cell.CellStyle = spanColumnCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 0));
        cell.SetCellValue("Total Manhours");

        cell = row.CreateCell(1);
        cell.CellStyle = headerFontStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(1, 1, diff_month, diff_month));
        cell.SetCellValue("Total Manshift X 7:30 hrs");

        for (int i = 1; i <= diff_month; i++)
        {
            int k = i + 1;
            cell = row.CreateCell(k);
            cell.CellStyle = lowCellStyle;
            excelsheet.AddMergedRegion(new CellRangeAddress(1, 1, k - 1, k - 1));
            cell.SetCellValue(tot_manshift[i] * 7.5);
        }
        // Create a row(4)
        row = excelsheet.CreateRow(4);
        row.Height = 500;

        cell = row.CreateCell(0);
        cell.CellStyle = spanColumnCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 0));
        cell.SetCellValue("Available Manshift");

        cell = row.CreateCell(1);
        cell.CellStyle = headerFontStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(1, 1, diff_month, diff_month));
        cell.SetCellValue("Available Manshift is calculated after reducing \n the absenteeism from the Total Manshift\n=(Total Manshift - Absentism in the \n team)+Extension");

        for (int i = 1; i <= diff_month; i++)
        {
            int k = i + 1;
            cell = row.CreateCell(k);
            cell.CellStyle = lowCellStyle;
            excelsheet.AddMergedRegion(new CellRangeAddress(1, 1, k, k));
            avail_manshift[i] = (tot_manshift[i] - leaves[i]) + (Ex_time[i] / 7.5);
            cell.SetCellValue(avail_manshift[i]);
        }
        // Create a row(5)
        row = excelsheet.CreateRow(5);
        row.Height = 500;

        cell = row.CreateCell(0);
        cell.CellStyle = spanColumnCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 0));
        cell.SetCellValue("Available Manhours");

        cell = row.CreateCell(1);
        cell.CellStyle = headerFontStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(1, 1, diff_month, diff_month));
        cell.SetCellValue("Available Manshift X 7:30 hrs");

        for (int i = 1; i <= diff_month; i++)
        {
            int k = i + 1;
            cell = row.CreateCell(k);
            cell.CellStyle = lowCellStyle;
            excelsheet.AddMergedRegion(new CellRangeAddress(1, 1, k, k));
            cell.SetCellValue(avail_manshift[i] * 7.5);
        }
        // Create a row(6)
        row = excelsheet.CreateRow(6);
        row.Height = 500;

        cell = row.CreateCell(0);
        cell.CellStyle = headerFontStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(6, 6, 0, 1 + diff_month));
        cell.SetCellValue("Manhours Deployed for Actual Production");

        // Create a row(7)
        row = excelsheet.CreateRow(7);
        row.Height = 500;

        cell = row.CreateCell(0);
        cell.CellStyle = spanColumnCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(7, 19, 0, 0));
        cell.SetCellValue("Manhours Deployed for Actual Production");

        cell = row.CreateCell(1);
        cell.CellStyle = headerFontStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(7, 7, 1, 1));
        cell.SetCellValue("Manhours deployed in the projects should match with the manhours indicated in the PPM");

        for (int i = 1; i <= diff_month; i++)
        {
            int k = i + 1;
            cell = row.CreateCell(k);
            cell.CellStyle = lowCellStyle;
            excelsheet.AddMergedRegion(new CellRangeAddress(7, 7, k, k));
            cell.SetCellValue(total_hrs[i]);
        }
        // Create a row(8)
        row = excelsheet.CreateRow(8);
        row.Height = 500;

        cell = row.CreateCell(1);
        cell.CellStyle = headerFontStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(8, 8, 1, 1));
        cell.SetCellValue("Other than 1st Pass Manhour");

        for (int i = 1; i <= diff_month; i++)
        {
            pass_man_hours[i] = 0;
            int k = i + 1;
            cell = row.CreateCell(k);
            cell.CellStyle = lowCellStyle;
            excelsheet.AddMergedRegion(new CellRangeAddress(8, 8, k, k));
            cell.SetCellValue(pass_man_hours[i]);
        }
        // Create a row(9)
        row = excelsheet.CreateRow(9);
        row.Height = 500;

        cell = row.CreateCell(1);
        cell.CellStyle = headerFontStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(9, 9, 1, 1));
        cell.SetCellValue("Manhours Deployed for Actual Production of Other Departments Project");

        for (int i = 1; i <= diff_month; i++)
        {
            pass_man_hours[i] = 0;
            int k = i + 1;
            cell = row.CreateCell(k);
            cell.CellStyle = lowCellStyle;
            excelsheet.AddMergedRegion(new CellRangeAddress(9, 9, k, k));
            cell.SetCellValue(pass_man_hours[i]);
        }
        // Create a row(10)
        row = excelsheet.CreateRow(10);
        row.Height = 800;

        cell = row.CreateCell(1);
        cell.CellStyle = headerFontStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(10, 10, 1, 1 + diff_month));
        cell.SetCellValue("Non-Production Activities to be taken into consideration \n as Production hrs (Project Training, Project Meeting, \n Trouble Shooting, Communication, Documentation)");
        // Create a row(11)
        row = excelsheet.CreateRow(11);
        row.Height = 500;

        cell = row.CreateCell(1);
        cell.CellStyle = headerFontStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(11, 11, 1, 1));
        cell.SetCellValue("New Project Training/Meeting");

        for (int i = 1; i <= diff_month; i++)
        {
            int k = i + 1;
            cell = row.CreateCell(k);
            cell.CellStyle = lowCellStyle;
            excelsheet.AddMergedRegion(new CellRangeAddress(11, 11, k, k));
            cell.SetCellValue(new_proj[i]);
        }
        // Create a row(12)
        row = excelsheet.CreateRow(12);
        row.Height = 500;

        cell = row.CreateCell(1);
        cell.CellStyle = headerFontStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(12, 12, 1, 1));
        cell.SetCellValue("Downloading/Uploading");

        for (int i = 1; i <= diff_month; i++)
        {
            int k = i + 1;
            cell = row.CreateCell(k);
            cell.CellStyle = lowCellStyle;
            excelsheet.AddMergedRegion(new CellRangeAddress(12, 12, k, k));
            cell.SetCellValue(downl[i]);
        }
        // Create a row(13)
        row = excelsheet.CreateRow(13);
        row.Height = 500;

        cell = row.CreateCell(1);
        cell.CellStyle = headerFontStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(13, 13, 1, 1));
        cell.SetCellValue("Practice Before Production");

        for (int i = 1; i <= diff_month; i++)
        {
            int k = i + 1;
            cell = row.CreateCell(k);
            cell.CellStyle = lowCellStyle;
            excelsheet.AddMergedRegion(new CellRangeAddress(13, 13, k, k));
            cell.SetCellValue(analysis[i]);
        }
        // Create a row(14)
        row = excelsheet.CreateRow(14);
        row.Height = 500;

        cell = row.CreateCell(1);
        cell.CellStyle = headerFontStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(14, 14, 1, 1));
        cell.SetCellValue("Project Meeting - Feedback Session Conducted for the Team");

        for (int i = 1; i <= diff_month; i++)
        {
            int k = i + 1;
            cell = row.CreateCell(k);
            cell.CellStyle = lowCellStyle;
            excelsheet.AddMergedRegion(new CellRangeAddress(14, 14, k, k));
            cell.SetCellValue(meet_time[i]);
        }
        // Create a row(15)
        row = excelsheet.CreateRow(15);
        row.Height = 500;

        cell = row.CreateCell(1);
        cell.CellStyle = headerFontStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(15, 15, 1, 1));
        cell.SetCellValue("Time Spent for trouble shooting");

        for (int i = 1; i <= diff_month; i++)
        {
            int k = i + 1;
            cell = row.CreateCell(k);
            cell.CellStyle = lowCellStyle;
            excelsheet.AddMergedRegion(new CellRangeAddress(15, 15, k, k));
            cell.SetCellValue(troubleshooting[i]);
        }
        // Create a row(16)
        row = excelsheet.CreateRow(16);
        row.Height = 500;

        cell = row.CreateCell(1);
        cell.CellStyle = headerFontStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(16, 16, 1, 1));
        cell.SetCellValue("Time Spent on Mail communication, Reading Messages.");

        for (int i = 1; i <= diff_month; i++)
        {
            int k = i + 1;
            cell = row.CreateCell(k);
            cell.CellStyle = lowCellStyle;
            excelsheet.AddMergedRegion(new CellRangeAddress(16, 16, k, k));
            cell.SetCellValue(communication[i]);
        }
        // Create a row(17)
        row = excelsheet.CreateRow(17);
        row.Height = 500;

        cell = row.CreateCell(1);
        cell.CellStyle = headerFontStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(17, 17, 1, 1));
        cell.SetCellValue("On Duty - Visit to other location/vendor.");

        for (int i = 1; i <= diff_month; i++)
        {
            int k = i + 1;
            cell = row.CreateCell(k);
            cell.CellStyle = lowCellStyle;
            excelsheet.AddMergedRegion(new CellRangeAddress(17, 17, k, k));
            cell.SetCellValue(on_duty[i]);
        }
        // Create a row(18)
        row = excelsheet.CreateRow(18);
        row.Height = 500;

        cell = row.CreateCell(1);
        cell.CellStyle = headerFontStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(18, 18, 1, 1));
        cell.SetCellValue("Documentation if any");

        for (int i = 1; i <= diff_month; i++)
        {
            int k = i + 1;
            cell = row.CreateCell(k);
            cell.CellStyle = lowCellStyle;
            excelsheet.AddMergedRegion(new CellRangeAddress(18, 18, k, k));
            cell.SetCellValue(documentation[i]);
        }
        // Create a row(19)
        row = excelsheet.CreateRow(19);
        row.Height = 500;

        cell = row.CreateCell(1);
        cell.CellStyle = headerFontStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(19, 19, 1, 1));
        cell.SetCellValue("Shift Extension hours");

        for (int i = 1; i <= diff_month; i++)
        {
            int k = i + 1;
            cell = row.CreateCell(k);
            cell.CellStyle = lowCellStyle;
            excelsheet.AddMergedRegion(new CellRangeAddress(19, 19, k, k));
            cell.SetCellValue(Ex_time[i]);
        }

        // Create a row(20)
        row = excelsheet.CreateRow(20);
        row.Height = 500;

        cell = row.CreateCell(0);
        cell.CellStyle = spanColumnCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(20, 20, 0, 1));
        cell.SetCellValue("Resource Utilization For Production In hrs");

        for (int i = 1; i <= diff_month; i++)
        {
            int k = i + 1;
            cell = row.CreateCell(k);
            cell.CellStyle = lowCellStyle;
            excelsheet.AddMergedRegion(new CellRangeAddress(20, 20, k, k));
            cell.SetCellValue(total_hrs[i] + pass_man_hours[i] + new_proj[i] + downl[i] + analysis[i] + meet_time[i] + troubleshooting[i] + communication[i] + on_duty[i] + documentation[i] + Ex_time[i]);
        }

        // Create a row(21)
        row = excelsheet.CreateRow(21);
        row.Height = 500;

        cell = row.CreateCell(0);
        cell.CellStyle = spanColumnCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(21, 21, 0, 0));
        cell.SetCellValue("Resource Utilization \n For Production In %");

        cell = row.CreateCell(1);
        cell.CellStyle = spanColumnCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(21, 21, 1, 1));
        cell.SetCellValue("Production (C5+C6)");

        for (int i = 1; i <= diff_month; i++)
        {
            production[i] = Convert.ToInt32(((total_hrs[i] + pass_man_hours[i] + new_proj[i] + downl[i] + analysis[i] + meet_time[i] + troubleshooting[i] + communication[i] + on_duty[i] + documentation[i] + Ex_time[i]) / avail_manshift[i] * 7.5));
            int k = i + 1;
            cell = row.CreateCell(k);
            cell.CellStyle = spanColumnCellStyle;
            excelsheet.AddMergedRegion(new CellRangeAddress(21, 21, k, k));
            cell.SetCellValue(production[i] + "%");
        }

        // Create a row(22)
        row = excelsheet.CreateRow(22);
        row.Height = 500;

        cell = row.CreateCell(0);
        cell.CellStyle = headerFontStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(22, 22, 0, 1 + diff_month));
        cell.SetCellValue("Manhours Deployed for Non-Production Activities");

        // Create a row(23)
        row = excelsheet.CreateRow(23);
        row.Height = 500;

        cell = row.CreateCell(0);
        cell.CellStyle = spanColumnCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(23, 28, 0, 0));
        cell.SetCellValue("Manhours Deployed for Non-Production Activities (Idle Activities, RND)");

        cell = row.CreateCell(1);
        cell.CellStyle = headerFontStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(23, 23, 1, 1));
        cell.SetCellValue("Idle Activities: System Breakdown, Power Shutdown, Internet/Intranet conectivity Failure");

        for (int i = 1; i <= diff_month; i++)
        {
            int k = i + 1;
            cell = row.CreateCell(k);
            cell.CellStyle = lowCellStyle;
            excelsheet.AddMergedRegion(new CellRangeAddress(23, 23, k, k));
            cell.SetCellValue(shutdown[i]);
        }

        // Create a row(24)
        row = excelsheet.CreateRow(24);
        row.Height = 500;

        cell = row.CreateCell(1);
        cell.CellStyle = headerFontStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(24, 24, 1, 1));
        cell.SetCellValue("No Input");

        for (int i = 1; i <= diff_month; i++)
        {
            int k = i + 1;
            cell = row.CreateCell(k);
            cell.CellStyle = lowCellStyle;
            excelsheet.AddMergedRegion(new CellRangeAddress(24, 24, k, k));
            cell.SetCellValue(no_input[i]);
        }

        // Create a row(25)
        row = excelsheet.CreateRow(25);
        row.Height = 500;

        cell = row.CreateCell(1);
        cell.CellStyle = headerFontStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(25, 25, 1, 1));
        cell.SetCellValue("RND - Sample Analysis and  Creation");

        for (int i = 1; i <= diff_month; i++)
        {
            int k = i + 1;
            cell = row.CreateCell(k);
            cell.CellStyle = lowCellStyle;
            excelsheet.AddMergedRegion(new CellRangeAddress(25, 25, k, k));
            cell.SetCellValue(rnd_sample[i]);
        }

        // Create a row(26)
        row = excelsheet.CreateRow(26);
        row.Height = 500;

        cell = row.CreateCell(1);
        cell.CellStyle = headerFontStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(26, 26, 1, 1));
        cell.SetCellValue("New Technology Learning/Training to adapt new projects");

        for (int i = 1; i <= diff_month; i++)
        {
            int k = i + 1;
            cell = row.CreateCell(k);
            cell.CellStyle = lowCellStyle;
            excelsheet.AddMergedRegion(new CellRangeAddress(26, 26, k, k));
            cell.SetCellValue(new_technology[i]);
        }

        // Create a row(27)
        row = excelsheet.CreateRow(27);
        row.Height = 500;

        cell = row.CreateCell(1);
        cell.CellStyle = headerFontStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(27, 27, 1, 1));
        cell.SetCellValue("General Meetings (MRM, ISO) and Skill Enhancement Trainings");

        for (int i = 1; i <= diff_month; i++)
        {
            int k = i + 1;
            cell = row.CreateCell(k);
            cell.CellStyle = lowCellStyle;
            excelsheet.AddMergedRegion(new CellRangeAddress(27, 27, k, k));
            cell.SetCellValue(general[i]);
        }

        // Create a row(28)
        row = excelsheet.CreateRow(28);
        row.Height = 500;

        cell = row.CreateCell(1);
        cell.CellStyle = headerFontStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(28, 28, 1, 1));
        cell.SetCellValue("Shift Extension hours");

        for (int i = 1; i <= diff_month; i++)
        {
            int k = i + 1;
            cell = row.CreateCell(k);
            cell.CellStyle = lowCellStyle;
            excelsheet.AddMergedRegion(new CellRangeAddress(28, 28, k, k));
            cell.SetCellValue(Ex_time[i]);
        }

        // Create a row(29)
        row = excelsheet.CreateRow(29);
        row.Height = 500;

        cell = row.CreateCell(0);
        cell.CellStyle = spanColumnCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(29, 29, 0, 1));
        cell.SetCellValue("Resource Utilization For Non-Production In hrs");

        for (int i = 1; i <= diff_month; i++)
        {
            int k = i + 1;
            cell = row.CreateCell(k);
            cell.CellStyle = lowCellStyle;
            excelsheet.AddMergedRegion(new CellRangeAddress(29, 29, k, k));
            cell.SetCellValue(shutdown[i] + no_input[i] + rnd_sample[i] + new_technology[i] + general[i] + Ex_time[i]);
        }

        // Create a row(30)
        row = excelsheet.CreateRow(30);
        row.Height = 500;

        cell = row.CreateCell(0);
        cell.CellStyle = spanColumnCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(30, 30, 0, 0));
        cell.SetCellValue("Resource Utilization For Non-Production In %");

        cell = row.CreateCell(1);
        cell.CellStyle = spanColumnCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(30, 30, 1, 1));
        cell.SetCellValue("Balance (if any) have to be indicated separately");

        for (int i = 1; i <= diff_month; i++)
        {
            non_production[i] = Convert.ToInt32(((shutdown[i] + no_input[i] + rnd_sample[i] + new_technology[i] + general[i] + Ex_time[i]) / avail_manshift[i] * 7.5));
            int k = i + 1;
            cell = row.CreateCell(k);
            cell.CellStyle = spanColumnCellStyle;
            excelsheet.AddMergedRegion(new CellRangeAddress(30, 30, k, k));
            cell.SetCellValue(non_production[i] + "%");
        }

        // Create a row(31)
        row = excelsheet.CreateRow(31);
        row.Height = 500;

        cell = row.CreateCell(0);
        cell.CellStyle = orangeCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(31, 31, 0, 0));
        cell.SetCellValue("Total %");

        cell = row.CreateCell(1);
        cell.CellStyle = orangeCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(31, 31, 1, 1));
        cell.SetCellValue("");

        for (int i = 1; i <= diff_month; i++)
        {
            prod_and_nonprod[i] = production[i] + non_production[i];
            int k = i + 1;
            cell = row.CreateCell(k);
            cell.CellStyle = orangeCellStyle;
            excelsheet.AddMergedRegion(new CellRangeAddress(31, 31, k, k));
            cell.SetCellValue(prod_and_nonprod[i] + "%");
        }
    }
    protected void NCR_report(string header, int diff_month, int month, string year)
    {
        ISheet excelsheet = hssfworkbook.CreateSheet("NCR Summary");

        // Set the styles of content rows and columns
        IFont lowFont = hssfworkbook.CreateFont();
        ICellStyle lowCellStyle = hssfworkbook.CreateCellStyle();
        lowFont.FontHeight = 14 * 14;
        lowCellStyle.Alignment = HorizontalAlignment.Center;
        lowCellStyle.WrapText = true;
        lowCellStyle.VerticalAlignment = VerticalAlignment.Center;
        lowCellStyle.SetFont(lowFont);
        // Set the border styles of content rows and columns
        lowCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
        lowCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
        lowCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
        lowCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
        lowCellStyle.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
        lowCellStyle.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
        lowCellStyle.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
        lowCellStyle.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;

        // Set the styles of content rows and columns
        IFont lowFont1 = hssfworkbook.CreateFont();
        ICellStyle lowCellStyle1 = hssfworkbook.CreateCellStyle();
        lowFont1.FontHeight = 14 * 14;
        lowCellStyle1.Alignment = HorizontalAlignment.Left;
        lowCellStyle1.WrapText = true;
        lowCellStyle1.VerticalAlignment = VerticalAlignment.Center;
        lowCellStyle1.SetFont(lowFont1);
        //styles of content rows and columns
        lowCellStyle1.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
        lowCellStyle1.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
        lowCellStyle1.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
        lowCellStyle1.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
        lowCellStyle1.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
        lowCellStyle1.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
        lowCellStyle1.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
        lowCellStyle1.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;

        // set the Styles of colspan and rowspan fields

        IFont spanFont = hssfworkbook.CreateFont();
        ICellStyle spanColumnCellStyle = hssfworkbook.CreateCellStyle();
        spanFont.Boldweight = (short)FontBoldWeight.Bold;
        spanFont.FontHeight = 14 * 14;
        spanColumnCellStyle.Alignment = HorizontalAlignment.Center;
        spanColumnCellStyle.WrapText = true;
        spanColumnCellStyle.VerticalAlignment = VerticalAlignment.Center;
        // Set the border styles of colspan and rowspan fields
        spanColumnCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
        spanColumnCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
        spanColumnCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
        spanColumnCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
        spanColumnCellStyle.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
        spanColumnCellStyle.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
        spanColumnCellStyle.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
        spanColumnCellStyle.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
        spanColumnCellStyle.SetFont(spanFont);
        spanColumnCellStyle.FillForegroundColor = HSSFColor.LightYellow.Index;
        spanColumnCellStyle.FillPattern = FillPattern.SolidForeground;

        ICell cell;
        excelsheet.DisplayGridlines = true;
        IRow row;

        // Create a row(0)

        row = excelsheet.CreateRow(0);
        row.Height = 750;

        // set the image of column to selected range

        cell = row.CreateCell(0);
        cell.CellStyle = lowCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 1));
        var patriarch = excelsheet.CreateDrawingPatriarch();
        HSSFClientAnchor anchor;
        anchor = new HSSFClientAnchor(0, 0, 0, 0, 0, 0, 0, 0);
        anchor.AnchorType = 2;
        string path = Server.MapPath("~/");
        path = path + @"assets\img\logo.png";
        var picture = patriarch.CreatePicture(anchor, LoadImage(path, hssfworkbook));
        picture.Resize();

        cell = row.CreateCell(1);
        cell.CellStyle = lowCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(0, 0, 2, 4));
        cell.SetCellValue("");

        excelsheet.SetColumnWidth(0, 2000);
        excelsheet.SetColumnWidth(1, 2000);
        excelsheet.SetColumnWidth(2, 6500);
        excelsheet.SetColumnWidth(3, 6500);
        excelsheet.SetColumnWidth(4, 6500);

        // Create a row(1)

        row = excelsheet.CreateRow(1);
        row.Height = 500;

        cell = row.CreateCell(2);
        cell.CellStyle = spanColumnCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(1, 1, 2, 4));
        cell.SetCellValue(header + "NON-COMPLIANCE CONSOLIDATED REPORT (IQA)");

        // Create a row(2)

        row = excelsheet.CreateRow(2);

        cell = row.CreateCell(2);
        cell.CellStyle = spanColumnCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(2, 2, 2, 2));
        cell.SetCellValue("Clause");

        cell = row.CreateCell(3);
        cell.CellStyle = spanColumnCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(2, 2, 3, 3));
        cell.SetCellValue("Software");

        cell = row.CreateCell(4);
        cell.CellStyle = spanColumnCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(2, 2, 4, 4));
        cell.SetCellValue("Total");

        // Create a row(3)

        row = excelsheet.CreateRow(3);

        cell = row.CreateCell(2);
        cell.CellStyle = lowCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(3, 3, 2, 2));
        cell.SetCellValue("4");

        cell = row.CreateCell(3);
        cell.CellStyle = lowCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(3, 3, 3, 3));
        cell.SetCellValue("");

        cell = row.CreateCell(4);
        cell.CellStyle = lowCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(3, 3, 4, 4));
        cell.SetCellValue("");


        // Create a row(4)

        row = excelsheet.CreateRow(4);

        cell = row.CreateCell(2);
        cell.CellStyle = lowCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(4, 4, 2, 2));
        cell.SetCellValue("5");

        cell = row.CreateCell(3);
        cell.CellStyle = lowCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(4, 4, 3, 3));
        cell.SetCellValue("");

        cell = row.CreateCell(4);
        cell.CellStyle = lowCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(4, 4, 4, 4));
        cell.SetCellValue("");

        // Create a row(5)

        row = excelsheet.CreateRow(5);

        cell = row.CreateCell(2);
        cell.CellStyle = lowCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(5, 5, 2, 2));
        cell.SetCellValue("6");

        cell = row.CreateCell(3);
        cell.CellStyle = lowCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(5, 5, 3, 3));
        cell.SetCellValue("");

        cell = row.CreateCell(4);
        cell.CellStyle = lowCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(5, 5, 4, 4));
        cell.SetCellValue("");

        // Create a row(6)

        row = excelsheet.CreateRow(6);

        cell = row.CreateCell(2);
        cell.CellStyle = lowCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(6, 6, 2, 2));
        cell.SetCellValue("7");

        cell = row.CreateCell(3);
        cell.CellStyle = lowCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(6, 6, 3, 3));
        cell.SetCellValue("");

        cell = row.CreateCell(4);
        cell.CellStyle = lowCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(6, 6, 4, 4));
        cell.SetCellValue("");

        // Create a row(7)

        row = excelsheet.CreateRow(7);

        cell = row.CreateCell(2);
        cell.CellStyle = lowCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(7, 7, 2, 2));
        cell.SetCellValue("8");

        cell = row.CreateCell(3);
        cell.CellStyle = lowCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(7, 7, 3, 3));
        cell.SetCellValue("");

        cell = row.CreateCell(4);
        cell.CellStyle = lowCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(7, 7, 4, 4));
        cell.SetCellValue("");

        // Create a row(8)

        row = excelsheet.CreateRow(8);

        cell = row.CreateCell(2);
        cell.CellStyle = spanColumnCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(8, 8, 2, 2));
        cell.SetCellValue("Total");

        cell = row.CreateCell(3);
        cell.CellStyle = spanColumnCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(8, 8, 3, 3));
        cell.SetCellValue("");

        cell = row.CreateCell(4);
        cell.CellStyle = spanColumnCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(8, 8, 4, 4));
        cell.SetCellValue("");

        // Create a row(12)

        row = excelsheet.CreateRow(12);

        cell = row.CreateCell(2);
        cell.CellStyle = lowCellStyle1;
        excelsheet.AddMergedRegion(new CellRangeAddress(12, 12, 2, 2));
        cell.SetCellValue("Clause :");

        // Create a row(13)

        row = excelsheet.CreateRow(13);

        cell = row.CreateCell(2);
        cell.CellStyle = lowCellStyle1;
        excelsheet.AddMergedRegion(new CellRangeAddress(13, 13, 2, 2));
        cell.SetCellValue("4-General Requirements");

        // Create a row(14)

        row = excelsheet.CreateRow(14);

        cell = row.CreateCell(2);
        cell.CellStyle = lowCellStyle1;
        excelsheet.AddMergedRegion(new CellRangeAddress(14, 14, 2, 2));
        cell.SetCellValue("5-Management Responsibilities");

        // Create a row(15)

        row = excelsheet.CreateRow(15);

        cell = row.CreateCell(2);
        cell.CellStyle = lowCellStyle1;
        excelsheet.AddMergedRegion(new CellRangeAddress(15, 15, 2, 2));
        cell.SetCellValue("6-Resource Management");

        // Create a row(16)

        row = excelsheet.CreateRow(16);

        cell = row.CreateCell(2);
        cell.CellStyle = lowCellStyle1;
        excelsheet.AddMergedRegion(new CellRangeAddress(16, 16, 2, 2));
        cell.SetCellValue("7-Product Realization");

        // Create a row(17)

        row = excelsheet.CreateRow(17);

        cell = row.CreateCell(2);
        cell.CellStyle = lowCellStyle1;
        excelsheet.AddMergedRegion(new CellRangeAddress(17, 17, 2, 2));
        cell.SetCellValue("8-Measurement, Analysis and Improvements");
    }
    protected void quality_objective_Report(string header)
    {
        ISheet excelsheet = hssfworkbook.CreateSheet("Quality Objective");

        // Set the styles of content rows and columns
        IFont lowFont = hssfworkbook.CreateFont();
        ICellStyle lowCellStyle = hssfworkbook.CreateCellStyle();
        lowFont.FontHeight = 14 * 14;
        lowCellStyle.Alignment = HorizontalAlignment.Center;
        lowCellStyle.WrapText = true;
        lowCellStyle.VerticalAlignment = VerticalAlignment.Center;
        lowCellStyle.SetFont(lowFont);
        // Set the border styles of content rows and columns
        lowCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
        lowCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
        lowCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
        lowCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
        lowCellStyle.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
        lowCellStyle.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
        lowCellStyle.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
        lowCellStyle.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;

        // Set the styles of content rows and columns
        IFont topFont = hssfworkbook.CreateFont();
        ICellStyle topCellStyle = hssfworkbook.CreateCellStyle();
        topFont.Boldweight = (short)FontBoldWeight.Bold;
        topFont.FontHeight = 14 * 14;
        topCellStyle.Alignment = HorizontalAlignment.Center;
        topCellStyle.WrapText = true;
        topCellStyle.VerticalAlignment = VerticalAlignment.Center;
        topCellStyle.SetFont(topFont);
        //styles of content rows and columns
        topCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
        topCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
        topCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
        topCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
        topCellStyle.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
        topCellStyle.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
        topCellStyle.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
        topCellStyle.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;

        // set the Styles of colspan and rowspan fields

        IFont spanFont = hssfworkbook.CreateFont();
        ICellStyle spanColumnCellStyle = hssfworkbook.CreateCellStyle();
        spanFont.Boldweight = (short)FontBoldWeight.Bold;
        spanFont.FontHeight = 14 * 14;
        spanColumnCellStyle.Alignment = HorizontalAlignment.Center;
        spanColumnCellStyle.WrapText = true;
        spanColumnCellStyle.VerticalAlignment = VerticalAlignment.Center;
        // Set the border styles of colspan and rowspan fields
        spanColumnCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
        spanColumnCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
        spanColumnCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
        spanColumnCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
        spanColumnCellStyle.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
        spanColumnCellStyle.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
        spanColumnCellStyle.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
        spanColumnCellStyle.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
        spanColumnCellStyle.SetFont(spanFont);
        spanColumnCellStyle.FillForegroundColor = HSSFColor.LightYellow.Index;
        spanColumnCellStyle.FillPattern = FillPattern.SolidForeground;

        ICell cell;
        excelsheet.DisplayGridlines = true;
        IRow row;

        // Create a row(0)

        row = excelsheet.CreateRow(0);
        row.Height = 500;

        excelsheet.SetColumnWidth(0, 3000);
        excelsheet.SetColumnWidth(1, 3500);
        excelsheet.SetColumnWidth(2, 4500);
        excelsheet.SetColumnWidth(3, 3500);
        excelsheet.SetColumnWidth(4, 7000);
        excelsheet.SetColumnWidth(5, 3000);
        excelsheet.SetColumnWidth(6, 7000);

        // set the image of column to selected range

        cell = row.CreateCell(0);
        cell.CellStyle = topCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(0, 1, 0, 1));
        var patriarch = excelsheet.CreateDrawingPatriarch();
        HSSFClientAnchor anchor;
        anchor = new HSSFClientAnchor(0, 0, 0, 0, 0, 0, 0, 0);
        anchor.AnchorType = 2;
        string path = Server.MapPath("~/");
        path = path + @"assets\img\logo.png";
        var picture = patriarch.CreatePicture(anchor, LoadImage(path, hssfworkbook));
        picture.Resize();

        cell = row.CreateCell(2);
        cell.CellStyle = topCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(0, 0, 2, 4));
        cell.SetCellValue("PMIS");

        cell = row.CreateCell(5);
        cell.CellStyle = topCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(0, 0, 5, 5));
        cell.SetCellValue("TM02");

        cell = row.CreateCell(6);
        cell.CellStyle = topCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(0, 0, 6, 6));
        cell.SetCellValue("Version No.: 1");

        // Create a row(1)

        row = excelsheet.CreateRow(1);
        row.Height = 500;

        cell = row.CreateCell(2);
        cell.CellStyle = topCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(1, 1, 2, 4));
        cell.SetCellValue("QUALITY OBJECTIVES - Software");

        cell = row.CreateCell(5);
        cell.CellStyle = topCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(0, 0, 5, 5));
        cell.SetCellValue("MR");

        cell = row.CreateCell(6);
        cell.CellStyle = topCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(0, 0, 6, 6));
        cell.SetCellValue("Date: " + DateTime.Now.Date.ToShortDateString());

        // Create a row(2)

        row = excelsheet.CreateRow(2);
        row.Height = 500;

        cell = row.CreateCell(0);
        cell.CellStyle = spanColumnCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(2, 2, 0, 0));
        cell.SetCellValue("S.NO");

        cell = row.CreateCell(1);
        cell.CellStyle = spanColumnCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(2, 2, 1, 1));
        cell.SetCellValue("Objective");

        cell = row.CreateCell(2);
        cell.CellStyle = spanColumnCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(2, 2, 2, 2));
        cell.SetCellValue("Quarter");

        cell = row.CreateCell(3);
        cell.CellStyle = spanColumnCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(2, 2, 3, 3));
        cell.SetCellValue("Target");

        cell = row.CreateCell(4);
        cell.CellStyle = spanColumnCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(2, 2, 4, 4));
        cell.SetCellValue("Actual Status");

        cell = row.CreateCell(5);
        cell.CellStyle = spanColumnCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(2, 2, 5, 6));
        cell.SetCellValue("Action Plan");

        // Create a row(3)

        row = excelsheet.CreateRow(3);
        row.Height = 1500;

        cell = row.CreateCell(0);
        cell.CellStyle = lowCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(3, 3, 0, 0));
        cell.SetCellValue("1");

        cell = row.CreateCell(1);
        cell.CellStyle = lowCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(3, 3, 1, 1));
        cell.SetCellValue("Quality");

        cell = row.CreateCell(2);
        cell.CellStyle = lowCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(3, 4, 2, 2));
        cell.SetCellValue(header);

        cell = row.CreateCell(3);
        cell.CellStyle = lowCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(3, 3, 3, 3));
        cell.SetCellValue("To reach 100% Quality in all the projects.");

        cell = row.CreateCell(4);
        cell.CellStyle = lowCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(3, 3, 4, 4));
        cell.SetCellValue("");

        cell = row.CreateCell(5);
        cell.CellStyle = lowCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(3, 3, 5, 6));
        cell.SetCellValue("We should have a separate testing \n team to QC the product \n in all the testing methods \n and prepare an actual test \n cases and test reports.");

        // Create a row(4)

        row = excelsheet.CreateRow(4);
        row.Height = 1800;

        cell = row.CreateCell(0);
        cell.CellStyle = lowCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(4, 4, 0, 0));
        cell.SetCellValue("2");

        cell = row.CreateCell(1);
        cell.CellStyle = lowCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(4, 4, 1, 1));
        cell.SetCellValue("TAT");

        cell = row.CreateCell(3);
        cell.CellStyle = lowCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(4, 4, 3, 3));
        cell.SetCellValue("To deliver all the files within the given TAT");

        cell = row.CreateCell(4);
        cell.CellStyle = lowCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(4, 4, 4, 4));
        cell.SetCellValue("");

        cell = row.CreateCell(5);
        cell.CellStyle = lowCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(4, 4, 5, 6));
        cell.SetCellValue("We can categorize the Software \n team into two parts. \n One team for doing only \n in-house projects. \n Other team will do only \n outsourcing projects. \n Since if someone working \n in both in-house and \n external projects then there is \n a chance for missing some \n of the requirements.");
    }
    protected void Customer_Feedback_Report(string header)
    {
        ISheet excelsheet = hssfworkbook.CreateSheet("Customer Feedback");

        // Set the styles of content rows and columns
        IFont lowFont = hssfworkbook.CreateFont();
        ICellStyle lowCellStyle = hssfworkbook.CreateCellStyle();
        lowFont.FontHeight = 14 * 14;
        lowCellStyle.Alignment = HorizontalAlignment.Center;
        lowCellStyle.WrapText = true;
        lowCellStyle.VerticalAlignment = VerticalAlignment.Center;
        lowCellStyle.SetFont(lowFont);
        // Set the border styles of content rows and columns
        lowCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
        lowCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
        lowCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
        lowCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
        lowCellStyle.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
        lowCellStyle.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
        lowCellStyle.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
        lowCellStyle.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;

        // Set the styles of content rows and columns
        IFont topFont = hssfworkbook.CreateFont();
        ICellStyle topCellStyle = hssfworkbook.CreateCellStyle();
        topFont.Boldweight = (short)FontBoldWeight.Bold;
        topFont.FontHeight = 14 * 14;
        topCellStyle.Alignment = HorizontalAlignment.Center;
        topCellStyle.WrapText = true;
        topCellStyle.VerticalAlignment = VerticalAlignment.Center;
        topCellStyle.SetFont(topFont);
        //styles of content rows and columns
        topCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
        topCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
        topCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
        topCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
        topCellStyle.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
        topCellStyle.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
        topCellStyle.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
        topCellStyle.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;

        // set the Styles of colspan and rowspan fields

        IFont spanFont = hssfworkbook.CreateFont();
        ICellStyle spanColumnCellStyle = hssfworkbook.CreateCellStyle();
        spanFont.Boldweight = (short)FontBoldWeight.Bold;
        spanFont.FontHeight = 14 * 14;
        spanColumnCellStyle.Alignment = HorizontalAlignment.Center;
        spanColumnCellStyle.WrapText = true;
        spanColumnCellStyle.VerticalAlignment = VerticalAlignment.Center;
        // Set the border styles of colspan and rowspan fields
        spanColumnCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
        spanColumnCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
        spanColumnCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
        spanColumnCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
        spanColumnCellStyle.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
        spanColumnCellStyle.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
        spanColumnCellStyle.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
        spanColumnCellStyle.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
        spanColumnCellStyle.SetFont(spanFont);
        spanColumnCellStyle.FillForegroundColor = HSSFColor.LightYellow.Index;
        spanColumnCellStyle.FillPattern = FillPattern.SolidForeground;

        ICell cell;
        excelsheet.DisplayGridlines = true;
        IRow row;

        excelsheet.SetColumnWidth(0, 3000);
        excelsheet.SetColumnWidth(1, 5000);
        excelsheet.SetColumnWidth(2, 3000);
        excelsheet.SetColumnWidth(3, 3000);
        excelsheet.SetColumnWidth(4, 3000);
        excelsheet.SetColumnWidth(5, 3000);
        excelsheet.SetColumnWidth(6, 3000);
        excelsheet.SetColumnWidth(7, 5000);
        excelsheet.SetColumnWidth(8, 5000);

        // Create a row(0)

        row = excelsheet.CreateRow(0);
        row.Height = 500;

        // set the image of column to selected range

        cell = row.CreateCell(0);
        cell.CellStyle = topCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 1));
        var patriarch = excelsheet.CreateDrawingPatriarch();
        HSSFClientAnchor anchor;
        anchor = new HSSFClientAnchor(0, 0, 0, 0, 0, 0, 0, 0);
        anchor.AnchorType = 2;
        string path = Server.MapPath("~/");
        path = path + @"assets\img\logo.png";
        var picture = patriarch.CreatePicture(anchor, LoadImage(path, hssfworkbook));
        picture.Resize();

        cell = row.CreateCell(2);
        cell.CellStyle = topCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(0, 0, 2, 6));
        cell.SetCellValue("PMIS");

        cell = row.CreateCell(7);
        cell.CellStyle = topCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(0, 0, 7, 7));
        cell.SetCellValue("TM05");

        cell = row.CreateCell(8);
        cell.CellStyle = topCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(0, 0, 8, 8));
        cell.SetCellValue("");

        // Create a row(1)

        row = excelsheet.CreateRow(1);
        row.Height = 300;

        cell = row.CreateCell(0);
        cell.CellStyle = topCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 6));
        cell.SetCellValue("CUSTOMER SATISFACTION MEASURES");

        cell = row.CreateCell(7);
        cell.CellStyle = topCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(1, 1, 7, 7));
        cell.SetCellValue("TOP MGMT");

        cell = row.CreateCell(8);
        cell.CellStyle = topCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(2, 2, 8, 8));
        cell.SetCellValue("");

        // Create a row(2)

        row = excelsheet.CreateRow(2);
        row.Height = 300;

        cell = row.CreateCell(0);
        cell.CellStyle = topCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(2, 2, 0, 7));
        cell.SetCellValue("Order No : " + header + "                    Customer : ");

        cell = row.CreateCell(8);
        cell.CellStyle = topCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(2, 2, 8, 8));
        cell.SetCellValue("");

        // Create a row(3)

        row = excelsheet.CreateRow(3);
        row.Height = 800;

        cell = row.CreateCell(0);
        cell.CellStyle = lowCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(3, 3, 0, 7));
        cell.SetCellValue("Please take a few minutes to give your response to following customer satisfaction \n measure, which will help us continuously improve our efforts to serve you better. \n For the following attributes if you are fully satisfied tick (a) 5.");

        cell = row.CreateCell(8);
        cell.CellStyle = spanColumnCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(3, 3, 8, 8));
        cell.SetCellValue("Production Comments");

        // Create a row(4)

        row = excelsheet.CreateRow(4);
        row.Height = 250;

        cell = row.CreateCell(0);
        cell.CellStyle = topCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(4, 4, 0, 0));
        cell.SetCellValue("No");

        cell = row.CreateCell(1);
        cell.CellStyle = topCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(4, 4, 1, 1));
        cell.SetCellValue("Service Attribute");

        cell = row.CreateCell(2);
        cell.CellStyle = topCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(4, 4, 2, 2));
        cell.SetCellValue("5");

        cell = row.CreateCell(3);
        cell.CellStyle = topCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(4, 4, 3, 3));
        cell.SetCellValue("4");

        cell = row.CreateCell(4);
        cell.CellStyle = topCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(4, 4, 4, 4));
        cell.SetCellValue("3");

        cell = row.CreateCell(5);
        cell.CellStyle = topCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(4, 4, 5, 5));
        cell.SetCellValue("2");

        cell = row.CreateCell(6);
        cell.CellStyle = topCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(4, 4, 6, 6));
        cell.SetCellValue("1");

        cell = row.CreateCell(7);
        cell.CellStyle = topCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(4, 4, 7, 7));
        cell.SetCellValue("Remarks");

        cell = row.CreateCell(8);
        cell.CellStyle = topCellStyle;
        excelsheet.AddMergedRegion(new CellRangeAddress(4, 4, 8, 8));
        cell.SetCellValue("");
    }
    protected void txt_FromDate_TextChanged(object sender, EventArgs e)
    {
        txt_ToDate.Text = "";
        string ToDateLimitStart = Convert.ToString(DateTime.ParseExact(txt_FromDate.Text.Trim(), "dd/MM/yyyy", null).ToString("MM/dd/yyyy"));
        calext_ToDate.StartDate = Convert.ToDateTime(ToDateLimitStart);
    }
}

