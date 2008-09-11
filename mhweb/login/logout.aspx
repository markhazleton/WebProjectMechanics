<%@ Page Language="VB" Debug="True" CodePage="20127" LCID="1033" %>
<%@ OutputCache Location="None" %>
<script language="VB" runat="server">
	'*********************
	'  Page Load Handler
	'*********************
	Private Sub Page_Load(ByVal s As System.Object, ByVal e As System.EventArgs)
		FormsAuthentication.Signout ' Logout
		Session.Abandon() ' Clear Session Variables
		response.redirect("/")
	End Sub
</script>
