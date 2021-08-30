using TestTetris.Stages;
using UnityEngine;

namespace TestTetris
{
    public class Spawner : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private Shape _shapePrefab;
        [SerializeField] private Block _blockPrefab;
        [Space]
        public Shape[] shapes;
        [Header("Containers")]
        [SerializeField] private Transform _nextShapeContainer;
        [SerializeField] private Transform _movementShapeContainer;
        [SerializeField] private Transform _blockFieldContainer;

        private Playfield _playfield;
        private Shape _nextShape = null;
        private Shape _shape;


        private void Awake()
        {
            _playfield = new Playfield();
            _playfield.PrepareGrid(_blockPrefab, _blockFieldContainer);
            
            InstantiateShapes();

            MainMenuStage.NewGameEvent += OnNewGame;
        }

        private void OnDestroy()
        {
            MainMenuStage.NewGameEvent -= OnNewGame;
        }

        public void SpawnNext()
        {
            if (!_nextShape.IsEmpty)
            {
                _shape.transform.localPosition = _movementShapeContainer.localPosition;
                _shape.transform.eulerAngles = Vector3.zero;

                _shape.BuildBlocks(_nextShape.Blocks);
                if (_shape.IsValidGridPos())
                {
                    _shape.SetType(_nextShape.Type);
                    _shape.SetColor(_nextShape.Color);
                    _shape.UpdateGrid();
                    _shape.gameObject.SetActive(true);
                    _shape.enabled = true;
                    _shape.ResetScore();

                    GameManager.ShapeCoutnerManager.Inc(_nextShape.Type);
                }
                else
                {
                    _playfield.SetState(Playfield.State.GameOver);
                    GameManager.Stages.SetState(Stages.StageTypes.Win);
                }
            }
            Shape rndShape = shapes[Random.Range(0, shapes.Length)];
            _nextShape.transform.eulerAngles = Vector3.zero;
            _nextShape.SetColor(GameManager.ColorPatternManager.GetRndColor());
            _nextShape.SetType(rndShape.Type);
            _nextShape.BuildBlocks(rndShape.Blocks);
        }


        private void InstantiateShapes()
        {
            InstantiateNextShape();
            InstantiatePlayerShape();
        }

        private void InstantiatePlayerShape()
        {
            _shape = Instantiate<Shape>(_shapePrefab, transform);
            _shape.transform.localPosition = _movementShapeContainer.localPosition;
            _shape.Bind(_playfield);
        }

        private void InstantiateNextShape()
        {
            _nextShape = Instantiate<Shape>(_shapePrefab, _nextShapeContainer);
            _nextShape.transform.localPosition = Vector3.zero;
            _nextShape.enabled = false;
            _nextShape.Bind(_playfield);
        }

        private void OnNewGame()
        {
            _playfield.Clean(true);
            _nextShape.Clean();
            _nextShape.SetVisibilityOfBlocks(true);
            _shape.Clean();

            SpawnNext();
            SpawnNext();

            _playfield.SetState(Playfield.State.Play);
        }
    }
}
