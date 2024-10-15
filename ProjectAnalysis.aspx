<%@ Page Title="Project Analysis" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="ProjectAnalysis.aspx.cs" Inherits="in_projectanalysis" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    <%--Heading Of MainContent--%>
    <div class="card">
        <div class="card-header">
            <div class="card-title">
                <div class="title">
                    Project Analysis</div>
            </div>
        </div>
        
        <%--Main Content--%>
        <div class="card-body">
            <%-- Form Elements --%>
            <asp:Label ID="lbl_Status" runat="server" Text=""></asp:Label>
            <div class="form-group">
            <div class="left-inner-addon">
                <label class="label-title">
                    Choose Project</label>
                <asp:Label ID="lbl_ProjectName" runat="server" Text=""></asp:Label>
                <i class="fa fa-folder-open"></i>
                <asp:DropDownList ID="ddl_ProjectName" runat="server" TabIndex="1" class="form-control"
                    Width="35%" AutoPostBack="true" OnSelectedIndexChanged="ddl_ProjectName_SelectedIndexChanged">
                    <asp:ListItem></asp:ListItem>
                </asp:DropDownList>
                <span style="color: #ff0000">**</span>
                <asp:HiddenField ID="hdn_ProjectID_Selected" runat="server" />
                <asp:HiddenField ID="hdn_ProjectNameSelected" runat="server" />
                <asp:HiddenField ID="hdn_FilePath" runat="server" />
                <asp:HiddenField ID="hdn_Extension" runat="server" />
                <asp:HiddenField ID="hdn_SelectedRecord" runat="server" />
                </div>
            </div>
            
            <%--Update Panel With Upload Control--%>
            <div class="form-group">
                <label class="label-title" style="float: left">
                    Add New File</label>
                <asp:UpdatePanel ID="upPnl_Upload" runat="server">
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btn_Upload" />
                    </Triggers>
                    <ContentTemplate>
                        <asp:FileUpload ID="upl_File" TabIndex="2" runat="server" />
                        <asp:Button ID="btn_Upload" Style="width: 17.5%" class="btn btn-info"
                            TabIndex="3" runat="server" Text="Upload" OnClick="UploadFile" />
                        <asp:Button ID="btn_Back" Style="width: 17.5%" class="btn btn-warning"
                            TabIndex="4" runat="server" Text="Back" OnClick="btn_Back_Click" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="form-group">
                <div class="card-title">
                    <h4>
                        <asp:Label ID="lbl_ProjectID_Name" class="title" runat="server"></asp:Label></h4>
                </div>
            </div>
            
            <%--GridView For Project Documents--%>
            <asp:GridView ID="grd_Projects" class="datatable table table-striped dataTable" runat="server"
                OnItemCommand="grd_Projects_ItemCommand" DataKeyNames="sno" AllowPaging="True"
                AutoGenerateColumns="false" OnSelectedIndexChanged="grd_Projects_SelectedIndexChanged"
                OnPageIndexChanging="OnPageIndexChanging" PageSize="5">
                <Columns>
                    <asp:TemplateField HeaderText="Download">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnk_Download" class="glyphicon glyphicon-download-alt" runat="server"
                                TabIndex="4" OnClick="lnk_Download_Click"></asp:LinkButton>
                        </ItemTemplate>
                        <ControlStyle ForeColor="#19B5FE" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Revision No">
                        <ItemTemplate>
                            <%#Container.DataItemIndex+1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="File Name">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lbl_FileName" Text='<%# Eval("File Name") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Uploaded On">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lbl_UpdatedOn" Text='<%# Eval("Updated On") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:TemplateField HeaderText="Delete">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnk_Delete" OnClientClick="ConfirmDelete(' this team? ')" TabIndex="4"
                                        runat="server" CommandName="Delete" ToolTip="Delete"  CssClass="btn btn-danger" Text="Delete">                    
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                </Columns>
                <PagerStyle CssClass="PaginationClass" />
            </asp:GridView>
        </div>
        <div>
            <div align="left" style="margin-left: 2%">
                <span style="color: #ff0000">**</span><font color="blue"> Required field</font>
            </div>
        </div>
    </div>
</asp:Content>
