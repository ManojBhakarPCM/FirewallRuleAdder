Imports System.IO
Imports System.ServiceProcess
Public Class FireWall
    Const NET_FW_PROFILE2_DOMAIN As Int32 = 1
    Const NET_FW_PROFILE2_PRIVATE As Int32 = 2
    Const NET_FW_PROFILE2_PUBLIC As Int32 = 4
    Const NET_FW_PROFILE2_ALL As Int32 = 2147483647

    Const NET_FW_IP_PROTOCOL_TCP As Int32 = 6
    Const NET_FW_IP_PROTOCOL_UDP As Int32 = 17
    Const NET_FW_IP_PROTOCOL_ICMPv4 As Int32 = 1
    Const NET_FW_IP_PROTOCOL_ICMPv6 As Int32 = 58

    'Direction
    Const NET_FW_RULE_DIR_IN As Int32 = 1
    Const NET_FW_RULE_DIR_OUT As Int32 = 2

    'Action()
    Const NET_FW_ACTION_BLOCK As Int32 = 0
    Const NET_FW_ACTION_ALLOW As Int32 = 1
    Public Structure fwRule
        Public Enabled As Boolean
        Public RuleGroup As String
        Public Name As String
        Public Discription As String
        Public AppPath As String
        Public RemoteAddr As String
        Public RemotePorts As String
        Public LocalAddr As String
        Public LocalPorts As String
        Public Protocol As String
        Public Profile As String
        Public Direction As String
        Public Action As String
        Public ServiceName As String
    End Structure
    Public Shared Function ListRules() As List(Of fwRule)
        Dim ruls As New List(Of fwRule)

        Dim PolicyType = Type.GetTypeFromProgID("HNetCfg.FwPolicy2")
        Dim Policy = Activator.CreateInstance(PolicyType)


        For Each Rule In Policy.Rules
            Dim aFwRule As New fwRule

            aFwRule.Name = Rule.Name
            aFwRule.AppPath = Rule.ApplicationName
            If aFwRule.AppPath Is Nothing Then aFwRule.AppPath = ""
            aFwRule.Discription = Rule.Description
            aFwRule.RuleGroup = Rule.Grouping
            aFwRule.LocalAddr = Rule.LocalAddresses
            aFwRule.LocalPorts = Rule.LocalPorts
            aFwRule.RemoteAddr = Rule.RemoteAddresses
            aFwRule.RemotePorts = Rule.RemotePorts
            aFwRule.ServiceName = Rule.ServiceName
            If (Rule.Enabled) Then
                aFwRule.Enabled = True
            Else
                aFwRule.Enabled = False
            End If
            If (Rule.Profiles = NET_FW_PROFILE2_ALL) Then
                aFwRule.Profile = "ALL"
            Else
                If ((Rule.Profiles & NET_FW_PROFILE2_DOMAIN) = NET_FW_PROFILE2_DOMAIN) Then aFwRule.Profile = "DOMAIN"
                If ((Rule.Profiles & NET_FW_PROFILE2_PRIVATE) = NET_FW_PROFILE2_PRIVATE) Then aFwRule.Profile = "PRIVATE"
                If ((Rule.Profiles & NET_FW_PROFILE2_PUBLIC) = NET_FW_PROFILE2_PUBLIC) Then aFwRule.Profile = "PUBLIC"
            End If
            Select Case Rule.Direction
                Case NET_FW_RULE_DIR_IN
                    aFwRule.Direction = "INBOUND"
                Case NET_FW_RULE_DIR_OUT
                    aFwRule.Direction = "OUTBOUND"
            End Select
            Select Case (Rule.Action)
                Case NET_FW_ACTION_ALLOW
                    aFwRule.Action = "ALLOW"
                Case NET_FW_ACTION_BLOCK
                    aFwRule.Action = "BLOCK"
            End Select
            Select Case Rule.Protocol
                Case NET_FW_IP_PROTOCOL_ICMPv4
                    aFwRule.Protocol = ("ICMPv4")
                Case NET_FW_IP_PROTOCOL_ICMPv6
                    aFwRule.Protocol = ("ICMPv6")
                Case NET_FW_IP_PROTOCOL_TCP
                    aFwRule.Protocol = ("TCP")
                Case NET_FW_IP_PROTOCOL_UDP
                    aFwRule.Protocol = ("UDP")
                Case Else
                    aFwRule.Protocol = Rule.Protocol.ToString
            End Select
            'ruls.Add(aFwRule)
            ruls.Add(aFwRule)
        Next
        Console.WriteLine("Total Rules Listed By FireWall.ListRules() = " & ruls.Count)
        Return ruls
    End Function
    Public Shared Sub RemoveRuleByName(ByVal Name As String)
        Dim PolicyType = Type.GetTypeFromProgID("HNetCfg.FwPolicy2")
        Dim Policy = Activator.CreateInstance(PolicyType)
        Policy.Rules.Remove(Name)
        Policy = Nothing
    End Sub
    Public Sub AddRule(Rule As fwRule)
        Dim fwPolicy2 = CreateObject("HNetCfg.FwPolicy2")
        Dim NewRule = CreateObject("HNetCfg.FWRule")
        Dim RulesObject = fwPolicy2.Rules
        Dim CurrentProfiles = fwPolicy2.CurrentProfileTypes

        RulesObject.Add(Rule)
        fwPolicy2 = Nothing
        NewRule = Nothing
    End Sub
    Public Shared Function SearchRulesExact(SearchPattern As fwRule, Optional ByVal includeEnablationState As Boolean = False) As List(Of fwRule)
        Dim ruleList As List(Of fwRule) = ListRules()
        'We will remove all rules that do not match pattern and remaining list will be returned as output.
        For Each itm In ruleList.ToList()   'here .ToList() is LifeSaver. it copy the ruleList to saprate list. so when orignal list is modified, it dont give modified index error.
            If Not SearchPattern.Name = "" And Not SearchPattern.Name = itm.Name Then ruleList.Remove(itm) : Continue For
            If Not SearchPattern.Discription = "" And Not SearchPattern.Discription = itm.Discription Then ruleList.Remove(itm) : Continue For
            If Not SearchPattern.AppPath = "" And Not SearchPattern.AppPath = itm.AppPath Then ruleList.Remove(itm) : Continue For
            If Not SearchPattern.Action = "" And Not SearchPattern.Action = itm.Action Then ruleList.Remove(itm) : Continue For
            If Not SearchPattern.Direction = "" And Not SearchPattern.Direction = itm.Direction Then ruleList.Remove(itm) : Continue For
            If Not SearchPattern.LocalAddr = "" And Not SearchPattern.LocalAddr = itm.LocalAddr Then ruleList.Remove(itm) : Continue For
            If Not SearchPattern.LocalPorts = "" And Not SearchPattern.LocalPorts = itm.LocalPorts Then ruleList.Remove(itm) : Continue For
            If Not SearchPattern.Protocol = "" And Not SearchPattern.Protocol = itm.Protocol Then ruleList.Remove(itm) : Continue For
            If Not SearchPattern.RemoteAddr = "" And Not SearchPattern.RemoteAddr = itm.RemoteAddr Then ruleList.Remove(itm) : Continue For
            If Not SearchPattern.RemotePorts = "" And Not SearchPattern.RemotePorts = itm.RemotePorts Then ruleList.Remove(itm) : Continue For
            If Not SearchPattern.ServiceName = "" And Not SearchPattern.ServiceName = itm.ServiceName Then ruleList.Remove(itm) : Continue For
            If Not SearchPattern.RuleGroup = "" And Not SearchPattern.RuleGroup = itm.RuleGroup Then ruleList.Remove(itm) : Continue For
            If Not SearchPattern.Profile = "" And Not SearchPattern.Profile = itm.Profile Then ruleList.Remove(itm) : Continue For

            If includeEnablationState Then
                If Not SearchPattern.Enabled = itm.Enabled Then ruleList.Remove(itm) : Continue For
            End If
        Next
        Return ruleList
    End Function
    Public Shared Function SearchRulesContaining(SearchPattern As fwRule, Optional ByVal includeEnablationState As Boolean = False) As List(Of fwRule)
        Dim ruleList As List(Of fwRule) = ListRules()
        'We will remove all rules that do not match pattern and remaining list will be returned as output.
        For Each itm In ruleList.ToList()   'here .ToList() is LifeSaver. it copy the ruleList to saprate list. so when orignal list is modified, it dont give modified index error.

            If Not SearchPattern.Name = "" And Not itm.Name.Contains(SearchPattern.Name) Then ruleList.Remove(itm) : Continue For
            If Not SearchPattern.Discription = "" And Not itm.Discription.Contains(SearchPattern.Discription) Then ruleList.Remove(itm) : Continue For
            If Not SearchPattern.AppPath = "" And Not itm.AppPath.Contains(SearchPattern.AppPath) Then ruleList.Remove(itm) : Continue For
            If Not SearchPattern.Action = "" And Not itm.Action.Contains(SearchPattern.Action) Then ruleList.Remove(itm) : Continue For
            If Not SearchPattern.Direction = "" And Not itm.Direction.Contains(SearchPattern.Direction) Then ruleList.Remove(itm) : Continue For
            If Not SearchPattern.LocalAddr = "" And Not itm.LocalAddr.Contains(SearchPattern.LocalAddr) Then ruleList.Remove(itm) : Continue For
            If Not SearchPattern.LocalPorts = "" And Not itm.LocalPorts.Contains(SearchPattern.LocalPorts) Then ruleList.Remove(itm) : Continue For
            If Not SearchPattern.Protocol = "" And Not itm.Protocol.Contains(SearchPattern.Protocol) Then ruleList.Remove(itm) : Continue For
            If Not SearchPattern.RemoteAddr = "" And Not itm.RemoteAddr.Contains(SearchPattern.RemoteAddr) Then ruleList.Remove(itm) : Continue For
            If Not SearchPattern.RemotePorts = "" And Not itm.RemotePorts.Contains(SearchPattern.RemotePorts) Then ruleList.Remove(itm) : Continue For
            If Not SearchPattern.ServiceName = "" And Not itm.ServiceName.Contains(SearchPattern.ServiceName) Then ruleList.Remove(itm) : Continue For
            If Not SearchPattern.RuleGroup = "" And Not itm.RuleGroup.Contains(SearchPattern.RuleGroup) Then ruleList.Remove(itm) : Continue For
            If Not SearchPattern.Profile = "" And Not itm.Profile.Contains(SearchPattern.Profile) Then ruleList.Remove(itm) : Continue For

            If includeEnablationState Then
                If Not SearchPattern.Enabled = itm.Enabled Then ruleList.Remove(itm) : Continue For
            End If

        Next
        Return ruleList
    End Function
    Public Shared Function Search(ByVal what As String, ByVal value As String) As List(Of fwRule)
        Dim ruleList As List(Of fwRule) = ListRules()

        'We will remove all rules that do not match pattern and remaining list will be returned as output.
        If (Not what = "") And (Not value = "") Then
            For Each itm In ruleList.ToList()
                'On Error Resume Next
                Select Case what
                    Case "Name"
                        If Not itm.Name.Contains(value) Then ruleList.Remove(itm) : Continue For
                    Case "AppPath"
                        If Not itm.AppPath.Contains(value) Then ruleList.Remove(itm) : Continue For
                    Case "Discription"
                        If Not itm.Discription.Contains(value) Then ruleList.Remove(itm) : Continue For
                    Case "Protocol"
                        If Not itm.Protocol.Contains(value) Then ruleList.Remove(itm) : Continue For
                    Case "RemoteADDR"
                        If Not itm.RemoteAddr.Contains(value) Then ruleList.Remove(itm) : Continue For
                    Case "LocalADDR"
                        If Not itm.LocalAddr.Contains(value) Then ruleList.Remove(itm) : Continue For
                    Case "ServiceName"
                        If (Not itm.ServiceName Is Nothing) Then
                            If (Not itm.ServiceName.Contains(value)) Then ruleList.Remove(itm) : Continue For
                        End If
                End Select
            Next
        End If
        Return ruleList
    End Function
    Public Shared Function RemoveRulesWithConfirmation(SearchPattern As fwRule) As Integer 'Returns matching rules still found after deletion. -1 if no rule was found.
        Dim Rules As List(Of fwRule) = ListRules()
        Dim nam As Integer
        If Rules.Count = 0 Then
            Return -1
        End If
        For Each itm In Rules
            nam = itm.Name
            RemoveRuleByName(itm.Name)
        Next
        'confirming they are deleted.
        Dim RulesAfter As List(Of fwRule) = SearchRulesExact(SearchPattern)
        Return RulesAfter.Count
    End Function
    Public Shared Sub RemoveRules(SearchPattern As fwRule) 'Returns matching rules still found after deletion. -1 if no rule was found.
        Dim Rules As List(Of fwRule) = ListRules()
        Dim nam As Integer
        If Rules.Count = 0 Then
            Exit Sub
        End If
        For Each itm In Rules
            nam = itm.Name
            RemoveRuleByName(itm.Name)
        Next
        'confirming they are deleted.
    End Sub
    Public Shared Function BackUpRules(SelectionPattern As fwRule, Filepath As String) As String 'Returns Number of string of lines directly written to file.
        Return Nothing
        'TODO: Add Functionality.
    End Function
    Public Shared Function RestoreRules(ByVal filepath As String, ByVal SelectionPattern As fwRule) As Integer 'Return Number of Rules Restored.
        Return Nothing
        'TODO: Add Functionality
    End Function
    Public Shared Function isFirewallEnabled() As Boolean
        For Each s As ServiceController In ServiceController.GetServices()
            If s.ServiceName = "Windows Firewall" AndAlso s.Status = ServiceControllerStatus.Running Then
                Return True
            End If
        Next
    End Function
    Public Shared Function TurnONorOFFFirewall(ByVal turnON As Boolean) As Boolean
        Return Nothing
        'TODO: Add functionality
    End Function
    Public Shared Function BlockApplication(ByVal AppPath As String, ByVal isOutBound As Boolean)
        Dim fwPolicy2 = CreateObject("HNetCfg.FwPolicy2")
        Dim NewRule = CreateObject("HNetCfg.FWRule")
        Dim RulesObject = fwPolicy2.Rules
        Dim CurrentProfiles = fwPolicy2.CurrentProfileTypes
        Dim AppName As String = IO.Path.GetFileName(AppPath) & "_AppBlockM9JFW"
        Dim AppDiscription As String = Today.Date & "_" & AppPath
        'If isOutBound Then
        'AppDiscription = AppDiscription & "OutGoing Internet Connection Blocked for this app By M9JSOFT FireWall Rule Editor"
        'Else
        'AppDiscription = AppDiscription & "Incoming Internet Connection Blocked for this app By M9JSOFT FireWall Rule Editor"
        'End If
        '--------------------------------------------------
        NewRule.Name = AppName                                        'String Name To Display
        NewRule.Description = AppDiscription                          'String Discription
        NewRule.Applicationname = AppPath                             'String Path of application
        'NewRule.serviceName =                                        'String ServiceName
        'NewRule.LocalAddresses = "*"                                 'String ie. 255.255.255.255
        'NewRule.LocalPorts = "*"                                     'String ie. 8080
        'NewRule.RemoteAddresses = "*"                                'String
        'NewRule.RemotePorts = "*"                                    'String
        NewRule.Protocol = NET_FW_IP_PROTOCOL_TCP                     'integer 6 = TCP, 256 = ANY, 17 = UDP
        NewRule.Action = 0                                            'Integer Allow = 1, Block = 0
        If isOutBound = True Then
            NewRule.Direction = 2                                     'Integer inComing = 1 , outGoing = 2
        Else
            NewRule.Direction = 1
        End If
        NewRule.Enabled = True                                        'Boolean
        NewRule.Profiles = NET_FW_PROFILE2_ALL                        'Integer 1= Domain, 2 = Private , 4 = Public , 2147483647 = All
        '------------------Special
        NewRule.Grouping = "@firewallapi.dll,-23255"                  'String. find in FirewallApi.dll.
        ' NewRule.EdgeTraversal = False                               'Boolean
        ' NewRule.IcmpTypesAndCodes = "?"                             'String
        ' NewRule.Interfaces = ""                                     'Object
        ' NewRule.InterfaceTypes = "?"                                'String ie. "LAN"

        '------------------------
        RulesObject.Add(NewRule)
        fwPolicy2 = Nothing
        NewRule = Nothing
    End Function
    Public Shared Function BlockIP(ByVal IP As String, ByVal isOutBound As Boolean, Optional ByVal appUsingIt As String = "")
        Dim fwPolicy2 = CreateObject("HNetCfg.FwPolicy2")
        Dim NewRule = CreateObject("HNetCfg.FWRule")
        Dim RulesObject = fwPolicy2.Rules
        Dim CurrentProfiles = fwPolicy2.CurrentProfileTypes

        Dim AppDiscription As String = IP & "_" & appUsingIt & "_" & Today
        'If isOutBound Then
        'AppDiscription = AppDiscription & "OutGoing Internet Connections to this IP are Blocked By M9JSOFT FireWall Rule Editor"
        'Else
        'AppDiscription = AppDiscription & "Incoming Internet Connections to this IP are Blocked By M9JSOFT FireWall Rule Editor"
        'End If
        '--------------------------------------------------
        NewRule.Name = "IPblockM9JFW_" & IP & "_" & Path.GetFileName(appUsingIt)                                'String Name To Display
        NewRule.Description = AppDiscription                          'String Discription
        'NewRule.Applicationname = "*"                                'String Path of application
        'NewRule.serviceName =                                        'String ServiceName
        'NewRule.LocalAddresses = "*"                                 'String ie. 255.255.255.255
        'NewRule.LocalPorts = "*"                                     'String ie. 8080
        NewRule.RemoteAddresses = IP                                  'String
        'NewRule.RemotePorts = "*"                                    'String
        NewRule.Protocol = NET_FW_IP_PROTOCOL_TCP                     'integer 6 = TCP, 256 = ANY, 17 = UDP
        NewRule.Action = 0                                            'Integer Allow = 1, Block = 0
        If isOutBound = True Then
            NewRule.Direction = 2                                     'Integer inComing = 1 , outGoing = 2
        Else
            NewRule.Direction = 1
        End If
        NewRule.Enabled = True                                        'Boolean
        NewRule.Profiles = NET_FW_PROFILE2_ALL                        'Integer 1= Domain, 2 = Private , 4 = Public , 2147483647 = All
        '------------------Special
        NewRule.Grouping = "@firewallapi.dll,-23255"                  'String. find in FirewallApi.dll.
        ' NewRule.EdgeTraversal = False                               'Boolean
        ' NewRule.IcmpTypesAndCodes = "?"                             'String
        ' NewRule.Interfaces = ""                                     'Object
        ' NewRule.InterfaceTypes = "?"                                'String ie. "LAN"

        '------------------------
        RulesObject.Add(NewRule)
        fwPolicy2 = Nothing
        NewRule = Nothing
    End Function
End Class

