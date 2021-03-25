Option Strict On
Option Explicit On

Public Class Form1
    Dim sineWave(360) As PointF
    Dim cosineWave(360) As PointF
    Dim wait As Boolean = False
    Dim g As Graphics

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Resizing()
    End Sub

    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles MyBase.SizeChanged
        Resizing()

    End Sub

    Sub Resizing()
        Dim xShifter As Integer = (Me.Width - ExitButton.Width - 20)
        Dim position As Point = New Point(xShifter, Me.Height - 70)
        ExitButton.Location = position
        xShifter -= ClearButton.Width
        position = New Point(xShifter, Me.Height - 70)
        ClearButton.Location = position
        xShifter -= DrawWaveformsButton.Width
        position = New Point(xShifter, Me.Height - 70)
        DrawWaveformsButton.Location = position
        xShifter -= SelectColorButton.Width
        position = New Point(xShifter, Me.Height - 70)
        SelectColorButton.Location = position
        PictureBox1.Height = ExitButton.Location.Y - 40
        PictureBox1.Width = Me.Width - 35
    End Sub

    Sub DrawLine(mouse As Point)
        Dim zero As Point
        Static lastPosistion As Point
        Static i As Integer = 0
        Dim g As Graphics = PictureBox1.CreateGraphics

        Dim pen As New Pen(PenColor(False))

        If lastPosistion = zero Then
            g.DrawLine(pen, mouse, mouse)
        Else
            g.DrawLine(pen, lastPosistion, mouse)
        End If
        lastPosistion = mouse


        pen.Dispose()
        g.Dispose()
    End Sub

    Dim x As Integer = 250
    Dim y As Integer = 250
    Sub DrawStuff(direction As Integer)
        Dim g As Graphics = PictureBox1.CreateGraphics
        Dim pen As New Pen(PenColor(False))

        Select Case direction
            Case 0
                g.DrawLine(pen, x, y, x, y + 1)
                y += 1
            Case 1
                g.DrawLine(pen, x, y, x - 1, y)
                x -= 1
            Case 2
                g.DrawLine(pen, x, y, x, y - 1)
                y -= 1
            Case 3
                g.DrawLine(pen, x, y, x + 1, y)
                x += 1
        End Select
        pen.Dispose()
        g.Dispose()
    End Sub

    'Private Sub Form1_Click(sender As Object, e As EventArgs) Handles Me.MouseClick
    '    DrawLine(MousePosition)
    'End Sub

    Private Sub WASD_Press(sender As Object, e As KeyPressEventArgs) Handles PictureBox1.KeyPress
        Select Case e.KeyChar
            Case CChar("s")
                DrawStuff(0)
            Case CChar("a")
                DrawStuff(1)
            Case CChar("w")
                DrawStuff(2)
            Case CChar("d")
                DrawStuff(3)
        End Select
    End Sub

    Private Sub Mouser_Move(sender As Object, e As MouseEventArgs) Handles PictureBox1.MouseDown, PictureBox1.MouseMove
        Dim mousePosition As Point = New Point(e.X, e.Y)
        Dim mouseButton As String = e.Button.ToString

        Me.Text = ($"{e.X},{e.Y} Button:{mouseButton}")
        Select Case mouseButton
            Case "Left"
                DrawLine(mousePosition)
                'MsgBox($"{e.X},{e.Y} Button:{mouseButton}")
            Case "Right"
                ContextMenuStrip1.Show(mousePosition)
            Case "Middle"
                PenColor(True)
            Case Else
        End Select

    End Sub

    Private Sub Mouser_Click(sender As Object, e As MouseEventArgs) Handles Me.MouseDown
        Dim mousePosition As Point = New Point(e.X, e.Y)
        Dim mouseButton As String = e.Button.ToString

        If mouseButton = "Right" Then
            ContextMenuStrip1.Show(mousePosition)
        End If
    End Sub

    Function PenColor(changeColor As Boolean) As Color
        Static color As New ColorDialog

        If changeColor = True Then
            color.ShowDialog()
        End If
        Return color.Color
    End Function

    Sub Waveform()
        Dim g As Graphics = PictureBox1.CreateGraphics
        Dim pen As New Pen(Color.Black)

        For i = 0 To 360
            sineWave(i) = New PointF(CSng(PictureBox1.Width * i / 360), CSng(Math.Sin(i * Math.PI / 180) * PictureBox1.Height / 2) + CSng(PictureBox1.Height / 2))
            cosineWave(i) = New PointF(CSng(PictureBox1.Width * i / 360), CSng(Math.Cos(i * Math.PI / 180) * PictureBox1.Height / 2) + CSng(PictureBox1.Height / 2))
        Next

        g.Clear(BackColor)

        For i = 0 To 10
            g.DrawLine(pen, CSng(PictureBox1.Width * (i / 10)), 0, CSng(PictureBox1.Width * (i / 10)), CSng(PictureBox1.Height))
            g.DrawLine(pen, 0, CSng(PictureBox1.Height * (i / 10)), CSng(PictureBox1.Width), CSng(PictureBox1.Height * (i / 10)))
        Next

        pen = New Pen(Color.Blue)
        'g.DrawArc(pen, 0, 0, CSng(PictureBox1.Width / 2), CSng(PictureBox1.Height), 0, 180)
        'g.DrawArc(pen, CSng(PictureBox1.Width / 2), 0, CSng(PictureBox1.Width / 2), CSng(PictureBox1.Height), 0, -180)
        g.DrawCurve(pen, sineWave)

        pen = New Pen(Color.Red)
        g.DrawCurve(pen, cosineWave)
    End Sub

    Private Sub ClearButton_Click(sender As Object, e As EventArgs) Handles ClearButton.Click
        ClearPicture()
    End Sub

    Private Sub Menu_Click(sender As Object, e As EventArgs) Handles ClearToolStripMenuItem.Click, ExitToolStripMenuItem.Click, SelectColorToolStripMenuItem.Click, DrawWaveformToolStripMenuItem.Click, AboutToolStripMenuItem.Click, ClearToolStripMenuItem1.Click, EditToolStripMenuItem1.Click, SelectColorToolStripMenuItem1.Click, DrawWaveformsToolStripMenuItem.Click, AboutToolStripMenuItem1.Click
        Dim itemClicked As String = sender.ToString
        Select Case itemClicked
            Case "Exit"
                Me.Close()
            Case "Clear"
                ClearPicture()
            Case "Select Color"
                PenColor(True)
            Case "Draw Waveforms"
                Waveform()
            Case "About"

        End Select
    End Sub

    Sub ClearPicture()
        Dim g As Graphics = PictureBox1.CreateGraphics
        g.Clear(BackColor)
    End Sub

    Private Sub ExitButton_Click(sender As Object, e As EventArgs) Handles ExitButton.Click
        Me.Close()
    End Sub

    Private Sub SelectColorButton_Click(sender As Object, e As EventArgs) Handles SelectColorButton.Click
        PenColor(True)
    End Sub

    Private Sub DrawWaveformsButton_Click(sender As Object, e As EventArgs) Handles DrawWaveformsButton.Click
        Waveform()
    End Sub

End Class
