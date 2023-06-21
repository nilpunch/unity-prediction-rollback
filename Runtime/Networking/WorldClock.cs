using System;

namespace UPR.Networking
{
    public struct CommandsPacket
    {
        public long SimulationTime;

        public NetCommand[] Commands;
    }

    public struct NetCommand
    {
        public int Tick;
        public int TargetId;
        public byte[] SerializedData;
    }

    public class WorldClock
    {

    }
}
