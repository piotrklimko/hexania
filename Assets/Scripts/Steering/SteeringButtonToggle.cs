using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class SteeringButtonToggle : MonoBehaviour, IPointerDownHandler{

	public bool is_pressed;

	public Sprite image_pressed;
	public Sprite image_released;



	Image image;

	void Start () {

		image = GetComponent<Image>();

		if (is_pressed){
			image.sprite = image_pressed;
		}else{
			image.sprite = image_released;
		}

	}

	public void OnPointerDown(PointerEventData e){

		is_pressed = !is_pressed;

		if (is_pressed){
			image.sprite = image_pressed;
		}else{
			image.sprite = image_released;
		}
	}
	/*public void OnPointerUp(PointerEventData e){

		image.sprite = image_released;
		is_pressed = false;
	}*/


}
