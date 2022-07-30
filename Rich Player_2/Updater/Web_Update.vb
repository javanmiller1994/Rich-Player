Imports System.Windows.Forms
Imports System.Drawing

Imports System.IO
Imports System.Net
Imports System.Text
Imports Rich_Player.CsWinFormsBlackApp

Class Web_Update
    Dim version As String
    Public Shared URI As String = "https://www.dropbox.com/s/znc96pmq0sbygik/version.txt?dl=1"
    Public Shared DownUri As String = "https://www.dropbox.com/s/zpnuna43em868w4/Rich%20Player%20-%20Full%20Install.exe?dl=1"
    Public Shared UpdateUri As String = "https://www.dropbox.com/s/b4djfuown4xhxus/Rich%20Player%20-%20Upgrade.exe?dl=1"
    Public Shared UpdateQuickUri As String = "https://www.dropbox.com/s/b4djfuown4xhxus/Rich%20Player%20-%20Upgrade.exe?dl=1"
    Public Shared Downloading As Boolean = False
    Public Shared VersionNumber As String
    Public Shared MaxCheck As Integer = 0

    Public Shared Sub Main2()
        If InternetConnection() = False Then
            MyMsgBox.Show("No Internet Connection", "Error", True)
            Exit Sub
        End If

       
        '  URI = "https://drive.google.com/uc?export=download&id=1G6M_rz3mi1z56ly6C1cW1vXlR4ABsBUp"    '"http://www.rexfordrich.com/uploads/9/9/5/5/9955575/version.txt"

        '  DownUri = "https://drive.google.com/uc?export=download&id=1aB2msywi1BLikpnhgiiLMOIdLyIASd3B"   '"https://www.dropbox.com/s/qo6fc18yo3ynqze/Rich%20Player%20-%20Full%20Install.exe?dl=0"

        '  UpdateUri = "https://drive.google.com/uc?export=download&id=18WLG1NkmIFozdMUCXkreN6F0ej5Dzjbz"    '"http://www.rexfordrich.com/uploads/9/9/5/5/9955575/rich_player_-_upgrade_install.exe"    '"https://www.dropbox.com/s/h4pghhpr7meebhw/Rich%20Player%20-%20Upgrade%20Install.exe?dl=0"

        ' UpdateQuickUri = "http://www.rexfordrich.com/uploads/9/9/5/5/9955575/rich_player_-_upgrade_install_quick.exe"    '"https://www.dropbox.com/s/to5ue4ej4kwfvv3/Rich%20Player%20-%20Upgrade%20Install%20Quick.exe?dl=0"


        Try
            Dim address As String = URI
            Dim client As WebClient = New WebClient() : Dim reader As StreamReader = New StreamReader(client.OpenRead(address))
            VersionNumber = reader.ReadToEnd() : Dim version As Integer = CInt(VersionNumber.Replace(".", ""))

            If version > CInt(Application.ProductVersion.Replace(".", "")) Then
                Try
                    UpdateDialog.ShowDialog()
                Catch ex As Exception : End Try

            Else
                MyMsgBox.Show("There are no new updates", "Update", True)
            End If
            Return
        Catch ex As Exception
            MyMsgBox.Show("Error checking for updates! You may not be connected to internet! Please check your connection", "Error", True)
            ' MsgBox("Error checking for updates! You may not be connected to internet! Please check your connection.")
        End Try


    End Sub 'Main

    Public Shared Sub GetVersionNumber()

        If Retry >= 2 Then
            Exit Sub
        End If
        If MaxCheck >= 3 Then
            Exit Sub
        End If

        If InternetConnection() = False Then
            'Form1.ShowMsg("No Internet Connection", "Error", True)
            Exit Sub
        End If
        Try
            Dim address As String = "https://www.dropbox.com/s/znc96pmq0sbygik/version.txt?dl=1"
            ' "https://www.dropbox.com/s/znc96pmq0sbygik/version.txt?dl=1"
            ' "https://drive.google.com/uc?export=download&id=1G6M_rz3mi1z56ly6C1cW1vXlR4ABsBUp"
            Dim client As WebClient = New WebClient()
        
            Dim _UserAgent As String = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.77 Safari/537.36"
            client.Headers.Add(HttpRequestHeader.UserAgent, _UserAgent)
            client.Headers.Add("Content-Type", "application / zip, application / octet - stream")
            client.Headers.Add("Accept-Language", "pt-BR, pt;q=0.5")
            client.Headers.Add("Accept", "Text/ html, application / xhtml + Xml, application / Xml;q=0.9,*/*;q=0.8")

            Dim reader As StreamReader = New StreamReader(client.OpenRead(address))
            VersionNumber = reader.ReadToEnd()

            Dim version As String = CInt(VersionNumber.Replace(".", ""))
        Catch ex As Exception
            Retry += 1


        End Try

        MaxCheck += 1

        ' VersionNumber = version
    End Sub

    



    Public Shared Function InternetConnection() As Boolean
        Try
            Return My.Computer.Network.Ping("www.google.com")
        Catch
            Return False
        End Try

    End Function



    Public Shared Sub AutoUpdate()
        If InternetConnection() = False Then
            Exit Sub
        End If
        If Retry >= 2 Then
            Exit Sub
        End If
        Try

            GetVersionNumber()
            Dim version As String = CInt(VersionNumber.Replace(".", ""))

            If version > CInt(Application.ProductVersion.Replace(".", "")) Then
                Try

                    Form1.Label_Update.Visible = True
                    Form1.Label_Update.BringToFront()

                Catch ex As Exception
                    Retry += 1

                End Try
            End If
            Return
        Catch ex As Exception : End Try
    End Sub

    Public Shared Retry As Integer = 0
    Public Shared Function UpdateAvailable(ByVal Available As Boolean)
        If InternetConnection() = False Then
            'Form1.ShowMsg("No Internet Connection", "Error", True)
            Exit Function
        End If
        If Retry >= 2 Then
            Exit Function
        End If
        Try
           GetVersionNumber()

            Dim version As Integer = CInt(VersionNumber.Replace(".", ""))
            If version > CInt(Application.ProductVersion.Replace(".", "")) Then
                Available = True

            Else
                Available = False
            End If

        Catch ex As Exception
            Retry += 1


        End Try

        Return Available

    End Function

End Class 'Web_update

