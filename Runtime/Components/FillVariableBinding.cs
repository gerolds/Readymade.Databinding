#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using NaughtyAttributes;
#endif
using UnityEngine;
using UnityEngine.UI;

namespace Readymade.Databinding.Components {
    /// <inheritdoc />
    /// <summary>
    /// Binds a <see cref="T:Readymade.Databinding.SoVariable`1" /> to the fill value of a <see cref="T:UnityEngine.UI.Image" />.
    /// </summary>
    [RequireComponent ( typeof ( Image ) )]
    public abstract class FillVariableBinding<T, TVariable> : MonoBehaviour where TVariable : SoVariable<T> where T : struct {
        [Tooltip ( "The image to bind the variable to." )]
        [HideIf ( nameof ( variable ) )]
        [SerializeField]
        private Image _target;

        [Tooltip ( "The variable to bind." )]
        [SerializeField]
        private TVariable variable;

        /// <summary>
        /// Unity event.
        /// </summary>
        private void Awake () {
            if ( _target == null ) {
                _target = GetComponent<Image> ();
            }
        }

        /// <summary>
        /// Unity event.
        /// </summary>
        private void Reset () {
            _target = GetComponent<Image> ();
        }

        /// <summary>
        /// Unity event.
        /// </summary>
        private void Start () {
            UpdateWidget ();
        }

        /// <summary>
        /// Unity event.
        /// </summary>
        private void OnEnable () {
            if ( variable != null ) {
                variable.Changed += ValueChangedHandler;
            }

            UpdateWidget ();
        }

        /// <summary>
        /// Unity event.
        /// </summary>
        private void OnDisable () {
            if ( variable != null ) {
                variable.Changed -= ValueChangedHandler;
            }
        }

        /// <summary>
        /// Called whenever the value of the variable changes. Updates the widget.
        /// </summary>
        /// <param name="args">The event args containing a variable reference and the old and new values.</param>
        private void ValueChangedHandler ( (SoVariable<T> source, T oldValue, T newValue) args ) {
            UpdateWidget ();
        }

        /// <summary>
        /// Updates the widget with the state of the variable.
        /// </summary>
        private void UpdateWidget () {
            if ( !_target ) {
                return;
            }

            if ( !variable ) {
                return;
            }


            _target.fillAmount = GetFillValue ( variable.Value );
        }

        /// <summary>
        /// Converter function to allow mapping of any struct to a fill.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A float in the range [0, 1]</returns>
        protected abstract float GetFillValue ( T value );
    }
}