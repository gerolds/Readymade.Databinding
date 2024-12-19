using TMPro;
using UnityEngine;

namespace Readymade.Databinding.Components {
    /// <inheritdoc />
    /// <summary>
    /// Specialization of a text variable binding for integer values.
    /// </summary>
    [AddComponentMenu ( "Databinding/" + nameof ( TextIntegerVariableBinding ) )]
    public class TextIntegerVariableBinding : ComponentBinding<int, IntegerVariable, TMP_Text, string> {
        [Tooltip ( "A C# format string." )]
        [SerializeField]
        private string _format = "{0}";

        /// <inheritdoc />
        protected override void OnSubscribe ( TMP_Text component ) {
        }

        /// <inheritdoc />
        protected override void OnUnsubscribe ( TMP_Text component ) {
        }

        /// <inheritdoc />
        protected override void OnUpdateViewWithoutNotify ( TMP_Text component, string value ) {
            component.text = value;
        }

        /// <inheritdoc />
        protected override string ModelToView ( int value ) => value.ToString ( _format );

        /// <inheritdoc />
        protected override int ViewToModel ( string value ) {
            throw new System.NotImplementedException ();
        }
    }
}