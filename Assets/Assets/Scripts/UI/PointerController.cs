using UnityEngine;

public class PointerController : MonoBehaviour
{
    public static PointerController Instance;
    private RectTransform pointerRect;
    public AudioClip moveClip;
    public AudioClip selectClip;
    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        pointerRect = GetComponent<RectTransform>();
        audioSource = GetComponent<AudioSource>();

    }

    public void ApontarPara(RectTransform target, Vector2 offset)
    {
        if (target == null) return;

        pointerRect.SetParent(target.parent);
        pointerRect.anchorMin = target.anchorMin;
        pointerRect.anchorMax = target.anchorMax;
        pointerRect.pivot = new Vector2(1, 0.5f);

        pointerRect.anchoredPosition = target.anchoredPosition + offset;
        // if (moveClip != null && audioSource != null)
        // {
        //     audioSource.PlayOneShot(moveClip);
        // }
    }

    public void Esconder()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    private float lastSoundTime = -1f;
    private const float minSoundInterval = 0.05f; // 50ms

    public void PlaySelectSound()
    {
        if (selectClip != null && audioSource != null && Time.time - lastSoundTime > minSoundInterval)
        {
            audioSource.PlayOneShot(selectClip);
            lastSoundTime = Time.time;
        }
    }
}
