using UnityEngine;

namespace TestTetris
{
    public class Block : MonoBehaviour
    {
        public Color Color { get; private set; }
        public bool IsBaked { get; private set; } = false;
        public Vector2Int Position { get; private set; } = Vector2Int.zero;

        private SpriteRenderer _sprite;


        private void Awake()
        {
            _sprite = GetComponent<SpriteRenderer>();
        }

        public void Init(int x, int y)
        {
            Position = new Vector2Int(x, y);
        }

        public Vector2Int GetPos()
        {
            Vector3 vPos = transform.parent.localPosition - (transform.parent.position - transform.position);
            Vector2Int pos = Vector2Int.zero;
            pos.x = Mathf.RoundToInt(vPos.x);
            pos.y = Mathf.RoundToInt(vPos.y);
            Position = pos;
            return Position;
        }

        internal void SetColor(Color color)
        {
            Color = color;
            _sprite.color = color;
        }

        internal void Push(Color color, bool isBake = false)
        {
            SetColor(color);
            gameObject.SetActive(true);
            IsBaked = isBake;
        }

        internal void Pop()
        {
            gameObject.SetActive(false);
            IsBaked = false;
        }
    }
}
