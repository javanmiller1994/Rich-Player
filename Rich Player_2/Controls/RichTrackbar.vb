Imports System.ComponentModel
Imports System.Drawing.Drawing2D
Imports System.Drawing.Text
Imports System.Drawing

<DefaultEvent("ValueChanged")> _
Public Class RichTrackBar
    Inherits Control

#Region "Declarations"
    Private _Maximum As Integer = 100
    Dim _Minimum As Integer = 0
    Private _Value As Integer = 50
    Private CaptureMovement As Boolean = False
    Private Bar As Rectangle = New Rectangle(0, 10, Width - 16, BarHeight) ' Height - 31)
    Private Track As Size = New Size(14, 14)
    Dim _Color_Bar As Color = Color.Transparent
    Private _Color_Text As Color = Color.Silver
    Private _Color_BarLeft As Color = Color.FromArgb(23, 119, 151)
    Public _Color_BarLeftInactive As Color = Color.Gray
    Private _Color_BarRight As Color = Color.FromArgb(60, 60, 60)
    Private _Color_Border As Color = Color.FromArgb(35, 35, 35)
    Private _Color_Slider As Color = Color.Silver  'Color.FromArgb(47, 47, 47)
    Private _Color_SliderLow As Color = Color.FromArgb(185, 185, 185)
    Private _Color_Sliderhigh As Color = Color.FromArgb(200, 200, 200)
    Dim _BorderThickness As Integer = 1
    Private Init As Boolean = True
    Public IsMouseEnter As Boolean


    Dim BarHeight As Integer = 3
    '   Dim _SliderShape As String = "Circle"
    Dim PaintTimer As New Timer

#End Region

    Public Enum SliderShapes
        Circle
        RectangleNarrow
        RectangleWide
        Square
        Pentagon
        RoundedRectangle
    End Enum
    Public Enum SliderFilled
        Yes
        No
    End Enum
    Public Enum ShowText
        No
        Yes
    End Enum

#Region "Properties"

    <Category("Colors")> _
    Public Property Color_Bar As Color
        Get
            Return _Color_Bar
        End Get
        Set(value As Color)
            _Color_Bar = value
        End Set
    End Property

    <Category("Colors")> _
    Public Property Color_Border As Color
        Get
            Return _Color_Border
        End Get
        Set(value As Color)
            _Color_Border = value
        End Set
    End Property
    <Category("Colors")> _
    Public Property BorderThickness As Integer
        Get
            Return _BorderThickness
        End Get
        Set(value As Integer)
            _BorderThickness = value
        End Set
    End Property


    <Category("Colors")> _
    Public Property Color_Slider As Color
        Get
            Return _Color_Slider
        End Get
        Set(value As Color)
            _Color_Slider = value
        End Set
    End Property

    <Category("Colors")> _
    Public Property Color_BarLeft As Color
        Get
            Return _Color_BarLeft
        End Get
        Set(value As Color)
            _Color_BarLeft = value
        End Set
    End Property

    <Category("Colors")> _
    Public Property Color_BarLeftInactive As Color
        Get
            Return _Color_BarLeftInactive
        End Get
        Set(value As Color)
            _Color_BarLeftInactive = value
        End Set
    End Property

    <Category("Colors")> _
    Public Property Color_BarRight As Color
        Get
            Return _Color_BarRight
        End Get
        Set(value As Color)
            _Color_BarRight = value
        End Set
    End Property

    <Category("Colors")> _
    Public Property Color_Text As Color
        Get
            Return _Color_Text
        End Get
        Set(value As Color)
            _Color_Text = value
        End Set
    End Property

    Dim _ShowText As ShowText
    <Category("Appearance")> _
    Public Property TextShown() As ShowText
        Get
            Return _ShowText
        End Get
        Set(value As ShowText)
            _ShowText = value
            Invalidate()
        End Set
    End Property

    Dim _SliderShape As SliderShapes
    <Category("Appearance")> _
<Description("Shape for the Slider")> _
    Public Property SliderShape() As SliderShapes
        Get
            Return _SliderShape ' _SliderShape
        End Get
        Set(ByVal value As SliderShapes)
            _SliderShape = value

            Invalidate()
            RaiseEvent ValueChanged(Me)
        End Set
    End Property
    Dim _SliderFilled As SliderFilled
    <Category("Appearance")> _
<Description("Fill for the Slider")> _
    Public Property SliderFill() As SliderFilled
        Get
            Return _SliderFilled
        End Get
        Set(ByVal value As SliderFilled)
            _SliderFilled = value
            Invalidate()
        End Set
    End Property


    Public Property Maximum() As Integer
        Get
            Return _Maximum
        End Get
        Set(ByVal value As Integer)
            If Not Init Then
                If value <= _Minimum Then value = _Minimum '- 10
                If _Value > value Then _Value = value
            End If
            _Maximum = value
            Invalidate()
        End Set
    End Property
    Public Property Minimum() As Integer
        Get
            Return _Minimum
        End Get
        Set(ByVal value As Integer)
            If Not Init Then
                If value >= _Maximum Then value = _Maximum '+ 10
                If _Value < value Then _Value = value
            End If
            _Minimum = value
            Invalidate()
        End Set
    End Property


    Public Event ValueChanged(sender As Object)

    Public Property Value() As Integer
        Get
            Return _Value
        End Get
        Set(ByVal value As Integer)
            If _Value <> value Then
                If value < _Minimum Then
                    _Value = _Minimum
                Else
                    If value > _Maximum Then
                        _Value = _Maximum
                    Else
                        _Value = value
                    End If
                End If
                Me.Invalidate()
                RaiseEvent ValueChanged(Me)
            End If
        End Set
    End Property


    Private _TickInterval As Integer = 10
    <Category("Appearance Slider")> _
    <Description("The Interval between the Tick Marks")> _
    Public Property TickInterval() As Integer
        Get
            Return _TickInterval
        End Get
        Set(ByVal value As Integer)
            _TickInterval = value
            Me.Invalidate()
        End Set
    End Property

    Private _TickWidth As Integer = 5
    <Category("Appearance Slider")> _
    <Description("How long to draw the Tick Marks")> _
    Public Property TickWidth() As Integer
        Get
            Return _TickWidth
        End Get
        Set(ByVal value As Integer)
            _TickWidth = value
            Me.Invalidate()
        End Set
    End Property


    Protected Overrides Sub OnHandleCreated(ByVal e As System.EventArgs)
        BackColor = Color.Transparent
        MyBase.OnHandleCreated(e)
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        Dim MovementPoint As New Rectangle(New Point(e.Location.X, e.Location.Y), New Size(1, 1))
        Dim Bar As New Rectangle(9, 10, Width - 16, Height - 21)
        '  If New Rectangle(New Point(Bar.X + CInt(Bar.Width * (Value / Maximum)) - CInt(Track.Width / 2 - 1), 0), New Size(Track.Width, Height)).IntersectsWith(MovementPoint) Then
        If e.Button = Forms.MouseButtons.Left Then
            CaptureMovement = True

            Try
                Value = CInt(((MovementPoint.X - Bar.X) / (Bar.Width / (Maximum - Minimum))) + Minimum)
            Catch ex As Exception
            End Try
        End If

        'End If
    End Sub



    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        CaptureMovement = False
    End Sub

    Protected Overrides Sub OnMouseMove(ByVal e As MouseEventArgs)
        MyBase.OnMouseMove(e)
        If CaptureMovement Then
            Dim MovementPoint As New Point(e.X, e.Y)
            Dim Bar As New Rectangle(9, 10, Width - 16, Height - 21)
            Try
                Value = CInt(((MovementPoint.X - Bar.X) / (Bar.Width / (Maximum - Minimum))) + Minimum)
            Catch ex As Exception
            End Try
            Invalidate()

        End If
        MouseX = e.X
        MouseY = e.Y
    End Sub

    Dim MouseX As Single
    Dim MouseY As Single

    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        MyBase.OnMouseLeave(e) : CaptureMovement = False
        Me.IsMouseEnter = False
        Me.Invalidate()
    End Sub
    Protected Overrides Sub OnMouseEnter(ByVal e As EventArgs)
        MyBase.OnMouseEnter(e) ': CaptureMovement = True
        Me.IsMouseEnter = True
        Me.Invalidate()

    End Sub


#End Region

#Region "Draw Control"

    Sub New()
        SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or _
                    ControlStyles.UserPaint Or ControlStyles.Selectable Or _
                    ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        PaintTimer.Interval = 1
        AddHandler PaintTimer.Tick, AddressOf PaintTimer_Elasped
        PaintTimer.Start()
        Init = False
        Me.MaximumSize = New Size(2000, 23)

    End Sub


    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height) : Dim G = Graphics.FromImage(B)
        With G
            .SmoothingMode = SmoothingMode.HighQuality : .PixelOffsetMode = PixelOffsetMode.HighQuality : .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
            Bar = New Rectangle(9, 10, Width - 16, BarHeight)
            Dim StartY1 As Integer = CInt((Height / 2) - 4)
            .Clear(_Color_Bar) : .SmoothingMode = SmoothingMode.AntiAlias : .TextRenderingHint = TextRenderingHint.AntiAliasGridFit
            .FillRectangle(New SolidBrush(_Color_BarRight), New Rectangle(3, StartY1, Width - 13, 3))
            '                                                      replace   CInt((Height / 2) - 4) with Bar.Y - (Track.Height / 2)

            Dim StartXLine As Integer = CInt(Bar.X + ((Bar.Width) * ((Value - Minimum) / (Maximum - Minimum))) - CInt(Track.Width / 2)) + 2
            Dim StartX As Integer = StartXLine - 4
            If IsMouseEnter Then
                ' Solid Left Line   
                .FillRectangle(New SolidBrush(_Color_BarLeft), New Rectangle(3, StartY1, StartXLine, 3))
                '' Gradient Left Line
                ' Dim BarLeftBrush As New LinearGradientBrush(New Point(0, 0), New Point(Width, 0), _Color_BarLeft, Color.FromArgb(68, 131, 168))
                ' .FillRectangle(BarLeftBrush, New Rectangle(3, CInt((Height / 2) - 4), CInt(Bar.Width * ((_Value - _Minimum) / (_Maximum - _Minimum)))   + CInt(Track.Width / 2), 3))
            Else
                .FillRectangle(New SolidBrush(_Color_BarLeftInactive), New Rectangle(3, StartY1, StartXLine, 3))
            End If
            Dim StartY As Integer = StartY1 - (Track.Height / 2) + 2
            If IsMouseEnter Then
                Select Case SliderShape
                    Case SliderShape.Circle
                        .SmoothingMode = SmoothingMode.HighQuality : .PixelOffsetMode = PixelOffsetMode.HighQuality
                        If SliderFill = SliderFilled.Yes Then
                            .FillEllipse(New SolidBrush(_Color_Slider), StartX, StartY, Track.Width, Track.Height)
                        End If
                        .DrawEllipse(New Pen(_Color_Border, _BorderThickness), StartX, StartY, Track.Width, Track.Height)

                    Case SliderShape.RectangleNarrow
                        .SmoothingMode = SmoothingMode.Default : .PixelOffsetMode = PixelOffsetMode.Default
                        If SliderFill = SliderFilled.Yes Then
                            .FillRectangle(New SolidBrush(_Color_Slider), StartX + 3, StartY, 5, Track.Height)
                        End If
                        .DrawRectangle(New Pen(_Color_Border, _BorderThickness), StartX + 3, StartY, 5, Track.Height)

                    Case SliderShape.RectangleWide
                        .SmoothingMode = SmoothingMode.Default : .PixelOffsetMode = PixelOffsetMode.Default
                        If SliderFill = SliderFilled.Yes Then
                            .FillRectangle(New SolidBrush(_Color_Slider), StartX, StartY, 20, Track.Height)
                            ' .FillRectangle(New LinearGradientBrush(New Point(StartX, 4), New Point(StartX, 14 + 4), _Color_Sliderhigh, _Color_SliderLow), _
                            '                Bar.X + CInt(Bar.Width * ((_Value - _Minimum) / (_Maximum - _Minimum)))   - CInt(Track.Width / 2), 4, 20, Track.Height)
                        End If
                        .DrawRectangle(New Pen(_Color_Border, _BorderThickness), StartX, StartY, 20, Track.Height)


                    Case SliderShape.Square
                        .SmoothingMode = SmoothingMode.Default : .PixelOffsetMode = PixelOffsetMode.Default
                        If SliderFill = SliderFilled.Yes Then
                            .FillRectangle(New SolidBrush(_Color_Slider), StartX, StartY, Track.Width, Track.Height)
                        End If
                        .DrawRectangle(New Pen(_Color_Border, _BorderThickness), StartX, StartY, Track.Width, Track.Height)

                    Case SliderShape.Pentagon
                        .SmoothingMode = SmoothingMode.Default : .PixelOffsetMode = PixelOffsetMode.Default
                        Dim point1 As New Point(StartX, StartY) : Dim point2 As New Point(StartX, StartY + 12) : Dim point3 As New Point(StartX + 6, StartY + 18) : Dim point4 As New Point(StartX + 12, StartY + 12) : Dim point5 As New Point(StartX + 12, StartY) : Dim path As Point() = {point1, point2, point3, point4, point5}
                        If SliderFill = SliderFilled.Yes Then
                            .FillPolygon(New SolidBrush(_Color_Slider), path)
                        End If
                        .DrawPolygon(New Pen(_Color_Border, _BorderThickness), path)

                    Case SliderShape.RoundedRectangle
                        .SmoothingMode = SmoothingMode.HighSpeed : .PixelOffsetMode = PixelOffsetMode.HighSpeed
                        Dim r As New Rectangle(StartX + 2, StartY, 8, Track.Height)
                        Dim Roundness As Integer = 6
                        If SliderFill = SliderFilled.Yes Then
                            FillRoundedRectangle(G, r, Roundness, New SolidBrush(_Color_Slider))
                        End If
                        If _BorderThickness > 1 Then
                            .SmoothingMode = SmoothingMode.HighQuality : .PixelOffsetMode = PixelOffsetMode.HighQuality
                        End If
                        DrawRoundedRectangle(G, r, Roundness, New Pen(_Color_Border, _BorderThickness))
                End Select : End If : End With
        G.Dispose() : e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic : e.Graphics.DrawImageUnscaled(B, 0, 0) : B.Dispose()

        'Create Text
        If _ShowText = ShowText.Yes Then
            If IsMouseEnter Then
                Dim x As Single = Me.ClientRectangle.X + (Bar.Width / 2) - 4
                Dim y As Single = Me.ClientRectangle.Y + 28
                Dim x2 As Single
                If MouseX >= (Me.ClientRectangle.Width / 2) Then
                    x2 = MouseX - 35
                Else
                    x2 = MouseX + 15
                End If
                e.Graphics.DrawString(Me.Text, Me.Font, New SolidBrush(_Color_Text), x2, MouseY)
            End If

        End If

    End Sub
    Public Sub FillRoundedRectangle(ByVal g As Drawing.Graphics, ByVal r As Rectangle, ByVal d As Integer, ByVal b As Brush)
        Dim mode As Drawing2D.SmoothingMode = g.SmoothingMode
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed
        g.FillPie(b, r.X, r.Y, d, d, 180, 90)
        g.FillPie(b, r.X + r.Width - d, r.Y, d, d, 270, 90)
        g.FillPie(b, r.X, r.Y + r.Height - d, d, d, 90, 90)
        g.FillPie(b, r.X + r.Width - d, r.Y + r.Height - d, d, d, 0, 90)
        g.FillRectangle(b, CInt(r.X + d / 2), r.Y, r.Width - d, CInt(d / 2))
        g.FillRectangle(b, r.X, CInt(r.Y + d / 2), r.Width, CInt(r.Height - d))
        g.FillRectangle(b, CInt(r.X + d / 2), CInt(r.Y + r.Height - d / 2), CInt(r.Width - d), CInt(d / 2))
        g.SmoothingMode = mode
    End Sub
    Public Sub DrawRoundedRectangle(ByVal g As Drawing.Graphics, ByVal r As Rectangle, ByVal d As Integer, ByVal p As Pen)
        g.DrawArc(p, r.X, r.Y, d, d, 180, 90)
        g.DrawLine(p, CInt(r.X + d / 2), r.Y, CInt(r.X + r.Width - d / 2), r.Y)
        g.DrawArc(p, r.X + r.Width - d, r.Y, d, d, 270, 90)
        g.DrawLine(p, r.X, CInt(r.Y + d / 2), r.X, CInt(r.Y + r.Height - d / 2))
        g.DrawLine(p, CInt(r.X + r.Width), CInt(r.Y + d / 2), CInt(r.X + r.Width), CInt(r.Y + r.Height - d / 2))
        g.DrawLine(p, CInt(r.X + d / 2), CInt(r.Y + r.Height), CInt(r.X + r.Width - d / 2), CInt(r.Y + r.Height))
        g.DrawArc(p, r.X, r.Y + r.Height - d, d, d, 90, 90)
        g.DrawArc(p, r.X + r.Width - d, r.Y + r.Height - d, d, d, 0, 90)
    End Sub

    Public Sub PaintTimer_Elasped()
        Try
            Me.Invalidate()
        Catch ex As Exception

        End Try
    End Sub



#End Region

End Class
