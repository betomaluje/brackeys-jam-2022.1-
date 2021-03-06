using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TarodevController
{
    public class PlayerCollision: MonoBehaviour
    {
        public bool LandingThisFrame { get; private set; }
        
        [Header("COLLISION")] 
        [SerializeField] private Bounds _characterBounds;
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private int _detectorCount = 3;
        [SerializeField] private float _detectionRayLength = 0.1f;
        [SerializeField] [Range(0.1f, 0.3f)] private float _rayBuffer = 0.1f; // Prevents side detectors hitting the ground
        [Header("JUMPING")]
        [SerializeField] private float _coyoteTimeThreshold = 0.1f;

        private RayRange _raysUp, _raysRight, _raysDown, _raysLeft;
        private bool _colUp, _colRight, _colDown, _colLeft;
        private float _timeLeftGrounded;
        public bool CoyoteUsable;

        public bool CanUseCoyote => CoyoteUsable && !ColDown && TimeLeftGrounded + _coyoteTimeThreshold > Time.time;

        public bool ColUp => _colUp;
        public bool ColDown => _colDown;
        public bool ColLeft => _colLeft;
        public bool ColRight => _colRight;
        public float TimeLeftGrounded
        {
            get => _timeLeftGrounded;
            set => _timeLeftGrounded = value;
        }

        public LayerMask GroundLayer => _groundLayer;
        public Vector3 CharacterBounds => _characterBounds.size;

        public Action OnLanded = delegate { };

        public void ResetJump()
        {
            CoyoteUsable = false;
            TimeLeftGrounded = float.MinValue;
        }
        
        // We use these raycast checks for pre-collision information
        public void RunCollisionChecks() {
            // Generate ray ranges. 
            CalculateRayRanged();

            // Ground
            LandingThisFrame = false;
            var groundedCheck = RunDetection(_raysDown);
            if (_colDown && !groundedCheck) _timeLeftGrounded = Time.time; // Only trigger when first leaving
            else if (!_colDown && groundedCheck) {
                CoyoteUsable = true; // Only trigger when first touching
                LandingThisFrame = true;
                OnLanded?.Invoke();
            }

            _colDown = groundedCheck;

            // The rest
            _colUp = RunDetection(_raysUp);
            _colLeft = RunDetection(_raysLeft);
            _colRight = RunDetection(_raysRight);

            bool RunDetection(RayRange range) {
                return EvaluateRayPositions(range).Any(point => Physics2D.Raycast(point, range.Dir, _detectionRayLength, _groundLayer));
            }
        }

        private void CalculateRayRanged() {
            // This is crying out for some kind of refactor. 
            var b = new Bounds(transform.position, _characterBounds.size);

            _raysDown = new RayRange(b.min.x + _rayBuffer, b.min.y, b.max.x - _rayBuffer, b.min.y, Vector2.down);
            _raysUp = new RayRange(b.min.x + _rayBuffer, b.max.y, b.max.x - _rayBuffer, b.max.y, Vector2.up);
            _raysLeft = new RayRange(b.min.x, b.min.y + _rayBuffer, b.min.x, b.max.y - _rayBuffer, Vector2.left);
            _raysRight = new RayRange(b.max.x, b.min.y + _rayBuffer, b.max.x, b.max.y - _rayBuffer, Vector2.right);
        }
        
        private IEnumerable<Vector2> EvaluateRayPositions(RayRange range) {
            for (var i = 0; i < _detectorCount; i++) {
                var t = (float)i / (_detectorCount - 1);
                yield return Vector2.Lerp(range.Start, range.End, t);
            }
        }
        
        private void OnDrawGizmosSelected() {
            // Bounds
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position + _characterBounds.center, _characterBounds.size);

            // Rays
            if (!Application.isPlaying) {
                CalculateRayRanged();
                Gizmos.color = Color.blue;
                foreach (var range in new List<RayRange> { _raysUp, _raysRight, _raysDown, _raysLeft }) {
                    foreach (var point in EvaluateRayPositions(range)) {
                        Gizmos.DrawRay(point, range.Dir * _detectionRayLength);
                    }
                }
            }

            if (!Application.isPlaying) return;

            // Draw the future position. Handy for visualizing gravity
            // Gizmos.color = Color.red;
            // var move = new Vector3(_currentHorizontalSpeed, _currentVerticalSpeed) * Time.deltaTime;
            // Gizmos.DrawWireCube(transform.position + move, _characterBounds.size);
        }
    }
}