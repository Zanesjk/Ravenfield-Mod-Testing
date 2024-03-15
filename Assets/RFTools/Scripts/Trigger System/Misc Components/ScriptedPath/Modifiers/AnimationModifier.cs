using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
	public class AnimationModifier : ScriptedPathEdgeModifier
	{
		public enum Animation
		{
			SquadHail,
			SquadRegroup,
			SquadHalt,
			SquadMove,
			Chat,



			MeleeSwing,
		}

		const Animation MAX_ANIMATION_VALUE = Animation.MeleeSwing;



		public Animation animation;

#if UNITY_EDITOR
		public override bool DrawEditorGUI() {

			GUILayout.Label(this.animation.ToString(), ScriptedPath.Debug.BODY_GUI_STYLE);
			var newAnimation = (Animation) (GUILayout.HorizontalSlider((float)this.animation, 0, (float)MAX_ANIMATION_VALUE) + 0.5f);

			if(newAnimation != this.animation) {
				this.animation = newAnimation;
				return true;
			}
			return false;
		}
#endif

		public override void OnPassed(ScriptedPathSeeker seeker) {
			seeker.callbackTarget.OnScriptedPathAnimationTriggered(this.animation);
		}
	}
}