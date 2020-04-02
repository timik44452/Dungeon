using UnityEngine;

namespace InventorySystem
{
    [RequireComponent(typeof(Inventory))]
    public class PickUpController : MonoBehaviour
    {
        public float PickUpProgress { get; private set; }

        public float PickUpDuration = 0.35F;

        public ItemObject currentItem { get; private set; }

        private TargetSystem targetSystem;
        private Inventory inventory;

        private void Start()
        {
            inventory = GetComponent<Inventory>();
            targetSystem = FindObjectOfType<TargetSystem>();
        }

        private void FixedUpdate()
        {
            if (!TargetSystem.ITargetIsNull(targetSystem.Target) &&
                targetSystem.Target.gameObject.GetComponent<ItemObject>() &&
                Vector3.Distance(targetSystem.Target.transform.position, transform.position) < 2)
            {
                currentItem = targetSystem.Target.gameObject.GetComponent<ItemObject>();

                if (Input.GetKey(KeyCode.E))
                {
                    PickUpProgress += Time.fixedDeltaTime / PickUpDuration;
                }
                else
                {
                    PickUpProgress = 0;
                }

                if (PickUpProgress >= 1.0F && inventory.TryAddItem(currentItem))
                {
                    Destroy(currentItem.gameObject);
                }
            }
            else
            {
                currentItem = null;
            }
        }
    }
}
