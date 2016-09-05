using UnityEngine;
using System.Collections;
using UniRx;
public class AddKillCountWhenDied : MonoBehaviour {

    CharacterProperty mCharacterProperty;
    GameController gameController;

    // Use this for initialization
    void Start()
    {
        mCharacterProperty = GetComponent<CharacterProperty>();

        GameObject gameGameController = GameObject.FindGameObjectWithTag("GameController");

        if (gameGameController)
        {
            gameController = gameGameController.GetComponent<GameController>();

            mCharacterProperty
            .OnDiedAsObservable
            .Subscribe(t =>
            {
                if (t.CompareTag("Player"))
                {
                    gameController.AddKillCount();
                }
            });
        }


    }


    // Update is called once per frame
    void Update () {
	
	}
}
