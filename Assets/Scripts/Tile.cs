using UnityEngine;
using UnityEngine.Serialization;

namespace Maze
{
    public class Tile : MonoBehaviour
    {
        public int m_index;

        public int m_value;

        private SpriteRenderer spriteRenderer;

        public Color Color
        {
            get => spriteRenderer.color;
            set => spriteRenderer.color = value;
        }

        public void Initialize(int _value, Color _color)
        {
            m_value = _value;
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.color = _color;
        }
    }
}