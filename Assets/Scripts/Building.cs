using UnityEngine;

public class Building : MonoBehaviour
{
    public bool collision;
    public Material greenMaterial; // Atanacak materyal
    public Material redMaterial; // Atanacak materyal

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        collision = true;
        ReplaceAllMaterials(redMaterial);
    }

    private void OnTriggerExit(Collider other)
    {
        collision = false;
        ReplaceAllMaterials(greenMaterial);
    }



    public void ReplaceAllMaterials(Material mat)
    {
        foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>())
        {
            Material[] originalMats = renderer.sharedMaterials;
            Material[] newMats = new Material[originalMats.Length];

            int index = 0;
            foreach (Material _ in originalMats)
            {
                newMats[index] = mat;
                index++;
            }

            renderer.materials = newMats;
        }

        Debug.Log("Tüm materyaller başarıyla değiştirildi.");
    }
}
