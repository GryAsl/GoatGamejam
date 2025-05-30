using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UIManager))]
public class DebugThings : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        UIManager script = (UIManager)target;

        //if (GUILayout.Button("NewOrder"))
        //{
        //    script.NewOrder(); 
        //}
    }
}
