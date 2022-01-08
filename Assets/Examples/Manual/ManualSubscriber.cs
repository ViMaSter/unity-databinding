using Newtonsoft.Json.Linq;
using UnityEngine;

namespace DataBinding.Examples
{
    /// <summary>
    /// Example implementation of a manual subscriber to a specific path inside a data binding document
    /// </summary>
    [DefaultExecutionOrder(-100)]
    public class ManualSubscriber : MonoBehaviour
    {
        [SerializeField]private Document _document;
        [SerializeField]private string _documentPath;
        [SerializeField]private RectTransform _textElementTransform;

        public void Awake()
        {
            _document.Subscribe(_documentPath, OnChange);
        }

        public void OnChange(JToken value)
        {
            var position = new Vector2(value["anchoredPosition"]!["x"]!.ToObject<float>(), value["anchoredPosition"]!["y"]!.ToObject<float>());
            _textElementTransform.anchoredPosition = position;
        }
    }
}
