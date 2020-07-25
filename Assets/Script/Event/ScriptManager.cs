using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptManager : MonoBehaviour
{
    public GameObject m_panel;
    public GameObject m_scriptWindow;
    public Text[] m_texts; // 0: 발화자 1: 내용 2: 오브젝트 내용

    public GameObject m_selection;
    public Text[] m_selectionTexts; // 선택지 갯수 2개?
    public Image[] m_selectionImages;

    public GameObject m_object;
    public Image m_objectImage;


    private AudioManager mgrAudio;
    

    public List<string> listSpeakers = new List<string>();
    public List<string> listSentences = new List<string>();

    private int count = 0;

    private string m_scriptName = "";
    private int m_scriptNum = 0;
    private int m_selectStart = 0;

    private bool m_isFinished;
    private bool m_isSelect;
    private bool m_isSkip;
    private bool m_pointer;


    public bool GetLayerState()
    {
        return m_panel.activeSelf;
    }

    public void ScriptLayerOn()
    {
        m_panel.SetActive(true);
        m_scriptWindow.SetActive(true);
    }
    public void ObjectLayerOn()
    {
        m_panel.SetActive(true);
        m_object.SetActive(true);
    }
    public void ScriptLayerOff()
    {
        m_panel.SetActive(false);
        m_scriptWindow.SetActive(false);
        m_object.SetActive(false);
    }

    private void ActiveSelection()
    {
        m_selection.SetActive(true);
        m_selectionImages[0].gameObject.SetActive(true);
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

    void PlayKeyboard(int c)
    {
        switch (c % 5)
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
    }


    // Selection
    private void SelectionPointer() // 선택지 여러 개? 될 수도 있어서 코드 수정하기 (추후)
    {
        int pointer = (m_pointer == true) ? 1 : 0; // 더 깔쌈한 처리 방식 없을까 ...
        int nPointer = (m_pointer == true) ? 0 : 1;

        m_selectionImages[pointer].gameObject.SetActive(true);
        m_selectionImages[nPointer].gameObject.SetActive(false);
    }

    private void SelectionScript() // 포켓몬처럼 위아래 슬라이드 해서 스페이스바로 선택 어떤지
    {
        listSentences.Clear();
        listSpeakers.Clear();
        InActiveSelection();
        ResetText();

        bool check = false;
        bool isObj = false;
        int order = (m_pointer == false) ? 1 : 2; // 우선은 두 개니까

        List<LoadJson.Script> scripts = LoadJson.scriptDic[m_scriptName];
        for (int i = m_selectStart; i < scripts[m_scriptNum].InnerScripts.Count; i++)
        {
            if (scripts[m_scriptNum].InnerScripts[i].number / 10 == order)
            {
                if (!check) // 첫 문장 넘어가기
                {
                    check = true;
                    if (scripts[m_scriptNum].InnerScripts[i].number % 10 == 0)
                        scripts[m_scriptNum].InnerScripts[0].finished = false;
                    else if (scripts[m_scriptNum].InnerScripts[i].number % 10 == 3) // object
                        isObj = true;
                }
                else
                {
                    listSentences.Add(scripts[m_scriptNum].InnerScripts[i].script);
                    listSpeakers.Add(scripts[m_scriptNum].InnerScripts[i].name);
                }
            }
        }


        m_isSelect = false; // 선택지 연속인 경우 대비해서 추후 수정하기
        count = 0;
        StopAllCoroutines();

        if (isObj)
        {
            m_objectImage.sprite = Resources.Load<Sprite>("Object/" + listSpeakers[0]) as Sprite;
            ObjectLayerOn();
            StartCoroutine(ObjectCoroutine());
        }
        else
        {
            StartCoroutine(ScriptCoroutine());
        }
    }


    // Object
    public void ShowObject(string script, int num)
    {
        List<LoadJson.Script> scripts = LoadJson.scriptDic[script];
        if (scripts[num].InnerScripts[0].finished) // 이미 완료했다면
        {
            Debug.Log("이미 완료한 이벤트");
            return;
        }

        scripts[num].InnerScripts[0].finished = true;
        listSpeakers.Add(scripts[num].InnerScripts[0].name);
        listSentences.Add(scripts[num].InnerScripts[0].script);

        m_objectImage.sprite = Resources.Load<Sprite>("Object/" + listSpeakers[0]) as Sprite; // 이미지 설정.
        ObjectLayerOn();
        StartCoroutine(ObjectCoroutine());
    }


    IEnumerator ObjectCoroutine()
    {
        yield return new WaitForSeconds(1f);
        Destroy(GameObject.Find(listSpeakers[0])); // 여기서 오브젝트 사라지면 될 듯

        m_scriptWindow.SetActive(true);
        m_isFinished = false;
        for (int i = 0; i < listSentences[count].Length; i++)
        {
            if (m_isSkip) // 요건 짧으니까 스킵이 안 되게끔 할까?
            {
                m_texts[2].text = listSentences[count];
                yield return new WaitForSeconds(0.4f);
                m_isSkip = false;
                break;
            }

            m_texts[2].text += listSentences[count][i];
            PlayKeyboard(i);
            yield return new WaitForSeconds(0.035f);
        }
        m_isFinished = true;
        yield break;
    }


    // Script
    public void ShowScript(string script, int num)
    {
        List<LoadJson.Script> scripts = LoadJson.scriptDic[script];
        if (scripts[num].InnerScripts[0].finished) // 이미 완료했다면
        {
            Debug.Log("이미 완료한 이벤트");
            return;
        }

        scripts[num].InnerScripts[0].finished = true;
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
            if (m_isSkip)
            {
                m_texts[1].text = listSentences[count];
                yield return new WaitForSeconds(0.4f); // 스킵 너무 호다닥 돼서 텀 추가. 시간적 여유가 된다면 냥냥 발 커서같은 귀여운 거 추가
                m_isSkip = false;
                break;
            }

            m_texts[1].text += listSentences[count][i];
            PlayKeyboard(i);
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
                m_pointer = (m_pointer == true) ? false : true;
                SelectionPointer();
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                SelectionScript();
            }
        }
        else if (Input.GetMouseButtonDown(0) && m_scriptWindow.activeSelf)
        {
            if (!m_isFinished)
            {
                m_isSkip = true;
            }
            else
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
}