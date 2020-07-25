using UnityEngine;

public class ExitGame : MonoBehaviour
{
    public void Quit()
    {
        if (!FindObjectOfType<ScriptManager>().GetLayerState())
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
    }
}