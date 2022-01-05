using System.Collections.Generic;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEngine;

namespace Tests.DataBinding.Document
{
    internal class GeneratePrettyPrint
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

        [Test]
        public void IsEqualToSerializedDictionary()
        {
            GameObject gameObject = new GameObject();
            global::DataBinding.Document document = gameObject.AddComponent<global::DataBinding.Document>();

            TestValue testValue = new TestValue(new TestValue(null));

            document.Set(nameof(testValue), testValue);
            var expected = JsonConvert.SerializeObject(new Dictionary<string, object>() {{nameof(testValue), testValue}}, Formatting.Indented);
            var actual = document.GeneratePrettyPrint();
            Assert.AreEqual(expected, actual);
        }
    }
}