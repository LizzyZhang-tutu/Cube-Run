using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour {

    private Transform m_Transform;
    private MapManager m_MapManager;
    private CameraFollow m_CameraFollow;
    private UIManager m_UIManager;

    public int z = 6;
    private int x = 2;
    private Color color_1 = new Color(146 / 255f, 106 / 255f, 248 / 255f);
    private Color color_2 = new Color(158 / 255f, 127 / 255f, 236 / 255f);

    public bool alive = false;
    private int gemCount = 0;
    public int distCount = 0;

    private void AddGemCount()
    {
        gemCount++;
        m_UIManager.UpdateData(distCount, gemCount);
    }

    private void AddDist()
    {
        distCount++;
        m_UIManager.UpdateData(distCount, gemCount);
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt("gem", gemCount);
        if(distCount > PlayerPrefs.GetInt("dist",0))
        {
            PlayerPrefs.SetInt("dist", distCount);
        }
    }

    void Start () {
        gemCount = PlayerPrefs.GetInt("gem", 0);

        m_Transform = gameObject.GetComponent<Transform>();
        m_MapManager = GameObject.Find("MapManager").GetComponent<MapManager>();
        m_CameraFollow = GameObject.Find("Main Camera").GetComponent<CameraFollow>();
        m_UIManager = GameObject.Find("UI Root").GetComponent<UIManager>();
	}
	
	void Update () {
        if(alive)
        {
            PlayerControl();
        }
    }

    public void StartGame()
    {
        alive = true;
        SetPlayerPos();
        m_CameraFollow.startFollow = true;
        m_MapManager.StartTileDown();
    }

    /// <summary>
    /// Set Player's Position.
    /// </summary>
    private void SetPlayerPos()
    {
        Transform playerPos = m_MapManager.maplist[z][x].GetComponent<Transform>();
        MeshRenderer child = null;
        m_Transform.position = playerPos.position + new Vector3(0, 0.254f / 2, 0);
        m_Transform.rotation = playerPos.rotation;
        if (playerPos.tag == "tile")
        {
            child = playerPos.Find("normal_a2").GetComponent<MeshRenderer>();
        }else if(playerPos.tag == "spikes")
        {
            child = playerPos.Find("moving_spikes_a2").GetComponent<MeshRenderer>();
        }else if(playerPos.tag == "smash_spikes")
        {
            child = playerPos.Find("smashing_spikes_a2").GetComponent<MeshRenderer>();
        }
        // set color changes when player goes by.
        if(child != null)
        {
            if (z % 2 == 0)
            {
                child.material.color = color_1;
            }
            else
            {
                child.material.color = color_2;
            }
        }
        else
        {
            gameObject.AddComponent<Rigidbody>().AddForce(Vector3.down * 1000);
            StartCoroutine("GameOver", true);
        }
    }

    /// <summary>
    /// Go Left.
    /// </summary>
    public void Left()
    {
        if (x != 0)
        {
            if (z % 2 == 0)
            {
                x--;
            }
            z++;
            AddDist();
        }
        SetPlayerPos();
    }

    /// <summary>
    /// Go Right.
    /// </summary>
    public void Right()
    {
        if (x != 4 || z % 2 == 0)
        {
            if (z % 2 == 1)
            {
                x++;
            }
            z++;
            AddDist();
        }
        SetPlayerPos();
    }

    /// <summary>
    /// Player Controller.
    /// </summary>
    private void PlayerControl()
    {
        if(alive)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                Left();
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                Right();
            }
            CalcPosition();
        }
    }

    /// <summary>
    /// Calculate the player's position.
    /// </summary>
    private void CalcPosition()
    {
        if(m_MapManager.maplist.Count - z <= 12)
        {
            //ADD probability.
            m_MapManager.AddPr();
            //create new map
            float offsetZ = m_MapManager.maplist[m_MapManager.maplist.Count - 1][0].
                GetComponent<Transform>().position.z + m_MapManager.tile_length / 2;
            m_MapManager.CreateGround(offsetZ);
        }
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "spikes_attack")
        {
            StartCoroutine("GameOver", false);
        }
        if(coll.tag == "gem")
        {
            GameObject.Destroy(coll.gameObject.GetComponent<Transform>().parent.gameObject);
            AddGemCount();
        }
    }

    /// <summary>
    /// Game Over.
    /// </summary>
    public IEnumerator GameOver(bool b)
    {
        if(b)
        {
            yield return new WaitForSeconds(0.05f);
        }
        if(alive)
        {
            Debug.Log("game over");
            alive = false;
            m_CameraFollow.startFollow = false;
            SaveData();
            StartCoroutine("ResetGame");
        }
    }

    private void ResetPlayer()
    {
        //remove player's rigidbody.
        GameObject.Destroy(gameObject.GetComponent<Rigidbody>());
        //reset player's starting position.
        z = 6;
        x = 2;
        //player alive;
        alive = true;
        //distance score back to zero.
        distCount = 0;
}

    private IEnumerator ResetGame()
    {
        yield return new WaitForSeconds(2);
        ResetPlayer();
        m_MapManager.ResetGameMap();
        m_UIManager.ResetUI();
        m_CameraFollow.ResetCamera();
    }
}


