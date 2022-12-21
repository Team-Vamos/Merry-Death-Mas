using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField]
    private GameObject SnowPile;

    public GameObject StarUpgrade;

    public GameObject SnowManObj;

    [Header("====== Player Info ======")]
    [SerializeField]
    private GameObject player;

    private int candy = 0; //플레이어가 지닌 사탕 수
    private int snows = 0; //플레이어가 지닌 눈 수
    private float shovelDmg = 4f; //삽 데미지 @@@@@@

    public int santaSpawnRate = 4; //산타 스폰 주기

    public float multiplySnow = 1; // 얻는 눈 개수 배율
    public float multiplyCandy = 1; // 얻는 사탕 개수 배율
    public int snowSpawnRate = 8; //눈 소환 주기
    public float snowPileTime = 60f; //눈 1단계 쌓이는 시간

    public int respawnTime = 3;    //부활 시간
    public int playerHp;   //플레이어 체력 @@@@@@
    public int playerMaxHp = 10;
    public float playerAutoHealingCoolTime = 10f;
    public float playerAtkSpd = 1f; //(60 프레임 * playerAtkSpd)에 한번 휘두름 @@@@@@
    public float FreezeTime = 3f; //적 빙결 시간
    public int SnowBallDmg = 10; //눈덩이 데미지 @@@@@@

    [Header("====== Upgarde Info ======")]
    public float StarDmg = 1f; //별 터질때 데미지 @@@@@@
    public float StartDelay = 1f;

    public float TurretDmg = 1f; //터렛 데미지 @@@@@@
    public float TurrentDelay = 1f;
    public float TurretRange = 1f;

    public int BallSize = 3;
    public float BallSpd = 50f;
    public float BallDmg = 3f;
    public float BallAtkDelay = 0.5f;

    [Header("====== Tree Info ======")]
    public float TreeMaxHp = 10f;
    public float TreeHp;
    public float TreeHeal = 10f;

    #region ====== Getters ======
    public int getCandy
    {
        get => candy;
    }

    public int getSnow
    {
        get => snows;
    }

    public float ShovelDmg
    {
        get => shovelDmg;
    }
    #endregion

    [Header("====== Pools ======")]
    public Transform snowPoolManager;

    public Transform presentPoolManager;

    public Transform addCandyPool;

    [SerializeField]
    private Image BGBtn;

    [SerializeField]
    private Image EnvBtn;

    [SerializeField]
    private Sprite[] onOffSprite;

    [Header("====== UIs ======")]
    [SerializeField]
    private Text candyTxt;

    [SerializeField]
    private Image NightImage;

    [SerializeField]
    private Image AfterNoonImage;

    [SerializeField]
    private Text addCcandyTxt;

    [SerializeField]
    private Transform textPanel;

    [SerializeField]
    private Text RespawnTxt;

    [SerializeField]
    private GameObject RespawnPanel;

    [SerializeField]
    private Text SnowCnt;

    public GameObject Hert;

    public bool Enabled = false;

    [SerializeField]
    private GameObject outOfSnow;

    [SerializeField]
    private GameObject Danger;

    [SerializeField]
    private GameObject settingPanel;
    
    [SerializeField]
    private GameObject escPanel;

    [SerializeField]
    private Slider BGSlider;

    [SerializeReference]
    private Slider EnvSlider;

    [SerializeField]
    private Button[] EndPanels;

    [SerializeField]
    private Text[] endTexts;

    [Header("Sounds")]
    [SerializeField]
    private AudioClip Vanish;

    private AudioListener listener;

    [SerializeField]
    private AudioMixer mixer;

    public AudioMixerGroup Mixer;

    [SerializeField]
    private AudioMixerGroup BGmixer;

    bool BGMuted = false;
    bool ENVMuted = false;

    public AudioClip vanishSound
    {
        get => Vanish;
    }

    [SerializeField]
    private AudioClip[] hitSounds;

    public int presents
    {
        get => presentPoolManager.childCount;
    }

    [Header("BGM_Manager")]
    [SerializeField]
    private Transform record;

    [SerializeField]
    private RawImage recordImage;

    [SerializeField]
    private Image elbumImage;

    private bool End = false;

    private void Start()
    {
        playerHp = playerMaxHp;
        TreeHp = TreeMaxHp;
        listener = GetComponent<AudioListener>();
        StartCoroutine(CreateSnow());
        AddCandy(0);
        AddSnow(0);
        BGM_Manager.Instance.BtnSounds();
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            if(Enabled == false)
            {
                OnESC();
            }
            else if(Enabled == true)
            {
                Resume();
            }
        }
    }

    IEnumerator CreateSnow()
    {
        while (true)
        {
            SpawnSnow();
            yield return new WaitForSeconds(snowSpawnRate);
        }
    }

    private void SpawnSnow()
    {
        if (snowPoolManager.childCount > 1)
        {
            snowPoolManager.GetChild(0).position = RandomPos();
            snowPoolManager.GetChild(0).gameObject.SetActive(true);
            snowPoolManager.GetChild(0).SetParent(null);
        }
    }


    public void GetTreeDmg(int amount) => TreeHp -= amount;

    public void AddSnow(int amount)
    {
        snows += (int)(amount * multiplySnow);
        SnowCnt.text = snows.ToString();
    }

    public void AddCandy(int amount)
    {
        if (amount > 0) AddCandyPool(amount);
        candy += (int)(amount * multiplyCandy);
        candyTxt.text = $"{candy}";
    }

    public void ChangeImage(bool night)
    {
        if(night==true)
        {
            AfterNoonImage.enabled = true;
            NightImage.enabled = false;
        }
        else if(night == false)
        {
            AfterNoonImage.enabled = false;
            NightImage.enabled = true;
        }
    }

    public void UseSnow()
    {
        --snows;
        SnowCnt.text = snows.ToString();
    }

    public void MultiplyShovelDmg(float amount) => shovelDmg = amount;

    private Vector3 RandomPos()
    {
        float randomZ = Random.Range(10f, 30f);
        float randomX = Random.Range(10f, 30f);
        Vector3 pos = new Vector3(randomX, -1.48f, randomZ);
        return pos;
    }

    public int RandomCandy(int min, int max)
    {
        int Candy = Random.Range(min, max + 1);
        return Candy;
    }

    public void RespawnPlayer()
    {
        listener.enabled = true;
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        RespawnPanel.SetActive(true);
        for (int i = respawnTime; i > 0; i--)
        {
            RespawnTxt.text = $"{i}";
            yield return new WaitForSeconds(1f);
        }
        RespawnPanel.SetActive(false);
        playerHp = playerMaxHp;
        listener.enabled = false;
        player.SetActive(true);
    }

    public void ResetSanta(GameObject santa)
    {
        float randomX = Random.Range(-30f, 30f);

        santa.transform.position = new Vector3(randomX, 25f, -100f);
        santa.SetActive(false);
        StartCoroutine(spawnSanta(randomX, santa));
    }

    IEnumerator spawnSanta(float randomTime, GameObject santa)
    {
        yield return new WaitForSeconds((randomTime/3) * santaSpawnRate);
        santa.SetActive(true);
    }

    public void AddCandyPool(int candyCnt)
    {
        Text text;
        string sign = "";
        sign = candyCnt > 0 ? "+" : "";
        if (addCandyPool.childCount < 1)
        {
            text = Instantiate(addCcandyTxt).GetComponent<Text>();
            text.transform.SetParent(textPanel);
            text.transform.localScale = Vector3.one;
            text.text = $"{sign}{candyCnt}";
        }
        else
        {
            text = addCandyPool.GetChild(0).GetComponent<Text>();
            text.gameObject.SetActive(true);
            text.transform.SetParent(textPanel);
            text.transform.localScale = Vector3.one;
            text.text = $"{sign}{candyCnt}";
        }
    }

    public void OutOfSnow()
    {
        outOfSnow.SetActive(true);
    }

    public void DangerOn(bool on)
    {
        Danger.SetActive(on);
    }

    public AudioClip randomHitSound()
    {
        int randomInt = Random.Range(0, 1);
        return hitSounds[randomInt];
    }

    private void OnESC()
    {
        Debug.Log(",");
        escPanel.SetActive(true);
        settingPanel.SetActive(false);
        BGM_Manager.Instance.Init(record, recordImage, elbumImage);
        Enabled = true;
        Time.timeScale = 0;
    }

    public void Resume()
    {
        Debug.Log("!");
        escPanel.SetActive(false);
        settingPanel.SetActive(false);
        Enabled = false;
        Time.timeScale = 1;
    }

    public void Setting(bool on)
    {
        settingPanel.SetActive(on);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void MuteEnv()
    {
        if(ENVMuted)
        {
            mixer.SetFloat("EnvVolume", Mathf.Log10(EnvSlider.value) * 20);
            EnvBtn.sprite = onOffSprite[0];
        }
        else
        {
            mixer.SetFloat("EnvVolume", Mathf.Log10(0.001f) * 20);
            EnvBtn.sprite = onOffSprite[1];
        }
        ENVMuted = !ENVMuted;
    }

    public void MuteBGM()
    {
        if (BGMuted)
        {
            mixer.SetFloat("BGMVolume", Mathf.Log10(BGSlider.value) * 20);
            BGBtn.sprite = onOffSprite[0];
        }
        else
        {
            mixer.SetFloat("BGMVolume", Mathf.Log10(0.001f) * 20);
            BGBtn.sprite = onOffSprite[1];
        }
        BGMuted = !BGMuted;
    }

    public void SetEnv()
    {
        float value = EnvSlider.value;
        mixer.SetFloat("EnvVolume", Mathf.Log10(value) * 20);
        if (value <= 0.003f) EnvBtn.sprite = onOffSprite[1];
        else EnvBtn.sprite = onOffSprite[0];
    }

    public void SetBGM()
    {
        float value = BGSlider.value;
        mixer.SetFloat("BGMVolume", Mathf.Log10(value) * 20);
        if (value <= 0.003f) BGBtn.sprite = onOffSprite[1];
        else EnvBtn.sprite = onOffSprite[0];
    }

    public void Ending(int num)
    {
        if(!End)
        {
            StartCoroutine(FadeImage(num));
        }
    }

    private IEnumerator FadeImage(int num)
    {
        Button EndPanel = EndPanels[num];
        EndPanel.gameObject.SetActive(true);

        End = true;
        float i = 0;
        Color _color = EndPanel.image.color;
        while (EndPanel.image.color.a < 0.99f)
        {
            EndPanel.image.color = (_color + new Color(0f, 0f, 0f, i));
            i += 0.01f;
            yield return new WaitForSeconds(0.02f);
        }
        Time.timeScale = 0;
        endTexts[num].gameObject.SetActive(true);
        EndPanel.onClick.AddListener(() =>
        {
            End = false;
            Time.timeScale = 1;
            SceneManager.LoadScene("MainTitle");
        });
    }
}
