using UnityEngine;
using UnityEngine.UI;

namespace Readymade.Databinding.Components {
    /// <inheritdoc />
    [RequireComponent ( typeof ( Button ) )]
    [AddComponentMenu ( "Databinding/" + nameof ( ButtonLongVariableBinding ) )]
    public class ButtonLongVariableBinding : ComponentBinding<long, LongVariable, Button, object> {
        [Tooltip ( "The value to be added to the variable whenever the button is clicked." )]
        [SerializeField]
        private long _increment = 1;

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
        protected override object ModelToView ( long value ) {
            // do nothing
            return null;
        }

        /// <inheritdoc />
        protected override long ViewToModel ( object value ) {
            // on each click increment the variable.
            return Variable.Value + _increment;
        }
    }
}