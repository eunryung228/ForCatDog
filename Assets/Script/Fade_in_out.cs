using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class Fade_in_out : MonoBehaviour
{
    public Image Panel;
    public Image TitleImage;
    public Image Mainlogo;
    float time = 0;
    float F_time = 1f;

    public void Fade()
    {
        StartCoroutine(FadeFlow());
    }
    IEnumerator FadeFlow()
    {
        Panel.gameObject.SetActive(true);
        time = 0f;
        Color alpha = Panel.color;

        while (alpha.a < 1f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(0, 1, time);
            Panel.color = alpha;
            yield return null;
        }

        time = 0f;
        yield return new WaitForSeconds(1f);
        Mainlogo.gameObject.SetActive(false);

        while (alpha.a > 0f)
        {       
            TitleImage.gameObject.SetActive(true);
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(1, 0, time);
            Panel.color = alpha;
            yield return null;
        }
        Panel.gameObject.SetActive(false);
        yield return null;
    }
}
