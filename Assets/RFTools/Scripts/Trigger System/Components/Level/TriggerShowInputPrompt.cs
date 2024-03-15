using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
    [AddComponentMenu("Trigger/Player/Trigger Show Input Prompt")]
    [TriggerDoc("When triggered, shows an input prompt (press key to do action) using the ingame UI.")]
    public partial class TriggerShowInputPrompt : TriggerReceiver
    {
        public enum BindType
		{
            SingleBind,
            AxisBind,
            CompoundBind,
            CombinationBind,
		}

        public BindType bindType;
        public SteelInput.KeyBinds actionBind;
        [ConditionalField("bindType", BindType.CompoundBind, BindType.CombinationBind)] public SteelInput.KeyBinds altActionBind;

        public string actionLabel;

        public bool autoHideOnPressed = true;
    }
}