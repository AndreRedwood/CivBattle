using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
	[SerializeField]
	const int TileSize = 40;

	[SerializeField]
	private GameObject tilePrefab;
	[SerializeField]
	private int mapSize = 10;
    // Start is called before the first frame update
    void Start()
    {
        for(int x = 0; x < mapSize; x++) 
		{
			for (int y = 0; y < mapSize; y++)
			{
				GameObject tile = Instantiate(tilePrefab);
				tile.transform.position = new Vector3(x * TileSize, 0, y * TileSize);
			}
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
