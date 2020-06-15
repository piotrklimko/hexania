using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchArea : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler{

	//bool released = true;

	public Vector2 delta_pos;

	public static TouchArea instance;

	// Use this for initialization
	void Start () {

		instance = this;
		
	}


	public void OnPointerDown(PointerEventData data){
		//released = false;
		delta_pos = Vector2.zero;

	}
	public void OnDrag(PointerEventData data){
		//released = false;

		delta_pos=data.delta;

	}
	public void OnPointerUp(PointerEventData data){
		//released = true;
		delta_pos = Vector2.zero;
	
	}

}
