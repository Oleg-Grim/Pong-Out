using ME.ECS;
using UnityEngine;

namespace Project.Components
{
    public struct BallTag : IComponent {}
    public struct BallDirection : IComponent {public Vector3 Value;}
    public struct BallSpawnTime : IComponent {public float Value;}
    public struct BallLaunchTime : IComponent {public float Value;}
    public struct BallRadius : IComponent {public float Value;}
    public struct Despawn : IComponent {}
    public struct CurrentPlayer : IComponent {public int Value;}
    public struct BallLaunched : IComponent {}
}