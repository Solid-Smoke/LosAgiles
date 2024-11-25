using back_end.Application.Commands;
using back_end.Application.Queries;
using back_end.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace back_end.APIS
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : Controller
    {
        [HttpGet("CompletedOrders/{id}")]
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
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet("CompletedOrders/{id}/pdf")]
        public ActionResult CompletedOrdersReportPDF(
            int id, string startDate, string endDate,
            [FromServices] GenerateCompletedOrderReportPDF generateCompletedOrderReportPDF)
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
                return File(generateCompletedOrderReportPDF.Execute(baseFilters), "application/pdf", "CompletedOrdersReport(" + DateTime.Now.ToLongDateString() + ").pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("CancelledOrders")]
        public ActionResult GenerateAllCancelledOrdersReport(
            string startDate, string endDate,
            [FromServices] GenerateAllCancelledOrdersReport generateAllCancelledOrdersReport) {
            try {

                if (!DateTime.TryParse(startDate, out var start) || !DateTime.TryParse(endDate, out var end)) {
                    return BadRequest("Invalid date format.");
                }

                var baseFilters = new ReportBaseFilters {
                    ClientID = 0,
                    StartDate = start,
                    EndDate = end
                };
                return Ok(generateAllCancelledOrdersReport.Execute(baseFilters));
            }
            catch (SqlException sqlEx) {
                return StatusCode(StatusCodes.Status500InternalServerError, sqlEx.Message);
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet("CancelledOrders/pdf")]
        public ActionResult AllCancelledOrdersReportPDF(
            string startDate, string endDate,
            [FromServices] GenerateAllCancelledOrderReportPDF generateAllCancelledOrderReportPDF) {
            try {

                if (!DateTime.TryParse(startDate, out var start) || !DateTime.TryParse(endDate, out var end)) {
                    return BadRequest("Invalid date format.");
                }

                var baseFilters = new ReportBaseFilters {
                    ClientID = 0,
                    StartDate = start,
                    EndDate = end
                };
                return File(generateAllCancelledOrderReportPDF.Execute(baseFilters), "application/pdf", "AllCancelledOrdersReport(" + DateTime.Now.ToLongDateString() + ").pdf");
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
