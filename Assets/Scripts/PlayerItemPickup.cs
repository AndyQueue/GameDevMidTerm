using UnityEngine;

public class PlayerItemPickup : MonoBehaviour
{
    private GameObject objectInHand;
    [SerializeField] private float interactRadius = 0.5f;
    [SerializeField] private LayerMask interactableLayers;
    private Collider2D GetInteractable()
    {
        return Physics2D.OverlapCircle(transform.position, interactRadius, interactableLayers);
    }
    public void OnInteract()
    {
        if (HasHoldable())
        {
            if (TryFeedEater())
                return;
            else
            {
                DropHoldable();
                return;
            }
        }
        TryPickUpHoldable();
    }

    private void TryPickUpHoldable()
    {
        // Test if interactable is close
        Collider2D hit = GetInteractable();
        if (hit == null) return;

        // Test if interactable is holdable
        IHoldable holdable = hit.GetComponent<IHoldable>();
        if (holdable == null) return;

        // Perform pickup 
        PickUpHoldable(hit.gameObject);
        holdable.OnPickUp();
    }
    private void PickUpHoldable(GameObject holdable)
    {
        objectInHand = holdable;
        // "Attach" carrot to player
        holdable.transform.SetParent(transform);
        // Move carrot on top of player position
        holdable.transform.localPosition = Vector3.up * 0.5f;
        // Disable further interaction with carrot
        holdable.GetComponent<Collider2D>().enabled = false;
    }
    private bool HasHoldable() { return objectInHand != null; }
    private void DropHoldable()
    {
        // set Holdable parent to player parent (likely level or scene)
        objectInHand.transform.SetParent(transform.parent);
        objectInHand.transform.position += RandomDropOffset();
        // enable collider for future interactions.
        objectInHand.GetComponent<Collider2D>().enabled = true;
        objectInHand = null;
    }
    private Vector3 RandomDropOffset()
    {
        return new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0f);
    }

    private bool TryFeedEater()
    {
        // Test if holdable is feedable
        IFeedable feedable = objectInHand.GetComponent<IFeedable>();
        if (feedable == null) return false;

        Collider2D hit = GetInteractable();
        if (hit == null) return false;

        IEater eater = hit.GetComponent<IEater>();
        if (eater == null) return false;
        eater.Feed();

        Destroy(objectInHand);
        objectInHand = null;
        return true;
    }
}