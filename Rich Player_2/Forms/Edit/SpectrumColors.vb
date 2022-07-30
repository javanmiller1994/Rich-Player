Public Class SpectrumColors 

    Private Sub SpectrumColors_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        pb_Color1.BackColor = My.Settings.SpectrumColor1
        pb_Color2.BackColor = My.Settings.SpectrumColor2
        ColorDialog1.Color = My.Settings.SpectrumColor1
        ColorDialog2.Color = My.Settings.SpectrumColor2

        RefreshImage()



    End Sub
    Public Sub RefreshImage()
        Dim x As Integer
        Dim y As Integer
        Dim red As Byte
        Dim green As Byte
        Dim blue As Byte

        Dim img As Bitmap = My.Resources.Spectrum_ex

        For x = 0 To img.Width - 1
            For y = 0 To img.Height - 1
                red = img.GetPixel(x, y).R
                green = img.GetPixel(x, y).G
                blue = img.GetPixel(x, y).B
                If red < 255 Then
                    img.SetPixel(x, y, ColorDialog1.Color)
                End If
                If green < 255 Then
                    img.SetPixel(x, y, ColorDialog2.Color)
                End If
            Next
        Next
        pb_Spectrum.BackgroundImageLayout = ImageLayout.Center
        pb_Spectrum.Image = img
    End Sub


    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        My.Settings.SpectrumColor1 = pb_Color1.BackColor
        My.Settings.SpectrumColor2 = pb_Color2.BackColor
        My.Settings.Save()
    End Sub

    Private Sub pb_Color1_Click(sender As Object, e As EventArgs) Handles pb_Color1.Click
        If ColorDialog1.ShowDialog = DialogResult.OK Then
            pb_Color1.BackColor = ColorDialog1.Color
            RefreshImage()
        End If
    End Sub

    Private Sub pb_Color2_Click(sender As Object, e As EventArgs) Handles pb_Color2.Click
        If ColorDialog2.ShowDialog = DialogResult.OK Then
            pb_Color2.BackColor = ColorDialog2.Color
            RefreshImage()
        End If
    End Sub

    Private Sub but_Default_Click(sender As Object, e As EventArgs) Handles but_Default.Click
        pb_Color1.BackColor = Color.FromArgb(132, 200, 255)
        pb_Color2.BackColor = Color.FromArgb(174, 145, 251)
        ColorDialog1.Color = Color.FromArgb(132, 200, 255)
        ColorDialog2.Color = Color.FromArgb(174, 145, 251)
        RefreshImage()
    End Sub

    Private Sub but_Saved_Click(sender As Object, e As EventArgs) Handles but_Saved.Click
        pb_Color1.BackColor = My.Settings.SpectrumColor1
        pb_Color2.BackColor = My.Settings.SpectrumColor2
        ColorDialog1.Color = My.Settings.SpectrumColor1
        ColorDialog2.Color = My.Settings.SpectrumColor2

        RefreshImage()
    End Sub

End Class