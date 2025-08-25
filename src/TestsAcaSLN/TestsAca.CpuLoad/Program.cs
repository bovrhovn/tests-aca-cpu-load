var threadCount = Environment.ProcessorCount; // Use all available cores
Console.WriteLine($"Starting {threadCount} threads to simulate CPU load...");

for (var i = 0; i < threadCount; i++)
{
    var thread = new Thread(() =>
    {
        while (true)
        {
            // Busy loop to consume CPU
            var x = Math.Sqrt(new Random().NextDouble());
        }
    })
    {
        IsBackground = true
    };

    thread.Start();
}

// Keep the main thread alive
while (true)
{
    Thread.Sleep(10000);
}