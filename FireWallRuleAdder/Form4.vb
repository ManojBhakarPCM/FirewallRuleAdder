Public Class Form4
    Public Shared currentListType As String = ""
    Private Sub RemoveToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RemoveToolStripMenuItem.Click
        'Dim rName As String
        If Not ListView1.SelectedItems.Count > 0 Then Exit Sub
        Dim Addrs As String = ListView1.SelectedItems(0).SubItems(3).Text
        FireWall.RemoveRuleByName(Addrs)
        FireWall.RemoveRuleByName(Addrs) 'delete two times , because rule is both incoming and outgoing
        MsgBox("Rule Deleted from Firewall")
        If currentListType = "IP" Then
            refreshBlockedIPList()
        Else
            refreshBlockedAppList()
        End If
    End Sub
End Class