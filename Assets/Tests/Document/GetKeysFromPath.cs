using System.Linq;
using NUnit.Framework;

namespace Tests.DataBinding.Document
{
    internal class GetKeysFromPath
    {
        [Test]
        public void PrintsAllPaths()
        {
            const string path = "object.array[4].subObject";
            var actual = global::DataBinding.Document.GetKeysFromPath(path);
            string[] expected = {
                "object",
                "object.array",
                "object.array[4]",
                "object.array[4].subObject"
            };
            Assert.AreEqual(
                expected.OrderBy(value => value),
                actual.OrderBy(value => value)
            );
        }
    }
}