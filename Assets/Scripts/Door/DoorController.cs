using System.Collections;
using UnityEngine;

namespace Door
{
    public class DoorController : MonoBehaviour
    {
        [SerializeField] private Transform door;
        [SerializeField] private float openAngle = 90f;
        [SerializeField] private float openSpeed = 2f;
        
        private bool _isOpen = false;
        private Quaternion _closedRotation;
        private Quaternion _openRotation;

        private void Start()
        {
            _closedRotation = door.rotation;
            _openRotation = Quaternion.Euler(0, openAngle, 0) * _closedRotation;
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

        public void ToggleDoor()
        {
            _isOpen = !_isOpen;
            StopAllCoroutines();
            StartCoroutine(RotateDoor(_isOpen ? _openRotation : _closedRotation));
        }
    }
}