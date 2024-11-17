using back_end.Application.Commands;
using back_end.Application.Queries;
using back_end.Domain;
using back_end.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace back_end.APIS
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly ClientsHandler clientsHandler;

        public ClientsController()
        {
            clientsHandler = new ClientsHandler();
        }

        [HttpGet]
        public List<ClientModel> Get()
        {
            var clients = clientsHandler.ObtenerClientes();
            return clients;
        }

        [HttpPost]
        public async Task<ActionResult<bool>> RegisterUser(ClientModel client)
        {
            try
            {
                if (client == null)
                {
                    return BadRequest();
                }

                ClientsHandler clientsHandler = new ClientsHandler();
                var result = clientsHandler.RegisterUser(client);
                return new JsonResult(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creating user");
            }
        }

        [HttpGet("~/api/Login")]
        public List<ClientModel> Login(string UserName, string UserPassword)
        {
            var userLogin = clientsHandler.Authenticate(UserName, UserPassword);
            return userLogin;
        }

        [HttpGet("{id}/ReportCompletedOrders")]
        public ActionResult GenerateCompletedOrdersReport(
            int id, string startDate, string endDate,
            [FromServices] GenerateCompletedOrdersReport generateCompletedOrdersReport)
        {
            try
            {


                List<ReportCompletedOrderData> reportData;
                string orderIDs;

                if (!DateTime.TryParse(startDate, out var start) || !DateTime.TryParse(endDate, out var end))
                {
                    return BadRequest("Invalid date format.");
                }

                var baseFilters = new ReportBaseFilters
                {
                    ClientID = id,
                    StartDate = start,
                    EndDate = end
                };
                var success = generateCompletedOrdersReport.Execute(baseFilters, out reportData, out orderIDs);

                if (success)
                {
                    return Ok(new { ReportData = reportData, OrderIDs = orderIDs });
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        "Failed to generate the report.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
