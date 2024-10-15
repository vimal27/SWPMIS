using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Text;
using System.Web.Script.Services;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.ServiceModel.Web;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using System.Data.Sql;
using System.Web.Script;

namespace swpmis
{
    /// <summary>
    /// Web Service to Top Bar Status in Count
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    public class WebService : System.Web.Services.WebService
    {
        [WebMethod(EnableSession = true), ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public string getChartsData()
        {
            string chartData = string.Empty;
            string pie_Completed = string.Empty;
            string pie_WIP = string.Empty;
            string pie_YetToStart = string.Empty;
            string pie_Hold = string.Empty;
            string pie_Closed = string.Empty;
            string report = "";
            int pc_WIP, pc_Completed, pc_Hold, pc_Closed, pc_YetToStart, pc_Total;
            StringBuilder MonthReport = new StringBuilder();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString());
            SqlCommand com_Users = new SqlCommand();
            Dictionary<string, string> lst_Result = new Dictionary<string, string>();
            string Rights = Convert.ToString(HttpContext.Current.Session["Rights"]);
            if (Rights == "Administrator")
            {
                com_Users.Connection = con;
                con.Close();
                con.Open();
                for (int i = 1; i < 13; i++)
                {
                    com_Users.CommandText = "SELECT  COUNT(task) from prmsProductionHour_Backup where statusoftask=100 and year(CurrentDate)='" + DateTime.Now.Year + "' and  month(CurrentDate)='" + i + "' and ProjectID!='NA' and task!='-1' group by month(CurrentDate)";
                    SqlDataReader sdr = com_Users.ExecuteReader();
                    if (sdr.HasRows)
                    {
                        sdr.Read();
                        report = sdr[0].ToString();
                        {
                            if (i == 1)
                            {
                                MonthReport.Append("[");
                            }
                            if (i != 12)
                            {
                                MonthReport.Append(sdr[0].ToString() + ",");
                            }
                        }

                        if (i == 12)
                        {
                            if (MonthReport.ToString().EndsWith(","))
                            {
                                MonthReport.Length--;
                            }
                            MonthReport.Append("]");
                            if (MonthReport.ToString() == "[]")
                            {
                                MonthReport.Replace("[]", "[0,0,0,0,0,0,0,0,0,0,0,0]");
                            }
                        }
                    }
                    else
                    {
                        if (DateTime.Today.Month >= i)
                        {
                            if (i == 1)
                            {
                                MonthReport.Append("[");
                            }
                            if (i != 12)
                            {
                                MonthReport.Append("0" + ",");
                            }
                            if (i == 12)
                            {
                                if (MonthReport.ToString().EndsWith(","))
                                {
                                    MonthReport.Length--;
                                }
                                MonthReport.Append("]");
                                if (MonthReport.ToString() == "[]")
                                {
                                    MonthReport.Replace("[]", "[0,0,0,0,0,0,0,0,0,0,0,0]");
                                }
                            }
                        }
                        if (i == 12)
                        {
                            if (MonthReport.ToString().EndsWith(","))
                            {
                                MonthReport.Length--;
                            }
                            MonthReport.Append("]");
                            if (MonthReport.ToString() == "[]")
                            {
                                MonthReport.Replace("[]", "[0,0,0,0,0,0,0,0,0,0,0,0]");
                            }
                        }
                    }
                    sdr.Close();
                }
                chartData = MonthReport.ToString();

                //To Get PieChart Data
                com_Users.CommandText = "select top 1(select COUNT(id) from tbl_taskmaster where taskstatus='Completed')[Completed],(select COUNT(id) from tbl_taskmaster where taskstatus='WIP')[WIP],(select COUNT(id) from tbl_taskmaster where taskstatus='Hold')[Hold],(select COUNT(id) from tbl_taskmaster where taskstatus='Closed')[Closed],(select COUNT(id) from tbl_taskmaster where taskstatus='Yet To Start')[Yet To Start],(select COUNT(id) from tbl_taskmaster)[Total] from tbl_taskmaster";
                SqlDataReader pie = com_Users.ExecuteReader();
                if (pie.HasRows)
                {
                    pie.Read();
                    pc_Total = pie[5] as int? ?? default(int);
                    pc_Completed = pie[0] as int? ?? default(int);
                    pc_WIP = pie[1] as int? ?? default(int);
                    pc_Hold = pie[2] as int? ?? default(int);
                    pc_Closed = pie[3] as int? ?? default(int);
                    pc_YetToStart = pie[4] as int? ?? default(int);
                    pie_Completed = Convert.ToString(((double)pc_Completed / (double)pc_Total) * 100);
                    pie_WIP = Convert.ToString(((double)pc_WIP / (double)pc_Total) * 100);
                    pie_Hold = Convert.ToString(((double)pc_Hold / (double)pc_Total) * 100);
                    pie_Closed = Convert.ToString(((double)pc_Closed / (double)pc_Total) * 100);
                    pie_YetToStart = Convert.ToString(((double)pc_YetToStart / (double)pc_Total) * 100);
                }
                pie.Close();
            }

            //If Team Leader Pie Chart and Bar chart values
            else if (Rights == "Team Leader")
            {
                com_Users.Connection = con;
                con.Close();
                con.Open();
                for (int i = 1; i < 13; i++)
                {
                    com_Users.CommandText = "SELECT COUNT(b.task) from prmsProductionHour_Backup b inner join tbl_ProjectReq a on b.ProjectID=a.projectid where b.statusoftask=100 and year(b.CurrentDate)='" + DateTime.Now.Year + "' and  month(b.CurrentDate)='" + i + "' and b.ProjectID!='NA' and a.allotedteamid like '%'+(select TeamID from tbl_teamAllotmentMaster where teamleader like ('%" + Session["Userid"] + "%'))+'%' group by month(b.CurrentDate)";
                    SqlDataReader sdr = com_Users.ExecuteReader();
                    if (sdr.HasRows)
                    {
                        sdr.Read();
                        report = sdr[0].ToString();
                        {
                            if (i == 1)
                            {
                                MonthReport.Append("[");
                            }
                            if (i != 12)
                            {
                                MonthReport.Append(sdr[0].ToString() + ",");
                            }
                        }

                        if (i == 12)
                        {
                            if (MonthReport.ToString().EndsWith(","))
                            {
                                MonthReport.Length--;
                            }
                            MonthReport.Append("]");
                            if (MonthReport.ToString() == "[]")
                            {
                                MonthReport.Replace("[]", "[0,0,0,0,0,0,0,0,0,0,0,0]");
                            }
                        }
                    }
                    else
                    {
                        if (DateTime.Today.Month >= i)
                        {
                            if (i == 1)
                            {
                                MonthReport.Append("[");
                            }
                            if (i != 12)
                            {
                                MonthReport.Append("0" + ",");
                            }
                            if (i == 12)
                            {
                                if (MonthReport.ToString().EndsWith(","))
                                {
                                    MonthReport.Length--;
                                }
                                MonthReport.Append("]");
                                if (MonthReport.ToString() == "[]")
                                {
                                    MonthReport.Replace("[]", "[0,0,0,0,0,0,0,0,0,0,0,0]");
                                }
                            }
                        }
                        if (i == 12)
                        {
                            if (MonthReport.ToString().EndsWith(","))
                            {
                                MonthReport.Length--;
                            }
                            MonthReport.Append("]");
                            if (MonthReport.ToString() == "[]")
                            {
                                MonthReport.Replace("[]", "[0,0,0,0,0,0,0,0,0,0,0,0]");
                            }
                        }
                    }
                    sdr.Close();
                }
                chartData = MonthReport.ToString();

                //Pie Chart Values
                com_Users.CommandText = "select top 1(select COUNT(id) from tbl_taskmaster a inner join tbl_ProjectReq b on b.projectid=a.projectid and b.allotedteamid like '%'+(select TeamID from tbl_teamAllotmentMaster where teamleader like ('%" + Session["Userid"] + "%'))+'%' where taskstatus='Completed')[Completed],(select COUNT(id) from tbl_taskmaster a inner join tbl_ProjectReq b on b.projectid=a.projectid and b.allotedteamid in (select TeamID from tbl_teamAllotmentMaster where teamleader like ('%" + Session["Userid"] + "%')) where taskstatus='WIP')[WIP],(select COUNT(id) from tbl_taskmaster a inner join tbl_ProjectReq b on b.projectid=a.projectid and b.allotedteamid in (select TeamID from tbl_teamAllotmentMaster where teamleader like ('%" + Session["Userid"] + "%')) where taskstatus='Hold')[Hold],(select COUNT(id) from tbl_taskmaster a inner join tbl_ProjectReq b on b.projectid=a.projectid and b.allotedteamid in (select TeamID from tbl_teamAllotmentMaster where teamleader like ('%" + Session["Userid"] + "%')) where taskstatus='Closed')[Closed],(select COUNT(id) from tbl_taskmaster a inner join tbl_ProjectReq b on b.projectid=a.projectid and b.allotedteamid in (select TeamID from tbl_teamAllotmentMaster where teamleader like ('%" + Session["Userid"] + "%')) where taskstatus='Yet To Start')[Yet To Start],(select COUNT(id) from tbl_taskmaster a inner join tbl_ProjectReq b on b.projectid=a.projectid and b.allotedteamid in (select TeamID from tbl_teamAllotmentMaster where teamleader like ('%" + Session["Userid"] + "%')))[Total] from tbl_taskmaster";
                SqlDataReader pie = com_Users.ExecuteReader();
                if (pie.HasRows)
                {
                    pie.Read();
                    pc_Total = pie[5] as int? ?? default(int);
                    pc_Completed = pie[0] as int? ?? default(int);
                    pc_WIP = pie[1] as int? ?? default(int);
                    pc_Hold = pie[2] as int? ?? default(int);
                    pc_Closed = pie[3] as int? ?? default(int);
                    pc_YetToStart = pie[4] as int? ?? default(int);
                    pie_Completed = Convert.ToString(((double)pc_Completed / (double)pc_Total) * 100);
                    pie_WIP = Convert.ToString(((double)pc_WIP / (double)pc_Total) * 100);
                    pie_Hold = Convert.ToString(((double)pc_Hold / (double)pc_Total) * 100);
                    pie_Closed = Convert.ToString(((double)pc_Closed / (double)pc_Total) * 100);
                    pie_YetToStart = Convert.ToString(((double)pc_YetToStart / (double)pc_Total) * 100);
                }
                pie.Close();
            }
            else
            {
                //If Users Pie chart and Barchart Data
                com_Users.Connection = con;
                con.Close();
                con.Open();
                for (int i = 1; i < 13; i++)
                {
                    com_Users.CommandText = "SELECT COUNT(task) from prmsProductionHour_Backup where statusoftask=100 and year(CurrentDate)='" + DateTime.Now.Year + "' and month(CurrentDate)='" + i + "' and EmpNo='" + Session["Userid"] + "' and ProjectID!='NA' group by month(CurrentDate)";
                    SqlDataReader sdr = com_Users.ExecuteReader();
                    if (sdr.HasRows)
                    {
                        sdr.Read();
                        report = sdr[0].ToString();
                        {
                            if (i == 1)
                            {
                                MonthReport.Append("[");
                            }
                            if (i != 12)
                            {
                                MonthReport.Append(sdr[0].ToString() + ",");
                            }
                        }

                        if (i == 12)
                        {
                            if (MonthReport.ToString().EndsWith(","))
                            {
                                MonthReport.Length--;
                            }
                            MonthReport.Append("]");
                            if (MonthReport.ToString() == "[]")
                            {
                                MonthReport.Replace("[]", "[0,0,0,0,0,0,0,0,0,0,0,0]");
                            }
                        }
                    }
                    else
                    {
                        if (DateTime.Today.Month >= i)
                        {
                            if (i == 1)
                            {
                                MonthReport.Append("[");
                            }
                            if (i != 12)
                            {
                                MonthReport.Append("0" + ",");
                            }
                            if (i == 12)
                            {
                                if (MonthReport.ToString().EndsWith(","))
                                {
                                    MonthReport.Length--;
                                }
                                MonthReport.Append("]");
                                if (MonthReport.ToString() == "[]")
                                {
                                    MonthReport.Replace("[]", "[0,0,0,0,0,0,0,0,0,0,0,0]");
                                }
                            }
                        }
                        if (i == 12)
                        {
                            if (MonthReport.ToString().EndsWith(","))
                            {
                                MonthReport.Length--;
                            }
                            MonthReport.Append("]");
                            if (MonthReport.ToString() == "[]")
                            {
                                MonthReport.Replace("[]", "[0,0,0,0,0,0,0,0,0,0,0,0]");
                            }
                        }
                    }
                    sdr.Close();
                }
                chartData = MonthReport.ToString();
                //Pie Chart Values
                com_Users.CommandText = "select top 1(select COUNT(id) from tbl_taskmaster where taskstatus='Completed' and userid='" + Session["Userid"] + "')[Completed],(select COUNT(id) from tbl_taskmaster where taskstatus='WIP' and userid='" + Session["Userid"] + "')[WIP],(select COUNT(id) from tbl_taskmaster where taskstatus='Hold' and userid='" + Session["Userid"] + "')[Hold],(select COUNT(id) from tbl_taskmaster where taskstatus='Closed' and userid='" + Session["Userid"] + "')[Closed],(select COUNT(id) from tbl_taskmaster where taskstatus='Yet To Start' and userid='" + Session["Userid"] + "')[Yet To Start],(select COUNT(id) from tbl_taskmaster where userid='" + Session["Userid"] + "')[Total] from tbl_taskmaster where userid='" + Session["Userid"] + "'";
                SqlDataReader pie = com_Users.ExecuteReader();
                if (pie.HasRows)
                {
                    pie.Read();
                    pc_Total = pie[5] as int? ?? default(int);
                    pc_Completed = pie[0] as int? ?? default(int);
                    pc_WIP = pie[1] as int? ?? default(int);
                    pc_Hold = pie[2] as int? ?? default(int);
                    pc_Closed = pie[3] as int? ?? default(int);
                    pc_YetToStart = pie[4] as int? ?? default(int);
                    pie_Completed = Convert.ToString(((double)pc_Completed / (double)pc_Total) * 100);
                    pie_WIP = Convert.ToString(((double)pc_WIP / (double)pc_Total) * 100);
                    pie_Hold = Convert.ToString(((double)pc_Hold / (double)pc_Total) * 100);
                    pie_Closed = Convert.ToString(((double)pc_Closed / (double)pc_Total) * 100);
                    pie_YetToStart = Convert.ToString(((double)pc_YetToStart / (double)pc_Total) * 100);
                }
                pie.Close();
            }
            lst_Result.Add("YetToStart", pie_YetToStart);
            lst_Result.Add("Hold", pie_Hold);
            lst_Result.Add("WIP", pie_WIP);
            lst_Result.Add("Closed", pie_Closed);
            lst_Result.Add("Completed", pie_Completed);
            lst_Result.Add("ChartData", chartData);
            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(lst_Result);
            return json;
        }
        [WebMethod(EnableSession = true), ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public string getDashboardData()
        {
            return "";
        }
    }
}
