using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class OnlineGameController : MonoBehaviour
{
    // stores the location of all criss & cross
    private int[,] checkBoxes = new int[3, 3];

    // reference to criss & cross buttons in 1D array
    [SerializeField] private GameObject[] crissButtons1D;
    [SerializeField] private GameObject[] crossButtons1D;

    // reference to criss & cross buttons in 2D array
    private GameObject[,] crissButtons2D = new GameObject[3, 3];
    private GameObject[,] crossButtons2D = new GameObject[3, 3];

    // stores the active player number
    private int activePlayerNumber;

    // stores the criss and cross prefabs in the array
    [SerializeField] private GameObject[] crissAndCross = new GameObject[2];

    // stores the criss and cross buttons in the array
    [SerializeField] private GameObject[] crissAndCrossButtons = new GameObject[2];

    // temporary game objects used in RandomlyAssignCrissCross function
    private GameObject tmpGameObject;
    private int tmpIndex;

    // stores player background
    [SerializeField] private GameObject crissBackground;
    [SerializeField] private GameObject crossBackground;

    // stores active player highlighter 
    [SerializeField] private GameObject activePlayer1Highlighter;
    [SerializeField] private GameObject activePlayer2Highlighter;

    // stores the array index
    // subtracting to match with array index
    private int upperRow = 1 - 1;
    private int middleRow = 2 - 1;
    private int lowerRow = 3 - 1;
    private int leftColumn = 1 - 1;
    private int middleColumn = 2 - 1;
    private int rightColumn = 3 - 1;

    // stores the boxes position
    private float upperRowPosition = 2.9f;
    private float middleRowPosition = 0f;
    private float lowerRowPosition = -2.9f;
    private float leftColumnPosition = -2.9f;
    private float middleColumnPosition = 0f;
    private float rightColumnPosition = 2.9f;

    // stores the above positions in array for easiness to code;
    private float[] AIrowPosition = new float[3];
    private float[] AIcolumnPosition = new float[3];

    // checks sleeping of AI
    private bool AIsleeping = true;

    // checks for gameOverl
    [SerializeField] private static bool gameOver = false;

    //store the winning bars
    [SerializeField] private GameObject[] rowBar;
    [SerializeField] private GameObject[] columnBar;
    [SerializeField] private GameObject diagonal45;
    [SerializeField] private GameObject diagonal135;
    [SerializeField] private TMP_Text player1;
    [SerializeField] private TMP_Text player2;

    // Start is called before the first frame update
    void Start()
    {
        SetPlayerNames();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetPlayerNames()
    {
        if (PhotonNetwork.IsMasterClient)
            player1.text = PhotonNetwork.NickName;
        else
            player2.text = PhotonNetwork.NickName;
    }
}
