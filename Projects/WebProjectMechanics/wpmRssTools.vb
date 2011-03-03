Imports System.Xml
Imports System.Text

Public Class wpmRssTools
    Public Interface IwpmRSS
        Function GetOneItem(ByVal itemnumber As Integer) As wpmRSSItem
        Function makefeed() As String
        Property FeedIsGood() As Boolean
        Property LastError() As String
        Property NuberOfItems() As Integer
        Property Items() As ArrayList
        Property Title() As String
        Property Link() As String
        Property Description() As String
        Property Language() As String
        Property ImageTitle() As String
        Property ImageLink() As String
        Property ImageURL() As String
        Property ImageDescription() As String
        Function getRSSFeed(ByVal templatePath As String) As String
    End Interface
    Public Class wpmRSS
        Implements IwpmRSS
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
                Title = mychan("title").InnerText
                Link = mychan("link").InnerText

                Try
                    Description = mychan("description").InnerText
                Catch
                    Description = ""
                End Try

                Try
                    Language = mychan("language").InnerText
                Catch
                    Language = ""
                End Try
                Try
                    Dim myimageinfo As XmlNode = mychan.SelectSingleNode("/rss/channel/image")
                    ImageTitle = myimageinfo("title").InnerText
                    ImageLink = myimageinfo("link").InnerText
                    ImageURL = myimageinfo("url").InnerText
                Catch
                    ImageTitle = ""
                    ImageLink = ""
                    ImageURL = ""
                End Try

                Dim myitems As XmlNodeList = mydoc.SelectNodes("/rss/channel/item")
                Dim myitemslist As New ArrayList
                Dim a As Integer = 1
                For Each myitem As XmlNode In myitems
                    Dim myenclos As XmlNode = mydoc.SelectSingleNode(String.Format("/rss/channel/item[{0}]", a))
                    Dim newitem As New wpmRSSItem() With {.Title = myitem("title").InnerText, .Link = myitem("link").InnerText}
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

                Items = myitemslist
                NuberOfItems = myitemslist.Count
                LastError = STR_None
                FeedIsGood = True

            Catch ex As Exception

                FeedIsGood = False
                LastError = ex.Message.ToString

            End Try

        End Sub


        Sub New()

            FeedIsGood = False

        End Sub

#End Region

#Region "Internal methodes and properties"
        Public Function GetOneItem(ByVal itemnumber As Integer) As wpmRSSItem Implements IwpmRSS.GetOneItem
            Dim newrssitem As New wpmRSSItem
            newrssitem = CType(Items(itemnumber), wpmRSSItem)
            Return newrssitem
        End Function

        Public Function makefeed() As String Implements IwpmRSS.makefeed
            Dim myfeed As New StringBuilder
            myfeed.Append("<?xml version='1.0' encoding='UTF-8' ?><rss version='2.0' xmlns:rdf='http://www.w3.org/1999/02/22-rdf-syntax-ns#' xmlns:cc='http://web.resource.org/cc/' xmlns:itunes='http://www.itunes.com/dtds/podcast-1.0.dtd'><channel>")
            myfeed.Append("This feature is not yet Implemented.")
            Return myfeed.ToString
        End Function
        Public Property FeedIsGood() As Boolean Implements IwpmRSS.FeedIsGood
        Public Property LastError() As String Implements IwpmRSS.LastError
        Public Property NuberOfItems() As Integer Implements IwpmRSS.NuberOfItems
#End Region

#Region "property info"
        Public Property Items() As ArrayList Implements IwpmRSS.Items
        Public Property Title() As String Implements IwpmRSS.Title
        Public Property Link() As String Implements IwpmRSS.Link
        Public Property Description() As String Implements IwpmRSS.Description
        Public Property Language() As String Implements IwpmRSS.Language

#End Region


#Region "image information"
        ' image information
        Public Property ImageTitle() As String Implements IwpmRSS.ImageTitle
        Public Property ImageLink() As String Implements IwpmRSS.ImageLink
        Public Property ImageURL() As String Implements IwpmRSS.ImageURL
        Public Property ImageDescription() As String Implements IwpmRSS.ImageDescription
#End Region


        Public Function getRSSFeed(ByVal templatePath As String) As String Implements IwpmRSS.getRSSFeed
            Dim sItem As New StringBuilder
            Dim myOutput As New StringBuilder
            Dim sbBlogTemplate As New StringBuilder
            If Not wpmFileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath(templatePath & "/BlogPostsTemplate.txt"), sbBlogTemplate) Then
                wpmFileProcessing.ReadTextFile(HttpContext.Current.Server.MapPath(String.Format("{0}/blog/BlogPostsTemplate.txt", wpmApp.Config.wpmWebHome)), sbBlogTemplate)
            End If
            If FeedIsGood Then
                For Each myitem As wpmRSSItem In Items
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
        Public Property HasEnclosure() As Boolean
        Public Property Enclosure() As String
        Public Property author() As String
        Public Property Category() As String
        Public Property Comments() As String
        Public Property guid() As String
        Public Property Source() As String
        Public Property pubDate() As String
        Public Property Description() As String
        Public Property content() As String
        Public Property Link() As String
        Public Property Title() As String
        Public Property thumbnailURL() As String
        Public Property thumbnailHeight() As String
        Public Property thumbnailWidth() As String
        Public Property mediaContent_URL() As String
        Public Property mediaContent_Type() As String
        Public Property mediaContent_Height() As String
        Public Property mediaContent_Width() As String
    End Class
End Class




