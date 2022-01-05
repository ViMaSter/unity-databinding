#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace DataBinding
{
    [DefaultExecutionOrder(-110)]
    public class Document : MonoBehaviour
    {
        private interface ISubscriptionCollection
        {
            public void CallSubscriptions(object? value);
            public int Count { get; }
        }

        private class SubscriptionCollection<T> : ISubscriptionCollection
        {
            private readonly List<Action<T>> _subscriptions = new List<Action<T>>();

            public void Add(Action<T> action)
            {
                _subscriptions.Add(action);
            }

            public bool Remove(Action<T> action)
            {
                return _subscriptions.Remove(action);
            }

            public void CallSubscriptions(object? value)
            {
                foreach (Action<T> subscription in _subscriptions)
                {
                    subscription((T)value!);
                }
            }

            public int Count => _subscriptions.Count;
        }

        private readonly Dictionary<string, Dictionary<Type, ISubscriptionCollection>> _typeSpecificSubscriptions = new Dictionary<string, Dictionary<Type, ISubscriptionCollection>>();
        private readonly Dictionary<string, SubscriptionCollection<JToken>> _typeAgnosticSubscriptions = new Dictionary<string, SubscriptionCollection<JToken>>();
        /// <summary>
        /// Subscribe to updates of a specific path in the document if the value type matches <see cref="T"/>
        /// </summary>
        /// <remarks>
        /// Subscriptions set with this method are only called, if the underlying value of the path matches <see cref="T"/>.
        /// If the path is updated with a mismatching type, a warning is printed to the console and none of the mismatching events get called.
        /// For subscriptions that are called even if value types change, use <see cref="Subscribe"/>
        /// </remarks>
        /// <param name="path">Absolute data binding path</param>
        /// <param name="action">Action to call on changes to the value at <see cref="path"/></param>
        /// <returns>The <see cref="action"/> parameter</returns>
        public Action<T> Subscribe<T>(string path, Action<T> action)
        {
            if (!_typeSpecificSubscriptions.ContainsKey(path))
            {
                _typeSpecificSubscriptions[path] = new Dictionary<Type, ISubscriptionCollection>();
            }

            if (!_typeSpecificSubscriptions[path].ContainsKey(typeof(T)))
            {
                _typeSpecificSubscriptions[path][typeof(T)] = new SubscriptionCollection<T>();
            }

            ((SubscriptionCollection<T>)_typeSpecificSubscriptions[path][typeof(T)]).Add(action);

            JToken? currentValue = _documentRoot.SelectToken(path, false);
            if (currentValue == null)
            {
                return action;
            }

            T attemptedCast = default;
            bool successfulCast = false;
            try
            {
                attemptedCast = currentValue.ToObject<T>();
                successfulCast = true;
            }
            catch (Exception)
            {
                // any exception during object conversion is acceptable, so we need to suppress them to continue execution
            }

            if (successfulCast)
            {
                action(attemptedCast!);
            }

            return action;
        }

        /// <summary>
        /// Subscribe to updates of a specific path in the document no matter of value type
        /// </summary>
        /// <remarks>
        /// Subscriptions set with this method are called even if the underlying value of the path changes.
        /// For subscriptions that are limited to a specific value type, use <see cref="Subscribe{T}"/>
        /// </remarks>
        /// <param name="path">Absolute data binding path</param>
        /// <param name="action">Action to call on changes to the value at <see cref="path"/></param>
        /// <returns>The <see cref="action"/> parameter</returns>
        public Action<JToken> Subscribe(string path, Action<JToken> action)
        {
            if (!_typeAgnosticSubscriptions.ContainsKey(path))
            {
                _typeAgnosticSubscriptions[path] = new SubscriptionCollection<JToken>();
            }

            _typeAgnosticSubscriptions[path].Add(action);
            JToken? currentValue = _documentRoot.SelectToken(path, false);
            if (currentValue != null)
            {
                action(currentValue);
            }

            return action;
        }

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

        /// <summary>
        /// Get the value at the specific <see cref="path"/> inside the document
        /// </summary>
        /// <param name="path">Absolute path to the value inside the document</param>
        /// <param name="defaultValue">Value to return instead, if no value is found at the specified <see cref="path"/></param>
        /// <typeparam name="T">Type to cast the stored object to</typeparam>
        /// <returns>A representation of the value</returns>
        /// <exception cref="Exception">Thrown if the document has no value at the requested <see cref="path"/> and <see cref="defaultValue"/> is set to `null`</exception>
        public T Get<T>(string path, JToken? defaultValue = null)
        {
            try
            {
                // ReSharper disable once PossibleNullReferenceException "False positive: will throw JsonException instead"
                return _documentRoot.SelectToken(path, true)!.ToObject<T>()!;
            }
            catch (Exception)
            {
                if (defaultValue != null)
                {
                    return defaultValue.ToObject<T>()!;
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
        public object Get(string path, Type type, JToken? defaultValue = null)
        {
            try
            {
                // ReSharper disable once PossibleNullReferenceException "False positive: will throw JsonException instead"
                return _documentRoot.SelectToken(path, true)!.ToObject(type)!;
            }
            catch (Exception)
            {
                if (defaultValue != null)
                {
                    return defaultValue.ToObject(type)!;
                }
                throw;
            }
        }

        public static IEnumerable<string> GetKeysFromPath(string path)
        { 
            string[] allPathParts = path.Split('.');
            List<string> pathParts = new List<string>();
            List<string> newElements = new List<string>(allPathParts.Length);
            foreach (string key in allPathParts)
            {
                int openIndex = key.IndexOf("[", StringComparison.Ordinal);
                if (openIndex != -1)
                {
                    Debug.Assert(key.IndexOf("]", StringComparison.Ordinal) != -1, $"Key '{key}' of path '{path}' starts with [ but has no ]");
                    newElements.Add($"{string.Join(".", pathParts)}.{key.Substring(0, openIndex)}");
                }

                pathParts.Add(key);
                newElements.Add(string.Join(".", pathParts));
            }

            return newElements;
        }

        public static IEnumerable<string> GetKeysFromJToken(JToken jToken)
        {
            switch (jToken.Type)
            {
                case JTokenType.Object:
                    JObject jObject = (JObject) jToken;
                    IEnumerable<string> nestedObjects = jObject.Properties().Where(property => property.Value.Type == JTokenType.Object).SelectMany(subProperties => subProperties.Value.Values()).Select(GetKeysFromJToken).ToList().SelectMany(key => key);
                    IEnumerable<string> nestedArrays = jObject.Properties().Where(property => property.Value.Type == JTokenType.Array).SelectMany(subProperties => subProperties.Value.Values()).Select(GetKeysFromJToken).ToList().SelectMany(key => key);
                    IEnumerable<string> directKeys = jObject.Properties().Select(property => property.Path);
                    IEnumerable<string> nestedArrayKeys = jObject.Properties().Where(property => property.Value.Type == JTokenType.Array).SelectMany(subProperties => subProperties.Values()).Where(value => value.Type == JTokenType.Object).Select(objectValue => objectValue.Path);
                    IEnumerable<string> results = nestedObjects.Concat(nestedArrays).Concat(directKeys).Concat(nestedArrayKeys);

                    if (!string.IsNullOrEmpty(jToken.Path))
                    {
                        results = results.Append(jToken.Path);
                    }

                    return results;
                case JTokenType.Array:
                    return ((JArray)jToken).SelectMany(GetKeysFromJToken);
                default:
                    if (!string.IsNullOrEmpty(jToken.Path))
                    {
                        return new[]{string.Join(".", jToken.Path) };
                    }

                    return Array.Empty<string>();
            }
        }

        /// <summary>
        /// Sets or overwrites a value at <see cref="path"/> inside the document
        /// </summary>
        /// <param name="path">Absolute path to the value to create or overwrite</param>
        /// <param name="value">Value to start at <see cref="path"/></param>
        public void Set<T>(string path, T value)
        {
            JToken valueAsJToken = JToken.FromObject(value!);
            List<string> splitPath = path.Split('.').ToList();
            string currentPath = "";
            List<(string parentPath, Type valueType)> parentUpdates = new List<(string parentPath, Type valueType)>();
            foreach (string keyInPath in splitPath)
            {
                int arraySize = 0;
                int indexOfArrayOpenBracket = -1;
                if (keyInPath.Contains("["))
                {
                    indexOfArrayOpenBracket = keyInPath.IndexOf("[", StringComparison.Ordinal);
                    int indexOfArrayClosingBracket = keyInPath.IndexOf("]", StringComparison.Ordinal);
                    bool canParseInt = int.TryParse(keyInPath.Substring(indexOfArrayOpenBracket+1, keyInPath.Length - indexOfArrayClosingBracket), out arraySize);
                    ++arraySize;
                    Debug.Assert(canParseInt, $"Unable to parse index operator of '{keyInPath}' of '{path}'");
                    bool hasClosingBracket = keyInPath.Contains("]");
                    Debug.Assert(hasClosingBracket, $"Found [ but no matching ] inside path '{path}'");
                }
                currentPath += keyInPath;
                JToken? tokenAtPath = _documentRoot.SelectToken(currentPath);
                if (tokenAtPath == null)
                {
                    List<string> list = currentPath.Split('.').ToList();
                    string currentKey = list[list.Count - 1];
                    if (arraySize != 0)
                    {
                        currentKey = currentKey.Substring(0, indexOfArrayOpenBracket);
                    }
                    list.RemoveAt(list.Count - 1);
                    _documentRoot.SelectToken(string.Join(".", list))![currentKey] = arraySize == 0 ? new JObject() : new JArray(Enumerable.Range(0, arraySize).Select(i => new JObject())) as JToken;
                    tokenAtPath = _documentRoot.SelectToken(string.Join(".", list.Append(currentKey)));
                }

                switch (tokenAtPath!.Type)
                {
                    case JTokenType.Object:
                        parentUpdates.Add((currentPath, typeof(JObject)));
                        currentPath += ".";
                        continue;
                    case JTokenType.Array:
                        parentUpdates.Add((currentPath, typeof(JArray)));
                        currentPath += ".";
                        continue;
                }

                if (currentPath == path && tokenAtPath.Type == valueAsJToken.Type)
                {
                    continue;
                }
                throw new NotSupportedException($"'{tokenAtPath.Path}' is of type '{tokenAtPath.Type}' not of '{valueAsJToken.Type}'; setting '{tokenAtPath.Path}' would replace this value to a different type implicitly. To change types, delete this value first.");
            }

            parentUpdates = parentUpdates.Where(parentUpdate => parentUpdate.parentPath != path).ToList();

            JToken? tokenOfPathInDocument = _documentRoot.SelectToken(path);
            tokenOfPathInDocument!.Replace(valueAsJToken);

            foreach ((string parentPath, Type valueType) in parentUpdates)
            {
                InformSubscribersForPath(parentPath, _documentRoot.SelectToken(parentPath)!, valueType);
            }

            InformSubscribersForPath(path, valueAsJToken, typeof(T));
        }

        /// <summary>
        /// Removes an object and all it's parents 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool Delete(string? path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("'path' cannot be empty or null", path);
            }

            JToken? child = _documentRoot.SelectToken(path!);
            if (child == null)
            {
                return false;
            }

            var splitPath = path!.Split('.').ToList();
            var lastIndex = splitPath.Count - 1;
            var childKey = splitPath[lastIndex];
            splitPath.RemoveAt(lastIndex);

            JToken? parent = _documentRoot;
            if (splitPath.Any())
            {
                parent = _documentRoot.SelectToken(string.Join(".", splitPath), false);
            }

            InformSubscribersForPathDeletion(path!);
            ((JObject)parent).Remove(childKey);
            return true;
        }

        private void InformSubscribersForPathDeletion(string path)
        {
            IEnumerable<string> subscriptionPathsToInform = new[] { path };

            IEnumerable<KeyValuePair<string, Dictionary<Type, ISubscriptionCollection>>> typeSpecificSubscribersToInform = _typeSpecificSubscriptions.Where(keyValue => subscriptionPathsToInform.Contains(keyValue.Key));
            foreach (KeyValuePair<string, Dictionary<Type, ISubscriptionCollection>> typeSpecificSubscribersByPath in typeSpecificSubscribersToInform)
            {
                foreach (KeyValuePair<Type, ISubscriptionCollection> keyValuePair in typeSpecificSubscribersByPath.Value)
                {
                    keyValuePair.Value.CallSubscriptions(null);
                }
            }

            IEnumerable<KeyValuePair<string, SubscriptionCollection<JToken>>> typeAgnosticSubscribersToInform = _typeAgnosticSubscriptions.Where(keyValue => subscriptionPathsToInform.Contains(keyValue.Key));
            foreach (KeyValuePair<string, SubscriptionCollection<JToken>> keyValuePair in typeAgnosticSubscribersToInform)
            {
                keyValuePair.Value.CallSubscriptions(null);
            }
        }

        private void InformSubscribersForPath(string path, JToken valueAsJToken, Type valueType)
        {
            void InformTypeSpecificSubscribersForPath(string innerPath, JToken innerValueAsJToken, Type innerValueType)
            {
                if (!_typeSpecificSubscriptions.ContainsKey(innerPath))
                {
                    return;
                }

                foreach (KeyValuePair<Type, ISubscriptionCollection> subscriberCollectionByType in _typeSpecificSubscriptions[innerPath])
                {
                    if (subscriberCollectionByType.Value.Count == 0)
                    {
                        continue;
                    }
                    object? attemptedCast = null;
                    try
                    {
                        attemptedCast = innerValueAsJToken.ToObject(subscriberCollectionByType.Key);
                    }
                    catch (Exception)
                    {
                        // any exception during object conversion is acceptable, so we need to suppress them to continue execution
                    }

                    if (attemptedCast == null)
                    {
                        Debug.LogWarning($"{subscriberCollectionByType.Value.Count} subscribers of path '{innerPath}' are of '{subscriberCollectionByType.Key}', but the current value cannot be cast to it, as it's of type '{innerValueType}'");
                        continue;
                    }
                    _typeSpecificSubscriptions[innerPath][subscriberCollectionByType.Key].CallSubscriptions(innerValueAsJToken.ToObject(subscriberCollectionByType.Key));
                }
            }


            void InformTypeAgnosticSubscribersForPath(string innerPath, JToken innerValueAsJToken)
            {
                if (!_typeAgnosticSubscriptions.ContainsKey(innerPath))
                {
                    return;
                }
                _typeAgnosticSubscriptions[innerPath].CallSubscriptions(innerValueAsJToken);
            }

            InformTypeSpecificSubscribersForPath(path, valueAsJToken, valueType);
            InformTypeAgnosticSubscribersForPath(path, valueAsJToken);
        }

        /// <summary>
        /// Attempts to unsubscribe a specific action from a data binding path
        /// </summary>
        /// <param name="path">Absolute data binding path</param>
        /// <param name="action">Specific action to remove</param>
        /// <returns>True, if the action was previously subscribe and has been successfully removed. False, if the action was never subscribed to this path.</returns>
        public bool Unsubscribe<T>(string path, Action<T> action)
        {
            if (!_typeSpecificSubscriptions.ContainsKey(path))
            {
                return false;
            }

            return ((SubscriptionCollection<T>)_typeSpecificSubscriptions[path][typeof(T)]).Remove(action);
        }

        /// <summary>
        /// Attempts to unsubscribe a specific action from a data binding path
        /// </summary>
        /// <param name="path">Absolute data binding path</param>
        /// <param name="action">Specific action to remove</param>
        /// <returns>True, if the action was previously subscribe and has been successfully removed. False, if the action was never subscribed to this path.</returns>
        public bool Unsubscribe(string path, Action<JToken> action)
        {
            if (!_typeAgnosticSubscriptions.ContainsKey(path))
            {
                return false;
            }

            return _typeAgnosticSubscriptions[path].Remove(action);
        }
    }
}
