using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv.IO;
using Asv.Mavlink;
using R3;

namespace Core.Services
{
    public class MavlinkCommand
    {
        private string _connectionString = "tcp://127.0.0.1:5762";
        private readonly CancellationTokenSource _cancel = new CancellationTokenSource();
        private readonly ReaderWriterLockSlim _rw = new ReaderWriterLockSlim();
        private readonly List<DisplayRow> _items = new List<DisplayRow>();
        private DateTime _lastUpdate = DateTime.Now;
        private readonly List<MavlinkMessage> _lastPackets = new List<MavlinkMessage>();
        private int MaxHistorySize = 20;
        private int _packetCount;
        private int _lastPacketCount;
        private IProtocolRouter conn;


        public void RunMavlink(string? connection = null)
        {
            _connectionString = connection ?? _connectionString;
            conn = Protocol.Create(builder =>
            {
                builder.RegisterMavlinkV2Protocol();
            }).CreateRouter("ROUTER");

            conn.OnRxMessage.FilterByType<MavlinkMessage>().Subscribe(OnPacket);
            conn.AddPort(_connectionString);
            //conn.OnRxMessage.AsSystemObservable().Where(m => m is MavlinkMessage).Subscribe();

            //while (!_cancel.IsCancellationRequested)
            //{
            //    Task.Delay(3000, _cancel.Token).Wait();
            //}
        }



        private void OnPacket(MavlinkMessage packet)
        {
            System.Diagnostics.Debug.WriteLine($"Received {packet.Name}");
            Interlocked.Increment(ref _packetCount);
            try
            {
                _rw.EnterWriteLock();
                _lastPackets.Add(packet);
                if (_lastPackets.Count >= MaxHistorySize) _lastPackets.RemoveAt(0);
                var exist = _items.FirstOrDefault(r => packet.Id == r.Msg);
                if (exist == null)
                {
                    _items.Add(new DisplayRow { Msg = packet.Id, Message = packet.Name });
                }
                else
                {
                    exist.Count++;
                }
            }
            finally
            {
                _rw.ExitWriteLock();
            }
        }

        internal class DisplayRow
        {
            public string? Message { get; set; }
            public int Count { get; set; }
            public string? Freq { get; set; }
            public int Msg { get; set; }
        }
    }
}