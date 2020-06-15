using UnityEngine;
using System.Collections;

public class MyInput : MonoBehaviour {


	float rotation;
	bool thrust;
	bool brake;

	public static MyInput instance;
	public static float my_steering_v = 0;
	public static float my_steering_h = 0;
    public static float my_steering_up_down = 0;
    public static bool jump = false;

	

	
	//public SteeringWheel wheel;

	public SteeringButton b_left;
	public SteeringButton b_right;
	public SteeringButton b_forward;
	public SteeringButton b_backward;
	public SteeringButton b_jump;

    public SteeringButton b_up;
    public SteeringButton b_down;

    public SteeringButtonToggle b_run;

    //Transform steering_object;

    public bool fly_mode=false;            

	public bool steering_enabled=true;

    int side_modifier = 1;

	// Use this for initialization
	
	void Awake(){
		instance = this;


	}
	void Start () {
		my_steering_h=0;
		my_steering_v=0;

        if (fly_mode) {
            b_jump.gameObject.SetActive(false);

            b_up.gameObject.SetActive(true);
            b_down.gameObject.SetActive(true);
        }
        else {
            b_jump.gameObject.SetActive(true);

            b_up.gameObject.SetActive(false);
            b_down.gameObject.SetActive(false);

        }

        



    }

	// Update is called once per frame
	void Update () {

		my_steering_h = 0;
		my_steering_v = 0;
        my_steering_up_down = 0;

        my_steering_h = b_right.is_pressed ? 1 : my_steering_h;
		my_steering_h = b_left.is_pressed ? -1 : my_steering_h;
		my_steering_v = b_backward.is_pressed ? -1 : my_steering_v;
		my_steering_v = b_forward.is_pressed ? 1 : my_steering_v;

        my_steering_up_down = b_up.is_pressed ? 1 : my_steering_up_down;
        my_steering_up_down = b_down.is_pressed ? -1 : my_steering_up_down;

        jump = b_jump.is_pressed;
		if (Input.GetKey (KeyCode.Space))
			jump = true;
		if (Input.GetKey (KeyCode.W))
			my_steering_v = 1;
		if (Input.GetKey (KeyCode.S))
			my_steering_v = -1;
		if (Input.GetKey (KeyCode.D))
			my_steering_h = 1;
		if (Input.GetKey (KeyCode.A))
			my_steering_h = -1;

        if (Input.GetKey(KeyCode.R))
            my_steering_up_down = 1;
        if (Input.GetKey(KeyCode.F))
            my_steering_up_down = -1;


        my_steering_h = my_steering_h * side_modifier;
    }

	void LateUpdate(){
		//jump = false;
		
	}


    public void ToogleFlyMode() {

        fly_mode = !fly_mode;

        if (fly_mode) {
            b_jump.gameObject.SetActive(false);

            b_up.gameObject.SetActive(true);
            b_down.gameObject.SetActive(true);
            GameObject.FindObjectOfType<FPController>().FlyMode(true);
        }
        else {
            b_jump.gameObject.SetActive(true);

            b_up.gameObject.SetActive(false);
            b_down.gameObject.SetActive(false);
            GameObject.FindObjectOfType<FPController>().FlyMode(false);
        }


    }

}
