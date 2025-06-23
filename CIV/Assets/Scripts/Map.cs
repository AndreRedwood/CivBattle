using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
	//for Future - throw it to singleton
	[SerializeField]
	private GameObject unitPrefab;
	[SerializeField]
	private LayerMask clickable;
	[SerializeField] 
	private LayerMask ground;
	[SerializeField]
	private int mapSize = 300;
	public Unit formation;
    // Start is called before the first frame update
    void Start()
    {
		GameObject unitGameObject = new GameObject("Formation");
		unitGameObject.AddComponent<Unit>();
		formation = unitGameObject.GetComponent<Unit>();
		formation.Initialize(unitPrefab, 60, 3);
    }

    // Update is called once per frame
    void Update()
    {
		//
    }
}
