using UnityEngine;
public class RoomWallSimpleMover : MonoBehaviour
{
    public Collider roomTrigger;          // Odanýn tamamýný saran collider (isTrigger açýk olacak)
    public Vector3 downOffset = new Vector3(0, -2, 0);
    public float moveSpeed = 3f;

    private Vector3 upPos;
    private Vector3 downPos;
    private bool isDown = false;
    private Coroutine moveCoroutine;

    void Start()
    {
        upPos = transform.position;
        downPos = upPos + downOffset;
    }

    void Update()
    {
        if (roomTrigger != null)
        {
            // Player collider'ýn içindeyse
            bool playerInside = false;
            foreach (var col in Physics.OverlapBox(roomTrigger.bounds.center, roomTrigger.bounds.extents, roomTrigger.transform.rotation))
            {
                if (col.CompareTag("Player"))
                {
                    playerInside = true;
                    break;
                }
            }

            if (playerInside && !isDown)
            {
                MoveWall(downPos);
                isDown = true;
            }
            else if (!playerInside && isDown)
            {
                MoveWall(upPos);
                isDown = false;
            }
        }
    }

    void MoveWall(Vector3 target)
    {
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        moveCoroutine = StartCoroutine(MoveWallCoroutine(target));
    }

    System.Collections.IEnumerator MoveWallCoroutine(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * moveSpeed);
            yield return null;
        }
        transform.position = target;
    }
}