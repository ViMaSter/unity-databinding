using System.Linq;
using DataBinding;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DocumentDebugger : MonoBehaviour
{
    private bool _isShown;
    public Document Document;
    public Text Text;
    public IInputActionCollection PlayerInput;

    private const string DEBUG_PATH = "_global.document.debug";

    private void Start()
    {
        PlayerInput ??= new PlayerInput();
        PlayerInput.Enable();
        PlayerInput.First(a => a.name == "Toggle Debug View").performed += OnKeyCombinationPerformed;

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
