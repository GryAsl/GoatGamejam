using UnityEngine;

public class Building : MonoBehaviour
{
    public string buildingName;
    public bool collision;
    public Material greenMaterial; 
    public Material redMaterial;
    public bool isGhost;

    Builder builder;

    public Vector3 lastSnappedPos;


    void Start()
    {
        builder = GameObject.Find("GameManager").GetComponent<Builder>();
        if (isGhost)
        {
            ReplaceAllMaterials(greenMaterial);
            if(buildingName == "3d")
                ReplaceAllMaterials(redMaterial);

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (buildingName == "3d" && !builder.tezgahTypeShi)
            ReplaceAllMaterials(redMaterial);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isGhost)
            return;

        other.TryGetComponent<Building>(out Building building);
        if (building)
        {
            if (building.buildingName == "tezgah" && buildingName == "3d")
            {
                Debug.Log("2");
                builder.tezgahTypeShi = true;
                lastSnappedPos = builder.snappedPos;
                transform.position = building.gameObject.transform.GetChild(0).transform.position;
                ReplaceAllMaterials(greenMaterial);
            }
            else
            {
                collision = true;
                ReplaceAllMaterials(redMaterial);
            }
        }
        else
        {
            collision = true;
            ReplaceAllMaterials(redMaterial);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isGhost)
            return;

        collision = false;
        if(buildingName == "3d" && !builder.tezgahTypeShi)
            ReplaceAllMaterials(redMaterial);
        else
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

    }
}
