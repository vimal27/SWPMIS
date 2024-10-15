<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ganttChart.aspx.cs" Inherits="ganttChart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%--External CSS Files To Support jquery and gantt chart--%>
    <link rel="stylesheet" type="text/css" href="assets/css/jquery-ui-1.8.4.css" />
    <link rel="stylesheet" type="text/css" href="assets/css/charts/jquery.ganttView.css" />
    <style type="text/css">
        <%--Inline CSS To Gantt chart Style--%>
        body {
            font-family: tahoma, verdana, helvetica;
            padding: 0px;
        }
		.ganttview{
		width:1242px !important;
		}
        div.ganttview-block-text {
            color: #000;
        }

        .flat-blue .progress .progress-bar.progress-bar-success {
            background-color:;
        }

        .flat-blue .progress .progress-bar.progress-bar-danger {
            background-color:;
        }

        .flat-blue .progress .progress-bar.progress-bar-info {
            background-color:;
        }

        .flat-blue .progress .progress-bar.progress-bar-warning {
            background-color: rgb(255, 255, 136) !important;
            border-radius: 1;
        }
    </style>
    <%--External JS to Support Jquery,gantt chart--%>
    <script type="text/javascript" src="assets/js/charts/jquery-1.4.2.js"></script>
    <script type="text/javascript" src="assets/js/charts/date.js"></script>
    <script type="text/javascript" src="assets/js/jquery-ui-1.8.4.js"></script>
    <script type="text/javascript" src="assets/bower_components/jquery/dist/jquery.ganttView.js"></script>

    <%--External JS To Data For Gantt Chart like Json Format created on dashboard with corresponding  projectid and taskid--%>
    <script type="text/javascript" src="assets/gantt_Data/<%=Session["ProjectID"]+""+Session["Tasks"] %>.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="card">
        
        <%--Heading Of MainContent--%>
        <div class="card-header">
            <div class="card-title">
                <div class="title" id="TitleOfPage" runat="server">
                    Gantt Chart
                </div>
            </div>
        </div>
        
        <%--Main Content--%>
        <div class="card-body">
            
            <%--Css Loader--%>

            <%--Gantt Chart Block--%>
            <div id="ganttChart">
            </div>
            <br />
            <br />

            <%--Instructions of Ganttchart Colors--%>
            <div id="Message">
                <table class="table table table-hover">
                    <tr>
                        <td>
                            <div class="ganttview-block-container">
                                <div class="ganttview-block" style="float: left; width: 80px; margin-left: 45px;
                                    background-color: rgb(255, 255, 136);">
                                    <div class="ganttview-block-text">
                                    </div>
                                </div>
                                - Planned
                            </div>
                        </td>
                        <td>
                            <div class="ganttview-block-container">
                                <div class="ganttview-block" style="float: left; width: 80px; margin-left: 45px;
                                    background-color: rgb(136, 232, 245);">
                                    <div class="ganttview-block-text">
                                    </div>
                                </div>
                                - WIP(Work In Progress)
                            </div>
                        </td>
                        <td>
                            <div class="ganttview-block-container">
                                <div class="ganttview-block" style="float: left; width: 80px; margin-left: 45px;
                                    background-color: rgb(230, 33, 33);">
                                    <div class="ganttview-block-text">
                                    </div>
                                </div>
                                - WIP with Time Extend
                            </div>
                        </td>
                        <td>
                            <div class="ganttview-block-container">
                                <div class="ganttview-block" style="float: left; width: 80px; margin-left: 45px;
                                    background-color: rgb(255, 156, 124);">
                                    <div class="ganttview-block-text">
                                    </div>
                                </div>
                                - Time Extended and Completed
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="ganttview-block-container">
                                <div class="ganttview-block" style="float: left; width: 80px; margin-left: 45px;
                                    background-color: rgb(128, 245, 132);">
                                    <div class="ganttview-block-text">
                                    </div>
                                </div>
                                - Completed
                            </div>
                        </td>
                        <td>
                            <div class="ganttview-block-container">
                                <div class="ganttview-block" style="float: left; width: 80px; margin-left: 45px;
                                    background-color: #eee">
                                    <div class="ganttview-block-text">
                                    </div>
                                </div>
                                - Holded
                            </div>
                        </td>
                    </tr>
                </tr>
            </div>
        </div>
    </div>
    
    <%--Javascript Function To Create Gantt Chart Dynamically--%>
    <script type="text/javascript">
        $(function () {
            $("#ganttChart").ganttView({
                data: gantt,
                slideWidth: 1000,
                behavior: {
            }
        });
        
        //Set width For gantt Chart
        $("#ganttChart").ganttView("setSlideWidth", 1000);
    });
    </script>
</asp:Content>
