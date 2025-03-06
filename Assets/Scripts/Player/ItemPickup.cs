using Game;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Player
{
    public class ItemPickup : MonoBehaviour
    {
        [SerializeField] private Transform holdPosition;
        [SerializeField] private Button dropButton;
        [SerializeField] private float rayDistance;
        [SerializeField] private float forcePower;

        private Item _heldItem;
        private PlayerControls _controls;

        private void Awake()
        {
            _controls = new PlayerControls();
            _controls.Gameplay.Pickup.performed += _ => TryPickupItem();
        }

        private void OnEnable()
        {
            dropButton.onClick.AddListener(DropItem);
            _controls.Gameplay.Enable();
        }

        private void OnDisable()
        {
            dropButton.onClick.RemoveListener(DropItem);
            _controls.Gameplay.Disable();
        }

        private void TryPickupItem()
        {
            if (_heldItem != null) return;

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

        private void PickupItem(Item item)
        {
            _heldItem = item;
            _heldItem.gameObject.transform.SetParent(holdPosition);
            _heldItem.gameObject.transform.localPosition = Vector3.zero;
            _heldItem.gameObject.transform.localRotation = Quaternion.identity;

            var rb = _heldItem.GetItemRb();
            rb.isKinematic = true;
            rb.useGravity = false;

            dropButton.gameObject.SetActive(true);
        }

        private void DropItem()
        {
            if (!_heldItem) return;

            _heldItem.gameObject.transform.SetParent(null);

            var rb = _heldItem.GetItemRb();
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.AddForce(Camera.main!.transform.forward * forcePower, ForceMode.Impulse);

            _heldItem = null;
            dropButton.gameObject.SetActive(false);
        }
    }
}