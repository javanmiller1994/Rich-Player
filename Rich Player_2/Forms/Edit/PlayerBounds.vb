Public Class PlayerBounds 
    Public Sub LoadLayout()
        '
        'label_Size
        '
        Me.label_Size.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold)
        Me.label_Size.Location = New System.Drawing.Point(12, 9)
        Me.label_Size.Size = New System.Drawing.Size(76, 21)
        '
        'RichLabel1
        '
        Me.RichLabel1.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold)
        Me.RichLabel1.Location = New System.Drawing.Point(12, 194)
        Me.RichLabel1.Size = New System.Drawing.Size(111, 21)
        '
        'tb_Height
        '
        Me.tb_Height.Location = New System.Drawing.Point(91, 91)
        Me.tb_Height.MaximumSize = New System.Drawing.Size(230, 35)
        Me.tb_Height.MinimumSize = New System.Drawing.Size(230, 35)
        Me.tb_Height.Size = New System.Drawing.Size(230, 35)
        '
        'tb_Width
        '
        Me.tb_Width.Location = New System.Drawing.Point(91, 50)
        Me.tb_Width.MaximumSize = New System.Drawing.Size(230, 35)
        Me.tb_Width.MinimumSize = New System.Drawing.Size(230, 35)
        Me.tb_Width.Size = New System.Drawing.Size(230, 35)
        '
        'tb_X
        '
        Me.tb_X.Location = New System.Drawing.Point(91, 227)
        Me.tb_X.MaximumSize = New System.Drawing.Size(230, 35)
        Me.tb_X.MinimumSize = New System.Drawing.Size(230, 35)
        Me.tb_X.Size = New System.Drawing.Size(230, 35)
        '
        'tb_Y
        '
        Me.tb_Y.Location = New System.Drawing.Point(91, 268)
        Me.tb_Y.MaximumSize = New System.Drawing.Size(230, 35)
        Me.tb_Y.MinimumSize = New System.Drawing.Size(230, 35)
        Me.tb_Y.Size = New System.Drawing.Size(230, 35)
        '
        'but_Reset
        '
        Me.but_Reset.Appearance.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.but_Reset.Location = New System.Drawing.Point(91, 313)
        Me.but_Reset.Size = New System.Drawing.Size(230, 48)
        '
        'but_Okay
        '
        Me.but_Okay.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.but_Okay.Appearance.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.but_Okay.Location = New System.Drawing.Point(38, 394)
        Me.but_Okay.Size = New System.Drawing.Size(132, 35)
        '
        'but_Cancel
        '
        Me.but_Cancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.but_Cancel.Appearance.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.but_Cancel.Location = New System.Drawing.Point(176, 394)
        Me.but_Cancel.Size = New System.Drawing.Size(132, 35)
        '
        'RichLabel2
        '
        Me.RichLabel2.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.RichLabel2.Location = New System.Drawing.Point(26, 54)
        Me.RichLabel2.Size = New System.Drawing.Size(55, 21)
        '
        'RichLabel3
        '
        Me.RichLabel3.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.RichLabel3.Location = New System.Drawing.Point(26, 95)
        Me.RichLabel3.Size = New System.Drawing.Size(59, 21)
        '
        'RichLabel4
        '
        Me.RichLabel4.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.RichLabel4.Location = New System.Drawing.Point(26, 231)
        Me.RichLabel4.Size = New System.Drawing.Size(22, 21)
        '
        'RichLabel5
        '
        Me.RichLabel5.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.RichLabel5.Location = New System.Drawing.Point(26, 272)
        Me.RichLabel5.Size = New System.Drawing.Size(22, 21)
        '
        'SimpleButton1
        '
        Me.SimpleButton1.Appearance.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.SimpleButton1.Location = New System.Drawing.Point(91, 132)
        Me.SimpleButton1.Size = New System.Drawing.Size(230, 48)
    End Sub
    Private Sub PreVentFlicker()
        With Me
            .SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
            .SetStyle(ControlStyles.UserPaint, True)
            .SetStyle(ControlStyles.AllPaintingInWmPaint, True)
            .UpdateStyles()
        End With
    End Sub

    Private Sub PlayerBounds_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PreVentFlicker()
        LoadLayout()

        tb_Width.Text = My.Settings.FormSize.Width
        tb_Height.Text = My.Settings.FormSize.Height
        tb_X.Text = My.Settings.PlayerLocation.X
        tb_Y.Text = My.Settings.PlayerLocation.Y



    End Sub

    Private Sub but_Okay_Click(sender As Object, e As EventArgs) Handles but_Okay.Click
        My.Settings.FormSize = New Drawing.Size(tb_Width.Text, tb_Height.Text)
        My.Settings.PlayerLocation = New Drawing.Point(tb_X.Text, tb_Y.Text)


        My.Settings.Save()

    End Sub

    Private Sub but_Reset_Click(sender As Object, e As EventArgs) Handles but_Reset.Click
        Dim result As Integer = Forms.MessageBox.Show("Are you sure you want to reset app location?" & Environment.NewLine & Environment.NewLine & "Should you change your mind, click cancel from the Rich Player Bounds window.", "Reset App Location?", MessageBoxButtons.YesNo)
        If result = DialogResult.Yes Then
            tb_X.Text = 0
            tb_Y.Text = 0
        End If
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        Dim result As Integer = Forms.MessageBox.Show("Are you sure you want to reset app size?" & Environment.NewLine & Environment.NewLine & "Should you change your mind, click cancel from the Rich Player Bounds window.", "Reset App Size?", MessageBoxButtons.YesNo)
        If result = DialogResult.Yes Then
            tb_Width.Text = 800
            tb_Height.Text = 440
        End If
    End Sub


End Class