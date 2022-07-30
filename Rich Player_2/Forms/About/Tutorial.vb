Public Class Tutorial

#Region " Graphics"

#Region " Labels"

    Dim labelHover As Boolean = False
    Private Sub label_ShortcutKeys_MouseEnter(sender As Object, e As EventArgs) Handles label_ShortcutKeys.MouseEnter, label_Songs.MouseEnter, label_Playlist.MouseEnter, _
        label_Media.MouseEnter, label_Edit.MouseEnter, label_View.MouseEnter, label_Settings.MouseEnter
        labelHover = True
        sender.BackColor = Color.FromArgb(100, 100, 100)
    End Sub

    Private Sub label_ShortcutKeys_MouseLeave(sender As Object, e As EventArgs) Handles label_ShortcutKeys.MouseLeave, label_Songs.MouseLeave, label_Playlist.MouseLeave, _
        label_Media.MouseLeave, label_Edit.MouseLeave, label_View.MouseLeave, label_Settings.MouseLeave
        labelHover = False
        sender.BackColor = Color.Transparent
    End Sub

    Private Sub label_ShortcutKeys_MouseDown(sender As Object, e As MouseEventArgs) Handles label_ShortcutKeys.MouseDown, label_Songs.MouseDown, label_Playlist.MouseDown, _
        label_Media.MouseDown, label_Edit.MouseDown, label_View.MouseDown, label_Settings.MouseDown
        sender.BackColor = Color.FromArgb(75, 75, 75)
    End Sub

    Private Sub label_ShortcutKeys_MouseUp(sender As Object, e As MouseEventArgs) Handles label_ShortcutKeys.MouseUp, label_Songs.MouseUp, label_Playlist.MouseUp, _
        label_Media.MouseUp, label_Edit.MouseUp, label_View.MouseUp, label_Settings.MouseUp
        If labelHover Then
            sender.BackColor = Color.FromArgb(100, 100, 100)
        Else
            sender.BackColor = Color.Transparent
        End If
    End Sub

#End Region


#End Region
    Public Function IsOnScreen(ByVal form As Form) As Boolean
        Dim screens() As Screen = Screen.AllScreens
        For Each scrn As Screen In screens
            Dim formRectangle As Rectangle = New Rectangle(form.Left, form.Top, form.Width, form.Height)
            If scrn.WorkingArea.Contains(formRectangle) Then
                Return True
            End If
        Next
        Return False
    End Function

    Dim CurrentSelection As String = "Home"
    Private Sub but_Close_Click(sender As Object, e As EventArgs) Handles but_Close.Click
        Select Case CurrentSelection
            Case "Home"
                Me.Close()
            Case "Songs"
                but_Close.Text = "Close"
                panel_Songs.Visible = False
                CurrentSelection = "Home"
            Case "Shortcut Keys"
                but_Close.Text = "Close"
                panel_ShortcutKeys.Visible = False
                CurrentSelection = "Home"
            Case "Playlists"
                but_Close.Text = "Close"
                panel_Playlists.Visible = False
                CurrentSelection = "Home"

            Case "Media"
                but_Close.Text = "Close"
                Panel_Media.Visible = False
                CurrentSelection = "Home"

            Case "Edit"
                but_Close.Text = "Close"
                Panel_Edit.Visible = False
                CurrentSelection = "Home"

            Case "View"
                but_Close.Text = "Close"
                Panel_View.Visible = False
                CurrentSelection = "Home"

            Case "Settings"
                but_Close.Text = "Close"
                panel_Settings.Visible = False
                CurrentSelection = "Home"


        End Select

        but_Close.BringToFront()
    End Sub

    
    Private Sub label_Songs_Click(sender As Object, e As EventArgs) Handles label_Songs.Click, label_ShortcutKeys.Click, label_Playlist.Click, label_Media.Click, _
        label_Edit.Click, label_View.Click, label_Settings.Click
        Select Case sender.name
            Case "label_ShortcutKeys"
                CurrentSelection = "Shortcut Keys"
                panel_ShortcutKeys.Visible = True
                panel_ShortcutKeys.BringToFront()
            Case "label_Songs"
                CurrentSelection = "Songs"
                panel_Songs.Visible = True
                panel_Songs.BringToFront()
            Case "label_Playlist"
                CurrentSelection = "Playlists"
                panel_Playlists.Visible = True
                panel_Playlists.BringToFront()

            Case "label_Media"
                CurrentSelection = "Media"
                Panel_Media.Visible = True
                Panel_Media.BringToFront()

            Case "label_Edit"
                CurrentSelection = "Edit"
                Panel_Edit.Visible = True
                Panel_Edit.BringToFront()

            Case "label_View"
                CurrentSelection = "View"
                Panel_View.Visible = True
                Panel_View.BringToFront()

            Case "label_Settings"
                CurrentSelection = "Settings"
                Panel_Settings.Visible = True
                Panel_Settings.BringToFront()

        End Select

        but_Close.Text = "Back"
        but_Close.BringToFront()


    End Sub

    Private Sub Tutorial_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Panel_View.Parent = Me
        Panel_Edit.Parent = Me
        Panel_Media.Parent = Me
        Panel_Settings.Parent = Me

        panel_Songs.Parent = Me
        panel_ShortcutKeys.Parent = Me
        panel_Playlists.Parent = Me
        Do Until IsOnScreen(Me)
            Dim i As Integer = Me.Top - 1
            Me.Top = i
        Loop

    End Sub


End Class