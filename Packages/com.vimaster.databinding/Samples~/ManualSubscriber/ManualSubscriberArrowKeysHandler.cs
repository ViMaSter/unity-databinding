using UnityEngine;

namespace DataBinding.EventGeneration
{
    public class ManualSubscriberArrowKeysHandler : MonoBehaviour
    {
        [SerializeField] private Document _document;
        [SerializeField] private string _documentPath;
        private Vector2 _position = Vector2.zero;
        private ManualSubscriberPlayerInput movement;

        private void Start()
        {
            movement = new ManualSubscriberPlayerInput();
            movement.Enable();
        }

        private void Update()
        {
            var moveBy = movement.Default.Movement.ReadValue<Vector2>();
            _position += moveBy * (Time.deltaTime * 100);
            UpdateDocument();
        }

        private void UpdateDocument()
        {
            _document.Set(_documentPath, new { anchoredPosition = new { _position.x, _position.y } });
        }
    }
}
