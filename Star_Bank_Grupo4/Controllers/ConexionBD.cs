using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Text;

namespace Star_Bank_Grupo4.Controllers
{
        public class ConexionBD
        {
            private const string servidor = "localhost";
            private const string baseDatos = "start_bark";
            private const string usuario = "root";
            private const string password = " ";

            public MySqlConnection conexion;

            public ConexionBD()
            {
                string connectionString = $"Server={servidor};Database={baseDatos};Uid={usuario};Pwd={password};";
                conexion = new MySqlConnection(connectionString);
            }

            public void AbrirConexion()
            {
                try
                {
                    conexion.Open();
                    Console.WriteLine("Conexión establecida correctamente.");
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Error al abrir la conexión: " + ex.Message);
                }
            }

            public void CerrarConexion()
            {
                try
                {
                    conexion.Close();
                    Console.WriteLine("Conexión cerrada correctamente.");
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Error al cerrar la conexión: " + ex.Message);
                }
            }

            // Aquí puedes agregar métodos adicionales para realizar operaciones en la base de datos, como consultas, inserciones, actualizaciones, etc.
        }
}

