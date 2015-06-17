using UnityEngine;
using System.Collections;

public class Crowd : MonoBehaviour {
	public GameObject fab;
	public Vector3 speed;
	public int count = 100;

	void Start () {
		for (var i = 0; i < count; i++) {
			var pos = 5f * Random.insideUnitSphere;
			var inst = (GameObject)Instantiate(fab, pos, Random.rotationUniform);
			inst.transform.SetParent(transform, false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		transform.localRotation *= Quaternion.Euler(speed * Time.deltaTime);
	}
}
