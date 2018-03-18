Imports System.IO

Module CommonFuncs
    Public Sub refreshBlockedAppList()
        Dim lst As List(Of FireWall.fwRule)
        lst = FireWall.Search("Name", "_AppBlockM9JFW")
        Form4.Text = "List Of Blocked Apps By this Software"
        Form4.ImageList1.Images.Clear()
        With Form4.ListView1
            .Clear()
            .Columns.Add("App", 150)
            .Columns.Add("Date", 150)
            .Columns.Add("Path", 150)
            .Columns.Add("RuleName", 500)
            Dim iName As String
            Dim iApp As String
            Dim iDate As String
            Dim ar() As String
            Dim count As Integer = 0

            For Each itm In lst
                If itm.Direction = "OUTBOUND" Then
                    ar = Split(itm.Discription, "_")
                    iApp = ar(1)
                    iDate = ar(0)
                    iName = Path.GetFileName(iApp)
                    If Not File.Exists(iApp) Then GoTo lab
                    LvdisplayPathWithIcon(Form4.ListView1, Form4.ImageList1, iApp, Path.GetFileName(iApp))
                    '.Items.Add(iName)
                    .Items(count).SubItems.Add(iDate)
                    .Items(count).SubItems.Add(iApp)
                    .Items(count).SubItems.Add(itm.Name)
                    count += 1
lab:
                End If
            Next
        End With
    End Sub
    Delegate Sub LvdisplayPathWithIcon_delegate(ListView As ListView, ImageList As ImageList, path As String, StringToDisplay As String)
    Public Sub LvdisplayPathWithIcon(ListView As ListView, ImageList As ImageList, path As String, StringToDisplay As String)
        If path Is Nothing Then Exit Sub
        If ListView.InvokeRequired Then
            Dim d As New LvdisplayPathWithIcon_delegate(AddressOf LvdisplayPathWithIcon)
            ListView.BeginInvoke(d, {ListView, ImageList, path, StringToDisplay})
        Else

            Dim exeIcon As System.Drawing.Icon
            If ImageList.Images.ContainsKey(path) Then 'already have it.
                ListView.Items.Add(StringToDisplay, path) 'text,ImageKey overload
            Else 'dont have it. create a new one.
                Try
                    exeIcon = Icon.ExtractAssociatedIcon(path)
                Catch
                    exeIcon = Form4.Icon
                End Try
                If Not (exeIcon Is Nothing) Then
                    ImageList.Images.Add(path, exeIcon)
                    ListView.Items.Add(StringToDisplay, path)
                Else
                    ListView.Items.Add(StringToDisplay, "defaultexe")
                End If
            End If
        End If
    End Sub
    Public Sub refreshBlockedIPList()
        Dim lst As List(Of FireWall.fwRule)
        lst = FireWall.Search("Name", "IPblockM9JFW_")
        Form4.Text = "List Of Blocked IP's By this Software"
        Form4.ImageList1.Images.Clear()

        With Form4.ListView1
            .Clear()
            .Columns.Add("IP", 100)
            .Columns.Add("AppUsing", 150)
            .Columns.Add("Date", 150)
            .Columns.Add("RuleName", 400)
            .Columns.Add("PathOfApp", 1000)
            Dim iName As String
            Dim iIP As String
            Dim iApp As String
            Dim iDate As String
            Dim ar() As String
            Dim count As Integer = 0
            For Each itm In lst
                If itm.Direction = "INBOUND" Then
                    iName = itm.Name
                    ar = Split(itm.Discription, "_")
                    iIP = ar(0)
                    iApp = ar(1)
                    iDate = ar(2)
                    '.Items.Add(iIP)
                    LvdisplayPathWithIcon(Form4.ListView1, Form4.ImageList1, iApp, iIP)
                    .Items(count).SubItems.Add(Path.GetFileName(iApp))
                    .Items(count).SubItems.Add(iDate)
                    .Items(count).SubItems.Add(itm.Name)
                    .Items(count).SubItems.Add(iApp)
                    count += 1
                End If
            Next

        End With
    End Sub
End Module
