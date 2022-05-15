using ME.ECS;
using UnityEngine;

namespace Project.Components
{
    public struct CollisionRect : IComponent {public Vector2 Value;}
    public struct WallTag : IComponent {}
    public struct Normal : IComponent {public Vector3 Value;}
    public struct ScoreWall : IComponent {public int Value;}
}