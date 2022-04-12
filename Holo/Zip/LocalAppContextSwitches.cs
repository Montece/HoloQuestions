using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Holo.Zip
{
    public static class LocalAppContextSwitches
    {
        private static int _zipFileUseBackslash;

        private static int _doNotAddTrailingSeparator;

        internal const string ZipFileUseBackslashName = "Switch.System.IO.Compression.ZipFile.UseBackslash";

        internal const string DoNotAddTrailingSeparatorString = "Switch.System.IO.Compression.DoNotAddTrailingSeparator";

        public static bool ZipFileUseBackslash
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return LocalAppContext.GetCachedSwitchValue("Switch.System.IO.Compression.ZipFile.UseBackslash", ref LocalAppContextSwitches._zipFileUseBackslash);
            }
        }

        public static bool DoNotAddTrailingSeparator
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return LocalAppContext.GetCachedSwitchValue("Switch.System.IO.Compression.DoNotAddTrailingSeparator", ref LocalAppContextSwitches._doNotAddTrailingSeparator);
            }
        }
    }
}
