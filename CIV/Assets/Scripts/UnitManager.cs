using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
	[SerializeField]
	private List<GameObject> unitModels;
	[SerializeField]
	private BoxCollider unitHitBox;

	[SerializeField] 
	private int ranks;
	[SerializeField]
	private float[] modelDensity;

	int row = 20;

	public void Initialize(GameObject modelPrefab, int modelCount, int ranks, float[] modelDensity)
	{
		//here creating models and placing repositioning them
		//density[0] - row gap, density[1] - rank gap
		unitHitBox = this.AddComponent<BoxCollider>();
		this.modelDensity = new float[2] { modelDensity[0], modelDensity[1] };
		CalculateHitBox();
	}

	private void CalculateHitBox()
	{
		unitHitBox.size = new Vector3(ranks * modelDensity[1], 2, row * modelDensity[0]);
		unitHitBox.center = new Vector3(0.5f * (ranks - 1) * modelDensity[1], 1, 0.5f * row * modelDensity[0]);
	}

	private void FormUp()
	{
		//Generate Formation reworked here
	}
}
