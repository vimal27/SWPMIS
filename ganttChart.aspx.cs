//Required Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

public partial class ganttChart : System.Web.UI.Page
{
    /// <summary>
    /// Declarations Part For Strings
    /// </summary>
    string UserName, Stage, ProjectID, Status, CountTasks;

	/// <summary>
    /// Page Load Function
	/// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        //System.Web.UI.HtmlControls.HtmlGenericControl Side_Menu = (System.Web.UI.HtmlControls.HtmlGenericControl)Master.FindControl("Side_Menu");
        //System.Web.UI.HtmlControls.HtmlGenericControl Header = (System.Web.UI.HtmlControls.HtmlGenericControl)Master.FindControl("nav_Header");
        //System.Web.UI.HtmlControls.HtmlGenericControl Footer = (System.Web.UI.HtmlControls.HtmlGenericControl)Master.FindControl("nav_Header");
        //Header.Style.Add("display", "none");
        //Side_Menu.Style.Add("display", "none");

		//The Following Lines are Currently not Used,which are for debugging
        string gantt_data = @"[
{
id: 1, name: ""Feature 1"", series: [
{ name: ""Planned"", start: new Date(2010,00,01), end: new Date(2010,00,03) },
{ name: ""Actual"", start: new Date(2010,00,02), end: new Date(2010,00,05), color: ""#f0f0f0"" }
]
}

];";

        //lbl_UserID.Text = Session["JsonTasks"].ToString();
        //UserID = Session["EmpNO"].ToString();
        //UserName = Session["EmpName"].ToString();
        //ProjectID = Session["ProjectID"].ToString();
        //Stage = Session["Stage"].ToString();
        //Status = Session["Status"].ToString();
        if (!IsPostBack)
        {
            CountTasks = Convert.ToString(Session["JsonTasks"]);
            //lbl_UserID.Text = CountTasks;
        }
    }

}