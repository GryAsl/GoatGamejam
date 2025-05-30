using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CustomerManager))]
public class CustomerManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CustomerManager script = (CustomerManager)target;

        if (GUILayout.Button("New Customer"))
        {
            script.NewCustomer();
        }
    }
}
