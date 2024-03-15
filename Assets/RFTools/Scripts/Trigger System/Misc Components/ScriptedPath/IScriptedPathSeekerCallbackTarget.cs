using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
	public interface IScriptedPathSeekerCallbackTarget
	{
		void OnScriptedPathFireWeapon(float holdFireTime);
		void OnScriptedPathAnimationTriggered(AnimationModifier.Animation animation);
		void OnScriptedPathCompleted(bool stayAtEnd);
		void ForceTeleportToPosition(Vector3 position);
	}
}