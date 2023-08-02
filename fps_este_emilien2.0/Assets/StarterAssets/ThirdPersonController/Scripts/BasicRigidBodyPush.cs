using UnityEngine;

namespace MyFirstPersonController
{
    [RequireComponent(typeof(CharacterController))]
    public class FirstPersonController : MonoBehaviour
    {
        public float MoveSpeed = 5.0f;
        public float JumpHeight = 1.2f;
        public float Gravity = -15.0f;
        public LayerMask GroundLayers;

        public bool CanPush = true;
        public LayerMask PushLayers;
        [Range(0.5f, 5f)] public float PushStrength = 1.1f;

        private CharacterController _controller;
        private Vector3 _playerVelocity;
        private bool _isGrounded;

        private void Start()
        {
            _controller = GetComponent<CharacterController>();
        }

        private void Update()
        {
            MovePlayer();
        }

        private void MovePlayer()
        {
            // Check if the player is grounded
            _isGrounded = Physics.CheckSphere(transform.position, 0.1f, GroundLayers, QueryTriggerInteraction.Ignore);

            if (_isGrounded && _playerVelocity.y < 0)
            {
                _playerVelocity.y = 0f;
            }

            // Get input for movement
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            // Move the player in the desired direction
            Vector3 moveDirection = transform.forward * moveVertical + transform.right * moveHorizontal;
            _controller.Move(moveDirection * MoveSpeed * Time.deltaTime);

            // Jump
            if (_isGrounded && Input.GetButtonDown("Jump"))
            {
                _playerVelocity.y += Mathf.Sqrt(JumpHeight * -2f * Gravity);
            }

            // Apply gravity
            _playerVelocity.y += Gravity * Time.deltaTime;
            _controller.Move(_playerVelocity * Time.deltaTime);
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (CanPush)
            {
                PushRigidBodies(hit);
            }
        }

        private void PushRigidBodies(ControllerColliderHit hit)
        {
            // make sure we hit a non kinematic rigidbody
            Rigidbody body = hit.collider.attachedRigidbody;
            if (body == null || body.isKinematic) return;

            // make sure we only push desired layer(s)
            var bodyLayerMask = 1 << body.gameObject.layer;
            if ((bodyLayerMask & PushLayers.value) == 0) return;

            // We dont want to push objects below us
            if (hit.moveDirection.y < -0.3f) return;

            // Calculate push direction from move direction, horizontal motion only
            Vector3 pushDir = new Vector3(hit.moveDirection.x, 0.0f, hit.moveDirection.z);

            // Apply the push and take strength into account
            body.AddForce(pushDir * PushStrength, ForceMode.Impulse);
        }
    }
}
