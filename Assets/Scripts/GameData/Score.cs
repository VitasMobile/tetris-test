using System;
using UnityEngine;

namespace TestTetris.GameData
{
    public class Score
    {
        public static event Action<int> ChangeScoreEvent;
        public static event Action<int> ChangeTopScoreEvent;

        private const int DEFAULT_TOP_SCORE = 10000;

        private static int _score = 0;
        private static int _topScore = DEFAULT_TOP_SCORE;


        public Score()
        {
            _topScore = PlayerPrefs.GetInt("topScore", DEFAULT_TOP_SCORE);
            ChangeTopScoreEvent?.Invoke(_topScore);
            Reset();
        }

        /// <summary>Сбросить счетчик очков</summary>
        public void Reset()
        {
            _score = 0;
            ChangeScoreEvent?.Invoke(_score);
        }

        /// <summary>Увеличить игровой счет на количество очков</summary>
        /// <param name="score">Количество очков</param>
        public static void Add(int score)
        {
            _score += score;
            ChangeScoreEvent?.Invoke(_score);
        }

        /// <summary>Проверить побит ли рекорд</summary>
        /// <returns>если рекорд побит то вернется True иначе False</returns>
        public static bool IsTopScore()
        {
            if (_score > _topScore)
            {
                _topScore = _score;
                PlayerPrefs.SetInt("topScore", _topScore);
                ChangeTopScoreEvent?.Invoke(_topScore);

                return true;
            }

            return false;
        }

        public static void DoUpdate()
        {
            ChangeScoreEvent?.Invoke(_score);
            ChangeTopScoreEvent?.Invoke(_topScore);
        }
    }
}
