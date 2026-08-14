using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Apple.Accessibility
{
    /// <summary>
    /// Constants that describe how an accessibility element behaves.
    /// </summary>
    [Flags]
    public enum AccessibilityTrait
    {
        None = 0,
        Button = 1 << 0,
        Link = 1 << 1,
        Image = 1 << 2,
        Selected = 1 << 3,
        PlaysSound = 1 << 4,
        KeyboardKey = 1 << 5,
        StaticText = 1 << 6,
        SummaryElement = 1 << 7,
        NotEnabled = 1 << 8,
        UpdatesFrequently = 1 << 9,
        SearchField = 1 << 10,
        StartsMediaSession = 1 << 11,
        Adjustable = 1 << 12,
        AllowsDirectInteraction = 1 << 13,
        CausesPageTurn = 1 << 14,
        TabBar = 1 << 15,
        Header = 1 << 16,
    };

    /// <summary>
    /// The direction of a scrolling action.
    /// </summary>
    public enum AccessibilityScrollDirection
    {
        Right = 1,
        Left = 2,
        Up = 3,
        Down = 4,
        Next = 5,
        Previous = 6
    }

    /// <summary>
    /// A custom action to perform on an accessible object.
    /// </summary>
    public struct AccessibilityCustomAction
    {
        public string name;
        public Func<bool> onAction;

        internal Guid customActionIdentifier;

        public AccessibilityCustomAction(string _name, Func<bool> _onAction)
        {
            name = _name;
            onAction = _onAction;
            customActionIdentifier = Guid.NewGuid();
        }
    }

    public interface IAccessibilityFrame
    {
        Rect AccessibilityFrame();
    }

    public interface IAccessibilityLabel
    {
        string AccessibilityLabel();
    }

    public interface IAccessibilityCustomActions
    {
        AccessibilityCustomAction[] AccessibilityCustomActions();
    }

    public interface IAccessibilityValue
    {
        string AccessibilityValue();
    }

    public interface IAccessibilityHint
    {
        string AccessibilityHint();
    }

    public interface IAccessibilityIdentifier
    {
        string AccessibilityIdentifier();
    }

    public interface IAccessibilityViewIsModal
    {
        bool AccessibilityViewIsModal();
    }

    public interface IAccessibilityAdjustable
    {
        bool AccessibilityIncrement();
        bool AccessibilityDecrement();
    }

    public interface IAccessibilityActivationPoint
    {
        Vector2 AccessibilityActivationPoint();
    }

    public interface IAccessibilitySlider : IAccessibilityAdjustable, IAccessibilityValue
    {

    }

    [DisallowMultipleComponent]
    public sealed class AccessibilityNode : MonoBehaviour
    {
        /// <summary>
        /// A Boolean value that indicates whether the element is an accessibility element that an assistive app can access.
        /// </summary>
        public bool IsAccessibilityElement
        {
            set
            {
                _userIsAccessibilityElementProvided = true;
                m_userIsAccessibilityElement = value;
            }
            get
            {
                return _isAccessibilityElement();
            }
        }

        public Func<bool> isAccessibilityElementDelegate = null;


        /// <summary>
        /// The combination of accessibility traits that best characterizes the accessibility element.
        /// The default value for this property is None.
        /// </summary>
        public AccessibilityTrait AccessibilityTraits
        {
            set
            {
                _userAccessibilityTraitsProvided = true;
                m_userAccessibilityTraits = value;
            }
            get
            {
                return _accessibilityTraits();
            }
        }

        public Func<AccessibilityTrait> accessibilityTraitsDelegate = null;


        /// <summary>
        /// The frame of the accessibility element in screen coordinates.
        /// </summary>
        public Rect AccessibilityFrame
        {
            set
            {
                _userAccessibilityFrameProvided = true;
                m_userAccessibilityFrame = value;
            }
            get
            {
                return _accessibilityFrame();
            }
        }

        public Func<Rect> accessibilityFrameDelegate = null;


        /// <summary>
        /// A succinct label in a localized string that identifies the accessibility element.
        /// </summary>
        public string AccessibilityLabel
        {
            set
            {
                _userAccessibilityLabelProvided = true;
                m_userAccessibilityLabel = value;
            }
            get
            {
                return _accessibilityLabel();
            }
        }

        public Func<string> accessibilityLabelDelegate = null;


        /// <summary>
        /// A localized string that contains the value of the accessibility element.
        /// </summary>
        public string AccessibilityValue
        {
            set
            {
                _userAccessibilityValueProvided = true;
                m_userAccessibilityValue = value;
            }
            get
            {
                return _accessibilityValue();
            }
        }

        public Func<string> accessibilityValueDelegate = null;


        /// <summary>
        /// A localized string that contains a brief description of the result of performing an action on the accessibility element.
        /// </summary>
        public string AccessibilityHint
        {
            set
            {
                _userAccessibilityHintProvided = true;
                m_userAccessibilityHint = value;
            }
            get
            {
                return _accessibilityHint();
            }
        }

        public Func<string> accessibilityHintDelegate = null;


        /// <summary>
        /// A string that identifies the element.
        /// An identifier can be used to uniquely identify an element in the scripts you write using the UI Automation interfaces.
        /// Using an identifier allows you to avoid inappropriately setting or accessing an element’s accessibility label.
        /// </summary>
        public string AccessibilityIdentifier
        {
            set
            {
                _userAccessibilityIdentifierProvided = true;
                m_userAccessibilityIdentifier = value;
            }
            get
            {
                return _accessibilityIdentifier();
            }
        }

        public Func<string> accessibilityIdentifierDelegate = null;


        /// <summary>
        /// An array of custom actions to display along with the built-in actions.
        /// </summary>
        public AccessibilityCustomAction[] AccessibilityCustomActions
        {
            set
            {
                _userAccessibilityCustomActionsProvided = true;
                m_userAccessibilityCustomActions = value;
            }
            get
            {
                return _accessibilityCustomActions();
            }
        }

        public Func<AccessibilityCustomAction[]> accessibilityCustomActionsDelegate = null;


        /// <summary>
        /// A Boolean value that indicates whether VoiceOver ignores the accessibility elements within views that are siblings of the element.
        /// </summary>
        public bool AccessibilityViewIsModal
        {
            set
            {
                _userAccessibilityViewIsModalProvided = true;
                m_userAccessibilityViewIsModal = value;
            }
            get
            {
                return _accessibilityViewIsModal();
            }
        }

        public Func<bool> accessibilityViewIsModalDelegate = null;


        /// <summary>
        /// The activation point for the accessibility element in screen coordinates.
        /// </summary>
        public Vector2 AccessibilityActivationPoint
        {
            set
            {
                _userAccessibilityActivationPointProvided = true;
                m_userAccessibilityActivationPoint = value;
            }
            get
            {
                return _accessibilityActivationPoint();
            }
        }

        public Func<Vector2> accessibilityActivationPointDelegate = null;


        /// <summary>
        /// Performs a salient action.
        /// </summary>
        public Func<bool> onAccessibilityPerformMagicTap = null;
        internal bool AccessibilityPerformMagicTap()
        {
            return _accessibilityPerformMagicTap();
        }

        /// <summary>
        /// Dismisses a modal view and returns the success or failure of the action.
        /// </summary>
        public Func<bool> onAccessibilityPerformEscape = null;
        internal bool AccessibilityPerformEscape()
        {
            return _accessibilityPerformEscape();
        }

        /// <summary>
        /// Tells the element to activate itself and report the success or failure of the operation.
        /// </summary>
        public Func<bool> onAccessibilityActivate = null;
        internal bool AccessibilityActivate()
        {
            return _accessibilityActivate();
        }

        /// <summary>
        /// Scrolls screen content in an application-specific way and returns the success or failure of the action.
        /// </summary>
        public Func<AccessibilityScrollDirection, bool> onAccessibilityScroll = null;
        internal bool AccessibilityScroll(AccessibilityScrollDirection direction)
        {
            return _accessibilityScroll(direction);
        }

        /// <summary>
        /// Tells the accessibility element to increment the value of its content.
        /// </summary>
        public Func<bool> onAccessibilityIncrement = null;
        internal bool AccessibilityIncrement()
        {
            return _accessibilityIncrement();
        }

        /// <summary>
        /// Tells the accessibility element to increment the value of its content.
        /// </summary>
        public Func<bool> onAccessibilityDecrement = null;
        internal bool AccessibilityDecrement()
        {
            return _accessibilityDecrement();
        }


        #region Private

        private void Start()
        {
            RegisterAXElement(this);
        }

        private void OnEnable()
        {
            RegisterAXElement(this);
        }

        private void OnDisable()
        {
            UnregisterAXElement(this);
        }

        private void OnDestroy()
        {
            UnregisterAXElement(this);
        }

#if UNITY_6000_4_OR_NEWER
        private static Dictionary<ulong, AccessibilityNode> axElements = new Dictionary<ulong, AccessibilityNode>();
#else
        private static Dictionary<int, AccessibilityNode> axElements = new Dictionary<int, AccessibilityNode>();
#endif

        static internal void RegisterAXElement(AccessibilityNode obj)
        {
#if UNITY_6000_4_OR_NEWER
            if (axElements.ContainsKey(EntityId.ToULong(obj.gameObject.GetEntityId())))
#else
            if (axElements.ContainsKey(obj.gameObject.GetInstanceID()))
#endif
            {
                return;
            }

#if UNITY_6000_4_OR_NEWER
            axElements.Add(EntityId.ToULong(obj.gameObject.GetEntityId()), obj);
            AccessibilityNode parent = obj._accessibilityParent();
            ulong parentId = EntityId.ToULong(EntityId.None);
#else
            axElements.Add(obj.gameObject.GetInstanceID(), obj);
            AccessibilityNode parent = obj._accessibilityParent();
            int parentId = -1;
#endif

            if (parent)
            {
#if UNITY_6000_4_OR_NEWER
                parentId = EntityId.ToULong(parent.gameObject.GetEntityId());
#else
                parentId = parent.gameObject.GetInstanceID();
#endif
            }

#if (UNITY_IOS || UNITY_TVOS) && !UNITY_EDITOR
#if UNITY_6000_4_OR_NEWER
            _UnityAX_RegisterElementWithIdentifier2(EntityId.ToULong(obj.gameObject.GetEntityId()), parentId, parent != null);
#else
            _UnityAX_RegisterElementWithIdentifier(obj.gameObject.GetInstanceID(), parentId, parent != null);
#endif
#endif
        }

        static internal void UnregisterAXElement(AccessibilityNode obj)
        {
#if UNITY_6000_4_OR_NEWER
            axElements.Remove(EntityId.ToULong(obj.gameObject.GetEntityId()));
#else
            axElements.Remove(obj.gameObject.GetInstanceID());
#endif
#if (UNITY_IOS || UNITY_TVOS) && !UNITY_EDITOR
#if UNITY_6000_4_OR_NEWER
            _UnityAX_UnregisterElementWithIdentifier2(EntityId.ToULong(obj.gameObject.GetEntityId()));
#else
            _UnityAX_UnregisterElementWithIdentifier(obj.gameObject.GetInstanceID());
#endif
#endif
        }

        private readonly Dictionary<string, (Component, Component)> cachedAttributeComponentMap = new Dictionary<string, (Component, Component)>();

        private T GetAccessibilityAttribute<T>(bool userProvided, T staticProvider, Func<T> staticDelegateProvider, string methodName, Func<Component, T> defaultProvider, bool recursiveSearch = false)
        {
            T func(int _)
            {
                return staticDelegateProvider();
            }
            return GetAccessibilityAttribute(userProvided, staticProvider, staticDelegateProvider != null ? func : (Func<int, T>)null, 0, methodName, defaultProvider, recursiveSearch);
        }

        private T GetAccessibilityAttribute<TParam, T>(bool userProvided, T staticProvider, Func<TParam, T> staticDelegateProvider, TParam param, string methodName, Func<Component, T> defaultProvider, bool recursiveSearch = false)
        {
            if (staticDelegateProvider != null)
            {
                return staticDelegateProvider(param);
            }
            else if (userProvided)
            {
                return staticProvider;
            }
            else
            {
                if (!cachedAttributeComponentMap.ContainsKey(methodName))
                {
                    Component component = null;
                    var candidates = GetComponents<Component>();
                    foreach (Component c in candidates)
                    {
                        if (c != this)
                        {
                            var type = c.GetType();
                            var mi = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                            if (mi != null && mi.ReturnType == typeof(T))
                            {
                                component = c;
                                cachedAttributeComponentMap[methodName] = (c, null);
                                break;
                            }
                            if (component == null)
                            {
                                var pi = type.GetProperty(methodName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                                if (pi != null && pi.PropertyType == typeof(T))
                                {
                                    component = c;
                                    cachedAttributeComponentMap[methodName] = (c, null);
                                    break;
                                }
                            }
                        }
                    }
                    if (component == null)
                    {
                        if (recursiveSearch)
                        {
                            candidates = GetComponentsInChildren<Component>();
                        }
                        foreach (Component c in candidates)
                        {
                            Type[] types = { c.GetType(), gameObject.GetType() };
                            var mi = typeof(AccessibilityComponentExtensions).GetMethod(methodName + "ForComponent", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public, null, types, null);
                            if (mi != null && mi.ReturnType == typeof(T))
                            {
                                component = c;
                                cachedAttributeComponentMap[methodName] = (null, c);
                                break;
                            }
                        }
                    }
                    if (component == null)
                    {
                        cachedAttributeComponentMap[methodName] = (null, null);
                    }
                }
                if (cachedAttributeComponentMap.ContainsKey(methodName))
                {
                    var pair = cachedAttributeComponentMap[methodName];
                    if (pair.Item1 != null)
                    {
                        MethodInfo mi = pair.Item1.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.GetField);
                        if (mi != null && mi.ReturnType == typeof(T))
                        {
                            return (T)mi.Invoke(pair.Item1, null);
                        }
                        var pi = pair.Item1.GetType().GetProperty(methodName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                        if (pi != null && pi.PropertyType == typeof(T))
                        {
                            return (T)pi.GetValue(pair.Item1);
                        }
                    }
                    else if (pair.Item2 != null)
                    {
                        Type[] types = { pair.Item2.GetType(), gameObject.GetType() };
                        var mi = typeof(AccessibilityComponentExtensions).GetMethod(methodName + "ForComponent", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public, null, types, null);
                        if (mi != null && mi.ReturnType == typeof(T))
                        {
                            object[] parameters = { pair.Item2, gameObject };
                            return (T)mi.Invoke(null, parameters);
                        }
                    }
                    else
                    {
                        return defaultProvider(this);
                    }
                }
            }
            return defaultProvider(this);
        }

        internal AccessibilityNode _accessibilityParent()
        {
            foreach (AccessibilityNode parent in gameObject.GetComponentsInParent<AccessibilityNode>(true))
            {
                if (parent.gameObject != gameObject)
                {
                    return parent;
                }
            }
            return null;
        }

        [SerializeField] private bool _userIsAccessibilityElementProvided = true;
        [SerializeField] private bool m_userIsAccessibilityElement = true;
        internal bool _isAccessibilityElement()
        {
            bool defaultProvider(Component c)
            {
                return false;
            }
            return GetAccessibilityAttribute(_userIsAccessibilityElementProvided, m_userIsAccessibilityElement, isAccessibilityElementDelegate, "IsAccessibilityElement", defaultProvider);
        }


        [SerializeField] private bool _userAccessibilityTraitsProvided = true;
        [SerializeField] private AccessibilityTrait m_userAccessibilityTraits = AccessibilityTrait.None;
        internal AccessibilityTrait _accessibilityTraits()
        {
            AccessibilityTrait defaultProvider(Component c)
            {
                if (c is Behaviour)
                {
                    return ((Behaviour)c).enabled ? AccessibilityTrait.NotEnabled : AccessibilityTrait.None;
                }
                return AccessibilityTrait.None;
            }
            return GetAccessibilityAttribute(_userAccessibilityTraitsProvided, m_userAccessibilityTraits, accessibilityTraitsDelegate, "AccessibilityTraits", defaultProvider);
        }


        private bool _userAccessibilityFrameProvided = false;
        private Rect m_userAccessibilityFrame;
        internal Rect _accessibilityFrame()
        {
            Rect defaultProvider(Component c)
            {
                Rect defaultFrame = Rect.zero;
                foreach (Renderer r in c.GetComponents<Renderer>())
                {
                    Vector3 center = r.bounds.center;
                    Vector3 extents = r.bounds.extents;

                    Vector3[] vertices = new[] {
                        new Vector3( center.x + extents.x, center.y + extents.y, center.z + extents.z ),
                        new Vector3( center.x + extents.x, center.y + extents.y, center.z - extents.z ),
                        new Vector3( center.x + extents.x, center.y - extents.y, center.z + extents.z ),
                        new Vector3( center.x + extents.x, center.y - extents.y, center.z - extents.z ),
                        new Vector3( center.x - extents.x, center.y + extents.y, center.z + extents.z ),
                        new Vector3( center.x - extents.x, center.y + extents.y, center.z - extents.z ),
                        new Vector3( center.x - extents.x, center.y - extents.y, center.z + extents.z ),
                        new Vector3( center.x - extents.x, center.y - extents.y, center.z - extents.z ),
                    };

                    IEnumerable<Vector3> screenVertices = vertices.Select(corner => Camera.main.WorldToScreenPoint(corner));
                    float maxX = screenVertices.Max(corner => corner.x);
                    float minX = screenVertices.Min(corner => corner.x);
                    float maxY = screenVertices.Max(corner => corner.y);
                    float minY = screenVertices.Min(corner => corner.y);

                    defaultFrame = new Rect(minX, Screen.height - maxY, maxX - minX, maxY - minY);
                    break;
                }
                return defaultFrame;
            }
            return GetAccessibilityAttribute(_userAccessibilityFrameProvided, m_userAccessibilityFrame, accessibilityFrameDelegate, "AccessibilityFrame", defaultProvider, true);
        }


        [SerializeField] private bool _userAccessibilityLabelProvided = true;
        [SerializeField] private string m_userAccessibilityLabel;
        internal string _accessibilityLabel()
        {
            string defaultProvider(Component c)
            {
                return null;
            }
            return GetAccessibilityAttribute(_userAccessibilityLabelProvided, m_userAccessibilityLabel, accessibilityLabelDelegate, "AccessibilityLabel", defaultProvider);
        }


        [SerializeField] private bool _userAccessibilityValueProvided = false;
        [SerializeField] private string m_userAccessibilityValue;
        internal string _accessibilityValue()
        {
            string defaultProvider(Component c)
            {
                return null;
            }
            return GetAccessibilityAttribute(_userAccessibilityValueProvided, m_userAccessibilityValue, accessibilityValueDelegate, "AccessibilityValue", defaultProvider);
        }


        [SerializeField] private bool _userAccessibilityHintProvided = false;
        [SerializeField] private string m_userAccessibilityHint;
        internal string _accessibilityHint()
        {
            string defaultProvider(Component c)
            {
                return null;
            }
            return GetAccessibilityAttribute(_userAccessibilityHintProvided, m_userAccessibilityHint, accessibilityHintDelegate, "AccessibilityHint", defaultProvider);
        }


        [SerializeField] private bool _userAccessibilityIdentifierProvided = false;
        [SerializeField] private string m_userAccessibilityIdentifier;
        internal string _accessibilityIdentifier()
        {
            string defaultProvider(Component c)
            {
                return null;
            }
            return GetAccessibilityAttribute(_userAccessibilityIdentifierProvided, m_userAccessibilityIdentifier, accessibilityIdentifierDelegate, "AccessibilityIdentifier", defaultProvider);
        }


        private bool _userAccessibilityActivationPointProvided = false;
        private Vector2 m_userAccessibilityActivationPoint;
        internal Vector2 _accessibilityActivationPoint()
        {
            Vector2 defaultProvider(Component c)
            {
                // use positiveInfinity to indicate no-value.
                return Vector2.positiveInfinity;
            }
            return GetAccessibilityAttribute(_userAccessibilityActivationPointProvided, m_userAccessibilityActivationPoint, accessibilityActivationPointDelegate, "AccessibilityActivationPoint", defaultProvider);
        }


        private bool _userAccessibilityCustomActionsProvided = false;
        private AccessibilityCustomAction[] m_userAccessibilityCustomActions;
        internal AccessibilityCustomAction[] _accessibilityCustomActions()
        {
            AccessibilityCustomAction[] defaultProvider(Component c)
            {
                return null;
            }
            return GetAccessibilityAttribute(_userAccessibilityCustomActionsProvided, m_userAccessibilityCustomActions, accessibilityCustomActionsDelegate, "AccessibilityCustomActions", defaultProvider);
        }


        [SerializeField] private bool _userAccessibilityViewIsModalProvided = false;
        [SerializeField] private bool m_userAccessibilityViewIsModal;
        internal bool _accessibilityViewIsModal()
        {
            bool defaultProvider(Component c)
            {
                return false;
            }
            return GetAccessibilityAttribute(_userAccessibilityViewIsModalProvided, m_userAccessibilityViewIsModal, accessibilityViewIsModalDelegate, "AccessibilityViewIsModal", defaultProvider);
        }


        internal bool _accessibilityIncrement()
        {
            bool defaultProvider(Component c)
            {
                return false;
            }
            return GetAccessibilityAttribute(false, false, onAccessibilityIncrement, "AccessibilityIncrement", defaultProvider);
        }

        internal bool _accessibilityDecrement()
        {
            bool defaultProvider(Component c)
            {
                return false;
            }
            return GetAccessibilityAttribute(false, false, onAccessibilityDecrement, "AccessibilityDecrement", defaultProvider);
        }

        internal bool _accessibilityScroll(AccessibilityScrollDirection direction)
        {
            bool defaultProvider(Component c)
            {
                return false;
            }
            if (!GetAccessibilityAttribute(false, false, onAccessibilityScroll, direction, "AccessibilityScroll", defaultProvider))
            {
                foreach (AccessibilityNode parent in gameObject.GetComponentsInParent<AccessibilityNode>())
                {
                    if (parent.GetAccessibilityAttribute(false, false, parent.onAccessibilityScroll, direction, "AccessibilityScroll", defaultProvider))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        internal bool _accessibilityPerformEscape()
        {
            bool defaultProvider(Component c)
            {
                return false;
            }
            if (!GetAccessibilityAttribute(false, false, onAccessibilityPerformEscape, "AccessibilityPerformEscape", defaultProvider))
            {
                foreach (AccessibilityNode parent in gameObject.GetComponentsInParent<AccessibilityNode>())
                {
                    if (parent.GetAccessibilityAttribute(false, false, parent.onAccessibilityPerformEscape, "AccessibilityPerformEscape", defaultProvider))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        internal bool _accessibilityPerformMagicTap()
        {
            bool defaultProvider(Component c)
            {
                return false;
            }
            if (!GetAccessibilityAttribute(false, false, onAccessibilityPerformMagicTap, "AccessibilityPerformMagicTap", defaultProvider))
            {
                foreach (AccessibilityNode parent in gameObject.GetComponentsInParent<AccessibilityNode>())
                {
                    if (parent.GetAccessibilityAttribute(false, false, parent.onAccessibilityPerformMagicTap, "AccessibilityPerformMagicTap", defaultProvider))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        internal bool _accessibilityActivate()
        {
            bool defaultProvider(Component c)
            {
                return false;
            }
            if (!GetAccessibilityAttribute(false, false, onAccessibilityActivate, "AccessibilityActivate", defaultProvider))
            {
                foreach (AccessibilityNode parent in gameObject.GetComponentsInParent<AccessibilityNode>())
                {
                    if (parent.GetAccessibilityAttribute(false, false, parent.onAccessibilityActivate, "AccessibilityActivate", defaultProvider))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static string _stringForRect(Rect rect)
        {
            return "{{" + $"{rect.x}, {rect.y}" + "}, {" + $"{rect.width}, {rect.height}" + "}}";
        }

        private static string _stringForPoint(Vector2 point)
        {
            return "{" + $"{point.x}, {point.y}" + "}";
        }

        internal static bool DebugUIEnabled()
        {
            return true;
        }

        private void OnDrawGizmos()
        {
            if (!DebugUIEnabled())
            {
                return;
            }

            GUIStyle style = new GUIStyle();
            Color color = Color.red;
            style.normal.textColor = color;
#if UNITY_EDITOR
            UnityEditor.Handles.color = color;
            if (_isAccessibilityElement())
            {
                UnityEditor.Handles.Label(transform.position, $"Traits:{_accessibilityTraits()} Label:{_accessibilityLabel()} Value:{_accessibilityValue()} Frame:{_accessibilityFrame()}", style);
            }

            Camera cam = Camera.main;
            Vector3 worldPos = transform.position;
            Vector3 screenPos = cam.WorldToScreenPoint(worldPos);
            Vector3 ul = screenPos;
            ul.x -= _accessibilityFrame().width / 2.0f;
            ul.y += _accessibilityFrame().height / 2.0f;
            ul = cam.ScreenToWorldPoint(ul);

            Vector3 ur = screenPos;
            ur.x += _accessibilityFrame().width / 2.0f;
            ur.y += _accessibilityFrame().height / 2.0f;
            ur = cam.ScreenToWorldPoint(ur);

            UnityEditor.Handles.DrawLine(transform.position, ur);
#endif
        }

        #endregion

        #region Native Bridge

        private static bool __registered = false;
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void RegisterCallbacks()
        {
            if (!__registered)
            {
#if (UNITY_IOS || UNITY_TVOS) && !UNITY_EDITOR
#if UNITY_6000_4_OR_NEWER
                _UnityAX_InitializeAXRuntime(2);
                _UnityAX_registerAccessibilityFrame2(_UnityAX_accessibilityFrame2);
                _UnityAX_registerAccessibilityLabel2(_UnityAX_accessibilityLabel2);
                _UnityAX_registerAccessibilityTraits2(_UnityAX_accessibilityTraits2);
                _UnityAX_registerAccessibilityIsElement2(_UnityAX_IsAccessibilityElement2);
                _UnityAX_registerAccessibilityHint2(_UnityAX_AccessibilityHint2);
                _UnityAX_registerAccessibilityValue2(_UnityAX_AccessibilityValue2);
                _UnityAX_registerAccessibilityIdentifier2(_UnityAX_AccessibilityIdentifier2);
                _UnityAX_registerAccessibilityViewIsModal2(_UnityAX_AccessibilityViewIsModal2);
                _UnityAX_registerAccessibilityActivationPoint2(_UnityAX_AccessibilityActivationPoint2);
                _UnityAX_registerAccessibilityPerformMagicTap2(_UnityAX_AccessibilityPerformMagicTap2);
                _UnityAX_registerAccessibilityPerformEscape2(_UnityAX_AccessibilityPerformEscape2);
                _UnityAX_registerAccessibilityActivate2(_UnityAX_AccessibilityActivate2);
                _UnityAX_registerAccessibilityIncrement2(_UnityAX_AccessibilityIncrement2);
                _UnityAX_registerAccessibilityDecrement2(_UnityAX_AccessibilityDecrement2);
                _UnityAX_registerAccessibilityScroll2(_UnityAX_AccessibilityScroll2);
                _UnityAX_registerAccessibilityCustomActionsCount2(_UnityAX_AccessibilityCustomActionCount2);
                _UnityAX_registerAccessibilityPerformCustomAction2(_UnityAX_PerformCustomAction2);
                _UnityAX_registerAccessibilityCustomActionName2(_UnityAX_CustomActionName2);
#else
                _UnityAX_InitializeAXRuntime(0);
                _UnityAX_registerAccessibilityFrame(_UnityAX_accessibilityFrame);
                _UnityAX_registerAccessibilityLabel(_UnityAX_accessibilityLabel);
                _UnityAX_registerAccessibilityTraits(_UnityAX_accessibilityTraits);
                _UnityAX_registerAccessibilityIsElement(_UnityAX_IsAccessibilityElement);
                _UnityAX_registerAccessibilityHint(_UnityAX_AccessibilityHint);
                _UnityAX_registerAccessibilityValue(_UnityAX_AccessibilityValue);
                _UnityAX_registerAccessibilityIdentifier(_UnityAX_AccessibilityIdentifier);
                _UnityAX_registerAccessibilityViewIsModal(_UnityAX_AccessibilityViewIsModal);
                _UnityAX_registerAccessibilityActivationPoint(_UnityAX_AccessibilityActivationPoint);
                _UnityAX_registerAccessibilityPerformMagicTap(_UnityAX_AccessibilityPerformMagicTap);
                _UnityAX_registerAccessibilityPerformEscape(_UnityAX_AccessibilityPerformEscape);
                _UnityAX_registerAccessibilityActivate(_UnityAX_AccessibilityActivate);
                _UnityAX_registerAccessibilityIncrement(_UnityAX_AccessibilityIncrement);
                _UnityAX_registerAccessibilityDecrement(_UnityAX_AccessibilityDecrement);
                _UnityAX_registerAccessibilityScroll(_UnityAX_AccessibilityScroll);
                _UnityAX_registerAccessibilityCustomActionsCount(_UnityAX_AccessibilityCustomActionCount);
                _UnityAX_registerAccessibilityPerformCustomAction(_UnityAX_PerformCustomAction);
                _UnityAX_registerAccessibilityCustomActionName(_UnityAX_CustomActionName);
#endif
#endif
                __registered = true;
            }
        }

#if (UNITY_IOS || UNITY_TVOS) && !UNITY_EDITOR
        // Parameter callbackOption tells native code which set of calbacks to use. Valid values: 0, 2
        [DllImport("__Internal")] private static extern void _UnityAX_InitializeAXRuntime(int callbackOption);
#if UNITY_6000_4_OR_NEWER
        [DllImport("__Internal")] private static extern void _UnityAX_RegisterElementWithIdentifier2(ulong identifier, ulong parentIdentifier, bool hasParent);
        [DllImport("__Internal")] private static extern void _UnityAX_UnregisterElementWithIdentifier2(ulong identifier);
#else
        [DllImport("__Internal")] private static extern void _UnityAX_RegisterElementWithIdentifier(int identifier, int parentIdentifier, bool hasParent);
        [DllImport("__Internal")] private static extern void _UnityAX_UnregisterElementWithIdentifier(int identifier);
#endif

#if UNITY_6000_4_OR_NEWER
        private delegate string AccessibilityFrameDelegate2(ulong identifier);
        [DllImport("__Internal")] private static extern void _UnityAX_registerAccessibilityFrame2(AccessibilityFrameDelegate2 actionDelegate);
        [AOT.MonoPInvokeCallback(typeof(AccessibilityFrameDelegate2))]
        private static string _UnityAX_accessibilityFrame2(ulong identifier)
#else
        private delegate string AccessibilityFrameDelegate(int identifier);
        [DllImport("__Internal")] private static extern void _UnityAX_registerAccessibilityFrame(AccessibilityFrameDelegate actionDelegate);
        [AOT.MonoPInvokeCallback(typeof(AccessibilityFrameDelegate))]
        private static string _UnityAX_accessibilityFrame(int identifier)
#endif
        {
            Rect rect = Rect.zero;
            if (axElements.TryGetValue(identifier, out AccessibilityNode obj))
            {
                rect = obj._accessibilityFrame();
            }
            return _stringForRect(rect);
        }

#if UNITY_6000_4_OR_NEWER
        private delegate string AccessibilityLabelDelegate2(ulong identifier);
        [DllImport("__Internal")] private static extern void _UnityAX_registerAccessibilityLabel2(AccessibilityLabelDelegate2 actionDelegate);
        [AOT.MonoPInvokeCallback(typeof(AccessibilityLabelDelegate2))]
        private static string _UnityAX_accessibilityLabel2(ulong identifier)
#else
        private delegate string AccessibilityLabelDelegate(int identifier);
        [DllImport("__Internal")] private static extern void _UnityAX_registerAccessibilityLabel(AccessibilityLabelDelegate actionDelegate);
        [AOT.MonoPInvokeCallback(typeof(AccessibilityLabelDelegate))]
        private static string _UnityAX_accessibilityLabel(int identifier)
#endif
        {
            if (axElements.TryGetValue(identifier, out AccessibilityNode obj))
            {
                return obj._accessibilityLabel();
            }
            return null;
        }

#if UNITY_6000_4_OR_NEWER
        private delegate ulong AccessibilityTraitsDelegate2(ulong identifier);
        [DllImport("__Internal")] private static extern void _UnityAX_registerAccessibilityTraits2(AccessibilityTraitsDelegate2 actionDelegate);
        [AOT.MonoPInvokeCallback(typeof(AccessibilityTraitsDelegate2))]
        private static ulong _UnityAX_accessibilityTraits2(ulong identifier)
#else
        private delegate ulong AccessibilityTraitsDelegate(int identifier);
        [DllImport("__Internal")] private static extern void _UnityAX_registerAccessibilityTraits(AccessibilityTraitsDelegate actionDelegate);
        [AOT.MonoPInvokeCallback(typeof(AccessibilityTraitsDelegate))]
        private static ulong _UnityAX_accessibilityTraits(int identifier)
#endif
        {
            if (axElements.TryGetValue(identifier, out AccessibilityNode obj))
            {
                return (ulong)obj._accessibilityTraits();
            }
            return 0;
        }

#if UNITY_6000_4_OR_NEWER
        private delegate bool IsAccessibilityElementDelegate2(ulong identifier);
        [DllImport("__Internal")] private static extern void _UnityAX_registerAccessibilityIsElement2(IsAccessibilityElementDelegate2 actionDelegate);
        [AOT.MonoPInvokeCallback(typeof(IsAccessibilityElementDelegate2))]
        private static bool _UnityAX_IsAccessibilityElement2(ulong identifier)
#else
        private delegate bool IsAccessibilityElementDelegate(int identifier);
        [DllImport("__Internal")] private static extern void _UnityAX_registerAccessibilityIsElement(IsAccessibilityElementDelegate actionDelegate);
        [AOT.MonoPInvokeCallback(typeof(IsAccessibilityElementDelegate))]
        private static bool _UnityAX_IsAccessibilityElement(int identifier)
#endif
        {
            if (axElements.TryGetValue(identifier, out AccessibilityNode obj))
            {
                return obj._isAccessibilityElement();
            }
            return true;
        }

#if UNITY_6000_4_OR_NEWER
        private delegate string AccessibilityHintDelegate2(ulong identifier);
        [DllImport("__Internal")] private static extern void _UnityAX_registerAccessibilityHint2(AccessibilityHintDelegate2 actionDelegate);
        [AOT.MonoPInvokeCallback(typeof(AccessibilityHintDelegate2))]
        private static string _UnityAX_AccessibilityHint2(ulong identifier)
#else
        private delegate string AccessibilityHintDelegate(int identifier);
        [DllImport("__Internal")] private static extern void _UnityAX_registerAccessibilityHint(AccessibilityHintDelegate actionDelegate);
        [AOT.MonoPInvokeCallback(typeof(AccessibilityHintDelegate))]
        private static string _UnityAX_AccessibilityHint(int identifier)
#endif
        {
            if (axElements.TryGetValue(identifier, out AccessibilityNode obj))
            {
                return obj._accessibilityHint();
            }
            return null;
        }

#if UNITY_6000_4_OR_NEWER
        private delegate string AccessibilityValueDelegate2(ulong identifier);
        [DllImport("__Internal")] private static extern void _UnityAX_registerAccessibilityValue2(AccessibilityValueDelegate2 actionDelegate);
        [AOT.MonoPInvokeCallback(typeof(AccessibilityValueDelegate2))]
        private static string _UnityAX_AccessibilityValue2(ulong identifier)
#else
        private delegate string AccessibilityValueDelegate(int identifier);
        [DllImport("__Internal")] private static extern void _UnityAX_registerAccessibilityValue(AccessibilityValueDelegate actionDelegate);
        [AOT.MonoPInvokeCallback(typeof(AccessibilityValueDelegate))]
        private static string _UnityAX_AccessibilityValue(int identifier)
#endif
        {
            if (axElements.TryGetValue(identifier, out AccessibilityNode obj))
            {
                return obj._accessibilityValue();
            }
            return null;
        }

#if UNITY_6000_4_OR_NEWER
        private delegate string AccessibilityIdentifierDelegate2(ulong identifier);
        [DllImport("__Internal")] private static extern void _UnityAX_registerAccessibilityIdentifier2(AccessibilityIdentifierDelegate2 identifierDelegate);
        [AOT.MonoPInvokeCallback(typeof(AccessibilityIdentifierDelegate2))]
        private static string _UnityAX_AccessibilityIdentifier2(ulong identifier)
#else
        private delegate string AccessibilityIdentifierDelegate(int identifier);
        [DllImport("__Internal")] private static extern void _UnityAX_registerAccessibilityIdentifier(AccessibilityIdentifierDelegate identifierDelegate);
        [AOT.MonoPInvokeCallback(typeof(AccessibilityIdentifierDelegate))]
        private static string _UnityAX_AccessibilityIdentifier(int identifier)
#endif
        {
            if (axElements.TryGetValue(identifier, out AccessibilityNode obj))
            {
                return obj._accessibilityIdentifier();
            }
            return null;
        }

#if UNITY_6000_4_OR_NEWER
        private delegate bool AccessibilityViewIsModalDelegate2(ulong identifier);
        [DllImport("__Internal")] private static extern void _UnityAX_registerAccessibilityViewIsModal2(AccessibilityViewIsModalDelegate2 viewIsModalDelegate);
        [AOT.MonoPInvokeCallback(typeof(AccessibilityViewIsModalDelegate2))]
        private static bool _UnityAX_AccessibilityViewIsModal2(ulong identifier)
#else
        private delegate bool AccessibilityViewIsModalDelegate(int identifier);
        [DllImport("__Internal")] private static extern void _UnityAX_registerAccessibilityViewIsModal(AccessibilityViewIsModalDelegate viewIsModalDelegate);
        [AOT.MonoPInvokeCallback(typeof(AccessibilityViewIsModalDelegate))]
        private static bool _UnityAX_AccessibilityViewIsModal(int identifier)
#endif
        {
            if (axElements.TryGetValue(identifier, out AccessibilityNode obj))
            {
                return obj._accessibilityViewIsModal();
            }
            return false;
        }

#if UNITY_6000_4_OR_NEWER
        private delegate string AccessibilityActivationPointDelegate2(ulong identifier);
        [DllImport("__Internal")] private static extern void _UnityAX_registerAccessibilityActivationPoint2(AccessibilityActivationPointDelegate2 actionDelegate);
        [AOT.MonoPInvokeCallback(typeof(AccessibilityActivationPointDelegate2))]
        private static string _UnityAX_AccessibilityActivationPoint2(ulong identifier)
#else
        private delegate string AccessibilityActivationPointDelegate(int identifier);
        [DllImport("__Internal")] private static extern void _UnityAX_registerAccessibilityActivationPoint(AccessibilityActivationPointDelegate actionDelegate);
        [AOT.MonoPInvokeCallback(typeof(AccessibilityActivationPointDelegate))]
        private static string _UnityAX_AccessibilityActivationPoint(int identifier)
#endif
        {
            Vector2 value = Vector2.positiveInfinity;
            if (axElements.TryGetValue(identifier, out AccessibilityNode obj))
            {
                value = obj._accessibilityActivationPoint();
            }
            return value == Vector2.positiveInfinity ? null : _stringForPoint(value);
        }

#if UNITY_6000_4_OR_NEWER
        private delegate bool AccessibilityScrollDelegate2(ulong identifier, int direction);
        [DllImport("__Internal")] private static extern void _UnityAX_registerAccessibilityScroll2(AccessibilityScrollDelegate2 actionDelegate);
        [AOT.MonoPInvokeCallback(typeof(AccessibilityScrollDelegate2))]
        private static bool _UnityAX_AccessibilityScroll2(ulong identifier, int direction)
#else
        private delegate bool AccessibilityScrollDelegate(int identifier, int direction);
        [DllImport("__Internal")] private static extern void _UnityAX_registerAccessibilityScroll(AccessibilityScrollDelegate actionDelegate);
        [AOT.MonoPInvokeCallback(typeof(AccessibilityScrollDelegate))]
        private static bool _UnityAX_AccessibilityScroll(int identifier, int direction)
#endif
        {
            if (axElements.TryGetValue(identifier, out AccessibilityNode obj))
            {
                return obj.AccessibilityScroll((AccessibilityScrollDirection)direction);
            }
            return false;
        }

#if UNITY_6000_4_OR_NEWER
        private delegate void AccessibilityIncrementDelegate2(ulong identifier);
        [DllImport("__Internal")] private static extern void _UnityAX_registerAccessibilityIncrement2(AccessibilityIncrementDelegate2 actionDelegate);
        [AOT.MonoPInvokeCallback(typeof(AccessibilityIncrementDelegate2))]
        private static void _UnityAX_AccessibilityIncrement2(ulong identifier)
#else
        private delegate void AccessibilityIncrementDelegate(int identifier);
        [DllImport("__Internal")] private static extern void _UnityAX_registerAccessibilityIncrement(AccessibilityIncrementDelegate actionDelegate);
        [AOT.MonoPInvokeCallback(typeof(AccessibilityIncrementDelegate))]
        private static void _UnityAX_AccessibilityIncrement(int identifier)
#endif
        {
            if (axElements.TryGetValue(identifier, out AccessibilityNode obj))
            {
                obj.AccessibilityIncrement();
            }
        }

#if UNITY_6000_4_OR_NEWER
        private delegate void AccessibilityDecrementDelegate2(ulong identifier);
        [DllImport("__Internal")] private static extern void _UnityAX_registerAccessibilityDecrement2(AccessibilityDecrementDelegate2 actionDelegate);
        [AOT.MonoPInvokeCallback(typeof(AccessibilityDecrementDelegate2))]
        private static void _UnityAX_AccessibilityDecrement2(ulong identifier)
#else
        private delegate void AccessibilityDecrementDelegate(int identifier);
        [DllImport("__Internal")] private static extern void _UnityAX_registerAccessibilityDecrement(AccessibilityDecrementDelegate actionDelegate);
        [AOT.MonoPInvokeCallback(typeof(AccessibilityDecrementDelegate))]
        private static void _UnityAX_AccessibilityDecrement(int identifier)
#endif
        {
            if (axElements.TryGetValue(identifier, out AccessibilityNode obj))
            {
                obj.AccessibilityDecrement();
            }
        }

#if UNITY_6000_4_OR_NEWER
        private delegate ulong AccessibilityCustomActionsCountDelegate2(ulong identifier);
        [DllImport("__Internal")] private static extern void _UnityAX_registerAccessibilityCustomActionsCount2(AccessibilityCustomActionsCountDelegate2 actionDelegate);
        [AOT.MonoPInvokeCallback(typeof(AccessibilityCustomActionsCountDelegate2))]
        private static ulong _UnityAX_AccessibilityCustomActionCount2(ulong identifier)
#else
        private delegate ulong AccessibilityCustomActionsCountDelegate(int identifier);
        [DllImport("__Internal")] private static extern void _UnityAX_registerAccessibilityCustomActionsCount(AccessibilityCustomActionsCountDelegate actionDelegate);
        [AOT.MonoPInvokeCallback(typeof(AccessibilityCustomActionsCountDelegate))]
        private static ulong _UnityAX_AccessibilityCustomActionCount(int identifier)
#endif
        {
            if (axElements.TryGetValue(identifier, out AccessibilityNode obj))
            {
                var actions = obj._accessibilityCustomActions();
                if (actions != null)
                {
                    return (ulong)actions.Length;
                }
            }
            return 0;
        }

#if UNITY_6000_4_OR_NEWER
        private delegate string AccessibilityCustomActionNameDelegate2(ulong identifier, int idx);
        [DllImport("__Internal")] private static extern void _UnityAX_registerAccessibilityCustomActionName2(AccessibilityCustomActionNameDelegate2 actionDelegate);
        [AOT.MonoPInvokeCallback(typeof(AccessibilityCustomActionNameDelegate2))]
        private static string _UnityAX_CustomActionName2(ulong identifier, int idx)
#else
        private delegate string AccessibilityCustomActionNameDelegate(int identifier, int idx);
        [DllImport("__Internal")] private static extern void _UnityAX_registerAccessibilityCustomActionName(AccessibilityCustomActionNameDelegate actionDelegate);
        [AOT.MonoPInvokeCallback(typeof(AccessibilityCustomActionNameDelegate))]
        private static string _UnityAX_CustomActionName(int identifier, int idx)
#endif
        {
            if (axElements.TryGetValue(identifier, out AccessibilityNode obj))
            {
                AccessibilityCustomAction[] actions = obj._accessibilityCustomActions();
                if (actions != null)
                {
                    if (actions.Length <= idx)
                    {
                        return idx.ToString();
                    }
                    else
                    {
                        return actions[idx].name;
                    }
                }
            }
            return null;
        }

#if UNITY_6000_4_OR_NEWER
        private delegate bool AccessibilityPerformCustomActionDelegate2(ulong identifier, int idx);
        [DllImport("__Internal")] private static extern void _UnityAX_registerAccessibilityPerformCustomAction2(AccessibilityPerformCustomActionDelegate2 actionDelegate);
        [AOT.MonoPInvokeCallback(typeof(AccessibilityPerformCustomActionDelegate2))]
        private static bool _UnityAX_PerformCustomAction2(ulong identifier, int idx)
#else
        private delegate bool AccessibilityPerformCustomActionDelegate(int identifier, int idx);
        [DllImport("__Internal")] private static extern void _UnityAX_registerAccessibilityPerformCustomAction(AccessibilityPerformCustomActionDelegate actionDelegate);
        [AOT.MonoPInvokeCallback(typeof(AccessibilityPerformCustomActionDelegate))]
        private static bool _UnityAX_PerformCustomAction(int identifier, int idx)
#endif
        {
            if (axElements.TryGetValue(identifier, out AccessibilityNode obj))
            {
                AccessibilityCustomAction[] actions = obj._accessibilityCustomActions();
                if (actions != null)
                {
                    if (actions.Length <= idx)
                    {
                        return false;
                    }
                    else
                    {
                        return actions[idx].onAction();
                    }
                }
            }
            return false;
        }

#if UNITY_6000_4_OR_NEWER
        private delegate bool AccessibilityPerformMagicTapDelegate2(ulong identifier);
        [DllImport("__Internal")] private static extern void _UnityAX_registerAccessibilityPerformMagicTap2(AccessibilityPerformMagicTapDelegate2 actionDelegate);
        [AOT.MonoPInvokeCallback(typeof(AccessibilityPerformMagicTapDelegate2))]
        internal static bool _UnityAX_AccessibilityPerformMagicTap2(ulong identifier)
#else
        private delegate bool AccessibilityPerformMagicTapDelegate(int identifier);
        [DllImport("__Internal")] private static extern void _UnityAX_registerAccessibilityPerformMagicTap(AccessibilityPerformMagicTapDelegate actionDelegate);
        [AOT.MonoPInvokeCallback(typeof(AccessibilityPerformMagicTapDelegate))]
        internal static bool _UnityAX_AccessibilityPerformMagicTap(int identifier)
#endif
        {
            if (axElements.TryGetValue(identifier, out AccessibilityNode obj))
            {
                return obj.AccessibilityPerformMagicTap();
            }
            return false;
        }

#if UNITY_6000_4_OR_NEWER
        private delegate bool AccessibilityPerformEscapeDelegate2(ulong identifier);
        [DllImport("__Internal")] private static extern void _UnityAX_registerAccessibilityPerformEscape2(AccessibilityPerformEscapeDelegate2 actionDelegate);
        [AOT.MonoPInvokeCallback(typeof(AccessibilityPerformEscapeDelegate2))]
        internal static bool _UnityAX_AccessibilityPerformEscape2(ulong identifier)
#else
        private delegate bool AccessibilityPerformEscapeDelegate(int identifier);
        [DllImport("__Internal")] private static extern void _UnityAX_registerAccessibilityPerformEscape(AccessibilityPerformEscapeDelegate actionDelegate);
        [AOT.MonoPInvokeCallback(typeof(AccessibilityPerformEscapeDelegate))]
        internal static bool _UnityAX_AccessibilityPerformEscape(int identifier)
#endif
        {
            if (axElements.TryGetValue(identifier, out AccessibilityNode obj))
            {
                return obj.AccessibilityPerformEscape();
            }
            return false;
        }

#if UNITY_6000_4_OR_NEWER
        private delegate bool AccessibilityActivateDelegate2(ulong identifier);
        [DllImport("__Internal")] private static extern void _UnityAX_registerAccessibilityActivate2(AccessibilityActivateDelegate2 actionDelegate);
        [AOT.MonoPInvokeCallback(typeof(AccessibilityActivateDelegate2))]
        private static bool _UnityAX_AccessibilityActivate2(ulong identifier)
#else
        private delegate bool AccessibilityActivateDelegate(int identifier);
        [DllImport("__Internal")] private static extern void _UnityAX_registerAccessibilityActivate(AccessibilityActivateDelegate actionDelegate);
        [AOT.MonoPInvokeCallback(typeof(AccessibilityActivateDelegate))]
        private static bool _UnityAX_AccessibilityActivate(int identifier)
#endif
        {
            if (axElements.TryGetValue(identifier, out AccessibilityNode obj))
            {
                return obj.AccessibilityActivate();
            }
            return false;
        }
#endif // (UNITY_IOS || UNITY_TVOS) && !UNITY_EDITOR

        #endregion // Native Bridge
    }
}
