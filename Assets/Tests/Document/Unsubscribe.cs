using System;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.DataBinding.Document
{
    internal class Unsubscribe
    {
        private const string DATABINDING_PATH = "path";
        private const string CORRECT_VALUE = "C:/";
        private const string INCORRECT_VALUE = "wrong";
        [Test]
        public void ToDirectPathAndCorrectType()
        {
            GameObject gameObject = new GameObject();
            global::DataBinding.Document document = gameObject.AddComponent<global::DataBinding.Document>();

            bool called = false;
            void Callback(string path)
            {
                called = path == CORRECT_VALUE;
            }

            document.Subscribe(DATABINDING_PATH, (Action<string>)Callback);
            document.Unsubscribe(DATABINDING_PATH, (Action<string>)Callback);
            document.Set(DATABINDING_PATH, CORRECT_VALUE);
            Assert.IsFalse(called);
            document.Set(DATABINDING_PATH, INCORRECT_VALUE);
            Assert.IsFalse(called);
        }

        [Test]
        public void ToDirectPathAndIncorrectType()
        {
            GameObject gameObject = new GameObject();
            global::DataBinding.Document document = gameObject.AddComponent<global::DataBinding.Document>();

            bool called = false;
            void Callback(int path)
            {
                called = true;
            }

            document.Subscribe(DATABINDING_PATH, (Action<int>)Callback);
            document.Unsubscribe(DATABINDING_PATH, (Action<int>)Callback);
            document.Set(DATABINDING_PATH, CORRECT_VALUE);
            Assert.IsFalse(called);
            document.Set(DATABINDING_PATH, INCORRECT_VALUE);
            Assert.IsFalse(called);

            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void ToDirectPathAndIndirectType()
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

        private class NestedValue
        {
            public string StringValue = "";
        }
        [Test]
        public void ToNestedPathAndDirectType()
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
        public void ToNestedPathAndIncorrectType()
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
        public void ToNestedPathAndIndirectType()
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