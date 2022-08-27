using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
	public Text score;
	public CharacterMovementController character;

    void Start()
    {
       score.text = "Your score: " + character.getScore(); 
    }
}
