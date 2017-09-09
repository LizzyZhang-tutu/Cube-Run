using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour {

    private Transform m_Transform;
    private Transform m_Gem;

	void Start () {
        m_Transform = gameObject.GetComponent<Transform>();
        m_Gem = m_Transform.Find("gem 3");
	}
	

	void Update () {
        m_Gem.Rotate(Vector3.up, Random.Range(0.5f,10.0f));
	}
}
