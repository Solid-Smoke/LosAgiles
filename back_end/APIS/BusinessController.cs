using back_end.Application.Commands;
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

        [HttpDelete("{businessId}")]
        public async Task<ActionResult<bool>> DeleteBusiness(int businessId, [FromServices] BusinessDelete _businessDeleteCommand)
        {
            List<int> productsIdsFailedToDelete;
            try
            {
                _businessDeleteCommand.DeleteBusiness(businessId);
                return Ok();
            }
            catch (Exception ex)
            {
                if (ex.Data.Contains("ProductIds"))
                {
                    productsIdsFailedToDelete = (List<int>)ex.Data["ProductIds"];
                    return Conflict(new
                    {
                        message = "No se pudo eliminar el emprendimiento, porque algunos de sus productos están asociados a órdenes activas",
                        productsIdsFailedToDelete
                    });
                }
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error eliminando productos.");
            }
        }
    }
}