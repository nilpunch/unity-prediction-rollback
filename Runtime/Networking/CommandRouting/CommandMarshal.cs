using System.Collections.Generic;
using UPR.Serialization;

namespace UPR.Networking
{
    public class CommandMarshal : ICommandMarshal
    {
        private readonly Dictionary<int, ICommandMarshal> _commandMarshals = new Dictionary<int, ICommandMarshal>();

        public void BindCommand(int typeIndex, ICommandMarshal commandMarshal)
        {
            _commandMarshals.Add(typeIndex, commandMarshal);
        }

        public void DeserializeAndForward(ReadHandle commandData)
        {
            while (!commandData.IsEnded)
            {
                int commandType = commandData.ReadInt();
                _commandMarshals[commandType].DeserializeAndForward(commandData);
            }
        }
    }
}
