Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Drawing
Imports DevExpress.XtraEditors.ViewInfo

Namespace DXSample
    Public Class RichListBox
        Inherits ListBoxControl
        Public Sub New()
            MyBase.New()
        End Sub
        Protected Overrides Function CreateViewInfo() As BaseStyleControlViewInfo
            Return New MyBaseListBoxViewInfo(Me)
        End Function

        Private Sub InitializeComponent()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
    End Class

    Public Class MyListBoxItemPainter
        Inherits ListBoxItemPainter
        Public Sub New()
            MyBase.New()
        End Sub
        Protected Overrides Sub DrawItemBar(e As ListBoxItemObjectInfoArgs)
            e.PaintAppearance.FillRectangle(e.Cache, e.Bounds)
        End Sub
    End Class

    Public Class MyBaseListBoxViewInfo
        Inherits BaseListBoxViewInfo
        Public Sub New(owner As BaseListBoxControl)
            MyBase.New(owner)
        End Sub

        Protected Overrides Function CreateItemPainter() As BaseListBoxItemPainter
            If IsSkinnedHighlightingEnabled Then
                Return New MyListBoxSkinItemPainter()
            End If
            Return New MyListBoxItemPainter()
        End Function
    End Class

    Public Class MyListBoxSkinItemPainter
        Inherits ListBoxSkinItemPainter
        Public Sub New()
            MyBase.New()
        End Sub
        Protected Overrides Sub DrawItemBar(e As ListBoxItemObjectInfoArgs)
            DrawItemBarCore(e)
        End Sub
    End Class
End Namespace