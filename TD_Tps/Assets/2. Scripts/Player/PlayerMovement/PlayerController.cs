using UnityEngine;

namespace HM_TopView.Controller
{
   [RequireComponent(typeof(Rigidbody), typeof(Animator))]
   public class PlayerController : MonoBehaviour
   {
      public float moveSpeed = 5f;
      public float rotateSpeed = 3f;
      
      private Animator animator;
      private Rigidbody rb;
      private Camera mainCam;
   
      private Vector3 camForward;
      private Vector3 move;
      private Vector3 moveInput;
   
      private float forwardAmount;
      private float turnAmount;
      private float animSpeed;
   
      private void Start()
      {
         animator = GetComponent<Animator>();
         rb = GetComponent<Rigidbody>();
         mainCam = Camera.main;
      }

      private void FixedUpdate()
      {
         RotatePlayerToMouse();
         MovePlayer();
      }

      private void MovePlayer()
      {
         float moveX = Input.GetAxis("Horizontal");
         float moveY = Input.GetAxis("Vertical");

         Vector3 _move = new Vector3(moveX, 0, moveY).normalized;
         animSpeed = _move.magnitude;
         
         if (mainCam != null)
         {
            camForward = Vector3.Scale(mainCam.transform.up, new Vector3(1, 0, 1)).normalized;
            move = moveY * camForward + moveX * mainCam.transform.right;
         }
         else
         {
            move = moveY * camForward + moveX * mainCam.transform.right;
         }

         if (move.magnitude > 1)
         {
            move.Normalize();
         }
          
         Move(move);
          
         if (animSpeed > 0)
         {
            // 이동 방향을 현재 캐릭터의 회전 방향을 기준으로 설정
            Vector3 moveDirection = new Vector3(moveX, 0, moveY).normalized;
            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
         }
      }
      
      private void Move(Vector3 _move)
      {
         if (_move.magnitude > 1)
         {
            _move.Normalize();
         }
      
         moveInput = _move;
            
         ConvertMoveInput();
         UpdateAnimator();
      }
       
      private void ConvertMoveInput()
      {
         Vector3 localMove = transform.InverseTransformDirection(moveInput);
      
         turnAmount = localMove.x;
         forwardAmount = localMove.z;
      }
      
      private void UpdateAnimator()
      {
         animator.SetFloat("MoveX", forwardAmount, 0.1f, Time.deltaTime);
         animator.SetFloat("MoveY", turnAmount, 0.1f, Time.deltaTime);
         animator.SetFloat("Speed", animSpeed);
      }
      
      private void RotatePlayerToMouse()
      {
         Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
      
         if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, LayerMask.GetMask("Ground")))
         {
            Vector3 targetPosition = hitInfo.point;
            Vector3 direction = targetPosition - transform.position;
            direction.y = 0; 
      
            if (direction != Vector3.zero)
            {
               Quaternion targetRotation = Quaternion.LookRotation(direction);
               transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);
            }
         }
      }
   }
}

