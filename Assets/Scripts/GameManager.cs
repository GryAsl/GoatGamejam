using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    UIManager uiMan;
    public AudioManager audioManager;
    public CutSceneManager cutSceneManager;
    public bool isGameOn;
    public Camera mainCam;
    public Builder builder;
    public CustomerManager customerMan;

    public bool isLevelUP;
    public bool isBuildingDone;
    public float timerMultiplier = 1f;
    
    [Header("Game Score")]
    public int score = 0;  // This will be visible in the inspector

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
                timerMultiplier = 1f;
                break;
            case GameState.building:
                StartCoroutine(uiMan.TurnOnPanel(uiMan.buildingPanel));
                timerMultiplier = 0f;
                break;
        }
    }

    public void StartGame()
    {
        mainCam.enabled = false;
        StartCoroutine(cutSceneManager.CutScene1());
        audioManager.ChangeMusicToInGame();
        StartCoroutine(uiMan.TurnOffPanel(uiMan.mainMenu));

    }

    public void CutsceneDone()
    {
        mainCam.enabled = true;
        isGameOn = true;
        ChangeState(GameState.building);
    }

    public void BuildingDone()
    {
        if (!isBuildingDone)
        {
            isBuildingDone = true;
            customerMan.StartSpawning();
            StartCoroutine(uiMan.NewPopUP(uiMan.popUP1));
            StartCoroutine(uiMan.NewPopUP(uiMan.popUP2));
        }
        StartCoroutine(uiMan.NewPopUP(uiMan.popUP3));
        StartCoroutine(uiMan.NewPopUP(uiMan.popUP4));
        ChangeState(GameState.normal);
        
    }

    public void Level1Passed()
    {
        builder.ovenNormalValue = 1;
        isLevelUP = true;
        ChangeState(GameState.building);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().ToString());
    }

    public void ENDGAME()
    {
        StartCoroutine(uiMan.TurnOnPanel(uiMan.endMenu));
        isGameOn = false;
    }

}
