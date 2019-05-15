using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetBase : MonoBehaviour
{
    [SerializeField] private GameObject pet;

    private Grid grid;

    void Start()
    {
        grid = GameObject.Find("Grid").GetComponent<Grid>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Instantiate(pet, new Vector3(Random.Range(4.5f, -4.5f), 3, Random.Range(6, 7)), Quaternion.Euler(0, 180, 0));
            grid.GridUpdate();
        }
    }
}
