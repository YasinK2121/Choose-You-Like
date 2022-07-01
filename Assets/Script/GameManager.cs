using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using ElephantSDK;

public class GameManager : MonoBehaviour
{

    public CategoriesData categoriesData;
    public LevelsDisplayer levelData;

    public GameObject gobject;
    public GameObject checkObject;
    public GameObject RotPoint;
    public GameObject inGamePanel;
    public GameObject nextLevelPanel;
    public GameObject againPanel;
    public GameObject startPanel;
    public GameObject tutorialPanel;
    public GameObject loveEmoji;
    public GameObject sadEmoji;

    public List<GameObject> allObject;
    public List<GameObject> againList;
    public List<GameObject> trueObject;
    public List<GameObject> falseObject;

    public Vector3 midPos;

    public float rightPosX;
    public float leftPosX;
    public float mouseNowPos;
    public float destroyTimer;
    public float loseTimer;
    public float speed;
    private float savedTİmer;

    public bool moveCheck;
    public bool midMoveCheck;
    public bool turnCheck;
    public bool gameStarted;
    public bool destroyTimerCheck;
    public bool rightCheck;
    public bool leftCheck;
    public bool moveControl;
    public bool loseTimerCheck;
    private bool moveTouchCheck;

    public int whichLevel;
    public int textLevel;
    public int whichCategories;
    public int whichCategoriesObjectCount;

    public TextMeshProUGUI categoriesText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI nextLevelText;
    public TextMeshProUGUI againText;

    public Button startButton;
    public Button againButton;
    public Button nextLevelButton;

    [Header("LevelCountBar")]
    public Image imageBar;
    public float imageFielled;
    public float imageFielledCalcu;
    public bool barCheck;
    public bool barMoveCheck;

    void Start()
    {
        PlayerPrefs.SetInt("whichLevel", whichLevel);
        PlayerPrefs.SetInt("textLevel", textLevel);
        PlayerPrefs.Save();
        whichLevel = PlayerPrefs.GetInt("whichLevel");
        textLevel = PlayerPrefs.GetInt("textLevel");
        startButton.onClick.AddListener(() => StartButton());
        nextLevelButton.onClick.AddListener(() => NextLevelButton());
        againButton.onClick.AddListener(() => AgainButton());
        inGamePanel.SetActive(false);
        nextLevelPanel.SetActive(false);
        againPanel.SetActive(false);
        startPanel.SetActive(true);
        gameStarted = false;
        loseTimer = 1f;
        midPos = RotPoint.transform.position;
    }

    public Vector3 dic;

    void Update()
    {
        if (gameStarted)
        {
            TurnObject();
            TouchController();
            ObjectMovementController();
            BarControl();
            ObjectRightOrLeftMovement();
        }
        if (!loseTimerCheck)
        {
            GameTimer();
        }
        LoseTimer();
    }

    public void StartButton()
    {
        inGamePanel.SetActive(true);
        startPanel.SetActive(false);
        FillTheLevel();
        SpawnObject();
        rightPosX = gobject.transform.position.x + 0.3f;
        leftPosX = gobject.transform.position.x - 0.3f;
        savedTİmer = destroyTimer;
        gameStarted = true;
        if (whichLevel == 0)
        {
            tutorialPanel.SetActive(true);
        }
        else
        {
            tutorialPanel.SetActive(false);
        }
    }

    public void AgainButton()
    {
        Elephant.LevelFailed(textLevel);
        loseTimerCheck = false;
        loseTimer = 1f;
        allObject.Clear();
        for (int i = 0; i < againList.Count; i++)
        {
            allObject.Add(againList[i]);
        }
        destroyTimer = savedTİmer;
        destroyTimerCheck = false;
        againPanel.SetActive(false);
        inGamePanel.SetActive(true);
        gameStarted = true;
        loveEmoji.SetActive(false);
        sadEmoji.SetActive(false);
        rightCheck = false;
        leftCheck = false;
        imageBar.fillAmount = 0;
        imageFielledCalcu = 1f / allObject.Count;
        imageFielled = imageFielledCalcu;
        Destroy(gobject);
        SpawnObject();
    }

    public void NextLevelButton()
    {
        gameStarted = true;
        barMoveCheck = false;
        nextLevelPanel.SetActive(false);
        inGamePanel.SetActive(true);
        Elephant.LevelCompleted(textLevel);
        if (textLevel > levelData.LevelAndStepList.Count)
        {
            int a = Random.Range(0, levelData.LevelAndStepList.Count);
            whichLevel = a;
        }
        else
        {
            whichLevel++;
        }
        textLevel++;
        PlayerPrefs.SetInt("whichLevel", whichLevel);
        PlayerPrefs.SetInt("textLevel", textLevel);
        PlayerPrefs.Save();
        FillTheLevel();
        SpawnObject();
    }

    public void BarControl()
    {
        if (barCheck)
        {
            imageBar.fillAmount += Time.deltaTime;
            barMoveCheck = true;
            if (imageBar.fillAmount >= imageFielled)
            {
                imageFielled += imageFielledCalcu;
                barCheck = false;
            }
        }
    }

    public void FillTheLevel()
    {
        allObject.Clear();
        trueObject.Clear();
        falseObject.Clear();
        againList.Clear();
        levelText.text = (textLevel + 1).ToString();
        for (int a = 0; a < levelData.LevelAndStepList[whichLevel].trueObject.Count; a++)
        {
            whichCategories = (int)levelData.LevelAndStepList[whichLevel].trueObject[a].categories;
            categoriesText.text = levelData.LevelAndStepList[whichLevel].trueObject[a].categories.ToString().ToUpper();
            whichCategoriesObjectCount = levelData.LevelAndStepList[whichLevel].trueObject[a].gObjectCount;
            for (int b = 0; b < categoriesData.categorie.Count; b++)
            {
                if ((int)categoriesData.categorie[b].WhichCategorie == whichCategories)
                {
                    for (int c = 0; c < whichCategoriesObjectCount; c++)
                    {
                        int d = Random.Range(0, categoriesData.categorie[whichCategories].categorieObjects.Count);
                        if (!allObject.Contains(categoriesData.categorie[whichCategories].categorieObjects[d].gObject))
                        {
                            allObject.Add(categoriesData.categorie[whichCategories].categorieObjects[d].gObject);
                            againList.Add(categoriesData.categorie[whichCategories].categorieObjects[d].gObject);
                            trueObject.Add(categoriesData.categorie[whichCategories].categorieObjects[d].gObject);
                        }
                        else
                        {
                            c--;
                        }
                    }
                }
            }
        }

        for (int a = 0; a < levelData.LevelAndStepList[whichLevel].falseObject.Count; a++)
        {
            whichCategories = (int)levelData.LevelAndStepList[whichLevel].falseObject[a].categories;
            whichCategoriesObjectCount = levelData.LevelAndStepList[whichLevel].falseObject[a].gObjectCount;
            for (int b = 0; b < categoriesData.categorie.Count; b++)
            {
                if ((int)categoriesData.categorie[b].WhichCategorie == whichCategories)
                {
                    for (int c = 0; c < whichCategoriesObjectCount; c++)
                    {
                        int d = Random.Range(0, categoriesData.categorie[whichCategories].categorieObjects.Count);
                        if (!allObject.Contains(categoriesData.categorie[whichCategories].categorieObjects[d].gObject))
                        {
                            allObject.Add(categoriesData.categorie[whichCategories].categorieObjects[d].gObject);
                            againList.Add(categoriesData.categorie[whichCategories].categorieObjects[d].gObject);
                            falseObject.Add(categoriesData.categorie[whichCategories].categorieObjects[d].gObject);
                        }
                        else
                        {
                            c--;
                        }
                    }
                }
            }
        }
        imageBar.fillAmount = 0;
        imageFielledCalcu = 1f / allObject.Count;
        imageFielled = imageFielledCalcu;
    }

    public void SpawnObject()
    {
        if (allObject.Count == 0)
        {
            gameStarted = false;
            inGamePanel.SetActive(false);
            nextLevelPanel.SetActive(true);
            nextLevelText.text = "LEVEL " + (textLevel + 1).ToString() + " COMPLETED";
        }
        else
        {
            int a = Random.Range(0, allObject.Count);
            checkObject = allObject[a];
            RotPoint.transform.eulerAngles = new Vector3(0, 0, 0);
            gobject = Instantiate(allObject[a], RotPoint.transform.position, allObject[a].transform.rotation);
            gobject.transform.parent = RotPoint.transform;
            allObject.RemoveAt(a);
            turnCheck = true;
            barMoveCheck = false;
            moveCheck = true;
            moveControl = true;
            moveTouchCheck = true;
            Elephant.LevelStarted(textLevel);
        }
    }

    private Vector3 screenPosition;
    private Vector3 worldPosition;
    private Vector3 worldPosition2;
    public Vector3 move;
    public void TouchController()
    {
        screenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -1.1f);
        worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        if (Input.GetMouseButtonDown(0))
        {
            worldPosition2 = Camera.main.ScreenToWorldPoint(screenPosition);
        }

        if (Input.GetMouseButton(0))
        {
            move = worldPosition - worldPosition2;
            mouseNowPos = Input.GetAxis("Mouse X");
            Move();
            tutorialPanel.SetActive(false);
            moveCheck = false;
        }

        if (Input.GetMouseButtonUp(0))
        {
            moveCheck = true;
            ObjectTouchUpMovementController();
        }
    }
    public void TurnObject()
    {
        if (turnCheck)
        {
            RotPoint.transform.Rotate(0, 0, 1f);
            gobject.transform.position = new Vector3(gobject.transform.position.x, RotPoint.transform.position.y, gobject.transform.position.z);
            if (RotPoint.transform.eulerAngles.z >= 359)
            {
                turnCheck = false;
            }
        }
    }

    public void LookTrueOrFalseObject(string whichDirection)
    {
        if (trueObject.Contains(checkObject))
        {
            if (whichDirection == "right")
            {
                loveEmoji.SetActive(true);
            }
            else
            {
                sadEmoji.SetActive(true);
                loseTimerCheck = true;
            }
        }
        else if (falseObject.Contains(checkObject))
        {
            if (whichDirection == "left")
            {
                loveEmoji.SetActive(true);
            }
            else
            {
                sadEmoji.SetActive(true);
                loseTimerCheck = true;
            }
        }
    }

    public void ObjectMovementController()
    {
        if (moveCheck)
        {
            if (mouseNowPos > 1)
            {
                moveControl = false;
                midMoveCheck = false;
                rightCheck = true;
            }
            else if (mouseNowPos < -1)
            {
                moveControl = false;
                midMoveCheck = false;
                leftCheck = true;
            }
            else
            {
                mouseNowPos = 0;
            }
        }

        if (midMoveCheck)
        {
            MidMovement();
            if (gobject.transform.position == midPos)
            {
                midMoveCheck = false;
            }
        }
    }

    public void ObjectTouchUpMovementController()
    {
        if (moveControl)
        {
            if (rightPosX < gobject.transform.position.x)
            {
                rightCheck = true;
            }
            else if (leftPosX > gobject.transform.position.x)
            {
                leftCheck = true;
            }
            else
            {
                midMoveCheck = true;
            }
        }
    }

    public void ObjectRightOrLeftMovement()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            rightCheck = true;
            tutorialPanel.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            leftCheck = true;
            tutorialPanel.SetActive(false);
        }

        if (rightCheck)
        {
            speed = 5;
            RightMovement();
            LookTrueOrFalseObject("right");
            destroyTimerCheck = true;
            moveTouchCheck = false;
        }
        else if (leftCheck)
        {
            speed = 5;
            LeftMovement();
            LookTrueOrFalseObject("left");
            destroyTimerCheck = true;
            moveTouchCheck = false;
        }
    }

    public void RightMovement()
    {
        gobject.transform.position = new Vector3(gobject.transform.position.x + Time.deltaTime * speed, gobject.transform.position.y, gobject.transform.position.z);
    }

    public void LeftMovement()
    {
        gobject.transform.position = new Vector3(gobject.transform.position.x - Time.deltaTime * speed, gobject.transform.position.y, gobject.transform.position.z);
    }

    public void MidMovement()
    {
        gobject.transform.position = Vector3.MoveTowards(gobject.transform.position, midPos, Time.deltaTime * 10);
    }

    public void Move()
    {
        if (moveTouchCheck)
        {
            gobject.transform.position = Vector3.MoveTowards(gobject.transform.position, new Vector3(midPos.x - move.x, gobject.transform.position.y, gobject.transform.position.z), Time.deltaTime * 10);
        }
    }

    public void GameTimer()
    {
        if (destroyTimerCheck)
        {
            destroyTimer -= Time.deltaTime * 1f;
            moveCheck = false;
            mouseNowPos = 0;
            if (!barCheck && !barMoveCheck)
            {
                barCheck = true;
            }
        }

        if (destroyTimer <= 0)
        {
            destroyTimer = savedTİmer;
            destroyTimerCheck = false;
            loveEmoji.SetActive(false);
            sadEmoji.SetActive(false);
            rightCheck = false;
            leftCheck = false;
            Destroy(gobject);
            SpawnObject();
        }
    }

    public void LoseTimer()
    {
        if (loseTimerCheck)
        {
            loseTimer -= Time.deltaTime * 1f;
            moveCheck = false;
            mouseNowPos = 0;
        }

        if (loseTimer <= 0)
        {
            gameStarted = false;
            inGamePanel.SetActive(false);
            againPanel.SetActive(true);
            againText.text = "LEVEL " + (textLevel + 1).ToString() + " FAILED";
        }
    }
}