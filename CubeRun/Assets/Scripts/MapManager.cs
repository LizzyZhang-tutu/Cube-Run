using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MAP MANAGER.
/// </summary>
public class MapManager : MonoBehaviour {
    //Prefabs.
    private GameObject m_prefab_tile;
    private GameObject m_prefab_wall;
    private GameObject m_prefab_spikes;
    private GameObject m_prefab_smash_spikes;
    private GameObject m_prefab_gem;

    public List<GameObject[]> maplist = new List<GameObject[]>();

    private Transform m_Transform;
    private CubeController m_CubeController;

    public float tile_length = Mathf.Sqrt(2) * 0.254f;
    private int index = 0;

    private Color color_wall = new Color(71 / 255f, 57 / 255f, 105 / 255f);
    private Color colorOne = new Color(124 / 255f, 155 / 255f, 230 / 255f);
    private Color colorTwo = new Color(125 / 255f, 169 / 255f, 233 / 255f);

    //Probabilities.
    private int pr_hole = 0;
    private int pr_spikes = 0;
    private int pr_smash_spikes = 0;
    private int pr_gem = 2;


    void Start () {
        m_prefab_tile = Resources.Load("tile_white") as GameObject;
        m_prefab_wall = Resources.Load("wall2") as GameObject;
        m_prefab_spikes = Resources.Load("moving_spikes") as GameObject;
        m_prefab_smash_spikes = Resources.Load("smashing_spikes") as GameObject;
        m_prefab_gem = Resources.Load("gem 2") as GameObject;

        m_Transform = gameObject.GetComponent<Transform>();
        m_CubeController = GameObject.Find("cube_books").GetComponent<CubeController>();
        CreateGround(0);
    }

    /// <summary>
    /// Create A piece of Ground.
    /// </summary>
    public void CreateGround(float offsetZ)
    {
        for (int i = 0; i < 10; i++)
        {
            //odd.
            GameObject[] item_odd = new GameObject[6];
            for (int j = 0; j < 6; j++)
            {
                Vector3 pos = new Vector3(j * tile_length, 0, offsetZ + i * tile_length);
                Vector3 rot = new Vector3(-90, 45, 0);
                GameObject tile = null;
                if (j == 0 || j == 5)
                {
                    tile = GameObject.Instantiate(m_prefab_wall, pos, Quaternion.Euler(rot));
                    tile.GetComponent<MeshRenderer>().material.color = color_wall;
                }
                else
                {
                    int pr = CalcPr();
                    if (pr == 0) //tile.
                    {
                        tile = GameObject.Instantiate(m_prefab_tile, pos, Quaternion.Euler(rot));
                        tile.GetComponent<Transform>().Find("normal_a2").GetComponent<MeshRenderer>().material.color = colorTwo;
                        tile.GetComponent<MeshRenderer>().material.color = colorTwo;
                    }else if(pr == 1) //hole.
                    {
                        tile = new GameObject();
                        tile.GetComponent<Transform>().position = pos;
                        tile.GetComponent<Transform>().rotation = Quaternion.Euler(rot);
                    }else if(pr == 2) //spikes.
                    {
                        tile = GameObject.Instantiate(m_prefab_spikes, pos, Quaternion.Euler(rot));
                    }else if(pr == 3) // sky_spikes.
                    {
                        tile = GameObject.Instantiate(m_prefab_smash_spikes, pos, Quaternion.Euler(rot));
                    }
                }
                tile.GetComponent<Transform>().SetParent(m_Transform);
                item_odd[j] = tile;
            }
            maplist.Add(item_odd);

            //even.
            GameObject[] item_even = new GameObject[5];
            for (int j = 0; j < 5; j++)
            {
                Vector3 pos = new Vector3(j * tile_length + tile_length / 2, 0, offsetZ + i * tile_length + tile_length / 2);
                Vector3 rot = new Vector3(-90, 45, 0);
                GameObject tile = null;

                int pr = CalcPr();
                if (pr == 0) //tile.
                {
                    tile = GameObject.Instantiate(m_prefab_tile, pos, Quaternion.Euler(rot));
                    tile.GetComponent<Transform>().Find("normal_a2").GetComponent<MeshRenderer>().material.color = colorOne;
                    tile.GetComponent<MeshRenderer>().material.color = colorOne;
                    //see if gem is created.
                    int gemPr = CalcPr();
                    if(gemPr == 1)
                    {
                        //instantiate a gem.
                        GameObject gem = GameObject.Instantiate(m_prefab_gem, tile.GetComponent<Transform>().position +
                            new Vector3(0, 0.06f, 0), Quaternion.identity);
                        gem.GetComponent<Transform>().SetParent(tile.GetComponent<Transform>());
                    }
                }
                else if (pr == 1) //hole.
                {
                    tile = new GameObject();
                    tile.GetComponent<Transform>().position = pos;
                    tile.GetComponent<Transform>().rotation = Quaternion.Euler(rot);
                }
                else if (pr == 2) //spikes.
                {
                    tile = GameObject.Instantiate(m_prefab_spikes, pos, Quaternion.Euler(rot));
                }
                else if (pr == 3) // sky_spikes.
                {
                    tile = GameObject.Instantiate(m_prefab_smash_spikes, pos, Quaternion.Euler(rot));
                }
                tile.GetComponent<Transform>().SetParent(m_Transform);
                item_even[j] = tile;
            }
            maplist.Add(item_even);
        }
    }
    /// <summary>
    /// get tile's position(for debug.)
    /// </summary>
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < maplist.Count; i++)
            {
                for (int j = 0; j < maplist[i].Length; j++)
                {
                    maplist[i][j].name = i + "-" + j;
                }
            }
        }
    }

    /// <summary>
    /// start TileDown.
    /// </summary>
    public void StartTileDown()
    {
        StartCoroutine("TileDown");
    }

    /// <summary>
    /// Stop TileDown.
    /// </summary>
    public void StopTileDown()
    {
        StopCoroutine("TileDown");
    }

    /// <summary>
    /// Tiles go down.
    /// </summary>
    private IEnumerator TileDown()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            for (int i = 0; i < maplist[index].Length; i++)
            {
                Rigidbody rb = maplist[index][i].AddComponent<Rigidbody>();
                rb.angularVelocity = new Vector3(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f))
                    * Random.Range(1, 20);
                GameObject.Destroy(maplist[index][i], 1.0f);
            }
            if (m_CubeController.z == index)
            {
                StopTileDown();
                Rigidbody m_rb = m_CubeController.gameObject.AddComponent<Rigidbody>();
                m_rb.angularVelocity = new Vector3(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)) * Random.Range(1, 20);
                m_CubeController.StartCoroutine("GameOver", true);
            }
            index++;
        }
    }
    
    /// <summary>
    /// Calculate Probability.
    /// 0:tile.
    /// 1:hole.
    /// 2:spikes.
    /// 3:sky snare.
    /// </summary>
    private int CalcPr()
    {
        int pr = Random.Range(1, 100);
        if(pr <= pr_hole)
        {
            return 1;
        }else if(31 < pr && pr < pr_spikes + 30)
        {
            return 2;
        }else if(61 < pr && pr < pr_smash_spikes + 60 )
        {
            return 3;
        }
        return 0;
    }

    /// <summary>
    /// Calculate Probability for gems.
    /// </summary>
    private int CalcPr_gem()
    {
        int pr = Random.Range(1, 100);
        if (pr <= pr_gem)
        {
            return 1;
        }
        return 0;
    }

    /// <summary>
    /// Add Probability.
    /// </summary>
    public void AddPr()
    {
        pr_hole += 2;
        pr_spikes += 1;
        pr_smash_spikes += 1;
    }

    public void ResetGameMap()
    {
        //Destroy remaining tiles.
        Transform[] sonTransform = m_Transform.GetComponentsInChildren<Transform>();
        for (int i = 1; i < sonTransform.Length; i++)
        {
            GameObject.Destroy(sonTransform[i].gameObject);
        }
        // Reset Probabilities;
        pr_hole = 0;
        pr_spikes = 0;
        pr_smash_spikes = 0;
        pr_gem = 2;
        //Reset falling tiles' index.
        index = 0;
        //Reset maplist.
        maplist.Clear();
        CreateGround(0);
    }
}
