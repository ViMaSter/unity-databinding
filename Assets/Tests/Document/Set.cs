using System;
using NUnit.Framework;
using UnityEngine;

namespace Tests.DataBinding.Document
{
    internal class Set
    {
        [Test]
        public void ThrowsIfSamePathIsSetWithDifferentValueTypes()
        {
            GameObject gameObject = new GameObject();
            global::DataBinding.Document document = gameObject.AddComponent<global::DataBinding.Document>();

            document.Set("path", 1);
            Assert.Throws<NotSupportedException>(() => {
                document.Set("path", "");
            });
        }
        [Test]
        public void ThrowsIfSamePathIsSetWithDifferentValueAfterBeingDeletedFirstTypes()
        {
            GameObject gameObject = new GameObject();
            global::DataBinding.Document document = gameObject.AddComponent<global::DataBinding.Document>();

            document.Set("path", 1);
            document.Delete("path");
            Assert.DoesNotThrow(() => {
                document.Set("path", "");
            });
        }
    }
}