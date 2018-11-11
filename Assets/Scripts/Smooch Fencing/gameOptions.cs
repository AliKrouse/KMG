using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameOptions : MonoBehaviour
{
    public static int ptw = 3;
    public static string mode;
    public static int pink, blue;

    public static bool[] playerIsUsingController = new bool[2];
    public static int[] controllerChoice = new int[2];

    public static string pinkName = "PINK";
    public static string blueName = "BLUE";

    public static int[] characterChoices = new int[2];

    public static string[] controllerTypes = new string[2];
}
