using UnityEngine;

public class EnemyAI : MonoBehaviour
{
  
    public float speed = 20f;
    public float rotateSpeed = 3f;
    public float attackRange = 50f;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float shootInterval = 2f;

    private Rigidbody rb;
    private Vector3 randomDirection;

    public int DmgRate;
  

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        InvokeRepeating(nameof(Shoot), 2f, shootInterval);
        InvokeRepeating(nameof(SetRandomDirection), 0f, 5f); // Change direction every 5 seconds
    }

    void Update()
    {
        float distanceToTarget = Vector3.Distance(transform.position, PlayerInstance.Instance.PlayerPos.position);

        if (distanceToTarget <= attackRange)
        {
            // Attack the target
            Vector3 direction = (PlayerInstance.Instance.PlayerPos.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotateSpeed);
            PlayerInstance.Instance.PlayerAlertMusic();
        }
        else
        {
            // Fly randomly
            Quaternion lookRotation = Quaternion.LookRotation(randomDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotateSpeed);
           PlayerInstance.Instance.StopAlerMusic();
        }

        rb.velocity = transform.forward * speed;
    }

    void Shoot()
    {
        float distanceToTarget = Vector3.Distance(transform.position, PlayerInstance.Instance.PlayerPos.position);
        if (distanceToTarget <= attackRange)
        {
            Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        }
    }

    void SetRandomDirection()
    {
        randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            PlayerInstance.Instance.TakeDmg(DmgRate);
            Destroy(gameObject);
        }
      
              
    }

    
}
