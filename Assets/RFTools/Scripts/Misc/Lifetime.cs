using UnityEngine;
using System.Collections;

public class Lifetime : MonoBehaviour {

	public float lifetime = 1f;
	TimedAction lifetimeAction;

	// Use this for initialization
	void Start () {
		this.lifetimeAction = new TimedAction(this.lifetime);
		this.lifetimeAction.Start();
	}
	
	// Update is called once per frame
	void Update () {
		if(this.lifetimeAction.TrueDone()) {
			Destroy(this.gameObject);
		}
	}
}
