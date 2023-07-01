﻿using NUnit.Framework;
using UPR.Networking;
using UPR.Tests;

namespace UPD.Networking.Tests
{
    public class CommandMarshalingTests
    {
        [Test]
        public void TimelineSerializationAndDeserialization()
        {
            const int originalValue = 999;


            var targetRegistry = new TargetRegistry<TestEntity>();
            var testEntity = new TestEntity(originalValue);
            targetRegistry.Add(testEntity, new TargetId(0));

            // ...

            // var timelineSerializer = new CollectionSerializer<IncreaseValueCommand>();

            // var increaseValueSerializer = new NetCommandSerializer<IncreaseValueCommand>(new IncreaseValueCommandSerializer());
            // var increaseValueDeserializer = new NetCommandDeserializer<IncreaseValueCommand>(new IncreaseValueCommandDeserializer());

            var commandRouter = new CommandRouter<IncreaseValueCommand>(targetRegistry);

            // ...
        }
    }
}
