Option Strict On

Option Explicit On

Imports System.Text
Imports System.Reflection

Namespace System.Linq.Dynamic
    Public MustInherit Class DynamicClass
        Public Overrides Function ToString() As String
            Dim props = Me.GetType().GetProperties(BindingFlags.Instance Or BindingFlags.Public)
            Dim sb As New StringBuilder()
            sb.Append("{")
            For i As Integer = 0 To props.Length - 1
                If (i > 0) Then sb.Append(", ")
                sb.Append(props(i).Name)
                sb.Append("=")
                sb.Append(props(i).GetValue(Me, Nothing))
            Next i

            sb.Append("}")

            Return sb.ToString()
        End Function
    End Class
End Namespace
