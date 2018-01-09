using System.Threading;

namespace oSIP.Net
{
    public static class Parser
    {
        private static int _isInitialized;

        internal static void InitializeIfNecessary()
        {
            if (Interlocked.Exchange(ref _isInitialized, 1) == 1)
            {
                return;
            }

            NativeMethods.parser_init();
        }
    }
}