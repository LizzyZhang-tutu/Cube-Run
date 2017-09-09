using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    private GameObject m_StartUI;
    private GameObject m_GameUI;

    private UILabel m_Score_Label;
    private UILabel m_Gem_Label;
    private UILabel m_GameScore_Label;
    private UILabel m_GameGem_Label;
    private UILabel m_LatestRun_Label;

    private GameObject m_PlayButton;
    private GameObject m_Left;
    private GameObject m_Right;

    private CubeController m_CubeController;

    void Start () {
        m_StartUI = GameObject.Find("Start_UI");
        m_GameUI = GameObject.Find("Game_UI");

        m_Score_Label = GameObject.Find("Score_Label").GetComponent<UILabel>();
        m_Gem_Label = GameObject.Find("Gem_Label").GetComponent<UILabel>();

        m_GameScore_Label = GameObject.Find("GameScore_Label").GetComponent<UILabel>();
        m_GameGem_Label = GameObject.Find("GameGem_Label").GetComponent<UILabel>();

        m_LatestRun_Label = GameObject.Find("LatestRun_Label").GetComponent<UILabel>();

        m_CubeController = GameObject.Find("cube_books").GetComponent<CubeController>();

        m_PlayButton = GameObject.Find("play_btn");
        UIEventListener.Get(m_PlayButton).onClick = PlayButtonClick;

        m_Left = GameObject.Find("Left");
        UIEventListener.Get(m_Left).onClick = Left;
        m_Right = GameObject.Find("Right");
        UIEventListener.Get(m_Right).onClick = Right;
        Init();

        m_GameUI.SetActive(false);
    }

    private void Init()
    {
        m_Score_Label.text = PlayerPrefs.GetInt("dist", 0) + "";
        m_Gem_Label.text = PlayerPrefs.GetInt("gem", 0) + "/500";
        m_LatestRun_Label.text = "0";
        m_GameScore_Label.text = "0";
        m_GameGem_Label.text = PlayerPrefs.GetInt("gem", 0) + "/500";
    }

    public void UpdateData(int score, int gem)
    {
        m_Score_Label.text = PlayerPrefs.GetInt("dist", 0) + "";
        m_Gem_Label.text = gem + "/500";
        m_LatestRun_Label.text = score.ToString();
        m_GameScore_Label.text = score.ToString();
        m_GameGem_Label.text = gem + "/500";
    }

    private void PlayButtonClick(GameObject go)
    {
        m_StartUI.SetActive(false);
        m_GameUI.SetActive(true);
        m_CubeController.StartGame();
    }

    private void Left(GameObject go)
    {
        m_CubeController.Left();
    }

    private void Right(GameObject go)
    {
        m_CubeController.Right();
    }

    public void ResetUI()
    {
        m_StartUI.SetActive(true);
        m_GameUI.SetActive(false);
        m_GameScore_Label.text = "0";
    }
}

