using DG.Tweening;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{

    public RectTransform bgImageRecttransform;
    public Vector2 imageEndPos;
    public float duration;
    private SceneFlowManager sceneChange;



    private void Start()
    {
        MoveImage(imageEndPos, duration);
    }


    public void MoveImage(Vector2 endPos, float duration)
    {
        bgImageRecttransform.DOAnchorPos(endPos, duration);
    }
   
    public void MainSceneButton()
    {
        if (sceneChange == null)
        {
            sceneChange = SceneFlowManager.Instance;
        }
        sceneChange.SceneChange(SceneFlowManager.SceneType.MainScene);
    }

    public void ApplicatorQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
Application.Quit();

#endif
    }
}
