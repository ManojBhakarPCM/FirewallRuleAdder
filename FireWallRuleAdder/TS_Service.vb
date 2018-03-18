Imports System.ServiceProcess
Imports System.Management

Public Class TS_Service

    ' Is the service running?
    ' Returns true if we need to start the service
    Public Function CheckService(ByVal PC As String) As Boolean
        Dim obj As ManagementObject

        obj = New ManagementObject("\\" & PC & "\root\cimv2:Win32_Service.Name='TermService'")
        If Not IsNothing(obj) Then
            If obj("StartMode").ToString <> "Disabled" And obj("State").ToString = "Running" Then
                Return False
            End If
        End If
        Return True
    End Function

    Public Sub StartService(ByVal PC As String)
        Dim obj As ManagementObject
        Dim inParams, outParams As ManagementBaseObject
        Dim Result As Integer
        Dim sc As ServiceController

        obj = New ManagementObject("\\" & PC & "\root\cimv2:Win32_Service.Name='TermService'")

        ' Change the Start Mode to Manual
        If obj("StartMode").ToString = "Disabled" Then
            ' Get an input parameters object for this method
            inParams = obj.GetMethodParameters("ChangeStartMode")
            inParams("StartMode") = "Manual"

            ' do it!
            outParams = obj.InvokeMethod("ChangeStartMode", inParams, Nothing)
            Result = Convert.ToInt32(outParams("returnValue"))
            If Result <> 0 Then
                Throw New Exception("ChangeStartMode method error code " & Result)
            End If
        End If

        ' Start the service
        If obj("State").ToString <> "Running" Then
            ' now start the service
            sc = New ServiceController("TermService", PC)
            sc.Start()
            sc.WaitForStatus(ServiceControllerStatus.Running)
        End If
    End Sub

    Public Sub StopService(ByVal PC As String)
        Dim obj As ManagementObject
        Dim inParams, outParams As ManagementBaseObject
        Dim Result As Integer


        ' we can only disable the service and wait
        ' until the next reboot to stop it


        obj = New ManagementObject("\\" & PC & "\root\cimv2:Win32_Service.Name='TermService'")

        ' change the Start Mode to Disabled
        If obj("StartMode").ToString <> "Disabled" Then
            ' Get an input parameters object for this method
            inParams = obj.GetMethodParameters("ChangeStartMode")
            inParams("StartMode") = "Disabled"

            ' do it!
            outParams = obj.InvokeMethod("ChangeStartMode", inParams, Nothing)
            Result = Convert.ToInt32(outParams("returnValue"))
            If Result <> 0 Then
                Throw New Exception("ChangeStartMode method error code " & Result)
            End If
        End If
    End Sub
End Class
