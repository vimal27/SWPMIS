<%@ Page Title="Non-Production Entry" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Non_ProductionReport.aspx.cs" Inherits="Non_ProductionReport" %>

<%-- Ajax control toolkit extender to support calendar extendar --%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	
	<%-- External CSS To Bootstrap Datatable --%>
    <link rel="stylesheet" type="text/css" href="assets/css/Datatable/dataTables.bootstrap.min.css" />
    
    <%-- Inline CSS --%>
    <style>
        .datatable tbody {
            text-align: center !important;
        }

        .datatable thead th {
            text-align: center !important;
        }

        .modal-backdrop.in {
            opacity: 0.4 !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
        <div class="card">
            
            <%--Heading Of MainContent--%>
            <div class="card-header">
                <div class="card-title">
                    <div class="title" id="TitleOfPage" runat="server">
                        Non-Production Report
                    </div>
                </div>
            </div>
            
            <%-- Main Content --%>
            <div class="card-body" style="min-height:700px !important">
            	
                <%-- Form Elements --%>
                <div class="form-group">
                <div class="left-inner-addon">
                    <label id="Label1" for="lbl_EmployeeID" runat="server" class="label-title">
                        Employee</label>
                    <asp:Label ID="lbl_CurrentEmployeeID" runat="server"></asp:Label>
                    <i class="fa fa-user" runat="server" id="icon_EmployeeDdl" Visible="false"></i>
                    <asp:DropDownList ID="ddl_EmployeeID" Visible="false" CausesValidation="true" class="form-control"
                        runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_EmployeeID_SelectedIndexChanged">
                        <asp:ListItem></asp:ListItem>
                    </asp:DropDownList>
                    <asp:HiddenField ID="hdn_UserID" runat="server" />
                    </div>
                </div>
                <div class="form-group">
                <div class="left-inner-addon">
                    <label id="lbl_Scope" runat="server" class="label-title">
                        Scope</label>
                        <i class="fa fa-language"></i>
                    <asp:DropDownList ID="ddl_Scope" CausesValidation="true" class="form-control" runat="server"
                        AutoPostBack="true" OnSelectedIndexChanged="ddl_Scope_SelectedIndexChanged">
                        <asp:ListItem></asp:ListItem>
                    </asp:DropDownList>
                    <asp:HiddenField ID="hdn_Scope" runat="server" />
                    </div>
                </div>
                <div class="form-group">
                <div class="left-inner-addon">
                    <label id="lbl_Stage" runat="server" class="label-title">
                        Stage</label>
                        <i class="glyphicon glyphicon-stats"></i>
                    <asp:DropDownList ID="ddl_Stage" CausesValidation="true" class="form-control" runat="server"
                        AutoPostBack="true" OnSelectedIndexChanged="ddl_Stage_SelectedIndexChanged">
                        <asp:ListItem></asp:ListItem>
                    </asp:DropDownList>
                    <asp:HiddenField ID="hdn_Stage" runat="server" />
                    </div>
                </div>
                <div class="form-group">
                <div class="left-inner-addon">
                    <label for="lbl_FromDate" class="label-title">
                        From Date</label>
                        <i class="glyphicon glyphicon-calendar"></i>
                    <asp:TextBox ID="txt_FromDate" AutoPostBack="true" CausesValidation="true" class="form-control"
                        runat="server" OnTextChanged="txt_FromDate_TextChanged"></asp:TextBox>
                    <cc1:CalendarExtender ID="calext_FromDate" CssClass="custom-calendar" PopupPosition="TopLeft"
                        runat="server" TargetControlID="txt_FromDate" Format="dd/MM/yyyy">
                    </cc1:CalendarExtender>
                    </div>
                </div>
                <div class="form-group">
                <div class="left-inner-addon">
                    <label for="lbl_ToDate" class="label-title">
                        To Date</label>
                        <i class="glyphicon glyphicon-calendar"></i>
                    <asp:TextBox ID="txt_ToDate" AutoPostBack="true" CausesValidation="true" class="form-control"
                        runat="server" OnTextChanged="txt_ToDate_TextChanged"></asp:TextBox>
                    <cc1:CalendarExtender ID="calext_ToDate" CssClass="custom-calendar" PopupPosition="TopLeft"
                        runat="server" TargetControlID="txt_ToDate" Format="dd/MM/yyyy">
                    </cc1:CalendarExtender>
                    </div>
                </div>
                <div class="form-group">
					<asp:Button ID="btn_Report" class="btn btn-success" CausesValidation="true" runat="server"
                        Text="Report" TabIndex="9" OnClick="btn_Report_Click"></asp:Button>
                    <asp:Button ID="btn_Export" class="btn btn-info" CausesValidation="true" runat="server"
                        Text="Export" TabIndex="10" OnClick="btn_Export_Click"></asp:Button>
                    <asp:Button ID="btn_Cancel" class="btn btn-warning" runat="server" Text="Cancel"
                        CausesValidation="false" OnClick="btn_Cancel_Click"></asp:Button>
                </div>
                
                <%--GridView Of Report--%>
                <div id="block_Grid" runat="server" visible="false" style="overflow-x: auto">
                    <div class="form-group">
                        <asp:GridView ID="grd_DPR" runat="server" CssClass="dataTables_wrapper dt-foundation display datatable"
                            Style="overflow: scroll" AutoGenerateColumns="False" DataKeyNames="slno">
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
                                        <asp:Label ID="lbl_Date" runat="server" Text='<%# Eval("CurrentDate", "{0:dd/MM/yyyy}") %>'></asp:Label>
                                        <asp:HiddenField ID="hdn_Date" runat="server" Value='<%# Eval("Date")%>'></asp:HiddenField>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Project">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Project" runat="server" Text='<%# Eval("ProjectID") %>'></asp:Label>
                                        <asp:HiddenField ID="hdn_ProjectID" runat="server" Value='<%# Eval("Project")%>'></asp:HiddenField>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Scope">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Scope" runat="server" Text='<%# Eval("Scope") %>'></asp:Label>
                                        <asp:HiddenField ID="hdn_ScopeID" runat="server" Value='<%# Eval("ScopeID")%>'></asp:HiddenField>
                                        <asp:HiddenField ID="hdn_TaskID" runat="server" Value='<%# Eval("TaskID")%>'></asp:HiddenField>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Stage">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Stage" runat="server" Text='<%# Eval("Stage") %>'></asp:Label>
                                        <asp:HiddenField ID="hdn_StageID" runat="server" Value='<%# Eval("StageID")%>'></asp:HiddenField>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Task">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Task" runat="server" Text='<%# Eval("Task") %>'></asp:Label>
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
                            </Columns>
                            <PagerStyle CssClass="PaginationClass" />
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
    
    <%-- External Js To Support Jquery,Bootstrap,Datatable --%>
    <script type="text/javascript" charset="utf8" src="assets/js/Datatable/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="assets/js/Datatable/dataTables.bootstrap.min.js"></script>
    
    <%--Datatable Initialization--%>
    <script type="text/javascript">
        $(function () {
            // Setup - add a text input to each footer cell
            $("#ctl00_ContentPlaceHolder1_grd_DPR").prepend($("<thead></thead><tfoot></tfoot>").append($("#ctl00_ContentPlaceHolder1_grd_DPR").find("tr:first")));

            $('#ctl00_ContentPlaceHolder1_grd_DPR tfoot th').each(function () {
                var title = $(this).text();
                if (title != "Edit") {
                    $(this).replaceWith('<td><input type="text" class="form-control" style="width:100%" placeholder="Search ' + title + '" /></td>');
                }
                else {
                    $(this).replaceWith('<td></td>');
                }
            });
            var table = $("#ctl00_ContentPlaceHolder1_grd_DPR").dataTable({
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
</asp:Content>
