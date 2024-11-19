namespace back_end.APIS
{
    using back_end.Application.Queries;
    using back_end.Domain;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly GetMonthlyRevenueQuery _getMonthlyRevenueQuery;

        public AdminController(GetMonthlyRevenueQuery getMonthlyRevenueQuery)
        {
            _getMonthlyRevenueQuery = getMonthlyRevenueQuery;
        }

        [HttpGet("Monthly/Revenue")]
        public ActionResult<List<MonthlyDataModel>> GetMonthlyRevenue()
        {
            try
            {
                return Ok(_getMonthlyRevenueQuery.Execute());
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
