Imports System.Data.OleDb
Imports System.Text

Public Class PartDirectory
    Public MyStringBuilder As StringBuilder = New StringBuilder
    Private ReadOnly RequestCID As String
    Private ReadOnly RequestLID As String
    Private ReadOnly RequestAction As String
    Private ReadOnly bRequestSubmit As Boolean
    Private errorMSG As String = String.Empty
    Private ReadOnly ActiveCompany As ActiveCompany
    Private ReadOnly PartList As New PartList
    Private ReadOnly myPart As New Part()
    Private ipage As Double
    Private ReturnURL As String

    Sub New(ByRef thisCompany As ActiveCompany)
        ActiveCompany = thisCompany
        RequestCID = wpm_GetProperty("cid", String.Empty)
        RequestLID = wpm_GetProperty("id", String.Empty)
        RequestAction = wpm_GetProperty("md", String.Empty)
        ipage = Val(wpm_GetProperty("page", String.Empty))
        PartList.AddRange(ActiveCompany.PartList)

        ReturnURL = "/default.aspx?c=" & ActiveCompany.CurrentLocationID()

        If wpm_GetProperty("submit", String.Empty) <> String.Empty Then
            bRequestSubmit = True
            myPart.Title = wpm_FormatTextEntry(HttpContext.Current.Request("textname"))
            myPart.URL = wpm_FormatTextEntry(HttpContext.Current.Request("texturl"))
            myPart.Description = wpm_FormatTextEntry(HttpContext.Current.Request("textdesc"))
            Dim arrCat As System.Array = Split(HttpContext.Current.Request("slist"), "~")
            myPart.PartTypeCD = HttpContext.Current.Request("linktype")
            myPart.AmazonIndex = wpm_FormatTextEntry(HttpContext.Current.Request("textasin"))
            myPart.ModifiedDT = System.DateTime.Now()
        Else
            myPart.PartSortOrder = 0
            myPart.AmazonIndex = String.Empty
            myPart.PartID = String.Empty
            myPart.Title = String.Empty
            myPart.Description = String.Empty
            myPart.PartTypeCD = String.Empty
            myPart.URL = String.Empty
            myPart.LocationID = ActiveCompany.CurrentLocationID()
            myPart.PartCategoryID = RequestCID
            myPart.ModifiedDT = System.DateTime.Now()

            bRequestSubmit = False
        End If
    End Sub
    Sub CreateAdminPartDirectory()
        Dim mySQL As String = String.Empty
        Select Case RequestAction
            Case "add"
                If bRequestSubmit Then
                    errorMSG = AddNewLink(wpm_CurrentSiteID)
                End If
                DrawLinkEntryForm()
            Case "del"
                If wpm_IsAdmin Then
                    If RequestLID <> String.Empty Then
                        mySQL = String.Format("DELETE FROM Link WHERE ID={0} and CompanyID={1} ", RequestLID, wpm_CurrentSiteID)
                        wpm_RunDeleteSQL(mySQL, "wpmLinkDirectory.CreateAdminLinkDirectory")
                        HttpContext.Current.Response.Redirect(String.Format("{0}&cid={1}", ReturnURL, RequestCID))
                    End If
                Else
                    HttpContext.Current.Response.Redirect(String.Format("{0}&cid={1}", ReturnURL, RequestCID))
                End If
            Case "edit"
                If wpm_IsAdmin Then
                    errorMSG = UpdateLink()
                End If
                DrawLinkEntryForm()
            Case Else
                DisplayAdminLinkDirectory()
        End Select
    End Sub
    Sub CreateLinkDirectory()
        Dim mySQL As String = String.Empty
        Select Case RequestAction
            Case "add"
                If bRequestSubmit Then
                    errorMSG = AddNewLink(wpm_CurrentSiteID)
                End If
                DrawLinkEntryForm()
            Case "del"
                If wpm_IsAdmin Then
                    If RequestLID <> String.Empty Then
                        mySQL = String.Format("DELETE FROM Link WHERE ID={0} and CompanyID={1} ", RequestLID, wpm_CurrentSiteID)
                        wpm_RunDeleteSQL(mySQL, "wpmLinkDirectory.CreateLinkDirectory.Link")
                        HttpContext.Current.Response.Redirect(String.Format("{0}&cid={1}", ReturnURL, RequestCID))
                    End If
                Else
                    HttpContext.Current.Response.Redirect(String.Format("{0}&cid={1}", ReturnURL, RequestCID))
                End If
            Case "edit"
                If wpm_IsAdmin Then
                    errorMSG = UpdateLink()
                End If
                DrawLinkEntryForm()
            Case Else
                DisplayLinkDirectory()
        End Select
    End Sub
    Sub ECHO(ByVal MSG As String)
        MyStringBuilder.Append(MSG & vbNewLine)
    End Sub
    Sub DisplayAdminLinkDirectory()
        ReturnURL = String.Format("/admin/AdminLink.aspx?c={0}", ActiveCompany.CurrentLocationID)
        ECHO("<h1>Parts By Category</h1>")
        ECHO("<ul>")
        ECHO("<li><span class=""highlight-one"">Site Type (Content appears on every page)</span></li>")
        ECHO("<li><span class=""highlight-two"">Site Cateogry (Content appears only for a single category)</span></li>")
        ECHO("<li><span class=""highlight-three"">Company (Content appears for specific company)</span></li>")
        ECHO("<li><span class=""highlight-four"">Page (Content appears on single location)</span></li>")
        ECHO("</ul>")
        ECHO("<div>")
        ECHO("<div class=""row"">")
        For Each myCat As PartCategory In ActiveCompany.LinkCategoryList
            ECHO("<div class=""col-md-3"" >")
            ECHO(String.Format("<div class=""panel panel-default""><div class=""panel-heading""><b><A href=""/admin/maint/default.aspx?type=PartCategory&LinkCategoryID={1}"">{2}</A></B> ({3}) ", ReturnURL, myCat.ID, myCat.Name, CategoryLinkCount(myCat.ID)))
            ECHO(String.Format("<br/>{0}</div><div class=""panel-body""><br/>{1}</div><div class=""panel-footer""></div></div>", myCat.Description, GetCategoryLinks(myCat.ID)))
            ECHO("</div>")
        Next
        ECHO("</div>")
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
    Sub DrawLinkEntryForm()
        ECHO("<TABLE border=""0"" align=""left"" CELLPADDING=""0"" CELLSPACING=""0"" >")
        ECHO(String.Format("<TR><TD>{0}&nbsp;</TD></TR>", errorMSG))
        ECHO("<TR>")
        ECHO(String.Format("<TD><b>Add Link</B><a href=""{0}link/default.aspx?cid={1}"">Return to Category</a></TD>", wpm_SiteConfig.ApplicationHome(), RequestCID))
        ECHO("</TR>")
        ECHO("<TR>")
        ECHO("<TD>")
        ECHO(String.Format("<FORM ACTION=""default.aspx?md=add&cid={0}"" METHOD=""POST"">", RequestCID))
        ECHO("<DIV ALIGN=""CENTER"">")
        ECHO("<TABLE border=""0"" CELLPADDING=""5"" CELLSPACING=""5"" >")
        ECHO("<TR>")
        ECHO("<TD width=""100"">Link Name</TD>")
        ECHO(String.Format("<TD><INPUT TYPE=""TEXT"" SIZE=""40"" NAME=""textname"" value=""{0}"" ></TD>", myPart.Title))
        ECHO("</TR>")
        ECHO("<TR>")
        ECHO("<TD width=""100"">URL</TD>")
        ECHO(String.Format("<TD><INPUT TYPE=""TEXT"" SIZE=""40"" NAME=""texturl"" value=""{0}""></TD>", myPart.URL))
        ECHO("</TR>")
        ECHO("<TR>")
        ECHO("<TD width=""100"">Description</TD>")
        ECHO(String.Format("<TD><TEXTAREA COLS=""30"" ROWS=""10"" NAME=""textdesc"">{0}</TEXTAREA></TD>", myPart.Description))
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
        ECHO(String.Format("<TD><INPUT TYPE=""TEXT"" SIZE=""40"" NAME=""textasin"" VALUE=""{0}""></TD>", myPart.AmazonIndex))
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
        Dim sReturn As String = (String.Empty)
        For Each myCatrow As PartCategory In ActiveCompany.LinkCategoryList
            If myCatrow.LocationID = ActiveCompany.CurrentLocationID() Then
                iCategoryCount = iCategoryCount + 1
                If myCatrow.ID = RequestCID Then
                    sReturn = String.Format("<A href=""{0}&cid={1}"">{2}</A> -> {3}", ReturnURL, myCatrow.ID, myCatrow.Name, sReturn)
                    sReturn = ParentCategory(myCatrow.ParentID) & sReturn
                End If
            End If
        Next
        If RequestCID <> String.Empty Then
        End If
        sReturn = String.Format("<h2><A href=""{0}"">{1}</A> --> {2}</h2>", ReturnURL, ActiveCompany.CurrentLocationNM(), sReturn)
        ECHO(sReturn)
        Return iCategoryCount
    End Function

    Function ParentCategory(ByVal ParentID As String) As String
        Dim pcReturn As String = String.Empty
        For Each myCatrow As PartCategory In ActiveCompany.LinkCategoryList
            If myCatrow.ID = ParentID Then
                pcReturn = String.Format("<A href=""{0}&cid={1}"">{2}</A> > {3}", ReturnURL, myCatrow.ID, myCatrow.Name, pcReturn)
                pcReturn = ParentCategory(myCatrow.ParentID) & pcReturn
            End If
        Next
        Return pcReturn
    End Function
    ' ########################
    ' // Unapproved Links
    ' ########################
    Shared Function UnapprovedLinks(ByVal CurrentPageID As Integer) As Integer
        Dim linksCount As Integer = 0
        For Each row As DataRow In wpm_GetDataTable(String.Format("SELECT COUNT(*) as LinkCount FROM Link WHERE Views=NO and PageID={0} ", CurrentPageID), "UnapprovedLinks").Rows
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
        For Each myCatrow As PartCategory In ActiveCompany.LinkCategoryList
            If myCatrow.ID = CatID Then
                dReturn = myCatrow.PartCount
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
        For Each myCatrow As PartCategory In ActiveCompany.LinkCategoryList
            If myCatrow.ParentID = ParentID Then
                dReturn = dReturn + myCatrow.PartCount
                If myCatrow.ID = myCatrow.ParentID Then
                    ApplicationLogging.AuditLog(String.Format("Category Parent same as Self - {0} - {1}", myCatrow.ID, myCatrow.Description), "wpmPartGroup.ChildCategoryLinkCount")
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
    Public Shared Function DeleteCat(ByVal intCategoryID As String, ByVal CurrentPageID As String) As Boolean
        Dim sqlwrk As String = String.Format("DELETE FROM Link WHERE CategoryID={0} and PageID={1} ", intCategoryID, CurrentPageID)
        wpm_RunDeleteSQL(sqlwrk, "wpmLinkDirectory.DeleteCat.Link")
        sqlwrk = String.Format("DELETE FROM LinkCategory WHERE ID={0} and PageID={1} ", intCategoryID, CurrentPageID)
        wpm_RunDeleteSQL(sqlwrk, "wpmLinkDirectory.DeleteCat.LinkCategory")
        sqlwrk = String.Format("DELETE FROM LinkRank WHERE CateID={0} and PageID={1} ", intCategoryID, CurrentPageID)
        wpm_RunDeleteSQL(sqlwrk, "wpmLinkDirectory.DeleteCat.LinkRank")
        sqlwrk = String.Format("SELECT ID FROM LinkCategory WHERE ParentID={0} and PageID={1} ", intCategoryID, CurrentPageID)
        For Each row As DataRow In wpm_GetDataTable(sqlwrk, String.Format("DeleteCat - {0} ", intCategoryID)).Rows
            DeleteCat(row.Item("ID").ToString, "wpmLinkDirectory.DeleteCat.LinkCategory")
        Next
        Return True
    End Function

    ' #######################
    ' // Checking User Name
    ' #######################
    Public Shared Function CheckUserName(ByVal UN2Check As String) As Boolean
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
    Public Shared Function Rankimg(ByVal intRank As Integer) As String
        If intRank = 0 Then
            Return String.Format("<img border=""0"" alt=""This link is ranked a {0}"" src=""{1}link/images/rank0.gif"" width=""40"" height=""6"">", intRank, wpm_SiteConfig.ApplicationHome())
        Else
            Return String.Format("<img border=""0"" alt=""This link is ranked a {0}"" src=""{1}link/images/rank{0}.gif"" width=""40"" height=""6"">", intRank, wpm_SiteConfig.ApplicationHome())
        End If
    End Function

    ' #######################
    ' // Checking Email
    ' #######################
    Public Shared Function CheckEmail(ByVal E2Check As String) As Boolean
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
    Public Shared Function isLetter(ByVal L2Check As String) As Boolean
        If Asc((L2Check.ToUpper)) > 64 And Asc((L2Check.ToUpper)) < 91 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Shared Function CheckURL(ByVal U2Check As String, ByVal Ltypecode As String) As Boolean
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
        Return bCheckURL
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
        For Each myCat As PartCategory In ActiveCompany.LinkCategoryList
            If myCat.ParentID = RequestCID And myCat.LocationID = ActiveCompany.CurrentLocationID() Then
                If wpm_IsBreak(i, 2) Then
                    sbRight.Append(String.Format("<p><b><A href=""{0}&cid={1}"">{2}</A></B> ({3}) <br />", ReturnURL, myCat.ID, myCat.Name, CategoryLinkCount(myCat.ID)))
                    If wpm_IsAdmin Then
                        sbRight.Append(String.Format("<A href=""{0}&cid={1}&md=edit"">Edit</A> | <A href=""{0}&cid={1}&md=DEL"""">Delete Category</A><br />", ReturnURL, myCat.ID))
                    End If
                    sbRight.Append(String.Format("<br>{0}</p>", myCat.Description))
                Else
                    sbLeft.Append(String.Format("<p><b><A href=""{0}&cid={1}"">{2}</A></B> ({3}) <br />", ReturnURL, myCat.ID, myCat.Name, CategoryLinkCount(myCat.ID)))
                    If wpm_IsAdmin Then
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
    Sub DrawAdminLinks(ByVal CurrentCID As String)
        ECHO("<hr>")
        For Each myLink As Part In PartList
            If myLink.PartCategoryID = CurrentCID Then
                If Left(myLink.LocationID, 3) = "CAT" Then
                    ECHO(String.Format("<span style=""background-color:yellow;color:red;"">{0}</span>", myLink.Title))
                    ECHO(String.Format("  [<A href=""/admin/default.aspx?type=Part&PartID={0}"">Edit</A> ]<br>", myLink.PartID, RequestCID))
                Else
                    ECHO(myLink.Title)
                    ECHO(String.Format("  [<A href=""/admin/maint/default.aspx?type=Part&PartID={0}"">Edit</A> ]<br>", myLink.PartID, RequestCID))
                End If
            End If
        Next
    End Sub

    Private Function GetCategoryLinks(ByVal CurrentCID As String) As String
        Dim sbLinks As New StringBuilder("<div class=""list-grroup"">")
        Dim myLocation As Location
        Dim sLocationInfo As String = String.Empty
        For Each myPart As Part In PartList
            If myPart.PartCategoryID = CurrentCID Then
                If myPart.LocationID <> String.Empty Then
                    myLocation = ActiveCompany.LocationList.FindLocation(myPart.LocationID, 0)
                    sLocationInfo = String.Format("{0} - {1} [{2}] ", myPart.PartSortOrder, myPart.Title, myLocation.LocationName)
                Else
                    sLocationInfo = String.Format("{0} - {1}", myPart.PartSortOrder, myPart.Title)
                End If
                If myPart.CompanyID <> String.Empty Then
                    sLocationInfo = String.Format("{0} [{1}]", sLocationInfo, (From i In GetCompanyLookupList() Where i.Value = myPart.CompanyID Select i.Name).SingleOrDefault)
                End If



                If Not myPart.View Then
                    sLocationInfo = "(i) " & sLocationInfo
                End If

                If myPart.LocationID = String.Empty AndAlso myPart.CompanyID = String.Empty Then
                    sbLinks.Append(String.Format("<a href=""/admin/maint/default.aspx?type=Part&PartID={0}"" class=""list-group-item highlight-one"">{1}*</a>", myPart.PartID, sLocationInfo))
                ElseIf myPart.LocationID = String.Empty Then
                    sbLinks.Append(String.Format("<a href=""/admin/maint/default.aspx?type=Part&PartID={0}"" class=""list-group-item highlight-three"">{1}*</a>", myPart.PartID, sLocationInfo))
                Else
                    sbLinks.Append(String.Format("<a href=""/admin/maint/default.aspx?type=Part&PartID={0}"" class=""list-group-item highlight-four"">{1}*</a>", myPart.PartID, sLocationInfo))
                End If
            End If
        Next
        sbLinks.Append("</div>")
        Return sbLinks.ToString
    End Function

    Private Function GetAmazonLinks() As String
        Dim j As Integer
        Dim i As Integer
        Dim mySB As New StringBuilder(String.Empty)
        For Each myPart As Part In PartList
            If Not IsNothing(myPart.LocationID) Or Not IsDBNull(myPart.LocationID) Then
                If myPart.LocationID = ActiveCompany.CurrentLocationID() And myPart.PartCategoryID = RequestCID Then
                    j = j + 1
                    If (j >= i) And (j < i + 10) Then
                        If Not IsDBNull(myPart.AmazonIndex) Then
                            If Trim(myPart.AmazonIndex) & "*" <> "*" Then
                                mySB.Append(String.Format(" &nbsp;<a target=""_blank"" href=""http://www.amazon.com/exec/obidos/ASIN/{0}/~~AmazonAssociatesTag~~?creative=327641&camp=14573&link_code=as1""><img border=""0"" src=""http://images.amazon.com/images/P/{0}.01.THUMBZZZ.jpg"" alt=""{1}""></a>&nbsp; ", myPart.AmazonIndex, myPart.URL))
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
        Dim i As Double = 0
        Dim iRecordCount As Integer = 0
        Dim tPage As String = String.Empty
        Dim iLinkCount As Integer = 0
        If ipage = 0 Then
            ipage = 1
        End If
        If ipage = 1 Then
            i = 0
        Else
            i = (ipage - 1) * CType(10, Double)
        End If
        ' Insert Amazon Links
        ECHO(GetAmazonLinks)
        For Each myLink As Part In PartList
            If myLink.LocationID = ActiveCompany.CurrentLocationID() And myLink.PartCategoryID = RequestCID Then
                j = j + 1
                If ((j >= i) And (j < i + CType(10, Double)) Or PageCategoryCount < 2) Then
                    If iLinkCount = 2 Then
                        iLinkCount = 0
                    End If
                    iLinkCount = iLinkCount + 1
                    If wpm_IsUser() Then
                        ECHO(String.Format("<A href=""{0}link/rank.aspx?id={1}&cid={2}"">{3}</A>", wpm_SiteConfig.ApplicationHome(), myLink.PartID, RequestCID, Rankimg(myLink.PartSortOrder)))
                    Else
                        ECHO(Rankimg(myLink.PartSortOrder))
                    End If
                    ECHO(String.Format("&nbsp;<A target=""_new"" href=""{0}"">{1}</A>", myLink.URL, myLink.Title))
                    If wpm_IsAdmin Then
                        ECHO(String.Format("  [<A href=""{2}maint/default.aspx?type=Part&PartID={0}"">Edit</A> ]", myLink.PartID, RequestCID, wpm_SiteConfig.AdminFolder))
                    End If
                    ECHO(String.Format("&nbsp;<em>{0}</em><br />", myLink.Description))
                End If
                iRecordCount = iRecordCount + 1
            End If
        Next

        If iLinkCount = 1 Then
        End If

        If iRecordCount > CType(10, Double) Then
            If Fix(iRecordCount / CType(10, Double)) < iRecordCount / CType(10, Double) Then
                tPage = (Fix(iRecordCount / CType(10, Double)) + 1).ToString
            Else
                tPage = Fix(iRecordCount / CType(10, Double)).ToString
            End If
            If tPage > "1" Then
                ECHO("<center><br /><br />Select Page ")
                For i = 1 To CInt(tPage)
                    If i = ipage Then
                        ECHO(i & " ")
                    Else
                        ECHO(String.Format("<A href=""{0}link/default.aspx?cid={1}&page={2}"">{2}</A> ", wpm_SiteConfig.ApplicationHome(), RequestCID, i))
                    End If
                Next
                ECHO("</center>")
            End If
        End If
    End Sub
    Sub DrawAmazonPictures(ByVal iLinksPerPage As Integer, ByVal i As Integer)
        Dim j As Integer = 0
        For Each myLink As Part In PartList
            If Not IsNothing(myLink.LocationID) Or Not IsDBNull(myLink.LocationID) Then
                If myLink.LocationID = ActiveCompany.CurrentLocationID() And myLink.PartCategoryID = RequestCID Then
                    j = j + 1
                    If (j >= i) And (j < i + iLinksPerPage) Then
                        If Not IsDBNull(myLink.AmazonIndex) Then
                            If Trim(myLink.AmazonIndex) & "*" <> "*" Then
                                ECHO(String.Format("<a target=""_blank"" href=""http://www.amazon.com/exec/obidos/ASIN/{0}/controlorigins-20?creative=327641&camp=14573&link_code=as1""><img border=""0"" src=""http://images.amazon.com/images/P/{0}.01.THUMBZZZ.jpg"" alt=""{1}""></a>", myLink.AmazonIndex, myLink.URL))
                            End If
                        End If
                    End If
                End If
            End If
        Next
        j = 0

    End Sub
    Sub DrawYUILinks(ByRef mySitemap As ActiveCompany)
        Dim ipage As Integer = CInt(HttpContext.Current.Request("page"))
        Dim iRecordCount As Integer = 0
        Dim tPage As Integer = 0
        Dim iLinkCount As Integer = 0
        Const iLinksPerPage As Integer = 24
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
        ECHO("<div class=""yui-g""><div class=""yui-u first"">")
        For Each myLink As Part In PartList
            If myLink.LocationID = mySitemap.CurrentLocationID() Then
                j = j + 1
                If (j >= i) And (j < i + iLinksPerPage) Then
                    iLinkCount = iLinkCount + 1
                    ECHO(String.Format("<div><A target=""_new"" href=""{0}"">{1}</A>", myLink.URL, myLink.Title))
                    If wpm_IsAdmin Then
                        ECHO(String.Format("  [<A href=""{2}maint/default.aspx?type=Part&PartID={0}"">Edit</A> ]", myLink.PartID, RequestCID, wpm_SiteConfig.AdminFolder))
                    End If
                    ECHO(String.Format("<br/><em>{0}</em><br /></div>", myLink.Description))
                End If
                iRecordCount = iRecordCount + 1
            End If
        Next

        If iLinkCount = 1 Then
            ECHO(String.Empty)
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
                        ECHO(String.Format("<A href=""{0}link/default.aspx?cid={1}&page={2}"">{2}</A> ", wpm_SiteConfig.ApplicationHome(), RequestCID, i))
                    End If
                Next
                ECHO("</center>")
            End If
        End If

    End Sub
    Sub TopRatedLinks(ByVal cateID As Integer, ByVal CurrentPageID As Integer)
        Dim sbAmazonLinks As New StringBuilder
        Dim myDT As DataTable
        Dim myASIN As String = (String.Empty)
        Dim lID As String = String.Empty
        Dim ltitle As String = String.Empty
        Dim lURL As String = String.Empty
        Dim lranks As String = String.Empty
        Dim ldescription As String = String.Empty
        Dim sqlwrk As String = (String.Format("SELECT id,title,url,ranks,description,asin FROM Link WHERE Views=YES and PageID={0} ORDER BY id DESC", CurrentPageID))
        '      Dim myRowCount As Integer = 0
        myDT = wpm_GetDataTable(sqlwrk, "TopRatedLinks")
        If myDT.Rows.Count > 15 Then
            For TopNumber As Integer = 0 To 15
                myASIN = myDT.Rows.Item(TopNumber).Item("asin").ToString
                lURL = myDT.Rows.Item(TopNumber).Item("url").ToString
                If Trim(myDT.Rows.Item(TopNumber).Item("asin").ToString) <> String.Empty Then
                    sbAmazonLinks.Append(String.Format("<a target=""_blank"" href=""http://www.amazon.com/exec/obidos/ASIN/{0}/controlorigins-20?creative=327641&camp=14573&link_code=as1""><img border=""0"" src=""http://images.amazon.com/images/P/{0}.01.THUMBZZZ.jpg"" alt=""{1}""></a>", myASIN, lURL))
                End If
            Next
        Else
            For Each row As DataRow In myDT.Rows
                myASIN = row.Item("asin").ToString
                lURL = row.Item("url").ToString
                If Trim(row.Item("asin").ToString) <> String.Empty Then
                    sbAmazonLinks.Append(String.Format("<a target=""_blank"" href=""http://www.amazon.com/exec/obidos/ASIN/{0}/controlorigins-20?creative=327641&camp=14573&link_code=as1""><img border=""0"" src=""http://images.amazon.com/images/P/{0}.01.THUMBZZZ.jpg"" alt=""{1}""></a>", myASIN, lURL))
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
                If wpm_IsUser() Then
                    ECHO(String.Format("<TD width=""50"" VALIGN=""TOP""><A href=""{0}link/rank.aspx?id={1}&cid={2}"">{3}</A></TD>", wpm_SiteConfig.ApplicationHome(), lID, cateID, Rankimg(CInt(lranks))))
                Else
                    ECHO(String.Format("<TD width=""50"" VALIGN=""TOP""><br />{0}</TD>", Rankimg(CInt(lranks))))
                End If
                ECHO(String.Format("<TD><A target=""_new"" href=""{0}"">{1}</A>", lURL, ltitle))
                If wpm_IsAdmin Then
                    ECHO(String.Format("  [<A href=""{0}link/default.aspx?cid={1}&md=edit&id={2}"">Edit</A> |<A href=""{0}link/default.aspx?cid={1}&md=del&id={2}""> Delete</A>]", wpm_SiteConfig.ApplicationHome(), cateID, lID))
                End If
                ECHO(String.Format("&nbsp;<em>{0}</em>", ldescription))
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
        For Each row As DataRow In wpm_GetDataTable(sqlwrk, "mhLinkDirectory.LinkAdmin").Rows
            ECHO("<TABLE border=""0"" CELLPADDING=""2"" CELLSPACING=""2"" >")
            ECHO("<TR>")
            ECHO("<TD width=""50"" VALIGN=""TOP"">")
            ECHO(String.Format("<A href=""{0}link/rank.aspx?id={1}&cid={2}""></A></TD>", wpm_SiteConfig.ApplicationHome(), row.Item("id"), cateID))
            ECHO(String.Format("<TD><A target=""_new"" href=""{0}"">{1}</A>", row.Item("url"), row.Item("title")))
            ECHO(String.Format("  [<A href=""{0}link/default.aspx?cid={1}&md=edit&id={2}"">Edit</A>", wpm_SiteConfig.ApplicationHome(), cateID, row.Item("id")))
            ECHO(String.Format(" |<A href=""{0}link/default.aspx?cid={1}&md=del&id={2}""> Delete</A>]", wpm_SiteConfig.ApplicationHome(), cateID, row.Item("id")))
            ECHO(String.Format("&nbsp;<em>{0}</em>", row.Item("description")))
            ECHO("</TD>")
            ECHO("</TR>")
            ECHO("</TABLE>")
        Next
    End Sub

    ' #######################
    ' // Category List Box
    ' #######################
    Public Sub BuildCategory()
        ECHO("<SELECT NAME=""slist"">")
        ECHO("<OPTION value=''>Please Select</OPTION>")
        BuildCategoryList()
        ECHO("</SELECT>")
    End Sub

    Public Sub BuildCategoryList()
        For Each myCategoryList As PartCategory In ActiveCompany.LinkCategoryList
            If myCategoryList.LocationID = ActiveCompany.CurrentLocationID() Then
                ECHO(String.Format("<OPTION VALUE='{0}~{1}'", myCategoryList.ID, myCategoryList.Name))
                ECHO(String.Format(">{0}</option>", myCategoryList.Name))
            End If
        Next
    End Sub
    Sub BuildLinkType(ByVal CurrentLinkTypeCD As String)
        Dim mydt As DataTable = wpm_GetDataTable("select linktypecd,linktypedesc from linktype where linktypetarget=""_blank""", "mhLinkDirectory.BuildLinkType")
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
        If myPart.Title = String.Empty Then
            errorMSG = String.Format("{0}Please insert link name.<br />", errorMSG)
        End If
        If Trim(myPart.AmazonIndex & "*") <> "*" Then
            myPart.URL = String.Format("http://www.amazon.com/exec/obidos/ASIN/{0}/controlorigins-20?creative=327641&camp=14573&link_code=as1", myPart.AmazonIndex)
        End If
        If Not CheckURL(myPart.URL, myPart.PartTypeCD) Then
            errorMSG = errorMSG & "Invalid URL.<br />"
        End If
        If wpm_IsAdmin Then
            myPart.View = True
        Else
            myPart.View = False
        End If
        If errorMSG = String.Empty Then
            If Not CreateLink(myPart, CompanyID) Then
                errorMSG = "Problem Saving New Link, Please contact system administrator."
            End If
            HttpContext.Current.Response.Redirect(String.Format("~{0}link/default.aspx?cid={1}", wpm_SiteConfig.ApplicationHome(), RequestCID))
        End If
        Return errorMSG
    End Function
    ' ** DB FUNCTIONS **
    Private Function GetLinkByLinkID(ByVal CompanyID As String) As Part
        Dim tempLinkRow As New Part
        Using connection As New OleDbConnection(wpm_SQLDBConnString)
            connection.Open()
            Using command As New OleDbCommand("SELECT id,title,description,url,categoryID,linktypecd,ASIN FROM Link WHERE ID= @RequestLID and CompanyID=@CompanyID", connection)
                command.Parameters.AddWithValue("@RequestLID", RequestLID)
                command.Parameters.AddWithValue("@CompanyID", CompanyID)
                Using reader As OleDbDataReader = command.ExecuteReader()
                    If reader.Read() Then
                        tempLinkRow.PartID = reader.GetString(0)
                        tempLinkRow.Title = reader.GetString(1)
                        tempLinkRow.Description = reader.GetString(2)
                        tempLinkRow.URL = reader.GetString(3)
                        tempLinkRow.PartCategoryID = reader.GetString(4)
                        tempLinkRow.PartTypeCD = reader.GetString(5)
                        tempLinkRow.AmazonIndex = reader.GetString(6)
                    End If
                End Using
            End Using
            connection.Close()
        End Using
        Return tempLinkRow
    End Function
    Private Shared Function CreateLink(ByVal myLink As Part, ByVal CompanyID As String) As Boolean
        Using connection As New OleDbConnection(wpm_SQLDBConnString)
            connection.Open()
            Using command As New OleDbCommand("INSERT INTO Link (title,description,url,categoryID,LinkTypeCD,companyID) VALUES (@title,@description,@url,@categoryID,@LinkTypeCD,@companyID)", connection)
                command.Parameters.AddWithValue("@title", myLink.Title)
                command.Parameters.AddWithValue("@descripton", myLink.Description)
                command.Parameters.AddWithValue("@url", myLink.URL)
                command.Parameters.AddWithValue("@categoryID", myLink.PartCategoryID)
                command.Parameters.AddWithValue("@LinkTypeCD", myLink.PartTypeCD)
                command.Parameters.AddWithValue("@asin", myLink.AmazonIndex)
                command.Parameters.AddWithValue("@companyID", CompanyID)
                Try
                    command.ExecuteNonQuery()
                    command.Parameters.Clear()
                    command.CommandText = "select @@IDENTITY"
                    Dim newLinkID As String = command.ExecuteScalar().ToString
                    myLink.PartID = newLinkID
                Catch ex As Exception
                    ApplicationLogging.SQLExceptionLog(String.Format("ERROR CreateLink - {0}", command.CommandText), ex)
                    Return False
                End Try
            End Using
            connection.Close()
        End Using
        Return True
    End Function
    Private Function UpdateLink() As String
        Dim arrCat As System.Array
        Dim CurrentLink As Part = GetLinkByLinkID(wpm_CurrentSiteID)
        If CInt(CurrentLink.PartID) > 0 Then
            If bRequestSubmit Then
                If Len(myPart.Title) = 0 Then
                    errorMSG = "Link title is required<br />" & vbNewLine
                End If
                If Not CheckURL(myPart.URL, myPart.PartTypeCD) Then
                    errorMSG = errorMSG & "Invalid URL.<br />"
                End If
                If Not IsDBNull(myPart.AmazonIndex) Then
                    myPart.URL = String.Format("http://www.amazon.com/exec/obidos/ASIN/{0}/controlorigins-20?creative=327641&camp=14573&link_code=as1", myPart.AmazonIndex)
                End If
                If errorMSG = String.Empty Then
                    arrCat = Split(HttpContext.Current.Request("slist"), "~")

                    myPart.PartCategoryID = arrCat.GetValue(0).ToString
                    If UBound(arrCat) > 0 Then
                        myPart.LocationID = arrCat.GetValue(1).ToString
                    End If
                    Using connection As New OleDbConnection(wpm_SQLDBConnString)
                        connection.Open()
                        Using command As New OleDbCommand("UPDATE Link SET title=@LinkTitle,url=@LinkURL,description=@LinkDS,linktypecd=@LinkTypeCD,categoryID=@LinkCategoryID,pageID=@LinkPageID,asin=@LinkASIN  WHERE ID=@LinkID and CompanyID=@CompanyID", connection)
                            command.Parameters.AddWithValue("@LinkTitle", myPart.Title)
                            command.Parameters.AddWithValue("@LINKURL", myPart.URL)
                            command.Parameters.AddWithValue("@LinkDS", myPart.Description)
                            command.Parameters.AddWithValue("@LinkTypeCD", myPart.PartTypeCD)
                            command.Parameters.AddWithValue("@LinkCategoryID", myPart.PartCategoryID)
                            command.Parameters.AddWithValue("@LinkPageID", myPart.LocationID)
                            command.Parameters.AddWithValue("@LinkASIN", myPart.AmazonIndex)
                            command.Parameters.AddWithValue("@LinkID", myPart.PartID)
                            command.Parameters.AddWithValue("@CompanyID", wpm_CurrentSiteID)
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
