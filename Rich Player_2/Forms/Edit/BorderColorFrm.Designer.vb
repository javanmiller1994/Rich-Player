<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BorderColorFrm
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
        Me.DefaultLookAndFeel1 = New DevExpress.LookAndFeel.DefaultLookAndFeel(Me.components)
        Me.cb_EnableBorders = New System.Windows.Forms.CheckBox()
        Me.but_ChangeColor = New DevExpress.XtraEditors.SimpleButton()
        Me.but_Close = New DevExpress.XtraEditors.SimpleButton()
        Me.ColorDialog1 = New System.Windows.Forms.ColorDialog()
        Me.LabelNoCopy1 = New Rich_Player.RichLabel()
        Me.SuspendLayout()
        '
        'DefaultLookAndFeel1
        '
        Me.DefaultLookAndFeel1.LookAndFeel.SkinName = "DevExpress Dark Style"
        '
        'cb_EnableBorders
        '
        Me.cb_EnableBorders.AutoSize = True
        Me.cb_EnableBorders.Location = New System.Drawing.Point(12, 45)
        Me.cb_EnableBorders.Name = "cb_EnableBorders"
        Me.cb_EnableBorders.Size = New System.Drawing.Size(221, 25)
        Me.cb_EnableBorders.TabIndex = 1
        Me.cb_EnableBorders.Text = "Enable use of Border Colors"
        Me.cb_EnableBorders.UseVisualStyleBackColor = True
        '
        'but_ChangeColor
        '
        Me.but_ChangeColor.AllowFocus = False
        Me.but_ChangeColor.Appearance.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.but_ChangeColor.Appearance.Options.UseFont = True
        Me.but_ChangeColor.Location = New System.Drawing.Point(12, 96)
        Me.but_ChangeColor.Name = "but_ChangeColor"
        Me.but_ChangeColor.Size = New System.Drawing.Size(117, 34)
        Me.but_ChangeColor.TabIndex = 2
        Me.but_ChangeColor.Text = "Change Color"
        '
        'but_Close
        '
        Me.but_Close.AllowFocus = False
        Me.but_Close.Appearance.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.but_Close.Appearance.Options.UseFont = True
        Me.but_Close.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.but_Close.Location = New System.Drawing.Point(220, 96)
        Me.but_Close.Name = "but_Close"
        Me.but_Close.Size = New System.Drawing.Size(117, 34)
        Me.but_Close.TabIndex = 3
        Me.but_Close.Text = "Close"
        '
        'ColorDialog1
        '
        Me.ColorDialog1.FullOpen = True
        '
        'LabelNoCopy1
        '
        Me.LabelNoCopy1.AutoSize = True
        Me.LabelNoCopy1.Font = New System.Drawing.Font("Segoe UI", 12.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelNoCopy1.Location = New System.Drawing.Point(9, 5)
        Me.LabelNoCopy1.Name = "LabelNoCopy1"
        Me.LabelNoCopy1.Size = New System.Drawing.Size(201, 21)
        Me.LabelNoCopy1.TabIndex = 0
        Me.LabelNoCopy1.Text = "Choose App Border Color"
        '
        'BorderColorFrm
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 21.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(349, 143)
        Me.Controls.Add(Me.but_Close)
        Me.Controls.Add(Me.but_ChangeColor)
        Me.Controls.Add(Me.cb_EnableBorders)
        Me.Controls.Add(Me.LabelNoCopy1)
        Me.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "BorderColorFrm"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Border Color"
        Me.TopMost = True
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents DefaultLookAndFeel1 As DevExpress.LookAndFeel.DefaultLookAndFeel
    Friend WithEvents LabelNoCopy1 As Rich_Player.RichLabel
    Friend WithEvents cb_EnableBorders As System.Windows.Forms.CheckBox
    Friend WithEvents but_ChangeColor As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents but_Close As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents ColorDialog1 As System.Windows.Forms.ColorDialog
End Class
