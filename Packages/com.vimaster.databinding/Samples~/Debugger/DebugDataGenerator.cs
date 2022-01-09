using DataBinding;
using UnityEngine;

public class DebugDataGenerator : MonoBehaviour
{
    public Document Document;

    private void Start()
    {
        var childObject = new { d = 1, e = 2.0f, f = "3" };
        var parentObject = new object[] { childObject, childObject, childObject };
        Document.Set("data", new
        {
            a = new
            {
                b = new
                {
                    c = parentObject
                },
                d = new
                {
                    c = parentObject
                },
                e = new
                {
                    c = parentObject
                },
                f = new
                {
                    c = parentObject
                },
                g = new
                {
                    c = parentObject
                },
                h = new
                {
                    c = parentObject
                }
            }
        });
    }
}
