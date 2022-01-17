using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.DataBinding.PrefabGenerator
{
    public class ManageGameObjects : MonoBehaviour
    {
        private const string PATH = "childrenPathExample";
        private const string PREFAB_NAME = "ChildExample";

        [TearDown]
        public void TearDown()
        {
            var a = FindObjectsOfType<GameObject>().ToList();
            a.ForEach(b => {

                try
                {
                    DestroyImmediate(b);

                }
                catch (NullReferenceException e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            });
        }

        [UnityTest]
        public IEnumerator CreatesAndDeletesGameObjectsAsNeeded()
        {
            var containingGameObject = new GameObject();
            var document = containingGameObject.AddComponent<global::DataBinding.Document>();
            var generator = containingGameObject.AddComponent<global::DataBinding.PrefabGenerator>();

            var prefab = Resources.Load<GameObject>(PREFAB_NAME);
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
                Assert.AreEqual(childPath, childObject.GetComponent<global::DataBinding.AutomatedSubscriber>().DocumentPath);
                Assert.AreEqual(i.ToString(), childObject.GetComponent<UnityEngine.UI.Text>().text);
            }

            // delete the last object
            var reducedChildData = new List<object>(allChildData);
            reducedChildData.RemoveAt(childCount - 1);
            document.Set(generator.DocumentPath, reducedChildData);
            yield return new WaitForEndOfFrame();

            for (var i = 0; i < childCount - 1; i++)
            {
                var childPath = $"{generator.DocumentPath}[{i}]";
                var childObject = GameObject.Find(childPath);
                Assert.IsNotNull(GameObject.Find(childPath));
                Assert.AreEqual(childPath, GameObject.Find(childPath).GetComponent<global::DataBinding.AutomatedSubscriber>().DocumentPath);
                Assert.AreEqual(i.ToString(), GameObject.Find(childPath).GetComponent<UnityEngine.UI.Text>().text);
            }

            Assert.IsNull(GameObject.Find($"{generator.DocumentPath}[{childCount - 1}]"));

            // re-create the last object
            document.Set(generator.DocumentPath, allChildData);
            yield return new WaitForEndOfFrame();

            for (var i = 0; i < childCount; i++)
            {
                var childPath = $"{generator.DocumentPath}[{i}]";
                var childObject = GameObject.Find(childPath);
                Assert.AreEqual(childPath, childObject.GetComponent<global::DataBinding.AutomatedSubscriber>().DocumentPath);
                Assert.AreEqual(i.ToString(), childObject.GetComponent<UnityEngine.UI.Text>().text);
            }

            // delete the last object again
            document.Set(generator.DocumentPath, reducedChildData);
            yield return new WaitForEndOfFrame();

            for (var i = 0; i < childCount - 1; i++)
            {
                var childPath = $"{generator.DocumentPath}[{i}]";
                var childObject = GameObject.Find(childPath);
                Assert.AreEqual(childPath, childObject.GetComponent<global::DataBinding.AutomatedSubscriber>().DocumentPath);
                Assert.AreEqual(i.ToString(), childObject.GetComponent<UnityEngine.UI.Text>().text);
            }

            Assert.IsNull(GameObject.Find($"{generator.DocumentPath}[{childCount - 1}]"));
        }

        [UnityTest]
        public IEnumerator DeletesGeneratedGameObjectsPrefabGeneratorIsDeleted()
        {
            var containingGameObject = new GameObject();
            var document = containingGameObject.AddComponent<global::DataBinding.Document>();
            var generator = containingGameObject.AddComponent<global::DataBinding.PrefabGenerator>();

            var prefab = Resources.Load<GameObject>(PREFAB_NAME);
            generator.Prefab = prefab;
            generator.DocumentPath = PATH;
            generator.Document = document;

            const int childCount = 3;
            var allChildData = new List<object>(childCount);

            for (var i = 0; i < childCount; i++)
            {
                allChildData.Add(new { text = i.ToString() });
            }

            yield return new WaitForEndOfFrame();

            document.Set(PATH, allChildData);

            yield return new WaitForEndOfFrame();

            for (var i = 0; i < childCount; i++)
            {
                var childPath = $"{PATH}[{i}]";
                var childObject = GameObject.Find(childPath);
                Assert.AreEqual(childPath, childObject.GetComponent<global::DataBinding.AutomatedSubscriber>().DocumentPath);
                Assert.AreEqual(i.ToString(), childObject.GetComponent<UnityEngine.UI.Text>().text);
            }

            Destroy(generator);

            yield return new WaitForFixedUpdate();
            yield return new WaitForEndOfFrame();

            for (var i = 0; i < childCount; i++)
            {
                var childPath = $"{PATH}[{i}]";
                Assert.IsNull(GameObject.Find(childPath));
            }

            // delete the last object
            var reducedChildData = new List<object>(allChildData);
            reducedChildData.RemoveAt(childCount - 1);
            document.Set(PATH, reducedChildData);

            yield return new WaitForEndOfFrame();

            for (var i = 0; i < childCount; i++)
            {
                var childPath = $"{PATH}[{i}]";
                Assert.IsNull(GameObject.Find(childPath));
            }
        }
    }
}