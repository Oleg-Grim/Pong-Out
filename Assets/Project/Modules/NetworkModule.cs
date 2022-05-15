using ME.ECS;
using Photon.Pun;
using Project.Markers;
using UnityEngine;

namespace Project.Modules
{
    using TState = ProjectState;
#if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
    public class NetworkModule : ME.ECS.Network.NetworkModule<TState>
    {
        public int Count = 2;
        
        private int orderId;
        private PhotonTransporter photonTransporter;
        public ME.ECS.Network.ISerializer GetSerializer()
        {
            return serializer;
        }

        protected override int GetRPCOrder()
        {
            return PhotonNetwork.LocalPlayer.ActorNumber;
            //this.orderId;
        }

        protected override ME.ECS.Network.NetworkType GetNetworkType()
        {
            return ME.ECS.Network.NetworkType.SendToNet | ME.ECS.Network.NetworkType.RunLocal;
        }

        public void SetOrderId(int orderId)
        {
            this.orderId = orderId;
        }

        public void AddToQueue(byte[] bytes)
        {
            photonTransporter.AddToQueue(bytes);
        }

        public void AddToSystemQueue(byte[] bytes)
        {
            photonTransporter.AddToSystemQueue(bytes);
        }

        public void SetRoomName(string name)
        {
            photonTransporter.SetRoomName(name);
        }

        public void SetRoom(Photon.Realtime.Room room)
        {
            photonTransporter.SetRoom(room);
        }

        protected override void OnInitialize()
        {
            var tr = new PhotonTransporter(world.id);
            var instance = (ME.ECS.Network.INetworkModuleBase) this;
            instance.SetTransporter(tr);
            instance.SetSerializer(new FSSerializer());

            photonTransporter = tr;

            SetRoomName("TestRoom");
        }
    }

    public class PhotonTransporter : ME.ECS.Network.ITransporter
    {
        private System.Collections.Generic.Queue<byte[]> queue = new System.Collections.Generic.Queue<byte[]>();
        private System.Collections.Generic.Queue<byte[]> queueSystem = new System.Collections.Generic.Queue<byte[]>();
        private PhotonView photonView;
        private PhotonReceiver photonReceiver;
        private Photon.Realtime.Room room;

        private int sentCount;
        private int sentBytesCount;
        private int receivedCount;
        private int receivedBytesCount;

        public PhotonTransporter(int id)
        {
            var photon = new GameObject("PhotonTransporter", typeof(PhotonView),
                typeof(PhotonReceiver));
            photonReceiver = photon.GetComponent<PhotonReceiver>();
            var view = photon.GetComponent<PhotonView>();
            view.ViewID = id;
            PhotonNetwork.RegisterPhotonView(view);

            photonView = view;

            photonReceiver.Initialize();
        }

        public void SetRoomName(string name)
        {
            photonReceiver.roomName = name;
        }

        public void SetRoom(Photon.Realtime.Room room)
        {
            this.room = room;
        }

        public bool IsConnected()
        {
            return PhotonNetwork.IsConnectedAndReady == true && room != null;
        }

        public void Send(byte[] bytes)
        {
            // Just send RPC calls to others because of network type (RunLocal flag)
            photonView.RPC("RPC_CALL", RpcTarget.Others, bytes);

            sentBytesCount += bytes.Length;
            ++sentCount;
        }

        public void SendSystem(byte[] bytes)
        {
            photonView.RPC("RPC_SYSTEM_CALL", RpcTarget.Others, bytes);

            sentBytesCount += bytes.Length;
        }

        public void AddToQueue(byte[] bytes)
        {
            queue.Enqueue(bytes);
        }

        public void AddToSystemQueue(byte[] bytes)
        {
            queueSystem.Enqueue(bytes);
        }

        public byte[] Receive()
        {
            if (queue.Count == 0)
            {
                if (queueSystem.Count == 0) return null;

                var bytes = queueSystem.Dequeue();
                receivedBytesCount += bytes.Length;

                return bytes;
            }
            else
            {
                var bytes = queue.Dequeue();

                ++receivedCount;
                receivedBytesCount += bytes.Length;

                return bytes;
            }
        }

        public int GetEventsSentCount()
        {
            return sentCount;
        }

        public int GetEventsBytesSentCount()
        {
            return sentBytesCount;
        }

        public int GetEventsReceivedCount()
        {
            return receivedCount;
        }

        public int GetEventsBytesReceivedCount()
        {
            return receivedBytesCount;
        }
    }

    public class PhotonReceiver : MonoBehaviourPunCallbacks
    {
        public string roomName;

        private bool timeSyncedConnected = false;
        private bool timeSynced = false;

        [PunRPC]
        public void RPC_HISTORY_CALL(byte[] bytes)
        {
            var world = Worlds.currentWorld;
            var storageNetworkModule = world.GetModule<NetworkModule>();
            var networkModule = world.GetModule<ME.ECS.Network.INetworkModuleBase>();
            var storage = storageNetworkModule.GetSerializer().DeserializeStorage(bytes);
            networkModule.LoadHistoryStorage(storage);
        }

        [PunRPC]
        public void RPC_CALL(byte[] bytes)
        {
            var world = Worlds.currentWorld;
            var networkModule = world.GetModule<NetworkModule>();
            networkModule.AddToQueue(bytes);
        }

        [PunRPC]
        public void RPC_SYSTEM_CALL(byte[] bytes)
        {
            var world = Worlds.currentWorld;
            var networkModule = world.GetModule<NetworkModule>();
            networkModule.AddToSystemQueue(bytes);
        }

        public void Initialize()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
            PhotonNetwork.JoinLobby(Photon.Realtime.TypedLobby.Default);
        }

        public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
        {
            base.OnDisconnected(cause);
            Debug.Log("Disconnected because of " + cause);
            var ww = Worlds.currentWorld;
            WorldUtilities.ReleaseWorld<TState>(ref ww);
            timeSyncedConnected = false;
            timeSynced = false;
            Worlds.currentWorld = null;
            var go = FindObjectOfType<InitializerBase>();
            if (go != null)
            {
                go.gameObject.SetActive(false);
                DestroyImmediate(go);
            }

            Debug.Log("World destroyed");
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();

            if (PhotonNetwork.InRoom == true)
            {
                Debug.Log("OnJoinedRoom. IsMaster: " + PhotonNetwork.IsMasterClient);

                var world = Worlds.currentWorld;
                var networkModule = world.GetModule<NetworkModule>();
                networkModule.SetRoom(PhotonNetwork.CurrentRoom);

                if (PhotonNetwork.IsMasterClient == true)
                {
                    // Put server time into the room properties
                    var serverTime = PhotonNetwork.Time;
                    PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable()
                    {
                        {"t", serverTime},
                        {"cc", 1}
                    });
                }

                timeSyncedConnected = false;
                timeSynced = false;
                UpdateTime();

                world.AddMarker(new NetworkSetActivePlayer {PlayerID = PhotonNetwork.LocalPlayer.ActorNumber});
            }
        }

        public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
        {
            base.OnRoomPropertiesUpdate(propertiesThatChanged);

            var world = Worlds.currentWorld;
            var networkModule = world.GetModule<NetworkModule>();
            if ((networkModule as ME.ECS.Network.INetworkModuleBase).GetRPCOrder() == 0)
            {
                var orderId = (int) PhotonNetwork.CurrentRoom.CustomProperties["cc"];
                networkModule.SetOrderId(orderId);
            }
        }

        private void UpdateTime()
        {
            if (PhotonNetwork.InRoom == false) return;

            if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("t") == true)
            {
                // Set current time since start from master client
                var world = Worlds.currentWorld;
                var serverTime = PhotonNetwork.Time;
                var gameStartTime = serverTime - (double) PhotonNetwork.CurrentRoom.CustomProperties["t"];

                world.SetTimeSinceStart(gameStartTime);
                timeSynced = true;
            }
        }

        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);

            if (PhotonNetwork.IsMasterClient == true)
            {
                var world = Worlds.currentWorld;
                var props = PhotonNetwork.CurrentRoom.CustomProperties;
                props["cc"] = (int) props["cc"] + 1;
                PhotonNetwork.CurrentRoom.SetCustomProperties(props);

                // Send all history events to client
                var networkModule = world.GetModule<NetworkModule>();
                var history = world.GetModule<ME.ECS.StatesHistory.IStatesHistoryModuleBase>().GetHistoryStorage();
                photonView.RPC("RPC_HISTORY_CALL", newPlayer,
                    networkModule.GetSerializer().SerializeStorage(history));
            }
        }

        public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
        {
            base.OnPlayerLeftRoom(otherPlayer);

            if (PhotonNetwork.IsMasterClient == true)
            {
                var world = Worlds.currentWorld;
                world.AddMarker(new NetworkPlayerDisconnected {PlayerID = otherPlayer.ActorNumber});
            }
        }

        public override void OnRoomListUpdate(System.Collections.Generic.List<Photon.Realtime.RoomInfo> roomList)
        {
            PhotonNetwork.JoinOrCreateRoom(roomName,
                new Photon.Realtime.RoomOptions() {MaxPlayers = 2, PublishUserId = true},
                Photon.Realtime.TypedLobby.Default);
        }

        public void LateUpdate()
        {
            UpdateTime();

            var world = Worlds.currentWorld;
            if (timeSynced == true && timeSyncedConnected == false)
            {
                var networkModule = world.GetModule<NetworkModule>();
                if (((ME.ECS.Network.INetworkModuleBase) networkModule).GetRPCOrder() > 0)
                {
                    // Here we are check if all required players connected to the game
                    // So we could start the game sending the special message
                    if (PhotonNetwork.CurrentRoom.PlayerCount == networkModule.Count)
                    {
                        timeSyncedConnected = true;
                        world.AddMarker(new NetworkPlayerConnectedTimeSynced());
                    }
                }
            }
        }
    }
    
    public class FSSerializer : ME.ECS.Network.ISerializer
    {
        public byte[] SerializeStorage(ME.ECS.StatesHistory.HistoryStorage historyEvent)
        {
            return ME.ECS.Serializer.Serializer.Pack(historyEvent);
        }

        public ME.ECS.StatesHistory.HistoryStorage DeserializeStorage(byte[] bytes)
        {
            return ME.ECS.Serializer.Serializer.Unpack<ME.ECS.StatesHistory.HistoryStorage>(bytes);
        }

        public byte[] Serialize(ME.ECS.StatesHistory.HistoryEvent historyEvent)
        {
            return ME.ECS.Serializer.Serializer.Pack(historyEvent);
        }

        public ME.ECS.StatesHistory.HistoryEvent Deserialize(byte[] bytes)
        {
            return ME.ECS.Serializer.Serializer.Unpack<ME.ECS.StatesHistory.HistoryEvent>(bytes);
        }

        public byte[] SerializeWorld(World.WorldState data)
        {
            return ME.ECS.Serializer.Serializer.Pack(data);
        }

        public World.WorldState DeserializeWorld(byte[] bytes)
        {
            return ME.ECS.Serializer.Serializer.Unpack<World.WorldState>(bytes);
        }
    }
}