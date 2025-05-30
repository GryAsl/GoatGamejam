using UnityEngine;

public class DropZone : MonoBehaviour
{
    private GameObject currentItem;

    public bool IsOccupied => currentItem != null;

    public void SetItem(GameObject item)
    {
        if (IsOccupied)
        {
            Debug.LogWarning("DropZone already occupied!");
            return;
        }

        currentItem = item;
        item.transform.SetParent(transform);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;
    }

    public void ClearItem()
    {
        currentItem = null;
    }

    public GameObject GetItem()
    {
        return currentItem;
    }

    private void Update()
    {
        // Eğer eşya başka yere taşındıysa DropZone temizlensin
        if (currentItem != null && currentItem.transform.parent != transform)
        {
            ClearItem();
        }
    }
}