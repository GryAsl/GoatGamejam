using UnityEngine;

[CreateAssetMenu(fileName = "Foods", menuName = "Scriptable Objects/Foods")]
public class Foods : ScriptableObject
{
    public string foodName;
    public string Ingredients;
    public float maxTime;
}
