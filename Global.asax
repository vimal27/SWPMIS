<%@ Application Language="C#" %>

<%--Namespaces To Support Web Routing To Make Project With or without .aspx--%>
<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="System.Web.Routing" %>
<%@ Import Namespace="System.IO" %>
<script RunAt="server">
    void Application_Start(object sender, EventArgs e)
    {
        RegisterRoutes(RouteTable.Routes);
    }
    static void RegisterRoutes(RouteCollection routes)
    {
        routes.MapPageRoute("404", "404", "~/404.aspx");
        routes.MapPageRoute("ErrorPage", "ErrorPage", "~/ErrorPage.aspx");
        routes.MapPageRoute("Login", "Login", "~/Login.aspx");
        routes.MapPageRoute("Dashboard", "Dashboard", "~/Dashboard.aspx");
        routes.MapPageRoute("ganttchart", "ganttchart", "~/ganttchart.aspx");
        routes.MapPageRoute("Status", "Status", "~/Status.aspx");
        routes.MapPageRoute("Projects", "Projects", "~/Projects.aspx");
        routes.MapPageRoute("ProjectAnalysis", "ProjectAnalysis", "~/ProjectAnalysis.aspx");
        routes.MapPageRoute("Stages", "Stages", "~/Stages.aspx");
        routes.MapPageRoute("Users", "Users", "~/Users.aspx");
        routes.MapPageRoute("Teams", "Teams", "~/Teams.aspx");
        routes.MapPageRoute("TeamAllotment", "TeamAllotment", "~/TeamAllotment.aspx");
        routes.MapPageRoute("Tasks", "Tasks", "~/Tasks.aspx");
        routes.MapPageRoute("Milestones", "Milestones", "~/Milestones.aspx");
        routes.MapPageRoute("TimeTrackingSheet", "TimeTrackingSheet", "~/TimeTrackingSheet.aspx");
        routes.MapPageRoute("JobCardReport", "JobCardReport", "~/JobCardReport.aspx");
        routes.MapPageRoute("ProductionReport", "ProductionReport", "~/ProductionReport.aspx");
        routes.MapPageRoute("Non_ProductionReport", "Non_ProductionReport", "~/Non_ProductionReport.aspx");
        routes.MapPageRoute("DPR_NonEntry", "DPR_NonEntry", "~/DPR_NonEntry.aspx");
        routes.MapPageRoute("MIS_Report", "MIS_Report", "~/MIS_Report.aspx");
        routes.MapPageRoute("MIS_Report_Mail", "MIS_Report_Mail", "~/MIS_Report.aspx?action=Mail");
        routes.MapPageRoute("MRM_Report", "MRM_Report", "~/MRM_Report.aspx");
        routes.MapPageRoute("Holidays", "Holidays", "~/Holidays.aspx");
        routes.MapPageRoute("AutoMisReport", "AutoMisReport", "~/AutoMisReport.aspx");
        routes.MapPageRoute("DCS", "DCS", "~/DCS.aspx");
        routes.MapPageRoute("CodingStandardRating", "CodingStandardRating", "~/CodingStandardRating.aspx");
        routes.MapPageRoute("ProductivityReport", "ProductivityReport", "~/ProductivityReport.aspx");
        routes.MapPageRoute("Status_1", "Status_1", "~/Status.aspx?status=yettostart");
        routes.MapPageRoute("Status_2", "Status_2", "~/Status.aspx?status=WIP");
        routes.MapPageRoute("Status_3", "Status_3", "~/Status.aspx?status=Hold");
        routes.MapPageRoute("Status_4", "Status_4", "~/Status.aspx?status=Closed");
        routes.MapPageRoute("Status_5", "Status_5", "~/Status.aspx?status=Completed");
        routes.MapPageRoute("Status_6", "Status_6", "~/Status.aspx?status=OnEarly");
        routes.MapPageRoute("Status_7", "Status_7", "~/Status.aspx?status=OnTime");
        routes.MapPageRoute("Status_8", "Status_8", "~/Status.aspx?status=Extended");
        routes.MapPageRoute("Status_A", "Status_A", "~/Status.aspx?status=Active");

    }
    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown

    }

    void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs
        // Code that runs when an unhandled error occurs
        //var serverError = Server.GetLastError() as HttpException;
        //if (null != serverError)
        //{
        //    int errorCode = serverError.GetHttpCode();
        //    if (404 == errorCode)
        //    {
        //        Server.ClearError();

        //        Response.Redirect("404");
        //    }
        //    else
        //    {
        //        Response.Redirect("ErrorPage");
        //    }
        //}
    }

    void Session_Start(object sender, EventArgs e)
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }

</script>
