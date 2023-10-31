using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace SimpleNetworkDemo.Player
{
    public class PlayerHealth : NetworkBehaviour
    {
        [SerializeField] private int _maxHealth = 100;

        private readonly NetworkVariable<int> _currentHealth = new();

        public override void OnNetworkSpawn()
        {
            _currentHealth.Value = _maxHealth;
        }

        private void OnGUI()
        {
            // Please do not do this
            GUI.Label(
                new Rect(10, 100 + (OwnerClientId * 20), 100, 20), 
                $"#{OwnerClientId} - {_currentHealth.Value} / {_maxHealth}");
        }

        public void TakeDamage(int damage)
        {
            _currentHealth.Value = Math.Max(0, _currentHealth.Value - damage);
            if (_currentHealth.Value <= 0)
            {
                Die();
            }

            

        }
        

        private void Die()
        {
            GetComponent<NetworkObject>().Despawn();
        }
    }
}
