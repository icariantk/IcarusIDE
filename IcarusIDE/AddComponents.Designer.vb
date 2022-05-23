<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AddComponents
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim ListViewGroup1 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Basicos", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup2 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Periféricos", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup3 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Controladores", System.Windows.Forms.HorizontalAlignment.Left)
        Me.ListView1 = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ListView2 = New System.Windows.Forms.ListView()
        Me.ColumnHeader5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader7 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader8 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Button3 = New System.Windows.Forms.Button()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.ListBox1 = New System.Windows.Forms.ListBox()
        Me.ListBox2 = New System.Windows.Forms.ListBox()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.Button6 = New System.Windows.Forms.Button()
        Me.OpenFileDialog2 = New System.Windows.Forms.OpenFileDialog()
        Me.OpenFileDialog3 = New System.Windows.Forms.OpenFileDialog()
        Me.Button7 = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'ListView1
        '
        Me.ListView1.AllowColumnReorder = True
        Me.ListView1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4})
        Me.ListView1.GridLines = True
        Me.ListView1.Location = New System.Drawing.Point(32, 99)
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(930, 198)
        Me.ListView1.TabIndex = 0
        Me.ListView1.UseCompatibleStateImageBehavior = False
        Me.ListView1.Visible = False
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Nombre"
        Me.ColumnHeader1.Width = 182
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Ocupación aproximada"
        Me.ColumnHeader2.Width = 140
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Latencia"
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Descripción"
        Me.ColumnHeader4.Width = 514
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(160, 70)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(103, 23)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "Lista"
        Me.Button1.UseVisualStyleBackColor = True
        Me.Button1.Visible = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(29, 75)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(125, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Cambiar vista del control:"
        Me.Label1.Visible = False
        '
        'ListView2
        '
        Me.ListView2.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader5, Me.ColumnHeader6, Me.ColumnHeader7, Me.ColumnHeader8})
        ListViewGroup1.Header = "Basicos"
        ListViewGroup1.Name = "Basicos"
        ListViewGroup2.Header = "Periféricos"
        ListViewGroup2.Name = "Periféricos"
        ListViewGroup3.Header = "Controladores"
        ListViewGroup3.Name = "Controladores"
        Me.ListView2.Groups.AddRange(New System.Windows.Forms.ListViewGroup() {ListViewGroup1, ListViewGroup2, ListViewGroup3})
        Me.ListView2.Location = New System.Drawing.Point(32, 303)
        Me.ListView2.Name = "ListView2"
        Me.ListView2.Size = New System.Drawing.Size(930, 198)
        Me.ListView2.TabIndex = 3
        Me.ListView2.UseCompatibleStateImageBehavior = False
        Me.ListView2.Visible = False
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "Nombre"
        '
        'ColumnHeader6
        '
        Me.ColumnHeader6.Text = "Ocupación aproximada"
        '
        'ColumnHeader7
        '
        Me.ColumnHeader7.Text = "Latencia"
        '
        'ColumnHeader8
        '
        Me.ColumnHeader8.Text = "Descripción"
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(831, 59)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(129, 34)
        Me.Button3.TabIndex = 5
        Me.Button3.Text = "Generar"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'FolderBrowserDialog1
        '
        Me.FolderBrowserDialog1.RootFolder = System.Environment.SpecialFolder.MyComputer
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(660, 23)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(165, 34)
        Me.Button2.TabIndex = 6
        Me.Button2.Text = "Agregar"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button4
        '
        Me.Button4.Location = New System.Drawing.Point(660, 60)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(165, 34)
        Me.Button4.TabIndex = 7
        Me.Button4.Text = "Desde FMI"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.DefaultExt = "FMI"
        Me.OpenFileDialog1.Filter = "Fuente Microcontrolador Icarus|*.FMI"
        '
        'ListBox1
        '
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.Location = New System.Drawing.Point(63, 108)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(255, 394)
        Me.ListBox1.TabIndex = 8
        Me.ListBox1.Visible = False
        '
        'ListBox2
        '
        Me.ListBox2.FormattingEnabled = True
        Me.ListBox2.Location = New System.Drawing.Point(422, 108)
        Me.ListBox2.Name = "ListBox2"
        Me.ListBox2.Size = New System.Drawing.Size(257, 394)
        Me.ListBox2.TabIndex = 9
        Me.ListBox2.Visible = False
        '
        'Button5
        '
        Me.Button5.Location = New System.Drawing.Point(524, 24)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(130, 33)
        Me.Button5.TabIndex = 10
        Me.Button5.Text = "Compilar FMI"
        Me.Button5.UseVisualStyleBackColor = True
        '
        'Button6
        '
        Me.Button6.Location = New System.Drawing.Point(526, 60)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(128, 32)
        Me.Button6.TabIndex = 11
        Me.Button6.Text = "Generar archivo Objeto"
        Me.Button6.UseVisualStyleBackColor = True
        '
        'OpenFileDialog2
        '
        Me.OpenFileDialog2.AddExtension = False
        Me.OpenFileDialog2.FileName = "top_level.vhd"
        Me.OpenFileDialog2.Filter = "Archivo VHDL|*.vhd"
        '
        'OpenFileDialog3
        '
        Me.OpenFileDialog3.FileName = "Ram.vhd"
        Me.OpenFileDialog3.Filter = "Archivo RAM|*.vhd"
        '
        'Button7
        '
        Me.Button7.Location = New System.Drawing.Point(388, 24)
        Me.Button7.Name = "Button7"
        Me.Button7.Size = New System.Drawing.Size(130, 33)
        Me.Button7.TabIndex = 12
        Me.Button7.Text = "Añadir Componentes"
        Me.Button7.UseVisualStyleBackColor = True
        '
        'AddComponents
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(974, 521)
        Me.Controls.Add(Me.Button7)
        Me.Controls.Add(Me.Button6)
        Me.Controls.Add(Me.Button5)
        Me.Controls.Add(Me.ListBox2)
        Me.Controls.Add(Me.ListBox1)
        Me.Controls.Add(Me.Button4)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.ListView2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.ListView1)
        Me.Name = "AddComponents"
        Me.Text = "Menú Principal"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ListView2 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader7 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader8 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents FolderBrowserDialog1 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    Friend WithEvents ListBox2 As System.Windows.Forms.ListBox
    Friend WithEvents Button5 As System.Windows.Forms.Button
    Friend WithEvents Button6 As System.Windows.Forms.Button
    Friend WithEvents OpenFileDialog2 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents OpenFileDialog3 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents Button7 As System.Windows.Forms.Button

End Class
