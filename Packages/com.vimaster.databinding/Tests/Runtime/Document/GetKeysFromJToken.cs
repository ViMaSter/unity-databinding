using System.Linq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Tests.DataBinding.Document
{
    internal class GetKeysFromJToken
    {
        [Test]
        public void DirectSubObject()
        {
            var newObject = new {a = new {b = 3}};
            var newObjectAsToken = JToken.FromObject(newObject);
            var actual = global::DataBinding.Document.GetKeysFromJToken(newObjectAsToken["a"]!);
            Assert.AreEqual(new[] { "a.b", "a" }, actual);
        }
        [Test]
        public void Array()
        {
            var actual = global::DataBinding.Document.GetKeysFromJToken(JToken.FromObject(new[]{"a" , "b" , ""}));
            Assert.AreEqual(new[]{ "[0]" , "[1]", "[2]" }, actual);
        }

        [Test]
        public void Value()
        {
            var actual = global::DataBinding.Document.GetKeysFromJToken(JToken.FromObject(1));
            Assert.AreEqual(new string[0], actual);
        }

        [Test]
        public void Object()
        {
            var actual = global::DataBinding.Document.GetKeysFromJToken(JToken.FromObject(new
            {
                stringValue = "abc",
                intArray = new[] {
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
            }));
            string[] expected = {
                "stringValue",
                "intArray",
                "intArray[0]",
                "intArray[1]",
                "intArray[2]",
                "objectArray",
                "objectArray[0]",
                "objectArray[0].a",
                "objectArray[1]",
                "objectArray[1].b",
                "objectArray[2]",
                "objectArray[2].c"
            };
            Assert.AreEqual(
                expected.OrderBy(value => value),
                actual.OrderBy(value => value)
            );
        }
    }
}