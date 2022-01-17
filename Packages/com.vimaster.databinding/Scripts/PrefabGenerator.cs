using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace DataBinding
{
    /// <summary>
    /// Generates instances of prefabs and binds them data based on the amount of elements in an array
    /// </summary>
    [DefaultExecutionOrder(-103)]
    public class PrefabGenerator : SupportsRelativeDocumentPath
    {
        public GameObject Prefab;

        private readonly Dictionary<int, GameObject> _instantiatedGameObjects = new Dictionary<int, GameObject>();

        public void Start()
        {
            Document.Subscribe<JArray>($"{DocumentPath}", LengthChanged);
        }

        private void LengthChanged(JArray array)
        {
            var newLength = array.Count;
            var currentLength = _instantiatedGameObjects.Count;
            if (currentLength > newLength)
            {
                for (var i = newLength; i < currentLength; ++i)
                {
                    Destroy(_instantiatedGameObjects[i]);
                    _instantiatedGameObjects.Remove(i);
                }
                return;
            }

            if (currentLength < newLength)
            {
                for (var i = currentLength; i < newLength; ++i)
                {
                    var absolutePathOfPrefab = $"{DocumentPath}[{i}]";
                    _instantiatedGameObjects[i] = Instantiate(Prefab, Vector3.zero, Quaternion.identity, transform);
                    _instantiatedGameObjects[i].name = absolutePathOfPrefab;
                    foreach (var componentThatSupportsRelativeDocumentPath in _instantiatedGameObjects[i].GetComponentsInChildren<SupportsRelativeDocumentPath>())
                    {
                        componentThatSupportsRelativeDocumentPath.PropagateParentSettings(Document, absolutePathOfPrefab);
                    }
                }
            }
        }

        private void OnDestroy()
        {
            for (var i = 0; i < _instantiatedGameObjects.Count; i++)
            {
                Destroy(_instantiatedGameObjects[i]);
            }
        }
    }
}