using UnityEngine;
using System.Collections;

public class MutfakAlet : MonoBehaviour, IInteractable
{
    [SerializeField] public Transform Yemekpos;
    
    [Header("Food 1 Phases")]
    [SerializeField] private GameObject food1Phase1;
    [SerializeField] private GameObject food1Phase2;
    [SerializeField] private GameObject food1Phase3;
    
    [Header("Food 2 Phases")]
    [SerializeField] private GameObject food2Phase1;
    [SerializeField] private GameObject food2Phase2;
    [SerializeField] private GameObject food2Phase3;
    
    [Header("Food Types")]
    [SerializeField] private Foods food1;
    [SerializeField] private Foods food2;
    
    private bool isPrinting = false;
    private GameObject currentPhase;
    private Foods currentFood;
    private bool isFood1Selected = true;

    private void Start()
    {
        // Null kontrolleri
        if (Yemekpos == null)
        {
            Debug.LogError($"{gameObject.name}: Yemekpos transform atanmamış!");
        }
        
        if (food1Phase1 == null || food1Phase2 == null || food1Phase3 == null)
        {
            Debug.LogError($"{gameObject.name}: Food1 phase prefab'ları eksik!");
        }
        
        if (food2Phase1 == null || food2Phase2 == null || food2Phase3 == null)
        {
            Debug.LogError($"{gameObject.name}: Food2 phase prefab'ları eksik!");
        }
    }

    public void Interact(PlayerInteraction player)
    {
        if (!isPrinting && Input.GetKeyDown(KeyCode.E))
        {
            isFood1Selected = !isFood1Selected;
            StartPrinting();
        }
    }

    public void Setcooke(Transform pos)
    {
        if (pos != null)
            Yemekpos = pos;
    }

    private void StartPrinting()
    {
        if (isPrinting || Yemekpos == null) return;
        
        isPrinting = true;
        currentFood = isFood1Selected ? food1 : food2;
        StartCoroutine(PrintingProcess());
    }

    private IEnumerator PrintingProcess()
    {
        if (currentPhase != null) Destroy(currentPhase);

        // Phase 1
        GameObject phase1Prefab = isFood1Selected ? food1Phase1 : food2Phase1;
        if (phase1Prefab == null)
        {
            Debug.LogError("Phase 1 prefab null!");
            isPrinting = false;
            yield break;
        }
        
        currentPhase = Instantiate(phase1Prefab, Yemekpos.position, Yemekpos.rotation);
        currentPhase.transform.SetParent(transform);
        yield return new WaitForSeconds(5f);

        // Phase 2
        Destroy(currentPhase);
        GameObject phase2Prefab = isFood1Selected ? food1Phase2 : food2Phase2;
        if (phase2Prefab == null)
        {
            Debug.LogError("Phase 2 prefab null!");
            isPrinting = false;
            yield break;
        }
        
        currentPhase = Instantiate(phase2Prefab, Yemekpos.position, Yemekpos.rotation);
        currentPhase.transform.SetParent(transform);
        yield return new WaitForSeconds(5f);

        // Phase 3
        Destroy(currentPhase);
        GameObject phase3Prefab = isFood1Selected ? food1Phase3 : food2Phase3;
        if (phase3Prefab == null)
        {
            Debug.LogError("Phase 3 prefab null!");
            isPrinting = false;
            yield break;
        }
        
        currentPhase = Instantiate(phase3Prefab, Yemekpos.position, Yemekpos.rotation);
        currentPhase.transform.SetParent(transform);
        
        // Component'ları güvenli şekilde ekle
        if (currentPhase.GetComponent<Food>() == null)
            currentPhase.AddComponent<Food>();
            
        if (currentPhase.GetComponent<BoxCollider>() == null)
            currentPhase.AddComponent<BoxCollider>();
            
        currentPhase.layer = LayerMask.NameToLayer("Interactable");
        
        yield return new WaitForSeconds(5f);
        isPrinting = false;
    }
}
