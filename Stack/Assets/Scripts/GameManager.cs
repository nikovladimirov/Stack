using System.Collections.Generic;
using Behaviours;
using DefaultNamespace.Enums;
using DefaultNamespace.Logics;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        private GameState _gameState;

        private GameLogic _gameLogic;
        private List<CubeBehaviour> _cubes = new List<CubeBehaviour>();
        private CubeBehaviour _lastCube;
        private int _score = -1;
        private int _topScore = -1;

        public delegate void GameStateChangedArgs(GameState state);

        public event GameStateChangedArgs GameStateChanged;

        public delegate void GameScoreChangedArgs(int score);

        public event GameScoreChangedArgs GameScoreChanged;

        public delegate void GameTopScoreChangedArgs(int topscore);

        public event GameTopScoreChangedArgs GameTopScoreChanged;

        [SerializeField] private GameObject _gameObjects;
        [SerializeField] private GameObject _parentCubes;
        [SerializeField] private GameObject _cubePrefab;
        [SerializeField] private GameObject _gui;

        public int TopScore => _topScore;

        private void Awake()
        {
            Instance = this;
            _gui.SetActive(true);
            _topScore = PlayerPrefs.GetInt(TopScoreConst, 0);
        }

        public void SetGameState(GameState newState)
        {
            switch (_gameState)
            {
                case GameState.Menu:
                    if (newState != GameState.Playing)
                        return;
                    break;
                case GameState.Playing:
                    if (newState == GameState.Playing)
                        return;
                    break;
                case GameState.Death:
                    if (newState != GameState.Menu)
                        return;
                    break;
                default:
                    return;
            }

            _gameState = newState;

            switch (newState)
            {
                case GameState.Playing:
                    _cubes.Clear();
                    if (_parentCubes != null)
                        Destroy(_parentCubes);

                    _parentCubes = new GameObject("Cubes");
                    _parentCubes.transform.SetParent(_gameObjects.transform);
                    _score = -1;
                    OnGameScoreChanged();
                    SpawnFirstCube();
                    SpawnNextCube();

                    break;
                case GameState.Death:
                    if (_score > _topScore)
                    {
                        PlayerPrefs.SetInt(TopScoreConst, _score);
                        _topScore = _score;
                        OnGameTopScoreChanged();
                    }

                    break;
            }

            OnGameStateChanged();
        }

        private const string TopScoreConst = "TopScore";
        private const string TopBuildConst = "TopBuild";

        private void OnGameStateChanged()
        {
            var hanler = GameStateChanged;
            if (hanler == null)
                return;

            Debug.Log($"GameState: {_gameState}");
            hanler(_gameState);
        }

        private void OnGameScoreChanged()
        {
            var hanler = GameScoreChanged;
            if (hanler == null)
                return;

            Debug.Log($"GameScore: {_score}");
            hanler(_score);
        }

        private void OnGameTopScoreChanged()
        {
            var hanler = GameTopScoreChanged;
            if (hanler == null)
                return;

            Debug.Log($"GameTopScore: {_topScore}");
            hanler(_topScore);
        }

        public void SpawnFirstCube()
        {
            var cube = SpawnCube();
            cube.Init(null);
            _lastCube = cube;
        }

        public void SpawnNextCube()
        {
            if (_lastCube.transform.position.y < _lastCube.MinY)
            {
                SetGameState(GameState.Death);
                return;
            }

            _score++;
            OnGameScoreChanged();

            var cube = SpawnCube();
            cube.Init(_lastCube);
            _cubes.Add(cube);

            _lastCube = cube;
        }

        private List<Color> _colors = new List<Color>
            {Color.red, new Color(1, 0.64f, 0), Color.yellow, Color.green, Color.cyan, Color.blue, new Color(0.5f,0,0.5f)};
        
        private CubeBehaviour SpawnCube()
        {
            var go = Instantiate(_cubePrefab);
            go.transform.SetParent(_parentCubes.transform);

            var color = _lastCube?.Color ?? _colors[Random.Range(0, _colors.Count)];
            var generateNewColor = Random.Range(0, 10) > 8;
            if (generateNewColor)
            {
                var newIndex = _colors.IndexOf(color) + 1;
                if (newIndex >= _colors.Count)
                    newIndex = 0;
                color = _colors[newIndex];
            }
            
            var cube = go.GetComponent<CubeBehaviour>();
            cube.Color = color;
            return cube;
        }

        private void Update()
        {
            if (!Input.GetKeyUp(KeyCode.Space))
                return;

            if (_lastCube == null)
                return;

            _lastCube.Drop();
        }
    }
}