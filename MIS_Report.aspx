<%@ Page Title="MIS Report" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="MIS_Report.aspx.cs" Inherits="MIS_Report" %>

<%--Ajax control toolkit extender to support calendar extender--%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<%--External CSS to Support Bootstrap Datatable--%>
	<link rel="stylesheet" type="text/css" href="assets/bower_components/DataTables/media/css/jquery.dataTables.min.css" />
    <link rel="stylesheet" type="text/css" href="assets/vendor/css/dataTables.bootstrap.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="card" style="min-height: 450px;">
        <%--Heading Of MainContent--%>
        <div class="card-header">
            <div class="card-title">
                <div class="title" id="TitleOfPage" runat="server">
                    MIS Report
                </div>
            </div>
        </div>
	
        <%--MainContent--%>
        <div class="card-body">
        	       	
            
            <%--Form Elements--%>
            <div class="form-group">
            <div class="left-inner-addon">
                <label for="lbl_ToDate" class="label-title">
                    As On Date</label>
                    <i class="glyphicon glyphicon-calendar"></i>
                <asp:TextBox ID="txt_AsOnDate" AutoPostBack="true" CausesValidation="true" class="form-control"
                    runat="server" OnTextChanged="txt_AsOnDate_TextChanged"></asp:TextBox>
                <cc1:CalendarExtender ID="calext_ToDate" CssClass="custom-calendar" PopupPosition="TopLeft"
                    runat="server" TargetControlID="txt_AsOnDate" Format="dd/MM/yyyy">
                </cc1:CalendarExtender>
                </div>
            </div>
            <div class="form-group">
                <asp:Button ID="btn_Export" class="btn btn-info" CausesValidation="true" runat="server"
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
                                    <asp:HiddenField ID="hdn_EmployeeID" runat="server" Value='<%# Eval("userid")%>'>
                                    </asp:HiddenField>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Employee Name">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_EmployeeName" runat="server" Text='<%# Eval("username") %>'></asp:Label>
                                    <asp:HiddenField ID="hdn_EmployeeName" runat="server" Value='<%# Eval("username")%>'>
                                    </asp:HiddenField>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--<asp:TemplateField HeaderText="Date">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Date" runat="server" Text='<%# Eval("CurrentDate", "{0:dd/MM/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                        </Columns>
                        <PagerStyle CssClass="PaginationClass" />
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
    
    <%--External Js To Supprot Bootstrap datatable--%>
    <script type="text/javascript" charset="utf8" src="assets/bower_components/DataTables/media/js/jquery.dataTables.min.js"></script>
   	<script type="text/javascript" src="assets/vendor/js/dataTables.bootstrap.js"></script>
   	<%--<script type="text/javascript">
			$(function () {
			   
				$("#ctl00_ContentPlaceHolder1_grd_DPR").prepend( $("<thead></thead>").append( $("#ctl00_ContentPlaceHolder1_grd_DPR").find("tr:first") ) ).dataTable();
    </script>--%>
    <%--<script type="text/javascript">

	        var array = $('#hdn_UserID').val().split(",");

	        var dllist = $('#ddl_EmployeeID');

	        $.each(array, function (i) {
	            dllist.append(
	        $('<option></option>').val(array[i]).html(array[i])
	    	);
	        });
	        
	       
		
    </script>--%>
    
    <script type="text/javascript">
    
	    
    </script>
</asp:Content>
