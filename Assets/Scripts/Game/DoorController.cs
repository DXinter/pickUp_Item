using System.Collections;
using Player;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game
{
    public class DoorController : MonoBehaviour
    {
        [SerializeField] private Transform door;
        [SerializeField] private Button doorButton;
        [SerializeField] private float openAngle = 90f;
        [SerializeField] private float openSpeed = 2f;
        [SerializeField] private float interactDistance = 2f;

        private PlayerController _playerController;
        private bool _isOpen = false;
        private Quaternion _closedRotation;
        private Quaternion _openRotation;
        private Transform _player;

        [Inject]
        public void Construct(PlayerController playerController)
        {
            _playerController = playerController;
        }

        private void Start()
        {
            _closedRotation = door.rotation;
            _openRotation = Quaternion.Euler(0, openAngle, 0) * _closedRotation;
            doorButton.gameObject.SetActive(false);
            doorButton.onClick.AddListener(ToggleDoor);
            _player = _playerController.gameObject.transform;
        }

        private void Update()
        {
            var distance = Vector3.Distance(_player.position, transform.position);

            doorButton.gameObject.SetActive(distance < interactDistance);
        }

        private void ToggleDoor()
        {
            _isOpen = !_isOpen;
            StopAllCoroutines();
            StartCoroutine(RotateDoor(_isOpen ? _openRotation : _closedRotation));
        }

        private IEnumerator RotateDoor(Quaternion targetRotation)
        {
            var time = 0f;
            var startRotation = door.rotation;

            while (time < 1f)
            {
                time += Time.deltaTime * openSpeed;
                door.rotation = Quaternion.Lerp(startRotation, targetRotation, time);
                yield return null;
            }

            door.rotation = targetRotation;
        }
    }
}