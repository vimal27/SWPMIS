<%@ Page Title="Status" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" AutoEventWireup="true"
    CodeFile="Status.aspx.cs" Inherits="YetToStart" %>

<%-- Ajax Toolkit For Ajax Calendar Extender --%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .label-title {
            width: 40% !important;
        }

        .datatable tbody {
            text-align: center !important;
        }

        .datatable thead th {
            text-align: center !important;
        }

        .modal-backdrop.in {
            opacity: 0.4 !important;
        }

        td.column_style_right {
            border-right: 1px solid black;
        }

        .progress {
            background-color: #ddd;
        }

        input[type=radio] + label {
            cursor: not-allowed !important;
        }
    </style>
    <%-- External css Files to Font Awesome and bootstrap Datatable --%>
    <link rel="stylesheet" type="text/css" href="assets/css/Datatable/dataTables.bootstrap.min.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="card">
        <%-- Header --%>
        <div class="card-header">
            <div class="card-title">
                <div class="title" id="TitleOfPage" runat="server">
                </div>
                <asp:TextBox ID="txt_Status" Visible="false" runat="server"></asp:TextBox>
            </div>
            <asp:DropDownList ID="ddl_FilterYear" CssClass="form-control" AutoPostBack="true"
                Style="width: 9%; float: right; margin-top: 1%" OnSelectedIndexChanged="ddl_FilterYear_SelectedIndexChanged"
                runat="server">
            </asp:DropDownList>
            <asp:Label ID="lbl_SeparatorMonthYear" Style="float: right; margin-top: 2%" runat="server"
                Text="  /  "></asp:Label>
            <asp:DropDownList ID="ddl_FilterMonth" CssClass="form-control" AutoPostBack="true"
                OnSelectedIndexChanged="ddl_FilterMonth_SelectedIndexChanged" Style="width: 9%; float: right; margin-top: 1%"
                runat="server">
            </asp:DropDownList>
        </div>
        <%-- Main content --%>
        <div class="card-body">
            <%--Block For Tasks GridView--%>
            <div id="block_Grid" runat="server" style="float: right; width: 100%; overflow-x: auto">
                <asp:Button ID="btn_ExportToExcel" runat="server" Text="Export" OnClick="btn_ExportToExcel_Click"
                    CssClass="btn btn-info" />
                <asp:GridView ID="grd_WIP" runat="server" Style="overflow-x: auto !important;" CssClass="dataTables_wrapper dt-foundation display datatable"
                    AutoGenerateColumns="False" DataKeyNames="id" OnSelectedIndexChanged="grd_WIP_SelectedIndexChanged"
                    OnRowDataBound="grd_WIP_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Task By">
                            <ItemTemplate>
                                <asp:Label ID="lbl_TaskBy" runat="server" Text='<%# Eval("username") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Project">
                            <ItemTemplate>
                                <asp:Label ID="lbl_Project" runat="server" Text='<%# Eval("Project") %>'></asp:Label>
                                <asp:HiddenField ID="lbl_RequestNo" runat="server" Value='<%# Eval("requestid") %>'></asp:HiddenField>
                                <asp:HiddenField ID="hdn_projectid" runat="server" Value='<%# Eval("projectid")%>'></asp:HiddenField>
                                <asp:HiddenField ID="hdn_taskid" runat="server" Value='<%# Eval("id")%>'></asp:HiddenField>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Task">
                            <ItemTemplate>
                                <asp:Label ID="lbl_Task" runat="server" Text='<%# Eval("taskname") %>'></asp:Label>
                                <asp:HiddenField ID="hdn_TaskDescription" runat="server" Value='<%# Eval("taskdescription")%>'></asp:HiddenField>
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
                        <asp:TemplateField HeaderText="Productivity">
                            <ItemTemplate>
                                <div class="progress" style="margin-top: 10%">
                                    <div class="progress-bar progress-bar-success progress-bar-striped" id="blk_ProgressBar"
                                        runat="server" role="progressbar" aria-valuenow='<%# Eval("Production") %>' aria-valuemin="0"
                                        aria-valuemax="140">
                                        <asp:Label ID="lbl_Production" runat="server" Text='<%# Eval("Production") %>'>
                                        </asp:Label>
                                    </div>
                                </div>
                                <asp:Label ID="lbl_Rating" runat="server" Style="text-weight: bold" Text="Rating">
                                </asp:Label>
                                <span id="spn_RatingForCodingStandard" class="fa-stack fa-lg" runat="server" title="Rating For Coding Standard"
                                    style="width: 10%; color: #5bc0de; cursor: pointer"><i class="fa fa-circle fa-stack-2x"></i><i id="icon_CodingStandardRating" runat="server" class="fa fa-inverse fa-stack-1x"
                                        style="font-size: 10.5px; margin-left: 50%; cursor: pointer">NA</i></span>
                                <span id="spn_RatingForQuality" class="fa-stack fa-lg" runat="server" title="Rating For Quality"
                                    style="margin-left: 15%; width: 10%; color: #eea236; cursor: pointer"><i class="fa fa-circle fa-stack-2x"></i><i id="icon_QualityRating" runat="server" class="fa fa-inverse fa-stack-1x" style="font-size: 10.5px; margin-left: 50%; cursor: pointer"
                                        tooltip="Rating For Quality">NA</i></span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>
                                <asp:LinkButton ToolTip="Select" ID="lnk_TaskStatus" runat="server" CommandName="Select"></asp:LinkButton>
                                <asp:LinkButton ID="lnk_View" Style="color: rgba(159, 0, 147, 0.62)" Text='<i class="glyphicon glyphicon-eye-open fa-2x"'
                                    runat="server" OnClick="SelectCurrentData" CommandArgument='<%# bind("id") %>'
                                    ToolTip="View Tasks">
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerStyle CssClass="PaginationClass" />
                </asp:GridView>
            </div>
            <%--Block For Projects GridView--%>
            <div class="form-group">
                <asp:GridView ID="grd_Projects" runat="server" CssClass="datatable table table-striped dataTable"
                    AutoGenerateColumns="False" DataKeyNames="projectid" AllowPaging="true" OnPageIndexChanging="OnPageIndexChangingProject"
                    PageSize="10" OnRowDataBound="grd_Projects_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Request ID" HeaderStyle-CssClass="firstcolumn">
                            <ItemTemplate>
                                <asp:Label ID="lbl_ProjectID" runat="server" Text='<%# Eval("projectid") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="firstcolumn"></HeaderStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Project ID" HeaderStyle-CssClass="ProjectID">
                            <ItemTemplate>
                                <asp:Label ID="lbl_ClientProjectID" runat="server" Text='<%# Eval("manualid") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Project Name" HeaderStyle-CssClass="ProjectName">
                            <ItemTemplate>
                                <asp:Label ID="lbl_ProjectName" runat="server" Text='<%# Eval("projectname") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Project Type">
                            <ItemTemplate>
                                <asp:Label ID="lbl_ProjectType" runat="server" Text='<%# Eval("typeproject") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Alloted Teams" HeaderStyle-CssClass="AllotedTeams">
                            <ItemTemplate>
                                <asp:Label ID="lbl_AllotedTeamName" runat="server" Text='<%# Eval("allotedteamname") %>'></asp:Label>
                                <asp:HiddenField ID="hdn_AllotedTeamName" runat="server" Value='<%# Eval("allotedteamname") %>'></asp:HiddenField>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Received Date" HeaderStyle-CssClass="ReceivedDate">
                            <ItemTemplate>
                                <asp:Label ID="lbl_ReceivedDate" runat="server" Text='<%# Eval("receiveddate") %>'></asp:Label>
                                <asp:HiddenField ID="hdn_Description" runat="server" Value='<%# Eval("ProjectDesc") %>' />
                                <asp:HiddenField ID="hdn_Remark" runat="server" Value='<%# Eval("remarks") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Due Date" HeaderStyle-CssClass="DueDate">
                            <ItemTemplate>
                                <asp:Label ID="lbl_DueDate" runat="server" Text='<%# Eval("DueDate2")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Completed Date" HeaderStyle-CssClass="CompletedDate">
                            <ItemTemplate>
                                <asp:Label ID="lbl_CompletedDate" runat="server" Text='<%# ViewState["CompletedDate"]%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Status" HeaderStyle-CssClass="lastcolumn">
                            <ItemTemplate>
                                <asp:LinkButton Style="width: 100%; cursor: default" ID="lnk_ProjectStatus" runat="server"
                                    Text='<%# Eval("projectstatus") %>' CommandArgument='<%# bind("projectid") %>'
                                    OnClick="lnk_ProjectStatus_Click"></asp:LinkButton>
                            </ItemTemplate>
                            <HeaderStyle CssClass="lastcolumn"></HeaderStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Edit" HeaderStyle-CssClass="removecolumn">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnk_View" runat="server" OnClick="SelectCurrentProjectData" CommandArgument='<%# bind("projectid") %>'
                                    ToolTip="View Project" Text='<i class="glyphicon glyphicon-eye-open"></i>' ForeColor="#19B5FE"
                                    Font-Size="Large">                    
                                </asp:LinkButton>
                                <asp:LinkButton ID="lnk_Delete" TabIndex="4" runat="server" CommandArgument='<%# bind("projectid") %>'
                                    ToolTip="Delete" OnClick="lnk_Delete_Click" Text='<i class="glyphicon glyphicon-trash"></i>'
                                    ForeColor="#D45E5E" Font-Size="Large">    
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                            </EditItemTemplate>
                            <HeaderStyle CssClass="removecolumn"></HeaderStyle>
                        </asp:TemplateField>
                    </Columns>
                    <PagerStyle CssClass="PaginationClass" />
                </asp:GridView>
                <asp:Button ID="btn_BackToDashboard" runat="server" Text="Back" OnClick="btn_BackToDashboard_Click"
                    CssClass="btn btn-info" />
                <%--Block For Popup While Delete--%>
                <asp:LinkButton Text="" ID="lnk_DeletePopup" runat="server" />
                <cc1:ModalPopupExtender ID="popup_Delete" PopupControlID="pnl_ProjectDel" TargetControlID="lnk_DeletePopup"
                    runat="server">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="pnl_ProjectDel" CssClass="panel panel-primary" runat="server" Style="display: none; width: 40%; height: 40%">
                    <div class="panel-heading">
                        Confirm Delete?
                    </div>
                    <div class="panel-body">
                        <div class="form-group" style="margin-left: 10%">
                            <label class="label-title">
                                Reason</label>
                            <asp:TextBox ID="txt_Reason" class="form-control" Width="50%" runat="server" TextMode="MultiLine"
                                ValidationGroup="Delete"></asp:TextBox><span style="color: #ff0000"> **</span>
                            <asp:RequiredFieldValidator ID="rf_Reason" runat="server" ErrorMessage="Reason is Mandatory"
                                Display="Dynamic" ValidationGroup="Delete" ControlToValidate="txt_Reason"></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group" style="margin-left: 10%">
                            <label class="label-title">
                                Status</label>
                            <asp:DropDownList ID="ddl_DelStatus" CssClass="form-control" Width="50%" runat="server">
                                <asp:ListItem Value="Hold">Hold</asp:ListItem>
                                <asp:ListItem Value="Closed">Closed</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="form-group" style="margin-left: 10%">
                            <asp:Button ID="btn_DeleteProject" class="btn btn-info" ValidationGroup="Delete"
                                runat="server" OnClick="btn_DeleteProject_Click" Text="Submit" />
                            <asp:Button ID="btn_Cancel" class="btn btn-danger" runat="server" OnClick="btn_Close"
                                Text="Cancel" />
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <%--Block For Popup While Change Project To Completed State--%>
            <asp:LinkButton Text="" ID="lnk_CompletedPopup" runat="server" />
            <cc1:ModalPopupExtender ID="popup_CompletedState" PopupControlID="pnl_CompletedState"
                TargetControlID="lnk_CompletedPopup" runat="server">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="pnl_CompletedState" CssClass="panel panel-primary" runat="server"
                Style="display: none; width: 40%; height: 40%">
                <div class="panel-heading">
                    Confirm Completed?
                </div>
                <div class="panel-body">
                    <div class="form-group" style="margin-left: 10%">
                        <label class="label-title">
                            State</label>
                        <asp:DropDownList ID="ddl_ProjectState" AutoPostBack="true" OnSelectedIndexChanged="ddl_ProjectState_SelectedIndexChanged"
                            Width="50%" CssClass="form-control" runat="server">
                            <asp:ListItem Value="Completed">Completed</asp:ListItem>
                            <asp:ListItem Value="WIP">WIP</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group" style="margin-left: 10%">
                        <label class="label-title" runat="server" id="lbl_CompletedDate">
                            Date</label>
                        <asp:TextBox ID="txt_CompletedDate" Width="50%" OnTextChanged="txt_CompletedDate_TextChanged"
                            runat="server" AutoPostBack="true" class="form-control" TabIndex="4" ValidationGroup="Complete"></asp:TextBox>
                        <cc1:CalendarExtender ID="calext_CompletedDate" CssClass="custom-calendar" PopupPosition="TopLeft"
                            runat="server" TargetControlID="txt_CompletedDate" Format="dd/MM/yyyy">
                        </cc1:CalendarExtender>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Completed Date is Mandatory"
                            Display="Dynamic" ValidationGroup="Complete" ControlToValidate="txt_CompletedDate"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group" style="margin-left: 10%">
                        <asp:Button ID="btn_CompleteProject" OnClick="btn_CompleteProject_Click" class="btn btn-info"
                            ValidationGroup="Complete" runat="server" Text="Submit" />
                        <asp:Button ID="btn_Exit" class="btn btn-danger" runat="server" Text="Cancel" />
                    </div>
                </div>
            </asp:Panel>
        </div>
        <%--Block For Task View--%>
        <%--<asp:UpdatePanel ID="upl_ProjectsView" runat="server">
            <ContentTemplate>--%>
                <div id="block_View" class="card-body" runat="server" visible="false">
                    <div class="form-group">
                        <label class="label-title" id="lbl_TaskByView" runat="server" style="font-weight: bold">
                            Task By
                        </label>
                        <label id="lbl_TaskByTextView" style="font-weight: normal" class="label-title" runat="server">
                        </label>
                        </br>
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
                <label class="label-title" id="lbl_TaskDescriptionView" runat="server" style="font-weight: bold">
                    Task Description
                </label>
                        <label id="lbl_TaskDescriptionTextView" style="font-weight: normal" runat="server"
                            class="label-title">
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
                    Rating For Coding Standard
                </label>
                        <div class="stars" style="width: 320px !important;">
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
                <asp:Button runat="server" ID="btn_BackButton" Text="Back" OnClick="btn_BackButton_Click"
                    CssClass="btn btn-info" Style="margin-left: 5%" />
                    </div>
                    </br>
            <div class="card-header">
                <div class="card-title">
                    <div class="title" id="Div1" runat="server">
                        DPR's
                    </div>
                </div>
            </div>
                    <%-- Task Details Gridview --%>
                    <div class="form-group">
                        <asp:Panel ID="pnl_TaskDetails" runat="server" Style="width: 100%; overflow-x: auto;">
                            <asp:GridView ID="grd_TaskDetails" runat="server" CssClass="datatable table table-striped dataTable"
                                AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="OnPageIndexChanging"
                                PageSize="10">
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
                        <%-- Tasks Holds Details --%>
                        <div class="card-header">
                            <div class="card-title">
                                <div class="title" id="title_TaskHolds" visible="false" runat="server">
                                    Task Holds
                                </div>
                            </div>
                        </div>
                        <div class="card-body">
                            <asp:Panel ID="pnl_TaskHolds" runat="server" Style="width: 100%; overflow-x: auto;">
                                <asp:GridView ID="grd_TaskHolds" runat="server" Visible="false" CssClass="datatable table table-striped dataTable"
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
                                        <asp:TemplateField HeaderText="Holded Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_HoldedDate" runat="server" Text='<%# Eval("holdeddate", "{0:dd/MM/yyyy}") %>'>></asp:Label>
                                                <asp:HiddenField ID="hdn_HoldID" runat="server" Value='<%# Eval("id") %>'></asp:HiddenField>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Released Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_ReleasedDate" runat="server" Text='<%# Eval("wipdate", "{0:dd/MM/yyyy}") %>'></asp:Label>
                                                <asp:HiddenField ID="hdn_TaskID" runat="server" Value='<%# Eval("taskid") %>'></asp:HiddenField>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
                            <asp:Label ID="lbl_ClosedDetails" runat="server" Text=""></asp:Label>
                        </div>
                    </div>
                </div>
           <%-- </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btn_BackButton" />
            </Triggers>
        </asp:UpdatePanel>--%>
        <%--Block For Projects View--%>
        <div id="block_ProjectsView" class="card-body" runat="server" visible="false">
            <div class="form-group">
                <label class="label-title" id="lbl_ProjectIDView" runat="server" style="font-weight: bold">
                    Request ID
                </label>
                <label class="label-title" id="lbl_ProjectIDTextView" style="font-weight: normal"
                    runat="server">
                </label>
            </div>
            <div class="form-group">
                <label class="label-title" id="lbl_ProjectNameView" runat="server" style="font-weight: bold">
                    Project Name
                </label>
                <label id="lbl_ProjectNameTextView" style="font-weight: normal" class="label-title"
                    runat="server">
                </label>
            </div>
            <div class="form-group">
                <label class="label-title" id="lbl_ManualIDView" runat="server" style="font-weight: bold">
                    Project ID
                </label>
                <label id="lbl_ManualIDTextView" style="font-weight: normal" runat="server" class="label-title">
                </label>
            </div>
            <div class="form-group">
                <label class="label-title" id="lbl_ProjectTypeView" runat="server" style="font-weight: bold">
                    Project Type
                </label>
                <label id="lbl_ProjectTypeTextView" style="font-weight: normal" runat="server" class="label-title">
                </label>
            </div>
            <div class="form-group">
                <label class="label-title" id="lbl_AllotedTeamView" runat="server" style="font-weight: bold">
                    Alloted Teams
                </label>
                <label id="lbl_AllotedTeamTextView" style="font-weight: normal" class="label-title"
                    runat="server">
                </label>
            </div>
            <div class="form-group">
                <label class="label-title" id="lbl_ReceivedDateView" runat="server" style="font-weight: bold">
                    Received Date
                </label>
                <label id="lbl_ReceivedDateTextView" style="font-weight: normal" runat="server" class="label-title">
                </label>
            </div>
            <div class="form-group">
                <label class="label-title" id="lbl_DueDateView" runat="server" style="font-weight: bold">
                    Due Date
                </label>
                <label id="lbl_DueDateTextView" style="font-weight: normal" runat="server" class="label-title">
                </label>
            </div>
            <div class="form-group">
                <label class="label-title" id="lbl_ProjectDescView" runat="server" style="font-weight: bold">
                    Project Description
                </label>
                <label id="lbl_ProjectDescTextView" style="font-weight: normal" runat="server" class="label-title">
                </label>
            </div>
            <div class="form-group">
                <label class="label-title" id="lbl_RemarksView" runat="server" style="font-weight: bold">
                    Remarks
                </label>
                <label id="lbl_RemarksTextView" style="font-weight: normal" runat="server" class="label-title">
                </label>
            </div>
            <div class="form-group">
                <asp:Button class="btn btn-info" runat="server" ID="btn_ProjectBackButton" OnClick="btn_ProjectBackButton_Click"
                    Text="Back" />
            </div>
        </div>
    </div>
    <%-- External JS To Jquery,Bootstrap,Datatable --%>
    <script type="text/javascript" charset="utf8" src="assets/js/Datatable/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="assets/js/Datatable/dataTables.bootstrap.min.js"></script>
    <script type="text/javascript">
                        
                        
    </script>
    <script type="text/javascript">
        $(function () {
            // Setup - add a text input to each footer cell
            $("#ctl00_ContentPlaceHolder1_grd_WIP").prepend($("<thead></thead><tfoot></tfoot>").append($("#ctl00_ContentPlaceHolder1_grd_WIP").find("tr:first")));

            $('#ctl00_ContentPlaceHolder1_grd_WIP tfoot th').each(function () {
                var title = $(this).text();
                if (title != "Actions") {
                    $(this).replaceWith('<td><input type="text" class="form-control" style="width:100%" placeholder="Search ' + title + '" /></td>');
                }
                else {
                    $(this).replaceWith('<td></td>');
                }
            });
            var table = $("#ctl00_ContentPlaceHolder1_grd_WIP").dataTable({
                "lengthMenu": [[6, 10, 15, -1], [6, 10, 15, "All"]],
                "aaSorting": [],
                "deferRender": true,
                "sDom": 'C<"clear">lfrtip'
            });
            table.api().columns().every(function () {
                var that = this;
                $('input', this.footer()).on('keyup change', function () {
                    if (that.search() !== this.value) {
                        that
                            .search(this.value)
                            .draw();
                    }
                });
            });
        });
    </script>
    <script type="text/javascript">
        $(function () {
            // Setup - add a text input to each footer cell
            $("#ctl00_ContentPlaceHolder1_grd_TaskDetails").prepend($("<thead></thead>").append($("#ctl00_ContentPlaceHolder1_grd_TaskDetails").find("tr:first")));
            var table = $("#ctl00_ContentPlaceHolder1_grd_TaskDetails").dataTable({
                "deferRender": true,
                "pageLength": 5,
                "sDom": '<"top"i>rt<"bottom"p><"clear">',
                "ordering": false,
                "info": false
            });
        });
    </script>
</asp:Content>
