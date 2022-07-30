Imports System.Windows.Forms
Imports SpotifyAPI.Web
Imports SpotifyAPI.Web.Auth
Imports SpotifyAPI.Web.Enums
Imports SpotifyAPI.Web.Models
Imports System.Threading.Tasks
Imports System.Net
Imports System.IO
Imports System.Net.Http
Imports System.Web.Http
Imports System.Threading
Imports System.ComponentModel



Public Class SpotifyWebClientUserControl

    Public Shared _Spotify As New SpotifyAPI.Web.SpotifyWebAPI
    Public Shared _Title As String
    Public Shared _Artist As String
    Public Shared _Album As String
    Public Shared LoadPlaylists As Boolean = False

    Dim start As Boolean = True
    Private Sub SpotifyWebClientForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If My.Settings.spotify_username <> "" Then
            tb_Username.Text = My.Settings.spotify_username
        End If
        If My.Settings.spotify_password <> "" Then
            tb_Password.Text = My.Settings.spotify_password
        End If

        start = False
    End Sub
    Private Sub tb_Username_TextChanged(sender As Object, e As EventArgs) Handles tb_Username.TextChanged, tb_Password.TextChanged
        If start Then Return
        My.Settings.spotify_username = tb_Username.Text
        My.Settings.spotify_password = tb_Password.Text
    End Sub


    Private Sub but_SignIn_Click(sender As Object, e As EventArgs) Handles but_SignIn.Click
        LoadPlaylists = True

     
        Task.Run(Function() RunAuthentication())
    End Sub

    Private Sub tb_Password_KeyUp(sender As Object, e As KeyEventArgs) Handles tb_Password.KeyUp
        If e.KeyCode = Keys.Enter Then
            Task.Run(Function() RunAuthentication())
        End If
    End Sub

    Private _profile As PrivateProfile
    Private _savedTracks As List(Of FullTrack)
    Private _playlists As List(Of SimplePlaylist)
    Private _playlistTracks As List(Of FullTrack)

    Public Sub New()
        InitializeComponent()


        _savedTracks = New List(Of FullTrack)()
    End Sub

    Private Async Function InitialSetup() As Task


        If InvokeRequired Then
            Invoke(New Action(AddressOf InitialSetup))
            Return
        End If
        label_Loading.Visible = True
        label_Loading.BringToFront()
        but_SignIn.Enabled = False

        Dim source As SpotifyAPI.Web.Models.SimpleArtist



        _profile = Await _Spotify.GetPrivateProfileAsync()

        _savedTracks = GetSavedTracks()
        ' savedTracksCountLabel.Text = _savedTracks.Count.ToString()

        _savedTracks.ForEach(Function(track) lv_songs.Items.Add(New ListViewItem _
            ({track.Name, track.Artists(0).Name, track.Album.Name})))


        _playlists = GetPlaylists()
        ' playlistsCountLabel.Text = _playlists.Count.ToString()
        _playlists.ForEach(Function(playlist) lb_Playlist.Items.Add(playlist.Name))

        timer_RefreshStrings.Start()


        label_Loading.Visible = False
        but_SignIn.Enabled = True
        LoadPlaylists = False
    End Function

    Private Function GetSavedTracks() As List(Of FullTrack)
        Dim savedTracks As Paging(Of SavedTrack) = _Spotify.GetSavedTracks
        Dim list As List(Of FullTrack) = savedTracks.Items.[Select](Function(track) track.Track).ToList()

        While savedTracks.[Next] IsNot Nothing
            savedTracks = _Spotify.GetSavedTracks(20, savedTracks.Offset + savedTracks.Limit)
            list.AddRange(savedTracks.Items.[Select](Function(track) track.Track))
        End While

        Return list
    End Function

    Private Function GetPlaylists() As List(Of SimplePlaylist)
        Dim playlists As Paging(Of SimplePlaylist) = _Spotify.GetUserPlaylists(_profile.Id)
        Dim list As List(Of SimplePlaylist) = playlists.Items.ToList()

        While playlists.[Next] IsNot Nothing
            playlists = _Spotify.GetUserPlaylists(_profile.Id, 20, playlists.Offset + playlists.Limit)
            list.AddRange(playlists.Items)
        End While

        Return list
    End Function

    Private Function GetPlaylistTracks() As List(Of FullTrack)
        Dim playlistTracks As Paging(Of PlaylistTrack) = _Spotify.GetPlaylistTracks(_profile.Id, _Spotify.GetUserPlaylists(_profile.Id, 50, lb_Playlist.SelectedIndex).Items(lb_Playlist.SelectedIndex).Id, "", 100, 0)
        ' Dim list As List(Of FullTrack) = _Spotify.GetUserPlaylists(_profile.Id, 20, playlists.Offset + playlists.Limit).Items
        Dim list As List(Of FullTrack) = playlistTracks.Items.[Select](Function(track) track.Track).ToList()



        While playlistTracks.[Next] IsNot Nothing
            ' playlistTracks() '= _Spotify.GetPlaylistTracks(_profile.Id, lb_Playlist.SelectedItem)
            list.AddRange(playlistTracks.Items.[Select](Function(track) track.Track))
        End While

        Return list
    End Function



    Dim Sbgw As New BackgroundWorker
    Public Sub Sbgw_Dowork(sender As Object, e As DoWorkEventArgs)
        InitialSetup()
    End Sub
    Private Async Function RunAuthentication() As Task

        Dim _clientId As String = "79f9d83ae2be40f6af9a10fea21355b4"
        Dim _secretId As String = "94c4dbe622c240faa355139a2bfde8ce"
        Dim _URL As String = "http://localhost:8000"
        Dim _scopes As SpotifyAPI.Web.Enums.Scope = Scope.UserReadPrivate Or Scope.UserModifyPlaybackState Or Scope.Streaming Or Scope.UserReadPlaybackState
        Dim _state As String = "spotify_auth_state"


        Dim Auth As New SpotifyAPI.Web.Auth.AutorizationCodeAuth()
        Auth.ClientId = _clientId
        Auth.RedirectUri = _URL
        Auth.Scope = _scopes
        Auth.State = _state

        Dim ifWorks As Boolean = False
        If ifWorks Then
            AddHandler Auth.OnResponseReceivedEvent, Function(payload)
                                                         Auth.StopHttpServer()
                                                         Dim token As Token = Auth.ExchangeAuthCode(payload.Code, _secretId)
                                                         Dim api As SpotifyWebAPI = New SpotifyWebAPI() With {
                                                             .TokenType = token.TokenType,
                                                             .AccessToken = token.AccessToken
                                                         }
                                                         _Spotify = api
                                                     End Function

            Auth.StartHttpServer()
            ' auth.OpenBrowser()

        Else
            Dim webApiFactory As New WebAPIFactory("http://localhost", 8000, _clientId, _scopes) '_

            Try
                _Spotify = Await webApiFactory.GetWebApi()
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try

            If _Spotify Is Nothing Then
                Return
            End If

            AddHandler Sbgw.DoWork, AddressOf Sbgw_Dowork
            If Not Sbgw.IsBusy Then
                If LoadPlaylists Then Sbgw.RunWorkerAsync()

            End If

        End If






    End Function



    Private Sub lb_Playlist_DoubleClick(sender As Object, e As EventArgs) Handles lb_Playlist.DoubleClick
        lv_PlaylistSongs.Items.Clear()
        Try
            _playlistTracks = GetPlaylistTracks()
            _playlistTracks.ForEach(Function(track) lv_PlaylistSongs.Items.Add(New ListViewItem _
              ({track.Name, track.Artists(0).Name, track.Album.Name})))
        Catch ex As Exception
            MsgBox("Error")
        End Try



    End Sub



    Private Sub timer_RefreshStrings_Tick(sender As Object, e As EventArgs) Handles timer_RefreshStrings.Tick

        _Title = _Spotify.GetPlayback.Item.Name
        _Artist = _Spotify.GetPlayback.Item.Artists(0).Name
        _Album = _Spotify.GetPlayback.Item.Album.Name


    End Sub

End Class

