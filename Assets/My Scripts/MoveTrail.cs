using UnityEngine;
using System.Collections;

public class MoveTrail : MonoBehaviour {

	public int moveSpeed = 1;

	// Update is called once per frame
	void Update () {
		transform.Translate (Vector3.right * Time.deltaTime * moveSpeed);
		Destroy (gameObject, 1);
	}
}
