<%@ Page Title="Time Tracking Sheet" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="TimeTrackingSheet.aspx.cs" Inherits="TimeTrackingSheet" EnableEventValidation="false" %>

<%--Ajax Control Toolkit For Support Ajax Calendar Extender--%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%--Inline CSS--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        var isSubmitted = false;
        function preventMultipleSubmissions() {
            if (!isSubmitted) {
                $('#<%=btn_SaveStatus.ClientID %>').val('Submitting.. Plz Wait..');
                isSubmitted = true;
                return true;
            }
            else {
                return false;
            }
        }
    </script>

    <style type="text/css">
        .table > thead > tr > th, .table > tbody > tr > th, .table > tfoot > tr > th, .table > thead > tr > td, .table > tbody > tr > td, .table > tfoot > tr > td {
            border-width: 0.1px;
            border: 1px solid #ddd;
        }

        .label-title {
            width: 162px !important;
        }
    </style>
    <style type="text/css">
        .custom-date-style {
            background-color: red !important;
        }

        .input {
        }

        .modal-backdrop.in {
            opacity: 0.4 !important;
        }

        .input-wide {
            width: 500px;
        }
    </style>
    <%--ClockPicker Plugin--%>
    <script type="text/javascript" src="assets/bower_components/jquery/dist/jquery.min.js"></script>
    <script type="text/javascript" src="assets/bower_components/bootstrap/dist/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="assets/bower_components/jquery/dist/jquery.min.js"></script>
    <script type="text/javascript" src="assets/bower_components/bootstrap/dist/js/bootstrap.min.js"></script>
    <script src="assets/clockpicker/js/jquery-clockpicker.min.js"></script>
    <script src="assets/clockpicker/js/bootstrap-clockpicker.min.js"></script>
    <script type="text/javascript" src="assets/clockpicker/js/bootstrap-clockpicker.min.js"></script>
    <%--Status Of Completed Task--%>
    <script type="text/javascript">
        function ChangeTaskPercentage() {
            var selecteditem = document.getElementById("<%=ddl_CompletedTask.ClientID %>");
            var selectedvalue = selecteditem.value + '%';

            document.getElementById("<%=spn_TaskPercentage.ClientID %>").innerText = selectedvalue;
            document.getElementById("<%=spn_TaskPercentage.ClientID %>").innerText = selectedvalue;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="card">
        <%--Heading Of MainContent--%>
        <div class="card-header">
            <div class="card-title">
                <div class="title" id="TitleOfPage" runat="server">
                    Individual Job Card
                </div>
            </div>
        </div>
        <%--Main Content--%>
        <div class="card-body">
            <%--CSS Loader--%>
            <%--Form elements--%>
            <div>
                <div id="popupdiv" title="Basic modal dialog" style="display: none">
                    <b>Start Time is Invalid</b>
                </div>
            </div>
            <%--Block For Insert DPR Status--%>
            <div id="block_Register" runat="server">
                <asp:UpdatePanel ID="upl_SaveStatus" runat="server">
                    <ContentTemplate>
                        <table class="table table-responsive table-condensed">
                            <tr>
                                <div class="form-group">
                                    <td>
                                        <label class="label-title">
                                            Employee</label>
                                    </td>
                                    <td>
                                        <div class="left-inner-addon">
                                            <i class="fa fa-user" runat="server" id="icon_EmployeeDdl"></i>
                                            <asp:DropDownList runat="server" AutoPostBack="true" class="form-control" Style="width: 80%"
                                                TabIndex="1" ID="ddl_EmpNo" OnSelectedIndexChanged="ddl_EmpNo_SelectedIndexChanged">
                                                <asp:ListItem>
                                                </asp:ListItem>
                                            </asp:DropDownList>
                                            <span
                                                id="span_EmpNo" runat="server" style="color: #ff0000;">** </span>
                                        </div>
                                        <div class="left-inner-addon">
                                            <i class="fa fa-user" runat="server" id="icon_EmployeeTxt"></i>
                                            <asp:TextBox ID="txt_EmpNo" Style="width: 80%" class="form-control" TabIndex="1"
                                                runat="server" ReadOnly="True" OnTextChanged="txt_EmpNo_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <asp:HiddenField ID="hdn_EmpNo" runat="server" />
                                            <asp:HiddenField ID="hdn_Slno" runat="server" />
                                        </div>
                                    </td>
                                </div>
                                <div class="form-group">
                                    <td>
                                        <label class="label-title">
                                            Shift</label>
                                    </td>
                                    <td>
                                        <div class="left-inner-addon">
                                            <i class="fa fa-random"></i>
                                            <asp:DropDownList ID="ddl_Shift" runat="server" AutoPostBack="False" class="form-control"
                                                OnSelectedIndexChanged="ddl_Shift_SelectedIndexChanged" Style="width: 80%" TabIndex="7">
                                                <asp:ListItem>Select</asp:ListItem>
                                                <asp:ListItem>First</asp:ListItem>
                                                <asp:ListItem>General</asp:ListItem>
                                                <asp:ListItem>Special</asp:ListItem>
                                                <asp:ListItem>Second</asp:ListItem>
                                                <asp:ListItem>Evening</asp:ListItem>
                                                <asp:ListItem>Middle</asp:ListItem>
                                                <asp:ListItem>Night</asp:ListItem>
                                            </asp:DropDownList>
                                            <span style="color: #ff0000">** </span>
                                            <asp:TextBox ID="txt_EmpName" runat="server" class="form-control" ReadOnly="True"
                                                Style="width: 80%" Visible="false"></asp:TextBox>
                                            <asp:HiddenField ID="hdn_EmpName" runat="server" />
                                        </div>
                                    </td>
                                </div>
                            </tr>
                            <tr>
                                <div class="form-group">
                                    <td>
                                        <label class="label-title">
                                            Project</label>
                                    </td>
                                    <td>
                                        <div class="left-inner-addon">
                                            <i class="fa fa-folder-open"></i>
                                            <asp:DropDownList ID="ddl_ProjectID" runat="server" class="form-control" TabIndex="2"
                                                Style="width: 80%" AutoPostBack="true" OnSelectedIndexChanged="ddl_ProjectID_SelectedIndexChanged">
                                                <asp:ListItem>Select</asp:ListItem>
                                            </asp:DropDownList>
                                            <span style="color: #ff0000">**</span>
                                        </div>
                                    </td>
                                </div>
                                <div class="form-group">
                                    <td>
                                        <label class="label-title">
                                            Date</label>
                                    </td>
                                    <td>
                                        <div class="left-inner-addon">
                                            <i class="fa fa-calendar"></i>
                                            <asp:TextBox ID="txt_Date" AutoPostBack="true" TabIndex="8" OnTextChanged="txt_Date_TextChanged"
                                                runat="server" Style="width: 80%" class="form-control" ToolTip="dd/MM/yyyy"></asp:TextBox>
                                            <cc1:CalendarExtender ID="txt_Date_CalendarExtender" CssClass="custom-calendar" runat="server"
                                                Enabled="True" Format="dd/MM/yyyy" TargetControlID="txt_Date">
                                            </cc1:CalendarExtender>
                                            <span style="color: #ff0000">**</span>
                                        </div>
                                    </td>
                                </div>
                            </tr>
                            <tr>
                                <div class="form-group">
                                    <td>
                                        <label class="label-title">
                                            Stage</label>
                                    </td>
                                    <td>
                                        <div class="left-inner-addon">
                                            <i class="glyphicon glyphicon-stats"></i>
                                            <asp:DropDownList ID="ddl_Stage" TabIndex="3" class="form-control" Style="width: 80%"
                                                runat="server" AppendDataBoundItems="True" AutoPostBack="True" OnSelectedIndexChanged="ddl_Stage_SelectedIndexChanged">
                                                <asp:ListItem>Select</asp:ListItem>
                                            </asp:DropDownList>
                                            <span style="color: #ff0000">** </span>
                                        </div>
                                    </td>
                                </div>
                                <div class="form-group">
                                    <td>
                                        <label class="label-title" id="lbl_StartTime" runat="server">
                                            Start Time(24 Hours Format)
                                        </label>
                                    </td>
                                    <td>
                                        <div class="left-inner-addon">
                                            <i class="glyphicon glyphicon-time"></i>
                                            <asp:TextBox ID="txt_StartTime" onchange="calculate()" MaxLength="5" TabIndex="9" Style="width: 80%" runat="server"
                                                placeholder="HH:MM" class="form-control timepicker calculate" onkeypress="return no_alpha(); return true;"
                                                Enabled="True"></asp:TextBox>
                                            <cc1:MaskedEditExtender ID="me_StartTime" runat="server" Mask="99:99" MaskType="Time"
                                                UserTimeFormat="TwentyFourHour" TargetControlID="txt_StartTime" ClearMaskOnLostFocus="False">
                                            </cc1:MaskedEditExtender>
                                            <span style="color: #ff0000">**</span>
                                            <asp:Label ID="lbl_StartTime0" runat="server" Font-Bold="False" ForeColor="Black"></asp:Label>
                                        </div>
                                    </td>
                                </div>
                            </tr>
                            <tr>
                                <div class="form-group">
                                    <td>
                                        <label class="label-title">
                                            Scope</label>
                                    </td>
                                    <td>
                                        <div class="left-inner-addon">
                                            <i class="fa fa-language"></i>
                                            <asp:DropDownList ID="ddl_Scope" TabIndex="4" class="form-control" Style="width: 80%"
                                                runat="server" AppendDataBoundItems="True" AutoPostBack="True" OnSelectedIndexChanged="ddl_Scope_SelectedIndexChanged">
                                                <asp:ListItem>Select</asp:ListItem>
                                            </asp:DropDownList>
                                            <span style="color: #ff0000">** </span>
                                        </div>
                                    </td>
                                </div>
                                <div class="form-group">
                                    <td>
                                        <label class="label-title">
                                            End Time</label>
                                    </td>
                                    <td>
                                        <div class="left-inner-addon">
                                            <i class="glyphicon glyphicon-time"></i>
                                            <asp:TextBox ID="txt_EndTime" onchange="calculate()" TabIndex="10" MaxLength="5" runat="server" class="form-control timepicker calculate"
                                                placeholder="HH:MM" Style="width: 80%"></asp:TextBox>
                                            <cc1:MaskedEditExtender ID="me_EndTime" runat="server" Mask="99:99" MaskType="Time"
                                                UserTimeFormat="TwentyFourHour" TargetControlID="txt_EndTime" ClearMaskOnLostFocus="False">
                                            </cc1:MaskedEditExtender>
                                            <span style="color: #ff0000">**</span>
                                        </div>
                                    </td>
                                </div>
                            </tr>
                            <tr>
                                <td>
                                    <label class="label-title">
                                        Task</label>
                                </td>
                                <td>
                                    <div class="left-inner-addon">
                                        <i class="glyphicon glyphicon-tasks"></i>
                                        <asp:DropDownList ID="ddl_Task" TabIndex="5" class="form-control" Style="width: 80%"
                                            runat="server" AppendDataBoundItems="True" AutoPostBack="True" OnSelectedIndexChanged="ddl_Task_SelectedIndexChanged">
                                            <asp:ListItem>Select</asp:ListItem>
                                        </asp:DropDownList>
                                        <span style="color: #ff0000">** </span>
                                    </div>
                                </td>
                                <td>
                                    <label style="float: left" class="label-title">
                                        Break Time</label>
                                </td>
                                <td>
                                    <asp:RadioButtonList ID="rbl_BreakTime" onchange="calculate()" CssClass="calculate" TabIndex="11" RepeatDirection="Horizontal"
                                        runat="server">
                                        <asp:ListItem Value="0">0</asp:ListItem>
                                        <asp:ListItem Value="15">15</asp:ListItem>
                                        <asp:ListItem Value="20">20</asp:ListItem>
                                        <asp:ListItem Value="30">30</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <asp:PlaceHolder ID="ph_DueDate" runat="server" Visible="false">
                                <tr>
                                    <td>
                                        <label class="label-title">Due Date</label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_DueDate" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </asp:PlaceHolder>
                            <tr>
                                <div class="form-group">
                                    <td>
                                        <label class="label-title">
                                            % Of Task Completed</label>
                                    </td>
                                    <td>
                                        <div class="left-inner-addon">
                                            <i class="fa fa-check-circle"></i>
                                            <asp:DropDownList ID="ddl_CompletedTask" onchange="ChangeTaskPercentage()" TabIndex="6" Style="width: 80%" class="form-control"
                                                runat="server" AppendDataBoundItems="True">
                                                <asp:ListItem Value="0">Select</asp:ListItem>
                                                <asp:ListItem Value="10" Text="10%"></asp:ListItem>
                                                <asp:ListItem Value="20" Text="20%"></asp:ListItem>
                                                <asp:ListItem Value="30" Text="30%"></asp:ListItem>
                                                <asp:ListItem Value="40" Text="40%"></asp:ListItem>
                                                <asp:ListItem Value="50" Text="50%"></asp:ListItem>
                                                <asp:ListItem Value="60" Text="60%"></asp:ListItem>
                                                <asp:ListItem Value="70" Text="70%"></asp:ListItem>
                                                <asp:ListItem Value="80" Text="80%"></asp:ListItem>
                                                <asp:ListItem Value="90" Text="90%"></asp:ListItem>
                                                <asp:ListItem Value="100" Text="100%"></asp:ListItem>
                                            </asp:DropDownList>
                                            <span style="color: #ff0000">** </span>
                                            <asp:HiddenField ID="hdn_TaskPercentage" runat="server" />
                                            <%--Show Percentage Of Task Completed--%>
                                            <span class="badge info" id="spn_TaskPercentage" runat="server" visible="false" style="color: white" runat="server">0</span>
                                            <span id="spn_TaskPercentage2" class="fa-stack fa-lg" runat="server" visible="false" style="width: 10%; color: #5bc0de">
                                                <i class="fa fa-circle fa-stack-2x"></i>
                                                <i id="icon_TaskPercentage" runat="server" class="fa fa-inverse fa-stack-1x" style="font-size: 10.5px"></i>
                                            </span>
                                        </div>
                                    </td>
                                </div>
                                </div>
					            <div class="form-group">
                                    <td>
                                        <label class="label-title">
                                            Total Time(HH:MM)</label>
                                    </td>
                                    <td>
                                        <div class="left-inner-addon">
                                            <i class="glyphicon glyphicon-time"></i>
                                            <asp:TextBox ID="txt_TotalTime" TabIndex="12" class="form-control" Style="width: 80%"
                                                ReadOnly="True" runat="server"></asp:TextBox>
                                            <asp:HiddenField ID="hdn_TotalTime" runat="server" />
                                        </div>
                                    </td>
                                </div>
                            </tr>
                            <tr>
                                <div class="form-group">
                                    <td>
                                        <label class="label-title">
                                            Meeting Time</label>
                                    </td>
                                    <td>
                                        <div class="left-inner-addon">
                                            <i class="glyphicon glyphicon-time"></i>
                                            <asp:TextBox ID="txt_MeetingTime" TabIndex="10"
                                                MaxLength="5" runat="server" onchange="calculate()" class="form-control timepicker calculate" placeholder="HH:MM"
                                                Style="width: 80%"></asp:TextBox>
                                            <cc1:MaskedEditExtender ID="MaskedEditExtender2" runat="server" Mask="99:99" MaskType="Time"
                                                UserTimeFormat="TwentyFourHour" TargetControlID="txt_MeetingTime" ClearMaskOnLostFocus="False">
                                            </cc1:MaskedEditExtender>
                                        </div>
                                    </td>
                                </div>
                                <div class="form-group">
                                    <td>
                                        <label class="label-title">
                                            Meeting Remarks</label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_MeetingRemarks" TabIndex="13" runat="server" class="form-control"
                                            Style="float: left; height: 100%; width: 80%" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </div>
                            </tr>
                            <tr>
                                <div class="form-group">
                                    <td>
                                        <label class="label-title">
                                            Remarks</label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txt_Remarks" TabIndex="13" runat="server" class="form-control" Style="float: left; width: 93%; height: 125%"
                                            TextMode="MultiLine"></asp:TextBox><span style="color: #ff0000; margin-left: 1%">**</span>
                                    </td>
                                </div>
                            </tr>
                            <tr>
                                <div class="form-group">
                                    <td></td>
                                    <td colspan="3">

                                        <%--<asp:Button ID="btn_SaveStatus" TabIndex="14" CssClass="btn btn-success calculate" runat="server"
                                            OnClientClick="return validate(); CheckIsRepeat(); preventMultipleSubmissions();" Text="Save" OnClick="btn_SaveStatus_Click"
                                            Enabled="true" />--%>
                                        <asp:Button ID="btn_SaveStatus" TabIndex="14" CssClass="btn btn-success calculate" runat="server"
                                            OnClientClick="return validate();" Text="Save" OnClick="btn_SaveStatus_Click"
                                            Enabled="true" />
                                        <asp:Button ID="btn_Reset" TabIndex="15" CssClass="btn btn-warning" runat="server"
                                            Text="Reset" Enabled="true" OnClick="btn_Reset_Click" />
                                        <asp:Button ID="btn_Back" TabIndex="16" CssClass="btn btn-info" runat="server" Text="Back"
                                            Enabled="true" OnClick="btn_Back_Click" />
                                    </td>
                                </div>
                            </tr>
                            <tr>
                                <div class="form-group">
                                    <td colspan="4">
                                        <span style="color: #ff0000">**</span><font color="blue"> Required field</font>
                                    </td>
                                </div>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <%--Block For DPR Gridview--%>
            <div id="block_Grid" runat="server" style="overflow-x: auto">
                <asp:UpdatePanel ID="upl_Grid" runat="server">
                    <ContentTemplate>
                        <div class="form-group">
                            <asp:GridView ID="grd_DPR" runat="server" CssClass="datatable table table-striped dataTable"
                                Style="overflow: scroll" AutoGenerateColumns="False" DataKeyNames="slno,Project,TaskID,EmpNo"
                                OnRowDeleting="grd_DPR_RowDeleting" AllowPaging="true" OnPageIndexChanging="OnPageIndexChanging"
                                PageSize="5" OnSelectedIndexChanged="grd_DPR_SelectedIndexChanged" OnRowDataBound="grd_DPR_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Employee">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Employee" runat="server" Text='<%# Eval("Employee") %>'></asp:Label>
                                            <asp:HiddenField ID="hdn_EmployeeID" runat="server" Value='<%# Eval("EmpNo")%>'></asp:HiddenField>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Shift">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Shift" runat="server" Text='<%# Eval("Shift") %>'></asp:Label>
                                            <asp:HiddenField ID="hdn_Slno" runat="server" Value='<%# Eval("slno")%>'></asp:HiddenField>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Date" runat="server" Text='<%# Eval("CurrentDate") %>'></asp:Label>
                                            <asp:HiddenField ID="hdn_Date" runat="server" Value='<%# Eval("Date")%>'></asp:HiddenField>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Project">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Project" runat="server" Text='<%# Eval("ProjectID") %>'></asp:Label>
                                            <asp:HiddenField ID="hdn_ProjectID" runat="server" Value='<%# Eval("Project")%>'></asp:HiddenField>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Stage">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Stage" runat="server" Text='<%# Eval("Stage") %>'></asp:Label>
                                            <asp:HiddenField ID="hdn_StageID" runat="server" Value='<%# Eval("StageID")%>'></asp:HiddenField>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Scope">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Scope" runat="server" Text='<%# Eval("Scope") %>'></asp:Label>
                                            <asp:HiddenField ID="hdn_ScopeID" runat="server" Value='<%# Eval("ScopeID")%>'></asp:HiddenField>
                                            <asp:HiddenField ID="hdn_TaskID" runat="server" Value='<%# Eval("TaskID")%>'></asp:HiddenField>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Task">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Task" runat="server" Text='<%# Eval("Task")  %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status Of Task">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_StatusOfTask" runat="server" Text='<%# Eval("statusoftask")+"%" %>'></asp:Label>
                                            <asp:HiddenField ID="hdn_StatusOfTask" runat="server" Value='<%# Eval("statusoftask") %>'></asp:HiddenField>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Start Time">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_StartTime" runat="server" Text='<%# Eval("StartTime") %>'></asp:Label>
                                            <asp:HiddenField ID="hdn_StartTime" runat="server" Value='<%# Eval("TimeStart")%>'></asp:HiddenField>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="End Time">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_EndTime" runat="server" Text='<%# Eval("EndTime") %>'></asp:Label>
                                            <asp:HiddenField ID="hdn_EndTime" runat="server" Value='<%# Eval("TimeEnd")%>'></asp:HiddenField>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Break">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Break" runat="server" Text='<%# Eval("BreakTime") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Meeting Time">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_MeetingTime" runat="server" Text='<%# Eval("meetingtime") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Meeting Remarks">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_MeetingRemarks" runat="server" Text='<%# Eval("meetingremarks") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total Time">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_TotalTime" runat="server" Text='<%# Eval("TotalTime") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Remarks">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Remarks" runat="server" Text='<%# Eval("Remarks") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Edit">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnk_Edit" TabIndex="3" runat="server" CommandName="Select" ToolTip="Select"
                                                Text='<i class="fa fa-pencil-square-o"></i>' ForeColor="#5AC15A" Font-Size="23px">
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="lnk_Delete" OnClientClick="ConfirmDelete(' this report? ')" TabIndex="4"
                                                runat="server" CommandName="Delete" ToolTip="Delete" Text='<i class="glyphicon glyphicon-trash"></i>'
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
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <%--Javascript Validations--%>
            <script language="javascript" type="text/javascript">
                function validate() {
                    if (document.getElementById("<%=ddl_Shift.ClientID%>").selectedIndex == 0) {
                        swal("Shift field can not be blank", '', 'warning');
                        document.getElementById("<%=ddl_Shift.ClientID%>").focus();
                        return false;
                    }
                    if (document.getElementById("<%=ddl_ProjectID.ClientID%>").selectedIndex == 0) {
                        swal("Project id field can not be blank", '', 'warning');
                        document.getElementById("<%=ddl_ProjectID.ClientID%>").focus();
                        return false;
                    }
                    if (document.getElementById("<%=ddl_Stage.ClientID%>").selectedIndex == 0) {
                        swal("Stage field can not be blank", '', 'warning');
                        document.getElementById("<%=ddl_Stage.ClientID%>").focus();
                        return false;
                    }
                    if (document.getElementById("<%=ddl_Scope.ClientID%>").selectedIndex == 0) {
                        swal("Scope field can not be blank", '', 'warning');
                        document.getElementById("<%=ddl_Scope.ClientID%>").focus();
                        return false;
                    }
                    if (document.getElementById("<%=ddl_Task.ClientID%>").selectedIndex == 0) {
                        swal("Task field can not be blank", '', 'warning');
                        document.getElementById("<%=ddl_Task.ClientID%>").focus();
                        return false;
                    }
                    if (document.getElementById("<%=ddl_CompletedTask.ClientID%>").selectedIndex == 0) {
                        swal("% Of Task Completed field can not be blank", '', 'warning');
                        document.getElementById("<%=ddl_CompletedTask.ClientID%>").focus();
                        return false;
                    }
                    if (document.getElementById("<%=txt_StartTime.ClientID%>").value == "__:__" || document.getElementById("<%=txt_StartTime.ClientID%>").value == "") {
                        swal("Start time field can not be blank", '', 'warning');
                        document.getElementById("<%=txt_StartTime.ClientID%>").focus();
                        return false;
                    }
                    if (document.getElementById("<%=txt_EndTime.ClientID%>").value == "" || document.getElementById("<%=txt_EndTime.ClientID%>").value == "__:__") {
                        swal("End time field can not be blank", '', 'warning');
                        document.getElementById("<%=txt_EndTime.ClientID%>").focus();
                        return false;
                    }
                    if (document.getElementById("<%=txt_MeetingTime.ClientID%>").value.indexOf('_') == "-1" && document.getElementById("<%=txt_MeetingRemarks.ClientID%>").value=="") {
                        swal("Meeting Remarks field can not be blank", '', 'warning');
                        document.getElementById("<%=txt_Remarks.ClientID%>").focus();
                        return false;
                    }
                    if (document.getElementById("<%=txt_Remarks.ClientID%>").value == "") {
                        swal("Remarks field can not be blank", '', 'warning');
                        document.getElementById("<%=txt_Remarks.ClientID%>").focus();
                        return false;
                    }

                    calculate();
                    preventMultipleSubmissions();
                    return true;

                }


            </script>
            <script type="text/javascript">
                function dateselect(sender, args) {
                    var calendarBehavior1 = sender;
                    var d = calendarBehavior1._selectedDate;
                    var now = new Date();
                    calendarBehavior1.get_element().value = d.format("dd/MM/yyyy") + " " + now.format("HH:mm")
                    //calendarBehavior1.hide();

                }
            </script>
            <%--ClockPicker for Start time and End Time--%>
            <script type="text/javascript">
                var input = $('.timepicker');
                input.clockpicker({
                    placement: 'top',
                    autoclose: true
                });
                Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function (evt, args) {
                    var input = $('.timepicker');
                    input.clockpicker({
                        placement: 'top',
                        autoclose: true
                    });
                });
            </script>
            <%--Total Time Calculation--%>
            <script type="text/javascript">
                function calculate() {
                    var starttime = document.getElementById("<%=txt_StartTime.ClientID %>").value.toString();
                    var endtime = document.getElementById("<%=txt_EndTime.ClientID %>").value.toString();
                    var meetingtime = document.getElementById("<%=txt_MeetingTime.ClientID %>").value.toString();
                    if (starttime != "__:__" && endtime != "__:__") {
                        //Calculate Totaltime function
                        var startparts = starttime.toString().split(":");
                        var endparts = endtime.toString().split(":");
                        var meetingparts = meetingtime.toString().split(":");
                        var fromhours = startparts[0];
                        var frommins = startparts[1];
                        var tohours = endparts[0];
                        var tomins = endparts[1];
                        var meetinghours = meetingparts[0];
                        var meetingmins = meetingparts[1];
                        //Checking for start time and end time is valid time
                        if (fromhours > 23 || frommins > 59) {
                            swal("Start Time is Invalid");
                        }
                        else if (tohours > 23 || tomins > 59) {
                            swal("End Time is Invalid");
                        }
                        else {
                            var totalhours = tohours - fromhours;
                            if (totalhours.toString().substring(0, 1) == '-') {
                                totalhours = 24 + totalhours;
                            }
                            var totalmins = tomins - frommins;
                            if (totalmins.toString().substring(0, 1) == '-') {
                                totalmins = 60 + totalmins;
                                totalhours = totalhours - 1;
                            }

                            //breaktime
                            var breaktime = document.getElementById("<%=rbl_BreakTime.ClientID %>");
                            var breaktime_ListItem = document.getElementsByTagName('input');
                            for (var i = 0; i < breaktime_ListItem.length; i++) {
                                if (breaktime_ListItem[i].checked) {
                                    var lblAspradiobuttonValue = document.getElementById('<%= rbl_BreakTime.ClientID %>');
                                    var selectedvalueradio = breaktime_ListItem[i].value;
                                    totalmins = totalmins - selectedvalueradio;
                                    if (totalmins.toString().substring(0, 1) == '-') {
                                        totalmins = 60 + totalmins;
                                        totalhours = totalhours - 1;
                                    }
                                }
                            }
                            //meetingtime
                            if (meetingtime != "__:__") {
                                if (meetinghours > 23 || meetingmins > 59) {
                                    swal("Meeting Time is Invalid");
                                }
                                else {
                                    totalhours = totalhours - meetinghours;
                                    totalmins = totalmins - meetingmins;
                                    if (totalhours.toString().substring(0, 1) == '-') {

                                    }
                                    if (totalmins.toString().substring(0, 1) == '-') {
                                        totalmins = 60 + totalmins;
                                        totalhours = totalhours - 1;
                                    }
                                }
                            }
                            //Total time conversion
                            if (totalmins < 10) {
                                totalmins = '0' + totalmins;
                            }
                            if (totalhours < 10) {
                                totalhours = '0' + totalhours;
                            }
                            var totaltime = totalhours + ':' + totalmins;
                            //document.getElementById("<%=txt_TotalTime.ClientID %>").value = totaltime;
                            $totTime = $("#<%=txt_TotalTime.ClientID %>");
                            $($totTime).val(totaltime);
                            $("#<%=hdn_TotalTime.ClientID %>").val(totaltime);
                        }
                }
            }
            </script>
        </div>
    </div>
</asp:Content>
