using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Items
{
    public class ItemPickupController : MonoBehaviour
    {
        [SerializeField] private Transform holdPosition;
        [SerializeField] private float rayDistance;
        [SerializeField] private float forcePower;

        private Item _heldItem;
        private PlayerControls _controls;

        public UnityEvent<bool> onUpdateState;

        public Item HeldItem
        {
            get => _heldItem;
            private set
            {
                _heldItem = value;
                onUpdateState.Invoke(value != null);
            }
        }

        private void Awake()
        {
            _controls = new PlayerControls();
            _controls.Gameplay.Pickup.performed += _ => TryPickupItem();
        }

        private void OnEnable()
        {
            _controls.Gameplay.Enable();
        }

        private void OnDisable()
        {
            _controls.Gameplay.Disable();
        }

        private void TryPickupItem()
        {
            if (HeldItem != null) return;

            RaycastHit hit;

            if (Physics.Raycast(Camera.main!.ScreenPointToRay(Touchscreen.current.primaryTouch.position.ReadValue()),
                    out hit, rayDistance))
            {
                if (hit.collider.gameObject.TryGetComponent<Item>(out var item))
                {
                    PickupItem(item);
                }
            }
        }

        private void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                TryPickUpByClick();
            }
        }

        private void TryPickUpByClick()
        {
            if (HeldItem != null) return;

            var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit, 10f))
            {
                if (hit.collider.gameObject.TryGetComponent<Item>(out var item))
                {
                    PickupItem(item);
                }
            }
        }

        private void PickupItem(Item item)
        {
            HeldItem = item;
            HeldItem.gameObject.transform.SetParent(holdPosition);
            HeldItem.gameObject.transform.localPosition = Vector3.zero;
            HeldItem.gameObject.transform.localRotation = Quaternion.identity;

            var rb = HeldItem.GetItemRb();
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        public void DropItem()
        {
            if (!HeldItem) return;

            HeldItem.gameObject.transform.SetParent(null);

            var rb = HeldItem.GetItemRb();
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.AddForce(Camera.main!.transform.forward * forcePower, ForceMode.Impulse);

            HeldItem = null;
        }
    }
}