Imports System.ComponentModel
Imports System.IO
Imports System.Net
Imports System.Reflection
Imports System.ServiceProcess

Public Class Form3
    Structure ConnectedAppList
        Dim AppPath As String
        Dim PID As Integer
        Dim RemoteAddr As String
        Dim stat As Integer
    End Structure
    Dim NewList As New List(Of ConnectedAppList)
    Dim BAcount As Integer
    Dim isNetAvailable As Boolean = False
    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ListView1.Columns.Add("Process", 300)
        ListView1.Columns.Add("IP of Website", 150)
        ListView1.Columns.Add("PID", 50)






        Handle_NetworkAvailabilityChanged()
        MakeSureServiceRunning()
        RefreshFWState()
        ListView1.Items.Clear()
        BackgroundWorker1.RunWorkerAsync()
        'RefreshConnections()
        Timer1.Enabled = True

    End Sub
    Private Sub RefreshFWState()
        For Each s As ServiceController In ServiceController.GetServices()
            If s.ServiceName = "MpsSvc" Then
                Select Case s.Status
                    Case ServiceControllerStatus.Running
                        picFWState.Image = My.Resources.green
                    Case ServiceControllerStatus.StartPending
                        picFWState.Image = My.Resources.yellow
                    Case ServiceControllerStatus.Paused
                        picFWState.Image = My.Resources.yellow
                    Case ServiceControllerStatus.PausePending
                        picFWState.Image = My.Resources.yellow
                    Case ServiceControllerStatus.Stopped
                        picFWState.Image = My.Resources.red
                    Case ServiceControllerStatus.StopPending
                        picFWState.Image = My.Resources.red
                    Case ServiceControllerStatus.ContinuePending
                        picFWState.Image = My.Resources.yellow
                End Select
                Exit Sub
            End If
        Next
        picFWState.Image = My.Resources.red
        MsgBox("Windows Firewall is Uninstalled from this computer. I never knew it can also be done! Wow!")
    End Sub
    Private Sub DisplayAvailability(ByVal available As Boolean)
        isNetAvailable = available
        RefreshNetState()
    End Sub

    Private Sub MyComputerNetwork_NetworkAvailabilityChanged(
    ByVal sender As Object,
    ByVal e As Devices.NetworkAvailableEventArgs)

        DisplayAvailability(e.IsNetworkAvailable)
    End Sub

    Private Sub Handle_NetworkAvailabilityChanged()
        AddHandler My.Computer.Network.NetworkAvailabilityChanged,
       AddressOf MyComputerNetwork_NetworkAvailabilityChanged
        DisplayAvailability(My.Computer.Network.IsAvailable)
    End Sub
    Private Sub RefreshNetState()
        If isNetAvailable = False Then
            picNetState.Image = My.Resources.red
            Exit Sub
        End If
        If CheckForInternetConnection() Then
            picNetState.Image = My.Resources.green
        Else
            picNetState.Image = My.Resources.red
        End If
    End Sub
    Private Sub RefreshConnections()
        Dim procPath As String
        Dim itm As New ConnectedAppList
        NewList.Clear()
        Dim procs As Win32Process() = Win32Process.GetProcesses()
        For Each Row In GetAllTCPConnections()
            procPath = GetProcPathByPID(procs, Row.PID)
            Dim ipad As String = New IPAddress(Row.RemoteAddress).ToString
            If (ipad.Contains("0.0.0.0")) OrElse (ipad.Contains("127.0.0.1")) OrElse procPath = "Idle" Then Continue For 'skipping non internets
            itm.AppPath = procPath
            itm.PID = Row.PID
            itm.RemoteAddr = ipad
            itm.stat = Row.state
            NewList.Add(itm)
        Next
        Dim port As Integer
        For Each row In GetAllTCP6Connections()
            port = ((row.RemotePort - (row.RemotePort Mod 256)) / 256 + (row.RemotePort Mod 256) * 256)
            If port = 443 OrElse port = 80 Then
                itm.AppPath = GetProcPathByPID(procs, row.owningPID)
                itm.PID = row.owningPID
                itm.RemoteAddr = IntegersToIPv6String(row.remoteAddr, row.remoteAddr2, row.remoteAddr3, row.remoteAddr4)
                itm.stat = row.state
                NewList.Add(itm)
            End If
        Next
    End Sub

    Private Sub refreshListView()
        Dim found As Boolean
        Dim i As Integer, j As Integer
        ListView1.BeginUpdate()

        For i = ListView1.Items.Count - 1 To 0 Step -1
            For j = NewList.Count - 1 To 0 Step -1
                If ListView1.Items(i).SubItems(2).Text = NewList(j).PID Then
                    found = True
                    Exit For
                End If
            Next
            If Not found Then
                ListView1.Items.RemoveAt(i)
            Else
                NewList.RemoveAt(j)
            End If
            found = False
        Next


        For Each itm In NewList
            LvdisplayPathWithIcon(ListView1, ImageList1, itm.AppPath, IO.Path.GetFileName(itm.AppPath))
            If ListView1.Items.Count > 0 Then
                If itm.AppPath Is Nothing Then itm.AppPath = "*"
                ListView1.Items(ListView1.Items.Count - 1).Tag = itm.AppPath
                ListView1.Items(ListView1.Items.Count - 1).ToolTipText = itm.AppPath
                ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(itm.RemoteAddr)
                ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(itm.PID)
            End If
        Next
        'sorting the listview
        'ListView1.Sort()
        'https://www.ultratools.com/tools/ipv6InfoResult?ipAddress=2404:A680:A403:c01:000:000:000:0bc
        Label2.Text = "Total Connections : " & ListView1.Items.Count
        ListView1.EndUpdate()

    End Sub

    Private Function GetProcPathByPID(ByRef proc As Win32Process(), ByRef pid As Integer) As String
        GetProcPathByPID = ""
        For Each itm In proc
            If itm.PID = pid Then
                GetProcPathByPID = itm.ExecutablePath
                Exit For
            End If
        Next
    End Function


    Public Sub DisplayAll(Someobject)
        Dim _type As Type = Someobject.GetType()
        Dim properties() As PropertyInfo = _type.GetProperties()
        ' On Error Resume Next
        For Each _property As PropertyInfo In properties
            Console.WriteLine("Name: " + _property.Name + ", Value: " + _property.GetValue(Someobject, Nothing).ToString)
        Next
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Timer1.Enabled = False
        'RefreshConnections()
        'ListView1.Items.Clear()
        'refreshListView()
        'Timer1.Enabled = True
        BackgroundWorker1.RunWorkerAsync()
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged

    End Sub



    Private Sub ListView1_DrawItem(sender As Object, e As DrawListViewItemEventArgs) Handles ListView1.DrawItem
        e.DrawDefault = True
    End Sub

    Private Sub ListView1_DrawSubItem(sender As Object, e As DrawListViewSubItemEventArgs) Handles ListView1.DrawSubItem
        e.DrawDefault = True
    End Sub

    Private Sub LocateIPToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LocateIPToolStripMenuItem.Click
        LocateIP()

    End Sub

    Private Sub LocateFileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LocateFileToolStripMenuItem.Click
        LocateFile()
    End Sub
    Private Sub LocateFile()
        If (ListView1.SelectedItems.Count > 0) Then
            Dim path As String = ListView1.SelectedItems(0).Tag
            Process.Start(“explorer”, “/select,” & path)
        End If
    End Sub
    Private Sub LocateIP()
        If Not ListView1.SelectedItems.Count > 0 Then Exit Sub
        Dim ReplaceSite As String = "http://www.ipgeek.net/"
        Dim Addrs As String = Replace(ListView1.SelectedItems(0).SubItems(1).Text, "http://", ReplaceSite)
        Addrs = Replace(Addrs, "https://", ReplaceSite)
        If InStr(Addrs, ":") > 7 Or InStr(Addrs, ":") = 0 Then
            Addrs = ReplaceSite & Addrs
        End If
        If Not InStr(Addrs, "http:") > 0 Then Addrs = ReplaceSite & Addrs
        Process.Start(Addrs)
    End Sub
    Private Sub BlockApp()
        If Not ListView1.SelectedItems.Count > 0 Then Exit Sub
        Dim path As String = ListView1.SelectedItems(0).Tag
        If checkIfAppAlreadyBlocked(path) Then
            MsgBox("This App is Already Blocked")
            Exit Sub
        End If
        FireWall.BlockApplication(path, True)
        FireWall.BlockApplication(path, False)
    End Sub
    Private Sub BlockIP()
        If Not ListView1.SelectedItems.Count > 0 Then Exit Sub
        Dim Addrs As String = Replace(ListView1.SelectedItems(0).SubItems(1).Text, "http://", "")
        Addrs = Replace(Addrs, "https://", "")
        If InStr(Addrs, ":") > 0 Then
            Addrs = Mid(Addrs, 0, InStr(Addrs, ":") + 1)
        End If
        If checkIfIPAlreadyBlocked(Addrs) Then
            MsgBox("This IP is Already Blocked")
            Exit Sub
        End If
        FireWall.BlockIP(Addrs, True, ListView1.SelectedItems(0).Tag)
        FireWall.BlockIP(Addrs, False, ListView1.SelectedItems(0).Tag)
    End Sub
    Private Sub ListView1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles ListView1.MouseDoubleClick
        LocateFile()
    End Sub

    Private Sub ListView1_KeyDown(sender As Object, e As KeyEventArgs) Handles ListView1.KeyDown
        If e.KeyCode = Keys.I Then
            LocateIP()
        ElseIf e.KeyCode = Keys.F Then
            LocateFile()
        End If
    End Sub

    Private Sub BlockThisAppToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BlockThisAppToolStripMenuItem.Click
        BlockApp()
        MsgBox("Application Blocked. Now it can't use Internet")
    End Sub

    Private Sub BlockThisIPToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BlockThisIPToolStripMenuItem.Click

        BlockIP()
        MsgBox("IP is Blocked! Now you can't connect to this IP")
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles picExit.Click
        Me.Close()
    End Sub

    Private Sub ManojBhakarPCMToolStripMenuItem_Click(sender As Object, e As EventArgs)
        'BackgroundWorker1.RunWorkerAsync()
        'MsgBox("thread started")
    End Sub
    Private Sub testThreadFunction()

    End Sub

    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        'Dim addrs As String = ""
        RefreshConnections()
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        refreshListView()
        Timer1.Enabled = True
        'Label1.Text = e.Result
    End Sub
    '---------------------------------------------------------------
    'PictureBox1 == Bottom Bar. for form dragging.
    '---------------------------------------------------------------
    Dim drag As Boolean
    Dim mousex As Integer
    Dim mousey As Integer

    Private Sub Picturebox1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox1.MouseDown
        drag = True 'Sets the variable drag to true.
        mousex = Cursor.Position.X - Me.Left 'Sets variable mousex
        mousey = Cursor.Position.Y - Me.Top 'Sets variable mousey
    End Sub

    Private Sub Picturebox1_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox1.MouseMove
        'If drag is set to true then move the form accordingly.
        If drag Then
            Me.Top = Cursor.Position.Y - mousey
            Me.Left = Cursor.Position.X - mousex
        End If
    End Sub

    Private Sub Picturebox1_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox1.MouseUp

        drag = False 'Sets drag to false, so the form does not move according to the code in MouseMove
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        ContextMenuStrip2.Show(MousePosition.X, MousePosition.Y)
    End Sub

    Private Sub AddFileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddFileToolStripMenuItem.Click
        Dim path As String
        OpenFileDialog1.ShowDialog()
        path = OpenFileDialog1.FileName
        If Not path = "" Then
            'MsgBox(path)
            If checkIfAppAlreadyBlocked(path) Then
                MsgBox("This App is Already Blocked")
                Exit Sub
            End If
            FireWall.BlockApplication(path, False)
            FireWall.BlockApplication(path, True)
        End If
    End Sub

    Private Sub AddFolderToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddFolderToolStripMenuItem.Click
        Dim path As String
        FolderBrowserDialog1.ShowDialog()
        path = FolderBrowserDialog1.SelectedPath
        If Not path = "" Then
            BAcount = 0
            path = path & "\"
            DirSearchBlockApps(path)
            MsgBox(BAcount & " apps Blocked")
        End If
    End Sub
    Sub DirSearchBlockApps(ByVal sDir As String)
        Dim d As String
        Dim f As String

        Try
            For Each d In Directory.GetDirectories(sDir)
                For Each f In Directory.GetFiles(d, "*.exe")
                    FireWall.BlockApplication(f, False)
                    FireWall.BlockApplication(f, True)
                    BAcount += 1
                Next
                DirSearchBlockApps(d)
            Next
        Catch excpt As System.Exception
            Debug.WriteLine(excpt.Message)
        End Try
    End Sub

    Private Sub ToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem1.Click
        'Shows List of Blocked IPs
        Form4.Show()
        Form4.currentListType = "IP"
        refreshBlockedIPList()
    End Sub

    Private Sub SeeBlockedAppListToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SeeBlockedAppListToolStripMenuItem.Click
        Form4.Show()
        Form4.currentListType = "APP"
        refreshBlockedAppList()
    End Sub
    Private Function checkIfIPAlreadyBlocked(ByVal sIP As String) As Boolean
        Dim lst As List(Of FireWall.fwRule)
        lst = FireWall.Search("Name", "IPblockM9JFW_")
        Dim ipp As String
        For Each itm In lst
            ipp = Mid(itm.RemoteAddr, 1, InStr(itm.RemoteAddr, "/") - 1)
            If ipp = sIP Then
                Return True
                Exit Function
            End If
        Next
    End Function
    Private Function checkIfAppAlreadyBlocked(ByVal sAppPath As String) As Boolean
        Dim lst As List(Of FireWall.fwRule)
        lst = FireWall.Search("Name", "_AppBlockM9JFW")
        Dim ipp As String
        For Each itm In lst
            'ipp = Mid(itm.AppPath, 1, InStr(itm.AppPath, "/") - 1)
            If itm.AppPath = sAppPath Then
                Return True
                Exit Function
            End If
        Next
    End Function
    Public Shared Function CheckForInternetConnection() As Boolean
        Try
            Using client = New WebClient()
                Using stream = client.OpenRead("http://www.google.com")
                    Return True
                End Using
            End Using
        Catch
            Return False
        End Try
    End Function

    Private Sub CloseThisConnectionToolStripMenuItem_Click(sender As Object, e As EventArgs)
        If Not ListView1.SelectedItems.Count > 0 Then Exit Sub
        Dim Addrs As String = Replace(ListView1.SelectedItems(0).SubItems(1).Text, "http://", "")
        Addrs = Replace(Addrs, "https://", "")
        If InStr(Addrs, ":") > 0 Then
            Addrs = Mid(Addrs, 0, InStr(Addrs, ":") + 1)
        End If
        CloseRemoteIP(Addrs)
    End Sub

    Private Sub KillApplicationToolStripMenuItem_Click(sender As Object, e As EventArgs)
        If Not ListView1.SelectedItems.Count > 0 Then Exit Sub
        Dim ipath As String = ListView1.SelectedItems(0).Tag
        'Dim pName As String = 
        Dim proc = Process.GetProcessesByName(Replace(Path.GetFileName(ipath), ".exe", ""))
        For i As Integer = 0 To proc.Count - 1
            proc(i).CloseMainWindow()
        Next i
    End Sub

    Private Sub Button6_Click_1(sender As Object, e As EventArgs)
        MsgBox(ServiceHelper.isServiceDisabled("WinRM").ToString)
        ServiceHelper.ChangeStartMode("WinRM", ServiceStartMode.Automatic)
    End Sub

    Private Sub picFWState_Click(sender As Object, e As EventArgs) Handles picFWState.Click
        MakeSureServiceRunning()
    End Sub
    Private Sub MakeSureServiceRunning()
        'Make sure Service start mode is automatic
        ServiceHelper.ChangeStartMode("MpsSvc", ServiceStartMode.Automatic)
        'Make sure service is running
        Dim s As ServiceController
        For Each s In ServiceController.GetServices()
            If s.ServiceName = "MpsSvc" Then
                Exit For
            End If
        Next
        If Not s.Status = ServiceControllerStatus.Running Then
            s.Start()
            s.WaitForStatus(ServiceControllerStatus.Running)
        End If
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) 
        Dim a
        a = GetAllTCP6Connections()

    End Sub
End Class