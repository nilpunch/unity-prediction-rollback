using NUnit.Framework;
using UPR.Networking;
using UPR.Tests;

namespace UPD.Networking.Tests
{
    public class CommandMarshalTests
    {
        [Test]
        public void DeserializeAndForward_ShouldDoItsMagic()
        {
            const int originalValue = 999;


            var targetRegistry = new TargetRegistry<TestEntity>();
            var testEntity = new TestEntity(originalValue);

            targetRegistry.Add(testEntity, new TargetId(0));

            // ...

            var commandMarshal = new CommandMarshal();
            commandMarshal.BindCommand(0,
                new SingleCommandMarshal<IncreaseValueCommand>(
                    new CommandRouter<IncreaseValueCommand>(targetRegistry),
                    new IncreaseValueCommandDeserializer()));

            // ...
        }
    }
}
