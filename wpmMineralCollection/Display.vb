Imports System.Data.Linq
Imports System.Globalization
Imports System.Text

Public Module Display
    Public Function DisplayDate(ByVal dbObject As Object) As String
        Dim sReturn As String = String.Empty
        If Not IsDBNull(dbObject) And Not IsNothing(dbObject) Then
            Try
                sReturn = CDate(dbObject).ToShortDateString
            Catch
                sReturn = String.Empty
            End Try
        End If
        Return sReturn
    End Function
    Public Function DisplayPrice(ByVal dbObject As Object) As String
        Dim sReturn As String = String.Empty
        If Not IsDBNull(dbObject) And Not IsNothing(dbObject) Then
            Try
                sReturn = dbObject.ToString()
            Catch
                sReturn = String.Empty
            End Try
        End If
        Return sReturn
    End Function
    Public Function DisplayCollectionItemID(ByVal sValue As String, ByVal IsAdmin As Boolean) As String
        If String.IsNullOrEmpty(sValue) Then
            Return String.Empty
        Else
            If IsAdmin Then
                Return String.Format("<div class="" display_field""><a href=""/MineralCollection/Admin.aspx?Type=Specimen&CollectionItemID={0}"">Edit Item</a></div>", sValue)
            Else
                Return String.Empty
            End If
        End If
    End Function
    Public Function DisplayBoolean(ByVal sLabel As String, ByVal sValue As String) As String
        If String.IsNullOrEmpty(sValue) Then
            Return String.Empty
        Else
            Dim myInt As Int32 = 0
            Int32.TryParse(sValue, myInt)
            If (myInt <> 0) Then
                Return String.Format("(<span style=""color:red;display:inline;"">{0}</span>)", sLabel.ToUpper())
            Else
                Return String.Empty
            End If
        End If
    End Function
    Public Function DisplayWithSold(ByVal sLabel As String, ByVal sValue As String, ByVal isSold As String) As String
        If String.IsNullOrEmpty(sValue) Then
            Return String.Empty
        Else
            Dim myInt As Int32 = 0
            Int32.TryParse(isSold, myInt)
            If (myInt <> 0) Then
                Return String.Format("<div class="" display_field""><em>{0}:  </em>  {1} (<span style=""color:red;display:inline;"">SOLD</span>)</div>", sLabel, sValue)
            Else
                Return String.Format("<div class="" display_field""><em>{0}:  </em>  {1}</div>", sLabel, sValue)
            End If
        End If
    End Function
    Public Function DisplayField(ByVal sLabel As String, ByVal sValue As String) As String
        If String.IsNullOrEmpty(sValue) Then
            Return String.Empty
        Else
            Return String.Format("<div class="" display_field""><em>{0}:  </em>  {1}</div>", sLabel, sValue)
        End If
    End Function
    Public Function DisplayNumericField(ByVal sLabel As String, ByVal sValue As String) As String
        If String.IsNullOrEmpty(sValue) Then
            Return String.Empty
        Else
            If IsNumeric(sValue) AndAlso CInt(sValue) > 0 Then
                Return String.Format("<div class="" display_field""><em>{0}:  </em>  {1}</div>", sLabel, sValue)
            Else
                Return String.Empty
            End If
        End If
    End Function
    Public Function DisplayNumericField(ByVal sLabel As String, ByVal sValue As String, ByVal ShowFiled As Boolean) As String
        If ShowFiled Then
            If String.IsNullOrEmpty(sValue) Then
                Return String.Format("<div class=""display_field""><em>{0}: </em>  {1}</div>", sLabel, "{not set}")
            Else
                If IsNumeric(sValue) AndAlso CInt(sValue) > 0 Then
                    Return String.Format("<div class=""display_field""><em>{0}: </em>  {1}</div>", sLabel, Math.Round(CDbl(sValue), 2).ToString("0,0.00", CultureInfo.InvariantCulture))
                Else
                    Return String.Empty
                End If
            End If
        Else
            Return String.Empty
        End If
    End Function
    Public Function DisplayField(ByVal sLabel As String, ByVal sValue As String, ByVal ShowFiled As Boolean) As String
        If ShowFiled Then
            If String.IsNullOrEmpty(sValue) Then
                Return String.Format("<div class="" display_field""><em>{0}:</em>  {1}</div>", sLabel, "{not set}")
            Else
                Return String.Format("<div class="" display_field""><em>{0}:</em>  {1}</div>", sLabel, sValue)
            End If
        Else
            Return String.Empty
        End If
    End Function

    Public Function DisplayLinkField(ByVal sLabel As String, ByVal sValue As String, ByVal sType As String, ByVal sIndex As String) As String
        If String.IsNullOrEmpty(sValue) Then
            Return String.Empty
        Else
            Return String.Format("<div class="" display_field""><em>{0}:</em>  <a title=""Filter by {1}"" href=""/MineralCollection/CollectionItemView.aspx?{2}={3}"" >  {1}</a></div>", sLabel, sValue, sType, sIndex)
        End If
    End Function
    Public Function DisplayPrimaryMineral(ByVal sLabel As String, ByVal sValue As String, ByVal sType As String, ByVal sIndex As String) As String
        If String.IsNullOrEmpty(sValue) Then
            Return String.Empty
        Else
            Return String.Format("<div class="" display_field""><em>{0}:</em><a title=""Filter by Mineral:{1}"" href=""/MineralCollection/CollectionItemView.aspx?{2}={3}"" >{1}</a></div>", sLabel, sValue, sType, sIndex)
        End If
    End Function
    Public Function DisplaySecondaryMinerals(ByRef myMineralList As EntitySet(Of CollectionItemMineral)) As String
        If myMineralList.Count > 0 Then
            Dim myReturn As New StringBuilder("<div class="" display_field""><em>Associated Minerals:</em>")
            For Each CoItemMineral In myMineralList
                myReturn.Append(String.Format("<a title=""Filter by Related Mineral:{0}"" href=""/MineralCollection/CollectionItemView.aspx?MineralID={1}"" >{0}</a><br/>", CoItemMineral.Mineral.MineralNM, CoItemMineral.Mineral.MineralID))
            Next
            myReturn.Append("</div>")
            Return myReturn.ToString()
        Else
            Return String.Empty
        End If
    End Function
    Public Function DisplayItemImage(ByVal ImageFileNM As String, ByVal CollectionItemID As String, ByVal MineralNM As String, ByVal SpecimenNumber As String) As String
        Dim sReturn As New StringBuilder(String.Empty)
        sReturn.Append(String.Format("<img src='{0}' class='img-responsive' />", GetThumbnailURL(ImageFileNM), CollectionItemID, MineralNM, SpecimenNumber))
        Return sReturn.ToString()
    End Function

    Public Function DisplayCollectionItems(ByRef myMineral As Mineral) As String
        Dim myReturn As New StringBuilder(String.Empty)
        myReturn.Append(String.Format("<section class='blue'><div class='content'><strong>{0}</strong><div class='slider related'>", myMineral.MineralNM))

        Using myCon As New DataController
            For Each i In myCon.usp_RelatedCollectionItem_SelectByMineralID(myMineral.MineralID)
                myReturn.Append(GetSpecimenPhotoLink(i.ImageFileNM, i.CollectionItemID, CInt(i.SpecimenNumber), i.Description))
            Next
        End Using
        myReturn.Append("</div></div></section>")
        Return myReturn.ToString()
    End Function
    Public Function DisplayRelatedMinerals(ByRef ci As CollectionItem) As String
        Dim myReturn As New StringBuilder(String.Empty)
        Dim myMineralNM As String = String.Empty
        Using myCon As New DataController
            For Each i In myCon.usp_RelatedCollectionItem_SelectByCollectionItemID(ci.CollectionItemID)
                If myMineralNM <> i.MineralNM Then
                    If myMineralNM = String.Empty Then
                        myReturn.Append(String.Format("<section class='blue'><div class='content'><strong>{0}</strong><div class='slider related'>", i.MineralNM))
                    Else
                        myReturn.Append("</div></div></section>")
                        myReturn.Append(String.Format("<section class='blue'><div class='content'><strong>{0}</strong><div class='slider related'>", i.MineralNM))
                    End If
                    myMineralNM = i.MineralNM
                End If
                myReturn.Append(GetSpecimenPhotoLink(i.ImageFileNM, i.CollectionItemID, CInt(i.SpecimenNumber), i.Description))

            Next
            myReturn.Append("</div></div></section>")
        End Using
        Return myReturn.ToString()
    End Function
    Private Function DisplayCollectionItemPhoto(ByRef myCollectionItem As CollectionItem) As String
        Dim myReturn As New StringBuilder(String.Empty)
        For Each myImage As CollectionItemImage In (From i In myCollectionItem.CollectionItemImages Order By i.DisplayOrder Ascending).ToList()
            If myImage.ImageType = "Photo" Then
                myReturn.Append(GetSpecimenPhotoLink(myImage.ImageFileNM, myCollectionItem.CollectionItemID, CInt(myCollectionItem.SpecimenNumber), myCollectionItem.Description))
                Exit For
            End If
        Next
        Return myReturn.ToString()
    End Function
    Private Function GetSpecimenPhotoLink(ByRef ImageFileNM As String, ByRef CollectionItemID As Integer, ByRef SpecimenNumber As Integer, ByRef Description As String) As String
        Return String.Format("<div>{1}<div class='image'>{1}<a href='/MineralCollection/CollectionItemView.aspx?CollectionItemID={2}'><img class='img-responsive' alt='Specimen #{4}' src='{3}' /><span data-tooltip aria-haspopup='true' class='has-tip' title='Specimen #{4} - {5}'>Specimen #{4}</span></a>{1}</div>{1}</div>{1}", ImageFileNM, vbCrLf, CollectionItemID, GetThumbnailURL(ImageFileNM), SpecimenNumber, Description)
    End Function
    Public Function BuildImageThumbnailListItem(ByVal LargeImageURL As String, ByVal ThumnailURL As String) As String
        Return String.Format("<li><a href=""/sites/nrc/images/{1}""><img data-zoom-image='/sites/nrc/images/{1}'  src='{2}'  style='max-width: 50px;max-height: 50px;'  onMouseOver=""javascript:setSpecLarge(this);""  ></a></li>{0}", vbCrLf, LargeImageURL, ThumnailURL)
    End Function
    Public Function DisplayImageList(ByRef myImageList As EntitySet(Of CollectionItemImage)) As String
        Dim myReturn As New StringBuilder(String.Empty)
        If myImageList.Count > 1 Then
            myReturn.Append("<div class='spec_images' >")
            myReturn.Append("<div class='spec_thumb row' ><ul class='clearing-thumbs' data-clearing>")
            For Each myImage As CollectionItemImage In (From i In myImageList Select i Order By i.DisplayOrder).ToList()
                If myImage.ImageType = "Photo" Then
                    myReturn.Append(BuildImageThumbnailListItem(myImage.ImageFileNM, wpmMineralCollection.Display.GetThumbnailURL(myImage.ImageFileNM)))
                End If
            Next
            For Each myImage As CollectionItemImage In (From i In myImageList Select i Order By i.DisplayOrder).ToList()
                If myImage.ImageType <> "Photo" Then
                    myReturn.Append(BuildImageThumbnailListItem(myImage.ImageFileNM, wpmMineralCollection.Display.GetThumbnailURL(myImage.ImageFileNM)))
                End If
            Next
            myReturn.Append("</ul></div>")
            myReturn.Append("<div class='row spec_large' >")
            For Each myImage As CollectionItemImage In (From i In myImageList Select i Order By i.DisplayOrder).ToList()
                If myImage.ImageType = "Photo" Then
                    myReturn.Append(String.Format("<img class='img_zoom' src='/sites/nrc/images/{1}' data-zoom-image='/sites/nrc/images/{1}' alt='{2}' /></a>", wpmMineralCollection.Display.GetThumbnailURL(myImage.ImageFileNM), myImage.ImageFileNM, myImage.ImageDS))
                    Exit For
                End If
            Next
            myReturn.Append("</div>")
            myReturn.Append("</div>")
        Else
            myReturn.Append("<div class='row spec_large' >")
            For Each myImage As CollectionItemImage In (From i In myImageList Select i Order By i.DisplayOrder).ToList()
                If myImage.ImageType = "Photo" Then
                    myReturn.Append(String.Format("<img class='img_zoom' src='/sites/nrc/images/{1}' data-zoom-image='/sites/nrc/images/{1}' alt='{2}' /></a>", wpmMineralCollection.Display.GetThumbnailURL(myImage.ImageFileNM), myImage.ImageFileNM, myImage.ImageDS))
                    Exit For
                End If
            Next
            myReturn.Append("</div>")
        End If
        Return myReturn.ToString()
    End Function

    Public Function GetThumbnailHTML(ByVal FileName As String) As String
        Dim myReturn As New StringBuilder
        myReturn.Append(String.Format("<img class='CollectionItemImage' src='{1}' >", FileName, GetThumbnailURL(FileName)))
        Return myReturn.ToString()
    End Function
    Public Function GetThumbnailHTML(ByVal FileName As String, ByVal iWidth As Integer) As String
        Dim myReturn As New StringBuilder
        myReturn.Append(String.Format("<img width='{1}' src='{2}' >", FileName, iWidth, GetThumbnailURL(FileName)))
        Return myReturn.ToString()
    End Function
    Public Function GetImageHTML(ByVal FileName As String) As String
        Dim myReturn As New StringBuilder
        myReturn.Append(String.Format("<a target='_blank' class='th' href='/sites/nrc/images/{0}'><img class='CollectionItemImage' src='{1}'></a>", FileName, GetThumbnailURL(FileName)))
        Return myReturn.ToString()
    End Function
    Public Function GetImageURL(ByVal FileName As String) As String
        Return String.Format("/sites/nrc/images/{0}", FileName)
    End Function
    Public Function GetThumbnailURL(ByVal FileName As String) As String
        If String.IsNullOrEmpty(FileName) Then
            Return "/admin/images/spacer.gif"
        Else
            Return String.Format("/sites/nrc/Thumbnails/{0}", FileName.ToLower().Replace(".jpg", ".png"))
        End If
    End Function
End Module