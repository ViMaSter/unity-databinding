using System;
using UnityEngine;

namespace DataBinding
{
    public class DataBindingValueUpdate : MonoBehaviour
    {
        [SerializeField] private Document _document;
        [SerializeField] private string _keyRoot;
        private Vector2 _position = Vector2.zero;
        private PlayerInput movement;

        private void Start()
        {
            movement = new PlayerInput();
            movement.Default.Movement.performed += Movement_performed;
        }

        private void Movement_performed(UnityEngine.InputSystem.InputAction.CallbackContext movementContext)
        {
            var moveBy = movementContext.ReadValue<Vector2>();
            _position += moveBy;
            UpdateDocument();
        }

        private void UpdateDocument()
        {
            _document.Set(_keyRoot, new { anchoredPosition = new { _position.x, _position.y } });
        }
    }
}
