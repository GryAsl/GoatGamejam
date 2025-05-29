using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.ObjectChangeEventStream;

public class Builder : MonoBehaviour
{
    public GameObject prefab;
    public GameObject ghost;
    public List<GameObject> prefabs = new List<GameObject>();
    public List<GameObject> builded = new List<GameObject>();

    public float gridSize = 1f;
    public LayerMask buildBlockerLayers;

    void Start()
    {
        CreateGhost(prefab);
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, buildBlockerLayers))
        {
            Vector3 snappedPos = SnapToGridFunc(hit.point);
            ghost.transform.position = snappedPos;
            Debug.Log(hit.collider);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            ghost.transform.Rotate(0f, 90f, 0);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            ghost.transform.Rotate(0f, -90f, 0f);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (!ghost.GetComponent<Building>().collision)
                builded.Add(Instantiate(ghost));
        }


    }

    public void CreateGhost(GameObject go)
    {
        if (ghost)
            Destroy(ghost);
        ghost = Instantiate(go);
    }

    Vector3 SnapToGridFunc(Vector3 pos)
    {
        float x = Mathf.Round(pos.x / gridSize) * gridSize;
        float y = Mathf.Round(pos.y / gridSize) * gridSize;
        float z = Mathf.Round(pos.z / gridSize) * gridSize;
        return new Vector3(x, y, z);
    }
}
