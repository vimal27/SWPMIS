<%@ Page Title="DCS" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="DCS.aspx.cs" Inherits="DCS" Culture="en-GB" EnableEventValidation="false" %>

<%-- Ajax Control Toolkit for ajax calendar extender --%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
        
        .modal-backdrop.in
        {
            opacity: 0.4 !important;
        }
    </style>
    <%-- External CSS For Font Awesome,Bootstrap Datatable --%>
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
                                DCS Generation
                            </div>
                        </div>
                    </div>
                    <%--Main Content--%>
                    <%--Block For Add Task--%>
                    <div id="block_Register" runat="server">
                        <div class="table-responsive">
                            <table class="table">
                                <tbody>
                                    <tr>
                                        <div class="table-responsive">
                                            <table class="table table-bordered">
                                                <tbody>
                                                    <tr>
                                                        <td style="width: 20%">
                                                            <asp:Label ID="lbl_OrderNO" Style="margin-left: 5%" runat="server" Text="Order No"></asp:Label>
                                                        </td>
                                                        <td style="width: 30%">
                                                            <asp:Label ID="lbl_OrderNO_View" Style="margin-left: 5%" runat="server" Text="NA"></asp:Label>
                                                        </td>
                                                        <td style="width: 20%">
                                                            <asp:Label ID="lbl_DepartmentName" Style="margin-left: 5%" runat="server" Text="Department Name"></asp:Label>
                                                        </td>
                                                        <td style="width: 30%">
                                                            <div class="left-inner-addon">
                                                                <i class="fa fa-users"></i>
                                                                <asp:TextBox ID="txt_DepartmentName" MaxLength="25" runat="server" Style="width: 80%"
                                                                    CssClass="form-control"></asp:TextBox>
                                                                <span style="color: #ff0000">**</span>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 20%">
                                                            <asp:Label ID="lbl_ProjectID" Style="margin-left: 5%" runat="server" Text="Project ID"></asp:Label>
                                                        </td>
                                                        <td style="width: 30%">
                                                            <div class="left-inner-addon">
                                                                <i class="fa fa-folder-open"></i>
                                                                <asp:DropDownList ID="ddl_ProjectID" runat="server" Style="width: 80%" OnSelectedIndexChanged="ddl_ProjectID_SelectedIndexChanged"
                                                                    AutoPostBack="true" CssClass="form-control">
                                                                    <asp:ListItem>Select</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <span style="color: #ff0000">**</span>
                                                            </div>
                                                        </td>
                                                        <td style="width: 20%">
                                                            <asp:Label ID="lbl_ProjectName" Style="margin-left: 5%" runat="server" Text="Project Name"></asp:Label>
                                                        </td>
                                                        <td style="width: 30%">
                                                            <asp:Label ID="lbl_ProjectNameView" Style="margin-left: 5%" runat="server" Text=""></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <%--  <td style="width: 20%">
                                                                        <asp:Label ID="lbl_Slno" Style="margin-left: 5%" runat="server" Text="S.No."></asp:Label>
                                                                    </td>
                                                                    <td style="width: 30%">
                                                                        <asp:Label ID="lbl_SlnoView" Style="margin-left: 5%" runat="server" Text="NA"></asp:Label>
                                                                    </td>--%>
                                                        <td style="width: 20%">
                                                            <asp:Label ID="lbl_ReceivedDate" Style="margin-left: 5%" runat="server" Text="Received Date"></asp:Label>
                                                        </td>
                                                        <td style="width: 30%">
                                                            <div class="left-inner-addon">
                                                                <i class="glyphicon glyphicon-calendar"></i>
                                                                <asp:TextBox ID="txt_ReceivedDate" runat="server" Style="width: 80%" AutoPostBack="true"
                                                                    OnTextChanged="txt_ReceivedDate_TextChanged" CssClass="form-control"></asp:TextBox>
                                                                <cc1:CalendarExtender ID="calext_ReceivedDate" PopupPosition="TopLeft" runat="server"
                                                                    TargetControlID="txt_ReceivedDate" CssClass="custom-calendar" Format="dd/MM/yyyy">
                                                                </cc1:CalendarExtender>
                                                                <span style="color: #ff0000">**</span>
                                                            </div>
                                                        </td>
                                                        <td style="width: 20%">
                                                            <asp:Label ID="lbl_DispatchDate" Style="margin-left: 5%" runat="server" Text="Dispatch Date"></asp:Label>
                                                        </td>
                                                        <td style="width: 30%">
                                                            <div class="left-inner-addon">
                                                                <i class="glyphicon glyphicon-calendar"></i>
                                                                <asp:TextBox ID="txt_DispatchDate" runat="server" Style="width: 80%" CssClass="form-control"></asp:TextBox>
                                                                <cc1:CalendarExtender ID="calext_DispatchDate" PopupPosition="TopLeft" runat="server"
                                                                    TargetControlID="txt_DispatchDate" CssClass="custom-calendar" Format="dd/MM/yyyy">
                                                                </cc1:CalendarExtender>
                                                                <span style="color: #ff0000">**</span>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="1">
                                                            <asp:Label ID="lbl_ScopeDispatchDesc" Style="margin-left: 2%" runat="server" Text="Scope / Dispatch Description"></asp:Label>
                                                        </td>
                                                        <td colspan="3">
                                                            <asp:TextBox ID="txt_ScopeDispatchDesc" runat="server" Style="width: 88%" TextMode="MultiLine"
                                                                CssClass="form-control"></asp:TextBox>
                                                            <span style="color: #ff0000">**</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 20%">
                                                            <asp:Label ID="lbl_BatchQuantity" Style="margin-left: 5%" runat="server" Text="Batch Quantity"></asp:Label>
                                                        </td>
                                                        <td style="width: 30%">
                                                            <div class="left-inner-addon">
                                                                <i class="fa fa-bar-chart"></i>
                                                                <asp:TextBox ID="txt_BatchQuantity" onkeypress="return onlyDotsAndNumbers(event)"
                                                                    MaxLength="3" runat="server" Style="width: 80%" CssClass="form-control"></asp:TextBox>
                                                                <span style="color: #ff0000">**</span>
                                                            </div>
                                                        </td>
                                                        <td style="width: 20%">
                                                            <asp:Label ID="lbl_DipatchQuantity" Style="margin-left: 5%" runat="server" Text="Dispatch Quantity"></asp:Label>
                                                            <div class="left-inner-addon">
                                                                <i class="fa fa-bar-chart"></i>
                                                                <asp:TextBox ID="txt_DispatchQuantity" onkeypress="return onlyDotsAndNumbers(event)"
                                                                    MaxLength="4" runat="server" Style="width: 80%" CssClass="form-control"></asp:TextBox>
                                                                <span style="color: #ff0000">**</span>
                                                            </div>
                                                        </td>
                                                        <td style="width: 30%">
                                                            <asp:Label ID="lbl_TimeTaken" Style="margin-left: 5%" runat="server" Text="Time Taken(In Hours)"></asp:Label>
                                                            <div class="left-inner-addon">
                                                                <i class="glyphicon glyphicon-time"></i>
                                                                <asp:TextBox ID="txt_TimeTaken" MaxLength="6" onkeypress="return onlyNumbers(event)"
                                                                    runat="server" Style="width: 40%" CssClass="form-control"></asp:TextBox>
                                                                <cc1:MaskedEditExtender ID="me_TimeTaken" runat="server" Mask="999:99" TargetControlID="txt_TimeTaken"
                                                                    ClearMaskOnLostFocus="False">
                                                                </cc1:MaskedEditExtender>
                                                                <span style="color: #ff0000">**</span>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 20%">
                                                            <asp:Label ID="lbl_InputDetails" Style="margin-left: 5%" runat="server" Text="Input Details"></asp:Label>
                                                        </td>
                                                        <td style="width: 30%">
                                                            <div class="left-inner-addon">
                                                                <i class="glyphicon glyphicon-cloud-upload"></i>
                                                                <asp:TextBox ID="txt_InputDetails" runat="server" Style="width: 80%" CssClass="form-control"></asp:TextBox>
                                                                <span style="color: #ff0000">**</span>
                                                            </div>
                                                        </td>
                                                        <td style="width: 20%">
                                                            <asp:Label ID="lbl_OutputDetails" Style="margin-left: 5%" runat="server" Text="Output Details"></asp:Label>
                                                        </td>
                                                        <td style="width: 30%">
                                                            <div class="left-inner-addon">
                                                                <i class="glyphicon glyphicon-cloud-download"></i>
                                                                <asp:TextBox ID="txt_OutputDetails" runat="server" Style="width: 80%" CssClass="form-control"></asp:TextBox>
                                                                <span style="color: #ff0000">**</span>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <b>
                                                                <asp:Label ID="lbl_Activity" Style="margin-left: 3%" runat="server" Text="Activity"></asp:Label></b>
                                                        </td>
                                                        <td colspan="2">
                                                            <b>
                                                                <asp:Label ID="lbl_Signature" Style="margin-left: 2%" runat="server" Text="Signature"></asp:Label></b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Label ID="lbl_Activity1" Style="margin-left: 3%" runat="server" Text="Dispatch Qty Measured by"></asp:Label>
                                                        </td>
                                                        <td colspan="2">
                                                            <div class="left-inner-addon">
                                                                <i class="fa fa-user"></i>
                                                                <asp:DropDownList ID="ddl_Activity1" CssClass="form-control" runat="server">
                                                                    <asp:ListItem Value="0">Select</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <span style="color: #ff0000">**</span>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Label ID="lbl_Activity2" Style="margin-left: 3%" runat="server" Text="Discrepancy Report Created by"></asp:Label>
                                                        </td>
                                                        <td colspan="2">
                                                            <div class="left-inner-addon">
                                                                <i class="fa fa-user"></i>
                                                                <asp:DropDownList ID="ddl_Activity2" CssClass="form-control" runat="server">
                                                                    <asp:ListItem Value="0">Select</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <span style="color: #ff0000">**</span>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Label ID="lbl_Activity3" Style="margin-left: 3%" runat="server" Text="Discrepancy Report Checked by"></asp:Label>
                                                        </td>
                                                        <td colspan="2">
                                                            <div class="left-inner-addon">
                                                                <i class="fa fa-user"></i>
                                                                <asp:DropDownList ID="ddl_Activity3" CssClass="form-control" runat="server">
                                                                    <asp:ListItem Value="0">Select</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <span style="color: #ff0000">**</span>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Label ID="lbl_Activity4" Style="margin-left: 3%" runat="server" Text="Packed by"></asp:Label>
                                                        </td>
                                                        <td colspan="2">
                                                            <div class="left-inner-addon">
                                                                <i class="fa fa-user"></i>
                                                                <asp:DropDownList ID="ddl_Activity4" CssClass="form-control" runat="server">
                                                                    <asp:ListItem Value="0">Select</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <span style="color: #ff0000">**</span>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Label ID="lbl_Activity5" Style="margin-left: 3%" runat="server" Text="Uploaded by"></asp:Label>
                                                        </td>
                                                        <td colspan="2">
                                                            <div class="left-inner-addon">
                                                                <i class="fa fa-user"></i>
                                                                <asp:DropDownList ID="ddl_Activity5" CssClass="form-control" runat="server">
                                                                    <asp:ListItem Value="0">Select</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <span style="color: #ff0000">**</span>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Label ID="lbl_Activity6" Style="margin-left: 3%" runat="server" Text="Discrepancy and Message Sent by"></asp:Label>
                                                        </td>
                                                        <td colspan="2">
                                                            <div class="left-inner-addon">
                                                                <i class="fa fa-user"></i>
                                                                <asp:DropDownList ID="ddl_Activity6" CssClass="form-control" runat="server">
                                                                    <asp:ListItem Value="0">Select</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <span style="color: #ff0000">**</span>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="4">
                                                            <center>
                                                                <asp:Label ID="lbl_Note" Style="margin-left: 3%" runat="server" Text="<b>NOTE</b> : Details mentioned above has been checked and approved for invoicing."></asp:Label></center>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 20%">
                                                            <asp:Label ID="lbl_CheckedBy" Style="margin-left: 5%" runat="server" Text="Checked by"></asp:Label>
                                                        </td>
                                                        <td style="width: 30%">
                                                            <div class="left-inner-addon">
                                                                <i class="fa fa-user"></i>
                                                                <%--<asp:TextBox ID="txt_CheckedBy" MaxLength="30" runat="server" Style="width: 80%" CssClass="form-control"></asp:TextBox>--%>
                                                                <asp:DropDownList ID="ddl_CheckedBy" CssClass="form-control" runat="server" Width="80%">
                                                                    <asp:ListItem Value="0">Select</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <span style="color: #ff0000">**</span>
                                                            </div>
                                                        </td>
                                                        <td style="width: 20%">
                                                            <asp:Label ID="lbl_ApprovedBy" Style="margin-left: 5%" runat="server" Text="Approved by"></asp:Label>
                                                        </td>
                                                        <td style="width: 30%">
                                                            <div class="left-inner-addon">
                                                                <i class="fa fa-user"></i>
                                                                <asp:TextBox ID="txt_ApprovedBy" MaxLength="30" runat="server" Style="width: 80%"
                                                                    CssClass="form-control"></asp:TextBox>
                                                                <span style="color: #ff0000">**</span>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="4">
                                                            <asp:Button ID="btn_Save" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btn_Save_Click" />
                                                            <asp:Button ID="btn_Cancel" runat="server" CssClass="btn btn-warning" Text="Cancel"
                                                                OnClick="btn_Cancel_Click" />
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                        <%--Block For Gridview DCS--%>
                                        <div class="table-responsive">
                                            <asp:GridView ID="grd_DCS" runat="server" AutoGenerateColumns="false" AllowPaging="false"
                                                AllowSorting="false" DataKeyNames="id" OnSelectedIndexChanged="grd_DCS_SelectedIndexChanged"
                                                OnRowDeleting="grdDCS_RowDeleting" CssClass="table table-striped table-bordered table-hover">
                                                <EmptyDataTemplate>
                                                    <asp:Table ID="tbl_DCS" runat="server" CssClass="table table-striped table-bordered table-hover blueHeading kraTable emptyTable">
                                                        <asp:TableHeaderRow>
                                                            <asp:TableHeaderCell Text="Delete"></asp:TableHeaderCell>
                                                            <asp:TableHeaderCell Text="SlNo"></asp:TableHeaderCell>
                                                            <asp:TableHeaderCell Text="Project ID"></asp:TableHeaderCell>
                                                            <%--<asp:TableHeaderCell Text="DCS SlNo"></asp:TableHeaderCell>--%>
                                                            <asp:TableHeaderCell Text="Order No"></asp:TableHeaderCell>
                                                            <asp:TableHeaderCell Text="Project Name"></asp:TableHeaderCell>
                                                            <asp:TableHeaderCell Text="Department"></asp:TableHeaderCell>
                                                            <asp:TableHeaderCell Text="Received Date"></asp:TableHeaderCell>
                                                            <asp:TableHeaderCell Text="Dispatch Date"></asp:TableHeaderCell>
                                                            <asp:TableHeaderCell Text="Batch Qty"></asp:TableHeaderCell>
                                                            <asp:TableHeaderCell Text="Dispatch Qty"></asp:TableHeaderCell>
                                                            <asp:TableHeaderCell Text="Time Taken"></asp:TableHeaderCell>
                                                        </asp:TableHeaderRow>
                                                        <asp:TableRow>
                                                            <asp:TableCell ColumnSpan="5" Text="No Data Available"> </asp:TableCell>
                                                        </asp:TableRow>
                                                        <asp:TableFooterRow>
                                                            <asp:TableCell ColumnSpan="5"></asp:TableCell>
                                                        </asp:TableFooterRow>
                                                    </asp:Table>
                                                </EmptyDataTemplate>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="SNO" SortExpression="slno">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_slno" runat="server" Text='<%# Bind("Sno") %>'></asp:Label>
                                                            <asp:HiddenField ID="hdn_DispatchDescription" runat="server" Value='<%# Bind("dispatchdescription") %>'>
                                                            </asp:HiddenField>
                                                            <asp:HiddenField ID="hdn_InputDetails" runat="server" Value='<%# Bind("inputdetails") %>'>
                                                            </asp:HiddenField>
                                                            <asp:HiddenField ID="hdn_OutputDetails" runat="server" Value='<%# Bind("outputdetails") %>'>
                                                            </asp:HiddenField>
                                                            <asp:HiddenField ID="hdn_Activity1" runat="server" Value='<%# Bind("Activity1") %>'>
                                                            </asp:HiddenField>
                                                            <asp:HiddenField ID="hdn_Activity2" runat="server" Value='<%# Bind("Activity2") %>'>
                                                            </asp:HiddenField>
                                                            <asp:HiddenField ID="hdn_Activity3" runat="server" Value='<%# Bind("Activity3") %>'>
                                                            </asp:HiddenField>
                                                            <asp:HiddenField ID="hdn_Activity4" runat="server" Value='<%# Bind("Activity4") %>'>
                                                            </asp:HiddenField>
                                                            <asp:HiddenField ID="hdn_Activity5" runat="server" Value='<%# Bind("Activity5") %>'>
                                                            </asp:HiddenField>
                                                            <asp:HiddenField ID="hdn_Activity6" runat="server" Value='<%# Bind("Activity6") %>'>
                                                            </asp:HiddenField>
                                                            <asp:HiddenField ID="hdn_CheckedBy" runat="server" Value='<%# Bind("checkedby") %>'>
                                                            </asp:HiddenField>
                                                            <asp:HiddenField ID="hdn_ApprovedBy" runat="server" Value='<%# Bind("ApprovedBy") %>'>
                                                            </asp:HiddenField>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Project ID" SortExpression="projectid">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_ProjectID" runat="server" Text='<%# Eval("manualid") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:TemplateField HeaderText="DCS SlNo" SortExpression="DCSSlNo" >
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbl_DCSSlNo" runat="server" Text='<%# Bind("requestid") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>--%>
                                                    <asp:TemplateField HeaderText="Order No" SortExpression="orderno">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_orderno" runat="server" Text='<%# Bind("requestid") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Project Name" SortExpression="projectname">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_ProjectName" runat="server" Text='<%# Bind("projectname") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Department" SortExpression="department">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_Department" runat="server" Text='<%# Bind("department") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Received Date" SortExpression="receiveddate">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_ReceivedDate" runat="server" Text='<%# Eval("receiveddate", "{0:dd/MM/yyyy}") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Dispatch Date" SortExpression="Dispatchdate">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_DispatchDate" runat="server" Text='<%# Eval("dispatchdate", "{0:dd/MM/yyyy}") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Batch Qty" SortExpression="Batchname">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_BatchQty" runat="server" Text='<%# Bind("batchqty") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Dispatch Qty" SortExpression="dispsentby">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_DispatchQty" runat="server" Text='<%# Bind("DispatchQty") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Time Taken(in Hours)" SortExpression="timetaken">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_TimeTaken" runat="server" Text='<%# Bind("Timetaken")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ShowHeader="true" HeaderText="Actions" Visible="true">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnk_Download" TabIndex="3" runat="server" ToolTip="Download"
                                                                OnClick="lnk_Download_Click" Text='<i class="fa fa-download"></i>' ForeColor="blue"
                                                                CommandArgument='<%# Bind("requestid") %>' Font-Size="23px">
                                                            </asp:LinkButton>
                                                            <asp:LinkButton ID="lnk_Edit" TabIndex="3" runat="server" CommandName="Select" ToolTip="Select"
                                                                Text='<i class="fa fa-pencil-square-o"></i>' ForeColor="#5AC15A" Font-Size="23px">
                                                            </asp:LinkButton>
                                                            <asp:LinkButton ID="lnk_Delete" runat="server" CommandName="Delete" OnClientClick="ConfirmDelete(' this DCS? ')"
                                                                Text='<i class="glyphicon glyphicon-trash"></i>' ForeColor="#D45E5E" Font-Size="20px">
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:CommandField Visible="false" ShowEditButton="true" HeaderText="Edit" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                        <%--</ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btn_Report" />
                                <asp:PostBackTrigger ControlID="btn_Save" />
                            </Triggers>
                        </asp:UpdatePanel>--%>
                                        <%--<asp:UpdatePanel runat="server" ID="upl_Register" ChildrenAsTriggers="true">
                            <ContentTemplate>--%>
                                        <div>
                                            <div align="left" style="margin-left: 5%">
                                                <span style="color: #ff0000">**</span><font color="blue"> Required field</font>
                                            </div>
                                        </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="lbl_SlnoView" runat="server" Value="" />
        <%--Validations Required Field--%>
        <script type="text/javascript" charset="utf8" src="assets/js/Datatable/jquery.dataTables.min.js"></script>
        <script type="text/javascript" src="assets/js/Datatable/dataTables.bootstrap.min.js"></script>
        <script language="javascript" type="text/javascript">
            function validate() {
                if (document.getElementById("<%=txt_DepartmentName.ClientID%>").value == "") {
                    swal("Department Name Field can not be blank");
                    document.getElementById("<%=txt_DepartmentName.ClientID%>").focus();
                    return false;
                }
                if (document.getElementById("<%=ddl_ProjectID.ClientID%>").selectedIndex == 0) {
                    swal("Project Field is Mandatory");
                    document.getElementById("<%=ddl_ProjectID.ClientID%>").focus();
                    return false;
                }
                if (document.getElementById("<%=txt_DispatchDate.ClientID%>").value == "") {
                    swal("Dispatch Date Field can not be blank");
                    document.getElementById("<%=txt_DispatchDate.ClientID%>").focus();
                    return false;
                }
                if (document.getElementById("<%=txt_ScopeDispatchDesc.ClientID%>").value == "") {
                    swal("Scope / Dispatch Description Field can not be blank");
                    document.getElementById("<%=txt_ScopeDispatchDesc.ClientID%>").focus();
                    return false;
                }
                if (document.getElementById("<%=txt_BatchQuantity.ClientID%>").value == "") {
                    swal("Batch Quantitiy Field can not be blank");
                    document.getElementById("<%=txt_BatchQuantity.ClientID%>").focus();
                    return false;
                }
                if (document.getElementById("<%=txt_DispatchQuantity.ClientID%>").value == "") {
                    swal("Dispatch Quantity Field can not be blank");
                    document.getElementById("<%=txt_DispatchQuantity.ClientID%>").focus();
                    return false;
                }
                if (document.getElementById("<%=txt_TimeTaken.ClientID%>").value == "___:__") {
                    swal("Time Taken Field can not be blank");
                    document.getElementById("<%=txt_TimeTaken.ClientID%>").focus();
                    return false;
                }
                if (document.getElementById("<%=txt_InputDetails.ClientID%>").value == "") {
                    swal("Input Details Field can not be blank");
                    document.getElementById("<%=txt_InputDetails.ClientID%>").focus();
                    return false;
                }
                if (document.getElementById("<%=txt_OutputDetails.ClientID%>").value == "") {
                    swal("Output Details Field can not be blank");
                    document.getElementById("<%=txt_OutputDetails.ClientID%>").focus();
                    return false;
                }
                if (document.getElementById("<%=ddl_Activity1.ClientID%>").selectedIndex == 0) {
                    swal("All Signatures are mandatory");
                    document.getElementById("<%=ddl_Activity1.ClientID%>").focus();
                    return false;
                }
                if (document.getElementById("<%=ddl_Activity2.ClientID%>").selectedIndex == 0) {
                    swal("All Signatures are mandatory");
                    document.getElementById("<%=ddl_Activity2.ClientID%>").focus();
                    return false;
                }
                if (document.getElementById("<%=ddl_Activity3.ClientID%>").selectedIndex == 0) {
                    swal("All Signatures are mandatory");
                    document.getElementById("<%=ddl_Activity3.ClientID%>").focus();
                    return false;
                }
                if (document.getElementById("<%=ddl_Activity4.ClientID%>").selectedIndex == 0) {
                    swal("All Signatures are mandatory");
                    document.getElementById("<%=ddl_Activity4.ClientID%>").focus();
                    return false;
                }
                if (document.getElementById("<%=ddl_Activity5.ClientID%>").selectedIndex == 0) {
                    swal("All Signatures are mandatory");
                    document.getElementById("<%=ddl_Activity5.ClientID%>").focus();
                    return false;
                }
                if (document.getElementById("<%=ddl_Activity6.ClientID%>").selectedIndex == 0) {
                    swal("All Signatures are mandatory");
                    document.getElementById("<%=ddl_Activity6.ClientID%>").focus();
                    return false;
                }
                if (document.getElementById("<%=ddl_CheckedBy.ClientID%>").selectedIndex == 0) {
                    swal("Checked by Field is mandatory");
                    document.getElementById("<%=ddl_CheckedBy.ClientID%>").focus();
                    return false;
                }
                if (document.getElementById("<%=txt_ApprovedBy.ClientID%>").value == "") {
                    swal("Approved by Field can not be blank");
                    document.getElementById("<%=txt_ApprovedBy.ClientID%>").focus();
                    return false;
                }
                return true;
            }
            function onlyDotsAndNumbers(event) {
                var charCode = (event.which) ? event.which : event.keyCode
                if (charCode == 46) {
                    return true;
                }
                if (charCode > 31 && (charCode < 48 || charCode > 57))
                    return false;
            }
            function onlyNumbers(event) {
                var charCode = (event.which) ? event.which : event.keyCode
                if (charCode > 31 && (charCode < 48 || charCode > 57))
                    return false;
            }
        </script>
        <script type="text/javascript">

            $(function () {

                // Setup - add a text input to each footer cell
                $("#ctl00_ContentPlaceHolder1_grd_DCS").prepend($("<thead></thead><tfoot></tfoot>").append($("#ctl00_ContentPlaceHolder1_grd_DCS").find("tr:first")));

                $('#ctl00_ContentPlaceHolder1_grd_DCS tfoot th').each(function () {
                    var title = $(this).text();
                    if (title != "Actions") {
                        $(this).replaceWith('<td><input type="text" class="form-control" style="width:100%" placeholder="Search ' + title + '" /></td>');
                    }
                    else {
                        $(this).replaceWith('<td></td>');
                    }
                });
                var table = $("#ctl00_ContentPlaceHolder1_grd_DCS").dataTable({
                    "lengthMenu": [[6, 10, 15, -1], [6, 10, 15, "All"]],
                    "aaSorting": [],
                    "deferRender": true,
                    "scrollX": true,
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
            var prm = Sys.WebForms.PageRequestManager.getInstance();

            prm.add_initializeRequest(InitializeRequest);

            function InitializeRequest(sender, args) {
                if (sender._postBackSettings.sourceElement.id == "DownloadFile") {
                    var iframe = document.createElement("iframe");
                    iframe.src = "Dcs_Report.aspx";
                    iframe.style.display = "none";
                    document.body.appendChild(iframe);
                }
            }
        </script>
</asp:Content>
