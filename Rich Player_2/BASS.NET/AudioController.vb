
Imports System.Collections.Generic
Imports Un4seen.Bass
Imports Un4seen.Bass.AddOn.Fx
Imports Un4seen.Bass.AddOn.Tags
Imports System.IO
Imports System.Runtime.InteropServices

Namespace AudioController

    Public Class AudioPlayer
        Public Delegate Sub delAudioHandleLoaded()

        Public Event AudioHandleLoaded As delAudioHandleLoaded

        Public Sub OnAudioHandleLoaded(sender As Object, e As EventArgs)
            RaiseEvent AudioHandleLoaded()
        End Sub

        Public Delegate Sub delTrackChanged()

        Public Event TrackChanged As delTrackChanged

        Public Sub OnTrackChanged(sender As Object, e As EventArgs)
            RaiseEvent TrackChanged()
        End Sub

        Private mySyncProcEnd As SYNCPROC

        Private syncMetaUpdated As SYNCPROC

        Private mySyncHandleEnd As Integer

        Private Shared m_instance As AudioPlayer

        Private device As Integer

        Public Property EnableTempo() As Boolean
            Get
                Return m_EnableTempo
            End Get
            Set(value As Boolean)
                m_EnableTempo = Value
            End Set
        End Property
        Private m_EnableTempo As Boolean

        Public Function GetDevice() As Integer
            Return Bass.BASS_GetDevice()
        End Function

        Public Function SetDevice(device As Integer) As Boolean
            Me.device = device
            Return True
        End Function

        Public Shared ReadOnly Property Instance() As AudioPlayer
            Get
                If m_instance Is Nothing Then
                    m_instance = New AudioPlayer()

                    m_instance.EnableTempo = True


                    m_instance.m_currentAudioHandle = New AudioHandle()
                End If

                Return m_instance
            End Get
        End Property

        Private Shared Sub Tracks_CollectionChanged(sender As Object, e As System.Collections.Specialized.NotifyCollectionChangedEventArgs)
            m_instance.CurrentTrackIndex = m_instance.TrackList.Tracks.IndexOf(m_instance.CurrentTrack)
        End Sub

        Private m_currentAudioHandle As AudioHandle

        Public ReadOnly Property CurrentAudioHandle() As AudioHandle
            Get
                If m_currentAudioHandle Is Nothing Then
                    m_currentAudioHandle = New AudioHandle()
                End If

                Return m_currentAudioHandle
            End Get
        End Property

        Private m_tracklist As New TrackList()

        Public Property TrackList() As TrackList
            Get
                Return m_tracklist
            End Get
            Set(value As TrackList)
                m_tracklist = value
                AddHandler m_instance.TrackList.Tracks.CollectionChanged, AddressOf Tracks_CollectionChanged

            End Set
        End Property

        Public Property CurrentTrack() As Track
            Get
                Return m_CurrentTrack
            End Get
            Private Set(value As Track)
                m_CurrentTrack = value
            End Set
        End Property
        Private m_CurrentTrack As Track

        Private m_currentTrackIndex As Integer

        Public Property CurrentTrackIndex() As Integer
            Get
                Return m_currentTrackIndex
            End Get
            Set(value As Integer)
                m_currentTrackIndex = value

                Dim lastTrack As Track = CurrentTrack

                If m_currentTrackIndex >= 0 AndAlso m_currentTrackIndex < m_instance.tracklist.Tracks.Count Then
                    CurrentTrack = m_instance.tracklist.Tracks(m_currentTrackIndex)
                Else
                    CurrentTrack = Nothing
                End If

                ' if track index changes, the track currently playing stops
                If CurrentTrack Is Nothing OrElse lastTrack IsNot Nothing AndAlso lastTrack.Equals(CurrentTrack) = False Then
                    If m_instance.GetStreamStatus() <> BASSActive.BASS_ACTIVE_STOPPED Then
                        m_instance.[Stop]()
                    End If

                    m_currentAudioHandle.FreeResources()
                End If

                ' fire event
                OnTrackChanged(Me, Nothing)
            End Set
        End Property

        Public Sub SyncProcEndCallback(handle As Integer, channel As Integer, data As Integer, user As IntPtr)
            Bass.BASS_ChannelRemoveSync(AudioPlayer.Instance.CurrentAudioHandle.CurrentHandle, mySyncHandleEnd)

            OnTrackEnded()
        End Sub

        Public Sub ResetTrackList()
            Me.CurrentTrackIndex = 0

            Me.TrackList.Tracks.Clear(True)
        End Sub

        Public Sub Forward([loop] As Boolean)
            If TrackList.Tracks.Count > 0 Then
                Dim currentTrackIndex As Integer = m_instance.CurrentTrackIndex

                If currentTrackIndex < (TrackList.Tracks.Count - 1) Then
                    m_instance.CurrentTrackIndex += 1
                ElseIf [loop] Then
                    m_instance.CurrentTrackIndex = 0
                End If
            End If
        End Sub

        Public Sub Backward([loop] As Boolean)
            If TrackList.Tracks.Count > 0 Then
                Dim currentTrackIndex__1 As Integer = m_instance.CurrentTrackIndex

                If currentTrackIndex__1 > 0 Then
                    CurrentTrackIndex -= 1
                ElseIf [loop] Then
                    CurrentTrackIndex = TrackList.Tracks.Count - 1


                End If
            End If
        End Sub



        Private Function OpenM3UFile(filename As String) As Boolean
            Try
                ' Create an instance of StreamReader to read from a file.
                ' The using statement also closes the StreamReader.
                Using sr As New StreamReader(filename)
                    Dim line As String
                    ' Read and display lines from the file until the end of
                    ' the file is reached.
                    While (InlineAssignHelper(line, sr.ReadLine())) IsNot Nothing
                        If line.Substring(0, 1).Equals("#") = False Then
                            Dim track__1 As Track = Track.GetTrack(line, False)

                            Me.TrackList.Tracks.Add(track__1)
                        End If
                    End While
                End Using
            Catch e As Exception
                ' Let the user know what went wrong.
                Console.WriteLine("The file could not be read:")
                Console.WriteLine(e.Message)
            End Try

            Return True
        End Function

        Private Shared Sub SaveM3UFile(filename As String)
            Const header As String = "#EXTM3U"
            Const extraInfo As String = "#EXTINF"

            Using file As New System.IO.StreamWriter(filename, True)
                file.WriteLine(header)

                For Each track As Track In AudioPlayer.Instance.TrackList.Tracks
                    Dim seconds As Integer = CInt(Math.Round(track.Length))
                    Dim artist As String = track.Artist
                    Dim title As String = track.Title
                    Dim location As String = track.Location

                    Dim format As String = "{0}:{1},{2} - {3}"

                    file.WriteLine([String].Format(format, extraInfo, seconds.ToString(), artist, title))

                    file.WriteLine(location)
                Next
            End Using


        End Sub


        Public Delegate Sub delMetaUpdated()

        Public Event MetaUpdated As delMetaUpdated

        Public Sub OnMetaUpdated(sender As Object, e As EventArgs)
            RaiseEvent MetaUpdated()
        End Sub

        Private Sub MetaSync(handle As Integer, channel As Integer, data As Integer, user As IntPtr)
            Dim tagInfo As TAG_INFO = AudioPlayer.instance.currentAudioHandle.TagInfo

            ' BASS_SYNC_META is triggered on meta changes of SHOUTcast streams
            Dim isUpdated As Boolean = tagInfo.UpdateFromMETA(Bass.BASS_ChannelGetTags(channel, BASSTag.BASS_TAG_META), True, tagInfo.tagType = BASSTag.BASS_TAG_META)

            OnMetaUpdated(Me, Nothing)
        End Sub

        Public Function LoadAudio() As Boolean
            'prepareNextTrack = false;

            Dim isStreamOk As Boolean = False

            Try
                If CurrentTrack Is Nothing Then
                    Return False
                End If

                If GetDevice() <> device Then
                    Bass.BASS_SetDevice(device)
                End If

                m_currentAudioHandle.FreeResources()


                Dim flagForHStream As BASSFlag = BASSFlag.BASS_SAMPLE_FLOAT
                Dim flagForHMusic As BASSFlag = BASSFlag.BASS_MUSIC_FLOAT Or BASSFlag.BASS_MUSIC_PRESCAN Or BASSFlag.BASS_MUSIC_POSRESETEX
                Dim flagForHStreamURL As BASSFlag = BASSFlag.BASS_DEFAULT

                If EnableTempo Then
                    flagForHStream = flagForHStream Or BASSFlag.BASS_STREAM_DECODE
                    flagForHMusic = flagForHMusic Or BASSFlag.BASS_STREAM_DECODE
                End If

                m_currentAudioHandle.CreateHandle(CurrentTrack.Location, flagForHStream, flagForHMusic, flagForHStreamURL)

                If EnableTempo = True Then

                    m_currentAudioHandle.CreateTempoHandle(m_currentAudioHandle.Handle, BASSFlag.BASS_DEFAULT)
                End If

                If m_currentAudioHandle.Handle = 0 Then
                    Return False
                End If

                Dim isModule As Boolean = m_currentAudioHandle.IsModule()

                If m_currentAudioHandle.CurrentHandle = 0 Then
                    Return False
                End If
                Dim streamFX As Integer = m_currentAudioHandle.CurrentHandle
                Bass.BASS_ChannelSetAttribute(streamFX, BASSAttribute.BASS_ATTRIB_TEMPO_OPTION_PREVENT_CLICK, 1)
                Select Case My.Settings.AudioOptimization
                    Case 0
                        Bass.BASS_ChannelSetAttribute(streamFX, BASSAttribute.BASS_ATTRIB_TEMPO_OPTION_SEQUENCE_MS, 82)
                    Case 1
                        Bass.BASS_ChannelSetAttribute(streamFX, BASSAttribute.BASS_ATTRIB_TEMPO_OPTION_SEQUENCE_MS, 1)
                End Select
                Bass.BASS_ChannelSetAttribute(streamFX, BASSAttribute.BASS_ATTRIB_TEMPO_OPTION_SEEKWINDOW_MS, 14)
                Bass.BASS_ChannelSetAttribute(streamFX, BASSAttribute.BASS_ATTRIB_TEMPO_OPTION_OVERLAP_MS, 12)




                mySyncProcEnd = New SYNCPROC(AddressOf SyncProcEndCallback)

                mySyncHandleEnd = Bass.BASS_ChannelSetSync(m_currentAudioHandle.CurrentHandle, BASSSync.BASS_SYNC_END, 0, mySyncProcEnd, IntPtr.Zero)

                If m_currentAudioHandle.IsRemoteURL Then
                    syncMetaUpdated = New SYNCPROC(AddressOf MetaSync)
                    Bass.BASS_ChannelSetSync(m_currentAudioHandle.Handle, BASSSync.BASS_SYNC_META, 0, syncMetaUpdated, IntPtr.Zero)
                End If

                isStreamOk = True


                OnAudioHandleLoaded(Me, Nothing)
            Catch e As Exception
                isStreamOk = False
            End Try

            Return isStreamOk

        End Function

        Public Delegate Sub DelegateStatusChanged()

        Public Event StatusChanged As DelegateStatusChanged

        Public Sub OnStatusChanged()
            RaiseEvent StatusChanged()
        End Sub

        Public Delegate Sub DelegateTrackEnded()

        Public Event TrackEnded As DelegateTrackEnded

        Public Sub OnTrackEnded()
            RaiseEvent TrackEnded()
        End Sub

        Public Function Play(restart As Boolean) As Boolean
            If m_currentAudioHandle.Handle = 0 Then
                LoadAudio()
            End If

            Dim status As Boolean

            status = Bass.BASS_ChannelPlay(m_currentAudioHandle.CurrentHandle, restart)

            OnStatusChanged()

            Return status
        End Function

        Public Function Pause() As Boolean
            Dim status As Boolean = Bass.BASS_ChannelPause(m_currentAudioHandle.CurrentHandle)

            OnStatusChanged()

            Return status
        End Function

        Public Function [Stop]() As Boolean
            Dim status As Boolean = Bass.BASS_ChannelStop(m_currentAudioHandle.CurrentHandle)

            'instance.Position = 0;

            Position = 0

            OnStatusChanged()

            Return status
        End Function

        Public Function GetStreamStatus() As BASSActive
            Return Bass.BASS_ChannelIsActive(m_currentAudioHandle.CurrentHandle)
        End Function


        Public Function GetElapsedTime() As String
            If m_currentAudioHandle.GetElapsedTime > 3600 Then

                Return [String].Format(" {0}", Utils.FixTimespan(m_currentAudioHandle.GetElapsedTime(), "HHMMSS"))
            Else
                Return [String].Format(" {0}", Utils.FixTimespan(m_currentAudioHandle.GetElapsedTime(), "MMSS"))
            End If




        End Function

        Public Function GetTotalTime() As String
            If m_currentAudioHandle.LengthInSeconds > 3600 Then
                Return Utils.FixTimespan(m_currentAudioHandle.LengthInSeconds, "HHMMSS")
            Else
                Return Utils.FixTimespan(m_currentAudioHandle.LengthInSeconds, "MMSS")
            End If

        End Function

        Public Function GetRemainingTime() As String
            If (m_currentAudioHandle.LengthInSeconds - m_currentAudioHandle.GetElapsedTime()) > 3600 Then

                Return [String].Format("-{0}", Utils.FixTimespan((m_currentAudioHandle.LengthInSeconds - m_currentAudioHandle.GetElapsedTime()), "HHMMSS"))
            Else
                Return [String].Format("-{0}", Utils.FixTimespan((m_currentAudioHandle.LengthInSeconds - m_currentAudioHandle.GetElapsedTime()), "MMSS"))

            End If



        End Function

        Public Property Position() As Long
            Get
                Return m_currentAudioHandle.Position
            End Get
            Set(value As Long)
                m_currentAudioHandle.Position = value
            End Set
        End Property
        Public Shared Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
            target = value
            Return value
        End Function

    End Class




    Public Class AudioHandle
        Private Const ReverbChain As Integer = 1
        Private Const EchoChain As Integer = 2
        Private Const ChorusChain As Integer = 3
        Private Const FlangerChain As Integer = 4
        Private Const DistortionChain As Integer = 5

        Public Property originalBPM() As Single
            Get
                Return m_originalBPM
            End Get
            Private Set(value As Single)
                m_originalBPM = Value
            End Set
        End Property
        Private m_originalBPM As Single

        Private fxEQ As Integer() = {}

        Private fxChorusHandle As Integer = 0
        Private chorus As New BASS_DX8_CHORUS()

        Private fxFlangerHandle As Integer = 0
        Private flanger As New BASS_DX8_FLANGER()

        Private fxReverbHandle As Integer = 0
        Private reverb As New BASS_DX8_REVERB()

        Private fxEchoHandle As Integer = 0
        Private echo As New BASS_DX8_ECHO()

        Private fxDistortionHandle As Integer = 0
        Private distortion As New BASS_DX8_DISTORTION()

        Public Shared _Tempo As Integer
        Public Shared _Pitch As Integer


        Public Property TagInfo() As TAG_INFO
            Get
                Return m_TagInfo
            End Get
            Set(value As TAG_INFO)
                m_TagInfo = Value
            End Set
        End Property
        Private m_TagInfo As TAG_INFO

        Public Property ChannelInfo() As BASS_CHANNELINFO
            Get
                Return m_ChannelInfo
            End Get
            Private Set(value As BASS_CHANNELINFO)
                m_ChannelInfo = Value
            End Set
        End Property
        Private m_ChannelInfo As BASS_CHANNELINFO

        Public Property IsTagAvailable() As Boolean
            Get
                Return m_IsTagAvailable
            End Get
            Set(value As Boolean)
                m_IsTagAvailable = Value
            End Set
        End Property
        Private m_IsTagAvailable As Boolean

        Public Property Handle() As Integer
            Get
                Return m_Handle
            End Get
            Private Set(value As Integer)
                m_Handle = Value
            End Set
        End Property
        Private m_Handle As Integer

        Public Property HandleFX() As Integer
            Get
                Return m_HandleFX
            End Get
            Private Set(value As Integer)
                m_HandleFX = Value
            End Set
        End Property
        Private m_HandleFX As Integer

        'public Ptr<int> CurrentHandle { get; private set; }

        Public Property CurrentHandle() As Integer
            Get
                Return m_CurrentHandle
            End Get
            Private Set(value As Integer)
                m_CurrentHandle = Value
            End Set
        End Property
        Private m_CurrentHandle As Integer

        Public Property IsRemoteURL() As Boolean
            Get
                Return m_IsRemoteURL
            End Get
            Set(value As Boolean)
                m_IsRemoteURL = Value
            End Set
        End Property
        Private m_IsRemoteURL As Boolean

        Public Property IsTempoEnabled() As Boolean
            Get
                Return m_IsTempoEnabled
            End Get
            Private Set(value As Boolean)
                m_IsTempoEnabled = Value
            End Set
        End Property
        Private m_IsTempoEnabled As Boolean

        Public Sub FreeResources()
            Bass.BASS_StreamFree(HandleFX)
            Bass.BASS_StreamFree(Handle)
            Handle = 0
            HandleFX = 0
            IsRemoteURL = False
        End Sub

        Public Sub CreateHandle(filename As String, flagSample As BASSFlag, flagMusic As BASSFlag, flagStream As BASSFlag)
            IsRemoteURL = False

            ' sample stream
            Me.Handle = Bass.BASS_StreamCreateFile(filename, 0L, 0L, flagSample)

            ' music stream
            If Me.Handle = 0 Then
                ' BASS_MUSIC_PRESCAN calculate the playback length of the music, and enable seeking in bytes
                Me.Handle = Bass.BASS_MusicLoad(filename, 0L, 0, flagMusic, 0)

                If Me.Handle <> 0 Then
                    Dim bpm As Single = 0

                    Bass.BASS_ChannelGetAttribute(Me.Handle, BASSAttribute.BASS_ATTRIB_MUSIC_BPM, bpm)

                    originalBPM = bpm
                End If
            End If

            ' streaming server
            If Me.Handle = 0 Then
                If File.Exists(filename) Then

                    Me.Handle = Bass.BASS_StreamCreateURL(filename, 0, flagStream, Nothing, IntPtr.Zero)

                End If
                If Me.Handle <> 0 Then
                    IsRemoteURL = True

                End If
            End If

            If Me.Handle <> 0 Then
                ChannelInfo = Bass.BASS_ChannelGetInfo(Handle)

                TagInfo = New TAG_INFO(filename)
                IsTagAvailable = If(IsRemoteURL, BassTags.BASS_TAG_GetFromURL(Handle, TagInfo), BassTags.BASS_TAG_GetFromFile(Handle, TagInfo))
            End If

            Me.CurrentHandle = Me.Handle

            'this.CurrentHandle = new Ptr<int>(() => this.Handle, v => this.Handle = v);
        End Sub

        Public Sub CreateTempoHandle(stream As Integer, flag As BASSFlag)
            If IsRemoteURL Then
                'this.HandleFX = this.Handle;

                Me.LengthInBytes = 0
                Me.LengthInSeconds = 0
            Else
                Me.HandleFX = BassFx.BASS_FX_TempoCreate(stream, flag)

                If Me.HandleFX <> 0 Then
                    Me.LengthInBytes = Bass.BASS_ChannelGetLength(HandleFX)
                    Me.LengthInSeconds = Bass.BASS_ChannelBytes2Seconds(HandleFX, Me.LengthInBytes)

                    IsTempoEnabled = True


                    'this.CurrentHandle = new Ptr<int>(() => this.HandleFX, v => this.HandleFX = v);
                    Me.CurrentHandle = Me.HandleFX
                End If
            End If
        End Sub

        'public void InitEQ(KeyValuePair<int, float>[] eqItems)
        '{
        '    fxEQ = new int[eqItems.Length];

        '    foreach (KeyValuePair<int, float> item in eqItems)
        '    {

        '        fxEQ[item.Key] = Bass.BASS_ChannelSetFX(CurrentHandle, BASSFXType.BASS_FX_DX8_PARAMEQ, 0);

        '        BASS_DX8_PARAMEQ eq = new BASS_DX8_PARAMEQ();
        '        eq.fCenter = item.Value;

        '        Bass.BASS_FXSetParameters(fxEQ[item.Key], eq);
        '    }
        '}

        Public Sub InitEQ(eqItems As Single())
            fxEQ = New Integer(eqItems.Length - 1) {}

            For i As Integer = 0 To eqItems.Length - 1
                fxEQ(i) = Bass.BASS_ChannelSetFX(CurrentHandle, BASSFXType.BASS_FX_DX8_PARAMEQ, 0)

                Dim eq As New BASS_DX8_PARAMEQ()
                eq.fCenter = eqItems(i)

                Bass.BASS_FXSetParameters(fxEQ(i), eq)
            Next
        End Sub

        Public Sub SetEQ(index As Integer, gain As Single)
            Dim eq As New BASS_DX8_PARAMEQ()
            If fxEQ.Length > 0 Then
                If Bass.BASS_FXGetParameters(fxEQ(index), eq) Then
                    eq.fGain = gain
                    Bass.BASS_FXSetParameters(fxEQ(index), eq)
                End If
            End If
        End Sub


        Public Sub SetPan(value As Single)
            Dim attrib As BASSAttribute
            Dim stream As Integer

            If IsModule() Then
                attrib = BASSAttribute.BASS_ATTRIB_MUSIC_PANSEP
                stream = Handle
            Else
                attrib = BASSAttribute.BASS_ATTRIB_PAN
                stream = CurrentHandle
            End If

            Bass.BASS_ChannelSetAttribute(stream, attrib, value)
        End Sub

        Public Sub SetChorusFX(wetDryMix As Single, depth As Single, feedback As Single, frequency As Single, waveform As Integer, delay As Single, _
            phase As BASSFXPhase)
            chorus.fWetDryMix = wetDryMix
            chorus.fDepth = depth
            chorus.fFeedback = feedback
            chorus.fFrequency = frequency
            chorus.lWaveform = waveform
            chorus.fDelay = delay
            chorus.lPhase = phase

            Bass.BASS_FXSetParameters(fxChorusHandle, chorus)
        End Sub


        Public Sub SetFlangerFX(wetDryMix As Single, depth As Single, feedback As Single, frequency As Single, waveform As Integer, delay As Single, _
            phase As BASSFXPhase)
            flanger.fWetDryMix = wetDryMix
            flanger.fDepth = depth
            flanger.fFeedback = feedback
            flanger.fFrequency = frequency
            flanger.lWaveform = waveform
            flanger.fDelay = delay
            flanger.lPhase = phase

            Bass.BASS_FXSetParameters(fxFlangerHandle, flanger)
        End Sub


        Public Sub SetReverbFX(inGain As Single, reverbMix As Single, reverbTime As Single, highFreqRTRatio As Single)
            reverb.fInGain = inGain
            reverb.fReverbMix = reverbMix
            reverb.fReverbTime = reverbTime
            reverb.fHighFreqRTRatio = highFreqRTRatio

            Bass.BASS_FXSetParameters(fxReverbHandle, reverb)
        End Sub


        Public Sub SetEchoFX(wetDryMix As Single, feedback As Single, leftDelay As Single, rightDelay As Single, panDelay As Boolean)
            echo.fWetDryMix = wetDryMix
            echo.fFeedback = feedback
            echo.fLeftDelay = leftDelay
            echo.fRightDelay = rightDelay
            echo.lPanDelay = panDelay

            Bass.BASS_FXSetParameters(fxEchoHandle, echo)
        End Sub

        Public Sub SetDistortionFX(gain As Single, edge As Single, postEQCenterFrequency As Single, postEQBandwidth As Single, preLowpassCutoff As Single)
            distortion.fGain = gain
            distortion.fEdge = edge
            distortion.fPostEQCenterFrequency = postEQCenterFrequency
            distortion.fPostEQBandwidth = postEQBandwidth
            distortion.fPreLowpassCutoff = preLowpassCutoff

            Bass.BASS_FXSetParameters(fxDistortionHandle, distortion)
        End Sub

        Public Sub ToggleChorus(enable As Boolean, Optional chain As Integer = ChorusChain)
            If enable Then
                Me.fxChorusHandle = Bass.BASS_ChannelSetFX(CurrentHandle, BASSFXType.BASS_FX_DX8_CHORUS, chain)
                Bass.BASS_FXSetParameters(fxChorusHandle, chorus)
            Else
                Bass.BASS_ChannelRemoveFX(CurrentHandle, fxChorusHandle)
            End If
        End Sub

        Public Sub ToggleFlanger(enable As Boolean, Optional chain As Integer = FlangerChain)
            If enable Then
                Me.fxFlangerHandle = Bass.BASS_ChannelSetFX(CurrentHandle, BASSFXType.BASS_FX_DX8_FLANGER, chain)
                Bass.BASS_FXSetParameters(fxFlangerHandle, flanger)
            Else
                Bass.BASS_ChannelRemoveFX(CurrentHandle, fxFlangerHandle)
            End If
        End Sub


        Public Sub ToggleEcho(enable As Boolean, Optional chain As Integer = EchoChain)
            If enable Then
                Me.fxEchoHandle = Bass.BASS_ChannelSetFX(CurrentHandle, BASSFXType.BASS_FX_DX8_ECHO, chain)
                Bass.BASS_FXSetParameters(fxEchoHandle, echo)
            Else
                Bass.BASS_ChannelRemoveFX(CurrentHandle, fxEchoHandle)
            End If
        End Sub

        Public Sub ToggleDistortion(enable As Boolean, Optional chain As Integer = DistortionChain)
            If enable Then
                Me.fxDistortionHandle = Bass.BASS_ChannelSetFX(CurrentHandle, BASSFXType.BASS_FX_DX8_DISTORTION, chain)

                Bass.BASS_FXSetParameters(fxDistortionHandle, distortion)
            Else
                Bass.BASS_ChannelRemoveFX(CurrentHandle, fxDistortionHandle)
            End If
        End Sub


        Public Sub SetVolume(volume As Single)
            Dim isModule__1 As Boolean = IsModule()

            'forced to false
            isModule__1 = False

            Dim attrib As BASSAttribute
            Dim stream As Integer

            If isModule__1 Then
                attrib = BASSAttribute.BASS_ATTRIB_MUSIC_VOL_GLOBAL
                stream = Me.Handle
            Else
                attrib = BASSAttribute.BASS_ATTRIB_VOL
                stream = Me.CurrentHandle
            End If

            Bass.BASS_ChannelSetAttribute(stream, attrib, volume)
        End Sub


        Public Sub SetTempo(tempo As Single)
            Dim attrib As BASSAttribute
            Dim stream As Integer

            attrib = BASSAttribute.BASS_ATTRIB_TEMPO
            stream = CurrentHandle
            _Tempo = tempo
            If _Tempo <> 0 Then
                '   Bass.BASS_ChannelSetAttribute(HandleFX, BASSAttribute.BASS_ATTRIB_TEMPO_OPTION_SEQUENCE_MS, 1)
            End If

            Bass.BASS_ChannelSetAttribute(stream, attrib, tempo)
           
        End Sub

        Public Sub SetModuleBPM(tempo As Single)
            If IsModule() Then
                Dim attrib As BASSAttribute = BASSAttribute.BASS_ATTRIB_MUSIC_BPM
                Dim stream As Integer = Handle

                Bass.BASS_ChannelSetAttribute(stream, attrib, tempo)
            End If
        End Sub

        Public Sub SetPitch(pitch As Single)
            ' change the pitch (key) by one octave (12 semitones)
            If Not IsModule() Then
                _Pitch = pitch
                If _Tempo <> 0 Then
                    ' Bass.BASS_ChannelSetAttribute(HandleFX, BASSAttribute.BASS_ATTRIB_TEMPO_OPTION_SEQUENCE_MS, 50)
                End If

                Bass.BASS_ChannelSetAttribute(HandleFX, BASSAttribute.BASS_ATTRIB_TEMPO_PITCH, pitch)
            End If
        End Sub


        Public Sub ToggleReverb(enable As Boolean, Optional chain As Integer = ReverbChain)
            If enable Then
                Me.fxReverbHandle = Bass.BASS_ChannelSetFX(CurrentHandle, BASSFXType.BASS_FX_DX8_REVERB, chain)
                Bass.BASS_FXSetParameters(fxReverbHandle, reverb)
            Else
                Bass.BASS_ChannelRemoveFX(CurrentHandle, fxReverbHandle)
            End If
        End Sub

        Public Property Position() As Long
            Get
                Return Bass.BASS_ChannelGetPosition(Handle)
            End Get
            Set(value As Long)
                Bass.BASS_ChannelSetPosition(Handle, CLng(value))
            End Set
        End Property

        Public Property LengthInBytes() As Long
            Get
                Return m_LengthInBytes
            End Get
            Private Set(value As Long)
                m_LengthInBytes = Value
            End Set
        End Property
        Private m_LengthInBytes As Long

        Public Property LengthInSeconds() As Double
            Get
                Return m_LengthInSeconds
            End Get
            Private Set(value As Double)
                m_LengthInSeconds = Value
            End Set
        End Property
        Private m_LengthInSeconds As Double

        Public Function GetElapsedTime() As Double
            Return Bass.BASS_ChannelBytes2Seconds(Handle, Position)
        End Function

        Public Function IsModule() As Boolean
            If Me.ChannelInfo Is Nothing Then
                Return False
            End If

            Return (ChannelInfo.[ctype] And BASSChannelType.BASS_CTYPE_MUSIC_MOD) = BASSChannelType.BASS_CTYPE_MUSIC_MOD
        End Function


    End Class
End Namespace

