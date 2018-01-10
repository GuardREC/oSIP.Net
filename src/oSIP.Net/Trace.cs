using System;
using System.Runtime.InteropServices;

namespace oSIP.Net
{
    public static class Trace
    {
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private static readonly TraceCallback CallbackDelegate;
        private static Action<TraceEvent> _callback;

        private delegate void TraceCallback(
            IntPtr filenamePtr, int line, TraceLevel level, IntPtr formatPtr, IntPtr argPtr);

        static Trace()
        {
            CallbackDelegate = OnLog;

            IntPtr callbackPtr = Marshal.GetFunctionPointerForDelegate(CallbackDelegate);
            NativeMethods.osip_trace_initialize_func(TraceLevel.Info4 + 1, callbackPtr);
        }

        public static void SetLogger(Action<TraceEvent> callback)
        {
            _callback = callback;
        }

        public static void SetLevel(TraceLevel level)
        {
            foreach (TraceLevel l in Enum.GetValues(typeof (TraceLevel)))
            {
                if (l <= level)
                {
                    NativeMethods.osip_trace_enable_level(l);
                }
                else
                {
                    NativeMethods.osip_trace_disable_level(l);
                }
            }
        }

        private static void OnLog(IntPtr filenamePtr, int line, TraceLevel level, IntPtr formatPtr, IntPtr argPtr)
        {
            if (_callback == null)
            {
                return;
            }

            string filename = Marshal.PtrToStringAnsi(filenamePtr);

            IntPtr buffer = Marshal.AllocHGlobal(2024);

            int count = NativeMethods.vsprintf(buffer, formatPtr, argPtr);
            var message = count >= 0
                ? Marshal.PtrToStringAnsi(buffer, count)
                : Marshal.PtrToStringAnsi(formatPtr);

            _callback(new TraceEvent(filename, line, level, message));
        }
    }
}