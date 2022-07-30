Imports System.Drawing
Imports System.Drawing.Imaging

Public Class HSLFilter
    Inherits BasicFilter

    Private _hue As Double = 0.0
    Private _saturation As Double = 0.0
    Private _lightness As Double = 0.0

    ''' <summary>
    ''' Get or set hue correction value.
    ''' </summary>
    ''' <value>Any double, will be scaled to range [0..360).</value>
    ''' <returns>Double in range [0..360).</returns>
    Public Property Hue() As Double
        Get
            Return _hue
        End Get
        Set(ByVal value As Double)
            _hue = value
            Do While _hue < 0.0
                _hue += 360
            Loop
            Do While _hue >= 360.0
                _hue -= 360
            Loop
        End Set
    End Property

    ''' <summary>
    ''' Get or set saturation correction value.
    ''' </summary>
    ''' <value>Double in range [-100..+100]%.</value>
    ''' <returns>Double in range [-100..+100]%.</returns>
    ''' <remarks></remarks>
    Public Property Saturation() As Double
        Get
            Return _saturation
        End Get
        Set(ByVal value As Double)
            If ((value >= -100.0) And (value <= 100.0)) Then
                _saturation = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' Get or set lightness correction value.
    ''' </summary>
    ''' <value>Double in range [-100..+100]%.</value>
    ''' <returns>Double in range [-100..+100]%.</returns>
    ''' <remarks></remarks>
    Public Property Lightness() As Double
        Get
            Return _lightness
        End Get
        Set(ByVal value As Double)
            If ((value >= -100.0) And (value <= 100.0)) Then
                _lightness = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' Execute filter and return filtered image.
    ''' </summary>
    ''' <param name="img"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function ExecuteFilter(ByVal img As System.Drawing.Image) As System.Drawing.Image
        Select Case img.PixelFormat
            Case PixelFormat.Format16bppGrayScale
                Return img
            Case PixelFormat.Format24bppRgb, _
                     PixelFormat.Format32bppArgb, PixelFormat.Format32bppRgb
                Return ExecuteRgb8(img)
            Case PixelFormat.Format48bppRgb
                Return img
            Case Else
                Return img
        End Select
    End Function

    ''' <summary>
    ''' Execute filter on (A)RGB image with 8 bits per color.
    ''' </summary>
    ''' <param name="img"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ExecuteRgb8(ByVal img As System.Drawing.Image) As System.Drawing.Image
        Const c1o60 As Double = 1 / 60
        Const c1o255 As Double = 1 / 255
        Dim result As Bitmap = New Bitmap(img)
        result.SetResolution(img.HorizontalResolution, img.VerticalResolution)
        Dim bmpData As BitmapData = result.LockBits( _
                       New Rectangle(0, 0, result.Width, result.Height), _
                       ImageLockMode.ReadWrite, img.PixelFormat)
        Dim pixelBytes As Integer = _
                 System.Drawing.Image.GetPixelFormatSize(img.PixelFormat) \ 8
        'Get the address of the first line.
        Dim ptr As IntPtr = bmpData.Scan0
        Dim size As Integer = bmpData.Stride * result.Height
        Dim pixels(size - 1) As Byte
        Dim index As Integer
        Dim R, G, B As Double
        Dim H, S, L, H1 As Double
        Dim min, max, dif, sum As Double
        Dim f1, f2 As Double
        Dim v1, v2, v3 As Double
        Dim sat As Double = 127 * _saturation / 100
        Dim lum As Double = 127 * _lightness / 100
        'Copy the RGB values into the array.
        System.Runtime.InteropServices.Marshal.Copy(ptr, pixels, 0, size)
        'Main loop.
        For row As Integer = 0 To result.Height - 1
            For col As Integer = 0 To result.Width - 1
                index = (row * bmpData.Stride) + (col * pixelBytes)
                R = pixels(index + 2)
                G = pixels(index + 1)
                B = pixels(index + 0)
                'Conversion to HSL space.
                min = R
                If (G < min) Then min = G
                If (B < min) Then min = B
                max = R : f1 = 0.0 : f2 = G - B
                If (G > max) Then
                    max = G : f1 = 120.0 : f2 = B - R
                End If
                If (B > max) Then
                    max = B : f1 = 240.0 : f2 = R - G
                End If
                dif = max - min
                sum = max + min
                L = 0.5 * sum
                If (dif = 0) Then
                    H = 0.0 : S = 0.0
                Else
                    If (L < 127.5) Then
                        S = 255.0 * dif / sum
                    Else
                        S = 255.0 * dif / (510.0 - sum)
                    End If
                    H = (f1 + 60.0 * f2 / dif)
                    If H < 0.0 Then H += 360.0
                    If H >= 360.0 Then H -= 360.0
                End If
                'Apply transformation.
                H = H + _hue
                If H >= 360.0 Then H = H - 360.0
                S = S + sat
                If S < 0.0 Then S = 0.0
                If S > 255.0 Then S = 255.0
                L = L + lum
                If L < 0.0 Then L = 0.0
                If L > 255.0 Then L = 255.0
                'Conversion back to RGB space.
                If (S = 0) Then
                    R = L : G = L : B = L
                Else
                    If (L < 127.5) Then
                        v2 = c1o255 * L * (255 + S)
                    Else
                        v2 = L + S - c1o255 * S * L
                    End If
                    v1 = 2 * L - v2
                    v3 = v2 - v1
                    H1 = H + 120.0
                    If (H1 >= 360.0) Then H1 -= 360.0
                    If (H1 < 60.0) Then
                        R = v1 + v3 * H1 * c1o60
                    ElseIf (H1 < 180.0) Then
                        R = v2
                    ElseIf (H1 < 240.0) Then
                        R = v1 + v3 * (4 - H1 * c1o60)
                    Else
                        R = v1
                    End If
                    H1 = H
                    If (H1 < 60.0) Then
                        G = v1 + v3 * H1 * c1o60
                    ElseIf (H1 < 180.0) Then
                        G = v2
                    ElseIf (H1 < 240.0) Then
                        G = v1 + v3 * (4 - H1 * c1o60)
                    Else
                        G = v1
                    End If
                    H1 = H - 120.0
                    If (H1 < 0.0) Then H1 += 360.0
                    If (H1 < 60.0) Then
                        B = v1 + v3 * H1 * c1o60
                    ElseIf (H1 < 180.0) Then
                        B = v2
                    ElseIf (H1 < 240.0) Then
                        B = v1 + v3 * (4 - H1 * c1o60)
                    Else
                        B = v1
                    End If
                End If
                'Save new values.
                pixels(index + 2) = CByte(R)
                pixels(index + 1) = CByte(G)
                pixels(index + 0) = CByte(B)
            Next
        Next
        'Copy the RGB values back to the bitmap
        System.Runtime.InteropServices.Marshal.Copy(pixels, 0, ptr, size)
        'Unlock the bits.
        result.UnlockBits(bmpData)
        Return result
    End Function

End Class