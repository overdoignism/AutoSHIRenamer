﻿Imports System.Security.Cryptography
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
        Dim GetThePatch As String = Replace(My.Computer.FileSystem.GetFileInfo(TextBox1.Text).FullName, ".html", "_files\")
        Dim hasher As MD5 = MD5.Create()

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
            FULLstrOrg = GJFILE.ReadToEnd
            GJFILE.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
            Exit Sub
        End Try

        Dim IDX1, IDX2, IDX3, IDX4, IDX5 As Integer

        Dim FileTypeS() As String = {"jpg", "jpeg", "png", "webp", "gif", "php"}
        Dim FileTypeMany As Integer = FileTypeS.Count - 1
        Dim FileTypeSCount(FileTypeMany) As Integer
        Dim FileTypeLastUse As String

        Dim CountFormat As String
        Dim FileTmp1, FileTypeUp As String

        FULLstr = FULLstrOrg.ToUpper()
        FULLstr = Replace(FULLstr, "\", "/")
        FULLstr = Replace(FULLstr, """", "/")
        FULLstr = Replace(FULLstr, "'", "/")
        FULLstr = Replace(FULLstr, "=", "/")
        FULLstr = Replace(FULLstr, ">", "/")
        FULLstrArray = FULLstr.Split(" ")
        FULLstrArrayOrg = FULLstrOrg.Split(" ")

        Dim GetBackOne As String = ""

        IDX1 = 0
        IDX2 = UBound(FULLstrArray)


        For IDX1 = 0 To IDX2

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
                            FileTypeSCount(IDX5) += 1
                            FULLstrOrg = Replace(FULLstrOrg, GetBackOne + FileTmp1 + FileTypeLastUse, GetBackOne + CountFormat + "." + FileTypeLastUse)
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

        Next


        If FixHtml Then
            Dim FH1 As IO.StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(TheHtmlFile, False)
            FH1.Write(FULLstrOrg)
            FH1.Close()
        End If

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        End
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start("https://lunbanplantman.blogspot.com/2020/08/blog-post_29.html")
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Process.Start("Readme.txt")
    End Sub
End Class
