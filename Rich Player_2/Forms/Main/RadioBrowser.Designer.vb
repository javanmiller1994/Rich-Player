<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RadioBrowser
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(RadioBrowser))
        Me.DefaultLookAndFeel1 = New DevExpress.LookAndFeel.DefaultLookAndFeel(Me.components)
        Me.but_Back = New DevExpress.XtraEditors.SimpleButton()
        Me.but_Forward = New DevExpress.XtraEditors.SimpleButton()
        Me.but_Refresh = New DevExpress.XtraEditors.SimpleButton()
        Me.but_Add = New DevExpress.XtraEditors.SimpleButton()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.RichLabel2 = New Rich_Player.RichLabel()
        Me.RichLabel1 = New Rich_Player.RichLabel()
        Me.tb_StationName = New System.Windows.Forms.TextBox()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'DefaultLookAndFeel1
        '
        Me.DefaultLookAndFeel1.LookAndFeel.SkinName = "DevExpress Dark Style"
        '
        'but_Back
        '
        Me.but_Back.AllowFocus = False
        Me.but_Back.Appearance.Font = New System.Drawing.Font("Tahoma", 15.0!, System.Drawing.FontStyle.Bold)
        Me.but_Back.Appearance.Options.UseFont = True
        Me.but_Back.Location = New System.Drawing.Point(12, 5)
        Me.but_Back.Name = "but_Back"
        Me.but_Back.Size = New System.Drawing.Size(32, 35)
        Me.but_Back.TabIndex = 0
        Me.but_Back.Text = "‹"
        '
        'but_Forward
        '
        Me.but_Forward.AllowFocus = False
        Me.but_Forward.Appearance.Font = New System.Drawing.Font("Tahoma", 15.0!, System.Drawing.FontStyle.Bold)
        Me.but_Forward.Appearance.Options.UseFont = True
        Me.but_Forward.Location = New System.Drawing.Point(50, 5)
        Me.but_Forward.Name = "but_Forward"
        Me.but_Forward.Size = New System.Drawing.Size(32, 35)
        Me.but_Forward.TabIndex = 0
        Me.but_Forward.Text = "›"
        '
        'but_Refresh
        '
        Me.but_Refresh.AllowFocus = False
        Me.but_Refresh.Appearance.Font = New System.Drawing.Font("Tahoma", 15.0!)
        Me.but_Refresh.Appearance.Options.UseFont = True
        Me.but_Refresh.Location = New System.Drawing.Point(88, 5)
        Me.but_Refresh.Name = "but_Refresh"
        Me.but_Refresh.Size = New System.Drawing.Size(32, 35)
        Me.but_Refresh.TabIndex = 0
        Me.but_Refresh.Text = "↺"
        '
        'but_Add
        '
        Me.but_Add.AllowFocus = False
        Me.but_Add.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.but_Add.Location = New System.Drawing.Point(495, 5)
        Me.but_Add.Name = "but_Add"
        Me.but_Add.Size = New System.Drawing.Size(83, 35)
        Me.but_Add.TabIndex = 0
        Me.but_Add.Text = "Add to Playlist"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.RichLabel2)
        Me.Panel1.Controls.Add(Me.RichLabel1)
        Me.Panel1.Controls.Add(Me.tb_StationName)
        Me.Panel1.Controls.Add(Me.but_Add)
        Me.Panel1.Controls.Add(Me.but_Refresh)
        Me.Panel1.Controls.Add(Me.but_Forward)
        Me.Panel1.Controls.Add(Me.but_Back)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(590, 65)
        Me.Panel1.TabIndex = 1
        '
        'RichLabel2
        '
        Me.RichLabel2.AutoSize = True
        Me.RichLabel2.Location = New System.Drawing.Point(12, 45)
        Me.RichLabel2.Name = "RichLabel2"
        Me.RichLabel2.Size = New System.Drawing.Size(536, 13)
        Me.RichLabel2.TabIndex = 3
        Me.RichLabel2.Text = "Instructions:  Search for desired radio station, click to play it, then proceed w" & _
    "ith naming and adding to playlist."
        Me.RichLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'RichLabel1
        '
        Me.RichLabel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RichLabel1.AutoSize = True
        Me.RichLabel1.Location = New System.Drawing.Point(274, 16)
        Me.RichLabel1.Name = "RichLabel1"
        Me.RichLabel1.Size = New System.Drawing.Size(105, 13)
        Me.RichLabel1.TabIndex = 2
        Me.RichLabel1.Text = "Name Radio Station:"
        '
        'tb_StationName
        '
        Me.tb_StationName.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tb_StationName.BackColor = System.Drawing.Color.Gray
        Me.tb_StationName.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.tb_StationName.Location = New System.Drawing.Point(385, 11)
        Me.tb_StationName.MaximumSize = New System.Drawing.Size(104, 25)
        Me.tb_StationName.MinimumSize = New System.Drawing.Size(104, 25)
        Me.tb_StationName.Name = "tb_StationName"
        Me.tb_StationName.Size = New System.Drawing.Size(104, 14)
        Me.tb_StationName.TabIndex = 1
        '
        'RadioBrowser
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(590, 345)
        Me.Controls.Add(Me.Panel1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "RadioBrowser"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Radio"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents DefaultLookAndFeel1 As DevExpress.LookAndFeel.DefaultLookAndFeel
    Friend WithEvents but_Back As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents but_Forward As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents but_Refresh As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents but_Add As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents RichLabel1 As Rich_Player.RichLabel
    Friend WithEvents tb_StationName As System.Windows.Forms.TextBox
    Friend WithEvents RichLabel2 As Rich_Player.RichLabel
End Class
