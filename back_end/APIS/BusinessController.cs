using back_end.Application.Commands;
using back_end.Application.interfaces;
using back_end.Application.Queries;
using back_end.Domain;
using Microsoft.AspNetCore.Mvc;

namespace back_end.APIS
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessController : ControllerBase
    {
        public BusinessController() { }

        [HttpGet]
        public ActionResult<List<BusinessModel>> GetAllBusinesses(
            [FromServices] GetAllBusiness getAllBusinessQuery)
        {
            var businesses = getAllBusinessQuery.Execute();
            return Ok(businesses);
        }

        [HttpGet("Employee/{employeeID}")]
        public ActionResult<List<BusinessModel>> GetBusinessesByEmployeeID(
            string employeeID,
            [FromServices] GetBusinessByEmployeeID getBusinessByEmployeeIDQuery)
        {
            var businesses = getBusinessByEmployeeIDQuery.Execute(employeeID);
            return Ok(businesses);
        }

        [HttpGet("{businessID}/Addresses")]
        public ActionResult<List<BusinessAddressModel>> GetBusinessAddresses(
            int businessID,
            [FromServices] GetBusinessAddressByBusinessID getBusinessAddressByBusinessIDQuery)
        {
            var addresses = getBusinessAddressByBusinessIDQuery.Execute(businessID.ToString());
            return Ok(addresses);
        }

        [HttpPost]
        public ActionResult<bool> CreateBusiness(
            [FromBody] CompleteBusinessModel newBusiness,
            [FromServices] InsertNewBusiness insertNewBusinessCommand)
        {
            if (newBusiness == null)
            {
                return BadRequest("Invalid business data.");
            }

            try
            {
                var result = insertNewBusinessCommand.Execute(newBusiness);
                if (result)
                {
                    return Ok();
                }
                return BadRequest("Failed to create new business.");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating new business.");
            }
        }

        [HttpGet("{businessID}")]
        public ActionResult<BusinessModel> GetBusinessByID(int businessID, [FromServices] IBusinessHandler businessHandler)
        {
            try
            {
                var business = businessHandler.getBusinessByID(businessID);
                if (business == null)
                {
                    return NotFound($"No se encontró un negocio con el ID {businessID}");
                }
                return Ok(business);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener el negocio: {ex.Message}");
            }
        }
    }
}