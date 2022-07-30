Imports System.Drawing
Imports System.Drawing.Drawing2D

Public Interface IFilter

    Function ExecuteFilter( _
             ByVal inputImage As System.Drawing.Image) As System.Drawing.Image

End Interface

Public MustInherit Class BasicFilter
    Implements IFilter

    ''' <summary>
    ''' Background color. Default is a transparent background.
    ''' </summary>
    Private _bgColor As Color = Color.FromArgb(0, 0, 0, 0)
    ''' <summary>
    ''' Interpolation mode. Default is highest quality.
    ''' </summary>
    Private _interpolation As InterpolationMode = _
             InterpolationMode.HighQualityBicubic

    ''' <summary>
    ''' Get or set background color.
    ''' </summary>
    Public Property BackgroundColor() As Color
        Get
            Return _bgColor
        End Get
        Set(ByVal value As Color)
            _bgColor = value
        End Set
    End Property

    ''' <summary>
    ''' Get or set resize interpolation mode.
    ''' </summary>
    Public Property Interpolation() As InterpolationMode
        Get
            Return _interpolation
        End Get
        Set(ByVal value As InterpolationMode)
            _interpolation = value
        End Set
    End Property

    ''' <summary>
    ''' Execute filter function and return new filtered image.
    ''' </summary>
    ''' <param name="img">Image to be filtered.</param>
    ''' <returns>New filtered image.</returns>
    Public MustOverride Function ExecuteFilter( _
             ByVal img As System.Drawing.Image) _
             As System.Drawing.Image Implements IFilter.ExecuteFilter

End Class