using System;
using Unity.Netcode;
using UnityEngine;

namespace SimpleNetworkDemo.Player
{
    public class Projectile : NetworkBehaviour 
    {
        [SerializeField] private Vector2 _velocity;

        public override void OnNetworkSpawn()
        {
            if (!IsServer)
            {
                return;
            }
            Invoke(nameof(DestroyItself), 5);
        }

        private void FixedUpdate()
        {
            var translation = Vector2.up * _velocity * Time.fixedDeltaTime;
            transform.Translate(translation, Space.Self);
        }

        private void DestroyItself()
        {
            GetComponent<NetworkObject>().Despawn();
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!IsServer)
            {
                return;
            }
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
