Imports System.Runtime.InteropServices
Imports System.Net.NetworkInformation
Imports System.Net

Public Module TCPGet
    Declare Auto Function GetExtendedTcpTable Lib "iphlpapi.dll" (ByVal pTCPTable As IntPtr, ByRef OutLen As Integer, ByVal Sort As Boolean, ByVal IpVersion As Integer, ByVal dwClass As Integer, ByVal Reserved As Integer) As Integer
    <DllImport("iphlpapi.dll")>
    Private Function SetTcpEntry(ByVal pTcprow As IntPtr) As Integer
    End Function
    <DllImport("iphlpapi.dll")>
    Private Function GetTcpTable(ByVal pTcpTable As IntPtr, ByRef pdwSize As Integer, ByVal bOrder As Boolean) As Integer
    End Function
    'Convert 16-bit value from network to host byte order
    <DllImport("wsock32.dll")>
    Private Function ntohs(ByVal netshort As Integer) As Integer
    End Function
    'Convert 16-bit value back again
    <DllImport("wsock32.dll")>
    Private Function htons(ByVal netshort As Integer) As Integer
    End Function
    Const TCP_TABLE_OWNER_PID_ALL As Integer = 5
    <StructLayout(LayoutKind.Sequential)>
    Public Structure MIB_TCPTABLE_OWNER_PID
        Public NumberOfEntries As Integer 'number of rows
        Public Table As IntPtr 'array of tables
    End Structure
    <StructLayout(LayoutKind.Sequential)>
    Public Structure MIB_TCPROW_OWNER_PID
        Public state As Integer 'state of the connection
        Public localAddress As Integer
        Public LocalPort As Integer
        Public RemoteAddress As UInteger
        Public remotePort As Integer
        Public PID As Integer 'Process ID
    End Structure
    Public Structure MIB_TCP6ROW_OWNER_PID
        'UCHAR ucLocalAddr[16];
        'DWORD dwLocalScopeId;
        'DWORD dwLocalPort;
        'UCHAR ucRemoteAddr[16];
        'DWORD dwRemoteScopeId;
        'DWORD dwRemotePort;
        'DWORD dwState;
        'DWORD dwOwningPid;
        Public LocalAddr As Integer
        Public LocalAddr2 As Integer
        Public LocalAddr3 As Integer
        Public LocalAddr4 As Integer
        Public LocalScopeId As Integer
        Public LocalPort As Integer
        Public remoteAddr As Integer
        Public remoteAddr2 As Integer
        Public remoteAddr3 As Integer
        Public remoteAddr4 As Integer
        Public remoteScopeID As Integer
        Public RemotePort As Integer
        Public state As Integer
        Public owningPID As Integer
    End Structure
    Private Structure MIB_TCPROW
        Public dwState As Integer
        Public dwLocalAddr As Integer
        Public dwLocalPort As Integer
        Public dwRemoteAddr As Integer
        Public dwRemotePort As Integer
    End Structure
    Structure TcpConnection
        Public State As TcpState
        Public localAddress As String
        Public RemoteAddress As String
        Public Proc As String
        Public PID As Integer
        Public ProcPath As String
    End Structure
    Public Enum State
        All = 0
        Closed = 1
        Listen = 2
        Syn_Sent = 3
        Syn_Rcvd = 4
        Established = 5
        Fin_Wait1 = 6
        Fin_Wait2 = 7
        Close_Wait = 8
        Closing = 9
        Last_Ack = 10
        Time_Wait = 11
        Delete_TCB = 12
    End Enum
    'Close all connection to the remote IP
    Public Sub CloseRemoteIP(ByVal IP As String)
        Dim rows As MIB_TCPROW() = getTcpTable()
        For i As Integer = 0 To rows.Length - 1
            If rows(i).dwRemoteAddr = IPStringToInt(IP) Then
                rows(i).dwState = CInt(State.Delete_TCB)
                Dim ptr As IntPtr = GetPtrToNewObject(rows(i))
                Dim ret As Integer = SetTcpEntry(ptr)
            End If
        Next
    End Sub
    'Close all connections at current local IP
    Public Sub CloseLocalIP(ByVal IP As String)
        Dim rows As MIB_TCPROW() = getTcpTable()
        For i As Integer = 0 To rows.Length - 1
            If rows(i).dwLocalAddr = IPStringToInt(IP) Then
                rows(i).dwState = CInt(State.Delete_TCB)
                Dim ptr As IntPtr = GetPtrToNewObject(rows(i))
                Dim ret As Integer = SetTcpEntry(ptr)
            End If
        Next
    End Sub
    'Closes all connections to the remote port
    Public Sub CloseRemotePort(ByVal port As Integer)
        Dim rows As MIB_TCPROW() = getTcpTable()
        For i As Integer = 0 To rows.Length - 1
            If port = ntohs(rows(i).dwRemotePort) Then
                rows(i).dwState = CInt(State.Delete_TCB)
                Dim ptr As IntPtr = GetPtrToNewObject(rows(i))
                Dim ret As Integer = SetTcpEntry(ptr)
            End If
        Next
    End Sub
    'Closes all connections to the local port
    Public Sub CloseLocalPort(ByVal port As Integer)
        Dim rows As MIB_TCPROW() = getTcpTable()
        For i As Integer = 0 To rows.Length - 1
            If port = ntohs(rows(i).dwLocalPort) Then
                rows(i).dwState = CInt(State.Delete_TCB)
                Dim ptr As IntPtr = GetPtrToNewObject(rows(i))
                Dim ret As Integer = SetTcpEntry(ptr)
            End If
        Next
    End Sub
    'Close a connection by returning the connectionstring
    Public Sub CloseConnection(ByVal connectionstring As String)
        Try
            'Split the string to its subparts
            Dim parts As String() = connectionstring.Split("-"c)
            If parts.Length <> 4 Then
                Throw New Exception("Invalid connectionstring - use the one provided by Connections.")
            End If
            Dim loc As String() = parts(0).Split(":"c)
            Dim [rem] As String() = parts(1).Split(":"c)
            Dim locaddr As String() = loc(0).Split("."c)
            Dim remaddr As String() = [rem](0).Split("."c)
            'Fill structure with data
            Dim row As New MIB_TCPROW()
            row.dwState = 12
            Dim bLocAddr As Byte() = New Byte() {Byte.Parse(locaddr(0)), Byte.Parse(locaddr(1)), Byte.Parse(locaddr(2)), Byte.Parse(locaddr(3))}
            Dim bRemAddr As Byte() = New Byte() {Byte.Parse(remaddr(0)), Byte.Parse(remaddr(1)), Byte.Parse(remaddr(2)), Byte.Parse(remaddr(3))}
            row.dwLocalAddr = BitConverter.ToInt32(bLocAddr, 0)
            row.dwRemoteAddr = BitConverter.ToInt32(bRemAddr, 0)
            row.dwLocalPort = htons(Integer.Parse(loc(1)))
            row.dwRemotePort = htons(Integer.Parse([rem](1)))
            'Make copy of the structure into memory and use the pointer to call SetTcpEntry
            Dim ptr As IntPtr = GetPtrToNewObject(row)
            Dim ret As Integer = SetTcpEntry(ptr)
            If ret = -1 Then
                Throw New Exception("Unsuccessful")
            End If
            If ret = 65 Then
                Throw New Exception("User has no sufficient privilege to execute this API successfully")
            End If
            If ret = 87 Then
                Throw New Exception("Specified port is not in state to be closed down")
            End If
            If ret <> 0 Then
                Throw New Exception("Unknown error (" & ret & ")")
            End If
        Catch ex As Exception
            Throw New Exception((("CloseConnection failed (" & connectionstring & ")! [") + ex.[GetType]().ToString() & ",") + ex.Message & "]")
        End Try
    End Sub
    'Gets all connections
    Public Function Connections() As String()
        Return Connections(State.All)
    End Function
    'Gets a connection list of connections with a defined state
    Public Function Connections(ByVal state__1 As State) As String()
        Dim rows As MIB_TCPROW() = getTcpTable()

        Dim arr As New ArrayList()

        For Each row As MIB_TCPROW In rows
            If state__1 = State.All OrElse state__1 = DirectCast(row.dwState, State) Then
                Dim localaddress As String = (IPIntToString(row.dwLocalAddr) & ":") + ntohs(row.dwLocalPort)
                Dim remoteaddress As String = (IPIntToString(row.dwRemoteAddr) & ":") + ntohs(row.dwRemotePort)
                arr.Add((((localaddress & "-") + remoteaddress & "-") + DirectCast(row.dwState, State).ToString() & "-") + row.dwState)
            End If
        Next

        Return DirectCast(arr.ToArray(GetType(System.String)), String())
    End Function
    'The function that fills the MIB_TCPROW array with connectioninfos
    Private Function getTcpTable() As MIB_TCPROW()
        Dim pdwSize As Integer
        Dim iRetVal As Integer
        Dim i As Integer
        Dim TcpTableRow As MIB_TCPROW
        Dim pStructPointer As IntPtr = IntPtr.Zero
        Dim iNumberOfStructures As Integer

        '------
        iRetVal = GetTcpTable(pStructPointer, pdwSize, 0)

        pStructPointer = Marshal.AllocHGlobal(pdwSize)
        iRetVal = GetTcpTable(pStructPointer, pdwSize, 0)
        iNumberOfStructures = Math.Ceiling((pdwSize - 4) / Marshal.SizeOf(GetType(MIB_TCPROW)))


        Dim TCPTable(iNumberOfStructures - 1) As MIB_TCPROW
        For i = 0 To iNumberOfStructures - 1
            Dim pStructPointerTemp As IntPtr = New IntPtr(pStructPointer.ToInt32() + 4 + (i * Marshal.SizeOf(GetType(MIB_TCPROW))))

            TcpTableRow = New MIB_TCPROW()
            With TcpTableRow
                .dwLocalAddr = 0
                .dwState = 0
                .dwLocalPort = 0
                .dwRemoteAddr = 0
                .dwRemotePort = 0
            End With
            'Marshal.PtrToStructure(pStructPointerTemp, TcpTableRow)
            TcpTableRow = CType(Marshal.PtrToStructure(pStructPointerTemp, GetType(MIB_TCPROW)), MIB_TCPROW)
            ' Process each MIB_TCPROW here
            TCPTable(i) = TcpTableRow
        Next

        Marshal.FreeHGlobal(pStructPointer)
        Return TCPTable
    End Function
    Private Function GetPtrToNewObject(ByVal obj As Object) As IntPtr
        Dim ptr As IntPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(obj))
        Marshal.StructureToPtr(obj, ptr, False)
        Return ptr
    End Function
    'Convert an IP string to the INT value
    Private Function IPStringToInt(ByVal IP As String) As Integer
        If IP.IndexOf(".") < 0 Then
            Throw New Exception("Invalid IP address")
        End If
        Dim addr As String() = IP.Split("."c)
        If addr.Length <> 4 Then
            Throw New Exception("Invalid IP address")
        End If
        Dim bytes As Byte() = New Byte() {Byte.Parse(addr(0)), Byte.Parse(addr(1)), Byte.Parse(addr(2)), Byte.Parse(addr(3))}
        Return BitConverter.ToInt32(bytes, 0)
    End Function
    'Convert an IP integer to IP string
    Private Function IPIntToString(ByVal IP As Integer) As String
        Dim addr As Byte() = System.BitConverter.GetBytes(IP)
        Return (((addr(0) & ".") + addr(1) & ".") + addr(2) & ".") + addr(3)
    End Function
    'Main Function
    Function GetAllTCPConnections() As MIB_TCPROW_OWNER_PID()
        GetAllTCPConnections = Nothing
        Dim cb As Integer
        GetExtendedTcpTable(Nothing, cb, False, 2, 5, 0)
        Dim tcptable As IntPtr = Marshal.AllocHGlobal(cb)
        If GetExtendedTcpTable(tcptable, cb, False, 2, 5, 0) = 0 Then
            Dim tab As MIB_TCPTABLE_OWNER_PID = Marshal.PtrToStructure(tcptable, GetType(MIB_TCPTABLE_OWNER_PID))
            Dim Mibs(tab.NumberOfEntries - 1) As MIB_TCPROW_OWNER_PID
            Dim row As IntPtr
            For i As Integer = 0 To tab.NumberOfEntries - 1
                row = New IntPtr(tcptable.ToInt32 + Marshal.SizeOf(tab.NumberOfEntries) + Marshal.SizeOf(GetType(MIB_TCPROW_OWNER_PID)) * i)
                Mibs(i) = Marshal.PtrToStructure(row, GetType(MIB_TCPROW_OWNER_PID))
            Next
            GetAllTCPConnections = Mibs
        End If
        Marshal.FreeHGlobal(tcptable)
    End Function
    Function GetAllTCP6Connections() As MIB_TCP6ROW_OWNER_PID()
        GetAllTCP6Connections = Nothing
        Dim cb As Integer
        GetExtendedTcpTable(Nothing, cb, False, 23, 5, 0)
        Dim tcptable As IntPtr = Marshal.AllocHGlobal(cb)
        If GetExtendedTcpTable(tcptable, cb, False, 23, 5, 0) = 0 Then
            Dim tab As MIB_TCPTABLE_OWNER_PID = Marshal.PtrToStructure(tcptable, GetType(MIB_TCPTABLE_OWNER_PID))
            Dim Mibs(tab.NumberOfEntries - 1) As MIB_TCP6ROW_OWNER_PID
            Dim row As IntPtr
            For i As Integer = 0 To tab.NumberOfEntries - 1
                row = New IntPtr(tcptable.ToInt32 + Marshal.SizeOf(tab.NumberOfEntries) + Marshal.SizeOf(GetType(MIB_TCP6ROW_OWNER_PID)) * i)
                Mibs(i) = Marshal.PtrToStructure(row, GetType(MIB_TCP6ROW_OWNER_PID))
                'If Mibs(i).remoteAddr = 0 AndAlso Mibs(i).remoteAddr2 = 0 AndAlso Mibs(i).remoteAddr3 = 0 Then GoTo LB

                'Debug.Print("---" & i & "---------")
                'On Error Resume Next
                'Debug.Print("LocalAddr:" & IntegersToIPv6String(Mibs(i).LocalAddr, Mibs(i).LocalAddr2, Mibs(i).LocalAddr3, Mibs(i).LocalAddr4))
                'Debug.Print("LocalScope:" & Mibs(i).LocalScopeId)
                'Debug.Print("LocalPort:" & Mibs(i).LocalPort)
                'Debug.Print("remoteAddr:" & IntegersToIPv6String(Mibs(i).remoteAddr, Mibs(i).remoteAddr2, Mibs(i).remoteAddr3, Mibs(i).remoteAddr4))
                'Debug.Print("RemoteScope:" & Mibs(i).remoteScopeID)
                'Debug.Print("RemotePort:" & ((Mibs(i).RemotePort - (Mibs(i).RemotePort Mod 256)) / 256 + (Mibs(i).RemotePort Mod 256) * 256))
                'Debug.Print("State:" & Mibs(i).state)
                'Debug.Print("OwningPID:" & Mibs(i).owningPID)
LB:
            Next
            GetAllTCP6Connections = Mibs
        End If
        Marshal.FreeHGlobal(tcptable)
    End Function
    Function MIB_ROW_To_TCP(ByVal row As MIB_TCPROW_OWNER_PID) As TcpConnection
        On Error Resume Next
        Dim tcp As New TcpConnection
        Dim port As Integer
        Dim ports As String

        'tcp.State = DirectCast(row.state, TcpState)
        'Dim ipad As New IPAddress(row.localAddress)
        'port = (row.LocalPort / 256 + (row.LocalPort Mod 256) * 256)
        'tcp.localAddress = ipad.ToString & ":" & port.ToString
        Dim ipad As New IPAddress(row.RemoteAddress)
        port = (row.remotePort / 256 + (row.remotePort Mod 256) * 256)
        If port = 443 Then
            ports = "https://"
        ElseIf port = 80 Then
            ports = "http://"
        End If
        tcp.RemoteAddress = String.Concat(ports, ipad.ToString)
        Dim p As Process = Process.GetProcessById(row.PID)
        tcp.Proc = p.ProcessName
        tcp.PID = row.PID
        'tcp.ProcPath = p.Threads.Count.ToString
        p.Dispose()
        'done.
        Return tcp
    End Function
    Function MIB_IPv6ROW_TO_TCP(ByVal row As MIB_TCP6ROW_OWNER_PID) As TcpConnection
        Dim tcp As New TcpConnection
        tcp.PID = row.owningPID
        tcp.RemoteAddress = IntegersToIPv6String(row.remoteAddr, row.remoteAddr2, row.remoteAddr3, row.remoteAddr4)
        Return tcp
    End Function
    Public Function IntegersToIPv6String(ByRef int1 As Integer, ByRef int2 As Integer, ByRef int3 As Integer, ByRef int4 As Integer) As String
        Dim outt As String
        Dim mergedArray(16) As Byte
        Dim ba1() As Byte = BitConverter.GetBytes(int1)
        outt &= String.Concat(Hex(ba1(0)), ba1(1).ToString("X2"), ":", Hex(ba1(2)), ba1(3).ToString("X2"), ":")
        Dim ba2() As Byte = BitConverter.GetBytes(int2)
        outt &= String.Concat(Hex(ba2(0)), ba2(1).ToString("X2"), ":", Hex(ba2(2)), ba2(3).ToString("X2"), ":")
        Dim ba3() As Byte = BitConverter.GetBytes(int3)
        outt &= String.Concat(Hex(ba3(0)), ba3(1).ToString("X2"), ":", Hex(ba3(2)), ba3(3).ToString("X2"), ":")
        Dim ba4() As Byte = BitConverter.GetBytes(int4)
        outt &= String.Concat(Hex(ba4(0)), ba4(1).ToString("X2"), ":", Hex(ba4(2)), ba4(3).ToString("X2"))
        Return outt.ToLower
    End Function
End Module
