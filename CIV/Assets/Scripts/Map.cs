using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
	[SerializeField]
	private GameObject unitPrefab;
	[SerializeField]
	private int mapSize = 300;
	public Unit formation;
    // Start is called before the first frame update
    void Start()
    {
		formation = new Unit(unitPrefab, 60, 3);
    }

    // Update is called once per frame
    void Update()
    {
		//
    }
}
