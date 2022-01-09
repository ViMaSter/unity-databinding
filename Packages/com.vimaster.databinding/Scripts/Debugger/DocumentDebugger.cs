using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace DataBinding.Debugger
{
    public class DocumentDebugger : MonoBehaviour
    {
        private bool _isShown;
        private DebuggerPlayerInput _debuggerPlayerInput;
        
        public Document Document;
        public Text Text;

        private const string DEBUG_PATH = "_global.document.debug";

        private void Start()
        {
            _debuggerPlayerInput ??= new DebuggerPlayerInput();
            _debuggerPlayerInput.Enable();
            _debuggerPlayerInput.Default.ToggleDebugView.performed += OnKeyCombinationPerformed;

            Document.Subscribe(DEBUG_PATH, OnToggleDebugFlag);
        }

        private void OnKeyCombinationPerformed(InputAction.CallbackContext obj)
        {
            ToggleDebugView();
        }

        public void ToggleDebugView()
        {
            Document.Set(DEBUG_PATH, !Document.Get<bool>("_global.document.debug", false));
        }

        private void OnToggleDebugFlag(JToken obj)
        {
            _isShown = obj.ToObject<bool>();

            Text.GetComponentInParent<Canvas>().enabled = _isShown;
        }

        private void Update()
        {
            if (_isShown)
            {
                Text.text = Document.GeneratePrettyPrint();
            }
        }

        public void CopyToClipboard()
        {
            GUIUtility.systemCopyBuffer = Text.text;
        }
    }

}