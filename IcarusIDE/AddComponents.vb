Imports System.IO

Public Class AddComponents
    Dim ComponentesDS As New DataSet("Componentes")
    Dim file_fmi As String

    Private Sub compilar()
        'Esta funcion toma los valores que se encuentran en el listbox1(Lista de instrucciones de algun programa FMI) y listobx2(Lista de Asignaciones de memoria de algun programa FMI)
        'y los sustituye con los valores de algun TOP_LEVEL generado para algun hardware de un Micro Controlador Ícarus

        Generar.Text = ""   'Generar.Text sera la salida en pantalla para problemas o avance de compilación
        Dim archivo, temp(), temp2() As String  'archivo mantendrá el contenido de algun programa VHDL, temp y temp2 serán utilizadas para dividir campos en una cadena
        Dim compuesto() As String
        Dim nemonico As String
        Dim inst As String  'cadena que representará al listbox1
        Dim asig As String  'cadena que representará al listbox2

        inst = ""
        asig = ""

        Dim filas(), columnas() As String

        If ListBox1.Items.Count <> 0 Or ListBox2.Items.Count <> 0 Then  'Verificar si hay algo que traducir de código ensamblador a maquina
            For c = 0 To ListBox1.Items.Count - 1
                inst = inst + ListBox1.Items(c) + vbCrLf 'unir todos los elementos de la lista de instrucciones a una cadena

            Next
            For c = 0 To ListBox2.Items.Count - 1
                asig = asig + ListBox2.Items(c) + vbCrLf 'unir todos los elementos de la lista de asignaciones a una cadena

            Next
            OpenFileDialog2.InitialDirectory = file_fmi
            archivo = OpenFileDialog2.ShowDialog() 'Buscar el archivo TOP_LEVEL.VHDL de donde sacar las direcciones de memoria de los componentes
            If archivo = CStr(vbOK) Then

                archivo = FileIO.FileSystem.ReadAllText(OpenFileDialog2.FileName)
                temp = Split(archivo, "Begin")

                filas = Split(temp(temp.Length - 1), vbCrLf)
                '-----------------------------
                'reemplazando los nemonicos con las direcciones del top level
                '-----------------------------
                For c = 0 To filas.Length - 1
                    temp = Split(filas(c), "--")

                    If temp.Length = 2 Then

                        columnas = Split(temp(1), "|")

                        compuesto = Split(columnas(1), ".")

                        nemonico = compuesto(0)
                        If compuesto.Length <> 1 Then
                            compuesto = Split(compuesto(1), ")")
                            inst = Replace(inst, nemonico, columnas(2) + "+")
                            ' asig = Replace(asig, nemonico, columnas(2) + "+")

                        End If
                    End If
                Next
                For c = 0 To filas.Length - 1
                    temp = Split(filas(c), "--")

                    If temp.Length = 2 Then

                        columnas = Split(temp(1), "|")

                        compuesto = Split(columnas(1), ".")

                        nemonico = compuesto(0)

                        compuesto = Split(compuesto(1), ")")

                        temp2 = Split(asig, vbCrLf)
                        For cc = 0 To temp2.Length - 1
                            Dim temp3() As String
                            temp3 = Split(temp2(cc), ",")
                            If temp3.Length = 3 Then

                                temp3(1) = Replace(temp3(1), nemonico, columnas(2) + "+")

                                temp3(1) = temp3(1).Replace(".", "")
                                temp3(1) = temp3(1).Replace(")", "")
                                temp2(cc) = Join(temp3, ",")
                            End If
                        Next
                        asig = Join(temp2, vbCrLf)

                    End If
                Next
                '-----------------------------
                'reemplazando caracteres innecesarios
                '-----------------------------
                'MsgBox(asig)
                inst = inst.Replace(".", "")

                inst = inst.Replace(")", "")

                '-----------------------------
                'Removiendo el numero de linea
                '-----------------------------
                filas = Split(inst, vbCrLf)
                For c = 0 To filas.Length - 2
                    filas(c) = filas(c).Substring(0, filas(c).LastIndexOf(","))
                Next
                inst = Join(filas, vbCrLf)
                filas = Split(asig, vbCrLf)
                For c = 0 To filas.Length - 2
                    filas(c) = filas(c).Substring(0, filas(c).LastIndexOf(","))
                Next
                asig = Join(filas, vbCrLf)
                '-----------------------------
                '  sumando el offset de direcciones e invirtiendo registros "de donde" "a donde"
                '-----------------------------
                filas = Split(inst, vbCrLf)
                For c = 0 To filas.Length - 2
                    columnas = Split(filas(c), ",")
                    temp = Split(columnas(0), "+")
                    If temp.Length = 2 Then
                        columnas(0) = Hex(Val("&H" & temp(0)) + CLng(temp(1)))
                    End If
                    temp = Split(columnas(1), "+")
                    If temp.Length = 2 Then
                        columnas(1) = Hex(Val("&H" & temp(0)) + CLng(temp(1)))
                    End If
                    Dim t As String

                    t = columnas(1)
                    columnas(1) = columnas(0)
                    columnas(0) = t
                    filas(c) = Join(columnas, ",")

                Next
                inst = Join(filas, vbCrLf)
                filas = Split(asig, vbCrLf)
                '---------------------------------------------------
                '            sumando los offsets al valor hexadecimal de las direcciones bases
                '---------------------------------------------------
                For c = 0 To filas.Length - 2
                    columnas = Split(filas(c), ",")
                    temp = Split(columnas(1), "+")
                    If temp.Length = 2 Then
                        columnas(1) = Hex(Val("&H" & temp(0)) + CLng(temp(1)))
                    End If
                    filas(c) = Join(columnas, ",")

                Next
                asig = Join(filas, vbCrLf)
                '-----------------------------------------------------------------
                'Escribiendo las direcciones correspondientes a cada instruccion
                '-----------------------------------------------------------------

                inst = inst.Replace(",", vbCrLf)

                filas = Split(inst, vbCrLf)
                For c = 0 To filas.Length - 2

                    filas(c) = CStr(c) + "=>" + "x""" + filas(c) + """" + ","
                Next
                inst = Join(filas, vbCrLf)
                '-----------------------------------------------------------------
                'Escribiendo los valores correspondientes a cada direccion de memoria en las asignaciones
                '-----------------------------------------------------------------

                filas = Split(asig, vbCrLf)
                For c = 0 To filas.Length - 2
                    temp = Split(filas(c), ".")
                    If temp.Length = 2 Then
                        temp2 = Split(filas(c), ",")
                        filas(c) = temp(1).Substring(0, temp(1).IndexOf(")")) + "=>" + "x""" + temp2(1) + """" + ","
                    End If

                Next
                asig = Join(filas, vbCrLf)
                Dim programa As String

                programa = inst + vbCrLf + asig
                OpenFileDialog3.InitialDirectory = file_fmi

                archivo = OpenFileDialog3.ShowDialog()
                If archivo = vbOK Then
                    archivo = FileIO.FileSystem.ReadAllText(OpenFileDialog3.FileName)
                End If
                programa = programa.Replace(vbCrLf, "")
                '-----------------------------------------------------------------
                'Inyectar el programa codificado en el tag --<programa> del archivo vhdl de la instanciación de la ram.
                '-----------------------------------------------------------------

                archivo = archivo.Replace("--<programa>", programa)
                FileIO.FileSystem.WriteAllText(OpenFileDialog3.FileName, archivo, False)




            End If

        Else
            MsgBox("No se ha compilado ningun archivo FMI")
        End If

    End Sub



    Private Sub ListView1_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ListView1.MouseClick
        'Al hacer click en el listbox1 con el boton derecho, se despliegan las propiedades del elemento en una nueva forma
        If ListView1.SelectedIndices(0) <> -1 Then
            If e.Button = Windows.Forms.MouseButtons.Right Then
                ViewProps.Label1.Text = ListView1.SelectedItems(0).SubItems(0).Text + vbCrLf + ListView1.SelectedItems(0).SubItems(1).Text + vbCrLf + ListView1.SelectedItems(0).SubItems(2).Text + vbCrLf
                ViewProps.Label2.Text = ListView1.SelectedItems(0).SubItems(3).Text
                ViewProps.PictureBox1.ImageLocation = ".\Files\" + ListView1.SelectedItems(0).SubItems(5).Text
                ViewProps.docum = ListView1.SelectedItems(0).SubItems(6).Text


                ViewProps.ShowDialog()
            End If

        End If


    End Sub

    Private Sub ListView1_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ListView1.MouseDoubleClick
        'Al hacer doble click a un elemento del listview1, se agrega al listview2
        Dim a As New ListViewItem
        Dim gp As String
        Dim c As Integer
        Dim d As Integer

        If ListView1.SelectedIndices(0) <> -1 Then
            d = 0
            For c = 0 To ListView2.Items.Count - 1
                If ListView2.Items(c).SubItems(7).Text = ListView1.SelectedItems(0).SubItems(7).Text Then
                    d = d + 1
                End If
            Next

            a = ListView1.SelectedItems(0).Clone
            a.Text = a.Text + "|" + CStr(a.SubItems(7).Text) + "(" + CStr(d) + ".0)"
            Generar.Text = Generar.Text + "Añadiendo componente: " + a.Text + " número " + CStr(d) + vbCrLf
            gp = ListView1.SelectedItems(0).Group.Header
            ListView2.Items.Add(a)
            For c = 0 To ListView2.Groups.Count - 1
                If ListView2.Groups.Item(c).Header = gp Then
                    ListView2.Items.Item(ListView2.Items.Count - 1).Group = ListView2.Groups.Item(c)
                End If
            Next c
            ListView1.RedrawItems(0, ListView1.Items.Count - 1, False)
        End If
    End Sub



    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        'Este boton cambia la apariencia de los listview1 y listview2
        Dim a As Integer
        a = 0
        If ListView1.View = View.Details Then
            Button1.Text = "Lista"
            ListView1.View = View.LargeIcon
            ListView2.View = View.LargeIcon
            a = 1
        End If
        If ListView1.View = View.LargeIcon And a = 0 Then
            Button1.Text = "Iconos pequeños"
            ListView1.View = View.List
            ListView2.View = View.List
            a = 1
        End If
        If ListView1.View = View.List And a = 0 Then
            Button1.Text = "Mosaico"
            ListView1.View = View.SmallIcon
            ListView2.View = View.SmallIcon
            a = 1
        End If
        If ListView1.View = View.SmallIcon And a = 0 Then
            Button1.Text = "Detalles"
            ListView1.View = View.Tile
            ListView2.View = View.Tile
            a = 1
        End If
        If ListView1.View = View.Tile And a = 0 Then
            Button1.Text = "Iconos grandes"
            ListView1.View = View.Details
            ListView2.View = View.Details

        End If


    End Sub

    Private Sub ListView2_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ListView2.MouseDoubleClick
        'al dar doble click en algun elemento del listview2, este se elimina de esa listview
        ListView2.Items.RemoveAt(ListView2.SelectedIndices(0))

        If ListView2.Items.Count <> 0 Then
            ListView2.RedrawItems(0, ListView2.Items.Count - 1, False)
        End If


    End Sub


    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        'Esta función creará las uniones pertinentes entre los componentes seleccionados para crear un micro controlador Ícarus
        'el elemento que este seleccionado en el listview 2 será el elemento con el cual el contador de programa comenzará a contar.
        Generar.Text = ""
        Dim apuntador As String
        Dim pointer As Integer

        Dim ok As Integer
        If ListView2.SelectedItems.Count <> 0 Then
            ok = MsgBox("Se esta utilizando: " + ListView2.SelectedItems(0).Text + " como primera direccion, esta de acuerdo?", MsgBoxStyle.YesNo, "Dirección Base")
        Else
            MsgBox("No ha seleccionado el componente con el cual comenzará el contador de programa a ejecutar los comandos", MsgBoxStyle.Exclamation, "Atención")
            ok = vbNo

        End If
        'se selecciona una carpeta para depositar los archivos necesarios en él.
        If ok <> vbNo Then
            If FolderBrowserDialog1.ShowDialog() <> Windows.Forms.DialogResult.OK Then
                MsgBox("Debe seleccionar un directorio para poder continuar con la generación de los archivos.", MsgBoxStyle.Exclamation, "Información")
            Else
                file_fmi = FolderBrowserDialog1.SelectedPath

                apuntador = ListView2.SelectedItems(0).Index

                Dim MyTXTWriter As System.IO.StreamWriter
                'el archivo creado como top level será llamado top_level.vhd y la cadena que lo conformara
                'se llama top, itm lleva la cuenta del puerto extra a agregar en la variable extras
                'mux será la cadena que conformará el archuivo mux.vhd el cual se encarga de habilitar los módulos
                'seleccionados en el listview2 y al contador de programa.

                MyTXTWriter = My.Computer.FileSystem.OpenTextFileWriter(FolderBrowserDialog1.SelectedPath + "\top_level.vhd", False)

                Dim top As String
                Dim itm As Integer
                Dim mux As String
                Dim hecho As New Collection
                Dim mux_comp As New Collection        'Esta coleccion guarda los componentes a habilitar
                Dim mux_comp_muti As New Collection  'Esta coleccion guarda los componentes de varios registros
                Dim mux_comp_temp As New Collection
                Dim mux_comp_simple As New Collection  'Esta coleccion guarda los componentes de 1 solo registro
                Dim mux_comp_final As New Collection   'Esta coleccion contiene a todos los elementos del mux ordenados

                mux = ""
                Dim entidad(), nombres() As String
                ReDim entidad(ListView2.Items.Count)
                ReDim nombres(ListView2.Items.Count)




                itm = 0

                Dim fileReader, temp() As String
                Dim extras(100) As String  'Esta variable guardará todos los puertos que no se conecten internamente para darles un puerto de salida en el top_level
                Dim b(), bb() As String
                top = vbCrLf + "library ieee;" + vbCrLf + "use ieee.std_logic_1164.ALL;" + vbCrLf + "use ieee.numeric_std.ALL;" + vbCrLf + "library UNISIM;" + vbCrLf + "use UNISIM.Vcomponents.ALL;" + vbCrLf + vbCrLf + "entity Top_1 is" + vbCrLf + "   port (rst:in std_logic;"

                mux = vbCrLf + "library ieee;" + vbCrLf + "use ieee.std_logic_1164.ALL;" + vbCrLf + "use ieee.numeric_std.ALL;" + vbCrLf + "library UNISIM;" + vbCrLf + "use UNISIM.Vcomponents.ALL;" + vbCrLf + vbCrLf + "entity mux is" + vbCrLf
                mux += vbCrLf
                mux += "port ( habilitadores   : out   std_logic_vector (" + CStr(ListView2.Items.Count) + " downto 0); "
                mux += vbCrLf
                mux += "direccion : in    std_logic_vector (31 downto 0));"
                mux += vbCrLf
                mux += "end mux;" + vbCrLf
                mux += "architecture Comportamiento of Mux is"
                mux += vbCrLf
                mux += "Begin"
                mux += vbCrLf
                mux += " habilitadores <=  (" + CStr(ListView2.Items.Count) + "=>'1',others=>'0') when direccion=x""00000000"" else"
                mux += vbCrLf


                For c = 0 To ListView2.Items.Count - 1
                    'Se busca el archivo vhdl correspondiente de cada componente seleccionado en el listview2

                    fileReader = My.Computer.FileSystem.ReadAllText(".\FILES\" + ListView2.Items(c).SubItems(4).Text)
                    fileReader = fileReader.ToUpper

                    b = Split(fileReader, "ENTITY")
                    temp = Split(b(1), "IS")
                    nombres(c) = Trim(temp(0))

                    bb = Split(b(1), "ARCHITECTURE")
                    b = Split(bb(0), "(")

                    ReDim bb(b.Length - 2)


                    For d = 1 To b.Length - 1
                        bb(d - 1) = b(d)
                    Next

                    b = Split(Join(bb, "("), ");")
                    ReDim bb(b.Length - 2)


                    For d = 0 To b.Length - 2
                        bb(d) = b(d)
                    Next


                    entidad(c) = Join(bb, ");")
                    bb = Split(Join(bb, ");"), ";")

                    For d = 0 To bb.Length - 1
                        b = Split(bb(d), ":")
                        b(0) = Trim(b(0).Replace(vbTab, ""))
                        b(0) = Trim(b(0).Replace(vbCrLf, ""))
                        b(0) = Trim(b(0).Replace(vbCr, ""))
                        b(0) = b(0).Replace(" ", "")
                        'Si el archivo contiene puertos distintos a los del estandar del micro controlador ícarus, lo agrega a la variable extras
                        If b(0) <> "DIRECCION" And b(0) <> "DATOS" And b(0) <> "ENABLE" And b(0) <> "WE" And b(0) <> "CLK" And b(0) <> "CLK_FAST" Then
                            b(0) = b(0) + "_" + CStr(c)
                            extras(itm) = b(0)
                            top = top + vbCrLf + "          " + Join(b, ":") + ";"
                            itm = itm + 1
                        End If
                    Next
                Next

                top = top + vbCrLf + " CLK       : in    std_logic); " + vbCrLf + "end Top_1;" + vbCrLf + "architecture Comportamiento of Top_1 is"
                top = top + vbCrLf + "signal Habilitadores   : std_logic_vector (" + CStr(ListView2.Items.Count) + " downto 0);"
                top = top + vbCrLf + "signal Reloj_lento   : std_logic;"
                top = top + vbCrLf + "signal Reloj_medio   : std_logic;"
                top += vbCrLf + "signal datos:Std_logic_vector(31 downto 0);"
                top = top + vbCrLf + "signal WriteEnable   : std_logic;"
                top = top + vbCrLf + "signal Direccion_interna : std_logic_vector (31 downto 0);"

                'se agregan los componentes necesarios para poder hacer las instancias de los mismos (solo 1 se necesita)
                For c = 0 To ListView2.Items.Count - 1
                    If Not (hecho.Contains(nombres(c))) Then

                        top = top + vbCrLf + "component " + nombres(c) + vbCrLf + "Port(" + entidad(c) + ");" + vbCrLf + "end component;"
                        hecho.Add(nombres(c), nombres(c))

                    End If


                Next
                'Se agregan los componentes bases para el micro controlador Ícarus
                top = top + vbCrLf
                top = top + "component Divisor"
                top = top + vbCrLf
                top += "      port ( CLK_in  : in    std_logic; "
                top = top + vbCrLf
                top += "      Clk_out : inout std_logic);"
                top = top + vbCrLf
                top += "    end component;"
                top = top + vbCrLf
                top = top + vbCrLf
                top += "component PC"
                top = top + vbCrLf

                top += "generic ("
                top = top + vbCrLf
                top += "primeradireccion: std_logic_vector(31 downto 0));"
                top = top + vbCrLf
                top += "port ( clk       : in    std_logic;"
                top = top + vbCrLf
                top += "rst       : in    std_logic;"
                top = top + vbCrLf
                top += "Datos     : inout std_logic_vector (31 downto 0); "
                top = top + vbCrLf
                top += "enable : in    std_logic; "
                top = top + vbCrLf
                top += "WE        : in    std_logic);"
                top = top + vbCrLf
                top += "end component;"
                top = top + vbCrLf
                top = top + vbCrLf
                top += "        component Mux"
                top = top + vbCrLf
                top += "port ( habilitadores   : out   std_logic_vector (" + CStr(ListView2.Items.Count) + " downto 0); "
                top = top + vbCrLf
                top += "direccion : in    std_logic_vector (31 downto 0));"
                top = top + vbCrLf
                top += "end component;"
                top = top + vbCrLf
                top = top + vbCrLf
                top += "component Core"
                top = top + vbCrLf
                top += "port ( clk       : in    std_logic; "
                top = top + vbCrLf
                top += "Direccion : out   std_logic_vector (31 downto 0); "

                top = top + vbCrLf

                top += "rst       : in    std_logic;"
                top = top + vbCrLf
                top += "Datos     : inout std_logic_vector (31 downto 0); "
                top = top + vbCrLf
                top += "WE        : out   std_logic);"
                top = top + vbCrLf
                top += "end component;"
                top = top + vbCrLf
                top = top + vbCrLf
                top = top + "Begin" + vbCrLf
                itm = 0
                Dim flag As Integer



                For c = 0 To ListView2.Items.Count - 1
                    'Se hacen las instancias de cada componente agregado al listview2, haciendo las uniones pertinentes a los buses internos 
                    'del micro controlador
                    flag = 0
                    entidad(c) = entidad(c).Replace(":=", "??")

                    b = Split(entidad(c), ";")

                    For d = 0 To b.Length - 1
                        bb = b(d).Split(":")
                        If Trim(bb(0).ToUpper.Replace(vbTab, "").Replace(vbCrLf, "").Replace(vbCr, "")) = "DIRECCION" Then
                            temp = bb(1).Split("(")
                            temp = temp(1).Split(")")
                            flag = 1
                            mux_comp.Add(temp(0))

                            bb(1) = "Direccion_interna(" + temp(0) + ")"
                        ElseIf Trim(bb(0).ToUpper.Replace(vbTab, "").Replace(vbCrLf, "").Replace(vbCr, "")) = "WE" Then
                            bb(1) = "WriteEnable"
                        ElseIf Trim(bb(0).ToUpper.Replace(vbTab, "").Replace(vbCrLf, "").Replace(vbCr, "")) = "RST" Then
                            bb(1) = "RST"
                        ElseIf Trim(bb(0).ToUpper.Replace(vbTab, "").Replace(vbCrLf, "").Replace(vbCr, "")) = "CLK_FAST" Then
                            bb(1) = "clk"
                        ElseIf Trim(bb(0).ToUpper.Replace(vbTab, "").Replace(vbCrLf, "").Replace(vbCr, "")) = "ENABLE" Then
                            bb(1) = "Habilitadores(" + CStr(c) + ")"
                        ElseIf Trim(bb(0).ToUpper.Replace(vbTab, "").Replace(vbCrLf, "").Replace(vbCr, "")) = "CLK" Then
                            If ListView2.Items.Item(c).Text <> "Memoria RAMb16" Then
                                bb(1) = "Reloj_lento"
                            Else
                                bb(1) = "Reloj_lento"
                            End If
                        ElseIf Trim(bb(0).ToUpper.Replace(vbTab, "").Replace(vbCrLf, "").Replace(vbCr, "")) = "DATOS" Then
                            bb(1) = "DATOS"
                        Else
                            bb(1) = extras(itm)
                            itm += 1
                        End If
                        b(d) = Join(bb, "=>")
                    Next
                    entidad(c) = Join(b, ",")
                    entidad(c) = entidad(c).Replace("??", ":=")
                    top = top + vbCrLf + "union_" + nombres(c) + "_" + CStr(c) + ": " + nombres(c) + vbCrLf + "Port map(" + entidad(c) + ");" + vbCrLf
                    If flag = 0 Then
                        mux_comp.Add("1")
                    End If
                Next
                top += vbCrLf
                top += "Core_principal : Core"
                top += vbCrLf
                top += "port map (clk=>Reloj_lento,"
                top += vbCrLf
                top += "Direccion=>Direccion_Interna,"
                top += vbCrLf
                top += "Rst=>rst,"
                top += vbCrLf
                top += "WE=>WriteEnable,"
                top += vbCrLf
                top += "Datos(31 downto 0)=>Datos(31 downto 0));"
                top += vbCrLf
                top += vbCrLf
                top += "Mux_Habilitador: Mux"
                top += vbCrLf
                top += "port map (direccion(31 downto 0)=>Direccion_Interna(31 downto 0),"
                top += vbCrLf
                top += "habilitadores=>Habilitadores);"
                top += vbCrLf
                top += vbCrLf
                top += "ContadorDePrograma:   PC"
                top += vbCrLf
                top += "Generic map("
                top += vbCrLf
                top += "primeradireccion => x""<????>"")"
                top += vbCrLf
                top += "port map (clk=>Reloj_Lento,"
                top += vbCrLf
                top += "enable=>habilitadores(" + CStr(ListView2.Items.Count) + "),"
                top += vbCrLf
                top += "WE=>WriteEnable,"
                top += vbCrLf
                top += "RST=>RST,"
                top += vbCrLf
                top += "Datos=>Datos);"
                top += vbCrLf
                top += vbCrLf

                top += "DivisorDeFrecuencia: Divisor"
                top += vbCrLf
                top += "port map (CLK_in=>CLK,"
                top += vbCrLf
                top += "Clk_out=>Reloj_lento);"
                top += vbCrLf

                top += vbCrLf
                top += "end Comportamiento;"
                'Se dividen los componentes entre los que tienen mas de 1 registro y los que no.
                For c = 1 To mux_comp.Count
                    If mux_comp.Item(c) <> "1" Then
                        mux_comp_muti.Add(c)

                    Else
                        mux_comp_simple.Add(c)
                    End If
                Next
                ReDim temp(1)
                Dim top2 As Long

                top2 = 4294967295
                temp(0) = mux_comp.Item(CInt(mux_comp_muti.Item(1))).ToString.ToUpper().Replace("DOWNTO", "TO")
                b = Split(temp(0), "TO")
                Dim itm2 As Long

                itm2 = Math.Pow(2, Math.Abs(CInt(b(0)) - CInt(b(1))) + 1)
                pointer = 1
                Dim t() As Long
                ReDim t(mux_comp_muti.Count)
                Dim may As Integer
                'Se calculan cuantas direcciones contiene cada elemento de los que tienen mas de 1 registro
                For c = 1 To mux_comp_muti.Count

                    temp(0) = mux_comp.Item(CInt(mux_comp_muti.Item(c))).ToString.ToUpper().Replace("DOWNTO", "TO")
                    b = Split(temp(0), "TO")
                    t(c - 1) = Math.Pow(2, Math.Abs(CInt(b(0)) - CInt(b(1))) + 1)

                Next
                mux_comp_temp.Clear()

                Dim dd As Integer
                'Se ordenan los componentes de mayor a menor
                dd = mux_comp_muti.Count
                For c = 1 To dd
                    may = 0
                    For cc = 1 To dd
                        If may <= t(cc - 1) Then
                            may = t(cc - 1)
                            pointer = cc
                        End If
                    Next

                    t(pointer - 1) = 0
                    'y cada componente que va saliendo como de mayor tamaño, se va agregando a la coleccion temporal del mux
                    mux_comp_temp.Add(mux_comp_muti.Item(pointer))

                Next
                mux_comp_muti.Clear()
                mux_comp_muti = mux_comp_temp

                For c = 1 To mux_comp_muti.Count
                    temp(0) = mux_comp.Item(CInt(mux_comp_muti.Item(c))).ToString.ToUpper().Replace("DOWNTO", "TO")
                    b = Split(temp(0), "TO")
                    itm = Math.Pow(2, Math.Abs(CInt(b(0)) - CInt(b(1))) + 1)
                    'se calcula de nuevo cuantas direcciones contienen los elementos y se agregan con separadores a la coleccion
                    'mux_comp_final
                    mux_comp_final.Add(CStr(top2) + "," + CStr(top2 - itm), CStr(mux_comp_muti.Item(c)))
                    top2 -= itm
                Next
                'despues de acomodar los componentes con más de 1 registro, se añaden todos los de 1 solo registro cronológicamente
                For c = 1 To mux_comp_simple.Count
                    mux_comp_final.Add(CStr(top2) + ", " + CStr(top2 - 1), CStr(mux_comp_simple.Item(c)))
                    top2 -= 1
                Next
                Dim k As String
                'Se adaptan todas las direcciones a 8 caracteres hexadecimales
                For c = 1 To mux_comp_final.Count
                    temp = Split(CStr(mux_comp_final(CStr(c))), ",")
                    k = CStr(Hex(CLng(temp(0))))
                    While (k.Length < 8)
                        k = "F" + k
                    End While

                    'se hace el mux de selección dependiendo la dirección de cada componente
                    mux += "(" + CStr(c - 1) + "=>'1',others=>'0') when direccion <=x""" + k + """"
                    k = CStr(Hex(CLng(temp(1))))
                    While (k.Length < 8)
                        k = "F" + k
                    End While
                    'se agregan comentarios para que el compilador sepa en que direcciones exactamente estan los componentes
                    mux += " and direccion > x""" + k + """ else --" + ListView2.Items(c - 1).Text + "|" + CStr(Hex(CLng(temp(1)) + 1)) + "|" + CStr(CLng(temp(1)))
                    If (c - 1) = apuntador Then
                        Dim aux As String
                        aux = CStr(Hex(CLng(temp(1)) + 1))
                        While aux.Length < 8
                            aux = "F" + aux
                        End While
                        'el tag "<????>" en el top_level indica la dirección con la cual el contador de programa comenzará a contar
                        top = top.Replace("<????>", aux)
                    End If
                    mux += vbCrLf
                Next
                mux += "(others=>'0');"
                mux += vbCrLf
                mux += "end Comportamiento;"

                Generar.Show()

                MyTXTWriter.WriteLine(top)
                MyTXTWriter.WriteLine(vbCrLf)
                MyTXTWriter.WriteLine(vbCrLf)
                MyTXTWriter.WriteLine(vbCrLf)
                MyTXTWriter.WriteLine(mux)


                MyTXTWriter.Close()
                'se copian los archivos pertinentes de la base de datos a la carpeta seleccionada
                For c = 0 To ListView2.Items.Count - 1
                    temp = Split(ListView2.Items(c).SubItems(4).Text, "\")
                    System.IO.File.Copy(".\FILES\" + ListView2.Items(c).SubItems(4).Text, FolderBrowserDialog1.SelectedPath + "\" + temp(temp.Length - 1), True)
                Next
                System.IO.File.Copy(".\FILES\core.vhd", FolderBrowserDialog1.SelectedPath + "\core.vhd", True)
                System.IO.File.Copy(".\FILES\pc.vhd", FolderBrowserDialog1.SelectedPath + "\pc.vhd", True)
                System.IO.File.Copy(".\FILES\divisor.vhd", FolderBrowserDialog1.SelectedPath + "\divisor.vhd", True)

            End If
        End If
        'Si se desea, se puede compilar un programa FMI si se ha desarrollado el hardware desde ese archivo
        If ListBox1.Items.Count <> 0 Then
            Dim a As Integer
            a = MsgBox("Desea generar un archivo objeto del programa FMI?", MsgBoxStyle.YesNo, "Compilar archivo FMI")
            If a = vbYes Then
                compilar()
            Else
                MsgBox("Fin de generación de código VHDL del Micro Controlador Ícarus")
            End If


        End If
    End Sub

    Public Sub carga()
        'Al iniciar el prorgama, se lee la base de datos COMPONENTES.XML y se agregan a la listview1

        Me.Top = 1

        Dim itm As Integer
        itm = 0

        ComponentesDS.Clear()
        ComponentesDS.ReadXml(".\Componentes.xml")
        ListView1.Clear()
        ListView2.Clear()

        For c = 0 To ComponentesDS.Tables.Count - 1
            ListView1.Groups.Add(ComponentesDS.Tables.Item(c).TableName, ComponentesDS.Tables.Item(c).TableName)
            ListView2.Groups.Add(ComponentesDS.Tables.Item(c).TableName, ComponentesDS.Tables.Item(c).TableName)
            For d = 0 To ComponentesDS.Tables(c).Rows.Count - 1
                ListView1.Items.Add(ComponentesDS.Tables(c).Rows(d).Item(0)) 'Nombre
                ListView1.Items(itm).SubItems.Add(ComponentesDS.Tables(c).Rows(d).Item(1)) 'Ocupacion
                ListView1.Items(itm).SubItems.Add(ComponentesDS.Tables(c).Rows(d).Item(2)) 'Latencia
                ListView1.Items(itm).SubItems.Add(ComponentesDS.Tables(c).Rows(d).Item(3)) 'Descripcion
                ListView1.Items(itm).SubItems.Add(ComponentesDS.Tables(c).Rows(d).Item(4)) 'Archivo VHDL
                ListView1.Items(itm).SubItems.Add(ComponentesDS.Tables(c).Rows(d).Item(5)) 'Imagen
                ListView1.Items(itm).SubItems.Add(ComponentesDS.Tables(c).Rows(d).Item(6)) 'Documentacion
                ListView1.Items(itm).SubItems.Add(ComponentesDS.Tables(c).Rows(d).Item(7)) 'nemonico
                ListView1.Items(itm).SubItems.Add(ComponentesDS.Tables(c).Rows(d).Item(8)) 'registros
                ListView1.Items(itm).Group = ListView1.Groups.Item(ComponentesDS.Tables(c).TableName)
                itm = itm + 1
            Next

        Next
    End Sub
    Private Sub AddComponents_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        
        carga()


    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        'Agregar un elemento del listview1 al listview2 al dar click en AGREGAR
        ListBox1.Visible = False
        ListBox2.Visible = False

        ListView1.Visible = True
        ListView2.Visible = True
        Label1.Visible = True
        Button1.Visible = True


        Dim a As New ListViewItem
        Dim gp As String
        Dim c, cc As Integer
        For cc = 0 To ListView1.SelectedItems.Count - 1


            a = ListView1.SelectedItems(cc).Clone
            Generar.Text = Generar.Text + "Añadiendo componente: " + a.Text + vbCrLf
            gp = ListView1.SelectedItems(cc).Group.Header
            ListView2.Items.Add(a)
            For c = 0 To ListView2.Groups.Count - 1

                If ListView2.Groups.Item(c).Header = gp Then
                    ListView2.Items.Item(ListView2.Items.Count - 1).Group = ListView2.Groups.Item(c)
                End If
            Next c
        Next
        ListView1.RedrawItems(0, ListView1.Items.Count - 1, False)
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        'Desde aqui se agregan los componentes en hardware desde un archivo FMI
        Generar.Text = ""
        ListBox1.Visible = False
        ListBox2.Visible = False

        ListView1.Visible = True
        ListView2.Visible = True
        Label1.Visible = True
        Button1.Visible = True
        ListView2.Items.Clear()


        Dim archivo As String
        Dim conteo As Integer
        conteo = 0
        Dim instrucciones As New Collection
        Dim inicio As String
        Dim filas() As String
        Dim columnas() As String
        Dim cp As Integer
        Dim linea As Integer

        Dim enters As New Collection
        Dim labels As New Collection
        Dim componentes As New Collection
        Dim asignaciones As New Collection
        Dim toadd As New Collection
        Dim aliases As New Collection

        Dim errores As New Collection
        Dim fixed_instrucciones As New Collection
        Dim fixed_asignaciones As New Collection
        Dim fixed_asignaciones2 As New Collection
        Dim fixed_instrucciones2 As New Collection
        enters.Clear()
        labels.Clear()
        asignaciones.Clear()
        aliases.Clear()
        errores.Clear()
        ListBox1.Items.Clear()
        ListBox2.Items.Clear()
        fixed_asignaciones.Clear()
        fixed_instrucciones.Clear()
        fixed_asignaciones2.Clear()
        fixed_instrucciones2.Clear()
        toadd.Clear()
        aliases.Add("0", "cp")
        inicio = 0
        cp = 0
        linea = 0
        Generar.Show()



        For c = 0 To ListView1.Items.Count - 1
            componentes.Add(c, ListView1.Items(c).SubItems(7).Text)
        Next


        archivo = OpenFileDialog1.ShowDialog()

        If archivo = vbOK Then
            Generar.TextBox1.Text = Generar.TextBox1.Text + "Iniciando proceso de generación del Micro Controlador Ícarus" + vbCrLf
            file_fmi = OpenFileDialog1.InitialDirectory
            'Se busca el archivo FMI y se lee
            archivo = FileIO.FileSystem.ReadAllText(OpenFileDialog1.FileName)
            'Se eliminan todos los elementos no descriptivos
            archivo = Replace(archivo, vbTab, "")
            archivo = Replace(archivo, " ", "")
            filas = Split(archivo, vbCrLf)
            For c = 0 To filas.Length - 1
                columnas = Split(filas(c), ":")
                If columnas.Length = 2 Then
                    If columnas(1).Trim() <> "" Then
                        filas(c) = Replace(filas(c), ":", ":" + vbCrLf)
                        'se separan las labels de los renglones de las instrucciones
                        enters.Add("añadido", c + 2)
                    End If
                End If
                columnas = Split(filas(c), "'")
                filas(c) = columnas(0)


            Next
            archivo = Join(filas, vbCrLf)
            filas = Split(archivo, vbCrLf)
            Dim desglosado As String

            For cc = 0 To filas.Count - 1
                'Y se analisa línea a línea en busca de instrucciones, asignaciones o del origen del programa
                desglosado = filas(cc)

                conteo = conteo + 1
                linea = linea + 1
                If enters.Contains(conteo) Then
                    linea = linea - 1
                End If
                If desglosado.Trim() <> "" Then
                    columnas = Split(desglosado, ",")
                    If columnas.Length > 3 Then
                        errores.Add("Demasiados parametros en linea " + CStr(linea))
                    Else

                        If columnas(0) = "asigna" Then
                            'asigna un valor, el segundo al priemro

                            If columnas.Length = 3 Then
                                asignaciones.Add(columnas(1) + "," + columnas(2) + "," + CStr(linea))
                            Else
                                errores.Add("Parametros incorrectos en linea " + CStr(linea))
                            End If
                        ElseIf columnas(0) = "origen" Then
                            If columnas.Length > 2 Then
                                errores.Add("Demasiados parametros en linea " + CStr(linea))
                            Else
                                Dim tempA(), tempb() As String
                                tempA = Split(columnas(1), "(")
                                If tempA.Length <> 2 Then
                                    errores.Add("Falta '(' en linea " + CStr(linea))
                                Else
                                    tempb = Split(tempA(1), ")")
                                    If tempb.Length <> 2 Then
                                        errores.Add("Falta ')' en linea " + CStr(linea))
                                    Else
                                        tempA = Split(tempb(0), ".")
                                        If tempA.Length <> 2 Then
                                            errores.Add("Parametros incorrectos en linea ", CStr(linea))
                                        Else
                                            If IsNumeric(tempA(0)) And IsNumeric(tempA(1)) Then
                                                inicio = columnas(1)
                                                Generar.TextBox1.Text = Generar.TextBox1.Text + "Inicio del contador de programa encontrado" + vbCrLf
                                            Else
                                                errores.Add("Parametros incorrectos en linea ", CStr(linea))
                                            End If

                                        End If

                                    End If

                                End If


                            End If

                        ElseIf columnas(0) = "alias" Then
                            'cambia todos los nombres encontrados como alias, por lo del segundo parametro
                            If columnas.Length = 3 Then
                                If aliases.Contains(columnas(2)) Then
                                    errores.Add("Alias repetido en linea " + CStr(linea))
                                Else
                                    aliases.Add(columnas(1), columnas(2))
                                    Generar.TextBox1.Text = Generar.TextBox1.Text + "ALIAS encontrado " + CStr(columnas(2)) + vbCrLf
                                End If


                            Else
                                errores.Add("Parametros incorrectos en linea ", CStr(linea))
                            End If
                        ElseIf columnas(0).EndsWith(":") Then
                            If labels.Contains(columnas(0).Substring(0, columnas(0).Length - 1)) Then
                                errores.Add("Label repetido en linea " + CStr(linea))
                            Else
                                If inicio = "" Then
                                    errores.Add("Imposible determinar direccion de etiqueta en linea " + CStr(linea) + " Falta origen")
                                Else
                                    Dim tempa(), tempb(), tempc As String
                                    tempa = Split(inicio, "(")
                                    tempb = Split(tempa(1), ")")
                                    tempa = Split(tempb(0), ".")
                                    tempc = CStr(CInt(tempa(1)) + CInt(cp))
                                    tempa = Split(inicio, ".")
                                    tempa(1) = tempc + ")"
                                    tempc = Join(tempa, ".")
                                    labels.Add(tempc, columnas(0).Substring(0, columnas(0).Length - 1))
                                End If
                            End If
                        Else
                            If columnas.Length = 2 Then
                                instrucciones.Add(columnas(0) + "," + columnas(1) + "," + CStr(linea))
                                cp = cp + 2
                            Else
                                errores.Add("Parametros incorrectos en linea " + CStr(linea))
                            End If
                        End If
                    End If

                End If

            Next

            Generar.TextBox1.Text = Generar.TextBox1.Text + "Reemplazando etiquetas" + vbCrLf

            For c = 1 To instrucciones.Count
                columnas = Split(instrucciones.Item(c), ",")
                If columnas(0).StartsWith("$") Then
                    If aliases.Contains(columnas(0).Substring(1, columnas(0).Length - 1)) Then
                        columnas(0) = aliases(columnas(0).Substring(1, columnas(0).Length - 1))
                    Else
                        errores.Add("No se encuentra el Alias: " + columnas(0) + " en linea " + columnas(2))
                    End If
                End If
                If columnas(1).StartsWith("$") Then
                    If aliases.Contains(columnas(1).Substring(1, columnas(1).Length - 1)) Then
                        columnas(1) = aliases(columnas(1).Substring(1, columnas(1).Length - 1))
                    Else
                        errores.Add("No se encuentra el Alias: " + columnas(1) + " en linea " + columnas(2))
                    End If
                End If
                If columnas(0).StartsWith("@") Then
                    If labels.Contains(columnas(0).Substring(0, columnas(0).Length - 1)) Then
                        columnas(1) = labels(columnas(0).Substring(1, columnas(0).Length - 1))
                    Else
                        errores.Add("No se encuentra la etiqueta: " + columnas(0) + " en linea " + columnas(2))
                    End If
                End If
                If columnas(1).StartsWith("@") Then
                    If labels.Contains(columnas(1).Substring(1, columnas(1).Length - 1)) Then
                        columnas(1) = labels(columnas(1).Substring(1, columnas(1).Length - 1))
                    Else
                        errores.Add("No se encuentra la etiqueta: " + columnas(1) + " en linea " + columnas(2))
                    End If
                End If
                fixed_instrucciones.Add(Join(columnas, ","))
            Next
            For c = 1 To asignaciones.Count
                columnas = Split(asignaciones.Item(c), ",")
                If columnas(0).StartsWith("$") Then
                    If aliases.Contains(columnas(0).Substring(1, columnas(0).Length - 1)) Then
                        columnas(0) = aliases(columnas(0).Substring(1, columnas(0).Length - 1))
                    Else
                        errores.Add("No se encuentra el Alias: " + columnas(0) + " en linea " + columnas(2))
                    End If
                End If
                If columnas(1).StartsWith("$") Then
                    If aliases.Contains(columnas(1).Substring(1, columnas(1).Length - 1)) Then
                        columnas(1) = aliases(columnas(1).Substring(1, columnas(1).Length - 1))
                    Else
                        errores.Add("No se encuentra el Alias: " + columnas(1) + " en linea " + columnas(2))
                    End If
                End If
                If columnas(0).StartsWith("@") Then
                    If labels.Contains(columnas(0).Substring(0, columnas(0).Length - 1)) Then
                        columnas(1) = labels(columnas(0).Substring(1, columnas(0).Length - 1))
                    Else
                        errores.Add("No se encuentra la etiqueta: " + columnas(0) + " en linea " + columnas(2))
                    End If
                End If
                If columnas(1).StartsWith("@") Then
                    If labels.Contains(columnas(1).Substring(1, columnas(1).Length - 1)) Then
                        columnas(1) = labels(columnas(1).Substring(1, columnas(1).Length - 1))
                    Else
                        errores.Add("No se encuentra la etiqueta: " + columnas(1) + " en linea " + columnas(2))
                    End If
                End If
                fixed_asignaciones.Add(Join(columnas, ","))
            Next


            Generar.TextBox1.Text = Generar.TextBox1.Text + "Estandarizando datos a 32 bits" + vbCrLf
            For c = 1 To fixed_instrucciones.Count
                columnas = Split(fixed_instrucciones.Item(c), ",")
                If IsNumeric(columnas(0)) Then
                    columnas(0) = Hex(CStr(columnas(0)))
                    While (columnas(0).Length <> 8)
                        columnas(0) = "0" + columnas(0)
                    End While
                End If
                If IsNumeric(columnas(1)) Then
                    columnas(1) = Hex(CStr(columnas(1)))
                    While (columnas(1).Length <> 8)
                        columnas(1) = "0" + columnas(1)
                    End While
                End If
                If columnas(1) = "$cp" Or columnas(1) = "00000000" Then
                    errores.Add("El contador de programas no debe ser leído en linea " + columnas(2))
                Else
                    fixed_instrucciones2.Add(Join(columnas, ","))
                End If

            Next


            'MsgBox("fixed asignaciones")
            For c = 1 To fixed_asignaciones.Count
                columnas = Split(fixed_asignaciones.Item(c), ",")
                If IsNumeric(columnas(0)) Then
                    columnas(0) = Hex(CStr(columnas(0)))
                    While (columnas(0).Length <> 8)
                        columnas(0) = "0" + columnas(0)
                    End While
                End If
                If IsNumeric(columnas(1)) Then
                    columnas(1) = Hex(CStr(columnas(1)))
                    While (columnas(1).Length <> 8)
                        columnas(1) = "0" + columnas(1)
                    End While
                End If
                fixed_asignaciones2.Add(Join(columnas, ","))
            Next
            fixed_instrucciones.Clear()
            Dim temp() As String
            Dim temp2() As String
            Dim temp3() As String
            Generar.TextBox1.Text = Generar.TextBox1.Text + "Añadiendo hardware a la lista" + vbCrLf
            For c = 1 To fixed_instrucciones2.Count
                columnas = Split(fixed_instrucciones2.Item(c), ",")
                temp = Split(columnas(0), "(")
                If temp.Length <> 1 Then
                    temp2 = Split(temp(1), ")")
                    If temp2.Length <> 2 Then
                        errores.Add("Falta ')' en linea " + columnas(2))
                    Else
                        temp3 = Split(temp2(0), ".")
                        If temp3.Length <> 2 Then
                            errores.Add("Mala descripcion de direccion en linea " + columnas(2))
                        Else
                            If IsNumeric(temp3(0)) And IsNumeric(temp3(1)) Then
                                If componentes.Contains(temp(0)) Then
                                    If Not toadd.Contains(CStr(CStr(componentes(temp(0))) + "_" + temp3(0))) Then
                                        Dim aa(), ab(), ac() As String
                                        aa = Split(inicio, "(")
                                        ab = Split(aa(1), ")")
                                        ac = Split(ab(0), ".")




                                        If CStr(componentes(aa(0))) + "_" + CStr(ac(0)) = CStr(componentes(temp(0))) + "_" + temp3(0) Then
                                            If Not toadd.Contains(CStr(componentes(temp(0))) + "_" + temp3(0)) Then
                                                toadd.Add(temp(0) + "(" + CStr(temp3(0)) + ".0)," + CStr(componentes(temp(0))) + ",inicio", CStr(componentes(temp(0))) + "_" + CStr(temp3(0)))
                                            End If
                                        Else
                                            If Not toadd.Contains(CStr(componentes(temp(0))) + "_" + temp3(0)) Then
                                                toadd.Add(temp(0) + "(" + CStr(temp3(0)) + ".0)," + CStr(componentes(temp(0))), CStr(componentes(temp(0))) + "_" + CStr(temp3(0)))
                                            End If
                                        End If

                                    End If

                                Else
                                    errores.Add("No existe componente " + temp(0) + " en linea " + columnas(2))

                                End If


                            Else
                                errores.Add("Mala descripcion de direccion en linea " + columnas(2))
                            End If
                        End If

                    End If


                End If

                temp = Split(columnas(1), "(")
                If temp.Length <> 1 Then
                    temp2 = Split(temp(1), ")")
                    If temp2.Length <> 2 Then
                        errores.Add("Falta ')' en linea " + columnas(2))
                    Else
                        temp3 = Split(temp2(0), ".")
                        If temp3.Length <> 2 Then
                            errores.Add("Mala descripcion de direccion en linea " + columnas(2))
                        Else
                            If IsNumeric(temp3(0)) And IsNumeric(temp3(1)) Then
                                If componentes.Contains(temp(0)) Then
                                    Dim aa(), ab(), ac() As String
                                    aa = Split(inicio, "(")
                                    ab = Split(aa(1), ")")
                                    ac = Split(ab(0), ".")




                                    If CStr(componentes(aa(0))) + "_" + CStr(ac(0)) = CStr(componentes(temp(0))) + "_" + temp3(0) Then
                                        If Not toadd.Contains(CStr(componentes(temp(0))) + "_" + temp3(0)) Then
                                            toadd.Add(temp(0) + "(" + CStr(temp3(0)) + ".0)," + CStr(componentes(temp(0))) + ",inicio", CStr(componentes(temp(0))) + "_" + CStr(temp3(0)))
                                        End If
                                    Else
                                        If Not toadd.Contains(CStr(componentes(temp(0))) + "_" + temp3(0)) Then
                                            toadd.Add(temp(0) + "(" + CStr(temp3(0)) + ".0)," + CStr(componentes(temp(0))), CStr(componentes(temp(0))) + "_" + CStr(temp3(0)))
                                        End If
                                    End If

                                Else
                                    errores.Add("No existe componente " + temp(0) + " en linea " + columnas(2))

                                End If


                            Else
                                errores.Add("Mala descripcion de direccion en linea " + columnas(2))
                            End If
                        End If

                    End If


                End If
            Next
            temp = Split(inicio, "(")
            temp2 = Split(temp(1), ")")
            temp3 = Split(temp2(0), ".")
            Generar.TextBox1.Text = Generar.TextBox1.Text + "Copiando elementos del repertorio IP" + vbCrLf
            Dim start As Integer
            For c = 1 To toadd.Count

                start = 0
                Dim a As ListViewItem
                Dim gp As String

                Dim t() As String
                t = Split(toadd.Item(c), ",")
                If t.Length = 3 Then
                    a = ListView1.Items.Item(CInt(t(1))).Clone

                    gp = ListView1.Items.Item(CInt(t(1))).Group.Header
                    start = 1
                Else
                    a = ListView1.Items.Item(CInt(t(1))).Clone
                    gp = ListView1.Items.Item(CInt(t(1))).Group.Header
                End If
                'ListView1.Items.RemoveAt(ListView1.SelectedIndices(0))
                a.Text = a.Text + "|" + t(0)
                Generar.Text = Generar.Text + "Añadiendo componente: " + a.Text + vbCrLf
                ListView2.Items.Add(a)
                For cd = 0 To ListView2.Groups.Count - 1

                    If ListView2.Groups.Item(cd).Header = gp Then
                        ListView2.Items.Item(ListView2.Items.Count - 1).Group = ListView2.Groups.Item(cd)
                    End If
                Next cd

                If start = 1 Then

                    ListView2.Items(ListView2.Items.Count - 1).Selected = True
                    ListView2.Select()


                    start = 0
                End If
                ListView1.RedrawItems(0, ListView1.Items.Count - 1, False)

            Next
            For c = 1 To fixed_instrucciones2.Count
                ListBox1.Items.Add(fixed_instrucciones2.Item(c))
            Next

            For c = 1 To fixed_asignaciones2.Count
                ListBox2.Items.Add(fixed_asignaciones2.Item(c))

            Next
            If inicio <> "" Then

            End If
            If errores.Count <> 0 Then
                ListBox1.Items.Clear()
                ListBox2.Items.Clear()
                ListView2.Items.Clear()


                Generar.TextBox1.Text = "Errores encontrados en compilación:" + vbCrLf + "----------------------------------------------------------------------------------------------------------------------------------------------------------------------------" + vbCrLf
                For c = 1 To errores.Count
                    Generar.TextBox1.Text = Generar.TextBox1.Text + errores.Item(c) + vbCrLf
                Next
                Generar.TextBox1.Text += "----------------------------------------------------------------------------------------------------------------------------------------------------------------------------" + vbCrLf



            End If


        End If
        Generar.Show()
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        'Esta subrutina genera un código hexadecimal de un archivo FMI tomando en cuenta una generación de un micro controlador Ícarus previa
        Generar.Text = ""
        ListBox1.Visible = True
        ListBox2.Visible = True
        ListView1.Visible = False
        ListView2.Visible = False
        Label1.Visible = False
        Button1.Visible = False





        Dim archivo As String
        Dim conteo As Integer
        conteo = 0
        Dim instrucciones As New Collection
        Dim inicio As String
        Dim filas() As String
        Dim columnas() As String
        Dim cp As Integer
        Dim linea As Integer

        Dim enters As New Collection
        Dim labels As New Collection
        Dim componentes As New Collection
        Dim asignaciones As New Collection

        Dim aliases As New Collection

        Dim errores As New Collection
        Dim fixed_instrucciones As New Collection
        Dim fixed_asignaciones As New Collection
        Dim fixed_asignaciones2 As New Collection
        Dim fixed_instrucciones2 As New Collection
        enters.Clear()
        labels.Clear()
        asignaciones.Clear()
        aliases.Clear()
        ListBox1.Items.Clear()
        ListBox2.Items.Clear()

        errores.Clear()
        fixed_asignaciones.Clear()
        fixed_instrucciones.Clear()
        fixed_asignaciones2.Clear()
        fixed_instrucciones2.Clear()

        aliases.Add("0", "cp")
        inicio = 0
        cp = 0
        linea = 0

        For c = 0 To ListView1.Items.Count - 1
            componentes.Add(c, ListView1.Items(c).SubItems(7).Text)
        Next


        archivo = OpenFileDialog1.ShowDialog()
        If archivo = vbOK Then
            file_fmi = OpenFileDialog1.InitialDirectory

            archivo = FileIO.FileSystem.ReadAllText(OpenFileDialog1.FileName)

            archivo = Replace(archivo, vbTab, "")
            archivo = Replace(archivo, " ", "")
            filas = Split(archivo, vbCrLf)
            For c = 0 To filas.Length - 1
                columnas = Split(filas(c), ":")
                If columnas.Length = 2 Then
                    If columnas(1).Trim() <> "" Then
                        filas(c) = Replace(filas(c), ":", ":" + vbCrLf)
                        enters.Add("añadido", c + 2)
                    End If
                End If
                columnas = Split(filas(c), "'")
                filas(c) = columnas(0)


            Next
            archivo = Join(filas, vbCrLf)
            'archivo = Replace(archivo, ":", ":" + vbCrLf)
            filas = Split(archivo, vbCrLf)
            Dim desglosado As String

            For cc = 0 To filas.Count - 1
                desglosado = filas(cc)

                conteo = conteo + 1
                linea = linea + 1
                If enters.Contains(conteo) Then
                    linea = linea - 1
                End If
                If desglosado.Trim() <> "" Then
                    columnas = Split(desglosado, ",")
                    If columnas.Length > 3 Then
                        errores.Add("Demasiados parametros en linea " + CStr(linea))
                    Else

                        If columnas(0) = "asigna" Then
                            'asigna un valor, el segundo al priemro

                            If columnas.Length = 3 Then
                                asignaciones.Add(columnas(1) + "," + columnas(2) + "," + CStr(linea))
                            Else
                                errores.Add("Parametros incorrectos en linea " + CStr(linea))
                            End If
                        ElseIf columnas(0) = "origen" Then
                            If columnas.Length > 2 Then
                                errores.Add("Demasiados parametros en linea " + CStr(linea))
                            Else
                                Dim tempA(), tempb() As String
                                tempA = Split(columnas(1), "(")
                                If tempA.Length <> 2 Then
                                    errores.Add("Falta '(' en linea " + CStr(linea))
                                Else
                                    tempb = Split(tempA(1), ")")
                                    If tempb.Length <> 2 Then
                                        errores.Add("Falta ')' en linea " + CStr(linea))
                                    Else
                                        tempA = Split(tempb(0), ".")
                                        If tempA.Length <> 2 Then
                                            errores.Add("Parametros incorrectos en linea ", CStr(linea))
                                        Else
                                            If IsNumeric(tempA(0)) And IsNumeric(tempA(1)) Then
                                                inicio = columnas(1)
                                            Else
                                                errores.Add("Parametros incorrectos en linea ", CStr(linea))
                                            End If

                                        End If

                                    End If

                                End If


                            End If

                        ElseIf columnas(0) = "alias" Then
                            'cambia todos los nombres enconrados como alias, por lo del segundo parametro
                            If columnas.Length = 3 Then
                                If aliases.Contains(columnas(2)) Then
                                    errores.Add("Alias repetido en linea " + CStr(linea))
                                Else
                                    aliases.Add(columnas(1), columnas(2))
                                End If


                            Else
                                errores.Add("Parametros incorrectos en linea ", CStr(linea))
                            End If
                        ElseIf columnas(0).EndsWith(":") Then
                            If labels.Contains(columnas(0).Substring(0, columnas(0).Length - 1)) Then
                                errores.Add("Label repetido en linea " + CStr(linea))
                            Else
                                If inicio = "" Then
                                    errores.Add("Imposible determinar direccion de etiqueta en linea " + CStr(linea) + " Falta origen")
                                Else
                                    Dim tempa(), tempb(), tempc As String
                                    tempa = Split(inicio, "(")
                                    tempb = Split(tempa(1), ")")
                                    tempa = Split(tempb(0), ".")
                                    tempc = CStr(CInt(tempa(1)) + CInt(cp))
                                    tempa = Split(inicio, ".")
                                    tempa(1) = tempc + ")"
                                    tempc = Join(tempa, ".")


                                    labels.Add(tempc, columnas(0).Substring(0, columnas(0).Length - 1))
                                End If

                            End If

                        Else
                            If columnas.Length = 2 Then
                                instrucciones.Add(columnas(0) + "," + columnas(1) + "," + CStr(linea))
                                cp = cp + 2
                            Else
                                errores.Add("Parametros incorrectos en linea " + CStr(linea))
                            End If
                        End If
                    End If

                End If

            Next



            For c = 1 To instrucciones.Count
                columnas = Split(instrucciones.Item(c), ",")
                If columnas(0).StartsWith("$") Then
                    If aliases.Contains(columnas(0).Substring(1, columnas(0).Length - 1)) Then
                        columnas(0) = aliases(columnas(0).Substring(1, columnas(0).Length - 1))
                    Else
                        errores.Add("No se encuentra el Alias: " + columnas(0) + " en linea " + columnas(2))
                    End If
                End If
                If columnas(1).StartsWith("$") Then
                    If aliases.Contains(columnas(1).Substring(1, columnas(1).Length - 1)) Then
                        columnas(1) = aliases(columnas(1).Substring(1, columnas(1).Length - 1))
                    Else
                        errores.Add("No se encuentra el Alias: " + columnas(1) + " en linea " + columnas(2))
                    End If
                End If
                If columnas(0).StartsWith("@") Then
                    If labels.Contains(columnas(0).Substring(0, columnas(0).Length - 1)) Then
                        columnas(1) = labels(columnas(0).Substring(1, columnas(0).Length - 1))
                    Else
                        errores.Add("No se encuentra la etiqueta: " + columnas(0) + " en linea " + columnas(2))
                    End If
                End If
                If columnas(1).StartsWith("@") Then
                    If labels.Contains(columnas(1).Substring(1, columnas(1).Length - 1)) Then
                        columnas(1) = labels(columnas(1).Substring(1, columnas(1).Length - 1))
                    Else
                        errores.Add("No se encuentra la etiqueta: " + columnas(1) + " en linea " + columnas(2))
                    End If
                End If
                fixed_instrucciones.Add(Join(columnas, ","))
            Next
            For c = 1 To asignaciones.Count
                columnas = Split(asignaciones.Item(c), ",")
                If columnas(0).StartsWith("$") Then
                    If aliases.Contains(columnas(0).Substring(1, columnas(0).Length - 1)) Then
                        columnas(0) = aliases(columnas(0).Substring(1, columnas(0).Length - 1))
                    Else
                        errores.Add("No se encuentra el Alias: " + columnas(0) + " en linea " + columnas(2))
                    End If
                End If
                If columnas(1).StartsWith("$") Then
                    If aliases.Contains(columnas(1).Substring(1, columnas(1).Length - 1)) Then
                        columnas(1) = aliases(columnas(1).Substring(1, columnas(1).Length - 1))
                    Else
                        errores.Add("No se encuentra el Alias: " + columnas(1) + " en linea " + columnas(2))
                    End If
                End If
                If columnas(0).StartsWith("@") Then
                    If labels.Contains(columnas(0).Substring(0, columnas(0).Length - 1)) Then
                        columnas(1) = labels(columnas(0).Substring(1, columnas(0).Length - 1))
                    Else
                        errores.Add("No se encuentra la etiqueta: " + columnas(0) + " en linea " + columnas(2))
                    End If
                End If
                If columnas(1).StartsWith("@") Then
                    If labels.Contains(columnas(1).Substring(1, columnas(1).Length - 1)) Then
                        columnas(1) = labels(columnas(1).Substring(1, columnas(1).Length - 1))
                    Else
                        errores.Add("No se encuentra la etiqueta: " + columnas(1) + " en linea " + columnas(2))
                    End If
                End If
                fixed_asignaciones.Add(Join(columnas, ","))
            Next

            'MsgBox("fixed instruciones")
            For c = 1 To fixed_instrucciones.Count
                columnas = Split(fixed_instrucciones.Item(c), ",")
                If IsNumeric(columnas(0)) Then
                    columnas(0) = Hex(CStr(columnas(0)))
                    While (columnas(0).Length <> 8)
                        columnas(0) = "0" + columnas(0)
                    End While
                End If
                If IsNumeric(columnas(1)) Then
                    columnas(1) = Hex(CStr(columnas(1)))
                    While (columnas(1).Length <> 8)
                        columnas(1) = "0" + columnas(1)
                    End While
                End If
                If columnas(1) = "$cp" Or columnas(1) = "00000000" Then
                    errores.Add("El contador de programas no debe ser leído en linea " + columnas(2))
                Else
                    fixed_instrucciones2.Add(Join(columnas, ","))
                End If

            Next


            'MsgBox("fixed asignaciones")
            For c = 1 To fixed_asignaciones.Count
                columnas = Split(fixed_asignaciones.Item(c), ",")
                If IsNumeric(columnas(0)) Then
                    columnas(0) = Hex(CStr(columnas(0)))
                    While (columnas(0).Length <> 8)
                        columnas(0) = "0" + columnas(0)
                    End While
                End If
                If IsNumeric(columnas(1)) Then
                    columnas(1) = Hex(CStr(columnas(1)))
                    While (columnas(1).Length <> 8)
                        columnas(1) = "0" + columnas(1)
                    End While
                End If
                fixed_asignaciones2.Add(Join(columnas, ","))
            Next
            fixed_instrucciones.Clear()
            Dim temp() As String
            Dim temp2() As String
            Dim temp3() As String

            For c = 1 To fixed_instrucciones2.Count
                columnas = Split(fixed_instrucciones2.Item(c), ",")
                temp = Split(columnas(0), "(")
                If temp.Length <> 1 Then
                    temp2 = Split(temp(1), ")")
                    If temp2.Length <> 2 Then
                        errores.Add("Falta ')' en linea " + columnas(2))
                    Else
                        temp3 = Split(temp2(0), ".")
                        If temp3.Length <> 2 Then
                            errores.Add("Mala descripcion de direccion en linea " + columnas(2))
                        Else
                            If IsNumeric(temp3(0)) And IsNumeric(temp3(1)) Then
                                If Not componentes.Contains(temp(0)) Then


                                    errores.Add("No existe componente " + temp(0) + " en linea " + columnas(2))

                                End If


                            Else
                                errores.Add("Mala descripcion de direccion en linea " + columnas(2))
                            End If
                        End If

                    End If


                End If

                temp = Split(columnas(1), "(")
                If temp.Length <> 1 Then
                    temp2 = Split(temp(1), ")")
                    If temp2.Length <> 2 Then
                        errores.Add("Falta ')' en linea " + columnas(2))
                    Else
                        temp3 = Split(temp2(0), ".")
                        If temp3.Length <> 2 Then
                            errores.Add("Mala descripcion de direccion en linea " + columnas(2))
                        Else
                            If IsNumeric(temp3(0)) And IsNumeric(temp3(1)) Then
                                If Not componentes.Contains(temp(0)) Then



                                    errores.Add("No existe componente " + temp(0) + " en linea " + columnas(2))

                                End If


                            Else
                                errores.Add("Mala descripcion de direccion en linea " + columnas(2))
                            End If
                        End If

                    End If


                End If
            Next
            temp = Split(inicio, "(")
            temp2 = Split(temp(1), ")")
            temp3 = Split(temp2(0), ".")

            For c = 1 To fixed_instrucciones2.Count
                ListBox1.Items.Add(fixed_instrucciones2.Item(c))
            Next

            For c = 1 To fixed_asignaciones2.Count
                ListBox2.Items.Add(fixed_asignaciones2.Item(c))
            Next
            If inicio <> "" Then

            End If
            If errores.Count <> 0 Then
                ListBox1.Items.Clear()
                ListBox2.Items.Clear()


                Generar.TextBox1.Text = "Errores encontrados en compilación:" + vbCrLf + "----------------------------------------------------------------------------------------------------------------------------------------------------------------------------" + vbCrLf
                For c = 1 To errores.Count
                    Generar.TextBox1.Text = Generar.TextBox1.Text + errores.Item(c) + vbCrLf
                Next
                Generar.TextBox1.Text += "----------------------------------------------------------------------------------------------------------------------------------------------------------------------------" + vbCrLf
                Generar.Show()

            End If


        End If


    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click

        compilar()
    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        Componentes.Show()

    End Sub
End Class

