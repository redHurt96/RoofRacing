﻿using Cinemachine;
using RoofRace.Car.WithLocalGravity;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace RoofRace.Car
{
    [RequireComponent(typeof(CinemachineImpulseSource))]
    public class GroundedCameraShaking : MonoBehaviour
    {
        [SerializeField] private GroundedStateIndicator _indicator;
        [SerializeField] private LocalGravityApplier _localGravityApplier;

        private CinemachineImpulseSource _impulseSource;

        private bool _enabled;

        private void Awake()
        {
            _impulseSource = GetComponent<CinemachineImpulseSource>();

            LevelStateMachine.Instance.LevelStarted += Enable;
            _indicator.StateChanged += ShakeIfGrounded;
        }

        private void OnDestroy()
        {
            if (LevelStateMachine.IsInstanceExist)
                LevelStateMachine.Instance.LevelStarted -= Enable;

            if (_indicator != null)
                _indicator.StateChanged -= ShakeIfGrounded;
        }

        private void Enable() => StartCoroutine(DelayedEnable());

        private IEnumerator DelayedEnable()
        {
            yield return new WaitForSeconds(1f);
            _enabled = true;
        }

        private void ShakeIfGrounded(bool isGrounded)
        {
            if (isGrounded && _localGravityApplier.Value.y < 0 && _enabled)
                Shake();
        }

        [Button]
        private void Shake() => 
            _impulseSource.GenerateImpulse();
    }
}