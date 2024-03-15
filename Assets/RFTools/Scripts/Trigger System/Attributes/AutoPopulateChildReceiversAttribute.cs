using System;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Ravenfield.Trigger {
#if UNITY_EDITOR
	[CustomPropertyDrawer(typeof(AutoPopulateChildReceiversAttribute))]
	public class AutoPopulateChildReceiversAttributeDrawer : PropertyDrawer
	{

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			if (!(attribute is AutoPopulateChildReceiversAttribute)) return EditorGUI.GetPropertyHeight(property);

			return EditorGUI.GetPropertyHeight(property);
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

			var buttonRect = new Rect(position.x + position.width / 2, position.y, position.width / 2, 20f);

			if (GUI.Button(buttonRect, "Set Children")) {
				try {
					var destinationsProperty = property.FindPropertyRelative("destinations");
					AutoPopulateChildren(destinationsProperty);
				}
				catch(System.Exception e) {
					Debug.LogException(e);
				}
			}

			EditorGUI.PropertyField(position, property, label, true);
		}

		void AutoPopulateChildren(SerializedProperty property) {
			var objectComponent = (Component)property.serializedObject.targetObject;

			Undo.RecordObject(objectComponent, $"Auto Populate {property.displayName}");

			var childReceivers = objectComponent.GetComponentsInChildren<TriggerReceiver>(true).ToList();

			// Remove any siblings that appear over this component
			var siblingReceivers = objectComponent.GetComponents<Component>().ToList();

			foreach(var sibling in siblingReceivers) {

				var siblingAsReceiver = sibling as TriggerReceiver;

				if(siblingAsReceiver != null) {
					childReceivers.Remove(siblingAsReceiver);
				}
				
				if (sibling == objectComponent) {
					break;
				}
			}

			property.arraySize = childReceivers.Count;

			for (int i = 0; i < childReceivers.Count; i++) {
				var elementProperty = property.GetArrayElementAtIndex(i);
				elementProperty.objectReferenceValue = childReceivers[i];
			}
		}
	}
#endif

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class AutoPopulateChildReceiversAttribute : PropertyAttribute
	{
		public AutoPopulateChildReceiversAttribute() { }
	}
}