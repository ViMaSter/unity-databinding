using System;
using System.Linq;
using UnityEngine;

namespace DataBinding.Examples.Prefabs
{
    public class UpdateCount : MonoBehaviour
    {
        public class DataSet
        {
            public class LabelValues
            {
                // ReSharper disable once InconsistentNaming Required for automated component property update
                public string text = "0";
            }
            public class AnchoredPositionValues
            {
                public class Rect
                {
                    // ReSharper disable once InconsistentNaming Required for automated component property update
                    public int y;
                }
                // ReSharper disable once InconsistentNaming Required for automated component property update
                public Rect anchoredPosition = new Rect();
            }

            public AnchoredPositionValues Rect = new AnchoredPositionValues();
            public LabelValues Label = new LabelValues();

            public DataSet(int index)
            {
                Label.text = index.ToString();
                Rect.anchoredPosition.y = index * -15;
            }
        }

        [SerializeField] private Document _document;
        [SerializeField] private string _keyRoot;
        private int _currentCount;

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                ++_currentCount;
                _document.Set(_keyRoot, Enumerable.Range(0, _currentCount).Select(i => new DataSet(i)));
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                --_currentCount;
                _currentCount = Math.Max(0, _currentCount);
                _document.Set(_keyRoot, Enumerable.Range(0, _currentCount).Select(i => new DataSet(i)));
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (_currentCount == 0)
                {
                    return;
                }

                var labelAsInt = int.Parse(_document.Get<string>($"{_keyRoot}[{_currentCount - 1}].label.text"));
                ++labelAsInt;

                _document.Set($"{_keyRoot}[{_currentCount - 1}].{nameof(DataSet.Label)}.{nameof(DataSet.LabelValues.text)}", labelAsInt.ToString());
                _document.Set($"{_keyRoot}[{_currentCount - 1}].{nameof(DataSet.Rect)}.{nameof(DataSet.Rect.anchoredPosition)}.{nameof(DataSet.Rect.anchoredPosition.y)}", labelAsInt*-15);
            }
        }
    }
}
