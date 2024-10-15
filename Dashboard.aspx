<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Dashboard.aspx.cs" Inherits="Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%--Jquery Link--%>
    <%--<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js">--%>
    <script type="text/javascript" src="assets/js/charts/jquery.min.js"></script>
    <%--Angular JS Plugin--%>
    <script type="text/javascript" src="assets/js/angular.min.js"></script>
    <%--CSS Styles--%>
    <style type="text/css">
        .panel-heading span {
            margin-top: -20px;
            font-size: 15px;
        }

        .clickable {
            cursor: pointer;
        }

        table {
            width: 100%;
        }

            table tr td {
                border: solid 1px green;
            }

        ul li {
            list-style-type: none;
        }

        .gantt {
            background: maroon;
            color: #fff;
            margin-top: 1px;
        }

        .card yellow summary-inline:hover {
            background-color: #565800;
        }

        .card.summary-inline .card-body .content .title {
            font-size: 2.5em !important;
        }

        .card.summary-inline .card-body .content .sub-title {
            font-size: 0.90em !important;
            margin-top: 0px !important;
        }

        .fa-3x {
            font-size: 2.0em !important;
        }

        .col-lg-4 {
            width: 19.333333% !important;
        }
    </style>
    <%--Css External Files To Support GanttChart--%>
    <link rel="stylesheet" type="text/css" href="assets/css/charts/jquery.ganttView.css" />
    <link rel="stylesheet" type="text/css" href="assets/css/jquery-ui.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--Heading Of MainContent--%>
    <div class="card" ng-app="swpmis">
        <div class="card-header">
            <div class="card-title">
                <div class="title" id="TitleOfPage" runat="server">
                    Dashboard
                </div>
            </div>
        </div>
        <%--Main Content--%>
        <div id="block_all" runat="server">
            <div class="card-body">
                <h4 class="thumbnail-label">Tasks</h4>
                <div class="row">
                    <%--URL Query String To Redirect to Yet To Start--%>
                    <div class="col-md-2" style="min-width: 20%">
                        <a href="Status_1">
                            <div class="card yellow summary-inline" style="background-color: #9FA400">
                                <div class="card-body">
                                    <%--<div class="cssload-container">
                                    <div class="cssload-whirlpool">
                                    </div>
                                </div>--%>
                                    <i class="icon fa fa-inbox fa-3x"></i>
                                    <div class="content">
                                        <div class="title">
                                            <asp:Label ID="lbl_YetToStart" runat="server" Text="0"></asp:Label>
                                        </div>
                                        <div class="sub-title">
                                            Yet To Start
                                        </div>
                                    </div>
                                    <div class="clear-both">
                                    </div>
                                </div>
                            </div>
                        </a>
                    </div>
                    <%--URL Query String To Redirect to WIP--%>
                    <div class="col-md-2" style="min-width: 20%">
                        <a href="Status_2">
                            <div class="card blue summary-inline">
                                <div class="card-body">
                                    <i class="icon glyphicon glyphicon-play fa-3x"></i>
                                    <div class="content">
                                        <div class="title">
                                            <asp:Label ID="lbl_WIP" runat="server" Text="0"></asp:Label>
                                        </div>
                                        <div class="sub-title" runat="server" id="title_WIP">
                                            WIP
                                        </div>
                                    </div>
                                    <div class="clear-both">
                                    </div>
                                </div>
                            </div>
                        </a>
                    </div>
                    <%--URL Query String To Redirect to Hold--%>
                    <div class="col-md-2" style="min-width: 20%">
                        <a href="Status_3">
                            <div class="card yellow summary-inline">
                                <div class="card-body">
                                    <i class="icon glyphicon glyphicon-pause fa-3x"></i>
                                    <div class="content">
                                        <div class="title">
                                            <asp:Label ID="lbl_Hold" runat="server" Text="0"></asp:Label>
                                        </div>
                                        <div class="sub-title" runat="server" id="title_Hold">
                                            Hold
                                        </div>
                                    </div>
                                    <div class="clear-both">
                                    </div>
                                </div>
                            </div>
                        </a>
                    </div>
                    <%--URL Query String To Redirect to Completed--%>
                    <div class="col-md-2" style="min-width: 20%">
                        <a href="Status_5">
                            <div class="card green summary-inline">
                                <div class="card-body">
                                    <i class="icon glyphicon glyphicon-check fa-3x"></i>
                                    <div class="content">
                                        <div class="title">
                                            <asp:Label ID="lbl_Completed" runat="server" Text="0"></asp:Label>
                                        </div>
                                        <div class="sub-title" runat="server" id="title_Completed">
                                            Completed
                                        </div>
                                    </div>
                                    <div class="clear-both">
                                    </div>
                                </div>
                            </div>
                        </a>
                    </div>
                    <%--URL Query String To Redirect to Closed--%>
                    <div class="col-md-2" style="min-width: 20%">
                        <a href="Status_4">
                            <div class="card red summary-inline">
                                <div class="card-body">
                                    <i class="icon glyphicon glyphicon-off fa-3x"></i>
                                    <div class="content">
                                        <div class="title">
                                            <asp:Label ID="lbl_Cancelled" runat="server" Text="0"></asp:Label>
                                        </div>
                                        <div class="sub-title" runat="server" id="title_Cancelled">
                                            Closed
                                        </div>
                                    </div>
                                    <div class="clear-both">
                                    </div>
                                </div>
                            </div>
                        </a>
                    </div>
                </div>
                <%--URL Query String To Redirect to Project Yet To Start--%>
                <div id="block_Project" runat="server">
                    <h4 style="margin-top: 10%;" class="thumbnail-label">Projects</h4>
                    <div class="col-lg-4 col-md-4 col-sm-4 col-xs-10">
                        <a href="Status?status=projects_yettostart">
                            <div class="card yellow summary-inline" style="background-color: #9FA400">
                                <div class="card-body">
                                    <i class="icon fa fa-inbox fa-3x"></i>
                                    <div class="content">
                                        <div class="title">
                                            <asp:Label ID="lbl_ProjectYetToStart" runat="server" Text="0"></asp:Label>
                                        </div>
                                        <div class="sub-title">
                                            Yet To Start
                                        </div>
                                    </div>
                                    <div class="clear-both">
                                    </div>
                                </div>
                            </div>
                        </a>
                    </div>
                    <%--URL Query String To Redirect to Project WIP--%>
                    <div class="col-lg-4 col-md-4 col-sm-4 col-xs-10">
                        <a href="Status?status=projects_WIP">
                            <div class="card blue summary-inline">
                                <div class="card-body">
                                    <i class="icon glyphicon glyphicon-play fa-3x"></i>
                                    <div class="content">
                                        <div class="title">
                                            <asp:Label ID="lbl_ProjectWIP" runat="server" Text="0"></asp:Label>
                                        </div>
                                        <div class="sub-title" runat="server" id="Div1">
                                            WIP
                                        </div>
                                    </div>
                                    <div class="clear-both">
                                    </div>
                                </div>
                            </div>
                        </a>
                    </div>
                    <%--URL Query String To Redirect to Project Hold--%>
                    <div class="col-lg-4 col-md-4 col-sm-4 col-xs-10">
                        <a href="Status?status=projects_Hold">
                            <div class="card yellow summary-inline">
                                <div class="card-body">
                                    <i class="icon glyphicon glyphicon-pause fa-3x"></i>
                                    <div class="content">
                                        <div class="title">
                                            <asp:Label ID="lbl_ProjectHold" runat="server" Text="0"></asp:Label>
                                        </div>
                                        <div class="sub-title" runat="server" id="Div2">
                                            Hold
                                        </div>
                                    </div>
                                    <div class="clear-both">
                                    </div>
                                </div>
                            </div>
                        </a>
                    </div>
                    <%--URL Query String To Redirect to Project Completed--%>
                    <div class="col-lg-4 col-md-4 col-sm-4 col-xs-10">
                        <a href="Status?status=projects_Completed">
                            <div class="card green summary-inline">
                                <div class="card-body">
                                    <i class="icon glyphicon glyphicon-check fa-3x"></i>
                                    <div class="content">
                                        <div class="title">
                                            <asp:Label ID="lbl_ProjectCompleted" runat="server" Text="0"></asp:Label>
                                        </div>
                                        <div class="sub-title" runat="server" id="Div3">
                                            Completed
                                        </div>
                                    </div>
                                    <div class="clear-both">
                                    </div>
                                </div>
                            </div>
                        </a>
                    </div>
                    <%--URL Query String To Redirect to Project Closed--%>
                    <div class="col-lg-4 col-md-4 col-sm-4 col-xs-6" style="backgroud-color: #F44336 !important">
                        <a href="Status?status=projects_Closed">
                            <div class="card red summary-inline">
                                <div class="card-body">
                                    <i class="icon glyphicon glyphicon-off fa-3x"></i>
                                    <div class="content">
                                        <div class="title">
                                            <asp:Label ID="lbl_ProjectClosed" runat="server" Text="0"></asp:Label>
                                        </div>
                                        <div class="sub-title" runat="server" id="Div4">
                                            Closed
                                        </div>
                                    </div>
                                    <div class="clear-both">
                                    </div>
                                </div>
                            </div>
                        </a>
                    </div>
                </div>
                <%--Bar Chart and Pie Chart--%>
                <div class="panel panel-info">
                    <div class="panel-heading">
                        <h3 class="panel-title">MonthWise Task Report
                        </h3>
                        <span class="pull-right clickable"><i class="glyphicon glyphicon-chevron-up"></i>
                        </span>
                    </div>
                    <div class="panel-body">
                        <div ng-controller="chartsController" class="table-responsive">
                            <%--Pie Chart--%>
                            <div class="col-sm-4 col-xs-4">
                                <div id="pieChart">
                                </div>
                            </div>
                            <%--Bar Chart--%>
                            <div class="col-sm-8 col-xs-8">
                                <div id="ChartTasks">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <%--Coding Standard and Quality Rating Line Chart--%>
                <div id="pnl_RatingChart" class="panel panel-success">
                    <div class="panel-heading">
                        <h3 class="panel-title">Coding Standard / Quality Rating Chart</h3>
                        <span class="pull-right clickable"><i class="glyphicon glyphicon-chevron-up"></i>
                        </span>
                    </div>
                    <div class="panel-body">
                        <div class="col-sm-12 table-responsive" style="margin-top: 2%; margin-left: 0%; float: left;">
                            <%--Line Chart For Coding Standard / Qulaity Rating--%>
                            <asp:DropDownList ID="ddl_FromMonth" runat="server" Style="float: right">
                            </asp:DropDownList>
                            <asp:DropDownList ID="ddl_FromYear" runat="server" Style="float: right">
                            </asp:DropDownList>
                            <asp:Label ID="lbl_Duration" runat="server" Text="For -" Style="float: right; margin-right: 1%">
                            </asp:Label>
                            <asp:DropDownList ID="ddl_EmpNo" runat="server" Style="float: right; margin-right: 2%">
                            </asp:DropDownList>
                            <%-- <asp:Label ID="lbl_EmpNo" runat="server" Text="Employee -" Style="float: right; margin-right: 1%">
                            </asp:Label>--%>
                            <div class="col-sm-12 table table-responsive">
                                <div id="chart_CodingStandardRating" runat="server">
                                    <div id="ContainerCodingStandard">
                                    </div>
                                </div>
                            </div>
                            <%-- <div class="row">
                        <div class="card card-success">
                            <div class="card-header">
                                <div class="card-title">
                                    <div class="title">
                                        <asp:Panel runat="server" Style="float: left" ID="pnl_All">
                                            <i class="fa fa-comments-o" style="float: left; color: #fff"></i><span style="color: #fff">
                                                Tasks</span>
                                        </asp:Panel>
                                    </div>
                                </div>
                                <div class="clear-both">
                                </div>
                            </div>
                            <div class="card-body no-padding">
                                <asp:HiddenField ID="hdn_TaskJson" runat="server" />
                                <asp:Label ID="lbl_TaskID" runat="server" Visible="false"></asp:Label>
                                <ul id="ul_Message" class="message-list" style="overflow-y: auto;" runat="server">
                                </ul>
                                <asp:LinkButton ID="lnk_All" OnClick="lnk_All_Click" CssClass="btn btn-block" Style="text-align: center"
                                    runat="server" Text="Full Gantt Chart"></asp:LinkButton>
                            </div>
                        </div>
                    </div>--%>
                        </div>
                    </div>
                </div>
                <%--</div>--%>
                <%--Gridview Block--%>
                <div class="panel panel-warning" id="pnl_CurrentTasks">
                    <div class="panel-heading">
                        <h3 class="panel-title">Current Tasks
                        </h3>
                        <span class="pull-right clickable"><i class="glyphicon glyphicon-chevron-down"></i>
                        </span>
                    </div>
                    <div class="panel-body" style="display: none">
                        <div class="col-sm-6 col-xs-6" style="float: right; margin-top: 2%; width: 100%; overflow-x: auto">
                            <asp:GridView ID="grd_WIP" runat="server" Style="overflow-x: auto !important;" CssClass="datatable table table-responsive dataTable"
                                AutoGenerateColumns="False" DataKeyNames="id" AllowPaging="true" OnSelectedIndexChanged="grd_WIP_SelectedIndexChanged"
                                OnPageIndexChanging="OnPageIndexChanging" PageSize="10" OnRowDataBound="grd_WIP_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Project">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Project" runat="server" Text='<%# Eval("Project") %>'></asp:Label>
                                            <asp:HiddenField ID="lbl_RequestNo" runat="server" Value='<%# Eval("requestid") %>'></asp:HiddenField>
                                            <asp:HiddenField ID="hdn_projectid" runat="server" Value='<%# Eval("projectid")%>'></asp:HiddenField>
                                            <asp:HiddenField ID="hdn_taskid" runat="server" Value='<%# Eval("id")%>'></asp:HiddenField>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Scope">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Scope" runat="server" Text='<%# Eval("Scope") %>'></asp:Label>
                                            <asp:HiddenField ID="hdn_ScopeID" runat="server" Value='<%# Eval("scopeid")%>'></asp:HiddenField>
                                            <asp:HiddenField ID="hdn_TeamID" runat="server" Value='<%# Eval("teamid")%>'></asp:HiddenField>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Stage">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Stage" runat="server" Text='<%# Eval("Stage") %>'></asp:Label>
                                            <asp:HiddenField ID="hdn_StageID" runat="server" Value='<%# Eval("stageid")%>'></asp:HiddenField>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Task">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Task" runat="server" Text='<%# Eval("taskname") %>'></asp:Label>
                                            <asp:HiddenField ID="hdn_TaskDescription" runat="server" Value='<%# Eval("taskdescription")%>'></asp:HiddenField>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Request Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_RequestDate" runat="server" Text='<%# Eval("requestdate") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Required Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_RequiredDate" runat="server" Text='<%# Eval("requireddate") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Actions">
                                        <ItemTemplate>
                                            <asp:LinkButton ToolTip="Select" ID="lnk_TaskStatus" runat="server" CommandName="Select"></asp:LinkButton>
                                            <asp:LinkButton ID="lnk_View" Style="color: rgba(159, 0, 147, 0.62)" Text='<i class="glyphicon glyphicon-eye-open fa-2x"></i>'
                                                runat="server" OnClick="SelectCurrentData" CommandArgument='<%# bind("id") %>'
                                                ToolTip="View Tasks">
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerStyle CssClass="PaginationClass" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>

        </div>
        <%--Block For Task View--%>
        <div id="block_View" class="card-body" runat="server" visible="false">
            <div class="form-group">
                <label class="label-title" id="lbl_ProjectView" runat="server" style="font-weight: bold">
                    Project
                </label>
                <label id="lbl_ProjectTextView" style="font-weight: normal" class="label-title" runat="server">
                </label>
                </br>
                    <label class="label-title" id="lbl_ScopeView" runat="server" style="font-weight: bold">
                        Scope
                    </label>
                <label id="lbl_ScopeTextView" style="font-weight: normal" runat="server" class="label-title">
                </label>
                </br>
                    <label class="label-title" id="lbl_StageView" runat="server" style="font-weight: bold">
                        Stage
                    </label>
                <label id="lbl_StageTextView" style="font-weight: normal" runat="server" class="label-title">
                </label>
                </br>
                    <label class="label-title" id="lbl_TaskView" runat="server" style="font-weight: bold">
                        Task
                    </label>
                <label id="lbl_TaskTextView" style="font-weight: normal" runat="server" class="label-title">
                </label>
                </br>
                    <label class="label-title" id="lbl_RequestDateView" runat="server" style="font-weight: bold">
                        Request Date
                    </label>
                <label id="lbl_RequestDateTextView" style="font-weight: normal" runat="server" class="label-title">
                </label>
                </br>
                    <label class="label-title" id="lbl_RequiredDateView" runat="server" style="font-weight: bold">
                        Required Date
                    </label>
                <label id="lbl_RequiredDateTextView" style="font-weight: normal" runat="server" class="label-title">
                </label>
                </br>
                    <label class="label-title" id="lbl_EstimatedHoursView" runat="server" style="font-weight: bold">
                        Estimated Hours
                    </label>
                <label id="lbl_EstimatedHoursTextView" style="font-weight: normal" runat="server"
                    class="label-title">
                </label>
                </br>
                    <label class="label-title" id="lbl_TimeTakenView" runat="server" style="font-weight: bold">
                        Time Taken
                    </label>
                <label id="lbl_TimeTakenTextView" style="font-weight: normal" runat="server" class="label-title">
                </label>
                </br>
                    <label class="label-title" id="lbl_RatingView" runat="server" style="font-weight: bold">
                        Rating For Task
                    </label>
                <div class="stars" style="width: 320px !important">
                    <form action="">
                        <input class="star star-5" id="star-5" type="radio" value="5" name="star" disabled="true" />
                        <label class="star star-5" for="star-5">
                        </label>
                        <input class="star star-4" id="star-4" type="radio" value="4" name="star" disabled="true" />
                        <label class="star star-4" for="star-4">
                        </label>
                        <input class="star star-3" id="star-3" type="radio" value="3" name="star" disabled="true" />
                        <label class="star star-3" for="star-3">
                        </label>
                        <input class="star star-2" id="star-2" type="radio" value="2" name="star" disabled="true" />
                        <label class="star star-2" for="star-2">
                        </label>
                        <input class="star star-1" id="star-1" type="radio" value="1" name="star" disabled="true" />
                        <label class="star star-1" for="star-1">
                        </label>
                    </form>
                    <asp:HiddenField ID="hdn_ScoredRating" runat="server" />
                </div>
                </br>
                    <label class="label-title" id="lbl_RatingQualityView" runat="server" style="font-weight: bold">
                        Rating For Quality
                    </label>
                <div class="stars" style="width: 320px !important">
                    <form action="">
                        <input class="star star-5" id="qualitystar-5" type="radio" value="5" name="star"
                            disabled="true" />
                        <label class="star star-5" for="qualitystar-5">
                        </label>
                        <input class="star star-4" id="qualitystar-4" type="radio" value="4" name="star"
                            disabled="true" />
                        <label class="star star-4" for="qualitystar-4">
                        </label>
                        <input class="star star-3" id="qualitystar-3" type="radio" value="3" name="star"
                            disabled="true" />
                        <label class="star star-3" for="qualitystar-3">
                        </label>
                        <input class="star star-2" id="qualitystar-2" type="radio" value="2" name="star"
                            disabled="true" />
                        <label class="star star-2" for="qualitystar-2">
                        </label>
                        <input class="star star-1" id="qualitystar-1" type="radio" value="1" name="star"
                            disabled="true" />
                        <label class="star star-1" for="qualitystar-1">
                        </label>
                    </form>
                    <asp:HiddenField ID="hdn_ScoredQualityRating" runat="server" />
                </div>
                </br>
                    <asp:Button CssClass="btn btn-info" runat="server" ID="btn_BackButton" Text="Back"
                        OnClick="btn_BackButton_Click" />
            </div>
            <%--Panel With DPR Details With Task--%>
            <div class="form-group">
                <asp:Panel ID="pnl_TaskDetails" runat="server">
                    <asp:GridView ID="grd_TaskDetails" runat="server" CssClass="datatable table table-striped dataTable"
                        AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="OnPageIndexChanging"
                        PageSize="5">
                        <Columns>
                            <%--<asp:TemplateField HeaderText="Task ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_task" runat="server" Text='<%# Eval("requestid") %>'></asp:Label>
                                            <asp:HiddenField ID="lbl_RequestNo" runat="server" Value='<%# Eval("requestid") %>'>
                                            </asp:HiddenField>
                                            <asp:HiddenField ID="hdn_Taskno" runat="server" Value='<%# Eval("id")%>'>
                                            </asp:HiddenField>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="Date">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_DateView" runat="server" Text='<%# Eval("CurrentDate") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Employee">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_EmployeeView" runat="server" Text='<%# Eval("Employee") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Start Time">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_StartTimeView" runat="server" Text='<%# Eval("StartTime") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="End Time">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_EndTimeView" runat="server" Text='<%# Eval("EndTime") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Break">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_BreakView" runat="server" Text='<%# Eval("BreakTime") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total Time">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_TotalTimeView" runat="server" Text='<%# Eval("TotalTime") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Remarks">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_RemarksView" runat="server" Text='<%# Eval("Remarks") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status" HeaderStyle-CssClass="GrdStatusOfTaskView">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_StatusOfTaskView" runat="server" Text='<%# Eval("statusoftask")+"%" %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
            </div>
        </div>
    </div>
    <script type="text/javascript">

        $(".navbar-expand-toggle").click(function () {
            var div1Class = $('.navbar-expand-toggle').attr('class');
            if (div1Class == "navbar-expand-toggle") {
                $("#pnl_RatingChart").css("margin-top", "8%")
                $(".card-body").css("height", "115px")
                if ($("#icon_Login").is(":visible") == false) {
                    $("#Side_Menu").show();
                }
            }
            else {
                $("#pnl_RatingChart").css("margin-top", "")
                $(".card-body").css("height", "")
                if ($("#icon_Login").is(":visible") == true) {
                    $("#Side_Menu").hide();
                }

            }
        });
        function loadLineChart() {
            var data = {};
            data.EmpID = $('select[id$=ddl_EmpNo]').val();
            data.Month = $('select[id$=ddl_FromMonth]').val();
            data.Year = $('select[id$=ddl_FromYear]').val();
            $.ajax({
                type: 'POST',
                data: JSON.stringify(data),
                url: "Dashboard.aspx/generateRatingChart1",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (data) {
                    var Dates = data.d[0]
                    var Rating = data.d[1]
                    var Quality = data.d[2]

                    $('#ContainerCodingStandard').highcharts({
                        chart: {
                            type: 'area',
                            spacingBottom: 30
                        },
                        title: {
                            text: 'Coding Standard Rating *'
                        },
                        subtitle: {
                            text: '* Dates',
                            floating: true,
                            align: 'right',
                            verticalAlign: 'bottom',
                            y: 15
                        },

                        xAxis: {
                            categories: Dates
                        },
                        yAxis: {
                            title: {
                                text: 'Rating'
                            },
                            labels: {
                                formatter: function () {
                                    return this.value;
                                }
                            }
                        },
                        tooltip: {
                            formatter: function () {
                                return '<b>' + this.series.name + '</b><br/>' +
                            this.x + ': ' + this.y;
                            }
                        },
                        plotOptions: {
                            area: {
                                fillOpacity: 0.5
                            }
                        },
                        credits: {
                            enabled: false
                        },
                        series: [
                        {
                            name: 'Coding Standard Rating', data: Rating
                        },
                        {
                            name: 'Quality Rating', color: 'orange', data: Quality
                        }
                        ]


                    });
                }
            });
        }
    </script>
    <%--Javascript Functions To Charts and Validations--%>
    <script>
        var url = "WebService.asmx/getChartsData";
        var app = angular.module('swpmis', []);
        var barChartData;

        app.controller('chartsController', function ($scope) {
            var userName;
            $.ajax({
                type: 'GET',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                url: url,
                success: function (data) {
                    var jsonData = JSON.parse(data.d);
                    $scope.values = {
                        yetToStart: jsonData["YetToStart"],
                        wip: jsonData["WIP"],
                        hold: jsonData["Hold"],
                        closed: jsonData["Closed"],
                        completed: jsonData["Completed"],
                        chartData: jsonData["ChartData"]
                    };
                    var yetToStart = parseFloat($scope.values.yetToStart);
                    var wip = parseFloat($scope.values.wip);
                    var hold = parseFloat($scope.values.hold);
                    var closed = parseFloat($scope.values.closed);
                    var completed = parseFloat($scope.values.completed);
                    barChartData = JSON.parse($scope.values.chartData);
                    $('#pieChart').highcharts({
                        chart: {
                            plotBackgroundColor: null,
                            plotBorderWidth: null,
                            plotShadow: false,
                            type: 'pie'
                        },
                        title: {
                            text: 'Task Status'
                        },
                        tooltip: {
                            pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                        },
                        plotOptions: {
                            pie: {
                                allowPointSelect: true,
                                cursor: 'pointer',
                                dataLabels: {
                                    enabled: false
                                },
                                showInLegend: true
                            }
                        },
                        series: [{
                            name: 'Tasks',
                            colorByPoint: true,
                            data: [{
                                name: 'WIP',
                                y: wip
                            }, {
                                name: 'Yet To Start',
                                y: yetToStart,
                            }, {
                                name: 'Completed',
                                y: completed
                            }, {
                                name: 'Hold',
                                y: hold
                            }, {
                                name: 'Closed',
                                y: closed
                            }]
                        }]
                    });
                    $('#ChartTasks').highcharts({
                        chart: {
                            type: 'column',
                            margin: 75,
                            options3d: {
                                enabled: true,
                                alpha: 10,
                                beta: 25,
                                depth: 70
                            }
                        },
                        title: {
                            text: 'Monthwise Task Report <%=DateTime.Now.Year%>'
                        },
                        subtitle: {
                            text: 'Software Team'
                        },
                        plotOptions: {
                            column: {
                                depth: 25
                            }
                        },
                        xAxis: {
                            categories: Highcharts.getOptions().lang.shortMonths
                        },
                        yAxis: {
                            title: {
                                text: null
                            }
                        },
                        series: [{
                            name: 'Tasks',
                            data: barChartData
                        }]
                    });

                },
                error: function (error) { }
            });
        });
    </script>
    <script type="text/javascript">
        function getID(value) {
            $('lbl_TaskID').val = value;
        }
        $('#<%=ddl_FromMonth.ClientID %>').change(function () {
            loadLineChart();
        });
        $('#<%=ddl_EmpNo.ClientID %>').change(function () {
            loadLineChart();
        });
        $('#<%=ddl_FromYear.ClientID %>').change(function () {
            loadLineChart();
        });

    </script>
    <script type="text/javascript">
        jQuery(function ($) {
            $('.panel-heading span.clickable').on("click", function (e) {

                if ($(this).hasClass('panel-collapsed')) {
                    // expand the panel
                    $(this).parents('.panel').find('.panel-body').slideDown();
                    $(this).removeClass('panel-collapsed');
                    $(this).find('i').removeClass('glyphicon-chevron-down').addClass('glyphicon-chevron-up');

                }
                else {
                    // collapse the panel
                    if ($(this).find('i').attr('class') == "glyphicon glyphicon-chevron-down") {
                        $(this).parents('.panel').find('.panel-body').slideDown();
                        $(this).removeClass('panel-collapsed');
                        $(this).find('i').removeClass('glyphicon-chevron-down').addClass('glyphicon-chevron-up');
                    }
                    else {
                        $(this).parents('.panel').find('.panel-body').slideUp();
                        $(this).addClass('panel-collapsed');
                        $(this).find('i').removeClass('glyphicon-chevron-up').addClass('glyphicon-chevron-down');
                    }
                }
            });
        });

    </script>
</asp:Content>
