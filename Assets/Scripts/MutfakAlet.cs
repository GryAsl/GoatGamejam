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

    }

    public void Interact(PlayerInteraction player)
    {
        Debug.LogWarning("MutfakAlet Interact");
        if (isPrinting) return;
        
        if (player.IsHoldingItem() && CompareTag("Filament"))
        {
            GameObject heldItem = player.GetHeldItem();
            
            // Food tipini belirle
            if (heldItem.name.Contains("Filament1"))
                isFood1Selected = true;
            else
                isFood1Selected = false;
            
            // Eldeki itemi yok et (filament olarak kullanıldı)
            player.TrashItem();
            
            // Printing işlemini başlat
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
        if (isPrinting == true)
        {
            return;
            
        }
            
        else
        {
            isPrinting = true;
            currentFood = isFood1Selected ? food1 : food2;
            StartCoroutine(PrintingProcess());
        }
        
    }

    private IEnumerator PrintingProcess()
    {
        if (currentPhase != null) Destroy(currentPhase);

        // Phase 1
        GameObject phase1Prefab = isFood1Selected ? food1Phase1 : food2Phase1;
        if (phase1Prefab == null)
        {
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