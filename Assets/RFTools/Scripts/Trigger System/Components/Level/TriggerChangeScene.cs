using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
	[AddComponentMenu("Trigger/Level/Trigger Change Scene")]
	[TriggerDoc("When Triggered, Loads another scene referenced by name or returns to the main menu/campaign lobby scene.")]
	public partial class TriggerChangeScene : TriggerReceiver
	{
		public enum Type
		{
			LoadSceneByName,
			ReturnToCampaignLobbyScene,
			ReturnToMainMenu,
		}

		public Type type;
		[ConditionalField("type", Type.LoadSceneByName)] public string sceneName = "MyMap.rfl";
	}
}