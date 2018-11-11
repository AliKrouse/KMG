using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SDOptions : MonoBehaviour
{
    public static int AINumber = 50;
    public static int hitsToFall = 3;
    public static float timer = 120;

    public static bool[] playerIsUsingController = new bool[4];
    public static int[] controllerChoice = new int[4];

    public static string p1Name = "Player 1";
    public static string p2Name = "Player 2";
    public static string p3Name = "Player 3";
    public static string p4Name = "Player 4";

    public static int[] characterChoices = new int[4];
    public static string[] controllerType = new string[4];

    public static bool[] playerIsInGame = new bool[4];
}
