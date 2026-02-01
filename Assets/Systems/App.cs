using Unity.Entities;
using Unity.Scenes;

public struct AppTag : IComponentData { }
public struct MainMenuToggle : IComponentData { }
public struct GameplayToggle : IComponentData { }

public struct AppCommand : IComponentData
{
    public AppCommandType Type;
}

public enum AppCommandType
{
    LoadGameplay,
    LoadMainMenu
}

[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial class MainMenuSystemGroup : ComponentSystemGroup
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<MainMenuToggle>();
    }
}

[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial class GameplaySystemGroup : ComponentSystemGroup
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GameplayToggle>();
    }
}

[UpdateInGroup(typeof(SceneSystemGroup))]
public partial struct AppSystem : ISystem
{
    private EntityQuery gameplayQuery;
    private EntityQuery mainMenuQuery;

    public void OnCreate(ref SystemState state)
    {
        gameplayQuery = state.GetEntityQuery(typeof(GameplayToggle));
        mainMenuQuery = state.GetEntityQuery(typeof(MainMenuToggle));

        var entity = state.EntityManager.CreateEntity(
            typeof(AppTag),
            //typeof(MainMenuToggle)
            typeof(GameplayToggle),
            typeof(InputState)
        );

        state.EntityManager.SetComponentData(
            state.EntityManager.CreateEntity(typeof(AppCommand)),
            new AppCommand { Type = AppCommandType.LoadGameplay }
            //new AppCommand { Type = AppCommandType.LoadMainMenu }
        );

        state.RequireForUpdate<AppCommand>();
    }

    public void OnUpdate(ref SystemState state)
    {
        var appEntity = SystemAPI.GetSingletonEntity<AppTag>();
        var ecb = SystemAPI
            .GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
            .CreateCommandBuffer(state.WorldUnmanaged);

        foreach (var (cmd, entity) in SystemAPI.Query<RefRO<AppCommand>>().WithEntityAccess())
        {
            switch (cmd.ValueRO.Type)
            {
                case AppCommandType.LoadMainMenu:
                    ecb.RemoveComponent<GameplayToggle>(appEntity);
                    ecb.AddComponent<MainMenuToggle>(appEntity);
                    break;
                case AppCommandType.LoadGameplay:
                    ecb.RemoveComponent<MainMenuToggle>(appEntity);
                    ecb.AddComponent<GameplayToggle>(appEntity);
                    StartNewGame(ref state);
                    break;
            }
            ecb.DestroyEntity(entity);
        }
    }

    private void StartNewGame(ref SystemState state)
    {

    }
}

