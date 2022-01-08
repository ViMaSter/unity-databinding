using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DataBinding.EventGeneration.Implementations.UnityEngine.UI
{
    public class ButtonEventGenerator : IComponentEventGenerator<Button>
    {
        public class UIButtonListener : EventTrigger
        {
            private Document _document;
            private string _path;

            private void OnDisable()
            {
                _document.Delete($"{_path}");
            }

            public void Setup(Document document, string path)
            {
                _document = document;
                _path = path;
            }

            public override void OnPointerDown(PointerEventData data)
            {
                _document.Set($"{_path}.events.down", true);
            }

            private IEnumerator OnClick()
            {
                _document.Set($"{_path}.events.clicked", true);
                yield return new WaitForEndOfFrame();
                _document.Set($"{_path}.events.clicked", false);
            }

            public override void OnPointerUp(PointerEventData data)
            {
                _document.Set($"{_path}.events.down", false);
                StartCoroutine(OnClick());
            }
        }

        public ButtonEventGenerator(Document document, string path, Component component)
        {
            var listener = component.gameObject.AddComponent<UIButtonListener>();
            listener.Setup(document, path);
        }
    }
}