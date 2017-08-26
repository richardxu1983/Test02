using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFind : UnitySingleton<PathFind>
{
    public void FindPath(Node startNode, Node targetNode, ref List<Node> path)
    {
        Heap<Node> openSet = new Heap<Node>(GSceneMap.Instance.MaxSize);
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);
        Node currentNode;

        while(openSet.Count>0)
        {
            currentNode = openSet.RemoveFirst();
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                RetracePath(startNode, targetNode, ref path);
                return;
            }

            foreach (Node neighbour in GSceneMap.Instance.GetNeighbours(currentNode))
            {
                if (neighbour.block || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                    else
                    {
                        //openSet.UpdateItem(neighbour);
                    }
                }
            }
        }
    }

    void RetracePath(Node startNode, Node endNode, ref List<Node> path)
    {
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridId.x - nodeB.gridId.x);
        int dstY = Mathf.Abs(nodeA.gridId.y - nodeB.gridId.y);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
