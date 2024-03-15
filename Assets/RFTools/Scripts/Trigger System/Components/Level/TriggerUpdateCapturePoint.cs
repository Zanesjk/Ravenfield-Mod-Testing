using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
	[AddComponentMenu("Trigger/Level/Trigger Update Capture Point")]
	[TriggerDoc("When triggered, Updates the CapturePoint state.")]
	public partial class TriggerUpdateCapturePoint : TriggerReceiver
    {
		public CapturePoint capturePoint;

		public enum ChangeOwnerInfo
		{
			Keep,
			Blue,
			Red,
		};

		public ChangeOwnerInfo owner;
		public bool canCapture = false;
		public float captureRate = 1f;
	}
}