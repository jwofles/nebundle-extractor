Imports System.IO
Public Class Form1
    Dim filePresent As Boolean = False
    Dim filePath As String
    Dim fileName As String
    Dim bytePurge As Integer
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        openAndWrite()
    End Sub

    Sub openAndWrite()
        'Dim fs As New FileStream(filePath, FileMode.Open)
        'Dim bw As New BinaryWriter(fs)
        updater.Text = ("This may take a while...")
        Dim imageNum As Integer = 0
        Dim fileIsOpen As Boolean = False
        Dim bytes() As Byte '= {137, 80, 78, 71, 13, 10, 26, 10}
        bytes = File.ReadAllBytes(filePath)
        Button1.Enabled = False

        'For Each number In bytes
        'Dim pngHeader() As Byte = {137, 80, 78, 71, 13, 10, 26, 10}
        If (Not System.IO.Directory.Exists(My.Computer.FileSystem.SpecialDirectories.Desktop & "\" & fileName & "_extracted")) Then
            System.IO.Directory.CreateDirectory(My.Computer.FileSystem.SpecialDirectories.Desktop & "\" & fileName & "_extracted")
        End If

        'File.WriteAllBytes("C:\Users\henry\Desktop\" & fileName & "_extracted\" & imageNum & ".png", bytes)

        Dim ihdrBytes() As Byte = {0, 0, 0, 13, 73, 72, 68, 82}
        Dim iendBytes() As Byte = {73, 69, 78, 68, 174, 66, 96, 130}
        Dim findLen As Integer = ihdrBytes.Length
        Dim retLen As Integer = 10
        Dim imageBytes As List(Of Byte) = New List(Of Byte)
        Dim searchingForEnd As Boolean = False
        For i As Integer = 0 To bytes.Length - 1
            If searchingForEnd = False Then
                If bytes(i) = ihdrBytes(0) Then
                    If bytes.Skip(i).Take(findLen).SequenceEqual(ihdrBytes) Then
                        'Dim output() As Byte = bytes.Skip(x).Take(retLen).ToArray
                        imageNum += 1
                        Label2.Text = ("Currently extracting image" & imageNum)
                        Application.DoEvents()
                        fileIsOpen = True
                        imageBytes.Clear()
                        searchingForEnd = True
                        ''WB.Write({137, 80, 78, 71, 13, 10, 26, 10}) ' PNG HEADER
                        'WB.Write({0, 0, 0, 13, 73, 72, 68, 82})
                    End If
                End If
            End If
            If fileIsOpen = True Then
                ''WB.Write(bytes(i))
                imageBytes.Add(bytes(i))
            End If
            If searchingForEnd = True Then
                If bytes(i) = iendBytes(0) Then
                    If bytes.Skip(i).Take(findLen).SequenceEqual(iendBytes) Then
                        'Dim output() As Byte = bytes.Skip(x).Take(retLen).ToArray
                        Dim path As String = My.Computer.FileSystem.SpecialDirectories.Desktop & "\" & fileName & "_extracted\image" & imageNum & ".png"
                        Dim FS As New FileStream(path, FileMode.Create)
                        Dim WB As New BinaryWriter(FS)
                        WB.Write({137, 80, 78, 71, 13, 10, 26, 10}) ' PNG HEADER
                        WB.Write(imageBytes.ToArray)
                        WB.Write({73, 69, 78, 68, 174, 66, 96, 130}) 'iend
                        fileIsOpen = False
                        FS.Close()
                        Dim fi As New IO.FileInfo(path)
                        If fi.Length <= bytePurge Then
                            My.Computer.FileSystem.DeleteFile(path)
                            Application.DoEvents()
                            'Label2.Text = ("Image" & imageNum & " only " & fi.Length & " bytes, it has been purged")
                            imageNum -= 1
                        End If
                        searchingForEnd = False
                    End If
                End If
            End If
        Next

        Button1.Enabled = True
        MsgBox("Finished")
        updater.Text = ("Drag And drop the nebundle you would Like to extract")
    End Sub

    'Sub writeAndSave()
    '    Dim fs As New FileStream(filePath, FileMode.Create)
    '    Dim bw As New BinaryWriter(fs)
    '    For imagenum = 0 To 123
    '        File.Create("C:\Desktop\" & fileName & "\" & imagenum & ".png").Dispose()
    '    Next
    'End Sub

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Me.AllowDrop = True
        Button1.Enabled = False
        TextBox1.Text = ("500")
        CheckBox1.Checked = True
        'TextBox1.Enabled = True
    End Sub

    Private Sub Form1_DragDrop(sender As System.Object, e As System.Windows.Forms.DragEventArgs) Handles Me.DragDrop
        Dim files() As String = e.Data.GetData(DataFormats.FileDrop)
        Button1.Enabled = True
        For Each path In files
            filePath = path
            updater.Text = (path)
            fileName = System.IO.Path.GetFileName(path)
        Next
        label2.Text = ("File loaded")
    End Sub

    Private Sub Form1_DragEnter(sender As System.Object, e As System.Windows.Forms.DragEventArgs) Handles Me.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        CheckBox1.Text = ("Purge files smaller than ") & TextBox1.Text & (" bytes")
        Try
            bytePurge = Convert.ToInt32(TextBox1.Text)
        Catch
            bytePurge = 0
            CheckBox1.Text = ("Purge files smaller than 0 bytes")
        End Try
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked = True Then
            TextBox1.Enabled = True
        Else
            TextBox1.Enabled = False
        End If
    End Sub

End Class
