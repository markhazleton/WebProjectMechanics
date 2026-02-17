Imports WebProjectMechanics
Imports LINQHelper.System.Linq.Dynamic

Public Class MineralCollection_controls_SpecimenList
    Inherits ApplicationUserControl
    Implements IApplicationList


    Public Sub GetData(sWhere As String, OutputCSV As Boolean) Implements IApplicationList.GetData
        Dim myObjects As New List(Of Object)
        Using myCon As New wpmMineralCollection.DataController()
            If String.IsNullOrEmpty(sWhere) Then
                myObjects.AddRange((From i In myCon.vwCollectionItems Select i).ToList())
            Else
                myObjects.AddRange((From i In myCon.vwCollectionItems.Where(sWhere) Select i).ToList())
            End If
        End Using
        BuildDataTable(myObjects)
    End Sub

    Public Sub GetData1(FilterList As SQLFilterList, OutputCSV As Boolean) Implements IApplicationList.GetData
        Dim myObjects As New List(Of Object)
        Using myCon As New wpmMineralCollection.DataController()
            If String.IsNullOrEmpty(FilterList.GetLINQWhere) Then
                myObjects.AddRange((From i In myCon.vwCollectionItems Select i).ToList())
            Else
                myObjects.AddRange((From i In myCon.vwCollectionItems.Where(FilterList.GetLINQWhere) Select i).ToList())
            End If
        End Using
        BuildDataTable(myObjects)
    End Sub

    Private Sub BuildDataTable(ByRef myObjects As List(Of Object))
        Dim myTableHeader As New DisplayTableHeader() With {.TableTitle = "Specimens (<a href='/MineralCollection/Admin.aspx?Type=Specimen&CollectionItemID=0'>Add New Specimen</a> | <a href='/MineralCollection/ExportCollection.ashx' target='_blank' > Export Collection to CSV File</a> )",
                                                    .DetailFieldName = "SpecimenNumber",
                                                    .DetailKeyName = "CollectionItemID",
                                                    .DetailPath = "/MineralCollection/Admin.aspx?Type=Specimen&CollectionItemID={0}"}

        'myTableHeader.AddLinkHeaderItem("Thumbnail",
        '                                "ImageFileNM",
        '                                "/MineralCollection/Admin.aspx?Type=Specimen&CollectionItemID={0}",
        '                                "CollectionItemID",
        '                                "ThumbImageFileNM",
        '                                "/sites/nrc/thumbnails/")
        myTableHeader.AddHeaderItem("Sold", "IsSold", True, DisplayFormat.YesNo)
        myTableHeader.AddHeaderItem("Featured", "IsFeatured", True, DisplayFormat.YesNo)
        myTableHeader.AddHeaderItem("Nickname", "Nickname")
        myTableHeader.AddLinkHeaderItem("Primary Mineral", "MineralNM", "/MineralCollection/Admin.aspx?Type=Specimen&MineralID={0}", "PrimaryMineralID", "MineralNM")
        myTableHeader.AddLinkHeaderItem("Variety", "MineralVariety", "/MineralCollection/Admin.aspx?Type=Specimen&MineralVariety={0}", "MineralVariety", "MineralVariety")
        myTableHeader.AddLinkHeaderItem("Purchased From", "CompanyNM", "/MineralCollection/Admin.aspx?Type=Specimen&CompanyID={0}", "PurchasedFromCompanyID", "CompanyNM")
        myTableHeader.AddLinkHeaderItem("Show Where Purchased", "ShowWherePurchased", "/MineralCollection/Admin.aspx?Type=Specimen&ShowWherePurchased={0}", "ShowWherePurchased", "ShowWherePurchased")
        myTableHeader.AddLinkHeaderItem("Mine", "MineNM", "/MineralCollection/Admin.aspx?Type=Specimen&MineNM={0}", "MineNM", "MineNM")
        myTableHeader.AddLinkHeaderItem("Country", "CountryNM", "/MineralCollection/Admin.aspx?Type=Specimen&LocationCountryID={0}", "LocationCountryID", "CountryNM")
        dtList.BuildTable(myTableHeader, myObjects)


    End Sub
End Class
