using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {

	public bool fullScreen;
	public float masterVolume;
	public float musicVolume;
	public float fxVolume;

	public GameObject mainMenuCanvas;
	public GameObject settingsCanvas;
	public GameObject selectLevelCanvas;

	public int[] screenWidths;
	public int[] screenHeights;
	public Toggle fullScreenToggle;
	public Dropdown resolutionDropdown;
	public Slider masterSlider;
	public Slider musicSlider;
	public Slider fxSlider;
	public AudioSource musicSource;
	public AudioSource fxSource;

	public int activeResIndex;

	// Force add listeners to objects
	void OnEnable () {
		fullScreenToggle.onValueChanged.AddListener (delegate {OnFullScreenToggle ();});
		resolutionDropdown.onValueChanged.AddListener (delegate {OnResolutionChange (resolutionDropdown.value);});
		masterSlider.onValueChanged.AddListener (delegate {OnVolumeChange();});
		musicSlider.onValueChanged.AddListener (delegate {OnVolumeChange();});
		fxSlider.onValueChanged.AddListener (delegate {OnVolumeChange();});

		activeResIndex = PlayerPrefs.GetInt ("screen res");
		fullScreen = (PlayerPrefs.GetInt ("fullscreen") == 1) ? true : false;
		AudioListener.volume = masterSlider.value = PlayerPrefs.GetFloat ("master volume");
		musicSource.volume = musicSlider.value = PlayerPrefs.GetFloat ("music volume");
		fxSource.volume = fxSlider.value = PlayerPrefs.GetFloat ("effects volume");
	}

	void Start () {
		activeResIndex = PlayerPrefs.GetInt ("screen res");
		bool fullScreen = (PlayerPrefs.GetInt ("fullscreen") == 1) ? true : false;
		AudioListener.volume = masterSlider.value = PlayerPrefs.GetFloat ("master volume");
		musicSource.volume = musicSlider.value = PlayerPrefs.GetFloat ("music volume");
		fxSource.volume = fxSlider.value = PlayerPrefs.GetFloat ("effects volume");

		mainMenuCanvas.SetActive (true);
		settingsCanvas.SetActive (false);
		selectLevelCanvas.SetActive (false);
		activeResIndex = resolutionDropdown.value;
	}

	// Toggle fullscreen
	public void OnFullScreenToggle () {
		Screen.fullScreen = fullScreenToggle.isOn;

		PlayerPrefs.SetInt ("fullscreen", ((fullScreen) ? 1 : 0));
		PlayerPrefs.Save ();
	}

	// Change resolution
	public void OnResolutionChange (int activeResIndex) {
		Screen.SetResolution(screenWidths[resolutionDropdown.value], screenHeights[resolutionDropdown.value], Screen.fullScreen);
		PlayerPrefs.SetInt ("screen res", activeResIndex);
		PlayerPrefs.Save ();
	}

	// Change volume
	public void OnVolumeChange () {
		AudioListener.volume = masterSlider.value;
		PlayerPrefs.SetFloat ("master volume", masterSlider.value);
		musicSource.volume = musicSlider.value;
		PlayerPrefs.SetFloat ("music volume", musicSlider.value);
		fxSource.volume = fxSlider.value;
		PlayerPrefs.SetFloat ("effects volume", fxSlider.value);
		PlayerPrefs.Save ();
	}

	// Start game on press
	public void StartGame (string levelName) {
		SceneManager.LoadScene (levelName);
	}

	// Changes the active canvas
	public void SelectLevelCanvas () {
		selectLevelCanvas.SetActive (true);
		mainMenuCanvas.SetActive(false);
		settingsCanvas.SetActive (false);
	}

	public void SettingsCanvas () {
		settingsCanvas.SetActive (true);
		mainMenuCanvas.SetActive(false);
		selectLevelCanvas.SetActive (false);

		activeResIndex = PlayerPrefs.GetInt ("screen res");
		fullScreen = (PlayerPrefs.GetInt ("fullscreen") == 1) ? true : false;
		AudioListener.volume = masterSlider.value = PlayerPrefs.GetFloat ("master volume");
		musicSource.volume = musicSlider.value = PlayerPrefs.GetFloat ("music volume");
		fxSource.volume = fxSlider.value = PlayerPrefs.GetFloat ("effects volume");
	}

	public void BackButton () {
		mainMenuCanvas.SetActive (true);
		settingsCanvas.SetActive (false);
		selectLevelCanvas.SetActive (false);
	}

	// Play sound on click
	public void ClickSound () {
		fxSource.Play ();
	}

	// Quit game on press (doesn't function in unity editor)
	public void ClickExit () {
		Application.Quit ();
		PlayerPrefs.Save();
	}
}
