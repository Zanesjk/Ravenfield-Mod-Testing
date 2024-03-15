using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
    [AddComponentMenu("Trigger/Player/Trigger Show\\Hide UI")]
    [TriggerDoc("When triggered, hides/shows elements of the player UI")]
    public partial class TriggerHideUI : TriggerReceiver
    {
        public enum Type
		{
            Hide,
            Show,
		}

        public enum ElementGroup
        {
            SingleElement,
            Everything,
            LeftSide,
            RightSide,
        }

        public Type type;
        public ElementGroup group;
        [ConditionalField("group", ElementGroup.SingleElement)] public IngameUI.UIElement element;
    }
}