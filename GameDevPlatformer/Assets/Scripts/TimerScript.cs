using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TimerScript : MonoBehaviour {

    public Text timer;

    float initialTime;

    bool isFinished = false;
    bool TimerStart = false;
    bool FirstPress = true;

	void Start () {
	}
		
    void Update () {
        float time = 0;
        string minutes = ((int)time / 60).ToString();
        string seconds = (time % 60).ToString("f2");

		// Detect if player has moved horizontally
		if (Input.GetKeyDown(KeyCode.LeftArrow)  || Input.GetKeyDown(KeyCode.RightArrow)) {
            if (FirstPress) {
                TimerStart = true;
                initialTime = Time.time;
            }
            FirstPress = false;
        }

        if (TimerStart) {
            time = Time.time - initialTime;
            minutes = ((int)time / 60).ToString();
            seconds = (time % 60).ToString("f2");
            timer.text = minutes + ":" + seconds;
        }
        timer.text = minutes + ":" + seconds;

		if (Input.GetKeyDown (KeyCode.R)) {
			TimerStart = false;
			FirstPress = true;
			time = 0;
		}
    }
}
