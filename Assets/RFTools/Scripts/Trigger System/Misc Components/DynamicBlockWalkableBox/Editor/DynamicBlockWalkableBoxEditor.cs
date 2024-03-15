using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Ravenfield.Trigger
{
	[CustomEditor(typeof(DynamicBlockWalkableBox)), CanEditMultipleObjects]
	public class DynamicBlockWalkableBoxEditor : Editor
	{
		public void OnSceneGUI() {

			var box = (DynamicBlockWalkableBox)this.target;
			Handles.matrix = box.transform.localToWorldMatrix;

			Color c = Color.red;

			Handles.color = c;
			Handles.zTest = UnityEngine.Rendering.CompareFunction.Less;
			Handles.DrawWireCube(Vector3.zero, Vector3.one);

			c.a = 0.2f;
			Handles.color = c;
			Handles.zTest = UnityEngine.Rendering.CompareFunction.Always;
			Handles.DrawWireCube(Vector3.zero, Vector3.one);

			Handles.matrix = Matrix4x4.identity;
		}
	}
}