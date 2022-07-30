<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AboutForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AboutForm))
        Me.XtraScrollableControl1 = New DevExpress.XtraEditors.XtraScrollableControl()
        Me.but_Tutorial = New DevExpress.XtraEditors.SimpleButton()
        Me.but_ReleaseNotes = New DevExpress.XtraEditors.SimpleButton()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.but_Close = New DevExpress.XtraEditors.SimpleButton()
        Me.DefaultLookAndFeel1 = New DevExpress.LookAndFeel.DefaultLookAndFeel(Me.components)
        Me.XtraScrollableControl1.SuspendLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'XtraScrollableControl1
        '
        Me.XtraScrollableControl1.AllowTouchScroll = True
        Me.XtraScrollableControl1.Controls.Add(Me.but_Tutorial)
        Me.XtraScrollableControl1.Controls.Add(Me.but_ReleaseNotes)
        Me.XtraScrollableControl1.Controls.Add(Me.Label4)
        Me.XtraScrollableControl1.Controls.Add(Me.Label3)
        Me.XtraScrollableControl1.Controls.Add(Me.Label2)
        Me.XtraScrollableControl1.Controls.Add(Me.PictureBox2)
        Me.XtraScrollableControl1.Controls.Add(Me.PictureBox1)
        Me.XtraScrollableControl1.Controls.Add(Me.Label1)
        Me.XtraScrollableControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.XtraScrollableControl1.Location = New System.Drawing.Point(0, 0)
        Me.XtraScrollableControl1.Name = "XtraScrollableControl1"
        Me.XtraScrollableControl1.Size = New System.Drawing.Size(632, 280)
        Me.XtraScrollableControl1.TabIndex = 1
        '
        'but_Tutorial
        '
        Me.but_Tutorial.AllowFocus = False
        Me.but_Tutorial.Appearance.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.but_Tutorial.Appearance.Options.UseFont = True
        Me.but_Tutorial.Location = New System.Drawing.Point(171, 88)
        Me.but_Tutorial.Name = "but_Tutorial"
        Me.but_Tutorial.Size = New System.Drawing.Size(149, 28)
        Me.but_Tutorial.TabIndex = 5
        Me.but_Tutorial.Text = "View Tutorial"
        '
        'but_ReleaseNotes
        '
        Me.but_ReleaseNotes.AllowFocus = False
        Me.but_ReleaseNotes.Appearance.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.but_ReleaseNotes.Appearance.Options.UseFont = True
        Me.but_ReleaseNotes.Location = New System.Drawing.Point(16, 88)
        Me.but_ReleaseNotes.Name = "but_ReleaseNotes"
        Me.but_ReleaseNotes.Size = New System.Drawing.Size(149, 28)
        Me.but_ReleaseNotes.TabIndex = 5
        Me.but_ReleaseNotes.Text = "View Release Notes"
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Segoe UI", 16.0!)
        Me.Label4.Location = New System.Drawing.Point(12, 9)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(118, 30)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "Rich Player"
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.Label3.Location = New System.Drawing.Point(12, 42)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(85, 21)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Created By"
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.Label2.Location = New System.Drawing.Point(13, 63)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(89, 15)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "by Rexford Rich"
        '
        'PictureBox2
        '
        Me.PictureBox2.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox2.BackgroundImage = Global.Rich_Player.My.Resources.Resources._Rexford_Rich_2
        Me.PictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.PictureBox2.Location = New System.Drawing.Point(103, 42)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(40, 40)
        Me.PictureBox2.TabIndex = 3
        Me.PictureBox2.TabStop = False
        '
        'PictureBox1
        '
        Me.PictureBox1.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox1.BackgroundImage = Global.Rich_Player.My.Resources.Resources.RichPlayer_6
        Me.PictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.PictureBox1.Location = New System.Drawing.Point(415, 9)
        Me.PictureBox1.MinimumSize = New System.Drawing.Size(197, 197)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(197, 197)
        Me.PictureBox1.TabIndex = 2
        Me.PictureBox1.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 130)
        Me.Label1.MaximumSize = New System.Drawing.Size(581, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(581, 27258)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = resources.GetString("Label1.Text")
        '
        'but_Close
        '
        Me.but_Close.AllowFocus = False
        Me.but_Close.Appearance.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.but_Close.Appearance.Options.UseFont = True
        Me.but_Close.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.but_Close.Location = New System.Drawing.Point(510, 240)
        Me.but_Close.Name = "but_Close"
        Me.but_Close.Size = New System.Drawing.Size(100, 28)
        Me.but_Close.TabIndex = 6
        Me.but_Close.Text = "Close"
        '
        'DefaultLookAndFeel1
        '
        Me.DefaultLookAndFeel1.LookAndFeel.SkinName = "DevExpress Dark Style"
        '
        'AboutForm
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 21.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(632, 280)
        Me.Controls.Add(Me.but_Close)
        Me.Controls.Add(Me.XtraScrollableControl1)
        Me.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "AboutForm"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "About"
        Me.TopMost = True
        Me.XtraScrollableControl1.ResumeLayout(False)
        Me.XtraScrollableControl1.PerformLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents XtraScrollableControl1 As DevExpress.XtraEditors.XtraScrollableControl
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents but_ReleaseNotes As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents but_Close As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents but_Tutorial As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents DefaultLookAndFeel1 As DevExpress.LookAndFeel.DefaultLookAndFeel
End Class
