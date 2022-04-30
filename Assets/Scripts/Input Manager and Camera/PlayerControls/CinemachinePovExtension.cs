using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Input_Manager_and_Camera;
using UnityEngine;

namespace Input_Manager
{
    public class CinemachinePovExtension : CinemachineExtension
    {
        [SerializeField] private float clampAngle = 80f;
        [SerializeField] private float horizontalSpeed = 10f;
        [SerializeField] private float verticalSpeed = 10f;
    
    
        private InputManager _inputManager;
        private Vector3 _startinRotation;

        protected override void Awake() {
            _inputManager = InputManager.Instance;
            base.Awake();

        }

        protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime) {
            if (vcam.Follow) {
                if (stage == CinemachineCore.Stage.Aim) {
                    if (_startinRotation == null)
                    {
                        _startinRotation = transform.localRotation.eulerAngles;
                        Vector2 deltaInput = _inputManager.GetMouseDelta();
                        _startinRotation.x += deltaInput.x * verticalSpeed * Time.deltaTime;
                        _startinRotation.y += deltaInput.y * horizontalSpeed * Time.deltaTime;
                        _startinRotation.y = Mathf.Clamp(_startinRotation.y, -clampAngle, clampAngle);
                        state.RawOrientation = Quaternion.Euler(-_startinRotation.y, _startinRotation.x, 0f);
                    }
                }
            }
        }
    }
}

