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
    public LayerMask buildToDestroyLayers;
    public Vector3 snappedPos;


    public int red = 1;
    public int yellow = 1;
    public int oven = 0;
    public int ovenNormalValue = 0;
    public int counter = 2;
    public int printer = 2;
    public int trash = 1;
    public CanvasGroup cgRed;
    public CanvasGroup cgYellow;
    public CanvasGroup cgOven;
    public CanvasGroup cgCounter;
    public CanvasGroup cgPrinter;
    public CanvasGroup cgTrash;


    public bool tezgahTypeShi;

    private GameManager gm;



    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {


        if (gm.gameState != GameManager.GameState.building)
            return;
        red = 1;
        yellow = 1;
        oven = ovenNormalValue;
        counter = 2;
        printer = 2;
        trash = 1;
        List<GameObject> tempList = new List<GameObject>(builded);

        foreach (GameObject go in tempList)
        {
            if (go == null)
                continue;
            switch (go.GetComponent<Building>().buildingIndex)
            {
                case 0:
                    red = 0;
                    break;
                case 1:
                    yellow = 0;
                    break;
                case 2:
                    oven = 0;
                    break;
                case 3:
                    counter -= 1;
                    break;
                case 4:
                    printer -= 1;
                    break;
                case 5:
                    trash -= 1;
                    break;
                default:
                    break;
            }
        }
        if (red == 0)
        {
            cgRed.alpha = .5f;
            cgRed.interactable = false;
            cgRed.blocksRaycasts = false;
        }
        else
        {
            cgRed.alpha = 1f;
            cgRed.interactable = true;
            cgRed.blocksRaycasts = true;
        }
        if (yellow == 0)
        {
            cgYellow.alpha = .5f;
            cgYellow.interactable = false;
            cgYellow.blocksRaycasts = false;
        }
        else
        {
            cgYellow.alpha = 1f;
            cgYellow.interactable = true;
            cgYellow.blocksRaycasts = true;
        }
        if (oven == 0)
        {
            cgOven.alpha = .5f;
            cgOven.interactable = false;
            cgOven.blocksRaycasts = false;
        }
        else
        {
            cgOven.alpha = 1f;
            cgOven.interactable = true;
            cgOven.blocksRaycasts = true;
        }
        if (counter == 0)
        {
            cgCounter.alpha = .5f;
            cgCounter.interactable = false;
            cgCounter.blocksRaycasts = false;
        }
        else
        {
            cgCounter.alpha = 1f;
            cgCounter.interactable = true;
            cgCounter.blocksRaycasts = true;
        }
        if (printer == 0)
        {
            cgPrinter.alpha = .5f;
            cgPrinter.interactable = false;
            cgPrinter.blocksRaycasts = false;
        }
        else
        {
            cgPrinter.alpha = 1f;
            cgPrinter.interactable = true;
            cgPrinter.blocksRaycasts = true;
        }
        if (trash == 0)
        {
            cgTrash.alpha = .5f;
            cgTrash.interactable = false;
            cgTrash.blocksRaycasts = false;
        }
        else
        {
            cgTrash.alpha = 1f;
            cgTrash.interactable = true;
            cgTrash.blocksRaycasts = true;
        }

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

            Debug.LogError("111");
            if (ghost.GetComponent<Building>().buildingName == "trash")
            {
                Debug.LogError("333");
                DestroyImmediate(ghost.GetComponent<Building>().currentCollisionGO);
            }
            else if (!ghost.GetComponent<Building>().currentCollision)
            {
                builded.Add(Instantiate(prefab));
                builded[builded.Count - 1].transform.position = ghost.transform.position;
                builded[builded.Count - 1].transform.rotation = ghost.transform.rotation;
                builded[builded.Count - 1].GetComponent<Building>().isGhost = false;
                Debug.LogError("444");
                DestroyImmediate(ghost);
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

    Vector3 SnapToGridFunc(Vector3 pos)
    {
        float x = Mathf.Round(pos.x / gridSize) * gridSize;
        float y = Mathf.Round(pos.y / gridSize) * gridSize;
        float z = Mathf.Round(pos.z / gridSize) * gridSize;
        x += .1f;
        z -= .05f;
        return new Vector3(x, y, z);
    }
}


