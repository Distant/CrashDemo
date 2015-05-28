using UnityEngine;
using System.Collections;

public interface CharacterControl
{
	bool Spinning { get; }
	float height { get; }

	void Jump (float speed);
	void Spin ();
	IEnumerator Flip (Vector3 dir);
	IEnumerator SpinAnim ();
	void Stop();
	void Die ();
}