/*
using System;
using System.Reactive.Linq;
using Newtonsoft.Json;
using ReactiveUI;

#region Models

// Represents the battery status data from MAVLink (both radio and JSON are the same)
public class BatteryStatusMessage
{
    public int BatteryRemaining { get; set; }
    public int CurrentConsumed { get; set; }
    public int Temperature { get; set; }
    public int[] Voltages { get; set; }

    public override string ToString()
    {
        return $"BatteryRemaining: {BatteryRemaining}%, " +
               $"CurrentConsumed: {CurrentConsumed}, " +
               $"Temperature: {Temperature}°C, " +
               $"Voltages: {string.Join(", ", Voltages)}";
    }
}

#endregion

#region ViewModel

// The ReactiveUI view model that exposes the combined battery status.
public class BatteryStatusViewModel : ReactiveObject
{
    // Backing field for the reactive property.
    private readonly ObservableAsPropertyHelper<BatteryStatusMessage> _latestBatteryStatus;

    // Exposed read-only property.
    public BatteryStatusMessage LatestBatteryStatus => _latestBatteryStatus.Value;

    public BatteryStatusViewModel(
        IObservable<BatteryStatusMessage> radioStream,
        IObservable<BatteryStatusMessage> jsonStream)
    {
        // Combine the latest values from both streams.
        // Since the data is identical, we simply log both values and choose one (here, the radio data).
        var combinedStream = radioStream.CombineLatest(jsonStream, (radio, json) =>
        {
            Console.WriteLine("Radio data: " + radio);
            Console.WriteLine("JSON data:  " + json);
            return radio;
        });

        // Expose the combined stream as a ReactiveUI property (updating on the main thread).
        combinedStream
            .ObserveOn(RxApp.MainThreadScheduler)
            .ToProperty(this, vm => vm.LatestBatteryStatus, out _latestBatteryStatus);
    }
}

#endregion

#region Program

public class Program
{
    public static void Main()
    {
        // Simulate the radio stream: emits a BatteryStatusMessage every second.
        var radioStream = Observable.Interval(TimeSpan.FromSeconds(1))
            .Select(i => new BatteryStatusMessage
            {
                BatteryRemaining = 100 - (int)(i % 100),
                CurrentConsumed = (int)(i * 10),
                Temperature = 25 + (int)(i % 5),
                Voltages = new int[] { 12000, 11950, 11900 }
            });

        // Simulate the JSON stream: emits a JSON string (which is then deserialized) every second.
        // The JSON represents exactly the same data as produced by the radio stream.
        var jsonStream = Observable.Interval(TimeSpan.FromSeconds(1))
            .Select(i =>
            {
                var msg = new BatteryStatusMessage
                {
                    BatteryRemaining = 100 - (int)(i % 100),
                    CurrentConsumed = (int)(i * 10),
                    Temperature = 25 + (int)(i % 5),
                    Voltages = new int[] { 12000, 11950, 11900 }
                };

                // Serialize to JSON and then immediately deserialize it.
                string json = JsonConvert.SerializeObject(msg);
                return JsonConvert.DeserializeObject<BatteryStatusMessage>(json);
            });

        // Create the ReactiveUI view model with both streams.
        var viewModel = new BatteryStatusViewModel(radioStream, jsonStream);

        // Subscribe to the LatestBatteryStatus property and print the combined battery status.
        viewModel.WhenAnyValue(vm => vm.LatestBatteryStatus)
            .Subscribe(status =>
            {
                Console.WriteLine("Combined Battery Status: " + status);
                Console.WriteLine(new string('-', 50));
            });

        // Keep the application running.
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}

#endregion
*/