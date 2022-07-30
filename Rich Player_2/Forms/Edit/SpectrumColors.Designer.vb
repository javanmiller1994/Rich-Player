<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SpectrumColors
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
        Me.RichLabel2 = New Rich_Player.RichLabel()
        Me.SimpleButton2 = New DevExpress.XtraEditors.SimpleButton()
        Me.SimpleButton1 = New DevExpress.XtraEditors.SimpleButton()
        Me.RichLabel1 = New Rich_Player.RichLabel()
        Me.ColorDialog1 = New System.Windows.Forms.ColorDialog()
        Me.ColorDialog2 = New System.Windows.Forms.ColorDialog()
        Me.but_Default = New DevExpress.XtraEditors.SimpleButton()
        Me.but_Saved = New DevExpress.XtraEditors.SimpleButton()
        Me.pb_Spectrum = New System.Windows.Forms.PictureBox()
        Me.pb_Color2 = New System.Windows.Forms.PictureBox()
        Me.pb_Color1 = New System.Windows.Forms.PictureBox()
        CType(Me.pb_Spectrum, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pb_Color2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pb_Color1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RichLabel2
        '
        Me.RichLabel2.AutoSize = True
        Me.RichLabel2.BackColor = System.Drawing.Color.Transparent
        Me.RichLabel2.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.RichLabel2.Location = New System.Drawing.Point(182, 9)
        Me.RichLabel2.Name = "RichLabel2"
        Me.RichLabel2.Size = New System.Drawing.Size(61, 21)
        Me.RichLabel2.TabIndex = 5
        Me.RichLabel2.Text = "Color 1"
        '
        'SimpleButton2
        '
        Me.SimpleButton2.AllowFocus = False
        Me.SimpleButton2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.SimpleButton2.Appearance.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.SimpleButton2.Appearance.Options.UseFont = True
        Me.SimpleButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.SimpleButton2.Location = New System.Drawing.Point(173, 278)
        Me.SimpleButton2.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.SimpleButton2.Name = "SimpleButton2"
        Me.SimpleButton2.Size = New System.Drawing.Size(112, 37)
        Me.SimpleButton2.TabIndex = 3
        Me.SimpleButton2.Text = "Cancel"
        '
        'SimpleButton1
        '
        Me.SimpleButton1.AllowFocus = False
        Me.SimpleButton1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.SimpleButton1.Appearance.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.SimpleButton1.Appearance.Options.UseFont = True
        Me.SimpleButton1.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.SimpleButton1.Location = New System.Drawing.Point(51, 278)
        Me.SimpleButton1.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.SimpleButton1.Name = "SimpleButton1"
        Me.SimpleButton1.Size = New System.Drawing.Size(112, 37)
        Me.SimpleButton1.TabIndex = 4
        Me.SimpleButton1.Text = "Okay"
        '
        'RichLabel1
        '
        Me.RichLabel1.AutoSize = True
        Me.RichLabel1.BackColor = System.Drawing.Color.Transparent
        Me.RichLabel1.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.RichLabel1.Location = New System.Drawing.Point(182, 86)
        Me.RichLabel1.Name = "RichLabel1"
        Me.RichLabel1.Size = New System.Drawing.Size(61, 21)
        Me.RichLabel1.TabIndex = 5
        Me.RichLabel1.Text = "Color 2"
        '
        'ColorDialog1
        '
        Me.ColorDialog1.Color = System.Drawing.Color.FromArgb(CType(CType(132, Byte), Integer), CType(CType(200, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ColorDialog1.FullOpen = True
        '
        'ColorDialog2
        '
        Me.ColorDialog2.Color = System.Drawing.Color.FromArgb(CType(CType(174, Byte), Integer), CType(CType(145, Byte), Integer), CType(CType(251, Byte), Integer))
        Me.ColorDialog2.FullOpen = True
        '
        'but_Default
        '
        Me.but_Default.AllowFocus = False
        Me.but_Default.Appearance.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.but_Default.Appearance.Options.UseFont = True
        Me.but_Default.Location = New System.Drawing.Point(186, 168)
        Me.but_Default.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.but_Default.Name = "but_Default"
        Me.but_Default.Size = New System.Drawing.Size(138, 37)
        Me.but_Default.TabIndex = 6
        Me.but_Default.Text = "Default"
        '
        'but_Saved
        '
        Me.but_Saved.AllowFocus = False
        Me.but_Saved.Appearance.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.but_Saved.Appearance.Options.UseFont = True
        Me.but_Saved.Location = New System.Drawing.Point(186, 215)
        Me.but_Saved.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.but_Saved.Name = "but_Saved"
        Me.but_Saved.Size = New System.Drawing.Size(138, 37)
        Me.but_Saved.TabIndex = 6
        Me.but_Saved.Text = "Revert to Saved"
        '
        'pb_Spectrum
        '
        Me.pb_Spectrum.BackColor = System.Drawing.Color.Transparent
        Me.pb_Spectrum.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.pb_Spectrum.Location = New System.Drawing.Point(23, 78)
        Me.pb_Spectrum.Margin = New System.Windows.Forms.Padding(2)
        Me.pb_Spectrum.Name = "pb_Spectrum"
        Me.pb_Spectrum.Size = New System.Drawing.Size(137, 26)
        Me.pb_Spectrum.TabIndex = 1
        Me.pb_Spectrum.TabStop = False
        '
        'pb_Color2
        '
        Me.pb_Color2.BackColor = System.Drawing.Color.FromArgb(CType(CType(174, Byte), Integer), CType(CType(145, Byte), Integer), CType(CType(251, Byte), Integer))
        Me.pb_Color2.Location = New System.Drawing.Point(251, 86)
        Me.pb_Color2.Margin = New System.Windows.Forms.Padding(2)
        Me.pb_Color2.Name = "pb_Color2"
        Me.pb_Color2.Size = New System.Drawing.Size(75, 75)
        Me.pb_Color2.TabIndex = 0
        Me.pb_Color2.TabStop = False
        '
        'pb_Color1
        '
        Me.pb_Color1.BackColor = System.Drawing.Color.FromArgb(CType(CType(132, Byte), Integer), CType(CType(200, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.pb_Color1.Location = New System.Drawing.Point(251, 7)
        Me.pb_Color1.Margin = New System.Windows.Forms.Padding(2)
        Me.pb_Color1.Name = "pb_Color1"
        Me.pb_Color1.Size = New System.Drawing.Size(75, 75)
        Me.pb_Color1.TabIndex = 0
        Me.pb_Color1.TabStop = False
        '
        'SpectrumColors
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(337, 329)
        Me.Controls.Add(Me.but_Saved)
        Me.Controls.Add(Me.but_Default)
        Me.Controls.Add(Me.RichLabel1)
        Me.Controls.Add(Me.RichLabel2)
        Me.Controls.Add(Me.SimpleButton2)
        Me.Controls.Add(Me.SimpleButton1)
        Me.Controls.Add(Me.pb_Spectrum)
        Me.Controls.Add(Me.pb_Color2)
        Me.Controls.Add(Me.pb_Color1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "SpectrumColors"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "SpectrumColors"
        Me.TopMost = True
        CType(Me.pb_Spectrum, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pb_Color2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pb_Color1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pb_Color1 As System.Windows.Forms.PictureBox
    Friend WithEvents pb_Color2 As System.Windows.Forms.PictureBox
    Friend WithEvents pb_Spectrum As System.Windows.Forms.PictureBox
    Friend WithEvents RichLabel2 As Rich_Player.RichLabel
    Friend WithEvents SimpleButton2 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimpleButton1 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents RichLabel1 As Rich_Player.RichLabel
    Friend WithEvents ColorDialog1 As System.Windows.Forms.ColorDialog
    Friend WithEvents ColorDialog2 As System.Windows.Forms.ColorDialog
    Friend WithEvents but_Default As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents but_Saved As DevExpress.XtraEditors.SimpleButton
End Class
