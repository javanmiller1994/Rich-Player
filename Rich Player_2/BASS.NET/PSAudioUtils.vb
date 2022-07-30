
Imports System.Resources
Imports System.Drawing
Imports PitchAndShiftAudio.Properties
Imports System.Windows.Forms
Imports System.IO
Imports Un4seen.Bass
Imports Un4seen.Bass.AddOn.Enc
Imports Un4seen.Bass.AddOn.Fx
Imports System.Reflection
Imports AudioController
Imports System.Text.RegularExpressions
Imports Rich_Player.AudioController

Namespace PitchAndShiftAudio
    Class PSAudioUtils
        Public Shared Function IsWindowsOS() As Boolean
            Select Case Environment.OSVersion.Platform
                Case PlatformID.Win32NT, PlatformID.Win32S, PlatformID.Win32Windows
                    Return True
                Case Else
                    Return False
            End Select
        End Function

        Public Const NO_IMAGE As Integer = 999

       

        Public Shared Function IsModuleFile(filename As String) As Boolean
            Dim isMod As Integer = Bass.BASS_MusicLoad(filename, 0L, 0, BASSFlag.BASS_MUSIC_DECODE, 44100)
            Bass.BASS_StreamFree(isMod)
            Return (isMod <> 0)
        End Function

        Public Shared Function IsPlaylist(filename As String) As Boolean
            Dim ext As String = Path.GetExtension(filename)
            Return (ext.Equals(".pls", StringComparison.InvariantCultureIgnoreCase) OrElse ext.Equals(".m3u", StringComparison.InvariantCultureIgnoreCase))
        End Function

        Public Shared Sub InitBass(handle As IntPtr)
            Dim bassEmail As String = Settings.[Default].BassEmail
            Dim bassCode As String = Settings.[Default].BassCode

            Dim targetPath As String = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)

            If Utils.Is64Bit Then
                targetPath = Path.Combine(targetPath, "lib/x64")
            Else
                targetPath = Path.Combine(targetPath, "lib/x86")
            End If

            If Not String.IsNullOrEmpty(bassEmail) AndAlso Not String.IsNullOrEmpty(bassCode) Then
                BassNet.Registration(bassEmail, bassCode)
            End If

            If PSAudioUtils.IsWindowsOS() Then
                Dim isBassLoad As Boolean = Bass.LoadMe(targetPath)
                Dim isBassFxLoad As Boolean = BassFx.LoadMe(targetPath)

                'Globals.Instance.PluginsLoaded = Bass.BASS_PluginLoadDirectory(targetPath);

                'Globals.Instance.FileSupportedExtFilter = Utils.BASSAddOnGetPluginFileFilter(Globals.Instance.PluginsLoaded, "All supported Audio Files");

                Dim isBassEncLoad As Boolean = BassEnc.LoadMe(targetPath)
            End If



            Const allSupportedAudioFilesWord As String = "All supported Audio Files"
            Const playlistExtFilter As String = "*.pls;*.m3u"

         
            Dim allSupportedFilesPattern As New Regex(allSupportedAudioFilesWord & Convert.ToString("\|(.*?)\|"))
            '   Dim match As Match = allSupportedFilesPattern.Match(Globals.Instance.FileSupportedExtFilter)

            '         Dim allSupportedFiles As String = match.Value

            '    allSupportedFiles = [String].Format("{0};{1}|", allSupportedFiles.Substring(0, allSupportedFiles.Length - 1), playlistExtFilter)

            '  Globals.Instance.FileSupportedExtFilter = Globals.Instance.FileSupportedExtFilter.Replace(match.Value, allSupportedFiles)

            ' Globals.Instance.FileSupportedExtFilter += [String].Format("|Playlist|{0}", playlistExtFilter)



            Dim isBassInit As Boolean = False

            Dim devices As BASS_DEVICEINFO() = Bass.BASS_GetDeviceInfos()

            '     Globals.Instance.Devices = devices

            Dim devnum As Integer = 0

            For Each device As BASS_DEVICEINFO In devices
                isBassInit = Bass.BASS_Init(System.Math.Max(System.Threading.Interlocked.Increment(devnum), devnum - 1), 44100, BASSInit.BASS_DEVICE_DEFAULT, handle)
            Next

            AudioPlayer.Instance.SetDevice(Settings.[Default].DefaultDevice)

            'if (!isBassInit)
            '    throw new ApplicationException("Some errors occurred while initializing audio dll");

        End Sub
    End Class
End Namespace

