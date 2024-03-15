using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
	[AddComponentMenu("Trigger/Events/Trigger On Captured")]
	[TriggerDoc("Sends OnCaptured when the CapturePoint is captured by the specified Team.")]
	public partial class TriggerOnCaptured : TriggerBaseComponent
	{
		public CapturePoint capturePoint;
		public Team team;

		[SignalDoc("Sent when the capture point is captured")]
		public TriggerSend onCaptured;
	}
}