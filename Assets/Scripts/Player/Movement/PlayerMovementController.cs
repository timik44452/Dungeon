using UnityEngine;

using Player.Movement;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementController : MonoBehaviour
{
    public PlayeMovementConfiguration movementConfiguration;

    private const float inputThresold = 0.05F;

    private Vector3 viewForward
    {
        get
        {
            Vector3 direction = Camera.main.transform.forward;

            direction.y = 0;

            return direction;
        }
    }
    private Vector3 viewRight
    {
        get
        {
            Vector3 direction = Camera.main.transform.right;

            direction.y = 0;

            return direction;
        }
    }

    private float vertical
    {
        get => Input.GetAxis("Vertical");
    }

    private float horizontal
    {
        get => Input.GetAxis("Horizontal");
    }

    private CharacterController characterController;

    private Vector3 gravityForce;
    private Vector3 direction;

    private AxisDownController horizontalAxisController;
    private AxisDownController verticalAxisController;

    private float move = 0.0F;
    private float sprint = 0.0F;
    private float jerk = 0.0F;

    private void Start()
    {
        verticalAxisController = new AxisDownController();
        horizontalAxisController = new AxisDownController();

        verticalAxisController.OnDownEvent += () => { if (verticalAxisController.pressCount % 2 == 0) jerk = 1.0F; };
        horizontalAxisController.OnDownEvent += () => { if (horizontalAxisController.pressCount % 2 == 0) jerk = 1.0F; };

        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        bool is_move =
            Mathf.Abs(vertical) > inputThresold ||
            Mathf.Abs(horizontal) > inputThresold;

        if (Input.GetKeyDown(KeyCode.X))
        {
            sprint = 0.25F;
        }

        verticalAxisController.Update(vertical, Time.deltaTime);
        horizontalAxisController.Update(horizontal, Time.deltaTime);

        if (is_move)
        {
            direction = Vector3.Lerp(direction, viewForward * vertical + viewRight * horizontal, movementConfiguration.maneurSpeed * Time.deltaTime);

            sprint += 3F * sprint * Time.deltaTime;
            move += 3F * Time.deltaTime;
        }
        else
        {
            move -= 5F * Time.deltaTime;
            sprint = 0;
        }

        jerk -= 5.0F * Time.deltaTime;

        sprint = Mathf.Clamp01(sprint);
        jerk = Mathf.Clamp01(jerk);
        move = Mathf.Clamp01(move);

        Vector3 jerkForce = jerk * movementConfiguration.jerkPower * direction;
        Vector3 moveForce = jerkForce + move * Mathf.Lerp(movementConfiguration.walkSpeed, movementConfiguration.runSpeed, sprint) * transform.forward;

        characterController.Move((moveForce + gravityForce) * Time.deltaTime);

        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), 9.0F * Time.deltaTime);
        }

        if (characterController.isGrounded)
        {
            gravityForce = Input.GetKey(KeyCode.Space) ? Vector3.up * movementConfiguration.jumpPower : Vector3.zero;
        }
        else
        {
            gravityForce -= Vector3.up * movementConfiguration.gravity * Time.deltaTime;
        }
    }
}
