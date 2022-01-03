using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace DataBinding
{
    [DefaultExecutionOrder(-102)]
    public class Document : MonoBehaviour
    {
        private readonly JObject _documentRoot = new JObject();

        /// <summary>
        ///     Get the full contents of this document in human-readable format
        /// </summary>
        /// <returns>An indented JSON string of all values inside this document</returns>
        public string GeneratePrettyPrint()
        {
            return JsonConvert.SerializeObject(
                _documentRoot,
                Formatting.Indented,
                new StringEnumConverter()
            );
        }
        private readonly Dictionary<string, List<Action<JToken>>> _subscriptions = new Dictionary<string, List<Action<JToken>>>();

        /// <summary>
        /// Get the value at the specific <see cref="path"/> inside the document
        /// </summary>
        /// <param name="path">Absolute path to the value inside the document</param>
        /// <param name="defaultValue">Value to return instead, if no value is found at the specified <see cref="path"/></param>
        /// <typeparam name="T">Type to cast the stored object to</typeparam>
        /// <returns>A representation of the value</returns>
        /// <exception cref="JsonException">Thrown if the document has no value at the requested <see cref="path"/> and <see cref="defaultValue"/> is set to `null`</exception>
        public T Get<T>(string path, JToken defaultValue = null)
        {
            try
            {
                // ReSharper disable once PossibleNullReferenceException "False positive: will throw JsonException instead"
                return _documentRoot.SelectToken(path, true).ToObject<T>();
            }
            catch (JsonException)
            {
                if (defaultValue != null)
                {
                    return defaultValue.ToObject<T>();
                }
                throw;
            }
        }

        /// <summary>
        /// Get the value at the specific <see cref="path"/> inside the document
        /// </summary>
        /// <param name="path">Absolute path to the value inside the document</param>
        /// <param name="type">Type to cast the stored object to</param>
        /// <param name="defaultValue">Value to return instead, if no value is found at the specified <see cref="path"/></param>
        /// <returns>A representation of the value</returns>
        /// <exception cref="JsonException">Thrown if the document has no value at the requested <see cref="path"/> and <see cref="defaultValue"/> is set to `null`</exception>
        public object Get(string path, Type type, JToken defaultValue = null)
        {
            try
            {
                // ReSharper disable once PossibleNullReferenceException "False positive: will throw JsonException instead"
                return _documentRoot.SelectToken(path, true).ToObject(type);
            }
            catch (JsonException)
            {
                if (defaultValue != null)
                {
                    return defaultValue.ToObject(type);
                }
                throw;
            }
        }

        /// <summary>
        /// Get the values for a specific path in objects inside an array
        /// </summary>
        /// <param name="path">Absolute path to the key of objects inside an array</param>
        /// <returns>An enumerable of JToken representations of the values</returns>
        /// <exception cref="JsonException">Thrown if the document has no values at the requested <see cref="path"/></exception>
        public IEnumerable<JToken> GetKeysOfArrayOfObjects(string path)
        {
            return _documentRoot.SelectTokens(path, true);
        }

        private static IEnumerable<string> GetParentsFromPath(string path)
        {
            var splitPath = path.Trim('.').Split('.');
            List<string> result = new List<string>();
            string currentPath = "";
            foreach (string partOfPath in splitPath)
            {
                currentPath += partOfPath;
                result.Add(currentPath);
                currentPath += ".";
            }

            return result;
        }

        private static IEnumerable<string> GetKeysFromJToken(JToken jToken, string prefix)
        {
            switch (jToken.Type)
            {
                case JTokenType.Object:
                {
                    var jObject = (JObject)jToken;
                    var subObjects = jObject.Properties().Where(property => property.Value.Type == JTokenType.Object).SelectMany(subProperties => subProperties.Value.Values()).Select(subProperty => GetKeysFromJToken(subProperty, prefix)).ToList().SelectMany(key => key);
                    var subArrays = jObject.Properties().Where(property => property.Value.Type == JTokenType.Array).SelectMany(subProperties => subProperties.Value.Values()).Select(subProperty => GetKeysFromJToken(subProperty, prefix)).ToList().SelectMany(key => key);
                    var keys  = jObject.Properties().Select(property => prefix + property.Path);

                    return subObjects.Concat(subArrays).Concat(keys).Append((prefix + jToken.Path).TrimEnd('.'));
                }
                case JTokenType.Array:
                {
                    return ((JArray)jToken).SelectMany(array => GetKeysFromJToken(array, prefix));
                }
                default:
                    return new List<string>() {prefix.TrimEnd('.') + jToken.Path};
            }
        }

        /// <summary>
        /// Sets or overwrites a value at <see cref="path"/> inside the document
        /// </summary>
        /// <param name="path">Absolute path to the value to create or overwrite</param>
        /// <param name="value">Value to start at <see cref="path"/></param>
        public void Set(string path, object value)
        {
            JToken valueAsJToken = JToken.FromObject(value);
            var splitPath = path.Split('.').ToList();
            var currentPath = "";
            foreach (string keyInPath in splitPath)
            {
                int arraySize = 0;
                int indexOfArrayOpenBracket = -1;
                int indexOfArrayClosingBracket = -1;
                if (keyInPath.Contains("["))
                {
                    indexOfArrayOpenBracket = keyInPath.IndexOf("[", StringComparison.Ordinal);
                    indexOfArrayClosingBracket = keyInPath.IndexOf("]", StringComparison.Ordinal);
                    var canParseInt = int.TryParse(keyInPath.Substring(indexOfArrayOpenBracket+1, keyInPath.Length - indexOfArrayClosingBracket), out arraySize);
                    ++arraySize;
                    Debug.Assert(canParseInt, $"Unable to parse index operator of '{keyInPath}' of '{path}'");
                    var hasClosingBracket = keyInPath.Contains("]");
                    Debug.Assert(hasClosingBracket, $"Found [ but no matching ] inside path '{path}'");
                }
                currentPath += keyInPath;
                JToken tokenAtPath = _documentRoot.SelectToken(currentPath);
                if (tokenAtPath == null)
                {
                    var list = currentPath.Split('.').ToList();
                    var currentKey = list[list.Count - 1];
                    if (arraySize != 0)
                    {
                        currentKey = currentKey.Substring(0, indexOfArrayOpenBracket);
                    }
                    list.RemoveAt(list.Count - 1);
                    _documentRoot.SelectToken(string.Join(".", list))[currentKey] = arraySize == 0 ? new JObject() : new JArray(Enumerable.Range(0, arraySize).Select(i => new JObject())) as JToken;
                    tokenAtPath = _documentRoot.SelectToken(string.Join(".", list.Append(currentKey)));
                }
                currentPath += ".";

                if (arraySize == 0 && tokenAtPath.Type == JTokenType.Object)
                {
                    continue;
                }
                if (arraySize != 0 && tokenAtPath.Type == JTokenType.Array)
                {
                    continue;
                }

                if (currentPath == path && tokenAtPath.Type == valueAsJToken.Type)
                {
                    continue;
                }
                throw new NotSupportedException($"'{tokenAtPath.Path}' is of type '{tokenAtPath.Type}' not of '{JTokenType.Object}'; setting '{tokenAtPath.Path}' would replace this value to an object implicitly. Either overwrite '{tokenAtPath.Path}' directly or delete it first.");
            }

            var tokenOfPathInDocument = _documentRoot.SelectToken(path);
            tokenOfPathInDocument.Replace(valueAsJToken);

            InformSubscribersForPath(path, valueAsJToken);
        }

        /// <summary>
        /// Removes an object and all it's parents 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool Delete(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("'path' cannot be empty or null", path);
            }

            var token = _documentRoot.SelectToken(path, false);
            if (token == null)
            {
                return false;
            }
            InformSubscribersForPath(path, null);
            token.Remove();
            return true;
        }

        private void InformSubscribersForPath(string path, JToken valueAsJToken)
        {
            IEnumerable<string> subscriptionPathsToInform = new []{path};
            if (valueAsJToken != null)
            {
                subscriptionPathsToInform = GetKeysFromJToken(valueAsJToken, path + ".").ToList();
            }

            var subscriptionsToInform = _subscriptions.Where(keyValue => subscriptionPathsToInform.Contains(keyValue.Key));
            foreach (KeyValuePair<string, List<Action<JToken>>> keyValuePair in subscriptionsToInform)
            {
                foreach (Action<JToken> action in keyValuePair.Value)
                {
                    action(valueAsJToken?.SelectToken(keyValuePair.Key.Substring(Math.Min(keyValuePair.Key.Length, path.Length + 1))));
                }
            }
            Debug.Log($"Set '{path}' to '{valueAsJToken}'\r\nAttempted to inform subscriptions for: '{string.Join("','", subscriptionPathsToInform)}'");
        }

        /// <summary>
        /// Subscribe to updates of a specific path in the document
        /// </summary>
        /// <param name="path">Absolute data binding path</param>
        /// <param name="action">Action to call on changes to the value at <see cref="path"/></param>
        /// <returns>The <see cref="action"/> parameter</returns>
        public Action<JToken> Subscribe(string path, Action<JToken> action)
        {
            Debug.Log($"Subscribed to '{path}'");
            if (!_subscriptions.ContainsKey(path))
            {
                _subscriptions[path] = new List<Action<JToken>>();
            }
            _subscriptions[path].Add(action);
            return action;
        }

        /// <summary>
        /// Attempts to unsubscribe a specific action from a data binding path
        /// </summary>
        /// <param name="path">Absolute data binding path</param>
        /// <param name="action">Specific action to remove</param>
        /// <returns>True, if the action was previously subscribe and has been successfully removed. False, if the action was never subscribed to this path.</returns>
        public bool Unsubscribe(string path, Action<JToken> action)
        {
            Debug.Log($"Attempting to unsubscribe from '{path}'");
            if (!_subscriptions.ContainsKey(path))
            {
                return false;
            }
            return _subscriptions[path].Remove(action);
        }
    }
}
