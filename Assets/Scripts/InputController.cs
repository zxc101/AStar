using System;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] private InputEnum spawnSeeker;
    [SerializeField] private InputEnum destroySeeker;
    [SerializeField] private InputEnum moveSeeker;
    [SerializeField] private InputEnum spawnTarget;
    [SerializeField] private InputEnum destroyTarget;

    private Action actionSpawnSeeker;
    private Action actionDestroySeeker;
    private Action actionMoveSeeker;
    private Action actionSpawnTarget;
    private Action actionDestroyTarget;

    private void OnValidate()
    {
        SeekerController seekerController = transform.parent.Find("SeekerController").GetComponent<SeekerController>();

        actionSpawnSeeker = seekerController.SpawnAgent;
        actionDestroySeeker = seekerController.DestroyAgent;
        actionMoveSeeker = seekerController.StartMove;

        TargetController targetController = transform.parent.Find("TargetController").GetComponent<TargetController>();

        actionSpawnTarget = targetController.SpawnTarget;
        actionDestroyTarget = targetController.DestroyTarget;
    }

    private void Update()
    {
        InputMove(spawnSeeker, actionSpawnSeeker);
        InputMove(destroySeeker, actionDestroySeeker);
        InputMove(moveSeeker, actionMoveSeeker);
        InputMove(spawnTarget, actionSpawnTarget);
        InputMove(destroyTarget, actionDestroyTarget);
    }

    private void InputMove(InputEnum key, Action action)
    {
        if (key == InputEnum.Mouse0 || key == InputEnum.Mouse1)
        {
            if (Input.GetMouseButtonDown(key == InputEnum.Mouse0 ? 0 : 1))
            {
                action();
            }
        }
        else
        {
            if (Input.GetKeyDown(key.ToString()))
            {
                action();
            }
        }
    }
}
