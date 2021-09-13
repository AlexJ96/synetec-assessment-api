using Microsoft.AspNetCore.Mvc;
using SynetecAssessmentApi.Domain;
using SynetecAssessmentApi.Dtos;
using SynetecAssessmentApi.Services;
using System.Threading.Tasks;

namespace SynetecAssessmentApi.Controllers
{
    [Route("api/[controller]")]
    public class BonusPoolController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var bonusPoolService = new BonusPoolService();

            return Ok(await bonusPoolService.GetEmployeesAsync());
        }

        [HttpPost()]
        public async Task<IActionResult> CalculateBonus([FromBody] CalculateBonusDto request)
        {
            try
            {
                var bonusPoolService = new BonusPoolService();

                return Ok(await bonusPoolService.GetBonusAsync(
                request.TotalBonusPoolAmount,
                request.SelectedEmployeeId));
            } 
            catch (ExpectedException e)
            {
                return BadRequest(e.ToString());
            }

        }
    }
}
