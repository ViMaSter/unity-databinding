using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.AutomatedSubscriber
{
    public class CameraSubscriber : MonoBehaviour
    {
        [UnityTest]
        public IEnumerator UpdatesAllFieldsExposedToInspector_OnUnityInternalComponents()
        {
            const string path = "camera";
            var newGameObject = new GameObject();
            var document = newGameObject.AddComponent<global::DataBinding.Document>();

            var newCamera = newGameObject.AddComponent<Camera>();
            var automatedSubscriber = newGameObject.AddComponent<global::DataBinding.AutomatedSubscriber>();

            automatedSubscriber.Document = document;
            automatedSubscriber.TargetComponent = newCamera;
            automatedSubscriber.DocumentPath = path;

            yield return new WaitForEndOfFrame();

            var oldFOV = newCamera.fieldOfView;
            var newFOV = oldFOV + 1;

            document.Set($"{path}.{nameof(Camera.fieldOfView)}", newFOV);
            Assert.AreEqual(newFOV, newCamera.fieldOfView);
        }

        [UnityTest]
        public IEnumerator UpdatesAllFieldsExposedToInspector_OnCustomComponents()
        {
            const string path = "root";
            var values = new { 
                _privateField = 1f,
                PublicField = 2f,
                _privateSerializedField = 3f,

                // Properties
                PrivateProperty = 4f,
                PublicProperty = 5f,
                PrivateSerializedProperty = 6f
            };

            var newGameObject = new GameObject();
            var document = newGameObject.AddComponent<global::DataBinding.Document>();

            var exampleTargetComponent = newGameObject.AddComponent<ExampleTargetComponent>();
            var automatedSubscriber = newGameObject.AddComponent<global::DataBinding.AutomatedSubscriber>();

            automatedSubscriber.Document = document;
            automatedSubscriber.TargetComponent = exampleTargetComponent;
            automatedSubscriber.DocumentPath = path;

            yield return new WaitForEndOfFrame();

            document.Set(path, values);

            Assert.AreNotEqual(values._privateField, exampleTargetComponent.GetPrivateField());
            Assert.AreEqual(values.PublicField, exampleTargetComponent.PublicField);
            Assert.AreEqual(values._privateSerializedField, exampleTargetComponent.GetPrivateSerializedField());
            Assert.AreNotEqual(values.PrivateProperty, exampleTargetComponent.GetPrivateProperty());
            Assert.AreEqual(values.PublicProperty, exampleTargetComponent.PublicProperty);
            Assert.AreEqual(values.PrivateSerializedProperty, exampleTargetComponent.GetPrivateSerializedProperty());
        }
    }
}
