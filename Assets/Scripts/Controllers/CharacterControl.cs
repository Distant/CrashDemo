using UnityEngine;
using System.Collections;

public interface CharacterControl
{
	bool Spinning { get; }
	float height { get; }
	void Jump (float speed);

	IEnumerator Flip (Vector3 dir);

	void Spin ();

	IEnumerator SpinAnim ();

	void Die ();
}