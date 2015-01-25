using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;

namespace HFR4WinRT.Helpers
{
    public static class ThreadUI
    {
        private static CoreDispatcher _coreDispatcher;

        public static void setDispatcher(CoreDispatcher coreDispatcher)
        {
            _coreDispatcher = coreDispatcher;
        }

        public static IAsyncAction Invoke(Action a)
        {
            return _coreDispatcher.RunAsync(CoreDispatcherPriority.Normal, () => a());
        }
    }
}
