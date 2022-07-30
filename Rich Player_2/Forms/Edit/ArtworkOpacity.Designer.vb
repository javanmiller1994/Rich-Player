<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ArtworkOpacity
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
        Me.SimpleButton1 = New DevExpress.XtraEditors.SimpleButton()
        Me.SimpleButton2 = New DevExpress.XtraEditors.SimpleButton()
        Me.but_50 = New DevExpress.XtraEditors.SimpleButton()
        Me.but_75 = New DevExpress.XtraEditors.SimpleButton()
        Me.but_35 = New DevExpress.XtraEditors.SimpleButton()
        Me.Textbox_CustomOpacity = New DevExpress.XtraEditors.TextEdit()
        Me.RichLabel2 = New Rich_Player.RichLabel()
        Me.RichLabel1 = New Rich_Player.RichLabel()
        CType(Me.Textbox_CustomOpacity.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'SimpleButton1
        '
        Me.SimpleButton1.AllowFocus = False
        Me.SimpleButton1.Appearance.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.SimpleButton1.Appearance.Options.UseFont = True
        Me.SimpleButton1.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.SimpleButton1.Location = New System.Drawing.Point(68, 240)
        Me.SimpleButton1.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.SimpleButton1.Name = "SimpleButton1"
        Me.SimpleButton1.Size = New System.Drawing.Size(112, 37)
        Me.SimpleButton1.TabIndex = 0
        Me.SimpleButton1.Text = "Okay"
        '
        'SimpleButton2
        '
        Me.SimpleButton2.AllowFocus = False
        Me.SimpleButton2.Appearance.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.SimpleButton2.Appearance.Options.UseFont = True
        Me.SimpleButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.SimpleButton2.Location = New System.Drawing.Point(190, 240)
        Me.SimpleButton2.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.SimpleButton2.Name = "SimpleButton2"
        Me.SimpleButton2.Size = New System.Drawing.Size(112, 37)
        Me.SimpleButton2.TabIndex = 0
        Me.SimpleButton2.Text = "Cancel"
        '
        'but_50
        '
        Me.but_50.AllowFocus = False
        Me.but_50.Appearance.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.but_50.Appearance.Options.UseFont = True
        Me.but_50.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.but_50.Location = New System.Drawing.Point(148, 56)
        Me.but_50.Name = "but_50"
        Me.but_50.Size = New System.Drawing.Size(75, 55)
        Me.but_50.TabIndex = 1
        Me.but_50.Tag = "50"
        Me.but_50.Text = "50%"
        '
        'but_75
        '
        Me.but_75.AllowFocus = False
        Me.but_75.Appearance.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.but_75.Appearance.Options.UseFont = True
        Me.but_75.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.but_75.Location = New System.Drawing.Point(229, 56)
        Me.but_75.Name = "but_75"
        Me.but_75.Size = New System.Drawing.Size(75, 55)
        Me.but_75.TabIndex = 1
        Me.but_75.Tag = "75"
        Me.but_75.Text = "75%" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(Default)"
        '
        'but_35
        '
        Me.but_35.AllowFocus = False
        Me.but_35.Appearance.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.but_35.Appearance.Options.UseFont = True
        Me.but_35.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.but_35.Location = New System.Drawing.Point(67, 56)
        Me.but_35.Name = "but_35"
        Me.but_35.Size = New System.Drawing.Size(75, 55)
        Me.but_35.TabIndex = 1
        Me.but_35.Tag = "35"
        Me.but_35.Text = "35%"
        '
        'Textbox_CustomOpacity
        '
        Me.Textbox_CustomOpacity.EditValue = ""
        Me.Textbox_CustomOpacity.Location = New System.Drawing.Point(72, 159)
        Me.Textbox_CustomOpacity.MaximumSize = New System.Drawing.Size(226, 55)
        Me.Textbox_CustomOpacity.MinimumSize = New System.Drawing.Size(226, 55)
        Me.Textbox_CustomOpacity.Name = "Textbox_CustomOpacity"
        Me.Textbox_CustomOpacity.Size = New System.Drawing.Size(226, 20)
        Me.Textbox_CustomOpacity.TabIndex = 3
        '
        'RichLabel2
        '
        Me.RichLabel2.AutoSize = True
        Me.RichLabel2.Location = New System.Drawing.Point(153, 135)
        Me.RichLabel2.Name = "RichLabel2"
        Me.RichLabel2.Size = New System.Drawing.Size(64, 21)
        Me.RichLabel2.TabIndex = 2
        Me.RichLabel2.Text = "Custom"
        '
        'RichLabel1
        '
        Me.RichLabel1.AutoSize = True
        Me.RichLabel1.Location = New System.Drawing.Point(57, 9)
        Me.RichLabel1.Name = "RichLabel1"
        Me.RichLabel1.Size = New System.Drawing.Size(256, 21)
        Me.RichLabel1.TabIndex = 2
        Me.RichLabel1.Text = "Change the Opacity for the Artwork"
        '
        'ArtworkOpacity
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 21.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(370, 291)
        Me.Controls.Add(Me.Textbox_CustomOpacity)
        Me.Controls.Add(Me.RichLabel2)
        Me.Controls.Add(Me.RichLabel1)
        Me.Controls.Add(Me.but_75)
        Me.Controls.Add(Me.but_35)
        Me.Controls.Add(Me.but_50)
        Me.Controls.Add(Me.SimpleButton2)
        Me.Controls.Add(Me.SimpleButton1)
        Me.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "ArtworkOpacity"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Change Artwork Opacity"
        Me.TopMost = True
        CType(Me.Textbox_CustomOpacity.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents SimpleButton1 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimpleButton2 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents but_50 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents but_75 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents but_35 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents RichLabel1 As Rich_Player.RichLabel
    Friend WithEvents Textbox_CustomOpacity As DevExpress.XtraEditors.TextEdit
    Friend WithEvents RichLabel2 As Rich_Player.RichLabel
End Class
