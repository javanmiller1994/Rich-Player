<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ProgressBarForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.ProgressBarBackgroundWorker = New System.ComponentModel.BackgroundWorker()
        Me.TransferProgressBar = New DevExpress.XtraEditors.ProgressBarControl()
        CType(Me.TransferProgressBar.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ProgressBarBackgroundWorker
        '
        Me.ProgressBarBackgroundWorker.WorkerReportsProgress = True
        Me.ProgressBarBackgroundWorker.WorkerSupportsCancellation = True
        '
        'TransferProgressBar
        '
        Me.TransferProgressBar.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TransferProgressBar.EditValue = "50"
        Me.TransferProgressBar.Location = New System.Drawing.Point(0, 0)
        Me.TransferProgressBar.Name = "TransferProgressBar"
        Me.TransferProgressBar.Properties.LookAndFeel.SkinMaskColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.TransferProgressBar.Properties.LookAndFeel.SkinName = "Visual Studio 2013 Dark"
        Me.TransferProgressBar.Properties.LookAndFeel.UseDefaultLookAndFeel = False
        Me.TransferProgressBar.Size = New System.Drawing.Size(242, 12)
        Me.TransferProgressBar.TabIndex = 1
        '
        'ProgressBarForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(242, 12)
        Me.Controls.Add(Me.TransferProgressBar)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "ProgressBarForm"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "ProgressBarForm"
        Me.TopMost = True
        Me.TransparencyKey = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        CType(Me.TransferProgressBar.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ProgressBarBackgroundWorker As System.ComponentModel.BackgroundWorker
    Friend WithEvents TransferProgressBar As DevExpress.XtraEditors.ProgressBarControl
End Class
