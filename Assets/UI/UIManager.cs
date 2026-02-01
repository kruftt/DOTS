using Unity.Entities;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public UIDocument rootUIDocument;
    private EntityQuery appCommandQuery;

    private void Start()
    {
        var em = World.DefaultGameObjectInjectionWorld.EntityManager;
        appCommandQuery = em.CreateEntityQuery(
                ComponentType.ReadOnly<AppCommand>()
            );
    }

    private void Update()
    {

        if (!appCommandQuery.IsEmpty)
        {
            var cmd = appCommandQuery.GetSingleton<AppCommand>();
            rootUIDocument.rootVisualElement.Clear();

            switch (cmd.Type)
            {
                case AppCommandType.LoadMainMenu:
                    var MainMenu = new MainMenu();
                    rootUIDocument.rootVisualElement.Add(MainMenu.RootElement);
                    MainMenu.RootElement.StretchToParentSize();
                    break;

                case AppCommandType.LoadGameplay:
                    Debug.Log("TODO: Load Game UI");
                    break;
            }
        }
    }
}

public abstract class UIGroup
{
    public VisualElement RootElement;
}

public abstract class UIScreen
{
    public VisualElement RootElement;
    public UIScreen(VisualElement root)
    {
        RootElement = root;
        root.RemoveFromHierarchy();
    }
}