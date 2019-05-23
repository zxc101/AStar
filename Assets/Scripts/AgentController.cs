using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentController : MonoBehaviour
{
    private const float radius = 0.2f;

    public float speed = 1;
    private Ray ray;
    private RaycastHit hit;

    private bool moveComplete = true;

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            MouseClick();
        }
    }

    private void MouseClick()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit))
        {
            if(hit.transform.tag == "Finish")
            {
                Move(hit.point);
            }
        }
    }

    private void Move(Vector3 point)
    {
        if (!moveComplete)
        {
            StopCoroutine("MoveProc");
        }

        StartCoroutine(MoveProc(point));
    }

    private IEnumerator MoveProc(Vector3 point)
    {
        moveComplete = false;

        transform.LookAt(point + Vector3.up * transform.position.y);

        while (!moveComplete)
        {
            transform.position += transform.forward * speed * Time.deltaTime;
            if (Vector3.Distance(transform.position, point + Vector3.up * transform.position.y) < radius)
            {
                moveComplete = true;
            }
            yield return null;
        }

        yield break;
    }
}