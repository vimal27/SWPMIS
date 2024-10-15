<%@ Page Title="Tasks" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Milestones.aspx.cs" Inherits="Milestones" Culture="en-GB" EnableEventValidation="false" %>

<%--Ajax Control Toolkit Reference To Support Calendar Extender--%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

	<%-- Inline CSS --%>
    <style type="text/css">
        @media screen and (max-width: 1280px) and (min-width: 1024px)
        {
            .popover-content
            {
                padding: 9px 1px;
            }
            .input-medium
            {
                margin-top: 5px;
                width: 100px !important;
            }
            button.btn.btn-primary.editable-submit
            {
                padding-right: 5%;
                width: 17% !important;
            }
        
            button.btn.editable-cancel
            {
                width: 17% !important;
            }
        
            .firstcolumn .editable-container.popover
            {
                margin-left: 10%;
            }
            .filterable
            {
                float: right;
            }
            .firstcolumn .editable-container.popover
            {
                margin-left: 10%;
            }
            .TaskName .filterable
            {
            }
        
        }
        
        .filterable
        {
            float: right;
        }
        .modal-backdrop.in
        {
            opacity: 0.4 !important;
        }
        .firstcolumn .filterable
        {
            float: right;
        }
        .AllotedTo
        {
            width: 10%;
        }
        .lastcolumn .filterable
        {
            margin-left: 15%;
        }
        .lastcolumn
        {
            width: 10%;
        }
        .removecolumn .filterable
        {
            display: none;
        }
        .popover-title
        {
            background-color: #73849d !important;
        }
        .firstcolumn
        {
            width: 15%;
        }
        .GrdStatusOfTaskView
        {
            width: 10%;
        }
        .label-title
        {
            width: 40% !important;
        }
    </style>
    <%-- external css to Support font-awesome,bootstrap Datatable --%>
    <link rel="stylesheet" type="text/css" href="assets/css/Datatable/dataTables.bootstrap.min.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
        <div class="row">
            <div class="col-md-12">
                <div class="card">
                    
                    <%--Heading Of MainContent--%>
                    <div class="card-header">
                        <div class="card-title">
                            <div class="title" id="TitleOfPage" runat="server">
                                Milestones</div>
                        </div>
                    </div>
                    
                    <%--Main Content,Block For GridView Milestones--%>
                    <div class="card-body" id="block_Grid" runat="server">
                    	    
                        <%--Button With Add New Milestone and Export to excel--%>
                        <asp:Button ID="btn_newMilestone" class="btn btn-success" TabIndex="1" OnClick="btn_newMilestone_Click"
                            Text="Add New Milestone" runat="server" />
                        <asp:Button ID="btn_Export" class="btn btn-info" runat="server" TabIndex="2" CausesValidation="false"
                            Text="Export" OnClick="btn_Export_Click" />
                        
                        <%--GridView For Tasks--%>
                        <div class="form-group">
                            <asp:Panel ID="pnl_Milestones" Style="overflow-x: auto" runat="server">
                                <asp:GridView ID="grd_Milestones" Style="overflow-x: auto !important;" runat="server"
                                    CssClass="datatable table table-striped dataTable" AutoGenerateColumns="False"
                                    DataKeyNames="requestid,id" OnRowCancelingEdit="grd_Milestones_RowCancelingEdit" OnRowUpdating="grd_Milestones_RowUpdating"
                                    OnRowDeleting="grd_Milestones_RowDeleting" OnSelectedIndexChanged="grd_Milestones_SelectedIndexChanged"
                                    OnRowDataBound="grd_Milestones_RowDataBound">
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
                                                <asp:Label ID="lbl_Project" runat="server" Text='<%# Eval("projectreq") %>'></asp:Label>
                                                <asp:HiddenField ID="lbl_RequestNo" runat="server" Value='<%# Eval("requestid") %>'>
                                                </asp:HiddenField>
                                                <asp:HiddenField ID="hdn_projectid" runat="server" Value='<%# Eval("project")%>'>
                                                </asp:HiddenField>
                                                <asp:HiddenField ID="hdn_taskid" runat="server" Value='<%# Eval("id")%>'></asp:HiddenField>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Scope">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Scope" runat="server" Text='<%# Eval("scopename") %>'></asp:Label>
                                                <asp:HiddenField ID="hdn_ScopeID" runat="server" Value='<%# Eval("scope")%>'></asp:HiddenField>
                                                
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Stage">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Stage" runat="server" Text='<%# Eval("stagename") %>'></asp:Label>
                                                <asp:HiddenField ID="hdn_StageID" runat="server" Value='<%# Eval("stage")%>'></asp:HiddenField>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Milestone">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Task" runat="server" Text='<%# Eval("name") %>'></asp:Label>
                                                <asp:HiddenField ID="hdn_MilestoneDescription" runat="server" Value='<%# Eval("description")%>'>
                                                </asp:HiddenField>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField HeaderText="Task Description">
                                        <FooterTemplate>
                                            <asp:TextBox ID="txt_MilestoneDescriptionFilter" CssClass="form-control" Width="100%"
                                                runat="server"></asp:TextBox>
                                        </FooterTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_TaskDescription" runat="server" Text='<%# Eval("taskdescription") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                        
                                        <asp:TemplateField HeaderText="Request Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_RequestDate" runat="server" Text='<%# Eval("requestdate","{0:dd/MM/yyyy}")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Required Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_RequiredDate" runat="server" Text='<%# Eval("requireddate","{0:dd/MM/yyyy}")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Completed Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_CompletedDate" runat="server" Text='<%# ViewState["CompletedDate"]%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Task Status" HeaderStyle-CssClass="lastcolumn">
                                            <ItemTemplate>
                                                <asp:LinkButton Style="width: 100%; cursor: default" ID="lnk_TaskStatus" runat="server"
                                                    Text='<%# Eval("milestonestatus") %>' CommandArgument='<%# bind("id") %>' OnClick="lnk_TaskStatus_Click"></asp:LinkButton>
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
                                                <asp:LinkButton ID="lnk_Delete" OnClientClick="ConfirmDelete(' this task? ')" TabIndex="4"
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
                                <%--Block For Popup While Change Project To Completed State--%>
                                <asp:LinkButton Text="" ID="lnk_HoldPopup" runat="server" />
                                <cc1:ModalPopupExtender ID="popup_HoldState" PopupControlID="pnl_HoldState" TargetControlID="lnk_HoldPopup"
                                    runat="server">
                                </cc1:ModalPopupExtender>
                                <asp:Panel ID="pnl_HoldState" CssClass="panel panel-primary" runat="server" Style="display: none;
                                    width: 30%; height: 30%">
                                    <div class="panel-heading">
                                        Change State?</div>
                                    <div class="panel-body">
                                        <div class="form-group">
                                            <label class="label-title">
                                                Status</label>
                                            <asp:DropDownList ID="ddl_TaskStateChange" AutoPostBack="true" class="form-control"
                                                runat="server" OnSelectedIndexChanged="ddl_TaskStateChange_SelectedIndexChanged">
                                                <asp:ListItem Value="WIP">Active</asp:ListItem>
                                                <asp:ListItem Value="Closed">Closed</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Button ID="btn_WIPProject" Width="100%" OnClick="btn_WIPProject_Click" class="btn btn-info"
                                                ValidationGroup="WIP" runat="server" Text="OK" />
                                            <asp:Button ID="btn_Exit" Width="100%" class="btn btn-danger" runat="server" Text="Cancel" />
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
                                Milestone ID
                            </label>
                            <label id="lbl_TaskIDTextView" style="font-weight: normal" class="label-title" runat="server">
                            </label>
                            </br>
                            <label class="label-title" id="lbl_TasknameView" runat="server" style="font-weight: bold">
                                Name
                            </label>
                            <label id="lbl_TaskNameTextView" style="font-weight: normal" class="label-title"
                                runat="server">
                            </label>
                            </br>
                            <label class="label-title" id="lbl_TaskDescriptionView" runat="server" style="font-weight: bold">
                                Description
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
                            <asp:Button class="btn btn-info" runat="server" ID="btn_BackButton" OnClick="btn_BackButtonClick"
                                Text="Back" />
                        </div>
                    </div>
                    
                    <%--Block For Add Task--%>
                    <div id="block_Register" runat="server">
                        <div class="card-body">
                            <div class="form-group">
                                <label for="lbl_TaskName" runat="server" class="label-title">
                                    Request ID</label>
                                <asp:TextBox ID="txt_RequestNo" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                <asp:HiddenField ID="hdn_RequestNo" runat="server" />
                                <asp:HiddenField ID="hdn_TaskNo" runat="server" />
                            </div>
                            <div class="form-group">
                                <asp:Label ID="Label_Already" runat="server" Visible="false" ForeColor="red"></asp:Label>
                                <label for="lbl_Project" class="label-title">
                                    Project</label>
                                <asp:DropDownList ID="ddl_Project" AutoPostBack="true" class="form-control" runat="server"
                                    OnSelectedIndexChanged="ddl_Project_SelectedIndexChanged">
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                                <span style="color: #ff0000">**</span>
                            </div>
                            <div class="form-group">
                                <label for="lbl_Stage" class="label-title">
                                    Stage</label>
                                <asp:DropDownList ID="ddl_Stage" AutoPostBack="true" class="form-control" runat="server">
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                                <span style="color: #ff0000">**</span>
                                <asp:Button ID="btn_NewStage" Visible="false" runat="server" Text="Add New" CssClass="btn btn-primary"
                                    data-toggle="modal" data-target="#modalAddStage"></asp:Button>
                            </div>
                            <!-- Modal For Add New Stage -->
                            <div class="modal fade" id="modalAddStage" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
                                aria-hidden="true">
                                <div class="modal-dialog">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                                <span aria-hidden="true">&times;</span></button>
                                            <h4 class="modal-title" id="lbl_scope">
                                                Stage</h4>
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
                           
                            <div class="form-group">
                                <label for="lbl_Scope" class="label-title">
                                    Scope</label>
                                <asp:DropDownList ID="ddl_Scope" AutoPostBack="true" class="form-control" runat="server"
                                    OnSelectedIndexChanged="ddl_Scope_SelectedIndexChanged">
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                                <span style="color: #ff0000">**</span>
                                <asp:Button ID="btn_NewScope" Visible="false" runat="server" Text="Add New" CssClass="btn btn-primary"
                                    data-toggle="modal" data-target="#modalAddScope"></asp:Button>
                            </div>
                             <div class="form-group">
                                <label for="lbl_TaskName" class="label-title">
                                    Milestone Name</label>
                                <asp:TextBox ID="txt_MilestoneName" MaxLength="50" CausesValidation="true" class="form-control" runat="server"></asp:TextBox>
                                <span style="color: #ff0000">**</span>
                            </div>
                            <div class="form-group">
                                <label for="lbl_TaskDescription" class="label-title">
                                    Description</label>
                                <asp:TextBox ID="txt_MilestoneDescription" TextMode="MultiLine" CausesValidation="true"
                                    class="form-control" runat="server"></asp:TextBox>
                            </div>
                            
                            <!-- Modal For Add New Scope -->
                            <div class="modal fade" id="modalAddScope" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
                                aria-hidden="true">
                                <div class="modal-dialog">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                                <span aria-hidden="true">&times;</span></button>
                                            <h4 class="modal-title" id="H1">
                                                Scope</h4>
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
                            
                            <div class="form-group">
                                <label class="label-title">
                                    Request Date</label>
                                <asp:TextBox ID="txt_RequestDate" class="form-control" runat="server" AutoPostBack="true"
                                    OnTextChanged="txt_RequestDate_TextChanged"></asp:TextBox>
                                <span style="color: #ff0000">**</span>
                                <cc1:CalendarExtender ID="calext_RequestDate" PopupPosition="TopLeft" runat="server"
                                    TargetControlID="txt_RequestDate" CssClass="custom-calendar" Format="dd/MM/yyyy">
                                </cc1:CalendarExtender>
                            </div>
                            <div class="form-group">
                                <label class="label-title">
                                    Required Date</label>
                                <asp:TextBox ID="txt_RequiredDate" class="form-control" runat="server"></asp:TextBox>
                                <span style="color: #ff0000">**</span>
                                <cc1:CalendarExtender ID="calext_RequiredDate" CssClass="custom-calendar" PopupPosition="TopLeft"
                                    runat="server" TargetControlID="txt_RequiredDate" Format="dd/MM/yyyy">
                                </cc1:CalendarExtender>
                            </div>
                            <asp:Button ID="btn_Allot" class="btn btn-success" Style="margin-left: 25%" CausesValidation="true"
                                runat="server" Text="Add" OnClick="btn_Allot_Click">
                            </asp:Button>
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
                    
                    <%--External JS Files To Support Jquery,bootstrap,Datatable--%>
                    <script type="text/javascript" charset="utf8" src="assets/js/Datatable/jquery.dataTables.min.js"></script>
    				<script type="text/javascript" src="assets/js/Datatable/dataTables.bootstrap.min.js"></script>
    				
    				<%--Validations Required Field--%>
                    <script language="javascript" type="text/javascript">
                        function validate() {
                            if (document.getElementById("<%=txt_MilestoneName.ClientID%>").value == "") {
                                swal("Task Name Field can not be blank");
                                document.getElementById("<%=txt_MilestoneName.ClientID%>").focus();
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
                            var startDate = new Date.parse(document.getElementById('<%= txt_RequestDate.ClientID %>').value);
                            alert(startDate);
                            var EndDate = new Date.parse(document.getElementById('<%= txt_RequestDate.ClientID %>').value);
                            alert(EndDate);
                            if (startDate > EndDate) {
                                swal("End Date Field Less than be Requested Date");
                                return false;
                            }

                            return true;
                        }   
                    </script>
                                       
                    <script type="text/javascript">
		   				$(function () {
			    			$("#ctl00_ContentPlaceHolder1_grd_Milestones").prepend( $("<thead></thead>").append( $("#ctl00_ContentPlaceHolder1_grd_Milestones").find("tr:first"))).dataTable();
						});
				    </script>
</asp:Content>
