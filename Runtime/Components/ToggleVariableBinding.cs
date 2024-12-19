using UnityEngine;
using UnityEngine.UI;

namespace Readymade.Databinding.Components {
    [RequireComponent ( typeof ( Toggle ) )]
    public class ToggleVariableBinding : ComponentBinding<bool, BoolVariable, Toggle, bool> {
        [Tooltip ( "Whether to negate the variable's value." )]
        [SerializeField]
        private bool _negate = false;

        /// <inheritdoc />
        protected override void OnSubscribe ( Toggle component ) {
            component.onValueChanged.AddListener ( ViewChangedHandler );
        }

        /// <inheritdoc />
        protected override void OnUnsubscribe ( Toggle component ) {
            component.onValueChanged.RemoveListener ( ViewChangedHandler );
        }

        /// <inheritdoc />
        protected override void OnUpdateViewWithoutNotify ( Toggle component, bool value ) {
            component.SetIsOnWithoutNotify ( value );
        }

        /// <inheritdoc />
        protected override bool ModelToView ( bool value ) => _negate ? !value : value;

        /// <inheritdoc />
        protected override bool ViewToModel ( bool value ) => _negate ? !value : value;
    }
}