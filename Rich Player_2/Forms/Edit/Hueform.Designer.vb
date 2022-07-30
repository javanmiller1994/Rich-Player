<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Hueform
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
        Dim TrackBarLabel1 As DevExpress.XtraEditors.Repository.TrackBarLabel = New DevExpress.XtraEditors.Repository.TrackBarLabel()
        Dim TrackBarLabel2 As DevExpress.XtraEditors.Repository.TrackBarLabel = New DevExpress.XtraEditors.Repository.TrackBarLabel()
        Dim TrackBarLabel3 As DevExpress.XtraEditors.Repository.TrackBarLabel = New DevExpress.XtraEditors.Repository.TrackBarLabel()
        Me.DefaultLookAndFeel1 = New DevExpress.LookAndFeel.DefaultLookAndFeel(Me.components)
        Me.HueTrackBar = New DevExpress.XtraEditors.TrackBarControl()
        Me.SatTrackBar = New DevExpress.XtraEditors.TrackBarControl()
        Me.LightTrackBar = New DevExpress.XtraEditors.TrackBarControl()
        Me.Ok_But = New DevExpress.XtraEditors.SimpleButton()
        Me.Cancel_But = New DevExpress.XtraEditors.SimpleButton()
        Me.ColorBox = New System.Windows.Forms.PictureBox()
        CType(Me.HueTrackBar, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.HueTrackBar.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SatTrackBar, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SatTrackBar.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LightTrackBar, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LightTrackBar.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ColorBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'DefaultLookAndFeel1
        '
        Me.DefaultLookAndFeel1.LookAndFeel.SkinName = "DevExpress Dark Style"
        '
        'HueTrackBar
        '
        Me.HueTrackBar.EditValue = Nothing
        Me.HueTrackBar.Location = New System.Drawing.Point(12, 12)
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
        TrackBarLabel1.Label = "Hue"
        Me.HueTrackBar.Properties.Labels.AddRange(New DevExpress.XtraEditors.Repository.TrackBarLabel() {TrackBarLabel1})
        Me.HueTrackBar.Properties.Maximum = 360
        Me.HueTrackBar.Properties.ShowLabels = True
        Me.HueTrackBar.Properties.ShowValueToolTip = True
        Me.HueTrackBar.Properties.TickStyle = System.Windows.Forms.TickStyle.None
        Me.HueTrackBar.Size = New System.Drawing.Size(260, 72)
        Me.HueTrackBar.TabIndex = 0
        '
        'SatTrackBar
        '
        Me.SatTrackBar.EditValue = -100
        Me.SatTrackBar.Location = New System.Drawing.Point(12, 90)
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
        Me.SatTrackBar.TabIndex = 0
        Me.SatTrackBar.Value = -100
        '
        'LightTrackBar
        '
        Me.LightTrackBar.EditValue = -100
        Me.LightTrackBar.Location = New System.Drawing.Point(12, 177)
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
        TrackBarLabel3.Label = "Lightness"
        Me.LightTrackBar.Properties.Labels.AddRange(New DevExpress.XtraEditors.Repository.TrackBarLabel() {TrackBarLabel3})
        Me.LightTrackBar.Properties.Maximum = 100
        Me.LightTrackBar.Properties.Minimum = -100
        Me.LightTrackBar.Properties.ShowLabels = True
        Me.LightTrackBar.Properties.ShowValueToolTip = True
        Me.LightTrackBar.Properties.TickStyle = System.Windows.Forms.TickStyle.None
        Me.LightTrackBar.Size = New System.Drawing.Size(260, 72)
        Me.LightTrackBar.TabIndex = 0
        Me.LightTrackBar.Value = -100
        '
        'Ok_But
        '
        Me.Ok_But.AllowFocus = False
        Me.Ok_But.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Ok_But.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Ok_But.Location = New System.Drawing.Point(12, 298)
        Me.Ok_But.Name = "Ok_But"
        Me.Ok_But.Size = New System.Drawing.Size(128, 37)
        Me.Ok_But.TabIndex = 1
        Me.Ok_But.Text = "OK"
        '
        'Cancel_But
        '
        Me.Cancel_But.AllowFocus = False
        Me.Cancel_But.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Cancel_But.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_But.Location = New System.Drawing.Point(144, 298)
        Me.Cancel_But.Name = "Cancel_But"
        Me.Cancel_But.Size = New System.Drawing.Size(128, 37)
        Me.Cancel_But.TabIndex = 2
        Me.Cancel_But.Text = "Cancel"
        '
        'ColorBox
        '
        Me.ColorBox.Location = New System.Drawing.Point(12, 255)
        Me.ColorBox.Name = "ColorBox"
        Me.ColorBox.Size = New System.Drawing.Size(260, 37)
        Me.ColorBox.TabIndex = 3
        Me.ColorBox.TabStop = False
        '
        'Hueform
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(284, 349)
        Me.Controls.Add(Me.ColorBox)
        Me.Controls.Add(Me.Cancel_But)
        Me.Controls.Add(Me.Ok_But)
        Me.Controls.Add(Me.LightTrackBar)
        Me.Controls.Add(Me.SatTrackBar)
        Me.Controls.Add(Me.HueTrackBar)
        Me.Name = "Hueform"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Adjust Window Color"
        Me.TopMost = True
        CType(Me.HueTrackBar.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.HueTrackBar, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SatTrackBar.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SatTrackBar, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LightTrackBar.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LightTrackBar, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ColorBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents DefaultLookAndFeel1 As DevExpress.LookAndFeel.DefaultLookAndFeel
    Friend WithEvents HueTrackBar As DevExpress.XtraEditors.TrackBarControl
    Friend WithEvents SatTrackBar As DevExpress.XtraEditors.TrackBarControl
    Friend WithEvents LightTrackBar As DevExpress.XtraEditors.TrackBarControl
    Friend WithEvents Ok_But As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents Cancel_But As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents ColorBox As System.Windows.Forms.PictureBox
End Class
