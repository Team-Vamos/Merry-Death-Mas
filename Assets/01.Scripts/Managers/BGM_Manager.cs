using UnityEngine;
using UnityEngine.UI;

public class BGM_Manager : MonoSingleton<BGM_Manager>
{
    [SerializeField]
    private Transform record;

    [SerializeField]
    private RawImage recordImage;

    [SerializeField]
    private Image elbumImage;

    [SerializeField]
    private Sprite[] ImageSprites;

    [SerializeField]
    private AudioClip[] musics;

    private AudioSource audio;

    [SerializeField]
    private AudioSource btnAudio;

    public bool isFirst = true;

    private void Start()
    {
        Object[] objects = FindObjectsOfType<BGM_Manager>();
        DontDestroyOnLoad(this);
        if (objects.Length > 1) Destroy(gameObject);

        audio = GetComponent<AudioSource>();
        BtnSounds();
    }

    private void Update()
    {
        if(recordImage != null && recordImage.isActiveAndEnabled) SpinRecord();
    }

    public void Init(Transform _record, RawImage _recordImage, Image _elbumImage)
    {
        record = _record;
        recordImage = _recordImage;
        elbumImage = _elbumImage;
    }

    private void SpinRecord()
    {
        record.Rotate(0, 0, -5f);
    }

    public void TurnMusic(int music)
    {
        elbumImage.sprite = ImageSprites[music];
        recordImage.texture = ImageSprites[music].texture;
        audio.clip = musics[music];
        audio.Play();
    }

    public void BtnSounds()
    {
        Object[] btnsArr;
        btnsArr = FindObjectsOfTypeAll(typeof(Button));

        foreach (Object Btn in btnsArr)
        {
            Button btn = Btn as Button;
            btn.onClick.AddListener(() => btnAudio.Play());
        }
    }
}
