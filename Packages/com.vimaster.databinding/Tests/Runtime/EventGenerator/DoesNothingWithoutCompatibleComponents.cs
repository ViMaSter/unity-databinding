using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.DataBinding.EventGenerator
{
    public class DoesNothingWithoutCompatibleComponents : MonoBehaviour
    {
        [UnityTest]
        public IEnumerator ComponentCountDoesNotChanges()
        {
            var gameObject = new GameObject();
            var initialComponentCount = gameObject.GetComponents<Component>().Length;
            gameObject.AddComponent<global::DataBinding.EventGeneration.EventGenerator>();
            yield return new WaitForEndOfFrame();
            Assert.AreEqual(initialComponentCount + 1, gameObject.GetComponents<Component>().Length);
        }
    }
}