using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMover : MonoBehaviour
{
    public float speed = 4f;
    public Vector2Int Target { get; private set; }

    public void SetTarget(Vector2Int newTarget)
    {
        Target = newTarget;
    }

    public void Follow(List<GridNode> path)
    {
        StopAllCoroutines();
        StartCoroutine(FollowRoutine(path));
    }

    private IEnumerator FollowRoutine(List<GridNode> path)
    {
        foreach (var node in path)
        {
            Vector3 targetPos = new Vector3(node.Position.x, node.Position.y, 0);

            while ((transform.position - targetPos).sqrMagnitude > 0.01f)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    targetPos,
                    speed * Time.deltaTime
                );
                yield return null;
            }
        }
    }
}