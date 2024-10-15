<%@ Page Title="Teams" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Teams.aspx.cs" Inherits="Teams" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%--Inline Style--%>
    <style type="text/css">
        .datatable tbody
        {
            text-align: center !important;
        }
        .datatable thead th
        {
            text-align: center !important;
        }
        .modal-backdrop.in
        {
            opacity: 0.4 !important;
        }
    </style>
    <%--External CSS To Support Font Awesome and Bootstrap Datatale--%>
    <link rel="stylesheet" type="text/css" href="assets/css/Datatable/dataTables.bootstrap.min.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="card">
        <%--Heading Of MainContent--%>
        <div class="card-header">
            <div class="card-title">
                <div class="title" id="TitleOfPage" runat="server">
                    Teams</div>
            </div>
        </div>
        <%--Main Content,Block For Add Team--%>
        <div id="block_Register" runat="server">
            <div class="card-body">
                <%--Form Fields--%>
                <asp:Label class="alert alert-warning" role="alert" ID="lbl_Error" runat="server"
                    Text="" Visible="false"></asp:Label>
                <asp:HiddenField ID="hdn_TeamID" runat="server" />
            </div>
            <div class="form-group">
            <div class="left-inner-addon">
                <label class="label-title">
                    Team ID</label>
                    <i class="fa fa-database"></i>
                <input type="text" maxlength="6" runat="server" class="form-control" id="txt_TeamID"
                    onfocus="nextfield ='Button1';" tabindex="1" /><span style="color: #ff0000"> **</span>
                    </div>
            </div>
            <div class="form-group">
            <div class="left-inner-addon">
                <label class="label-title">
                    Team Name</label>
                    <i class="fa fa-user"></i>
                <input id="txt_TeamName" maxlength="50" runat="server" class="form-control" onfocus="nextfield ='Projectid';"
                    onkeypress="return alpha()" tabindex="1" type="text" />
                <span style="color: #ff0000">**</span>
                </div>
                <asp:HiddenField ID="hdn_ID" runat="server" />
            </div>
            <div class="form-group">
            <div class="left-inner-addon">
                <label class="label-title" style="float: left">
                    Status</label>
                <asp:RadioButtonList ID="rbl_Status" RepeatDirection="Horizontal" runat="server">
                    <asp:ListItem Value="1" Selected="True">Active</asp:ListItem>
                    <asp:ListItem Value="0">Inactive</asp:ListItem>
                </asp:RadioButtonList>
                </div>
            </div>
            <asp:Button ID="btn_Save" Style="margin-left: 25%" class="btn btn-success" runat="server"
                Text="Save" TabIndex="9" ValidationGroup="project" OnClientClick="return validate();"
                OnClick="btn_Save_Click"></asp:Button>
            <asp:Button ID="btn_Reset" class="btn btn-warning" runat="server" Text="Reset" CausesValidation="false"
                TabIndex="10" OnClick="btn_Reset_Click"></asp:Button>
            <asp:Button ID="Btn_Back" class="btn btn-info" runat="server" Text="Back" CausesValidation="false"
                TabIndex="11" OnClick="Btn_Back_Click"></asp:Button>
            <div>
                <div align="left" style="margin-left: 5%">
                    <span style="color: #ff0000">**</span><font color="blue"> Required field</font>
                </div>
            </div>
        </div>
        <%--Block For View Of Teams--%>
        <div id="block_View" class="card-body" runat="server" visible="false">
            <div class="form-group">
                <label class="label-title" id="lbl_TeamIDView" runat="server" style="font-weight: bold">
                    Team ID
                </label>
                <label id="lbl_TeamIDTextView" style="font-weight: normal" class="label-title" runat="server">
                </label>
                </br>
                <label class="label-title" id="lbl_TeamNameView" runat="server" style="font-weight: bold">
                    Team Name
                </label>
                <label id="lbl_TeamNameTextView" style="font-weight: normal" class="label-title"
                    runat="server">
                </label>
                </br>
                <label class="label-title" id="lbl_TeamLeaderView" runat="server" style="font-weight: bold">
                    Team Leader
                </label>
                <label id="lbl_TeamLeaderTextView" style="font-weight: normal" class="label-title"
                    runat="server">
                </label>
                </br>
                <label class="label-title" id="lbl_TeamMembersView" runat="server" style="font-weight: bold">
                    Team Members
                </label>
                <label id="lbl_TeamMembersTextView" style="font-weight: normal" class="label-title"
                    runat="server">
                </label>
                </br>
                <label class="label-title" id="lbl_StatusView" runat="server" style="font-weight: bold">
                    Status
                </label>
                <label id="lbl_StatusTextView" style="font-weight: normal" runat="server" class="label-title">
                </label>
                </br>
                <asp:Button class="btn btn-info" runat="server" ID="btn_BackButton" OnClick="btn_BackButtonClick"
                    Text="Back" />
            </div>
        </div>
        <%--Block For GridView Of Teams--%>
        <div class="card-body" id="block_Grid" runat="server">
            <asp:Button ID="btn_NewTeam" class="btn btn-success" TabIndex="1" Text="Add New Team"
                runat="server" OnClick="btn_NewTeams_Click" />
            <asp:Button ID="btn_Export" class="btn btn-info" runat="server" TabIndex="2" CausesValidation="false"
                Text="Export" OnClick="btn_Export_Click" />
            <%--Search--%>
            <div class="form-group">
                <asp:GridView ID="grd_Teams" runat="server" CssClass="dataTables_wrapper dt-foundation display datatable"
                    AutoGenerateColumns="False" DataKeyNames="ID" OnRowCancelingEdit="grd_Teams_RowCancelingEdit"
                    OnRowUpdating="grd_Teams_RowUpdating" OnRowDeleting="grd_Teams_RowDeleting" OnSelectedIndexChanged="grd_Teams_SelectedIndexChanged"
                    AllowPaging="true" OnPageIndexChanging="OnPageIndexChanging" PageSize="5">
                    <Columns>
                        <asp:TemplateField HeaderText="Team ID">
                            <ItemTemplate>
                                <asp:Label ID="lbl_TeamID" runat="server" Text='<%# Eval("TeamID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Team Name">
                            <ItemTemplate>
                                <asp:Label ID="lbl_TeamName" runat="server" Text='<%# Eval("Teamname") %>'></asp:Label>
                                <asp:HiddenField ID="hdn_ID" runat="server" Value='<%# Eval("ID")%>'></asp:HiddenField>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Status">
                            <ItemTemplate>
                                <%# Convert.ToString(Eval("status1")).ToLower() == "active" ? "<span class=\"btn btn-success\" style=\"width:100%;cursor:default\">Active</span>" : "<span class=\"btn btn-warning\" style=\"width:100%;cursor:default\">Inactive</span>"%>
                                <asp:HiddenField ID="hdn_Status" runat="server" Value='<%# Eval("status")%>'></asp:HiddenField>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Edit" HeaderStyle-CssClass="removecolumn">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnk_Edit" TabIndex="3" runat="server" CommandName="Select" ToolTip="Select"
                                    Text='<i class="fa fa-pencil-square-o"></i>' ForeColor="#5AC15A" Font-Size="23px">
                                </asp:LinkButton>
                                <asp:LinkButton ID="lnk_View" runat="server" OnClick="SelectCurrentData" CommandArgument='<%# bind("teamid") %>'
                                    ToolTip="View Teams" Text='<i class="glyphicon glyphicon-eye-open"></i>' ForeColor="#19B5FE"
                                    Font-Size="Large">                    
                                </asp:LinkButton>
                                <asp:LinkButton ID="lnk_Delete" OnClientClick="ConfirmDelete(' this team? ')" TabIndex="4"
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
        </div>
    </div>
    <%--Javascript Validations Required Field--%>
    <script language="javascript" type="text/javascript">

        function validate() {
            if (document.getElementById("<%=txt_TeamID.ClientID%>").value == "") {
                swal("Team ID Field can not be blank");
                document.getElementById("<%=txt_TeamID.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txt_TeamName.ClientID%>").value == "") {
                swal("Team Name Field can not be blank");
                document.getElementById("<%=txt_TeamName.ClientID%>").focus();
                return false;
            }

            return true;
        }
    </script>
    <%--External JS To Support Jquery,Boostrap,datatables--%>
    <script type="text/javascript" charset="utf8" src="assets/js/Datatable/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="assets/js/Datatable/dataTables.bootstrap.min.js"></script>
    <script type="text/javascript">
        $(function () {

            // Setup - add a text input to each footer cell
            $("#ctl00_ContentPlaceHolder1_grd_Teams").prepend($("<thead></thead><tfoot></tfoot>").append($("#ctl00_ContentPlaceHolder1_grd_Teams").find("tr:first")));

            $('#ctl00_ContentPlaceHolder1_grd_Teams tfoot th').each(function () {
                var title = $(this).text();
                if (title != "Edit") {
                    $(this).replaceWith('<td><input type="text" class="form-control" style="width:100%" placeholder="Search ' + title + '" /></td>');
                }
                else {
                    $(this).replaceWith('<td></td>');
                }
            });
            var table = $("#ctl00_ContentPlaceHolder1_grd_Teams").dataTable({
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
