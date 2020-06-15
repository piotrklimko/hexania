using UnityEngine;
using System.Collections;

public static class GlobalVars{

	public static bool rated = false;

	public static string gameover_msg="";
	public static int launches;
    public static bool sounds = true;
    public static bool music = false;
    public static string language="";
	public static bool saved_exists=false;
    public static bool first_launch = true;


    public static int GAME_READY=0;
    public static int GAME_RUNNING=1;
    public static int gamestate = GAME_READY;


    public const int READY = 0;
    public const int RUNNING = 1;
    public const int GAMEOVER = 2;
    public const int CRASH = 3;

    public static int gameState = RUNNING;


    public static int score;
    public static int hiScore;

}
