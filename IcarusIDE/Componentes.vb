Public Class Componentes



    Dim ComponentesDS As New DataSet("Componentes")
    Private Sub ComboBox1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ComboBox1.KeyDown
        e.SuppressKeyPress = True


    End Sub

    Private Sub cargar()

        ListBox1.Items.Clear()
        ListBox2.Items.Clear()
        ComponentesDS.Clear()

        ComponentesDS.ReadXml(".\Componentes.xml")

        For c = 0 To ComponentesDS.Tables.Count - 1
            For d = 0 To ComponentesDS.Tables(c).Rows.Count - 1
                ListBox1.Items.Add(ComponentesDS.Tables(c).Rows(d).Item(0)) 'Nombre
                ListBox2.Items.Add(CStr(c) + "," + CStr(d))
            Next

        Next

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        cargar()

    End Sub

    Private Sub ListBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListBox1.SelectedIndexChanged
        Dim a, temp() As String
        If ListBox1.SelectedIndex <> -1 Then
            a = ListBox2.Items(ListBox1.SelectedIndex)
            temp = Split(a, ",")
            If temp(0) = "0" Then
                ComboBox1.Text = "Básicos"
            End If
            If temp(0) = "1" Then
                ComboBox1.Text = "Controladores"
            End If
            If temp(0) = "2" Then
                ComboBox1.Text = "Periféricos"
            End If
            TextBox1.Text = ComponentesDS.Tables(CInt(temp(0))).Rows(CInt(temp(1))).Item(0)
            TextBox2.Text = ComponentesDS.Tables(CInt(temp(0))).Rows(CInt(temp(1))).Item(1)
            TextBox3.Text = ComponentesDS.Tables(CInt(temp(0))).Rows(CInt(temp(1))).Item(2)
            TextBox4.Text = ComponentesDS.Tables(CInt(temp(0))).Rows(CInt(temp(1))).Item(3)
            TextBox5.Text = ComponentesDS.Tables(CInt(temp(0))).Rows(CInt(temp(1))).Item(4)
            TextBox6.Text = ComponentesDS.Tables(CInt(temp(0))).Rows(CInt(temp(1))).Item(5)
            TextBox7.Text = ComponentesDS.Tables(CInt(temp(0))).Rows(CInt(temp(1))).Item(6)
            TextBox8.Text = ComponentesDS.Tables(CInt(temp(0))).Rows(CInt(temp(1))).Item(7)
            TextBox9.Text = ComponentesDS.Tables(CInt(temp(0))).Rows(CInt(temp(1))).Item(8)




        End If
    End Sub
    Private Sub habilitar()
        TextBox1.Enabled = Not TextBox1.Enabled
        TextBox2.Enabled = Not TextBox2.Enabled
        TextBox3.Enabled = Not TextBox3.Enabled
        TextBox4.Enabled = Not TextBox4.Enabled
        TextBox5.Enabled = Not TextBox5.Enabled
        TextBox6.Enabled = Not TextBox6.Enabled
        TextBox7.Enabled = Not TextBox7.Enabled
        TextBox8.Enabled = Not TextBox8.Enabled
        TextBox9.Enabled = Not TextBox9.Enabled
        ComboBox1.Enabled = Not ComboBox1.Enabled

    End Sub
    Private Sub agregar()
        Dim err As String
        err = ""
        Dim flag As Boolean
        flag = True
        If TextBox1.Text = "" Then
            err = "Nombre del componente en blanco" + vbCrLf
            flag = False
        End If
        If TextBox8.Text = "" Then
            err = "No se permite Nemonico En blanco" + vbCrLf
            flag = False
        End If
        For c = 0 To ComponentesDS.Tables.Count - 1
            For d = 0 To ComponentesDS.Tables(c).Rows.Count - 1
                If ComponentesDS.Tables(c).Rows(d).Item(7) = TextBox8.Text Then
                    flag = False
                    err = err + "Nemonico Repetido" + vbCrLf

                End If

            Next

        Next
        If Not My.Computer.FileSystem.FileExists(TextBox5.Text) Then
            err = err + "Debes proporcionar un nombre de archivo del módulo" + vbCrLf
            flag = 0


        End If
        If flag Then


            Dim nuevoelemento As DataRow
            Dim a As System.Collections.ObjectModel.ReadOnlyCollection(Of String)

            nuevoelemento = ComponentesDS.Tables(0).NewRow

            Dim temp As String
            Dim tabla As Integer
            tabla = 0
            If ComboBox1.Text = "Básicos" Then
                tabla = 0

                a = My.Computer.FileSystem.GetDirectories(My.Computer.FileSystem.CurrentDirectory + "\files\basicos\")
                temp = My.Computer.FileSystem.CurrentDirectory + "\files\basicos\"

            ElseIf ComboBox1.Text = "Controladores" Then
                tabla = 1

                a = My.Computer.FileSystem.GetDirectories(My.Computer.FileSystem.CurrentDirectory + "\files\Controladores\")
                temp = My.Computer.FileSystem.CurrentDirectory + "\files\Controladores\"

            Else
                tabla = 2


                a = My.Computer.FileSystem.GetDirectories(My.Computer.FileSystem.CurrentDirectory + "\files\Perifericos\")
                temp = My.Computer.FileSystem.CurrentDirectory + "\files\Perifericos\"
            End If

            Dim temp2 As String

            temp2 = CStr(a.Count + 1)
            While temp2.Length <> 4
                temp2 = "0" + temp2
            End While
            My.Computer.FileSystem.CreateDirectory(temp + temp2 + "_" + TextBox1.Text)

            My.Computer.FileSystem.CopyFile(TextBox5.Text, temp + temp2 + "_" + TextBox1.Text + "\" + TextBox8.Text + ".vhd", True)

            Dim tt() As String
            tt = Split(temp + temp2 + "_" + TextBox1.Text + "\" + TextBox8.Text + ".vhd", "files\")
            TextBox5.Text = tt(1)


            If TextBox6.Text <> "" Then
                Dim t() As String
                t = Split(TextBox6.Text, ".")

                If My.Computer.FileSystem.FileExists(TextBox6.Text) Then
                    My.Computer.FileSystem.CopyFile(TextBox6.Text, temp + temp2 + "_" + TextBox1.Text + "\" + TextBox8.Text + "." + t(t.Length - 1), True)

                    tt = Split(temp + temp2 + "_" + TextBox1.Text + "\" + TextBox8.Text + "." + t(t.Length - 1), "files\")
                    TextBox6.Text = tt(1)
                End If
            End If
            If TextBox7.Text <> "" Then
                My.Computer.FileSystem.CopyFile(TextBox7.Text, temp + temp2 + "_" + TextBox1.Text + "\" + TextBox8.Text + ".pdf", True)
                tt = Split(temp + temp2 + "_" + TextBox1.Text + "\" + TextBox8.Text + ".pdf", "files\")

                TextBox7.Text = tt(1)
            End If
            'My.Computer.FileSystem.CreateDirectory(My.Computer.FileSystem.CurrentDirectory + "\files\Perifericos\" + temp + "_" + TextBox1.Text)
            temp = My.Computer.FileSystem.CurrentDirectory + "\files\Perifericos\" + temp + "_" + TextBox1.Text

            nuevoelemento.Item(0) = TextBox1.Text
            nuevoelemento.Item(1) = TextBox2.Text
            nuevoelemento.Item(2) = TextBox3.Text
            nuevoelemento.Item(3) = TextBox4.Text
            nuevoelemento.Item(4) = TextBox5.Text
            nuevoelemento.Item(5) = TextBox6.Text
            nuevoelemento.Item(6) = TextBox7.Text
            nuevoelemento.Item(7) = TextBox8.Text
            nuevoelemento.Item(8) = TextBox9.Text
            ComponentesDS.Tables(tabla).Rows.Add(nuevoelemento)
            ComponentesDS.Tables(tabla).AcceptChanges()



            ComponentesDS.WriteXml("Componentes.xml")
            AddComponents.carga()
            cargar()
        Else
            MsgBox(err)
        End If
    End Sub
    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click


        habilitar()
        If TextBox1.Enabled = True Then
            TextBox1.Text = ""
            TextBox2.Text = ""
            TextBox3.Text = ""
            TextBox4.Text = ""
            TextBox5.Text = ""
            TextBox6.Text = ""
            TextBox7.Text = ""
            TextBox8.Text = ""
            TextBox9.Text = ""
            ComboBox1.Text = "Básicos"

        Else
            agregar()
        End If
    End Sub

    Private Sub Componentes_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cargar()

    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        If TextBox1.Enabled Then
            habilitar()
        Else
            Me.Close()

        End If
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        habilitar()
        If TextBox1.Enabled = False Then
            If ListBox1.SelectedIndex <> -1 Then
                Dim temp() As String
                temp = Split(ListBox2.Items(ListBox1.SelectedIndex), ",")



               
                ComponentesDS.Tables(CInt(temp(0))).Rows.RemoveAt(CInt(temp(1)))
                ComponentesDS.Tables(CInt(temp(0))).AcceptChanges()
               
                agregar()


            End If

        End If
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        Dim temp() As String
        If ListBox1.SelectedIndex <> -1 Then
            temp = Split(ListBox2.Items(ListBox1.SelectedIndex), ",")
            If MsgBox("Esta seguro de querer eliminar: " + ListBox1.Text, vbYesNo, "Confirmación de eliminación") = vbYes Then
                ComponentesDS.Tables(CInt(temp(0))).Rows.RemoveAt(CInt(temp(1)))
                ComponentesDS.Tables(CInt(temp(0))).AcceptChanges()
            End If
            ComponentesDS.WriteXml("Componentes.xml")
            AddComponents.carga()
            cargar()
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim a As String
        a = OpenFileDialog1.ShowDialog()
        If a = vbOK Then
            TextBox5.Text = OpenFileDialog1.FileName

            'IO.File.Copy(OpenFileDialog1.FileName, System.Reflection.Assembly.GetExecutingAssembly.Location())

        End If

    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim a As String
        a = OpenFileDialog2.ShowDialog()
        If a = vbOK Then
            TextBox6.Text = OpenFileDialog2.FileName

            'IO.File.Copy(OpenFileDialog1.FileName, System.Reflection.Assembly.GetExecutingAssembly.Location())

        End If
    End Sub

    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click
        Dim a As String
        a = OpenFileDialog3.ShowDialog()
        If a = vbOK Then
            TextBox7.Text = OpenFileDialog3.FileName

            'IO.File.Copy(OpenFileDialog1.FileName, System.Reflection.Assembly.GetExecutingAssembly.Location())

        End If
    End Sub


End Class