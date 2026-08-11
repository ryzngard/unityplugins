using UnityEngine;
using System.Runtime.InteropServices;

namespace Apple.Accessibility.UnitTests
{
    public class AccessibilityTests : MonoBehaviour
    {
        [DllImport("__Internal")]
        private static extern bool _UnityAX_RuniOSSideUnitTestWithName(string name);

        [DllImport("__Internal")]
        private static extern bool _UnityAX_RuniOSSideUnitTestWithKeyPathExpectingStringResult(int identifier, string keyPath, string expected);

        [DllImport("__Internal")]
        private static extern bool _UnityAX_RuniOSSideUnitTestWithKeyPathExpectingStringResult2(ulong identifier, string keyPath, string expected);

        public static bool RuniOSUnitTestWithName(string name)
        {
#if (UNITY_IOS || UNITY_TVOS) && !UNITY_EDITOR
            return _UnityAX_RuniOSSideUnitTestWithName(name);
#else
            return true;
#endif
        }

#if UNITY_6000_4_OR_NEWER
        public static bool RuniOSSideUnitTestWithKeyPathExpectingStringResult(ulong identifier, string keyPath, string expected)
        {
#if (UNITY_IOS || UNITY_TVOS) && !UNITY_EDITOR
            return _UnityAX_RuniOSSideUnitTestWithKeyPathExpectingStringResult2(identifier, keyPath, expected);
#else
            return true;
#endif
        }
#else
        public static bool RuniOSSideUnitTestWithKeyPathExpectingStringResult(int identifier, string keyPath, string expected)
        {
#if (UNITY_IOS || UNITY_TVOS) && !UNITY_EDITOR
            return _UnityAX_RuniOSSideUnitTestWithKeyPathExpectingStringResult( identifier, keyPath, expected);
#else
            return true;
#endif
        }
#endif

        [DllImport("__Internal")]
        private static extern void _UnityAX_PostFeatureEnabledNotification(string name, bool enabled);

        public static void PostFeatureEnabledNotification(string name, bool enabled)
        {
            _UnityAX_PostFeatureEnabledNotification(name, enabled);
        }
    }
}
