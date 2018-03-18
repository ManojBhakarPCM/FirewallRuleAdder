Public Class Form2
    Private Sub ShowRulesToDGV(ruls As List(Of FireWall.fwRule))

        DataGridView1.Rows.Clear()
        Dim i As Int16
        'MsgBox(ruls.Count)
        For Each item In ruls
            DataGridView1.Rows.Add()
            With DataGridView1.Rows(i)
                If item.Enabled Then
                    .Cells(0).Value = ImageList1.Images(0)
                Else
                    .Cells(0).Value = ImageList1.Images(1)
                End If
                .Cells(0).Tag = item.Enabled.ToString.ToUpper
                .Cells(0).ToolTipText = item.Enabled

                '-----------------------------
                If item.Action = "ALLOW" Then
                    .Cells(1).Value = ImageList1.Images(2)
                Else
                    .Cells(1).Value = ImageList1.Images(3)
                End If
                .Cells(1).Tag = item.Action
                .Cells(1).ToolTipText = item.Action
                '-----------------------------
                If item.Direction = "INBOUND" Then
                    .Cells(2).Value = ImageList1.Images(4)
                Else
                    .Cells(2).Value = ImageList1.Images(5)
                End If
                .Cells(2).Tag = item.Direction
                .Cells(2).ToolTipText = item.Direction
                '---------Name
                If item.Name = "" Then item.Name = "[Service]" & item.ServiceName
                .Cells(3).Value = item.RemoteAddr
                .Cells(3).Tag = item.RemoteAddr
                .Cells(3).ToolTipText = item.RemoteAddr
                '---------IP

                '---------Path
                .Cells(4).Value = item.AppPath
                .Cells(4).Tag = item.AppPath
                .Cells(4).ToolTipText = item.AppPath & vbCrLf & item.Name & vbCrLf & item.Discription

                .Cells(5).Value = item.Protocol
                .Cells(5).Tag = item.Protocol
                .Cells(5).ToolTipText = item.Protocol
                '---------Discription
                .Cells(6).Value = item.RemotePorts
                .Cells(6).Tag = item.RemotePorts
                .Cells(6).ToolTipText = item.RemotePorts
            End With
            i = i + 1
        Next
        Application.DoEvents()
        HideRows()
        lblinfo.Text = "Total : " & DataGridView1.Rows.Count

    End Sub
    Private Sub HideRows()
        For Each row As DataGridViewRow In DataGridView1.Rows
            DataGridView1.Rows.Item(row.Index).Visible = False
            If CheckBox1.Checked And Not row.Cells(5).Value = "TCP" Then Continue For 'TCP ONLY
            If CheckBox2.Checked And Not row.Cells(2).Tag = "BLOCKED" Then Continue For 'BLOCKED ONLY
            If CheckBox3.Checked And Not row.Cells(0).Tag = "TRUE" Then Continue For 'ENABLED ONLY
            If CheckBox4.Checked And row.Cells(4).Value Is Nothing Then Continue For 'Hide Windows path
            If CheckBox4.Checked And (row.Cells(4).Value = "" Or row.Cells(4).Value = "System" Or row.Cells(4).Value.ToString.Contains(":\Windows\")) Then Continue For
            DataGridView1.Rows.Item(row.Index).Visible = True
        Next
        lblinfo.Text = "Total : " & DataGridView1.Rows.GetRowCount(DataGridViewElementStates.Visible)
    End Sub

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Dim ruls As List(Of FireWall.fwRule) = FireWall.ListRules
        'ShowRulesToDGV(ruls)
        With ComboBox1.Items
            .Add("Name")
            .Add("Discription")
            .Add("AppPath")
            .Add("Protocol")
            .Add("RemoteADDR")
            .Add("LocalADDR")
            .Add("ServiceName")
            ComboBox1.SelectedIndex = 0
        End With
    End Sub

    Private Sub searchNow()
        TextBox1.Enabled = False
        DataGridView1.Enabled = False
        ShowRulesToDGV(FireWall.Search(ComboBox1.Text, TextBox1.Text))
        HideRows()
        DataGridView1.Enabled = True
        TextBox1.Enabled = True
        TextBox1.Focus()
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        searchNow()
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        searchNow()
    End Sub
    Private Sub CheckBox3_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox3.CheckedChanged
        HideRows()
    End Sub

    Private Sub CheckBox4_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox4.CheckedChanged
        HideRows()
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        HideRows()

    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged
        HideRows()
    End Sub
End Class