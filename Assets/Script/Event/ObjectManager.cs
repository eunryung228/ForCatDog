using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    private ScriptManager mgrScript;

    private string m_scriptName = "";
    private string m_objectName = "";
    private List<LoadJson.Script> scripts;


    // Click 함수: 오브젝트 상호작용 함수
    private void ClickHappy()
    {
        m_scriptName = "happy";
        scripts = LoadJson.scriptDic[m_scriptName];

        if (!scripts[0].InnerScripts[0].finished)
            mgrScript.ShowScript(m_scriptName, 0);
        else if (scripts[1].InnerScripts[0].finished)
            mgrScript.ShowScript(m_scriptName, 2);
        else // 이벤트 발생 x
            return;
    }

    private void ClickCalendar()
    {
        m_scriptName = "happy";
        scripts = LoadJson.scriptDic[m_scriptName];

        if (scripts[0].InnerScripts[0].finished && !scripts[2].InnerScripts[0].finished)
            mgrScript.ShowObject(m_scriptName, 1);
        else // 이벤트 발생 x
            return;
    }

    private void ClickJjongI()
    {
        m_scriptName = "jjongi";
        scripts = LoadJson.scriptDic[m_scriptName];

        if (!scripts[0].InnerScripts[0].finished)
            mgrScript.ShowScript(m_scriptName, 0);
        else if (scripts[1].InnerScripts[0].finished)
            mgrScript.ShowScript(m_scriptName, 2);
        else // 이벤트 발생 x
            return;
    }

    private void ClickPond()
    {
        m_scriptName = "jjongi";
        scripts = LoadJson.scriptDic[m_scriptName];

        if (scripts[0].InnerScripts[0].finished && !scripts[2].InnerScripts[0].finished)
            mgrScript.ShowScript(m_scriptName, 1);
        else // 이벤트 발생 x
            return;
    }

    private void ClickNavi()
    {
        m_scriptName = "navi";
        scripts = LoadJson.scriptDic[m_scriptName];

        if (!scripts[0].InnerScripts[0].finished)
            mgrScript.ShowScript(m_scriptName, 0);
        else if (scripts[1].InnerScripts[0].finished)
            mgrScript.ShowScript(m_scriptName, 2);
        else
            return;
    }

    private void ClickThread()
    {
        m_scriptName = "navi";
        scripts = LoadJson.scriptDic[m_scriptName];

        if (scripts[0].InnerScripts[0].finished && !scripts[2].InnerScripts[0].finished)
            mgrScript.ShowObject(m_scriptName, 1);
        else
            return;
    }

    private void ClickWanso()
    {
        m_scriptName = "wanso";
        scripts = LoadJson.scriptDic[m_scriptName];

        if (!scripts[0].InnerScripts[0].finished)
            mgrScript.ShowScript(m_scriptName, 0);
        else
            return;
    }


    private void Start()
    {
        mgrScript = FindObjectOfType<ScriptManager>();
    }

    void Update() // temp. 실제로는 이 방식 아님.
    {
        if (!mgrScript.GetLayerState() && Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);

            if (hit.collider != null)
            {
                m_objectName = hit.collider.name;
                // Debug.Log(hit.collider.name); // 로그 확인

                if (m_objectName == "해피") // 해당 오브젝트 (여기서는 해피)랑 충돌되고 스페이스바 클릭하면 아래 함수 실행되게 하시면 됩니다.
                    ClickHappy();
                else if (m_objectName == "달력")
                    ClickCalendar();
                else if (m_objectName == "쫑이")
                    ClickJjongI();
                else if (m_objectName == "호수")
                    ClickPond();
                else if (m_objectName == "나비")
                    ClickNavi();
                else if (m_objectName == "실타래")
                    ClickThread();
                else if (m_objectName == "완소")
                    ClickWanso();
                else
                    m_objectName = "";
            }
        }
    }
}