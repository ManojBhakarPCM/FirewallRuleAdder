<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form3
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form3))
        Me.ListView1 = New System.Windows.Forms.ListView()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.CloseThisConnectionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.KillApplicationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BlockThisIPToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BlockThisAppToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LocateFileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LocateIPToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ManojBhakarPCMToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.Button4 = New System.Windows.Forms.Button()
        Me.picExit = New System.Windows.Forms.PictureBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.ContextMenuStrip2 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AddFileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddFolderToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.SeeBlockedAppListToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.picFWState = New System.Windows.Forms.PictureBox()
        Me.picNetState = New System.Windows.Forms.PictureBox()
        Me.ContextMenuStrip1.SuspendLayout()
        CType(Me.picExit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip2.SuspendLayout()
        CType(Me.picFWState, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picNetState, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ListView1
        '
        Me.ListView1.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.ListView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ListView1.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ListView1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.ListView1.FullRowSelect = True
        Me.ListView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.ListView1.Location = New System.Drawing.Point(0, 0)
        Me.ListView1.MultiSelect = False
        Me.ListView1.Name = "ListView1"
        Me.ListView1.ShowItemToolTips = True
        Me.ListView1.Size = New System.Drawing.Size(517, 447)
        Me.ListView1.SmallImageList = Me.ImageList1
        Me.ListView1.TabIndex = 0
        Me.ListView1.UseCompatibleStateImageBehavior = False
        Me.ListView1.View = System.Windows.Forms.View.Details
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CloseThisConnectionToolStripMenuItem, Me.KillApplicationToolStripMenuItem, Me.BlockThisIPToolStripMenuItem, Me.BlockThisAppToolStripMenuItem, Me.LocateFileToolStripMenuItem, Me.LocateIPToolStripMenuItem, Me.ManojBhakarPCMToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(228, 186)
        '
        'CloseThisConnectionToolStripMenuItem
        '
        Me.CloseThisConnectionToolStripMenuItem.Image = Global.FireWallRuleAdder.My.Resources.Resources.Wrong_Pincode_32px
        Me.CloseThisConnectionToolStripMenuItem.Name = "CloseThisConnectionToolStripMenuItem"
        Me.CloseThisConnectionToolStripMenuItem.Size = New System.Drawing.Size(227, 26)
        Me.CloseThisConnectionToolStripMenuItem.Text = "Close This Connection"
        '
        'KillApplicationToolStripMenuItem
        '
        Me.KillApplicationToolStripMenuItem.Image = Global.FireWallRuleAdder.My.Resources.Resources.Close_Window_32px
        Me.KillApplicationToolStripMenuItem.Name = "KillApplicationToolStripMenuItem"
        Me.KillApplicationToolStripMenuItem.Size = New System.Drawing.Size(227, 26)
        Me.KillApplicationToolStripMenuItem.Text = "Kill Application"
        '
        'BlockThisIPToolStripMenuItem
        '
        Me.BlockThisIPToolStripMenuItem.Image = Global.FireWallRuleAdder.My.Resources.Resources.Web_Shield_32px
        Me.BlockThisIPToolStripMenuItem.Name = "BlockThisIPToolStripMenuItem"
        Me.BlockThisIPToolStripMenuItem.Size = New System.Drawing.Size(227, 26)
        Me.BlockThisIPToolStripMenuItem.Text = "Block this IP"
        '
        'BlockThisAppToolStripMenuItem
        '
        Me.BlockThisAppToolStripMenuItem.Image = CType(resources.GetObject("BlockThisAppToolStripMenuItem.Image"), System.Drawing.Image)
        Me.BlockThisAppToolStripMenuItem.Name = "BlockThisAppToolStripMenuItem"
        Me.BlockThisAppToolStripMenuItem.Size = New System.Drawing.Size(227, 26)
        Me.BlockThisAppToolStripMenuItem.Text = "Block This App"
        '
        'LocateFileToolStripMenuItem
        '
        Me.LocateFileToolStripMenuItem.Image = CType(resources.GetObject("LocateFileToolStripMenuItem.Image"), System.Drawing.Image)
        Me.LocateFileToolStripMenuItem.Name = "LocateFileToolStripMenuItem"
        Me.LocateFileToolStripMenuItem.Size = New System.Drawing.Size(227, 26)
        Me.LocateFileToolStripMenuItem.Text = "Locate File"
        '
        'LocateIPToolStripMenuItem
        '
        Me.LocateIPToolStripMenuItem.Image = CType(resources.GetObject("LocateIPToolStripMenuItem.Image"), System.Drawing.Image)
        Me.LocateIPToolStripMenuItem.Name = "LocateIPToolStripMenuItem"
        Me.LocateIPToolStripMenuItem.Size = New System.Drawing.Size(227, 26)
        Me.LocateIPToolStripMenuItem.Text = "Locate IP"
        '
        'ManojBhakarPCMToolStripMenuItem
        '
        Me.ManojBhakarPCMToolStripMenuItem.Name = "ManojBhakarPCMToolStripMenuItem"
        Me.ManojBhakarPCMToolStripMenuItem.Size = New System.Drawing.Size(227, 26)
        Me.ManojBhakarPCMToolStripMenuItem.Text = "ManojBhakarPCM"
        '
        'ImageList1
        '
        Me.ImageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit
        Me.ImageList1.ImageSize = New System.Drawing.Size(16, 16)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(762, 583)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(194, 79)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "Button1"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(371, 557)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(253, 104)
        Me.Button2.TabIndex = 2
        Me.Button2.Text = "Button2"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(261, 573)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(75, 23)
        Me.Button3.TabIndex = 3
        Me.Button3.Text = "Button3"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Timer1
        '
        Me.Timer1.Interval = 2000
        '
        'Button4
        '
        Me.Button4.Location = New System.Drawing.Point(771, 442)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(75, 23)
        Me.Button4.TabIndex = 5
        Me.Button4.Text = "Button4"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'picExit
        '
        Me.picExit.BackgroundImage = CType(resources.GetObject("picExit.BackgroundImage"), System.Drawing.Image)
        Me.picExit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.picExit.Location = New System.Drawing.Point(482, 427)
        Me.picExit.Name = "picExit"
        Me.picExit.Size = New System.Drawing.Size(23, 22)
        Me.picExit.TabIndex = 6
        Me.picExit.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(334, 93)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(51, 17)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "Label1"
        '
        'BackgroundWorker1
        '
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 430)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(51, 17)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "Label2"
        '
        'PictureBox1
        '
        Me.PictureBox1.Location = New System.Drawing.Point(0, 430)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(517, 17)
        Me.PictureBox1.TabIndex = 9
        Me.PictureBox1.TabStop = False
        '
        'Button5
        '
        Me.Button5.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Button5.ContextMenuStrip = Me.ContextMenuStrip2
        Me.Button5.FlatAppearance.BorderSize = 0
        Me.Button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button5.Location = New System.Drawing.Point(434, 430)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(22, 19)
        Me.Button5.TabIndex = 10
        Me.Button5.Text = "+"
        Me.Button5.UseVisualStyleBackColor = False
        '
        'ContextMenuStrip2
        '
        Me.ContextMenuStrip2.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.ContextMenuStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddFileToolStripMenuItem, Me.AddFolderToolStripMenuItem, Me.ToolStripMenuItem1, Me.SeeBlockedAppListToolStripMenuItem})
        Me.ContextMenuStrip2.Name = "ContextMenuStrip2"
        Me.ContextMenuStrip2.Size = New System.Drawing.Size(222, 108)
        '
        'AddFileToolStripMenuItem
        '
        Me.AddFileToolStripMenuItem.Image = CType(resources.GetObject("AddFileToolStripMenuItem.Image"), System.Drawing.Image)
        Me.AddFileToolStripMenuItem.Name = "AddFileToolStripMenuItem"
        Me.AddFileToolStripMenuItem.Size = New System.Drawing.Size(221, 26)
        Me.AddFileToolStripMenuItem.Text = "Add File"
        '
        'AddFolderToolStripMenuItem
        '
        Me.AddFolderToolStripMenuItem.Image = CType(resources.GetObject("AddFolderToolStripMenuItem.Image"), System.Drawing.Image)
        Me.AddFolderToolStripMenuItem.Name = "AddFolderToolStripMenuItem"
        Me.AddFolderToolStripMenuItem.Size = New System.Drawing.Size(221, 26)
        Me.AddFolderToolStripMenuItem.Text = "Add Folder"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Image = CType(resources.GetObject("ToolStripMenuItem1.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(221, 26)
        Me.ToolStripMenuItem1.Text = "See Blocked IP list"
        '
        'SeeBlockedAppListToolStripMenuItem
        '
        Me.SeeBlockedAppListToolStripMenuItem.Image = CType(resources.GetObject("SeeBlockedAppListToolStripMenuItem.Image"), System.Drawing.Image)
        Me.SeeBlockedAppListToolStripMenuItem.Name = "SeeBlockedAppListToolStripMenuItem"
        Me.SeeBlockedAppListToolStripMenuItem.Size = New System.Drawing.Size(221, 26)
        Me.SeeBlockedAppListToolStripMenuItem.Text = "See Blocked App List"
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'picFWState
        '
        Me.picFWState.Image = Global.FireWallRuleAdder.My.Resources.Resources.red
        Me.picFWState.Location = New System.Drawing.Point(358, 439)
        Me.picFWState.Name = "picFWState"
        Me.picFWState.Size = New System.Drawing.Size(32, 8)
        Me.picFWState.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.picFWState.TabIndex = 11
        Me.picFWState.TabStop = False
        '
        'picNetState
        '
        Me.picNetState.Image = Global.FireWallRuleAdder.My.Resources.Resources.red
        Me.picNetState.Location = New System.Drawing.Point(396, 439)
        Me.picNetState.Name = "picNetState"
        Me.picNetState.Size = New System.Drawing.Size(32, 8)
        Me.picNetState.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.picNetState.TabIndex = 12
        Me.picNetState.TabStop = False
        '
        'Form3
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(516, 449)
        Me.Controls.Add(Me.picNetState)
        Me.Controls.Add(Me.picFWState)
        Me.Controls.Add(Me.picExit)
        Me.Controls.Add(Me.Button5)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.Button4)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.ListView1)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Form3"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "TCP connections"
        Me.ContextMenuStrip1.ResumeLayout(False)
        CType(Me.picExit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip2.ResumeLayout(False)
        CType(Me.picFWState, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picNetState, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ListView1 As ListView
    Friend WithEvents Button1 As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents Button3 As Button
    Friend WithEvents Timer1 As Timer
    Friend WithEvents ImageList1 As ImageList
    Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
    Friend WithEvents BlockThisIPToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents BlockThisAppToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents LocateFileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents LocateIPToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents Button4 As Button
    Friend WithEvents picExit As PictureBox
    Friend WithEvents Label1 As Label
    Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
    Friend WithEvents Label2 As Label
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents Button5 As Button
    Friend WithEvents FolderBrowserDialog1 As FolderBrowserDialog
    Friend WithEvents OpenFileDialog1 As OpenFileDialog
    Friend WithEvents ContextMenuStrip2 As ContextMenuStrip
    Friend WithEvents AddFileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AddFolderToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents SeeBlockedAppListToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents picFWState As PictureBox
    Friend WithEvents picNetState As PictureBox
    Friend WithEvents CloseThisConnectionToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents KillApplicationToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ManojBhakarPCMToolStripMenuItem As ToolStripMenuItem
End Class
