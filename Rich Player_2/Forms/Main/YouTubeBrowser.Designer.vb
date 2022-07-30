<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class YouTubeBrowser
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(YouTubeBrowser))
        Me.DefaultLookAndFeel1 = New DevExpress.LookAndFeel.DefaultLookAndFeel(Me.components)
        Me.but_Back = New DevExpress.XtraEditors.SimpleButton()
        Me.but_Forward = New DevExpress.XtraEditors.SimpleButton()
        Me.but_Refresh = New DevExpress.XtraEditors.SimpleButton()
        Me.but_Add = New DevExpress.XtraEditors.SimpleButton()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.cb_Hotkeys = New DevExpress.XtraEditors.CheckEdit()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Panel1.SuspendLayout()
        CType(Me.cb_Hotkeys.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.Panel1.Controls.Add(Me.cb_Hotkeys)
        Me.Panel1.Controls.Add(Me.but_Add)
        Me.Panel1.Controls.Add(Me.but_Refresh)
        Me.Panel1.Controls.Add(Me.but_Forward)
        Me.Panel1.Controls.Add(Me.but_Back)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(590, 46)
        Me.Panel1.TabIndex = 1
        '
        'cb_Hotkeys
        '
        Me.cb_Hotkeys.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cb_Hotkeys.Location = New System.Drawing.Point(394, 12)
        Me.cb_Hotkeys.Name = "cb_Hotkeys"
        Me.cb_Hotkeys.Properties.AllowFocused = False
        Me.cb_Hotkeys.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 10.0!)
        Me.cb_Hotkeys.Properties.Appearance.Options.UseFont = True
        Me.cb_Hotkeys.Properties.Caption = "Use Hotkeys"
        Me.cb_Hotkeys.Size = New System.Drawing.Size(95, 20)
        Me.cb_Hotkeys.TabIndex = 1
        Me.cb_Hotkeys.ToolTip = "Check to control YouTube playback with Global Hotkeys"
        '
        'ToolTip1
        '
        Me.ToolTip1.BackColor = System.Drawing.Color.FromArgb(CType(CType(95, Byte), Integer), CType(CType(95, Byte), Integer), CType(CType(95, Byte), Integer))
        Me.ToolTip1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.ToolTip1.OwnerDraw = True
        '
        'YouTubeBrowser
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(590, 345)
        Me.Controls.Add(Me.Panel1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "YouTubeBrowser"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "YouTube"
        Me.Panel1.ResumeLayout(False)
        CType(Me.cb_Hotkeys.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents DefaultLookAndFeel1 As DevExpress.LookAndFeel.DefaultLookAndFeel
    Friend WithEvents but_Back As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents but_Forward As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents but_Refresh As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents but_Add As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents cb_Hotkeys As DevExpress.XtraEditors.CheckEdit
    Public WithEvents ToolTip1 As System.Windows.Forms.ToolTip
End Class
