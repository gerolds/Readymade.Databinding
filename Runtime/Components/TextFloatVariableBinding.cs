using TMPro;
using UnityEngine;

namespace Readymade.Databinding.Components {
    /// <inheritdoc />
    /// <summary>
    /// Specialization of a text variable binding for float values.
    /// </summary>
    [AddComponentMenu ( "Databinding/" + nameof ( TextFloatVariableBinding ) )]
    public class TextFloatVariableBinding : ComponentBinding<float, FloatVariable, TMP_Text, string> {
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
        protected override string ModelToView ( float value ) => value.ToString ( _format );

        /// <inheritdoc />
        protected override float ViewToModel ( string value ) {
            throw new System.NotImplementedException ();
        }
    }
}