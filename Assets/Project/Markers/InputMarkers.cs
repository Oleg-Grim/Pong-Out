using ME.ECS;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Markers
{

    public struct PlayerMovementMarker : IMarker
    {
        public float Input;
    }
    public struct LeftKeyMarker : IMarker
    {
        public int PlayerID;
        public KeyState State;
    }

    public struct RightKeyMarker : IMarker
    {
        public int PlayerID;
        public KeyState State;
    }

    public enum KeyState
    {
        Pressed, Released
    }
}