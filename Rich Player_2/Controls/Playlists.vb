Imports System.Data
Imports System.ComponentModel
Imports Rich_Player.CsWinFormsBlackApp
Imports System.Drawing.Drawing2D
'Imports Windows
Imports System.Drawing

Public Class Scroller
    Inherits DevExpress.XtraEditors.XtraScrollableControl
    Dim Timer1 As New Timer

    Public Sub New()

        Me.AutoSize = False
        Me.Dock = DockStyle.Fill
        Me.AllowTouchScroll = True
        Me.AutoScroll = True
        Me.FireScrollEventOnMouseWheel = True
        Me.EnableScrollOnMouseWheel()
        Me.InvertTouchScroll = True
        Me.AlwaysScrollActiveControlIntoView = False
        Me.ScrollBarSmallChange = 20



    End Sub



    Public Sub EnableScrollOnMouseWheel()
        AddHandler Me.VisibleChanged, AddressOf OnVisibleChanged
    End Sub

    Private Sub OnVisibleChanged(ByVal sender As Object, ByVal e As EventArgs)
        Me.Select()
        UnsubscribeFromMouseWheel(Me.Controls)
        SubscribeToMouseWheel(Me.Controls)
    End Sub

    Private Sub SubscribeToMouseWheel(ByVal controls As Control.ControlCollection)
        For Each ctrl As Control In controls
            AddHandler ctrl.MouseWheel, AddressOf OnMouseWheel
            SubscribeToMouseWheel(ctrl.Controls)
        Next ctrl
    End Sub

    Private Sub UnsubscribeFromMouseWheel(ByVal controls As Control.ControlCollection)
        For Each ctrl As Control In controls
            RemoveHandler ctrl.MouseWheel, AddressOf OnMouseWheel
            UnsubscribeFromMouseWheel(ctrl.Controls)
        Next ctrl
    End Sub

    Private Sub OnMouseWheel(ByVal sender As Object, ByVal e As MouseEventArgs)
        Timer1.Stop()
        DevExpress.Utils.DXMouseEventArgs.GetMouseArgs(e).Handled = True
        Dim scrollValue As Integer = Me.VerticalScroll.Value
        Dim largeChange As Integer = Me.VerticalScroll.LargeChange - 150
        If e.Delta < 0 Then
            Me.VerticalScroll.Value += largeChange 'Me.VerticalScroll.LargeChange
        Else
            If scrollValue < largeChange Then
                Me.VerticalScroll.Value = 0
            Else
                Me.VerticalScroll.Value -= largeChange
            End If
        End If


    End Sub

    Public Sub DisableScrollOnMouseWheel()
        RemoveHandler VisibleChanged, AddressOf OnVisibleChanged
        UnsubscribeFromMouseWheel(Me.Controls)
    End Sub



End Class

Public Class GridPlaylist
    Inherits DataGridView
   

    Public Sub New()
        Dim CellStyle2 As New DataGridViewCellStyle
        Me.AutoSize = True

        Me.ScrollBars = Forms.ScrollBars.None
        CellStyle2.BackColor = Color.FromArgb(110, 110, 110)


        Me.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(90, 90, 90)
        Me.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        Me.ColumnHeadersDefaultCellStyle.ForeColor = Color.Gainsboro
        Me.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(60, 63, 66)
        Me.ColumnHeadersDefaultCellStyle.SelectionForeColor = SystemColors.HighlightText
        Me.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
        Me.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9.75, FontStyle.Bold)



        'Initialize
        Me.AllowDrop = True
        Me.AllowUserToDeleteRows = False
        Me.AllowUserToResizeColumns = True
        Me.AllowUserToResizeRows = False
        Me.AllowUserToAddRows = False
        Me.BorderStyle = Forms.BorderStyle.None
        Me.ReadOnly = True


        Me.EnableHeadersVisualStyles = False
        Me.GridColor = SystemColors.ControlDarkDark
        Me.Location = New System.Drawing.Point(0, 0)
        '  Me.Dock = DockStyle.Fill
        '       Me.Size = New System.Drawing.Size(516, 280)
        Me.MultiSelect = False

        'Selection Color
        Me.DefaultCellStyle.SelectionBackColor = Color.FromArgb(60, 63, 66)
        Me.DefaultCellStyle.SelectionForeColor = SystemColors.HighlightText

        Me.DefaultCellStyle.WrapMode = DataGridViewTriState.True

        'Fonts
        Me.DefaultCellStyle.Font = New System.Drawing.Font("Segoe UI", 9.75, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DefaultCellStyle.ForeColor = Color.Gainsboro
        Me.RowsDefaultCellStyle.Font = New System.Drawing.Font("Segoe UI", 9.75, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))



        'Cells
        Me.DefaultCellStyle.BackColor = Color.Gray
        Me.AlternatingRowsDefaultCellStyle = CellStyle2
        Me.BackgroundColor = System.Drawing.SystemColors.ControlDarkDark
        BackgroundGradient(Me, Color.FromArgb(105, 105, 105), Color.FromArgb(114, 109, 123))
        Me.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft

        'Columns
        Dim TitleCol As New DataGridViewTextBoxColumn
        Dim ArtistCol As New DataGridViewTextBoxColumn
        Dim AlbumCol As New DataGridViewTextBoxColumn
        Dim DurationCol As New DataGridViewTextBoxColumn
        Dim FavoriteCol As New DataGridViewTextBoxColumn
        Dim PositionCol As New DataGridViewTextBoxColumn
        Dim LocationCol As New DataGridViewTextBoxColumn
        TitleCol.HeaderText = "Title"
        ArtistCol.HeaderText = "Artist"
        AlbumCol.HeaderText = "Album"
        DurationCol.HeaderText = "Duration"
        FavoriteCol.HeaderText = "Favorite"
        PositionCol.HeaderText = "Position"
        LocationCol.HeaderText = "Location"

        FavoriteCol.Width = 0
        PositionCol.Width = 0
        LocationCol.Width = 0


        Me.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        Me.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single


        Me.Columns.AddRange(New DataGridViewColumn() {TitleCol, ArtistCol, AlbumCol, DurationCol, FavoriteCol, PositionCol, LocationCol})
        Me.ColumnHeadersHeight = 25
        For Each col As DataGridViewColumn In Me.Columns
            col.SortMode = DataGridViewColumnSortMode.Programmatic
        Next



        'Rows
        Me.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single
        Me.RowHeadersVisible = False
        Me.RowHeadersDefaultCellStyle = Me.DefaultCellStyle
        Me.RowHeadersWidth = 18
        Me.RowTemplate.MinimumHeight = 10
        Me.RowTemplate.Height = 22
        Me.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.RowTemplate.Resizable = DataGridViewTriState.False

        Me.SelectionMode = DataGridViewSelectionMode.FullRowSelect



    End Sub
    Private fromIndex As Integer
    Private dragIndex As Integer
    Private dragRect As Rectangle



    Private Sub GridPlaylist_ColumnHeaderMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles Me.ColumnHeaderMouseClick
        Dim msg As String = "Are you sure you want to sort playlist by: " & Me.Columns(e.ColumnIndex).HeaderText.ToString & "?" _
                            & Environment.NewLine & " This cannot be undone!"
        Dim result As Integer = MyFullMsgBox.Show(msg, "Are you sure?", True, MyFullMsgBox.CustomButtons.YesNo)
        If result = DialogResult.Yes Then

            Select Case Me.SortOrder
                Case Forms.SortOrder.Ascending
                    Me.Sort(Me.Columns(e.ColumnIndex), ListSortDirection.Descending)
                Case Forms.SortOrder.Descending
                    Me.Sort(Me.Columns(e.ColumnIndex), ListSortDirection.Ascending)
                Case Else
                    Me.Sort(Me.Columns(e.ColumnIndex), ListSortDirection.Ascending)
            End Select
        End If
    End Sub


    Private Sub SkillGrid_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Me.DragDrop
        Dim p As Point = Me.PointToClient(New Point(e.X, e.Y))
        Dim newRow As DataGridViewRow = Me.Rows(Me.HitTest(p.X, p.Y).RowIndex) 'gets new row info
        If newRow.Index >= 0 Then 'And newRow.Index < Me.Rows.Count Then
            Dim oldRow As DataGridViewRow = DirectCast(e.Data.GetData(GetType(DataGridViewRow)), DataGridViewRow)
            'gets dropped row info


            If newRow.Index <> oldRow.Index Then 'if the two rows aren't the same it inserts dropped row
                Me.Rows.RemoveAt(oldRow.Index)
                Me.Rows.Insert(newRow.Index, oldRow)
                Me.Refresh()
                Me.Rows(oldRow.Index).Selected = True
                Me.Refresh()
            End If
        End If
        Me.Refresh()
    End Sub

    Private Sub me_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Me.DragEnter
        e.Effect = System.Windows.Forms.DragDropEffects.Move
    End Sub
    Public doubleclk As Boolean = False
    Private Sub GridPlaylist_CellMouseDoubleClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles Me.CellMouseDoubleClick
        doubleclk = True
    End Sub
    Private Sub me_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles me.MouseDown
        Try
            Me.Rows(Me.CurrentCell.RowIndex).DefaultCellStyle.BackColor = CurColor
        Catch ex As Exception

        End Try
        Return
        If doubleclk Then
            doubleclk = False
        Else
            If e.Button = MouseButtons.Left Then
                Dim info As DataGridView.HitTestInfo = Me.HitTest(e.X, e.Y)
                If info.RowIndex >= 0 Then
                    Dim view As DataGridViewRow = DirectCast(Me.Rows(info.RowIndex), DataGridViewRow)
                    If view IsNot Nothing Then

                        Me.DoDragDrop(view, System.Windows.Forms.DragDropEffects.Move)
                    End If
                End If
            End If
            Me.Refresh()
            Me.Rows(Me.CurrentCell.RowIndex).DefaultCellStyle.BackColor = CurColor
        End If

    End Sub




    Private Sub BackgroundGradient(ByRef Control As Object, ByVal Color1 As Drawing.Color, ByVal Color2 As Drawing.Color)

        Dim vLinearGradient As Drawing.Drawing2D.LinearGradientBrush = _
            New Drawing.Drawing2D.LinearGradientBrush(New Drawing.Point(Control.Width, Control.Height), _
                                                        New Drawing.Point(Control.Width, 0), Color1, Color2)

        Dim vGraphic As Drawing.Graphics = Control.CreateGraphics
        ' To tile the image background - Using the same image background of the image
        '  Dim vTexture As New Drawing.TextureBrush(Control.BackgroundImage)

        vGraphic.FillRectangle(vLinearGradient, Control.DisplayRectangle)
        ' vGraphic.FillRectangle(vTexture, Control.DisplayRectangle)

        vGraphic.Dispose() : vGraphic = Nothing ' : vTexture.Dispose() : vTexture = Nothing
    End Sub

    Dim CurColor As Color
    Dim CurSelectionColor As Color
    Private Sub DataGridView1_CellMouseEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Me.CellMouseEnter

        Try
            If e.RowIndex < 0 Then Return
            If Me.Rows(e.RowIndex).Selected Then
                ' CurSelectionColor = Me.Rows(e.RowIndex).DefaultCellStyle.SelectionBackColor
                '  Me.Rows(e.RowIndex).DefaultCellStyle.SelectionBackColor = Color.FromArgb(145, 145, 145)
            Else
                CurColor = Me.Rows(e.RowIndex).DefaultCellStyle.BackColor
                Me.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.FromArgb(145, 145, 145)
            End If



        Catch ex As Exception
        End Try


    End Sub

    Private Sub DataGridView1_CellMouseLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Me.CellMouseLeave
        If e.RowIndex < 0 Then Return
        If Me.Rows(e.RowIndex).Selected Then
            '         Me.Rows(e.RowIndex).DefaultCellStyle.SelectionBackColor = CurSelectionColor
        Else
            Me.Rows(e.RowIndex).DefaultCellStyle.BackColor = CurColor
        End If

    End Sub





End Class



Public Class SpotifyPlaylist
    Inherits Rich_Player.DXSample.RichListBox
    Public Sub New()
        Me.ForeColor = Color.Gainsboro
        Me.BackColor = Color.Gray
        Me.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.Font = New Font("Segoe UI", 9.75, FontStyle.Regular)



        Me.ItemHeight = 18

    End Sub

    Private Property SizeFromClientSize As Drawing.Size

    Private Sub InitializeComponent()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
End Class

Public Class SpotifyGridPlaylist
    Inherits DataGridView

    Public Sub New()
        Dim CellStyle2 As New DataGridViewCellStyle
        Me.AutoSize = False
        CellStyle2.BackColor = Color.FromArgb(110, 110, 110)


        Me.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(90, 90, 90)
        Me.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        Me.ColumnHeadersDefaultCellStyle.ForeColor = Color.Gainsboro
        Me.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(60, 63, 66)
        Me.ColumnHeadersDefaultCellStyle.SelectionForeColor = SystemColors.HighlightText
        Me.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
        Me.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9.75, FontStyle.Bold)



        'Initialize
        Me.AllowDrop = True
        Me.AllowUserToDeleteRows = False
        Me.AllowUserToResizeColumns = True
        Me.AllowUserToResizeRows = False
        Me.AllowUserToAddRows = False
        Me.BorderStyle = Forms.BorderStyle.None
        Me.ReadOnly = True

        Me.EnableHeadersVisualStyles = False
        Me.GridColor = SystemColors.ControlDarkDark
        Me.Location = New System.Drawing.Point(0, 0)
        Me.Dock = DockStyle.Fill
        '       Me.Size = New System.Drawing.Size(516, 280)
        Me.MultiSelect = False

        'Selection Color
        Me.DefaultCellStyle.SelectionBackColor = Color.FromArgb(60, 63, 66)
        Me.DefaultCellStyle.SelectionForeColor = SystemColors.HighlightText

        Me.DefaultCellStyle.WrapMode = DataGridViewTriState.True

        'Fonts
        Me.DefaultCellStyle.Font = New System.Drawing.Font("Segoe UI", 9.75, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DefaultCellStyle.ForeColor = Color.Gainsboro
        Me.RowsDefaultCellStyle.Font = New System.Drawing.Font("Segoe UI", 9.75, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))



        'Cells
        Me.DefaultCellStyle.BackColor = Color.Gray
        Me.AlternatingRowsDefaultCellStyle = CellStyle2
        Me.BackgroundColor = System.Drawing.SystemColors.ControlDarkDark
        Me.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft

        'Columns
        Dim TitleCol As New DataGridViewTextBoxColumn
        Dim ArtistCol As New DataGridViewTextBoxColumn
        Dim AlbumCol As New DataGridViewTextBoxColumn
        Dim DurationCol As New DataGridViewTextBoxColumn
        Dim FavoriteCol As New DataGridViewTextBoxColumn
        Dim PositionCol As New DataGridViewTextBoxColumn
        Dim LocationCol As New DataGridViewTextBoxColumn
        TitleCol.HeaderText = "Title"
        ArtistCol.HeaderText = "Artist"
        AlbumCol.HeaderText = "Album"
        DurationCol.HeaderText = "Duration"
        FavoriteCol.HeaderText = "Favorite"
        PositionCol.HeaderText = "Position"
        LocationCol.HeaderText = "Location"







        Me.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        Me.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single


        Me.Columns.AddRange(New DataGridViewColumn() {TitleCol, LocationCol})
        Me.ColumnHeadersHeight = 25




        'Rows
        Me.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single
        Me.RowHeadersVisible = False
        Me.RowHeadersDefaultCellStyle = Me.DefaultCellStyle
        Me.RowHeadersWidth = 18
        Me.RowTemplate.MinimumHeight = 10
        Me.RowTemplate.Height = 22
        Me.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.RowTemplate.Resizable = DataGridViewTriState.False

        Me.SelectionMode = DataGridViewSelectionMode.FullRowSelect


    End Sub


End Class


Public Class YouTubeGridPlaylist
    Inherits DataGridView

    Public Sub New()
        Me.AutoSize = False
        Dim CellStyle2 As New DataGridViewCellStyle
        CellStyle2.BackColor = Color.FromArgb(110, 110, 110)


        Me.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(90, 90, 90)
        Me.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        Me.ColumnHeadersDefaultCellStyle.ForeColor = Color.Gainsboro
        Me.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(60, 63, 66)
        Me.ColumnHeadersDefaultCellStyle.SelectionForeColor = SystemColors.HighlightText
        Me.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
        Me.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9.75, FontStyle.Bold)



        'Initialize
        Me.AllowDrop = True
        Me.AllowUserToDeleteRows = False
        Me.AllowUserToResizeColumns = True
        Me.AllowUserToResizeRows = False
        Me.AllowUserToAddRows = False
        Me.BorderStyle = Forms.BorderStyle.None
        Me.ReadOnly = True
        Me.EnableHeadersVisualStyles = False
        Me.GridColor = SystemColors.ControlDarkDark
        Me.Location = New System.Drawing.Point(0, 0)
        Me.Dock = DockStyle.Fill
        '       Me.Size = New System.Drawing.Size(516, 280)
        Me.MultiSelect = False

        'Selection Color
        Me.DefaultCellStyle.SelectionBackColor = Color.FromArgb(60, 63, 66)
        Me.DefaultCellStyle.SelectionForeColor = SystemColors.HighlightText

        Me.DefaultCellStyle.WrapMode = DataGridViewTriState.True

        'Fonts
        Me.DefaultCellStyle.Font = New System.Drawing.Font("Segoe UI", 9.75, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DefaultCellStyle.ForeColor = Color.Gainsboro
        Me.RowsDefaultCellStyle.Font = New System.Drawing.Font("Segoe UI", 9.75, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))



        'Cells
        Me.DefaultCellStyle.BackColor = Color.Gray
        Me.AlternatingRowsDefaultCellStyle = CellStyle2
        Me.BackgroundColor = System.Drawing.SystemColors.ControlDarkDark
        Me.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft

        'Columns
        Dim TitleCol As New DataGridViewTextBoxColumn
        Dim ThumbnailCol As New DataGridViewImageColumn
        Dim DescripCol As New DataGridViewTextBoxColumn
        Dim AlbumCol As New DataGridViewTextBoxColumn
        Dim DurationCol As New DataGridViewTextBoxColumn
        Dim FavoriteCol As New DataGridViewTextBoxColumn
        Dim PositionCol As New DataGridViewTextBoxColumn
        Dim IDcol As New DataGridViewTextBoxColumn
        TitleCol.HeaderText = "Title"
        ThumbnailCol.HeaderText = "Thumbnail"
        AlbumCol.HeaderText = "Album"
        DescripCol.HeaderText = "Description"
        IDcol.HeaderText = "Location"
        DurationCol.HeaderText = "Duration"
        FavoriteCol.HeaderText = "Favorite"
        PositionCol.HeaderText = "Position"







        Me.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        Me.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single


        Me.Columns.AddRange(New DataGridViewColumn() {ThumbnailCol, TitleCol, DescripCol, IDcol, FavoriteCol, PositionCol})
        Me.ColumnHeadersHeight = 25




        'Rows
        Me.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single
        Me.RowHeadersDefaultCellStyle = Me.DefaultCellStyle
        'Me.RowHeadersWidth = 18
        Me.RowHeadersVisible = False
        Me.RowTemplate.Height = 32
        Me.RowTemplate.Resizable = DataGridViewTriState.True
        'Me.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.SelectionMode = DataGridViewSelectionMode.FullRowSelect


    End Sub


End Class
