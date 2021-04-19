Imports System.IO

Public Class Form1

  

  Dim bmp As Bitmap
  Dim Bg, Bg1, Img As CImage
  Dim SpriteMap As CImage
    Dim SpriteMask As CImage
    Dim DragonIntro, DragonStand, DragonJump, DragonJumpDown, DragonUpperCut, DragonFireball, DragonFireballLow, DragonBurstFireball, DragonCruelSun, DragonUlt, DragonDeath, DragonOutro As CArrFrame
    Dim SM As CCharacter





  Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    'open image for background, assign to bg

    Bg = New CImage
        Bg.OpenImage("F:\Temp Assignment\CGA Sprites Magma Dragoon\File\MegamanX4.bmp")

        Bg.CopyImg(Img)

    Bg.CopyImg(Bg1)

    SpriteMap = New CImage
        SpriteMap.OpenImage("F:\Temp Assignment\CGA Sprites Magma Dragoon\File\MagmaDragoon.bmp")

        SpriteMap.CreateMask(SpriteMask)

        'initialize sprites

        DragonIntro = New CArrFrame
        DragonIntro.Insert(292, 468, 256, 435, 328, 517, 1)
        DragonIntro.Insert(138, 57, 112, 22, 172, 101, 1)
        DragonIntro.Insert(489, 479, 434, 449, 529, 516, 1)
        DragonIntro.Insert(170, 719, 126, 639, 221, 769, 1)
        DragonIntro.Insert(335, 65, 291, 25, 373, 105, 1)
        DragonIntro.Insert(153, 191, 109, 150, 205, 219, 1)

        DragonStand = New CArrFrame
        DragonStand.Insert(138, 57, 112, 22, 172, 101, 1)
        DragonStand.Insert(138, 57, 112, 22, 172, 101, 2)
        DragonStand.Insert(138, 57, 112, 22, 172, 101, 1)

        DragonJump = New CArrFrame
        DragonJump.Insert(371, 473, 335, 437, 413, 516, 1)

        DragonJumpDown = New CArrFrame
        DragonJumpDown.Insert(107, 464, 69, 429, 134, 513, 1)

        DragonUpperCut = New CArrFrame
        DragonUpperCut.Insert(153, 191, 109, 150, 205, 219, 1)
        DragonUpperCut.Insert(249, 183, 210, 149, 305, 223, 1)
        DragonUpperCut.Insert(346, 185, 311, 147, 410, 220, 1)
        DragonUpperCut.Insert(552, 179, 526, 125, 592, 230, 1)

        DragonFireball = New CArrFrame
        DragonFireball.Insert(153, 282, 118, 243, 206, 318, 1)
        DragonFireball.Insert(274, 288, 218, 251, 315, 325, 1)

        DragonFireballLow = New CArrFrame
        DragonFireballLow.Insert(163, 357, 123, 325, 197, 391, 1)
        DragonFireballLow.Insert(267, 365, 216, 332, 315, 392, 1)

        DragonBurstFireball = New CArrFrame
        DragonBurstFireball.Insert(488, 479, 433, 449, 529, 516, 1)
        DragonBurstFireball.Insert(151, 611, 107, 578, 198, 641, 1)

        DragonCruelSun = New CArrFrame
        DragonCruelSun.Insert(447, 612, 390, 562, 487, 651, 1)

        DragonUlt = New CArrFrame
        DragonUlt.Insert(168, 718, 126, 693, 221, 761, 1)
        DragonUlt.Insert(268, 719, 224, 694, 322, 761, 1)

        DragonDeath = New CArrFrame
        DragonDeath.Insert(396, 713, 350, 683, 426, 763, 1)

        DragonOutro = New CArrFrame
        DragonOutro.Insert(561, 738, 523, 701, 603, 754, 1)
        DragonOutro.Insert(477, 737, 440, 699, 518, 755, 1)
        SM = New CCharacter
    ReDim SM.ArrSprites(3)
        SM.ArrSprites(0) = DragonDeath
        SM.ArrSprites(1) = DragonStand
        SM.ArrSprites(2) = DragonCruelSun
        SM.ArrSprites(3) = DragonJumpDown

        SM.PosX = 300
    SM.PosY = 200
    SM.Vx = -5
    SM.Vy = 0
    SM.State(StateSplitMushroom.Walk, 0)
    SM.FDir = FaceDir.Left

    bmp = New Bitmap(Img.Width, Img.Height)


    DisplayImg()
    ResizeImg()




    Timer1.Enabled = True
  End Sub

  Sub PutSprite(ByVal c As CCharacter)

    Dim i, j As Integer
    'set background
    For i = 0 To Img.Width - 1
      For j = 0 To Img.Height - 1
        Img.Elmt(i, j) = Bg1.Elmt(i, j)
      Next
    Next

    Dim EF As CElmtFrame = c.ArrSprites(c.IdxArrSprites).Elmt(c.FrameIdx)
    Dim spritewidth = EF.Right - EF.Left
    Dim spriteheight = EF.Bottom - EF.Top


    If c.FDir = FaceDir.Left Then
      Dim spriteleft As Integer = c.PosX - EF.CtrPoint.x + EF.Left
      Dim spritetop As Integer = c.PosY - EF.CtrPoint.y + EF.Top
      'set mask
      For i = 0 To spritewidth
        For j = 0 To spriteheight
          Img.Elmt(spriteleft + i, spritetop + j) = OpAnd(Img.Elmt(spriteleft + i, spritetop + j), SpriteMask.Elmt(EF.Left + i, EF.Top + j))
        Next
      Next

      'set sprite
      For i = 0 To spritewidth
        For j = 0 To spriteheight
          Img.Elmt(spriteleft + i, spritetop + j) = OpOr(Img.Elmt(spriteleft + i, spritetop + j), SpriteMap.Elmt(EF.Left + i, EF.Top + j))
        Next
      Next
    Else 'facing right
      Dim spriteleft = c.PosX + EF.CtrPoint.x - EF.Right
      Dim spritetop = c.PosY - EF.CtrPoint.y + EF.Top
      'set mask
      For i = 0 To spritewidth
        For j = 0 To spriteheight
          Img.Elmt(spriteleft + i, spritetop + j) = OpAnd(Img.Elmt(spriteleft + i, spritetop + j), SpriteMask.Elmt(EF.Right - i, EF.Top + j))
        Next
      Next

      'set sprite
      For i = 0 To spritewidth
        For j = 0 To spriteheight
          Img.Elmt(spriteleft + i, spritetop + j) = OpOr(Img.Elmt(spriteleft + i, spritetop + j), SpriteMap.Elmt(EF.Right - i, EF.Top + j))
        Next
      Next

    End If

  End Sub

  Sub DisplayImg()
    'display bg and sprite on picturebox
    Dim i, j As Integer
    PutSprite(SM)

    For i = 0 To Img.Width - 1
      For j = 0 To Img.Height - 1
        bmp.SetPixel(i, j, Img.Elmt(i, j))
      Next
    Next

    PictureBox1.Refresh()

    PictureBox1.Image = bmp
    PictureBox1.Width = bmp.Width
    PictureBox1.Height = bmp.Height
    PictureBox1.Top = 0
    PictureBox1.Left = 0
    'Me.Width = PictureBox1.Width + 30
    'Me.Height = PictureBox1.Height + 100





  End Sub



  Sub ResizeImg()
    Dim w, h As Integer

    w = PictureBox1.Width
    h = PictureBox1.Height

    Me.ClientSize = New Size(w, h)

  End Sub




  Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick

    PictureBox1.Refresh()

    SM.Update()

    DisplayImg()


  End Sub

End Class
