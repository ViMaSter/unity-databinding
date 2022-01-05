using System;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using UnityEngine;

namespace Tests.DataBinding.Document
{
    internal class Delete
    {
        private const string PATH = "path.subObject";
        private const string CORRECT_VALUE = "correctValue";
        private const string NO_VALUE = "noValue";

        [Test]
        public void EmptyPath()
        {
            var gameObject = new GameObject();
            var document = gameObject.AddComponent<global::DataBinding.Document>();

            Assert.Throws<ArgumentException>(()=> document.Delete(null), "'path' cannot be empty or null");
            Assert.Throws<ArgumentException>(()=> document.Delete(""), "'path' cannot be empty or null");
        }

        [Test]
        public void PathWithNoObject()
        {
            var gameObject = new GameObject();
            var document = gameObject.AddComponent<global::DataBinding.Document>();

            Assert.IsFalse(document.Delete(PATH));
        }

        [Test]
        public void PathWithObject()
        {
            var gameObject = new GameObject();
            var document = gameObject.AddComponent<global::DataBinding.Document>();

            document.Set(PATH, CORRECT_VALUE);
            var actual = document.Get<string>(PATH, NO_VALUE);
            Assert.AreEqual(CORRECT_VALUE, actual);

            Assert.IsTrue(document.Delete(PATH));

            actual = document.Get<string>(PATH, NO_VALUE);
            Assert.AreEqual(NO_VALUE, actual);
        }

        [Test]
        public void CantDeleteTwice()
        {
            var gameObject = new GameObject();
            var document = gameObject.AddComponent<global::DataBinding.Document>();

            document.Set(PATH, CORRECT_VALUE);
            Assert.IsTrue(document.Delete(PATH));
            Assert.IsFalse(document.Delete(PATH));
        }

        [Test]
        public void DeletionInformsOnlyRootSubscriber()
        {
            var gameObject = new GameObject();
            var document = gameObject.AddComponent<global::DataBinding.Document>();

            var rootPath = PATH.Replace(".subObject", "");

            document.Set(PATH, CORRECT_VALUE);

            var informedAgnosticChildPathSubscriber = false;
            var informedAgnosticExactPathSubscriber = false;
            var informedSpecificChildPathSubscriber = false;
            var informedSpecificExactPathSubscriber = false;
            document.Subscribe(PATH, (jToken) => {
                informedAgnosticChildPathSubscriber = true;
            });
            document.Subscribe(rootPath, (jToken) => {
                informedAgnosticExactPathSubscriber = true;
            });
            document.Subscribe(PATH, (JObject jObject) => {
                informedSpecificChildPathSubscriber = true;
            });
            document.Subscribe(rootPath, (JObject jObject) => {
                informedSpecificExactPathSubscriber = true;
            });

            // subscribers for path with existing values automatically get called once on subscription
            // hence the target variables are reset again
            informedAgnosticChildPathSubscriber = false;
            informedAgnosticExactPathSubscriber = false;
            informedSpecificChildPathSubscriber = false;
            informedSpecificExactPathSubscriber = false;

            Assert.IsTrue(document.Delete(rootPath));

            Assert.IsFalse(informedAgnosticChildPathSubscriber);
            Assert.IsTrue(informedAgnosticExactPathSubscriber);
            Assert.IsFalse(informedSpecificChildPathSubscriber);
            Assert.IsTrue(informedSpecificExactPathSubscriber);
        }
    }
}