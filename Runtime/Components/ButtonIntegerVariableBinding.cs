using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Readymade.Databinding.Components {
    /// <inheritdoc />
    [RequireComponent ( typeof ( Button ) )]
    [AddComponentMenu ( "Databinding/" + nameof ( ButtonIntegerVariableBinding ) )]
    public class ButtonIntegerVariableBinding : ComponentBinding<int, IntegerVariable, Button, object> {
        [Tooltip ( "The value to be added to the variable whenever the button is clicked." )]
        [SerializeField]
        private int _increment = 1;

        /// <inheritdoc />
        protected override void OnSubscribe ( Button component ) {
            component.onClick.AddListener ( ClickHandler );
        }

        protected override void OnUnsubscribe ( Button component ) {
            component.onClick.RemoveListener ( ClickHandler );
        }

        private void ClickHandler () => ViewChangedHandler ( null );

        /// <inheritdoc />
        protected override void OnUpdateViewWithoutNotify ( Button component, object value ) {
            // do nothing
        }

        /// <inheritdoc />
        protected override object ModelToView ( int value ) {
            // do nothing
            return null;
        }

        /// <inheritdoc />
        protected override int ViewToModel ( object value ) {
            // on each click increment the variable.
            return Variable.Value + _increment;
        }
    }
}