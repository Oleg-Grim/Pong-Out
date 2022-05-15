using ME.ECS;

namespace Project.Components
{
    public struct StartGame : IComponent {}
    public struct RunGame : IComponent {}
    public struct GamePaused : IComponent {}
    public struct GameStartTime : IComponent {public float Value;}
    public struct TimerDefault : IComponent {public float Value;}
    public struct HalfTime : IComponent {}
    public struct Owner : IComponent {public Entity Value;}
}