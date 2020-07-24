using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptManager : MonoBehaviour
{
    public GameObject m_panel;
    public GameObject m_scriptWindow;
    public Text[] m_texts; // 0: 발화자 1: 내용

    public GameObject m_selection;
    public Text[] m_selectionTexts; // 선택지 갯수 2개?
    public Image[] m_selectionImages;

    private AudioManager mgrAudio;
    

    public List<string> listSpeakers = new List<string>();
    public List<string> listSentences = new List<string>();

    private int count = 0;

    private string m_scriptName = "";
    private int m_scriptNum = 0;
    private int m_selectStart = 0;

    private bool m_isFinished;
    private bool m_isSelect;
    private bool m_pointer;


    public void ClickTempDialougeButton()
    {
        ShowScript("cookie", 0);
    }


    public void ScriptLayerOn()
    {
        m_panel.SetActive(true);
        m_scriptWindow.SetActive(true);
    }
    public void ScriptLayerOff()
    {
        m_panel.SetActive(false);
        m_scriptWindow.SetActive(false);
    }

    private void ActiveSelection()
    {
        m_selection.SetActive(true);
        m_selectionImages[1].gameObject.SetActive(false);
    }
    private void InActiveSelection()
    {
        m_selection.SetActive(false);
    }

    void ResetText()
    {
        for (int i = 0; i < m_texts.Length; i++)
        {
            m_texts[i].text = "";
        }
        for (int j = 0; j < m_selectionTexts.Length; j++)
        {
            m_selectionTexts[j].text = "";
        }
    }


    private void SelectionPointer()
    {
        int pointer = (m_pointer == true) ? 1 : 0; // 더 깔쌈한 처리 방식 없을까 ...
        int nPointer = (m_pointer == true) ? 0 : 1;

        m_selectionImages[pointer].gameObject.SetActive(true);
        m_selectionImages[nPointer].gameObject.SetActive(false);
    }

    public void SelectionScript() // 포켓몬처럼 위아래 슬라이드 해서 스페이스바로 선택 어떤지
    {
        listSentences.Clear();
        listSpeakers.Clear();
        InActiveSelection();
        ResetText();

        bool isCorrect = false;
        bool check = false;
        int order = (m_pointer == false) ? 1 : 2; // 우선은 두 개니까

        List<LoadJson.Script> scripts = LoadJson.scriptDic[m_scriptName];
        for (int i = m_selectStart; i < scripts[m_scriptNum].InnerScripts.Count; i++)
        {
            if (scripts[m_scriptNum].InnerScripts[i].number / 10 == order)
            {
                if (!check) // 첫 문장 넘어가기
                {
                    check = true;
                    if (scripts[m_scriptNum].InnerScripts[i].number % 10 == 5)
                        isCorrect = true;
                }
                else
                {
                    listSentences.Add(scripts[m_scriptNum].InnerScripts[i].script);
                    listSpeakers.Add(scripts[m_scriptNum].InnerScripts[i].name);
                }
            }
        }

        if (isCorrect)
            scripts[m_scriptNum].InnerScripts[0].finished = true;


        m_isSelect = false;
        count = 0;
        StopAllCoroutines();
        StartCoroutine(ScriptCoroutine());
    }


    public void ShowScript(string script, int num)
    {
        List<LoadJson.Script> scripts = LoadJson.scriptDic[script];
        if (scripts[num].InnerScripts[0].finished) // 이미 완료했다면
        {
            Debug.Log("이미 완료한 이벤트");
            return;
        }

        m_scriptName = script;
        m_scriptNum = num;


        for (int i = 0; i < scripts[m_scriptNum].InnerScripts.Count; i++)
        {
            if (scripts[m_scriptNum].InnerScripts[i].number == -1)
            {
                listSentences.Add(scripts[m_scriptNum].InnerScripts[i].script);
                listSpeakers.Add(scripts[m_scriptNum].InnerScripts[i].name);
            }
            else if (scripts[m_scriptNum].InnerScripts[i].number == 0)
            {
                listSentences.Add(scripts[m_scriptNum].InnerScripts[i].script);
                listSpeakers.Add(scripts[m_scriptNum].InnerScripts[i].name);
                m_isSelect = true;
                m_selectStart = i + 1;
            }
            else
            {
                if (scripts[m_scriptNum].InnerScripts[i].number / 10 == 1 && m_selectionTexts[0].text == "")
                {
                    m_selectionTexts[0].text = scripts[m_scriptNum].InnerScripts[i].script;
                }
                else if (scripts[m_scriptNum].InnerScripts[i].number / 10 == 2 && m_selectionTexts[1].text == "")
                {
                    m_selectionTexts[1].text = scripts[m_scriptNum].InnerScripts[i].script;
                }
            }
        }

        ScriptLayerOn();
        StartCoroutine(ScriptCoroutine());
    }

    IEnumerator ScriptCoroutine()
    {
        m_isFinished = false;
        m_texts[0].text = listSpeakers[count];
        for (int i = 0; i < listSentences[count].Length; i++)
        {
            m_texts[1].text += listSentences[count][i];
            switch (i%5)
            {
                case 0:
                    mgrAudio.Play("keyboard1");
                    break;
                case 1:
                    mgrAudio.Play("keyboard2");
                    break;
                case 2:
                    mgrAudio.Play("keyboard3");
                    break;
                case 3:
                    mgrAudio.Play("keyboard4");
                    break;
                case 4:
                    mgrAudio.Play("keyboard5");
                    break;
                default:
                    break;
            }
            yield return new WaitForSeconds(0.035f);
        }
        m_isFinished = true;
        yield break;
    }

    private void Start()
    {
        mgrAudio = FindObjectOfType<AudioManager>();

        ScriptLayerOff();
        InActiveSelection();
    }


    private void ExitScripts()
    {
        m_isFinished = false;
        m_isSelect = false;
        m_pointer = false;

        count = 0;
        m_scriptNum = 0;
        m_selectStart = 0;
        m_scriptName = "";

        listSpeakers.Clear();
        listSentences.Clear();
        ResetText();
        ScriptLayerOff();
    }




    private void Update()
    {
        if (m_selection.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                Debug.Log(m_pointer);
                m_pointer = (m_pointer == true) ? false : true;
                SelectionPointer();
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                SelectionScript();
            }
        }
        else if (Input.GetMouseButtonDown(0) && m_isFinished && m_scriptWindow.activeSelf)
        {
            count++;

            if (count >= listSentences.Count)
            {
                StopAllCoroutines();
                if (m_isSelect)
                {
                    m_isFinished = false;
                    ActiveSelection();
                }
                else
                    ExitScripts();
            }
            else
            {
                m_texts[1].text = "";
                StopAllCoroutines();
                StartCoroutine(ScriptCoroutine());
            }
        }
    }
}