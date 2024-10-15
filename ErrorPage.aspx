<%@ Page Language="C#"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html>
<head runat="server">
    
    <%--ErrorDetails On Title Of Page--%>
	<title>500 - Internal Server Error</title>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">
    
    <%--External Css Style For Error Page--%>
	<link rel="stylesheet" href="assets/Errorpage/css/style.css">
</head>
<body>
<form id="form1" runat="server">
	<div class="wrap">
        
        <%--Heading--%>
		<h1>OOPS!</h1>
		<div class="banner">
            <%--Image Link in ErrorPage--%>
			<img src="assets/Errorpage/images/500.png" alt="" />
		</div>
		<div class="page">
            <%--Error Detail on Body--%>
			<h2>Internal Server Error</h2>
		</div>
        
        <%--Footer With Redirect to Login function--%>
		<div class="footer">
			<input action="action" style="background-color:gray;color:white;border-radius:15%;cursor:pointer" type="button" value="Back" onclick="window.history.go(-1)" />
		</div>
	</div>
	
	</form>
</body>
</html>
