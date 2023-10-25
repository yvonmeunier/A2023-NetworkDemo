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

        private int _currentHealth;

        private void Start()
        {
            _currentHealth = _maxHealth;
        }

        private void OnGUI()
        {
            int OwnerClientId = 1;
            
            // Please do not do this
            GUI.Label(
                new Rect(10, 100 + (OwnerClientId * 20), 100, 20), 
                $"#{OwnerClientId} - {_currentHealth} / {_maxHealth}");
        }

        public void TakeDamage(int damage)
        {
            _currentHealth = Math.Max(0, _currentHealth - damage);
            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Destroy(gameObject);
        }
    }
}
