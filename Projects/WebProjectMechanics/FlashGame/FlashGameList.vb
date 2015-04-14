Imports System.Xml.Serialization
Imports System.IO
Imports System.Xml
Imports System.Text
Imports System.Web


Public Class FlashGameList
    Inherits List(Of FlashGame)
    Public Sub New()
        MyBase.New()
    End Sub
    Public Sub New(ByVal capacity As Integer)
        MyBase.New(capacity)
    End Sub
    Public Sub New(ByVal collection As IEnumerable(Of FlashGame))
        MyBase.New(collection)
    End Sub
    Private myLocation As Location = New Location()
    Private Property curRecordsPerLocation As Integer = 10
    Public Function AddNewGame(ByRef NewGame As FlashGame) As Boolean
        Dim bSkip As Boolean = False
        For Each mygame As FlashGame In Me
            If mygame.Name = NewGame.Name Then
                bSkip = True
            End If
        Next
        If Not bSkip Then
            Add(NewGame)
        End If
        Return True
    End Function

    Public Function LoadFlashList(ByRef theLocation As Location) As FlashGameList
        myLocation.CopyLocation(theLocation)

        Dim mySiteList As FlashGameList = New FlashGameList
        Try
            If FileProcessing.IsValidPath(GetIndexFilePath) Then
                Using sr As New StreamReader(GetIndexFilePath)
                    Try
                        Dim xs As New XmlSerializer(GetType(FlashGameList))
                        mySiteList = DirectCast(xs.Deserialize(sr), FlashGameList)
                    Catch ex As Exception
                        ApplicationLogging.ErrorLog("Error Loading FlashGameList", ex.Message)
                    End Try
                End Using
            End If
        Catch ex As Exception
            ApplicationLogging.ErrorLog("Error Loading FlashGameList", ex.Message)
        End Try

        If mySiteList.Count > 0 Then
            Me.Clear()
            For Each myGame As FlashGame In mySiteList
                Me.Add(myGame)
            Next
        End If
        Return mySiteList
    End Function

    Private Shared Function GetIndexFilePath() As String
        If Not FileProcessing.VerifyFolderExists(wpm_SiteConfig.ConfigFolderPath & "game") Then
            FileProcessing.CreateFolder(wpm_SiteConfig.ConfigFolderPath & "game")
        End If
        Return Replace(String.Format("{0}\game\{1}.xml", wpm_SiteConfig.ConfigFolderPath, "FlashGame"), "\\", "\")
    End Function


    Public Function SaveXML() As Boolean
        Dim bReturn As Boolean = True
        HttpContext.Current.Application.Lock()
        Try
            Using sw As New System.IO.StreamWriter(GetIndexFilePath(), False)
                Try
                    Dim ViewWriter As New XmlSerializer(GetType(FlashGameList))
                    ViewWriter.Serialize(sw, Me)
                Catch ex As Exception
                    ApplicationLogging.ErrorLog(String.Format("Error Saving File - {0}", ex), "FlashGameList.SaveXML")
                    bReturn = False
                End Try
            End Using
        Catch ex As Exception
            ApplicationLogging.ErrorLog(String.Format("Error Before Saving File  - {0}", ex), "FlashGameList.SaveXML")
            bReturn = False
        End Try
        HttpContext.Current.Application.UnLock()
        Return bReturn
    End Function

    Private Function ReadFromXML(xmlOfAnObject As String) As FlashGameList
        Dim myObject As New FlashGameList()
        Dim read As System.IO.StringReader = New StringReader(xmlOfAnObject)
        Dim serializer As New System.Xml.Serialization.XmlSerializer(myObject.[GetType]())
        Dim reader As System.Xml.XmlReader = New XmlTextReader(read)
        Try
            myObject = DirectCast(serializer.Deserialize(reader), FlashGameList)
            Return myObject
        Catch
            Throw
        Finally
            reader.Close()
            read.Close()
            read.Dispose()
        End Try
    End Function


    Public Function BuildGameList(ByRef sbBlogTemplate As StringBuilder, ByVal iCurrentPageNumber As Integer, ByVal reqGameName As String) As String
        Dim sItem As New StringBuilder
        Dim sURL As String = (String.Format("/default.aspx?c={0}&amp;a={1}", myLocation.LocationID, myLocation.ArticleID))
        Dim sSubPageNav As String = ("")
        Dim iPageCount As Integer = 0
        Dim iFirstDisplay As Integer = 0
        Dim iLastDisplay As Integer = 0
        Dim iPageNumber As Integer = 0

        Dim iRow As Integer = 0
        If (reqGameName = String.Empty) Then
            If Count = 0 Then
                ' No Rows
            Else
                ' Count number of Images for this PageID, SETUP FOR REMAINDER OF TASKS
                ' determine page break
                If (curRecordsPerLocation < 1) Then
                    curRecordsPerLocation = 1
                End If
                iPageCount = CInt(Math.Round(Val(Count / curRecordsPerLocation), 0))
                ' If last image is on a page break, then subtract one page to avoid an empty page
                If (iPageCount * curRecordsPerLocation) > Count Then
                    ' Do nothing we have the right number of pages
                Else
                    If (Count Mod curRecordsPerLocation) = 0 Then
                    Else
                        iPageCount = iPageCount + 1
                    End If
                End If
                ' Determine First and Last record to display
                If Count > 0 Then
                    If iCurrentPageNumber <= 1 Then
                        ' We are on the first page
                        iPageNumber = 1
                        iFirstDisplay = 0
                        iLastDisplay = curRecordsPerLocation - 1
                    Else
                        iPageNumber = iCurrentPageNumber
                        iFirstDisplay = ((iPageNumber * curRecordsPerLocation) - curRecordsPerLocation)
                        iLastDisplay = iFirstDisplay + curRecordsPerLocation - 1
                    End If
                End If
                ' Build Sub Page Navigation
                If iPageNumber > 1 Then
                    sSubPageNav = String.Format("{0}<a title=""PREVIOUS"" href=""{1}&amp;Page={2}""><::</a>", sSubPageNav, sURL, iPageNumber - 1)
                End If
                If (iPageCount > 1) Then
                    sSubPageNav = String.Format("{0}<b>Page {1} of {2}</b>", sSubPageNav, iPageNumber, iPageCount)
                End If
                If iPageNumber < iPageCount Then
                    sSubPageNav = sSubPageNav & "<a title=""NEXT"" href=""" & sURL & "&amp;Page=" & iPageNumber + 1 & """>::></a>"
                End If
                If sSubPageNav <> "" Then
                    sItem.Append(String.Format("<center>{0}</center>{1}{1}", sSubPageNav, vbCrLf))
                End If
                ' Draw The current page

                For Each myGame As FlashGame In Me
                    ' Determine if this record is displayed
                    If (iRow >= iFirstDisplay) Then
                        If (iRow <= iLastDisplay) Then
                            AddArticleWithParm(myGame, sItem, sbBlogTemplate)
                        End If
                    End If
                    iRow = iRow + 1
                Next
            End If
            sItem.Append(String.Format("<center>{0}</center>{1}{1}", sSubPageNav, vbCrLf))
            sItem.Append(vbCrLf)
        Else
            For Each myGame As FlashGame In Me
                If wpm_FormatNameForURL(myGame.Name) = reqGameName Then
                    AddArticleWithParm(myGame, sItem, sbBlogTemplate)
                    Exit For
                End If
            Next
        End If
        Return sItem.ToString
    End Function

    Private Function AddArticleWithParm(ByVal myGame As FlashGame, ByRef sItem As StringBuilder, ByRef sbBlogTemplate As StringBuilder) As Boolean
        sItem.Append(sbBlogTemplate.ToString)
        sItem.Replace("~~GameName~~", myGame.Name)
        sItem.Replace("~~GameThumbnail~~", myGame.thumbnail_large_url)
        sItem.Replace("~~GameDescription~~", myGame.Description)
        sItem.Replace("~~GameHeight~~", myGame.Height.ToString())
        sItem.Replace("~~GameWidth~~", myGame.Width.ToString())
        sItem.Replace("~~GameURL~~", myGame.GameURL)
        sItem.Replace("~~GameAuthor~~", myGame.Author)
        sItem.Replace("~~GamePageURL~~", String.Format("{0}&GameName={1}", myLocation.TransferURL, wpm_FormatNameForURL(myGame.Name)))
        Return True
    End Function

End Class
