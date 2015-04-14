
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web

	<AttributeUsage(AttributeTargets.Method Or AttributeTargets.[Class])> _
	Public MustInherit Class HttpVerbAttribute
		Inherits Attribute

		Public MustOverride ReadOnly Property HttpVerb() As String

	End Class
