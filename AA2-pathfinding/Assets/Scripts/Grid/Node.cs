using UnityEngine;

namespace AI {

    public class Node {
        public bool Walkable { get; private set; }
        public Vector2Int GridPos { get; private set; }

        private SpriteRenderer spriteRndr;
        private float GCost;
        private float HCost;
        public float FCost => GCost + HCost;

        public void SetWalkable(bool _bool) {
            Walkable = _bool;
        }

        public void SetColorOfSprite(Color _color) {
            spriteRndr.color = _color;
        }

        public Node(bool _walkable, Vector2Int _pos, SpriteRenderer _spriteRndr) {
            Walkable = _walkable;
            GridPos = _pos;
            spriteRndr = _spriteRndr;
        }
    }
}