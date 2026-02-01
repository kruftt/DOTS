using Unity.Entities;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenu : UIGroup
{
    public MainScreen MainScreen;
    public SettingsScreen SettingsScreen;
    public HighScoresScreen HighScoresScreen;

    private UIScreen currentScreen;

    public MainMenu()
    {
        RootElement = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI/MainMenu/MainMenu.uxml").Instantiate();

        MainScreen = new MainScreen(RootElement.Q<TemplateContainer>("MainScreen"));
        MainScreen.NewGameButton.RegisterCallback<ClickEvent>(OnNewGame);
        MainScreen.HighScoresButton.RegisterCallback<ClickEvent>(OnHighScores);
        MainScreen.SettingsButton.RegisterCallback<ClickEvent>(OnSettings);
        MainScreen.QuitButton.RegisterCallback<ClickEvent>(OnQuit);

        SettingsScreen = new SettingsScreen(RootElement.Q<TemplateContainer>("SettingsScreen"));
        SettingsScreen.BackButton.RegisterCallback<ClickEvent>(OnMainMenu);

        HighScoresScreen = new HighScoresScreen(RootElement.Q<TemplateContainer>("HighScoresScreen"));
        HighScoresScreen.BackButton.RegisterCallback<ClickEvent>(OnMainMenu);

        RootElement.Add(MainScreen.RootElement);
        currentScreen = MainScreen;
    }

    private void LoadScreen(UIScreen screen)
    {
        currentScreen.RootElement.RemoveFromHierarchy();
        RootElement.Add(screen.RootElement);
        currentScreen = screen;
    }

    private void OnNewGame(ClickEvent evt)
    {
        EntityManager em = World.DefaultGameObjectInjectionWorld.EntityManager;
        em.SetComponentData(
            em.CreateEntity(typeof(AppCommand)),
            new AppCommand { Type = AppCommandType.LoadGameplay }
        );
    }

    private void OnHighScores(ClickEvent evt)
    {
        LoadScreen(HighScoresScreen);
    }

    private void OnSettings(ClickEvent evt)
    {
        LoadScreen(SettingsScreen);
    }

    private void OnMainMenu(ClickEvent evt)
    {
        LoadScreen(MainScreen);
    }

    private void OnQuit(ClickEvent evt)
    {
#if UNITY_STANDALONE
        Application.Quit();
#endif
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}

public class MainScreen : UIScreen
{
    public Button NewGameButton;
    public Button SettingsButton;
    public Button HighScoresButton;
    public Button QuitButton;

    public MainScreen(VisualElement root) : base(root)
    {
        NewGameButton = root.Q<Button>("NewGame");
        SettingsButton = root.Q<Button>("Settings");
        HighScoresButton = root.Q<Button>("HighScores");
        QuitButton = root.Q<Button>("Quit");
    }

}
public class HighScoresScreen : UIScreen
{
    public Button BackButton;

    public HighScoresScreen(VisualElement root) : base(root)
    {
        BackButton = root.Q<Button>("Back");
    }
}

public class SettingsScreen : UIScreen
{
    public Button BackButton;

    public SettingsScreen(VisualElement root) : base(root)
    {
        BackButton = root.Q<Button>("Back");
    }
}

