<%@ Page Language="VB"    %>
<script runat="server" type="text/VB" >
    Public mySiteMap As mhSiteMap
    Public myCatalog As mhcatalog
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)
        mySiteMap = New mhSiteMap(Session)
        myCatalog = New mhcatalog(mySiteMap)
        mySiteMap.mySession.ListPageURL = ("~/mhweb/admin/mhMissingPageImage.aspx")
        Dim mySiteTheme As New mhSiteTheme(mySiteMap, False, "MHADMIN")
        Dim myHTML As New StringBuilder(mySiteTheme.sbSiteTemplate.ToString)
        Dim myLinkDirectory As New mhLinkDirectory(mySiteMap)
        Dim myContents As New StringBuilder
        Dim myAdminForm As New StringBuilder
        mhfio.ReadTextFile(HttpContext.Current.Server.MapPath("/mhweb/admin/mhMissingPageImage.text"), myContents)
        mySiteMap.mySession.AddHTMLHead = myContents.ToString
        If GetProperty("DEL", "") <> "" Then
            DeleteImage(GetProperty("DEL", ""))
        End If
        
        If Request.ServerVariables("REQUEST_METHOD") = "POST" Then
            ProcessForm()
            Response.Redirect("/mhweb/admin/mhMissingPageImage.aspx")
        Else
            myAdminForm.Append("<form name=""frmPageImage"" method=""post"" action=""/mhweb/admin/mhMissingPageImage.aspx"">")
            myAdminForm.Append(GetPageList(mySiteMap.CurrentMapRow.PageID, True))
            myAdminForm.Append("<input type=""submit"" value=""Add Checked to Page""><hr/>")
            myAdminForm.Append("<input type=""button"" onclick=""SetAllCheckBoxes('frmPageImage', 'ImageCheckBox', true);"" value=""Check All"">&nbsp;&nbsp;")
            myAdminForm.Append("<input type=""button"" onclick=""SetAllCheckBoxes('frmPageImage', 'ImageCheckBox', false);"" value=""UnCheck All""><br />")
            myAdminForm.Append(GetImageList(mySiteMap.mySiteFile.SiteGallery))
            myAdminForm.Append("</form>")

            If myHTML.Length > 0 Then
                If (InStr(1, myHTML.ToString, "~~MainContent~~") > 0) Then
                    myHTML.Replace("~~MainContent~~", myAdminForm.ToString)
                End If
                myHTML.Replace("~~RightContent~~", mySiteMap.mySession.RightContent)
                myHTML.Replace("</head>", mySiteMap.mySession.AddHTMLHead & "</head>")
                mySiteMap.BuildTemplate(myHTML)
                mySiteMap.ResetSessionVariables()
            End If
            Response.Write(myHTML.ToString)
        End If
    End Sub
    Protected Function GetProperty(ByVal myProperty As String, ByVal curValue As String) As String
        Dim myValue As String = ""
        If Len(Request.QueryString(myProperty)) > 0 Then
            myValue = Request.QueryString(myProperty).ToString
        ElseIf Len(Request.Form.Item(myProperty)) > 0 Then
            myValue = Request.Form.Item(myProperty).ToString
        Else
            myValue = curValue
        End If
        Return myValue
    End Function

    Public Function DeleteImage(ByVal ReqImageID As String) As Boolean
        Dim result As Boolean
        Dim mySQL As String
        Try
            mySQL = ("delete from pageimage where imageid=" & ReqImageID)
            mhDB.RunDeleteSQL(mySQL, "PageImage by Image")
        Catch ex As Exception
            mhUTIL.AuditLog("Error on DeleteImage-pageimage", ex.ToString)
        End Try
        Try
            mySQL = ("delete from [image] where [image].[imageid]=" & ReqImageID)
            mhDB.RunDeleteSQL(mySQL, "Image by ImageID")
        Catch ex As Exception
            mhUTIL.AuditLog("Error on DeleteImage-image", ex.ToString)
        End Try
        Return result
    End Function

    Private Function ProcessForm() As String
        Dim Item As Object
        Dim sSQL As String
        Dim iPageImagePostion As Integer
        Dim iPageID As Integer
        Dim sPageID As String
        
        sPageID = Request.Form.GetValues("x_PageID")(0)
        If sPageID & "x" = "x" Then
            mhUTIL.AuditLog("No PageID Received", "mhMissingPageImage.ProcessForm")
            iPageID = 0
        Else
            iPageID = CInt(sPageID)
            sSQL = "SELECT Max(PageImage.PageImagePosition) FROM PageImage WHERE PageImage.PageID=" & iPageID & " "
            For Each myrow As DataRow In mhDB.GetDataTable(sSQL, "mhMissingPageImage.ProcessForm").Rows
                iPageImagePostion = mhUTIL.GetDBInteger(myrow.Item(0))
                Exit For
            Next
            If Not IsNothing(Request.Form.GetValues("ImageCheckBox")) Then
                For Each Item In Request.Form.GetValues("ImageCheckBox")
                    iPageImagePostion = iPageImagePostion + 1
                    sSQL = "INSERT INTO PageImage ( PageID, ImageID, PageImagePosition ) " & _
                                          "SELECT " & iPageID & "," & Item & " ," & iPageImagePostion & ";"
                    mhDB.RunInsertSQL(sSQL, "mhMissingPageImage.ProcessForm")
                Next
            End If
            myCatalog.FixFolders(sPageID)
        End If
        Return " "
    End Function
    
    Private Function GetPageList(ByVal sPageID As String, ByVal bRequired As Boolean) As String
        Dim sSQL As String = "SELECT PageID,PageName FROM Page WHERE Page.CompanyID=" & Me.mySiteMap.mySession.CompanyID & " order by PageName "
        Dim x_PageIDList As String = ""
        If (sPageID & "x") <> "x" Then
            sPageID = CInt(sPageID)
        End If
        If (bRequired) Then
            x_PageIDList = "<SELECT name='x_PageID'>"
        Else
            x_PageIDList = "<SELECT name='x_PageID'><OPTION value=''>Please Select</OPTION>"
        End If
        For Each myrow As DataRow In mhweb.mhDB.GetDataTable(sSQL, "mhMissingPageImage.GetPageList").Rows
            x_PageIDList = x_PageIDList & "<OPTION value='" & mhUTIL.GetDBInteger(myrow.Item(0)) & "'"
            x_PageIDList = x_PageIDList & ">" & mhUTIL.GetDBString(myrow.Item(1)) & "</option>"
        Next
        'For Each mySiteMapRow As mhSiteMapRow In mySiteMap.mySiteFile.SiteMapRows
        '    If mySiteMapRow.RecordSource = "Page" Then
        '        x_PageIDList = x_PageIDList & "<OPTION value='" & mySiteMapRow.PageID & "'"
        '        If mySiteMapRow.PageID = sPageID Then
        '            x_PageIDList = x_PageIDList & " selected"
        '        End If
        '        x_PageIDList = x_PageIDList & ">" & mySiteMapRow.PageName & "</option>"
        '    End If
        'Next
        x_PageIDList = x_PageIDList & "</SELECT>"
        Return x_PageIDList
    End Function

    Private Function GetImageList(ByVal sDirectory As String) As String
        Dim sReturn As String = ""
        Dim strSQL As String = "SELECT Image.ImageID, Image.ImageName, Image.ImageThumbFileName, Image.ImageFileName FROM [Image] LEFT JOIN PageImage ON Image.ImageID = PageImage.ImageID WHERE (((PageImage.PageImageID) Is Null)) and companyid=" & mySiteMap.mySession.CompanyID & " order by Image.ImageName "
        For Each myrow As DataRow In mhDB.GetDataTable(strSQL, "GetImageList").Rows
            sReturn = sReturn & "<SPAN class=""thumb""><a href=""" & mySiteMap.mySiteFile.SiteGallery & myrow.Item(3) & _
            """><img src=""/mhweb/catalog/ImageResize.aspx?h=150&img=" & mySiteMap.mySiteFile.SiteGallery & myrow.Item(3) & """ alt=""" & myrow.Item(1) & _
            " (" & myrow.Item(0) & ") ""></a><br /><a href=""mhMissingPageImage.aspx?DEL=" & myrow.Item(0) & """>DEL</a>&nbsp;&nbsp;"
            sReturn = sReturn & "<label for=""ImageCheckBox""><input type=""CHECKBOX"" name=""ImageCheckBox"" value=""" & myrow.Item(0) & """ id=""ImageCheckBox" & myrow.Item(0) & """>" & myrow.Item(1) & "</label><br /></SPAN>"
        Next
        Return sReturn
    End Function

</script>
