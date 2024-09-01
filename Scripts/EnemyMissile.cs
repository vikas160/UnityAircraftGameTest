using UnityEngine;

public class EnemyMissile : MonoBehaviour
{
    public float Missile_Range = 70f;
    public float MissileSpeed = 250f;
    public float dmgRate = 10;
   
    private void Update()
    {
        float distanceFromTarget = Vector3.Distance(transform.position, PlayerInstance.Instance.PlayerPos.position);

        if (distanceFromTarget <= Missile_Range)
        {
            Vector3 direction = (PlayerInstance.Instance.PlayerPos.position - transform.position).normalized;
            transform.position += direction * MissileSpeed * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(direction);
           // PlayerInstance.Instance.PlayerAlertMusic();
        }
        else
        {
            transform.Translate(transform.forward * (MissileSpeed * Time.deltaTime), Space.World);
           // PlayerInstance.Instance.StopAlerMusic();    
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, Missile_Range);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            PlayerInstance.Instance.TakeDmg(10);
            
        }

        Destroy(gameObject,0.1f);
    }
}
