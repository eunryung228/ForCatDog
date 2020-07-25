using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveScene : MonoBehaviour
{
    public void Click_Move_Main_Scene() // 은령: 수정을 조금 했습니다!
    {
        SceneManager.LoadScene(1);
        SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);
    }
}
