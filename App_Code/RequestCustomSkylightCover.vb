Public Class RequestCustomSkylightCover
    Private _Name As String
    Public Property Name() As String
        Get
            Return _Name
        End Get
        Set(ByVal value As String)
            _Name = mhUTIL.RemoveHtml(value)
        End Set
    End Property

    Private _Email As String
    Public Property Email() As String
        Get
            Return _Email
        End Get
        Set(ByVal value As String)
            _Email = mhUTIL.RemoveHtml(value)
        End Set
    End Property
    Private _Phone As String
    Public Property Phone() As String
        Get
            Return _Phone
        End Get
        Set(ByVal value As String)
            _Phone = mhUTIL.RemoveHtml(value)
        End Set
    End Property
    Private _Length As Double
    Public Property Length() As Double
        Get
            Return _Length
        End Get
        Set(ByVal value As Double)
            _Length = value
        End Set
    End Property
    Private _Width As Double
    Public Property Width() As Double
        Get
            Return _Width
        End Get
        Set(ByVal value As Double)
            _Width = value
        End Set
    End Property
    Private _Height As Double
    Public Property Height() As Double
        Get
            Return _Height
        End Get
        Set(ByVal value As Double)
            _Height = value
        End Set
    End Property

    Private _RetainerRingFlangeHeight As Double
    Public Property RetainerRingFlangeHeight() As Double
        Get
            Return _RetainerRingFlangeHeight
        End Get
        Set(ByVal value As Double)
            _RetainerRingFlangeHeight = value
        End Set
    End Property
    Private _DomeRise As Double
    Public Property DomeRise() As Double
        Get
            Return _DomeRise
        End Get
        Set(ByVal value As Double)
            _DomeRise = value
        End Set
    End Property
    Private _RoofFlangeClearance As Double
    Public Property RoofFlangeClearance() As Double
        Get
            Return _RoofFlangeClearance
        End Get
        Set(ByVal value As Double)
            _RoofFlangeClearance = value
        End Set
    End Property
    Private _ProductNumber As String
    Public Property ProductNumber() As String
        Get
            Return _ProductNumber
        End Get
        Set(ByVal value As String)
            _ProductNumber = value
        End Set
    End Property

    Public ReadOnly Property SquareInches() As Double
        Get
            Return Math.Round(Width * Length, 2)
        End Get
    End Property
    Private _ValidOrder As Boolean
    Public Property ValidOrder() As Boolean
        Get
            Return _ValidOrder
        End Get
        Set(ByVal value As Boolean)
            _ValidOrder = value
        End Set
    End Property
    Private _SkylightCoverProductInformation As String

    Public Property SkylightCoverProductInformation() As String
        Get
            Return _SkylightCoverProductInformation
        End Get
        Set(ByVal value As String)
            _SkylightCoverProductInformation = value
        End Set
    End Property
    Private _CatalogURL As String
    Public Property CatalogURL() As String
        Get
            Return _CatalogURL
        End Get
        Set(ByVal value As String)
            _CatalogURL = value
        End Set
    End Property
    Public Function CalculateCustomSkylight(ByVal strSource As String) As Boolean
        Dim sbContent As New StringBuilder()
        sbContent.Append("<br/>")
        sbContent.Append("Your Custom Skylight Cover is " & SquareInches.ToString & " square inches.<br/>")
        ValidOrder = True
        If DomeRise > 6 Then
            sbContent.Append("The Dome Rise height is too big for an online estimate, please call for more information.")
            ValidOrder = False
        Else
            sbContent.Append("<br/>")
            'If Square Inches less than 785 --> Price is $95
            'If Square Inches between 785 and 1025 --> Price is $125
            'If Square Inches between 1026 and 1445 --> Price is $135
            'If Square Inches between 1446 and 2500 --> Price is $145
            'If Square Inches between 2501 and 2600 --> Price is $155
            'If Square Inches between 2601 and 2700 --> Price is $160
            'If Square Inches greater than 2700 --> Call for more details (price will be SQ Inches * .06 )
            Select Case SquareInches
                Case Is < 785
                    CatalogURL = "http://shop.acoolerhouse.com/product.sc?categoryId=5&productId=9&SourceCode=" & strSource
                    sbContent.Append("<strong>Price: $ 95</strong><br/>")
                    ProductNumber = "0000020"
                Case Is < 1026
                    CatalogURL = "http://shop.acoolerhouse.com/product.sc?categoryId=5&productId=10&SourceCode=" & strSource
                    sbContent.Append("<strong>Price: $ 125</strong><br/>")
                    ProductNumber = "0000021"
                Case Is < 1446
                    CatalogURL = "http://shop.acoolerhouse.com/product.sc?categoryId=5&productId=11&SourceCode=" & strSource
                    sbContent.Append("<strong>Price: $ 135</strong><br/>")
                    ProductNumber = "0000022"
                Case Is < 2501
                    CatalogURL = "http://shop.acoolerhouse.com/product.sc?categoryId=5&productId=12&SourceCode=" & strSource
                    sbContent.Append("<strong>Price: $ 145</strong><br/>")
                    ProductNumber = "0000023"
                Case Is < 2601
                    CatalogURL = "http://shop.acoolerhouse.com/product.sc?categoryId=5&productId=13&SourceCode=" & strSource
                    sbContent.Append("<strong>Price: $ 155</strong><br/>")
                    ProductNumber = "0000024"
                Case Is < 2701
                    CatalogURL = "http://shop.acoolerhouse.com/product.sc?categoryId=5&productId=14&SourceCode=" & strSource
                    sbContent.Append("<strong>Price: $ 160</strong><br/>")
                    ProductNumber = "0000025"
                Case Else
                    CatalogURL = ""
                    sbContent.Append("The size is too big for an online estimate, please call for more information.")
                    ValidOrder = False
            End Select
        End If
        SkylightCoverProductInformation = (sbContent.ToString)
        sbContent = Nothing
        Return ValidOrder
    End Function
    Public Function FormatEmailNotification(ByRef mySiteMap As mhSiteMap) As String
        Dim sbReturn As New StringBuilder(String.Empty)
        mhfio.ReadFile(HttpContext.Current.Server.MapPath(mySiteMap.mySession.SiteGallery & "/CustomSkylightTemplate.html"), sbReturn)
        mySiteMap.BuildTemplate(sbReturn)
        sbReturn.Replace("~~RequestName~~", Me.Name)
        sbReturn.Replace("~~RequestEmail~~", Me.Email)
        sbReturn.Replace("~~RequestPhone~~", Me.Phone)
        sbReturn.Replace("~~TextProductInformation~~", mhUTIL.RemoveHtml(Me.SkylightCoverProductInformation))
        sbReturn.Replace("~~ProductInformation~~", Me.SkylightCoverProductInformation)
        sbReturn.Replace("~~SkylightLength~~", Me.Length)
        sbReturn.Replace("~~SkylightWidth~~", Me.Width)
        sbReturn.Replace("~~SkylightDomeRise~~", Me.DomeRise)
        sbReturn.Replace("~~TotalSquareInches~~", Me.SquareInches)
        sbReturn.Replace("~~SkylightProductNumber~~", Me.ProductNumber)
        sbReturn.Replace("~~RoofFlangeClearance~~", Me.RoofFlangeClearance)
        sbReturn.Replace("~~RetainerRingFlangeHeight~~", Me.RetainerRingFlangeHeight)


        Return sbReturn.ToString
    End Function
End Class