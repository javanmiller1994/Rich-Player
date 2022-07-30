<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Simplistic_Mode
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
        Me.DefaultLookAndFeel1 = New DevExpress.LookAndFeel.DefaultLookAndFeel(Me.components)
        Me.but_Menu = New System.Windows.Forms.PictureBox()
        Me.but_Playlists = New System.Windows.Forms.PictureBox()
        Me.but_Next = New System.Windows.Forms.PictureBox()
        Me.but_Previous = New System.Windows.Forms.PictureBox()
        Me.but_PlayPause = New System.Windows.Forms.PictureBox()
        Me.but_Back = New System.Windows.Forms.PictureBox()
        Me.pb_Artwork = New System.Windows.Forms.PictureBox()
        Me.But_WinMin = New System.Windows.Forms.PictureBox()
        Me.But_WinClose = New System.Windows.Forms.PictureBox()
        Me.label_Duration = New Rich_Player.RichLabel()
        Me.label_Time = New Rich_Player.RichLabel()
        Me.tb_Seek = New Rich_Player.RichTrackBar()
        Me.label_Album = New Rich_Player.RichLabel()
        Me.label_Artist = New Rich_Player.RichLabel()
        Me.label_SongTitle = New Rich_Player.RichLabel()
        Me.panel_Playlists = New System.Windows.Forms.Panel()
        CType(Me.but_Menu, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.but_Playlists, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.but_Next, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.but_Previous, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.but_PlayPause, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.but_Back, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pb_Artwork, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.But_WinMin, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.But_WinClose, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'DefaultLookAndFeel1
        '
        Me.DefaultLookAndFeel1.LookAndFeel.SkinName = "DevExpress Dark Style"
        '
        'but_Menu
        '
        Me.but_Menu.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.but_Menu.BackColor = System.Drawing.Color.Transparent
        Me.but_Menu.BackgroundImage = Global.Rich_Player.My.Resources.Resources.Three_Dots
        Me.but_Menu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.but_Menu.Location = New System.Drawing.Point(415, 218)
        Me.but_Menu.Name = "but_Menu"
        Me.but_Menu.Size = New System.Drawing.Size(40, 40)
        Me.but_Menu.TabIndex = 4
        Me.but_Menu.TabStop = False
        '
        'but_Playlists
        '
        Me.but_Playlists.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.but_Playlists.BackColor = System.Drawing.Color.Transparent
        Me.but_Playlists.BackgroundImage = Global.Rich_Player.My.Resources.Resources.Playlists_2
        Me.but_Playlists.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.but_Playlists.Location = New System.Drawing.Point(103, 218)
        Me.but_Playlists.Name = "but_Playlists"
        Me.but_Playlists.Size = New System.Drawing.Size(40, 40)
        Me.but_Playlists.TabIndex = 3
        Me.but_Playlists.TabStop = False
        '
        'but_Next
        '
        Me.but_Next.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.but_Next.BackColor = System.Drawing.Color.Transparent
        Me.but_Next.BackgroundImage = Global.Rich_Player.My.Resources.Resources.Next_5
        Me.but_Next.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.but_Next.Location = New System.Drawing.Point(342, 218)
        Me.but_Next.Name = "but_Next"
        Me.but_Next.Size = New System.Drawing.Size(30, 40)
        Me.but_Next.TabIndex = 2
        Me.but_Next.TabStop = False
        '
        'but_Previous
        '
        Me.but_Previous.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.but_Previous.BackColor = System.Drawing.Color.Transparent
        Me.but_Previous.BackgroundImage = Global.Rich_Player.My.Resources.Resources.Previous_4
        Me.but_Previous.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.but_Previous.Location = New System.Drawing.Point(176, 218)
        Me.but_Previous.Name = "but_Previous"
        Me.but_Previous.Size = New System.Drawing.Size(30, 40)
        Me.but_Previous.TabIndex = 1
        Me.but_Previous.TabStop = False
        '
        'but_PlayPause
        '
        Me.but_PlayPause.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.but_PlayPause.BackColor = System.Drawing.Color.Transparent
        Me.but_PlayPause.BackgroundImage = Global.Rich_Player.My.Resources.Resources.Play_4
        Me.but_PlayPause.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.but_PlayPause.Location = New System.Drawing.Point(249, 213)
        Me.but_PlayPause.Name = "but_PlayPause"
        Me.but_PlayPause.Size = New System.Drawing.Size(50, 50)
        Me.but_PlayPause.TabIndex = 0
        Me.but_PlayPause.TabStop = False
        '
        'but_Back
        '
        Me.but_Back.BackColor = System.Drawing.Color.Transparent
        Me.but_Back.BackgroundImage = Global.Rich_Player.My.Resources.Resources.SwitchBack
        Me.but_Back.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.but_Back.Location = New System.Drawing.Point(8, 8)
        Me.but_Back.Name = "but_Back"
        Me.but_Back.Size = New System.Drawing.Size(35, 35)
        Me.but_Back.TabIndex = 5
        Me.but_Back.TabStop = False
        '
        'pb_Artwork
        '
        Me.pb_Artwork.BackColor = System.Drawing.Color.Transparent
        Me.pb_Artwork.BackgroundImage = Global.Rich_Player.My.Resources.Resources.not_available1
        Me.pb_Artwork.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.pb_Artwork.Location = New System.Drawing.Point(59, 18)
        Me.pb_Artwork.Name = "pb_Artwork"
        Me.pb_Artwork.Size = New System.Drawing.Size(125, 125)
        Me.pb_Artwork.TabIndex = 1
        Me.pb_Artwork.TabStop = False
        '
        'But_WinMin
        '
        Me.But_WinMin.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.But_WinMin.BackColor = System.Drawing.Color.Transparent
        Me.But_WinMin.BackgroundImage = Global.Rich_Player.My.Resources.Resources.TitleBar_Min
        Me.But_WinMin.Location = New System.Drawing.Point(454, 8)
        Me.But_WinMin.Name = "But_WinMin"
        Me.But_WinMin.Size = New System.Drawing.Size(41, 29)
        Me.But_WinMin.TabIndex = 355
        Me.But_WinMin.TabStop = False
        '
        'But_WinClose
        '
        Me.But_WinClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.But_WinClose.BackColor = System.Drawing.Color.Transparent
        Me.But_WinClose.BackgroundImage = Global.Rich_Player.My.Resources.Resources.TitleBar_Close
        Me.But_WinClose.Location = New System.Drawing.Point(505, 8)
        Me.But_WinClose.Name = "But_WinClose"
        Me.But_WinClose.Size = New System.Drawing.Size(41, 29)
        Me.But_WinClose.TabIndex = 354
        Me.But_WinClose.TabStop = False
        '
        'label_Duration
        '
        Me.label_Duration.AutoSize = True
        Me.label_Duration.Font = New System.Drawing.Font("Century Gothic", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.label_Duration.ForeColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.label_Duration.Location = New System.Drawing.Point(505, 174)
        Me.label_Duration.MaximumSize = New System.Drawing.Size(450, 0)
        Me.label_Duration.MinimumSize = New System.Drawing.Size(41, 17)
        Me.label_Duration.Name = "label_Duration"
        Me.label_Duration.Size = New System.Drawing.Size(41, 17)
        Me.label_Duration.TabIndex = 357
        Me.label_Duration.Text = "0:00"
        Me.label_Duration.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'label_Time
        '
        Me.label_Time.AutoSize = True
        Me.label_Time.Font = New System.Drawing.Font("Century Gothic", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.label_Time.ForeColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.label_Time.Location = New System.Drawing.Point(6, 174)
        Me.label_Time.Name = "label_Time"
        Me.label_Time.Size = New System.Drawing.Size(33, 16)
        Me.label_Time.TabIndex = 356
        Me.label_Time.Text = "0:00"
        Me.label_Time.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tb_Seek
        '
        Me.tb_Seek.BackColor = System.Drawing.Color.Transparent
        Me.tb_Seek.BorderThickness = 1
        Me.tb_Seek.Color_Bar = System.Drawing.Color.Transparent
        Me.tb_Seek.Color_BarLeft = System.Drawing.Color.FromArgb(CType(CType(23, Byte), Integer), CType(CType(119, Byte), Integer), CType(CType(151, Byte), Integer))
        Me.tb_Seek.Color_BarLeftInactive = System.Drawing.Color.Gray
        Me.tb_Seek.Color_BarRight = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(60, Byte), Integer), CType(CType(60, Byte), Integer))
        Me.tb_Seek.Color_Border = System.Drawing.Color.FromArgb(CType(CType(35, Byte), Integer), CType(CType(35, Byte), Integer), CType(CType(35, Byte), Integer))
        Me.tb_Seek.Color_Slider = System.Drawing.Color.Silver
        Me.tb_Seek.Color_Text = System.Drawing.Color.Silver
        Me.tb_Seek.Location = New System.Drawing.Point(8, 155)
        Me.tb_Seek.Maximum = 100
        Me.tb_Seek.MaximumSize = New System.Drawing.Size(2000, 23)
        Me.tb_Seek.Minimum = 0
        Me.tb_Seek.Name = "tb_Seek"
        Me.tb_Seek.Size = New System.Drawing.Size(547, 23)
        Me.tb_Seek.SliderFill = Rich_Player.RichTrackBar.SliderFilled.Yes
        Me.tb_Seek.SliderShape = Rich_Player.RichTrackBar.SliderShapes.Circle
        Me.tb_Seek.TabIndex = 6
        Me.tb_Seek.Text = "RichTrackBar1"
        Me.tb_Seek.TextShown = Rich_Player.RichTrackBar.ShowText.No
        Me.tb_Seek.TickInterval = 10
        Me.tb_Seek.TickWidth = 5
        Me.tb_Seek.Value = 50
        '
        'label_Album
        '
        Me.label_Album.AutoSize = True
        Me.label_Album.Font = New System.Drawing.Font("Century Gothic", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.label_Album.ForeColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.label_Album.Location = New System.Drawing.Point(203, 80)
        Me.label_Album.Name = "label_Album"
        Me.label_Album.Size = New System.Drawing.Size(97, 18)
        Me.label_Album.TabIndex = 4
        Me.label_Album.Text = "Song Album"
        '
        'label_Artist
        '
        Me.label_Artist.AutoSize = True
        Me.label_Artist.Font = New System.Drawing.Font("Century Gothic", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.label_Artist.ForeColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.label_Artist.Location = New System.Drawing.Point(203, 56)
        Me.label_Artist.Name = "label_Artist"
        Me.label_Artist.Size = New System.Drawing.Size(258, 18)
        Me.label_Artist.TabIndex = 3
        Me.label_Artist.Text = "Song Artist - Long Artist Name here"
        '
        'label_SongTitle
        '
        Me.label_SongTitle.AutoSize = True
        Me.label_SongTitle.Font = New System.Drawing.Font("Century Gothic", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.label_SongTitle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.label_SongTitle.Location = New System.Drawing.Point(202, 22)
        Me.label_SongTitle.MaximumSize = New System.Drawing.Size(250, 0)
        Me.label_SongTitle.Name = "label_SongTitle"
        Me.label_SongTitle.Size = New System.Drawing.Size(204, 50)
        Me.label_SongTitle.TabIndex = 2
        Me.label_SongTitle.Text = "Song Title -- Really Long"
        '
        'panel_Playlists
        '
        Me.panel_Playlists.BackColor = System.Drawing.Color.Transparent
        Me.panel_Playlists.Location = New System.Drawing.Point(56, 18)
        Me.panel_Playlists.Name = "panel_Playlists"
        Me.panel_Playlists.Size = New System.Drawing.Size(399, 260)
        Me.panel_Playlists.TabIndex = 358
        '
        'Simplistic_Mode
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(88, Byte), Integer), CType(CType(88, Byte), Integer), CType(CType(88, Byte), Integer))
        Me.Controls.Add(Me.label_Duration)
        Me.Controls.Add(Me.label_Time)
        Me.Controls.Add(Me.but_Menu)
        Me.Controls.Add(Me.But_WinMin)
        Me.Controls.Add(Me.but_Playlists)
        Me.Controls.Add(Me.But_WinClose)
        Me.Controls.Add(Me.but_Next)
        Me.Controls.Add(Me.tb_Seek)
        Me.Controls.Add(Me.but_Previous)
        Me.Controls.Add(Me.but_Back)
        Me.Controls.Add(Me.but_PlayPause)
        Me.Controls.Add(Me.label_Album)
        Me.Controls.Add(Me.label_Artist)
        Me.Controls.Add(Me.label_SongTitle)
        Me.Controls.Add(Me.pb_Artwork)
        Me.Controls.Add(Me.panel_Playlists)
        Me.Name = "Simplistic_Mode"
        Me.Size = New System.Drawing.Size(558, 296)
        CType(Me.but_Menu, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.but_Playlists, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.but_Next, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.but_Previous, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.but_PlayPause, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.but_Back, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pb_Artwork, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.But_WinMin, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.But_WinClose, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents DefaultLookAndFeel1 As DevExpress.LookAndFeel.DefaultLookAndFeel
    Friend WithEvents but_PlayPause As System.Windows.Forms.PictureBox
    Friend WithEvents but_Previous As System.Windows.Forms.PictureBox
    Friend WithEvents but_Menu As System.Windows.Forms.PictureBox
    Friend WithEvents but_Playlists As System.Windows.Forms.PictureBox
    Friend WithEvents but_Next As System.Windows.Forms.PictureBox
    Friend WithEvents pb_Artwork As System.Windows.Forms.PictureBox
    Friend WithEvents label_SongTitle As Rich_Player.RichLabel
    Friend WithEvents label_Artist As Rich_Player.RichLabel
    Friend WithEvents label_Album As Rich_Player.RichLabel
    Friend WithEvents but_Back As System.Windows.Forms.PictureBox
    Friend WithEvents tb_Seek As Rich_Player.RichTrackBar
    Friend WithEvents But_WinMin As System.Windows.Forms.PictureBox
    Friend WithEvents But_WinClose As System.Windows.Forms.PictureBox
    Friend WithEvents label_Time As Rich_Player.RichLabel
    Friend WithEvents label_Duration As Rich_Player.RichLabel
    Friend WithEvents panel_Playlists As System.Windows.Forms.Panel

End Class
