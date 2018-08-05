using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WebApiEtwTracing
{
    /// <summary>
    /// Helper class to allow a <see cref="Guid"/> to be associated
    /// with ETW events.  This becomes the ActivityId field recorded
    /// by ETW listeners.
    /// </summary>
    internal static class ActivityId
    {
        private enum ActivityControl : uint
        {
            EVENT_ACTIVITY_CTRL_GET_ID = 1,
            EVENT_ACTIVITY_CTRL_SET_ID = 2,
            EVENT_ACTIVITY_CTRL_CREATE_ID = 3,
            EVENT_ACTIVITY_CTRL_GET_SET_ID = 4,
            EVENT_ACTIVITY_CTRL_CREATE_SET_ID = 5
        }

        /// <summary>
        /// Sets the given <see cref="Guid"/> as the current activity ID.
        /// </summary>
        /// <param name="id">The <see cref="Guid"/>.</param>
        [System.Security.SecurityCritical]
        public static void SetActivityId(Guid id)
        {
            EventActivityIdControl((uint)ActivityControl.EVENT_ACTIVITY_CTRL_SET_ID, ref id);
        }

        [DllImport("advapi32.dll")]
        static extern uint EventActivityIdControl(uint ControlCode, ref Guid ActivityId);
    }
}
