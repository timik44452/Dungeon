using UnityEngine;

// ------------------------
// Beviour model: Rabbit
// ------------------------
[RequireComponent(typeof(Health))]
public class Mannequin : MonoBehaviour, ITarget
{
    public Rect area = new Rect(-1, -1, 2, 2);

    public float speed = 4.0F;

    public float minimalDistance = 1.0F;

    private const int maneur_iterations = 5;

    private void Start()
    {
        var healthComponent = GetComponent<Health>();

        healthComponent.OnDamaged += Damage;
        healthComponent.OnWasted += Wasted;
    }

    private void FixedUpdate()
    {
        Transform playerTransform = SceneUtility.Player.transform;

        float distance = Vector3.Distance(playerTransform.position, transform.position);

        if(distance < minimalDistance)
        {
            Vector3 direction = (transform.position - playerTransform.position).normalized;

            direction.y = 0;

            for (int i = 0; i < maneur_iterations; i++)
            {
                if (Physics.Raycast(transform.position, direction, out RaycastHit hit, minimalDistance * 0.5F))
                {
                    Vector3 maneur = (direction + hit.normal) * 0.5F;

                    maneur.y = 0;

                    direction = maneur;
                }
                else
                {
                    break;
                }
            }

            float boost = Mathf.Clamp(minimalDistance / distance, 1, 2);

            transform.position += direction * speed * boost * Time.deltaTime;
        }
    }

    private void Damage(Component sender, float value)
    {
        var textObject = Instantiate(ResourceUtility.resourceDatabase.damageTextPrefab, transform.position, Quaternion.identity);

        textObject.GetComponentInChildren<TextMesh>().text = value.ToString();
    }

    private void Wasted(Component sender)
    {

    }
}
