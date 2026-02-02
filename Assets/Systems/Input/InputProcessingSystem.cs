using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial class InputProcessingSystem : SystemBase
{
    protected override void OnCreate()
    {
        RequireForUpdate(GetEntityQuery(typeof(InputState)));
    }

    protected override void OnUpdate()
    {
        var mousePos = Mouse.current.position.ReadValue();

        SystemAPI.SetSingleton(new InputState
        {
            MousePos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10.0f)),
        });
    }
}
