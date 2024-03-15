using UnityEngine;

namespace Ravenfield.Trigger
{
	[AddComponentMenu("Trigger/Level/Trigger On Destructible Damage")]
	[TriggerDoc("Sends a signal when the specificed destructible takes damage or is destroyed.")]
	public partial class TriggerOnDestructibleDamage : TriggerBaseComponent
	{
		public Destructible destructible;

		[SignalDoc("Sent when the destructible is damaged", actor = "The damaging actor", squad = "The squad the actor belongs to", weapon = "The damaging weapon")]
		public TriggerSend onDestructibleDamaged;

		[SignalDoc("Sent when the destructible is destroyed", actor = "The damaging actor", squad = "The squad the actor belongs to", weapon = "The damaging weapon")]
		public TriggerSend onDestructibleDestroyed;
	}
}