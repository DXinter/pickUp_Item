using UnityEngine;

namespace Game
{
    public class Item : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;

        public Rigidbody GetItemRb()
        {
            return rb;
        }
    }
}