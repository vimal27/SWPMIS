<%@ Page Title="Productivity Report" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="ProductivityReport.aspx.cs" Inherits="ProductivityReport" %>

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
        .card.summary-inline .card-body
        {
            height:115px !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="card" style="height: 1250px !important">
        <%--Heading Of MainContent--%>
        <div class="card-header">
            <div class="card-title">
                <div class="title" id="TitleOfPage" runat="server">
                    Productivity Report (Applicable Since 1st April 2016)
                </div>
            </div>
        </div>
        <%-- Main Content --%>
        <div class="card-body">
            <asp:UpdatePanel ID="upl_ALL" runat="server">
                <ContentTemplate>
                    <%--Average Production For Given Period--%>
                    <div class="col-lg-4 col-md-4 col-sm-4 col-xs-10" style="top: 0; right: 80px; float: right;
                        margin-bottom: 1%">
                        <div id="chartdiv" style="height: 285px; width: 428px">
                        </div>
                    </div>
                    <%-- Form Elements --%>
                    <div class="form-group">
                    <div class="left-inner-addon">
                        <label id="Label1" for="lbl_EmployeeID" runat="server" class="label-title">
                            Employee</label>
                        <asp:Label ID="lbl_CurrentEmployeeID" runat="server"></asp:Label>
                        <i class="fa fa-user" runat="server" id="icon_EmployeeDdl" Visible="false"></i>
                        <asp:DropDownList ID="ddl_EmployeeID" Visible="false" CausesValidation="true" class="form-control"
                            runat="server">
                            <asp:ListItem></asp:ListItem>
                        </asp:DropDownList>
                        </div>
                        <asp:HiddenField ID="hdn_UserID" runat="server" />
                    </div>
                    <div class="form-group">
                    <div class="left-inner-addon">
                        <label for="lbl_FromDate" class="label-title">
                            From Date</label>
                            <i class="fa fa-calendar"></i>
                        <asp:TextBox ID="txt_FromDate" CausesValidation="true" class="form-control" runat="server"></asp:TextBox>
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
                        <asp:TextBox ID="txt_ToDate" CausesValidation="true" class="form-control" runat="server"></asp:TextBox>
                        <cc1:CalendarExtender ID="calext_ToDate" CssClass="custom-calendar" PopupPosition="TopLeft"
                            runat="server" TargetControlID="txt_ToDate" Format="dd/MM/yyyy">
                        </cc1:CalendarExtender>
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Button ID="btn_Report" class="btn btn-success" CausesValidation="true" runat="server"
                            Text="Report" TabIndex="10" OnClick="btn_Report_Click"></asp:Button>
                        <asp:Button ID="btn_Export" class="btn btn-info" CausesValidation="true" runat="server"
                            Text="Export" TabIndex="11" OnClick="btn_Export_Click"></asp:Button>
                        <asp:Button ID="btn_Cancel" class="btn btn-warning" runat="server" Text="Cancel"
                            CausesValidation="false" OnClick="btn_Cancel_Click"></asp:Button>
                    </div>
                    <%--Task Details--%>
                    <div id="block_all" runat="server">
                        <div class="card-header">
                            <div class="card-title">
                                <div class="title" id="Div1" runat="server">
                                    Task Details
                                </div>
                            </div>
                        </div>
                        <div class="card-body">
                            <%--URL Query String To Redirect to On Early Delivery--%>
                            <div class="col-lg-4 col-md-4 col-sm-4 col-xs-10">
                                <asp:LinkButton ID="lnk_OnEarly" runat="server" OnClick="RedirectToStatus_6">
                                    <div class="card green summary-inline">
                                        <div class="card-body">
                                            <i class="icon fa fa-inbox fa-3x"></i>
                                            <div class="content">
                                                <div class="title">
                                                    <asp:Label ID="lbl_OnEarly" runat="server" Text="0"></asp:Label>
                                                </div>
                                                <div class="sub-title">
                                                    On Early Delivery
                                                </div>
                                            </div>
                                            <div class="clear-both">
                                            </div>
                                        </div>
                                    </div>
                                </asp:LinkButton>
                            </div>
                            <%--URL Query String To Redirect to On Time Delivery--%>
                            <div class="col-lg-4 col-md-4 col-sm-4 col-xs-10">
                                <asp:LinkButton ID="lnk_OnTime" runat="server" OnClick="RedirectToStatus_7">
                                    <div class="card blue summary-inline">
                                        <div class="card-body">
                                            <i class="icon glyphicon glyphicon-play fa-3x"></i>
                                            <div class="content">
                                                <div class="title">
                                                    <asp:Label ID="lbl_OnTime" runat="server" Text="0"></asp:Label>
                                                </div>
                                                <div class="sub-title" runat="server" id="title_OnTime">
                                                    On Time Delivery
                                                </div>
                                            </div>
                                            <div class="clear-both">
                                            </div>
                                        </div>
                                    </div>
                                </asp:LinkButton>
                            </div>
                            <%--URL Query String To Redirect to Extended Delivery--%>
                            <div class="col-lg-4 col-md-4 col-sm-4 col-xs-10" style="cursor: pointer">
                                <asp:LinkButton ID="lnk_OnExtend" runat="server" OnClick="RedirectToStatus_8">
                                    <div class="card yellow summary-inline">
                                        <div class="card-body">
                                            <i class="icon glyphicon glyphicon-pause fa-3x"></i>
                                            <div class="content">
                                                <div class="title">
                                                    <asp:Label ID="lbl_Extended" runat="server" Text="0"></asp:Label>
                                                </div>
                                                <div class="sub-title" runat="server" id="title_Extended">
                                                    Extended Delivery
                                                </div>
                                            </div>
                                            <div class="clear-both">
                                            </div>
                                        </div>
                                    </div>
                                </asp:LinkButton>
                            </div>
                            <div class="col-lg-12 col-md-12 col-sm-4 col-xs-10">
                                <div id="BarChart" style="height: 450px !important; margin-top: 2%">
                                </div>
                                <%--<div class="container-fluid">
                                    <div class="row text-center" style="overflow: hidden;">
                                        <div class="col-sm-3" style="float: none !important; display: inline-block;">
                                            <label class="text-left">
                                                Angle:</label>
                                            <input class="chart-input" data-property="angle" type="range" min="0" max="89" value="30"
                                                step="1" />
                                        </div>
                                        <div class="col-sm-3" style="float: none !important; display: inline-block;">
                                            <label class="text-left">
                                                Depth:</label>
                                            <input class="chart-input" data-property="depth3D" type="range" min="1" max="120"
                                                value="20" step="1" />
                                        </div>
                                    </div>
                                </div>--%>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btn_Report" />
                    <asp:PostBackTrigger ControlID="btn_Export" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <%--External Js To Bootstrap,Jquery,Datatables--%>
    <script type="text/javascript" charset="utf8" src="assets/js/Datatable/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="assets/js/Datatable/dataTables.bootstrap.min.js"></script>
    <%--AmCharts--%>
    <script src="assets/amcharts/amcharts.js"></script>
    <script type="text/javascript" src="assets/amcharts/amexport_combined.js"></script>
    <script src="assets/amcharts/gauge.js"></script>
    <script src="assets/amcharts/light.js"></script>
    <script type="text/javascript">
        $(document).ready(function(){
            setChart();
            setBarChart();
        });
        function setChart()
        {
            var gaugeChart = AmCharts.makeChart("chartdiv", {
                "type": "gauge",
                "theme": "light",
                "axes": [{
                    "axisThickness": 1,
                    "axisAlpha": 0.2,
                    "tickAlpha": 0.2,
                    "valueInterval": 20,
                    "bands": [{
                        "color": "red",
                        "endValue": 50,
                        "startValue": 0
                    },
                {
                    "color": "yellow",
                    "endValue": 80,
                    "startValue": 50
                },
                {
                    "color": "aqua",
                    "endValue": 100,
                    "startValue": 80
                },
                {
                    "color": "lime",
                    "endValue": 140,
                    "startValue": 100
                }],
                    "bottomText": "Production Rate",
                    "bottomTextYOffset": -20,
                    "endValue": 140
                }],
                "arrows": [{}],
                "export": {
                    "enabled": true
                }
            });

            setInterval(randomValue, 900);

            // set Value Of Production
            function randomValue() {
                var value = <%= Convert.ToInt32(Session["PercentageChart"])%>;
                if (gaugeChart) {
                    if (gaugeChart.arrows) {
                        if (gaugeChart.arrows[0]) {
                            if (gaugeChart.arrows[0].setValue) {
                                gaugeChart.arrows[0].setValue(value);
                                gaugeChart.axes[0].setBottomText("Production Rate : " + value);
                            }
                        }
                    }
                }
            }
         }
         //Bar Chart
         function setBarChart()
         {
            var arrMonthWise = '<%=Convert.ToString(Session["BarChartData"]) %>';
            if(arrMonthWise=="0")
            {
                arrMonthWise=0;
            }
            else
            {
                arrMonthWise=<%=Convert.ToString(Session["BarChartData"]) %>;
            }
            AmCharts.ready(function() {
            // SERIAL CHART
            chart = new AmCharts.AmSerialChart();
            chart.dataProvider = arrMonthWise;
            chart.categoryField = "country";
            chart.marginRight = 0;
            chart.marginTop = 0;    
            chart.autoMarginOffset = 0;
            
            // the following two lines makes chart 3D
            chart.depth3D = 20;
            chart.angle = 30;

            // AXES
            // category
            var categoryAxis = chart.categoryAxis;
            categoryAxis.labelRotation = 90;
            categoryAxis.dashLength = 5;
            categoryAxis.gridPosition = "start";
    
            categoryAxis.gridCount = arrMonthWise.length;
            categoryAxis.autoGridCount = false;

            // value
            var valueAxis = new AmCharts.ValueAxis();
            valueAxis.title = "Production";
            valueAxis.dashLength = 5;
            chart.addValueAxis(valueAxis);

            // GRAPH            
            var graph = new AmCharts.AmGraph();
            graph.valueField = "visits";
            graph.colorField = "color";
            graph.colors=["#67b7dc", "#fdd400", "#84b761", "#cc4748", "#cd82ad", "#2f4074", "#448e4d", "#b7b83f", "#b9783f", "#b93e3d", "#913167","#666","#777"];
            graph.balloonText = "[[category]]: [[value]]";
            graph.type = "column";
            graph.lineAlpha = 0;
            graph.fillAlphas = 1;
            chart.addGraph(graph);

            // WRITE
            chart.write("BarChart");
        });
         }
    </script>
</asp:Content>
