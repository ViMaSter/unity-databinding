using UnityEngine;

namespace Tests.AutomatedSubscriber
{
    public class ExampleTargetComponent : MonoBehaviour
    {
        // Fields
        private float _privateField = -1.0f;
        public float PublicField = -1.0f;
        [SerializeField] private float _privateSerializedField = -1.0f;

        // Properties
        private float PrivateProperty { get; set; } = -1.0f;
        public float PublicProperty { get; set; } = -1.0f;
        [field: SerializeField] private float PrivateSerializedProperty { get; set; } = -1.0f;

        public float GetPrivateField() => _privateField;
        public float GetPrivateProperty() => PrivateProperty;
        public float GetPrivateSerializedField() => _privateSerializedField;
        public float GetPrivateSerializedProperty() => PrivateSerializedProperty;
    }
}