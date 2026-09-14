using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _3N_LIDST_actividad04formulario
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string nombre = tbxName.Text;
            string apellidos = tbxApellidos.Text;
            string telefono = tbxTelefono.Text;
            string edad = tbxEdad.Text;
            string estatura = tbxEstatura.Text;
            string genero = "";
            if (rbMujer.Checked)
            {
                genero = "Femenino";
            }
            else if (rbHombre.Checked)
            {
                genero = "Masculino";
            }
            else if (rbOtros.Checked)
            {
                genero = "Otro";
            }
            string mensaje = $" Nombre: {nombre}\n\r Apellidos: {apellidos}\n\r Telefono: {telefono}\n\r Edad: {edad}\n\r Estatura: {estatura}\n\r Genero: {genero}";
            // ruta de almacenamiento
            string rutaArchivo = @"D:\Pruebadecsharp\archivo.txt";
            bool ArchivoExiste = File.Exists(rutaArchivo);
            using (StreamWriter escritor = new StreamWriter(rutaArchivo, true))
            {
                if (ArchivoExiste)
                {
                    escritor.WriteLine();
                }
                escritor.WriteLine(mensaje);

            }
            MessageBox.Show(mensaje, "Confirmación de resgistro", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            tbxName.Clear();
            tbxApellidos.Clear();
            tbxTelefono.Clear();
            tbxEstatura.Clear();
            tbxEdad.Clear();
            rbMujer.Checked = false;
            rbHombre.Checked = false;
            rbOtros.Checked = false;
        }
    }
}
