using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Mirror;

public class AnswersManager : MonoBehaviour
{
    public GameObject tittle;
    public static int[] Answers = new int[50];//0zle 1dobrze
    public int[] AnswersCorrect = new int[50];//1-a 2-b 3-c 4-d
    int[] AnswersGiven = new int[50];
    public int nrZadania;
    public static int IloscZadan;
    public GameObject[] Zadania;
    public GameObject[] przyciski;
    public GameObject[] przyciskiCorrect;
    public GameObject[] przyciskiWrong;

    public GameObject PanelZadan;
    public GameObject PanelPrzyciskow;

    public GameObject PanelLogowania;
    public static string myLogin;
    public InputField LoginInput;
    public InputField PassInput;
    public string[] Passwords;
    public string[] Logins;

    public GameObject SummaryPanel;
    public Text mySummaryNameNscore;
    public Text[] textOdpowiedzi;
    public string[] trescOdpowiedzi;
    public int odliczajOdp=4;

    public Slider exp;
    public Slider score;
    public Slider progress;

    public static float timer;
    public float expA=0;
    public float scoreA=0;
    public float progressA=0;
    public float expF;
    public float scoreF;
    public float progressF;
    public static int lvl=1;

    public Text timerTxt;

    public GameObject[] info;
    public GameObject particExp;
    public GameObject particScore;
    public GameObject particProgress;

    public GameObject manager;
    public int moznazaczac = 0;
    public GameObject pressR;

    int check;
    // Start is called before the first frame update
    void Start()
    {
        PanelLogowania.SetActive(true);
        tittle.SetActive(true);
    }

    public void Zaloguj()
    {
        AudioManager.instance.Play("click");
        check = -1;
        
        for (int i=0;i<Logins.Length;i++)
        {
            if (string.Compare(LoginInput.text, Logins[i])==0) check = i;
        }
        if (check == -1)
        {
            LoginInput.text = "";
            PassInput.text = "";
        }
        else
        {
            if (string.Compare(PassInput.text, Passwords[check]) == 0)
            {
                myLogin = LoginInput.text;
                PanelLogowania.SetActive(false);
                tittle.SetActive(false);
                nrZadania = 0;
                IloscZadan = Zadania.Length;

                //client
                manager.GetComponent<NetworkManager>().networkAddress = "46.170.206.205";
                manager.GetComponent<NetworkManager>().StartClient();

                Lobby();

            for (int i = 0; i < info.Length - 1; i++)//odpal info
                {
                    info[i].SetActive(true);
                }


            }
            else
            {
                LoginInput.text = "";
                PassInput.text = "";
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(expA >= 100)
        {
            expA = 0;
            lvl++;
            exp.value = 0;
            AudioManager.instance.Play("lvl");
        }
        if(expA> exp.value)
        {
            expF += Time.deltaTime*15;
            exp.value = expF;
            particExp.SetActive(true);
        }
        else
        {
            particExp.SetActive(false);
        }

        if (progressA > progress.value)
        {
            progressF += Time.deltaTime * 15;
            progress.value = progressF;
            particProgress.SetActive(true);
        }
        else
        {
            particProgress.SetActive(false);
        }

        if (scoreA > score.value)
        {
            scoreF += Time.deltaTime * 15;
            score.value = scoreF;
            particScore.SetActive(true);
        }
        else
        {
            particScore.SetActive(false);
        }
        if (info[0].activeSelf&& PanelZadan.activeSelf)
        {
            timer += Time.deltaTime;
            timerTxt.text = "Time: " + (int)timer;
        }
        if (moznazaczac == 1)
        {
            if (Input.GetKey(KeyCode.R))
            {
                Rozpocznij();
            }
        }

    }

    public void Solve(int odp)
    {
        //wylacz wszystkie przyciski
        przyciski[0].SetActive(false);
        przyciski[1].SetActive(false);
        przyciski[2].SetActive(false);
        przyciski[3].SetActive(false);
        przyciski[AnswersGiven[nrZadania] - 1].SetActive(true);

        AnswersGiven[nrZadania] = odp;
        float halo = 100 / IloscZadan;
        if (AnswersGiven[nrZadania] == AnswersCorrect[nrZadania])//dobrze
        {
            Answers[nrZadania] = 1;
            przyciskiCorrect[AnswersGiven[nrZadania]-1].SetActive(true);
            AudioManager.instance.Play("correctSound");
            expA += 9;
            scoreA +=halo;
        }
        else//zle
        {
            Answers[nrZadania] = 0;
            przyciskiWrong[AnswersGiven[nrZadania]-1].SetActive(true);
            AudioManager.instance.Play("wrongSound");
            expA += 1;
        }
        progressA += halo;
        Invoke("NextQuestion",0.5f);
    }

    public void NextQuestion()
    {
        przyciskiWrong[AnswersGiven[nrZadania]-1].SetActive(false);
        przyciskiCorrect[AnswersGiven[nrZadania]-1].SetActive(false);
        nrZadania++;
        
        if (nrZadania == IloscZadan)
        {
            Zadania[nrZadania - 1].SetActive(false);
            Summary();
        }
        else
        {
            Zadania[nrZadania - 1].SetActive(false);
            Zadania[nrZadania].SetActive(true);
            int temp=0;
            while(odliczajOdp < (nrZadania * 4) + 4)
            {
                textOdpowiedzi[temp].text = trescOdpowiedzi[odliczajOdp];
                odliczajOdp++;temp++;
            }
        }
        //wlacz wszystkie przyciski
        przyciski[0].SetActive(true);
        przyciski[1].SetActive(true);
        przyciski[2].SetActive(true);
        przyciski[3].SetActive(true);
    }

    public void Lobby()
    {
        SummaryPanel.SetActive(true);
        moznazaczac = 1;
        pressR.SetActive(true);
    }

    public void Rozpocznij()
    {
        pressR.SetActive(false);
        Zadania[nrZadania].SetActive(true);
        PanelPrzyciskow.SetActive(true);
        PanelZadan.SetActive(true);
        for (int i = 0; i < 4; i++)
        {
            textOdpowiedzi[i].text = trescOdpowiedzi[i];
        }
        SummaryPanel.SetActive(false);
    }

    private const string PLAYER_ID_PREFIX = "Player ";

    private static Dictionary<string, Player> players = new Dictionary<string, Player>();

    public static void RegisterPlayer(string _netID, Player _player)
    {
        if (players.ContainsKey(_netID))
        {
            return;
        }

        string _playerID = PLAYER_ID_PREFIX + _netID;
        players.Add(_playerID, _player);
        _player.transform.name = _playerID;
    }

    public static void UnRegisterPlayer(string _playerID)
    {
        players.Remove(_playerID);
    }

    public static Player GetPlayer(string _playerID)
    {
        Debug.LogError("get" + _playerID);

        return players[_playerID];
    }

    public static Player[] GetAllPlayers()
    {
        return players.Values.ToArray();
    }

    public void Summary()
    {
        expA += 30;
        PanelPrzyciskow.SetActive(false);
        PanelZadan.SetActive(false);
        SummaryPanel.SetActive(true);
    }
}
