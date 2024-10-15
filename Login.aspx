<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<%--Title On Browser--%>
    <title>Login</title>
    <meta charset="UTF-8" />
    <meta name="Designer" content="PremiumPixels.com" />
    <meta name="Author" content="$hekh@r d-Ziner, CSSJUNTION.com" />
    
    <%--Login Css Styles--%>
    <link rel="stylesheet" type="text/css" href="assets/css/font-awesome.css" />
    <link rel="stylesheet" type="text/css" href="assets/css/sweetalert.css" />
	<script type="text/javascript" src="assets/js/sweetalert.min.js"></script>
    <style>
        /* NOTE: The styles were added inline because Prefixfree needs access to your styles and they must be inlined if they are on local disk! */html, body
        {
            width: 100%;
            height: 100%;
        }
        body
        {
            margin: 0 auto;
            display: table;
            text-align: center;
            font-family: 'Open Sans' , sans-serif;
            background: #81b5d6;
            max-width: 33em;
        }
        .wrap
        {
            margin-top: 50px;
        }
        .flip-container
        {
            perspective: 1000;
            border-radius: 50%;
            margin: 0 auto 10px auto;
        }
        .logged-in
        {
            transform: rotateY(180deg);
        }
        .flip-container, .front, .back, .back-logo
        {
            width: 130px;
            height: 130px;
        }
        .flipper
        {
            transition-duration: 0.6s;
            transform-style: preserve-3d;
        }
        .front, .back
        {
            backface-visibility: hidden;
            position: absolute;
            top: 0;
            left: 0;
            background-size: cover;
        }
        .front
        {
            background: url(assets/img/profile/Flip_Img.png) 0 0 no-repeat;
            margin-top: 75%;
        }
        .back
        {
            transform: rotateY(180deg);
        }
        h1
        {
            font-size: 22px;
            color: #FFF;
            margin-top: 22%;
        }
        h1 span
        {
            font-weight: 300;
        }
        input[type=text], input[type=password]
        {
            color: #FFF;
            background: #68add8; /* Old browsers */
            background: linear-gradient(45deg,  #68add8 0%,#8cbede 100%); /* W3C */
            width: 250px;
            height: 40px;
            margin: 0 auto 10px auto;
            font-size: 14px;
            padding-left: 15px;
            border: none;
            box-shadow: -3px 3px #679acb;
            -webkit-appearance: none;
            border-radius: 0;
            border-top: 1px solid #92c5e2;
            border-right: 1px solid #92c5e2;
        }
        input::-webkit-input-placeholder
        {
            color: #FFF;
        }
        input:focus
        {
            outline: none;
        }
        input[type=submit]
        {
            color: #fff;
            background-color: #3f88b8;
            font-size: 14px;
            height: 40px;
            border: none;
            margin: 0 auto 0 17px;
            padding: 0 20px 0 20px;
            -webkit-appearance: none;
            border-radius: 0;
            cursor: pointer;
        }
        input[type=submit]:hover
        {
            background-color: #3f7ba2;
        }
        a
        {
            color: #1c70a7;
            font-weight: 600;
            font-size: 12px;
            text-decoration: none;
        }
        a:hover
        {
            color: #3f7ba2;
        }
        .hint
        {
            width: 250px;
            dislay: block;
            margin: 80px auto 0 auto;
            text-align: left;
        }
        .hint p
        {
            padding: 5px 0 5px 0;
            color: #FFF;
            font-weight: 600;
            font-size: 20px;
        }
        .hint p span
        {
            font-weight: 300;
            font-size: 16px;
        }
        .footer
        {
            position:fixed;
            left:0;
            color:White;
            padding-top:2%;
            bottom:0;
            width:100%;
            height:3%;   /* Height of the footer */
            background:#3f7ba2;
            opacity:0.7%;
        }
    </style>
    <script type="text/javascript" src="assets/js/login/prefixfree.min.js"></script>
</head>
<body>
    <div class="wrap">
        <div class="flip-container" id='flippr'>
            <div class="flipper">
                <div class="front">
                </div>
                <div class="back">
                </div>
            </div>
        </div>
        
        <%--Title On Login Page--%>
        <h1 class="text" style="color:#3f7ba2" id="H1">
            ELearning PMIS</h1>
        <form method='post' id="theForm" runat="server">
        <input type="text" id="txt_UserName" runat="server" tabindex="1" placeholder="User ID" />
        <input id="txt_Password" runat="server" tabindex="2" placeholder="Password" type="password" />
        <div class='login'>
            <asp:Button  ID="Btn_Login" CssClass="btnLogin" TabIndex="4" runat="server" OnClick="Btn_Login_Click"
                Text="Login"></asp:Button>
        </div>
        <!-- /login -->
        </form>
        <asp:Label runat="server" style="font-weight:bold" ForeColor="red" ID="lbl_Error" Visible="false"></asp:Label>
    </div>
    
    <%--Footer On Login Page--%>
    <div class="footer">
    </div>
    <script type="text/javascript" src="assets/js/login/index.js"></script>
</body>
</html>
