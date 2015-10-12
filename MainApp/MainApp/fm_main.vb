Public Class fm_main



    Private Sub btn_Checkupdates_Click(sender As Object, e As EventArgs) Handles btn_Checkupdates.Click
		Dim Check As Updaters = New Updaters()
		Check.CheckUpdate()

		'CheckForUpdates()
    End Sub


	'Public Sub CheckForUpdates()
	'    'TODO: Add handling of 404!!
	'    Try
	'        Dim request As System.Net.HttpWebRequest = System.Net.HttpWebRequest.Create("http://www.yourwebsite.com/main_version.txt")
	'        Dim response As System.Net.HttpWebResponse = request.GetResponse()
	'        Dim sr As System.IO.StreamReader = New System.IO.StreamReader(response.GetResponseStream())
	'        Dim newestversion As String = sr.ReadToEnd()
	'        Dim currentversion As String = Application.ProductVersion
	'        If newestversion.Contains(currentversion) Then
	'            MsgBox("You are up to date!")
	'        Else
	'            If MsgBox("Update Found, would you like to update now?", vbInformation + vbYesNo) = MsgBoxResult.Yes Then
	'                Process.Start("updater.exe")
	'            End If
	'        End If
	'    Catch ex As Exception
	'        MsgBox(ex.Message)
	'    End Try


	'End Sub


End Class
