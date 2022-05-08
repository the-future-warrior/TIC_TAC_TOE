using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    // stores the location of all criss & cross
    private int[,] checkBoxes = new int[3, 3];

    // reference to criss & cross buttons in 1D array
    public GameObject[] crissButtons1D;
    public GameObject[] crossButtons1D;

    // reference to criss & cross buttons in 2D array
    private GameObject[,] crissButtons2D = new GameObject[3, 3];
    private GameObject[,] crossButtons2D = new GameObject[3, 3];

    // stores the active player number
    private int activePlayerNumber;

    // stores the criss and cross prefabs in the array
    public GameObject[] crissAndCross = new GameObject[2];

    // stores the criss and cross buttons in the array
    public GameObject[] crissAndCrossButtons = new GameObject[2];

    // temporary game objects used in RandomlyAssignCrissCross function
    private GameObject tmpGameObject;
    private int tmpIndex;

    // stores player background
    public GameObject crissBackground;
    public GameObject crossBackground;

    // stores active player highlighter 
    public GameObject activePlayer1Highlighter;
    public GameObject activePlayer2Highlighter;

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
    public static bool gameOver = false;

    //store the winning bars
    public GameObject[] rowBar;
    public GameObject[] columnBar;
    public GameObject diagonal45;
    public GameObject diagonal135;

    // Start is called before the first frame update
    void Start()
    {
        // initiallizes the active player
        activePlayerNumber = 1;

        Assign2DAray();
        RandomlyAssignCrissCross();
        SetActiveButton();

        if (ButtonFunctions.playerVSai)
        {
            AIrowPosition[0] = 2.9f;
            AIrowPosition[1] = 0f;
            AIrowPosition[2] = -2.9f;

            AIcolumnPosition[0] = -2.9f;
            AIcolumnPosition[1] = 0f;
            AIcolumnPosition[2] = 2.9f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckWinner();

        if(ButtonFunctions.playerVSai)
        {
            if(activePlayerNumber == 2 && AIsleeping && !gameOver)
            {
                AIsleeping = false;
                AI();
            }
        }

        if(gameOver)
        {
            //SceneManager.LoadScene("Gameover Menu");
        }
    }

    // Randomly Assign Criss or Cross to player 1
    void RandomlyAssignCrissCross()
    {
        tmpIndex = UnityEngine.Random.Range(0, 1 + 1);
        tmpGameObject = crissAndCross[tmpIndex];

        if (tmpIndex == 1)
        {
            // interchanging the position of the criss cross backgrounds if it is other than default
            Vector3 tmpVector3 = crissBackground.transform.position;
            crissBackground.transform.position = crossBackground.transform.position;
            crossBackground.transform.position = tmpVector3;
        }
    }

    // Activating the first criss or cross button
    void SetActiveButton()
    {
        crissAndCrossButtons[tmpIndex].SetActive(true);
    }

    // Instantiates the criss cross game objects after checking the active player
    void InstantiateCrissCross(float row, float column)
    {
        if (activePlayerNumber == 1)
        {
            Instantiate(crissAndCross[tmpIndex], new Vector3(column, row, 0f), Quaternion.identity);
        }
        else if (tmpIndex == 0)
        {
            Instantiate(crissAndCross[tmpIndex + 1], new Vector3(column, row, 0f), Quaternion.identity);
        }
        else
        {
            Instantiate(crissAndCross[tmpIndex - 1], new Vector3(column, row, 0f), Quaternion.identity);
        }
    }

    // updates the active player
    void ChangeActivePlayer()
    {
        if (activePlayerNumber == 1 && !gameOver)
        {
            // changing active player to 2
            activePlayerNumber = 2;

            //changing the highlighter
            activePlayer2Highlighter.SetActive(true);
            activePlayer1Highlighter.SetActive(false);


            if (tmpIndex == 0) // checking the array index
            {
                crissAndCrossButtons[tmpIndex].SetActive(false);
                crissAndCrossButtons[tmpIndex + 1].SetActive(true);
            }
            else
            {
                crissAndCrossButtons[tmpIndex].SetActive(false);
                crissAndCrossButtons[tmpIndex - 1].SetActive(true);
            }
        }

        else if(activePlayerNumber == 2 && !gameOver)
        {
            // changing active player to 1
            activePlayerNumber = 1;

            //changing the highlighter
            activePlayer2Highlighter.SetActive(false);
            activePlayer1Highlighter.SetActive(true);

            if (tmpIndex == 0)  // checking the array index
            {
                crissAndCrossButtons[tmpIndex].SetActive(true);
                crissAndCrossButtons[tmpIndex + 1].SetActive(false);
            }
            else
            {
                crissAndCrossButtons[tmpIndex].SetActive(true);
                crissAndCrossButtons[tmpIndex - 1].SetActive(false);
            }
        }
    }

    // Setting buttons of 1D array to 2D array
    void Assign2DAray()
    {
        int k = 0;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                crissButtons2D[i, j] = crissButtons1D[k];
                crossButtons2D[i, j] = crossButtons1D[k];

                k++;
            }
        }
    }

    // Setting activity of criss & cross buttons
    void SetActivenessOfCrissCrossButtons(int row, int column)
    {
        crissButtons2D[row, column].SetActive(false);
        crossButtons2D[row, column].SetActive(false);
    }

    // Check Winner
    void CheckWinner()
    {
        for (int i = 0; i < 3; i++)
        {
            // checking rows
            if (checkBoxes[i, leftColumn] == checkBoxes[i, middleColumn] && checkBoxes[i, middleColumn] == checkBoxes[i, rightColumn] && checkBoxes[i, 0] != 0)
            {
                rowBar[i].SetActive(true);
                StartCoroutine(LoadGameoverMenu());
            }

            // checking columns
            if (checkBoxes[upperRow, i] == checkBoxes[middleRow, i] && checkBoxes[middleRow, i] == checkBoxes[lowerRow, i] && checkBoxes[0, i] != 0)
            {
                columnBar[i].SetActive(true);
                StartCoroutine(LoadGameoverMenu());
            }
        }

        //checking 135 degree diagonal
        if (checkBoxes[upperRow, leftColumn] == checkBoxes[middleRow, middleColumn] && checkBoxes[middleRow, middleColumn] == checkBoxes[lowerRow, rightColumn] && checkBoxes[0, 0] != 0)
        {
            diagonal135.SetActive(true);
            StartCoroutine(LoadGameoverMenu());
        }

        //checking 45 degree diagonal
        if (checkBoxes[upperRow, rightColumn] == checkBoxes[middleRow, middleColumn] && checkBoxes[middleRow, middleColumn] == checkBoxes[lowerRow, leftColumn] && checkBoxes[1, 1] != 0)
        {
            diagonal45.SetActive(true);
            StartCoroutine(LoadGameoverMenu());
        }

        //cheking if there are no boxes left
        bool check = false;
        for(int i = 0; i < 3; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                if(checkBoxes[i, j] == 0 && !check)
                {
                    check = true;
                    //LoadGameoverMenu();
                }
            }
        }
        if(!check)
        {
            StartCoroutine(LoadGameoverMenu());

            ButtonFunctions.resultString = "Draw.";
        }
    }

    IEnumerator LoadGameoverMenu()
    {
        //activePlayerNumber is reversed unfortunately
        //UnityEngine.Debug.Log("Winner");

        // disabling all active buttons
        for(int i = 0; i < 3; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                SetActivenessOfCrissCrossButtons(i, j);
            }
        }

        if (activePlayerNumber == 1)
        {
            ButtonFunctions.resultString = "Player " + (activePlayerNumber + 1) + " Won!";
        }
        else
        {
            ButtonFunctions.resultString = "Player " + (activePlayerNumber - 1) + " Won!";
        }

        if(ButtonFunctions.playerVSai && activePlayerNumber == 1)
        {
            ButtonFunctions.resultString = "Computer Won!";
        }
        gameOver = true;


        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Gameover Menu");
    }

    //**\\**//**\\**//**\\**//**\\**//**\\**//**\\**//**\\**//**\\**//**\\**//**\\**//**\\**//**\\**//**\\**//**\\**//**\\**//**\\**//**\\**//**\\**//**\\**//**\\**//**\\**//**\\**//**\\**//**\\**//**\\**
    // BOX BUTTON FUNCTIONS
    public void UpperLeft()
    {
        checkBoxes[upperRow, leftColumn] = activePlayerNumber;

        InstantiateCrissCross(upperRowPosition, leftColumnPosition);
        ChangeActivePlayer();
        SetActivenessOfCrissCrossButtons(upperRow, leftColumn);
    }

    public void UpperMiddle()
    {
        checkBoxes[upperRow, middleColumn] = activePlayerNumber;

        InstantiateCrissCross(upperRowPosition, middleColumnPosition);
        ChangeActivePlayer();
        SetActivenessOfCrissCrossButtons(upperRow, middleColumn);
    }

    public void UpperRight()
    {
        checkBoxes[upperRow, rightColumn] = activePlayerNumber;

        InstantiateCrissCross(upperRowPosition, rightColumnPosition);
        ChangeActivePlayer();
        SetActivenessOfCrissCrossButtons(upperRow, rightColumn);
    }

    public void MiddleLeft()
    {
        checkBoxes[middleRow, leftColumn] = activePlayerNumber;

        InstantiateCrissCross(middleRowPosition, leftColumnPosition);
        ChangeActivePlayer();
        SetActivenessOfCrissCrossButtons(middleRow, leftColumn);
    }

    public void MiddleMiddle()
    {
        checkBoxes[middleRow, middleColumn] = activePlayerNumber;

        InstantiateCrissCross(middleRowPosition, middleColumnPosition);
        ChangeActivePlayer();
        SetActivenessOfCrissCrossButtons(middleRow, middleColumn);
    }

    public void MiddleRight()
    {
        checkBoxes[middleRow, rightColumn] = activePlayerNumber;

        InstantiateCrissCross(middleRowPosition, rightColumnPosition);
        ChangeActivePlayer();
        SetActivenessOfCrissCrossButtons(middleRow, rightColumn);
    }

    public void LowerLeft()
    {
        checkBoxes[lowerRow, leftColumn] = activePlayerNumber;

        InstantiateCrissCross(lowerRowPosition, leftColumnPosition);
        ChangeActivePlayer();
        SetActivenessOfCrissCrossButtons(lowerRow, leftColumn);
    }

    public void LowerMiddle()
    {
        checkBoxes[lowerRow, middleColumn] = activePlayerNumber;

        InstantiateCrissCross(lowerRowPosition, middleColumnPosition);
        ChangeActivePlayer();
        SetActivenessOfCrissCrossButtons(lowerRow, middleColumn);
    }

    public void LowerRight()
    {
        checkBoxes[lowerRow, rightColumn] = activePlayerNumber;

        InstantiateCrissCross(lowerRowPosition, rightColumnPosition);
        ChangeActivePlayer();
        SetActivenessOfCrissCrossButtons(lowerRow, rightColumn);
    }
//**\\**//**\\**//**\\**//**\\**//**\\**//**\\**//**\\**//**\\**//**\\**//**\\**//**\\**//**\\**//**\\**//**\\**//**\\**//**\\**//**\\**//**\\**//**\\**//**\\**//**\\**//**\\**//**\\**//**\\**//**\\**

    //Minimax Algorithm
    private int finalRow, finalColumn;
    private int human = 1, compPlayer = 2;

    bool IsMovesLeft(int[,] board)
    {
        for(int i = 0; i < 3; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                if(board[i, j] == 0)
                {
                    return true;
                }
            }
        }
        return false;
    }

    int Evaluate(int[,] board)
    {
        for(int row = 0; row < 3; row++)
        {
            if(board[row, 0] == board[row, 1] && board[row, 1] == board[row, 2])
            {
                if(board[row, 0] == compPlayer)
                {
                    return +10;
                }
                else if(board[row, 0] == human)
                {
                    return -10;
                }
            }
        }

        for (int col = 0; col < 3; col++)
        {
            if (board[0, col] == board[1, col] && board[1, col] == board[2, col])
            {
                if (board[0, col] == compPlayer)
                {
                    return +10;
                }
                else if (board[0, col] == human)
                {
                    return -10;
                }
            }
        }

        if (board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2])
        {
            if (board[0, 0] == compPlayer)
            {
                return +10;
            }
            else if (board[0, 0] == human)
            {
                return -10;
            }
        }

        if (board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0])
        {
            if (board[0, 2] == compPlayer)
            {
                return +10;
            }
            else if (board[0, 2] == human)
            {
                return -10;
            }
        }

        return 0;
    }

    int minimax(int [,]board, int depth, bool isMax)
    {
        int score = Evaluate(board);

        if(score == 10 )//|| score == -10)
        {
            return score;
        }

        if(score == -10)
        {
            return score;
        }

        if(IsMovesLeft(board) == false)
        {
            return 0;
        }

        if(isMax)
        {
            int best = -1000;

            for(int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    if(board[i, j] == 0)
                    {
                        board[i, j] = compPlayer;

                        best = Mathf.Max(best, minimax(board, depth + 1, !isMax));

                        board[i, j] = 0;
                    }
                }
            }
            return best;
        }
        else
        {
            int best = 1000;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == 0)
                    {
                        board[i, j] = human;

                        best = Mathf.Min(best, minimax(board, depth + 1, !isMax));

                        board[i, j] = 0;
                    }
                }
            }
            return best;
        }
    }

    void findBestMove(int [,] board)
    {
        // initializing the best value
        int bestVal = -1000;

        // initializing the finals
        finalRow = -1;
        finalColumn = -1;

        for(int i = 0; i < 3; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                // looping through all available positions
                if(board[i, j] == 0)
                {
                    // setting the all blank positions to comp player one by one
                    board[i, j] = compPlayer;

                    // calling minimax func. and checking for the best suitable position for comp player
                    int moveVal = minimax(board, 0, false);

                    // after checking reassigning the blank position to null;
                    board[i, j] = 0;

                    if(moveVal > bestVal)
                    {
                        // reinitializing the finals and best value
                        finalRow = i;
                        finalColumn = j;
                        bestVal = moveVal;
                    }
                }
            }
        }
    }

    void AI()
    {
        
        findBestMove(checkBoxes);

        checkBoxes[finalRow, finalColumn] = activePlayerNumber;

        InstantiateCrissCross(AIrowPosition[finalRow], AIcolumnPosition[finalColumn]);
        ChangeActivePlayer();
        SetActivenessOfCrissCrossButtons(finalRow, finalColumn);

        AIsleeping = true;
    }
}