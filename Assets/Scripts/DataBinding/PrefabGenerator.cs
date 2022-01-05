using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace DataBinding
{
    /// <summary>
    /// Generates instances of prefabs and binds them data based on the amount of elements in an array
    /// </summary>
    [DefaultExecutionOrder(-103)]
    public class PrefabGenerator : MonoBehaviour
    {
        [SerializeField]private Document _document;
        [SerializeField]private string _keyRoot;
        [SerializeField]private GameObject _prefab;

        private readonly Dictionary<int, GameObject> _instantiatedGameObjects = new Dictionary<int, GameObject>();

        public void OnEnable()
        {
            _document.Subscribe<JArray>($"{_keyRoot}", LengthChanged);
        }

        private void LengthChanged(JArray array)
        {
            int newLength = array.Count;
            int currentLength = _instantiatedGameObjects.Count;
            if (currentLength > newLength)
            {
                for (int i = newLength; i < currentLength; ++i)
                {
                    Destroy(_instantiatedGameObjects[i]);
                    _instantiatedGameObjects.Remove(i);
                }
                return;
            }

            if (currentLength < newLength)
            {
                for (int i = currentLength; i < newLength; ++i)
                {
                    string absolutePathOfPrefab = $"{_keyRoot}[{i}]";
                    _instantiatedGameObjects[i] = Instantiate(_prefab, Vector3.zero, Quaternion.identity, transform);
                    _instantiatedGameObjects[i].name = absolutePathOfPrefab;
                    foreach (ReflectedSubscriber reflectedSubscriber in _instantiatedGameObjects[i].GetComponentsInChildren<ReflectedSubscriber>())
                    {
                        reflectedSubscriber.UpdatePrefabKey(_document, absolutePathOfPrefab);
                    }
                }
            }
        }

        private void OnDisable()
        {
            for (int i = 0; i < _instantiatedGameObjects.Count; i++)
            {
                Destroy(_instantiatedGameObjects[i]);
                _instantiatedGameObjects.Remove(i);
            }
        }
    }
}