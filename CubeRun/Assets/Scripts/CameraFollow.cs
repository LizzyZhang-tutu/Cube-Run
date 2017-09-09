using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraFollow : MonoBehaviour {

    private Transform m_Transform;
    private Transform player_Transform;
    public bool startFollow = false;
    private Vector3 start_Pos;


    void Start () {
        m_Transform = gameObject.GetComponent<Transform>();
        start_Pos = m_Transform.position;
        player_Transform = GameObject.Find("cube_books").GetComponent<Transform>();
	}
	

	void Update () {
        CameraMove();
	}

    /// <summary>
    /// let camera move with player.
    /// </summary>
    void CameraMove()
    {
        if (startFollow)
        {
            Vector3 nextPos = new Vector3(m_Transform.position.x, player_Transform.position.y + 1.50f, player_Transform.position.z);
            //m_Transform.position = nextPos;
            m_Transform.position = Vector3.Lerp(m_Transform.position, nextPos, Time.deltaTime);
        }
    }

    /// <summary>
    /// Reset Camera.
    /// </summary>
    public void ResetCamera()
    {
        m_Transform.position = start_Pos;
    }

}
