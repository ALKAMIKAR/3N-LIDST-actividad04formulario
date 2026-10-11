using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _3N_LIDST_actividad04formulario
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //declaración de manejadores/controladores en evento
            tbxName.TextChanged += ValidarNombre;
            tbxApellidos.TextChanged += ValidarApellido;
            tbxEdad.TextChanged += ValidarEdad;
            tbxEstatura.TextChanged += ValidarEstatura;
            tbxTelefono.Leave += ValidarTelefono;
            //leave cuando el usuario sale del textbox (quita el cursor)


        }
        private void ValidarTelefono(object sender, EventArgs e)
        {
            TextBox textboxLocal = (TextBox)sender;
            if (!TelefonoValido(textboxLocal.Text))
            {
                MessageBox.Show("Escriba un telefono valido", "Error Telefono", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textboxLocal.BackColor = System.Drawing.Color.LightPink;
                textboxLocal.BackColor = System.Drawing.Color.IndianRed;
                //textboxLocal.Clear();
            }
            else
            {
                textboxLocal.BackColor = System.Drawing.Color.LightGreen;
            }
        }
        

        private bool TelefonoValido(string valor)
        {
            long resultado;
            return long.TryParse(valor, out resultado) && valor.Length == 10;
        }

        private void ValidarEstatura(object sender, EventArgs e)
        {
            TextBox textboxLocal = (TextBox)sender;
            if (!EsDecimalValido(textboxLocal.Text))
            {
                MessageBox.Show("Escriba una estatura valida", "Error Estatura", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textboxLocal.Clear();
            }
        }

        private bool EsDecimalValido(string valor)
        {
            decimal resultado;
            return decimal.TryParse(valor, out resultado);
        }

        private void ValidarEdad(object sender, EventArgs e)
        {
            TextBox textLocal = (TextBox)sender;
            if (!EsEnteroValido(textLocal.Text))
            {
                MessageBox.Show("Escriba una edad valida", "Error Edad", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textLocal.Clear();
            }
        }

        private bool EsEnteroValido(string valor)
        {
            int resultado;
            return int.TryParse(valor, out resultado);
        }

        private void ValidarNombre(object sender, EventArgs e)
        {
            TextBox textboxLocal = (TextBox)sender;
            if (!EsTextoValido(textboxLocal.Text))
            {
                MessageBox.Show("Escriba un nombre valido", "Error Nombre", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textboxLocal.Clear();
            }
        }

        private void ValidarApellido(object sender, EventArgs e)
        {
            TextBox textboxLocal = (TextBox)sender;
            if (!EsTextoValido(textboxLocal.Text))
            {
                MessageBox.Show("Escriba un apellido valido", "Error Apellido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textboxLocal.Clear();
            }
        }

        private bool EsTextoValido(string Valor)
        {
            return Regex.IsMatch(Valor, @"^[a-zA-Z\s]+$");
            //^[a-zA-Z\s]+\$
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
