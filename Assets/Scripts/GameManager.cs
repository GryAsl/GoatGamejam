using UnityEngine;

public class GameManager : MonoBehaviour
{
    UIManager uiMan;

    public enum GameState
    {
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
                StartCoroutine(uiMan.TurnOffBuildingPanel());
                break;
            case GameState.building:
                StartCoroutine(uiMan.TurnOnBuildingPanel());
                break;
        }
    }
}
