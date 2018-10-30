Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Threading.Tasks
Imports System.Windows.Forms

Namespace GettingStarted
	Friend NotInheritable Class Program
		''' <summary>
		''' The main entry point for the application.
		''' </summary>
		Private Sub New()
		End Sub
		<STAThread> _
		Shared Sub Main()
			Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(DemoCommon.FindLicenseKey())
			Application.EnableVisualStyles()
			Application.SetCompatibleTextRenderingDefault(False)
			Application.Run(New Form1())
		End Sub
	End Class

	''' <summary>
	''' Represents a class that is used to find the licensing file for Syncfusion controls.
	''' </summary>
	Public Class DemoCommon
		''' <summary>
		''' Finds the license key from the Common folder.
		''' </summary>
		''' <returns>Returns the license key.</returns>
		Public Shared Function FindLicenseKey() As String
			Dim licenseKeyFile As String = "..\Common\SyncfusionLicense.txt"
			For n As Integer = 0 To 19
				If Not System.IO.File.Exists(licenseKeyFile) Then
					licenseKeyFile = "..\" & licenseKeyFile
					Continue For
				End If
				Return System.IO.File.ReadAllText(licenseKeyFile)
			Next n
			Return String.Empty
		End Function
	End Class
End Namespace
