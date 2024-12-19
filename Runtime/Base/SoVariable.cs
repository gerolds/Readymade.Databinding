#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using NaughtyAttributes;
#endif
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
#if ODIN_INSPECTOR
using ShowNonSerializedField = Sirenix.OdinInspector.ShowInInspectorAttribute;
using EInfoBoxType = Sirenix.OdinInspector.InfoMessageType;
#endif

namespace Readymade.Databinding
{
    /// <inheritdoc />
    /// <summary>
    /// An editor assignable reference to an atomic value of a given type. Provides API to observe value changes.
    /// </summary>
    /// <typeparam name="TValue">The type of the stored value.</typeparam>
    /// <remarks> Intended use case is to expose static values in a UI to the player and bind these values in components of
    /// dynamically instanced prefabs.</remarks>
    public abstract class SoVariable<TValue> : SoVariable where TValue : struct
    {
        [Tooltip("This value will be set when the instance is loaded or when Reset() is called.")]
        [SerializeField]
        [JsonIgnore]
        private TValue initialValue;

        [ShowNonSerializedField] private TValue _oldValue;

        [JsonProperty(PropertyName = "Value")]
        [ShowNonSerializedField]
        private TValue _value;

        [BoxGroup("Clamping")]
        [SerializeField]
        [ShowIf(nameof(CanBeClamped))]
        private bool clampValue;

        [BoxGroup("Clamping")]
        [SerializeField]
#if ODIN_INSPECTOR
        [ShowIf(nameof(CanBeClamped))]
        [ShowIf(nameof(clampValue))]
#else
        [ShowIf(EConditionOperator.And, nameof(CanBeClamped), nameof(clampValue))]
#endif
        private TValue minValue;

        [BoxGroup("Clamping")]
        [SerializeField]
        [ShowIf(nameof(CanBeClamped), nameof(clampValue))]
        private TValue maxValue;

        /// <summary>
        /// Invoked whenever <see cref="Value"/> is changed.
        /// </summary>
        public event Action<(SoVariable<TValue> source, TValue oldValue, TValue newValue)> Changed;

        /// <inheritdoc />
        /// <remarks>This value is provided for serialization purposes and in non-statically typed contexts. Get access will allocate due to boxing.</remarks>
        [JsonIgnore]
        public sealed override object BaseValue
        {
            get => Value;
            set => Value = (TValue)value;
        }

#if UNITY_EDITOR
        /// <summary>
        /// Set of all variable instances in editor.
        /// </summary>
        private static readonly HashSet<SoVariable<TValue>> s_instances = new();
#endif

        protected virtual void OnEnable()
        {
            ForceToInitialValueWithoutNotify();
            NotifyObservers();

#if UNITY_EDITOR
            if (UnityEditor.EditorSettings.enterPlayModeOptionsEnabled)
            {
                s_instances.Add(this);

                UnityEditor.EditorApplication.playModeStateChanged -= PlayModeStateChangedHandler;
                UnityEditor.EditorApplication.playModeStateChanged += PlayModeStateChangedHandler;
            }
#endif
        }

        private void OnDisable()
        {
#if UNITY_EDITOR
            // OnDisable will not be called when deleting the variable in the editor. Therefore, there might still be null
            // instances, while not ideal, this shouldn't cause issues.
            s_instances.Remove(this);
#endif
        }

#if UNITY_EDITOR
        private static void PlayModeStateChangedHandler(UnityEditor.PlayModeStateChange state)
        {
            if (state == UnityEditor.PlayModeStateChange.ExitingEditMode)
            {
                // Set initial values before any GameObject is initialized
                foreach (SoVariable<TValue> instance in s_instances)
                {
                    if (instance)
                    {
                        instance.ForceToInitialValueWithoutNotify();
                    }
                }
            }
            else if (state == UnityEditor.PlayModeStateChange.EnteredPlayMode)
            {
                // Invoke initial events during the first frame.
                foreach (SoVariable<TValue> instance in s_instances)
                {
                    if (instance)
                    {
                        instance.NotifyObservers();
                    }
                }
            }
        }
#endif

        /// <summary>
        /// Reset the variable to its initial value.
        /// </summary>
        public void ResetToInitialValue()
        {
            Value = initialValue;
        }

        /// <summary>
        /// Set the variable value equal to its initial value. This will not invoke <see cref="Changed"/>.
        /// </summary>
        private void ForceToInitialValueWithoutNotify() => SetValueWithoutNotify(initialValue, true);

        /// <summary>
        /// Set the variable value equal to its initial value. This will ignore whether the variable is immutable.
        /// </summary>
        [Button]
        private void ForceToInitialValue()
        {
            SetValueWithoutNotify(initialValue, true);
            NotifyObservers();
        }

        /// <summary>
        /// The current value of the variable.
        /// </summary>
        [Tooltip("The current value.")]
        [JsonIgnore]
        public TValue Value
        {
            get => _value;
            set
            {
                SetValueWithoutNotify(value);
                NotifyObservers();
            }
        }

        public void SetValueWithoutNotify(TValue value, bool force = false)
        {
            if (!force && !IsMutable)
            {
                Debug.LogWarning("Attempted to change immutable value. This is not allowed.", this);
                return;
            }

            _oldValue = _value;
            TValue clamped = clampValue ? OnClampValue(value, ClampMin, ClampMax) : value;
            _value = clamped;

            if (!Equals(_oldValue, _value))
            {
                if (IsDebug)
                {
                    Debug.Log($"[{name}] value changed from {_oldValue} to {_value}");
                }

#if UNITY_EDITOR
                if (IsTracking)
                {
                    EnsureHistory();
                    // prune history from time to time based on count
                    if (History.Count == Historysize * 2)
                    {
                        History.RemoveRange(0, Historysize);
                    }

                    History.Add((Time.time, Time.frameCount, _value,
                        IsTrackingCalls ? Environment.StackTrace : String.Empty));
                }
#endif
            }
        }

        /// <summary>
        /// Called whenever a new value is set. 
        /// </summary>
        /// <param name="value">The value to be clamped.</param>
        /// <param name="min">The min value that is allowed.</param>
        /// <param name="max">The may value that is allowed.</param>
        /// <returns>The clamped value.</returns>
        /// <remarks>By default the implementation is expected to use <see cref="ClampMin"/> and <see cref="ClampMax"/> to constrain the value range.</remarks>
        protected abstract TValue OnClampValue(in TValue value, in TValue min, in TValue max);

        /// <summary>
        /// Whether the value of this variable is clamped to a certain range.
        /// </summary>
        /// <seealso cref="ClampMin"/>
        /// <seealso cref="ClampMax"/>
        public bool IsClamped => clampValue;

        /// <summary>
        /// The min value of this variable that is enforced if <see cref="IsClamped"/> is true.
        /// </summary>
        public TValue ClampMin => minValue;

        /// <summary>
        /// The max value of this variable that is enforced if <see cref="IsClamped"/> is true.
        /// </summary>
        public TValue ClampMax => maxValue;

        /// <summary>
        /// Invokes the <see cref="Changed"/> event with the current <see cref="Value"/>. Useful for initialization and to
        /// handle certain edge cases. Ideally this is not needed.
        /// </summary>
        public void NotifyObservers() => Changed?.Invoke((source: this, oldValue: _oldValue, newValue: _value));

        /// <summary>
        /// Editor event.
        /// </summary>
        protected virtual void Reset()
        {
            ResetToInitialValue();
        }
    }

    /// <inheritdoc />
    /// <summary>
    /// An non-generic editor assignable reference to a value. Base class used for type-less access to a value where static
    /// type inference is impossible or inconvenient.
    /// </summary>
    public abstract class SoVariable : ScriptableObject
    {
        /// <summary>
        /// The current value of the variable. Typeless version that allows access where static type inference is impossible.
        /// </summary>
        [JsonIgnore]
        public abstract object BaseValue { get; set; }

        [FormerlySerializedAs("_id")]
        [ReadOnly]
        [SerializeField]
        [Tooltip("A serialized unique ID for this SO by which it can be identified from serialized data.")]
        private string id;

        private Guid _parsedId;

        /// <inheritdoc />
        public Guid Id
        {
            get
            {
                if (_parsedId == default)
                {
                    _parsedId = Guid.Parse(id);
                }

                return _parsedId;
            }
        }

        /// <summary>
        /// Whether this variable can be clamped.
        /// </summary>
        public abstract bool CanBeClamped { get; }

        /// <summary>
        /// Whether this value can be changed at runtime.
        /// </summary>
        [JsonIgnore]
        public bool IsMutable => isMutable;

        [Tooltip("Whether this value can be changed at runtime.")]
        [SerializeField]
        [JsonProperty(PropertyName = "IsMutable")]
        private bool isMutable = false;

        [Tooltip("Whether to log debug messages on value change.")]
        [SerializeField]
        [ShowIf(nameof(isMutable))]
        private bool debug;

        [Tooltip("Whether to keep a log of all value changes.")]
        [SerializeField]
        [InfoBox("Tracking changes is very inefficient, use only for debugging!", EInfoBoxType.Warning)]
#if ODIN_INSPECTOR
        [ShowIf("@" + nameof(isMutable) + " && " + nameof(debug))]
#else
        [ShowIf(EConditionOperator.And, nameof(isMutable), nameof(debug))]
#endif
        [ShowIf("isMutable")]
        private bool trackChanges = false;

        [Tooltip("Whether to keep a log of all value changes.")]
        [SerializeField]
        [InfoBox("Tracing changes is very inefficient, use only for debugging!", EInfoBoxType.Warning)]
#if ODIN_INSPECTOR
        [ShowIf("@" + nameof(isMutable) + " && " + nameof(debug) + " && " + nameof(trackChanges))]
#else
        [ShowIf(EConditionOperator.And, nameof(isMutable), nameof(debug), nameof(trackChanges))]
#endif
        private bool traceChanges;

        /// <summary>
        /// Whether to log debug messages.
        /// </summary>
        protected bool IsDebug => IsMutable && debug;

        /// <summary>
        /// Runtime event.
        /// </summary>
        protected virtual void Awake()
        {
#if UNITY_EDITOR
            EnsureID();
#endif
        }

#if UNITY_EDITOR
        /// <summary>
        /// Whether to log debug messages.
        /// </summary>
        protected bool IsTracking => IsDebug && trackChanges;

        /// <summary>
        /// Whether to track stack traces.
        /// </summary>
        protected bool IsTrackingCalls => IsTracking && traceChanges;

        private List<(float Time, int Frame, object Value, string Trace)> _history;

        // TODO: This should be displayed through a custom inspector.
        /// <summary>
        /// The value history of this variable. For internal and debugging only.
        /// </summary>
        public List<(float Time, int Frame, object Value, string Trace)> History => _history;

        protected void EnsureHistory()
        {
            if (History == null)
            {
                _history = new List<(float Time, int Frame, object Value, string Trace)>(Historysize * 2);
            }
        }

        public const int Historysize = 32;

        /// <summary>
        /// Generate a new unique ID (GUID) for this instance.
        /// </summary>
        [Button]
        public void GenerateNewID()
        {
            id = Guid.NewGuid().ToString();
            UnityEditor.EditorUtility.SetDirty(this);
        }

        /// <summary>
        /// Editor event.
        /// </summary>
        protected virtual void OnValidate()
        {
            EnsureID();
        }

        /// <summary>
        /// Ensure that the instance has a valid ID.
        /// </summary>
        private void EnsureID()
        {
            if (string.IsNullOrEmpty(id))
            {
                GenerateNewID();
            }
        }
#endif
    }
}