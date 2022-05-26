using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    // singlton instance
    public static PlayerController instance  { get; private set;}
    // game objects
    [SerializeField] private Camera fpsCam;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private GameObject impactEffect;
    // control variables
    [SerializeField] private float sensitivity;
    [SerializeField] private float speed;
    [SerializeField] private float fireRate = 2f;
    [SerializeField] private float nextTimeToShoot = 0f;
    [SerializeField] private AudioClip gunshot;
    private float horizontalRotation = 0;
    // components
    private AudioSource playerAudioPlayer;
    //private Rigidbody playerRb;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        instance = this;
        playerAudioPlayer = GetComponent<AudioSource>();
        //playerRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        MouseMovement();
        PlayerMovement();
        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToShoot)
        {
            nextTimeToShoot = Time.time + 1f / fireRate;
            Shoot();
        }
    }


    // handeling mouse movement
    private void MouseMovement()
    {
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivity;
        horizontalRotation -= mouseY;
        horizontalRotation = Mathf.Clamp(horizontalRotation, -90f, 90f);
        fpsCam.transform.localRotation = Quaternion.Euler(horizontalRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
    // player movement
    private void PlayerMovement()
    {
        float movementZ = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        float movementX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        transform.Translate(new Vector3(movementX, 0f, movementZ));
    }

    // shooting mechanic
    void Shoot()
    {
        muzzleFlash.Play();
        playerAudioPlayer.PlayOneShot(gunshot);
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit))
        {
            if (hit.transform.tag == "Target")
            {
                GameManager.Instance.CountUp();
                hit.transform.gameObject.SetActive(false);
            }
            if (hit.transform.tag != "Player")
            {
                GameObject impactRef = Instantiate(impactEffect , hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactRef, 2f);
            }
            if (hit.transform.CompareTag("Speaker"))
            {
                hit.transform.GetComponent<Speaker>().TrunMusicOnOff();
            }
        }
    }


    public void SetSensitivity(float value)
    {
        sensitivity = value * 10000;
    }
}
