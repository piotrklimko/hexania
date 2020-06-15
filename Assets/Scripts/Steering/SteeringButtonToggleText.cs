using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class SteeringButtonToggleText : MonoBehaviour, IPointerDownHandler {

    public bool is_pressed;

    //public Sprite image_pressed;
    //public Sprite image_released;
    public string text_pressed;
    public string text_released;

    string temp;

    Text text;

    void Start() {

        text = GetComponentInChildren<Text>();

        if (is_pressed) {
            text.text = text_pressed;
        }
        else {
            text.text = text_released;
        }

    }

    public void OnPointerDown(PointerEventData e) {

        is_pressed = !is_pressed;

        if (is_pressed) {
            text.text = text_pressed;
        }
        else {
            text.text = text_released;
        }
    }

    public void Swap() {
        is_pressed = !is_pressed;
        Start();
    }
    /*public void OnPointerUp(PointerEventData e){

		image.sprite = image_released;
		is_pressed = false;
	}*/


}
