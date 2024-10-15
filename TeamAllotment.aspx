<%@ Page Title="Team Allotment" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="TeamAllotment.aspx.cs" Inherits="TeamAllotment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%--Inline CSS--%>
    <style type="text/css">
        input[type="radio"], input[type="checkbox"]
        {
            margin: 10px 20px 0 !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="card">
        <%--Heading Of MainContent--%>
        <div class="card-header">
            <div class="card-title">
                <div class="title">
                    Team Allotment</div>
            </div>
        </div>
        <%--Main Content,Block For Team Allotment--%>
        <asp:UpdatePanel ID="upl_ALL" runat="server">
            <ContentTemplate>
                <div id="block_Register" runat="server">
                    <div class="card-body">
                        <%-- Form Elements --%>
                        <asp:Label class="alert alert-warning" role="alert" ID="lbl_Error" runat="server"
                            Text="" Visible="false"></asp:Label>
                        <asp:HiddenField ID="hdn_TeamID" runat="server" />
                    </div>
                    <div class="form-group">
                    <div class="left-inner-addon">
                        <label class="label-title">
                            Select Team</label>
                            <i class="fa fa-users"></i>
                        <asp:DropDownList ID="ddl_TeamName" runat="server" TabIndex="1" class="form-control"
                            Width="35%" AutoPostBack="true" OnSelectedIndexChanged="ddl_TeamName_SelectedIndexChanged">
                            <asp:ListItem></asp:ListItem>
                        </asp:DropDownList>
                        <span style="color: #ff0000">**</span>
                        </div>
                    </div>
                    <div class="form-group">
                    <div class="left-inner-addon">
                        <label class="label-title">
                            Choose Team Leader</label>
                            <i class="fa fa-user"></i>
                        <asp:DropDownList ID="ddl_TeamLeader" runat="server" TabIndex="1" class="form-control"
                            Width="35%" AutoPostBack="true">
                            <asp:ListItem></asp:ListItem>
                        </asp:DropDownList>
                        <span style="color: #ff0000">**</span>
                        </div>
                    </div>
                    <%-- Block Of Select Members --%>
                    <div id="block_SelectMembers" runat="server">
                        <div class="form-group">
                            <div class="panel panel-primary" style="width: 90%; margin-left: 5%">
                                <div class="panel-heading">
                                    Select Members
                                </div>
                                <div class="panel-body">
                                    <asp:CheckBox ID="chk_SelectAll" runat="server" AutoPostBack="true" OnCheckedChanged="chk_SelectAllClicked"
                                        Text="Select All" Visible="false" />
                                    <asp:CheckBoxList ID="chk_TeamMembers" RepeatColumns="4" RepeatDirection="Vertical"
                                        RepeatLayout="Table" CellPadding="10" CellSpacing="5" TextAlign="Right" runat="server"
                                        AutoPostBack="true">
                                    </asp:CheckBoxList>
                                </div>
                            </div>
                            <span style="color: #ff0000"></span>
                        </div>
                    </div>
                    <%-- Block Of Selected Members --%>
                    <div id="block_SelectedMembers" runat="server" visible="false">
                        <div class="form-group">
                            <div class="panel panel-primary" style="width: 90%; margin-left: 5%">
                                <div class="panel-heading">
                                    Selected Members
                                </div>
                                <div class="panel-body">
                                    <div runat="server" id="lbl_SelectedMembers">
                                    </div>
                                </div>
                            </div>
                            <span style="color: #ff0000"></span>
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Button ID="btn_Add" Style="margin-left: 25%" class="btn btn-success" runat="server"
                            Text="Add" TabIndex="9" OnClientClick="return validate();" OnClick="btn_Add_Click">
                        </asp:Button>
                        <asp:Button ID="btn_Reset" class="btn btn-warning" runat="server" Text="Reset" CausesValidation="false"
                            TabIndex="10" OnClick="btn_Reset_Click"></asp:Button>
                        <asp:Button ID="btn_Back" class="btn btn-info" runat="server" Text="Back" CausesValidation="false"
                            TabIndex="11" OnClick="btn_Back_Click"></asp:Button>
                    </div>
                    <div>
                        <div align="left" style="margin-left: 5%">
                            <span style="color: #ff0000">**</span><font color="blue"> Required field</font>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <%--Javascript Validations For Required Field Validations--%>
        <script language="javascript" type="text/javascript">
            function validate() {
                if (document.getElementById("<%=ddl_TeamName.ClientID%>").selectedIndex == 0) {
                    swal("Team Field is Mandatory"); document.getElementById("<%=ddl_TeamName.ClientID%>").focus();
                    return false;
                } if (document.getElementById("<%=ddl_TeamLeader.ClientID%>").selectedIndex == 0) {
                    swal("Team Leader Field is Mandatory"); document.getElementById("<%=ddl_TeamLeader.ClientID%>").focus();
                    return false;
                } return true;
            }
        </script>
</asp:Content>
