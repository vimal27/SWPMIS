<%@ Page Title="Users" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Users.aspx.cs" Inherits="Users" %>

<%@ Register TagPrefix="asp" Namespace="Saplin.Controls" Assembly="DropDownCheckBoxes" %>
<%@ Register Assembly="DropDownCheckBoxes" Namespace="Saplin.Controls" TagPrefix="cc2" %>
<%--Ajax control Toolkit To Support Ajax Calendar Extendar--%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%--Inline CSS--%>
    <style type="text/css">
        tbody
	    {
	    	text-align:center !important;
	    }
		th
		{
			text-align:center !important;
		}
        .modal-backdrop.in
        {
            opacity: 0.4 !important;
        } 
    </style>

    <%--External CSS To Support Font Awesome and Bootstrap datatable--%>
    <link rel="stylesheet" type="text/css" href="assets/css/Datatable/dataTables.bootstrap.min.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="card">
        <%--Heading Of MainContent--%>
        <div class="card-header">
            <div class="card-title">
                <div class="title" id="TitleOfPage" runat="server">
                    Users</div>
            </div>
        </div>
        <%--Main Content,Block For User ADD--%>
        <div id="block_Register" runat="server">
            <div class="card-body">
                <%--Form Elements--%>
                <asp:Label class="alert alert-warning" role="alert" ID="lbl_Error" runat="server"
                    Text="" Visible="false"></asp:Label>
                <asp:HiddenField ID="hdn_UserID" runat="server" />
            </div>
            <div class="form-group">               
                    <div class="left-inner-addon">
                    	<label class="label-title">
                    		Employee ID</label>
        				<i class="fa fa-database"></i>
                		<input type="text" maxlength="6" runat="server" class="form-control" id="txt_UserID" tabindex="1" />
                		<span style="color: #ff0000"> **</span>
                    </div>
            </div>
            <div class="form-group">
            <div class="left-inner-addon">
                <label class="label-title">
                    Employee Name</label>
                    <i class="icon-user"></i>
                <input id="txt_UserName" maxlength="50" runat="server" class="form-control" onkeypress="return alpha()"
                    tabindex="2" type="text" />
                <span style="color: #ff0000">**</span>
                </div>
                <asp:HiddenField ID="hdn_Sno" runat="server" />
            </div>
            <div class="form-group">
            <div class="left-inner-addon">
                <label class="label-title">
                    Join Date</label>
                    <i class="icon-calendar"></i>
                <asp:TextBox ID="txt_JoinDate" runat="server" class="form-control" TabIndex="3"></asp:TextBox>
                <span style="color: #ff0000">**</span>
                <cc1:CalendarExtender ID="calext_JoinDate" CssClass="custom-calendar" PopupPosition="TopLeft"
                    runat="server" TargetControlID="txt_JoinDate" Format="dd/MM/yyyy">
                </cc1:CalendarExtender>
                </div>
            </div>
            <div class="form-group">
            <div class="left-inner-addon">
                <label class="label-title">
                    User Role</label>
                    <i class="icon-th"></i>
                <asp:DropDownList TabIndex="4" ID="ddl_UserRole" class="form-control" runat="server">
                    <asp:ListItem Value="User" Selected="True">User</asp:ListItem>
                    <asp:ListItem Value="Administrator">Administrator</asp:ListItem>
                    <asp:ListItem Value="Team Leader">Team Leader</asp:ListItem>
                </asp:DropDownList>
                </div>
            </div>
            <div class="form-group">
            <div class="left-inner-addon">
                <label class="label-title" id="lbl_Password" runat="server">
                    Password</label>
                    <i class="icon-key"></i>
                <input id="txt_Password" runat="server" type="password" class="form-control" tabindex="5"
                    validationgroup="project" />
                <%--<asp:TextBox ID="txt_Password" runat="server" class="form-control" TabIndex="3" ValidationGroup="project"
                                TextMode="Password"></asp:TextBox>--%>
                <span style="color: #ff0000" id="req_Password" runat="server">**</span>
                </div	>
            </div>
            <div class="form-group">
                <label class="label-title" style="float: left">
                    Status</label>
                <asp:RadioButtonList ID="rbl_Status" TabIndex="6" RepeatDirection="Horizontal" runat="server">
                    <asp:ListItem Value="1" Selected="True">Active</asp:ListItem>
                    <asp:ListItem Value="0">Inactive</asp:ListItem>
                </asp:RadioButtonList>
            </div>
            <asp:Button ID="btn_Save" Style="margin-left: 25%" class="btn btn-success" runat="server"
                TabIndex="7" Text="Save" ValidationGroup="project" OnClientClick="return validate();"
                OnClick="btn_Save_Click"></asp:Button>
            <asp:Button ID="btn_Reset" class="btn btn-warning" runat="server" Text="Reset" CausesValidation="false"
                TabIndex="8" OnClick="btn_Reset_Click"></asp:Button>
            <asp:Button ID="btn_Back" class="btn btn-info" runat="server" Text="Back" CausesValidation="false"
                TabIndex="9" OnClick="btn_Back_Click"></asp:Button>
            <div>
                <div align="left" style="margin-left: 5%">
                    <span style="color: #ff0000">**</span><font color="blue"> Required field</font>
                </div>
            </div>
        </div>
        
        <%--Block For View For Users--%>
        <div id="block_View" class="card-body" runat="server" visible="false">
            <div class="form-group">
                <label class="label-title" id="lbl_UserIDView" runat="server" style="font-weight: bold">
                    User ID
                </label>
                <label id="lbl_UserIDTextView" style="font-weight: normal" class="label-title" runat="server">
                </label>
                </br>
                <label class="label-title" id="lbl_UserNameView" runat="server" style="font-weight: bold">
                    User Name
                </label>
                <label id="lbl_UserNameTextView" style="font-weight: normal" class="label-title"
                    runat="server">
                </label>
                </br>
                <label class="label-title" id="lbl_RoleView" runat="server" style="font-weight: bold">
                    Role
                </label>
                <label id="lbl_RoleTextView" style="font-weight: normal" runat="server" class="label-title">
                </label>
                </br>
                <label class="label-title" id="lbl_StatusView" runat="server" style="font-weight: bold">
                    Status
                </label>
                <label id="lbl_StatusTextView" style="font-weight: normal" class="label-title" runat="server">
                </label>
                </br>
                <asp:Button class="btn btn-info" runat="server" ID="btn_BackButton" OnClick="btn_BackButtonClick"
                    Text="Back" />
            </div>
        </div>
        <%--Block For Users GridView--%>
        <div class="card-body" id="block_Grid" runat="server">
            <div class="buttons"> 
            <asp:Button ID="btn_NewUser" class="btn btn-success" TabIndex="1" Text="Add New User"
                runat="server" OnClick="btn_NewUsers_Click" />
            <asp:Button ID="btn_Export" class="btn btn-info" runat="server" TabIndex="2" CausesValidation="false"
                Text="Export" OnClick="btn_Export_Click" />
           </div>
            <div class="form-group">
                <asp:GridView ID="grd_Users" runat="server" CssClass="dataTables_wrapper dt-foundation display datatable"
                    AutoGenerateColumns="False" DataKeyNames="sno" OnRowCancelingEdit="grd_Users_RowCancelingEdit"
                    OnRowUpdating="grd_Users_RowUpdating" OnRowDeleting="grd_Users_RowDeleting" OnSelectedIndexChanged="grd_Users_SelectedIndexChanged">
                    <Columns>
                        <asp:TemplateField HeaderText="User ID">
                            <ItemTemplate>
                                <asp:Label ID="lbl_UserID" runat="server" Text='<%# Eval("userid") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="User Name">
                            <ItemTemplate>
                                <asp:Label ID="lbl_UserName" runat="server" Text='<%# Eval("username") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Role">
                            <ItemTemplate>
                                <asp:Label ID="lbl_Rights" runat="server" Text='<%# Eval("rights") %>'></asp:Label>
                                <asp:HiddenField ID="hdn_Sno" runat="server" Value='<%# Eval("sno")%>'></asp:HiddenField>
                                <asp:HiddenField ID="hdn_Rights" runat="server" Value='<%# Eval("rights")%>'></asp:HiddenField>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Status">
                            <ItemTemplate>
                                <%# Convert.ToString(Eval("status1")).ToLower() == "active" ? "<asp:Label runat=\"server\" id=\"lbl_Status\" class=\"btn btn-success\" style=\"width:100%;cursor:default\">Active</asp:Label>" : "<asp:Label runat=\"server\" id=\"lbl_Status\" class=\"btn btn-warning\" style=\"width:100%;cursor:default\">Inactive</asp:Label>"%>
                                <asp:HiddenField ID="hdn_Status" runat="server" Value='<%# Eval("status")%>'></asp:HiddenField>
                                <asp:HiddenField ID="hdn_JoinDate" runat="server" Value='<%# Eval("joindate")%>'>
                                </asp:HiddenField>
                                <asp:HiddenField ID="hdn_ReleivedDate" runat="server" Value='<%# Eval("releiveddate")%>'>
                                </asp:HiddenField>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Edit" HeaderStyle-CssClass="removecolumn">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnk_Edit" runat="server" CommandName="Select" ToolTip="Select"
                                    Text='<i class="fa fa-pencil-square-o"></i>' ForeColor="#5AC15A" Font-Size="23px">
                                </asp:LinkButton>
                                <asp:LinkButton ID="lnk_View" runat="server" OnClick="SelectCurrentData" CommandArgument='<%# bind("userid") %>'
                                    ToolTip="View User" Text='<i class="glyphicon glyphicon-eye-open"></i>' ForeColor="#19B5FE"
                                    Font-Size="Large">                    
                                </asp:LinkButton>
                                <asp:LinkButton ID="lnk_Delete" runat="server" OnClientClick="ConfirmDelete(' this user? ')"
                                    CommandName="Delete" ToolTip="Delete" Text='<i class="glyphicon glyphicon-trash"></i>'
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
    <%--Javascript Validations--%>
    <script language="javascript" type="text/javascript">
        function validate() {
            var regularExpressionPassword = /^(?=.*[0-9])(?=.*[!@#$%^&*])[a-zA-Z0-9!@#$%^&*]{6,16}$/;
            var Password = document.getElementById("<%=txt_Password.ClientID%>").value;
            if (document.getElementById("<%=txt_UserID.ClientID%>").value == "") {
                swal("Employee ID Field can not be blank");
                document.getElementById("<%=txt_UserID.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txt_UserName.ClientID%>").value == "") {
                swal("Employee Name Field can not be blank");
                document.getElementById("<%=txt_UserName.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txt_JoinDate.ClientID%>").value == "") {
                swal("Join Date Field can not be blank");
                document.getElementById("<%=txt_JoinDate.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txt_Password.ClientID%>").value == "") {
                swal("Password Field can not be blank");
                document.getElementById("<%=txt_Password.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=txt_Password.ClientID%>").value.length < 6) {
                swal("Password must contain at least six characters!");
                document.getElementById("<%=txt_Password.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txt_Password.ClientID%>").value == document.getElementById("<%=txt_UserName.ClientID%>").value) {
                swal("Password must be different from Username!");
                document.getElementById("<%=txt_Password.ClientID%>").focus();
                return false;
            }
            re = /[0-9]/;
            if (!re.test(document.getElementById("<%=txt_Password.ClientID%>").value)) {
                swal("Password must contain at least one number (0-9)!");
                document.getElementById("<%=txt_Password.ClientID%>").focus();
                return false;
            }
            re = /[a-z]/;
            if (!re.test(document.getElementById("<%=txt_Password.ClientID%>").value)) {
                swal("Password must contain at least one lowercase letter (a-z)!");
                document.getElementById("<%=txt_Password.ClientID%>").focus();
                return false;
            }
            re = /[-!$%^@&*()_+|~=`{}\[\]:";'<>?,.\/]/;
            if (!re.test(document.getElementById("<%=txt_Password.ClientID%>").value)) {
                swal("Password must contain at least one special character!");
                document.getElementById("<%=txt_Password.ClientID%>").focus();
                return false;
            }
            return true;
        }
    </script>
    <%--External JS To Support Jquery,Bootstrap,Datatables--%>
  	<script type="text/javascript" charset="utf8" src="assets/js/Datatable/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="assets/js/Datatable/dataTables.bootstrap.min.js"></script>
     <script type="text/javascript">
		   $(function () {
			// Setup - add a text input to each footer cell
			$("#ctl00_ContentPlaceHolder1_grd_Users").prepend($("<thead></thead><tfoot></tfoot>").append($("#ctl00_ContentPlaceHolder1_grd_Users").find("tr:first")));

		    $('#ctl00_ContentPlaceHolder1_grd_Users tfoot th').each( function () {	
		        var title = $(this).text();
		        if(title!="Edit")
		        {
		        	$(this).replaceWith( '<td><input type="text" class="form-control" style="width:100%" placeholder="Search '+title+'" /></td>' );
		        }
		        else
		        {
		       		$(this).replaceWith( '<td></td>' );
		        }
		    } );	
            var table=$("#ctl00_ContentPlaceHolder1_grd_Users").dataTable({
            "lengthMenu": [[6, 10, 15, -1], [6, 10, 15, "All"]],
			"aaSorting": [],
			"deferRender": true,
			"sDom":    'C<"clear">lfrtip'
            });
            table.api().columns().every( function () {
	        var that = this;
	 
	        $( 'input', this.footer() ).on( 'keyup change', function () {
	            if ( that.search() !== this.value ) {
	                that
	                    .search( this.value )
	                    .draw();
	            }
	        } );
	    } );
        });
    </script>
</asp:Content>
