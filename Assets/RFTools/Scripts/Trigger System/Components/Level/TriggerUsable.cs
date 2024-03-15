using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
    [AddComponentMenu("Trigger/Level/Trigger Usable")]
    [TriggerDoc("Sends OnUse when the player presses the Use button while facing this object's collider. Sends OnKick when player kicks this object's collider. Choose between Use or Kick with the Type value.")]
    public partial class TriggerUsable : TriggerBaseComponent
    {
        public enum Type
		{
            Use,
            Kick,
            Both,
		}

        public enum TooltipInteraction
		{
            HideOnUseReappear,
            HideOnUseForever,
            AlwaysShow,
		}

        public Type type;

        [Header("Tooltip")]
        public bool showTooltip;
        [ConditionalField("showTooltip", true)] public string tooltipLabel = "";
        [ConditionalField("showTooltip", true)] public TooltipInteraction tooltipInteraction = TooltipInteraction.HideOnUseReappear;

        [Header("Sends")]

        [SignalDoc("Sent when used by player")]
        public TriggerSend onUse;

        [SignalDoc("Sent when kicked by player")]
        public TriggerSend onKick;
    }
}