Imports System.Windows.Forms
Imports System.Net
Imports System.IO
Imports System.ComponentModel
Imports Rich_Player.CsWinFormsBlackApp

Public Class UpdateDialog
    Dim Client As New WebClient()


    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Client.CancelAsync()
        ProgressBar1.EditValue = 0
        ProgressBar1.Visible = False
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel


        Form1.UpdateCanceled = True




    End Sub

    Dim str As String = My.Computer.FileSystem.SpecialDirectories.Temp.ToString & "\Rexford Rich\Rich Player\Rich Player - Upgrade Install.exe"

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        '   Process.Start("http://www.rexfordrich.com/files.html")
        '  Return
        If Not Directory.Exists(Path.GetDirectoryName(str)) Then
            Directory.CreateDirectory(Path.GetDirectoryName(str))
            If IO.File.Exists(str) = True Then
                IO.File.Delete(str)
            End If
        End If

        ' Dim client As New WebClient()
        AddHandler Client.DownloadProgressChanged, AddressOf ShowDownloadProgress
        AddHandler Client.DownloadFileCompleted, AddressOf OnDownloadComplete
        ' Dim result As Integer = MessageBox.Show("Would you like to try to download the quick update?" + Environment.NewLine + Environment.NewLine + _
        '                                         "Quick update does not guarantee complete installations of newly added components, but in most cases it " + _
        '                                         "will download just what you need." + Environment.NewLine + _
        '                                         "Quick update is good for slower internet connections.", "Try quick update?", MessageBoxButtons.YesNo)

        '  Dim result As Integer = MyFullMsgBox.Show("Would you like to try to download the quick update?" + Environment.NewLine + Environment.NewLine + _
        '                                          "Quick update does not guarantee complete installations of newly added components, but in most cases it " + _
        '                                          "will download just what you need." + Environment.NewLine + _
        '                                          "Quick update is good for slower internet connections." _
        '                                          + Environment.NewLine + "(To play it safe, click to download ""Regular Update"" version.)", _
        '                                          "Try quick update?", True, MyFullMsgBox.CustomButtons.YesNo)

        'If result = DialogResult.No Then
        Try
            Client.DownloadFileAsync(New Uri(Web_Update.UpdateUri), str)
        Catch ex As Exception
            Client.DownloadFileAsync(New Uri("https://www.dropbox.com/s/b4djfuown4xhxus/Rich%20Player%20-%20Upgrade.exe?dl=1"), str)
        End Try


        'ElseIf result = DialogResult.Yes Then
        'Client.DownloadFileAsync(New Uri(Web_Update.UpdateQuickUri), str)


        'End If


    End Sub

    Private Sub OnDownloadComplete(ByVal sender As Object, ByVal e As AsyncCompletedEventArgs)
        If Not e.Cancelled AndAlso e.Error Is Nothing Then
            MessageBox.Show("Download success." + Environment.NewLine + "IMPORTANT: If app fails to close, please close manually before installing!" + Environment.NewLine + "Failing to do so may result in loss of settings!")
            Process.Start(str)
            Application.Exit()
            Application.ExitThread()
        Else
            MessageBox.Show("Download failed")
            ProgressBar1.EditValue = 0
        End If
    End Sub

    Private Sub ShowDownloadProgress(ByVal sender As Object, ByVal e As DownloadProgressChangedEventArgs)
        ProgressBar1.Visible = True
        ProgressBar1.BringToFront()
        ProgressBar1.EditValue = e.ProgressPercentage
    End Sub





    Dim ReleasenotesPressed As Boolean = False
    Private Sub ReleaseNotesLabel_Click(sender As Object, e As EventArgs) Handles ReleaseNotesLabel.Click
        If ReleasenotesPressed = False Then
            Dim address As String = "https://www.dropbox.com/s/l8vejk5zoomdq93/Release%20Notes.txt?dl=1"
            ' "https://drive.google.com/uc?export=download&id=1hUty5AYdUZknvrRzt8YY2yffycr3CE3n"  '"http://www.rexfordrich.com/uploads/9/9/5/5/9955575/release_notes.txt"
            Dim client As WebClient = New WebClient()
            Dim reader As StreamReader = New StreamReader(client.OpenRead(address))

            ReleaseNotesLabel.Text = ReleaseNotesLabel.Text & Environment.NewLine & Environment.NewLine & reader.ReadToEnd
            ReleasenotesPressed = True
        Else
            ReleaseNotesLabel.Text = "￬ Click to view Release Notes:"
            ReleasenotesPressed = False
        End If


    End Sub

    Private Sub WebsiteBut_Click(sender As Object, e As EventArgs) Handles WebsiteBut.Click
        Process.Start("http://www.rexfordrich.com/files.html")
    End Sub

  
End Class