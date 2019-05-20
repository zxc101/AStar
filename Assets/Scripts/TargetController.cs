using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    [SerializeField] private GameObject target;

    private List<GameObject> saveTargets;

    public List<GameObject> SaveTargets { get => saveTargets; }

    private void Start()
    {
        saveTargets = new List<GameObject>();
    }

    public void SpawnTarget()
    {
        saveTargets.Add(Instantiate(target, new Vector3(Random.Range(-3, 3), 0.66f, Random.Range(-4.5f, -3)), Quaternion.identity));
    }

    public void DestroyTarget()
    {
        if(saveTargets.Count != 0)
        {
            int randomTargetIndex = Random.Range(0, saveTargets.Count - 1);
            Destroy(saveTargets[randomTargetIndex]);
            saveTargets.RemoveAt(randomTargetIndex);
        }
    }
}
