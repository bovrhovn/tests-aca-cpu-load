using Microsoft.AspNetCore.Mvc;

namespace TestsAca.CpuWebAppLoad.Controllers;

[ApiController]
[Route("loads")]
public class CpuLoadController(ILogger<CpuLoadController> logger) : ControllerBase
{
    [HttpGet("cpu")]
    public IActionResult GenerateCpuLoad()
    {
        var threadCount = Environment.ProcessorCount * 2; 
        logger.LogInformation("Starting CPU load generation for {Seconds} seconds.", threadCount);
        
        for (var currentThred = 0; currentThred < threadCount; currentThred++)
        {
            Task.Run(() =>
            {
                while (true)
                {
                    // Tight loop with floating-point math
                    double result = 0;
                    for (int j = 0; j < 1000000; j++)
                    {
                        result += Math.Sqrt(j) * Math.Sin(j);
                        logger.LogInformation("Intermediate result: {Result}", result);
                    }
                }
            });
        }

        // Keep the main thread alive
        Thread.Sleep(Timeout.Infinite);

        logger.LogInformation("Completed CPU load generation.");
        return Ok();
    }
}