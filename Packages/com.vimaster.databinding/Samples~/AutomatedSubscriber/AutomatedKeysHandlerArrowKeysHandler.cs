using UnityEngine;
using DataBinding;

public class AutomatedKeysHandlerArrowKeysHandler : MonoBehaviour
{
    [SerializeField] private Document _document;
    [SerializeField] private string _documentPath;
    private Vector2 _position = Vector2.zero;
    private AutomatedSubscriberPlayerInput movement;

    private void Start()
    {
        movement = new AutomatedSubscriberPlayerInput();
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
