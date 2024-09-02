<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
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
        Me.CallBackTimer = New System.Windows.Forms.Timer(Me.components)
        Me.StartBtn = New System.Windows.Forms.Button()
        Me.ExitBtn = New System.Windows.Forms.Button()
        Me.GuideBtn = New System.Windows.Forms.Button()
        Me.NextBtn1 = New System.Windows.Forms.Button()
        Me.ConfigBtn = New System.Windows.Forms.Button()
        Me.NextBtn2 = New System.Windows.Forms.Button()
        Me.EasyBtn = New System.Windows.Forms.Button()
        Me.NormalBtn = New System.Windows.Forms.Button()
        Me.HardBtn = New System.Windows.Forms.Button()
        Me.Title = New System.Windows.Forms.PictureBox()
        Me.ClearImg = New System.Windows.Forms.PictureBox()
        Me.DeadImg = New System.Windows.Forms.PictureBox()
        CType(Me.Title, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ClearImg, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DeadImg, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'CallBackTimer
        '
        Me.CallBackTimer.Enabled = True
        Me.CallBackTimer.Interval = 33
        '
        'StartBtn
        '
        Me.StartBtn.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.StartBtn.ForeColor = System.Drawing.Color.Coral
        Me.StartBtn.Image = Global.VBGameProject.My.Resources.Resources.BtnStart
        Me.StartBtn.Location = New System.Drawing.Point(1250, 360)
        Me.StartBtn.Name = "StartBtn"
        Me.StartBtn.Size = New System.Drawing.Size(310, 110)
        Me.StartBtn.TabIndex = 0
        Me.StartBtn.UseVisualStyleBackColor = False
        '
        'ExitBtn
        '
        Me.ExitBtn.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ExitBtn.ForeColor = System.Drawing.Color.Coral
        Me.ExitBtn.Image = Global.VBGameProject.My.Resources.Resources.BtnExit
        Me.ExitBtn.Location = New System.Drawing.Point(1250, 720)
        Me.ExitBtn.Name = "ExitBtn"
        Me.ExitBtn.Size = New System.Drawing.Size(310, 110)
        Me.ExitBtn.TabIndex = 1
        Me.ExitBtn.UseVisualStyleBackColor = False
        '
        'GuideBtn
        '
        Me.GuideBtn.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.GuideBtn.ForeColor = System.Drawing.Color.Coral
        Me.GuideBtn.Image = Global.VBGameProject.My.Resources.Resources.BtnGuide
        Me.GuideBtn.Location = New System.Drawing.Point(1250, 480)
        Me.GuideBtn.Name = "GuideBtn"
        Me.GuideBtn.Size = New System.Drawing.Size(310, 110)
        Me.GuideBtn.TabIndex = 2
        Me.GuideBtn.UseVisualStyleBackColor = False
        '
        'NextBtn1
        '
        Me.NextBtn1.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.NextBtn1.Enabled = False
        Me.NextBtn1.ForeColor = System.Drawing.Color.Coral
        Me.NextBtn1.Image = Global.VBGameProject.My.Resources.Resources.BtnNext
        Me.NextBtn1.Location = New System.Drawing.Point(1250, 720)
        Me.NextBtn1.Name = "NextBtn1"
        Me.NextBtn1.Size = New System.Drawing.Size(310, 110)
        Me.NextBtn1.TabIndex = 3
        Me.NextBtn1.UseVisualStyleBackColor = False
        Me.NextBtn1.Visible = False
        '
        'ConfigBtn
        '
        Me.ConfigBtn.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ConfigBtn.ForeColor = System.Drawing.Color.Coral
        Me.ConfigBtn.Image = Global.VBGameProject.My.Resources.Resources.BtnConfig
        Me.ConfigBtn.Location = New System.Drawing.Point(1250, 600)
        Me.ConfigBtn.Name = "ConfigBtn"
        Me.ConfigBtn.Size = New System.Drawing.Size(310, 110)
        Me.ConfigBtn.TabIndex = 4
        Me.ConfigBtn.UseVisualStyleBackColor = False
        '
        'NextBtn2
        '
        Me.NextBtn2.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.NextBtn2.Enabled = False
        Me.NextBtn2.ForeColor = System.Drawing.Color.Coral
        Me.NextBtn2.Image = Global.VBGameProject.My.Resources.Resources.BtnNext
        Me.NextBtn2.Location = New System.Drawing.Point(1250, 720)
        Me.NextBtn2.Name = "NextBtn2"
        Me.NextBtn2.Size = New System.Drawing.Size(310, 110)
        Me.NextBtn2.TabIndex = 5
        Me.NextBtn2.UseVisualStyleBackColor = False
        Me.NextBtn2.Visible = False
        '
        'EasyBtn
        '
        Me.EasyBtn.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.EasyBtn.Enabled = False
        Me.EasyBtn.ForeColor = System.Drawing.Color.Coral
        Me.EasyBtn.Image = Global.VBGameProject.My.Resources.Resources.BtnEasy
        Me.EasyBtn.Location = New System.Drawing.Point(300, 150)
        Me.EasyBtn.Name = "EasyBtn"
        Me.EasyBtn.Size = New System.Drawing.Size(310, 110)
        Me.EasyBtn.TabIndex = 6
        Me.EasyBtn.UseVisualStyleBackColor = False
        Me.EasyBtn.Visible = False
        '
        'NormalBtn
        '
        Me.NormalBtn.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.NormalBtn.Enabled = False
        Me.NormalBtn.ForeColor = System.Drawing.Color.Coral
        Me.NormalBtn.Image = Global.VBGameProject.My.Resources.Resources.BtnNormal
        Me.NormalBtn.Location = New System.Drawing.Point(650, 150)
        Me.NormalBtn.Name = "NormalBtn"
        Me.NormalBtn.Size = New System.Drawing.Size(310, 110)
        Me.NormalBtn.TabIndex = 7
        Me.NormalBtn.UseVisualStyleBackColor = False
        Me.NormalBtn.Visible = False
        '
        'HardBtn
        '
        Me.HardBtn.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.HardBtn.Enabled = False
        Me.HardBtn.ForeColor = System.Drawing.Color.Coral
        Me.HardBtn.Image = Global.VBGameProject.My.Resources.Resources.BtnHard
        Me.HardBtn.Location = New System.Drawing.Point(1000, 150)
        Me.HardBtn.Name = "HardBtn"
        Me.HardBtn.Size = New System.Drawing.Size(310, 110)
        Me.HardBtn.TabIndex = 8
        Me.HardBtn.UseVisualStyleBackColor = False
        Me.HardBtn.Visible = False
        '
        'Title
        '
        Me.Title.BackgroundImage = Global.VBGameProject.My.Resources.Resources.Title
        Me.Title.InitialImage = Global.VBGameProject.My.Resources.Resources.Title
        Me.Title.Location = New System.Drawing.Point(300, 88)
        Me.Title.Name = "Title"
        Me.Title.Size = New System.Drawing.Size(602, 201)
        Me.Title.TabIndex = 9
        Me.Title.TabStop = False
        '
        'ClearImg
        '
        Me.ClearImg.BackgroundImage = Global.VBGameProject.My.Resources.Resources.Clear
        Me.ClearImg.Enabled = False
        Me.ClearImg.Location = New System.Drawing.Point(87, 438)
        Me.ClearImg.Name = "ClearImg"
        Me.ClearImg.Size = New System.Drawing.Size(903, 301)
        Me.ClearImg.TabIndex = 10
        Me.ClearImg.TabStop = False
        Me.ClearImg.Visible = False
        '
        'DeadImg
        '
        Me.DeadImg.BackgroundImage = Global.VBGameProject.My.Resources.Resources.Dead
        Me.DeadImg.Enabled = False
        Me.DeadImg.Location = New System.Drawing.Point(87, 438)
        Me.DeadImg.Name = "DeadImg"
        Me.DeadImg.Size = New System.Drawing.Size(903, 301)
        Me.DeadImg.TabIndex = 11
        Me.DeadImg.TabStop = False
        Me.DeadImg.Visible = False
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(1584, 861)
        Me.Controls.Add(Me.DeadImg)
        Me.Controls.Add(Me.ClearImg)
        Me.Controls.Add(Me.Title)
        Me.Controls.Add(Me.HardBtn)
        Me.Controls.Add(Me.NormalBtn)
        Me.Controls.Add(Me.EasyBtn)
        Me.Controls.Add(Me.NextBtn2)
        Me.Controls.Add(Me.ConfigBtn)
        Me.Controls.Add(Me.NextBtn1)
        Me.Controls.Add(Me.GuideBtn)
        Me.Controls.Add(Me.ExitBtn)
        Me.Controls.Add(Me.StartBtn)
        Me.DoubleBuffered = True
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Form1"
        CType(Me.Title, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ClearImg, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DeadImg, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents CallBackTimer As Timer
    Friend WithEvents StartBtn As Button
    Friend WithEvents ExitBtn As Button
    Friend WithEvents GuideBtn As Button
    Friend WithEvents NextBtn1 As Button
    Friend WithEvents ConfigBtn As Button
    Friend WithEvents NextBtn2 As Button
    Friend WithEvents EasyBtn As Button
    Friend WithEvents NormalBtn As Button
    Friend WithEvents HardBtn As Button
    Friend WithEvents Title As PictureBox
    Friend WithEvents ClearImg As PictureBox
    Friend WithEvents DeadImg As PictureBox
End Class
