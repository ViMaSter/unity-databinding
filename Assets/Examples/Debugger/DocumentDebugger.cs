using System.Linq;
using DataBinding;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Document))]
public class DocumentDebugger : MonoBehaviour
{
    [SerializeField] private bool _isShown;
    public Document Document;
    public Text Text;
    public IInputActionCollection playerInput;

    private const string DEBUG_PATH = "_global.document.debug";

    private void Start()
    {
        playerInput ??= new PlayerInput();
        playerInput.Enable();
        playerInput.First(a => a.name == "Toggle Debug View").performed += OnKeyCombinationPerformed;

        Document = GetComponent<Document>();
        Debug.Assert(Document != null, "Document debugger must be attached to the same game object as the Document component");
        Document.Subscribe(DEBUG_PATH, OnToggleDebugFlag);

        var childObject = new { d = 1, e = 2.0f, f = "3" };
        var parentObject = new object[] { childObject, childObject, childObject };
        Document.Set("data", new {
            a = new {
                b = new {
                    c = parentObject
                } , 
                d = new {
                    c = parentObject
                },
                e = new
                {
                    c = parentObject
                },
                f = new
                {
                    c = parentObject
                },
                g = new
                {
                    c = parentObject
                },
                h = new
                {
                    c = parentObject
                }
            }
        });
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
