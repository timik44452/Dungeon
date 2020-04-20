using UnityEngine;

using LiveWorld.Mobs.Core;
using LiveWorld.Mobs.Core.BehaviourModels.Examples;

[RequireComponent(typeof(CharacterController))]
public class Wolf : Mob
{
    public float speed = 4F;
    public float gravity = 12F;

    public Weapon weapon;
    public Skill skill;

    private CharacterController characterController;
    private Vector3 gravitationForce;
    private Vector3 actualPoint;

    private void Start()
    {
        DNA dna = new DNA(0, 1, 2, 3);

        MobConfiguration configuration = new MobConfiguration();
        AgressiveBehaviourModel model = new AgressiveBehaviourModel(dna);

        configuration.fieldOfView = 75;
        configuration.eyePower = 1.0F;

        characterController = GetComponent<CharacterController>();

        Initialize(configuration, model);
    }

    private void Update()
    {
        Vector3 direction = (actualPoint - transform.position).normalized;

        transform.rotation = Quaternion.LookRotation(direction);

        Vector3 force = gravitationForce + transform.forward * speed;

        characterController.Move(force * Time.deltaTime);

        if (characterController.isGrounded)
        {
            gravitationForce = Vector3.zero;
        }
        else
        {
            gravitationForce -= Vector3.up * gravity;
        }
    }

    public override void Await()
    {
        base.Await();
    }

    public override void Attack(ITarget target)
    {
        weapon.Invoke(this, target, skill);
    }

    public override void MoveTo(ITarget target)
    {
        actualPoint = target.transform.position;
    }

    public override void RunAway(ITarget target)
    {
        actualPoint = target.transform.position + (transform.position - target.transform.position).normalized * safe_distace;
    }
}
