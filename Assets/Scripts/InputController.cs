using System;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] private InputEnum spawnAgent;
    [SerializeField] private InputEnum destroyAgent;
    [SerializeField] private InputEnum moveAgent;
    [SerializeField] private InputEnum spawnTarget;
    [SerializeField] private InputEnum destroyTarget;

    private Action actionSpawnAgent;
    private Action actionDestroyAgent;
    private Action actionMoveAgent;
    private Action actionSpawnTarget;
    private Action actionDestroyTarget;

    private void OnValidate()
    {
        AgentController agentController = transform.parent.Find("AgentController").GetComponent<AgentController>();

        actionSpawnAgent = agentController.SpawnAgent;
        actionDestroyAgent = agentController.DestroyAgent;
        actionMoveAgent = agentController.StartMove;

        TargetController targetController = transform.parent.Find("TargetController").GetComponent<TargetController>();

        actionSpawnTarget = targetController.SpawnTarget;
        actionDestroyTarget = targetController.DestroyTarget;
    }

    private void Update()
    {
        InputMove(spawnAgent, actionSpawnAgent);
        InputMove(destroyAgent, actionDestroyAgent);
        InputMove(moveAgent, actionMoveAgent);
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
