using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

namespace SimpleNetworkDemo.Player
{
    public class PlayerShooting : NetworkBehaviour
    {
        [SerializeField] private Projectile _projectile;
        [SerializeField] private AudioClip _projectileSound;
        [SerializeField] private float _cooldown = 0.5f;
        [SerializeField] private Transform _projectileSpawner;

        [SerializeField] private GameObject _bombPrefab;
        
        private float _lastFired = float.MinValue;

        public override void OnNetworkSpawn()
        {
            if (!IsOwner)
            {
                enabled = false;
            }
        }
        
        private void Update()
        {
            if (Input.GetMouseButton(0) && _lastFired + _cooldown < Time.time)
            {
                _lastFired = Time.time;
                ShootServerRpc();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                CreateBombServerRpc();
            }
        }
        [ServerRpc]
        private void CreateBombServerRpc()
        {
            var t = transform;
            
            var bomb = Instantiate(_bombPrefab, t.position, t.rotation);
            var networkObject = bomb.GetComponent<NetworkObject>();
            if (networkObject == null)
            {
                Debug.Log("NetworkObject is required for network", gameObject);
                return;
            }
            networkObject.Spawn();
        }

        [ServerRpc]
        private void ShootServerRpc()
        {
            var t = _projectileSpawner.transform;
            var projectile = Instantiate(_projectile, _projectileSpawner.position, _projectileSpawner.rotation);
            var networkObject = projectile.GetComponent<NetworkObject>();
            if (networkObject == null)
            {
                Debug.Log("NetworkObject is required for network", gameObject);
                return;
            }
            networkObject.Spawn();
            PlayShootingSoundClientRpc();
        }
        
        
        [ClientRpc]
        private void PlayShootingSoundClientRpc()
        {
            AudioSource.PlayClipAtPoint(_projectileSound, transform.position);
        }

        
        
    }
}
