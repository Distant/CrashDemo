using UnityEngine;
using System.Collections;

public interface CharacterControl
{
	bool Spinning { get; }
	bool Jumping { get; }
	float height { get; }
	Vector3 Velocity {get;}

	void Jump (float speed);
	void Spin ();
	IEnumerator FlipAnim (Vector3 dir);
	IEnumerator SpinAnim ();
	void Stop();
	void Die ();
	void Touching(Box box);
	void NotTouching(Box box);
}