using NaughtyAttributes;
using System;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace Readymade.Databinding {
    /// <summary>
    /// Allows definition of a value that can be assigned directly or queried from a variable.
    /// </summary>
    /// <typeparam name="TVariable">The type of the variable SO to reference.</typeparam>
    /// <typeparam name="TValue">The value type of the variable SO.</typeparam>
    [Serializable]
    public class VariableReference<TValue, TVariable> where TVariable : SoVariable<TValue> where TValue : struct {
        [Tooltip ( "Whether to use a plain value or a variable reference." )]
        [SerializeField]
        [AllowNesting]
        private bool _useVariable;

        [Tooltip ( "The variable used to query the value of this reference." )]
        [SerializeField]
        [AllowNesting]
        [ShowIf ( nameof ( _useVariable ) )]
        [Expandable]
        private TVariable _variable;

        [Tooltip ( "The value used instead of a variable reference." )]
        [SerializeField]
        [AllowNesting]
        [HideIf ( nameof ( _useVariable ) )]
        private TValue _value;

#if UNITY_EDITOR
        public void CreateVariable () {
            Object active = UnityEditor.Selection.activeObject;
            TVariable instance = ScriptableObject.CreateInstance<TVariable> ();
            string path = $"Assets/New {typeof ( TVariable ).Name}.asset";
            UnityEditor.AssetDatabase.CreateAsset ( instance, path );
            UnityEditor.AssetDatabase.SaveAssets ();
            UnityEditor.AssetDatabase.Refresh ();
            UnityEditor.EditorUtility.FocusProjectWindow ();
            UnityEditor.Selection.activeObject = instance; // snap to file in project
            UnityEditor.Selection.activeObject = active; // select previous object again
            _variable = instance;
            UnityEditor.EditorUtility.SetDirty ( active );
        }
#endif

        /// <summary>
        /// Whether a reference is assigned.
        /// </summary>
        public bool IsVariableAssigned => _variable != null;

        /// <summary>
        /// Whether this uses a plain value instead of a reference.
        /// </summary>
        public bool IsVariable => _useVariable;

        /// <summary>
        /// Whether this reference has a defined value. That is whether a variable is assigned or it uses a plain value.
        /// </summary>
        public bool HasValue => !IsVariable || IsVariableAssigned;

        /// <summary>
        /// Get the underlying variable supplying the value. Only defined if <see cref="IsVariable"/> and
        /// <see cref="IsVariableAssigned"/> are true.
        /// </summary>
        public TVariable Variable => _variable;

        /// <summary>
        /// The current value backing the reference. Can be either a plain value directly specified as part of this reference
        /// or queried from the assigned variable.
        /// </summary>
        /// <remarks>Returns the default value of <typeparamref name="TValue" /> while <see cref="IsVariable"/> is <c>true</c> and <see cref="IsVariableAssigned"/> is <c>false</c>.</remarks>

        [ShowNativeProperty]
        public TValue Value {
            get => IsVariable
                ? IsVariableAssigned ? _variable.Value : default
                : _value;
            set {
                if ( _variable != null ) {
                    _variable.Value = value;
                } else {
                    _value = value;
                }
            }
        }
    }
}