<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ReportProblemForm
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
        Me.txtMessage = New System.Windows.Forms.TextBox()
        Me.SimpleButton1 = New DevExpress.XtraEditors.SimpleButton()
        Me.SimpleButton2 = New DevExpress.XtraEditors.SimpleButton()
        Me.LabelNoCopy1 = New Rich_Player.RichLabel()
        Me.LabelNoCopy2 = New Rich_Player.RichLabel()
        Me.SuspendLayout()
        '
        'DefaultLookAndFeel1
        '
        Me.DefaultLookAndFeel1.LookAndFeel.SkinName = "DevExpress Dark Style"
        '
        'txtMessage
        '
        Me.txtMessage.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtMessage.BackColor = System.Drawing.Color.DarkGray
        Me.txtMessage.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtMessage.Location = New System.Drawing.Point(94, 9)
        Me.txtMessage.Margin = New System.Windows.Forms.Padding(2)
        Me.txtMessage.Multiline = True
        Me.txtMessage.Name = "txtMessage"
        Me.txtMessage.Size = New System.Drawing.Size(267, 272)
        Me.txtMessage.TabIndex = 1
        '
        'SimpleButton1
        '
        Me.SimpleButton1.AllowFocus = False
        Me.SimpleButton1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.SimpleButton1.Appearance.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SimpleButton1.Appearance.Options.UseFont = True
        Me.SimpleButton1.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.SimpleButton1.Location = New System.Drawing.Point(120, 340)
        Me.SimpleButton1.Margin = New System.Windows.Forms.Padding(2)
        Me.SimpleButton1.Name = "SimpleButton1"
        Me.SimpleButton1.Size = New System.Drawing.Size(71, 34)
        Me.SimpleButton1.TabIndex = 2
        Me.SimpleButton1.Text = "Send"
        '
        'SimpleButton2
        '
        Me.SimpleButton2.AllowFocus = False
        Me.SimpleButton2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.SimpleButton2.Appearance.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SimpleButton2.Appearance.Options.UseFont = True
        Me.SimpleButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.SimpleButton2.Location = New System.Drawing.Point(195, 340)
        Me.SimpleButton2.Margin = New System.Windows.Forms.Padding(2)
        Me.SimpleButton2.Name = "SimpleButton2"
        Me.SimpleButton2.Size = New System.Drawing.Size(71, 34)
        Me.SimpleButton2.TabIndex = 2
        Me.SimpleButton2.Text = "Cancel"
        '
        'LabelNoCopy1
        '
        Me.LabelNoCopy1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LabelNoCopy1.AutoSize = True
        Me.LabelNoCopy1.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.LabelNoCopy1.Location = New System.Drawing.Point(12, 283)
        Me.LabelNoCopy1.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.LabelNoCopy1.MaximumSize = New System.Drawing.Size(350, 0)
        Me.LabelNoCopy1.Name = "LabelNoCopy1"
        Me.LabelNoCopy1.Size = New System.Drawing.Size(349, 42)
        Me.LabelNoCopy1.TabIndex = 5
        Me.LabelNoCopy1.Text = "* You may include in your message your email address. This is required if the dev" & _
    "eloper needs to make a reply with regards to ask for more clarification of the p" & _
    "roblem you are reporting."
        '
        'LabelNoCopy2
        '
        Me.LabelNoCopy2.AutoSize = True
        Me.LabelNoCopy2.Font = New System.Drawing.Font("Tahoma", 12.0!)
        Me.LabelNoCopy2.Location = New System.Drawing.Point(11, 9)
        Me.LabelNoCopy2.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.LabelNoCopy2.Name = "LabelNoCopy2"
        Me.LabelNoCopy2.Size = New System.Drawing.Size(79, 19)
        Me.LabelNoCopy2.TabIndex = 4
        Me.LabelNoCopy2.Text = "Message: "
        '
        'ReportProblemForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(376, 385)
        Me.Controls.Add(Me.LabelNoCopy1)
        Me.Controls.Add(Me.LabelNoCopy2)
        Me.Controls.Add(Me.SimpleButton2)
        Me.Controls.Add(Me.SimpleButton1)
        Me.Controls.Add(Me.txtMessage)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.MinimumSize = New System.Drawing.Size(390, 200)
        Me.Name = "ReportProblemForm"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Report a Problem"
        Me.TopMost = True
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents DefaultLookAndFeel1 As DevExpress.LookAndFeel.DefaultLookAndFeel
    Friend WithEvents txtMessage As System.Windows.Forms.TextBox
    Friend WithEvents SimpleButton1 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimpleButton2 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LabelNoCopy2 As Rich_Player.RichLabel
    Friend WithEvents LabelNoCopy1 As Rich_Player.RichLabel
End Class
