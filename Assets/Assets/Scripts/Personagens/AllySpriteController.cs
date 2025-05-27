using UnityEngine;
using UnityEngine.UI;

public class AllySpriteController : MonoBehaviour
{
    public Image characterImage;
    public Sprite idleSprite;
    public Sprite attackSprite;
    public Sprite defendSprite;
    public Sprite healSprite;
    public Sprite deadSprite;
    public Sprite winSprite;
    public GameObject espada;

    public void SetIdle()
    {
        characterImage.sprite = idleSprite;
        espada.SetActive(false);
    }

    public void SetAttack()
    {
        characterImage.sprite = attackSprite;
        espada.SetActive(true);
    }

    public void SetDefend()
    {
        characterImage.sprite = defendSprite;
        espada.SetActive(false);
    }

    public void SetHeal()
    {
        characterImage.sprite = healSprite;
        espada.SetActive(false);
    }

    public void SetDead()
    {
        characterImage.sprite = deadSprite;
        espada.SetActive(false);
    }

    public void SetWin()
    {
        characterImage.sprite = winSprite;
        espada.SetActive(false);
    }
}
