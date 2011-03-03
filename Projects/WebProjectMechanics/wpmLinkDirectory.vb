Imports System.Data.OleDb
Imports System.Text

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
        RequestCID = wpmActiveSite.GetProperty("cid", "")
        RequestLID = wpmActiveSite.GetProperty("id", "")
        RequestAction = wpmActiveSite.GetProperty("md", "")
        ipage = Val(wpmActiveSite.GetProperty("page", ""))

        ReturnURL = "/default.aspx?c=" & ActiveSite.CurrentPageID()

        If wpmActiveSite.GetProperty("submit", "") <> "" Then
            bRequestSubmit = True
            myLink.LinkTitle = wpmUtil.FormatTextEntry(HttpContext.Current.Request("textname"))
            myLink.LinkURL = wpmUtil.FormatTextEntry(HttpContext.Current.Request("texturl"))
            myLink.LinkDescription = wpmUtil.FormatTextEntry(HttpContext.Current.Request("textdesc"))
            Dim arrCat As System.Array
            arrCat = Split(HttpContext.Current.Request("slist"), "~")
            'myLink.LinkCategoryID = arrCat(0)
            'myLink.PageID = arrCat(1)
            myLink.LinkTypeCD = HttpContext.Current.Request("linktype")
            myLink.LinkASIN = wpmUtil.FormatTextEntry(HttpContext.Current.Request("textasin"))
            myLink.ModifiedDT = System.DateTime.Now()
        Else
            myLink.LinkRank = 0
            myLink.AmazonIndex = ""
            myLink.LinkID = ""
            myLink.LinkTitle = ""
            myLink.LinkDescription = ""
            myLink.LinkTypeCD = ""
            myLink.LinkURL = ""
            myLink.PageID = ActiveSite.CurrentPageID()
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
                        mySQL = String.Format("DELETE FROM Link WHERE ID={0} and CompanyID={1} ", RequestLID, mySiteMap.CompanyID)
                        wpmDB.RunDeleteSQL(mySQL, "wpmLinkDirectory.CreateAdminLinkDirectory")
                        HttpContext.Current.Response.Redirect(String.Format("{0}&cid={1}", ReturnURL, RequestCID))
                    End If
                Else
                    HttpContext.Current.Response.Redirect(String.Format("{0}&cid={1}", ReturnURL, RequestCID))
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
                        mySQL = String.Format("DELETE FROM Link WHERE ID={0} and CompanyID={1} ", RequestLID, mySiteMap.CompanyID)
                        wpmDB.RunDeleteSQL(mySQL, "wpmLinkDirectory.CreateLinkDirectory.Link")
                        HttpContext.Current.Response.Redirect(String.Format("{0}&cid={1}", ReturnURL, RequestCID))
                    End If
                Else
                    HttpContext.Current.Response.Redirect(String.Format("{0}&cid={1}", ReturnURL, RequestCID))
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
        ReturnURL = "/wpm/admin/AdminLink.aspx?c=" & ActiveSite.CurrentPageID
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
        Dim PageCategoyCount As Integer = rootCategory()
        If PageCategoyCount > 1 Then
            DrawSubCategory()
        End If
        DrawLinks(PageCategoyCount)
    End Sub
    Sub DrawLinkEntryForm(ByVal CompanyID As String)
        ECHO("<TABLE border=""0"" align=""left"" CELLPADDING=""0"" CELLSPACING=""0"" >")
        ECHO(String.Format("<TR><TD>{0}&nbsp;</TD></TR>", errorMSG))
        ECHO("<TR>")
        ECHO(String.Format("<TD><b>Add Link</B><a href=""{0}link/default.aspx?cid={1}"">Return to Category</a></TD>", wpmApp.Config.wpmWebHome(), RequestCID))
        ECHO("</TR>")
        ECHO("<TR>")
        ECHO("<TD>")
        ECHO(String.Format("<FORM ACTION=""default.aspx?md=add&cid={0}"" METHOD=""POST"">", RequestCID))
        ECHO("<DIV ALIGN=""CENTER"">")
        ECHO("<TABLE border=""0"" CELLPADDING=""5"" CELLSPACING=""5"" >")
        ECHO("<TR>")
        ECHO("<TD width=""100"">Link Name</TD>")
        ECHO(String.Format("<TD><INPUT TYPE=""TEXT"" SIZE=""40"" NAME=""textname"" value=""{0}"" ></TD>", myLink.LinkTitle))
        ECHO("</TR>")
        ECHO("<TR>")
        ECHO("<TD width=""100"">URL</TD>")
        ECHO(String.Format("<TD><INPUT TYPE=""TEXT"" SIZE=""40"" NAME=""texturl"" value=""{0}""></TD>", myLink.LinkURL))
        ECHO("</TR>")
        ECHO("<TR>")
        ECHO("<TD width=""100"">Description</TD>")
        ECHO(String.Format("<TD><TEXTAREA COLS=""30"" ROWS=""10"" NAME=""textdesc"">{0}</TEXTAREA></TD>", myLink.LinkDescription))
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
        ECHO(String.Format("<TD><INPUT TYPE=""TEXT"" SIZE=""40"" NAME=""textasin"" VALUE=""{0}""></TD>", myLink.LinkASIN))
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
        For Each myCatrow As wpmPartGroup In ActiveSite.LinkCategoryList
            If myCatrow.PageID = ActiveSite.CurrentPageID() Then
                iCategoryCount = iCategoryCount + 1
                If myCatrow.ID = RequestCID Then
                    sReturn = String.Format("<A href=""{0}&cid={1}"">{2}</A> -> {3}", ReturnURL, myCatrow.ID, myCatrow.Name, sReturn)
                    sReturn = ParentCategory(myCatrow.ParentID) & sReturn
                End If
            End If
        Next
        If RequestCID <> "" Then
        End If
        sReturn = String.Format("<h2><A href=""{0}"">{1}</A> --> {2}</h2>", ReturnURL, ActiveSite.GetCurrentPageName(), sReturn)
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
        sqlwrk = String.Format("SELECT COUNT(*) as LinkCount FROM Link WHERE Views=NO and PageID={0} ", CurrentPageID)
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
        For Each myCatrow As wpmPartGroup In ActiveSite.LinkCategoryList
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
        For Each myCatrow As wpmPartGroup In ActiveSite.LinkCategoryList
            If myCatrow.ParentID = ParentID Then
                dReturn = dReturn + myCatrow.LinkCount
                If myCatrow.ID = myCatrow.ParentID Then
                    wpmLogging.AuditLog(String.Format("Category Parent same as Self - {0} - {1}", myCatrow.ID, myCatrow.Description), "wpmPartGroup.ChildCategoryLinkCount")
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
        Dim sqlwrk As String = String.Format("DELETE FROM Link WHERE CategoryID={0} and PageID={1} ", intCategoryID, CurrentPageID)
        Dim j As Integer = 0
        wpmDB.RunDeleteSQL(sqlwrk, "wpmLinkDirectory.DeleteCat.Link")
        sqlwrk = String.Format("DELETE FROM LinkCategory WHERE ID={0} and PageID={1} ", intCategoryID, CurrentPageID)
        wpmDB.RunDeleteSQL(sqlwrk, "wpmLinkDirectory.DeleteCat.LinkCategory")
        sqlwrk = String.Format("DELETE FROM LinkRank WHERE CateID={0} and PageID={1} ", intCategoryID, CurrentPageID)
        wpmDB.RunDeleteSQL(sqlwrk, "wpmLinkDirectory.DeleteCat.LinkRank")
        sqlwrk = String.Format("SELECT ID FROM LinkCategory WHERE ParentID={0} and PageID={1} ", intCategoryID, CurrentPageID)
        For Each row As DataRow In wpmDB.GetDataTable(sqlwrk, String.Format("DeleteCat - {0} ", intCategoryID)).Rows
            DeleteCat(row.Item("ID").ToString, "wpmLinkDirectory.DeleteCat.LinkCategory")
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
            Rankimg = String.Format("<img border=""0"" alt=""This link is ranked a {0}"" src=""{1}link/image/rank0.gif"" width=""40"" height=""6"">", intRank, wpmApp.Config.wpmWebHome())
        Else
            Rankimg = String.Format("<img border=""0"" alt=""This link is ranked a {0}"" src=""{1}link/image/rank{0}.gif"" width=""40"" height=""6"">", intRank, wpmApp.Config.wpmWebHome())
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


    Shared Function MapURL(ByRef path As String) As String
        Dim rootPath As String = HttpContext.Current.Server.MapPath("/")
        Dim url As String = Right(path, Len(path) - Len(rootPath))
        'Convert a physical file path to a URL for hypertext links.
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
                If wpmUtil.IsBreak(i, 2) Then
                    sbRight.Append(String.Format("<p><b><A href=""{0}&cid={1}"">{2}</A></B> ({3}) <br />", ReturnURL, myCat.ID, myCat.Name, CategoryLinkCount(myCat.ID)))
                    If wpmUser.IsAdmin() Then
                        sbRight.Append(String.Format("<A href=""{0}&cid={1}&md=edit"">Edit</A> | <A href=""{0}&cid={1}&md=DEL"""">Delete Category</A><br />", ReturnURL, myCat.ID))
                    End If
                    sbRight.Append(String.Format("<br>{0}</p>", myCat.Description))
                Else
                    sbLeft.Append(String.Format("<p><b><A href=""{0}&cid={1}"">{2}</A></B> ({3}) <br />", ReturnURL, myCat.ID, myCat.Name, CategoryLinkCount(myCat.ID)))
                    If wpmUser.IsAdmin() Then
                        sbLeft.Append(String.Format("<A href=""{0}&cid={1}&md=edit"">Edit</A> | <A href=""{0}&cid={1}&md=DEL"""">Delete Category</A><br />", ReturnURL, myCat.ID))
                    End If
                    sbLeft.Append(String.Format("<br>{0}</p>", myCat.Description))
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
            If wpmUtil.IsBreak(i, 2) Then
                sbRight.Append(String.Format("<div class=""box""><p><b><A href=""{0}&cid={1}"">{2}</A></B> ({3}) <br />", ReturnURL, myCat.ID, myCat.Name, CategoryLinkCount(myCat.ID)))
                sbRight.Append(String.Format("<A href=""/wpmgen/linkcategory_edit.aspx?ID={0}"">Edit</A> | <A href=""/wpmgen/linkcategory_delete.aspx?ID={0}"">Delete Category</A><br />", myCat.ID))
                sbRight.Append(String.Format("</p>{0}<br/>{1}</div>", myCat.Description, GetCategoryLinks(myCat.ID)))
            Else
                sbLeft.Append(String.Format("<div class=""box""><p><b><A href=""{0}&cid={1}"">{2}</A></B> ({3}) <br />", ReturnURL, myCat.ID, myCat.Name, CategoryLinkCount(myCat.ID)))
                sbLeft.Append(String.Format("<A href=""/wpmgen/linkcategory_edit.aspx?ID={0}"">Edit</A> | <A href=""/wpmgen/linkcategory_delete.aspx?ID={0}"">Delete Category</A><br />", myCat.ID))
                sbLeft.Append(String.Format("</p>{0}<br/>{1}</div>", myCat.Description, GetCategoryLinks(myCat.ID)))
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
                    sbLinks.Append(String.Format(" [ <a href=""/wpmgen/Link_edit.aspx?id={0}"">Edit</a> ", myPart.LinkID))
                    sbLinks.Append(String.Format("| <a href=""/wpmgen/Link_delete.aspx?id={0}&cid={1}""> Delete</a> ", myPart.LinkID, RequestCID))
                    sbLinks.Append(String.Format("| <a href=""/wpmgen/Link_add.aspx?id={0}"">Copy</a> ]</li>", myPart.LinkID))
                End If
            End If
        Next
        sbLinks.Append("</ul>")
        Return sbLinks.ToString
    End Function

    Private Function GetAmazonLinks() As String
        Dim j As Integer
        Dim i As Integer
        Dim mySB As New StringBuilder("")
        For Each myPart As wpmPart In ActiveSite.PartList
            If Not IsNothing(myPart.PageID) Or Not IsDBNull(myPart.PageID) Then
                If myPart.PageID = ActiveSite.CurrentPageID() And myPart.LinkCategoryID = RequestCID Then
                    j = j + 1
                    If (j >= i) And (j < i + 10) Then
                        If Not IsDBNull(myPart.AmazonIndex) Then
                            If Trim(myPart.AmazonIndex) & "*" <> "*" Then
                                mySB.Append(String.Format(" &nbsp;<a target=""_blank"" href=""http://www.amazon.com/exec/obidos/ASIN/{0}/~~AmazonAssociatesTag~~?creative=327641&camp=14573&link_code=as1""><img border=""0"" src=""http://images.amazon.com/images/P/{0}.01.THUMBZZZ.jpg"" alt=""{1}""></a>&nbsp; ", myPart.AmazonIndex, myPart.LinkURL))
                            End If
                        End If
                    End If
                End If
            End If
        Next
        If j > 0 Then
            mySB.Append("<br/>")
        End If

        Return mySB.ToString
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
        ' Insert Amazon Links
        ECHO(GetAmazonLinks)
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
                        ECHO(String.Format("<A href=""{0}link/rank.aspx?id={1}&cid={2}"">{3}</A>", wpmApp.Config.wpmWebHome(), myLink.LinkID, RequestCID, Rankimg(myLink.LinkRank)))
                    Else
                        ECHO(Rankimg(myLink.LinkRank))
                    End If
                    ECHO(String.Format("&nbsp;<A target=""_new"" href=""{0}"">{1}</A>", myLink.LinkURL, myLink.LinkTitle))
                    If wpmUser.IsAdmin() Then
                        ECHO(String.Format("  [<A href=""/wpmgen/link_edit.aspx?id={0}"">Edit</A> |<A href=""/wpmgen/link_delete.aspx?id={0}&cid={1}""> Delete</A>]", myLink.LinkID, RequestCID))
                    End If
                    ECHO(String.Format("&nbsp;<em>{0}</em><br />", myLink.LinkDescription))
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
                        ECHO(String.Format("<A href=""{0}link/default.aspx?cid={1}&page={2}"">{2}</A> ", wpmApp.Config.wpmWebHome(), RequestCID, i))
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
                        ECHO(String.Format("<A href=""{0}link/default.aspx?cid={1}&page={2}"">{2}</A> ", wpmApp.Config.wpmWebHome(), RequestCID, i))
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
        Dim sqlwrk As String = (String.Format("SELECT id,title,url,ranks,description,asin FROM Link WHERE Views=YES and PageID={0} ORDER BY id DESC", CurrentPageID))
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
                    ECHO("<TD width=""50"" VALIGN=""TOP""><A href=""" & wpmApp.Config.wpmWebHome() & "link/rank.aspx?id=" & lID & "&cid=" & cateID & """>" & Rankimg(CInt(lranks)) & "</A></TD>")
                Else
                    ECHO("<TD width=""50"" VALIGN=""TOP""><br />" & Rankimg(CInt(lranks)) & "</TD>")
                End If
                ECHO("<TD><A target=""_new"" href=""" & lURL & """>" & ltitle & "</A>")
                If wpmUser.IsAdmin() Then
                    ECHO("  [<A href=""" & wpmApp.Config.wpmWebHome() & "link/default.aspx?cid=" & cateID & "&md=edit&id=" & lID & """>Edit</A> |<A href=""" & wpmApp.Config.wpmWebHome() & "link/default.aspx?cid=" & cateID & "&md=del&id=" & lID & """> Delete</A>]")
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
            ECHO(String.Format("<A href=""{0}link/rank.aspx?id={1}&cid={2}""></A></TD>", wpmApp.Config.wpmWebHome(), row.Item("id"), cateID))
            ECHO(String.Format("<TD><A target=""_new"" href=""{0}"">{1}</A>", row.Item("url"), row.Item("title")))
            ECHO(String.Format("  [<A href=""{0}link/default.aspx?cid={1}&md=edit&id={2}"">Edit</A>", wpmApp.Config.wpmWebHome(), cateID, row.Item("id")))
            ECHO(String.Format(" |<A href=""{0}link/default.aspx?cid={1}&md=del&id={2}""> Delete</A>]", wpmApp.Config.wpmWebHome(), cateID, row.Item("id")))
            ECHO(String.Format("&nbsp;<em>{0}</em>", row.Item("description")))
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
        For Each myCategoryList As wpmPartGroup In ActiveSite.LinkCategoryList
            If myCategoryList.PageID = ActiveSite.CurrentPageID() Then
                ECHO(String.Format("<OPTION VALUE='{0}~{1}'", myCategoryList.ID, myCategoryList.Name))
                ECHO(String.Format(">{0}</option>", myCategoryList.Name))
            End If
        Next
    End Sub
    Sub BuildLinkType(ByVal CurrentLinkTypeCD As String)
        Dim mydt As DataTable = wpmDB.GetDataTable("select linktypecd,linktypedesc from linktype where linktypetarget=""_blank""", "mhLinkDirectory.BuildLinkType")
        ECHO("<SELECT NAME=""linktype"">")
        ECHO("<OPTION value=''>Please Select</OPTION>")
        For Each row As DataRow In mydt.Rows
            ECHO(String.Format("<OPTION VALUE='{0}'", row(0)))
            If row(0).ToString = CurrentLinkTypeCD Then
                ECHO(" selected")
            End If
            ECHO(String.Format(">{0}</option>", row(1)))
        Next
        ECHO("</SELECT>")
    End Sub
    Private Function AddNewLink(ByVal CompanyID As String) As String
        If myLink.LinkTitle = "" Then
            errorMSG = errorMSG & "Please insert link name.<br />"
        End If
        If Trim(myLink.LinkASIN & "*") <> "*" Then
            myLink.LinkURL = String.Format("http://www.amazon.com/exec/obidos/ASIN/{0}/thefrogsfolly-20?creative=327641&camp=14573&link_code=as1", myLink.LinkASIN)
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
            HttpContext.Current.Response.Redirect(String.Format("~{0}link/default.aspx?cid={1}", wpmApp.Config.wpmWebHome(), RequestCID))
        End If
        Return errorMSG
    End Function
    ' ** DB FUNCTIONS **
    Private Function GetLinkByLinkID(ByVal CompanyID As String) As wpmPart
        Dim tempLinkRow As New wpmPart
        Using connection As New OleDbConnection(wpmApp.ConnStr)
            connection.Open()
            Using command As New OleDbCommand("SELECT id,title,description,url,categoryID,linktypecd,ASIN " & _
              "FROM Link WHERE ID= @RequestLID and CompanyID=@CompanyID", connection)
                command.Parameters.AddWithValue("@RequestLID", RequestLID)
                command.Parameters.AddWithValue("@CompanyID", CompanyID)
                Using reader As OleDbDataReader = command.ExecuteReader()
                    If reader.Read() Then
                        tempLinkRow.LinkID = reader.GetString(0)
                        tempLinkRow.LinkTitle = reader.GetString(1)
                        tempLinkRow.LinkDescription = reader.GetString(2)
                        tempLinkRow.LinkURL = reader.GetString(3)
                        tempLinkRow.LinkCategoryID = reader.GetString(4)
                        tempLinkRow.LinkTypeCD = reader.GetString(5)
                        tempLinkRow.LinkASIN = reader.GetString(6)
                    End If
                End Using
            End Using
        End Using
        Return tempLinkRow
    End Function
    Private Shared Function CreateLink(ByVal myLink As wpmPart, ByVal CompanyID As String) As Boolean
        Using connection As New OleDbConnection(wpmApp.ConnStr)
            connection.Open()
            Using command As New OleDbCommand("INSERT INTO Link " _
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
                    command.ExecuteNonQuery()
                    command.Parameters.Clear()
                    command.CommandText = "select @@IDENTITY"
                    Dim newLinkID As String = command.ExecuteScalar().ToString
                    myLink.LinkID = newLinkID
                Catch ex As Exception
                    wpmLogging.AuditLog(command.CommandText.ToString, "ERROR CreateLink")
                    Return False
                End Try
            End Using
        End Using
        Return True
    End Function
    Private Function UpdateLink() As String
        Dim arrCat As System.Array
        Dim CurrentLink As wpmPart = GetLinkByLinkID(ActiveSite.CompanyID)
        If CInt(CurrentLink.LinkID) > 0 Then
            If bRequestSubmit Then
                If Len(myLink.LinkTitle) = 0 Then
                    errorMSG = "Link title is required<br />" & vbNewLine
                End If
                If Not CheckURL(myLink.LinkURL, myLink.LinkTypeCD) Then
                    errorMSG = errorMSG & "Invalid URL.<br />"
                End If
                If Not IsDBNull(myLink.LinkASIN) Then
                    myLink.LinkURL = String.Format("http://www.amazon.com/exec/obidos/ASIN/{0}/thefrogsfolly-20?creative=327641&camp=14573&link_code=as1", myLink.LinkASIN)
                End If
                If errorMSG = "" Then
                    arrCat = Split(HttpContext.Current.Request("slist"), "~")

                    myLink.LinkCategoryID = arrCat.GetValue(0).ToString
                    If UBound(arrCat) > 0 Then
                        myLink.PageID = arrCat.GetValue(1).ToString
                    End If
                    Using connection As New OleDbConnection(wpmApp.ConnStr)
                        connection.Open()
                        Using command As New OleDbCommand("UPDATE Link " & _
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
                            command.Parameters.AddWithValue("@CompanyID", ActiveSite.CompanyID)
                            Dim result As Boolean = False
                            If command.ExecuteNonQuery() > 0 Then result = True
                        End Using
                    End Using
                    HttpContext.Current.Response.Redirect(ReturnURL)
                End If
            End If
        End If
        Return errorMSG
    End Function
End Class
