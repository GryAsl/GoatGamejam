using UnityEngine;
using System.Collections;

public class WashMachine : MonoBehaviour
{
    [Header("Plate Settings")]
    [SerializeField] private Transform plateStackPosition;
    [SerializeField] private GameObject platePrefab;
    [SerializeField] private int initialPlateCount = 1;
    [SerializeField] private float plateSpacing = 0.02f;
    
    [Header("Washing Settings")]
    [SerializeField] private float washingTime = 6f;
    
    private int currentPlateCount = 0;
    private bool isWashing = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Başlangıçta 4 tabak oluştur
        for (int i = 0; i < initialPlateCount; i++)
        {
            SpawnPlate();
        }
    }
    
    public void StartWashing()
    {
        if (!isWashing)
        {
            StartCoroutine(WashingProcess());
        }
    }
    
    private IEnumerator WashingProcess()
    {
        isWashing = true;
        yield return new WaitForSeconds(washingTime);
        SpawnPlate();
        isWashing = false;
    }
    
    private void SpawnPlate()
    {
        if (plateStackPosition == null || platePrefab == null) return;
        
        Vector3 spawnPosition = plateStackPosition.position + Vector3.up * (currentPlateCount * plateSpacing);
        Quaternion plateRotation = Quaternion.Euler(0f, 0f, 0f);
        GameObject newPlate = Instantiate(platePrefab, spawnPosition, plateRotation, plateStackPosition);
        
        // Tabak alınabilir olsun
        if (!newPlate.GetComponent<PickableObject>())
        {
            newPlate.AddComponent<PickableObject>();
        }
        
        currentPlateCount++;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
