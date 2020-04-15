using UnityEngine;

using LiveWorld.Mobs.Core;
using LiveWorld.Mobs.Core.BehaviourModels.Examples;

[RequireComponent(typeof(Health))]
public class Firefly : Mob
{
    public float speed = 4.0F;

    private Vector3 actualPoint = Vector3.zero;
    private float time = 0.0F;

    private void Start()
    {
        var healthComponent = GetComponent<Health>();

        healthComponent.OnDamaged += Damage;
        healthComponent.OnWasted += Wasted;

        DNA dna = new DNA(0, 1, 2);

        MobConfiguration configuration = new MobConfiguration();
        InterestBehaviourModel model = new InterestBehaviourModel(dna);

        configuration.fieldOfView = 180;
        configuration.eyePower = 1.0F;

        actualPoint = transform.position;

        Initialize(configuration, model);
    }

    private void Update()
    {
        Vector3 offset = MathUtility.GetSpherePoint(2.0F, time, time);
        Vector3 direction = (actualPoint + offset - transform.position).normalized;

        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, 10 * 0.5F))
        {
            Vector3 maneur = (direction + hit.normal) * 0.5F;

            direction = maneur;
        }

        transform.position += direction * speed * Time.deltaTime;
        time = Mathf.Repeat(time + Random.value * Time.deltaTime * 0.5F, 1F);

        transform.position = new Vector3(
            transform.position.x,
            Mathf.Clamp(transform.position.y, 1.0F, 2.5F),
            transform.position.z);
    }

    public override void Await()
    {
        Vector3 circle = MathUtility.GetSpherePoint(10F, Random.value, Random.value);

        circle.y = 0;

        actualPoint += circle;

        GetComponent<LineRenderer>().positionCount = 0;
    }

    public override void MoveTo(ITarget target)
    {
        actualPoint = target.transform.position;

        //GetComponent<LineRenderer>().positionCount = 2;
        //GetComponent<LineRenderer>().SetPosition(0, transform.position);
        //GetComponent<LineRenderer>().SetPosition(1, actualPoint);
    }

    public override void RunAway(ITarget target)
    {
        actualPoint = target.transform.position + (transform.position - target.transform.position).normalized * safe_distace;

        //GetComponent<LineRenderer>().positionCount = 2;
        //GetComponent<LineRenderer>().SetPosition(0, transform.position);
        //GetComponent<LineRenderer>().SetPosition(1, actualPoint);
    }

    private void Damage(Component sender, float value)
    {
        var textObject = Instantiate(ResourceUtility.resourceDatabase.damageTextPrefab, transform.position, Quaternion.identity);

        textObject.GetComponentInChildren<TextMesh>().text = value.ToString();
    }

    private void Wasted(Component sender)
    { 
        Destroy(gameObject);
    }
}
