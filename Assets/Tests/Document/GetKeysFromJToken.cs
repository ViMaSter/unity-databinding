using System.Linq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Tests.DataBinding.Document
{
    class GetKeysFromJToken
    {

        [Test]
        public void WithoutPrefix_PrintsAllPaths()
        {
            object newValueObject = new
            {
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
            var actual = global::DataBinding.Document.GetKeysFromJToken(JToken.FromObject(newValueObject));
            var expected = new string[] {
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
                "objectArray[2].c",
            };
            var a = 3;
            Assert.AreEqual(
                expected.OrderBy(value => value),
                actual.OrderBy(value => value)
            );
        }

        [Test]
        public void WithPrefix_PrintsAllPaths()
        {
            const string prefix = "object.array[4].subObject";
            object newValueObject = new
            {
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
            var actual = global::DataBinding.Document.GetKeysFromJToken(JToken.FromObject(newValueObject)).Select(path => $"{prefix}.{path}").ToList();
            var expected = new string[] {
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
            var a = 3;
            Assert.AreEqual(
                expected.OrderBy(value => value),
                actual.OrderBy(value => value)
            );
        }

    }
}