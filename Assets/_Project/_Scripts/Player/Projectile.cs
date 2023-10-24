using System;
using Unity.Netcode;
using UnityEngine;

namespace SimpleNetworkDemo.Player
{
    public class Projectile : MonoBehaviour 
    {
        [SerializeField] private Vector2 _velocity;

        private void Start()
        {
            Invoke(nameof(DestroyItself), 5);
        }

        private void FixedUpdate()
        {
            var translation = Vector2.up * _velocity * Time.fixedDeltaTime;
            transform.Translate(translation, Space.Self);
        }

        private void DestroyItself()
        {
            Destroy(gameObject);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<PlayerVisual>(out var visual))
            {
                visual.ChangeColorServerRpc();
            }

            if (other.TryGetComponent<PlayerHealth>(out var health))
            {
                health.TakeDamage(20);
            }
            
            DestroyItself();
        }
    }
}
