Imports System.Drawing.Imaging
Imports System.Net.Mime.MediaTypeNames
Imports System.Net.Security
Imports System.Security.Cryptography
Imports System.Threading

Public Class Form1
    '탄 데이터
    Structure BulletData
        Dim xpos As Integer
        Dim ypos As Integer
        Dim direction As Integer
        Dim bulletType As Integer           ' 0 = notboss, 1 = boss missile, 2 = boss gun
        Dim bulletImg As Bitmap
    End Structure

    '몹 데이터
    Structure MonsterData
        Dim xpos As Integer
        Dim ypos As Integer
        Dim monsterType As Integer          ' 0 = Sphere, 1 = Drone, 2 = HeavyDrone, 3 = Boss
        Dim hp As Integer
        Dim act As Integer  'move , fire/ if type = boss, 0 = appear
        Dim weaponCasting As Integer
        Dim monsterImg As Bitmap
        Dim monsterFD As FrameDimension
        Dim monsterFC As Integer
        Dim monsterFN As UInteger
        Dim isDead As Boolean
        Dim isWeaponReady As Boolean
    End Structure

    '폭발 연출 데이터
    Structure BurstData
        Dim xpos As Integer
        Dim ypos As Integer
        Dim burstType As Integer            ' 0 = small/aerial, 1 = large/ground, 
        Dim burstImg As Bitmap
        Dim burstFD As FrameDimension
        Dim burstFC As Integer
        Dim burstFN As UInteger
    End Structure

    Private MainGameLoopTimer As System.Timers.Timer
    Private GameGuideLoopTimer As System.Timers.Timer

    Private bgImg1 As Bitmap
    Private bgImg2 As Bitmap
    Private hpImage As Bitmap
    Private guideImg As Bitmap

    Dim rnd As New Random
    Dim spawnPoint As Integer = 0

    '플레이어 데이터
    Dim playerImg As Bitmap
    Dim xpos As Integer
    Dim ypos As Integer
    Dim playerHP As Integer
    Dim playerFD As FrameDimension
    Dim playerFC As Integer
    Dim playerFN As UInteger = 0
    Dim playerDirection As Integer = 4 '1 = right, 2 = left, 3 = up, 4 = down
    Dim dodgeCoolTime As Integer = 0

    Dim killCount As Integer = 0
    Dim stage As Integer = 0

    Dim missileStart As Integer = 0
    Dim missileDelay As Integer = 0
    Dim missilePoint As Integer = 0

    'Dim monsterAct As Integer = 0   'move , fire
    Dim MonsterArray As New ArrayList
    Dim burstArray As New ArrayList
    Dim currentKeys As New ArrayList
    Dim playerBulletArray As New ArrayList
    Dim MonsterBulletArray As New ArrayList
    Dim MissileArray As New ArrayList
    'Dim spawnPointArray As New ArrayList

    Dim isEasy, isNormal, isHard As Boolean

    Dim gameSounds As New GameSounds

    Dim isImgL As Boolean = False
    Dim isImgR As Boolean = False
    Dim isImgD As Boolean = False
    Dim isImgU As Boolean = False
    Dim isFiring As Boolean = False
    Dim isBossOn As Boolean = False

    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        e.SuppressKeyPress = True   '비프음 제거
        If Not currentKeys.Contains(e.KeyCode) Then
            currentKeys.Add(e.KeyCode)
        End If
    End Sub

    Private Sub Form1_KeyUp(sender As Object, e As KeyEventArgs) Handles MyBase.KeyUp
        currentKeys.Remove(e.KeyCode)
        If playerDirection = 1 Then
            playerImg = My.Resources.Player_Waiting_toR
        ElseIf playerDirection = 2 Then
            playerImg = My.Resources.Player_Waiting_toL
        ElseIf playerDirection = 3 Then
            playerImg = My.Resources.Player_Waiting_toU
        ElseIf playerDirection = 4 Then
            playerImg = My.Resources.Player_Waiting_toD
        End If
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        isImgR = True
        isImgL = False
        isNormal = True

        bgImg1 = My.Resources.bg
        bgImg2 = My.Resources.bgstage5front
        hpImage = My.Resources.HP
        guideImg = My.Resources.Guide

        playerImg = My.Resources.Player_Waiting_toD
        playerFD = New FrameDimension(playerImg.FrameDimensionsList(0))
        playerFC = playerImg.GetFrameCount(playerFD)

        gameSounds.AddSound("background", "sound/background.mp3")
        gameSounds.AddSound("btnPressed", "sound/btnPressed.mp3")
        gameSounds.SetVolume("background", 300)

        MainGameLoopTimer = New System.Timers.Timer(40)
        AddHandler MainGameLoopTimer.Elapsed, AddressOf PlayerLoopFunction
        AddHandler MainGameLoopTimer.Elapsed, AddressOf MonsterLoopFunction
        AddHandler MainGameLoopTimer.Elapsed, AddressOf BattleLoopFunction

        GameGuideLoopTimer = New System.Timers.Timer(40)
        AddHandler GameGuideLoopTimer.Elapsed, AddressOf PlayerLoopFunction
        AddHandler GameGuideLoopTimer.Elapsed, AddressOf GuideLoopFunction
        AddHandler GameGuideLoopTimer.Elapsed, AddressOf BattleLoopFunction

        'monsterLoopTimer = New System.Timers.Timer(100)
        'AddHandler monsterLoopTimer.Elapsed, AddressOf MonsterLoopFunction
        'monsterLoopTimer.AutoReset = True
        'monsterLoopTimer.Enabled = True
    End Sub

    Private Sub PlayerLoopFunction()
        playerImg.SelectActiveFrame(playerFD, playerFN Mod playerFC)

        For i = 0 To currentKeys.Count - 1
            If currentKeys(i) = 13 Then
                currentKeys(i) = 0
            End If

            If dodgeCoolTime > 0 And dodgeCoolTime < 25 Then
                dodgeCoolTime += 1
            ElseIf dodgeCoolTime = 25 Then
                dodgeCoolTime = 0
            End If

            Select Case currentKeys(i)
                Case Keys.D     'Move Right
                    playerDirection = 1
                    If isImgR = False Then
                        isImgR = True
                        isImgL = False
                        isImgU = False
                        isImgD = False
                        playerImg = My.Resources.Player_Waiting_toR
                    End If
                    If xpos < 1480 Then
                        xpos += 5
                    End If
                Case Keys.A     'Move Left
                    playerDirection = 2
                    If isImgL = False Then
                        isImgR = False
                        isImgL = True
                        isImgU = False
                        isImgD = False
                        playerImg = My.Resources.Player_Waiting_toL
                    End If
                    If xpos > -20 Then
                        xpos -= 5
                    End If
                Case Keys.W     'Move Up
                    If isImgU = False Then
                        isImgR = False
                        isImgL = False
                        isImgU = True
                        isImgD = False
                        playerImg = My.Resources.Player_Waiting_toU
                    End If
                    playerDirection = 3
                    If ypos > -35 Then
                        ypos -= 5
                    End If
                Case Keys.S     'Move Down
                    playerDirection = 4
                    If isImgD = False Then
                        isImgL = False
                        isImgR = False
                        isImgU = False
                        isImgD = True
                        playerImg = My.Resources.Player_Waiting_toD
                    End If
                    If ypos < 730 Then
                        ypos += 5
                    End If
                Case Keys.Space 'Dodge
                    If dodgeCoolTime = 0 Then
                        dodgeCoolTime += 1
                        If playerDirection = 1 Then
                            If xpos + 100 > 1520 Then
                                xpos = 1480
                            Else
                                xpos += 100
                            End If
                        ElseIf playerDirection = 2 Then
                            If xpos - 100 < 0 Then
                                xpos = 50
                            Else
                                xpos -= 100
                            End If
                        ElseIf playerDirection = 3 Then
                            If ypos - 100 < -35 Then
                                ypos = -35
                            Else
                                ypos -= 100
                            End If
                        ElseIf playerDirection = 4 Then
                            If ypos + 100 > 730 Then
                                ypos = 730
                            Else
                                ypos += 100
                            End If
                        End If
                    End If
                Case Keys.F     'Firing
                    If playerFN Mod playerFC = 0 Then
                        If playerDirection = 1 Then       'Fire to Right
                            playerImg = My.Resources.Player_FireR
                        ElseIf playerDirection = 2 Then   'Fire to Left
                            playerImg = My.Resources.Player_FireL
                        ElseIf playerDirection = 3 Then   'Fire to Up
                            playerImg = My.Resources.Player_FireB
                        ElseIf playerDirection = 4 Then   'Fire to Down
                            playerImg = My.Resources.Player_FireF
                        End If
                        Select Case playerDirection
                            Case 1
                                Dim bullet As BulletData = New BulletData
                                bullet.xpos = xpos + 35
                                bullet.ypos = ypos + 27
                                bullet.direction = 1
                                bullet.bulletImg = My.Resources.Bullet_horizontal
                                playerBulletArray.Add(bullet)
                            Case 2
                                Dim bullet As BulletData = New BulletData
                                bullet.xpos = xpos - 32
                                bullet.ypos = ypos + 27
                                bullet.direction = 2
                                bullet.bulletImg = My.Resources.Bullet_horizontal
                                playerBulletArray.Add(bullet)
                            Case 3
                                Dim bullet As BulletData = New BulletData
                                bullet.xpos = xpos - 25
                                bullet.ypos = ypos + 13
                                bullet.direction = 3
                                bullet.bulletImg = My.Resources.Bullet_vertical
                                playerBulletArray.Add(bullet)
                            Case 4
                                Dim bullet As BulletData = New BulletData
                                bullet.xpos = xpos + 20
                                bullet.ypos = ypos + 10
                                bullet.direction = 4
                                bullet.bulletImg = My.Resources.Bullet_vertical
                                playerBulletArray.Add(bullet)
                        End Select
                    End If
                Case Keys.G     'Grenade
                    MonsterArray.RemoveAt(3)
            End Select
        Next

        For i = 0 To playerBulletArray.Count - 1
            Dim bullet = CType(playerBulletArray(i), BulletData)

            Select Case bullet.direction
                Case 1
                    bullet.xpos += 30
                Case 2
                    bullet.xpos -= 30
                Case 3
                    bullet.ypos -= 30
                Case 4
                    bullet.ypos += 30
            End Select

            playerBulletArray(i) = bullet
        Next

        Dim index As Integer = 0
        While index <> playerBulletArray.Count
            Dim bullet = CType(playerBulletArray(index), BulletData)
            If bullet.xpos > Me.Width Then
                playerBulletArray.RemoveAt(index)
            ElseIf bullet.xpos < -60 Then
                playerBulletArray.RemoveAt(index)
            ElseIf bullet.ypos > Me.Width Then
                playerBulletArray.RemoveAt(index)
            ElseIf bullet.ypos < -50 Then
                playerBulletArray.RemoveAt(index)
            Else
                index += 1
            End If
        End While

        playerFN += 1
        Invalidate()
    End Sub

    Private Sub MonsterLoopFunction()

        If stage < 5 Then
            If MonsterArray.Count < 5 Then
                Dim monster As MonsterData = New MonsterData
                spawnPoint = rnd.Next(0, 4)
                Select Case spawnPoint  '몬스터 스폰 지점 4 개 지정
                    Case 0
                        monster.xpos = 1700
                        monster.ypos = 300
                    Case 1
                        monster.xpos = 1700
                        monster.ypos = 700
                    Case 2
                        monster.xpos = -100
                        monster.ypos = 700
                    Case 3
                        monster.xpos = -100
                        monster.ypos = -300
                End Select

                If stage = 0 Then       'staeg = 0, monsterType = 0 스폰
                    monster.monsterType = 0
                    monster.monsterImg = My.Resources.sphereR
                    monster.hp = 100
                ElseIf stage = 1 Then   'staeg = 1, monsterType = 0와 monsterType = 1 스폰
                    Dim i As Integer = rnd.Next(0, 2)
                    If i = 0 Then
                        monster.monsterType = 0
                        monster.monsterImg = My.Resources.sphereR
                        monster.hp = 100
                    ElseIf i = 1 Then
                        monster.monsterType = 1
                        monster.monsterImg = My.Resources.DroneR
                        monster.hp = 150
                    End If
                ElseIf stage = 2 Then   'staeg = 2, monsterType = 1 스폰
                    monster.monsterType = 1
                    monster.monsterImg = My.Resources.DroneR
                    monster.hp = 150
                ElseIf stage = 3 Then   'staeg = 3, monsterType = 1과 monsterType = 2 스폰
                    Dim i As Integer = rnd.Next(1, 3)
                    If i = 1 Then
                        monster.monsterType = 1
                        monster.monsterImg = My.Resources.DroneR
                        monster.hp = 150
                    ElseIf i = 2 Then
                        monster.monsterType = 2
                        monster.monsterImg = My.Resources.HeavyDroneR
                        monster.hp = 300
                    End If
                ElseIf stage = 4 Then   'staeg = 3, monsterType = 2 스폰
                    monster.monsterType = 2
                    monster.monsterImg = My.Resources.HeavyDroneR
                    monster.hp = 300
                End If

                monster.monsterFD = New FrameDimension(monster.monsterImg.FrameDimensionsList(0))
                monster.monsterFC = monster.monsterImg.GetFrameCount(monster.monsterFD)
                monster.monsterFN = 0

                monster.weaponCasting = 0
                monster.act = 0
                MonsterArray.Add(monster)
            End If
        ElseIf stage = 5 Then       'monsterType = 3 스폰 및 등장 모션 출력
            If MonsterArray.Count = 0 And isBossOn = False Then
                Dim boss As MonsterData = New MonsterData

                If isEasy = True Then
                    boss.hp = 2000
                ElseIf isNormal = True Then
                    boss.hp = 3000
                ElseIf isHard = True Then
                    boss.hp = 5000
                End If

                boss.xpos = 277
                boss.ypos = -140
                boss.monsterType = 3

                xpos = 700
                ypos = 600

                boss.monsterImg = My.Resources.Boss_Appear
                boss.monsterFD = New FrameDimension(boss.monsterImg.FrameDimensionsList(0))
                boss.monsterFC = boss.monsterImg.GetFrameCount(boss.monsterFD)
                boss.monsterFN = 0

                boss.weaponCasting = 0
                boss.act = 0

                boss.isWeaponReady = False
                isBossOn = True
                bgImg1 = My.Resources.bgstage5

                MonsterArray.Add(boss)
                MonsterBulletArray.Clear()
            End If
        End If

        For i = 0 To MonsterArray.Count - 1
            Dim monster = CType(MonsterArray(i), MonsterData)

            Try
                monster.monsterImg.SelectActiveFrame(monster.monsterFD, monster.monsterFN Mod monster.monsterFC)
            Catch ex As Exception
                Debug.Write("error")
            End Try

            'sphere.sphereImage.SelectActiveFrame(monsterFD, monsterFN Mod monsterFC)

            '몬스터 무기 발사 쿨타임
            If monster.monsterType = 0 Then     'monsterType = 0 무기 발사
                If monster.weaponCasting = 0 Then
                    monster.act = rnd.Next(0, 13)
                ElseIf monster.weaponCasting = 60 Then
                    monster.weaponCasting = 0
                    monster.act = rnd.Next(0, 13)
                ElseIf monster.weaponCasting > 20 Then
                    monster.act = rnd.Next(0, 12)
                    monster.weaponCasting += 1
                ElseIf monster.weaponCasting = 10 Then
                    If monster.xpos < xpos Then
                        Dim bullet As BulletData = New BulletData
                        bullet.xpos = monster.xpos + 15
                        bullet.ypos = monster.ypos
                        bullet.direction = 0
                        bullet.bulletType = 0
                        bullet.bulletImg = My.Resources.SmallBullet
                        MonsterBulletArray.Add(bullet)
                    ElseIf monster.xpos > xpos Then
                        Dim bullet As BulletData = New BulletData
                        bullet.xpos = monster.xpos - 15
                        bullet.ypos = monster.ypos
                        bullet.direction = 1
                        bullet.bulletType = 0
                        bullet.bulletImg = My.Resources.SmallBullet
                        MonsterBulletArray.Add(bullet)
                    End If
                    monster.weaponCasting += 1
                Else
                    monster.weaponCasting += 1
                End If
            ElseIf monster.monsterType = 1 Then     'monsterType = 1 무기 발사
                If monster.weaponCasting = 0 Then
                    monster.act = rnd.Next(0, 13)
                ElseIf monster.weaponCasting = 60 Then
                    monster.weaponCasting = 0
                    monster.act = rnd.Next(0, 13)
                ElseIf monster.weaponCasting > 20 Then
                    monster.act = rnd.Next(0, 12)
                    monster.weaponCasting += 1
                ElseIf monster.weaponCasting = 10 Then
                    If monster.xpos < xpos Then
                        For j = 0 To 1
                            Dim bullet As BulletData = New BulletData
                            bullet.xpos = monster.xpos + 30
                            bullet.ypos = monster.ypos - 5 + (j * 10)
                            bullet.direction = 0
                            bullet.bulletType = 0
                            bullet.bulletImg = My.Resources.SmallBullet
                            MonsterBulletArray.Add(bullet)
                        Next
                    ElseIf monster.xpos > xpos Then
                        For j = 0 To 1
                            Dim bullet As BulletData = New BulletData
                            bullet.xpos = monster.xpos - 30
                            bullet.ypos = monster.ypos + 5 - (j * 10)
                            bullet.direction = 1
                            bullet.bulletType = 0
                            bullet.bulletImg = My.Resources.SmallBullet
                            MonsterBulletArray.Add(bullet)
                        Next
                    End If
                    monster.weaponCasting += 1
                Else
                    monster.weaponCasting += 1
                End If
            ElseIf monster.monsterType = 2 Then     'monsterType = 2 무기 발사
                If monster.weaponCasting = 0 Then
                    monster.act = rnd.Next(0, 13)
                ElseIf monster.weaponCasting = 60 Then
                    monster.weaponCasting = 0
                    monster.act = rnd.Next(0, 13)
                ElseIf monster.weaponCasting > 20 Then
                    monster.act = rnd.Next(0, 12)
                    monster.weaponCasting += 1
                ElseIf monster.weaponCasting = 10 Then
                    Dim amount As Integer
                    If isEasy = True Then
                        amount = 0
                    ElseIf isNormal = True Then
                        amount = 1
                    ElseIf isHard = True Then
                        amount = 2
                    End If
                    If monster.xpos < xpos Then
                        For j = 0 To amount
                            Dim bullet As BulletData = New BulletData
                            bullet.xpos = monster.xpos + 250 - (j * 75)
                            bullet.ypos = monster.ypos + 50
                            bullet.direction = 0
                            bullet.bulletType = 0
                            bullet.bulletImg = My.Resources.LargeBullet
                            MonsterBulletArray.Add(bullet)
                        Next
                        For j = 0 To amount
                            Dim bullet As BulletData = New BulletData
                            bullet.xpos = monster.xpos + 250 - (j * 75)
                            bullet.ypos = monster.ypos + 100
                            bullet.direction = 0
                            bullet.bulletType = 0
                            bullet.bulletImg = My.Resources.LargeBullet
                            MonsterBulletArray.Add(bullet)
                        Next
                    ElseIf monster.xpos > xpos Then
                        For j = 0 To amount
                            Dim bullet As BulletData = New BulletData
                            bullet.xpos = monster.xpos - 50 + (j * 75)
                            bullet.ypos = monster.ypos + 50
                            bullet.direction = 1
                            bullet.bulletType = 0
                            bullet.bulletImg = My.Resources.LargeBullet
                            MonsterBulletArray.Add(bullet)
                        Next
                        For j = 0 To amount
                            Dim bullet As BulletData = New BulletData
                            bullet.xpos = monster.xpos - 50 + (j * 75)
                            bullet.ypos = monster.ypos + 100
                            bullet.direction = 1
                            bullet.bulletType = 0
                            bullet.bulletImg = My.Resources.LargeBullet
                            MonsterBulletArray.Add(bullet)
                        Next
                    End If
                    monster.weaponCasting += 1
                Else
                    monster.weaponCasting += 1
                End If
            ElseIf monster.monsterType = 3 Then     'monsterType = 3 생성
                If monster.act = 0 And monster.monsterFC = monster.monsterFN Then
                    monster.act = 1
                    monster.monsterImg = My.Resources.Boss_Waiting
                    monster.monsterFD = New FrameDimension(monster.monsterImg.FrameDimensionsList(0))
                    monster.monsterFC = monster.monsterImg.GetFrameCount(monster.monsterFD)
                    monster.monsterFN = 0
                End If
            End If

            If monster.monsterType = 0 Then         'monsterType = 0 이동
                Select Case monster.act
                    Case 0, 1, 2, 3, 4, 5
                        If monster.xpos > xpos Then
                            monster.xpos -= 5
                            monster.monsterImg = My.Resources.SphereL
                        ElseIf monster.xpos < xpos Then
                            monster.xpos += 5
                            monster.monsterImg = My.Resources.sphereR
                        End If
                    Case 6, 7, 8, 9, 10, 11
                        If monster.ypos > ypos + 20 Then
                            monster.ypos -= 5
                        ElseIf monster.ypos < ypos + 20 Then
                            monster.ypos += 5
                        End If
                    Case 12
                        monster.weaponCasting += 1
                        monster.act = 20
                End Select
            ElseIf monster.monsterType = 1 Then     'monsterType = 1 이동
                Select Case monster.act
                    Case 0, 1, 2, 3, 4, 5
                        If monster.xpos > xpos Then
                            monster.xpos -= 5
                            monster.monsterImg = My.Resources.DroneL
                        ElseIf monster.xpos < xpos Then
                            monster.xpos += 5
                            monster.monsterImg = My.Resources.DroneR
                        End If
                    Case 6, 7, 8, 9, 10, 11
                        If monster.ypos > ypos + 20 Then
                            monster.ypos -= 5
                        ElseIf monster.ypos < ypos + 20 Then
                            monster.ypos += 5
                        End If
                    Case 12
                        monster.weaponCasting += 1
                        monster.act = 20
                End Select
            ElseIf monster.monsterType = 2 Then     'monsterType = 2 이동
                Select Case monster.act
                    Case 0, 1, 2, 3, 4, 5
                        If monster.xpos > xpos - 100 Then
                            monster.xpos -= 5
                            monster.monsterImg = My.Resources.HeavyDroneL
                        ElseIf monster.xpos < xpos - 100 Then
                            monster.xpos += 5
                            monster.monsterImg = My.Resources.HeavyDroneR
                        End If
                    Case 6, 7, 8, 9, 10, 11
                        If monster.ypos > ypos - 80 Then
                            monster.ypos -= 5
                        ElseIf monster.ypos < ypos - 80 Then
                            monster.ypos += 5
                        End If
                    Case 12
                        monster.weaponCasting += 1
                        monster.act = 20
                End Select
            ElseIf monster.monsterType = 3 Then     'monsterType = 3 등장 모션 끝
                If monster.weaponCasting > 0 Then
                    monster.weaponCasting -= 1
                End If

                If monster.act = 1 And monster.monsterFC = monster.monsterFN Then
                    If monster.weaponCasting = 0 Then
                        monster.act = rnd.Next(2, 4)

                    ElseIf monster.weaponCasting > 0 Then
                        monster.act = 3
                    End If

                    Select Case monster.act
                        Case 2
                            monster.monsterImg = My.Resources.Boss_AttackA
                            monster.weaponCasting += 120
                        Case 3
                            monster.monsterImg = My.Resources.Boss_AttackB
                            playerHP -= 10
                            'Case 4
                            '    monster.monsterImg = My.Resources.Boss_AttackB
                    End Select

                    monster.monsterFD = New FrameDimension(monster.monsterImg.FrameDimensionsList(0))
                    monster.monsterFC = monster.monsterImg.GetFrameCount(monster.monsterFD)
                    monster.monsterFN = 0

                    If monster.act = 2 Then
                        For j = 0 To 5
                            Dim missile As BulletData = New BulletData
                            missile.xpos = xpos
                            missile.ypos = -200
                            missile.bulletImg = My.Resources.Missile
                            missile.bulletType = 1
                            MissileArray.Add(missile)
                        Next
                    End If

                    monster.act = 0
                End If

            End If
            monster.monsterFN += 1

            MonsterArray(i) = monster
        Next



        For i = 0 To MonsterBulletArray.Count - 1
            Dim bullet = CType(MonsterBulletArray(i), BulletData)

            Select Case bullet.direction
                Case 0
                    bullet.xpos += 20
                Case 1
                    bullet.xpos -= 20
            End Select

            MonsterBulletArray(i) = bullet
        Next

        Dim indexA As Integer = 0
        While indexA <> MonsterBulletArray.Count
            Dim bullet = CType(MonsterBulletArray(indexA), BulletData)
            If bullet.xpos > Me.Width Then
                MonsterBulletArray.RemoveAt(indexA)
            ElseIf bullet.xpos < -60 Then
                MonsterBulletArray.RemoveAt(indexA)
            Else
                indexA += 1
            End If
        End While

        If MissileArray.Count = 0 Then
            missileStart = 0
        End If

        If missileStart < 50 Then
            missileStart += 1
        ElseIf missileStart = 50 Then
            Dim indexB As Integer = 0
            While indexB <> MissileArray.Count
                Dim missile = CType(MissileArray(indexB), BulletData)
                If missileDelay = 0 Then
                    missilePoint = xpos - 170
                    missileDelay += 1
                ElseIf missileDelay < 100 Then
                    missileDelay += 1
                ElseIf missileDelay = 100 Then
                    If missile.ypos < ypos Then

                        If missile.xpos < xpos + 10 And missile.xpos > xpos - 10 And missile.ypos < ypos + 10 And missile.ypos < ypos - 10 Then
                            playerHP -= 10
                        End If
                        MissileArray.RemoveAt(indexB)

                        Dim burst As BurstData = New BurstData
                        burst.xpos = missilePoint
                        burst.ypos = ypos - 350
                        burst.burstImg = My.Resources.BurstType1
                        burst.burstFD = New FrameDimension(burst.burstImg.FrameDimensionsList(0))
                        burst.burstFC = burst.burstImg.GetFrameCount(burst.burstFD)
                        burst.burstFN = 0
                        burst.burstType = 1

                        burstArray.Add(burst)

                        missileDelay = 0

                        If playerHP <= 0 Then
                            MainGameLoopTimer.AutoReset = False
                            MainGameLoopTimer.Enabled = False

                            StartBtn.Enabled = True
                            StartBtn.Visible = True

                            GuideBtn.Enabled = True
                            GuideBtn.Visible = True

                            ConfigBtn.Enabled = True
                            ConfigBtn.Visible = True

                            ExitBtn.Enabled = True
                            ExitBtn.Visible = True

                            Title.Visible = True
                            Title.Enabled = True

                            NextBtn1.Visible = True
                            NextBtn1.Enabled = True

                            isBossOn = False

                            If MonsterArray.Count > 0 Then
                                MonsterArray.Clear()
                            End If

                            If MonsterBulletArray.Count > 0 Then
                                MonsterBulletArray.Clear()
                            End If

                            If MissileArray.Count > 0 Then
                                MissileArray.Clear()
                            End If

                            If burstArray.Count > 0 Then
                                burstArray.Clear()
                            End If

                            bgImg1 = My.Resources.bg

                            Invalidate()
                        End If
                    End If
                End If

                indexB += 1
            End While
        End If

        Invalidate()
    End Sub

    Private Sub BattleLoopFunction()

        Try
            For i = 0 To MonsterArray.Count - 1
                Dim monster = CType(MonsterArray(i), MonsterData)
                For j = 0 To playerBulletArray.Count - 1
                    Dim bullet = CType(playerBulletArray(j), BulletData)
                    If monster.monsterType = 0 Then     'monsterType = 0 판정
                        If bullet.xpos > monster.xpos - 40 And bullet.xpos < monster.xpos + 40 And bullet.ypos > monster.ypos - 40 And bullet.ypos < monster.ypos + 40 Then
                            monster.hp -= 20
                            playerBulletArray.RemoveAt(j)
                            If monster.hp < 0 Then
                                monster.isDead = True
                            End If

                            MonsterArray(i) = monster

                            If monster.isDead = True Then
                                Dim burst As BurstData = New BurstData

                                burst.burstImg = My.Resources.BurstType0
                                burst.burstFD = New FrameDimension(burst.burstImg.FrameDimensionsList(0))
                                burst.burstFC = burst.burstImg.GetFrameCount(burst.burstFD)
                                burst.burstFN = 0

                                burst.xpos = monster.xpos
                                burst.ypos = monster.ypos
                                burst.burstType = 0

                                burstArray.Add(burst)

                                MonsterArray.RemoveAt(i)
                                killCount += 1
                            End If
                        End If
                    ElseIf monster.monsterType = 1 Then     'monsterType = 1 판정
                        If bullet.xpos > monster.xpos - 40 And bullet.xpos < monster.xpos + 40 And bullet.ypos > monster.ypos - 20 And bullet.ypos < monster.ypos + 20 Then
                            monster.hp -= 20
                            playerBulletArray.RemoveAt(j)
                            If monster.hp < 0 Then
                                monster.isDead = True
                            End If

                            MonsterArray(i) = monster

                            If monster.isDead = True Then
                                Dim burst As BurstData = New BurstData

                                burst.burstImg = My.Resources.BurstType0
                                burst.burstFD = New FrameDimension(burst.burstImg.FrameDimensionsList(0))
                                burst.burstFC = burst.burstImg.GetFrameCount(burst.burstFD)
                                burst.burstFN = 0

                                burst.xpos = monster.xpos
                                burst.ypos = monster.ypos
                                burst.burstType = 0

                                burstArray.Add(burst)


                                MonsterArray.RemoveAt(i)
                                killCount += 1
                            End If
                        End If
                    ElseIf monster.monsterType = 2 Then     'monsterType = 2 판정
                        If bullet.xpos > monster.xpos - 30 And bullet.xpos < monster.xpos + 230 And bullet.ypos > monster.ypos + 10 And bullet.ypos < monster.ypos + 170 Then
                            monster.hp -= 20
                            playerBulletArray.RemoveAt(j)
                            If monster.hp < 0 Then
                                monster.isDead = True
                            End If
                        End If

                        MonsterArray(i) = monster

                        If monster.isDead = True Then
                            Dim burst As BurstData = New BurstData

                            burst.burstImg = My.Resources.BurstType1
                            burst.burstFD = New FrameDimension(burst.burstImg.FrameDimensionsList(0))
                            burst.burstFC = burst.burstImg.GetFrameCount(burst.burstFD)
                            burst.burstFN = 0

                            burst.xpos = monster.xpos - 75
                            burst.ypos = monster.ypos - 250
                            burst.burstType = 1

                            burstArray.Add(burst)

                            MonsterArray.RemoveAt(i)
                            killCount += 1
                        End If
                    ElseIf monster.monsterType = 3 Then     'monsterType = 3 판정
                        If bullet.xpos > Me.Width / 2 - 300 And bullet.xpos < Me.Width / 2 + 150 And bullet.ypos > Me.Height / 2 - 300 And bullet.ypos < Me.Height / 2 - 50 Then
                            monster.hp -= 20
                            playerBulletArray.RemoveAt(i)
                            If monster.hp < 0 Then
                                monster.isDead = True
                            End If

                            MonsterArray(i) = monster

                            If monster.isDead = True Then

                                MonsterArray.RemoveAt(i)

                                StartBtn.Visible = True
                                StartBtn.Enabled = True

                                GuideBtn.Visible = True
                                GuideBtn.Enabled = True

                                ConfigBtn.Visible = True
                                ConfigBtn.Enabled = True

                                ExitBtn.Visible = True
                                ExitBtn.Enabled = True

                                Title.Visible = True
                                Title.Enabled = True

                                MainGameLoopTimer.AutoReset = False
                                MainGameLoopTimer.Enabled = False

                                bgImg1 = My.Resources.bg

                                Invalidate()
                            End If
                        End If
                    End If
                Next
            Next
        Catch ex As Exception
            Debug.Write("OutOfRange Occur")
        End Try

        Try
            For i = 0 To MonsterBulletArray.Count - 1
                Dim bullet = CType(MonsterBulletArray(i), BulletData)
                If bullet.xpos > xpos And bullet.xpos < xpos + 40 And bullet.ypos > ypos - 40 And bullet.ypos < ypos + 70 Then
                    MonsterBulletArray.RemoveAt(i)
                    playerHP -= 20
                    If playerHP <= 0 Then
                        MainGameLoopTimer.AutoReset = False
                        MainGameLoopTimer.Enabled = False

                        StartBtn.Enabled = True
                        StartBtn.Visible = True

                        GuideBtn.Enabled = True
                        GuideBtn.Visible = True

                        ConfigBtn.Enabled = True
                        ConfigBtn.Visible = True

                        ExitBtn.Enabled = True
                        ExitBtn.Visible = True

                        Title.Visible = True
                        Title.Enabled = True

                        If MonsterArray.Count > 0 Then
                            MonsterArray.Clear()
                        End If

                        If MonsterBulletArray.Count > 0 Then
                            MonsterBulletArray.Clear()
                        End If

                        Invalidate()
                    End If
                End If
            Next
        Catch ex As Exception
            Debug.Write("OutOfRange Occur")
        End Try

        For i = 0 To burstArray.Count - 1
            Try
                Dim burst = CType(burstArray(i), BurstData)
                burst.burstImg.SelectActiveFrame(burst.burstFD, burst.burstFN Mod burst.burstFC)

                '소리 출력 안됨
                If burst.burstFN = 1 Then
                    gameSounds.Play("burst_")
                End If

                burst.burstFN += 1

                If burst.burstFN = burst.burstFC - 1 Then
                    burstArray.RemoveAt(i)
                Else
                    burstArray(i) = burst
                End If
            Catch ex As Exception
                Debug.Write("OutOfRangeException Occur")
            End Try
        Next

        If stage < 5 Then
            If isEasy = True Then
                stage = killCount / 5
            ElseIf isNormal = True Then
                stage = killCount / 10
            ElseIf isHard = True Then
                stage = killCount / 20
            End If
        End If

        Invalidate()
    End Sub

    Private Sub GuideLoopFunction()

        If MonsterArray.Count = 0 Then
            Dim monster As MonsterData = New MonsterData

            monster.monsterImg = My.Resources.HeavyDroneL
            monster.monsterFD = New FrameDimension(monster.monsterImg.FrameDimensionsList(0))
            monster.monsterFC = monster.monsterImg.GetFrameCount(monster.monsterFD)
            monster.monsterFN = 0

            monster.hp = 50
            monster.xpos = 900
            monster.ypos = 320
            monster.monsterType = 2

            MonsterArray.Add(monster)
        End If

        If MonsterArray.Count > 0 Then
            Dim monster = CType(MonsterArray(0), MonsterData)

            monster.monsterImg.SelectActiveFrame(monster.monsterFD, monster.monsterFN Mod monster.monsterFC)
            monster.monsterFN += 1
        End If

        Invalidate()
    End Sub

    Private Sub CallBackTimer_Tick(sender As Object, e As EventArgs) Handles CallBackTimer.Tick

    End Sub

    Private Sub Form1_Paint(sender As Object, e As PaintEventArgs) Handles MyBase.Paint

        e.Graphics.DrawImage(bgImg1, 0, 0)              '필드 이미지 출력

        If MainGameLoopTimer.Enabled = True Then
            Try
                For Each data As MonsterData In MonsterArray
                    e.Graphics.DrawImage(data.monsterImg, data.xpos, data.ypos)    '몬스터 출력
                Next
                If stage >= 5 And isBossOn = True Then
                    e.Graphics.DrawImage(bgImg2, 0, 0)
                End If
                e.Graphics.DrawImage(playerImg, xpos, ypos)     '플레이어 이미지 출력
                For Each data As BulletData In playerBulletArray
                    e.Graphics.DrawImage(data.bulletImg, data.xpos, data.ypos)      '플레이어 탄 출력
                Next
                For Each data As BulletData In MonsterBulletArray
                    e.Graphics.DrawImage(data.bulletImg, data.xpos, data.ypos)      '적 탄 출력
                Next
                For Each data As BulletData In MissileArray
                    e.Graphics.DrawImage(data.bulletImg, data.xpos, data.ypos)
                Next
                For Each data As BurstData In burstArray
                    e.Graphics.DrawImage(data.burstImg, data.xpos, data.ypos)       '폭발 이펙트 출력
                Next

            Catch ex As Exception
                Debug.WriteLine("Img exception Throwing")
            End Try



            '체력바 이미지 출력
            Dim i As Integer
            For i = 0 To playerHP / 20 - 1
                e.Graphics.DrawImage(hpImage, 40 * i, 0)
            Next

        ElseIf GameGuideLoopTimer.Enabled = True Then
            e.Graphics.DrawImage(playerImg, xpos, ypos)         '플레이어 이미지 출력
            e.Graphics.DrawImage(guideImg, 0, 460)              '가이드 이미지 출력

            Try
                For Each data As BulletData In playerBulletArray
                    e.Graphics.DrawImage(data.bulletImg, data.xpos, data.ypos)
                Next
                For Each data As MonsterData In MonsterArray
                    e.Graphics.DrawImage(data.monsterImg, data.xpos, data.ypos)
                Next
                For Each data As BurstData In burstArray
                    e.Graphics.DrawImage(data.burstImg, data.xpos, data.ypos)
                Next
            Catch ex As Exception
                Debug.WriteLine("Img Exception Throwing")
            End Try

            '체력바 이미지 출력
            Dim i As Integer
            For i = 0 To playerHP / 20 - 1
                e.Graphics.DrawImage(hpImage, 40 * i, 0)
            Next
        End If


    End Sub

    Private Sub StartBtn_Click(sender As Object, e As EventArgs) Handles StartBtn.Click     '게임 시작
        gameSounds.Play("btnPressed")
        gameSounds.Play("background")

        StartBtn.Visible = False
        StartBtn.Enabled = False

        ConfigBtn.Visible = False
        ConfigBtn.Enabled = False

        GuideBtn.Visible = False
        GuideBtn.Enabled = False

        ExitBtn.Visible = False
        ExitBtn.Enabled = False

        Title.Visible = False
        Title.Enabled = False

        xpos = 700
        ypos = 350

        playerHP = 200
        playerFN = 0
        playerDirection = 0
        stage = 0
        killCount = 0
        isBossOn = False

        MainGameLoopTimer.AutoReset = True
        MainGameLoopTimer.Enabled = True
    End Sub

    Private Sub GuideBtn_Click(sender As Object, e As EventArgs) Handles GuideBtn.Click     '가이드
        gameSounds.Play("btnPressed")

        StartBtn.Visible = False
        StartBtn.Enabled = False

        ConfigBtn.Visible = False
        ConfigBtn.Enabled = False

        GuideBtn.Visible = False
        GuideBtn.Enabled = False

        ExitBtn.Visible = False
        ExitBtn.Enabled = False

        Title.Visible = False
        Title.Enabled = False

        NextBtn1.Visible = True
        NextBtn1.Enabled = True

        xpos = 700
        ypos = 350
        playerHP = 200

        GameGuideLoopTimer.AutoReset = True
        GameGuideLoopTimer.Enabled = True
    End Sub

    '난이도 조정
    Private Sub ConfigBtn_Click(sender As Object, e As EventArgs) Handles ConfigBtn.Click
        gameSounds.Play("btnPressed")

        StartBtn.Visible = False
        StartBtn.Enabled = False

        ConfigBtn.Visible = False
        ConfigBtn.Enabled = False

        GuideBtn.Visible = False
        GuideBtn.Enabled = False

        ExitBtn.Visible = False
        ExitBtn.Enabled = False

        Title.Visible = False
        Title.Enabled = False

        EasyBtn.Visible = True
        EasyBtn.Enabled = True

        NormalBtn.Visible = True
        NormalBtn.Enabled = True

        HardBtn.Visible = True
        HardBtn.Enabled = True

        NextBtn2.Visible = True
        NextBtn2.Enabled = True

    End Sub

    '난이도 선택
    Private Sub EasyBtn_Click(sender As Object, e As EventArgs) Handles EasyBtn.Click
        gameSounds.Play("btnPressed")

        isEasy = True
        EasyBtn.Enabled = False

        If isNormal = True Then
            isNormal = False
            NormalBtn.Enabled = True
        End If

        If isHard = True Then
            isHard = False
            HardBtn.Enabled = True
        End If
    End Sub

    Private Sub NormalBtn_Click(sender As Object, e As EventArgs) Handles NormalBtn.Click
        gameSounds.Play("btnPressed")

        isNormal = True
        NormalBtn.Enabled = False

        If isEasy = True Then
            isEasy = False
            EasyBtn.Enabled = True
        End If

        If isHard = True Then
            isHard = False
            HardBtn.Enabled = True
        End If
    End Sub

    Private Sub HardBtn_Click(sender As Object, e As EventArgs) Handles HardBtn.Click
        gameSounds.Play("btnPressed")

        isHard = True
        HardBtn.Enabled = False

        If isEasy = True Then
            isEasy = False
            EasyBtn.Enabled = True
        End If

        If isNormal = True Then
            isNormal = False
            NormalBtn.Enabled = True
        End If
    End Sub

    '가이드에서 메인화면으로
    Private Sub NextBtn1_Click(sender As Object, e As EventArgs) Handles NextBtn1.Click
        gameSounds.Play("btnPressed")

        StartBtn.Visible = True
        StartBtn.Enabled = True

        ConfigBtn.Visible = True
        ConfigBtn.Enabled = True

        GuideBtn.Visible = True
        GuideBtn.Enabled = True

        ExitBtn.Visible = True
        ExitBtn.Enabled = True

        Title.Visible = True
        Title.Enabled = True

        NextBtn1.Visible = False
        NextBtn1.Enabled = False

        GameGuideLoopTimer.AutoReset = False
        GameGuideLoopTimer.Enabled = False

        killCount = 0

        If MonsterArray.Count = 1 Then
            MonsterArray.RemoveAt(0)
        End If

        Invalidate()
    End Sub

    '설정에서 메인화면으로
    Private Sub NextBtn2_Click(sender As Object, e As EventArgs) Handles NextBtn2.Click
        gameSounds.Play("btnPressed")
        StartBtn.Visible = True
        StartBtn.Enabled = True

        ConfigBtn.Visible = True
        ConfigBtn.Enabled = True

        GuideBtn.Visible = True
        GuideBtn.Enabled = True

        ExitBtn.Visible = True
        ExitBtn.Enabled = True

        Title.Visible = True
        Title.Enabled = True

        EasyBtn.Visible = False
        EasyBtn.Enabled = False

        NormalBtn.Visible = False
        NormalBtn.Enabled = False

        HardBtn.Visible = False
        HardBtn.Enabled = False

        NextBtn2.Visible = False
        NextBtn2.Enabled = False

        Invalidate()
    End Sub

    Private Sub ExitBtn_Click(sender As Object, e As EventArgs) Handles ExitBtn.Click       '종료
        End
    End Sub
End Class