using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace oSIP.Net
{
    public unsafe class LinkedList<T> : IReadOnlyCollection<T>
    {
        private readonly osip_list_t* _native;
        private readonly Func<T, IntPtr> _toNative;
        private readonly Func<IntPtr, T> _fromNative;

        static LinkedList()
        {
            Parser.InitializeIfNecessary();
        }

        public LinkedList(Func<T, IntPtr> toNative, Func<IntPtr, T> fromNative)
        {
            _native = (osip_list_t*) Marshal.AllocHGlobal(Marshal.SizeOf<osip_list_t>());
            _toNative = toNative;
            _fromNative = fromNative;

            NativeMethods.osip_list_init(_native).ThrowOnError();
        }

        internal LinkedList(osip_list_t* native, Func<T, IntPtr> toNative, Func<IntPtr, T> fromNative)
        {
            _native = native;
            _toNative = toNative;
            _fromNative = fromNative;
        }

        public int Count
        {
            get
            {
                int size = NativeMethods.osip_list_size(_native);
                ((ErrorCode) size).ThrowOnError();
                return size;
            }
        }

        public void Add(T item)
        {
            Insert(Count, item);
        }

        public void Insert(int index, T item)
        {
            if (index < 0 || index > Count)
            {
                throw new IndexOutOfRangeException();
            }

            void* itemPtr = _toNative(item).ToPointer();
            NativeMethods.osip_list_add(_native, itemPtr, index).ThrowOnError();
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= Count)
            {
                throw new IndexOutOfRangeException();
            }

            NativeMethods.osip_list_remove(_native, index).ThrowOnError();
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                {
                    throw new IndexOutOfRangeException();
                }

                void* itemPtr = NativeMethods.osip_list_get(_native, index);
                return _fromNative(new IntPtr(itemPtr));
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class Enumerator : IEnumerator<T>
        {
            private readonly LinkedList<T> _list;
            private int _index;

            public Enumerator(LinkedList<T> list)
            {
                _list = list;
                _index = -1;
            }

            public bool MoveNext()
            {
                _index++;
                return _index < _list.Count;
            }

            public void Reset()
            {
                _index = -1;
            }

            public T Current => _index > -1 && _index < _list.Count 
                ? _list[_index] 
                : default(T);

            object IEnumerator.Current => Current;

            public void Dispose()
            {
            }
        }
    }
}