<%@ WebHandler Language="VB" Class="view_item" %>

Imports System
Imports System.Web
Imports System.Linq
Imports LINQHelper
Imports wpmMineralCollection

Public Class view_item : Implements IHttpHandler, System.Web.SessionState.IRequiresSessionState

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        context.Response.ContentType = "text/html"
        Dim Specimen As String = context.Request.QueryString("Specimen")

        If CType(context.Session("wpm_MineralListView"), MineralCollectionListView) Is Nothing Then
            context.Session("wpm_MineralListView") = New MineralCollectionListView
        End If

        Dim myMineralList As MineralCollectionListView = CType(context.Session("wpm_MineralListView"), MineralCollectionListView)

        myMineralList.CurrentCollectionItemID = Specimen
        Dim myItem As MineralCollectionItem = myMineralList.GetCurrentItem()

        If myItem Is Nothing Then
            context.Response.Redirect("/")
        End If

        context.Response.Write(GetDiv("cbp-l-project-title", String.Format("Speciman {0}", myItem.SpecimenNumber)))

        context.Response.Write("<div Class=""row"">")
        context.Response.Write("<div Class=""col-md-6"">")


        context.Response.Write("<div Class=""cbp-l-project-container"">")
        context.Response.Write("<div Class=""cbp-l-project-desc"">")
        context.Response.Write("<div Class=""cbp-l-project-desc-text"">")
        context.Response.Write(GetP("", String.Format("Primary Mineral: <strong>{0}</strong>", myItem.PrimaryMineralNM)))
        context.Response.Write(GetP("", myItem.Description))
        context.Response.Write("</div>")
        context.Response.Write("</div>")

        context.Response.Write("<div Class=""cbp-l-project-details"">")
        context.Response.Write("<ul Class=""cbp-l-project-details-list"">")
        context.Response.Write(GetLI("", String.Format("<strong>Primary Mineral:</strong>{0}", myItem.PrimaryMineralNM)))
        context.Response.Write(GetLI("", String.Format("<strong>City:</strong>{0}", myItem.City)))
        context.Response.Write(GetLI("", String.Format("<strong>State:</strong>{0}", myItem.StateNM)))
        context.Response.Write("</ul>")

        If (myItem.WidthIn > 0) Then
            context.Response.Write("<ul Class=""cbp-l-project-details-list"">")
            context.Response.Write(GetLI("", String.Format("<strong>Width (IN):</strong>{0}", myItem.WidthIn)))
            context.Response.Write(GetLI("", String.Format("<strong>Height (IN):</strong>{0}", myItem.HeightIn)))
            context.Response.Write(GetLI("", String.Format("<strong>Weight (GR):</strong>{0}", myItem.WeightGr)))
            context.Response.Write("</ul>")
        End If




        context.Response.Write("</div>")
        context.Response.Write("</div>")






        context.Response.Write("</div>")
        context.Response.Write("<div Class=""col-md-6"">")
        context.Response.Write("<div Class=""cbp-slider"">")
        context.Response.Write("<ul Class=""cbp-slider-wrap"">")

        If myItem.Images.Count = 0 Then
            context.Response.Write("<li Class=""cbp-slider-item"">")
            context.Response.Write(String.Format("<img class=""img-responsive"" src=""{0}"" alt""{1}"" >", GetImageURL(myItem.ImageFileNM), myItem.Description))
            context.Response.Write("</li>")

        Else
            For Each myImage In myItem.Images
                context.Response.Write("<li Class=""cbp-slider-item"">")
                context.Response.Write(String.Format("<img class=""img-responsive""  src=""{0}"" alt""{1}"" >", GetImageURL(myImage.ImageFileNM), myImage.ImageDS))
                context.Response.Write("</li>")
            Next

        End If



        context.Response.Write("</ul>")
        context.Response.Write("</div>")
        context.Response.Write("</div>")
        context.Response.Write("</div>")



    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

    Private Function GetDiv(cssclass As String, content As String) As String
        Return String.Format("<div class=""{0}"">{1}</div>", cssclass, content)
    End Function
    Private Function GetP(cssclass As String, content As String) As String
        Return String.Format("<p class=""{0}"">{1}</p>", cssclass, content)
    End Function
    Private Function GetLI(cssclass As String, content As String) As String
        Return String.Format("<li class=""{0}"">{1}</li>", cssclass, content)
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

    Public Function GetThumnailClassByOrder(ByVal iOrder As Integer) As String
        If iOrder = 0 Then
            Return " selected "
        Else
            Return " "
        End If
    End Function
    Public Function GetCaroselImageClassByOrder(ByVal iOrder As Integer) As String
        If iOrder = 0 Then
            Return " active item "
        Else
            Return " item "
        End If
    End Function

    Public Function GetImageList(ByRef myImageList As List(Of CollectionItemImage)) As List(Of CollectionItemImage)
        Return (From i In myImageList Order By i.DisplayOrder Select i).ToList
    End Function



End Class