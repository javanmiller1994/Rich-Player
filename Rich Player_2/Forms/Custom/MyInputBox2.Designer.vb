<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MyInputBox2
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
        Me.CancelBut = New DevExpress.XtraEditors.SimpleButton()
        Me.OkBut = New DevExpress.XtraEditors.SimpleButton()
        Me.TextEdit2 = New DevExpress.XtraEditors.TextEdit()
        Me.RichLabel1 = New Rich_Player.RichLabel()
        Me.Label1 = New Rich_Player.RichLabel()
        CType(Me.TextEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextEdit2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TextEdit1
        '
        Me.TextEdit1.Location = New System.Drawing.Point(12, 88)
        Me.TextEdit1.Name = "TextEdit1"
        Me.TextEdit1.Size = New System.Drawing.Size(174, 20)
        Me.TextEdit1.TabIndex = 4
        '
        'DefaultLookAndFeel1
        '
        Me.DefaultLookAndFeel1.LookAndFeel.SkinName = "DevExpress Dark Style"
        '
        'CancelBut
        '
        Me.CancelBut.AllowFocus = False
        Me.CancelBut.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CancelBut.Location = New System.Drawing.Point(102, 182)
        Me.CancelBut.Name = "CancelBut"
        Me.CancelBut.Size = New System.Drawing.Size(84, 34)
        Me.CancelBut.TabIndex = 5
        Me.CancelBut.Text = "Cancel"
        '
        'OkBut
        '
        Me.OkBut.AllowFocus = False
        Me.OkBut.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.OkBut.Location = New System.Drawing.Point(12, 182)
        Me.OkBut.Name = "OkBut"
        Me.OkBut.Size = New System.Drawing.Size(84, 34)
        Me.OkBut.TabIndex = 6
        Me.OkBut.Text = "Okay"
        '
        'TextEdit2
        '
        Me.TextEdit2.Location = New System.Drawing.Point(12, 156)
        Me.TextEdit2.Name = "TextEdit2"
        Me.TextEdit2.Size = New System.Drawing.Size(174, 20)
        Me.TextEdit2.TabIndex = 8
        '
        'RichLabel1
        '
        Me.RichLabel1.AutoSize = True
        Me.RichLabel1.Location = New System.Drawing.Point(12, 131)
        Me.RichLabel1.MaximumSize = New System.Drawing.Size(174, 0)
        Me.RichLabel1.MinimumSize = New System.Drawing.Size(174, 20)
        Me.RichLabel1.Name = "RichLabel1"
        Me.RichLabel1.Size = New System.Drawing.Size(174, 20)
        Me.RichLabel1.TabIndex = 9
        Me.RichLabel1.Text = "Text"
        Me.RichLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 11)
        Me.Label1.MaximumSize = New System.Drawing.Size(174, 0)
        Me.Label1.MinimumSize = New System.Drawing.Size(174, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(174, 20)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "Text"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'MyInputBox2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(198, 228)
        Me.Controls.Add(Me.RichLabel1)
        Me.Controls.Add(Me.TextEdit2)
        Me.Controls.Add(Me.TextEdit1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.CancelBut)
        Me.Controls.Add(Me.OkBut)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "MyInputBox2"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "MyInputBox2"
        Me.TopMost = True
        CType(Me.TextEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextEdit2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TextEdit1 As DevExpress.XtraEditors.TextEdit
    Friend WithEvents DefaultLookAndFeel1 As DevExpress.LookAndFeel.DefaultLookAndFeel
    Friend WithEvents Label1 As Rich_Player.RichLabel
    Friend WithEvents CancelBut As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents OkBut As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents TextEdit2 As DevExpress.XtraEditors.TextEdit
    Friend WithEvents RichLabel1 As Rich_Player.RichLabel
End Class
