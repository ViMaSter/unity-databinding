using System;
using NUnit.Framework;
using UnityEngine;

namespace Tests.DataBinding.Document.Get
{
    internal class TypeSpecific
    {
        [Test]
        public void DoesNotThrowOnIncorrectTypeIfDefaultValueSuppliedOnIn()
        {
            GameObject gameObject = new GameObject();
            global::DataBinding.Document document = gameObject.AddComponent<global::DataBinding.Document>();

            document.Set("testString", "testString");
            const int defaultValue = -1;
            int incorrectTypeGetResult = document.Get<int>("testString", defaultValue);
            Assert.AreEqual(incorrectTypeGetResult, defaultValue);
        }

        [Test]
        public void ThrowsOnIncorrectTypeIfNoDefaultValueSupplied()
        {
            GameObject gameObject = new GameObject();
            global::DataBinding.Document document = gameObject.AddComponent<global::DataBinding.Document>();

            document.Set("testString", "testString");

            Assert.Throws<FormatException>(() => {
                document.Get<int>("testString");
            }, " Input string was not in a correct format.");
        } 
    }

    internal class TypeAgnostic
    {
        [Test]
        public void DoesNotThrowOnIncorrectTypeIfDefaultValueSupplied()
        {
            GameObject gameObject = new GameObject();
            global::DataBinding.Document document = gameObject.AddComponent<global::DataBinding.Document>();

            document.Set("testString", "testString");
            const int defaultValue = -1;
            object incorrectTypeGetResult = document.Get("testString", typeof(int), defaultValue);
            Assert.AreEqual(incorrectTypeGetResult, defaultValue);
        }
        [Test]
        public void ThrowsOnIncorrectTypeIfNoDefaultValueSupplied()
        {
            GameObject gameObject = new GameObject();
            global::DataBinding.Document document = gameObject.AddComponent<global::DataBinding.Document>();

            document.Set("testString", "testString");

            Assert.Throws<FormatException>(() => {
                document.Get("testString", typeof(int));
            }, " Input string was not in a correct format.");
        }
    }
}