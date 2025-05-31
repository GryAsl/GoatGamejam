using UnityEngine;

public class GameManager : MonoBehaviour
{
    UIManager uiMan;
    public AudioManager audioManager;
    public bool isGameOn;
    public enum GameState
    {
        mainMenu,
        normal,
        building
    }

    public GameState gameState = GameState.normal;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uiMan = GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (gameState)
        {
            case GameState.building:

                break;
        }
    }

    public void ChangeState(GameState state)
    {
        gameState = state;
        switch (gameState)
        {

            case GameState.normal:
                StartCoroutine(uiMan.TurnOffPanel(uiMan.buildingPanel));
                break;
            case GameState.building:
                StartCoroutine(uiMan.TurnOnPanel(uiMan.buildingPanel));
                break;
        }
    }

    public void StartGame()
    {

        ChangeState(GameState.normal);
        audioManager.ChangeMusicToInGame();
        StartCoroutine(uiMan.TurnOffPanel(uiMan.mainMenu));
        isGameOn = true;

    }
}
