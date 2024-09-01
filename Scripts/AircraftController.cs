using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AircraftController : MonoBehaviour
{
    [Header("Aircraft Settings")]
    [SerializeField] private float Responsiveness = 2;
    [SerializeField] private float ThrottleIncrement = 0.1f;
    [SerializeField] private float MaxThrottle = 350;
    [SerializeField] private float MaxThrust = 200;
    [SerializeField] private float UpLift = 150;

    // References
    private Rigidbody AircraftRb;
    [Header("Aircraft Fps Camera")]
    [SerializeField] private GameObject AircraftFpsCam = default;
    private AudioSource AircraftAudioSource = null;    
    [Header("Aircraft Throttle Vfs")]
    [SerializeField] private GameObject AircraftThorttleVfx = default;
    [SerializeField] private float ThrottleVfxActiveSpeed = 15;

    //Aircraft Shooting Settings
    [Header("Aircraft Shooting Settings")]
    [Header("Missile Prefab")]
    [SerializeField] private GameObject MissilePrefab = default;
    [SerializeField] private Transform ShootingPoint = default;
    [SerializeField] private float NextTimeToFire = 5;
    private float time = 0;

    [Header("Audio")]
    [SerializeField] private AudioClip ShootSoundClip = default;
    [SerializeField] private AudioSource GunSoundsAudioSource = default;

    [Header("Aircraft Ui Elements")]
    public TextMeshProUGUI ThrottleText;
    public TextMeshProUGUI SpeedText;
    public TextMeshProUGUI HealthText;


    // Aircraft settings
    private float Throttle;
    private float yaw;
    private float pitch;
    private float roll;
    private bool isFpsCamActive = false;


 

    private float responseModifier
    {
        get
        {
            return (AircraftRb.mass / 10) * Responsiveness;
        }
    }

    private void Awake()
    {
        AircraftRb = GetComponent<Rigidbody>();
        AircraftAudioSource = GetComponent<AudioSource>();  
    }

    private void Update()
    {
        HandleInputs();
        SwitchCamera();
        HandleVfx();
        PlayEngineSound();
        ShootMissile();
        UiManager();
    }

    private void HandleInputs()
    {
        pitch = Inputs.Instance.VrInput;
        yaw = Inputs.Instance.HrInput;
        roll = Inputs.Instance.Roll_Input;

        if (Inputs.Instance.ThrottleBtn) Throttle += ThrottleIncrement;
        if (Inputs.Instance.BreakeBtn) Throttle -= ThrottleIncrement;
        Throttle = Mathf.Clamp(Throttle, 0, MaxThrottle);
    }

    private void FixedUpdate()
    {
        if (AircraftRb != null)
        {
            ControlAircraft();
        }
    }

    private void ControlAircraft()
    {
        AircraftRb.AddForce(transform.forward * MaxThrust * Throttle);
        AircraftRb.AddForce(Vector3.up * AircraftRb.velocity.magnitude * UpLift);


        AircraftRb.AddTorque(transform.up * yaw * responseModifier);
        AircraftRb.AddTorque(transform.right * pitch * responseModifier);
        AircraftRb.AddTorque(-transform.forward * roll * responseModifier * Time.deltaTime);


    }

    void SwitchCamera()
    {
       if(Inputs.Instance.CamSwitchBtn && !isFpsCamActive)
        {
            isFpsCamActive = true;
            AircraftFpsCam?.SetActive(true);
           
        }
       else if(Inputs.Instance.CamSwitchBtn && isFpsCamActive)
        {
            isFpsCamActive = false;
            AircraftFpsCam?.SetActive(false);
            
        }
    }


    void HandleVfx()
    {
        if(AircraftRb.velocity.magnitude>= ThrottleVfxActiveSpeed)
        {
            AircraftThorttleVfx.SetActive(true);
        }
        else
        {
            AircraftThorttleVfx.SetActive(false);
        }
    }

    void PlayEngineSound()
    {
        if(AircraftAudioSource!=null)
        AircraftAudioSource.volume = Throttle * 0.01f;
    }
    
    void ShootMissile()
    {

        time += Time.deltaTime;
        if (Inputs.Instance.ShootBtn && (time>NextTimeToFire))
        {
            GunSoundsAudioSource.PlayOneShot(ShootSoundClip);
          Instantiate(MissilePrefab, ShootingPoint.position,ShootingPoint.rotation);
            time = 0;
        }

    

    }

    void UiManager()
    {
        if ((ThrottleText != null || SpeedText != null)) {

            ThrottleText.text = "Throttle \n" + (int)Throttle;
            SpeedText.text = "Speed \n" + (int)AircraftRb.velocity.magnitude;
            HealthText.text = "Health = " + PlayerInstance.Instance.PlayerHealth;
        }

       
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (AircraftRb.velocity.magnitude >= 50 && collision.collider.tag != "Pm")
        {
            PlayerInstance.Instance.TakeDmg(100);
        }
    }

    #region InputDebug
    private void DebugInput()
    {
        if (Inputs.Instance.ShootBtn)
        {
            Debug.LogWarning("Shooting");
        }
    }
    #endregion
}   
