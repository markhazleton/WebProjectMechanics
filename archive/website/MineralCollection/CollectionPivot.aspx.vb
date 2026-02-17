Imports WebProjectMechanics
Imports wpmMineralCollection

Partial Class MineralCollection_CollectionPivot
    Inherits AdminPage

        Public Property PivotParms As String = "derivedAttributes: {},"
    Public Property PivotParmList As PivotParameterList
        Get
            If Session("PivotParmList") Is Nothing Then
                Dim myParmList As New PivotParameterList
                Try
                    myParmList = myParmList.GetXML(Server.MapPath("/App_Data/MineralPivotParameterList.xml"))
                Catch 
                    myParmList = New PivotParameterList
                End Try
                Session("PivotParmList") = myParmList
            End If
            Return CType(Session("PivotParmList"), PivotParameterList)
        End Get
        Set(value As PivotParameterList)
            Session("PivotParmList") = value
        End Set
    End Property


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not IsPostBack Then
            PivotParms = PivotParmList.GetParameterString("MineralCollection")
        End If
        

        SetPageName("Collection Pivot")
        Using myCon As New DataController()
            Dim myList = (From i In myCon.vwCollectionItems Select i.City,i.CollectionItemID,i.CollectionNM,i.CompanyNM,i.CountryNM, i.Description,i.DisplayOrder, i.IsFeatured, i.MineNM,i.PrimaryMineralNM, i.Nickname,i.PurchasePrice, i.SalePrice, i.Value, i.SpecimenNumber, i.StorageLocation).ToList


            Dim myTableHeader As New DisplayTableHeader() With {.TableTitle = "Collection Pivot",
                                                                .DetailFieldName = "ImageFileName",
                                                                .DetailKeyName = "ImageFileName",
                                                                .DetailPath = "/admin/catalog/default.aspx?ImageFileNM={0}"}
            myTableHeader.AddLinkHeaderItem("Thumbnail", "ImageFileName", "/admin/catalog/default.aspx?ImageFileName={0}", "ImageFileName", "ImageFileName")
            myTableHeader.AddHeaderItem("Image Name", "ImageName")
            myTableHeader.AddHeaderItem("Image Description", "ImageDescription")
            dtList.BuildTable(myList)
        End Using

    End Sub

        Protected Sub cmd_ReadJSON_Click(sender As Object, e As EventArgs)
        Dim myJSON As New JSONObject(tbJSON.Text)
        Dim myParms = New PivotParameter() With {.Name = "MineralCollection", .CSVFile = "MineralCollection"}
        For Each mycol As JSONValue In myJSON.GetProperty("cols").Value
            myParms.Cols.Add(mycol.Value())
        Next
        For Each mycol As JSONValue In myJSON.GetProperty("rows").Value
            myParms.Rows.Add(mycol.Value())
        Next
        For Each mycol As JSONValue In myJSON.GetProperty("vals").Value
            myParms.Vals.Add(mycol.Value())
        Next
        myParms.AggregatorName = myJSON.GetProperty("aggregatorName").Value
        myParms.rendererName = myJSON.GetProperty("rendererName").Value

        PivotParmList.AddToList(myParms)
        PivotParmList.SaveXML(Server.MapPath("/App_Data/MineralPivotParameterList.xml"))
        PivotParms = myParms.GetPivotParm
    End Sub



End Class
