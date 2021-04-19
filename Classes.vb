Imports System.IO

Public Enum StateSplitMushroom
  Walk
  JumpStart
  Jump
  JumpEnd
End Enum

Public Enum FaceDir
  Left
  Right
End Enum

Public Class CImage
  Public Width As Integer
  Public Height As Integer
  Public Elmt(,) As System.Drawing.Color
  Public ColorMode As Integer 'not used

  Sub OpenImage(ByVal FName As String)
    Dim s As String
    Dim L As Long
    Dim BR As BinaryReader
    Dim h, w, pos As Integer
    Dim r, g, b As Integer
    Dim pad As Integer

    BR = New BinaryReader(File.Open(FName, FileMode.Open))

    Try
      BlockRead(BR, 2, s)

      If s <> "BM" Then
        MsgBox("Not a BMP file")
      Else 'BMP file
        BlockReadInt(BR, 4, L) 'size
        'MsgBox("Size = " + CStr(L))
        BlankRead(BR, 4) 'reserved
        BlockReadInt(BR, 4, pos) 'start of data
        BlankRead(BR, 4) 'size of header
        BlockReadInt(BR, 4, Width) 'width
        'MsgBox("Width = " + CStr(I.Width))
        BlockReadInt(BR, 4, Height) 'height
        'MsgBox("Height = " + CStr(I.Height))
        BlankRead(BR, 2) 'color panels
        BlockReadInt(BR, 2, ColorMode) 'colormode
        If ColorMode <> 24 Then
          MsgBox("Not a 24-bit color BMP")
        Else

          BlankRead(BR, pos - 30)

          ReDim Elmt(Width - 1, Height - 1)
          pad = (4 - (Width * 3 Mod 4)) Mod 4

          For h = Height - 1 To 0 Step -1
            For w = 0 To Width - 1
              BlockReadInt(BR, 1, b)
              BlockReadInt(BR, 1, g)
              BlockReadInt(BR, 1, r)
              Elmt(w, h) = Color.FromArgb(r, g, b)

            Next
            BlankRead(BR, pad)

          Next

        End If

      End If

    Catch ex As Exception
      MsgBox("Error")

    End Try

    BR.Close()


  End Sub


  Sub CreateMask(ByRef Mask As CImage)
    Dim i, j As Integer

    Mask = New CImage
    Mask.Width = Width
    Mask.Height = Height

    ReDim Mask.Elmt(Mask.Width - 1, Mask.Height - 1)

    For i = 0 To Width - 1
      For j = 0 To Height - 1
        If Elmt(i, j).R = 0 And Elmt(i, j).G = 0 And Elmt(i, j).B = 0 Then
          Mask.Elmt(i, j) = Color.FromArgb(255, 255, 255)
        Else
          Mask.Elmt(i, j) = Color.FromArgb(0, 0, 0)
        End If
      Next
    Next

  End Sub


  Sub CopyImg(ByRef Img As CImage)
    'copies image to Img
    Img = New CImage
    Img.Width = Width
    Img.Height = Height
    ReDim Img.Elmt(Width - 1, Height - 1)

    For i = 0 To Width - 1
      For j = 0 To Height - 1
        Img.Elmt(i, j) = Elmt(i, j)
      Next
    Next

  End Sub

End Class

Public Class CCharacter
  Public PosX, PosY As Double
  Public Vx, Vy As Double
  Public CurrState As StateSplitMushroom
  Public FrameIdx As Integer
  Public CurrFrame As Integer
  Public ArrSprites() As CArrFrame
  Public IdxArrSprites As Integer
  Public FDir As FaceDir

  Public Sub State(state As StateSplitMushroom, idxspr As Integer)
    CurrState = state
    IdxArrSprites = idxspr
    CurrFrame = 0
    FrameIdx = 0

  End Sub

  Public Sub GetNextFrame()
    CurrFrame = CurrFrame + 1
    If CurrFrame = ArrSprites(IdxArrSprites).Elmt(FrameIdx).MaxFrameTime Then
      FrameIdx = FrameIdx + 1
      If FrameIdx = ArrSprites(IdxArrSprites).N Then
        FrameIdx = 0
      End If
      CurrFrame = 0

    End If

  End Sub

  Public Sub Update()
    Select Case CurrState
            Case StateSplitMushroom.Walk
                FDir = FaceDir.Left
                PosX = PosX + Vx
        GetNextFrame()
        If PosX <= 50 Then
          State(StateSplitMushroom.JumpStart, 1)
          Vx = 0
          Vy = 0
        End If
      Case StateSplitMushroom.JumpStart
        GetNextFrame()
        If FrameIdx = 1 And CurrFrame = 1 Then
          FDir = FaceDir.Right
        ElseIf FrameIdx = 0 Then
          State(StateSplitMushroom.Jump, 2)
          Vx = 5
          Vy = -5
        End If
      Case StateSplitMushroom.Jump
        PosX = PosX + Vx
        PosY = PosY + Vy
        Vy = Vy + 0.2
        GetNextFrame()
        If PosY >= 200 And Vy > 0 Then
          State(StateSplitMushroom.JumpEnd, 3)
          PosY = 200
          Vx = 0
          Vy = 0

        End If

      Case StateSplitMushroom.JumpEnd
        GetNextFrame()
        If FrameIdx = 2 And CurrFrame = 1 Then
          FDir = FaceDir.Left
        ElseIf FrameIdx = 0 Then
          State(StateSplitMushroom.Walk, 0)
          Vx = -5
          Vy = 0
        End If





    End Select

  End Sub

End Class

Public Class CElmtFrame
  Public CtrPoint As TPoint
  Public Top, Bottom, Left, Right As Integer
  Public Idx As Integer
  Public MaxFrameTime As Integer

  Public Sub New(ctrx As Integer, ctry As Integer, l As Integer, t As Integer, r As Integer, b As Integer, mft As Integer)
    CtrPoint.x = ctrx
    CtrPoint.y = ctry
    Top = t
    Bottom = b
    Left = l
    Right = r
    MaxFrameTime = mft

  End Sub
End Class

Public Class CArrFrame
  Public N As Integer
  Public Elmt As CElmtFrame()

  Public Sub New()
    N = 0
    ReDim Elmt(-1)
  End Sub

  Public Overloads Sub Insert(E As CElmtFrame)
    ReDim Preserve Elmt(N)
    Elmt(N) = E
    N = N + 1
  End Sub

  Public Overloads Sub Insert(ctrx As Integer, ctry As Integer, l As Integer, t As Integer, r As Integer, b As Integer, mft As Integer)
    Dim E As CElmtFrame
    E = New CElmtFrame(ctrx, ctry, l, t, r, b, mft)
    ReDim Preserve Elmt(N)
    Elmt(N) = E
    N = N + 1

  End Sub

End Class

Public Structure TPoint
  Dim x As Integer
  Dim y As Integer

End Structure

