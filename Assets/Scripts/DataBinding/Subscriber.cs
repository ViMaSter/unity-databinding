using Newtonsoft.Json.Linq;
using UnityEngine;

[DefaultExecutionOrder(101)]
public class Subscriber : MonoBehaviour
{
    public Document document;
    public string keyToSubscribeTo;

    public void Awake()
    {
        document.Subscribe(keyToSubscribeTo, OnChange);
    }

    public void OnChange(string key, JToken value)
    {
        Debug.Log($"Listening to '{keyToSubscribeTo}': Update for '{key}' -> '{value}'");
    }
}
