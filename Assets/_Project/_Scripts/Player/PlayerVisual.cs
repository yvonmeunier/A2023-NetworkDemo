using Unity.Netcode;
using UnityEngine;

namespace SimpleNetworkDemo.Player
{
    public class PlayerVisual : NetworkBehaviour 
    {
        private readonly Color[] _colors = { Color.red, Color.blue, Color.green, Color.yellow, Color.black, Color.white, Color.magenta, Color.gray };
        
        private readonly NetworkVariable<Color> _netColor = new();
        private int _index;

        [SerializeField] private SpriteRenderer _renderer;

        private void Awake() 
        {
            _netColor.OnValueChanged += OnValueChanged;
        }

        public override void OnDestroy() 
        {
            _netColor.OnValueChanged -= OnValueChanged;
        }

        private void OnValueChanged(Color prev, Color next) 
        {
            _renderer.material.color = next;
        }

        public override void OnNetworkSpawn() 
        {
            if (IsOwner) 
            {
                _index = (int)OwnerClientId;
                ChangeColorServerRpc();
            }
            else 
            {
                _renderer.material.color = _netColor.Value;
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void ChangeColorServerRpc() 
        {
            _netColor.Value = GetNextColor();
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (!IsOwner) return;
            ChangeColorServerRpc();
        }

        private Color GetNextColor() 
        {
            return _colors[_index++ % _colors.Length];
        }
    }
}
