Imports Un4seen.Bass
Imports System.Collections.Specialized
Imports System.Collections.ObjectModel
Imports Un4seen.Bass.AddOn.Tags
Imports System.Configuration
Imports System.Collections.Generic
Imports System
Imports System.IO

Namespace AudioController

    <SettingsSerializeAs(System.Configuration.SettingsSerializeAs.Xml)> _
    Public Class TrackList
        Inherits SettingAttribute
        Public Sub New()
            Me.m_tracks = New ObservableCollectionExt(Of Track)()
        End Sub

        Protected m_tracks As ObservableCollectionExt(Of Track)

        Public ReadOnly Property Tracks() As ObservableCollectionExt(Of Track)
            Get
                Return m_tracks
            End Get
        End Property

    End Class


    Public Class ObservableCollectionExt(Of T)
        Inherits ObservableCollection(Of T)
        Private fireCollectionChanged As Boolean = True

        Public Sub New()

            MyBase.New()
        End Sub

        Public Sub AddRange(collection As IEnumerable(Of T))
            For Each i As Object In collection
                Items.Add(i)
            Next
            OnCollectionChanged(New NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset))
        End Sub

        Public Sub RemoveRange(collection As IEnumerable(Of T))
            For Each i As Object In collection
                Items.Remove(i)
            Next
            OnCollectionChanged(New NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset))
        End Sub

        '
        '         * if refreshCurrentIndex = false, clear collection without firing OnCollectionChanged,
        '         * in order to leave the current track playing
        '         * 

        Public Sub Clear(refreshCurrentIndex As Boolean)
            Me.fireCollectionChanged = refreshCurrentIndex
            MyBase.ClearItems()
            Me.fireCollectionChanged = True
        End Sub

        Protected Overrides Sub OnCollectionChanged(e As NotifyCollectionChangedEventArgs)
            If fireCollectionChanged Then
                MyBase.OnCollectionChanged(e)
            End If
        End Sub

        Protected Overrides Sub ClearItems()
            MyBase.ClearItems()
        End Sub
    End Class



    Public Class Track
        Implements ICloneable

        ' Provide an "Explicit Interface Method Implementation"
        Private Function ICloneable_Clone() As Object Implements ICloneable.Clone
            Return Me.Clone()
        End Function

        Public Function Clone() As Track
            Return DirectCast(Me.MemberwiseClone(), Track)
        End Function


        Public Shared Function GetTrack(path As String, Optional loadInfo As Boolean = True) As Track

            Dim track As New Track()

            track.Location = path

            If loadInfo Then
                LoadTrackInfo(track)
            End If

            Return track
        End Function

        Protected Sub New()
        End Sub



        Public Shared Sub LoadTrackInfo(track As Track)
            Dim stream As Integer = Bass.BASS_StreamCreateFile(track.Location, 0L, 0L, BASSFlag.BASS_STREAM_DECODE Or BASSFlag.BASS_SAMPLE_MONO)

            Try
                Dim isURL As Boolean = False


                If stream = 0 Then
                    stream = Bass.BASS_MusicLoad(track.Location, 0L, 0, BASSFlag.BASS_STREAM_DECODE Or BASSFlag.BASS_SAMPLE_MONO Or BASSFlag.BASS_MUSIC_PRESCAN, 0)
                End If
                If stream = 0 Then

                    If File.Exists(track.Location) Then
                        stream = Bass.BASS_StreamCreateURL(track.Location, 0, BASSFlag.BASS_STREAM_DECODE, Nothing, IntPtr.Zero)

                    End If


                        If stream <> 0 Then
                        isURL = True
                    End If
                End If



                Dim tagInfo As New TAG_INFO()

                Dim isTagAvailable As Boolean = False

                isTagAvailable = If(isURL, BassTags.BASS_TAG_GetFromURL(stream, tagInfo), BassTags.BASS_TAG_GetFromFile(stream, tagInfo))

                Dim length As Double = Bass.BASS_ChannelBytes2Seconds(stream, Bass.BASS_ChannelGetLength(stream))

                track.Artist = tagInfo.artist

                track.Album = tagInfo.album

                track.Title = tagInfo.title

                track.Length = length

                Bass.BASS_StreamFree(stream)
            Catch ex As Exception
                Bass.BASS_StreamFree(stream)
            End Try

        End Sub

        Public Property Location() As String
            Get
                Return m_Location
            End Get
            Set(value As String)
                m_Location = value
            End Set
        End Property
        Private m_Location As String

        Public Property Title() As String
            Get
                Return m_Title
            End Get
            Set(value As String)
                m_Title = value
            End Set
        End Property
        Private m_Title As String

        Public Property Artist() As String
            Get
                Return m_Artist
            End Get
            Set(value As String)
                m_Artist = value
            End Set
        End Property
        Private m_Artist As String

        Public Property Album() As String
            Get
                Return m_Album
            End Get
            Set(value As String)
                m_Album = value
            End Set
        End Property
        Private m_Album As String

        Public Property Length() As Double
            Get
                Return m_Length
            End Get
            Set(value As Double)
                m_Length = value
            End Set
        End Property
        Private m_Length As Double
    End Class




End Namespace