using UnityEngine;

public class Building : MonoBehaviour
{
    public string buildingName;
    public bool currentCollision;
    public GameObject currentCollisionGO;
    public Material greenMaterial; 
    public Material redMaterial;
    public bool isGhost;

    Builder builder;

    public Vector3 lastSnappedPos;
    public LayerMask layer;


    void Start()
    {
                Debug.LogError("222 : "+ isGhost);
        builder = GameObject.Find("GameManager").GetComponent<Builder>();
        if (isGhost)
        {
            GetComponent<BoxCollider>().isTrigger = true;
            ReplaceAllMaterials(greenMaterial);
            if (buildingName == "3d")
                ReplaceAllMaterials(redMaterial);
        }
        else
        {
            GetComponent<BoxCollider>().isTrigger = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (buildingName == "3d" && !builder.tezgahTypeShi && isGhost)
            ReplaceAllMaterials(redMaterial);
        if(buildingName == "3d" && !isGhost)
        {
                Debug.Log("hit.collider");
            Ray ray = new Ray(transform.position, -transform.up);
            if (!Physics.Raycast(ray, out RaycastHit hit, 1f, layer))
            {
                Destroy(gameObject);
            }
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (!isGhost)
            return;

        other.TryGetComponent<Building>(out Building building);
        if (building)
        {
            currentCollisionGO = other.gameObject;
            if (building.buildingName == "tezgah" && buildingName == "3D")
            {
                Debug.Log("2");
                builder.tezgahTypeShi = true;
                lastSnappedPos = builder.snappedPos;
                transform.position = building.gameObject.transform.GetChild(0).transform.position;
                ReplaceAllMaterials(greenMaterial);
            }
            else
            {
                currentCollision = true;
                ReplaceAllMaterials(redMaterial);
            }
        }
        else
        {
            currentCollision = true;
            ReplaceAllMaterials(redMaterial);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isGhost)
            return;

        currentCollision = false;
        if (buildingName == "3d" && !builder.tezgahTypeShi)
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
