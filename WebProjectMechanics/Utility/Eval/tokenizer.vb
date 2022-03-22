Public Class tokenizer
    Private mString As String
    Private mLen As Integer
    Private mPos As Integer
    Private mCurChar As Char
    Private mParser As parser
    Private mSyntax As eParserSyntax

    Public startpos As Integer
    Public type As eTokenType
    Public value As New System.Text.StringBuilder

    Friend Sub New(ByVal Parser As parser, ByVal str As String, Optional ByVal syntax As eParserSyntax = eParserSyntax.Vb)
        mString = str
        mLen = str.Length
        mSyntax = syntax
        mPos = 0
        mParser = Parser
        NextChar()   ' start the machine
    End Sub

    Friend Sub RaiseError(ByVal msg As String, Optional ByVal ex As Exception = Nothing)
        If TypeOf ex Is Evaluator.parserException Then
            msg &= ". " & ex.Message
        Else
            msg &= String.Format("  at position {0}", startpos)
            If Not ex Is Nothing Then
                msg &= ". " & ex.Message
            End If
        End If
        Throw New Evaluator.parserException(msg, Me.mString, Me.mPos)
    End Sub

    Friend Sub RaiseUnexpectedToken(Optional ByVal msg As String = Nothing)
        If Len(msg) = 0 Then
            msg = String.Empty
        Else
            msg &= "; "
        End If
        RaiseError(String.Format("{0}Unexpected {1} : {2}", msg, type.ToString().Replace("_"c, " "c), value))
    End Sub

    Friend Sub RaiseWrongOperator(ByVal tt As eTokenType, ByVal ValueLeft As Object, ByVal valueRight As Object, Optional ByVal msg As String = Nothing)
        If Len(msg) > 0 Then
            msg.Replace("[op]", tt.GetType.ToString)
            msg &= ". "
        End If
        msg = "Cannot apply the operator " & tt.ToString
        If ValueLeft Is Nothing Then
            msg &= " on nothing"
        Else
            msg &= " on a " & ValueLeft.GetType.ToString()
        End If
        If Not valueRight Is Nothing Then
            msg &= " and a " & valueRight.GetType.ToString()
        End If
        RaiseError(msg)
    End Sub

    'Private Function IsOp() As Boolean
    '    Return mCurChar = "+"c _
    '    Or mCurChar = "-"c _
    '    Or mCurChar = "�"c _
    '    Or mCurChar = "%"c _
    '    Or mCurChar = "/"c _
    '    Or mCurChar = "("c _
    '    Or mCurChar = ")"c _
    '    Or mCurChar = "."c
    'End Function

    Public Sub NextToken()
        value.Length = 0
        type = eTokenType.none
        Do
            startpos = mPos
            Select Case mCurChar
                Case Nothing
                    type = eTokenType.end_of_formula
                Case "0"c To "9"c
                    ParseNumber()
                Case "-"c, "�"c
                    NextChar()
                    type = eTokenType.operator_minus
                Case "+"c
                    NextChar()
                    type = eTokenType.operator_plus
                Case "*"c
                    NextChar()
                    type = eTokenType.operator_mul
                Case "/"c
                    NextChar()
                    type = eTokenType.operator_div
                Case "%"c
                    NextChar()
                    type = eTokenType.operator_percent
                Case "("c
                    NextChar()
                    type = eTokenType.open_parenthesis
                Case ")"c
                    NextChar()
                    type = eTokenType.close_parenthesis
                Case "<"c
                    NextChar()
                    If mCurChar = "="c Then
                        NextChar()
                        type = eTokenType.operator_le
                    ElseIf mCurChar = ">"c Then
                        NextChar()
                        type = eTokenType.operator_ne
                    Else
                        type = eTokenType.operator_lt
                    End If
                Case ">"c
                    NextChar()
                    If mCurChar = "="c Then
                        NextChar()
                        type = eTokenType.operator_ge
                    Else
                        type = eTokenType.operator_gt
                    End If
                Case ","c
                    NextChar()
                    type = eTokenType.comma
                Case "="c
                    NextChar()
                    type = eTokenType.operator_eq
                Case "."c
                    NextChar()
                    type = eTokenType.dot
                Case "'"c, """"c
                    ParseString(True)
                    type = eTokenType.value_string
                Case "#"c
                    ParseDate()
                Case "&"c
                    NextChar()
                    type = eTokenType.operator_concat
                Case "["c
                    NextChar()
                    type = eTokenType.open_bracket
                Case "]"c
                    NextChar()
                    type = eTokenType.close_bracket
                Case Chr(0) To " "c
                    ' do nothing
                Case Else
                    ParseIdentifier()
            End Select
            If type <> eTokenType.none Then Exit Do
            NextChar()
        Loop
    End Sub

    Private Sub NextChar()
        If mPos < mLen Then
            mCurChar = mString.Chars(mPos)
            If mCurChar = Chr(147) Or mCurChar = Chr(148) Then
                mCurChar = """"c
            End If
            If mCurChar = Chr(145) Or mCurChar = Chr(146) Then
                mCurChar = "'"c
            End If
            mPos += 1
        Else
            mCurChar = Nothing
        End If
    End Sub

    Private Sub ParseNumber()
        type = eTokenType.value_number
        While mCurChar >= "0"c And mCurChar <= "9"c
            value.Append(mCurChar)
            NextChar()
        End While
        If mCurChar = "."c Then
            value.Append(mCurChar)
            NextChar()
            While mCurChar >= "0"c And mCurChar <= "9"c
                value.Append(mCurChar)
                NextChar()
            End While
        End If
    End Sub

    Private Sub ParseIdentifier()
        While (mCurChar >= "0"c And mCurChar <= "9"c) _
            Or (mCurChar >= "a"c And mCurChar <= "z"c) _
            Or (mCurChar >= "A"c And mCurChar <= "Z"c) _
            Or (mCurChar >= "A"c And mCurChar <= "Z"c) _
            Or (mCurChar >= Chr(128)) _
            Or (mCurChar = "_"c)
            value.Append(mCurChar)
            NextChar()
        End While
        Select Case value.ToString
            Case "and"
                type = eTokenType.operator_and
            Case "or"
                type = eTokenType.operator_or
            Case "not"
                type = eTokenType.operator_not
            Case "true", "yes"
                type = eTokenType.value_true
            Case "if"
                type = eTokenType.operator_if
            Case "false", "no"
                type = eTokenType.value_false
            Case Else
                type = eTokenType.value_identifier
        End Select
    End Sub

    Private Sub ParseString(ByVal InQuote As Boolean)
        Dim OriginalChar As Char
        If InQuote Then
            OriginalChar = mCurChar
            NextChar()
        End If

        ' Dim PreviousChar As Char
        Do While mCurChar <> Nothing
            If InQuote AndAlso mCurChar = OriginalChar Then
                NextChar()
                If mCurChar = OriginalChar Then
                    value.Append(mCurChar)
                Else
                    'End of String
                    Exit Sub
                End If
            ElseIf mCurChar = "%"c Then
                NextChar()
                If mCurChar = "["c Then
                    NextChar()
                    Dim SaveValue As System.Text.StringBuilder = value
                    Dim SaveStartPos As Integer = startpos
                    value = New System.Text.StringBuilder
                    NextToken() ' restart the tokenizer for the subExpr
                    Dim subExpr As Object
                    subExpr = Nothing
                    Try
                        ' subExpr = mParser.ParseExpr(0, ePriority.none)
                        If subExpr Is Nothing Then
                            value.Append("<nothing>")
                        Else
                            value.Append(Evaluator.ConvertToString(subExpr))
                        End If
                    Catch ex As Exception
                        ' XML don't like < and >
                        value.Append(String.Format("[Error {0}]", ex.Message))
                    End Try
                    SaveValue.Append(value.ToString)
                    value = SaveValue
                    startpos = SaveStartPos
                Else
                    value.Append("%"c)
                End If
            Else
                value.Append(mCurChar)
                NextChar()
            End If
        Loop
        If InQuote Then
            RaiseError(String.Format("Incomplete string, missing {0}; String started", OriginalChar))
        End If
    End Sub

    Private Sub ParseDate()
        NextChar() ' eat the #
        ' Dim zone As Integer = 0
        While (mCurChar >= "0"c And mCurChar <= "9"c) Or (mCurChar = "/"c) Or (mCurChar = ":"c) Or (mCurChar = " "c)
            value.Append(mCurChar)
            NextChar()
        End While
        If mCurChar <> "#" Then
            RaiseError("Invalid date should be #dd/mm/yyyy#")
        Else
            NextChar()
        End If
        type = eTokenType.value_date
    End Sub

End Class
