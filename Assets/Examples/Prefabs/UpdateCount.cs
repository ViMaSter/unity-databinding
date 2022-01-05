using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataBinding;
using UnityEngine;

public class UpdateCount : MonoBehaviour
{
    public class DataSet
    {
        public class Label
        {
            public string text = "0";
        }
        public class AnchoredPosition
        {
            public class Rect
            {
                public int y;
            }
            public Rect anchoredPosition = new Rect();
        }

        public AnchoredPosition rect = new AnchoredPosition();
        public Label label = new Label();

        public DataSet(int index)
        {
            label.text = index.ToString();
            rect.anchoredPosition.y = index * -15;
        }
    }

    [SerializeField] private Document _document;
    [SerializeField] private string _keyRoot;
    private int currentCount = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ++currentCount;
            _document.Set(_keyRoot, Enumerable.Range(0, currentCount).Select(i => new DataSet(i)));
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            --currentCount;
            currentCount = Math.Max(0, currentCount);
            _document.Set(_keyRoot, Enumerable.Range(0, currentCount).Select(i => new DataSet(i)));
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentCount == 0)
            {
                return;
            }

            var labelAsInt = int.Parse(_document.Get<string>($"{_keyRoot}[{(currentCount - 1)}].label.text"));
            ++labelAsInt;

            _document.Set($"{_keyRoot}[{currentCount - 1}].{nameof(DataSet.label)}.{nameof(DataSet.Label.text)}", labelAsInt.ToString());
            _document.Set($"{_keyRoot}[{currentCount - 1}].{nameof(DataSet.rect)}.{nameof(DataSet.rect.anchoredPosition)}.{nameof(DataSet.rect.anchoredPosition.y)}", labelAsInt*-15);
        }
    }
}
