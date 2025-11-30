using UnityEngine;

namespace AI {
    public class Enemy : MonoBehaviour
    {
        [SerializeField, Range(0, 1)] private float timeToMove;
        [SerializeField] private AI.Grid grid;

        private float remainingTime;
        private float direction = 1;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            remainingTime = timeToMove;
        }

        // Update is called once per frame
        void Update()
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime < 0)
            {
                remainingTime = timeToMove;

                grid.GetNodeFromWorld(this.transform.position).SetWalkable(true);

                this.transform.position += Vector3.up * direction;
                if (grid.GetNodeFromWorld(this.transform.position).pos.y == grid.height - 1 ||
                    grid.GetNodeFromWorld(this.transform.position).pos.y == 0) direction *= -1; 

                grid.GetNodeFromWorld(this.transform.position).SetWalkable(false);

                Agent.Instance.UpdatePath();
            }
        }
    }
}
