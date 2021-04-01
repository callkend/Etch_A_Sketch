'Kendall Callister
'RCET0265
'Spring2020
'Etch-A-Sketch
'https://github.com/callkend/Etch_A_Sketch.git

Option Strict On
Option Explicit On

Public Class Form1
    Dim g As Graphics

    'Handles startup
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Resizing()
        ToolTip1.SetToolTip(SelectColorButton, "Allows for pen color to be selected")
        ToolTip1.SetToolTip(DrawWaveformsButton, "Draws a Sine, Cosine, and Tangent Waveforms")
        ToolTip1.SetToolTip(ClearButton, "Clears the Screen")
        ToolTip1.SetToolTip(ExitButton, "Close Program")
        ToolTip1.SetToolTip(PictureBox1, "Hold Left Mouse Button to Draw")

    End Sub

    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles MyBase.SizeChanged
        Resizing()

    End Sub

    'Resizes the picture box, and moves controls to fit Window
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

    'Handles using the mouse to draw
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

    'Extra Function to draw With WASD
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

    'Extra Function to draw With WASD
    Private Sub WASD_Press(sender As Object, e As KeyPressEventArgs) Handles SelectColorButton.KeyPress
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

    'Grabs mouse position, and what button is pressed on it
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

    'Allows for the context menu on the form
    Private Sub Mouser_Click(sender As Object, e As MouseEventArgs) Handles Me.MouseDown
        Dim mousePosition As Point = New Point(e.X, e.Y)
        Dim mouseButton As String = e.Button.ToString

        If mouseButton = "Right" Then
            ContextMenuStrip1.Show(mousePosition)
        End If
    End Sub

    'Allows user to select color, and stores selected color
    Function PenColor(changeColor As Boolean) As Color
        Static color As New ColorDialog

        If changeColor = True Then
            color.ShowDialog()
        End If
        Return color.Color
    End Function

    'Draws waveforms
    Sub Waveform()
        Dim g As Graphics = PictureBox1.CreateGraphics
        Dim pen As New Pen(Color.Black)
        Dim sineWave(360) As PointF
        Dim cosineWave(360) As PointF

        g.Clear(BackColor)

        'Draws 10 x 10 graticule
        For i = 0 To 10
            g.DrawLine(pen, CSng(PictureBox1.Width * (i / 10)), 0, CSng(PictureBox1.Width * (i / 10)), CSng(PictureBox1.Height))
            g.DrawLine(pen, 0, CSng(PictureBox1.Height * (i / 10)), CSng(PictureBox1.Width), CSng(PictureBox1.Height * (i / 10)))
        Next

        'Calculates sine, cosine, and tangent waveforms
        For i = 0 To 360
            sineWave(i) = New PointF(CSng(PictureBox1.Width * i / 360), CSng(Math.Sin(i * Math.PI / 180) * PictureBox1.Height / 2) + CSng(PictureBox1.Height / 2))
            cosineWave(i) = New PointF(CSng(PictureBox1.Width * i / 360), CSng(Math.Cos(i * Math.PI / 180) * PictureBox1.Height / 2) + CSng(PictureBox1.Height / 2))
            Try
                g.DrawLine(New Pen(Color.Green), New PointF(CSng(PictureBox1.Width * i / 360), CSng(Math.Tan(i * Math.PI / 180) * PictureBox1.Height / 2) + CSng(PictureBox1.Height / 2)), New PointF(CSng(PictureBox1.Width * (i + 1) / 360), CSng(Math.Tan((i + 1) * Math.PI / 180) * PictureBox1.Height / 2) + CSng(PictureBox1.Height / 2)))
            Catch ex As Exception

            End Try

        Next


        'Draws Sine and Cosine Waveforms
        pen = New Pen(Color.Blue)
        g.DrawCurve(pen, sineWave)

        pen = New Pen(Color.Red)
        g.DrawCurve(pen, cosineWave)

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

    Private Sub ClearButton_Click(sender As Object, e As EventArgs) Handles ClearButton.Click
        ClearPicture()
    End Sub

    'Clears the picture and shakes the form
    Sub ClearPicture()
        Dim g As Graphics = PictureBox1.CreateGraphics
        Dim orginalX As Integer = Me.Location.X
        Dim orginalY As Integer = Me.Location.Y

        g.Clear(BackColor)
        For i = 0 To 100
            Dim point As New Point(orginalX + CInt(Rnd() * 10), orginalY + CInt(Rnd() * 10))
            Me.Location = (point)
        Next
        Me.Location = New Point(orginalX, orginalY)
    End Sub

    'Handles Select Color Button
    Private Sub SelectColorButton_Click(sender As Object, e As EventArgs) Handles SelectColorButton.Click
        PenColor(True)
    End Sub
    'Handles Waveform Button
    Private Sub DrawWaveformsButton_Click(sender As Object, e As EventArgs) Handles DrawWaveformsButton.Click
        Waveform()
    End Sub
    'Exits program
    Private Sub ExitButton_Click(sender As Object, e As EventArgs) Handles ExitButton.Click
        Me.Close()
    End Sub

End Class
