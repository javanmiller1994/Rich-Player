
Imports AudioController
Imports System.Configuration
Imports Rich_Player.AudioController

Namespace PitchAndShiftAudio
    '
    '     * IMPORTANT: All serializing objects must have public accessors on its properties
    '     * 

    <SettingsSerializeAs(System.Configuration.SettingsSerializeAs.Xml)> _
    Public Class AudioSettings
        Public Sub New()
            ' default settings
            Me.Volume = 50
            Me.EQ = New Integer(4) {}
            Me.TrackList = New TrackList()
        End Sub

        Public Property Volume() As Integer
            Get
                Return m_Volume
            End Get
            Set(value As Integer)
                m_Volume = value
            End Set
        End Property
        Private m_Volume As Integer
        Public Property EQ() As Integer()
            Get
                Return m_EQ
            End Get
            Set(value As Integer())
                m_EQ = value
            End Set
        End Property
        Private m_EQ As Integer()
        Public Property Pan() As Integer
            Get
                Return m_Pan
            End Get
            Set(value As Integer)
                m_Pan = value
            End Set
        End Property
        Private m_Pan As Integer
        Public Property Pitch() As Integer
            Get
                Return m_Pitch
            End Get
            Set(value As Integer)
                m_Pitch = value
            End Set
        End Property
        Private m_Pitch As Integer
        Public Property Speed() As Integer
            Get
                Return m_Speed
            End Get
            Set(value As Integer)
                m_Speed = value
            End Set
        End Property
        Private m_Speed As Integer
        Public Property LastTrack() As Integer
            Get
                Return m_LastTrack
            End Get
            Set(value As Integer)
                m_LastTrack = value
            End Set
        End Property
        Private m_LastTrack As Integer
        Public Property TrackList() As TrackList
            Get
                Return m_TrackList
            End Get
            Set(value As TrackList)
                m_TrackList = value
            End Set
        End Property
        Private m_TrackList As TrackList
    End Class
End Namespace

