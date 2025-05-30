using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CustomEditor(typeof(GameManager))]
public class GameManagerDebug : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GameManager script = (GameManager)target;

        if (GUILayout.Button("NormalState"))
        {
            script.ChangeState(GameManager.GameState.normal); // Veya direkt: Debug.Log("AH");
        }
        if (GUILayout.Button("BuildingState"))
        {
            script.ChangeState(GameManager.GameState.building); // Veya direkt: Debug.Log("AH");
        }
    }
}
