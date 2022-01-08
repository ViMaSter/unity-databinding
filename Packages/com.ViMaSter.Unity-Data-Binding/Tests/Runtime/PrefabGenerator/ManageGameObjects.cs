using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataBinding;
using DataBinding.EventGeneration;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ManageGameObjects : MonoBehaviour
{
    private const string PATH = "children";

    [TearDown]
    public void TearDown()
    {
        FindObjectsOfType<GameObject>().ToList().ForEach(DestroyImmediate);
    }

    [UnityTest]
    public IEnumerator CreatesAndDeletesGameObjectsAsNeeded()
    {
        var containingGameObject = new GameObject();
        var document = containingGameObject.AddComponent<Document>();
        var generator = containingGameObject.AddComponent<PrefabGenerator>();

        var prefab = Resources.Load<GameObject>("Prefab");
        generator.Prefab = prefab;
        generator.DocumentPath = PATH;
        generator.Document = document;

        const int childCount = 3;
        var allChildData = new List<object>(childCount);

        for (var i = 0; i < childCount; i++)
        {
            allChildData.Add(new {text = i.ToString()});
        }

        for (var i = 0; i < childCount; i++)
        {
            Assert.IsNull(GameObject.Find($"{generator.DocumentPath}[{i}]"));
        }

        yield return new WaitForEndOfFrame();

        // initially create childCount objects
        document.Set(generator.DocumentPath, allChildData);

        yield return new WaitForEndOfFrame();

        for (var i = 0; i < childCount; i++)
        {
            var childPath = $"{generator.DocumentPath}[{i}]";
            Assert.IsNotNull(GameObject.Find(childPath));
            Assert.AreEqual(childPath, GameObject.Find(childPath).GetComponent<AutomatedSubscriber>().DocumentPath);
            Assert.AreEqual(i.ToString(), GameObject.Find(childPath).GetComponent<UnityEngine.UI.Text>().text);
        }

        // delete the last object
        var reducedChildData = new List<object>(allChildData);
        reducedChildData.RemoveAt(childCount-1);
        document.Set(generator.DocumentPath, reducedChildData);
        yield return new WaitForEndOfFrame();

        for (var i = 0; i < childCount-1; i++)
        {
            var childPath = $"{generator.DocumentPath}[{i}]";
            Assert.IsNotNull(GameObject.Find(childPath));
            Assert.AreEqual(childPath, GameObject.Find(childPath).GetComponent<AutomatedSubscriber>().DocumentPath);
            Assert.AreEqual(i.ToString(), GameObject.Find(childPath).GetComponent<UnityEngine.UI.Text>().text);
        }
        Assert.IsNull(GameObject.Find($"{generator.DocumentPath}[{childCount - 1}]"));

        // re-create the last object
        document.Set(generator.DocumentPath, allChildData);
        yield return new WaitForEndOfFrame();

        for (var i = 0; i < childCount; i++)
        {
            var childPath = $"{generator.DocumentPath}[{i}]";
            Assert.IsNotNull(GameObject.Find(childPath));
            Assert.AreEqual(childPath, GameObject.Find(childPath).GetComponent<AutomatedSubscriber>().DocumentPath);
            Assert.AreEqual(i.ToString(), GameObject.Find(childPath).GetComponent<UnityEngine.UI.Text>().text);
        }

        // delete the last object again
        document.Set(generator.DocumentPath, reducedChildData);
        yield return new WaitForEndOfFrame();

        for (var i = 0; i < childCount-1; i++)
        {
            var childPath = $"{generator.DocumentPath}[{i}]";
            Assert.IsNotNull(GameObject.Find(childPath));
            Assert.AreEqual(childPath, GameObject.Find(childPath).GetComponent<AutomatedSubscriber>().DocumentPath);
            Assert.AreEqual(i.ToString(), GameObject.Find(childPath).GetComponent<UnityEngine.UI.Text>().text);
        }
        Assert.IsNull(GameObject.Find($"{generator.DocumentPath}[{childCount - 1}]"));
    }

    [UnityTest]
    public IEnumerator DeletesGeneratedGameObjectsPrefabGeneratorIsDeleted()
    {
        var containingGameObject = new GameObject();
        var document = containingGameObject.AddComponent<Document>();
        var generator = containingGameObject.AddComponent<PrefabGenerator>();

        var prefab = Resources.Load<GameObject>("Prefab");
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
            Assert.IsNotNull(GameObject.Find(childPath));
            Assert.AreEqual(childPath, GameObject.Find(childPath).GetComponent<AutomatedSubscriber>().DocumentPath);
            Assert.AreEqual(i.ToString(), GameObject.Find(childPath).GetComponent<UnityEngine.UI.Text>().text);
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
