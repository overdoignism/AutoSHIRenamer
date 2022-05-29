Imports System.Security.Cryptography
Imports System.Text

Public Class Form1

    Private Sub TextBox1_DragDrop(sender As Object, e As DragEventArgs) Handles TextBox1.DragDrop

        Dim files() As String = e.Data.GetData(DataFormats.FileDrop)

        If (files IsNot Nothing) Then
            TextBox1.Text = files(0)
        End If

    End Sub

    Private Sub TextBox1_DragEnter(sender As Object, e As DragEventArgs) Handles TextBox1.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click




        If Not My.Computer.FileSystem.FileExists(TextBox1.Text) Then
            MsgBox("檔案不存在!", 0, "錯誤")
            Exit Sub
        End If

        Dim ManyHTMLstr As String
        Dim hasher As MD5 = MD5.Create()
        Dim GetThePatch As String = My.Computer.FileSystem.GetFileInfo(TextBox1.Text).FullName

        GetThePatch = GetThePatch + "/"

        GetThePatch = Replace(GetThePatch, ".html/", "_files\")
        GetThePatch = Replace(GetThePatch, ".htm/", "_files\")

        If Not My.Computer.FileSystem.DirectoryExists(GetThePatch) Then
            MsgBox("路徑不存在!可能非網頁存檔", 0, "錯誤")
            Exit Sub
        End If

        Me.Enabled = False

        RenameImgHTML(TextBox1.Text, GetThePatch, CheckBox1.Checked)

        For Each ManyHTMLstr In My.Computer.FileSystem.GetFiles(GetThePatch)

            If InStr(ManyHTMLstr.ToUpper, ".HTM") > 0 Then

                Dim HTMLNameMD5 As Byte() = hasher.ComputeHash(Encoding.UTF8.GetBytes(ManyHTMLstr))
                Dim str01 As String = HTMLNameMD5(0).ToString("x2") + HTMLNameMD5(1).ToString("x2") + HTMLNameMD5(2).ToString("x2")

                RenameImgHTML(ManyHTMLstr, GetThePatch, CheckBox1.Checked, "_" + str01.ToString.ToUpper)

            End If

        Next

        MsgBox("Done", MsgBoxStyle.OkOnly, "Fin")
        Me.Enabled = True


    End Sub


    Private Sub RenameImgHTML(TheHtmlFile As String, ThePath As String, FixHtml As Boolean, Optional PreFixStr As String = "")

        Dim GJFILE As IO.StreamReader

        Dim FULLstrStart As String = ""
        Dim FULLstrOrg As String = ""
        Dim FULLstr As String = ""
        Dim FULLstrArray() As String
        Dim FULLstrArrayOrg() As String

        Try
            GJFILE = My.Computer.FileSystem.OpenTextFileReader(TheHtmlFile)
        Catch ex As Exception
            MsgBox(ex.Message)
            Exit Sub
        End Try

        Try
            FULLstrStart = GJFILE.ReadToEnd
            GJFILE.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
            Exit Sub
        End Try


        Dim IDX1, IDX2, IDX3, IDX4, IDX5, IDX6 As Integer
        Dim Rnd1 As New Random
        Dim SPCode As String = "?" + Hex(Rnd1.Next(16, 512)) + "?:-:!!"

        Dim FileTypeS() As String = {"jpg", "jpeg", "png", "webp", "gif", "php"}
        Dim SplitStr() As String = {" ", SPCode}
        Dim FileTypeMany As Integer = FileTypeS.Count - 1
        Dim FileTypeSCount(FileTypeMany) As Integer
        Dim FileTypeLastUse As String

        Dim CountFormat As String
        Dim FileTmp1, FileTypeUp As String

        FULLstrOrg = FULLstrStart
        FULLstrOrg = Replace(FULLstrOrg, "http:", SPCode + "http:")
        FULLstrOrg = Replace(FULLstrOrg, "https:", SPCode + "https:")
        'FULLstr = FULLstrStart.ToUpper

        FULLstr = FULLstrOrg.ToUpper()
        FULLstr = Replace(FULLstr, "\", "/")
        FULLstr = Replace(FULLstr, """", "/")
        FULLstr = Replace(FULLstr, "'", "/")
        FULLstr = Replace(FULLstr, "=", "/")
        FULLstr = Replace(FULLstr, ">", "/")
        FULLstrArray = FULLstr.Split(SplitStr, StringSplitOptions.None)
        FULLstrArrayOrg = FULLstrOrg.Split(SplitStr, StringSplitOptions.None)

        Dim GetBackOne As String = ""

        IDX1 = 0
        IDX2 = UBound(FULLstrArray)


        For IDX1 = 0 To IDX2

            If FULLstrArray(IDX1).Length = FULLstrArrayOrg(IDX1).Length Then

                For IDX5 = 0 To FileTypeMany

                    FileTypeUp = FileTypeS(IDX5).ToUpper
                    IDX3 = InStrRev(FULLstrArray(IDX1), "." + FileTypeUp)

                    If IDX3 > 0 Then

                        IDX4 = InStrRev(FULLstrArray(IDX1), "/", IDX3)
                        FileTmp1 = FULLstrArrayOrg(IDX1).Substring(IDX4, IDX3 - IDX4)
                        If IDX4 > 0 Then GetBackOne = FULLstrArrayOrg(IDX1).Substring(IDX4 - 1, 1)
                        FileTypeLastUse = FileTypeS(IDX5)
                        CountFormat = String.Format(FileTypeUp + PreFixStr + "_FILE{0:d5}", FileTypeSCount(IDX5))

                        Try


                            If My.Computer.FileSystem.FileExists(ThePath + FileTmp1 + FileTypeLastUse) Then

                                My.Computer.FileSystem.RenameFile(ThePath + FileTmp1 + FileTypeLastUse, CountFormat + "." + FileTypeLastUse)
                                FULLstrOrg = Replace(FULLstrOrg, GetBackOne + FileTmp1 + FileTypeLastUse, GetBackOne + CountFormat + "." + FileTypeLastUse)
                                FileTypeSCount(IDX5) += 1

                                For IDX6 = IDX1 To IDX2

                                    If InStr(FULLstrArrayOrg(IDX6), FileTmp1 + FileTypeLastUse) > 0 Then
                                        If InStr(FULLstrArray(IDX6), "HTTP://") = 0 And InStr(FULLstrArray(IDX6), "HTTPS://") = 0 Then
                                            FULLstrArrayOrg(IDX6) = Replace(FULLstrArrayOrg(IDX6), GetBackOne + FileTmp1 + FileTypeLastUse, GetBackOne + CountFormat + "." + FileTypeLastUse)
                                        End If
                                    End If
                                Next

                                Exit For
                            End If

                        Catch ex As Exception

                            If (MsgBox("Error: " + ex.Message, MsgBoxStyle.OkCancel, "Error") = MsgBoxResult.Cancel) Then
                                Exit Sub
                            End If
                            Exit For

                        End Try

                    End If

                Next

            End If

        Next


        If FixHtml Then
            Dim FH1 As IO.StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(TheHtmlFile, False)
            For Each WriteStr As String In FULLstrArrayOrg
                FH1.Write(WriteStr + " ")
            Next
            FH1.Close()
        End If

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        End
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start("https://ihavenomatk.blogspot.com/2020/10/blog-post.html")
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Process.Start("Readme.txt")
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class
