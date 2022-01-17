using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace DataBinding
{
    /// <summary>
    /// Automatically bind all fields and properties exposed to the Inspector of a target object to a key of a data binding document
    /// </summary>
    [DefaultExecutionOrder(-101)]
    public class AutomatedSubscriber : SupportsRelativeDocumentPath
    {
        public Component TargetComponent;
        private readonly List<Tuple<string, Action<JToken>>> _activeSubscriptions = new List<Tuple<string, Action<JToken>>>();
        private readonly List<Type> _supportedTypes = new List<Type> { typeof(string), typeof(Vector2), typeof(Vector3), typeof(Rect)};

        public void Start()
        {
            Debug.Assert(Document != null, "AutomatedSubscriber has no document set and thereby cannot subscribe to data binding");
            Debug.Assert(!string.IsNullOrEmpty(DocumentPath), "AutomatedSubscriber cannot have an empty string as key root");
            Debug.Assert(TargetComponent != null, "AutomatedSubscriber requires a target object to reflect changes in data binding to");

            // gather all fields and properties that are visible when the Inspector window is viewing the _targetObject
            // and subscribe to their respective keys of the Document

            // fields that are public
            var publicFields = TargetComponent.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public).Where(a => a.FieldType.IsPrimitive || a.FieldType == typeof(string)).ToList();
            foreach (var field in publicFields)
            {
                var absoluteKey = $"{DocumentPath}.{field.Name}";
                _activeSubscriptions.Add(new Tuple<string, Action<JToken>>($"{DocumentPath}.{field.Name}", Document.Subscribe(absoluteKey, token => {
                    field.SetValue(TargetComponent, token.ToObject(field.FieldType));
                })));
            }

            // special edge-case for Unity-internal Behaviors that exist only in native code
            var externalProperties = TargetComponent.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(prop => prop.CanWrite).Where(prop => publicFields.All(field => !field.Name.Contains(prop.Name))).Where(a => a.PropertyType.IsPrimitive || _supportedTypes.Contains(a.PropertyType)).Where(a => a.DeclaringType == a.ReflectedType).ToList();
            foreach (var property in externalProperties)
            {
                var absoluteKey = $"{DocumentPath}.{property.Name}";
                _activeSubscriptions.Add(new Tuple<string, Action<JToken>>(absoluteKey, Document.Subscribe(absoluteKey, token => {
                    property.SetValue(TargetComponent, token.ToObject(property.PropertyType));
                })));
            }

            var privateFieldsWithSerializeFieldAttribute = TargetComponent.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic).Where(a => a.GetCustomAttributes().Any(attr => attr is SerializeField)).Where(a => a.FieldType.IsPrimitive || _supportedTypes.Contains(a.FieldType)).ToList();

            // fields that are private but have the [SerializeField] attribute
            var privateFieldsWithSerializeFieldAttributeWithoutBackingFields = privateFieldsWithSerializeFieldAttribute.Where(a => !a.Name.Contains("k__BackingField")).ToList();
            foreach (var field in privateFieldsWithSerializeFieldAttributeWithoutBackingFields)
            {
                var absoluteKey = $"{DocumentPath}.{field.Name}";
                _activeSubscriptions.Add(new Tuple<string, Action<JToken>>($"{DocumentPath}.{field.Name}", Document.Subscribe(absoluteKey, token => {
                    field.SetValue(TargetComponent, token.ToObject(field.FieldType));
                })));
            }

            // properties that are private but have the [SerializeField] attribute
            var privatePropertiesWithSerializeFieldAttribute = privateFieldsWithSerializeFieldAttribute.Where(a => a.Name.Contains("k__BackingField")).Select(a => TargetComponent.GetType().GetProperty(a.Name.Substring(1, a.Name.Length - 17), BindingFlags.NonPublic | BindingFlags.Instance)).ToList();
            foreach (var property in privatePropertiesWithSerializeFieldAttribute)
            {
                var absoluteKey = $"{DocumentPath}.{property.Name}";
                _activeSubscriptions.Add(new Tuple<string, Action<JToken>>(absoluteKey, Document.Subscribe(absoluteKey, token => {
                    property.SetValue(TargetComponent, token.ToObject(property.PropertyType));
                })));
            }
        }

        private void OnDisable()
        {
            foreach (var (key, callback) in _activeSubscriptions)
            {
                Document.Unsubscribe(key, callback);
            }
        }
    }
}