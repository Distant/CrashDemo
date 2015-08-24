using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;

public enum PickupType {
    GEM_RED, GEM_BLUE
}

public class GameManager : MonoBehaviour
{
    private bool paused;
    private GameObject pauseMenu;
    private int loadedLevel = 0;
    private bool postInit;
    EventSystem events;

    private Dictionary<PickupType, bool> permanentItems;

	private int levelsUnlocked = 1;
	public int LevelsUnlocked { get { return levelsUnlocked; }}
	private List<int> levelsCompleted;

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(this);

		levelsCompleted = new List<int> ();

        permanentItems = new Dictionary<PickupType, bool>();
        permanentItems.Add(PickupType.GEM_RED, false);
        permanentItems.Add(PickupType.GEM_BLUE, false);

        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
        pauseMenu.GetComponent<Canvas>().enabled = false;
        DontDestroyOnLoad(pauseMenu);
        events = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        DontDestroyOnLoad(events.gameObject);
        LoadLevel(1);
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetButtonDown("Pause") || Input.GetButtonDown("Cancel")) && loadedLevel > 1)
        {
            if (paused)
            {
                VerifyPause();
                Time.timeScale = 1;
                paused = false;
                events.sendNavigationEvents = false;
                pauseMenu.GetComponent<Canvas>().enabled = false;
            }

            else
            {
                VerifyPause();
                Time.timeScale = 0;
				//events.SetSelectedGameObject(events.firstSelectedGameObject);
				events.sendNavigationEvents = true;
                pauseMenu.GetComponent<Canvas>().enabled = true;
				paused = true;
            }
        }
    }

	public void Quit(){
		Application.Quit ();
	}

    private void VerifyPause()
    {
        if (pauseMenu == null)
        {
            pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
        }
    }

    public void LoadLevel(int index)
    {
        if (loadedLevel != index)
        {
            Application.LoadLevel(index);
            loadedLevel = index;
            VerifyPause();
            Time.timeScale = 1;
            paused = false;
            events.sendNavigationEvents = false;
            pauseMenu.GetComponent<Canvas>().enabled = false;
        }
    }

	// for the level select button
	public void LeaveLevel(){
		GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>().CleanUp();
		LoadLevel (1);
	}
	
	public void AddItem(PickupType type)
    {
        Debug.Assert(permanentItems.ContainsKey(type));
        permanentItems[type] = true;
    }

    public bool CheckItem(PickupType type)
    {
        Debug.Assert(permanentItems.ContainsKey(type));
        return permanentItems[type];
    }

	public void EndLevel(int levelIndex){
		if (!levelsCompleted.Contains (levelIndex)) {
			levelsUnlocked ++;
			levelsCompleted.Add(levelIndex);
			print (levelIndex + "done");
		}
		LoadLevel (1);
	}
}