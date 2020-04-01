using UnityEngine;

[RequireComponent(typeof(Health))]
public class Mannequin : MonoBehaviour, ITarget
{
    public Rect area = new Rect(-1, -1, 2, 2);

    public float minimalDistance = 1.0F;
    

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

            transform.position += direction;
        }
    }

    private void Damage(float value)
    {
        var textObject = Instantiate(ResourceUtility.resourceDatabase.damageTextPrefab, transform.position, Quaternion.identity);

        textObject.GetComponentInChildren<TextMesh>().text = value.ToString();
    }

    private void Wasted()
    {

    }
}
