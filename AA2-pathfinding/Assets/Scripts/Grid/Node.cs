using System;
using UnityEngine;

namespace AI {

    public class Node {
        public bool Walkable { get; private set; }
        public bool Path { get; private set; }
        public Vector2Int pos { get; private set; }
        public Vector3 WorldPosition { get; private set; }

        private SpriteRenderer spriteRenderer;

        public Node(bool walkable, Vector2Int gridPos, Vector3 worldPos, SpriteRenderer renderer) {
            Walkable = walkable;
            pos = gridPos;
            WorldPosition = worldPos;
            spriteRenderer = renderer;
        }

        public void SetWalkable(bool walkable) {
            Walkable = walkable;
            UpdateVisual();
        }
        public void SetAsPath(bool path)
        {
            Path = path;
            //UpdateVisual();
        }

        public void ToggleWalkable() {
            Walkable = !Walkable;
            UpdateVisual();
        }

        public void SetColor(Color color) {
            if(spriteRenderer != null) spriteRenderer.color = color;
        }

        public void ResetVisual() {
            SetAsPath(false);
            if (spriteRenderer != null) spriteRenderer.color = Color.white;
        }

        public void UpdateDestinationVisual()
        {
            if (spriteRenderer == null) return;
            spriteRenderer.color = Color.green;
        }

        private void UpdateVisual() {
            if(spriteRenderer == null) return;
            spriteRenderer.color = Walkable ? Color.white : Color.black;
        }
    }
}