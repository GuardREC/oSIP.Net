using System;
using System.Runtime.InteropServices;
using NUnit.Framework;

namespace oSIP.Net.Tests
{
    [TestFixture]
    public class LinkedListTests
    {
        [Test]
        public void Shall_have_count_0_initially()
        {
            LinkedList<string> list = CreateList();

            Assert.That(list.Size, Is.EqualTo(0));
        }

        [Test]
        public void Shall_have_count_1_after_add()
        {
            LinkedList<string> list = CreateList();

            list.Add("42");

            Assert.That(list.Size, Is.EqualTo(1));
        }

        [Test]
        public void Shall_have_count_0_after_add_and_remove()
        {
            LinkedList<string> list = CreateList();

            list.Add("42");
            list.RemoveAt(0);

            Assert.That(list.Size, Is.EqualTo(0));
        }

        [Test]
        public void Shall_get_added_item()
        {
            LinkedList<string> list = CreateList();

            list.Add("42");

            Assert.That(list[0], Is.EqualTo("42"));
        }

        [Test]
        public void Shall_get_inserted_item()
        {
            LinkedList<string> list = CreateList();

            list.Add("42");
            list.Add("24");

            Assert.That(list[0], Is.EqualTo("42"));
            Assert.That(list[1], Is.EqualTo("24"));
        }

        [Test]
        public void Shall_throw_when_getting_out_of_range()
        {
            LinkedList<string> list = CreateList();

            Assert.That(() =>
            {
                // ReSharper disable once UnusedVariable
                string retrieved = list[-1];
            }, Throws.Exception.TypeOf<IndexOutOfRangeException>());

            Assert.That(() =>
            {
                // ReSharper disable once UnusedVariable
                string retrieved = list[0];
            }, Throws.Exception.TypeOf<IndexOutOfRangeException>());
        }

        [Test]
        public void Shall_throw_when_inserting_out_of_range()
        {
            LinkedList<string> list = CreateList();

            Assert.That(() =>
            {
                list.Insert(-1, "42");
            }, Throws.Exception.TypeOf<IndexOutOfRangeException>());

            Assert.That(() =>
            {
                list.Insert(1, "42");
            }, Throws.Exception.TypeOf<IndexOutOfRangeException>());
        }

        [Test]
        public void Shall_throw_when_removing_out_of_range()
        {
            LinkedList<string> list = CreateList();

            Assert.That(() =>
            {
                list.RemoveAt(-1);
            }, Throws.Exception.TypeOf<IndexOutOfRangeException>());

            Assert.That(() =>
            {
                list.RemoveAt(0);
            }, Throws.Exception.TypeOf<IndexOutOfRangeException>());
        }

        private static LinkedList<string> CreateList()
        {
            return new LinkedList<string>(Marshal.StringToHGlobalAnsi, Marshal.PtrToStringAnsi);
        }
    }
}