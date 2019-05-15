using UnityEngine;

public class TargetBase : MonoBehaviour
{
    [SerializeField] private GameObject target;

    private GameObject[] saveTargets;

    public GameObject[] SaveTargets { get => saveTargets; }

    private void Start()
    {
        saveTargets = new GameObject[0];
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            AddTarget();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RemoveLastTarget();
        }
    }

    private void AddTarget()
    {
        System.Array.Resize<GameObject>(ref saveTargets, saveTargets.Length + 1);
        saveTargets[saveTargets.Length - 1] = Instantiate(target, new Vector3(Random.Range(4.5f, -4.5f), 2f, Random.Range(-6, -5)), Quaternion.identity);
    }

    private void RemoveLastTarget()
    {
        if(saveTargets.Length != 0)
        {
            Destroy(saveTargets[saveTargets.Length - 1]);
            System.Array.Resize<GameObject>(ref saveTargets, saveTargets.Length - 1);
        }
    }
}
