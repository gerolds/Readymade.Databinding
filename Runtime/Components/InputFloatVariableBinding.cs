using TMPro;
using UnityEngine;

namespace Readymade.Databinding.Components {
    /// <inheritdoc />
    /// <summary>
    /// Binds a variable to a <see cref="T:TMPro.TMP_InputField" />.
    /// </summary>
    [RequireComponent ( typeof ( TMP_InputField ) )]
    [AddComponentMenu ( "Databinding/" + nameof ( InputFloatVariableBinding ) )]
    public class InputFloatVariableBinding : ComponentBinding<int, IntegerVariable, TMP_InputField, string> {
        /// <inheritdoc />
        protected override void OnSubscribe ( TMP_InputField component ) {
            component.onEndEdit.AddListener ( ViewChangedHandler );
        }

        /// <inheritdoc />
        protected override void OnUnsubscribe ( TMP_InputField component ) {
            component.onEndEdit.RemoveListener ( ViewChangedHandler );
        }

        /// <inheritdoc />
        protected override void OnUpdateViewWithoutNotify ( TMP_InputField component, string value ) {
            component.SetTextWithoutNotify ( value );
        }

        /// <inheritdoc />
        protected override string ModelToView ( int value ) => value.ToString ();

        /// <inheritdoc />
        protected override int ViewToModel ( string value ) => int.TryParse ( value, out int parsed ) ? parsed : default;
    }
}