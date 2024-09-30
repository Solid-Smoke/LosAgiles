using back_end.Models;
using Microsoft.AspNetCore.Components.Web;
using System.Data;
using System.Data.SqlClient;

namespace back_end.Handlers
{
    public class ClientsHandler
    {
        private SqlConnection _conexion;
        private string _rutaConexion;

        public ClientsHandler()
        {
            var builder = WebApplication.CreateBuilder();
            _rutaConexion = builder.Configuration.GetConnectionString("ClientsContext");
            _conexion = new SqlConnection(_rutaConexion);
        }

        private DataTable CrearTablaConsulta(string consulta)
        {
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, _conexion);
            SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsulta);
            DataTable consultaFormatoTabla = new DataTable();
            _conexion.Open();
            adaptadorParaTabla.Fill(consultaFormatoTabla);
            _conexion.Close();
            return consultaFormatoTabla;
        }

        public List<ClientModel> ObtenerClientes()
        {
            List<ClientModel> clients = new List<ClientModel>();
            string consulta = "SELECT * FROM dbo.Clients";
            DataTable tablaResultado = CrearTablaConsulta(consulta);

            foreach (DataRow columna in tablaResultado.Rows)
            {
                clients.Add(new ClientModel
                {
                    UserID = Convert.ToInt32(columna["UserID"]),
                    Name = Convert.ToString(columna["Name"]),
                    LastNames = Convert.ToString(columna["LastNames"]),
                    UserName = Convert.ToString(columna["UserName"]),
                    Email = Convert.ToString(columna["Email"]),
                    BirthDate = Convert.ToDateTime(columna["BirthDate"]),
                    UserPassword = Convert.ToString(columna["UserPassword"]),
                    AccountState = Convert.ToString(columna["AccountState"]),
                    Rol = Convert.ToString(columna["Rol"])
                });
            }
            return clients;
        }
    }
}
