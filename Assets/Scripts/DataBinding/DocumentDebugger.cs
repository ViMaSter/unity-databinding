using Newtonsoft.Json.Linq;
using UnityEngine;

namespace DataBinding
{
    [RequireComponent(typeof(Document))]
    public class DocumentDebugger : MonoBehaviour
    {
        private bool _isShown;
        private Document _document;
        [SerializeField] private UnityEngine.UI.Text _text;

        private const string debugPath = "_global.document.debug";

        private void Start()
        {
            _document = GetComponent<Document>();
            Debug.Assert(_document != null, "Document debugger must be attached to the same game object as the Document component");
            _document.Subscribe(debugPath, OnToggleDebugFlag);

            //debugHelperObject = Instantiate()
        }

        private void OnToggleDebugFlag(JToken obj)
        {
            _isShown = obj.ToObject<bool>();

            _text.GetComponentInParent<Canvas>().enabled = _isShown;
        }

        private void Update()
        {
            if (_isShown)
            {
                _text.text = _document.GeneratePrettyPrint();
            }

            if (!Input.GetKeyDown(KeyCode.D) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
            {
                return;
            }

            if (!(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
            {
                return;
            }

            if (!(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
            {
                return;
            }

            ToggleDebugView();
        }

        private void ToggleDebugView()
        {
            _document.Set("_global.document.debug", !_document.Get<bool>("_global.document.debug", false));
        }
    }
}
