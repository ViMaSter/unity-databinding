using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DataBinding;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.DataBinding.PrefabGenerator
{
    public class UpdatesRelativePaths : MonoBehaviour
    {
        private static object[] _relevantTypes = 
            typeof(SupportsRelativeDocumentPath).Assembly.GetTypes()
                .Where(type => type.IsSubclassOf(typeof(SupportsRelativeDocumentPath)))
                .Select(entry => new TestCaseData(entry).Returns(null))
                .ToArray();

        private const string PATH = "documentPath";

        [TearDown]
        public void TearDown()
        {
            FindObjectsOfType<GameObject>().ToList().ForEach(DestroyImmediate);
        }

        [UnityTest]
        [TestCaseSource(nameof(_relevantTypes))]
        public IEnumerator IsUpdatedOnPrefabInstantiation(Type type)
        {
            var containingGameObject = new GameObject();
            var document = containingGameObject.AddComponent<global::DataBinding.Document>();
            var generator = containingGameObject.AddComponent<global::DataBinding.PrefabGenerator>();

            var prefab = Resources.Load<GameObject>($"{type.Name}Prefab");
            generator.Prefab = prefab;
            generator.DocumentPath = PATH;
            generator.Document = document;

            const int childCount = 3;
            var allChildData = new List<object>(childCount);

            for (var i = 0; i < childCount; i++)
            {
                allChildData.Add(new { text = i.ToString() });
                Assert.IsNull(GameObject.Find($"{generator.DocumentPath}[{i}]"));
            }

            yield return new WaitForEndOfFrame();

            // initially create childCount objects
            document.Set(generator.DocumentPath, allChildData);

            yield return new WaitForEndOfFrame();

            for (var i = 0; i < childCount; i++)
            {
                var childPath = $"{generator.DocumentPath}[{i}]";
                var childObject = GameObject.Find(childPath);
                var component = (SupportsRelativeDocumentPath)childObject.GetComponent(type);
                Assert.AreEqual(childPath, component.DocumentPath);
            }
        }
    }
}