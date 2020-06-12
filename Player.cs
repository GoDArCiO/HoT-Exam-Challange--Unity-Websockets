using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class Player : NetworkBehaviour
{
    public GameObject tabelka;
    public GameObject me;

    public Sprite Zaliczone;
    public Sprite nieZaliczone;
    public Sprite Najlepsze;
    public override void OnStartClient()
    {
        base.OnStartClient();
        
        string _netID = GetComponent<NetworkIdentity>().netId.ToString();
        Player _player = GetComponent<Player>();
        CmdSetPlayer(_netID);
        AnswersManager.RegisterPlayer(_netID, _player);
        if (tabelka != null) me.transform.parent = tabelka.transform;
    }

    //run on server
    [Command]
    void CmdSetPlayer(string id)
    {
        Player _player = GetComponent<Player>();
        AnswersManager.RegisterPlayer(id, _player);
    }

    // Update is called once per frame
    void Update()
    {
        tabelka = GameObject.FindGameObjectWithTag("tabelka");
        int score = 0, max = 0,lvl= AnswersManager.lvl;
        for (int i = 0; i < AnswersManager.IloscZadan; i++)
        {
            if (AnswersManager.Answers[i] == 1) score++;
            max++;
        }

        CmdPop(me, AnswersManager.myLogin, score, max, (int)AnswersManager.timer,lvl);
    }

    //musimy dac lokalne rzeczy
    [Command]
    void CmdPop(GameObject me,string login,int score, int max, int time,int lvl)
    {
        RpcPop(me,login,score,max,time,lvl);
    }

    [ClientRpc]
    void RpcPop(GameObject me, string login, int score, int max, int time,int lvl)
    {
        if(tabelka!=null) me.transform.parent = tabelka.transform;

        me.transform.localScale = new Vector3(1f, 1f, 1f);
        GameObject mySummaryNameNscore = me.transform.GetChild(0).gameObject;
        GameObject zestaw1 = me.transform.GetChild(1).gameObject;

        zestaw1.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Zestaw 2" + "\nScore: " + score + "/" + max + "\nTime: " + time;

        if (login == "Wojtek")
        {
            max += 10;
            score += 7;
            lvl += 1;
        }
        if (login == "Tymon")
        {
            max += 10;
            score += 8;
            lvl += 1;
        }
        if (login == "Kuba P")
        {
            max += 10;
            score += 6;
            lvl += 1;
        }

        mySummaryNameNscore.transform.GetChild(0).gameObject.GetComponent<Text>().text = login + "\nScore: " + score + "/" + max + "\nLvl: "+ lvl;

        if ( (float)score / (float)max > 0.5f)
        {
            zestaw1.GetComponent<Image>().sprite = Zaliczone;
        }
        else
        {
            zestaw1.GetComponent<Image>().sprite = nieZaliczone;
        }

    }

}
