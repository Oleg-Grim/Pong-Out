using ME.ECS;

namespace Project.Markers
{
    public struct NetworkSetActivePlayer : IMarker
    {
        public int PlayerID;
    }

    public struct NetworkPlayerDisconnected : IMarker
    {
        public int PlayerID;
    }

    public struct NetworkPlayerConnectedTimeSynced : IMarker
    {
        
    }
}