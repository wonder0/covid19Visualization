using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class DataScraper : MonoBehaviour
{


    public List<DataBase> stateList = new List<DataBase>();

    public string data = "";
    public string lastUpdated = "";


    private string startString = "content clearfix";
    private string endString = "</div></div></div></div></div><a id";

    private void Start()
    {
        StartCoroutine(GetRequest("https://www.mygov.in/corona-data/covid19-statewise-status"));
        
    }

    private void ReadData()
    {
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                //Debug.Log(webRequest.downloadHandler.text);
                
                data = webRequest.downloadHandler.text;
                LoadData(data);

            }
        }
    }

    int lastUpdatedIndex;
    int totalConfirmedIndex;
    int totalCuredIndex;
    int DeceasedIndex;
    private void LoadData(string data)
    {
        //Debug.Log(data.IndexOf(startString));
        //Debug.Log(data.IndexOf(endString));

        
        data = data.Substring(data.IndexOf(startString), data.IndexOf(endString) - data.IndexOf(startString));
        string[] lines = data.Split(new string[] { "State Name" }, StringSplitOptions.None);

        Debug.Log(lines[0]);

        lastUpdatedIndex = lines[0].IndexOf("Covid India As On");
        for(int j = lastUpdatedIndex + 84; lines[0][j] != '<'; j++)
        {
            if(lines[0][j] != '>')
            {
                lastUpdated += lines[0][j];

            }
        }
        Debug.Log(lastUpdated);




        for(int i = 1; i < lines.Length; i++)
        {
            //if(i == 26)
            DataBase tempState = new DataBase();
            string tempStateName = "";
            string tempTotalCases = "";
            string tempCuredCases = "";
            string tempDeath = "";


            //Debug.Log(lines[i]);
            //Debug.Log(lines[i][67]);
            for (int j = 67; lines[i][j] != '<'; j++)
            {
                tempStateName += lines[i][j];
            }
            //Debug.Log(tempStateName);
            totalConfirmedIndex = lines[i].IndexOf("Total Confirmed");
            //Debug.Log(lines[i][totalConfirmedIndex + 82]);

            for (int j = totalConfirmedIndex + 82; lines[i][j] != '<'; j++)
            {
                tempTotalCases += lines[i][j];
            }
            //Debug.Log(tempTotalCases);

            tempState.totalCases = int.Parse(tempTotalCases);

            totalCuredIndex = lines[i].IndexOf("Cured/Discharged/Migrated");
            //Debug.Log(lines[i][totalCuredIndex + 92]);

            for (int j = totalCuredIndex + 92; lines[i][j] != '<'; j++)
            {
                tempCuredCases += lines[i][j];
            }
            //Debug.Log(tempCuredCases);
            tempState.cured = int.Parse(tempCuredCases);



            DeceasedIndex = lines[i].IndexOf("Death");
            //Debug.Log(lines[i][DeceasedIndex + 72]);


            for (int j = DeceasedIndex + 72; lines[i][j] != '<'; j++)
            {
                tempDeath += lines[i][j];
            }
            //Debug.Log(tempDeath);
            tempState.death = int.Parse(tempDeath);

            stateList.Add(tempState);

        }
    }

    public void MenuScene()
    {
        SceneManager.LoadScene("MenuScene");
    }

}


/////////////////////////*
//if(webRequest.downloadHandler.text.Contains("Name of State / UT"))
//                {
//                    Debug.Log("Yess");
//                }
//                string[] lines = webRequest.downloadHandler.text.Split('\n');
//                foreach(var line in lines)
//                {
//                    if(line.Contains("Name of State / UT"))
//                    {
//                        data = line;
//                        break;
//                    }
//               }



//string[] lines = data.Split(']');
//        foreach(var line in lines)
//        {
            
//            if (line.Contains(",["))
//            {
//                string myStateData = line.Remove(0, 2);
//string[] subLines = myStateData.Split(',');
//DataBase db = new DataBase();
//db.name = subLines[0].Remove(subLines[0].Length - 1, 1).Remove(0,1);
//db.totalCases = int.Parse(subLines[1]);
//db.death = int.Parse(subLines[2]);
//db.cured = int.Parse(subLines[3]);
//stateList.Add(db);
//            }
//        }
//        Debug.Log(stateList.Count);
    ////////////////







