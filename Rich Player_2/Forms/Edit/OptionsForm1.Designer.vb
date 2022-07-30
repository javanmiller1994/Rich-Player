<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OptionsForm1
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
        Dim SuperToolTip4 As DevExpress.Utils.SuperToolTip = New DevExpress.Utils.SuperToolTip()
        Dim ToolTipTitleItem3 As DevExpress.Utils.ToolTipTitleItem = New DevExpress.Utils.ToolTipTitleItem()
        Dim ToolTipItem4 As DevExpress.Utils.ToolTipItem = New DevExpress.Utils.ToolTipItem()
        Dim SuperToolTip1 As DevExpress.Utils.SuperToolTip = New DevExpress.Utils.SuperToolTip()
        Dim ToolTipItem1 As DevExpress.Utils.ToolTipItem = New DevExpress.Utils.ToolTipItem()
        Dim SuperToolTip2 As DevExpress.Utils.SuperToolTip = New DevExpress.Utils.SuperToolTip()
        Dim ToolTipTitleItem1 As DevExpress.Utils.ToolTipTitleItem = New DevExpress.Utils.ToolTipTitleItem()
        Dim ToolTipItem2 As DevExpress.Utils.ToolTipItem = New DevExpress.Utils.ToolTipItem()
        Me.PlayCheckBox = New DevExpress.XtraEditors.CheckEdit()
        Me.but_Cancel = New DevExpress.XtraEditors.SimpleButton()
        Me.but_Ok = New DevExpress.XtraEditors.SimpleButton()
        Me.StartupCheckbox = New DevExpress.XtraEditors.CheckEdit()
        Me.DefaultLookAndFeel1 = New DevExpress.LookAndFeel.DefaultLookAndFeel(Me.components)
        Me.but_Update = New DevExpress.XtraEditors.SimpleButton()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.CheckEditTouch = New DevExpress.XtraEditors.CheckEdit()
        Me.DeviceBox = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.AutoSaveCheck = New DevExpress.XtraEditors.CheckEdit()
        Me.rb_Pitch = New System.Windows.Forms.RadioButton()
        Me.rb_Speed = New System.Windows.Forms.RadioButton()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.but_ReportProblem = New DevExpress.XtraEditors.SimpleButton()
        Me.but_ManuallyUpdate = New DevExpress.XtraEditors.SimpleButton()
        Me.label_MainSettings = New Rich_Player.RichLabel()
        Me.label_AudioSettings = New Rich_Player.RichLabel()
        Me.but_VisitWebsite = New DevExpress.XtraEditors.SimpleButton()
        CType(Me.PlayCheckBox.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.StartupCheckbox.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CheckEditTouch.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DeviceBox.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AutoSaveCheck.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PlayCheckBox
        '
        Me.PlayCheckBox.Location = New System.Drawing.Point(14, 76)
        Me.PlayCheckBox.Name = "PlayCheckBox"
        Me.PlayCheckBox.Properties.AllowFocused = False
        Me.PlayCheckBox.Properties.Appearance.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PlayCheckBox.Properties.Appearance.Options.UseFont = True
        Me.PlayCheckBox.Properties.Caption = "Play song on App Start"
        Me.PlayCheckBox.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.[Default]
        Me.PlayCheckBox.Size = New System.Drawing.Size(193, 25)
        ToolTipTitleItem3.Text = "Play On Start"
        ToolTipItem4.LeftIndent = 6
        ToolTipItem4.Text = "Used when app was previously closed with" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "at least one item in the playlist. It w" & _
    "ill then play" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "where the song had left off"
        SuperToolTip4.Items.Add(ToolTipTitleItem3)
        SuperToolTip4.Items.Add(ToolTipItem4)
        Me.PlayCheckBox.SuperTip = SuperToolTip4
        Me.PlayCheckBox.TabIndex = 6
        '
        'but_Cancel
        '
        Me.but_Cancel.AllowFocus = False
        Me.but_Cancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.but_Cancel.Appearance.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.but_Cancel.Appearance.Options.UseFont = True
        Me.but_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.but_Cancel.Location = New System.Drawing.Point(411, 302)
        Me.but_Cancel.Name = "but_Cancel"
        Me.but_Cancel.Size = New System.Drawing.Size(116, 35)
        Me.but_Cancel.TabIndex = 4
        Me.but_Cancel.Text = "Cancel"
        '
        'but_Ok
        '
        Me.but_Ok.AllowFocus = False
        Me.but_Ok.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.but_Ok.Appearance.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.but_Ok.Appearance.Options.UseFont = True
        Me.but_Ok.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.but_Ok.Location = New System.Drawing.Point(289, 302)
        Me.but_Ok.Name = "but_Ok"
        Me.but_Ok.Size = New System.Drawing.Size(116, 35)
        Me.but_Ok.TabIndex = 5
        Me.but_Ok.Text = "Ok"
        '
        'StartupCheckbox
        '
        Me.StartupCheckbox.Location = New System.Drawing.Point(14, 45)
        Me.StartupCheckbox.Name = "StartupCheckbox"
        Me.StartupCheckbox.Properties.AllowFocused = False
        Me.StartupCheckbox.Properties.Appearance.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.StartupCheckbox.Properties.Appearance.Options.UseFont = True
        Me.StartupCheckbox.Properties.Caption = " Start at system startup"
        Me.StartupCheckbox.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.[Default]
        Me.StartupCheckbox.Size = New System.Drawing.Size(193, 25)
        Me.StartupCheckbox.TabIndex = 3
        '
        'DefaultLookAndFeel1
        '
        Me.DefaultLookAndFeel1.LookAndFeel.SkinName = "DevExpress Dark Style"
        '
        'but_Update
        '
        Me.but_Update.AllowFocus = False
        Me.but_Update.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.but_Update.Appearance.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.but_Update.Appearance.Options.UseFont = True
        Me.but_Update.Location = New System.Drawing.Point(16, 265)
        Me.but_Update.Name = "but_Update"
        Me.but_Update.Size = New System.Drawing.Size(116, 35)
        Me.but_Update.TabIndex = 8
        Me.but_Update.Text = "Check for Updates"
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.Label1.Location = New System.Drawing.Point(16, 215)
        Me.Label1.MaximumSize = New System.Drawing.Size(236, 35)
        Me.Label1.MinimumSize = New System.Drawing.Size(236, 35)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(236, 35)
        Me.Label1.TabIndex = 9
        Me.Label1.Text = "Current Version: "
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'CheckEditTouch
        '
        Me.CheckEditTouch.Location = New System.Drawing.Point(14, 107)
        Me.CheckEditTouch.Name = "CheckEditTouch"
        Me.CheckEditTouch.Properties.AllowFocused = False
        Me.CheckEditTouch.Properties.Appearance.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckEditTouch.Properties.Appearance.Options.UseFont = True
        Me.CheckEditTouch.Properties.Caption = "Touch Friendly Mode"
        Me.CheckEditTouch.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.[Default]
        Me.CheckEditTouch.Size = New System.Drawing.Size(193, 25)
        ToolTipItem1.Text = "\"
        SuperToolTip1.Items.Add(ToolTipItem1)
        Me.CheckEditTouch.SuperTip = SuperToolTip1
        Me.CheckEditTouch.TabIndex = 10
        '
        'DeviceBox
        '
        Me.DeviceBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DeviceBox.Location = New System.Drawing.Point(289, 63)
        Me.DeviceBox.Name = "DeviceBox"
        Me.DeviceBox.Properties.AllowFocused = False
        Me.DeviceBox.Properties.Appearance.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.DeviceBox.Properties.Appearance.Options.UseFont = True
        Me.DeviceBox.Properties.AppearanceDropDown.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.DeviceBox.Properties.AppearanceDropDown.Options.UseFont = True
        Me.DeviceBox.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.DeviceBox.Properties.DropDownItemHeight = 40
        Me.DeviceBox.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        Me.DeviceBox.Size = New System.Drawing.Size(232, 28)
        Me.DeviceBox.TabIndex = 12
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.Label2.Location = New System.Drawing.Point(285, 41)
        Me.Label2.MaximumSize = New System.Drawing.Size(236, 0)
        Me.Label2.MinimumSize = New System.Drawing.Size(236, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(236, 19)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "Audio Device"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'AutoSaveCheck
        '
        Me.AutoSaveCheck.Location = New System.Drawing.Point(14, 138)
        Me.AutoSaveCheck.Name = "AutoSaveCheck"
        Me.AutoSaveCheck.Properties.AllowFocused = False
        Me.AutoSaveCheck.Properties.Appearance.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.AutoSaveCheck.Properties.Appearance.Options.UseFont = True
        Me.AutoSaveCheck.Properties.Caption = "Auto Save every 10 mins"
        Me.AutoSaveCheck.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.[Default]
        Me.AutoSaveCheck.Size = New System.Drawing.Size(238, 25)
        Me.AutoSaveCheck.TabIndex = 11
        '
        'rb_Pitch
        '
        Me.rb_Pitch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rb_Pitch.AutoSize = True
        Me.rb_Pitch.BackColor = System.Drawing.Color.Transparent
        Me.rb_Pitch.Checked = True
        Me.rb_Pitch.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rb_Pitch.Location = New System.Drawing.Point(289, 138)
        Me.rb_Pitch.Name = "rb_Pitch"
        Me.rb_Pitch.Size = New System.Drawing.Size(62, 25)
        Me.rb_Pitch.TabIndex = 14
        Me.rb_Pitch.TabStop = True
        Me.rb_Pitch.Text = "Pitch"
        Me.rb_Pitch.UseVisualStyleBackColor = False
        '
        'rb_Speed
        '
        Me.rb_Speed.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rb_Speed.AutoSize = True
        Me.rb_Speed.BackColor = System.Drawing.Color.Transparent
        Me.rb_Speed.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rb_Speed.Location = New System.Drawing.Point(289, 169)
        Me.rb_Speed.Name = "rb_Speed"
        Me.rb_Speed.Size = New System.Drawing.Size(71, 25)
        Me.rb_Speed.TabIndex = 14
        Me.rb_Speed.Text = "Speed"
        Me.rb_Speed.UseVisualStyleBackColor = False
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.Label3.Location = New System.Drawing.Point(285, 107)
        Me.Label3.MaximumSize = New System.Drawing.Size(236, 0)
        Me.Label3.MinimumSize = New System.Drawing.Size(236, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(236, 21)
        Me.Label3.TabIndex = 15
        Me.Label3.Text = "Optimize Sound Quality for ..."
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'but_ReportProblem
        '
        Me.but_ReportProblem.AllowFocus = False
        Me.but_ReportProblem.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.but_ReportProblem.Appearance.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.but_ReportProblem.Appearance.Options.UseFont = True
        Me.but_ReportProblem.Location = New System.Drawing.Point(411, 271)
        Me.but_ReportProblem.Name = "but_ReportProblem"
        Me.but_ReportProblem.Size = New System.Drawing.Size(116, 25)
        ToolTipTitleItem1.Text = "Report a Problem"
        ToolTipItem2.LeftIndent = 6
        ToolTipItem2.Text = "For any bugs or glitches you may notice, use this to send an email to the develop" & _
    "er to made aware so that a fix can be made available as soon as possible."
        SuperToolTip2.Items.Add(ToolTipTitleItem1)
        SuperToolTip2.Items.Add(ToolTipItem2)
        Me.but_ReportProblem.SuperTip = SuperToolTip2
        Me.but_ReportProblem.TabIndex = 17
        Me.but_ReportProblem.Text = "Report A Problem"
        '
        'but_ManuallyUpdate
        '
        Me.but_ManuallyUpdate.AllowFocus = False
        Me.but_ManuallyUpdate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.but_ManuallyUpdate.Appearance.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.but_ManuallyUpdate.Appearance.Options.UseFont = True
        Me.but_ManuallyUpdate.Location = New System.Drawing.Point(138, 265)
        Me.but_ManuallyUpdate.Name = "but_ManuallyUpdate"
        Me.but_ManuallyUpdate.Size = New System.Drawing.Size(116, 35)
        Me.but_ManuallyUpdate.TabIndex = 18
        Me.but_ManuallyUpdate.Text = "Manually Update"
        '
        'label_MainSettings
        '
        Me.label_MainSettings.AutoSize = True
        Me.label_MainSettings.BackColor = System.Drawing.Color.Transparent
        Me.label_MainSettings.Font = New System.Drawing.Font("Segoe UI", 12.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.label_MainSettings.Location = New System.Drawing.Point(12, 14)
        Me.label_MainSettings.MaximumSize = New System.Drawing.Size(236, 0)
        Me.label_MainSettings.MinimumSize = New System.Drawing.Size(236, 0)
        Me.label_MainSettings.Name = "label_MainSettings"
        Me.label_MainSettings.Size = New System.Drawing.Size(236, 21)
        Me.label_MainSettings.TabIndex = 16
        Me.label_MainSettings.Text = "Main Settings"
        Me.label_MainSettings.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'label_AudioSettings
        '
        Me.label_AudioSettings.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.label_AudioSettings.AutoSize = True
        Me.label_AudioSettings.BackColor = System.Drawing.Color.Transparent
        Me.label_AudioSettings.Font = New System.Drawing.Font("Segoe UI", 12.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.label_AudioSettings.Location = New System.Drawing.Point(285, 14)
        Me.label_AudioSettings.MaximumSize = New System.Drawing.Size(236, 0)
        Me.label_AudioSettings.MinimumSize = New System.Drawing.Size(236, 0)
        Me.label_AudioSettings.Name = "label_AudioSettings"
        Me.label_AudioSettings.Size = New System.Drawing.Size(236, 21)
        Me.label_AudioSettings.TabIndex = 13
        Me.label_AudioSettings.Text = "Audio Settings"
        Me.label_AudioSettings.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'but_VisitWebsite
        '
        Me.but_VisitWebsite.AllowFocus = False
        Me.but_VisitWebsite.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.but_VisitWebsite.Appearance.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.but_VisitWebsite.Appearance.Options.UseFont = True
        Me.but_VisitWebsite.Location = New System.Drawing.Point(78, 306)
        Me.but_VisitWebsite.Name = "but_VisitWebsite"
        Me.but_VisitWebsite.Size = New System.Drawing.Size(116, 35)
        Me.but_VisitWebsite.TabIndex = 18
        Me.but_VisitWebsite.Text = "Visit Website"
        '
        'OptionsForm1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImageLayoutStore = System.Windows.Forms.ImageLayout.Stretch
        Me.BackgroundImageStore = Global.Rich_Player.My.Resources.Resources.form_options
        Me.ClientSize = New System.Drawing.Size(533, 349)
        Me.Controls.Add(Me.but_VisitWebsite)
        Me.Controls.Add(Me.but_ManuallyUpdate)
        Me.Controls.Add(Me.but_ReportProblem)
        Me.Controls.Add(Me.label_MainSettings)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.rb_Speed)
        Me.Controls.Add(Me.rb_Pitch)
        Me.Controls.Add(Me.label_AudioSettings)
        Me.Controls.Add(Me.DeviceBox)
        Me.Controls.Add(Me.AutoSaveCheck)
        Me.Controls.Add(Me.CheckEditTouch)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.but_Update)
        Me.Controls.Add(Me.PlayCheckBox)
        Me.Controls.Add(Me.but_Cancel)
        Me.Controls.Add(Me.but_Ok)
        Me.Controls.Add(Me.StartupCheckbox)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "OptionsForm1"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Options"
        Me.TopMost = True
        CType(Me.PlayCheckBox.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.StartupCheckbox.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CheckEditTouch.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DeviceBox.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.AutoSaveCheck.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PlayCheckBox As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents but_Cancel As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents but_Ok As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents StartupCheckbox As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents DefaultLookAndFeel1 As DevExpress.LookAndFeel.DefaultLookAndFeel
    Friend WithEvents but_Update As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents CheckEditTouch As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents DeviceBox As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents AutoSaveCheck As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents label_AudioSettings As Rich_Player.RichLabel
    Friend WithEvents rb_Pitch As System.Windows.Forms.RadioButton
    Friend WithEvents rb_Speed As System.Windows.Forms.RadioButton
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents label_MainSettings As Rich_Player.RichLabel
    Friend WithEvents but_ReportProblem As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents but_ManuallyUpdate As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents but_VisitWebsite As DevExpress.XtraEditors.SimpleButton
End Class
