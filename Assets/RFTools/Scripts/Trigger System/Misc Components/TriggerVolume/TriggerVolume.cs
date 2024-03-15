using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
	public class TriggerVolume : MonoBehaviour
	{
		public RuntimeData data;

		public bool checkPlayerOnly = false;

		public TriggerVolume() {
			this.data.ceiling = 10f;
			this.data.floor = 10f;

			this.data.vertices = new List<Vector2>() {
				new Vector2(-20f, 20f),
				new Vector2(20f, 20f),
				new Vector2(20f, -20f),
				new Vector2(-20f, -20f),
			};
		}

		void OnValidate() {
			this.data.ceiling = Mathf.Max(0f, this.data.ceiling);
			this.data.floor = Mathf.Max(0f, this.data.floor);

			this.data.GenerateBounds();
		}

		public void UpdateTransformData() {
			this.data.GenerateBounds();
			UpdateVolumeTransformData();
		}

		public void UpdateTransformDataIfNeeded() {
			if (this.transform.hasChanged) {
				UpdateVolumeTransformData();
			}
		}

		public void UpdateVolumeTransformData() {
			this.transform.hasChanged = false;
			this.data.UpdateTransform(this.transform.worldToLocalMatrix);
		}

		public void SetVertexPosition(int index, Vector3 worldPosition) {
			this.data.SetVertexPosition(index, worldPosition);
		}

		public Vector3 GetLocalVertexPosition(int index) {
			return this.data.vertices[index].ToVector3XZ();
		}

		public Matrix4x4 LocalToWorldMatrix() {
			return this.transform.localToWorldMatrix;
		}

		// Holds trigger data in value type for fast runtime queries
		[System.Serializable]
		public struct RuntimeData
		{
			public List<Vector2> vertices;

			public float ceiling;
			public float floor;

			[System.NonSerialized] public Bounds worldBounds;
			[System.NonSerialized] public Bounds localBounds;
			[System.NonSerialized] public Matrix4x4 worldToLocalMatrix;

			Vector3 WorldToLocal(Vector3 worldPoint) {
				return this.worldToLocalMatrix.MultiplyPoint(worldPoint);
			}

			public void GenerateBounds() {
				var min = this.vertices[0].ToVector3XZ();
				var max = min;

				min.y -= this.floor;
				max.y += this.ceiling;

				this.localBounds = new Bounds() {
					min = min,
					max = max,
				};

				for (int i = 1; i < this.vertices.Count; i++) {
					this.localBounds.Encapsulate(this.vertices[i].ToVector3XZ());
				}

				UpdateWorldBounds();
			}

			public void SetVertexPosition(int index, Vector3 worldPosition) {
				this.vertices[index] = WorldToLocal(worldPosition).ToVector2XZ();
				GenerateBounds();
			}

			public void UpdateTransform(Matrix4x4 worldToLocalMatrix) {
				this.worldToLocalMatrix = worldToLocalMatrix;
				UpdateWorldBounds();
			}

			private void UpdateWorldBounds() {
				this.worldBounds = TransformBounds(this.localBounds, this.worldToLocalMatrix.inverse);
			}

			public static Bounds TransformBounds(Bounds local, Matrix4x4 localToWorld) {

				Bounds bounds = new Bounds(localToWorld.MultiplyPoint(local.min), Vector3.zero);
				bounds.Encapsulate(localToWorld.MultiplyPoint(new Vector3(local.min.x, local.min.y, local.max.z)));
				bounds.Encapsulate(localToWorld.MultiplyPoint(new Vector3(local.max.x, local.min.y, local.max.z)));
				bounds.Encapsulate(localToWorld.MultiplyPoint(new Vector3(local.max.x, local.min.y, local.min.z)));

				bounds.Encapsulate(localToWorld.MultiplyPoint(new Vector3(local.min.x, local.max.y, local.min.z)));
				bounds.Encapsulate(localToWorld.MultiplyPoint(new Vector3(local.min.x, local.max.y, local.max.z)));
				bounds.Encapsulate(localToWorld.MultiplyPoint(local.max));
				bounds.Encapsulate(localToWorld.MultiplyPoint(new Vector3(local.max.x, local.max.y, local.min.z)));

				return bounds;
			}
		}
	}
}
