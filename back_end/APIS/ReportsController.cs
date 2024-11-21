using back_end.Application.Commands;
using back_end.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace back_end.APIS
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : Controller
    {
        [HttpGet("{id}/CompletedOrders")]
        public ActionResult GenerateCompletedOrdersReport(
            int id, string startDate, string endDate,
            [FromServices] GenerateCompletedOrdersReport generateCompletedOrdersReport)
        {
            try
            {

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
                return Ok(generateCompletedOrdersReport.Execute(baseFilters));
            }
            catch (SqlException sqlEx)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, sqlEx.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
