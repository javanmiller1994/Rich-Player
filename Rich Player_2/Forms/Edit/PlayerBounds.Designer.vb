<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PlayerBounds
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
        Me.label_Size = New Rich_Player.RichLabel()
        Me.RichLabel1 = New Rich_Player.RichLabel()
        Me.tb_Height = New DevExpress.XtraEditors.TextEdit()
        Me.tb_Width = New DevExpress.XtraEditors.TextEdit()
        Me.tb_X = New DevExpress.XtraEditors.TextEdit()
        Me.tb_Y = New DevExpress.XtraEditors.TextEdit()
        Me.but_Reset = New DevExpress.XtraEditors.SimpleButton()
        Me.but_Okay = New DevExpress.XtraEditors.SimpleButton()
        Me.but_Cancel = New DevExpress.XtraEditors.SimpleButton()
        Me.RichLabel2 = New Rich_Player.RichLabel()
        Me.RichLabel3 = New Rich_Player.RichLabel()
        Me.RichLabel4 = New Rich_Player.RichLabel()
        Me.RichLabel5 = New Rich_Player.RichLabel()
        Me.SimpleButton1 = New DevExpress.XtraEditors.SimpleButton()
        CType(Me.tb_Height.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.tb_Width.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.tb_X.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.tb_Y.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'label_Size
        '
        Me.label_Size.AutoSize = True
        Me.label_Size.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold)
        Me.label_Size.Location = New System.Drawing.Point(12, 9)
        Me.label_Size.Name = "label_Size"
        Me.label_Size.Size = New System.Drawing.Size(76, 21)
        Me.label_Size.TabIndex = 0
        Me.label_Size.Text = "App Size"
        '
        'RichLabel1
        '
        Me.RichLabel1.AutoSize = True
        Me.RichLabel1.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold)
        Me.RichLabel1.Location = New System.Drawing.Point(12, 194)
        Me.RichLabel1.Name = "RichLabel1"
        Me.RichLabel1.Size = New System.Drawing.Size(111, 21)
        Me.RichLabel1.TabIndex = 0
        Me.RichLabel1.Text = "App Location"
        '
        'tb_Height
        '
        Me.tb_Height.Location = New System.Drawing.Point(91, 91)
        Me.tb_Height.MaximumSize = New System.Drawing.Size(230, 35)
        Me.tb_Height.MinimumSize = New System.Drawing.Size(230, 35)
        Me.tb_Height.Name = "tb_Height"
        Me.tb_Height.Size = New System.Drawing.Size(230, 20)
        Me.tb_Height.TabIndex = 1
        '
        'tb_Width
        '
        Me.tb_Width.Location = New System.Drawing.Point(91, 50)
        Me.tb_Width.MaximumSize = New System.Drawing.Size(230, 35)
        Me.tb_Width.MinimumSize = New System.Drawing.Size(230, 35)
        Me.tb_Width.Name = "tb_Width"
        Me.tb_Width.Size = New System.Drawing.Size(230, 20)
        Me.tb_Width.TabIndex = 1
        '
        'tb_X
        '
        Me.tb_X.Location = New System.Drawing.Point(91, 227)
        Me.tb_X.MaximumSize = New System.Drawing.Size(230, 35)
        Me.tb_X.MinimumSize = New System.Drawing.Size(230, 35)
        Me.tb_X.Name = "tb_X"
        Me.tb_X.Size = New System.Drawing.Size(230, 20)
        Me.tb_X.TabIndex = 1
        '
        'tb_Y
        '
        Me.tb_Y.Location = New System.Drawing.Point(91, 268)
        Me.tb_Y.MaximumSize = New System.Drawing.Size(230, 35)
        Me.tb_Y.MinimumSize = New System.Drawing.Size(230, 35)
        Me.tb_Y.Name = "tb_Y"
        Me.tb_Y.Size = New System.Drawing.Size(230, 20)
        Me.tb_Y.TabIndex = 1
        '
        'but_Reset
        '
        Me.but_Reset.AllowFocus = False
        Me.but_Reset.Appearance.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.but_Reset.Appearance.Options.UseFont = True
        Me.but_Reset.Location = New System.Drawing.Point(91, 313)
        Me.but_Reset.Name = "but_Reset"
        Me.but_Reset.Size = New System.Drawing.Size(230, 48)
        Me.but_Reset.TabIndex = 2
        Me.but_Reset.Text = "Reset Location (Ctrl + Shift + L)"
        '
        'but_Okay
        '
        Me.but_Okay.AllowFocus = False
        Me.but_Okay.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.but_Okay.Appearance.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.but_Okay.Appearance.Options.UseFont = True
        Me.but_Okay.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.but_Okay.Location = New System.Drawing.Point(38, 394)
        Me.but_Okay.Name = "but_Okay"
        Me.but_Okay.Size = New System.Drawing.Size(132, 35)
        Me.but_Okay.TabIndex = 2
        Me.but_Okay.Text = "Okay"
        '
        'but_Cancel
        '
        Me.but_Cancel.AllowFocus = False
        Me.but_Cancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.but_Cancel.Appearance.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.but_Cancel.Appearance.Options.UseFont = True
        Me.but_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.but_Cancel.Location = New System.Drawing.Point(176, 394)
        Me.but_Cancel.Name = "but_Cancel"
        Me.but_Cancel.Size = New System.Drawing.Size(132, 35)
        Me.but_Cancel.TabIndex = 2
        Me.but_Cancel.Text = "Cancel"
        '
        'RichLabel2
        '
        Me.RichLabel2.AutoSize = True
        Me.RichLabel2.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.RichLabel2.Location = New System.Drawing.Point(26, 54)
        Me.RichLabel2.Name = "RichLabel2"
        Me.RichLabel2.Size = New System.Drawing.Size(55, 21)
        Me.RichLabel2.TabIndex = 3
        Me.RichLabel2.Text = "Width:"
        '
        'RichLabel3
        '
        Me.RichLabel3.AutoSize = True
        Me.RichLabel3.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.RichLabel3.Location = New System.Drawing.Point(26, 95)
        Me.RichLabel3.Name = "RichLabel3"
        Me.RichLabel3.Size = New System.Drawing.Size(59, 21)
        Me.RichLabel3.TabIndex = 3
        Me.RichLabel3.Text = "Height:"
        '
        'RichLabel4
        '
        Me.RichLabel4.AutoSize = True
        Me.RichLabel4.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.RichLabel4.Location = New System.Drawing.Point(26, 231)
        Me.RichLabel4.Name = "RichLabel4"
        Me.RichLabel4.Size = New System.Drawing.Size(22, 21)
        Me.RichLabel4.TabIndex = 3
        Me.RichLabel4.Text = "X:"
        '
        'RichLabel5
        '
        Me.RichLabel5.AutoSize = True
        Me.RichLabel5.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.RichLabel5.Location = New System.Drawing.Point(26, 272)
        Me.RichLabel5.Name = "RichLabel5"
        Me.RichLabel5.Size = New System.Drawing.Size(22, 21)
        Me.RichLabel5.TabIndex = 3
        Me.RichLabel5.Text = "Y:"
        '
        'SimpleButton1
        '
        Me.SimpleButton1.AllowFocus = False
        Me.SimpleButton1.Appearance.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.SimpleButton1.Appearance.Options.UseFont = True
        Me.SimpleButton1.Location = New System.Drawing.Point(91, 132)
        Me.SimpleButton1.Name = "SimpleButton1"
        Me.SimpleButton1.Size = New System.Drawing.Size(230, 48)
        Me.SimpleButton1.TabIndex = 4
        Me.SimpleButton1.Text = "Reset Size"
        '
        'PlayerBounds
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(346, 441)
        Me.Controls.Add(Me.SimpleButton1)
        Me.Controls.Add(Me.RichLabel5)
        Me.Controls.Add(Me.RichLabel4)
        Me.Controls.Add(Me.RichLabel3)
        Me.Controls.Add(Me.RichLabel2)
        Me.Controls.Add(Me.but_Cancel)
        Me.Controls.Add(Me.but_Okay)
        Me.Controls.Add(Me.but_Reset)
        Me.Controls.Add(Me.tb_Width)
        Me.Controls.Add(Me.tb_Y)
        Me.Controls.Add(Me.tb_X)
        Me.Controls.Add(Me.tb_Height)
        Me.Controls.Add(Me.RichLabel1)
        Me.Controls.Add(Me.label_Size)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "PlayerBounds"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Rich Player Bounds"
        Me.TopMost = True
        CType(Me.tb_Height.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.tb_Width.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.tb_X.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.tb_Y.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents label_Size As Rich_Player.RichLabel
    Friend WithEvents RichLabel1 As Rich_Player.RichLabel
    Friend WithEvents tb_Height As DevExpress.XtraEditors.TextEdit
    Friend WithEvents tb_Width As DevExpress.XtraEditors.TextEdit
    Friend WithEvents tb_X As DevExpress.XtraEditors.TextEdit
    Friend WithEvents tb_Y As DevExpress.XtraEditors.TextEdit
    Friend WithEvents but_Reset As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents but_Okay As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents but_Cancel As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents RichLabel2 As Rich_Player.RichLabel
    Friend WithEvents RichLabel3 As Rich_Player.RichLabel
    Friend WithEvents RichLabel4 As Rich_Player.RichLabel
    Friend WithEvents RichLabel5 As Rich_Player.RichLabel
    Friend WithEvents SimpleButton1 As DevExpress.XtraEditors.SimpleButton
End Class
