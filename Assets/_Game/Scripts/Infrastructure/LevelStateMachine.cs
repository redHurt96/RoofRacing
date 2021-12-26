using RH.Utilities.SingletonAccess;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace RoofRace
{
    public class LevelStateMachine : MonoBehaviourSingleton<LevelStateMachine>
    {
        public event Action LevelRestarted;

        [SerializeField, AssetsOnly] private PlayerCar _carPrefab;
        [SerializeField, AssetsOnly] private Level _levelPrefab;
        [SerializeField] private CameraLookPoint _cameraLookPoint;
        [SerializeField] private LevelCamera _levelCamera;

        [Header("UI")]
        [SerializeField] private GameObject _startUi;
        [SerializeField] private GameObject _finishUi;
        [SerializeField] private GameObject _failUi;

        private Level _level;
        private PlayerCar _car;

        private Vector3 _startPoint => _level.StartPoint.position;

        private void Start()
        {
            SwitchToStartState();
        }

        internal void StartLevel()
        {
            _startUi.SetActive(false);
            _car.Enable();
        }

        internal void FinishLevel()
        {
            _levelCamera.RotateAround(_car.transform);
            _finishUi.SetActive(true);
        }

        internal void FailLevel()
        {
            _failUi.SetActive(true);
        }

        internal void GoToNextLevel()
        {
            SwitchToStartState();
        }

        internal void RestartLevel()
        {
            LevelRestarted?.Invoke();
            SwitchToStartState();
        }

        private void SwitchToStartState()
        {
            CreateLevel();
            CreateCar();

            _cameraLookPoint.AttachTarget(_car.transform);
            _levelCamera.ResetToDefaultState();

            _startUi.SetActive(true);
            _finishUi.SetActive(false);
            _failUi.SetActive(false);
        }

        private void CreateLevel() => Create(_levelPrefab, ref _level, Vector3.zero);
        private void CreateCar() => Create(_carPrefab, ref _car, _startPoint);

        private void Create<T>(T prefab, ref T container, Vector3 atPosition) where T : Component
        {
            if (container != null)
                Destroy(container.gameObject);

            container = Instantiate(prefab, atPosition, Quaternion.identity);
        }
    }
}