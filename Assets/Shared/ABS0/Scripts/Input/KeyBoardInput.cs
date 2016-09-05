using UnityEngine;
using System.Collections;

public class KeyBoardInput : MonoBehaviour {

    public bool DirectionControllable;
    public bool AttackControllable;

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        if(DirectionControllable)
        {
            ABS0TouchInput.InputVector.y = Input.GetAxisRaw("Vertical");
            ABS0TouchInput.InputVector.x = Input.GetAxisRaw("Horizontal");
        }
        if(AttackControllable) {
            ABS0TouchInput.Attack = Input.GetButtonUp("Jump");
        }
       
    }
}
