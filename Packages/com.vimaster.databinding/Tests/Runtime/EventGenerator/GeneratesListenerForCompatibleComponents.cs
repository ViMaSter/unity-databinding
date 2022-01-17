using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Tests.DataBinding.EventGenerator
{
    public class GeneratesListenerForCompatibleComponents : MonoBehaviour
    {
        [UnityTest]
        public IEnumerator ComponentCountChanges()
        {
            var gameObject = new GameObject();
            gameObject.AddComponent<Button>();
            var initialComponentCount = gameObject.GetComponents<Component>().Length;
            gameObject.AddComponent<global::DataBinding.EventGeneration.EventGenerator>();
            yield return new WaitForEndOfFrame();
            Assert.AreEqual(initialComponentCount + 2, gameObject.GetComponents<Component>().Length);
        }
    }
}
