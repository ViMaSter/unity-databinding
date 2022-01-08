using DataBinding;
using UnityEngine;

public class UIArrowEventSubscriber : MonoBehaviour
{
    public Document Document;
    public string Path;

    private Vector2 _currentPosition;
    private const float SPEED = 25f;

    void Update()
    {
        Vector2 moveBy = new Vector2();
        if (Document.Get<bool>("Up.events.down", false))
        {
            moveBy.y = 1;
        }
        if (Document.Get<bool>("Down.events.down", false))
        {
            moveBy.y = -1;
        }
        if (Document.Get<bool>("Left.events.down", false))
        {
            moveBy.x = -1;
        }
        if (Document.Get<bool>("Right.events.down", false))
        {
            moveBy.x = 1;
        }

        _currentPosition += moveBy * (SPEED * Time.deltaTime);
        Document.Set($"{Path}.anchoredPosition", new { _currentPosition.x, _currentPosition.y });
    }
}
