<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SpotifyWebClientUserControl
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.lv_PlaylistSongs = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lv_songs = New System.Windows.Forms.ListView()
        Me.colTitle = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colArtist = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colAlbum = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.but_SignIn = New DevExpress.XtraEditors.SimpleButton()
        Me.lb_Playlist = New System.Windows.Forms.ListBox()
        Me.tb_Password = New System.Windows.Forms.TextBox()
        Me.tb_Username = New System.Windows.Forms.TextBox()
        Me.timer_RefreshStrings = New System.Windows.Forms.Timer(Me.components)
        Me.LabelNoCopy4 = New Rich_Player.RichLabel()
        Me.LabelNoCopy5 = New Rich_Player.RichLabel()
        Me.LabelNoCopy3 = New Rich_Player.RichLabel()
        Me.LabelNoCopy2 = New Rich_Player.RichLabel()
        Me.LabelNoCopy1 = New Rich_Player.RichLabel()
        Me.label_Loading = New Rich_Player.RichLabel()
        Me.SuspendLayout()
        '
        'lv_PlaylistSongs
        '
        Me.lv_PlaylistSongs.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3})
        Me.lv_PlaylistSongs.Location = New System.Drawing.Point(241, 157)
        Me.lv_PlaylistSongs.Name = "lv_PlaylistSongs"
        Me.lv_PlaylistSongs.Size = New System.Drawing.Size(306, 146)
        Me.lv_PlaylistSongs.TabIndex = 16
        Me.lv_PlaylistSongs.UseCompatibleStateImageBehavior = False
        Me.lv_PlaylistSongs.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Title"
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Artist"
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Album"
        '
        'lv_songs
        '
        Me.lv_songs.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colTitle, Me.colArtist, Me.colAlbum})
        Me.lv_songs.Location = New System.Drawing.Point(137, 36)
        Me.lv_songs.Name = "lv_songs"
        Me.lv_songs.Size = New System.Drawing.Size(410, 90)
        Me.lv_songs.TabIndex = 11
        Me.lv_songs.UseCompatibleStateImageBehavior = False
        Me.lv_songs.View = System.Windows.Forms.View.Details
        '
        'colTitle
        '
        Me.colTitle.Text = "Title"
        '
        'colArtist
        '
        Me.colArtist.Text = "Artist"
        '
        'colAlbum
        '
        Me.colAlbum.Text = "Album"
        '
        'but_SignIn
        '
        Me.but_SignIn.AllowFocus = False
        Me.but_SignIn.Location = New System.Drawing.Point(11, 56)
        Me.but_SignIn.Name = "but_SignIn"
        Me.but_SignIn.Size = New System.Drawing.Size(100, 38)
        Me.but_SignIn.TabIndex = 9
        Me.but_SignIn.Text = "Sign In"
        '
        'lb_Playlist
        '
        Me.lb_Playlist.FormattingEnabled = True
        Me.lb_Playlist.Location = New System.Drawing.Point(11, 157)
        Me.lb_Playlist.Name = "lb_Playlist"
        Me.lb_Playlist.Size = New System.Drawing.Size(224, 147)
        Me.lb_Playlist.TabIndex = 12
        '
        'tb_Password
        '
        Me.tb_Password.Location = New System.Drawing.Point(11, 74)
        Me.tb_Password.Name = "tb_Password"
        Me.tb_Password.Size = New System.Drawing.Size(100, 20)
        Me.tb_Password.TabIndex = 8
        Me.tb_Password.Visible = False
        '
        'tb_Username
        '
        Me.tb_Username.Location = New System.Drawing.Point(11, 30)
        Me.tb_Username.Name = "tb_Username"
        Me.tb_Username.Size = New System.Drawing.Size(100, 20)
        Me.tb_Username.TabIndex = 6
        Me.tb_Username.Visible = False
        '
        'timer_RefreshStrings
        '
        Me.timer_RefreshStrings.Interval = 5000
        '
        'LabelNoCopy4
        '
        Me.LabelNoCopy4.AutoSize = True
        Me.LabelNoCopy4.Font = New System.Drawing.Font("Tahoma", 12.0!)
        Me.LabelNoCopy4.ForeColor = System.Drawing.Color.White
        Me.LabelNoCopy4.Location = New System.Drawing.Point(7, 132)
        Me.LabelNoCopy4.Name = "LabelNoCopy4"
        Me.LabelNoCopy4.Size = New System.Drawing.Size(65, 19)
        Me.LabelNoCopy4.TabIndex = 13
        Me.LabelNoCopy4.Text = "Playlists"
        '
        'LabelNoCopy5
        '
        Me.LabelNoCopy5.AutoSize = True
        Me.LabelNoCopy5.Font = New System.Drawing.Font("Tahoma", 12.0!)
        Me.LabelNoCopy5.ForeColor = System.Drawing.Color.White
        Me.LabelNoCopy5.Location = New System.Drawing.Point(237, 132)
        Me.LabelNoCopy5.Name = "LabelNoCopy5"
        Me.LabelNoCopy5.Size = New System.Drawing.Size(172, 19)
        Me.LabelNoCopy5.TabIndex = 14
        Me.LabelNoCopy5.Text = "Selected Playlist Tracks"
        '
        'LabelNoCopy3
        '
        Me.LabelNoCopy3.AutoSize = True
        Me.LabelNoCopy3.Font = New System.Drawing.Font("Tahoma", 12.0!)
        Me.LabelNoCopy3.ForeColor = System.Drawing.Color.White
        Me.LabelNoCopy3.Location = New System.Drawing.Point(133, 10)
        Me.LabelNoCopy3.Name = "LabelNoCopy3"
        Me.LabelNoCopy3.Size = New System.Drawing.Size(102, 19)
        Me.LabelNoCopy3.TabIndex = 15
        Me.LabelNoCopy3.Text = "Saved Tracks"
        '
        'LabelNoCopy2
        '
        Me.LabelNoCopy2.AutoSize = True
        Me.LabelNoCopy2.ForeColor = System.Drawing.Color.White
        Me.LabelNoCopy2.Location = New System.Drawing.Point(11, 55)
        Me.LabelNoCopy2.Name = "LabelNoCopy2"
        Me.LabelNoCopy2.Size = New System.Drawing.Size(53, 13)
        Me.LabelNoCopy2.TabIndex = 10
        Me.LabelNoCopy2.Text = "Password"
        Me.LabelNoCopy2.Visible = False
        '
        'LabelNoCopy1
        '
        Me.LabelNoCopy1.AutoSize = True
        Me.LabelNoCopy1.ForeColor = System.Drawing.Color.White
        Me.LabelNoCopy1.Location = New System.Drawing.Point(11, 10)
        Me.LabelNoCopy1.Name = "LabelNoCopy1"
        Me.LabelNoCopy1.Size = New System.Drawing.Size(55, 13)
        Me.LabelNoCopy1.TabIndex = 7
        Me.LabelNoCopy1.Text = "Username"
        Me.LabelNoCopy1.Visible = False
        '
        'label_Loading
        '
        Me.label_Loading.AutoSize = True
        Me.label_Loading.Font = New System.Drawing.Font("Segoe UI", 50.0!)
        Me.label_Loading.ForeColor = System.Drawing.Color.White
        Me.label_Loading.Location = New System.Drawing.Point(0, 0)
        Me.label_Loading.MaximumSize = New System.Drawing.Size(559, 317)
        Me.label_Loading.MinimumSize = New System.Drawing.Size(559, 317)
        Me.label_Loading.Name = "label_Loading"
        Me.label_Loading.Size = New System.Drawing.Size(559, 317)
        Me.label_Loading.TabIndex = 17
        Me.label_Loading.Text = "Loading"
        Me.label_Loading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.label_Loading.UseWaitCursor = True
        Me.label_Loading.Visible = False
        '
        'SpotifyWebClientUserControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(88, Byte), Integer), CType(CType(88, Byte), Integer), CType(CType(88, Byte), Integer))
        Me.Controls.Add(Me.LabelNoCopy4)
        Me.Controls.Add(Me.LabelNoCopy5)
        Me.Controls.Add(Me.LabelNoCopy3)
        Me.Controls.Add(Me.lv_PlaylistSongs)
        Me.Controls.Add(Me.lv_songs)
        Me.Controls.Add(Me.but_SignIn)
        Me.Controls.Add(Me.lb_Playlist)
        Me.Controls.Add(Me.LabelNoCopy2)
        Me.Controls.Add(Me.LabelNoCopy1)
        Me.Controls.Add(Me.tb_Password)
        Me.Controls.Add(Me.tb_Username)
        Me.Controls.Add(Me.label_Loading)
        Me.Name = "SpotifyWebClientUserControl"
        Me.Size = New System.Drawing.Size(559, 317)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents LabelNoCopy4 As Rich_Player.RichLabel
    Friend WithEvents LabelNoCopy5 As Rich_Player.RichLabel
    Friend WithEvents LabelNoCopy3 As Rich_Player.RichLabel
    Friend WithEvents lv_PlaylistSongs As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents lv_songs As System.Windows.Forms.ListView
    Friend WithEvents colTitle As System.Windows.Forms.ColumnHeader
    Friend WithEvents colArtist As System.Windows.Forms.ColumnHeader
    Friend WithEvents colAlbum As System.Windows.Forms.ColumnHeader
    Friend WithEvents but_SignIn As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents lb_Playlist As System.Windows.Forms.ListBox
    Friend WithEvents LabelNoCopy2 As Rich_Player.RichLabel
    Friend WithEvents LabelNoCopy1 As Rich_Player.RichLabel
    Friend WithEvents tb_Password As System.Windows.Forms.TextBox
    Friend WithEvents tb_Username As System.Windows.Forms.TextBox
    Friend WithEvents timer_RefreshStrings As System.Windows.Forms.Timer
    Friend WithEvents label_Loading As Rich_Player.RichLabel

End Class
