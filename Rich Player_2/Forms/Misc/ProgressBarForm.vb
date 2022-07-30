


Public Class ProgressBarForm
    Private Sub ProgressBarForm_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        PreVentFlicker()
        TransferProgressBar.Visible = True
        ProgressBarBackgroundWorker.RunWorkerAsync()
    End Sub
    Private Sub PreVentFlicker()
        With Me
            .SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
            .SetStyle(ControlStyles.UserPaint, True)
            .SetStyle(ControlStyles.AllPaintingInWmPaint, True)
            .UpdateStyles()
        End With
    End Sub
    Private Sub ProgressBarBackgroundWorker_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles ProgressBarBackgroundWorker.DoWork
        For i = 0 To TransferProgressBar.Properties.Maximum
            'Dim Percentage As Integer = Math.Round(((i / (TransferProgressBar.Maximum - TransferProgressBar.Minimum)) * 100))
            ProgressBarBackgroundWorker.ReportProgress(i / 100)
        Next

    End Sub

    Private Sub ProgressBarBackgroundWorker_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles ProgressBarBackgroundWorker.ProgressChanged
        TransferProgressBar.EditValue = e.ProgressPercentage
        If TransferProgressBar.EditValue = 100 Then
            Me.Hide()
        End If
        'PercentageLabel.Text = "Processing....." & TransferProgressBar.Value.ToString() & "%"
    End Sub

    Private Sub ProgressBarBackgroundWorker_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles ProgressBarBackgroundWorker.RunWorkerCompleted
        '  MsgBox("Task Completed!")
        ' Me.Close()
    End Sub

    Public Sub UpdateProgress(pct As Integer)

        ' ToDo: Add error checking 
        ' TransferProgressBar.Value = TransferProgressBar.Maximum
        TransferProgressBar.EditValue = pct
    End Sub

End Class