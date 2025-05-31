using UnityEngine;

public class Food : MonoBehaviour
{
    public Foods foodData;
    public Vector3 plateOffset = new Vector3(0, 0.1f, 0); // Tabak üzerindeki pozisyonu ayarlamak için
    public bool isOnPlate = false;

    private void Start()
    {
        if (foodData == null)
        {
            Debug.LogError($"Food {gameObject.name} has no food data assigned!");
        }
    }

    public bool CanCombineWith(Food otherFood)
    {
        // Burada yemeklerin birleşebilme kurallarını belirleyebiliriz
        // Örneğin: Et ve patates birleşebilir
        return true; // Şimdilik hepsi birleşebilir
    }
} 