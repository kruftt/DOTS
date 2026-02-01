using System;
using Unity.Entities;
using UnityEngine;

[Serializable]
public struct InputState : IComponentData
{
    public Vector2 MousePos;
}
