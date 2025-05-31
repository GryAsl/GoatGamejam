using UnityEngine;

public class Trashcan : MonoBehaviour
{
    public LayerMask targetLayer;

    private void Update()
    {
        Collider[] hits = Physics.OverlapBox(transform.position, transform.localScale , Quaternion.identity, targetLayer);
        foreach (Collider hit in hits)
        {
            if (hit.gameObject != this.gameObject)
            {
                Destroy(hit.gameObject);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}