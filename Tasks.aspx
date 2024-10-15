<%@ Page Title="Tasks" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Tasks.aspx.cs" Inherits="Tasks" Culture="en-GB" EnableEventValidation="false" %>

<%-- Ajax Control Toolkit for ajax calendar extender --%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%-- Inline CSS --%>
    <%--<script type="text/javascript" src="//ajax.googleapis.com/ajax/libs/jquery/1.11.2/jquery.min.js"></script>--%>
    <script type="text/javascript">
        var isSubmitted = false;
        function preventMultipleSubmissions() {
            if (document.getElementById("<%=txt_TaskName.ClientID%>").value == "") {
                swal("Task name must not be empty", '', 'warning');
                document.getElementById("<%=txt_TaskName.ClientID%>").focus();
                return false;
            }
            else if (document.getElementById("<%=txt_TaskDescription.ClientID%>").value == "") {
                swal("Task description must not be empty", '', 'warning');
                document.getElementById("<%=txt_TaskDescription.ClientID%>").focus();
                return false;
            }
            else if (document.getElementById("<%=ddl_Project.ClientID%>").selectedIndex == 0) {
                swal("Select Project", '', 'warning');
                document.getElementById("<%=ddl_Project.ClientID%>").focus();
                return false;
            }
            else if (document.getElementById("<%=ddl_Stage.ClientID%>").selectedIndex == 0) {
                swal("Select Stage", '', 'warning');
                document.getElementById("<%=ddl_Stage.ClientID%>").focus();
                return false;
            }
            else if (document.getElementById("<%=ddl_Scope.ClientID%>").selectedIndex == 0) {
                swal("Select Scope", '', 'warning');
                document.getElementById("<%=ddl_Scope.ClientID%>").focus();
                return false;
            }
            else if (document.getElementById("<%=ddl_Team.ClientID%>").selectedIndex == 0) {
                swal("Select Team", '', 'warning');
                document.getElementById("<%=ddl_Team.ClientID%>").focus();
                return false;
            }
            else if (document.getElementById("<%=ddl_Users.ClientID%>").selectedIndex == 0) {
                swal("Select User", '', 'warning');
                document.getElementById("<%=ddl_Users.ClientID%>").focus();
                return false;
            }
            else if (document.getElementById("<%=txt_RequestDate.ClientID%>").value == "") {
                swal("Request date must not be empty", '', 'warning');
                document.getElementById("<%=txt_RequestDate.ClientID%>").focus();
                return false;
            }
            else if (document.getElementById("<%=txt_RequiredDate.ClientID%>").value == "") {
                swal("Required date must not be empty", '', 'warning');
                document.getElementById("<%=txt_RequiredDate.ClientID%>").focus();
                return false;
            }
            else if (document.getElementById("<%=txt_EstimatedHours.ClientID%>").value.indexOf("_") != "-1") {
                swal("Estimated hours is not valid", '', 'warning');
                document.getElementById("<%=txt_EstimatedHours.ClientID%>").focus();
                return false;
            }
            else if (!isSubmitted) {
                $('#<%=btn_Allot.ClientID %>').val('Submitting.. Plz Wait..');
                isSubmitted = true;
                return true;
            }
            else {
                return false;
            }
        }
    </script>


    <style type="text/css">
        .datatable tbody {
            text-align: center !important;
        }

        .datatable thead th {
            text-align: center !important;
        }

        .modal-backdrop.in {
            opacity: 0.4 !important;
        }

        .progress {
            background-color: #ddd;
        }
    </style>
    <%-- External CSS For Font Awesome,Bootstrap Datatable --%>
    <link rel="stylesheet" type="text/css" href="assets/css/Datatable/dataTables.bootstrap.min.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%-- <div>
        <div class="row">
            <div class="col-md-12">--%>
    <div class="card">
        <%--Heading Of MainContent--%>
        <div class="card-header">
            <div class="card-title">
                <div class="title" id="TitleOfPage" runat="server">
                    Tasks
                </div>
            </div>
            <asp:PlaceHolder ID="ph_Filters" runat="server">
                <%--<asp:UpdatePanel ID="upl_Filter" runat="server" UpdateMode="Conditional">
                <ContentTemplate>--%>
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
                <asp:DropDownList ID="ddl_FilterProject" CssClass="form-control" AutoPostBack="true"
                    Style="width: 40%; float: right; margin-top: 1%; margin-right: 1%" OnSelectedIndexChanged="ddl_FilterProject_SelectedIndexChanged"
                    runat="server">
                </asp:DropDownList>
            </asp:PlaceHolder>
            <%--</ContentTemplate>
                <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddl_FilterMonth" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="ddl_FilterYear" EventName="SelectedIndexChanged" />
                </Triggers>
            </asp:UpdatePanel>--%>
        </div>
        <%--Main Content,Block For GridView Tasks--%>
        <asp:UpdatePanel ID="upl_All" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="card-body" id="block_Grid" runat="server">
                    <%-- New Task,Export To Excel Button --%>
                    <asp:Button ID="btn_newTask" class="btn btn-success" TabIndex="1" OnClick="btn_newTask_Click"
                        Text="Add New Task" runat="server" />
                    <asp:Button ID="btn_Export" class="btn btn-info" runat="server" TabIndex="2" CausesValidation="false"
                        Text="Export" OnClick="btn_Export_Click" />
                    <%--GridView For Tasks--%>
                    <div class="form-group">
                        <asp:Panel ID="pnl_Tasks" runat="server">
                            <asp:GridView ID="grd_Tasks" runat="server" CssClass="dataTables_wrapper dt-foundation display datatable"
                                AutoGenerateColumns="False" DataKeyNames="requestid,id" OnRowCancelingEdit="grd_Tasks_RowCancelingEdit"
                                OnRowUpdating="grd_Tasks_RowUpdating" OnRowDeleting="grd_Tasks_RowDeleting" OnSelectedIndexChanged="grd_Tasks_SelectedIndexChanged"
                                OnRowDataBound="grd_Tasks_RowDataBound">
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
                                    <asp:TemplateField HeaderText="Project ID" HeaderStyle-CssClass="firstcolumn">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Project" runat="server" Text='<%# Eval("Project") %>'></asp:Label>
                                            <asp:HiddenField ID="lbl_RequestNo" runat="server" Value='<%# Eval("requestid") %>'></asp:HiddenField>
                                            <asp:HiddenField ID="hdn_projectid" runat="server" Value='<%# Eval("projectid")%>'></asp:HiddenField>
                                            <asp:HiddenField ID="hdn_taskid" runat="server" Value='<%# Eval("id")%>'></asp:HiddenField>
                                            <asp:HiddenField ID="hdn_EstimatedHours" runat="server" Value='<%# Eval("estimatedtime")%>'></asp:HiddenField>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Task" HeaderStyle-CssClass="TaskName">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Task" runat="server" Text='<%# Eval("taskname") %>'></asp:Label>
                                            <asp:HiddenField ID="hdn_TaskDescription" runat="server" Value='<%# Eval("taskdescription")%>'></asp:HiddenField>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField HeaderText="Task Description">
                                        <FooterTemplate>
                                            <asp:TextBox ID="txt_TaskDescriptionFilter" CssClass="form-control" Width="100%"
                                                runat="server"></asp:TextBox>
                                        </FooterTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_TaskDescription" runat="server" Text='<%# Eval("taskdescription") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="Alloted To" HeaderStyle-CssClass="AllotedTo">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Users" runat="server" Text='<%# Eval("Username") %>'></asp:Label>
                                            <asp:HiddenField ID="hdn_UserID" runat="server" Value='<%# Eval("userid")%>'></asp:HiddenField>
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
                                    <asp:TemplateField HeaderText="Completed Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_CompletedDate" runat="server" Text='<%# ViewState["CompletedDate"]%>'></asp:Label>
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
                                    <asp:TemplateField HeaderText="Task Status" HeaderStyle-CssClass="lastcolumn">
                                        <ItemTemplate>
                                            <asp:LinkButton Style="width: 100%; cursor: default" ID="lnk_TaskStatus" runat="server"
                                                Text='<%# Eval("taskstatus") %>' CommandArgument='<%# bind("id") %>' OnClick="lnk_TaskStatus_Click"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Edit" HeaderStyle-CssClass="removecolumn">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnk_Edit" TabIndex="3" runat="server" CommandName="Select" ToolTip="Select"
                                                Text='<i class="fa fa-pencil-square-o"></i>' ForeColor="#5AC15A" Font-Size="23px">
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="lnk_View" runat="server" OnClick="SelectCurrentData" CommandArgument='<%# bind("id") %>'
                                                ToolTip="View Tasks" Text='<i class="glyphicon glyphicon-eye-open"></i>' ForeColor="#19B5FE"
                                                Font-Size="Large">                    
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="lnk_Delete" OnClientClick="confirmBox(event,'Are you sure want to hold the task?')"
                                                TabIndex="4" runat="server" CommandName="Delete" ToolTip="Hold" Text='<i class="glyphicon glyphicon-pause"></i>'
                                                ForeColor="#D45E5E" Font-Size="Large">                    
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:LinkButton ID="lnk_Update" runat="server" CommandName="Update" ToolTip="Update Status"
                                                Text='<i class="glyphicon glyphicon-saved"></i>' ForeColor="#5AC15A" Font-Size="Large">                    
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="lnk_Cancel" runat="server" CommandName="Cancel" ToolTip="Cancel Status"
                                                Text='<i class="glyphicon glyphicon-remove"></i>' ForeColor="#5E5EDE" Font-Size="Large">                    
                                            </asp:LinkButton>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerStyle CssClass="PaginationClass" />
                            </asp:GridView>
                            <%--Block For Popup While Change Project To Completed State--%>
                            <asp:LinkButton Text="" ID="lnk_HoldPopup" runat="server" />
                            <cc1:ModalPopupExtender ID="popup_HoldState" PopupControlID="pnl_HoldState" TargetControlID="lnk_HoldPopup"
                                BackgroundCssClass="modalBackground" runat="server">
                            </cc1:ModalPopupExtender>
                            <asp:Panel ID="pnl_HoldState" CssClass="modalPopup" runat="server" Style="display: none; width: 30%; height: 30%">
                                <div class="panel-heading">
                                    Change State?
                                </div>
                                <div class="panel-body">
                                    <div class="form-group">
                                        <div class="left-inner-addon">
                                            <label class="label-title">
                                                Status</label>
                                            <i class="glyphicon glyphicon-equalizer"></i>
                                            <asp:DropDownList Width="40%" ID="ddl_TaskStateChange" AutoPostBack="true" class="form-control"
                                                runat="server" OnSelectedIndexChanged="ddl_TaskStateChange_SelectedIndexChanged">
                                                <asp:ListItem Value="WIP">Active</asp:ListItem>
                                                <asp:ListItem Value="Closed">Closed</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <asp:Button ID="btn_WIPProject" Width="100%" OnClick="btn_WIPProject_Click" class="btn btn-info"
                                            ValidationGroup="WIP" runat="server" Text="OK" />
                                        <asp:Button ID="btn_Exit" Width="100%" OnClick="btn_Exit_Click" class="btn btn-danger"
                                            runat="server" Text="Cancel" />
                                    </div>
                                </div>
                            </asp:Panel>
                        </asp:Panel>
                    </div>
                </div>
                <%--Block For Task View--%>
                <div id="block_View" class="card-body" runat="server" visible="false">
                    <div class="form-group">
                        <label class="label-title" id="lbl_TaskIDView" runat="server" style="font-weight: bold">
                            Task ID
                        </label>
                        <label id="lbl_TaskIDTextView" style="font-weight: normal" class="label-title" runat="server">
                        </label>
                        </br>
                        <label class="label-title" id="lbl_TasknameView" runat="server" style="font-weight: bold">
                            Task Name
                        </label>
                        <label id="lbl_TaskNameTextView" style="font-weight: normal" class="label-title"
                            runat="server">
                        </label>
                        </br>
                        <label class="label-title" id="lbl_TaskDescriptionView" runat="server" style="font-weight: bold">
                            Task Description
                        </label>
                        <label id="lbl_TaskDescriptionTextView" style="font-weight: normal" class="label-title"
                            runat="server">
                        </label>
                        </br>
                        <label class="label-title" id="lbl_ProjectView" runat="server" style="font-weight: bold">
                            Project
                        </label>
                        <label id="lbl_ProjectTextView" style="font-weight: normal" class="label-title" runat="server">
                        </label>
                        </br>
                        <label class="label-title" id="lbl_StageView" runat="server" style="font-weight: bold">
                            Stage
                        </label>
                        <label id="lbl_StageTextView" style="font-weight: normal" runat="server" class="label-title">
                        </label>
                        </br>
                        <label class="label-title" id="lbl_ScopeView" runat="server" style="font-weight: bold">
                            Scope
                        </label>
                        <label id="lbl_ScopeTextView" style="font-weight: normal" runat="server" class="label-title">
                        </label>
                        </br>
                        <label class="label-title" id="lbl_AllotedToView" runat="server" style="font-weight: bold">
                            Alloted To
                        </label>
                        <label id="lbl_AllotedToTextView" style="font-weight: normal" runat="server" class="label-title">
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
                        <div class="stars label-title" style="width: 320px !important">
                            <form action="">
                                <input class="star star-5" id="star-5" type="radio" value="5" name="star" />
                                <label class="star star-5" for="star-5">
                                </label>
                                <input class="star star-4" id="star-4" type="radio" value="4" name="star" />
                                <label class="star star-4" for="star-4">
                                </label>
                                <input class="star star-3" id="star-3" type="radio" value="3" name="star" />
                                <label class="star star-3" for="star-3">
                                </label>
                                <input class="star star-2" id="star-2" type="radio" value="2" name="star" />
                                <label class="star star-2" for="star-2">
                                </label>
                                <input class="star star-1" id="star-1" type="radio" value="1" name="star" />
                                <label class="star star-1" for="star-1">
                                </label>
                            </form>
                            <asp:LinkButton ID="btn_AddRating" runat="server" CssClass="btn btn-primary" ToolTip="Update Coding Standard Rating"
                                OnClick="btn_AddRating_Click" Text="<i class='fa fa-pencil icon'></i>" OnClientClick="storeRating()"></asp:LinkButton>
                            <asp:HiddenField ID="hdn_ScoredRating" runat="server" />
                        </div>
                        </br>
                        <label class="label-title" id="lbl_RatingQualityView" runat="server" style="font-weight: bold">
                            Rating For Quality
                        </label>
                        <div class="stars label-title" style="width: 320px !important">
                            <form action="">
                                <input class="star star-5" id="qualitystar-5" type="radio" value="5" name="star" />
                                <label class="star star-5" for="qualitystar-5">
                                </label>
                                <input class="star star-4" id="qualitystar-4" type="radio" value="4" name="star" />
                                <label class="star star-4" for="qualitystar-4">
                                </label>
                                <input class="star star-3" id="qualitystar-3" type="radio" value="3" name="star" />
                                <label class="star star-3" for="qualitystar-3">
                                </label>
                                <input class="star star-2" id="qualitystar-2" type="radio" value="2" name="star" />
                                <label class="star star-2" for="qualitystar-2">
                                </label>
                                <input class="star star-1" id="qualitystar-1" type="radio" value="1" name="star" />
                                <label class="star star-1" for="qualitystar-1">
                                </label>
                            </form>
                            <asp:LinkButton ID="btn_AddQualityRating" runat="server" CssClass="btn btn-warning"
                                ToolTip="Update Quality Rating" OnClick="btn_AddQualityRating_Click" Text="<i class='fa fa-pencil icon'></i>"
                                OnClientClick="storeQualityRating()"></asp:LinkButton>
                            <asp:HiddenField ID="hdn_ScoredQualityRating" runat="server" />
                        </div>
                        <div class="card-header">
                            <div class="card-title">
                                <div class="title" id="Div1" runat="server">
                                    DPR's
                                </div>
                            </div>
                        </div>
                        <%-- Gridview For Task Details From selected task --%>
                        <div class="form-group">
                            <asp:Panel ID="pnl_TaskDetails" runat="server" Style="width: 100%; overflow-x: auto;">
                                <asp:GridView ID="grd_TaskDetails" runat="server" CssClass="datatable table table-striped dataTable"
                                    AutoGenerateColumns="False">
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
                            <div class="card-header">
                                <div class="card-title">
                                    <div class="title" id="title_TaskHolds" visible="false" runat="server">
                                        Task Holds
                                    </div>
                                </div>
                            </div>
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
                        <asp:Button class="btn btn-info" runat="server" ID="btn_BackButton" OnClick="btn_BackButtonClick"
                            Text="Back" />
                    </div>
                </div>
                <%--Block For Add Task--%>
                <div id="block_Register" runat="server">
                    <div class="card-body">
                        <div class="left-inner-addon">
                            <div class="form-group">
                                <label for="lbl_TaskName" runat="server" class="label-title">
                                    Request ID</label>
                                <i class="fa fa-database"></i>
                                <asp:TextBox ID="txt_RequestNo" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                <asp:HiddenField ID="hdn_RequestNo" runat="server" />
                                <asp:HiddenField ID="hdn_TaskNo" runat="server" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="left-inner-addon">
                                <label for="lbl_TaskName" class="label-title">
                                    Task Name</label>
                                <i class="glyphicon glyphicon-tasks"></i>
                                <asp:TextBox ID="txt_TaskName" MaxLength="150" CausesValidation="true" class="form-control"
                                    runat="server"></asp:TextBox>
                                <span style="color: #ff0000">**</span>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="lbl_TaskDescription" class="label-title">
                                Task Description</label>
                            <asp:TextBox ID="txt_TaskDescription" TextMode="MultiLine" CausesValidation="true"
                                class="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <div class="left-inner-addon">
                                <asp:Label ID="Label_Already" runat="server" Visible="false" ForeColor="red"></asp:Label>
                                <label for="lbl_Project" class="label-title">
                                    Project</label>
                                <i class="fa fa-folder-open"></i>
                                <asp:DropDownList ID="ddl_Project" AutoPostBack="true" class="form-control" runat="server"
                                    OnSelectedIndexChanged="ddl_Project_SelectedIndexChanged">
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                                <span style="color: #ff0000">**</span>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="left-inner-addon">
                                <label for="lbl_Stage" class="label-title">
                                    Stage</label>
                                <i class="glyphicon glyphicon-stats"></i>
                                <asp:DropDownList ID="ddl_Stage" AutoPostBack="true" class="form-control" runat="server">
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                                <span style="color: #ff0000">**</span>
                                <asp:Button ID="btn_NewStage" Visible="false" runat="server" Text="Add New" CssClass="btn btn-primary"
                                    data-toggle="modal" data-target="#modalAddStage"></asp:Button>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="left-inner-addon">
                                <label for="lbl_Scope" class="label-title">
                                    Scope</label>
                                <i class="fa fa-language"></i>
                                <asp:DropDownList ID="ddl_Scope" AutoPostBack="true" class="form-control" runat="server"
                                    OnSelectedIndexChanged="ddl_Scope_SelectedIndexChanged">
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                                <span style="color: #ff0000">**</span>
                                <asp:Button ID="btn_NewScope" Visible="false" runat="server" Text="Add New" CssClass="btn btn-primary"
                                    data-toggle="modal" data-target="#modalAddScope"></asp:Button>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="left-inner-addon">
                                <label class="label-title">
                                    Team</label>
                                <i class="fa fa-users"></i>
                                <asp:DropDownList ID="ddl_Team" class="form-control" runat="server" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddl_Team_SelectedIndexChanged">
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                                <span style="color: #ff0000">**</span>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="left-inner-addon">
                                <label class="label-title">
                                    Users</label>
                                <i class="fa fa-user"></i>
                                <asp:DropDownList ID="ddl_Users" class="form-control" runat="server" OnSelectedIndexChanged="ddl_Users_SelectedIndexChanged">
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                                <span style="color: #ff0000">**</span>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="left-inner-addon">
                                <label class="label-title">
                                    Request Date</label>
                                <i class="fa fa-calendar"></i>
                                <asp:TextBox ID="txt_RequestDate" class="form-control" runat="server" AutoPostBack="true"
                                    OnTextChanged="txt_RequestDate_TextChanged"></asp:TextBox>
                                <span style="color: #ff0000">**</span>
                                <cc1:CalendarExtender ID="calext_RequestDate" PopupPosition="TopLeft" runat="server"
                                    TargetControlID="txt_RequestDate" CssClass="custom-calendar" Format="dd/MM/yyyy">
                                </cc1:CalendarExtender>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="left-inner-addon">
                                <label class="label-title">
                                    Required Date</label>
                                <i class="fa fa-calendar"></i>
                                <asp:TextBox ID="txt_RequiredDate" class="form-control" runat="server"></asp:TextBox>
                                <span style="color: #ff0000">**</span>
                                <cc1:CalendarExtender ID="calext_RequiredDate" CssClass="custom-calendar" PopupPosition="TopLeft"
                                    runat="server" TargetControlID="txt_RequiredDate" Format="dd/MM/yyyy">
                                </cc1:CalendarExtender>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="left-inner-addon">
                                <label class="label-title">
                                    Estimated Hours(HH:MM)</label>
                                <i class="glyphicon glyphicon-time"></i>
                                <asp:TextBox ID="txt_EstimatedHours" class="form-control" runat="server"></asp:TextBox>
                                <cc1:MaskedEditExtender ID="me_EstimatedHours" runat="server" Mask="999:99" TargetControlID="txt_EstimatedHours"
                                    ClearMaskOnLostFocus="False">
                                </cc1:MaskedEditExtender>
                                <span style="color: #ff0000">**</span>
                            </div>
                        </div>
                        <asp:Button ID="btn_Allot" OnClientClick="return preventMultipleSubmissions();" class="btn btn-success" Style="margin-left: 25%" CausesValidation="true"
                            runat="server" Text="Allot" ValidationGroup="project" OnClick="btn_Allot_Click"></asp:Button>
                        <asp:Button ID="btn_Reset" class="btn btn-warning" runat="server" Text="Reset" CausesValidation="false"
                            OnClick="btn_Reset_Click"></asp:Button>
                        <asp:Button ID="btn_Back" class="btn btn-info" runat="server" Text="Back" CausesValidation="false"
                            OnClick="btn_Back_Click"></asp:Button>
                    </div>
                    <div>
                        <div align="left" style="margin-left: 5%">
                            <span style="color: #ff0000">**</span><font color="blue"> Required field</font>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btn_Export" />
            </Triggers>
        </asp:UpdatePanel>
        <!-- Modal For Add New Stage -->
        <div class="modal fade" id="modalAddStage" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
            aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title" id="lbl_scope">Stage</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <label class="label-title">
                                Stage</label>
                            <asp:TextBox ID="txt_Stage" class="form-control" Style="width: 50%" runat="server"
                                ValidationGroup="stage"></asp:TextBox><span style="color: #ff0000"> **</span>
                            <asp:RequiredFieldValidator ID="rf_Stage" runat="server" ErrorMessage="Stage Name is Mandatory"
                                Display="Dynamic" ValidationGroup="stage" ControlToValidate="txt_Stage"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btn_Stage" class="btn btn-primary" ValidationGroup="stage" runat="server"
                            OnClick="btn_Stage_Click" Text="Add" />
                        <asp:Button ID="btn_CancelStage" Text="Cancel" class="btn btn-primary" runat="server"
                            OnClick="btn_CancelStage_Click" />
                    </div>
                </div>
            </div>
        </div>
        <!-- Modal For Add New Scope -->
        <div class="modal fade" id="modalAddScope" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
            aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title" id="H1">Scope</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <label class="label-title">
                                Scope</label>
                            <asp:TextBox ID="txt_Scope" class="form-control" Style="width: 50%" runat="server"
                                ValidationGroup="scope"></asp:TextBox><span style="color: #ff0000"> **</span>
                            <asp:RequiredFieldValidator ID="rf_Scope" runat="server" ErrorMessage="Scope Name is Mandatory"
                                Display="Dynamic" ValidationGroup="scope" ControlToValidate="txt_Scope"></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group">
                            <label class="label-title" style="vertical-align: middle;">
                                Description</label>
                            <asp:TextBox ID="txt_Description" TextMode="MultiLine" Width="50%" class="form-control"
                                runat="server" ValidationGroup="scope"></asp:TextBox><span style="color: #ff0000;">
                                    **</span>
                            <asp:RequiredFieldValidator ID="rf_Description" runat="server" ErrorMessage="Description is Mandatory"
                                Display="Dynamic" ValidationGroup="scope" ControlToValidate="txt_Description"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btn_Scope" class="btn btn-primary" ValidationGroup="scope" runat="server"
                            OnClick="btn_Scope_Click" Text="Add" />
                        <asp:Button ID="btn_CancelScope" Text="Cancel" class="btn btn-primary" runat="server"
                            OnClick="btn_CancelScope_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%--</div>
        </div>
    </div>--%>
    <%--Validations Required Field--%>
    <script type="text/javascript" charset="utf8" src="assets/js/Datatable/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="assets/js/Datatable/dataTables.bootstrap.min.js"></script>
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
    <script language="javascript" type="text/javascript">
        function validate() {
            if (document.getElementById("<%=txt_TaskName.ClientID%>").value == "") {
                swal("Task Name Field can not be blank");
                document.getElementById("<%=txt_TaskName.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=ddl_Project.ClientID%>").selectedIndex == 0) {
                swal("Project Field is Mandatory");
                document.getElementById("<%=ddl_Project.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=ddl_Stage.ClientID%>").selectedIndex == 0) {
                swal("Stage Field is Mandatory");
                document.getElementById("<%=ddl_Stage.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=ddl_Scope.ClientID%>").selectedIndex == 0) {
                swal("Scope Field is Mandatory");
                document.getElementById("<%=ddl_Scope.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=ddl_Team.ClientID%>").selectedIndex == 0) {
                swal("Team Field is Mandatory");
                document.getElementById("<%=ddl_Team.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=ddl_Users.ClientID%>").selectedIndex == 0) {
                swal("User Field is Mandatory");
                document.getElementById("<%=ddl_Users.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txt_RequestDate.ClientID%>").value == "") {
                swal("Request Date Field can not be blank");
                document.getElementById("<%=txt_RequestDate.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txt_RequiredDate.ClientID%>").value == "") {
                swal("Required Date Field can not be blank");
                document.getElementById("<%=txt_RequiredDate.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txt_EstimatedHours.ClientID%>").value == "___:__") {
                swal("Estimted Hours Field can not be blank");
                document.getElementById("<%=txt_EstimatedHours.ClientID%>").focus();
                return false;
            }
            return true;
        }
    </script>
    <script type="text/javascript">
        var table;
        var checkExists = 0;
        $(document).ready(function () {
            checkExists = 1;
            setFilters();
        });
        function setFilters() {
            if ($("#ctl00_ContentPlaceHolder1_block_Grid").is(":visible") == false) {
                $("#ctl00_ContentPlaceHolder1_ddl_FilterMonth").css("visibility", "hidden");
                $("#ctl00_ContentPlaceHolder1_lbl_SeparatorMonthYear").css("visibility", "hidden");
                $("#ctl00_ContentPlaceHolder1_ddl_FilterYear").css("visibility", "hidden");
                $("#ctl00_ContentPlaceHolder1_ddl_FilterProject").css("visibility", "hidden");
            }
            else {
                $("#ctl00_ContentPlaceHolder1_ddl_FilterMonth").css("visibility", "visible");
                $("#ctl00_ContentPlaceHolder1_lbl_SeparatorMonthYear").css("visibility", "visible");
                $("#ctl00_ContentPlaceHolder1_ddl_FilterYear").css("visibility", "visible");
                $("#ctl00_ContentPlaceHolder1_ddl_FilterProject").css("visibility", "visible");
            }
        }
        function setDatatable() {
                // Setup - add a text input to each footer cell
                $("#ctl00_ContentPlaceHolder1_grd_Tasks").prepend($("<thead></thead><tfoot></tfoot>").append($("#ctl00_ContentPlaceHolder1_grd_Tasks").find("tr:first")));

                $('#ctl00_ContentPlaceHolder1_grd_Tasks tfoot th').each(function () {
                    var title = $(this).text();
                    if (title != "Edit") {
                        $(this).replaceWith('<td><input type="text" class="form-control" style="width:100%" placeholder="Search ' + title + '" /></td>');
                    }
                    else {
                        $(this).replaceWith('<td></td>');
                    }
                });
                table = $("#ctl00_ContentPlaceHolder1_grd_Tasks").dataTable({
                    "lengthMenu": [[6, 10, 15, -1], [6, 10, 15, "All"]],
                    "aaSorting": [],
                    "deferRender": true,
                    "deferLoading": 100,
                    "scrollX": true,
                    "sDom": 'C<"clear">lfrtip',
                    "stateSave": true
                });
                if (checkExists == 1) {
                    table.api().state.clear();
                }
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
        }
    </script>
    <script type="text/javascript">
        var selectedGrade = 0;
        var jsonGrade;
        var taskID;
        function storeRating() {
            taskID = $("#hdn_TaskNo").val();
            if ($('#star-5[value=5]').is(':checked')) {
                selectedGrade = 5;
            }
            else if ($('#star-4[value=4]').is(':checked')) {
                selectedGrade = 4;
            }
            else if ($('#star-3[value=3]').is(':checked')) {
                selectedGrade = 3;
            }
            else if ($('#star-2[value=2]').is(':checked')) {
                selectedGrade = 2;
            }
            else if ($('#star-1[value=1]').is(':checked')) {
                selectedGrade = 1;
            }
            $('#<%= hdn_ScoredRating.ClientID %>').val(selectedGrade);
        }
        function storeQualityRating() {
            taskID = $("#hdn_TaskNo").val();
            if ($('#qualitystar-5[value=5]').is(':checked')) {
                selectedGrade = 5;
            }
            else if ($('#qualitystar-4[value=4]').is(':checked')) {
                selectedGrade = 4;
            }
            else if ($('#qualitystar-3[value=3]').is(':checked')) {
                selectedGrade = 3;
            }
            else if ($('#qualitystar-2[value=2]').is(':checked')) {
                selectedGrade = 2;
            }
            else if ($('#qualitystar-1[value=1]').is(':checked')) {
                selectedGrade = 1;
            }
            $('#<%= hdn_ScoredQualityRating.ClientID %>').val(selectedGrade);
        }

    </script>
</asp:Content>
