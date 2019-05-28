using System.Collections;
using UnityEngine;

public class SeekerController : MonoBehaviour
{
    [SerializeField] private GameObject agent;
    [SerializeField] private int speed = 1;
    [SerializeField] private int countCrossingNodes;

    private SeekerStruct currentSeeker;

    private TargetController targetController;
    //private New.GridController gridController;
    private RaycastHit hit;

    private void OnValidate()
    {
        if (speed < 1)
        {
            speed = 1;
        }
        if(countCrossingNodes < 0)
        {
            countCrossingNodes = 0;
        }
    }

    void Start()
    {
        targetController = transform.parent.Find("TargetController").GetComponent<TargetController>();
        //gridController = transform.parent.Find("NewGridController").GetComponent<New.GridController>();
    }

    public void SpawnAgent()
    {
        if (currentSeeker.prefab == null)
        {
            currentSeeker.seeker = new Vector3(Random.Range(-3.1f, 2.4f), 1.8f, Random.Range(3, 3.7f));
            currentSeeker.prefab = Instantiate(agent, currentSeeker.seeker, Quaternion.identity);
            if (Physics.Raycast(currentSeeker.prefab.transform.position, -Vector3.up, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Frame")))
            {
                currentSeeker.prefab.transform.position = Vector3.right * currentSeeker.prefab.transform.position.x + Vector3.up * (hit.transform.position.y + currentSeeker.prefab.transform.localScale.y * 3 / 4 ) + Vector3.forward * currentSeeker.prefab.transform.position.z;
            }
            currentSeeker.oldTargetIndex = -1;
            currentSeeker.speed = speed;
            //gridController.GridUpdate();
        }
    }

    public void DestroyAgent()
    {
        if(currentSeeker.prefab != null)
        {
            currentSeeker.speed = 0;
            currentSeeker.targetIndex = 0;
            currentSeeker.oldTargetIndex = 0;
            currentSeeker.path = null;
            currentSeeker.seeker = Vector3.zero;
            Destroy(currentSeeker.prefab);
        }
    }

    public void StartMove()
    {
        currentSeeker.targetIndex = Random.Range(0, targetController.SaveTargets.Count);

        while (currentSeeker.oldTargetIndex == currentSeeker.targetIndex)
            if (targetController.SaveTargets.Count <= 1)
                return;
            else
                currentSeeker.targetIndex = Random.Range(0, targetController.SaveTargets.Count);

        currentSeeker.path = New.Pathfinder.FindPath(currentSeeker.seeker, targetController.SaveTargets[currentSeeker.targetIndex].transform.position);
        //StopCoroutine(FollowPath());
        //StartCoroutine(FollowPath());
        //currentSeeker.oldTargetIndex = currentSeeker.targetIndex;
    }

    private IEnumerator FollowPath()
    {
        int targetIndex = 0;
        Vector3 currentWaypoint = currentSeeker.path[targetIndex].position;

        while (true)
        {
            currentSeeker.seeker = Vector3.MoveTowards(currentSeeker.seeker, currentWaypoint, currentSeeker.speed * Time.deltaTime);
            currentSeeker.prefab.transform.position = currentSeeker.seeker;
            if (currentSeeker.seeker == currentWaypoint &&
                targetIndex < currentSeeker.path.Count - 1)
            {
                targetIndex++;
                currentWaypoint = currentSeeker.path[targetIndex].position;
                //if (targetIndex >= currentSeeker.path.Count)
                //{
                //    yield break;
                //}
                //if(currentWaypoint.y == currentSeeker.path[targetIndex].position.y)
                //{
                //    print("stobile");
                //}
                //else if (currentWaypoint.y < currentSeeker.path[targetIndex].position.y)
                //{
                //    print("up");
                //}
                //else if (currentWaypoint.y > currentSeeker.path[targetIndex].position.y)
                //{
                //    print("down");
                //}
            }
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        if(currentSeeker.path != null)
        {
            for(int i = 0; i < currentSeeker.path.Count; i++)
            {
                Gizmos.color = new Color(255, 247, 157);
                Gizmos.DrawCube(currentSeeker.path[i].position, Vector3.one/10);
                Gizmos.color = Color.red;
                Gizmos.DrawLine(currentSeeker.path[i].position, currentSeeker.path[i].parent.position);
            }
        }
    }
}
