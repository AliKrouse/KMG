using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class THOptions : MonoBehaviour
{
    public static bool fourPlayer = false;
    public static int pointsToWin = 5;

    public static bool[] playerIsUsingController = new bool[4];
    public static int[] controllerChoice = new int[4];

    public static string p1Name = "P1";
    public static string p2Name = "P2";
    public static string p3Name = "P3";
    public static string p4Name = "P4";

    public static int[] characterChoices = new int[4];

    public static bool[] playerIsInGame = new bool[4];
}
