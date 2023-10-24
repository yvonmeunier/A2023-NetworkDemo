using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

namespace SimpleNetworkDemo.Player
{
    public class PlayerShooting : MonoBehaviour
    {
        [SerializeField] private Projectile _projectile;
        [SerializeField] private AudioClip _projectileSound;
        [SerializeField] private float _cooldown = 0.5f;
        [SerializeField] private Transform _projectileSpawner;

        [SerializeField] private GameObject _bombPrefab;
        
        private float _lastFired = float.MinValue;

        private void Update()
        {
            if (Input.GetMouseButton(0) && _lastFired + _cooldown < Time.time)
            {
                _lastFired = Time.time;
                Shoot();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                CreateBomb();
            }
        }

        private void CreateBomb()
        {
            var t = transform;
            var bomb = Instantiate(_bombPrefab, t.position, t.rotation);
        }

        private void Shoot()
        {
            var t = _projectileSpawner.transform;
            var projectile = Instantiate(_projectile, _projectileSpawner.position, _projectileSpawner.rotation);
            
            PlayShootingSound();
        }
        
        private void PlayShootingSound()
        {
            AudioSource.PlayClipAtPoint(_projectileSound, transform.position);
        }
    }
}
