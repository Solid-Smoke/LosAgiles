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
                var pdfBytes = generateCompletedOrderReportPDF.Execute(baseFilters);
                if (pdfBytes.Length == 0)
                {
                    return NotFound("No data available for the requested report.");
                }

                return File(pdfBytes, "application/pdf", "CompletedOrdersReport(" + DateTime.Now.ToLongDateString() + ").pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet("Earnings")]
        public ActionResult GenerateEarningsReport(
            int year, int? businessId,
            [FromServices] GenerateAdminEarningsReport generateEarningsReport)
        {
            try
            {
                if (year < 0)
                {
                    return BadRequest("Year must be a non-negative value.");
                }

                var filters = new ReportEarningsFilters
                {
                    Year = year,
                    BusinessID = businessId
                };

                return Ok(generateEarningsReport.Execute(filters));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet("Earnings/pdf")]
        public ActionResult GenerateEarningsReportPDF(
            int year, int? businessId,
           [FromServices] GenerateEarningsReportPDF generateEarningsReportPDF)
        {
            try
            {
                if (year < 0)
                {
                    return BadRequest("Year must be a non-negative value.");
                }

                var filters = new ReportEarningsFilters
                {
                    Year = year,
                    BusinessID = businessId
                };

                var pdfBytes = generateEarningsReportPDF.Execute(filters);

                if (pdfBytes.Length == 0)
                {
                    return NotFound("No data available for the requested report.");
                }

                return File(pdfBytes, "application/pdf", $"EarningsReport({year}).pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
