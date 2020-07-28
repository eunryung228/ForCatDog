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
    public void ClickHappy()
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

    public void ClickCalendar()
    {
        m_scriptName = "happy";
        scripts = LoadJson.scriptDic[m_scriptName];

        if (scripts[0].InnerScripts[0].finished && !scripts[2].InnerScripts[0].finished)
            mgrScript.ShowObject(m_scriptName, 1);
        else // 이벤트 발생 x
            return;
    }

    public void ClickJjongI()
    {
        if (mgrScript.GetLast() == "happy")
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
    }

    public void ClickPond()
    {
        m_scriptName = "jjongi";
        scripts = LoadJson.scriptDic[m_scriptName];

        if (scripts[0].InnerScripts[0].finished && !scripts[2].InnerScripts[0].finished)
            mgrScript.ShowScript(m_scriptName, 1);
        else // 이벤트 발생 x
            return;
    }

    public void ClickNavi()
    {
        if (mgrScript.GetLast() == "jjongi")
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
    }

    public void ClickThread()
    {
        m_scriptName = "navi";
        scripts = LoadJson.scriptDic[m_scriptName];

        if (scripts[0].InnerScripts[0].finished && !scripts[2].InnerScripts[0].finished)
            mgrScript.ShowObject(m_scriptName, 1);
        else
            return;
    }

    public void ClickWanso()
    {

        if (mgrScript.GetLast() == "navi")
        {
            m_scriptName = "wanso";
            scripts = LoadJson.scriptDic[m_scriptName];

            if (!scripts[0].InnerScripts[0].finished)
                mgrScript.ShowScript(m_scriptName, 0);
            else
                return;
        }
    }

    public void ClickNero()
    {
        if (mgrScript.GetLast() == "wanso")
        {
            m_scriptName = "nero";
            scripts = LoadJson.scriptDic[m_scriptName];

            if (!scripts[0].InnerScripts[0].finished)
                mgrScript.ShowScript(m_scriptName, 0);
            else if (scripts[1].InnerScripts[0].finished)
                mgrScript.ShowScript(m_scriptName, 2);
            else
                return;
        }
    }

    public void ClickFish()
    {
        m_scriptName = "nero";
        scripts = LoadJson.scriptDic[m_scriptName];

        if (scripts[0].InnerScripts[0].finished && !scripts[2].InnerScripts[0].finished)
            mgrScript.ShowObject(m_scriptName, 1);
        else
            return;
    }


    private void Start()
    {
        mgrScript = FindObjectOfType<ScriptManager>();
    }
}