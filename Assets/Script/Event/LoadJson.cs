using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

[System.Serializable]
public class LoadJson : MonoBehaviour
{
    void Start()
    {
        LoadScript();
    }


    private string[] scriptName = { "happy","jjongi" };
    private string[] scriptIdx = { "name", "script", "number" };
    private string[] scriptOrder = { "first", "second", "third", "fourth", "fifth", "sixth" };
    private int[] scriptOrderIdx = { 3, 3 };

    public static Dictionary<string, List<Script>> scriptDic = new Dictionary<string, List<Script>>();

    public void LoadScript()
    {
        for (int k = 0; k < scriptName.Length; k++)
        {
            TextAsset textasset = Resources.Load("Data/ScriptJson/" + scriptName[k]) as TextAsset;
            string loadstring = textasset.text;
            JObject loadScript = JObject.Parse(loadstring);

            List<Script> tmp_script = new List<Script>();

            for (int j = 0; j < scriptOrderIdx[k]; j++)
            {
                JArray loadArr = (JArray)loadScript[scriptOrder[j]];
                List<Script.innerScript> tmp_innerScript = new List<Script.innerScript>();
                for (int i = 0; i < loadArr.Count; i++)
                {
                    string n, s;
                    int c;

                    n = loadArr[i][scriptIdx[0]].ToString();
                    s = loadArr[i][scriptIdx[1]].ToString();
                    c = int.Parse(loadArr[i][scriptIdx[2]].ToString());
                    tmp_innerScript.Add(new Script.innerScript(n, s, c));
                }
                tmp_script.Add(new Script(scriptOrder[j], tmp_innerScript));
            }
            scriptDic.Add(scriptName[k], tmp_script);
        }
    }


    [SerializeField]
    public class Script
    {
        public string order;
        public List<innerScript> InnerScripts = new List<innerScript>();

        public Script(string order, List<innerScript> innerScripts)
        {
            this.order = order;
            InnerScripts = innerScripts;

        }
        [Serializable]
        public class innerScript
        {
            public string name;
            public string script;
            public int number;
            public bool finished;

            public innerScript(string name, string script, int num)
            {
                this.name = name;
                this.script = script;
                number = num;
                finished = false;
            }
        }
    }
}