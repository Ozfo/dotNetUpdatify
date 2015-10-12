Imports System.IO
Public Class Updaters
	Public Property UpdateURLUpdater As String = "http://www.yourwebsite.com/updater_version.txt"
	Public Property UpdateURLApplication As String = "http://www.yourwebsite.com/main_version.txt"
	Public Property UpdaterProcess As String = "updater.exe"
	Public Property UpdaterFile As String = "http://www.yourwebsite.com/updater.zip"
	Private Function KillUpdater()
		Try
			Dim Processes() As Process = Process.GetProcessesByName(Me.UpdaterProcess)
			For Each item In Processes
				item.Kill()
			Next
			'Killed all
			Return True
		Catch ex As Exception
			MsgBox(ex.Message)
		End Try
		Return False
	End Function
	Private Function isUpdaterUptoDate()
		Try
			Dim getVersion = FileVersionInfo.GetVersionInfo(Me.UpdaterProcess).FileVersion
			Dim request As System.Net.HttpWebRequest = System.Net.HttpWebRequest.Create(UpdateURLUpdater)
			Dim response As System.Net.HttpWebResponse = request.GetResponse()
			Dim sr As System.IO.StreamReader = New System.IO.StreamReader(response.GetResponseStream())
			Dim newestversion As String = sr.ReadToEnd()
			Return newestversion.Contains(getVersion)
		Catch ex As Exception
			MsgBox(ex.Message)
		End Try
		Return False
	End Function
	Private Function isApplicationUpToDate()
		Try
			Dim request As System.Net.HttpWebRequest = System.Net.HttpWebRequest.Create(Me.UpdateURLApplication)
			request.Timeout = 1500
			Dim response As System.Net.HttpWebResponse = request.GetResponse()
			Dim sr As System.IO.StreamReader = New System.IO.StreamReader(response.GetResponseStream())
			Dim newestversion As String = sr.ReadToEnd()
			Dim currentversion As String = Application.ProductVersion
			Return newestversion.Contains(currentversion)
		Catch ex As Exception
			MsgBox(ex.Message)
		End Try
		Return False
	End Function
	Private Sub UpdateUpdater()
		If KillUpdater() Then
			Try

				Dim appPath As String = Application.StartupPath
				Dim downloadDir As String = Application.StartupPath & "\UpdaterUpdates"
				Dim updateFiles As String = downloadDir & "\update.zip"
				Directory.CreateDirectory(downloadDir)
				My.Computer.Network.DownloadFile(Me.UpdaterFile, updateFiles)
				Dim shObj As Object = Activator.CreateInstance(Type.GetTypeFromProgID("Shell.Application"))
				Dim output As Object = shObj.NameSpace((appPath))
				Dim input As Object = shObj.NameSpace((updateFiles))
				output.CopyHere((input.Items), 16)
				Directory.Delete(downloadDir, True)

			Catch ex As Exception
				MsgBox(ex.Message)
			End Try
		End If
	End Sub
	Public Sub CheckUpdate()
		If KillUpdater() Then
			If isUpdaterUptoDate() = False Then
				UpdateUpdater()
			End If
			If isApplicationUpToDate(Me.UpdateURLApplication) = False Then
				Process.Start(Me.UpdaterProcess)
			End If
		End If
	End Sub
End Class
