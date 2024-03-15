using UnityEngine;

namespace Ravenfield.Trigger
{
	[TriggerDoc("When Triggered, adds an accessory skinned mesh renderer to the actor.")]
	[AddComponentMenu("Trigger/Actor/Trigger Add Actor Accessory")]
	public partial class TriggerAddActorAccessory : TriggerReceiver
    {
		public enum Type
		{
			AddAccessory,
			ClearAccessories,
		}

        public ActorReference actor;
		public Type type;

		[ConditionalField("type", Type.AddAccessory)] public ActorAccessory accessory;
	}
}