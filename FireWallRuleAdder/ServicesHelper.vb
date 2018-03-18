
Imports System.ComponentModel
Imports System.Management
Imports System.Runtime.InteropServices
Imports System.Security
Imports System.ServiceProcess

Public NotInheritable Class ServiceHelper
    <DllImport("advapi32.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
    Public Shared Function ChangeServiceConfig(hService As IntPtr, nServiceType As UInt32, nStartType As UInt32, nErrorControl As UInt32, lpBinaryPathName As [String], lpLoadOrderGroup As [String],
        lpdwTagId As IntPtr, <[In]> lpDependencies As Char(), lpServiceStartName As [String], lpPassword As [String], lpDisplayName As [String]) As [Boolean]
    End Function

    <DllImport("advapi32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
    Private Shared Function OpenService(hSCManager As IntPtr, lpServiceName As String, dwDesiredAccess As UInteger) As IntPtr
    End Function

    Public Declare Unicode Function OpenSCManager Lib "advapi32.dll" Alias "OpenSCManagerW" (machineName As String, databaseName As String, dwAccess As UInteger) As IntPtr

    <DllImport("advapi32.dll", EntryPoint:="CloseServiceHandle")>
    Public Shared Function CloseServiceHandle(hSCObject As IntPtr) As Integer
    End Function

    Private Const SERVICE_NO_CHANGE As UInteger = &HFFFFFFFFUI
    Private Const SERVICE_QUERY_CONFIG As UInteger = &H1
    Private Const SERVICE_CHANGE_CONFIG As UInteger = &H2
    Private Const SC_MANAGER_ALL_ACCESS As UInteger = &HF003F

    Public Shared Sub ChangeStartMode(svc As String, mode As ServiceStartMode)
        Dim scManagerHandle = OpenSCManager(Nothing, Nothing, SC_MANAGER_ALL_ACCESS)
        If scManagerHandle = IntPtr.Zero Then
            Throw New ExternalException("Open Service Manager Error")
        End If

        Dim serviceHandle = OpenService(scManagerHandle, svc, SERVICE_QUERY_CONFIG Or SERVICE_CHANGE_CONFIG)

        If serviceHandle = IntPtr.Zero Then
            Throw New ExternalException("Open Service Error")
        End If

        Dim result = ChangeServiceConfig(serviceHandle, SERVICE_NO_CHANGE, CUInt(mode), SERVICE_NO_CHANGE, Nothing, Nothing,
            IntPtr.Zero, Nothing, Nothing, Nothing, Nothing)

        If result = False Then
            Dim nError As Integer = Marshal.GetLastWin32Error()
            Dim win32Exception = New Win32Exception(nError)
            Throw New ExternalException("Could not change service start type: " + win32Exception.Message)
        End If

        CloseServiceHandle(serviceHandle)
        CloseServiceHandle(scManagerHandle)
    End Sub
    Public Shared Function isServiceDisabled(ByVal serviceName As String) As Boolean
        Try
            Dim fullTrust As New PermissionSet(System.Security.Permissions.PermissionState.Unrestricted)
            fullTrust.Demand()
            Dim wmiQuery As String = "SELECT * FROM Win32_Service WHERE Name='" + serviceName + "'"
            Dim searcher As New ManagementObjectSearcher(wmiQuery)
            Dim results As ManagementObjectCollection = searcher.[Get]
            Dim state As Boolean
            For Each service As ManagementObject In results
                If service("StartMode").ToString() = "Disabled" Then
                    state = True
                Else
                    state = False
                End If
            Next
            Return state
        Catch se As SecurityException
            Return False
            MsgBox("Refused")
        Catch e As Exception
            Return False
            MsgBox("ERROR")
        End Try
    End Function
End Class

