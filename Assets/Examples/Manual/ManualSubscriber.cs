using Newtonsoft.Json.Linq;
using UnityEngine;

namespace DataBinding.Examples
{
    /// <summary>
    /// Example implementation of a manual subscriber to a specific key inside a data binding document
    /// </summary>
    [DefaultExecutionOrder(-100)]
    public class ManualSubscriber : MonoBehaviour
    {
        [SerializeField]private Document _document;
        [SerializeField]private string _keyToSubscribeTo;
        [SerializeField]private RectTransform _textElementTransform;

        public void Awake()
        {
            _document.Subscribe(_keyToSubscribeTo, OnChange);
        }

        public void OnChange(JToken value)
        {
            Vector2 position = new Vector2(value["anchoredPosition"]!["x"]!.ToObject<float>(), value["anchoredPosition"]!["y"]!.ToObject<float>());
            _textElementTransform.anchoredPosition = position;
        }
    }
}
