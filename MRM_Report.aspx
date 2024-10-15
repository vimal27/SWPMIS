<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="MRM_Report.aspx.cs" Inherits="MRM_Report" %>

<%-- Ajax control toolkit extender to support Calendar Extendar --%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
        <div class="card">
            
            <%--Heading Of MainContent--%>
            <div class="card-header">
                <div class="card-title">
                    <div class="title" id="TitleOfPage" runat="server">
                        MRM Report</div>
                </div>
            </div>
            <%-- Main Content --%>
            <div class="card-body" style="min-height: 400px">
            
                
                <%-- Form Elements --%>
                <div class="form-group">
                <div class="left-inner-addon">
                    <label for="lbl_FromDate" class="label-title">
                        From Date</label>
                        <i class="glyphicon glyphicon-calendar"></i>
                    <asp:TextBox ID="txt_FromDate" AutoPostBack="true" CausesValidation="true" class="form-control" OnTextChanged="txt_FromDate_TextChanged"
                        runat="server"></asp:TextBox><span style="color: #ff0000"> **</span>
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
                    <asp:TextBox ID="txt_ToDate" CausesValidation="true" class="form-control"
                        runat="server"></asp:TextBox><span style="color: #ff0000"> **</span>
                    <cc1:CalendarExtender ID="calext_ToDate" CssClass="custom-calendar" PopupPosition="TopLeft"
                        runat="server" TargetControlID="txt_ToDate" Format="dd/MM/yyyy">
                    </cc1:CalendarExtender>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Button ID="btn_Cancel" TabIndex="10" class="btn btn-warning" runat="server" Text="Current Date"
                        CausesValidation="false" OnClick="btn_Cancel_Click"></asp:Button>
                    <asp:Button ID="btn_Report" class="btn btn-success" CausesValidation="true" runat="server"
                        Text="Report"  OnClick="btn_Report_Click"></asp:Button>
                    
                </div>
                <%--GridView Of Report
                <div id="block_Grid" runat="server" visible="false" style="overflow-x: auto">
                    <div class="form-group">
                        <asp:GridView ID="grd_DPR" runat="server" CssClass="datatable table table-striped dataTable"
                            Style="overflow: scroll" AutoGenerateColumns="False" DataKeyNames="slno" AllowPaging="true"
                            OnPageIndexChanging="OnPageIndexChanging" PageSize="5">
                            <Columns>
                                <asp:TemplateField HeaderText="Employee">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Employee" runat="server" Text='<%# Eval("Employee") %>'></asp:Label>
                                        <asp:HiddenField ID="hdn_EmployeeID" runat="server" Value='<%# Eval("EmpNo")%>'>
                                        </asp:HiddenField>
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
                                        <asp:Label ID="lbl_Date" runat="server" Text='<%# Eval("CurrentDate") %>'></asp:Label>
                                        <asp:HiddenField ID="hdn_Date" runat="server" Value='<%# Eval("Date")%>'></asp:HiddenField>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Project">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Project" runat="server" Text='<%# Eval("ProjectID") %>'></asp:Label>
                                        <asp:HiddenField ID="hdn_ProjectID" runat="server" Value='<%# Eval("Project")%>'>
                                        </asp:HiddenField>
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
                                        <asp:HiddenField ID="hdn_StatusOfTask" runat="server" Value='<%# Eval("statusoftask") %>'>
                                        </asp:HiddenField>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Start Time">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_StartTime" runat="server" Text='<%# Eval("StartTime") %>'></asp:Label>
                                        <asp:HiddenField ID="hdn_StartTime" runat="server" Value='<%# Eval("TimeStart")%>'>
                                        </asp:HiddenField>
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
                        </asp:GridView>--%>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">

        
    </script>
</asp:Content>
