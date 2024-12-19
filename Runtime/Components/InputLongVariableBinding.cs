using TMPro;
using UnityEngine;

namespace Readymade.Databinding.Components {
    /// <inheritdoc />
    /// <summary>
    /// Binds a variable to a <see cref="T:TMPro.TMP_InputField" />.
    /// </summary>
    [RequireComponent ( typeof ( TMP_InputField ) )]
    [AddComponentMenu ( "Databinding/" + nameof ( InputLongVariableBinding ) )]
    public class InputLongVariableBinding : ComponentBinding<long, LongVariable, TMP_InputField, string> {
        /// <inheritdoc />
        protected override void OnSubscribe ( TMP_InputField component ) {
            component.onEndEdit.AddListener ( ViewChangedHandler );
        }        /// <inheritdoc />


        protected override void OnUnsubscribe ( TMP_InputField component ) {
            component.onEndEdit.RemoveListener ( ViewChangedHandler );
        }

        /// <inheritdoc />
        protected override void OnUpdateViewWithoutNotify ( TMP_InputField component, string value ) {
            component.SetTextWithoutNotify ( value );
        }

        /// <inheritdoc />
        protected override string ModelToView ( long value ) => value.ToString ();

        /// <inheritdoc />
        protected override long ViewToModel ( string value ) => long.TryParse ( value, out long parsed ) ? parsed : default;
    }
}