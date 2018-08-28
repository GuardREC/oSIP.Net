using System;
using System.Linq;
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

            Assert.That(list.Count, Is.EqualTo(0));
        }

        [Test]
        public void Shall_have_count_1_after_add()
        {
            LinkedList<string> list = CreateList();

            list.Add("42");

            Assert.That(list.Count, Is.EqualTo(1));
        }

        [Test]
        public void Shall_have_count_0_after_add_and_remove()
        {
            LinkedList<string> list = CreateList();

            list.Add("42");
            list.RemoveAt(0);

            Assert.That(list.Count, Is.EqualTo(0));
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

        [Test]
        public void Shall_enumerate_empty_list()
        {
            LinkedList<string> list = CreateList();

            Assert.That(list, Is.Empty);
        }

        [Test]
        public void Shall_enumerate_list()
        {
            LinkedList<string> list = CreateList();
            list.Add("foo");
            list.Add("bar");

            Assert.That(list, Is.EquivalentTo(new[] {"foo", "bar"}));
            // ReSharper disable once UseCollectionCountProperty
            Assert.That(list.Count(), Is.EqualTo(2));
        }

        [Test]
        public void Shall_for_each_list()
        {
            LinkedList<string> list = CreateList();
            list.Add("foo");
            list.Add("bar");
            
            var index = 0;
            foreach (var s in list)
            {
                Assert.That(s, Is.EqualTo(list[index++]));
            }
        }

        [Test]
        public void Shall_filter_list()
        {
            LinkedList<string> list = CreateList();
            list.Add("foo");
            list.Add("bar");
            
            Assert.That(list.FirstOrDefault(x => x == "foo"), Is.EqualTo("foo"));
            Assert.That(list.FirstOrDefault(x => x == "baz"), Is.Null);
        }

        [Test]
        public void Shall_contain()
        {
            LinkedList<string> list = CreateList();
            list.Add("foo");
            list.Add("bar");

            Assert.That(list.Contains("foo"), Is.EqualTo(true));
            Assert.That(list.Contains("baz"), Is.EqualTo(false));
        }

        private static unsafe LinkedList<string> CreateList()
        {
            var native = (osip_list_t*) Marshal.AllocHGlobal(Marshal.SizeOf(typeof(osip_list_t)));
            NativeMethods.osip_list_init(native).ThrowOnError(() => Marshal.FreeHGlobal((IntPtr) native));

            return new LinkedList<string>(native, Marshal.StringToHGlobalAnsi, Marshal.PtrToStringAnsi);
        }
    }
}