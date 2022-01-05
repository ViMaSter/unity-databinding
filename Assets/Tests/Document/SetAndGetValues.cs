using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEngine;

namespace Tests.DataBinding
{
    namespace Document
    {
        public class SetAndGetValues
        {
            public class TestValue
            {
                // ReSharper disable UnusedMember.Global
                public float FloatValue = 1.0f;
                public int IntValue = 1;
                public string StringValue = "1.0";
                public object[] ArrayValue = { 1.0f, 1, "1.0" };
                public object ObjectValue = new { primitiveValueA = 1.0f, primitiveValueB = 1, primitiveValueC = "1.0" };
                // ReSharper restore UnusedMember.Global
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
                .Concat(PrimitiveValues.Select(primitiveValue => new[] { primitiveValue, primitiveValue, primitiveValue }))
                .Concat(PrimitiveValues.Select(primitiveValue => new { primitiveValueA = primitiveValue, primitiveValueB = primitiveValue, primitiveValueC = primitiveValue }));
            private static readonly IEnumerable<object[]> TestCaseSource = Paths.SelectMany(path => Values.Select(value => new[] { path, value }));

            [Test]
            [TestCaseSource(nameof(TestCaseSource))]
            public void WorksWithVariousObjects(string path, object value)
            {
                GameObject gameObject = new GameObject();
                global::DataBinding.Document document = gameObject.AddComponent<global::DataBinding.Document>();
                document.Set(path, value);
                Assert.AreEqual(JsonConvert.SerializeObject(value), JsonConvert.SerializeObject(document.Get(path, value.GetType())));
            }
        }
    }
}
