Imports System.Net
Imports System.Web.HttpUtility
Imports System.Drawing
Public Class AboutForm
    Private Sub PreVentFlicker()
        With Me
            .SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
            .SetStyle(ControlStyles.UserPaint, True)
            .SetStyle(ControlStyles.AllPaintingInWmPaint, True)
            .UpdateStyles()
        End With
    End Sub

   
    Private Sub AboutForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PreVentFlicker()
        but_ReleaseNotes.Location = New Drawing.Point(16, 88)
        but_ReleaseNotes.Size = New Drawing.Size(149, 28)
        but_ReleaseNotes.Font = New Font("Segoe UI", 12)
        but_Tutorial.Location = New Drawing.Point(171, 88)
        but_Tutorial.Size = New Drawing.Size(149, 28)
        but_Tutorial.Font = New Font("Segoe UI", 12)
        PictureBox1.Location = New Point(415, 9)
        PictureBox1.Size = New Size(197, 197)
        PictureBox2.Location = New Point(103, 42)
        PictureBox2.Size = New Size(40, 40)
        Label1.Location = New Point(12, 130)
    End Sub

    Private Sub but_ReleaseNotes_Click(sender As Object, e As EventArgs) Handles but_ReleaseNotes.Click
        Dim xform As New ReleaseNotes
        xform.ShowDialog()
    End Sub


    Private Sub but_Tutorial_Click(sender As Object, e As EventArgs) Handles but_Tutorial.Click
        Dim xform As New Tutorial
        xform.StartPosition = FormStartPosition.Manual
        xform.Location = New System.Drawing.Point(Me.Location.X + Me.Width / 2 - xform.Width / 2, Me.Location.Y + Me.Height / 2 - xform.Height / 2)
        xform.Show()
        xform.BringToFront()
        Me.Close()
        xform.TopMost = True
    End Sub
End Class