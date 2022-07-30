
Imports System.Net
Imports System.Collections.Generic
Imports System.Threading
Imports Un4seen.Bass

Namespace PitchAndShiftAudio
    Class Globals
        'public WebProxy WebProxy { get; set; }

        Private Shared m_instance As New Globals()

        Public Const URI_UPDATER As String = "http://dl.dropbox.com/u/55285635/audiops_updater.xml"

        Public Const AppTitle As String = "Audio Pitch & Shift"

        Public Const AppProgID As String = "AudioPitchShift"

        'public const string AppProgId = "AudioPS";

        Public Const AppHomepage As String = "http://audiops.codeplex.com/"

        Public tokenSource As CancellationTokenSource


        Protected Sub New()
        End Sub

        Public Shared ReadOnly Property Instance() As Globals
            Get
                Return m_instance
            End Get
        End Property

        Public PluginsLoaded As Dictionary(Of Integer, String)

        Public FileSupportedExtFilter As String

        Public Devices As BASS_DEVICEINFO()

        'private static AudioPlayer audioPlayer;

        'public AudioPlayer AudioPlayer { get; private set; }
    End Class
End Namespace

'=======================================================
'Service provided by Telerik (www.telerik.com)
'Conversion powered by NRefactory.
'Twitter: @telerik
'Facebook: facebook.com/telerik
'=======================================================
