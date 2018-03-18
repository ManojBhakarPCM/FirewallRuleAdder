
Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Linq
Imports System.Net
Imports System.Net.NetworkInformation
Imports System.Net.Sockets
Imports System.Runtime.InteropServices

Namespace IPStats
    Public NotInheritable Class TcpConnection
        Implements INotifyPropertyChanged
        Implements IEquatable(Of TcpConnection)
        Private _process As Process
        Private _row As ValueType

        Public Event PropertyChanged As PropertyChangedEventHandler
        Private Event INotifyPropertyChanged_PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        Private Sub New(row As ValueType)
            _row = row
        End Sub

        Public ReadOnly Property SegmentsOut() As ULong
            Get
                Dim rod As TCP_ESTATS_DATA_ROD_v0
                If Not DataStatsEnabled OrElse Not TryGetRodStats(Of TCP_ESTATS_DATA_ROD_v0)(TCP_ESTATS_TYPE.TcpConnectionEstatsData, rod) Then
                    Return 0
                End If

                Return rod.SegsOut
            End Get
        End Property

        Public ReadOnly Property SegmentsIn() As ULong
            Get
                Dim rod As TCP_ESTATS_DATA_ROD_v0
                If Not DataStatsEnabled OrElse Not TryGetRodStats(Of TCP_ESTATS_DATA_ROD_v0)(TCP_ESTATS_TYPE.TcpConnectionEstatsData, rod) Then
                    Return 0
                End If

                Return rod.SegsIn
            End Get
        End Property

        Public ReadOnly Property DataBytesIn() As ULong
            Get
                Dim rod As TCP_ESTATS_DATA_ROD_v0
                If Not DataStatsEnabled OrElse Not TryGetRodStats(Of TCP_ESTATS_DATA_ROD_v0)(TCP_ESTATS_TYPE.TcpConnectionEstatsData, rod) Then
                    Return 0
                End If

                Return rod.DataBytesIn
            End Get
        End Property

        Public ReadOnly Property DataBytesOut() As ULong
            Get
                Dim rod As TCP_ESTATS_DATA_ROD_v0
                If Not DataStatsEnabled OrElse Not TryGetRodStats(Of TCP_ESTATS_DATA_ROD_v0)(TCP_ESTATS_TYPE.TcpConnectionEstatsData, rod) Then
                    Return 0
                End If

                Return rod.DataBytesOut
            End Get
        End Property

        Public Property DataStatsEnabled() As Boolean
            Get
                Return GetStatsState(TCP_ESTATS_TYPE.TcpConnectionEstatsData, False)
            End Get
            Set
                If Value <> DataStatsEnabled Then
                    EnableStats(TCP_ESTATS_TYPE.TcpConnectionEstatsData, Value)
                    OnPropertyChanged("DataStatsEnabled")
                End If
            End Set
        End Property

        Public ReadOnly Property InboundBandwidth() As ULong
            Get
                Dim rod As TCP_ESTATS_BANDWIDTH_ROD_v0
                If Not InboundBandwidthStatsEnabled OrElse Not TryGetRodStats(Of TCP_ESTATS_BANDWIDTH_ROD_v0)(TCP_ESTATS_TYPE.TcpConnectionEstatsBandwidth, rod) Then
                    Return 0
                End If

                Return rod.InboundBandwidth
            End Get
        End Property

        Public Property InboundBandwidthStatsEnabled() As Boolean
            Get
                Return GetStatsState(TCP_ESTATS_TYPE.TcpConnectionEstatsBandwidth, 2, False)(1) = TCP_BOOLEAN_OPTIONAL.TcpBoolOptEnabled
            End Get
            Set
                If Value <> InboundBandwidthStatsEnabled Then
                    EnableStats(TCP_ESTATS_TYPE.TcpConnectionEstatsBandwidth, New TCP_BOOLEAN_OPTIONAL() {TCP_BOOLEAN_OPTIONAL.TcpBoolOptUnchanged, If(Value, TCP_BOOLEAN_OPTIONAL.TcpBoolOptEnabled, TCP_BOOLEAN_OPTIONAL.TcpBoolOptDisabled)})
                    OnPropertyChanged("InboundBandwidthStatsEnabled")
                End If
            End Set
        End Property

        Public ReadOnly Property OutboundBandwidth() As ULong
            Get
                Dim rod As TCP_ESTATS_BANDWIDTH_ROD_v0
                If Not OutboundBandwidthStatsEnabled OrElse Not TryGetRodStats(Of TCP_ESTATS_BANDWIDTH_ROD_v0)(TCP_ESTATS_TYPE.TcpConnectionEstatsBandwidth, rod) Then
                    Return 0
                End If

                Return rod.OutboundBandwidth
            End Get
        End Property

        Public Property OutboundBandwidthStatsEnabled() As Boolean
            Get
                Return GetStatsState(TCP_ESTATS_TYPE.TcpConnectionEstatsBandwidth, 2, False)(0) = TCP_BOOLEAN_OPTIONAL.TcpBoolOptEnabled
            End Get
            Set
                If Value <> OutboundBandwidthStatsEnabled Then
                    EnableStats(TCP_ESTATS_TYPE.TcpConnectionEstatsBandwidth, New TCP_BOOLEAN_OPTIONAL() {If(Value, TCP_BOOLEAN_OPTIONAL.TcpBoolOptEnabled, TCP_BOOLEAN_OPTIONAL.TcpBoolOptDisabled), TCP_BOOLEAN_OPTIONAL.TcpBoolOptUnchanged})
                    OnPropertyChanged("OutboundBandwidthStatsEnabled")
                End If
            End Set
        End Property

        Public ReadOnly Property Process() As Process
            Get
                If _process Is Nothing AndAlso ProcessId <> 0 Then
                    Try
                        _process = Process.GetProcessById(ProcessId)
                        ' happens...
                    Catch
                    End Try
                End If
                Return _process
            End Get
        End Property

        Public Property ProcessId() As Integer
            Get
                Return m_ProcessId
            End Get
            Private Set
                m_ProcessId = Value
            End Set
        End Property
        Private m_ProcessId As Integer
        Public Property LocalEndPoint() As IPEndPoint
            Get
                Return m_LocalEndPoint
            End Get
            Private Set
                m_LocalEndPoint = Value
            End Set
        End Property
        Private m_LocalEndPoint As IPEndPoint
        Public Property RemoteEndPoint() As IPEndPoint
            Get
                Return m_RemoteEndPoint
            End Get
            Private Set
                m_RemoteEndPoint = Value
            End Set
        End Property
        Private m_RemoteEndPoint As IPEndPoint
        Public Property State() As TcpState
            Get
                Return m_State
            End Get
            Private Set
                m_State = Value
            End Set
        End Property
        Private m_State As TcpState
        Public Property HasChanged() As Boolean
            Get
                Return m_HasChanged
            End Get
            Private Set
                m_HasChanged = Value
            End Set
        End Property
        Private m_HasChanged As Boolean

        Public ReadOnly Property ProtocolVersion() As String
            Get
                Return If(LocalEndPoint.AddressFamily = AddressFamily.InterNetworkV6, "V6", "V4")
            End Get
        End Property

        Private Sub OnPropertyChanged(name As String)
            ' Dim handler As PropertyChanged
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(name))
        End Sub

        Public Overrides Function ToString() As String
            Return ("problem function ToString override is called")
            'Return (State.ToString & ":" & LocalEndPoint.ToString & " -> " & RemoteEndPoint)
        End Function

        Public Overloads Function Equals(other As TcpConnection) As Boolean
            If other Is Nothing Then
                Return False
            End If

            Return State = other.State AndAlso LocalEndPoint.Equals(other.LocalEndPoint) AndAlso RemoteEndPoint.Equals(other.RemoteEndPoint)
        End Function

        Public Overrides Function Equals(obj As Object) As Boolean
            Return Equals(TryCast(obj, TcpConnection))
        End Function

        Public Overrides Function GetHashCode() As Integer
            Return State.GetHashCode() Xor LocalEndPoint.GetHashCode() Xor RemoteEndPoint.GetHashCode()
        End Function

        Public Shared Function GetAll() As IList(Of TcpConnection)
            Return GetAll(False)
        End Function

        Public Shared Function GetAll(observable As Boolean) As IList(Of TcpConnection)
            Dim list As IList(Of TcpConnection) = If(observable, DirectCast(New ObservableCollection(Of TcpConnection)(), IList(Of TcpConnection)), DirectCast(New List(Of TcpConnection)(), IList(Of TcpConnection)))
            If Socket.OSSupportsIPv4 Then
                Add(list, AF_INET)
            End If
            If Socket.OSSupportsIPv6 Then
                Add(list, AF_INET6)
            End If
            Return list
        End Function

        Private Shared Sub Add(list As IList(Of TcpConnection), af As Integer)
            Dim pdwSize As Integer = 0
            Dim hr As Integer = GetExtendedTcpTable(IntPtr.Zero, pdwSize, False, af, TCP_TABLE_CLASS.TCP_TABLE_OWNER_PID_ALL, 0)
            If hr <> ERROR_INSUFFICIENT_BUFFER Then
                Throw New Win32Exception(hr)
            End If

            Dim pTcpTable As IntPtr = Marshal.AllocCoTaskMem(pdwSize)
            Try
                hr = GetExtendedTcpTable(pTcpTable, pdwSize, False, af, TCP_TABLE_CLASS.TCP_TABLE_OWNER_PID_ALL, 0)
                If hr <> 0 Then
                    Throw New Win32Exception(hr)
                End If

                Dim ptr As IntPtr = pTcpTable
                Dim [structure] As Integer = Marshal.ReadInt32(ptr)
                ptr += Marshal.SizeOf([structure])
                For i As Integer = 0 To [structure] - 1
                    Dim connection As TcpConnection
                    If af = AF_INET6 Then
                        Dim entry As MIB_TCP6ROW_OWNER_PID = CType(Marshal.PtrToStructure(ptr, GetType(MIB_TCP6ROW_OWNER_PID)), MIB_TCP6ROW_OWNER_PID)
                        ptr += Marshal.SizeOf(entry)
                        Dim row As New MIB_TCP6ROW() With {
                            .dwLocalScopeId = entry.dwLocalScopeId,
                            .dwRemoteScopeId = entry.dwRemoteScopeId,
                            .localPort1 = entry.localPort1,
                            .localPort2 = entry.localPort2,
                            .localPort3 = entry.localPort3,
                            .localPort4 = entry.localPort4,
                            .remotePort1 = entry.remotePort1,
                            .remotePort2 = entry.remotePort2,
                            .remotePort3 = entry.remotePort3,
                            .remotePort4 = entry.remotePort4,
                            .dwState = entry.dwState,
                            .ucLocalAddr = entry.ucLocalAddr,
                            .ucRemoteAddr = entry.ucRemoteAddr
                        }

                        connection = New TcpConnection(row)
                        connection.ProcessId = entry.dwOwningPid
                        connection.State = entry.dwState
                        connection.LocalEndPoint = New IPEndPoint(New IPAddress(entry.ucLocalAddr, entry.dwLocalScopeId), (entry.localPort1 << 8) Or entry.localPort2)
                        connection.RemoteEndPoint = New IPEndPoint(New IPAddress(entry.ucRemoteAddr, entry.dwRemoteScopeId), (entry.remotePort1 << 8) Or entry.remotePort2)
                    Else
                        Dim entry As MIB_TCPROW_OWNER_PID = CType(Marshal.PtrToStructure(ptr, GetType(MIB_TCPROW_OWNER_PID)), MIB_TCPROW_OWNER_PID)
                        ptr += Marshal.SizeOf(entry)
                        Dim mib_tcprow2 As New MIB_TCPROW() With {
                            .localPort1 = entry.localPort1,
                            .localPort2 = entry.localPort2,
                            .localPort3 = entry.localPort3,
                            .localPort4 = entry.localPort4,
                            .remotePort1 = entry.remotePort1,
                            .remotePort2 = entry.remotePort2,
                            .remotePort3 = entry.remotePort3,
                            .remotePort4 = entry.remotePort4,
                            .dwState = entry.dwState,
                            .dwLocalAddr = entry.dwLocalAddr,
                            .dwRemoteAddr = entry.dwRemoteAddr
                        }

                        connection = New TcpConnection(mib_tcprow2)
                        connection.ProcessId = entry.dwOwningPid
                        connection.State = entry.dwState
                        connection.LocalEndPoint = New IPEndPoint(entry.dwLocalAddr, (entry.localPort1 << 8) Or entry.localPort2)
                        connection.RemoteEndPoint = New IPEndPoint(entry.dwRemoteAddr, (entry.remotePort1 << 8) Or entry.remotePort2)
                    End If
                    list.Add(connection)
                Next
            Finally
                Marshal.FreeCoTaskMem(pTcpTable)
            End Try
        End Sub

        Private Sub EnableStats(type As TCP_ESTATS_TYPE, enable As Boolean)
            Dim ptr As IntPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(_row.[GetType]()))
            Try
                Dim hr As Integer
                Marshal.StructureToPtr(_row, ptr, False)
                If LocalEndPoint.AddressFamily = AddressFamily.InterNetwork Then
                    hr = SetPerTcpConnectionEStats(ptr, type, enable, 0, 1, 0)
                Else
                    hr = SetPerTcp6ConnectionEStats(ptr, type, enable, 0, 1, 0)
                End If

                If hr <> 0 Then
                    Throw New Win32Exception(hr)
                End If
            Finally
                Marshal.FreeCoTaskMem(ptr)
            End Try
        End Sub

        Private Sub EnableStats(type As TCP_ESTATS_TYPE, ParamArray options As TCP_BOOLEAN_OPTIONAL())
            Dim ptr As IntPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(_row.[GetType]()))
            Try
                Dim hr As Integer
                Marshal.StructureToPtr(_row, ptr, False)
                If LocalEndPoint.AddressFamily = AddressFamily.InterNetwork Then
                    hr = SetPerTcpConnectionEStats(ptr, type, options, 0, options.Length * 4, 0)
                Else
                    hr = SetPerTcp6ConnectionEStats(ptr, type, options, 0, options.Length * 4, 0)
                End If

                If hr <> 0 Then
                    Throw New Win32Exception(hr)
                End If
            Finally
                Marshal.FreeCoTaskMem(ptr)
            End Try
        End Sub

        Private Function GetStatsState(type As TCP_ESTATS_TYPE, throwOnError As Boolean) As Boolean
            Dim flag As Boolean
            Dim ptr As IntPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(_row.[GetType]()))
            Try
                Dim hr As Integer
                Dim state As Byte
                Marshal.StructureToPtr(_row, ptr, False)
                If LocalEndPoint.AddressFamily = AddressFamily.InterNetwork Then
                    hr = GetPerTcpConnectionEStats(ptr, type, state, 0, 1, IntPtr.Zero,
                        0, 0, IntPtr.Zero, 0, 0)
                Else
                    hr = GetPerTcp6ConnectionEStats(ptr, type, state, 0, 1, IntPtr.Zero,
                        0, 0, IntPtr.Zero, 0, 0)
                End If

                If hr <> 0 AndAlso throwOnError Then
                    Throw New Win32Exception(hr)
                End If

                flag = state <> 0
            Finally
                Marshal.FreeCoTaskMem(ptr)
            End Try
            Return flag
        End Function

        Private Function GetStatsState(type As TCP_ESTATS_TYPE, count As Integer, throwOnError As Boolean) As TCP_BOOLEAN_OPTIONAL()
            Dim ptr As IntPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(_row.[GetType]()))
            Try
                Dim hr As Integer
                Marshal.StructureToPtr(_row, ptr, False)
                Dim options As TCP_BOOLEAN_OPTIONAL() = New TCP_BOOLEAN_OPTIONAL(count - 1) {}
                For i As Integer = 0 To options.Length - 1
                    options(i) = TCP_BOOLEAN_OPTIONAL.TcpBoolOptUnchanged
                Next

                If LocalEndPoint.AddressFamily = AddressFamily.InterNetwork Then
                    hr = GetPerTcpConnectionEStats(ptr, type, options, 0, count * 4, IntPtr.Zero,
                        0, 0, IntPtr.Zero, 0, 0)
                Else
                    hr = GetPerTcp6ConnectionEStats(ptr, type, options, 0, count * 4, IntPtr.Zero,
                        0, 0, IntPtr.Zero, 0, 0)
                End If

                If hr <> 0 AndAlso throwOnError Then
                    Throw New Win32Exception(hr)
                End If

                Return options
            Finally
                Marshal.FreeCoTaskMem(ptr)
            End Try
        End Function

        Private Function TryGetRodStats(Of T)(type As TCP_ESTATS_TYPE, ByRef value As T) As Boolean
            Dim rod As IntPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(GetType(T)))
            Try
                Dim ptr As IntPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(_row.[GetType]()))
                Try
                    Dim hr As Integer
                    Marshal.StructureToPtr(_row, ptr, False)
                    If LocalEndPoint.AddressFamily = AddressFamily.InterNetwork Then
                        hr = GetPerTcpConnectionEStats(ptr, type, IntPtr.Zero, 0, 0, IntPtr.Zero,
                            0, 0, rod, 0, Marshal.SizeOf(GetType(T)))
                    Else
                        hr = GetPerTcp6ConnectionEStats(ptr, type, IntPtr.Zero, 0, 0, IntPtr.Zero,
                            0, 0, rod, 0, Marshal.SizeOf(GetType(T)))
                    End If

                    If hr <> 0 Then
                        value = Nothing
                        Return False
                    End If

                    value = DirectCast(Marshal.PtrToStructure(rod, GetType(T)), T)
                    Return True
                Finally
                    Marshal.FreeCoTaskMem(ptr)
                End Try
            Finally
                Marshal.FreeCoTaskMem(rod)
            End Try
        End Function

        Private Sub Update()
            If DataStatsEnabled Then
                OnPropertyChanged("DataBytesIn")
                OnPropertyChanged("DataBytesOut")
                OnPropertyChanged("SegmentsIn")
                OnPropertyChanged("SegmentsOut")
            End If
            If InboundBandwidthStatsEnabled Then
                OnPropertyChanged("InboundBandwidth")
            End If
            If OutboundBandwidthStatsEnabled Then
                OnPropertyChanged("OutboundBandwidth")
            End If
        End Sub

        Public Shared Sub Update(list As IList(Of TcpConnection))
            If list Is Nothing Then
                Throw New ArgumentNullException("list")
            End If

            Dim all As List(Of TcpConnection) = DirectCast(GetAll(False), List(Of TcpConnection))
            Dim removed As New List(Of TcpConnection)(list)
            For Each cnx As Object In list
                Dim item As TcpConnection = list.FirstOrDefault '(Of TcpConnection)'(Function(c) c.Equals(cnx))
                If item IsNot Nothing Then
                    item.Update()
                    removed.Remove(item)
                Else
                    list.Add(cnx)
                End If
            Next

            For Each cnx As Object In removed
                list.Remove(cnx)
            Next
        End Sub

        <DllImport("iphlpapi.dll")>
        Private Shared Function GetExtendedTcpTable(pTcpTable As IntPtr, ByRef pdwSize As Integer, bOrder As Boolean, ulAf As Integer, TableClass As TCP_TABLE_CLASS, Reserved As Integer) As Integer
        End Function
        <DllImport("iphlpapi.dll")>
        Private Shared Function GetPerTcp6ConnectionEStats(Row As IntPtr, EstatsType As TCP_ESTATS_TYPE, Rw As TCP_BOOLEAN_OPTIONAL(), RwVersion As Integer, RwSize As Integer, Ros As IntPtr,
            RosVersion As Integer, RosSize As Integer, Rod As IntPtr, RodVersion As Integer, RodSize As Integer) As Integer
        End Function
        <DllImport("iphlpapi.dll")>
        Private Shared Function GetPerTcp6ConnectionEStats(Row As IntPtr, EstatsType As TCP_ESTATS_TYPE, ByRef Rw As Byte, RwVersion As Integer, RwSize As Integer, Ros As IntPtr,
            RosVersion As Integer, RosSize As Integer, Rod As IntPtr, RodVersion As Integer, RodSize As Integer) As Integer
        End Function
        <DllImport("iphlpapi.dll")>
        Private Shared Function GetPerTcp6ConnectionEStats(Row As IntPtr, EstatsType As TCP_ESTATS_TYPE, Rw As IntPtr, RwVersion As Integer, RwSize As Integer, Ros As IntPtr,
            RosVersion As Integer, RosSize As Integer, Rod As IntPtr, RodVersion As Integer, RodSize As Integer) As Integer
        End Function
        <DllImport("iphlpapi.dll")>
        Private Shared Function GetPerTcpConnectionEStats(Row As IntPtr, EstatsType As TCP_ESTATS_TYPE, Rw As TCP_BOOLEAN_OPTIONAL(), RwVersion As Integer, RwSize As Integer, Ros As IntPtr,
            RosVersion As Integer, RosSize As Integer, Rod As IntPtr, RodVersion As Integer, RodSize As Integer) As Integer
        End Function
        <DllImport("iphlpapi.dll")>
        Private Shared Function GetPerTcpConnectionEStats(Row As IntPtr, EstatsType As TCP_ESTATS_TYPE, ByRef Rw As Byte, RwVersion As Integer, RwSize As Integer, Ros As IntPtr,
            RosVersion As Integer, RosSize As Integer, Rod As IntPtr, RodVersion As Integer, RodSize As Integer) As Integer
        End Function
        <DllImport("iphlpapi.dll")>
        Private Shared Function GetPerTcpConnectionEStats(Row As IntPtr, EstatsType As TCP_ESTATS_TYPE, Rw As IntPtr, RwVersion As Integer, RwSize As Integer, Ros As IntPtr,
            RosVersion As Integer, RosSize As Integer, Rod As IntPtr, RodVersion As Integer, RodSize As Integer) As Integer
        End Function
        <DllImport("iphlpapi.dll")>
        Private Shared Function SetPerTcp6ConnectionEStats(Row As IntPtr, EstatsType As TCP_ESTATS_TYPE, Rw As TCP_BOOLEAN_OPTIONAL(), RwVersion As Integer, RwSize As Integer, Offset As Integer) As Integer
        End Function
        <DllImport("iphlpapi.dll")>
        Private Shared Function SetPerTcp6ConnectionEStats(Row As IntPtr, EstatsType As TCP_ESTATS_TYPE, ByRef Rw As Boolean, RwVersion As Integer, RwSize As Integer, Offset As Integer) As Integer
        End Function
        <DllImport("iphlpapi.dll")>
        Private Shared Function SetPerTcpConnectionEStats(Row As IntPtr, EstatsType As TCP_ESTATS_TYPE, Rw As TCP_BOOLEAN_OPTIONAL(), RwVersion As Integer, RwSize As Integer, Offset As Integer) As Integer
        End Function
        <DllImport("iphlpapi.dll")>
        Private Shared Function SetPerTcpConnectionEStats(Row As IntPtr, EstatsType As TCP_ESTATS_TYPE, ByRef Rw As Boolean, RwVersion As Integer, RwSize As Integer, Offset As Integer) As Integer
        End Function

        Private Function IEquatable_Equals(other As TcpConnection) As Boolean Implements IEquatable(Of TcpConnection).Equals
            Throw New NotImplementedException()
        End Function

        Private Const AF_INET As Integer = 2
        Private Const AF_INET6 As Integer = 23
        Private Const ERROR_INSUFFICIENT_BUFFER As Integer = 122

        <StructLayout(LayoutKind.Sequential)>
        Private Structure MIB_TCP6ROW
            Public dwState As TcpState
            <MarshalAs(UnmanagedType.ByValArray, SizeConst:=16)>
            Public ucLocalAddr As Byte()
            Public dwLocalScopeId As Integer
            Public localPort1 As Byte
            Public localPort2 As Byte
            Public localPort3 As Byte
            Public localPort4 As Byte
            <MarshalAs(UnmanagedType.ByValArray, SizeConst:=16)>
            Public ucRemoteAddr As Byte()
            Public dwRemoteScopeId As Integer
            Public remotePort1 As Byte
            Public remotePort2 As Byte
            Public remotePort3 As Byte
            Public remotePort4 As Byte
        End Structure

        <StructLayout(LayoutKind.Sequential)>
        Private Structure MIB_TCP6ROW_OWNER_PID
            <MarshalAs(UnmanagedType.ByValArray, SizeConst:=16)>
            Public ucLocalAddr As Byte()
            Public dwLocalScopeId As Integer
            Public localPort1 As Byte
            Public localPort2 As Byte
            Public localPort3 As Byte
            Public localPort4 As Byte
            <MarshalAs(UnmanagedType.ByValArray, SizeConst:=16)>
            Public ucRemoteAddr As Byte()
            Public dwRemoteScopeId As Integer
            Public remotePort1 As Byte
            Public remotePort2 As Byte
            Public remotePort3 As Byte
            Public remotePort4 As Byte
            Public dwState As TcpState
            Public dwOwningPid As Integer
        End Structure

        <StructLayout(LayoutKind.Sequential)>
        Private Structure MIB_TCPROW
            Public dwState As TcpState
            Public dwLocalAddr As UInteger
            Public localPort1 As Byte
            Public localPort2 As Byte
            Public localPort3 As Byte
            Public localPort4 As Byte
            Public dwRemoteAddr As UInteger
            Public remotePort1 As Byte
            Public remotePort2 As Byte
            Public remotePort3 As Byte
            Public remotePort4 As Byte
        End Structure

        <StructLayout(LayoutKind.Sequential)>
        Private Structure MIB_TCPROW_OWNER_PID
            Public dwState As TcpState
            Public dwLocalAddr As UInteger
            Public localPort1 As Byte
            Public localPort2 As Byte
            Public localPort3 As Byte
            Public localPort4 As Byte
            Public dwRemoteAddr As UInteger
            Public remotePort1 As Byte
            Public remotePort2 As Byte
            Public remotePort3 As Byte
            Public remotePort4 As Byte
            Public dwOwningPid As Integer
        End Structure

        Private Enum TCP_BOOLEAN_OPTIONAL
            TcpBoolOptDisabled = 0
            TcpBoolOptEnabled = 1
            TcpBoolOptUnchanged = -1
        End Enum

        <StructLayout(LayoutKind.Sequential)>
        Private Structure TCP_ESTATS_BANDWIDTH_ROD_v0
            Public OutboundBandwidth As ULong
            Public InboundBandwidth As ULong
            Public OutboundInstability As ULong
            Public InboundInstability As ULong
            Public OutboundBandwidthPeaked As Boolean
            Public InboundBandwidthPeaked As Boolean
        End Structure

        <StructLayout(LayoutKind.Sequential)>
        Private Structure TCP_ESTATS_DATA_ROD_v0
            Public DataBytesOut As ULong
            Public DataSegsOut As ULong
            Public DataBytesIn As ULong
            Public DataSegsIn As ULong
            Public SegsOut As ULong
            Public SegsIn As ULong
            Public SoftErrors As UInteger
            Public SoftErrorReason As UInteger
            Public SndUna As UInteger
            Public SndNxt As UInteger
            Public SndMax As UInteger
            Public ThruBytesAcked As ULong
            Public RcvNxt As UInteger
            Public ThruBytesReceived As ULong
        End Structure

        Private Enum TCP_ESTATS_TYPE
            TcpConnectionEstatsSynOpts
            TcpConnectionEstatsData
            TcpConnectionEstatsSndCong
            TcpConnectionEstatsPath
            TcpConnectionEstatsSendBuff
            TcpConnectionEstatsRec
            TcpConnectionEstatsObsRec
            TcpConnectionEstatsBandwidth
            TcpConnectionEstatsFineRtt
            TcpConnectionEstatsMaximum
        End Enum

        Private Enum TCP_TABLE_CLASS
            TCP_TABLE_BASIC_LISTENER
            TCP_TABLE_BASIC_CONNECTIONS
            TCP_TABLE_BASIC_ALL
            TCP_TABLE_OWNER_PID_LISTENER
            TCP_TABLE_OWNER_PID_CONNECTIONS
            TCP_TABLE_OWNER_PID_ALL
            TCP_TABLE_OWNER_MODULE_LISTENER
            TCP_TABLE_OWNER_MODULE_CONNECTIONS
            TCP_TABLE_OWNER_MODULE_ALL
        End Enum
    End Class
End Namespace

'=======================================================
'Service provided by Telerik (www.telerik.com)
'Conversion powered by NRefactory.
'Twitter: @telerik
'Facebook: facebook.com/telerik
'=======================================================

