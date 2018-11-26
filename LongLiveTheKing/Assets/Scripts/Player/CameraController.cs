using System;
using UnityEngine;

namespace LLtK
{
    [RequireComponent(typeof(InputHandler))]
    public class CameraController : MonoBehaviour
    {
        private InputHandler _inputHandler;

        public Transform FollowTargetTransform;
        public float FollowDistance = 5f;
        public float MovementSpeed = 10f;
        public float MinZoomDistance = 2f;
        public float MaxZoomDistance = 15f;
        public float ZoomSpeed = 15f;
        public bool LimitPosition = false;
        public float LimitX = 5f;
        public float LimitZ = 5f;

        public bool IsFollowingTarget
        {
            get { return FollowTargetTransform != null; }
        }

        private void Start()
        {
            _inputHandler = GetComponent<InputHandler>();
            _inputHandler.MoveCameraEvent += OnMoveCameraEvent;
            _inputHandler.ZoomEvent += OnZoomEvent;
        }

        private void Update()
        {
            if (IsFollowingTarget)
            {
                FollowTarget();
            }
        }

        private void OnMoveCameraEvent(Vector3 direction)
        {
            if (IsFollowingTarget) return;

            transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, MovementSpeed * Time.deltaTime);

            LimitMovement();
        }

        private void OnZoomEvent(float zoomValue)
        {
            Vector3 zoomTargetPosition =  transform.position + zoomValue * transform.forward;

            float distance = GetDistanceFromDecor(zoomTargetPosition);
            if (distance <= MinZoomDistance || distance >= MaxZoomDistance) return;

            transform.position = Vector3.Lerp(transform.position, zoomTargetPosition, Time.deltaTime * ZoomSpeed);
        }

        private void FollowTarget()
        {
            Vector3 followTargetPosition =  FollowTargetTransform.transform.position - transform.forward * FollowDistance;
            transform.position = Vector3.MoveTowards(transform.position, followTargetPosition, Time.deltaTime * MovementSpeed);

            LimitMovement();
        }

        private void LimitMovement()
        {
            if (!LimitPosition) return;
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -LimitX, LimitX), transform.position.y, Mathf.Clamp(transform.position.z, -LimitZ, LimitZ));
        }

        private float GetDistanceFromDecor(Vector3 position)
        {
            Ray ray = new Ray(position, transform.forward);
            Debug.DrawRay(position, transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f)) // TODO: Layer check instead of distance
            {
                return hit.distance;
            }
            else
            {
                return 0f;
            }
        }
    }
}
