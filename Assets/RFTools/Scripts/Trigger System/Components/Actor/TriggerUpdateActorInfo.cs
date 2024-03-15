using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
	[AddComponentMenu("Trigger/Actor/Trigger Update Actor Info")]
	[TriggerDoc("When Triggered, Updates the status of an Actor.")]
	public partial class TriggerUpdateActorInfo : TriggerReceiver
	{
		public enum NameUpdate
		{
			Keep,
			Change,
		}

		public enum HealthUpdate
		{
			Keep,
			Normal,
			HeroArmor,
			Invulnerable,
			SetHealth,
			IncrementHealth,
		}

		public enum TargetUpdate
		{
			Keep,
			Default,
			CannotBeTargeted,
			IgnoreEngagementRules,
			MakeHighPriorityTarget,
		}

		public ActorReference actor;
		new public NameUpdate name;
		[ConditionalField("name", NameUpdate.Change)] public string newName;

		public HealthUpdate health;
		[ConditionalField("health", HealthUpdate.SetHealth, HealthUpdate.IncrementHealth)] public float healthAmount;
		[ConditionalField("health", HealthUpdate.IncrementHealth)] public float maxHealth = 100f;

		public TargetUpdate target;

		[ConditionalField("target", TargetUpdate.MakeHighPriorityTarget)] public float highPriorityDuration = 10f;
	}
}