<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MyFullMsgBox
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
        Me.but_OK = New DevExpress.XtraEditors.SimpleButton()
        Me.but_Cancel = New DevExpress.XtraEditors.SimpleButton()
        Me.Message = New Rich_Player.RichLabel()
        Me.SuspendLayout()
        '
        'DefaultLookAndFeel1
        '
        Me.DefaultLookAndFeel1.LookAndFeel.SkinName = "DevExpress Dark Style"
        '
        'but_OK
        '
        Me.but_OK.AllowFocus = False
        Me.but_OK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.but_OK.Appearance.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.but_OK.Appearance.Options.UseFont = True
        Me.but_OK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.but_OK.Location = New System.Drawing.Point(12, 40)
        Me.but_OK.Name = "but_OK"
        Me.but_OK.Size = New System.Drawing.Size(109, 40)
        Me.but_OK.TabIndex = 1
        Me.but_OK.Text = "Okay"
        '
        'but_Cancel
        '
        Me.but_Cancel.AllowFocus = False
        Me.but_Cancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.but_Cancel.Appearance.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.but_Cancel.Appearance.Options.UseFont = True
        Me.but_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.but_Cancel.Location = New System.Drawing.Point(163, 40)
        Me.but_Cancel.Name = "but_Cancel"
        Me.but_Cancel.Size = New System.Drawing.Size(109, 40)
        Me.but_Cancel.TabIndex = 1
        Me.but_Cancel.Text = "Cancel"
        Me.but_Cancel.Visible = False
        '
        'Message
        '
        Me.Message.AutoSize = True
        Me.Message.Dock = System.Windows.Forms.DockStyle.Top
        Me.Message.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.Message.Location = New System.Drawing.Point(0, 0)
        Me.Message.MaximumSize = New System.Drawing.Size(280, 0)
        Me.Message.MinimumSize = New System.Drawing.Size(280, 25)
        Me.Message.Name = "Message"
        Me.Message.Size = New System.Drawing.Size(280, 25)
        Me.Message.TabIndex = 0
        Me.Message.Text = "Message"
        Me.Message.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'MyFullMsgBox
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.CancelButton = Me.but_Cancel
        Me.ClientSize = New System.Drawing.Size(284, 92)
        Me.Controls.Add(Me.but_Cancel)
        Me.Controls.Add(Me.but_OK)
        Me.Controls.Add(Me.Message)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.Name = "MyFullMsgBox"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Title"
        Me.TopMost = True
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents DefaultLookAndFeel1 As DevExpress.LookAndFeel.DefaultLookAndFeel
    Friend WithEvents Message As Rich_Player.RichLabel
    Friend WithEvents but_OK As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents but_Cancel As DevExpress.XtraEditors.SimpleButton
End Class
