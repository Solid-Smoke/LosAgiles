using back_end.Domain;
using back_end.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace back_end.APIS
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessController : ControllerBase
    {
        private readonly BusinessHandler _businessHandler;

        public BusinessController()
        {
            _businessHandler = new BusinessHandler();
        }

        [HttpGet("~/api/AllBusinessData")]
        public List<BusinessModel> getAllBusinessData()
        {
            var businesses = _businessHandler.getAllBusiness();
            return businesses;
        }

        [HttpGet("~/api/BusinessDataByEmployeeID")]
        public List<BusinessModel> getBusinessDataByEmployeeID(string employeeID)
        {
            var businesses = _businessHandler.getBusinessByEmployeeID(employeeID);
            return businesses;
        }
        [HttpGet("~/api/BusinesessAddresessByBusinessID")]
        public List<BusinessAddressModel> getBusinesessAddresessByBusinessID(int businessID)
        {
            var businesses = _businessHandler.getBusinessAddressByBusinessID(Convert.ToString(businessID));
            return businesses;
        }

        [HttpPost("~/api/NewBusiness")]
        public async Task<ActionResult<bool>> createNewBusiness(CompleteBusinessModel newBusiness)
        {
            try
            {
                if (newBusiness == null)
                {
                    return BadRequest();
                }
                var result = _businessHandler.insertNewBusiness(newBusiness);
                return new JsonResult(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error on createNewBusiness");
            }
        }
    }
}
