<%@ Page Language="C#"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html>
<head runat="server">
	<title>404 - Page Not Found</title>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">
	<link rel="stylesheet" href="assets/Errorpage/css/style.css">
</head>
<body>
<form id="form1" runat="server">
	<div class="wrap">
        <%--Title--%>
		<h1>OOPS!</h1>
		<div class="banner">
			<img src="assets/Errorpage/images/banner.png" alt="" />
		</div>
		<div class="page">
            <%--Detail Of Error Type--%>
			<h2>Page Not Found!</h2>
		</div>
		<div class="footer">
            <%--Footer With Redirect to Login Page--%>
			<input action="action" style="background-color:gray;color:white;border-radius:15%;cursor:pointer" type="button" class="btn btn-default" value="Go To Login" onclick="Redirect()" />
		</div>
	</div>
	
	</form>
</body>
<%--Javascript Function To Redirect to Login Page--%>
 <script type="text/javascript">
        function Redirect() {
            window.location = "Login";
        }
    </script>
</html>
