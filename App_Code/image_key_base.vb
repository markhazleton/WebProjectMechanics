Imports System
Imports System.Text
Namespace PMGEN

	' make it serializable
	<Serializable> _ 
	Public MustInherit Class Imagekey_base

		' ****************
		' * Constructor
		' ****************

		Public Sub New()
		End Sub
		Public Sub SetupValues(ByVal values() As String)
			If values Is Nothing OrElse values.Length <> 1 Then
				Throw New Exception("Invalid parameters")
			End If
			Try
			If (Not String.IsNullOrEmpty(values(0))) Then
				ImageID = Convert.ToInt32(values(0))
			End If
			Catch e As Exception
				Throw New Exception("Invalid parameters. Cannot setup key values. Please check the values of master row.")
			End Try
		End Sub

		''' <summary>
		''' ImageID Field
		''' </summary>

		Private _ImageID As Int32
		Public Overridable Property ImageID As Int32
			Get 
				return _ImageID
			End Get
			Set 
				_ImageID = value
			End Set
		End Property

		''' <summary>
		''' Determines whether the specified object is equal to the current instance.
		''' </summary>

		Public Overloads Function Equals(ByVal obj As Imagekey_base) As Boolean
			If (obj Is Nothing) Then Return False
			Dim isEquals As Boolean = True
			isEquals = ((Me.ImageID = obj.ImageID) AndAlso isEquals)
			Return isEquals
		End Function

		''' <summary>
		''' Returns a System.String that represents the current key
		''' </summary>

		Public Overrides Function ToString() As String
			Dim result As StringBuilder = New StringBuilder()
			If (result.Length > 0) Then result.Append(vbCr)
			result.Append(Me.ImageID)
			Return result.ToString()
		End Function
	End Class
End Namespace