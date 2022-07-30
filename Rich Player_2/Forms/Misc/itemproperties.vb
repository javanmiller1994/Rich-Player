Imports System.IO


Public Class itemproperties


    Private Sub itemproperties_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PreVentFlicker()

        'If CsWinFormsBlackApp.Form1.XtraTabControl1.SelectedTabPageIndex = 0 Then
        Titlelabel.Text = CsWinFormsBlackApp.Form1._title
        artistlabel.Text = CsWinFormsBlackApp.Form1._Artist
        albumlabel.Text = CsWinFormsBlackApp.Form1._Album
        locationlabel.Text = CsWinFormsBlackApp.Form1.filelocation
        ' descriptionlabel.Text = CsWinFormsBlackApp.Form1._tracknum
        durationlabel.Text = CsWinFormsBlackApp.Form1._Duration
        Genrelabel.Text = CsWinFormsBlackApp.Form1._genre
        datelabel.Text = CsWinFormsBlackApp.Form1._date
        tracknumlabel.Text = CsWinFormsBlackApp.Form1._tracknum
        ' trackidlabel.Text = CsWinFormsBlackApp.Form1._trackid
        encodelabel.Text = CsWinFormsBlackApp.Form1._encode
        publisherlabel.Text = CsWinFormsBlackApp.Form1._publisher
        '  nowplayinglabel.Text = CsWinFormsBlackApp.Form1._nowplaying
        ratinglabel.Text = CsWinFormsBlackApp.Form1._rating
        filesize.Text = CsWinFormsBlackApp.Form1._filesize
    
    End Sub
    Private Sub PreVentFlicker()
        With Me
            .SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
            .SetStyle(ControlStyles.UserPaint, True)
            .SetStyle(ControlStyles.AllPaintingInWmPaint, True)
            .UpdateStyles()
        End With
    End Sub
    'Tooltips
    Private Sub locationlabel_MouseHover(sender As Object, e As EventArgs) Handles locationlabel.MouseHover
        ToolTip1.SetToolTip(locationlabel, locationlabel.Text)
    End Sub
    Private Sub Titlelabel_MouseHover(sender As Object, e As EventArgs) Handles Titlelabel.MouseHover
        ToolTip1.SetToolTip(Titlelabel, Titlelabel.Text)
    End Sub
    Private Sub albumlabel_MouseHover(sender As Object, e As EventArgs) Handles albumlabel.MouseHover, Genrelabel.MouseHover, durationlabel.MouseHover, datelabel.MouseHover, tracknumlabel.MouseHover, ratinglabel.MouseHover, publisherlabel.MouseHover, encodelabel.MouseHover, filesize.MouseHover
        ToolTip1.SetToolTip(albumlabel, albumlabel.Text)
    End Sub
    Private Sub artistlabel_MouseHover(sender As Object, e As EventArgs) Handles artistlabel.MouseHover
        ToolTip1.SetToolTip(artistlabel, artistlabel.Text)
    End Sub
    'Open
    Private Sub BarButtonOpenFileLocation_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonOpenFileLocation.ItemClick
        Process.Start("explorer.exe", Path.GetDirectoryName(locationlabel.Text))
    End Sub

    Private Sub itemproperties_Activated(sender As Object, e As EventArgs) Handles MyBase.Activated
        Me.TopMost = False
    End Sub
End Class