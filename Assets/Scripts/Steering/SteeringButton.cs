using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class SteeringButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	public bool is_pressed;

	public Sprite image_pressed;
	public Sprite image_released;

	Image image;

	void Start () {

		image = GetComponent<Image>();

	}

	public void OnPointerDown(PointerEventData e){

		image.sprite = image_pressed;
		is_pressed = true;
	}
	public void OnPointerUp(PointerEventData e){

		image.sprite = image_released;
		is_pressed = false;
	}


}
