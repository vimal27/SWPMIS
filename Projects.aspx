<%@ Page Title="Projects" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Projects.aspx.cs" Inherits="ProjectReq" %>

<%@ Register TagPrefix="asp" Namespace="Saplin.Controls" Assembly="DropDownCheckBoxes" %>
<%@ Register Assembly="DropDownCheckBoxes" Namespace="Saplin.Controls" TagPrefix="cc2" %>
<%-- Ajax Control Toolkit Extender To Support Calendar Extender --%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%--Head Content--%>
    <script type="text/javascript">

        function deleteProject() {
            $('modalDelete').show();
        }
    </script>
    <%-- Inline CSS --%>
    <style type="text/css">
        .datatable tbody
        {
            text-align: center !important;
        }
        
        .datatable thead th
        {
            text-align: center !important;
        }
        
        .table > thead > tr > th, .table > tbody > tr > th, .table > tfoot > tr > th, .table > thead > tr > td, .table > tbody > tr > td, .table > tfoot > tr > td
        {
            border-width: 0.1px;
            border: 1px solid #ddd;
        }
        
        .modal-backdrop.in
        {
            opacity: 0.4 !important;
        }
        
        .label-title
        {
            width: 162px !important;
        }
        
        .input
        {
        }
        
        .input-wide
        {
            width: 500px;
        }
        
        .CSS_Pager span
        {
            padding-left: 10px;
            padding-right: 10px;
            font-weight: bold;
        }
        
        input[type="radio"], input[type="checkbox"]
        {
            margin: 0 7px 0px !important;
        }
        
        .chkTeam input
        {
            margin-left: -20px !important;
        }
        
        .chkTeam td
        {
            padding-left: 20px !important;
        }
        
        block_Register .flat-blue .table > tbody > tr > td, .flat-blue .table > tbody > tr > th, .flat-blue .table > tfoot > tr > td, .flat-blue .table > tfoot > tr > th
        {
            border-color: #FFFFFF !important;
        }
    </style>
    <%-- External CSS To Bootstrap Datatable And Fone Awesome--%>
    <link rel="stylesheet" type="text/css" href="assets/css/Datatable/dataTables.bootstrap.min.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--Heading Of MainContent--%>
    <div class="card">
        <div class="card-header">
            <div class="card-title">
                <div class="title" id="TitleOfPage" runat="server">
                    Project Master
                </div>
            </div>
        </div>
        <%--Main Content--%>
        <asp:UpdatePanel ID="upl_All" runat="server">
            <ContentTemplate>
                <div class="card-body" id="block_Grid" runat="server">
                    <%-- Add New Project and Export To Excel Button --%>
                    <div class="toolbar">
                        <asp:Button ID="btn_NewProject" class="btn btn-success" TabIndex="1" Text="Add New Project"
                            runat="server" OnClick="btn_NewProject_Click" />
                        <asp:Button ID="btn_Export" class="btn btn-info" runat="server" TabIndex="2" CausesValidation="false"
                            Text="Export" OnClick="btn_Export_Click" />
                    </div>
                    <%--GridView For Projects--%>
                    <div class="form-group">
                        <asp:Panel ID="pnl_Projects" runat="server">
                            <asp:GridView ID="grd_Projects" runat="server" CssClass="dataTables_wrapper dt-foundation display datatable"
                                AutoGenerateColumns="False" DataKeyNames="projectid" OnRowCancelingEdit="grd_Projects_RowCancelingEdit"
                                OnRowUpdating="grd_Projects_RowUpdating" OnRowDeleting="grd_Projects_RowDeleting"
                                OnSelectedIndexChanged="grd_Projects_SelectedIndexChanged" OnRowEditing="grd_Projects_RowEditing"
                                OnRowDataBound="grd_Projects_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Request ID" HeaderStyle-CssClass="firstcolumn">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_ProjectID" runat="server" Text='<%# Eval("projectid") %>'></asp:Label>
                                            <asp:HiddenField ID="hdn_ReceivedFrom" runat="server" Value='<%# Eval("receivedfrom") %>'>
                                            </asp:HiddenField>
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
                                            <asp:HiddenField ID="hdn_AllotedTeamName" runat="server" Value='<%# Eval("allotedteamname") %>'>
                                            </asp:HiddenField>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Received Date" HeaderStyle-CssClass="ReceivedDate">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_ReceivedDate" runat="server" Text='<%# Eval("receiveddate2") %>'></asp:Label>
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
                                            <asp:LinkButton ID="lnk_Edit" runat="server" TabIndex="3" CommandName="Select" ToolTip="Update"
                                                Text='<i class="fa fa-pencil-square-o"></i>' ForeColor="#5AC15A" Font-Size="23px">
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="lnk_View" runat="server" OnClick="SelectCurrentData" CommandArgument='<%# bind("projectid") %>'
                                                ToolTip="View Project" Text='<i class="glyphicon glyphicon-eye-open"></i>' ForeColor="#19B5FE"
                                                Font-Size="Large">                    
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="lnk_Delete" TabIndex="4" runat="server" CommandArgument='<%# bind("projectid") %>'
                                                ToolTip="Delete" OnClick="lnk_Delete_Click" Text='<i class="glyphicon glyphicon-pause"></i>'
                                                ForeColor="#D45E5E" Font-Size="Large">    
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="lnk_TaskDetails" TabIndex="5" OnClick="lnk_TaskDetails_Click"
                                                runat="server" CommandArgument='<%# bind("projectid") %>' ToolTip="View Task Details"
                                                Text='<i class="glyphicon glyphicon-search"></i>' ForeColor="lime" Font-Size="Large">    
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
                                        <HeaderStyle CssClass="removecolumn"></HeaderStyle>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                        <%--Block For Popup While Delete--%>
                        <asp:LinkButton Text="" ID="lnk_DeletePopup" runat="server" />
                        <cc1:ModalPopupExtender ID="popup_Delete" PopupControlID="pnl_ProjectDel" TargetControlID="lnk_DeletePopup"
                            BackgroundCssClass="modalBackground" runat="server">
                        </cc1:ModalPopupExtender>
                        <asp:Panel ID="pnl_ProjectDel" CssClass="modalPopup" runat="server" Style="display: none;
                            width: 60%; height: 50%; border-color: rgba(230, 8, 8, 0.48)">
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
                                    <div class="left-inner-addon">
                                        <label class="label-title">
                                            Status</label>
                                        <i class="icon-exclamation-sign"></i>
                                        <asp:DropDownList ID="ddl_DelStatus" CssClass="form-control" Width="50%" runat="server">
                                            <asp:ListItem Value="Hold">Hold</asp:ListItem>
                                            <asp:ListItem Value="Closed">Closed</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group" style="margin-left: 10%">
                                    <asp:Button ID="btn_DeleteProject" class="btn btn-info" ValidationGroup="Delete"
                                        runat="server" OnClick="btn_DeleteProject_Click" Text="Submit" />
                                    <asp:Button ID="btn_Cancel" class="btn btn-danger" runat="server" OnClick="btn_Close"
                                        Text="Cancel" />
                                </div>
                            </div>
                        </asp:Panel>
                        <%--Block For Popup While Release Hold--%>
                        <asp:LinkButton Text="" ID="lnk_ReleasePopup" runat="server" />
                        <cc1:ModalPopupExtender ID="popup_Release" PopupControlID="pnl_ProjectRelease" TargetControlID="lnk_ReleasePopup"
                            BackgroundCssClass="modalBackground" runat="server">
                        </cc1:ModalPopupExtender>
                        <asp:Panel ID="pnl_ProjectRelease" CssClass="modalPopup" runat="server" Style="display: none;
                            width: 40%; height: 40%">
                            <div class="panel-heading">
                                Change State?
                            </div>
                            <div class="panel-body">
                                <div class="form-group" style="margin-left: 10%">
                                    <div class="left-inner-addon">
                                        <label class="label-title">
                                            Status</label>
                                        <i class="icon-exclamation-sign"></i>
                                        <asp:DropDownList ID="ddl_RelStatus" CssClass="form-control" Width="50%" runat="server">
                                            <asp:ListItem Value="WIP">Active</asp:ListItem>
                                            <asp:ListItem Value="Closed">Closed</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group" style="margin-left: 10%">
                                    <asp:Button ID="btn_ReleaseProject" class="btn btn-info" ValidationGroup="Release"
                                        runat="server" OnClick="btn_ReleaseProject_Click" Text="Ok" />
                                    <asp:Button ID="btn_CancelRelease" class="btn btn-danger" runat="server" OnClick="btn_Close"
                                        Text="Cancel" />
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                    <%--Block For Popup While Change Project To Completed State--%>
                    <asp:LinkButton Text="" ID="lnk_CompletedPopup" runat="server" />
                    <cc1:ModalPopupExtender ID="popup_CompletedState" PopupControlID="pnl_CompletedState"
                        BackgroundCssClass="modalBackground" TargetControlID="lnk_CompletedPopup" runat="server">
                    </cc1:ModalPopupExtender>
                    <asp:Panel ID="pnl_CompletedState" CssClass="modalPopup" runat="server" Style="display: none;
                        width: 50%; height: 48%">
                        <div class="panel-heading" id="title_OnCompleted" runat="server">
                            Confirm Completed?
                        </div>
                        <div class="panel-body">
                            <div class="form-group" style="margin-left: 10%">
                                <div class="left-inner-addon">
                                    <label class="label-title">
                                        State</label>
                                    <i class="fa fa-signal"></i>
                                    <asp:DropDownList ID="ddl_ProjectState" AutoPostBack="true" OnSelectedIndexChanged="ddl_ProjectState_SelectedIndexChanged"
                                        Width="50%" CssClass="form-control" runat="server">
                                        <asp:ListItem Value="Completed">Completed</asp:ListItem>
                                        <asp:ListItem Value="WIP">WIP</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group" style="margin-left: 10%">
                                <div class="left-inner-addon">
                                    <label class="label-title" runat="server" id="lbl_CompletedDate">
                                        Date</label>
                                    <i class="fa fa-calendar" runat="server" visible="false" id="icon_CompletedDate">
                                    </i>
                                    <asp:TextBox ID="txt_CompletedDate" Width="50%" OnTextChanged="txt_CompletedDate_TextChanged"
                                        runat="server" AutoPostBack="true" class="form-control" TabIndex="4" ValidationGroup="Complete"></asp:TextBox>
                                    <cc1:CalendarExtender ID="calext_CompletedDate" CssClass="custom-calendar" PopupPosition="TopLeft"
                                        runat="server" TargetControlID="txt_CompletedDate" Format="dd/MM/yyyy">
                                    </cc1:CalendarExtender>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Completed Date is Mandatory"
                                        Display="Dynamic" ValidationGroup="Complete" ControlToValidate="txt_CompletedDate"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <%--Start Project Re-Open Block--%>
                            <div runat="server" id="block_ReOpenProject" visible="false">
                                <div class="form-group" style="margin-left: 10%">
                                    <div class="left-inner-addon">
                                        <label class="label-title" runat="server" id="lbl_ReceivedDateReOpen">
                                            Received Date</label>
                                        <i class="fa fa-calendar"></i>
                                        <asp:TextBox ID="txt_ReceivedDateReOpen" ValidationGroup="Complete" OnTextChanged="txt_ReceivedDateReOpen_TextChanged"
                                            AutoPostBack="true" Width="50%" runat="server" class="form-control"></asp:TextBox>
                                        <cc1:CalendarExtender ID="calext_ReceivedDateReOpen" CssClass="custom-calendar" PopupPosition="TopLeft"
                                            runat="server" TargetControlID="txt_ReceivedDateReOpen" Format="dd/MM/yyyy">
                                        </cc1:CalendarExtender>
                                        <asp:RequiredFieldValidator ID="rfv_ReceivedDateReOpen" ValidationGroup="Complete"
                                            runat="server" ErrorMessage="Received Date For Project Re-Open is Mandatory"
                                            Display="Dynamic" ControlToValidate="txt_ReceivedDateReOpen"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group" style="margin-left: 10%">
                                    <div class="left-inner-addon">
                                        <label class="label-title" runat="server" id="lbl_DueDateReOpen">
                                            Due Date</label>
                                        <i class="fa fa-calendar"></i>
                                        <asp:TextBox ID="txt_DueDateReOpen" ValidationGroup="Complete" Width="50%" runat="server"
                                            class="form-control"></asp:TextBox>
                                        <cc1:CalendarExtender ID="calext_DueDateReOpen" CssClass="custom-calendar" PopupPosition="TopLeft"
                                            runat="server" TargetControlID="txt_DueDateReOpen" Format="dd/MM/yyyy">
                                        </cc1:CalendarExtender>
                                        <asp:RequiredFieldValidator ID="rfv_DueDateReOpen" ValidationGroup="Complete" runat="server"
                                            ErrorMessage="Due Date For Project Re-Open is Mandatory" Display="Dynamic" ControlToValidate="txt_DueDateReOpen">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group" style="margin-left: 10%">
                                    <label class="label-title" runat="server" id="lbl_DescriptionReOpen">
                                        Description</label>
                                    <asp:TextBox ID="txt_DescriptionReOpen" TextMode="MultiLine" ValidationGroup="Complete"
                                        Width="50%" runat="server" class="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfv_DescriptionReOpen" ValidationGroup="Complete"
                                        runat="server" ErrorMessage="Description is Mandatory" Display="Dynamic" ControlToValidate="txt_DescriptionReOpen">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <%--End Project Re-Open Block--%>
                            <div class="form-group" style="margin-left: 10%">
                                <b>
                                    <asp:Label runat="server" ID="lbl_ReasonForExtend" CssClass="label-title" Text="Reason For Extend"
                                        Visible="false"></asp:Label></b>
                                <asp:TextBox ID="txt_ReasonForExtend" Width="50%" runat="server" TextMode="MultiLine"
                                    class="form-control" TabIndex="4" ValidationGroup="Complete"></asp:TextBox>
                            </div>
                            <div class="form-group" style="margin-left: 10%">
                                <asp:Button ID="btn_CompleteProject" OnClick="btn_CompleteProject_Click" class="btn btn-info"
                                    ValidationGroup="Complete" runat="server" Text="Submit" />
                                <asp:Button ID="btn_Exit" class="btn btn-danger" OnClick="btn_Exit_Click" runat="server"
                                    Text="Cancel" />
                            </div>
                        </div>
                    </asp:Panel>
                </div>
                <%--Block For Projects View--%>
                <div id="block_View" class="card-body" runat="server" visible="false">
                    <div class="form-group">
                        <label class="label-title" id="lbl_ProjectIDView" runat="server" style="font-weight: bold">
                            Request ID
                        </label>
                        <label class="label-title" id="lbl_ProjectIDTextView" style="font-weight: normal"
                            runat="server">
                        </label>
                        </br>
                        <label id="lbl_ProjectNameView" runat="server" class="label-title" style="font-weight: bold">
                            Project Name
                        </label>
                        <label id="lbl_ProjectNameTextView" runat="server" class="label-title" style="font-weight: normal">
                        </label>
                        </br>
                        <label id="lbl_ManualIDView" runat="server" class="label-title" style="font-weight: bold">
                            Project ID
                        </label>
                        <label id="lbl_ManualIDTextView" runat="server" class="label-title" style="font-weight: normal">
                        </label>
                        </br>
                        <label id="lbl_ProjectTypeView" runat="server" class="label-title" style="font-weight: bold">
                            Project Type
                        </label>
                        <label id="lbl_ProjectTypeTextView" runat="server" class="label-title" style="font-weight: normal">
                        </label>
                        </br>
                        <label id="lbl_AllotedTeamView" runat="server" class="label-title" style="font-weight: bold">
                            Alloted Teams
                        </label>
                        <label id="lbl_AllotedTeamTextView" runat="server" class="label-title" style="font-weight: normal">
                        </label>
                        </br>
                        <label id="lbl_ReceivedDateView" runat="server" class="label-title" style="font-weight: bold">
                            Received Date
                        </label>
                        <label id="lbl_ReceivedDateTextView" runat="server" class="label-title" style="font-weight: normal">
                        </label>
                        </br>
                        <label id="lbl_DueDateView" runat="server" class="label-title" style="font-weight: bold">
                            Due Date
                        </label>
                        <label id="lbl_DueDateTextView" runat="server" class="label-title" style="font-weight: normal">
                        </label>
                        </br>
                        <label id="lbl_ProjectDescView" runat="server" class="label-title" style="font-weight: bold">
                            Project Description
                        </label>
                        <label id="lbl_ProjectDescTextView" runat="server" class="label-title" style="font-weight: normal">
                        </label>
                        </br>
                        <label id="lbl_RemarksView" runat="server" class="label-title" style="font-weight: bold">
                            Remarks
                        </label>
                        <label id="lbl_RemarksTextView" runat="server" class="label-title" style="font-weight: normal">
                        </label>
                        </br>
                        <asp:Button ID="btn_ExportDPRs" runat="server" class="btn btn-info" OnClick="btn_ExportDPRs_Click"
                            Text="Export DPR List" />
                        <asp:Button ID="btn_BackButton" runat="server" class="btn btn-warning" OnClick="btn_BackButtonClick"
                            Text="Back" />
                        </br>
                    </div>
                    <%-- Add Daily Remark For Project --%>
                    <div class="card-header">
                        <div class="card-title">
                            <div class="title" id="title_Remarks" runat="server">
                                Daily Remarks
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="left-inner-addon">
                            <label class="label-title">
                                Date</label>
                            <i class="fa fa-calendar" style="margin-top: 5%;"></i>
                            <asp:TextBox ID="txt_RemarkDate" Style="margin-top: 5%;" class="form-control" runat="server"></asp:TextBox>
                            <cc1:CalendarExtender ID="calext_RemarkDate" CssClass="custom-calendar" PopupPosition="TopLeft"
                                runat="server" TargetControlID="txt_RemarkDate" Format="dd/MM/yyyy">
                            </cc1:CalendarExtender>
                            <span style="color: #ff0000">**</span>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="label-title">
                            Add Remark</label>
                        <asp:TextBox ID="tbx_ProjectRemarks" class="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                        <span style="color: #ff0000">**</span>
                    </div>
                    <%-- Button For Submit Project Remarks --%>
                    <asp:Button ID="Btn_AddProjectRemarks" OnClick="Btn_AddProjectRemarks_Click" Text="Add"
                        class="btn btn-info" runat="server"></asp:Button>
                    <div class="form-group">
                        <asp:GridView ID="grd_AddRemark" runat="server" CssClass="datatable table table-striped dataTable"
                            AutoGenerateColumns="False" DataKeyNames="id" OnRowDeleting="grd_AddRemark_RowDeleting">
                            <Columns>
                                <asp:TemplateField HeaderText="Request ID" HeaderStyle-CssClass="firstcolumn">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_RequestID" runat="server" Text='<%#Container.DataItemIndex + 1%>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="firstcolumn"></HeaderStyle>
                                    <HeaderStyle Width="15%"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remark" HeaderStyle-CssClass="ProjectID">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_ProjectRemark" runat="server" Text='<%# Eval("remark") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Width="50%"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remark By" HeaderStyle-CssClass="ProjectID">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_RemarkBy" runat="server" Text='<%# Eval("requestby") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Width="15%"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Date" HeaderStyle-CssClass="ProjectName">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_RemarkDate" runat="server" Text='<%# Eval("date","{0:dd/MM/yyyy}") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Width="10%"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Delete" HeaderStyle-CssClass="Delete">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnk_RemarkDelete" TabIndex="4" OnClientClick="ConfirmDelete(' this Remark? ')"
                                            CommandName="Delete" runat="server" ToolTip="Delete" Text='<i class="glyphicon glyphicon-trash"></i>'
                                            ForeColor="#D45E5E" Font-Size="Large">    
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle Width="10%"></HeaderStyle>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <%-- Add Daily Remark For Project --%>
                    <div id="block_ProjectHistory" runat="server" visible="false">
                        <div class="card-header">
                            <div class="card-title">
                                <div class="title" id="titleHistoryProject" runat="server">
                                    Project History
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:GridView ID="grd_ProjectHistory" runat="server" CssClass="datatable table table-striped dataTable"
                            AutoGenerateColumns="False" DataKeyNames="id">
                            <Columns>
                                <asp:TemplateField HeaderText="Revision" HeaderStyle-CssClass="firstcolumn">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_HistoryID" runat="server" Text='<%#Container.DataItemIndex + 1%>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="firstcolumn"></HeaderStyle>
                                    <HeaderStyle Width="5%"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Project" HeaderStyle-CssClass="ProjectID">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_HistoryProject" runat="server" Text='<%# Eval("projectreq") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Width="12.8%"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description" HeaderStyle-CssClass="ProjectID">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_HistoryDescription" runat="server" Text='<%# Eval("projectdesc") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Width="12.8%"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Received Date" HeaderStyle-CssClass="ProjectName">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_HistoryReceivedDate" runat="server" Text='<%# Eval("receiveddate") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Width="12.8%"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Due Date" HeaderStyle-CssClass="ProjectName">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_HistoryDueDate" runat="server" Text='<%# Eval("duedate") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Width="12.8%"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Completed Date" HeaderStyle-CssClass="ProjectName">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_HistoryCompletedDate" runat="server" Text='<%# Eval("completeddate") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Width="12.8%"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Reason For Extend" HeaderStyle-CssClass="ProjectName">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_HistoryExtendReason" runat="server" Text='<%# Eval("extendreason") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Width="17.8%"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Reopened On" HeaderStyle-CssClass="ProjectName">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_HistoryReOpenedDate" runat="server" Text='<%# Eval("ReOpenDate") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Width="12.8%"></HeaderStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Reopened By" HeaderStyle-CssClass="ProjectName">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_HistoryReOpenedBy" runat="server" Text='<%# Eval("ReOpenBy") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Width="12.8%"></HeaderStyle>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <%--Block For Project ADD--%>
                <div id="block_Register" runat="server">
                    <div class="card-body" style="overflow-x: auto">
                        <asp:Label class="alert alert-danger" role="alert" ID="lbl_Already" runat="server"
                            Text="" Visible="false"></asp:Label>
                        <asp:HiddenField ID="hdn_ProjectID" runat="server" />
                        <table id="Register" class="table table-responsive table-bordered">
                            <tr>
                                <div class="form-group">
                                    <td>
                                        <label class="label-title">
                                            Request ID</label>
                                    </td>
                                    <td>
                                        <div class="left-inner-addon">
                                            <i class="fa fa-database"></i>
                                            <input type="text" style="width: 80%" runat="server" class="form-control" id="txt_ProjectID"
                                                onfocus="nextfield ='Button1';" tabindex="1" onclick="return txt_ProjectID_onclick()" /><span
                                                    style="color: #ff0000"></span>
                                            <asp:DropDownList ID="ddl_ProjectID" runat="server" AutoPostBack="true" TabIndex="15"
                                                OnSelectedIndexChanged="ddl_ProjectID_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                </div>
                                <div class="form-group">
                                    <td>
                                        <label class="label-title">
                                            Received Date</label>
                                    </td>
                                    <td>
                                        <div class="left-inner-addon">
                                            <i class="fa fa-calendar"></i>
                                            <asp:TextBox Style="width: 80%" ID="txt_ReceivedDate" runat="server" AutoPostBack="true"
                                                class="form-control" TabIndex="5" ValidationGroup="project" OnTextChanged="txt_ReceivedDate_TextChanged"></asp:TextBox>
                                            <cc1:CalendarExtender ID="calext_ReceivedDate" CssClass="custom-calendar" PopupPosition="TopLeft"
                                                runat="server" TargetControlID="txt_ReceivedDate" Format="dd/MM/yyyy">
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
                                            Project ID</label>
                                    </td>
                                    <td>
                                        <div class="left-inner-addon">
                                            <i class="fa fa-folder-open"></i>
                                            <input id="txt_ClientProjectID" style="width: 80%" maxlength="20" runat="server"
                                                class="form-control" onfocus="nextfield ='Projectid';" onkeypress="return alphanumeric()"
                                                tabindex="1" type="text" />
                                            <span style="color: #ff0000">**</span>
                                        </div>
                                    </td>
                                </div>
                                <div class="form-group">
                                    <td>
                                        <label class="label-title">
                                            Due Date</label>
                                    </td>
                                    <td>
                                        <div class="left-inner-addon">
                                            <i class="fa fa-calendar"></i>
                                            <asp:TextBox ID="txt_DueDate" Style="width: 80%" runat="server" class="form-control"
                                                TabIndex="6" ValidationGroup="project"></asp:TextBox>
                                            <cc1:CalendarExtender ID="txt_DueDateCalendarExtender" CssClass="custom-calendar"
                                                PopupPosition="TopLeft" runat="server" TargetControlID="txt_DueDate" Format="dd/MM/yyyy">
                                            </cc1:CalendarExtender>
                                        </div>
                                    </td>
                                </div>
                            </tr>
                            <tr>
                                <div class="form-group">
                                    <td>
                                        <label class="label-title">
                                            Project Name</label>
                                    </td>
                                    <td>
                                        <div class="left-inner-addon">
                                            <i class="fa fa-file-text"></i>
                                            <input id="txt_ProjectName" maxlength="200" style="width: 80%" runat="server" class="form-control"
                                                onfocus="nextfield ='Projectid';" onkeypress="return alphanumeric()" tabindex="2"
                                                type="text" validationgroup="project" />
                                            <span style="color: #ff0000">**</span>
                                        </div>
                                    </td>
                                </div>
                                <div class="form-group">
                                    <td>
                                        <label class="label-title">
                                            Description</label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbx_Desc" MaxLength="5000" Style="width: 80%" class="form-control"
                                            runat="server" TabIndex="7" TextMode="MultiLine" ValidationGroup="project"></asp:TextBox>
                                        <span style="color: #ff0000">**</span>
                                    </td>
                                </div>
                            </tr>
                            <tr>
                                <div class="form-group">
                                    <td>
                                        <label style="float: down" class="label-title">
                                            Project Type</label><br>
                                        <b>
                                            <asp:Label ID="lbl_ProjectSubType" runat="server" class="label-title" Style="margin-top: 4%"
                                                Visible="false">Project Sub Type</asp:Label>
                                        </b><b>
                                            <asp:Label ID="lbl_ReceivedFrom" runat="server" class="label-title" Style="margin-top: 4%"
                                                Visible="false">Received From</asp:Label>
                                        </b>
                                    </td>
                                    <td>
                                        <div class="left-inner-addon">
                                            <i class="fa fa-th-large"></i>
                                            <asp:DropDownList ID="ddl_ProjectType" AutoPostBack="true" OnSelectedIndexChanged="ddl_ProjectType_SelectedIndexChanged"
                                                TabIndex="3" Style="width: 80%" CssClass="form-control" runat="server">
                                                <asp:ListItem Value="">Select</asp:ListItem>
                                                <asp:ListItem Value="Internal">Internal</asp:ListItem>
                                                <asp:ListItem Value="External">External</asp:ListItem>
                                                <asp:ListItem Value="CB">CB</asp:ListItem>
                                            </asp:DropDownList>
                                            <span style="color: #ff0000">**</span>
                                        </div>
                                        <div class="left-inner-addon">
                                            <i class="fa fa-th" id="icon_SubType" visible="false" runat="server" style="margin-top: 2%">
                                            </i>
                                            <asp:DropDownList ID="ddl_ProjectSubType" Visible="false" Style="width: 80%; margin-top: 2%"
                                                CssClass="form-control" runat="server">
                                                <asp:ListItem></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <%--Reason For Extend Textbox--%>
                                        <div class="left-inner-addon">
                                            <i class="fa fa-user" id="icon_ReceivedFrom" visible="false" runat="server" style="margin-top: 2%">
                                            </i>
                                            <asp:TextBox ID="txt_ReceivedFrom" MaxLength="50" Style="width: 80%; margin-top: 2%"
                                                CssClass="form-control" runat="server" Visible="false"></asp:TextBox>
                                        </div>
                                    </td>
                                </div>
                                <div class="form-group">
                                    <td>
                                        <label class="label-title">
                                            Scopes</label>
                                    </td>
                                    <td>
                                        <asp:GridView ID="grd_Scope" runat="server" Style="width: 80%" CssClass="table" AutoGenerateColumns="False"
                                            DataKeyNames="ID" OnRowCancelingEdit="grd_Scope_RowCancelingEdit" OnRowEditing="grd_Scope_RowEditing"
                                            OnRowUpdating="grd_Scope_RowUpdating" OnRowDeleting="grd_Scope_RowDeleting" AllowPaging="true"
                                            OnPageIndexChanging="OnPageIndexChangingScope" PageSize="3">
                                            <PagerStyle CssClass="CSS_Pager" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Scope">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txt_Scope" runat="server" Text='<%# Eval("Scope") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Scope" runat="server" Text='<%# Eval("Scope") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Description">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txt_Description" runat="server" Text='<%# Eval("Description") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Description" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Edit">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnk_Edit" runat="server" CommandName="Edit" ToolTip="edit" Text='<i class="fa fa-pencil-square-o"></i>'
                                                            ForeColor="#19B5FE" Font-Size="23px">
                                                        </asp:LinkButton>
                                                        <asp:LinkButton ID="lnk_Delete" OnClick="lnk_Delete_Click" OnClientClick="ConfirmDelete(' this scope? ')"
                                                            runat="server" CommandName="Delete" ToolTip="Hold/Close" Text='<i class="glyphicon glyphicon-pause"></i>'
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
                                        </asp:GridView>
                                        <button type="button" class="btn btn-primary" tabindex="8" data-toggle="modal" data-target="#modalDefault">
                                            Add Scope
                                        </button>
                                    </td>
                                </div>
                            </tr>
                            <tr>
                                <div class="form-group">
                                    <td>
                                        <label class="label-title" style="float: left">
                                            Allot Team</label>
                                    </td>
                                    <td>
                                        <div class="panel panel-primary" id="pnl_Team" style="width: 80%">
                                            <div class="panel-heading" style="font-weight: bold">
                                                Assign Team For This Project<span style="color: #ff0000;"> **</span>
                                            </div>
                                            <div class="panel-body">
                                                <asp:CheckBoxList ID="chk_Team" RepeatColumns="3" TabIndex="4" RepeatDirection="Horizontal"
                                                    RepeatLayout="Table" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chk_Team_SelectedIndexChanged"
                                                    CellPadding="5" CellSpacing="1" Font-Names="Verdana">
                                                </asp:CheckBoxList>
                                            </div>
                                        </div>
                                    </td>
                                </div>
                                <div class="form-group">
                                    <td>
                                        <label class="label-title">
                                            Co-Ordinator</label>
                                    </td>
                                    <td>
                                        <asp:GridView ID="grd_Coordinator" runat="server" Style="width: 80%" CssClass="table"
                                            AutoGenerateColumns="False" DataKeyNames="ID" OnRowCancelingEdit="grd_Coordinator_RowCancelingEdit"
                                            OnRowEditing="grd_Coordinator_RowEditing" OnRowUpdating="grd_Coordinator_RowUpdating"
                                            OnRowDeleting="grd_Coordinator_RowDeleting" AllowPaging="true" OnPageIndexChanging="OnPageIndexChangingCoordinator"
                                            PageSize="3">
                                            <PagerStyle CssClass="CSS_Pager" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Co-Ordinators">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txt_Coordinator" runat="server" Text='<%# Eval("name") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Coordinator" runat="server" Text='<%# Eval("name") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Edit">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnk_Edit" runat="server" CommandName="Edit" ToolTip="edit" Text='<i class="fa fa-pencil-square-o"></i>'
                                                            ForeColor="#19B5FE" Font-Size="23px">
                                                        </asp:LinkButton>
                                                        <asp:LinkButton ID="lnk_Delete" OnClick="lnk_DeleteCoordinator_Click" OnClientClick="ConfirmDelete(' this Co-Ordinator? ')"
                                                            runat="server" CommandName="Delete" ToolTip="Delete Co-Ordinator" Text='<i class="glyphicon glyphicon-trash"></i>'
                                                            ForeColor="#D45E5E" Font-Size="Large">                    
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:LinkButton ID="lnk_UpdateCoordinator" runat="server" CommandName="Update" ToolTip="Update Co-Ordinator"
                                                            Text='<i class="glyphicon glyphicon-saved"></i>' ForeColor="#5AC15A" Font-Size="Large">                    
                                                        </asp:LinkButton>
                                                        <asp:LinkButton ID="lnk_Cancel" runat="server" CommandName="Cancel" ToolTip="Cancel Edit"
                                                            Text='<i class="glyphicon glyphicon-remove"></i>' ForeColor="#5E5EDE" Font-Size="Large">                    
                                                        </asp:LinkButton>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <button type="button" class="btn btn-primary" tabindex="8" data-toggle="modal" data-target="#modalCoordinator">
                                            Add Co-Ordinator
                                        </button>
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
                                        <asp:TextBox ID="tbx_Remarks" Style="width: 80%" class="form-control" runat="server"
                                            TabIndex="9" TextMode="MultiLine" ValidationGroup="project"></asp:TextBox>
                                    </td>
                                </div>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td colspan="3">
                                    <asp:Button ID="btn_Save" class="btn btn-success" OnClick="btn_Save_Click" runat="server"
                                        Text="Save" TabIndex="10" OnClientClick="javascript: return validate();" CausesValidation="true"
                                        ValidationGroup="project"></asp:Button>
                                    <asp:Button ID="btn_Reset" class="btn btn-warning" OnClick="btn_Reset_Click" runat="server"
                                        Text="Reset" CausesValidation="false" TabIndex="11"></asp:Button>
                                    <asp:Button ID="Btn_Back" class="btn btn-info" OnClick="btn_Back_Click" runat="server"
                                        Text="Back" CausesValidation="false" TabIndex="12"></asp:Button>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <div align="left" style="margin-left: 5%">
                                        <span style="color: #ff0000">**</span><font color="blue"> Required field</font>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <br />
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btn_Export" />
                <asp:PostBackTrigger ControlID="btn_ExportDPRS" />
            </Triggers>
        </asp:UpdatePanel>
        <%--Button Trigger Modal For Scope--%>
        <!-- Modal For Scope -->
        <div class="modal fade" id="modalDefault" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
            aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title" id="lbl_scope">
                            Scope/Services</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <div class="left-inner-addon">
                                <label class="label-title">
                                    Scope</label>
                                <i class="fa fa-language"></i>
                                <asp:TextBox ID="txt_Scope" class="form-control" Style="width: 50%" runat="server"
                                    ValidationGroup="scope"></asp:TextBox><span style="color: #ff0000"> **</span>
                                <asp:RequiredFieldValidator ID="rf_Scope" runat="server" ErrorMessage="Scope Name is Mandatory"
                                    Display="Dynamic" ValidationGroup="scope" ControlToValidate="txt_Scope"></asp:RequiredFieldValidator>
                            </div>
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
        <!-- Modal -->
        <%--Button Trigger Modal For Scope--%>
        <!-- Modal For Scope -->
        <div class="modal fade" id="modalCoordinator" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
            aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title" id="H1">
                            Add Co-Ordinator</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <div class="left-inner-addon">
                                <label class="label-title">
                                    Name</label>
                                <i class="fa fa-language"></i>
                                <asp:TextBox ID="txt_CoordinatorName" class="form-control" Style="width: 50%" runat="server"
                                    ValidationGroup="coordinator"></asp:TextBox><span style="color: #ff0000"> **</span>
                                <asp:RequiredFieldValidator ID="rfv_Coordinator" runat="server" ErrorMessage="Co-Ordinator Name is Mandatory"
                                    Display="Dynamic" ValidationGroup="coordinator" ControlToValidate="txt_CoordinatorName"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btn_AddCordinator" class="btn btn-primary" ValidationGroup="coordinator"
                            runat="server" OnClick="btn_AddCoordinator_Click" Text="Add" />
                        <asp:Button ID="btn_ClosePopup" Text="Cancel" class="btn btn-primary" runat="server"
                            OnClick="btn_ClosePopup_Click" />
                    </div>
                </div>
            </div>
        </div>
        <!-- Modal -->
    </div>
    <%--Javascript Validations--%>
    <script language="javascript" type="text/javascript">
        var atLeast = 1; function validate() {
            if (document.getElementById("<%=txt_ClientProjectID.ClientID%>").value == "") {
                swal("Project ID Field can not be blank"); document.getElementById("<%=txt_ClientProjectID.ClientID%>").focus();
                return false;
            }
            else if (document.getElementById("<%=txt_ProjectName.ClientID%>").value == "") {
                swal("Project Name Field can not be blank"); document.getElementById("<%=txt_ProjectName.ClientID%>").focus();
                return false;
            }
            else if (document.getElementById("<%=ddl_ProjectType.ClientID%>").selectedIndex == 0) {
                swal("Project Type Field is Mandatory");
                document.getElementById("<%=ddl_ProjectType.ClientID%>").focus();
                return false;
            }
            var CHK = document.getElementById("<%=chk_Team.ClientID%>");
            var checkbox = CHK.getElementsByTagName("input"); var counter = 0;
            for (var i = 0; i < checkbox.length; i++) {
                if (checkbox[i].checked) {
                    counter++;
                }
            }
            if (atLeast > counter) {
                swal("Please Select Atleast " + atLeast + " Team"); document.getElementById("<%=chk_Team.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txt_ReceivedDate.ClientID%>").value == "") {
                swal("Received Date Field can not be blank"); document.getElementById("<%=txt_ReceivedDate.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=tbx_Desc.ClientID%>").value == "") {
                swal("Description Field can not be blank"); document.getElementById("<%=tbx_Desc.ClientID%>").focus();
                return false;
            }
        }
    </script>
    <%-- External JS To Jquery,Bootstrap,Datatables --%>
    <script type="text/javascript" charset="utf8" src="assets/js/Datatable/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="assets/js/Datatable/dataTables.bootstrap.min.js"></script>
    <script type="text/javascript">
        var table;
        var checkExists = 0;
        $(document).ready(function () {
            checkExists = 1;
            setDatatable();
        });

        function setDatatable() {

            // Setup - add a text input to each footer cell
            $("#ctl00_ContentPlaceHolder1_grd_Projects").prepend($("<thead></thead><tfoot></tfoot>").append($("#ctl00_ContentPlaceHolder1_grd_Projects").find("tr:first")));

            $('#ctl00_ContentPlaceHolder1_grd_Projects tfoot th').each(function () {
                var title = $(this).text();
                if (title != "Edit") {
                    $(this).replaceWith('<td><input type="text" class="form-control" style="width:100%" placeholder="Search ' + title + '" /></td>');
                }
                else {
                    $(this).replaceWith('<td></td>');
                }
            });
            table = $("#ctl00_ContentPlaceHolder1_grd_Projects").dataTable({
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
</asp:Content>
