using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

[DefaultExecutionOrder(100)]
public class Document : MonoBehaviour
{
    private Newtonsoft.Json.Linq.JObject a = new JObject();
    private Dictionary<string, List<Action<string, JToken>>> subscriptions = new Dictionary<string, List<Action<string, JToken>>>();

    private void Start()
    {
        Set("test", new
        {
            a = new
            {
                b = 1,
                c = new[] { 2, 3, 4 },
                d = new[] { new {e = 1}, new {e = 2}}
            }
        });
    }

    public JToken GetTokenAt(string key)
    {
        return a.SelectToken(key, true);
    }

    public IEnumerable<JToken> GetTokensAt(string key)
    {
        return a.SelectTokens(key, true);
    }

    private IEnumerable<string> GetKeysFromObject(JToken ob, string prefix)
    {
        if (ob.Type == JTokenType.Object)
        {
            var asOb = (JObject)ob;
            var objs = asOb.Properties().Where(a => a.Value.Type == JTokenType.Object).SelectMany(a => a.Value.Values()).Select(prop => GetKeysFromObject(prop, prefix)).ToList().SelectMany(a => a);
            var arrays = asOb.Properties().Where(a => a.Value.Type == JTokenType.Array).SelectMany(a => a.Value.Values()).Select(prop => GetKeysFromObject(prop, prefix)).ToList().SelectMany(a => a);
            var keys  = asOb.Properties().Select(a => prefix + a.Path);

            return objs.Concat(arrays).Concat(keys).Append((prefix + ob.Path).TrimEnd('.'));
        }
        if (ob.Type == JTokenType.Array)
        {
            var asOb = (JArray)ob;
            return asOb.SelectMany(a=>GetKeysFromObject(a, prefix)).Append(prefix + ob.Path);
        }
        return new[] { prefix + ob.Path };
    }

    public void Set(string key, object value)
    {
        if (key.Contains("."))
        {
            throw new NotSupportedException("Cannot set sub-objects; pass a dynamic object set to its parent key to overwrite it");
        }

        var newToken = JObject.FromObject(value);
        var events = GetKeysFromObject(newToken, key+".").ToList();

        a[key] = newToken;
        Debug.Log(JsonConvert.SerializeObject(a));

        var notify = subscriptions.Where(keyValue => events.Contains(keyValue.Key));
        foreach (KeyValuePair<string, List<Action<string, JToken>>> keyValuePair in notify)
        {
            foreach (Action<string, JToken> action in keyValuePair.Value)
            {
                action(keyValuePair.Key, newToken.SelectToken(keyValuePair.Key.Substring(Math.Min(keyValuePair.Key.Length, key.Length + 1))));
            }
        }
    }

    public void Subscribe(string key, Action<string, JToken> callback)
    {
        if (!subscriptions.ContainsKey(key))
        {
            subscriptions[key] = new List<Action<string, JToken>>();
        }
        subscriptions[key].Add(callback);
    }
}
