using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActorAccessory {

	public Mesh mesh;
	public Material[] materials;

    public ActorAccessory(Mesh mesh, Material[] materials) {
        this.mesh = mesh;
        this.materials = materials;
    }
}
