using System;
using System.Collections.Immutable;
using System.Net.WebSockets;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using Asv.Common;
using Asv.IO;
using Asv.Mavlink;
using Asv.Mavlink.Common;
using Asv.Mavlink.Minimal;
using DeepEqual.Syntax;
using R3;

public static class MavlinkExtensions
{
    public static Observable<MavlinkV2Message<TPayload>> FilterByPayload<TPayload>(
        this Observable<MavlinkMessage> source) where TPayload : IPayload
    {
        return source.Where(message => message is MavlinkV2Message<TPayload>)
            .Select(message => (MavlinkV2Message<TPayload>)message);
    }
}

public static class DateTimeExtensions
{
    private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    private static readonly DateTime MavlinkEpoch = new DateTime(2015, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public static ulong ToMavlinkTimestamp(this DateTime dateTime)
    {
        return (ulong)((dateTime - MavlinkEpoch).TotalMicroseconds / 10);
    }

    public static DateTime FromMavlinkTimestamp(this ulong mavlinkTime)
    {
        return MavlinkEpoch.AddMicroseconds(10 * (ulong)mavlinkTime);
    }
}

public class Program
{
    public static async Task Main()
    {
        //TestJSONSerialization();
        //await TestVirtualConnection();
        await TestDirectVirtualConnection();
    }

    static async Task TestDirectVirtualConnection()
    {
        var protocol = Protocol.Create(builder =>
            builder.RegisterMavlinkV2Protocol());

        var link = protocol.CreateVirtualConnection();

        var client = link.Client;
        var server = link.Server;

        //Simulated client and server devices

        var clientCore = new CoreServices(
            client,
            new PacketSequenceCalculator(),
            logFactory: null,
            timeProvider: null,
            new DefaultMeterFactory());

        var clientIdentity = new MavlinkClientIdentity(255, 1, 0, 0);
        var clientDeviceId = new MavlinkClientDeviceId("client", clientIdentity);
        var clientConfig = new GenericDeviceConfig();
        var genericClientDevice = new GenericDevice(
            clientDeviceId,
            clientConfig,
            ImmutableArray<IClientDeviceExtender>.Empty,
            clientCore);

        var serverCore = new CoreServices(
            server,
            new PacketSequenceCalculator(),
            logFactory: null,
            timeProvider: null,
            new DefaultMeterFactory());

        var serverIdentity = new MavlinkClientIdentity(1, 1, 0, 0);
        var serverDeviceId = new MavlinkClientDeviceId("server", serverIdentity);
        var serverConfig = new GenericDeviceConfig(); // params, commands and FTP configurations
        var genericServerDevice = new GenericDevice(
            serverDeviceId,
            serverConfig,
            ImmutableArray<IClientDeviceExtender>.Empty,
            serverCore);
        
        client.OnRxMessage.FilterByType<MavlinkMessage>().Subscribe(packet =>
        {
            Console.WriteLine(protocol.PrintMessage(packet));
            
        });

        client.OnRxMessage.FilterByType<MavlinkMessage>().FilterByPayload<HeartbeatPayload>().Subscribe(packet =>
        {
            Console.WriteLine($"Received : {packet.Name} at {packet.Signature.Timestamp.FromMavlinkTimestamp()}");
        });
        
        var mavTimestamp = DateTime.UtcNow.ToMavlinkTimestamp();
        
        //Server sends a message to the client
        var serverMessage = new HeartbeatPacket
        {
            SystemId = serverIdentity.Self.SystemId,
            ComponentId = serverIdentity.Self.ComponentId,
            CompatFlags = 0x00,
            IncompatFlags = 0x01,
            Signature = { IsPresent = true, Timestamp = mavTimestamp, Sign = 0xAABBCCDDEEFF, LinkId = 0x03 },
            Payload = { 
                Autopilot = MavAutopilot.MavAutopilotInvalid,
                BaseMode = MavModeFlag.MavModeFlagManualInputEnabled,
                CustomMode = 0x00,
                MavlinkVersion = 0x02, 
                Type = MavType.MavTypeGeneric,
                SystemStatus = MavState.MavStateStandby,
            }
        };
        
        //Send it
        var task = server.Send(serverMessage);
        await task;
        await Task.Delay(3000);
        Console.WriteLine($"Ended");


    }


    static void TestJSONSerialization()
    {
        var batteryStatus = new BatteryStatusPacket
        {
            Payload =
            {
                CurrentConsumed = 1200,
                EnergyConsumed = 5000,
                Temperature = 3000,
                Voltages = [4000, 4001, 3999, 4002, 4003, 4004, 4005, 4006, 4007, 4008],
                CurrentBattery = 100,
                Id = 1,
                BatteryFunction = MavBatteryFunction.MavBatteryFunctionPropulsion,
                Type = MavBatteryType.MavBatteryTypeLife,
                BatteryRemaining = 85,
                TimeRemaining = 600,
                ChargeState = MavBatteryChargeState.MavBatteryChargeStateLow,
                Mode = MavBatteryMode.MavBatteryModeUnknown,
                FaultBitmask = 0
            }
        };

        // Configuração da serialização
        var options = new JsonSerializerOptions
        {
            WriteIndented = true, // Para JSON legível
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault // Evita valores padrão nulos ou 0
        };

        var serializedMessage = new
        {
            Name = batteryStatus.Name,
            Id = batteryStatus.Id,
            TimestampMilliseconds = System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            Payload = batteryStatus.Payload
        };

        var json = JsonSerializer.Serialize(serializedMessage, options);
        Console.WriteLine(json);

        //Deserialize the payload using the Payload property of the json object

        using (JsonDocument document = JsonDocument.Parse(json))
        {
            JsonElement json_payload = document.RootElement.GetProperty("Payload");
            var payload = JsonSerializer.Deserialize<BatteryStatusPayload>(json_payload, options);
            Console.WriteLine(payload);
        }
    }

    static async Task TestVirtualConnection()
    {
        var protocol = Protocol.Create(builder =>
        {
            builder.RegisterMavlinkV2Protocol();
        });

        var link = protocol.CreateVirtualConnection();
        

        //Client configuration
        var identity = new MavlinkClientIdentity(1, 1, 0, 0);
        var clientSequence = new PacketSequenceCalculator();

        var clientCore = new CoreServices(
            link.Client,
            clientSequence,
            logFactory: null,
            timeProvider: null,
            new DefaultMeterFactory()
        );

        var deviceId = new MavlinkClientDeviceId("virtual", identity);
        var deviceConfig = new VehicleClientDeviceConfig();
        var arduCopter = new ArduCopterClientDevice(
            deviceId,
            deviceConfig,
            ImmutableArray<IClientDeviceExtender>.Empty,
            clientCore
        );

        //Server configuration

        var serverSequence = new PacketSequenceCalculator();
        var serverCore = new CoreServices(
            link.Server,
            serverSequence,
            logFactory: null,
            timeProvider: null,
            new DefaultMeterFactory()
        );

        var mavlinkServer = new HeartbeatServer(
            identity.Target,
            new MavlinkHeartbeatServerConfig(),
            serverCore
        );

        //Send data from server to client

        var completionSource = new TaskCompletionSource<HeartbeatPayload>();
        var cancellationSource = new CancellationTokenSource(
            TimeSpan.FromSeconds(10), TimeProvider.System);
        cancellationSource.Token.Register(() => completionSource.TrySetCanceled());

        arduCopter.Heartbeat.RawHeartbeat.Subscribe((HeartbeatPayload p)  =>
        {
            Console.WriteLine(p);
        });

        mavlinkServer.Start();

        HeartbeatPayload? payload = null;

        mavlinkServer.Set(p =>
        {
            p.Type = MavType.MavTypeBattery;
            p.BaseMode = MavModeFlag.MavModeFlagCustomModeEnabled;
            Console.WriteLine($"From server: {p.Type}");
            payload = p;
        });

        var result = await completionSource.Task;
        Console.WriteLine($"Result: {result.Type}");
    }

    static async Task TestSITLConnection()
    {
        var protocol = Protocol.Create(builder =>
        {
            builder.RegisterMavlinkV2Protocol();
        });

        var router = protocol.CreateRouter(("router"));
        router.AddTcpClientPort(port =>
        {
            port.Host = "127.0.0.1";
            port.Port = 5762;
        }
        );

        router.OnRxMessage.Subscribe(message =>
        {
            var messageType = message.GetType();

            if (messageType.IsGenericType && messageType.GetGenericTypeDefinition() == typeof(MavlinkV2Message<>))
            {
                var payloadType = messageType.GetGenericArguments()[0];
                Console.WriteLine($"Received message with payload type {payloadType.Name}");
            }
        });

        var identity = new MavlinkClientIdentity(255, 0, 1, 1);
        var clientSequence = new PacketSequenceCalculator();

        var clientCore = new CoreServices(
            router,
            clientSequence,
            logFactory: null,
            timeProvider: null,
            new DefaultMeterFactory()
        );

        var deviceId = new MavlinkClientDeviceId("virtual", identity);
        var deviceConfig = new VehicleClientDeviceConfig();
        var arduCopter = new ArduCopterClientDevice(
            deviceId,
            deviceConfig,
            ImmutableArray<IClientDeviceExtender>.Empty,
            clientCore
        );

        //Server configuration

        var serverSequence = new PacketSequenceCalculator();
        var serverCore = new CoreServices(
            router,
            serverSequence,
            logFactory: null,
            timeProvider: null,
            new DefaultMeterFactory()
        );

        var mavlinkServer = new HeartbeatServer(
            identity.Target,
            new MavlinkHeartbeatServerConfig(),
            serverCore
        );

        //Send data from server to client

        var completionSource = new TaskCompletionSource<HeartbeatPayload>();
        var cancellationSource = new CancellationTokenSource(
            TimeSpan.FromSeconds(60), TimeProvider.System);
        cancellationSource.Token.Register(() => completionSource.TrySetCanceled());

        var called = 0;

        arduCopter.Heartbeat.RawHeartbeat.Subscribe((HeartbeatPayload packet) =>
        {
            if (packet is null) return;

            Console.WriteLine($"Heartbeat type: {packet.Type}\tMode: {packet.BaseMode}");
            if (called != 20)
            {
                called++;
                return;
            }

            completionSource.TrySetResult(packet);
        });



        var result = await completionSource.Task;
        Console.WriteLine($"Result: {result.Type}");
    }
}

