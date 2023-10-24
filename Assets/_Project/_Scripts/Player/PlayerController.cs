using Unity.Netcode;
using UnityEngine;

namespace SimpleNetworkDemo.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private Vector2 _velocity;

        [Header("Dependencies")]
        [SerializeField] private Rigidbody2D _body;
        [SerializeField] private Transform _weapon;
        
        private Vector2 _currentMousePosition;
        private Vector2 _movement;

        private void Update() 
        {
            _movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            _currentMousePosition = Input.mousePosition;
        }

        private void FixedUpdate()
        {
            MoveCharacter();
            RotateCharacter();
        }

        private void MoveCharacter()
        {
            var movement = _movement * _velocity * Time.fixedDeltaTime;
            _body.MovePosition(_body.position + movement);
        }
        
        private void RotateCharacter()
        {
            var worldPosition = Camera.main.ScreenToWorldPoint(_currentMousePosition);
            var newAim = (worldPosition - transform.position).normalized;
            Rotate(newAim);
        }

        private void Rotate(Vector2 newAimDirection)
        {
            var rotationZ = Mathf.Atan2(newAimDirection.y, newAimDirection.x) * Mathf.Rad2Deg;
            _weapon.rotation = Quaternion.Euler(0, 0, rotationZ);
        }
    }
}
