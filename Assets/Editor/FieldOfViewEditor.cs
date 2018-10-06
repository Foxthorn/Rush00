using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor (typeof(enemyCharacter))]
public class FieldOfViewEditor : Editor {

	void OnSceneGUI() {
		enemyCharacter fow = (enemyCharacter)target;
		Handles.color = Color.white;
		Handles.DrawWireArc(fow.transform.position, Vector3.forward, Vector2.right, 360, fow.viewRadius);
		Handles.DrawWireArc(fow.transform.position, Vector3.forward, Vector2.right, 360, fow.viewMinRadius);
		Vector3 viewAngleA = fow.DirectionFromAngle(-fow.viewAngle / 2, false);
		Vector3 viewAngleB = fow.DirectionFromAngle(fow.viewAngle / 2, false);

		Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.viewRadius);
		Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.viewRadius);
	}
}
