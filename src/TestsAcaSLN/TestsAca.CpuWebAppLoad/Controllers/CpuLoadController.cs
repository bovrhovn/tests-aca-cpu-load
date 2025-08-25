using Microsoft.AspNetCore.Mvc;

namespace TestsAca.CpuWebAppLoad.Controllers;

[ApiController]
[Route("loads")]
public class CpuLoadController(ILogger<CpuLoadController> logger) : ControllerBase
{
    [HttpGet("cpu-with-time-limit")]
    public IActionResult GenerateCpuLoadForSeconds([FromQuery]int seconds=30)
    {
        var threadCount = Environment.ProcessorCount * 2;
        logger.LogInformation("Starting CPU load generation with {ThreadCount} thread count.", threadCount);
        var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(seconds));
        for (var currentThred = 0; currentThred < threadCount; currentThred++) 
        {
            Task.Run(() =>
            {
                while (!cancellationTokenSource.Token.IsCancellationRequested)
                {
                    // Tight loop with floating-point math
                    double result = 0;
                    for (var j = 0; j < 1000000; j++)
                    {
                        result += Math.Sqrt(j) * Math.Sin(j);
                        logger.LogInformation("Intermediate result for time limits: {Result}", result);
                    }
                }
            }, cancellationTokenSource.Token);
        }
        logger.LogInformation("CPU load generation will run for {Seconds} seconds.", seconds);
        return Ok($"CPU load generation started for {seconds} seconds.");
    }

    [HttpGet("cpu-no-time-limit")]
    public IActionResult GenerateCpuLoad()
    {
        var threadCount = Environment.ProcessorCount * 2; 
        logger.LogInformation("Starting CPU load generation with {ThreadCount} thread count.", threadCount);
        
        for (var currentThred = 0; currentThred < threadCount; currentThred++) 
        {
            Task.Run(() =>
            {
                while (true)
                {
                    // Tight loop with floating-point math
                    double result = 0;
                    for (var j = 0; j < 1000000; j++)
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