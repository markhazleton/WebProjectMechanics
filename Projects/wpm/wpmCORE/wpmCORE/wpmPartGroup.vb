Imports System.Web
Imports System.Data.OleDb

Public Class wpmPartGroup
    Private _ID As String
    Public Property ID() As String
        Get
            Return _ID
        End Get
        Set(ByVal value As String)
            _ID = value
        End Set
    End Property
    Private _Name As String
    Public Property Name() As String
        Get
            Return _Name
        End Get
        Set(ByVal value As String)
            _Name = value
        End Set
    End Property
    Private _Description As String
    Public Property Description() As String
        Get
            Return _Description
        End Get
        Set(ByVal value As String)
            _Description = value
        End Set
    End Property
    Private _ParentID As String
    Public Property ParentID() As String
        Get
            Return _ParentID
        End Get
        Set(ByVal value As String)
            _ParentID = value
        End Set
    End Property
    Private _PageID As String
    Public Property PageID() As String
        Get
            Return _PageID
        End Get
        Set(ByVal value As String)
            _PageID = value
        End Set
    End Property
    Private _LinkCount As Integer
    Public Property LinkCount() As Integer
        Get
            Return _LinkCount
        End Get
        Set(ByVal value As Integer)
            _LinkCount = value
        End Set
    End Property
End Class
Public Class wpmPartGroupList
    Inherits List(Of wpmPartGroup)

    Public Sub PopulateLinkCategoryList(ByVal CompanyID As String)
        Try
            For Each myrow As DataRow In wpmDataCon.GetLinkCategoryList(CompanyID).Rows
                Dim MyLinkCategory As New wpmPartGroup
                MyLinkCategory.ID = wpmUTIL.GetDBString(myrow("ID"))
                MyLinkCategory.Name = wpmUTIL.GetDBString(myrow("Title"))
                MyLinkCategory.Description = wpmUTIL.GetDBString(myrow("Description"))
                MyLinkCategory.ParentID = wpmUTIL.GetDBString(myrow("ParentID"))
                MyLinkCategory.PageID = wpmUTIL.GetDBString(myrow("PageID"))
                MyLinkCategory.LinkCount = wpmUTIL.GetDBInteger(myrow("CountOfID"))
                Me.Add(MyLinkCategory)
            Next
        Catch ex As Exception
            wpmUTIL.AuditLog("Error on mhLinkCategoryList.New(ByVal CompanyID As String)", ex.ToString)
        End Try
    End Sub

End Class

Public Class wpmLinkDirectory
    Public MyStringBuilder As StringBuilder = New StringBuilder
    Private RequestCID As String
    Private RequestLID As String
    Private RequestAction As String
    Private bRequestSubmit As Boolean
    Private errorMSG As String = ""
    Private ActiveSite As wpmActiveSite
    Private myLink As New wpmPart
    Private ipage As Double

    Private ReturnURL As String

    Sub New(ByRef thisActiveSite As wpmActiveSite)
        ActiveSite = thisActiveSite
        RequestCID = ActiveSite.GetProperty("cid", "")
        RequestLID = ActiveSite.GetProperty("id", "")
        RequestAction = ActiveSite.GetProperty("md", "")
        ipage = Val(ActiveSite.GetProperty("page", ""))

        ReturnURL = "/default.aspx?c=" & Me.ActiveSite.CurrentPageID()

        If ActiveSite.GetProperty("submit", "") <> "" Then
            bRequestSubmit = True
            myLink.LinkTitle = wpmUTIL.FormatTextEntry(HttpContext.Current.Request("textname"))
            myLink.LinkURL = wpmUTIL.FormatTextEntry(HttpContext.Current.Request("texturl"))
            myLink.LinkDescription = wpmUTIL.FormatTextEntry(HttpContext.Current.Request("textdesc"))
            Dim arrCat As System.Array
            arrCat = Split(HttpContext.Current.Request("slist"), "~")
            'myLink.LinkCategoryID = arrCat(0)
            'myLink.PageID = arrCat(1)
            myLink.LinkTypeCD = HttpContext.Current.Request("linktype")
            myLink.LinkASIN = wpmUTIL.FormatTextEntry(HttpContext.Current.Request("textasin"))
            myLink.ModifiedDT = System.DateTime.Now()
        Else
            myLink.LinkRank = 0
            myLink.AmazonIndex = ""
            myLink.LinkID = ""
            myLink.LinkTitle = ""
            myLink.LinkDescription = ""
            myLink.LinkTypeCD = ""
            myLink.LinkURL = ""
            myLink.PageID = Me.ActiveSite.CurrentPageID()
            myLink.LinkCategoryID = RequestCID
            myLink.ModifiedDT = System.DateTime.Now()

            bRequestSubmit = False
        End If
    End Sub
    Sub CreateAdminLinkDirectory(ByRef mySiteMap As wpmActiveSite)
        Dim UnApprovedCount As String = ""
        Dim mySQL As String = ""
        Select Case RequestAction
            Case "add"
                If bRequestSubmit Then
                    errorMSG = AddNewLink(mySiteMap.CompanyID)
                End If
                DrawLinkEntryForm(mySiteMap.CompanyID)
            Case "del"
                If wpmUser.IsAdmin() Then
                    If RequestLID <> "" Then
                        mySQL = "DELETE FROM Link WHERE ID=" & RequestLID & " and CompanyID=" & mySiteMap.CompanyID & " "
                        wpmDB.RunDeleteSQL(mySQL, mySiteMap.sitedb)
                        HttpContext.Current.Response.Redirect(ReturnURL & "&cid=" & RequestCID)
                    End If
                Else
                    HttpContext.Current.Response.Redirect(ReturnURL & "&cid=" & RequestCID)
                End If
            Case "edit"
                If wpmUser.IsAdmin() Then
                    errorMSG = UpdateLink()
                End If
                DrawLinkEntryForm(mySiteMap.CompanyID)
            Case Else
                DisplayAdminLinkDirectory()
        End Select
    End Sub
    Sub CreateLinkDirectory(ByRef mySiteMap As wpmActiveSite)
        Dim UnApprovedCount As String = ""
        Dim mySQL As String = ""
        Select Case RequestAction
            Case "add"
                If bRequestSubmit Then
                    errorMSG = AddNewLink(mySiteMap.CompanyID)
                End If
                DrawLinkEntryForm(mySiteMap.CompanyID)
            Case "del"
                If wpmUser.IsAdmin() Then
                    If RequestLID <> "" Then
                        mySQL = "DELETE FROM Link WHERE ID=" & RequestLID & " and CompanyID=" & mySiteMap.CompanyID & " "
                        wpmDB.RunDeleteSQL(mySQL, mySiteMap.sitedb)
                        HttpContext.Current.Response.Redirect(ReturnURL & "&cid=" & RequestCID)
                    End If
                Else
                    HttpContext.Current.Response.Redirect(ReturnURL & "&cid=" & RequestCID)
                End If
            Case "edit"
                If wpmUser.IsAdmin() Then
                    errorMSG = UpdateLink()
                End If
                DrawLinkEntryForm(mySiteMap.CompanyID)
            Case Else
                DisplayLinkDirectory()
        End Select
    End Sub
    Sub ECHO(ByVal MSG As String)
        MyStringBuilder.Append(MSG & vbNewLine)
    End Sub
    Sub DisplayAdminLinkDirectory()
        ReturnURL = "/wpm/admin/AdminLink.aspx?c=" & Me.ActiveSite.CurrentPageID
        ECHO("<h1>Parts By Category</h1>")
        ECHO("<ul>")
        ECHO("<li><span class=""highlight-one"">Site Type (Content appears on every page)</span></li>")
        ECHO("<li><span class=""highlight-two"">Site Cateogry (Content appears only for a single category)</span></li>")
        ECHO("<li><span class=""highlight-three"">Company (Content appears on every page)</span></li>")
        ECHO("<li><span class=""highlight-four"">Page (Content appears on single page)</span></li>")
        ECHO("</ul>")
        ECHO("<div>")
        DrawAdminCategory()
        DrawAdminLinks(RequestCID)
        ECHO("</div><br/>")
    End Sub
    Sub DisplayLinkDirectory()
        Dim unapprovedcount As New Integer
        Dim PageCategoyCount As Integer = rootCategory()
        If PageCategoyCount > 1 Then
            DrawSubCategory()
        End If
        DrawLinks(PageCategoyCount)
    End Sub
    Sub DrawLinkEntryForm(ByVal CompanyID As String)
        ECHO("<TABLE border=""0"" align=""left"" CELLPADDING=""0"" CELLSPACING=""0"" >")
        ECHO("<TR><TD>" & errorMSG & "&nbsp;</TD></TR>")
        ECHO("<TR>")
        ECHO("<TD><b>Add Link</B><a href=""" & App.Config.wpmWebHome() & "link/default.aspx?cid=" & RequestCID & """>Return to Category</a></TD>")
        ECHO("</TR>")
        ECHO("<TR>")
        ECHO("<TD>")
        ECHO("<FORM ACTION=""default.aspx?md=add&cid=" & RequestCID & """ METHOD=""POST"">")
        ECHO("<DIV ALIGN=""CENTER"">")
        ECHO("<TABLE border=""0"" CELLPADDING=""5"" CELLSPACING=""5"" >")
        ECHO("<TR>")
        ECHO("<TD width=""100"">Link Name</TD>")
        ECHO("<TD><INPUT TYPE=""TEXT"" SIZE=""40"" NAME=""textname"" value=""" & myLink.LinkTitle & """ ></TD>")
        ECHO("</TR>")
        ECHO("<TR>")
        ECHO("<TD width=""100"">URL</TD>")
        ECHO("<TD><INPUT TYPE=""TEXT"" SIZE=""40"" NAME=""texturl"" value=""" & myLink.LinkURL & """></TD>")
        ECHO("</TR>")
        ECHO("<TR>")
        ECHO("<TD width=""100"">Description</TD>")
        ECHO("<TD><TEXTAREA COLS=""30"" ROWS=""10"" NAME=""textdesc"">" & myLink.LinkDescription & "</TEXTAREA></TD>")
        ECHO("</TR>")
        ECHO("<TR>")
        ECHO("<TD width=""100"">Link Category</TD>")
        ECHO("<TD>")
        Call BuildCategory()
        ECHO("</TD>")
        ECHO("</TR>")
        ECHO("<TR>")
        ECHO("<TD width=""100"">Link Type</TD>")
        ECHO("<TD>")
        Call BuildLinkType("HTTP")
        ECHO("</TD>")
        ECHO("</TR>")
        ECHO("<TR>")
        ECHO("<TD width=""100"">Amazon ASIN</TD>")
        ECHO("<TD><INPUT TYPE=""TEXT"" SIZE=""40"" NAME=""textasin"" VALUE=""" & myLink.LinkASIN & """></TD>")
        ECHO("</TR>")
        ECHO("<TR>")
        ECHO("<TD colspan=""2""><br /><INPUT TYPE=""SUBMIT"" VALUE=""Save New Link"" NAME=""submit""><br /></TD>")
        ECHO("</TR>")
        ECHO("</TABLE>")
        ECHO("</DIV>")
        ECHO("</FORM>")
        ECHO("</TD>")
        ECHO("</TR>")
        ECHO("</TABLE>")
    End Sub
    ' ########################
    ' // Root Category
    ' ########################
    Private Function rootCategory() As Integer
        Dim iCategoryCount As Integer = 0
        Dim sReturn As String = ("")
        For Each myCatrow As wpmPartGroup In Me.ActiveSite.LinkCategoryList
            If myCatrow.PageID = Me.ActiveSite.CurrentPageID() Then
                iCategoryCount = iCategoryCount + 1
                If myCatrow.ID = RequestCID Then
                    sReturn = "<A href=""" & ReturnURL & "&cid=" & myCatrow.ID & """>" & myCatrow.Name & "</A> -> " & sReturn
                    sReturn = ParentCategory(myCatrow.ParentID) & sReturn
                End If
            End If
        Next
        If RequestCID <> "" Then
        End If
        sReturn = "<h2><A href=""" & ReturnURL & """>" & Me.ActiveSite.GetCurrentPageName() & "</A> --> " & sReturn & "</h2>"
        ECHO(sReturn)
        Return iCategoryCount
    End Function

    Function ParentCategory(ByVal ParentID As String) As String
        Dim pcReturn As String = ""
        For Each myCatrow As wpmPartGroup In Me.ActiveSite.LinkCategoryList
            If myCatrow.ID = ParentID Then
                pcReturn = "<A href=""" & ReturnURL & "&cid=" & myCatrow.ID & """>" & myCatrow.Name & "</A> > " & pcReturn
                pcReturn = ParentCategory(myCatrow.ParentID) & pcReturn
            End If
        Next
        Return pcReturn
    End Function
    ' ########################
    ' // Unapproved Links
    ' ########################
    Function UnapprovedLinks(ByVal CurrentPageID As Integer, ByRef mySiteMap As wpmActiveSite) As Integer
        Dim sqlwrk As String
        Dim linksCount As Integer = 0
        sqlwrk = "SELECT COUNT(*) as LinkCount FROM Link WHERE Views=NO and PageID=" & CurrentPageID & " "
        For Each row As DataRow In wpmDB.GetDataTable(sqlwrk, "UnapprovedLinks").Rows
            linksCount = CInt(row.Item("LinkCount"))
            Exit For
        Next
        Return linksCount
    End Function
    ' ########################
    ' // Count Links
    ' ########################
    Function CategoryLinkCount(ByVal CatID As String) As Double
        Dim dReturn As Double = 0
        For Each myCatrow As wpmPartGroup In Me.ActiveSite.LinkCategoryList
            If myCatrow.ID = CatID Then
                dReturn = myCatrow.LinkCount
                dReturn = dReturn + ChildCategoryLinkCount(myCatrow.ID)
            End If
        Next
        Return dReturn
    End Function
    '
    '  Recursive Call For Children to get total count of Links
    '
    Function ChildCategoryLinkCount(ByVal ParentID As String) As Double
        Dim dReturn As Double = 0
        For Each myCatrow As wpmPartGroup In Me.ActiveSite.LinkCategoryList
            If myCatrow.ParentID = ParentID Then
                dReturn = dReturn + myCatrow.LinkCount
                If myCatrow.ID = myCatrow.ParentID Then
                    wpmUTIL.AuditLog("Category Parent same as Self - " & myCatrow.ID & " - " & myCatrow.Description, "wpmPartGroup.ChildCategoryLinkCount")
                Else
                    dReturn = dReturn + ChildCategoryLinkCount(myCatrow.ID)
                End If
            End If
        Next
        Return dReturn

    End Function

    ' ########################
    ' // Delete Categorys Links
    ' ########################
    Private Function DeleteCat(ByVal intCategoryID As String, ByVal CurrentPageID As String) As Boolean
        Dim sqlwrk As String
        Dim j As Integer = 0
        sqlwrk = "DELETE FROM Link WHERE CategoryID=" & intCategoryID & " and PageID=" & CurrentPageID & " "
        wpmDB.RunDeleteSQL(sqlwrk, ActiveSite.sitedb)
        sqlwrk = "DELETE FROM LinkCategory WHERE ID=" & intCategoryID & " and PageID=" & CurrentPageID & " "
        wpmDB.RunDeleteSQL(sqlwrk, ActiveSite.sitedb)
        sqlwrk = "DELETE FROM LinkRank WHERE CateID=" & intCategoryID & " and PageID=" & CurrentPageID & " "
        wpmDB.RunDeleteSQL(sqlwrk, ActiveSite.sitedb)
        sqlwrk = "SELECT ID FROM LinkCategory WHERE ParentID=" & intCategoryID & " and PageID=" & CurrentPageID & " "
        For Each row As DataRow In wpmDB.GetDataTable(sqlwrk, "DeleteCat - " & intCategoryID & " ").Rows
            DeleteCat(row.Item("ID").ToString, CurrentPageID)
        Next
        Return True
    End Function

    ' #######################
    ' // Checking User Name
    ' #######################
    Function CheckUserName(ByVal UN2Check As String) As Boolean
        Dim i As Integer
        CheckUserName = True
        If Len(UN2Check) > 3 Then
            If Not CBool(isLetter(Mid(UN2Check, 1, 1))) Then
                CheckUserName = False
            End If

            If Not CBool(isLetter(Mid(UN2Check, Len(UN2Check), 1))) And Not CBool(IsNumeric(Mid(UN2Check, Len(UN2Check), 1))) Then
                CheckUserName = False
            End If

            Dim oneLetter As String
            For i = 1 To Len(UN2Check) - 1
                oneLetter = Mid(UN2Check, i + 1, 1)
                If Not CBool(isLetter(oneLetter)) And Not CBool(IsNumeric(oneLetter)) And oneLetter <> "_" And oneLetter <> "-" Then
                    CheckUserName = False
                    Exit Function
                End If
            Next
        Else
            CheckUserName = False
        End If
    End Function

    ' ######################
    ' // Image of rank
    ' ######################
    Function Rankimg(ByVal intRank As Integer) As String
        If intRank = 0 Then
            Rankimg = "<img border=""0"" alt=""This link is ranked a " & intRank.ToString & """ src=""" & App.Config.wpmWebHome() & "link/image/rank0.gif"" width=""40"" height=""6"">"
        Else
            Rankimg = "<img border=""0"" alt=""This link is ranked a " & intRank.ToString & """ src=""" & App.Config.wpmWebHome() & "link/image/rank" & intRank.ToString & ".gif"" width=""40"" height=""6"">"
        End If
    End Function

    ' #######################
    ' // Checking Email
    ' #######################
    Function CheckEmail(ByVal E2Check As String) As Boolean
        Dim emailID As String
        Dim emailHost As String
        If Len(E2Check) > 5 Then
            If InStr(1, E2Check, "@") > 0 Then
                emailID = Left(E2Check, InStr(1, E2Check, "@") - 1)
                emailHost = Right(E2Check, Len(E2Check) - Len(emailID) - 1)
                CheckEmail = CheckUserName(emailID)
            Else
                CheckEmail = False
            End If
        Else
            CheckEmail = False
        End If
    End Function

    ' #######################
    ' // Checking Is It Between a-Z
    ' #######################
    Function isLetter(ByVal L2Check As String) As Boolean
        If Asc((L2Check.ToUpper)) > 64 And Asc((L2Check.ToUpper)) < 91 Then
            isLetter = True
        Else
            isLetter = False
        End If
    End Function

    Function CheckURL(ByVal U2Check As String, ByVal Ltypecode As String) As Boolean
        Dim bCheckURL As Boolean = False
        If (Ltypecode = "NAV" Or Ltypecode = "FILE") Then
            bCheckURL = True
        Else
            If Len(U2Check) > 10 Then
                If (Left(U2Check.ToLower, 7)) = "http://" Or (Left(U2Check.ToLower, 6)) = "ftp://" Then
                    If InStr(1, U2Check, ".") > 0 Then
                        bCheckURL = True
                    End If
                End If
            End If
        End If
        CheckURL = bCheckURL
    End Function


    Function MapURL(ByRef path As String) As String
        Dim rootPath As String
        Dim url As String
        'Convert a physical file path to a URL for hypertext links.
        rootPath = HttpContext.Current.Server.MapPath("/")
        url = Right(path, Len(path) - Len(rootPath))
        MapURL = Replace(url, "\", "/")
    End Function


    ' #######################
    ' // Draw Sub Category
    ' #######################
    Sub DrawSubCategory()
        Dim i As Integer = 0
        Dim sbRight As New StringBuilder
        Dim sbLeft As New StringBuilder
        For Each myCat As wpmPartGroup In Me.ActiveSite.LinkCategoryList
            If myCat.ParentID = RequestCID And myCat.PageID = Me.ActiveSite.CurrentPageID() Then
                If wpmUTIL.IsBreak(i, 2) Then
                    sbRight.Append("<p><b><A href=""" & ReturnURL & "&cid=" & myCat.ID & """>" & myCat.Name & "</A></B> (" & CategoryLinkCount(myCat.ID).ToString & ") <br />")
                    If wpmUser.IsAdmin() Then
                        sbRight.Append("<A href=""" & ReturnURL & "&cid=" & myCat.ID & "&md=edit"">Edit</A> | <A href=""" & ReturnURL & "&cid=" & myCat.ID & "&md=DEL"""">Delete Category</A><br />")
                    End If
                    sbRight.Append("<br>" & myCat.Description & "</p>")
                Else
                    sbLeft.Append("<p><b><A href=""" & ReturnURL & "&cid=" & myCat.ID & """>" & myCat.Name & "</A></B> (" & CategoryLinkCount(myCat.ID).ToString & ") <br />")
                    If wpmUser.IsAdmin() Then
                        sbLeft.Append("<A href=""" & ReturnURL & "&cid=" & myCat.ID & "&md=edit"">Edit</A> | <A href=""" & ReturnURL & "&cid=" & myCat.ID & "&md=DEL"""">Delete Category</A><br />")
                    End If
                    sbLeft.Append("<br>" & myCat.Description & "</p>")
                End If
                i = i + 1
            End If
        Next
        ECHO("<div class=""yui-g"">")
        ECHO("<div class=""yui-u first"" >")
        ECHO(sbLeft.ToString)
        ECHO("</div>")
        ECHO("<div class=""yui-u "" >")
        ECHO(sbRight.ToString)
        ECHO("</div>")
        ECHO("</div>")
    End Sub
    Sub DrawAdminCategory()
        Dim i As Integer = 0
        Dim sbRight As New StringBuilder
        Dim sbLeft As New StringBuilder
        For Each myCat As wpmPartGroup In Me.ActiveSite.LinkCategoryList
            If wpmUTIL.IsBreak(i, 2) Then
                sbRight.Append("<div class=""box""><p><b><A href=""" & ReturnURL & "&cid=" & myCat.ID & """>" & myCat.Name & "</A></B> (" & CategoryLinkCount(myCat.ID).ToString & ") <br />")
                sbRight.Append("<A href=""/wpmgen/linkcategory_edit.aspx?ID=" & myCat.ID & """>Edit</A> | <A href=""/wpmgen/linkcategory_delete.aspx?ID=" & myCat.ID & """>Delete Category</A><br />")
                sbRight.Append("</p>" & myCat.Description & "<br/>" & GetCategoryLinks(myCat.ID) & "</div>")
            Else
                sbLeft.Append("<div class=""box""><p><b><A href=""" & ReturnURL & "&cid=" & myCat.ID & """>" & myCat.Name & "</A></B> (" & CategoryLinkCount(myCat.ID).ToString & ") <br />")
                sbLeft.Append("<A href=""/wpmgen/linkcategory_edit.aspx?ID=" & myCat.ID & """>Edit</A> | <A href=""/wpmgen/linkcategory_delete.aspx?ID=" & myCat.ID & """>Delete Category</A><br />")
                sbLeft.Append("</p>" & myCat.Description & "<br/>" & GetCategoryLinks(myCat.ID) & "</div>")
            End If
            i = i + 1
        Next
        ECHO("<div class=""yui-g"">")
        ECHO("<div class=""yui-u first"" >")
        ECHO(sbLeft.ToString)
        ECHO("</div>")
        ECHO("<div class=""yui-u "" >")
        ECHO(sbRight.ToString)
        ECHO("</div>")
        ECHO("</div>")
    End Sub
    Sub DrawAdminLinks(ByVal CurrentCID As String)
        ECHO("<hr>")
        For Each myLink As wpmPart In Me.ActiveSite.PartList
            If myLink.LinkCategoryID = CurrentCID Then
                If Left(myLink.PageID, 3) = "CAT" Then
                    ECHO("<span style=""background-color:yellow;color:red;"">" & myLink.LinkTitle & "</span>")
                    ECHO("  [<A href=""/wpmgen/SiteLink_edit.aspx?id=" & myLink.LinkID & """>Edit</A> | <A href=""/wpmgen/SiteLink_delete.aspx?id=" & myLink.LinkID & "&cid=" & RequestCID & """> Delete</A>]<br>")
                Else
                    ECHO(myLink.LinkTitle)
                    ECHO("  [<A href=""/wpmgen/link_edit.aspx?id=" & myLink.LinkID & """>Edit</A> | <A href=""/wpmgen/link_delete.aspx?id=" & myLink.LinkID & "&cid=" & RequestCID & """> Delete</A>]<br>")
                End If
            End If
        Next
    End Sub

    Private Function GetCategoryLinks(ByVal CurrentCID As String) As String
        Dim sbLinks As New StringBuilder("<ul>")
        Dim myLocation As wpmLocation
        Dim sLocationInfo As String = String.Empty
        For Each myPart As wpmPart In Me.ActiveSite.PartList
            If myPart.LinkCategoryID = CurrentCID Then
                If myPart.PageID <> String.Empty Then
                    myLocation = Me.ActiveSite.LocationList.FindLocation(myPart.PageID, "")
                    sLocationInfo = myPart.LinkRank & " - " & myPart.LinkTitle & " [" & myLocation.PageName & "] "
                Else
                    sLocationInfo = myPart.LinkRank & " - " & myPart.LinkTitle
                End If

                If Not myPart.View Then
                    sLocationInfo = "(i) " & sLocationInfo
                End If

                If myPart.LinkSource = "SiteLink" Then
                    If myPart.PageID = String.Empty Then
                        sbLinks.Append("<li><span class=""highlight-one"">" & sLocationInfo & "</span>")
                    Else
                        sbLinks.Append("<li><span class=""highlight-two"">" & sLocationInfo & "</span>")
                    End If
                    sbLinks.Append(" [ <a href=""/wpmgen/SiteLink_edit.aspx?id=" & myPart.LinkID & """>Edit</a> ")
                    sbLinks.Append("| <a href=""/wpmgen/SiteLink_delete.aspx?id=" & myPart.LinkID & "&cid=" & RequestCID & """> Delete</a> ")
                    sbLinks.Append("| <a href=""/wpmgen/SiteLink_add.aspx?id=" & myPart.LinkID & """>Copy</a> ]</li>")
                Else
                    If myPart.PageID = String.Empty Then
                        sbLinks.Append("<li><span class=""highlight-three"">" & sLocationInfo & "</span>")
                    Else
                        sbLinks.Append("<li><span class=""highlight-four"">" & sLocationInfo & "</span>")
                    End If
                    sbLinks.Append(" [ <a href=""/wpmgen/Link_edit.aspx?id=" & myPart.LinkID & """>Edit</a> ")
                    sbLinks.Append("| <a href=""/wpmgen/Link_delete.aspx?id=" & myPart.LinkID & "&cid=" & RequestCID & """> Delete</a> ")
                    sbLinks.Append("| <a href=""/wpmgen/Link_add.aspx?id=" & myPart.LinkID & """>Copy</a> ]</li>")
                End If
            End If
        Next
        sbLinks.Append("</ul>")
        Return sbLinks.ToString
    End Function

    Sub DrawLinks(ByVal PageCategoryCount As Integer)
        Dim j As Integer = 0

        Dim lURL As String = ""
        Dim i As Double = 0
        Dim iRecordCount As Integer = 0
        Dim tPage As String = ""
        Dim iLinkCount As Integer = 0
        Dim iLinksPerPage As Double = 10
        If ipage = 0 Then
            ipage = 1
        End If
        If ipage = 1 Then
            i = 0
        Else
            i = (ipage - 1) * iLinksPerPage
        End If
        iLinkCount = 0
        ' Insert Amazon Links
        For Each myLink As wpmPart In Me.ActiveSite.PartList
            If Not IsNothing(myLink.PageID) Or Not IsDBNull(myLink.PageID) Then
                If myLink.PageID = Me.ActiveSite.CurrentPageID() And myLink.LinkCategoryID = RequestCID Then
                    j = j + 1
                    If (j >= i) And (j < i + iLinksPerPage) Then
                        If Not IsDBNull(myLink.AmazonIndex) Then
                            If Trim(myLink.AmazonIndex) & "*" <> "*" Then
                                ECHO(" &nbsp;<a target=""_blank"" href=""http://www.amazon.com/exec/obidos/ASIN/" & myLink.AmazonIndex & "/~~AmazonAssociatesTag~~?creative=327641&camp=14573&link_code=as1""><img border=""0"" src=""http://images.amazon.com/images/P/" & myLink.AmazonIndex & ".01.THUMBZZZ.jpg"" alt=""" & myLink.LinkURL & """></a>&nbsp; ")
                            End If
                        End If
                    End If
                End If
            End If
        Next
        If j > 0 Then
            ECHO("<br/>")
        End If
        j = 0
        For Each myLink As wpmPart In Me.ActiveSite.PartList
            If myLink.PageID = Me.ActiveSite.CurrentPageID() And myLink.LinkCategoryID = RequestCID Then
                j = j + 1
                If ((j >= i) And (j < i + iLinksPerPage) Or PageCategoryCount < 2) Then
                    If iLinkCount = 2 Then
                        iLinkCount = 0
                    End If
                    iLinkCount = iLinkCount + 1
                    If wpmUser.IsUser() Then
                        ECHO("<A href=""" & App.Config.wpmWebHome() & "link/rank.aspx?id=" & myLink.LinkID & "&cid=" & RequestCID & """>" & Rankimg(myLink.LinkRank) & "</A>")
                    Else
                        ECHO(Rankimg(myLink.LinkRank))
                    End If
                    ECHO("&nbsp;<A target=""_new"" href=""" & myLink.LinkURL & """>" & myLink.LinkTitle & "</A>")
                    If wpmUser.IsAdmin() Then
                        ECHO("  [<A href=""/wpmgen/link_edit.aspx?id=" & myLink.LinkID & """>Edit</A> |<A href=""/wpmgen/link_delete.aspx?id=" & myLink.LinkID & "&cid=" & RequestCID & """> Delete</A>]")
                    End If
                    ECHO("&nbsp;<em>" & myLink.LinkDescription & "</em><br />")
                End If
                iRecordCount = iRecordCount + 1
            End If
        Next

        If iLinkCount = 1 Then
        End If

        If iRecordCount > iLinksPerPage Then
            If Fix(iRecordCount / iLinksPerPage) < iRecordCount / iLinksPerPage Then
                tPage = (Fix(iRecordCount / iLinksPerPage) + 1).ToString
            Else
                tPage = Fix(iRecordCount / iLinksPerPage).ToString
            End If
            If tPage > "1" Then
                ECHO("<center><br /><br />Select Page ")
                For i = 1 To CInt(tPage)
                    If i = ipage Then
                        ECHO(i & " ")
                    Else
                        ECHO("<A href=""" & App.Config.wpmWebHome() & "link/default.aspx?cid=" & RequestCID & "&page=" & i & """>" & i & "</A> ")
                    End If
                Next
                ECHO("</center>")
            End If
        End If
    End Sub
    Sub DrawAmazonPictures(ByVal iLinksPerPage As Integer, ByVal i As Integer)
        Dim j As Integer = 0
        For Each myLink As wpmPart In Me.ActiveSite.PartList
            If Not IsNothing(myLink.PageID) Or Not IsDBNull(myLink.PageID) Then
                If myLink.PageID = Me.ActiveSite.CurrentPageID() And myLink.LinkCategoryID = RequestCID Then
                    j = j + 1
                    If (j >= i) And (j < i + iLinksPerPage) Then
                        If Not IsDBNull(myLink.AmazonIndex) Then
                            If Trim(myLink.AmazonIndex) & "*" <> "*" Then
                                ECHO("<a target=""_blank"" href=""http://www.amazon.com/exec/obidos/ASIN/" & myLink.AmazonIndex & "/thefrogsfolly-20?creative=327641&camp=14573&link_code=as1""><img border=""0"" src=""http://images.amazon.com/images/P/" & myLink.AmazonIndex & ".01.THUMBZZZ.jpg"" alt=""" & myLink.LinkURL & """></a>")
                            End If
                        End If
                    End If
                End If
            End If
        Next
        j = 0

    End Sub
    Sub DrawYUILinks(ByRef mySitemap As wpmActiveSite)
        Dim ipage As Integer = CInt(HttpContext.Current.Request("page"))
        Dim iRecordCount As Integer = 0
        Dim tPage As Integer = 0
        Dim iLinkCount As Integer = 0
        Dim iLinksPerPage As Integer = 24
        Dim i As Integer = 0
        Dim j As Integer = 0
        If ipage = 0 Then
            ipage = 1
        End If
        If ipage = 1 Then
            i = 0
        Else
            i = (ipage - 1) * iLinksPerPage
        End If
        iLinkCount = 0
        Dim sbRight As New StringBuilder
        Dim sbLeft As New StringBuilder

        ECHO("<div class=""yui-g""><div class=""yui-u first"">")
        For Each myLink As wpmPart In mySitemap.PartList
            If myLink.PageID = mySitemap.CurrentPageID() Then
                j = j + 1
                If (j >= i) And (j < i + iLinksPerPage) Then
                    iLinkCount = iLinkCount + 1
                    'If mhwcm.IsBreak(iLinkCount, 2) Then
                    '    ECHO(iLinkCount & "-ODD<br/>")
                    'Else
                    '    ECHO(iLinkCount & "-EVEN<br/>")
                    'End If
                    ECHO("<div><A target=""_new"" href=""" & myLink.LinkURL & """>" & myLink.LinkTitle & "</A>")
                    If wpmUser.IsAdmin() Then
                        ECHO("  [<A href=""/wpmgen/link_edit.aspx?id=" & myLink.LinkID & """>Edit</A> |<A href=""/wpmgen/link_delete.aspx?id=" & myLink.LinkID & "&cid=" & RequestCID & """> Delete</A>]")
                    End If
                    ECHO("<br/><em>" & myLink.LinkDescription & "</em><br /></div>")
                End If
                iRecordCount = iRecordCount + 1
            End If
        Next

        If iLinkCount = 1 Then
            ECHO("")
        End If
        ECHO("</div><div class=""yui-u""><br/></div></div>")

        If iRecordCount > iLinksPerPage Then
            If Fix(iRecordCount / iLinksPerPage) < iRecordCount / iLinksPerPage Then
                tPage = CInt(Fix(iRecordCount / iLinksPerPage) + 1)
            Else
                tPage = CInt(Fix(iRecordCount / iLinksPerPage))
            End If
            If tPage > 1 Then
                ECHO("<center><br /><br />Select Page ")
                For i = 1 To tPage
                    If i = ipage Then
                        ECHO(i & " ")
                    Else
                        ECHO("<A href=""" & App.Config.wpmWebHome() & "link/default.aspx?cid=" & RequestCID & "&page=" & i & """>" & i & "</A> ")
                    End If
                Next
                ECHO("</center>")
            End If
        End If

    End Sub
    Sub TopRatedLinks(ByVal cateID As Integer, ByVal CurrentPageID As Integer)
        Dim sbAmazonLinks As New StringBuilder
        Dim myDT As DataTable
        Dim myASIN As String = ("")
        Dim lID As String = ""
        Dim ltitle As String = ""
        Dim lURL As String = ""
        Dim lranks As String = ""
        Dim ldescription As String = ""
        Dim sqlwrk As String = ("SELECT id,title,url,ranks,description,asin FROM Link WHERE Views=YES and PageID=" & CurrentPageID & " ORDER BY id DESC")
        Dim myRowCount As Integer = 0
        myDT = wpmDB.GetDataTable(sqlwrk, "TopRatedLinks")
        If myDT.Rows.Count > 15 Then
            For TopNumber As Integer = 0 To 15
                myASIN = myDT.Rows.Item(TopNumber).Item("asin").ToString
                lURL = myDT.Rows.Item(TopNumber).Item("url").ToString
                If Trim(myDT.Rows.Item(TopNumber).Item("asin").ToString) <> "" Then
                    sbAmazonLinks.Append("<a target=""_blank"" href=""http://www.amazon.com/exec/obidos/ASIN/" & myASIN & _
                          "/thefrogsfolly-20?creative=327641&camp=14573&link_code=as1""><img border=""0"" " & _
                          "src=""http://images.amazon.com/images/P/" & myASIN & ".01.THUMBZZZ.jpg"" alt=""" & lURL & """></a>")
                End If
            Next
        Else
            For Each row As DataRow In myDT.Rows
                myASIN = row.Item("asin").ToString
                lURL = row.Item("url").ToString
                If Trim(row.Item("asin").ToString) <> "" Then
                    sbAmazonLinks.Append("<a target=""_blank"" href=""http://www.amazon.com/exec/obidos/ASIN/" & myASIN & _
                          "/thefrogsfolly-20?creative=327641&camp=14573&link_code=as1""><img border=""0"" " & _
                          "src=""http://images.amazon.com/images/P/" & myASIN & ".01.THUMBZZZ.jpg"" alt=""" & lURL & """></a>")
                End If
            Next
        End If
        ECHO(sbAmazonLinks.ToString)
        For myRow As Integer = 0 To myDT.Rows.Count - 1
            If myRow < 15 Then
                lID = myDT.Rows.Item(myRow).Item(0).ToString
                ltitle = myDT.Rows.Item(myRow).Item(1).ToString
                lURL = myDT.Rows.Item(myRow).Item(2).ToString
                lranks = myDT.Rows.Item(myRow).Item(3).ToString
                ldescription = myDT.Rows.Item(myRow).Item(4).ToString
                ECHO("<TABLE border=""0"" CELLPADDING=""2"" CELLSPACING=""2"" >")
                ECHO("<TR>")
                If wpmUser.IsUser() Then
                    ECHO("<TD width=""50"" VALIGN=""TOP""><A href=""" & App.Config.wpmWebHome() & "link/rank.aspx?id=" & lID & "&cid=" & cateID & """>" & Rankimg(CInt(lranks)) & "</A></TD>")
                Else
                    ECHO("<TD width=""50"" VALIGN=""TOP""><br />" & Rankimg(CInt(lranks)) & "</TD>")
                End If
                ECHO("<TD><A target=""_new"" href=""" & lURL & """>" & ltitle & "</A>")
                If wpmUser.IsAdmin() Then
                    ECHO("  [<A href=""" & App.Config.wpmWebHome() & "link/default.aspx?cid=" & cateID & "&md=edit&id=" & lID & """>Edit</A> |<A href=""" & App.Config.wpmWebHome() & "link/default.aspx?cid=" & cateID & "&md=del&id=" & lID & """> Delete</A>]")
                End If
                ECHO("&nbsp;<em>" & ldescription & "</em>")
                ECHO("</TD>")
                ECHO("</TR>")
                ECHO("</TABLE>")
            Else
                Exit For
            End If
        Next
    End Sub

    Sub LinkAdmin(ByVal cateID As String, ByVal CompanyID As String)
        Dim sqlwrk As String = ("SELECT id,title,url,ranks,description,asin FROM Link WHERE PageID is null and CompanyID=" & CompanyID)
        ECHO("<hr/><h2>Uncategorized Links</h2>")
        For Each row As DataRow In wpmDB.GetDataTable(sqlwrk, "mhLinkDirectory.LinkAdmin").Rows
            ECHO("<TABLE border=""0"" CELLPADDING=""2"" CELLSPACING=""2"" >")
            ECHO("<TR>")
            ECHO("<TD width=""50"" VALIGN=""TOP"">")
            ECHO("<A href=""" & App.Config.wpmWebHome() & "link/rank.aspx?id=" & row.Item("id").ToString & "&cid=" & cateID & """>" & "</A></TD>")
            ECHO("<TD><A target=""_new"" href=""" & row.Item("url").ToString & """>" & row.Item("title").ToString & "</A>")
            ECHO("  [<A href=""" & App.Config.wpmWebHome() & "link/default.aspx?cid=" & cateID & "&md=edit&id=" & row.Item("id").ToString & """>Edit</A>")
            ECHO(" |<A href=""" & App.Config.wpmWebHome() & "link/default.aspx?cid=" & cateID & "&md=del&id=" & row.Item("id").ToString & """> Delete</A>]")
            ECHO("&nbsp;<em>" & row.Item("description").ToString & "</em>")
            ECHO("</TD>")
            ECHO("</TR>")
            ECHO("</TABLE>")
        Next
    End Sub

    ' #######################
    ' // Category List Box
    ' #######################
    Sub BuildCategory()
        ECHO("<SELECT NAME=""slist"">")
        ECHO("<OPTION value=''>Please Select</OPTION>")
        BuildCategoryList("", "", "")
        ECHO("</SELECT>")
    End Sub

    Sub BuildCategoryList(ByVal DefaultParentCat As String, ByVal CurrentCatID As String, ByVal extrastr As String)
        Dim tmpstra As String = ""
        Dim j As Integer = 0
        For Each myCategoryList As wpmPartGroup In Me.ActiveSite.LinkCategoryList
            If myCategoryList.PageID = Me.ActiveSite.CurrentPageID() Then
                ECHO("<OPTION VALUE='" & myCategoryList.ID & "~" & myCategoryList.Name & "'")
                ECHO(">" & myCategoryList.Name & "</option>")
            End If
        Next
    End Sub
    Sub BuildLinkType(ByVal CurrentLinkTypeCD As String)
        Dim sqlwrk As String = ""
        Dim mydt As DataTable
        mydt = wpmDB.GetDataTable("select linktypecd,linktypedesc from linktype where linktypetarget=""_blank""", "mhLinkDirectory.BuildLinkType")

        ECHO("<SELECT NAME=""linktype"">")
        ECHO("<OPTION value=''>Please Select</OPTION>")
        For Each row As DataRow In mydt.Rows
            ECHO("<OPTION VALUE='" & row(0).ToString & "'")
            If row(0).ToString = CurrentLinkTypeCD Then
                ECHO(" selected")
            End If
            ECHO(">" & row(1).ToString & "</option>")
        Next
        ECHO("</SELECT>")
    End Sub
    Private Function AddNewLink(ByVal CompanyID As String) As String
        If myLink.LinkTitle = "" Then
            errorMSG = errorMSG & "Please insert link name.<br />"
        End If
        If Trim(myLink.LinkASIN & "*") <> "*" Then
            myLink.LinkURL = "http://www.amazon.com/exec/obidos/ASIN/" & myLink.LinkASIN & "/thefrogsfolly-20?creative=327641&camp=14573&link_code=as1"
        End If
        If Not CheckURL(myLink.LinkURL, myLink.LinkTypeCD) Then
            errorMSG = errorMSG & "Invalid URL.<br />"
        End If
        If wpmUser.IsAdmin() Then
            myLink.View = True
        Else
            myLink.View = False
        End If
        If errorMSG = "" Then
            If Not CreateLink(myLink, CompanyID) Then
                errorMSG = "Problem Saving New Link, Please contact system administrator."
            End If
            HttpContext.Current.Response.Redirect("~" & App.Config.wpmWebHome() & "link/default.aspx?cid=" & RequestCID)
        End If
        Return errorMSG
    End Function
    ' ** DB FUNCTIONS **
    Private Function GetLinkByLinkID(ByVal CompanyID As String) As wpmPart
        Dim tempLinkRow As New wpmPart
        Dim connection As New OleDbConnection(wpmConfig.ConnStr)
        connection.Open()
        Dim command As New OleDbCommand("SELECT id,title,description,url,categoryID,linktypecd,ASIN " & _
          "FROM Link WHERE ID= @RequestLID and CompanyID=@CompanyID", connection)
        command.Parameters.AddWithValue("@RequestLID", RequestLID)
        command.Parameters.AddWithValue("@CompanyID", CompanyID)
        Dim reader As OleDbDataReader = command.ExecuteReader()
        If reader.Read() Then
            tempLinkRow.LinkID = reader.GetString(0)
            tempLinkRow.LinkTitle = reader.GetString(1)
            tempLinkRow.LinkDescription = reader.GetString(2)
            tempLinkRow.LinkURL = reader.GetString(3)
            tempLinkRow.LinkCategoryID = reader.GetString(4)
            tempLinkRow.LinkTypeCD = reader.GetString(5)
            tempLinkRow.LinkASIN = reader.GetString(6)
        End If
        reader.Close()
        connection.Close()
        Return tempLinkRow
    End Function
    Private Function CreateLink(ByVal myLink As wpmPart, ByVal CompanyID As String) As Boolean
        Dim connection As New OleDbConnection(wpmConfig.ConnStr)
        connection.Open()
        Dim command As New OleDbCommand("INSERT INTO Link " _
        & "(title,description,url,categoryID,LinkTypeCD,companyID) " & _
        "VALUES (@title,@description,@url,@categoryID,@LinkTypeCD,@companyID)", connection)
        command.Parameters.AddWithValue("@title", myLink.LinkTitle)
        command.Parameters.AddWithValue("@descripton", myLink.LinkDescription)
        command.Parameters.AddWithValue("@url", myLink.LinkURL)
        command.Parameters.AddWithValue("@categoryID", myLink.LinkCategoryID)
        command.Parameters.AddWithValue("@LinkTypeCD", myLink.LinkTypeCD)
        command.Parameters.AddWithValue("@asin", myLink.LinkASIN)
        command.Parameters.AddWithValue("@companyID", CompanyID)
        Try
            Command.ExecuteNonQuery()
            Command.Parameters.Clear()
            Command.CommandText = "select @@IDENTITY"
            Dim newLinkID As String = command.ExecuteScalar().ToString
            connection.Close()
            myLink.LinkID = newLinkID
        Catch ex As Exception
            wpmUTIL.AuditLog(Command.CommandText.ToString, "ERROR CreateLink")
            Return False
        End Try
        Return True
    End Function
    Private Function UpdateLink() As String
        Dim arrCat As System.Array
        Dim CurrentLink As wpmPart = GetLinkByLinkID(Me.ActiveSite.CompanyID)
        If CInt(CurrentLink.LinkID) > 0 Then
            If bRequestSubmit Then
                If Len(myLink.LinkTitle) = 0 Then
                    errorMSG = "Link title is required<br />" & vbNewLine
                End If
                If Not CheckURL(myLink.LinkURL, myLink.LinkTypeCD) Then
                    errorMSG = errorMSG & "Invalid URL.<br />"
                End If
                If Not IsDBNull(myLink.LinkASIN) Then
                    myLink.LinkURL = "http://www.amazon.com/exec/obidos/ASIN/" & myLink.LinkASIN & "/thefrogsfolly-20?creative=327641&camp=14573&link_code=as1"
                End If
                If errorMSG = "" Then
                    arrCat = Split(HttpContext.Current.Request("slist"), "~")

                    myLink.LinkCategoryID = arrCat.GetValue(0).ToString
                    If UBound(arrCat) > 0 Then
                        myLink.PageID = arrCat.GetValue(1).ToString
                    End If
                    Dim connection As New OleDbConnection(wpmConfig.ConnStr)
                    connection.Open()
                    Dim command As New OleDbCommand("UPDATE Link " & _
                                  "SET title=@LinkTitle," & _
                                  "url=@LinkURL," & _
                                  "description=@LinkDS," & _
                                  "linktypecd=@LinkTypeCD," & _
                                  "categoryID=@LinkCategoryID," & _
                                  "pageID=@LinkPageID," & _
                                  "asin=@LinkASIN " & _
                                  " WHERE ID=@LinkID and CompanyID=@CompanyID", connection)

                    command.Parameters.AddWithValue("@LinkTitle", myLink.LinkTitle)
                    command.Parameters.AddWithValue("@LINKURL", myLink.LinkURL)
                    command.Parameters.AddWithValue("@LinkDS", myLink.LinkDescription)
                    command.Parameters.AddWithValue("@LinkTypeCD", myLink.LinkTypeCD)
                    command.Parameters.AddWithValue("@LinkCategoryID", myLink.LinkCategoryID)
                    command.Parameters.AddWithValue("@LinkPageID", myLink.PageID)
                    command.Parameters.AddWithValue("@LinkASIN", myLink.LinkASIN)
                    command.Parameters.AddWithValue("@LinkID", myLink.LinkID)
                    command.Parameters.AddWithValue("@CompanyID", Me.ActiveSite.CompanyID)

                    '   command.Parameters.AddWithValue("@userName", mhwcm.GetUserName)
                    '   command.Parameters.AddWithValue("@userID", 0)
                    '   command.Parameters.AddWithValue("@views", myLink.View)

                    Dim result As Boolean = False
                    If command.ExecuteNonQuery() > 0 Then result = True
                    connection.Close()
                    HttpContext.Current.Response.Redirect(ReturnURL)
                End If
            End If
        End If
        Return errorMSG
    End Function

End Class
