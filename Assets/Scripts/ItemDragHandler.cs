using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

   Transform originalParent;
    CanvasGroup canvasGroup;
    public float minDropDistance = 0.4f;
    public float maxDropDistance = 0.5f;
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(transform.root);
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
        Slot dropSlot= eventData.pointerEnter?.GetComponent<Slot>();
        if (dropSlot == null)
        {
            GameObject dropItem=eventData.pointerEnter;
            if(dropItem != null)
            {
                dropSlot = dropItem.GetComponentInParent<Slot>();
            }
        }
        Slot originalSlot = originalParent.GetComponent<Slot>();
        if (dropSlot != null)
        {
            if (dropSlot.currentItem != null)
            {
                dropSlot.currentItem.transform.SetParent(originalSlot.transform);
                originalSlot.currentItem = dropSlot.currentItem;
                dropSlot.currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
            else
            {
                originalSlot.currentItem = null;
            }
            transform.SetParent(dropSlot.transform);
            dropSlot.currentItem = gameObject;
        }

        else
        {
            if (!IsWithinInventory(eventData.position))
            { 
                DropItem(originalSlot);
            }
            else
            {
                transform.SetParent(originalParent);                
            }

        }
        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }
    bool IsWithinInventory(Vector2 mousePosition)
    {
        RectTransform inventoryRect = originalParent.parent.GetComponent<RectTransform>();
        return RectTransformUtility.RectangleContainsScreenPoint(inventoryRect, mousePosition);
    }
    public Transform worldCanvasTransform;

    void DropItem(Slot originalSlot)
    {
        originalSlot.currentItem = null;
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (playerTransform == null)
        {
            Debug.LogError("Player object not found in the scene.");
            return;
        }

        Vector2 dropOffset = Random.insideUnitCircle * Random.Range(minDropDistance, maxDropDistance);
        Vector2 dropPosition = (Vector2)playerTransform.position + dropOffset;

        // Sinh ra và đặt ngay vào World Canvas, giữ nguyên thông số RectTransform ban đầu
        GameObject dropItem = Instantiate(gameObject, dropPosition, Quaternion.identity, worldCanvasTransform);

        // Đảm bảo tỷ lệ local không bị méo
        dropItem.transform.localScale = Vector3.one;

        dropItem.GetComponent<BounceEffect>().StartBounce();
        Destroy(gameObject);
    }
}