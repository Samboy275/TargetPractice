using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class GameManager : MonoBehaviour
{
    // singlton instance 
    public static GameManager Instance{ get; private set;}

    // UI elements
    [SerializeField] private Text counter;
    [SerializeField] private TextMeshProUGUI hintToStart;
    [SerializeField] private TextMeshProUGUI TimerText;
    [SerializeField] private TextMeshProUGUI endGameText;
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject startMenu;
    [SerializeField] private Button[] buttons;

    // components
    private AudioSource mainmenuMusic;
    // control variables
    [SerializeField] private float timer;
    [SerializeField] private int timeLimit = 60;
    private int count = 0;
    public bool isStarted { get; private set;}
    private bool isPaused;

    
    // Start is called before the first frame update
    void Awake()
    {
        mainmenuMusic = GetComponent<AudioSource>();
        for (int i = 0; i < buttons.Length; i++)
        {
            float diffcultyCalc = i * 0.25f;
            buttons[i].onClick.AddListener( () => StartShootingGallery(1 - diffcultyCalc, 2 - diffcultyCalc));
        }
        isStarted = false;
        if (Instance != null)
        {
            Destroy(this);
        }
        Instance = this;
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        startMenu.SetActive(true);
        isPaused = false;
        PlayerController.instance.enabled = false;
        hintToStart.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1;
                isPaused = false;
                pauseMenu.SetActive(false);
                PlayerController.instance.enabled = true;
            }
            else
            {
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                isPaused = true;
                PlayerController.instance.enabled = false;
                pauseMenu.SetActive(true);
            }
        }
        Timer();
    }


    public void CountUp ()
    {
        count++;
        counter.text = "Targets: " + count;
    }

    public void StartCounting()
    {
        isStarted = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SpawnManager.instance.StartSpawning();
            timer = 0;
            hintToStart.enabled = false;
        }
    }

    private void Timer()
    {
         if (isStarted)
        {
            Debug.Log("Game Starting up");
            timer += Time.deltaTime;
        }
        if (timer >= timeLimit)
        {
            isStarted = false;
            Debug.Log("time limit reached");
            SpawnManager.instance.StopSpawning();
            GameOver();
        }

        TimerText.text = "Time Limit: " + Mathf.RoundToInt(timer);
    }

    public void ChangeSesitivity()
    {
        PlayerController.instance.SetSensitivity(sensitivitySlider.value);
    }

    private void GameOver()
    {
        Cursor.lockState = CursorLockMode.None;
        gameOverMenu.SetActive(true);
        PlayerController.instance.enabled = false;
        endGameText.text = "You Shot " + count + " Out Of " + SpawnManager.instance.GetNumberOfTargets() + " Targets";
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartShootingGallery(float spawnRate, float ttl)
    {
        Cursor.lockState = CursorLockMode.Locked;
        mainmenuMusic.Stop();
        Speaker.instance.TrunMusicOnOff();
        SpawnManager.instance.SetSpawnRate(spawnRate);
        ObjectPooler.Instance.SetTimeToLiveForTargets(ttl);
        PlayerController.instance.enabled = true;
        startMenu.SetActive(false);
        count = 0;
        hintToStart.gameObject.SetActive(true);
        StartCoroutine(MakeTextDisapear());
    }

    IEnumerator MakeTextDisapear()
    {
        yield return new WaitForSeconds(10f);
        hintToStart.gameObject.SetActive(false);
    }
}
