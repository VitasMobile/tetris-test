using TestTetris.GameData;
using TestTetris.Sounds;
using UnityEngine;

namespace TestTetris
{
    public class Playfield
    {
        public enum State
        {
            Play,
            GameOver
        }

        #region EVENTS
        public delegate void OnChangedState(State state);
        public static event OnChangedState ChangeStateEvent;
        #endregion


        private static Block[,] _grid = new Block[Settings.COLUMNS, Settings.ROWS];
        public Block[,] Grid => _grid;
        private static float _scoreMul = 0.0f;
        private State _playState;


        public Playfield()
        {
            SetState(State.GameOver);
        }

        public void SetState(State playState)
        {
            _playState =  playState;
            ChangeStateEvent?.Invoke(_playState);
        }

        public void PrepareGrid(Block blockPrefab, Transform parentContainer)
        {
            for (int y = 0; y < Settings.ROWS; y++)
            {
                for (int x = 0; x < Settings.COLUMNS; x++)
                {
                    Block block = GameObject.Instantiate<Block>(blockPrefab, parentContainer);
                    block.transform.localPosition = new Vector2(x, y);
                    block.Init(x, y);
                    block.Pop();

                    _grid[x, y] = block;
                }
            }
        }

        public void DeleteFullRows()
        {
            _scoreMul = 1.0f;

            for (int y = 0; y < Settings.ROWS; y++)
            {
                if (IsRowFull(y))
                {
                    GameManager.IncLines();

                    DeleteRow(y);
                    DecreaseRowsAbove(y + 1);
                    y--;

                    Score.Add(Mathf.FloorToInt(100 * _scoreMul));
                    _scoreMul += 0.25f;
                }
            }
        }

        /// <summary>Очистить игрое поле от блоков</summary>
        /// <param name="isAll">очистить все, даже запеченные блоки</param>
        internal void Clean(bool isAll = false)
        {
            for (int y = 0; y < Settings.ROWS; y++)
            {
                for (int x = 0; x < Settings.COLUMNS; x++)
                {
                    if (!_grid[x, y].IsBaked || isAll)
                    {
                        _grid[x, y].Pop();
                    }
                }
            }
        }

        public static bool InsideBorder(Vector2 pos)
        {
            return ((int)pos.x >= 0 && (int)pos.x < Settings.COLUMNS && (int)pos.y >= 0);
        }

        private void DeleteRow(int y)
        {
            for (int x = 0; x < Settings.COLUMNS; x++)
            {
                _grid[x, y].Pop();
            }

            SoundManager.PlaySound(SoundClip.RemoveLine);
        }

        private void DecreaseRow(int y)
        {
            for (int x = 0; x < Settings.COLUMNS; x++)
            {
                if (_grid[x, y].gameObject.activeSelf)
                {
                    _grid[x, y - 1].Push(_grid[x, y].Color, true);
                    _grid[x, y].Pop();
                }
            }
        }

        private void DecreaseRowsAbove(int y)
        {
            for (int i = y; i < Settings.ROWS; i++)
            {
                DecreaseRow(i);
            }
        }

        private bool IsRowFull(int y)
        {
            for (int x = 0; x < Settings.COLUMNS; x++)
            {
                if (!_grid[x, y].gameObject.activeSelf)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
