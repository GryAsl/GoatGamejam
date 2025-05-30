using UnityEngine;

public class TableController : MonoBehaviour
{
    [SerializeField] private Transform platePoint; // Tabağın konulacağı nokta
    [SerializeField] private string requestedPlateType; // İstenen tabak tipi (PlateA, PlateB, vb.)
    [SerializeField] private bool needsPlate = false; // Tabak isteği var mı?
    
    // İstenen tabak tipi
    public string GetRequestedPlateType()
    {
        return requestedPlateType;
    }
    
    // Tabağın konulacağı nokta
    public Transform GetPlatePoint()
    {
        return platePoint;
    }
    
    // Tabak isteği var mı?
    public bool NeedsPlate()
    {
        return needsPlate;
    }
    
    // Tabak geldiğinde çağrılacak
    public void PlateDelivered(string plateType)
    {
        if (plateType == requestedPlateType)
        {
            needsPlate = false;
            Debug.Log("Table " + name + " received requested plate: " + plateType);
            
            // Burada müşteri için diğer mantığı ekleyebilirsiniz
            // Örneğin: yemeği sunma, müşterinin yemeği yemesi, vb.
        }
    }
    
    // Tabak isteği oluşturmak için
    public void RequestPlate(string plateType)
    {
        requestedPlateType = plateType;
        needsPlate = true;
        Debug.Log("Table " + name + " is requesting plate: " + plateType);
    }
    
    // Rastgele bir tabak isteği oluşturmak için test fonksiyonu
    public void RequestRandomPlate()
    {
        string[] plateTypes = { "PlateA", "PlateB", "PlateC" };
        int randomIndex = Random.Range(0, plateTypes.Length);
        RequestPlate(plateTypes[randomIndex]);
    }
}