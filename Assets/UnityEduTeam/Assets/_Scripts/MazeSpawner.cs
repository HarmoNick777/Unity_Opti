using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeSpawner : MonoBehaviour {

    [SerializeField] List<GameObject> Modules = new List<GameObject>();
    [SerializeField] List<GameObject> SpawnPoints = new List<GameObject>();
    private List<GameObject> MazeModules = new List<GameObject>();

	void Start () {
        for (int i=0; i<SpawnPoints.Count; i++)
        {
            MazeModules.Add(Instantiate(Modules[Random.Range(0, Modules.Count)], SpawnPoints[i].transform.position, Quaternion.identity));
        }
	}
}
