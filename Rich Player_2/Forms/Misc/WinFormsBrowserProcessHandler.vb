
Imports System
Imports System.Threading.Tasks
Imports CefSharp.Example.Handlers
Imports System.Timers




Namespace CefSharp.WinForms.Example.Handlers
    Public Interface BrowserProcessHandler
        Inherits System.IDisposable

        Sub OnContextInitialized()


        Sub OnScheduleMessagePumpWork(ByVal delay As Long)

    End Interface

    Public Class WinFormsBrowserProcessHandler
        '  Inherits BrowserProcessHandler

        Private timer As Timer

        Private factory As TaskFactory

        Public Sub New(ByVal scheduler As TaskScheduler)
            factory = New TaskFactory(scheduler)
            timer = New Timer With {.Interval = 100, .AutoReset = True}
            timer.Start()
            AddHandler timer.Elapsed, AddressOf TimerTick
        End Sub

        Private Sub TimerTick(ByVal sender As Object, ByVal e As EventArgs)
            '  factory.StartNew(Function() cef.DoMessageLoopWork())
        End Sub

        Protected Sub OnScheduleMessagePumpWork(ByVal delay As Integer)
            If delay <= 0 Then
                ' factory.StartNew(Function() cef.DoMessageLoopWork())
            End If
        End Sub

        Public Sub Dispose()
            If timer IsNot Nothing Then
                timer.[Stop]()
                timer.Dispose()
                timer = Nothing
            End If
        End Sub
    End Class
End Namespace
