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
            var gameObject = new GameObject();
            var document = gameObject.AddComponent<global::DataBinding.Document>();

            document.Set("testString", "testString");
            const int defaultValue = -1;
            var incorrectTypeGetResult = document.Get<int>("testString", defaultValue);
            Assert.AreEqual(incorrectTypeGetResult, defaultValue);
        }

        [Test]
        public void ThrowsOnIncorrectTypeIfNoDefaultValueSupplied()
        {
            var gameObject = new GameObject();
            var document = gameObject.AddComponent<global::DataBinding.Document>();

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
            var gameObject = new GameObject();
            var document = gameObject.AddComponent<global::DataBinding.Document>();

            document.Set("testString", "testString");
            const int defaultValue = -1;
            var incorrectTypeGetResult = document.Get("testString", typeof(int), defaultValue);
            Assert.AreEqual(incorrectTypeGetResult, defaultValue);
        }
        [Test]
        public void ThrowsOnIncorrectTypeIfNoDefaultValueSupplied()
        {
            var gameObject = new GameObject();
            var document = gameObject.AddComponent<global::DataBinding.Document>();

            document.Set("testString", "testString");

            Assert.Throws<FormatException>(() => {
                document.Get("testString", typeof(int));
            }, " Input string was not in a correct format.");
        }
    }
}