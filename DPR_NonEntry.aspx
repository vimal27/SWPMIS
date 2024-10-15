<%@ Page Title="DPR Non-Entry" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="DPR_NonEntry.aspx.cs" Inherits="DPR_NonEntry" %>

<%--Ajax Control Toolkit For Calendar Extender--%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <%--External CSS To Support Bootstrap Datatable--%>
    <link rel="stylesheet" type="text/css" href="assets/css/Datatable/dataTables.bootstrap.min.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="card" style="min-height: 450px;">
        <%--Heading Of MainContent--%>
        <div class="card-header">
            <div class="card-title">
                <div class="title" id="TitleOfPage" runat="server">
                    DPR Non-Entry
                </div>
            </div>
        </div>
        
        <%--Main Content--%>
        <div class="card-body">
            <div class="form-group">
                <asp:HiddenField ID="hdn_UserID" runat="server" />
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
                    Text="Report" OnClick="btn_Report_Click"></asp:Button>
                <asp:Button ID="btn_Export" class="btn btn-primary" CausesValidation="true" runat="server"
                    Text="Export" OnClick="btn_Export_Click"></asp:Button>
                <asp:Button ID="btn_Cancel" class="btn btn-warning" runat="server" Text="Cancel"
                    CausesValidation="false" OnClick="btn_Cancel_Click"></asp:Button>
            </div>

            <%--GridView Of Report--%>
            <div id="block_Grid" visible="false" runat="server" style="overflow-x: auto">
                <div class="form-group">
                    <asp:GridView ID="grd_DPR" runat="server" CssClass="datatable table table-striped dataTable"
                        Style="overflow: scroll" AutoGenerateColumns="False">
                        <Columns>
                            <asp:TemplateField HeaderText="Employee ID">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_EmployeeID" runat="server" Text='<%# Eval("userid") %>'></asp:Label>
                                    <asp:HiddenField ID="hdn_EmployeeID" runat="server" Value='<%# Eval("userid")%>'></asp:HiddenField>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Employee Name">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_EmployeeName" runat="server" Text='<%# Eval("username") %>'></asp:Label>
                                    <asp:HiddenField ID="hdn_EmployeeName" runat="server" Value='<%# Eval("username")%>'></asp:HiddenField>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Date" runat="server" Text='<%# Eval("CurrentDate", "{0:dd/MM/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerStyle CssClass="PaginationClass" />
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>

    <%--External JS Files to support Jquery,Bootstrap and bootstrap Datatable--%>
    <script type="text/javascript" charset="utf8" src="assets/js/Datatable/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="assets/js/Datatable/dataTables.bootstrap.min.js"></script>
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
