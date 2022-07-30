Imports Rich_Player.AudioController
Imports Rich_Player.CsWinFormsBlackApp.Form1
Imports Un4seen.Bass
Imports Rich_Player.CsWinFormsBlackApp


Namespace MediaButtons


    Public Class PlayPause
        Inherits PictureBox


        Public Sub New()
            Me.BackColor = System.Drawing.Color.Transparent
            Me.BackgroundImage = Form1.PlayImage
            Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
            
            Me.Location = New System.Drawing.Point(68, 3)
            Me.Size = New System.Drawing.Size(52, 52)
            Me.Anchor = AnchorStyles.None

        End Sub


        'Play
        Public Sub New_Play()
            Form1.Timer_Meta_and_Artwork.Start()
            Form1.CheckIfVideo()
            If UsingSpotify Then
                MySpotify.RefreshConnection()
                MySpotify.PlayPause()
            Else
                If IsVideo = False Then
                    VlcPlayer.Playlist.Stop()
                    'VlcPlayer.items.clear()
                    Dim currentAction As BASSActive = AudioPlayer.Instance.GetStreamStatus()
                    Select Case currentAction
                        Case BASSActive.BASS_ACTIVE_PLAYING     'Was Playing
                            AudioPlayer.Instance.Pause()
                            Exit Select
                        Case BASSActive.BASS_ACTIVE_PAUSED      'Was Paused
                            Form1.UpdatePitchByCurrentValue()
                            AudioPlayer.Instance.Play(False)
                            Form1.UpdatePitchByCurrentValue()
                            Form1.Bass_InitEQ()
                            Exit Select
                        Case Else                               'Was Stopped
                            If AudioPlayer.Instance.TrackList.Tracks.Count > 0 Then
                                If AudioPlayer.Instance.CurrentTrack Is Nothing Then
                                    AudioPlayer.Instance.CurrentTrackIndex = 0
                                End If
                                AudioPlayer.Instance.Play(True)
                                Form1.UpdatePitchByCurrentValue()
                                Form1.UpdateSpeedByCurrentValue()
                                AudioPlayer.Instance.Pause()
                                AudioPlayer.Instance.Play(False)
                                Form1.trackbar_Pitch2.Enabled = True
                                Form1.But_PitchDown.Enabled = True
                                Form1.But_PitchUp.Enabled = True
                                Form1.But_PitchDown.Visible = True
                                Form1.But_PitchUp.Visible = True
                                Form1.trackBar_Speed2.Enabled = True
                                SongStartOver = True
                                Form1.Bass_InitEQ()
                            End If
                            If Form1.InitiatePlayOnStart = False Then
                                Try
                                    AudioPlayer.Instance.Position = My.Settings.LastPlayedPositions.Item(Form1.PlaylistTabs.SelectedTabPageIndex)
                                Catch ex As Exception
                                End Try
                                Form1.InitiatePlayOnStart = True
                            End If
                            If Form1.InitiatePlayOnStartCurrent = False Then
                                Try
                                    AudioPlayer.Instance.Position = My.Settings.LastPlayedPositions.Item(Form1.PlaylistTabs.SelectedTabPageIndex)
                                Catch ex As Exception
                                End Try
                                Form1.InitiatePlayOnStartCurrent = True
                            End If
                            Exit Select
                    End Select
                Else
                    VlcPlayer.Playlist.Stop()
                    isVLCplaying = False
                    If Form1.InitiatePlayOnStart = True Then
                        PlaySub()
                    End If
                End If
            End If
        End Sub
        'Double Click to PLAY
        Public Sub playlistboxedit_MouseDoubleClick(sender As Object, e As MouseEventArgs)
            Form1.BarCheckBox_UseSpotifyold.Checked = False
            Form1.BarCheckBox_YoutubeUse.Checked = False
            DoubleClickPlay()
        End Sub
        Public Sub DoubleClickPlay()
            Form1.InitiatePlayOnStart = True
            Dim Playlist As GridPlaylist
            Dim Scroller As Scroller
            For Each c As Control In Form1.PlaylistTabs.SelectedTabPage.Controls
                If c.GetType Is GetType(Scroller) Then
                    Scroller = c
                    For Each c2 As Control In Scroller.Controls
                        If c2.GetType Is GetType(GridPlaylist) Then
                            Playlist = c2
                        End If
                    Next
                End If
            Next
            If Playlist.CurrentCell.RowIndex >= 0 AndAlso Playlist.CurrentCell.ColumnIndex >= 0 Then
                Dim row As Integer
                For i As Integer = 0 To Playlist.SelectedCells.Count - 1
                    row = Playlist.SelectedCells.Item(i).RowIndex
                Next
                Dim SongFilename As String = Playlist.Item(6, row).Value.ToString
                Form1.Timer_Seek.Stop()
                Form1.Timer_Meta_and_Artwork.Stop()
                AudioPlayer.Instance.ResetTrackList()
                Dim track As Track = AudioController.Track.GetTrack(SongFilename, True)
                AudioPlayer.Instance.TrackList.Tracks.Add(track)
                If AudioPlayer.Instance.TrackList.Tracks.Count > 0 Then
                    If AudioPlayer.Instance.CurrentTrack Is Nothing Then
                        AudioPlayer.Instance.CurrentTrackIndex = 0
                    End If
                    New_Play()
                    SongStartOver = True
                End If
                Form1.Timer_Meta_and_Artwork.Start() ' Set Artwork
                Form1.Timer_Seek.Start()
            End If
        End Sub
        Public Sub GridPlaylist_MouseDoubleClick(sender As Object, e As DataGridViewCellMouseEventArgs)
            Form1.InitiatePlayOnStart = True
            Dim Playlist As GridPlaylist
            Dim Scroller As Scroller
            For Each c As Control In Form1.PlaylistTabs.SelectedTabPage.Controls
                If c.GetType Is GetType(Scroller) Then
                    Scroller = c
                    For Each c2 As Control In Scroller.Controls
                        If c2.GetType Is GetType(GridPlaylist) Then
                            Playlist = c2
                        End If
                    Next
                End If
            Next
            If e.RowIndex >= 0 AndAlso e.ColumnIndex >= 0 Then
                Dim row As Integer
                For i As Integer = 0 To Playlist.SelectedCells.Count - 1
                    row = Playlist.SelectedCells.Item(i).RowIndex
                Next
                Dim SongFilename As String = Playlist.Item(6, row).Value.ToString
                Form1.Timer_Seek.Stop()
                Form1.Timer_Meta_and_Artwork.Stop()
                AudioPlayer.Instance.ResetTrackList()
                Dim track As Track = AudioController.Track.GetTrack(SongFilename, True)
                AudioPlayer.Instance.TrackList.Tracks.Add(track)
                If AudioPlayer.Instance.TrackList.Tracks.Count > 0 Then
                    If AudioPlayer.Instance.CurrentTrack Is Nothing Then
                        AudioPlayer.Instance.CurrentTrackIndex = 0
                    End If
                    New_Play()
                    SongStartOver = True
                End If
                Form1.Timer_Meta_and_Artwork.Start() ' Set Artwork
                Form1.Timer_Seek.Start()
            End If
        End Sub
        'VLC Play
        Public Sub PlaySub()

            Form1.Timer_Meta_and_Artwork.Start()
            AudioPlayer.Instance.Stop()
            AudioPlayer.Instance.ResetTrackList()
            If UsingSpotify Then
                MySpotify.RefreshConnection()
                MySpotify.PlayPause()
            Else
                If VlcPlayer.Playlist.isPlaying = True Then 'If VlcPlayer.Playlist.isPlaying = True Then
                    VlcPlayer.Playlist.TogglePause()
                    isVLCplaying = True
                    Try
                        Form1.SetTaskbarState(TaskbarState.Paused)
                    Catch
                    End Try
                    Me.BackgroundImage = PlayImage
                Else
                    If isVLCplaying Then
                        VlcPlayer.playlist.play()
                        isVLCplaying = True
                        Try
                            Form1.SetTaskbarState(TaskbarState.Playing)
                        Catch
                        End Try
                        Me.BackgroundImage = PauseImage
                    Else
                        isVLCplaying = False
                        VLC_Play()
                        VlcPlayer.playlist.play()
                        isVLCplaying = True
                        Try
                            Form1.SetTaskbarState(TaskbarState.Playing)
                        Catch
                        End Try
                        Me.BackgroundImage = PauseImage
                    End If
                    If Form1.InitiatePlayOnStart = False Then
                        VlcPlayer.input.position = My.Settings.LastPlayedPositions.Item(Form1.PlaylistTabs.SelectedTabPageIndex)
                        Form1.InitiatePlayOnStart = True
                    End If
                    If Form1.InitiatePlayOnStartCurrent = False Then
                        VlcPlayer.input.position = My.Settings.LastPlayedPositions.Item(Form1.PlaylistTabs.SelectedTabPageIndex)
                        Form1.InitiatePlayOnStartCurrent = True
                    End If
                End If
                ReApplySubtitles()
            End If
        End Sub
        Public Sub VLC_Play()
            Form1.BarCheckBox_UseSpotifyold.Checked = False
            Form1.BarCheckBox_YoutubeUse.Checked = False
            Dim Playlist As GridPlaylist
            Dim Scroller As Scroller
            For Each c As Control In Form1.PlaylistTabs.SelectedTabPage.Controls
                If c.GetType Is GetType(Scroller) Then
                    Scroller = c
                    For Each c2 As Control In Scroller.Controls
                        If c2.GetType Is GetType(GridPlaylist) Then
                            Playlist = c2
                            Form1.Timer_Seek.Stop()
                            Form1.Timer_Meta_and_Artwork.Stop()
                            'VlcPlayer.items.clear()
                            Dim row As Integer
                            For i As Integer = 0 To Playlist.SelectedCells.Count - 1
                                row = Playlist.SelectedCells.Item(i).RowIndex
                            Next
                            Dim SongFilename As String = Playlist.Item(6, row).Value.ToString
                            ' VlcPlayer.SetMedia(SongFilename)
                            VlcPlayer.playlist.add("file:///" & SongFilename)
                            VlcPlayer.playlist.play()
                            Form1.Timer_Seek.Start()
                            Form1.Timer_Meta_and_Artwork.Start() ' Set Artwork
                            Return
                        End If
                    Next
                End If
            Next
        End Sub
        'Youtube Play
        Public Sub PlayYoutube()
            If VlcPlayer.playlist.isPlaying = True Then 'If VlcPlayer.Playlist.isPlaying = True Then
                VlcPlayer.playlist.togglePause()
                isVLCplaying = True
                Try
                    Form1.SetTaskbarState(TaskbarState.Paused)
                Catch
                End Try
                Me.BackgroundImage = PlayImage
            Else
                If isVLCplaying Then
                    VlcPlayer.playlist.play()
                    isVLCplaying = True
                    Try
                        Form1.SetTaskbarState(TaskbarState.Playing)
                    Catch
                    End Try
                    Me.BackgroundImage = PauseImage
                Else
                    isVLCplaying = False
                    Form1.Timer_Seek.Stop()
                    Form1.Timer_Meta_and_Artwork.Stop()
                    VlcPlayer.playlist.stop()
                    'VlcPlayer.items.clear()

                    Try
                        Form1.SetTaskbarState(TaskbarState.Playing)
                    Catch
                    End Try
                    Me.BackgroundImage = PauseImage
                End If
                isVLCplaying = False
            End If
        End Sub
        Public Sub PlayALL()
            Form1.CheckIfVideo()
            If IsVideo = False Then
                If AudioPlayer.Instance.TrackList.Tracks.Count > 0 Then
                    New_Play()
                End If
            Else
                PlaySub()
            End If
        End Sub
        'Folder
        Dim firstplay As Boolean = True
        Public Sub PlayHotkeySub()
            If IsVideo = False Then
                If AudioPlayer.Instance.TrackList.Tracks.Count > 0 Then
                    New_Play()
                End If
            Else
                PlaySub()
            End If
        End Sub


    End Class


End Namespace
