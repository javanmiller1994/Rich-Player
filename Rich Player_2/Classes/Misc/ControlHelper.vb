Public Module ControlHelper
  
#Region "Redraw Suspend/Resume"
    Private Declare Ansi Function SendMessage Lib "user32.dll" Alias "SendMessageA" (hwnd As IntPtr, wMsg As Integer, wParam As Integer, lParam As Integer) As Integer
    Private Const WM_SETREDRAW As Integer = &HB

    <System.Runtime.CompilerServices.Extension> _
    Public Sub SuspendDrawing(target As Control)
        SendMessage(target.Handle, WM_SETREDRAW, 0, 0)
    End Sub

    <System.Runtime.CompilerServices.Extension> _
    Public Sub ResumeDrawing(target As Control)
        ResumeDrawing(target, True)
    End Sub
    <System.Runtime.CompilerServices.Extension> _
    Public Sub ResumeDrawing(target As Control, redraw As Boolean)
        SendMessage(target.Handle, WM_SETREDRAW, 1, 0)

        If redraw Then
            target.Refresh()
        End If
    End Sub
#End Region
End Module