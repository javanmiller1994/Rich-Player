<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class WindowsColors
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
        Dim TrackBarLabel4 As DevExpress.XtraEditors.Repository.TrackBarLabel = New DevExpress.XtraEditors.Repository.TrackBarLabel()
        Dim TrackBarLabel1 As DevExpress.XtraEditors.Repository.TrackBarLabel = New DevExpress.XtraEditors.Repository.TrackBarLabel()
        Dim TrackBarLabel2 As DevExpress.XtraEditors.Repository.TrackBarLabel = New DevExpress.XtraEditors.Repository.TrackBarLabel()
        Me.HueTrackBar = New DevExpress.XtraEditors.TrackBarControl()
        Me.DefaultLookAndFeel1 = New DevExpress.LookAndFeel.DefaultLookAndFeel(Me.components)
        Me.ColorBox = New System.Windows.Forms.PictureBox()
        Me.Cancel_But = New DevExpress.XtraEditors.SimpleButton()
        Me.Ok_But = New DevExpress.XtraEditors.SimpleButton()
        Me.LightTrackBar = New DevExpress.XtraEditors.TrackBarControl()
        Me.SatTrackBar = New DevExpress.XtraEditors.TrackBarControl()
        Me.but_ChangeColor = New DevExpress.XtraEditors.SimpleButton()
        Me.cb_EnableBorders = New System.Windows.Forms.CheckBox()
        Me.ColorDialog1 = New System.Windows.Forms.ColorDialog()
        Me.pb_Border = New System.Windows.Forms.PictureBox()
        Me.RichLabel1 = New Rich_Player.RichLabel()
        Me.LabelNoCopy1 = New Rich_Player.RichLabel()
        CType(Me.HueTrackBar, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.HueTrackBar.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ColorBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LightTrackBar, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LightTrackBar.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SatTrackBar, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SatTrackBar.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pb_Border, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'HueTrackBar
        '
        Me.HueTrackBar.EditValue = Nothing
        Me.HueTrackBar.Location = New System.Drawing.Point(12, 61)
        Me.HueTrackBar.Name = "HueTrackBar"
        Me.HueTrackBar.Properties.AllowFocused = False
        Me.HueTrackBar.Properties.Appearance.ForeColor = System.Drawing.Color.Gainsboro
        Me.HueTrackBar.Properties.Appearance.Options.UseForeColor = True
        Me.HueTrackBar.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Gainsboro
        Me.HueTrackBar.Properties.AppearanceDisabled.Options.UseForeColor = True
        Me.HueTrackBar.Properties.AppearanceFocused.ForeColor = System.Drawing.Color.Gainsboro
        Me.HueTrackBar.Properties.AppearanceFocused.Options.UseForeColor = True
        Me.HueTrackBar.Properties.AppearanceReadOnly.ForeColor = System.Drawing.Color.Gainsboro
        Me.HueTrackBar.Properties.AppearanceReadOnly.Options.UseForeColor = True
        Me.HueTrackBar.Properties.LabelAppearance.ForeColor = System.Drawing.Color.Gainsboro
        Me.HueTrackBar.Properties.LabelAppearance.Options.UseForeColor = True
        Me.HueTrackBar.Properties.LabelAppearance.Options.UseTextOptions = True
        Me.HueTrackBar.Properties.LabelAppearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        TrackBarLabel4.Label = "Hue"
        Me.HueTrackBar.Properties.Labels.AddRange(New DevExpress.XtraEditors.Repository.TrackBarLabel() {TrackBarLabel4})
        Me.HueTrackBar.Properties.Maximum = 360
        Me.HueTrackBar.Properties.ShowLabels = True
        Me.HueTrackBar.Properties.ShowValueToolTip = True
        Me.HueTrackBar.Properties.TickStyle = System.Windows.Forms.TickStyle.None
        Me.HueTrackBar.Size = New System.Drawing.Size(260, 72)
        Me.HueTrackBar.TabIndex = 4
        '
        'DefaultLookAndFeel1
        '
        Me.DefaultLookAndFeel1.LookAndFeel.SkinName = "DevExpress Dark Style"
        '
        'ColorBox
        '
        Me.ColorBox.Location = New System.Drawing.Point(12, 304)
        Me.ColorBox.Name = "ColorBox"
        Me.ColorBox.Size = New System.Drawing.Size(260, 37)
        Me.ColorBox.TabIndex = 9
        Me.ColorBox.TabStop = False
        '
        'Cancel_But
        '
        Me.Cancel_But.AllowFocus = False
        Me.Cancel_But.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Cancel_But.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_But.Location = New System.Drawing.Point(532, 354)
        Me.Cancel_But.Name = "Cancel_But"
        Me.Cancel_But.Size = New System.Drawing.Size(128, 37)
        Me.Cancel_But.TabIndex = 8
        Me.Cancel_But.Text = "Cancel"
        '
        'Ok_But
        '
        Me.Ok_But.AllowFocus = False
        Me.Ok_But.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Ok_But.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Ok_But.Location = New System.Drawing.Point(12, 354)
        Me.Ok_But.Name = "Ok_But"
        Me.Ok_But.Size = New System.Drawing.Size(128, 37)
        Me.Ok_But.TabIndex = 7
        Me.Ok_But.Text = "OK"
        '
        'LightTrackBar
        '
        Me.LightTrackBar.EditValue = -100
        Me.LightTrackBar.Location = New System.Drawing.Point(12, 226)
        Me.LightTrackBar.Name = "LightTrackBar"
        Me.LightTrackBar.Properties.AllowFocused = False
        Me.LightTrackBar.Properties.Appearance.ForeColor = System.Drawing.Color.Gainsboro
        Me.LightTrackBar.Properties.Appearance.Options.UseForeColor = True
        Me.LightTrackBar.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Gainsboro
        Me.LightTrackBar.Properties.AppearanceDisabled.Options.UseForeColor = True
        Me.LightTrackBar.Properties.AppearanceFocused.ForeColor = System.Drawing.Color.Gainsboro
        Me.LightTrackBar.Properties.AppearanceFocused.Options.UseForeColor = True
        Me.LightTrackBar.Properties.AppearanceReadOnly.ForeColor = System.Drawing.Color.Gainsboro
        Me.LightTrackBar.Properties.AppearanceReadOnly.Options.UseForeColor = True
        Me.LightTrackBar.Properties.LabelAppearance.ForeColor = System.Drawing.Color.Gainsboro
        Me.LightTrackBar.Properties.LabelAppearance.Options.UseForeColor = True
        Me.LightTrackBar.Properties.LabelAppearance.Options.UseTextOptions = True
        Me.LightTrackBar.Properties.LabelAppearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        TrackBarLabel1.Label = "Lightness"
        Me.LightTrackBar.Properties.Labels.AddRange(New DevExpress.XtraEditors.Repository.TrackBarLabel() {TrackBarLabel1})
        Me.LightTrackBar.Properties.Maximum = 100
        Me.LightTrackBar.Properties.Minimum = -100
        Me.LightTrackBar.Properties.ShowLabels = True
        Me.LightTrackBar.Properties.ShowValueToolTip = True
        Me.LightTrackBar.Properties.TickStyle = System.Windows.Forms.TickStyle.None
        Me.LightTrackBar.Size = New System.Drawing.Size(260, 72)
        Me.LightTrackBar.TabIndex = 5
        Me.LightTrackBar.Value = -100
        '
        'SatTrackBar
        '
        Me.SatTrackBar.EditValue = -100
        Me.SatTrackBar.Location = New System.Drawing.Point(12, 139)
        Me.SatTrackBar.Name = "SatTrackBar"
        Me.SatTrackBar.Properties.AllowFocused = False
        Me.SatTrackBar.Properties.Appearance.ForeColor = System.Drawing.Color.Gainsboro
        Me.SatTrackBar.Properties.Appearance.Options.UseForeColor = True
        Me.SatTrackBar.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Gainsboro
        Me.SatTrackBar.Properties.AppearanceDisabled.Options.UseForeColor = True
        Me.SatTrackBar.Properties.AppearanceFocused.ForeColor = System.Drawing.Color.Gainsboro
        Me.SatTrackBar.Properties.AppearanceFocused.Options.UseForeColor = True
        Me.SatTrackBar.Properties.AppearanceReadOnly.ForeColor = System.Drawing.Color.Gainsboro
        Me.SatTrackBar.Properties.AppearanceReadOnly.Options.UseForeColor = True
        Me.SatTrackBar.Properties.LabelAppearance.ForeColor = System.Drawing.Color.Gainsboro
        Me.SatTrackBar.Properties.LabelAppearance.Options.UseForeColor = True
        Me.SatTrackBar.Properties.LabelAppearance.Options.UseTextOptions = True
        Me.SatTrackBar.Properties.LabelAppearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        TrackBarLabel2.Label = "Saturation"
        Me.SatTrackBar.Properties.Labels.AddRange(New DevExpress.XtraEditors.Repository.TrackBarLabel() {TrackBarLabel2})
        Me.SatTrackBar.Properties.Maximum = 100
        Me.SatTrackBar.Properties.Minimum = -100
        Me.SatTrackBar.Properties.ShowLabels = True
        Me.SatTrackBar.Properties.ShowValueToolTip = True
        Me.SatTrackBar.Properties.TickStyle = System.Windows.Forms.TickStyle.None
        Me.SatTrackBar.Size = New System.Drawing.Size(260, 72)
        Me.SatTrackBar.TabIndex = 6
        Me.SatTrackBar.Value = -100
        '
        'but_ChangeColor
        '
        Me.but_ChangeColor.AllowFocus = False
        Me.but_ChangeColor.Appearance.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.but_ChangeColor.Appearance.Options.UseFont = True
        Me.but_ChangeColor.Location = New System.Drawing.Point(335, 90)
        Me.but_ChangeColor.Name = "but_ChangeColor"
        Me.but_ChangeColor.Size = New System.Drawing.Size(117, 34)
        Me.but_ChangeColor.TabIndex = 12
        Me.but_ChangeColor.Text = "Change Color"
        '
        'cb_EnableBorders
        '
        Me.cb_EnableBorders.AutoSize = True
        Me.cb_EnableBorders.Location = New System.Drawing.Point(335, 52)
        Me.cb_EnableBorders.Name = "cb_EnableBorders"
        Me.cb_EnableBorders.Size = New System.Drawing.Size(159, 17)
        Me.cb_EnableBorders.TabIndex = 11
        Me.cb_EnableBorders.Text = "Enable use of Border Colors"
        Me.cb_EnableBorders.UseVisualStyleBackColor = True
        '
        'ColorDialog1
        '
        Me.ColorDialog1.FullOpen = True
        '
        'pb_Border
        '
        Me.pb_Border.BackColor = System.Drawing.Color.FromArgb(CType(CType(88, Byte), Integer), CType(CType(88, Byte), Integer), CType(CType(88, Byte), Integer))
        Me.pb_Border.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.pb_Border.Location = New System.Drawing.Point(336, 130)
        Me.pb_Border.Name = "pb_Border"
        Me.pb_Border.Size = New System.Drawing.Size(324, 168)
        Me.pb_Border.TabIndex = 14
        Me.pb_Border.TabStop = False
        '
        'RichLabel1
        '
        Me.RichLabel1.AutoSize = True
        Me.RichLabel1.Font = New System.Drawing.Font("Segoe UI", 12.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RichLabel1.Location = New System.Drawing.Point(12, 12)
        Me.RichLabel1.Name = "RichLabel1"
        Me.RichLabel1.Size = New System.Drawing.Size(215, 42)
        Me.RichLabel1.TabIndex = 13
        Me.RichLabel1.Text = "Adjust Playback" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Controls Background Color"
        '
        'LabelNoCopy1
        '
        Me.LabelNoCopy1.AutoSize = True
        Me.LabelNoCopy1.Font = New System.Drawing.Font("Segoe UI", 12.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelNoCopy1.Location = New System.Drawing.Point(332, 12)
        Me.LabelNoCopy1.Name = "LabelNoCopy1"
        Me.LabelNoCopy1.Size = New System.Drawing.Size(201, 21)
        Me.LabelNoCopy1.TabIndex = 10
        Me.LabelNoCopy1.Text = "Choose App Border Color"
        '
        'WindowsColors
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(672, 403)
        Me.Controls.Add(Me.pb_Border)
        Me.Controls.Add(Me.RichLabel1)
        Me.Controls.Add(Me.but_ChangeColor)
        Me.Controls.Add(Me.cb_EnableBorders)
        Me.Controls.Add(Me.LabelNoCopy1)
        Me.Controls.Add(Me.HueTrackBar)
        Me.Controls.Add(Me.ColorBox)
        Me.Controls.Add(Me.Cancel_But)
        Me.Controls.Add(Me.Ok_But)
        Me.Controls.Add(Me.LightTrackBar)
        Me.Controls.Add(Me.SatTrackBar)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "WindowsColors"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Windows Colors"
        Me.TopMost = True
        CType(Me.HueTrackBar.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.HueTrackBar, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ColorBox, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LightTrackBar.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LightTrackBar, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SatTrackBar.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SatTrackBar, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pb_Border, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents HueTrackBar As DevExpress.XtraEditors.TrackBarControl
    Friend WithEvents DefaultLookAndFeel1 As DevExpress.LookAndFeel.DefaultLookAndFeel
    Friend WithEvents ColorBox As System.Windows.Forms.PictureBox
    Friend WithEvents Cancel_But As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents Ok_But As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LightTrackBar As DevExpress.XtraEditors.TrackBarControl
    Friend WithEvents SatTrackBar As DevExpress.XtraEditors.TrackBarControl
    Friend WithEvents but_ChangeColor As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents cb_EnableBorders As System.Windows.Forms.CheckBox
    Friend WithEvents ColorDialog1 As System.Windows.Forms.ColorDialog
    Friend WithEvents LabelNoCopy1 As Rich_Player.RichLabel
    Friend WithEvents RichLabel1 As Rich_Player.RichLabel
    Friend WithEvents pb_Border As System.Windows.Forms.PictureBox
End Class
