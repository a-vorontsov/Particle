using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TimerScript : MonoBehaviour {

    public Text timer;

    float initialTime;

    bool timerStart = false;
    bool firstPress = true;

	void Start() {
	}

    void Update() {
        float time = 0;
        string minutes = ((int)time / 60).ToString();
        string seconds = (time % 60).ToString("f2");

		// Detect if player has moved horizontally
		if (Input.GetKeyDown(KeyCode.LeftArrow)
            || Input.GetKeyDown(KeyCode.RightArrow)
            || Input.GetKeyDown(KeyCode.A)
            || Input.GetKeyDown(KeyCode.D)) {
            if (firstPress) {
                timerStart = true;
                initialTime = Time.time;
            }
            firstPress = false;
        }

        if (timerStart) {
            time = Time.time - initialTime;
            minutes = ((int)time / 60).ToString();
            seconds = (time % 60).ToString("f2");
            timer.text = minutes + ":" + seconds;
        }
        timer.text = minutes + ":" + seconds;

		if (Input.GetKeyDown(KeyCode.R)) {
			timerStart = false;
			firstPress = true;
			time = 0;
		}
    }
}
