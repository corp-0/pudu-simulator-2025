namespace PuduSimulator.Simulation;

public class Simulation(int tickRateSeconds, CancellationTokenSource cts)
{
    private readonly TimeSpan _tickRate = TimeSpan.FromSeconds(tickRateSeconds);
    private Task? _loopTask;

    public void Start()
    {
        _loopTask = Task.Run(() => RunSimulationLoop(cts.Token));
    }

    public void Stop()
    {
        Console.WriteLine("Requested Simulation Stop");
        cts.Cancel();
        _loopTask?.Wait();
        Console.WriteLine("Simulation stopped gracefully");
    }

    private async Task RunSimulationLoop(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            try
            {
                Update();
                await Task.Delay(_tickRate, token);
            }
            catch (TaskCanceledException)
            {
                break;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }

    private void Update()
    {
        Console.WriteLine("PuduSimulator Simulation tick!");
    }
}