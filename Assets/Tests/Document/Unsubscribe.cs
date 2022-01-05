using System;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.DataBinding.Document.Unsubscribe
{
    internal class ToPathWithNoSubscription
    {
        private const string DATABINDING_PATH = "path";

        [Test]
        public void SpecificType()
        {
            GameObject gameObject = new GameObject();
            global::DataBinding.Document document = gameObject.AddComponent<global::DataBinding.Document>();

            Assert.False(document.Unsubscribe(DATABINDING_PATH, (Action<string>)(path => { })));
        }

        [Test]
        public void AgnosticType()
        {
            GameObject gameObject = new GameObject();
            global::DataBinding.Document document = gameObject.AddComponent<global::DataBinding.Document>();

            Assert.False(document.Unsubscribe(DATABINDING_PATH, (path => { })));
        }
    }

    internal class ToDirectPath
    {
        private const string DATABINDING_PATH = "path";
        private const string CORRECT_VALUE = "C:/";
        private const string INCORRECT_VALUE = "wrong";

        [Test]
        public void CorrectType()
        {
            GameObject gameObject = new GameObject();
            global::DataBinding.Document document = gameObject.AddComponent<global::DataBinding.Document>();

            bool called = false;

            void Callback(string path)
            {
                called = path == CORRECT_VALUE;
            }

            document.Subscribe(DATABINDING_PATH, (Action<string>) Callback);
            document.Unsubscribe(DATABINDING_PATH, (Action<string>) Callback);
            document.Set(DATABINDING_PATH, CORRECT_VALUE);
            Assert.IsFalse(called);
            document.Set(DATABINDING_PATH, INCORRECT_VALUE);
            Assert.IsFalse(called);
        }

        [Test]
        public void IncorrectType()
        {
            GameObject gameObject = new GameObject();
            global::DataBinding.Document document = gameObject.AddComponent<global::DataBinding.Document>();

            bool called = false;

            void Callback(int path)
            {
                called = true;
            }

            document.Subscribe(DATABINDING_PATH, (Action<int>) Callback);
            document.Unsubscribe(DATABINDING_PATH, (Action<int>) Callback);
            document.Set(DATABINDING_PATH, CORRECT_VALUE);
            Assert.IsFalse(called);
            document.Set(DATABINDING_PATH, INCORRECT_VALUE);
            Assert.IsFalse(called);

            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void IndirectType()
        {
            GameObject gameObject = new GameObject();
            global::DataBinding.Document document = gameObject.AddComponent<global::DataBinding.Document>();

            bool called = false;

            void Callback(JToken token)
            {
                string path = token.ToObject<string>();
                called = path == CORRECT_VALUE;
            }

            document.Subscribe(DATABINDING_PATH, Callback);
            document.Unsubscribe(DATABINDING_PATH, Callback);
            document.Set(DATABINDING_PATH, CORRECT_VALUE);
            Assert.IsFalse(called);
            document.Set(DATABINDING_PATH, INCORRECT_VALUE);
            Assert.IsFalse(called);
        }
    }

    internal class ToNestedPath
    {
        private const string DATABINDING_PATH = "path";
        private const string CORRECT_VALUE = "C:/";
        private const string INCORRECT_VALUE = "wrong";

        private class NestedValue
        {
            public string StringValue = "";
        }
        [Test]
        public void DirectType()
        {
            GameObject gameObject = new GameObject();
            global::DataBinding.Document document = gameObject.AddComponent<global::DataBinding.Document>();

            bool called = false;
            void Callback(NestedValue nest)
            {
                called = nest.StringValue == CORRECT_VALUE;
            }

            document.Subscribe(DATABINDING_PATH, (Action<NestedValue>)Callback);
            document.Unsubscribe(DATABINDING_PATH, (Action<NestedValue>)Callback);
            document.Set($"{DATABINDING_PATH}.{nameof(NestedValue.StringValue)}", CORRECT_VALUE);
            Assert.IsFalse(called);
            document.Set($"{DATABINDING_PATH}.{nameof(NestedValue.StringValue)}", INCORRECT_VALUE);
            Assert.IsFalse(called);
        }

        [Test]
        public void IncorrectType()
        {
            GameObject gameObject = new GameObject();
            global::DataBinding.Document document = gameObject.AddComponent<global::DataBinding.Document>();

            bool called = false;
            void Callback(string nest)
            {
                called = nest == CORRECT_VALUE;
            }

            document.Subscribe(DATABINDING_PATH, (Action<string>)Callback);
            document.Unsubscribe(DATABINDING_PATH, (Action<string>)Callback);
            document.Set($"{DATABINDING_PATH}.{nameof(NestedValue.StringValue)}", CORRECT_VALUE);
            Assert.IsFalse(called);
            document.Set($"{DATABINDING_PATH}.{nameof(NestedValue.StringValue)}", INCORRECT_VALUE);
            Assert.IsFalse(called);

            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void IndirectType()
        {
            GameObject gameObject = new GameObject();
            global::DataBinding.Document document = gameObject.AddComponent<global::DataBinding.Document>();

            bool called = false;
            void Callback(JToken token)
            {
                NestedValue nestedValue = token.ToObject<NestedValue>();
                called = nestedValue!.StringValue == CORRECT_VALUE;
            }

            document.Subscribe(DATABINDING_PATH, Callback);
            document.Unsubscribe(DATABINDING_PATH, Callback);
            document.Set($"{DATABINDING_PATH}.{nameof(NestedValue.StringValue)}", CORRECT_VALUE);
            Assert.IsFalse(called);
            document.Set($"{DATABINDING_PATH}.{nameof(NestedValue.StringValue)}", INCORRECT_VALUE);
            Assert.IsFalse(called);
        }
    }
}