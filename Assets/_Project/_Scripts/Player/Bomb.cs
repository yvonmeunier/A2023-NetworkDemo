using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace SimpleNetworkDemo.Player
{
    public class Bomb : NetworkBehaviour
    {
        [SerializeField] private float _explosionRadius = 4f;
        [SerializeField] private int _maxDamage = 125;
        [SerializeField] private LayerMask _layerMask;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _explosionRadius);
        }

        public override void OnNetworkSpawn()
        {
            if (!IsServer)
            {
                return;
            }
            Invoke(nameof(Explode), 3f);
        }

        private void Explode()
        {
            var colliders = Physics2D.OverlapCircleAll(transform.position, _explosionRadius, _layerMask);
            foreach (var c in colliders)
            {
                if (c.TryGetComponent<PlayerHealth>(out var health))
                {
                    var damage = CalculateDamage(c.transform.position);
                    health.TakeDamage(damage);
                }
            }
            GetComponent<NetworkObject>().Despawn();
        }

        private int CalculateDamage(Vector3 targetPosition)
        {
            // return 25; // fixed damage instead of distance based
            
            // Find distance between explosion and target
            var explosionTarget = targetPosition - transform.position;
            var explosionDistance = explosionTarget.magnitude; // between 0 and radius

            // explosion_radius - target_distance / explosion_radius
            // 4 - 0.2 = 3.8 / 4.0 = 0.95  ==> 95% des degats max
            // 4 - 0.7 = 3.3 / 4.0 = 0.825 ==> 82.5% des degats max
            // 4 - 3.0 = 1.0 / 4.0 = 0.25  ==> 25% des degats max
            var relativeDistance = (_explosionRadius - explosionDistance) / _explosionRadius;
            var damage = (int)Mathf.Max(0, (relativeDistance * _maxDamage));
            return damage;
        }
    }
}
