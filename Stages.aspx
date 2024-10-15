<%@ Page Title="Stages" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Stages.aspx.cs" Inherits="Stages" %>

<%--Assembly Reference For Ajax Calendar Extender--%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <%--Heading Of MainContent--%>
    <div class="card">
        <div class="card-header">
            <div class="card-title">
                <div class="title">
                    Stage Activity Master
                </div>
            </div>
        </div>

        <%--MainContent--%>
        <div class="card-body">

            <%--CSS Loader--%>
            <%--<div class="cssload-container">
                <div class="cssload-whirlpool">
                </div>
            </div>--%>

            <%--Form Elements--%>
            <asp:UpdatePanel runat="server" ID="Register">
                <ContentTemplate>
                    <div class="form-group">
                    <div class="left-inner-addon">
                        <label for="lbl_Project" class="label-title">
                            Select Project</label>
                            <i class="fa fa-folder-open"></i>
                        <asp:DropDownList ID="ddl_Project" TabIndex="1" AutoPostBack="true" class="form-control"
                            Style="width: 35%" runat="server" OnSelectedIndexChanged="ddl_Project_SelectedIndexChanged">
                            <asp:ListItem></asp:ListItem>
                        </asp:DropDownList>
                        <span style="color: #ff0000">**</span>
                        </div>
                    </div>
                    <div class="form-group">
                    <div class="left-inner-addon">
                        <label for="lbl_AddStage" class="label-title">
                            Add Stage</label>
                            <i class="glyphicon glyphicon-stats"></i>
                        <asp:TextBox ID="txt_AddStage" onkeypress="return alphanumeric()" MaxLength="250" runat="server" TabIndex="2" class="form-control" Style="width: 35%"
                            CausesValidation="true" ValidationGroup="Add"></asp:TextBox><span style="color: #ff0000">
                        **</span>
                        </div>
                    </div>
                    <div class="form-group" style="margin-left: 25%">
                        <asp:Button ID="btn_Add" class="btn btn-success" TabIndex="4" Text="Add" runat="server"
                            OnClick="btn_Add_Click" />
                        <asp:Button ID="btn_Reset" class="btn btn-warning" TabIndex="5" Text="Reset" runat="server"
                            CausesValidation="False" OnClick="btn_Reset_Click" />
                        <asp:Button ID="btn_Back" class="btn btn-info" TabIndex="5" Text="Back" runat="server"
                            CausesValidation="False" OnClick="btn_Back_Click" />
                        <asp:HiddenField ID="hdn_SelectedValue" runat="server" />
                        <asp:HiddenField ID="hdn_SelectedText" runat="server" />
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div>
                <div class="form-group">
                    <div class="card-title">
                        <h4>
                            <asp:Label ID="lbl_ProjectID_Name" class="title" runat="server"></asp:Label></h4>
                    </div>
                </div>
                <div>

                    <%--GridView For Stage--%>
                    <asp:UpdatePanel runat="server" ID="upl_Grid">
                        <ContentTemplate>
                            <asp:GridView ID="grd_Scope" AllowPaging="True" OnPageIndexChanging="OnPageIndexChanging"
                                PageSize="5" runat="server" CssClass="datatable table table-striped dataTable"
                                AutoGenerateColumns="False" DataKeyNames="slno" OnRowCancelingEdit="grd_Scope_RowCancelingEdit"
                                OnRowEditing="grd_Scope_RowEditing" OnRowUpdating="grd_Scope_RowUpdating" OnRowDeleting="grd_Scope_RowDeleting">
                                <Columns>
                                    <asp:TemplateField HeaderText="Stage">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txt_Stage" runat="server" Text='<%# Eval("Name") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_StageName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="33%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status">
                                        <EditItemTemplate>
                                            <asp:RadioButtonList ID="rbl_Status" runat="server" RepeatColumns="2" SelectedValue='<%# Eval("Status") %>'>
                                                <asp:ListItem Value="1">Active</asp:ListItem>
                                                <asp:ListItem Value="0">In Active</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <%# Convert.ToString(Eval("status1")).ToLower() == "active" ? "<span class=\"btn btn-success\" style=\"width:50%;cursor:default\">Active</span>" : "<span class=\"btn btn-warning\" style=\"width:50%;cursor:default\">Inactive</span>"%>
                                        </ItemTemplate>
                                        <ItemStyle Width="33%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Edit">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnk_Edit" runat="server" CommandName="Edit" ToolTip="edit" Text='<i class="fa fa-pencil-square-o"></i>'
                                                ForeColor="#19B5FE" Font-Size="23px">
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="lnk_Delete" OnClientClick="ConfirmDelete(' this stage? ')" runat="server"
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
                                        <ItemStyle Width="33%" />
                                    </asp:TemplateField>
                                </Columns>
                                <PagerStyle CssClass="PaginationClass" />
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div>
                    <div align="left" style="margin-left: 5%">
                        <span style="color: #ff0000">**</span><font color="blue"> Required field</font>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
