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
        private readonly GetMonthlyShippingExpensesQuery _getMonthlyShippingExpenseQuery;
        private readonly GetOrdersInProgressQuery _getOrdersInProgressQuery;

        public AdminController(GetMonthlyRevenueQuery getMonthlyRevenueQuery, GetMonthlyShippingExpensesQuery getMonthlyShippingExpenseQuery, GetOrdersInProgressQuery getOrdersInProgressQuery)
        {
            _getMonthlyRevenueQuery = getMonthlyRevenueQuery;
            _getMonthlyShippingExpenseQuery = getMonthlyShippingExpenseQuery;
            _getOrdersInProgressQuery = getOrdersInProgressQuery;
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

        [HttpGet("Shipping/Expenses")]
        public ActionResult<List<MonthlyDataModel>> GetMonthlyShippingExpenses()
        {
            try
            {
                return Ok(_getMonthlyShippingExpenseQuery.Execute());
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("Orders/In/Progress")]
        public ActionResult<List<OrderModel>> GetOrdersInProgress()
        {
            try
            {
                return Ok(_getOrdersInProgressQuery.Execute());
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
