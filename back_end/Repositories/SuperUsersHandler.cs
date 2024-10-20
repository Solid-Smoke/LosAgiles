using back_end.Domain;
using System.Data;
using System.Data.SqlClient;

namespace back_end.Repositories {
    public class SuperUsersHandler {
        private SqlConnection _conexion;
        private string _rutaConexion;

        public SuperUsersHandler() {
            var builder = WebApplication.CreateBuilder();
            _rutaConexion = builder.Configuration.GetConnectionString("ClientsContext");
            _conexion = new SqlConnection(_rutaConexion);
        }

        private DataTable CrearTablaConsulta(string consulta) {
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, _conexion);
            SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsulta);
            DataTable consultaFormatoTabla = new DataTable();
            _conexion.Open();
            adaptadorParaTabla.Fill(consultaFormatoTabla);
            _conexion.Close();
            return consultaFormatoTabla;
        }

        public List<SuperUserModel> Authenticate(string UserName, string UserPassword) {
            List<SuperUserModel> superUsers = new List<SuperUserModel>();
            string consulta = @"SELECT * FROM dbo.SuperUsers WHERE UserName = '" + UserName + "' AND UserPassword = '" + UserPassword + "'";
            DataTable tablaResultado = CrearTablaConsulta(consulta);

            foreach (DataRow columna in tablaResultado.Rows) {
                superUsers.Add(new SuperUserModel {
                    SuperUserID = Convert.ToInt32(columna["SuperUserID"]),
                    UserName = Convert.ToString(columna["UserName"]),
                    UserPassword = Convert.ToString(columna["UserPassword"]),
                    CreatedByID = Convert.ToInt32(columna["CreatedByID"])
                });
            }
            return superUsers;
        }
    }
}
