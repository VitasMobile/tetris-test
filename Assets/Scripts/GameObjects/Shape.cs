using TestTetris.GameData;
using TestTetris.Sounds;
using TestTetris.Types;
using UnityEngine;

namespace TestTetris
{
    public class Shape : MonoBehaviour
    {
        [SerializeField] private Block[] _blocks = new Block[4];
        public Block[] Blocks => _blocks;
        [SerializeField] private ShapeType _type;
        public ShapeType Type { get => _type; }

        public bool IsEmpty { get; private set; } = true;
        public Color Color { get; private set; }


        private Playfield _playfield;
        private float _lastControlTime = 0.0f;
        private float _lastFall = 0.0f;
        private int _score = Settings.INIT_SHAPE_SCORE;
        private bool _fallBlocking = false;


        private void Update()
        {
            if (IsEmpty)
            {
                return;
            }

            float diffTime = Time.time - _lastControlTime;

            if (Input.GetKeyDown(KeyCode.LeftArrow) || (Input.GetKey(KeyCode.LeftArrow) && diffTime > Settings.MOVE_TIME))
            {
                LowerScore();
                Move(Vector2.left);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || (Input.GetKey(KeyCode.RightArrow) && diffTime > Settings.MOVE_TIME))
            {
                LowerScore();
                Move(Vector2.right);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) || (Input.GetKey(KeyCode.UpArrow) && diffTime > Settings.ROTATE_TIME))
            {
                LowerScore();
                Turn();
            }
            else if (
                (Input.GetKeyDown(KeyCode.DownArrow) || (Input.GetKey(KeyCode.DownArrow))
                && Time.time - _lastFall >= Settings.FALL_TIME && !_fallBlocking)
                || Time.time - _lastFall >= Settings.AUTO_DROP_TIME
            )
            {
                Fall();
            }

            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                _fallBlocking = false;
            }
        }

        public void Bind(Playfield playfield)
        {
            _playfield = playfield;
        }

        public void Clean()
        {
            SetVisibilityOfBlocks(false);
            IsEmpty = true;
        }

        public void ResetScore()
        {
            _score = Settings.INIT_SHAPE_SCORE;
        }

        public void SetType(ShapeType shapeType)
        {
            _type = shapeType;
        }

        public void BuildBlocks(Block[] blocks)
        {
            for (int i = 0; i < _blocks.Length; i++)
            {
                _blocks[i].transform.localPosition = blocks[i].transform.localPosition;
            }

            IsEmpty = false;
        }

        internal void SetColor(Color color)
        {
            Color = color;

            foreach (Block block in _blocks)
            {
                block.SetColor(color);
            }
        }

        public bool IsValidGridPos()
        {
            foreach (Block block in _blocks)
            {
                Vector2Int pos = block.GetPos();

                if (!Playfield.InsideBorder(pos))
                {
                    return false;
                }

                if (pos.y >= Settings.ROWS)
                {
                    continue;
                }

                if (_playfield.Grid[pos.x, pos.y].gameObject.activeSelf && _playfield.Grid[pos.x, pos.y].IsBaked)
                {
                    return false;
                }
            }

            return true;
        }

        public void UpdateGrid()
        {
            _playfield.Clean();

            Draw();
        }

        internal void SetVisibilityOfBlocks(bool isVisibility)
        {
            foreach (Block block in _blocks)
            {
                block.gameObject.SetActive(isVisibility);
            }
        }


        private void Draw()
        {
            foreach (Block block in _blocks)
            {
                if (block.GetPos().y >= Settings.ROWS)
                {
                    continue;
                }

                _playfield.Grid[block.GetPos().x, block.GetPos().y].Push(Color);
            }
        }

        /// <summary>Запекает Фигурку на поле.</summary>
        /// <remarks>Запекание означает что она не будет стираться с помощью метода Clean</remarks>
        private void Bake()
        {
            foreach (Block block in _blocks)
            {
                if (block.GetPos().y >= Settings.ROWS)
                {
                    continue;
                }

                _playfield.Grid[block.GetPos().x, block.GetPos().y].Push(Color, true);
            }
        }

        /// <summary>Вращение Фигурку на 90 градусов по часовой стрелке</summary>
        private void Turn()
        {
            transform.Rotate(Vector3.back * 90.0f);

            if (!IsValidGridPos())
            {
                transform.Rotate(Vector3.forward * 90.0f);
            }
            else
            {
                SoundManager.PlaySound(SoundClip.Turn);
                UpdateGrid();
            }

            _lastControlTime = Time.time;
        }

        private void Move(Vector2 direction)
        {
            if (TryMove(direction))
            {
                SoundManager.PlaySound(SoundClip.Move);
            }

            _lastControlTime = Time.time;
        }

        private void Fall()
        {
            if (!TryMove(Vector3.down))
            {
                _fallBlocking = true;
                SoundManager.PlaySound(SoundClip.Landed);
                Bake();
                gameObject.SetActive(false);
                _playfield.DeleteFullRows();
                FindObjectOfType<Spawner>().SpawnNext();
                Score.Add(_score);
            }

            _lastFall = Time.time;
        }

        /// <summary>Попытаться переместить Фигурку в направлении</summary>
        /// <param name="direction">Направление движения</param>
        /// <returns>удалось сдвинуть или нет</returns>
        private bool TryMove(Vector3 direction)
        {
            transform.position += direction;

            if (!IsValidGridPos())
            {
                transform.position -= direction;
                return false;
            }

            UpdateGrid();
            return true;
        }

        private void LowerScore()
        {
            if (_score > 2)
            {
                _score--;
            }
        }
    }
}
