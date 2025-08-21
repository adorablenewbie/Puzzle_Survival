using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class NPCLoadScene : MonoBehaviour
{
    private string[] sceneNames = {
        "MainScene",
        "PuzzleScene",
        "KillzoneScene"
    };
    private bool isSceneName = false;

    private void Start()
    {
        PlayerManager.Instance.Player.dialogueManager.npcLoadScene = this;
    }

    public void LoadScene(string sceneName)
    {
        if (SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            return;
        }
        foreach (string name in sceneNames)
        {
            if (name == sceneName)
            {
                isSceneName = true;
                break;
            }
        }
        if (isSceneName)
        {
            Debug.Log($"Loading scene: {sceneName}");
            isSceneName = false;
            SceneManager.LoadScene(sceneName);
        }

    }
}
