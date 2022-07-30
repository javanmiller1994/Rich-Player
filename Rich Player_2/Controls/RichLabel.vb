Imports System.ComponentModel
Imports System.Windows.Forms

Public Class RichLabel
    Inherits Label

    Private Const WM_LBUTTONDCLICK As Integer = &H203

    Private clipboardText As String

    <DefaultValue(False)> _
    <Description("Overrides default behavior of Label to copy label text to clipboard on double click")> _
    Public Property CopyTextOnDoubleClick As Boolean

    Protected Overrides Sub OnDoubleClick(e As System.EventArgs)
        If Not String.IsNullOrEmpty(clipboardText) Then Clipboard.SetData(DataFormats.Text, clipboardText)
        clipboardText = Nothing
        MyBase.OnDoubleClick(e)
    End Sub

    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        If Not CopyTextOnDoubleClick Then
            If m.Msg = WM_LBUTTONDCLICK Then
                Dim d As IDataObject = Clipboard.GetDataObject()
                If d.GetDataPresent(DataFormats.Text) Then
                    clipboardText = d.GetData(DataFormats.Text)
                End If
            End If
        End If

        MyBase.WndProc(m)
    End Sub

End Class