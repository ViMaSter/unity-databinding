using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Tests.DataBinding
{
    public class DocumentTests
    {
        public class TestValue
        {
            public float FloatValue = 1.0f;
            public int IntValue = 1;
            public string StringValue = "1.0";
            public object[] ArrayValue = { 1.0f, 1, "1.0" };
            public object ObjectValue = new { primitiveValueA = 1.0f, primitiveValueB = 1, primitiveValueC = "1.0" };
            public TestValue SelfValue;
            public TestValue(TestValue selfValue)
            {
                SelfValue = selfValue;
            }
        }

        private static readonly List<string> Paths = new List<string> {
            "test",
            "test.nested",
            "test.nested[0].array"
        };
        private static readonly List<object> PrimitiveValues = new List<object> {
            1.0f,
            1,
            "1.0",
            new object[]{1.0f, 1, "1.0"},
            new {primitiveValueA = 1.0f, primitiveValueB = 1, primitiveValueC = "1.0"},
            new TestValue(new TestValue(null))
        };

        private static readonly IEnumerable<object> Values = PrimitiveValues
            .Concat(PrimitiveValues.Select(primitiveValue => new[] {primitiveValue, primitiveValue, primitiveValue}))
            .Concat(PrimitiveValues.Select(primitiveValue => new {primitiveValueA = primitiveValue, primitiveValueB = primitiveValue, primitiveValueC = primitiveValue }));

        private static readonly IEnumerable<object[]> TestCaseSource = Paths.SelectMany(path => Values.Select(value => new object[]{path, value}));

        [Test]
        [TestCaseSource(nameof(TestCaseSource))]
        public void SetAndGetValues(string path, object value)
        {
            GameObject gameObject = new GameObject();
            global::DataBinding.Document document = gameObject.AddComponent<global::DataBinding.Document>();
            document.Set(path, value);
            Assert.AreEqual(JsonConvert.SerializeObject(value), JsonConvert.SerializeObject(document.Get(path, value.GetType())));
        }

        [Test]
        public void GetKeysFromJToken_PrintsAllPaths()
        {
            const string prefix = "object.array[4].subObject";
            object newValueObject = new {
                stringValue = "abc",
                intArray = new int[] {
                    1, 2, 3
                },
                objectArray = new object[] {
                    new {
                        a = 1
                    },
                    new {
                        b = 2
                    },
                    new {
                        c = 3
                    }
                }
            };
            var actual = global::DataBinding.Document.GetKeysFromJToken(JToken.FromObject(newValueObject), prefix);
            var expected = new string[] {
                "object",
                "object.array",
                "object.array[4]",
                "object.array[4].subObject",
                "object.array[4].subObject.stringValue",
                "object.array[4].subObject.intArray",
                "object.array[4].subObject.intArray[0]",
                "object.array[4].subObject.intArray[1]",
                "object.array[4].subObject.intArray[2]",
                "object.array[4].subObject.objectArray",
                "object.array[4].subObject.objectArray[0]",
                "object.array[4].subObject.objectArray[0].a",
                "object.array[4].subObject.objectArray[1]",
                "object.array[4].subObject.objectArray[1].b",
                "object.array[4].subObject.objectArray[2]",
                "object.array[4].subObject.objectArray[2].c",
            };
            Assert.AreEqual(expected.OrderBy(value=>value), actual.OrderBy(value=>value));
        }

    }
}
