<%@ Page Title="Coding Standard Rating" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="CodingStandardRating.aspx.cs" Inherits="CodingStandardRating" %>

<%-- Ajax Control Toolkit Extender To Suppport Calendar Extendar --%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%-- External CSS To Support Datatables --%>
    <link rel="stylesheet" type="text/css" href="assets/css/Datatable/dataTables.bootstrap.min.css" />
    <style>
		<%-- Inline CSS --%>
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
    <div class="card">
        <%--Heading Of MainContent--%>
        <div class="card-header">
            <div class="card-title">
                <div class="title" id="TitleOfPage" runat="server">
                    Coding Standard / Quality Rating (Applicable Since 16th April 2016)</div>
            </div>
        </div>
        <%-- Main Content --%>
        <div class="card-body" style="min-height: 500px !important">
            <%-- Form Elements --%>
            <div id="block_History" runat="server">
                <div class="form-group">
                    <div class="left-inner-addon">
                        <label id="Label1" for="lbl_EmployeeID" runat="server" class="label-title">
                            Employee</label>
                        <asp:Label ID="lbl_CurrentEmployeeID" runat="server"></asp:Label>
                        <i class="fa fa-user" runat="server" id="icon_EmployeeDdl" visible="false"></i>
                        <asp:DropDownList ID="ddl_EmployeeID" Visible="false" CausesValidation="true" class="form-control"
                            runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_EmployeeID_SelectedIndexChanged">
                            <asp:ListItem></asp:ListItem>
                        </asp:DropDownList>
                        <asp:HiddenField ID="hdn_UserID" runat="server" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="left-inner-addon">
                        <label for="lbl_FromDate" class="label-title">
                            From Date</label>
                        <i class="fa fa-calendar"></i>
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
                        <i class="fa fa-calendar"></i>
                        <asp:TextBox ID="txt_ToDate" AutoPostBack="true" CausesValidation="true" class="form-control"
                            runat="server" OnTextChanged="txt_ToDate_TextChanged"></asp:TextBox>
                        <cc1:CalendarExtender ID="calext_ToDate" CssClass="custom-calendar" PopupPosition="TopLeft"
                            runat="server" TargetControlID="txt_ToDate" Format="dd/MM/yyyy">
                        </cc1:CalendarExtender>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Button ID="btn_AddRating" class="btn btn-primary" CausesValidation="true" runat="server"
                        Text="Add Rating" OnClick="btn_AddRating_Click" TabIndex="9"></asp:Button>
                    <asp:Button ID="btn_Export" class="btn btn-info" CausesValidation="true" runat="server"
                        Text="Export" TabIndex="10" OnClick="btn_Export_Click"></asp:Button>
                    <asp:Button ID="btn_Report" class="btn btn-success" CausesValidation="true" runat="server"
                        Text="Report" TabIndex="11" OnClick="btn_Report_Click"></asp:Button>
                    <asp:Button ID="btn_Cancel" class="btn btn-warning" runat="server" Text="Cancel"
                        CausesValidation="false" OnClick="btn_Cancel_Click"></asp:Button>
                </div>
                <%--GridView Of Report--%>
                <div id="block_Grid" runat="server" visible="true" style="overflow-x: auto">
                    <div class="form-group">
                        <asp:GridView ID="grd_CodingRating" runat="server" CssClass="dataTables_wrapper dt-foundation display datatable"
                            Style="overflow: scroll" AutoGenerateColumns="False" DataKeyNames="id">
                            <Columns>
                                <asp:TemplateField HeaderText="Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Date" runat="server" Text='<%# Eval("date") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Project">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Project" runat="server" Text='<%# Eval("Project") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Task">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Task" runat="server" Text='<%# Eval("Task") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Task By">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Employee" runat="server" Text='<%# Eval("username") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Grade">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Grade" runat="server" Text='<%# Eval("gradeSelected") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Grade Given By">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_GradeBy" runat="server" Text='<%# Eval("gradeby") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Rating For">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_RatingFor" runat="server" Text='<%# Eval("RatingType") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerStyle CssClass="PaginationClass" />
                        </asp:GridView>
                    </div>
                </div>
            </div>
            <%--Block For Coding Standard Ratings--%>
            <div id="block_NewRating" runat="server" visible="false">
                <div class="form-group">
                    <asp:Panel ID="pnl_RadioButtonList" runat="server">
                        <div class="alert alert-info" role="alert">
                            <strong>Note!</strong> This rating is editable on only two days.
                        </div>
                        <div class="left-inner-addon" style="float: left">
                            <i class="fa fa-calendar"></i>
                            <asp:TextBox ID="txt_DateOfRating" Style="width: 80%" CausesValidation="true" class="form-control"
                                runat="server"></asp:TextBox>
                            <cc1:CalendarExtender ID="calext_DateOfRating" CssClass="custom-calendar" PopupPosition="TopLeft"
                                runat="server" TargetControlID="txt_DateOfRating" Format="dd/MM/yyyy">
                            </cc1:CalendarExtender>
                        </div>
                        <asp:DropDownList ID="rbl_RatingType" Style="float: right" CssClass="form-control"
                            runat="server">
                            <asp:ListItem Value="Quality Rating">Quality Rating</asp:ListItem>
                            <asp:ListItem Value="Coding Standard Rating">Coding Standard Rating</asp:ListItem>
                        </asp:DropDownList>
                        <span style="float: right; margin-right: 2%;margin-top:1%; font-weight: bold">Rating For :
                        </span></br></br></br></br></br></br>
                    </asp:Panel>
                    <asp:Button ID="btn_UpdateRating" class="btn btn-primary" runat="server" Text="Update Rating"
                        OnClick="btn_UpdateRating_Click"></asp:Button>
                    <asp:Button ID="btn_Back" class="btn btn-warning" runat="server" Text="Back" CausesValidation="false"
                        OnClick="btn_BackToHistory_Click"></asp:Button>
                    <div class="panel panel-default" style="margin-top: 2%">
                        <!-- Default panel contents -->
                        <div class="panel-heading">
                            Note</div>
                        <div class="panel-body">
                            <p>
                                <b>A</b> - Absent
                            </p>
                            <p>
                                <b>0 to 5 </b>- Coding Standard Rating Grade</p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%--External Js To Bootstrap,Jquery,Datatables--%>
    <script type="text/javascript" charset="utf8" src="assets/js/Datatable/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="assets/js/Datatable/dataTables.bootstrap.min.js"></script>
    <script type="text/javascript">
        $(function () {
            // Setup - add a text input to each footer cell
            $("#ctl00_ContentPlaceHolder1_grd_CodingRating").prepend($("<thead></thead><tfoot></tfoot>").append($("#ctl00_ContentPlaceHolder1_grd_CodingRating").find("tr:first")));

            $('#ctl00_ContentPlaceHolder1_grd_CodingRating tfoot th').each(function () {
                var title = $(this).text();
                if (title != "Edit") {
                    $(this).replaceWith('<td><input type="text" class="form-control" style="width:100%" placeholder="Search ' + title + '" /></td>');
                }
                else {
                    $(this).replaceWith('<td></td>');
                }
            });
            var table = $("#ctl00_ContentPlaceHolder1_grd_CodingRating").dataTable({
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
        $(document).ready(function () {
            $("#ctl00_ContentPlaceHolder1_btn_UpdateRating").attr('value', 'Update Coding Standard Rating');
            //Dropdownlist Selectedindexchanged event
            $('#ctl00_ContentPlaceHolder1_rbl_RatingType').change(function () {
                // Get Dropdownlist seleted item text
                $("#ctl00_ContentPlaceHolder1_btn_UpdateRating").attr('value', 'Update ' + $("#ctl00_ContentPlaceHolder1_rbl_RatingType option:selected").text() + '');
                return false;
            })
        });
    </script>
</asp:Content>
