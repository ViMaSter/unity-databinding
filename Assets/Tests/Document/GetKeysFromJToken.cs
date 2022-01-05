using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Tests.DataBinding.Document
{
    internal class GetKeysFromJToken
    {
        private readonly object _nestedValueObject = new
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
        };
        [Test]
        public void WithoutPrefix_PrintsAllPaths()
        {
            IEnumerable<string> actual = global::DataBinding.Document.GetKeysFromJToken(JToken.FromObject(_nestedValueObject));
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

        [Test]
        public void WithPrefix_PrintsAllPaths()
        {
            const string prefix = "object.array[4].subObject";
            List<string> actual = global::DataBinding.Document.GetKeysFromJToken(JToken.FromObject(_nestedValueObject)).Select(path => $"{prefix}.{path}").ToList();
            string[] expected = {
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
                "object.array[4].subObject.objectArray[2].c"
            };
            Assert.AreEqual(
                expected.OrderBy(value => value),
                actual.OrderBy(value => value)
            );
        }

    }
}