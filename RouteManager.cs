using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RouteManager : MonoBehaviour
{
    [HideInInspector]
    public bool initialized { get; private set; }

    public Vector3 DefaultPosition;
    public List<RouteNode> MainPath;
    public Dictionary<IRouteAgent, RouteNode> AgentLocations;

    private void Start()
    {
        initialized = false;
        AgentLocations = new Dictionary<IRouteAgent, RouteNode>();
        StartCoroutine(WaitingForPlayers(2));
    }

    private IEnumerator WaitingForPlayers(int playerCount)
    {
        ///..//

        initialized = true;
    }

    public CellInfo GetCellInfo(IRouteAgent agent)
    {
        CellInfo retValue = null;

        if (AgentLocations.TryGetValue(agent, out RouteNode value))
            retValue = value.CellInfo;

        return retValue;
    }

    public IEnumerator MoveAgent(IRouteAgent agent, int steps)
    {
        if (!AgentLocations.TryGetValue(agent, out RouteNode currentNode)) yield break;

        RouteNode nextNode = null;
        int sign = steps >= 0 ? 1 : -1;
        steps = Mathf.Abs(steps);
        

        for (int i = 0; i < steps; i++)
        {
            nextNode = GetNextNode(currentNode, sign);

            if (nextNode == null) yield break;

            yield return agent.MoveTo(nextNode.transform.position);
            UpdateLocation(agent, currentNode, nextNode);
            currentNode = nextNode;
        }
    }

    private RouteNode GetNextNode(RouteNode currentNode, int sign)
    {
        //...//

        return nextNode;
    }

    public IEnumerator TeleportAgent(IRouteAgent agent, MoveDirection teleportDirection)
    {
        if (!AgentLocations.TryGetValue(agent, out RouteNode currentNode)) yield break;

        RouteNode nextNode = null;

        switch (teleportDirection)
        {
            case MoveDirection.Forward:
                {
                    nextNode = currentNode.CellInfo.NextNodeShortcutTeleport;
                    break;
                }

            case MoveDirection.Backward:
                {
                    nextNode = currentNode.CellInfo.PreviousNodeShortcutTeleport;
                    break;
                }
        }

        if (nextNode == null) yield break;

        yield return agent.MoveTo(nextNode.transform.position);
        UpdateLocation(agent, currentNode, nextNode);
    }

    private void UpdateLocation(IRouteAgent agent, RouteNode previousNode, RouteNode newNode)
    {
        AgentLocations[agent] = newNode;
    }

    public void DefaultAgentPosition(IRouteAgent agent)
    {
        agent.MoveImmediately(DefaultPosition);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        for (int i = 1; i < MainPath.Count; i++)
        {
            Gizmos.DrawLine(MainPath[i].transform.position, MainPath[i - 1].transform.position);
        }
    }
}
