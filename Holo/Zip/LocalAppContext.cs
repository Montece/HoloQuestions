using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Holo.Zip
{
    public static class LocalAppContext
    {
        private delegate bool TryGetSwitchDelegate(string switchName, out bool value);

        private static TryGetSwitchDelegate TryGetSwitchFromCentralAppContext;

        private static bool s_canForwardCalls;

        private static Dictionary<string, bool> s_switchMap;

        private static readonly object s_syncLock;

        private static bool DisableCaching
        {
            get;
            set;
        }

        static LocalAppContext()
        {
            LocalAppContext.s_switchMap = new Dictionary<string, bool>();
            LocalAppContext.s_syncLock = new object();
            LocalAppContext.s_canForwardCalls = LocalAppContext.SetupDelegate();
            //AppContextDefaultValues.PopulateDefaultValues();
            LocalAppContext.DisableCaching = LocalAppContext.IsSwitchEnabled("TestSwitch.LocalAppContext.DisableCaching");
        }

        public static bool IsSwitchEnabled(string switchName)
        {
            if (LocalAppContext.s_canForwardCalls && LocalAppContext.TryGetSwitchFromCentralAppContext(switchName, out bool result))
            {
                return result;
            }
            return LocalAppContext.IsSwitchEnabledLocal(switchName);
        }

        private static bool IsSwitchEnabledLocal(string switchName)
        {
            Dictionary<string, bool> obj = LocalAppContext.s_switchMap;
            bool flag3;
            bool flag2;
            lock (obj)
            {
                flag2 = LocalAppContext.s_switchMap.TryGetValue(switchName, out flag3);
            }
            return flag2 && flag3;
        }

        private static bool SetupDelegate()
        {
            Type type = typeof(object).Assembly.GetType("System.AppContext");
            if (type == null)
            {
                return false;
            }
            MethodInfo method = type.GetMethod("TryGetSwitch", BindingFlags.Static | BindingFlags.Public, null, new Type[]
            {
                typeof(string),
                typeof(bool).MakeByRefType()
            }, null);
            if (method == null)
            {
                return false;
            }
            LocalAppContext.TryGetSwitchFromCentralAppContext = (LocalAppContext.TryGetSwitchDelegate)Delegate.CreateDelegate(typeof(LocalAppContext.TryGetSwitchDelegate), method);
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool GetCachedSwitchValue(string switchName, ref int switchValue)
        {
            return switchValue >= 0 && (switchValue > 0 || LocalAppContext.GetCachedSwitchValueInternal(switchName, ref switchValue));
        }

        private static bool GetCachedSwitchValueInternal(string switchName, ref int switchValue)
        {
            if (LocalAppContext.DisableCaching)
            {
                return LocalAppContext.IsSwitchEnabled(switchName);
            }
            bool flag = LocalAppContext.IsSwitchEnabled(switchName);
            switchValue = (flag ? 1 : -1);
            return flag;
        }

        internal static void DefineSwitchDefault(string switchName, bool initialValue)
        {
            LocalAppContext.s_switchMap[switchName] = initialValue;
        }
    }
}
