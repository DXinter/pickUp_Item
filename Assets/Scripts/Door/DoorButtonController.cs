using Player;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Door
{
    public class DoorButtonController : MonoBehaviour
    {
        [SerializeField] private Button doorButton;
        [SerializeField] private float interactDistance = 2f;

        private DoorController _doorController;
        private PlayerController _playerController;
        private Transform _player;

        [Inject]
        public void Construct(DoorController doorController, PlayerController playerController)
        {
            _doorController = doorController;
            _playerController = playerController;
        }

        private void Start()
        {
            doorButton.gameObject.SetActive(false);
            _player = _playerController.gameObject.transform;
        }

        private void OnEnable()
        {
            doorButton.onClick.AddListener(_doorController.ToggleDoor);
        }

        private void OnDisable()
        {
            doorButton.onClick.RemoveListener(_doorController.ToggleDoor);
        }

        private void Update()
        {
            var distance = Vector3.Distance(_player.position, _doorController.transform.position);
            doorButton.gameObject.SetActive(distance < interactDistance);
        }
    }
}