<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UpdateDialog
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
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.XtraScrollableControl1 = New DevExpress.XtraEditors.XtraScrollableControl()
        Me.WebsiteBut = New DevExpress.XtraEditors.SimpleButton()
        Me.ProgressBar1 = New DevExpress.XtraEditors.ProgressBarControl()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.Cancel_Button = New DevExpress.XtraEditors.SimpleButton()
        Me.OK_Button = New DevExpress.XtraEditors.SimpleButton()
        Me.LabelNoCopy1 = New Rich_Player.RichLabel()
        Me.ReleaseNotesLabel = New Rich_Player.RichLabel()
        Me.XtraScrollableControl1.SuspendLayout()
        CType(Me.ProgressBar1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'DefaultLookAndFeel1
        '
        Me.DefaultLookAndFeel1.LookAndFeel.SkinName = "DevExpress Dark Style"
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        Me.Timer1.Interval = 1000
        '
        'XtraScrollableControl1
        '
        Me.XtraScrollableControl1.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.XtraScrollableControl1.Appearance.Options.UseBackColor = True
        Me.XtraScrollableControl1.Controls.Add(Me.WebsiteBut)
        Me.XtraScrollableControl1.Controls.Add(Me.ProgressBar1)
        Me.XtraScrollableControl1.Controls.Add(Me.TableLayoutPanel1)
        Me.XtraScrollableControl1.Controls.Add(Me.LabelNoCopy1)
        Me.XtraScrollableControl1.Controls.Add(Me.ReleaseNotesLabel)
        Me.XtraScrollableControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.XtraScrollableControl1.Location = New System.Drawing.Point(0, 0)
        Me.XtraScrollableControl1.Name = "XtraScrollableControl1"
        Me.XtraScrollableControl1.Size = New System.Drawing.Size(220, 223)
        Me.XtraScrollableControl1.TabIndex = 17
        '
        'WebsiteBut
        '
        Me.WebsiteBut.AllowFocus = False
        Me.WebsiteBut.Location = New System.Drawing.Point(12, 120)
        Me.WebsiteBut.Name = "WebsiteBut"
        Me.WebsiteBut.Size = New System.Drawing.Size(197, 23)
        Me.WebsiteBut.TabIndex = 27
        Me.WebsiteBut.Text = "Visit Website"
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(12, 85)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(197, 29)
        Me.ProgressBar1.TabIndex = 26
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 0, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(35, 53)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(146, 29)
        Me.TableLayoutPanel1.TabIndex = 20
        '
        'Cancel_Button
        '
        Me.Cancel_Button.AllowFocus = False
        Me.Cancel_Button.Location = New System.Drawing.Point(76, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(67, 23)
        Me.Cancel_Button.TabIndex = 26
        Me.Cancel_Button.Text = "Cancel"
        '
        'OK_Button
        '
        Me.OK_Button.AllowFocus = False
        Me.OK_Button.Location = New System.Drawing.Point(3, 3)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(67, 23)
        Me.OK_Button.TabIndex = 26
        Me.OK_Button.Text = "Update"
        '
        'LabelNoCopy1
        '
        Me.LabelNoCopy1.AutoSize = True
        Me.LabelNoCopy1.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelNoCopy1.Location = New System.Drawing.Point(22, 20)
        Me.LabelNoCopy1.Name = "LabelNoCopy1"
        Me.LabelNoCopy1.Size = New System.Drawing.Size(193, 21)
        Me.LabelNoCopy1.TabIndex = 22
        Me.LabelNoCopy1.Text = "A new version is available!"
        Me.LabelNoCopy1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ReleaseNotesLabel
        '
        Me.ReleaseNotesLabel.AutoSize = True
        Me.ReleaseNotesLabel.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.ReleaseNotesLabel.Location = New System.Drawing.Point(23, 148)
        Me.ReleaseNotesLabel.MaximumSize = New System.Drawing.Size(168, 0)
        Me.ReleaseNotesLabel.MinimumSize = New System.Drawing.Size(168, 0)
        Me.ReleaseNotesLabel.Name = "ReleaseNotesLabel"
        Me.ReleaseNotesLabel.Size = New System.Drawing.Size(168, 15)
        Me.ReleaseNotesLabel.TabIndex = 23
        Me.ReleaseNotesLabel.Text = "￬ Click to view Release Notes:"
        Me.ReleaseNotesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'UpdateDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(220, 223)
        Me.Controls.Add(Me.XtraScrollableControl1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "UpdateDialog"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Update"
        Me.TopMost = True
        Me.XtraScrollableControl1.ResumeLayout(False)
        Me.XtraScrollableControl1.PerformLayout()
        CType(Me.ProgressBar1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents DefaultLookAndFeel1 As DevExpress.LookAndFeel.DefaultLookAndFeel
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents XtraScrollableControl1 As DevExpress.XtraEditors.XtraScrollableControl
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents LabelNoCopy1 As Rich_Player.RichLabel
    Friend WithEvents ReleaseNotesLabel As Rich_Player.RichLabel
    Friend WithEvents OK_Button As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents WebsiteBut As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents ProgressBar1 As DevExpress.XtraEditors.ProgressBarControl
    Friend WithEvents Cancel_Button As DevExpress.XtraEditors.SimpleButton
End Class
