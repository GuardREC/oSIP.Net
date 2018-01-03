using System;
using System.Runtime.InteropServices;

namespace oSIP.Net
{
    public unsafe class LinkedList<T>
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

            NativeMethods.osip_list_init(_native);
        }

        internal LinkedList(osip_list_t* native, Func<T, IntPtr> toNative, Func<IntPtr, T> fromNative)
        {
            _native = native;
            _toNative = toNative;
            _fromNative = fromNative;
        }

        public int Size => NativeMethods.osip_list_size(_native);

        public void Add(T item)
        {
            Insert(Size, item);
        }

        public void Insert(int index, T item)
        {
            if (index < 0 || index > Size)
            {
                throw new IndexOutOfRangeException();
            }

            void* itemPtr = _toNative(item).ToPointer();
            NativeMethods.osip_list_add(_native, itemPtr, index);
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= Size)
            {
                throw new IndexOutOfRangeException();
            }

            NativeMethods.osip_list_remove(_native, index);
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= Size)
                {
                    throw new IndexOutOfRangeException();
                }

                void* itemPtr = NativeMethods.osip_list_get(_native, index);
                return _fromNative(new IntPtr(itemPtr));
            }
        }
    }
}