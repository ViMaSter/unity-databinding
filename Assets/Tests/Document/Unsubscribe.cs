using System;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.DataBinding.Document
{
    class Unsubscribe
    {
        private const string DATABINDING_PATH = "path";
        [Test]
        public void ToDirectPathAndCorrectType()
        {
            GameObject gameObject = new GameObject();
            global::DataBinding.Document document = gameObject.AddComponent<global::DataBinding.Document>();

            const string value = "C:/";

            bool called = false;
            void Callback(string path)
            {
                called = path == value;
            }

            document.Subscribe(DATABINDING_PATH, (Action<string>)Callback);
            document.Unsubscribe(DATABINDING_PATH, (Action<string>)Callback);
            document.Set(DATABINDING_PATH, value);
            Assert.IsFalse(called);
            document.Set(DATABINDING_PATH, "wrong");
            Assert.IsFalse(called);
        }

        [Test]
        public void ToDirectPathAndIncorrectType()
        {
            GameObject gameObject = new GameObject();
            global::DataBinding.Document document = gameObject.AddComponent<global::DataBinding.Document>();

            const string value = "C:/";

            bool called = false;
            void Callback(int path)
            {
                called = true;
            }

            document.Subscribe(DATABINDING_PATH, (Action<int>)Callback);
            document.Unsubscribe(DATABINDING_PATH, (Action<int>)Callback);
            document.Set(DATABINDING_PATH, value);
            Assert.IsFalse(called);
            document.Set(DATABINDING_PATH, "wrong");
            Assert.IsFalse(called);

            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void ToDirectPathAndIndirectType()
        {
            GameObject gameObject = new GameObject();
            global::DataBinding.Document document = gameObject.AddComponent<global::DataBinding.Document>();

            const string value = "C:/";

            bool called = false;
            void Callback(JToken token)
            {
                string path = token.ToObject<string>();
                called = path == value;
            }

            document.Subscribe(DATABINDING_PATH, Callback);
            document.Unsubscribe(DATABINDING_PATH, Callback);
            document.Set(DATABINDING_PATH, value);
            Assert.IsFalse(called);
            document.Set(DATABINDING_PATH, "wrong");
            Assert.IsFalse(called);
        }
        class NestedValue
        {
            public string a;
        }
        [Test]
        public void ToNestedPathAndDirectType()
        {
            GameObject gameObject = new GameObject();
            global::DataBinding.Document document = gameObject.AddComponent<global::DataBinding.Document>();

            const string value = "C:/";

            var called = false;
            void Callback(NestedValue nest)
            {
                called = nest.a == value;
            }

            document.Subscribe(DATABINDING_PATH, (Action<NestedValue>)Callback);
            document.Unsubscribe(DATABINDING_PATH, (Action<NestedValue>)Callback);
            document.Set($"{DATABINDING_PATH}.a", value);
            Assert.IsFalse(called);
            document.Set($"{DATABINDING_PATH}.a", "wrong");
            Assert.IsFalse(called);
        }

        [Test]
        public void ToNestedPathAndIncorrectType()
        {
            GameObject gameObject = new GameObject();
            global::DataBinding.Document document = gameObject.AddComponent<global::DataBinding.Document>();

            const string value = "C:/";

            var called = false;
            void Callback(string nest)
            {
                called = nest == value;
            }

            document.Subscribe(DATABINDING_PATH, (Action<string>)Callback);
            document.Unsubscribe(DATABINDING_PATH, (Action<string>)Callback);
            document.Set($"{DATABINDING_PATH}.a", value);
            Assert.IsFalse(called);
            document.Set($"{DATABINDING_PATH}.a", "wrong");
            Assert.IsFalse(called);

            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void ToNestedPathAndIndirectType()
        {
            GameObject gameObject = new GameObject();
            global::DataBinding.Document document = gameObject.AddComponent<global::DataBinding.Document>();

            const string value = "C:/";

            bool called = false;
            void Callback(JToken token)
            {
                var nest = token.ToObject<NestedValue>();
                called = nest.a == value;
            }

            document.Subscribe(DATABINDING_PATH, Callback);
            document.Unsubscribe(DATABINDING_PATH, Callback);
            document.Set($"{DATABINDING_PATH}.a", value);
            Assert.IsFalse(called);
            document.Set($"{DATABINDING_PATH}.a", "wrong");
            Assert.IsFalse(called);
        }
    }
}