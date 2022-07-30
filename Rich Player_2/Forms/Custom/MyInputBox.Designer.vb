<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MyInputBox
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
        Me.TextEdit1 = New DevExpress.XtraEditors.TextEdit()
        Me.DefaultLookAndFeel1 = New DevExpress.LookAndFeel.DefaultLookAndFeel(Me.components)
        Me.OkBut = New DevExpress.XtraEditors.SimpleButton()
        Me.CancelBut = New DevExpress.XtraEditors.SimpleButton()
        Me.Label1 = New Rich_Player.RichLabel()
        CType(Me.TextEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TextEdit1
        '
        Me.TextEdit1.Location = New System.Drawing.Point(12, 66)
        Me.TextEdit1.Name = "TextEdit1"
        Me.TextEdit1.Size = New System.Drawing.Size(174, 20)
        Me.TextEdit1.TabIndex = 0
        '
        'DefaultLookAndFeel1
        '
        Me.DefaultLookAndFeel1.LookAndFeel.SkinName = "DevExpress Dark Style"
        '
        'OkBut
        '
        Me.OkBut.AllowFocus = False
        Me.OkBut.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.OkBut.Location = New System.Drawing.Point(12, 92)
        Me.OkBut.Name = "OkBut"
        Me.OkBut.Size = New System.Drawing.Size(84, 34)
        Me.OkBut.TabIndex = 1
        Me.OkBut.Text = "Okay"
        '
        'CancelBut
        '
        Me.CancelBut.AllowFocus = False
        Me.CancelBut.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CancelBut.Location = New System.Drawing.Point(102, 92)
        Me.CancelBut.Name = "CancelBut"
        Me.CancelBut.Size = New System.Drawing.Size(84, 34)
        Me.CancelBut.TabIndex = 1
        Me.CancelBut.Text = "Cancel"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.MaximumSize = New System.Drawing.Size(174, 0)
        Me.Label1.MinimumSize = New System.Drawing.Size(174, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(174, 20)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Text"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'MyInputBox
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(198, 138)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.CancelBut)
        Me.Controls.Add(Me.OkBut)
        Me.Controls.Add(Me.TextEdit1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "MyInputBox"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = " "
        Me.TopMost = True
        CType(Me.TextEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TextEdit1 As DevExpress.XtraEditors.TextEdit
    Friend WithEvents DefaultLookAndFeel1 As DevExpress.LookAndFeel.DefaultLookAndFeel
    Friend WithEvents OkBut As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents CancelBut As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents Label1 As Rich_Player.RichLabel
End Class
