Imports System.Xml
Imports System.Web

Public Class wpmRssTools
    Public Class wpmRSS
        Const STR_None As String = "None"
#Region "constructors"
        ''' <summary>
        ''' Initializes a new instance of the wpmRSS class. You must enter a feed url as a string.
        ''' </summary>
        ''' <param name="feedurl"></param>
        Sub New(ByVal feedurl As String)
            Try
                Dim mydoc As New XmlDocument
                mydoc.Load(feedurl)
                Dim mychan As XmlNode = mydoc.SelectSingleNode("/rss/channel")
                _title = mychan("title").InnerText
                _link = mychan("link").InnerText

                Try
                    _description = mychan("description").InnerText
                Catch
                    _description = ""
                End Try

                Try
                    _language = mychan("language").InnerText
                Catch
                    _language = ""
                End Try
                Try
                    Dim myimageinfo As XmlNode = mychan.SelectSingleNode("/rss/channel/image")
                    _imageTitle = myimageinfo("title").InnerText
                    _imageLink = myimageinfo("link").InnerText
                    _imageURL = myimageinfo("url").InnerText
                Catch
                    _imageTitle = ""
                    _imageLink = ""
                    _imageURL = ""
                End Try

                Dim myitems As XmlNodeList = mydoc.SelectNodes("/rss/channel/item")
                Dim myitemslist As New ArrayList
                Dim a As Integer = 1
                For Each myitem As XmlNode In myitems
                    Dim myenclos As XmlNode = mydoc.SelectSingleNode("/rss/channel/item[" & a.ToString & "]")
                    Dim newitem As New wpmRSSItem()
                    newitem.Title = myitem("title").InnerText
                    newitem.Link = myitem("link").InnerText
                    Try
                        newitem.pubDate = myitem("pubDate").InnerText
                    Catch
                        newitem.pubDate = ""
                    End Try
                    Try
                        newitem.author = myitem("author").InnerText
                    Catch
                        newitem.author = ""
                    End Try
                    Try
                        newitem.Category = myitem("category").InnerText
                    Catch
                        newitem.Category = ""
                    End Try
                    Try
                        newitem.Comments = myitem("comments").InnerText
                    Catch
                        newitem.Comments = ""
                    End Try
                    Try
                        newitem.content = myitem("content").InnerText
                    Catch
                        newitem.content = ""
                    End Try
                    Try
                        newitem.thumbnailURL = myitem("media:thumbnail").Attributes("url").InnerText
                        newitem.thumbnailHeight = myitem("media:thumbnail").Attributes("height").InnerText
                        newitem.thumbnailWidth = myitem("media:thumbnail").Attributes("width").InnerText
                    Catch
                        newitem.thumbnailURL = ""
                        newitem.thumbnailHeight = ""
                        newitem.thumbnailWidth = ""
                    End Try

                    Try
                        newitem.mediaContent_URL = myitem("media:content").Attributes("url").InnerText
                        newitem.mediaContent_Height = myitem("media:content").Attributes("height").InnerText
                        newitem.mediaContent_Width = myitem("media:content").Attributes("width").InnerText
                        newitem.mediaContent_Type = myitem("media:content").Attributes("type").InnerText

                    Catch
                        newitem.mediaContent_URL = ""
                        newitem.mediaContent_Height = ""
                        newitem.mediaContent_Width = ""
                        newitem.mediaContent_Type = ""
                    End Try


                    Try
                        newitem.Description = myitem("description").InnerText
                    Catch
                        newitem.Description = ""
                    End Try

                    Try
                        newitem.Enclosure = myenclos("enclosure").Attributes("url").InnerText
                        newitem.HasEnclosure = True
                    Catch ex As Exception
                        newitem.Enclosure = ""
                        newitem.HasEnclosure = False
                    End Try

                    a += 1

                    myitemslist.Add(newitem)
                Next

                _items = myitemslist
                _nuberOfItems = myitemslist.Count
                _lastError = STR_None
                _feedIsGood = True

            Catch ex As Exception

                _feedIsGood = False
                _lastError = ex.Message.ToString

            End Try

        End Sub


        Sub New()

            _feedIsGood = False

        End Sub

#End Region

#Region "Internal methodes and properties"
        Public Function GetOneItem(ByVal itemnumber As Integer) As wpmRSSItem
            Dim newrssitem As New wpmRSSItem
            newrssitem = CType(_items(itemnumber), wpmRSSItem)
            Return newrssitem
        End Function

        Public Function makefeed() As String
            Dim myfeed As New System.Text.StringBuilder
            myfeed.Append("<?xml version='1.0' encoding='UTF-8' ?><rss version='2.0' xmlns:rdf='http://www.w3.org/1999/02/22-rdf-syntax-ns#' xmlns:cc='http://web.resource.org/cc/' xmlns:itunes='http://www.itunes.com/dtds/podcast-1.0.dtd'><channel>")
            myfeed.Append("This feature is not yet Implemented.")
            Return myfeed.ToString
        End Function

        Private _feedIsGood As Boolean
        Public Property FeedIsGood() As Boolean
            Get
                Return _feedIsGood
            End Get
            Set(ByVal Value As Boolean)
                _feedIsGood = Value
            End Set
        End Property

        Private _lastError As String
        Public Property LastError() As String
            Get
                Return _lastError
            End Get
            Set(ByVal Value As String)
                _lastError = Value
            End Set
        End Property

        Private _nuberOfItems As Integer
        Public Property NuberOfItems() As Integer
            Get
                Return _nuberOfItems
            End Get
            Set(ByVal Value As Integer)
                _nuberOfItems = Value
            End Set
        End Property
#End Region

#Region "property info"
        Private _items As ArrayList
        Public Property Items() As ArrayList
            Get
                Return _items
            End Get
            Set(ByVal Value As ArrayList)
                _items = Value
            End Set
        End Property

        Private _title As String
        Public Property Title() As String
            Get
                Return _title
            End Get
            Set(ByVal Value As String)
                _title = Value
            End Set
        End Property

        Private _link As String
        Public Property Link() As String
            Get
                Return _link
            End Get
            Set(ByVal Value As String)
                _link = Value
            End Set
        End Property

        Private _description As String
        Public Property Description() As String
            Get
                Return _description
            End Get
            Set(ByVal Value As String)
                _description = Value
            End Set
        End Property

        Private _language As String
        Public Property Language() As String
            Get
                Return _language
            End Get
            Set(ByVal Value As String)
                _language = Value
            End Set
        End Property

#End Region


#Region "image information"
        ' image information
        Private _imageTitle As String
        Public Property ImageTitle() As String
            Get
                Return _imageTitle
            End Get
            Set(ByVal Value As String)
                _imageTitle = Value
            End Set
        End Property

        Private _imageLink As String
        Public Property ImageLink() As String
            Get
                Return _imageLink
            End Get
            Set(ByVal Value As String)
                _imageLink = Value
            End Set
        End Property

        Private _imageURL As String
        Public Property ImageURL() As String
            Get
                Return _imageURL
            End Get
            Set(ByVal Value As String)
                _imageURL = Value
            End Set
        End Property

        Private _imageDescription As String
        Public Property ImageDescription() As String
            Get
                Return _imageDescription
            End Get
            Set(ByVal Value As String)
                _imageDescription = Value
            End Set
        End Property
#End Region


        Public Function getRSSFeed(ByVal templatePath As String) As String
            Dim sItem As New StringBuilder
            Dim myOutput As New StringBuilder
            Dim sbBlogTemplate As New StringBuilder
            If Not wpmFileIO.ReadTextFile(HttpContext.Current.Server.MapPath(templatePath & "/BlogPostsTemplate.txt"), sbBlogTemplate) Then
                wpmFileIO.ReadTextFile(HttpContext.Current.Server.MapPath("/wpm/blog/BlogPostsTemplate.txt"), sbBlogTemplate)
            End If
            If Me.FeedIsGood Then
                For Each myitem As wpmRSSItem In Me.Items
                    sItem.Append(sbBlogTemplate.ToString)
                    sItem.Replace("~~PostID~~", "")
                    sItem.Replace("~~PostDate~~", myitem.pubDate)
                    sItem.Replace("~~PostURL~~", myitem.Link)
                    sItem.Replace("~~PostName~~", myitem.Title)
                    sItem.Replace("~~PostTitle~~", myitem.Title)
                    sItem.Replace("~~PostBody~~", myitem.content)
                    sItem.Replace("~~PostDescription~~", myitem.Description)
                    sItem.Replace("~~BlogPageURL~~", myitem.Link)
                    sItem.Replace("~~BlogPageName~~", myitem.Title)
                    sItem.Replace("~~PostAuthor~~", myitem.author)
                    sItem.Replace("~~ArticleAdmin~~", "")
                Next
            End If
            Return sItem.ToString
        End Function
    End Class

    Public Class wpmRSSItem

        Public Sub New()
        End Sub

        Private _hasEnclosure As Boolean
        Public Property HasEnclosure() As Boolean
            Get
                Return _hasEnclosure
            End Get
            Set(ByVal Value As Boolean)
                _hasEnclosure = Value
            End Set
        End Property

        Private _enclosure As String
        Public Property Enclosure() As String
            Get
                Return _enclosure
            End Get
            Set(ByVal Value As String)
                _enclosure = Value
            End Set
        End Property
        Private _author As String
        Public Property author() As String
            Get
                Return _author
            End Get
            Set(ByVal value As String)
                _author = value
            End Set
        End Property
        Private _category As String
        Public Property Category() As String
            Get
                Return _category
            End Get
            Set(ByVal value As String)
                _category = value
            End Set
        End Property
        Private _comments As String
        Public Property Comments() As String
            Get
                Return _comments
            End Get
            Set(ByVal value As String)
                _comments = value
            End Set
        End Property
        Private _guid As String
        Public Property guid() As String
            Get
                Return _guid
            End Get
            Set(ByVal value As String)
                _guid = value
            End Set
        End Property
        Private _source As String
        Public Property Source() As String
            Get
                Return _source
            End Get
            Set(ByVal value As String)
                _source = value
            End Set
        End Property

        Private _pubDate As String
        Public Property pubDate() As String
            Get
                Return _pubDate
            End Get
            Set(ByVal Value As String)
                _pubDate = Value
            End Set
        End Property

        Private _description As String
        Public Property Description() As String
            Get
                Return _description
            End Get
            Set(ByVal Value As String)
                _description = Value
            End Set
        End Property
        Private _content As String
        Public Property content() As String
            Get
                Return _content
            End Get
            Set(ByVal value As String)
                _content = value
            End Set
        End Property
        Private _link As String
        Public Property Link() As String
            Get
                Return _link
            End Get
            Set(ByVal Value As String)
                _link = Value
            End Set
        End Property

        Private _title As String
        Public Property Title() As String
            Get
                Return _title
            End Get
            Set(ByVal Value As String)
                _title = Value
            End Set
        End Property
        Private _thumbnailURL As String
        Public Property thumbnailURL() As String
            Get
                Return _thumbnailURL
            End Get
            Set(ByVal value As String)
                _thumbnailURL = value
            End Set
        End Property
        Private _thumbnailHeight As String
        Public Property thumbnailHeight() As String
            Get
                Return _thumbnailHeight
            End Get
            Set(ByVal value As String)
                _thumbnailHeight = value
            End Set
        End Property
        Private _thumbnailWidth As String
        Public Property thumbnailWidth() As String
            Get
                Return _thumbnailWidth
            End Get
            Set(ByVal value As String)
                _thumbnailWidth = value
            End Set
        End Property
        Private _mediaContent_URL As String
        Public Property mediaContent_URL() As String
            Get
                Return _mediaContent_URL
            End Get
            Set(ByVal value As String)
                _mediaContent_URL = value
            End Set
        End Property
        Private _mediaContent_Type As String
        Public Property mediaContent_Type() As String
            Get
                Return _mediaContent_Type
            End Get
            Set(ByVal value As String)
                _mediaContent_Type = value
            End Set
        End Property
        Private _mediaContent_Height As String
        Public Property mediaContent_Height() As String
            Get
                Return _mediaContent_Height
            End Get
            Set(ByVal value As String)
                _mediaContent_Height = value
            End Set
        End Property
        Private _mediaContent_Width As String
        Public Property mediaContent_Width() As String
            Get
                Return _mediaContent_Width
            End Get
            Set(ByVal value As String)
                _mediaContent_Width = value
            End Set
        End Property
    End Class
End Class




