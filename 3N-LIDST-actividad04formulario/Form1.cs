using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO; //Libreria para lectura de archivos de TXT
using System.Text.RegularExpressions; //Libreria para evaluaciones de estructuras de texto
using MySql.Data.MySqlClient; // Libreria para la conexion y uso de MySQL
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Remoting.Messaging;
using System.Runtime.InteropServices;

namespace _3N_LIDST_actividad04formulario
{
    public partial class Form1 : Form
    {
        // Cadena de conexión (ajusta Uid y Pwd a tu MySQL)
        //string SQLConection ="Server=localhost;Port=3306;Database=programacion;Uid=root;Pwd=;" ;
        string SQLConection = "Server=localhost;Port=3306;Database=data_base_pa;Uid=root;Pwd=admin;";

        public Form1()
        {
            InitializeComponent();

            // Declaración de manejadores de eventos
            tbxName.TextChanged += ValidarNombre;
            tbxApellidos.TextChanged += ValidarApellido;
            tbxEdad.TextChanged += ValidarEdad;
            tbxEstatura.TextChanged += ValidarEstatura;
            tbxTelefono.Leave += ValidarTelefono; // Leave: cuando el usuario sale del textbox
        }

        // ---------------- BASE DE DATOS ----------------
        private void InsertarRegistros(string nombre, string apellidos,
            int edad, decimal estatura, string telefono, string genero)
        {
            using (MySqlConnection conectar = new MySqlConnection(SQLConection))
            {
                conectar.Open();

                string insertQuery =
                    "INSERT INTO informes (nombre, apellidos, telefono, estatura, edad, genero) " +
                    "VALUES (@nombre, @apellidos, @telefono, @estatura, @edad, @genero)";

                using (MySqlCommand comando = new MySqlCommand(insertQuery, conectar))
                {
                    comando.Parameters.AddWithValue("@nombre", nombre);
                    comando.Parameters.AddWithValue("@apellidos", apellidos);
                    comando.Parameters.AddWithValue("@telefono", telefono);
                    comando.Parameters.AddWithValue("@estatura", estatura);
                    comando.Parameters.AddWithValue("@edad", edad);
                    comando.Parameters.AddWithValue("@genero", genero);

                    comando.ExecuteNonQuery();
                }
            }
        }

        // ---------------- VALIDACIONES ----------------
        private void ValidarNombre(object sender, EventArgs e)
        {
            TextBox cajaNombre = (TextBox)sender;
            if (cajaNombre.Text.Length > 0 && !EsTextoValido(cajaNombre.Text))
            {
                MessageBox.Show("Ingrese valores correctos para el nombre",
                    "Error Nombre", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cajaNombre.Clear();
            }
        }

        private void ValidarApellido(object sender, EventArgs e)
        {
            TextBox cajaApellidos = (TextBox)sender;
            if (cajaApellidos.Text.Length > 0 && !EsTextoValido(cajaApellidos.Text))
            {
                MessageBox.Show("Ingrese valores correctos para el apellido",
                    "Error Apellido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cajaApellidos.Clear();
            }
        }

        private bool EsTextoValido(string valor)
        {
            return Regex.IsMatch(valor, @"^[a-zA-Z\s]+$");
        }

        private void ValidarEdad(object sender, EventArgs e)
        {
            TextBox textboxEdad = (TextBox)sender;
            if (textboxEdad.Text.Length > 0 && !EsEnteroValido(textboxEdad.Text))
            {
                MessageBox.Show("Ingrese valores correctos para la edad",
                    "Error Edad", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textboxEdad.Clear();
            }
        }

        private bool EsEnteroValido(string valor)
        {
            int resultado;
            return int.TryParse(valor, out resultado);
        }

        private void ValidarEstatura(object sender, EventArgs e)
        {
            TextBox textBoxEstatura = (TextBox)sender;
            if (textBoxEstatura.Text.Length > 0 && !EsDecimalValido(textBoxEstatura.Text))
            {
                MessageBox.Show("Ingrese valores correctos para la estatura",
                    "Error Estatura", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxEstatura.Clear();
            }
        }

        private bool EsDecimalValido(string valor)
        {
            decimal resultado;
            return decimal.TryParse(valor, out resultado);
        }

        private void ValidarTelefono(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            string input = textBox.Text;

            if (input.Length > 10)
            {
                textBox.BackColor = Color.Red;       // demasiados dígitos
            }
            else if (!EsTelefonoValido(input))
            {
                textBox.BackColor = Color.Yellow;    // incompleto o con letras
            }
            else
            {
                textBox.BackColor = Color.SeaGreen;  // válido
            }
        }

        private bool EsTelefonoValido(string valor)
        {
            long resultado;
            return long.TryParse(valor, out resultado) && valor.Length == 10;
        }

        // ---------------- BOTONES ----------------
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

            // Validar que todos los campos tengan el formato correcto
            if (EsTextoValido(nombre) && EsTextoValido(apellidos) &&
                EsEnteroValido(edad) && EsDecimalValido(estatura) &&
                EsTelefonoValido(telefono) && genero != "")
            {
                string mensaje =
                    $"Nombre: {nombre}\r\nApellidos: {apellidos}\r\nTeléfono: {telefono}\r\n" +
                    $"Edad: {edad}\r\nEstatura: {estatura}\r\nGénero: {genero}\r\n";

                try
                {
                    // 1) Guardar en TXT
                    string rutaArchivo = Path.Combine(Application.StartupPath, "archivo.txt");
                    bool archivoExiste = File.Exists(rutaArchivo);

                    using (StreamWriter escritor = new StreamWriter(rutaArchivo, true))
                    {
                        if (archivoExiste)
                        {
                            escritor.WriteLine(); // separador entre registros
                        }
                        escritor.WriteLine(mensaje);
                    }

                    // 2) Guardar en la base de datos
                    InsertarRegistros(nombre, apellidos, int.Parse(edad),
                        decimal.Parse(estatura), telefono, genero);

                    MessageBox.Show("Datos guardados en TXT y en la Base de Datos:\n\n" + mensaje,
                        "Confirmación de registro", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ocurrió un error al guardar:\n" + ex.Message,
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Por favor, ingrese datos válidos en todos los campos.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            tbxName.Clear();
            tbxApellidos.Clear();
            tbxTelefono.Clear();
            tbxTelefono.BackColor = SystemColors.Window; // quita el color de la validación
            tbxEstatura.Clear();
            tbxEdad.Clear();
            rbMujer.Checked = false;
            rbHombre.Checked = false;
            rbOtros.Checked = false;
        }
    }
}