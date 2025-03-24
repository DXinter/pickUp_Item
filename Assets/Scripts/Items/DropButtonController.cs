using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Items
{
    public class DropButtonController : MonoBehaviour
    {
        private ItemPickupController _itemPickupController;
        [SerializeField] private Button dropButton;

        [Inject]
        public void Construct(ItemPickupController itemPickupController)
        {
            _itemPickupController = itemPickupController;
        }

        private void Start()
        {
            dropButton.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            _itemPickupController.onUpdateState.AddListener(UpdateState);
            dropButton.onClick.AddListener(_itemPickupController.DropItem);
        }

        private void OnDisable()
        {
            _itemPickupController.onUpdateState.RemoveListener(UpdateState);
            dropButton.onClick.RemoveListener(_itemPickupController.DropItem);
        }

        private void UpdateState(bool value)
        {
            dropButton.gameObject.SetActive(value);
        }
    }
}