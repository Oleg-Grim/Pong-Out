﻿
namespace ME.ECS.Tests {

    using Serializer;
    using System.Linq;

    public class SerializerTests {

        public struct Person {

            public enum Sex {

                Male,
                Female,

            }

            public int age;
            public string firstName;
            public string lastName;
            public Sex sex;

        }
        
        public struct PerfStruct {

            public Person[] data;

        }

        [NUnit.Framework.TestAttribute]
        public void PerformanceTest() {

            var l = System.Linq.Enumerable.Range(1, 100).Select(x => new Person { age = x, firstName = "Windows", lastName = "Server", sex = Person.Sex.Female }).ToArray();
            var test = new PerfStruct() {
                data = l,
            };
            
            byte[] lastTest = null;
            for (int i = 0; i < 100; ++i) {

                byte[] bytes;
                {
                    var serializersInternal = Serializer.GetInternalSerializers();
                    var serializers = Serializer.GetDefaultSerializers();
                    serializers.Add(ref serializersInternal);
                    serializersInternal.Dispose();

                    bytes = Serializer.Pack(serializers, test);
                    serializers.Dispose();

                }

                {
                    var serializersInternal = Serializer.GetInternalSerializers();
                    var serializers = Serializer.GetDefaultSerializers();
                    serializers.Add(ref serializersInternal);
                    serializersInternal.Dispose();

                    var testRes = Serializer.Unpack<PerfStruct>(serializers, bytes);
                    serializers.Dispose();

                    lastTest = bytes;
                }

            }
            
            UnityEngine.Debug.Log("Bytes length: " + lastTest.Length);

        }

        [NUnit.Framework.TestAttribute]
        public void BufferArraySerialization() {
            var test = new TestDataBufferArray {
                viewInfo = new ME.ECS.Views.ViewInfo(Entity.Empty, 12, 23),
                bufferComponents = new ME.ECS.Collections.BufferArray<object>(new object[] {
                    new ME.ECS.Views.ViewComponent { seed = 123u, viewInfo = new ME.ECS.Views.ViewInfo(Entity.Empty, 12, 23) }, 
                    default,
                    default,
                    default,
                    default,
                    new ME.ECS.Views.ViewComponent { seed = 123u, viewInfo = new ME.ECS.Views.ViewInfo(Entity.Empty, 12, 23) },
                    default,
                    default,
                }, 6),
                buffer = new ME.ECS.Collections.BufferArray<MyStruct>(new[] {
                    new MyStruct { bar = 1, foo = 2 },
                    new MyStruct { bar = 2, foo = 3 },
                    new MyStruct { bar = 4, foo = 5 },
                    new MyStruct { bar = 6, foo = 7 },
                    new MyStruct { bar = 8, foo = 9 }
                }, 5)
            };

            byte[] bytes;
            {
                var ser = new Serializers();
                ser.Add(new BufferArraySerializer());

                bytes = Serializer.Pack(test, ser);
                ser.Dispose();
            }
            
            {
                var ser = new Serializers();
                ser.Add(new BufferArraySerializer());

                var testRes = Serializer.Unpack<TestDataBufferArray>(bytes, ser);
                ser.Dispose();
                for (int i = 0; i < test.buffer.Length; ++i) {

                    NUnit.Framework.Assert.AreEqual(test.buffer.arr[i], testRes.buffer.arr[i]);

                }
            }

        }

        [NUnit.Framework.TestAttribute]
        public void NativeBufferArraySerialization() {
            var test = new TestDataNativeBufferArray {
                viewInfo = new ME.ECS.Views.ViewInfo(Entity.Empty, 12, 23),
                buffer = new ME.ECS.Collections.NativeBufferArray<MyStruct>(new[] {
                    new MyStruct { bar = 1, foo = 2 },
                    new MyStruct { bar = 2, foo = 3 },
                    new MyStruct { bar = 4, foo = 5 },
                    new MyStruct { bar = 6, foo = 7 },
                    new MyStruct { bar = 8, foo = 9 }
                }, 5)
            };

            byte[] bytes;
            {
                var ser = new Serializers();
                ser.Add(new BufferArraySerializer());

                bytes = Serializer.Pack(test, ser);
                ser.Dispose();
            }
            
            {
                var ser = new Serializers();
                ser.Add(new BufferArraySerializer());

                var testRes = Serializer.Unpack<TestDataNativeBufferArray>(bytes, ser);
                ser.Dispose();
                for (int i = 0; i < test.buffer.Length; ++i) {

                    NUnit.Framework.Assert.AreEqual(test.buffer.arr[i], testRes.buffer.arr[i]);

                }
            }

        }

		void DictionarySerializationTest1()
		{
			var test = new TestDataDictionary
			{
				someDict = new System.Collections.Generic.Dictionary<object, object>
				{
					["hello"] = 123,
					[456] = "yo"
				}
			};

			var bytes = Serializer.Pack(test);

			var testRes = Serializer.Unpack<TestDataDictionary>(bytes);
			NUnit.Framework.Assert.AreEqual(test.someDict, testRes.someDict);
		}

		void DictionarySerializationTest2()
		{
			var dic1 = new System.Collections.Generic.Dictionary<ETestEnum, string>();

			dic1.Add(ETestEnum.Second, "Second");
			dic1.Add(ETestEnum.First, "First");

			var test_data = new TestDataDictionary2
			{
				someDict = dic1,
			};

			var bytes = Serializer.Pack(test_data);

			var res_data = Serializer.Unpack<TestDataDictionary2>(bytes);
			NUnit.Framework.Assert.AreEqual(test_data.someDict, res_data.someDict);
		}

		void DictionarySerializationTest3()
		{
			var test_dic = new System.Collections.Generic.Dictionary<ETestEnum, string>();

			test_dic.Add(ETestEnum.Second, "Second");
			test_dic.Add(ETestEnum.First, "First");

			var bytes = Serializer.Pack(test_dic);

			var res_dic = Serializer.Unpack<System.Collections.Generic.Dictionary<ETestEnum, string>>(bytes);
			NUnit.Framework.Assert.AreEqual(res_dic, test_dic);
		}

		[NUnit.Framework.TestAttribute]
        public void DictionarySerialization() {
			DictionarySerializationTest1();
			DictionarySerializationTest2();
			//DictionarySerializationTest3();
		}

        [NUnit.Framework.TestAttribute]
        public void ArraysSerialization() {
            var test = new TestDataArray {
                buffer = new object[] { 1, 3, 5, 7, 9 },
                buffer2 = new object[] {
                    new[] { 1, 3, 5, 7, 9 },
                    new[] { 0, 2, 4, 6 },
                    new[] { 11, 22 }
                },
                buffer3 = new[] {
                    new object[] { 1, 3, 5, 7, 9 },
                    new object[] { "123", "asdsad", 4, 6 },
                    new object[] { 11, 22 }
                },
                buffer4 = new object[,] { { 1, 2 }, { 3, 4 }, { 5, 6 }, { 7, 8 } }
            };


            var bytes   = Serializer.Pack(test);
            var testRes = Serializer.Unpack<TestDataArray>(bytes);

            NUnit.Framework.Assert.AreEqual(test.buffer, testRes.buffer);
            NUnit.Framework.Assert.AreEqual(test.buffer2, testRes.buffer2);
            NUnit.Framework.Assert.AreEqual(test.buffer3, testRes.buffer3);
            NUnit.Framework.Assert.AreEqual(test.buffer4,testRes.buffer4);
        }

        [NUnit.Framework.TestAttribute]
        public void WorldSerialization() {
            
            World CreateWorld() {

                World world = null;
                WorldUtilities.CreateWorld<TestState>(ref world, 0.033f);
                {
                    world.AddModule<TestStatesHistoryModule>();
                    world.AddModule<FakeNetworkModule>();
                
                    world.SetState<TestState>(WorldUtilities.CreateState<TestState>());
                    world.SetSeed(1u);

                    //components
                    {
                        ref var sc = ref world.GetStructComponents();
                        ComponentsInitializerWorld.Setup(e => e.ValidateData<TestStructComponent>());
                        CoreComponentsInitializer.Init(ref sc);
                        sc.Validate<TestStructComponent>();
                    }
                    //settings
                    {
                        world.SetSettings(new WorldSettings {
                            useJobsForSystems = false,
                            useJobsForViews   = false,
                            turnOffViews      = false,
                            viewsSettings     = new WorldViewsSettings()
                        });
                        world.SetDebugSettings(WorldDebugSettings.Default);
                    }
                
                    var group = new SystemGroup(world, "GroupName");
                    group.AddSystem<TestSystem>();
                }
            
                var ent = new Entity("Test Entity");
                ent.SetPosition(UnityEngine.Vector3.zero);
                ent.Set(new TestStructComponent());
            
                world.SaveResetState<TestState>();

                return world;

            }

            var sourceWorld = CreateWorld();
            {
                var dt = 2f;
                sourceWorld.SetFromToTicks(0, 50);
                //sourceWorld.PreUpdate(dt);
                sourceWorld.Update(dt);
                //sourceWorld.LateUpdate(dt);
            }

            var bytes = sourceWorld.Serialize();
            
            var targetWorld = CreateWorld();
            {
                var dt = 2f;
                targetWorld.SetFromToTicks(0, 50);
                //sourceWorld.PreUpdate(dt);
                targetWorld.Update(dt);
                //sourceWorld.LateUpdate(dt);
            }
            targetWorld.Deserialize<TestState>(bytes, new System.Collections.Generic.List<byte[]>());
            
            UnityEngine.Debug.Log("Bytes: " + bytes.Length);
            var ent1 = sourceWorld.GetEntityById(1);
            var ent2 = targetWorld.GetEntityById(1);
            NUnit.Framework.Assert.True(ent1.Has<TestStructComponent>());
            NUnit.Framework.Assert.True(ent2.Has<TestStructComponent>());

            WorldUtilities.ReleaseWorld<TestState>(ref sourceWorld);
            WorldUtilities.ReleaseWorld<TestState>(ref targetWorld);

        }

        public sealed class  TestStatesHistoryModule : ME.ECS.StatesHistory.StatesHistoryModule<TestState> {

            protected override uint GetQueueCapacity() => 10u;

            protected override uint GetTicksPerState() => 20u;

        }
        
        public class TestSystem : ISystemFilter {

            private ViewId viewId;
            
            public World world { get; set; }

            public void OnConstruct() {
                
                var res = UnityEngine.Resources.Load<SerializerTestView>("SerializerTestView");
                if (res == null) UnityEngine.Debug.Log("View is null");
                this.viewId = this.world.RegisterViewSource(res);
                
                UnityEngine.Debug.Log("OKAY");

            }

            public void OnDeconstruct() {
            }

            public bool jobs => false;

            public int jobsBatchCount => 64;

            public Filter filter { get; set; }

            public Filter CreateFilter() => Filter.Create("Filter-TestStructComponent").With<TestStructComponent>().Push();

            public void AdvanceTick(in Entity entity, in float deltaTime) {

                ref var data = ref entity.Get<TestStructComponent>();
                ++data.f;
                
                var pos = entity.GetPosition();
                pos += UnityEngine.Vector3.one;
                entity.SetPosition(pos);
                
                if (entity.Has<ME.ECS.Views.ViewComponent>() == false) entity.InstantiateView(this.viewId);
                
            }

        }
        
        public class TestState : State { }

        public struct TestStructComponent : IComponent {

            public int f;

        }

        public struct TestDataNativeBufferArray {

            public ME.ECS.Views.ViewInfo viewInfo;
            public ME.ECS.Collections.NativeBufferArray<MyStruct> buffer;

        }

        public struct TestDataBufferArray {

            public ME.ECS.Views.ViewInfo viewInfo;
            public ME.ECS.Collections.BufferArray<MyStruct> buffer;
            public ME.ECS.Collections.BufferArray<object> bufferComponents;
        }

        public struct TestDataArray {

            public object[]   buffer;
            public object[]   buffer2;
            public object[][] buffer3;
            public object[,] buffer4;
        }

		public enum ETestEnum: byte { First, Second, Last }
        
        public struct TestDataDictionary {
            public System.Collections.Generic.Dictionary<object, object> someDict;
        }

		public struct TestDataDictionary2
		{
			public System.Collections.Generic.Dictionary<ETestEnum, string> someDict;
		}

		public struct MyStruct : System.IEquatable<MyStruct> {

            public int bar;
            public int foo;

            public override string ToString() => $"{this.bar} {this.foo}";

            public bool Equals(MyStruct other) => this.bar == other.bar && this.foo == other.foo;

            public override bool Equals(object obj) => obj is MyStruct other && Equals(other);

            public override int GetHashCode() {
                unchecked {
                    return (this.bar * 397) ^ this.foo;
                }
            }

            public static bool operator ==(MyStruct left, MyStruct right) => left.Equals(right);

            public static bool operator !=(MyStruct left, MyStruct right) => !left.Equals(right);

        }

        public class FakeNetworkModule : ME.ECS.Network.NetworkModule<TestState> {

            protected override void OnInitialize() {
                var tr       = new FakeTransporter();
                var instance = (ME.ECS.Network.INetworkModuleBase)this;
                instance.SetTransporter(tr);
                instance.SetSerializer(new FakeSerializer());

                // this.photonTransporter = tr;
            }

            public class FakeReceiver {
                
            }
            
            public class FakeTransporter : ME.ECS.Network.ITransporter {
                public System.Collections.Generic.Queue<byte[]> sentData       = new System.Collections.Generic.Queue<byte[]>();
                public System.Collections.Generic.Queue<byte[]> sentSystemData = new System.Collections.Generic.Queue<byte[]>();

                public System.Collections.Generic.Queue<byte[]> receivedData       = new System.Collections.Generic.Queue<byte[]>();
                public System.Collections.Generic.Queue<byte[]> receivedSystemData = new System.Collections.Generic.Queue<byte[]>();

                private int sentCount;
                private int sentBytesCount;
                private int receivedCount;
                private int receivedBytesCount;
                
                private FakeReceiver fakeReceiver;

                public bool IsConnected() => true;

                public void Send(byte[] bytes) {
                    this.sentData.Enqueue(bytes);

                    this.sentBytesCount += bytes.Length;
                    ++this.sentCount;
        }

                public void SendSystem(byte[] bytes) {
                    this.sentSystemData.Enqueue(bytes);

                    this.sentBytesCount += bytes.Length;
                }

                public byte[] Receive() {
                    if (this.receivedData.Count == 0) {

                        if (this.receivedSystemData.Count == 0) return null;

                        var bytes = this.receivedSystemData.Dequeue();

                        this.receivedBytesCount += bytes.Length;

                        return bytes;

                    } else {

                        var bytes = this.receivedData.Dequeue();

                        ++this.receivedCount;
                        this.receivedBytesCount += bytes.Length;

                        return bytes;

                    }
    }

                public int GetEventsSentCount()          => this.sentCount;
                public int GetEventsBytesSentCount()     => this.sentBytesCount;
                public int GetEventsReceivedCount()      => this.receivedCount;
                public int GetEventsBytesReceivedCount() => this.receivedBytesCount;

            }

            public class FakeTransportBridge {

                //todo meta for delays, packets drop, etc
                public System.Collections.Generic.List<FakeTransporter> transporters;

                public void Step() {
                    foreach (var fakeTransporter in this.transporters) {
                        foreach (var otherTransporter in this.transporters.Where(t => t != fakeTransporter)) {
                            foreach (var data in fakeTransporter.sentData) {
                                otherTransporter.receivedData.Enqueue(data);
                            }

                            foreach (var data in fakeTransporter.sentSystemData) {
                                otherTransporter.receivedSystemData.Enqueue(data);
                            }
                        }
                    }
                }

            }

            public class FakeSerializer : ME.ECS.Network.ISerializer {
                
                public byte[] SerializeWorld(World.WorldState data) {
                    var ser = new Serializers();
                    ser.Add(new BufferArraySerializer());
                    return ME.ECS.Serializer.Serializer.Pack(data, ser);
                }

                public World.WorldState DeserializeWorld(byte[] bytes) {
                    var ser = new Serializers();
                    ser.Add(new BufferArraySerializer());
                    return ME.ECS.Serializer.Serializer.Unpack<World.WorldState>(bytes, ser);
                }

                public ME.ECS.StatesHistory.HistoryStorage DeserializeStorage(byte[] bytes) {
                    throw new System.NotImplementedException();
                }

                public byte[] SerializeStorage(ME.ECS.StatesHistory.HistoryStorage historyStorage) {
                    throw new System.NotImplementedException();
                }

                public byte[] Serialize(ME.ECS.StatesHistory.HistoryEvent historyEvent) => ME.ECS.Serializer.Serializer.Pack(historyEvent);
                public ME.ECS.StatesHistory.HistoryEvent Deserialize(byte[] bytes) => ME.ECS.Serializer.Serializer.Unpack<ME.ECS.StatesHistory.HistoryEvent>(bytes);
                
            }
        }
    }
}