using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationHitBox : MonoBehaviour
{
	[SerializeField]
	private Vector3[] points;
	[SerializeField]
	private bool setUp = false;

	public void SetupHitBox(Vector2[] corners)
	{
		points = new Vector3[4]
		{
			new Vector3 (corners[0].x, 0.2f, corners[0].y),
			new Vector3 (corners[1].x, 0.2f, corners[1].y),
			new Vector3 (corners[2].x, 0.2f, corners[2].y),
			new Vector3 (corners[3].x, 0.2f, corners[3].y),
		};
		setUp = true;
	}

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
