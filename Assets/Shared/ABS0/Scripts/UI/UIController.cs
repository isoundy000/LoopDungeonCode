using UnityEngine;
using System.Collections;
using UniRx;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    public World World;
    Canvas mCanvas;

    public GameObject DamageText;
    public GameObject CritDamageText;

    public GameController gameController;

    public Text ComboText;
    public Animator ComboTextAnimator;

    int lastKillCount;

    // Use this for initialization
    void Start () {
        mCanvas = GetComponent<Canvas>();

        World.OnDamageAsObservable
            .Subscribe(damage =>
            {
                Vector3 screenPos = Camera.main.WorldToScreenPoint(damage.to.transform.position);
                Vector2 position;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(mCanvas.transform as RectTransform, screenPos, mCanvas.worldCamera, out position);

                GameObject damageTextObject;
                if(damage.isCirtical)
                {
                    damageTextObject = Instantiate(CritDamageText) as GameObject;
                } else
                {
                    damageTextObject = Instantiate(DamageText) as GameObject;
                }

                (damageTextObject.transform as RectTransform).SetParent(mCanvas.transform as RectTransform, false);
                (damageTextObject.transform as RectTransform).localPosition = position;

                if(damage.miss)
                {
                    damageTextObject.GetComponentInChildren<Text>().text = "MISS";
                }
                else
                {
                    damageTextObject.GetComponentInChildren<Text>().text = damage.value.ToString();
                }
            });

    }
	
	// Update is called once per frame
	void Update () {
	    if(gameController && lastKillCount != gameController.KillCount)
        {
            ComboText.gameObject.SetActive(gameController.KillCount >= 2);

            ComboText.text = "COMBO " + gameController.KillCount + "!";

            if(gameController.KillCount >= 10)
            {
                ComboTextAnimator.SetTrigger("killkillkill");
                ComboText.text = ComboText.text + "!";
            }
            lastKillCount = gameController.KillCount;
        }
	}

}
