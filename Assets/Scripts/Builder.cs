using NUnit.Framework;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.ObjectChangeEventStream;
using UnityEditor.EventSystems;
using UnityEngine.UIElements.InputSystem;

public class Builder : MonoBehaviour
{
    public GameObject prefab;
    public GameObject ghost;
    public List<GameObject> prefabs = new List<GameObject>();
    public List<GameObject> builded = new List<GameObject>();

    public float gridSize = 1f;
    public LayerMask buildBlockerLayers;
    public Vector3 snappedPos;

    public bool tezgahTypeShi;



    void Start()
    {
        CreateGhost(prefab);
    }

    void Update()
    {
        if (!ghost)
            return;
        bool stop = false;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, buildBlockerLayers))
        {
            snappedPos = SnapToGridFunc(hit.point);
            //Debug.LogWarning("lastSnappedPos: " + ghost.GetComponent<Building>().lastSnappedPos + " snappedPos: " + snappedPos);
            if (tezgahTypeShi)
            {
                if (ghost.GetComponent<Building>().lastSnappedPos == snappedPos)
                {
                    Debug.Log("Anayn ");
                    stop = true;
                }
                else tezgahTypeShi = false;
            }

            if (!stop)
            {
                ghost.transform.position = snappedPos;
            }
            //Debug.Log(hit.collider);
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
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            if (!ghost.GetComponent<Building>().collision)
            {
                builded.Add(Instantiate(prefab));
                builded[builded.Count - 1].transform.position = ghost.transform.position;
                builded[builded.Count - 1].transform.rotation = ghost.transform.rotation;
            }
        }


    }

    public void CreateGhost(GameObject go)
    {
        prefab = go;
        if (ghost)
        {
            Destroy(ghost.gameObject);
            ghost = Instantiate(go);
        }
        else ghost = Instantiate(go);
        ghost.GetComponent<Building>().isGhost = true;
    }

    private IEnumerator InstantiateAfterFrame(GameObject prefab)
    {
        yield return null;
        ghost = Instantiate(prefab);
    }

    Vector3 SnapToGridFunc(Vector3 pos)
    {
        float x = Mathf.Round(pos.x / gridSize) * gridSize;
        float y = Mathf.Round(pos.y / gridSize) * gridSize;
        float z = Mathf.Round(pos.z / gridSize) * gridSize;
        return new Vector3(x, y, z);
    }
}
