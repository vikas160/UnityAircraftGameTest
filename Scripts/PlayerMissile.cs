using UnityEngine;

public class PlayerMissile : MonoBehaviour
{
    public float Missile_Range = 70f;    
    public float MissileSpeed = 250f;    
    public LayerMask enemyLayer;         

    private Transform target;


    private void Start()
    {
        Destroy(gameObject,8);    
    }
    private void Update()
    {
        DetectNearestEnemy();  

        if (target != null)
        {
          
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * MissileSpeed * Time.deltaTime;

   
            transform.rotation = Quaternion.LookRotation(direction);
        }
        else
        {
         
            transform.Translate(transform.forward * (MissileSpeed * Time.deltaTime), Space.World);
        }
    }

    private void DetectNearestEnemy()
    {

        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, Missile_Range, enemyLayer);

        float nearestDistance = Mathf.Infinity; 
        Transform nearestEnemy = null;

        foreach (Collider enemy in enemiesInRange)
        {
         
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);


            if (distanceToEnemy < nearestDistance)
            {
                nearestDistance = distanceToEnemy;
                nearestEnemy = enemy.transform;
            }
        }

        target = nearestEnemy; 
    }



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            Destroy(collision.collider.gameObject);
            PlayerInstance.Instance.UpScore();

        }

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
   
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, Missile_Range);
    }
}
