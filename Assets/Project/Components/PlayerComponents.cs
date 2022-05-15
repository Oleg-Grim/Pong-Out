using ME.ECS;

namespace Project.Components
{
    public struct PlayerTag : IComponent {public int Value;}
    public struct IsNeutral : IComponent {}
    public struct PlayerScore : IComponent {public int Value;}
    public struct PlayerAvatar : IComponent {public Entity Value;}
}