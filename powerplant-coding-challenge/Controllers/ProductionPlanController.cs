using Microsoft.AspNetCore.Mvc;
using powerplant_coding_challenge.Models;

namespace powerplant_coding_challenge.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductionPlanController : ControllerBase
    {
        
        private readonly ILogger<ProductionPlanController> _logger;

        public ProductionPlanController(ILogger<ProductionPlanController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "ComputePowerPlan")]
        public IEnumerable<PowerplantUsage> Post(PayloadModel payload)
        {
            _logger.LogInformation("ComputePowerPlan POST called at {DT}",
                DateTime.UtcNow.ToLongTimeString());

            var powerplantsUsage = new List<PowerplantUsage>();

            double load = payload.Load;

            // 1.We start with windTurbines, free clean energy first.
            var windTurbines = payload.Powerplants.Where(powerplant => powerplant.Type == "windturbine");

            foreach (var turbine in windTurbines)
            {
                // Computing is not necessary if there's no wind or if the load is reached
                if(payload.Fuels.Wind == 0 || load == 0)
                {
                    powerplantsUsage.Add(new PowerplantUsage { Name = turbine.Name});
                    continue;
                }

                double availablePower = payload.Fuels.Wind * turbine.Pmax / 100.0f;
                double usedPower = (availablePower > load) ? load : availablePower;

                powerplantsUsage.Add(new PowerplantUsage { Name = turbine.Name, P = Math.Round(usedPower, 1) }); // Arbritrary choice of precision
                load -= usedPower;
            }

            // 2. Than we order/use each turbine based on cost/efficency ratio
            var orderdPowerplants = payload.Powerplants
                .Where((powerplant) => powerplant.Type != "windturbine")
                .OrderBy(powerplant => (powerplant.Type == "gas" ? payload.Fuels.Gas : payload.Fuels.Kerosine) / powerplant.Efficiency)
                .ToList();          
            
            foreach (var powerplant in orderdPowerplants)
            {
                // Each powerplant has a minimum power that need to be used
                if(powerplant.Pmin > load || load == 0)
                {
                    powerplantsUsage.Add(new PowerplantUsage { Name = powerplant.Name });
                    continue;
                }

                double usedPower = powerplant.Pmax > load ? load : powerplant.Pmax;

                powerplantsUsage.Add(new PowerplantUsage { Name = powerplant.Name, P = Math.Round(usedPower, 1) });

                load -= usedPower;
            }

            if(load > 0)
                _logger.LogError($"ComputePowerPlan Error : load has not been reached. Remaining load: {load}"); // We could throw if needed

            return powerplantsUsage.OrderByDescending(usage => usage.P).ToList();
        }
    }
}