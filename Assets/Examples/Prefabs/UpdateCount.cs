using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DataBinding.EventGeneration.Examples.Prefabs
{
    public class UpdateCount : MonoBehaviour
    {
        public class DataSet
        {
            public class LabelValues
            {
                // ReSharper disable once InconsistentNaming Required for automated component property update
                public string text = "0";
            }
            public class AnchoredPositionValues
            {
                public class Rect
                {
                    // ReSharper disable once InconsistentNaming Required for automated component property update
                    public int x;
                    // ReSharper disable once InconsistentNaming Required for automated component property update
                    public int y;
                }
                // ReSharper disable once InconsistentNaming Required for automated component property update
                public Rect anchoredPosition = new Rect();
            }

            public AnchoredPositionValues Rect = new AnchoredPositionValues();
            public LabelValues Label = new LabelValues();

            public DataSet(int index)
            {
                Label.text = index.ToString();
                Rect.anchoredPosition.y = index * -15;
            }
        }

        [SerializeField] private Document _document;
        [SerializeField] private string _documentPath;
        private PlayerInput _playerInput;
        private readonly List<DataSet> _children = new List<DataSet>();

        private void Start()
        {
            _playerInput = new PlayerInput();
            _playerInput.Enable();
            _playerInput.Default.Movement.performed += OnMovementPerformed;
        }

        private void OnMovementPerformed(InputAction.CallbackContext obj)
        {
            var change = obj.ReadValue<Vector2>();
            if (change == Vector2.zero)
            {
                return;
            }

            // Up and down arrow change count of items inside array
            if (change.y > 0)
            {
                _children.Add(new DataSet(_children.Count));
            }

            if (_children.Any())
            {
                // Left and right arrow change value of last item
                if (change.x > 0)
                {
                    _children[_children.Count - 1].Rect.anchoredPosition.x += 15;
                }

                if (change.x < 0)
                {
                    _children[_children.Count - 1].Rect.anchoredPosition.x -= 15;
                    _children[_children.Count - 1].Rect.anchoredPosition.x = Math.Max(0, _children[_children.Count - 1].Rect.anchoredPosition.x);
                }

                // Up and down arrow change count of items inside array
                if (change.y < 0)
                {
                    _children.RemoveAt(_children.Count - 1);
                }
            }

            _document.Set(_documentPath, _children);

        }
    }
}
