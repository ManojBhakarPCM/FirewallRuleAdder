Imports System.Management
Public Class Win32Process

    Public Property Caption() As String
    Public Property CommandLine() As String
    Public Property Description() As String
    Public Property ExecutablePath() As String
    Public Property Name() As String
    Public Property ParentPID() As UInt32?
    Public Property PID() As UInt32?

    Public Shared Function GetProcesses() As Win32Process()
        Using searcher As New ManagementObjectSearcher("select * from Win32_Process")
            Return (
                From
                    item As ManagementObject
                In
                    searcher.[Get]().Cast(Of ManagementObject)()
                Select New Win32Process() With {
                    .Caption = CType(item.Properties("Caption").Value, String),
                    .CommandLine = CType(item.Properties("CommandLine").Value, String),
                    .Description = CType(item.Properties("Description").Value, String),
                    .ExecutablePath = CType(item.Properties("ExecutablePath").Value, String),
                    .Name = CType(item.Properties("Name").Value, String),
                    .ParentPID = CType(item.Properties("ParentProcessId").Value, UInt32?),
                    .PID = CType(item.Properties("ProcessId").Value, UInt32?)
                }
            ).ToArray()
        End Using
    End Function

End Class