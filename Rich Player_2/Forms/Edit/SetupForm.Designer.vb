<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SetupForm
    Inherits DevExpress.XtraEditors.XtraForm

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SetupForm))
        Me.DefaultLookAndFeel1 = New DevExpress.LookAndFeel.DefaultLookAndFeel(Me.components)
        Me.Panel_Step1 = New System.Windows.Forms.Panel()
        Me.Label_DisplayModeInfo = New Rich_Player.RichLabel()
        Me.Pic_UseMiniModeNo = New System.Windows.Forms.PictureBox()
        Me.Pic_UseMiniModeYes = New System.Windows.Forms.PictureBox()
        Me.Rad_MiniModeNo = New System.Windows.Forms.RadioButton()
        Me.Rad_MiniModeYes = New System.Windows.Forms.RadioButton()
        Me.Panel_Buttons = New System.Windows.Forms.Panel()
        Me.Label_SelectedSkin = New System.Windows.Forms.Label()
        Me.But_Next = New DevExpress.XtraEditors.SimpleButton()
        Me.But_Previous = New DevExpress.XtraEditors.SimpleButton()
        Me.Label_Main = New Rich_Player.RichLabel()
        Me.Panel_Step2 = New System.Windows.Forms.Panel()
        Me.but_Layout_Zoom = New DevExpress.XtraEditors.SimpleButton()
        Me.but_Layout_Stretch = New DevExpress.XtraEditors.SimpleButton()
        Me.but_Layout_Center = New DevExpress.XtraEditors.SimpleButton()
        Me.but_Layout_Tile = New DevExpress.XtraEditors.SimpleButton()
        Me.but_Layout_None = New DevExpress.XtraEditors.SimpleButton()
        Me.label_Layout = New Rich_Player.RichLabel()
        Me.Group_Hd = New DevExpress.XtraEditors.GroupControl()
        Me.Scroller2 = New Rich_Player.Scroller()
        Me.Label_EnhancedCustom = New Rich_Player.RichLabel()
        Me.Pic_Skin13 = New System.Windows.Forms.PictureBox()
        Me.Pic_Skin15 = New System.Windows.Forms.PictureBox()
        Me.Pic_Skin14 = New System.Windows.Forms.PictureBox()
        Me.Pic_Skin11 = New System.Windows.Forms.PictureBox()
        Me.Pic_Skin12 = New System.Windows.Forms.PictureBox()
        Me.Pic_Skin10 = New System.Windows.Forms.PictureBox()
        Me.Pic_Skin9 = New System.Windows.Forms.PictureBox()
        Me.Pic_Skin8 = New System.Windows.Forms.PictureBox()
        Me.Pic_Skin7 = New System.Windows.Forms.PictureBox()
        Me.Pic_Skin6 = New System.Windows.Forms.PictureBox()
        Me.Group_Tiled = New DevExpress.XtraEditors.GroupControl()
        Me.Scroller1 = New Rich_Player.Scroller()
        Me.Label_EnhancedNone = New Rich_Player.RichLabel()
        Me.Pic_Skin5 = New System.Windows.Forms.PictureBox()
        Me.Pic_Skin1 = New System.Windows.Forms.PictureBox()
        Me.Pic_Skin4 = New System.Windows.Forms.PictureBox()
        Me.Pic_Skin2 = New System.Windows.Forms.PictureBox()
        Me.Pic_Skin3 = New System.Windows.Forms.PictureBox()
        Me.TimerNextEnabled = New System.Windows.Forms.Timer(Me.components)
        Me.Panel_Step1.SuspendLayout()
        CType(Me.Pic_UseMiniModeNo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Pic_UseMiniModeYes, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel_Buttons.SuspendLayout()
        Me.Panel_Step2.SuspendLayout()
        CType(Me.Group_Hd, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Group_Hd.SuspendLayout()
        Me.Scroller2.SuspendLayout()
        CType(Me.Pic_Skin13, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Pic_Skin15, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Pic_Skin14, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Pic_Skin11, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Pic_Skin12, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Pic_Skin10, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Pic_Skin9, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Pic_Skin8, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Pic_Skin7, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Pic_Skin6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Group_Tiled, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Group_Tiled.SuspendLayout()
        Me.Scroller1.SuspendLayout()
        CType(Me.Pic_Skin5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Pic_Skin1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Pic_Skin4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Pic_Skin2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Pic_Skin3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'DefaultLookAndFeel1
        '
        Me.DefaultLookAndFeel1.LookAndFeel.SkinName = "DevExpress Dark Style"
        '
        'Panel_Step1
        '
        Me.Panel_Step1.BackColor = System.Drawing.Color.FromArgb(CType(CType(88, Byte), Integer), CType(CType(88, Byte), Integer), CType(CType(88, Byte), Integer))
        Me.Panel_Step1.Controls.Add(Me.Label_DisplayModeInfo)
        Me.Panel_Step1.Controls.Add(Me.Pic_UseMiniModeNo)
        Me.Panel_Step1.Controls.Add(Me.Pic_UseMiniModeYes)
        Me.Panel_Step1.Controls.Add(Me.Rad_MiniModeNo)
        Me.Panel_Step1.Controls.Add(Me.Rad_MiniModeYes)
        Me.Panel_Step1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel_Step1.Location = New System.Drawing.Point(0, 21)
        Me.Panel_Step1.Name = "Panel_Step1"
        Me.Panel_Step1.Size = New System.Drawing.Size(584, 289)
        Me.Panel_Step1.TabIndex = 0
        '
        'Label_DisplayModeInfo
        '
        Me.Label_DisplayModeInfo.AutoSize = True
        Me.Label_DisplayModeInfo.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.Label_DisplayModeInfo.Location = New System.Drawing.Point(0, 288)
        Me.Label_DisplayModeInfo.MinimumSize = New System.Drawing.Size(584, 0)
        Me.Label_DisplayModeInfo.Name = "Label_DisplayModeInfo"
        Me.Label_DisplayModeInfo.Size = New System.Drawing.Size(584, 19)
        Me.Label_DisplayModeInfo.TabIndex = 4
        Me.Label_DisplayModeInfo.Text = "(This can be changed later in the main menu)"
        Me.Label_DisplayModeInfo.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Pic_UseMiniModeNo
        '
        Me.Pic_UseMiniModeNo.BackgroundImage = Global.Rich_Player.My.Resources.Resources.RichPlayer_Design_1_example
        Me.Pic_UseMiniModeNo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.Pic_UseMiniModeNo.Location = New System.Drawing.Point(238, 69)
        Me.Pic_UseMiniModeNo.Name = "Pic_UseMiniModeNo"
        Me.Pic_UseMiniModeNo.Size = New System.Drawing.Size(333, 200)
        Me.Pic_UseMiniModeNo.TabIndex = 2
        Me.Pic_UseMiniModeNo.TabStop = False
        '
        'Pic_UseMiniModeYes
        '
        Me.Pic_UseMiniModeYes.BackgroundImage = Global.Rich_Player.My.Resources.Resources.Rich_Player_mini_mode_example
        Me.Pic_UseMiniModeYes.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.Pic_UseMiniModeYes.Location = New System.Drawing.Point(12, 69)
        Me.Pic_UseMiniModeYes.Name = "Pic_UseMiniModeYes"
        Me.Pic_UseMiniModeYes.Size = New System.Drawing.Size(200, 200)
        Me.Pic_UseMiniModeYes.TabIndex = 2
        Me.Pic_UseMiniModeYes.TabStop = False
        '
        'Rad_MiniModeNo
        '
        Me.Rad_MiniModeNo.AutoSize = True
        Me.Rad_MiniModeNo.Checked = True
        Me.Rad_MiniModeNo.Location = New System.Drawing.Point(369, 39)
        Me.Rad_MiniModeNo.Name = "Rad_MiniModeNo"
        Me.Rad_MiniModeNo.Size = New System.Drawing.Size(78, 25)
        Me.Rad_MiniModeNo.TabIndex = 1
        Me.Rad_MiniModeNo.TabStop = True
        Me.Rad_MiniModeNo.Text = "Default"
        Me.Rad_MiniModeNo.UseVisualStyleBackColor = True
        '
        'Rad_MiniModeYes
        '
        Me.Rad_MiniModeYes.AutoSize = True
        Me.Rad_MiniModeYes.Location = New System.Drawing.Point(60, 39)
        Me.Rad_MiniModeYes.Name = "Rad_MiniModeYes"
        Me.Rad_MiniModeYes.Size = New System.Drawing.Size(103, 25)
        Me.Rad_MiniModeYes.TabIndex = 1
        Me.Rad_MiniModeYes.Text = "Mini Mode"
        Me.Rad_MiniModeYes.UseVisualStyleBackColor = True
        '
        'Panel_Buttons
        '
        Me.Panel_Buttons.Controls.Add(Me.Label_SelectedSkin)
        Me.Panel_Buttons.Controls.Add(Me.But_Next)
        Me.Panel_Buttons.Controls.Add(Me.But_Previous)
        Me.Panel_Buttons.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel_Buttons.Location = New System.Drawing.Point(0, 310)
        Me.Panel_Buttons.Name = "Panel_Buttons"
        Me.Panel_Buttons.Size = New System.Drawing.Size(584, 50)
        Me.Panel_Buttons.TabIndex = 1
        '
        'Label_SelectedSkin
        '
        Me.Label_SelectedSkin.AutoSize = True
        Me.Label_SelectedSkin.Location = New System.Drawing.Point(93, 15)
        Me.Label_SelectedSkin.MinimumSize = New System.Drawing.Size(398, 0)
        Me.Label_SelectedSkin.Name = "Label_SelectedSkin"
        Me.Label_SelectedSkin.Size = New System.Drawing.Size(398, 21)
        Me.Label_SelectedSkin.TabIndex = 1
        Me.Label_SelectedSkin.Text = "Enhanced Skin Selected:"
        Me.Label_SelectedSkin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.Label_SelectedSkin.Visible = False
        '
        'But_Next
        '
        Me.But_Next.AllowFocus = False
        Me.But_Next.Enabled = False
        Me.But_Next.Location = New System.Drawing.Point(496, 8)
        Me.But_Next.Name = "But_Next"
        Me.But_Next.Size = New System.Drawing.Size(75, 36)
        Me.But_Next.TabIndex = 0
        Me.But_Next.Text = "Next"
        '
        'But_Previous
        '
        Me.But_Previous.AllowFocus = False
        Me.But_Previous.Enabled = False
        Me.But_Previous.Location = New System.Drawing.Point(12, 8)
        Me.But_Previous.Name = "But_Previous"
        Me.But_Previous.Size = New System.Drawing.Size(75, 36)
        Me.But_Previous.TabIndex = 0
        Me.But_Previous.Text = "Previous"
        '
        'Label_Main
        '
        Me.Label_Main.AutoSize = True
        Me.Label_Main.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label_Main.Location = New System.Drawing.Point(0, 0)
        Me.Label_Main.MinimumSize = New System.Drawing.Size(584, 0)
        Me.Label_Main.Name = "Label_Main"
        Me.Label_Main.Size = New System.Drawing.Size(584, 21)
        Me.Label_Main.TabIndex = 2
        Me.Label_Main.Text = "Display Mode"
        Me.Label_Main.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Panel_Step2
        '
        Me.Panel_Step2.Controls.Add(Me.but_Layout_Zoom)
        Me.Panel_Step2.Controls.Add(Me.but_Layout_Stretch)
        Me.Panel_Step2.Controls.Add(Me.but_Layout_Center)
        Me.Panel_Step2.Controls.Add(Me.but_Layout_Tile)
        Me.Panel_Step2.Controls.Add(Me.but_Layout_None)
        Me.Panel_Step2.Controls.Add(Me.label_Layout)
        Me.Panel_Step2.Controls.Add(Me.Group_Hd)
        Me.Panel_Step2.Controls.Add(Me.Group_Tiled)
        Me.Panel_Step2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel_Step2.Location = New System.Drawing.Point(0, 0)
        Me.Panel_Step2.Name = "Panel_Step2"
        Me.Panel_Step2.Size = New System.Drawing.Size(584, 360)
        Me.Panel_Step2.TabIndex = 3
        Me.Panel_Step2.Visible = False
        '
        'but_Layout_Zoom
        '
        Me.but_Layout_Zoom.AllowFocus = False
        Me.but_Layout_Zoom.Location = New System.Drawing.Point(213, 250)
        Me.but_Layout_Zoom.Name = "but_Layout_Zoom"
        Me.but_Layout_Zoom.Size = New System.Drawing.Size(45, 23)
        Me.but_Layout_Zoom.TabIndex = 3
        Me.but_Layout_Zoom.Tag = "4"
        Me.but_Layout_Zoom.Text = "Zoom"
        '
        'but_Layout_Stretch
        '
        Me.but_Layout_Stretch.AllowFocus = False
        Me.but_Layout_Stretch.Location = New System.Drawing.Point(162, 250)
        Me.but_Layout_Stretch.Name = "but_Layout_Stretch"
        Me.but_Layout_Stretch.Size = New System.Drawing.Size(45, 23)
        Me.but_Layout_Stretch.TabIndex = 3
        Me.but_Layout_Stretch.Tag = "3"
        Me.but_Layout_Stretch.Text = "Stretch"
        '
        'but_Layout_Center
        '
        Me.but_Layout_Center.AllowFocus = False
        Me.but_Layout_Center.Location = New System.Drawing.Point(111, 250)
        Me.but_Layout_Center.Name = "but_Layout_Center"
        Me.but_Layout_Center.Size = New System.Drawing.Size(45, 23)
        Me.but_Layout_Center.TabIndex = 3
        Me.but_Layout_Center.Tag = "2"
        Me.but_Layout_Center.Text = "Center"
        '
        'but_Layout_Tile
        '
        Me.but_Layout_Tile.AllowFocus = False
        Me.but_Layout_Tile.Location = New System.Drawing.Point(60, 250)
        Me.but_Layout_Tile.Name = "but_Layout_Tile"
        Me.but_Layout_Tile.Size = New System.Drawing.Size(45, 23)
        Me.but_Layout_Tile.TabIndex = 3
        Me.but_Layout_Tile.Tag = "1"
        Me.but_Layout_Tile.Text = "Tile"
        '
        'but_Layout_None
        '
        Me.but_Layout_None.AllowFocus = False
        Me.but_Layout_None.Location = New System.Drawing.Point(12, 250)
        Me.but_Layout_None.Name = "but_Layout_None"
        Me.but_Layout_None.Size = New System.Drawing.Size(45, 23)
        Me.but_Layout_None.TabIndex = 3
        Me.but_Layout_None.Tag = "0"
        Me.but_Layout_None.Text = "None"
        '
        'label_Layout
        '
        Me.label_Layout.AutoSize = True
        Me.label_Layout.Location = New System.Drawing.Point(13, 226)
        Me.label_Layout.Name = "label_Layout"
        Me.label_Layout.Size = New System.Drawing.Size(163, 21)
        Me.label_Layout.TabIndex = 2
        Me.label_Layout.Text = "Choose Image Layout:"
        '
        'Group_Hd
        '
        Me.Group_Hd.Controls.Add(Me.Scroller2)
        Me.Group_Hd.Location = New System.Drawing.Point(296, 18)
        Me.Group_Hd.Name = "Group_Hd"
        Me.Group_Hd.Size = New System.Drawing.Size(276, 267)
        Me.Group_Hd.TabIndex = 1
        Me.Group_Hd.Text = "Enhanced Skins (HD)"
        '
        'Scroller2
        '
        Me.Scroller2.AllowTouchScroll = True
        Me.Scroller2.AlwaysScrollActiveControlIntoView = False
        Me.Scroller2.Controls.Add(Me.Label_EnhancedCustom)
        Me.Scroller2.Controls.Add(Me.Pic_Skin13)
        Me.Scroller2.Controls.Add(Me.Pic_Skin15)
        Me.Scroller2.Controls.Add(Me.Pic_Skin14)
        Me.Scroller2.Controls.Add(Me.Pic_Skin11)
        Me.Scroller2.Controls.Add(Me.Pic_Skin12)
        Me.Scroller2.Controls.Add(Me.Pic_Skin10)
        Me.Scroller2.Controls.Add(Me.Pic_Skin9)
        Me.Scroller2.Controls.Add(Me.Pic_Skin8)
        Me.Scroller2.Controls.Add(Me.Pic_Skin7)
        Me.Scroller2.Controls.Add(Me.Pic_Skin6)
        Me.Scroller2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Scroller2.FireScrollEventOnMouseWheel = True
        Me.Scroller2.InvertTouchScroll = True
        Me.Scroller2.Location = New System.Drawing.Point(2, 20)
        Me.Scroller2.Name = "Scroller2"
        Me.Scroller2.ScrollBarSmallChange = 20
        Me.Scroller2.Size = New System.Drawing.Size(272, 245)
        Me.Scroller2.TabIndex = 0
        '
        'Label_EnhancedCustom
        '
        Me.Label_EnhancedCustom.AutoSize = True
        Me.Label_EnhancedCustom.BackColor = System.Drawing.Color.FromArgb(CType(CType(110, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(110, Byte), Integer))
        Me.Label_EnhancedCustom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label_EnhancedCustom.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_EnhancedCustom.Location = New System.Drawing.Point(3, 632)
        Me.Label_EnhancedCustom.MinimumSize = New System.Drawing.Size(120, 120)
        Me.Label_EnhancedCustom.Name = "Label_EnhancedCustom"
        Me.Label_EnhancedCustom.Size = New System.Drawing.Size(120, 120)
        Me.Label_EnhancedCustom.TabIndex = 1
        Me.Label_EnhancedCustom.Text = "Custom"
        Me.Label_EnhancedCustom.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Pic_Skin13
        '
        Me.Pic_Skin13.BackColor = System.Drawing.Color.FromArgb(CType(CType(110, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(110, Byte), Integer))
        Me.Pic_Skin13.BackgroundImage = Global.Rich_Player.My.Resources.Resources.form_bg_13
        Me.Pic_Skin13.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.Pic_Skin13.Location = New System.Drawing.Point(129, 381)
        Me.Pic_Skin13.Name = "Pic_Skin13"
        Me.Pic_Skin13.Size = New System.Drawing.Size(120, 120)
        Me.Pic_Skin13.TabIndex = 0
        Me.Pic_Skin13.TabStop = False
        '
        'Pic_Skin15
        '
        Me.Pic_Skin15.BackColor = System.Drawing.Color.FromArgb(CType(CType(110, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(110, Byte), Integer))
        Me.Pic_Skin15.BackgroundImage = Global.Rich_Player.My.Resources.Resources.form_bg_15
        Me.Pic_Skin15.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.Pic_Skin15.Location = New System.Drawing.Point(129, 507)
        Me.Pic_Skin15.Name = "Pic_Skin15"
        Me.Pic_Skin15.Size = New System.Drawing.Size(120, 120)
        Me.Pic_Skin15.TabIndex = 0
        Me.Pic_Skin15.TabStop = False
        '
        'Pic_Skin14
        '
        Me.Pic_Skin14.BackColor = System.Drawing.Color.FromArgb(CType(CType(110, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(110, Byte), Integer))
        Me.Pic_Skin14.BackgroundImage = Global.Rich_Player.My.Resources.Resources.form_bg_14
        Me.Pic_Skin14.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.Pic_Skin14.Location = New System.Drawing.Point(3, 507)
        Me.Pic_Skin14.Name = "Pic_Skin14"
        Me.Pic_Skin14.Size = New System.Drawing.Size(120, 120)
        Me.Pic_Skin14.TabIndex = 0
        Me.Pic_Skin14.TabStop = False
        '
        'Pic_Skin11
        '
        Me.Pic_Skin11.BackColor = System.Drawing.Color.FromArgb(CType(CType(110, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(110, Byte), Integer))
        Me.Pic_Skin11.BackgroundImage = Global.Rich_Player.My.Resources.Resources.form_bg_11png
        Me.Pic_Skin11.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.Pic_Skin11.Location = New System.Drawing.Point(129, 255)
        Me.Pic_Skin11.Name = "Pic_Skin11"
        Me.Pic_Skin11.Size = New System.Drawing.Size(120, 120)
        Me.Pic_Skin11.TabIndex = 0
        Me.Pic_Skin11.TabStop = False
        '
        'Pic_Skin12
        '
        Me.Pic_Skin12.BackColor = System.Drawing.Color.FromArgb(CType(CType(110, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(110, Byte), Integer))
        Me.Pic_Skin12.BackgroundImage = Global.Rich_Player.My.Resources.Resources.form_bg_12
        Me.Pic_Skin12.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.Pic_Skin12.Location = New System.Drawing.Point(3, 381)
        Me.Pic_Skin12.Name = "Pic_Skin12"
        Me.Pic_Skin12.Size = New System.Drawing.Size(120, 120)
        Me.Pic_Skin12.TabIndex = 0
        Me.Pic_Skin12.TabStop = False
        '
        'Pic_Skin10
        '
        Me.Pic_Skin10.BackColor = System.Drawing.Color.FromArgb(CType(CType(110, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(110, Byte), Integer))
        Me.Pic_Skin10.BackgroundImage = Global.Rich_Player.My.Resources.Resources.form_bg_10
        Me.Pic_Skin10.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.Pic_Skin10.Location = New System.Drawing.Point(3, 255)
        Me.Pic_Skin10.Name = "Pic_Skin10"
        Me.Pic_Skin10.Size = New System.Drawing.Size(120, 120)
        Me.Pic_Skin10.TabIndex = 0
        Me.Pic_Skin10.TabStop = False
        '
        'Pic_Skin9
        '
        Me.Pic_Skin9.BackColor = System.Drawing.Color.FromArgb(CType(CType(110, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(110, Byte), Integer))
        Me.Pic_Skin9.BackgroundImage = Global.Rich_Player.My.Resources.Resources.form_bg_9
        Me.Pic_Skin9.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.Pic_Skin9.Location = New System.Drawing.Point(129, 129)
        Me.Pic_Skin9.Name = "Pic_Skin9"
        Me.Pic_Skin9.Size = New System.Drawing.Size(120, 120)
        Me.Pic_Skin9.TabIndex = 0
        Me.Pic_Skin9.TabStop = False
        '
        'Pic_Skin8
        '
        Me.Pic_Skin8.BackColor = System.Drawing.Color.FromArgb(CType(CType(110, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(110, Byte), Integer))
        Me.Pic_Skin8.BackgroundImage = Global.Rich_Player.My.Resources.Resources.form_bg_8
        Me.Pic_Skin8.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.Pic_Skin8.Location = New System.Drawing.Point(3, 129)
        Me.Pic_Skin8.Name = "Pic_Skin8"
        Me.Pic_Skin8.Size = New System.Drawing.Size(120, 120)
        Me.Pic_Skin8.TabIndex = 0
        Me.Pic_Skin8.TabStop = False
        '
        'Pic_Skin7
        '
        Me.Pic_Skin7.BackColor = System.Drawing.Color.FromArgb(CType(CType(110, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(110, Byte), Integer))
        Me.Pic_Skin7.BackgroundImage = Global.Rich_Player.My.Resources.Resources.form_bg_7
        Me.Pic_Skin7.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.Pic_Skin7.Location = New System.Drawing.Point(129, 3)
        Me.Pic_Skin7.Name = "Pic_Skin7"
        Me.Pic_Skin7.Size = New System.Drawing.Size(120, 120)
        Me.Pic_Skin7.TabIndex = 0
        Me.Pic_Skin7.TabStop = False
        '
        'Pic_Skin6
        '
        Me.Pic_Skin6.BackColor = System.Drawing.Color.FromArgb(CType(CType(110, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(110, Byte), Integer))
        Me.Pic_Skin6.BackgroundImage = Global.Rich_Player.My.Resources.Resources.form_bg_6
        Me.Pic_Skin6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.Pic_Skin6.Location = New System.Drawing.Point(3, 3)
        Me.Pic_Skin6.Name = "Pic_Skin6"
        Me.Pic_Skin6.Size = New System.Drawing.Size(120, 120)
        Me.Pic_Skin6.TabIndex = 0
        Me.Pic_Skin6.TabStop = False
        '
        'Group_Tiled
        '
        Me.Group_Tiled.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(88, Byte), Integer), CType(CType(88, Byte), Integer), CType(CType(88, Byte), Integer))
        Me.Group_Tiled.Appearance.Options.UseBackColor = True
        Me.Group_Tiled.Controls.Add(Me.Scroller1)
        Me.Group_Tiled.Location = New System.Drawing.Point(12, 18)
        Me.Group_Tiled.Name = "Group_Tiled"
        Me.Group_Tiled.Size = New System.Drawing.Size(278, 205)
        Me.Group_Tiled.TabIndex = 1
        Me.Group_Tiled.Text = "Enhanced Skins (Tiled)"
        '
        'Scroller1
        '
        Me.Scroller1.AllowTouchScroll = True
        Me.Scroller1.AlwaysScrollActiveControlIntoView = False
        Me.Scroller1.Controls.Add(Me.Label_EnhancedNone)
        Me.Scroller1.Controls.Add(Me.Pic_Skin5)
        Me.Scroller1.Controls.Add(Me.Pic_Skin1)
        Me.Scroller1.Controls.Add(Me.Pic_Skin4)
        Me.Scroller1.Controls.Add(Me.Pic_Skin2)
        Me.Scroller1.Controls.Add(Me.Pic_Skin3)
        Me.Scroller1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Scroller1.FireScrollEventOnMouseWheel = True
        Me.Scroller1.InvertTouchScroll = True
        Me.Scroller1.Location = New System.Drawing.Point(2, 20)
        Me.Scroller1.Name = "Scroller1"
        Me.Scroller1.ScrollBarSmallChange = 20
        Me.Scroller1.Size = New System.Drawing.Size(274, 183)
        Me.Scroller1.TabIndex = 1
        '
        'Label_EnhancedNone
        '
        Me.Label_EnhancedNone.AutoSize = True
        Me.Label_EnhancedNone.BackColor = System.Drawing.Color.FromArgb(CType(CType(110, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(110, Byte), Integer))
        Me.Label_EnhancedNone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label_EnhancedNone.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_EnhancedNone.Location = New System.Drawing.Point(3, 5)
        Me.Label_EnhancedNone.MinimumSize = New System.Drawing.Size(120, 120)
        Me.Label_EnhancedNone.Name = "Label_EnhancedNone"
        Me.Label_EnhancedNone.Size = New System.Drawing.Size(120, 120)
        Me.Label_EnhancedNone.TabIndex = 1
        Me.Label_EnhancedNone.Text = "None"
        Me.Label_EnhancedNone.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Pic_Skin5
        '
        Me.Pic_Skin5.BackColor = System.Drawing.Color.FromArgb(CType(CType(110, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(110, Byte), Integer))
        Me.Pic_Skin5.BackgroundImage = Global.Rich_Player.My.Resources.Resources.form_bg_5
        Me.Pic_Skin5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.Pic_Skin5.Location = New System.Drawing.Point(129, 257)
        Me.Pic_Skin5.Name = "Pic_Skin5"
        Me.Pic_Skin5.Size = New System.Drawing.Size(120, 120)
        Me.Pic_Skin5.TabIndex = 0
        Me.Pic_Skin5.TabStop = False
        '
        'Pic_Skin1
        '
        Me.Pic_Skin1.BackColor = System.Drawing.Color.FromArgb(CType(CType(110, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(110, Byte), Integer))
        Me.Pic_Skin1.BackgroundImage = Global.Rich_Player.My.Resources.Resources.form_bg
        Me.Pic_Skin1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.Pic_Skin1.Location = New System.Drawing.Point(129, 5)
        Me.Pic_Skin1.Name = "Pic_Skin1"
        Me.Pic_Skin1.Size = New System.Drawing.Size(120, 120)
        Me.Pic_Skin1.TabIndex = 0
        Me.Pic_Skin1.TabStop = False
        '
        'Pic_Skin4
        '
        Me.Pic_Skin4.BackColor = System.Drawing.Color.FromArgb(CType(CType(110, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(110, Byte), Integer))
        Me.Pic_Skin4.BackgroundImage = Global.Rich_Player.My.Resources.Resources.form_bg_4
        Me.Pic_Skin4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.Pic_Skin4.Location = New System.Drawing.Point(3, 257)
        Me.Pic_Skin4.Name = "Pic_Skin4"
        Me.Pic_Skin4.Size = New System.Drawing.Size(120, 120)
        Me.Pic_Skin4.TabIndex = 0
        Me.Pic_Skin4.TabStop = False
        '
        'Pic_Skin2
        '
        Me.Pic_Skin2.BackColor = System.Drawing.Color.FromArgb(CType(CType(110, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(110, Byte), Integer))
        Me.Pic_Skin2.BackgroundImage = Global.Rich_Player.My.Resources.Resources.form_bg_2
        Me.Pic_Skin2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.Pic_Skin2.Location = New System.Drawing.Point(3, 131)
        Me.Pic_Skin2.Name = "Pic_Skin2"
        Me.Pic_Skin2.Size = New System.Drawing.Size(120, 120)
        Me.Pic_Skin2.TabIndex = 0
        Me.Pic_Skin2.TabStop = False
        '
        'Pic_Skin3
        '
        Me.Pic_Skin3.BackColor = System.Drawing.Color.FromArgb(CType(CType(110, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(110, Byte), Integer))
        Me.Pic_Skin3.BackgroundImage = Global.Rich_Player.My.Resources.Resources.form_bg_3
        Me.Pic_Skin3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.Pic_Skin3.Location = New System.Drawing.Point(129, 131)
        Me.Pic_Skin3.Name = "Pic_Skin3"
        Me.Pic_Skin3.Size = New System.Drawing.Size(120, 120)
        Me.Pic_Skin3.TabIndex = 0
        Me.Pic_Skin3.TabStop = False
        '
        'TimerNextEnabled
        '
        Me.TimerNextEnabled.Enabled = True
        Me.TimerNextEnabled.Interval = 1
        '
        'SetupForm
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 21.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(584, 360)
        Me.Controls.Add(Me.Panel_Step1)
        Me.Controls.Add(Me.Label_Main)
        Me.Controls.Add(Me.Panel_Buttons)
        Me.Controls.Add(Me.Panel_Step2)
        Me.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "SetupForm"
        Me.ShowIcon = False
        Me.Text = "Setup"
        Me.TopMost = True
        Me.Panel_Step1.ResumeLayout(False)
        Me.Panel_Step1.PerformLayout()
        CType(Me.Pic_UseMiniModeNo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Pic_UseMiniModeYes, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel_Buttons.ResumeLayout(False)
        Me.Panel_Buttons.PerformLayout()
        Me.Panel_Step2.ResumeLayout(False)
        Me.Panel_Step2.PerformLayout()
        CType(Me.Group_Hd, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Group_Hd.ResumeLayout(False)
        Me.Scroller2.ResumeLayout(False)
        Me.Scroller2.PerformLayout()
        CType(Me.Pic_Skin13, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Pic_Skin15, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Pic_Skin14, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Pic_Skin11, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Pic_Skin12, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Pic_Skin10, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Pic_Skin9, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Pic_Skin8, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Pic_Skin7, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Pic_Skin6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Group_Tiled, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Group_Tiled.ResumeLayout(False)
        Me.Scroller1.ResumeLayout(False)
        Me.Scroller1.PerformLayout()
        CType(Me.Pic_Skin5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Pic_Skin1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Pic_Skin4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Pic_Skin2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Pic_Skin3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents DefaultLookAndFeel1 As DevExpress.LookAndFeel.DefaultLookAndFeel
    Friend WithEvents Panel_Step1 As System.Windows.Forms.Panel
    Friend WithEvents Panel_Buttons As System.Windows.Forms.Panel
    Friend WithEvents But_Next As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents But_Previous As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents Rad_MiniModeNo As System.Windows.Forms.RadioButton
    Friend WithEvents Rad_MiniModeYes As System.Windows.Forms.RadioButton
    Friend WithEvents Pic_UseMiniModeNo As System.Windows.Forms.PictureBox
    Friend WithEvents Pic_UseMiniModeYes As System.Windows.Forms.PictureBox
    Friend WithEvents Label_Main As Rich_Player.RichLabel
    Friend WithEvents Label_DisplayModeInfo As Rich_Player.RichLabel
    Friend WithEvents Panel_Step2 As System.Windows.Forms.Panel
    Friend WithEvents Group_Hd As DevExpress.XtraEditors.GroupControl
    Friend WithEvents Group_Tiled As DevExpress.XtraEditors.GroupControl
    Friend WithEvents Pic_Skin5 As System.Windows.Forms.PictureBox
    Friend WithEvents Pic_Skin4 As System.Windows.Forms.PictureBox
    Friend WithEvents Pic_Skin3 As System.Windows.Forms.PictureBox
    Friend WithEvents Pic_Skin2 As System.Windows.Forms.PictureBox
    Friend WithEvents Pic_Skin1 As System.Windows.Forms.PictureBox
    Friend WithEvents Scroller2 As Rich_Player.Scroller
    Friend WithEvents Pic_Skin13 As System.Windows.Forms.PictureBox
    Friend WithEvents Pic_Skin15 As System.Windows.Forms.PictureBox
    Friend WithEvents Pic_Skin14 As System.Windows.Forms.PictureBox
    Friend WithEvents Pic_Skin11 As System.Windows.Forms.PictureBox
    Friend WithEvents Pic_Skin12 As System.Windows.Forms.PictureBox
    Friend WithEvents Pic_Skin10 As System.Windows.Forms.PictureBox
    Friend WithEvents Pic_Skin9 As System.Windows.Forms.PictureBox
    Friend WithEvents Pic_Skin8 As System.Windows.Forms.PictureBox
    Friend WithEvents Pic_Skin7 As System.Windows.Forms.PictureBox
    Friend WithEvents Pic_Skin6 As System.Windows.Forms.PictureBox
    Friend WithEvents Scroller1 As Rich_Player.Scroller
    Friend WithEvents Label_EnhancedNone As Rich_Player.RichLabel
    Friend WithEvents Label_SelectedSkin As System.Windows.Forms.Label
    Friend WithEvents TimerNextEnabled As System.Windows.Forms.Timer
    Friend WithEvents Label_EnhancedCustom As Rich_Player.RichLabel
    Friend WithEvents but_Layout_Zoom As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents but_Layout_Stretch As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents but_Layout_Center As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents but_Layout_Tile As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents but_Layout_None As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents label_Layout As Rich_Player.RichLabel
End Class
