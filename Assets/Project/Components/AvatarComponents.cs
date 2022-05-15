using ME.ECS;

namespace Project.Components
{
    public struct AvatarTag : IComponent {}
    public struct MoveInput : IComponent {public float Value;}
    public struct MoveSpeed : IComponent {public float Value;}
    public struct PadWidth : IComponent {public float Value;}
    public struct BallAvatar : IComponent {public Entity Value;}
}