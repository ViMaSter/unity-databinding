using DataBinding;
using UnityEngine;

namespace DataBinding
{
    public class DataBindingValueUpdate : MonoBehaviour
    {
        [SerializeField] private Document _document;
        [SerializeField] private string _keyRoot;
        private Vector2 _position = Vector2.zero;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                _position.y--;
                UpdateDocument();
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                _position.y++;
                UpdateDocument();
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                _position.x--;
                UpdateDocument();
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                _position.x++;
                UpdateDocument();
            }
        }

        private void UpdateDocument()
        {
            _document.Set(_keyRoot, new { anchoredPosition = new { _position.x, _position.y } });
        }
    }
}
