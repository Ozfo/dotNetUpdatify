Imports System.IO


Public Class updater

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Try
            CheckForUpdates()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub doUpdate()
        Try
            'Declare application path
            Dim appPath As String = Application.StartupPath

            'Declare download directory path
            Dim downloadDir As String = Application.StartupPath & "\DownloadedUpdates"

            'Declare update files path
            Dim updateFiles As String = downloadDir & "\update.zip"

            'Create download directory
            Directory.CreateDirectory(downloadDir)

            'Download updates file in .zip file
            My.Computer.Network.DownloadFile("http://www.yourwesbite.com/mainapp.zip", updateFiles)

            'TODO: Add handling of 404!!

            'Get windows default zip creator/extrator 
            Dim shObj As Object = Activator.CreateInstance(Type.GetTypeFromProgID("Shell.Application"))

            'Declare the folder where the items will be extracted.
            Dim output As Object = shObj.NameSpace((appPath))

            'Declare the input zip file.
            Dim input As Object = shObj.NameSpace((updateFiles))

            'Extract the items from the zip file. Used option 16 to auto overwrite: https://msdn.microsoft.com/en-us/library/windows/desktop/bb787866%28v=vs.85%29.aspx?f=255&MSPPError=-2147217396
            output.CopyHere((input.Items), 16)

            'Clean up
            Directory.Delete(downloadDir, True)

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try


    End Sub



    Private Sub CheckIfRunning_Then_update()
        Try
            Dim p() As Process
            p = Process.GetProcessesByName("MainApp")
            'check if process of main app running
            If p.Count > 0 Then

                If MsgBox("Application Is Running! Would you like to close it?", vbYesNo + vbInformation, "Process running") = MsgBoxResult.Yes Then
					For Each Process As Process In p
						'kill process of main app
						Process.Kill()
					Next
                    MsgBox("Update now ready, click ok to continue.")
                    'do update of main app
                    doUpdate()
                    MsgBox("Update Completed!")
                    'start main app
                    Process.Start("MainApp.exe")
                    'exit updater
                    Application.Exit()
                Else
                    MsgBox("Update could be run anytime.")
                    Application.Exit()
                End If

            Else
                doUpdate()
                MsgBox("Update Completed!")
                Process.Start("MainApp.exe")
                Application.Exit()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try


    End Sub

    Private Sub CheckForUpdates()
        'TODO: Add handling of 404!!
        Try
            'check for updates to updater
            Dim request As System.Net.HttpWebRequest = System.Net.HttpWebRequest.Create("http://www.yourwebsite.com/updater_version.txt")
            Dim response As System.Net.HttpWebResponse = request.GetResponse()
            Dim sr As System.IO.StreamReader = New System.IO.StreamReader(response.GetResponseStream())
            Dim newestversion As String = sr.ReadToEnd()
            Dim currentversion As String = Application.ProductVersion
            If newestversion.Contains(currentversion) Then
                'do update for main app if no update found for updater
                MsgBox("No updates for updater found, main app update will continue") 'Do not show the user this...

                CheckIfRunning_Then_update()
            Else
                'update found for updater
                MsgBox("Update Found for updater, click Ok to update now. Updater will exit and restart", vbInformation)
                'start daemon to perform update of updater
                Process.Start("daemon.exe")

            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try


    End Sub
End Class
