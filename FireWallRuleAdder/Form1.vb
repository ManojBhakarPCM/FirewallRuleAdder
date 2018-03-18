Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs)
        ' Option Explicit On



        ' Protocol
        Const NET_FW_IP_PROTOCOL_TCP = 6

        'Action
        Const NET_FW_ACTION_ALLOW = 1

        ' Create the FwPolicy2 object.
        Dim fwPolicy2 = CreateObject("HNetCfg.FwPolicy2")
        Dim NewRule = CreateObject("HNetCfg.FWRule")
        Dim RulesObject = fwPolicy2.Rules
        Dim CurrentProfiles = fwPolicy2.CurrentProfileTypes


        NewRule.Name = "Stud_Pe"
        NewRule.Description = "Allow my application network traffic"
        NewRule.Applicationname = "G:\portableApps\StudPE"
        NewRule.Protocol = NET_FW_IP_PROTOCOL_TCP

        NewRule.LocalPorts = 4000
        NewRule.Enabled = True
        NewRule.Grouping = "@firewallapi.dll,-23255"
        NewRule.Profiles = CurrentProfiles
        NewRule.Action = NET_FW_ACTION_ALLOW
        RulesObject.Add(NewRule)

        ' Rule.Name = Application.ProductName;
        'Rule.ApplicationName = Application.ExecutablePath;
        '   Rule.Description = "This is a sample GPO entry.";
        'Rule.Profiles = NET_FW_PROFILE2_PUBLIC;
        'Rule.Direction = NET_FW_RULE_DIR_IN;
        'Rule.Action = NET_FW_ACTION_ALLOW;
        'Rule.Protocol = NET_FW_IP_PROTOCOL_TCP;
        'Rule.RemoteAddresses = "10.1.1.1/255.255.255.255";
        'Rule.RemotePorts = "*";
        'Rule.LocalAddresses = "*";
        'Rule.LocalPorts = "*";
        'Rule.Enabled = True;
        ' Rule.InterfaceTypes = "All";

    End Sub
    Private Function AddToFireWall(ByVal AppName As String, ByVal AppDiscription As String, ByVal AppPath As String, ByVal Protocol As Integer, ByVal Ports As Integer, ByVal AlowOrBlock As Boolean)
        Dim fwPolicy2 = CreateObject("HNetCfg.FwPolicy2")
        Dim NewRule = CreateObject("HNetCfg.FWRule")
        Dim RulesObject = fwPolicy2.Rules
        Dim CurrentProfiles = fwPolicy2.CurrentProfileTypes


        NewRule.Name = AppName
        NewRule.Description = AppDiscription
        NewRule.Applicationname = AppPath
        NewRule.Protocol = Protocol
        NewRule.LocalPorts = 4000
        NewRule.Enabled = True
        NewRule.Grouping = "@firewallapi.dll,-23255"
        NewRule.Profiles = CurrentProfiles
        NewRule.Action = AlowOrBlock
        RulesObject.Add(NewRule)
        Return Nothing
    End Function
    Private Sub BlockAnIP(ByVal IPAddress As String)
        Dim rule As NetFwTypeLib.INetFwRule
        rule.Action = NetFwTypeLib.NET_FW_ACTION_.NET_FW_ACTION_BLOCK '0
        rule.RemoteAddresses = ""
        rule.Protocol = NetFwTypeLib.NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_TCP '6 = TCP, 256 = ANY, 17 = UDP
        rule.ApplicationName = ""
        '----------------------------------------------------------

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)

        MsgBox("OK")
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If (FolderBrowserDialog1.ShowDialog() = DialogResult.OK) Then
            Foo(FolderBrowserDialog1.SelectedPath)
        End If
    End Sub
    Private Sub Foo(ByVal directory As String)
        For Each filename As String In IO.Directory.GetFiles(directory, "*.exe", IO.SearchOption.AllDirectories)

            'Dim fName As String = IO.Path.GetExtension(filename)
            'If fName = ".*" Then
            ListBox1.Items.Add(filename)
            ' End If|
        Next
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        If ListBox1.SelectedItems.Count > 0 Then
            ListBox1.Items.Remove(ListBox1.SelectedItem)
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If (OpenFileDialog1.ShowDialog() = DialogResult.OK) Then
            ListBox1.Items.Add(OpenFileDialog1.FileName)
        End If
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        If ListBox1.Items.Count = 0 Then
            llog.Items.Add("No Items In listBox")
            Exit Sub
        End If
        For Each itm In ListBox1.Items
            BlockAnAppR(itm, True)
            BlockAnAppR(itm, False)
            llog.Items.Add("Added " & itm)
        Next
        MsgBox("OK")
    End Sub
    Public Sub BlockAnAppR(itm As String, OutBound As Boolean)
        Dim fwPolicy2 = CreateObject("HNetCfg.FwPolicy2")
        Dim NewRule = CreateObject("HNetCfg.FWRule")



        Dim RulesObject = fwPolicy2.Rules
        Dim CurrentProfiles = fwPolicy2.CurrentProfileTypes
        '--------------------------------------------------
        NewRule.Name = Replace(TextBox1.Text, "%exe%", IO.Path.GetFileName(itm))                                   'String Name To Display
        NewRule.Description = Replace(TextBox2.Text, "%exe%", IO.Path.GetFileName(itm))                                   'String Discription
        NewRule.Applicationname = itm                                'String Path of application
        'NewRule.serviceName =                                       'String ServiceName
        'NewRule.LocalAddresses = "*"                                 'String ie. 255.255.255.255
        'NewRule.LocalPorts = "*"                                     'String ie. 8080
        'NewRule.RemoteAddresses = "*"                                'String
        'NewRule.RemotePorts = "*"                                    'String
        NewRule.Protocol = 6                                        'integer 6 = TCP, 256 = ANY, 17 = UDP

        NewRule.Action = 0                                          'Integer Allow = 1, Block = 0
        If OutBound = True Then
            NewRule.Direction = 2                                       'Integer inComing = 1 , outGoing = 2
        Else
            NewRule.Direction = 1
        End If
        NewRule.Enabled = True                                      'Boolean
        NewRule.Profiles = 2147483647                               'Integer 1= Domain, 2 = Private , 4 = Public , 2147483647 = All
        '------------------Special
        NewRule.Grouping = "@firewallapi.dll,-23255"                'String. find in FirewallApi.dll.
        ' NewRule.EdgeTraversal = False                               'Boolean
        ' NewRule.IcmpTypesAndCodes = "?"                             'String
        ' NewRule.Interfaces = ""                                     'Object
        ' NewRule.InterfaceTypes = "?"                                'String ie. "LAN"

        '------------------------
        RulesObject.Add(NewRule)
        fwPolicy2 = Nothing
        NewRule = Nothing
    End Sub
    Public Sub BlockAnIPR(ipAddr As String, outBound As Boolean)
        Dim fwPolicy2 = CreateObject("HNetCfg.FwPolicy2")
        Dim NewRule = CreateObject("HNetCfg.FWRule")



        Dim RulesObject = fwPolicy2.Rules
        Dim CurrentProfiles = fwPolicy2.CurrentProfileTypes
        '--------------------------------------------------
        NewRule.Name = Replace(TextBox4.Text, "%IP%", IO.Path.GetFileName(ipAddr))                                   'String Name To Display
        NewRule.Description = Replace(TextBox3.Text, "%IP%", IO.Path.GetFileName(ipAddr))                                   'String Discription
        'NewRule.Applicationname = itm                                'String Path of application
        'NewRule.serviceName =                                       'String ServiceName
        'NewRule.LocalAddresses = "*"                                 'String ie. 255.255.255.255
        'NewRule.LocalPorts = "*"                                     'String ie. 8080
        NewRule.RemoteAddresses = ipAddr                                'String
        'NewRule.RemotePorts = "*"                                    'String
        NewRule.Protocol = 6                                        'integer 6 = TCP, 256 = ANY, 17 = UDP

        NewRule.Action = 0                                          'Integer Allow = 1, Block = 0
        If outBound = True Then
            NewRule.Direction = 2                                       'Integer inComing = 1 , outGoing = 2
        Else
            NewRule.Direction = 1
        End If
        NewRule.Enabled = True                                      'Boolean
        NewRule.Profiles = 2147483647                               'Integer 1= Domain, 2 = Private , 4 = Public , 2147483647 = All
        '------------------Special
        NewRule.Grouping = "@firewallapi.dll,-23255"                'String. find in FirewallApi.dll.
        ' NewRule.EdgeTraversal = False                               'Boolean
        ' NewRule.IcmpTypesAndCodes = "?"                             'String
        ' NewRule.Interfaces = ""                                     'Object
        ' NewRule.InterfaceTypes = "?"                                'String ie. "LAN"

        '------------------------
        RulesObject.Add(NewRule)
        fwPolicy2 = Nothing
        NewRule = Nothing
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        If Not TextBox5.Text = "" Then
            ListBox3.Items.Add(TextBox5.Text)
        End If
    End Sub

    Private Sub Button2_Click_1(sender As Object, e As EventArgs) Handles Button2.Click
        If ListBox3.SelectedItems.Count > 0 Then
            ListBox3.Items.Remove(ListBox3.SelectedItem)
        End If
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        If ListBox3.Items.Count = 0 Then
            logg.Items.Add("Nothing To Block")
            Exit Sub
        End If

        For Each itm In ListBox3.Items
            BlockAnIPR(itm, True)
            BlockAnIPR(itm, False)
            logg.Items.Add("Added " & itm)
        Next
        MsgBox("OK")
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        Form2.Show()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class
