using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Snake : MonoBehaviour {

    private enum Direction{
        Left,
        Right,
        Up,
        Down
    }
    private bool canChangeSpeed = true;
    public float SpeedIncrement = 1f;
    private Direction gridMoveDirection;
    private Vector2Int gridPosition;
    private float gridMoveTimer;
    private float gridMoveTimerMax;
    public float SnakeSpeed;
    private LevelGrid levelGrid;
    private int snakeBodySize;
    private List<SnakeMovePosition> snakeMovePositionList;
    private List<SnakeBodyPart> snakeBodyPartList;
    public AudioSource Powiekszenie;
    public AudioSource Pause;
    public AudioSource Dead;
    public AudioSource BackgroundMusic;
    public GameObject GameOverPanel;
    public GameObject PausePanel;
    public void Setup(LevelGrid levelGrid){this.levelGrid = levelGrid;}
    bool isPaused = false;
    public int playerScore=0;
    public Text scoreText;
    public int HighScore;
    public Text ScoreText2;

    private void Awake(){
        SnakeSpeed = PlayerPrefs.GetFloat("SnakeSpeed", SnakeSpeed);
        gridPosition = new Vector2Int(0,-18);
        gridMoveTimerMax = 1f;
        gridMoveTimer = gridMoveTimerMax;
        gridMoveDirection = Direction.Up;

        snakeMovePositionList = new List<SnakeMovePosition>();
        snakeBodySize = 0;
        snakeBodyPartList = new List<SnakeBodyPart>();

        CheckHighScore();
	    UpdateHighScoreText();
	    if (!PlayerPrefs.HasKey("HighScore")){PlayerPrefs.SetInt("HighScore", 0);}

    }

    [ContextMenu("Increase Score")]
    public void addScore(){
	    playerScore=playerScore+1;
	    scoreText.text=playerScore.ToString();
	    CheckHighScore();
	    UpdateHighScoreText();
        }
    void UpdateHighScoreText()
        {ScoreText2.text=PlayerPrefs.GetInt("HighScore").ToString();}
    void CheckHighScore()
        {if(playerScore>PlayerPrefs.GetInt("HighScore")){PlayerPrefs.SetInt("HighScore",playerScore);}}

    private void Update(){

        HandleInput();
        HandlerGridMovement();

        if (Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.KeypadPlus) && canChangeSpeed){SnakeSpeed = Mathf.Max(SnakeSpeed + SpeedIncrement, 3f);}
        if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus) && canChangeSpeed){SnakeSpeed = Mathf.Max(SnakeSpeed - SpeedIncrement, 3f);}
        if (!GameOverPanel.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {   isPaused = !isPaused;
            if (isPaused){Time.timeScale = 0;PausePanel.SetActive(!PausePanel.activeSelf);canChangeSpeed = false;Pause.Play();}
            else {Time.timeScale = 1;PausePanel.SetActive(!PausePanel.activeSelf);canChangeSpeed = true;Pause.Play();}
            }
    }

    void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.CompareTag("Wall")){
            SnakeSpeed = 0f;
            GameOverPanel.SetActive(!GameOverPanel.activeSelf);
            Dead.Play();
            Debug.Log("Game Over");
            canChangeSpeed = false;
            }
        }

    private Vector2Int GetDirectionVector(Direction direction) {
        switch (direction) {
            case Direction.Up:
                return new Vector2Int(0, 1);
            case Direction.Down:
                return new Vector2Int(0, -1);
            case Direction.Left:
                return new Vector2Int(-1, 0);
            case Direction.Right:
                return new Vector2Int(1, 0);
            default:
                return Vector2Int.zero;
        }
    }

    private Direction GetDirectionFromVector(Vector2Int directionVector) {
        if (directionVector == Vector2Int.up)
            return Direction.Up;
        else if (directionVector == Vector2Int.down)
            return Direction.Down;
        else if (directionVector == Vector2Int.left)
            return Direction.Left;
        else
            return Direction.Right;
    }

private Vector2Int nextGridMoveDirection;
    
    
private float baseInputCooldown = 1f;
private float directionChangeDelay = 0.5f;
private float lastInputTime;
private float[] lastDirectionChangeTime = new float[4];
private Direction previousDirection;
private bool isChangingDirection;
private float GetEffectiveInputCooldown(){return baseInputCooldown / SnakeSpeed;}

private void HandleInput()
{
    if (Time.time - lastInputTime < GetEffectiveInputCooldown())
    {return;}

    float horizontalInput = Input.GetAxis("Horizontal");
    float verticalInput = Input.GetAxis("Vertical");

    if (horizontalInput < 0)
    {
        if (gridMoveDirection != Direction.Right && Time.time - lastDirectionChangeTime[(int)Direction.Left] > directionChangeDelay && previousDirection != Direction.Right && !isChangingDirection)
        {StartCoroutine(ChangeDirection(Direction.Left));}
    }
    else if (horizontalInput > 0)
    {
        if (gridMoveDirection != Direction.Left && Time.time - lastDirectionChangeTime[(int)Direction.Right] > directionChangeDelay && previousDirection != Direction.Left && !isChangingDirection)
        {StartCoroutine(ChangeDirection(Direction.Right));}
    }
    else if (verticalInput > 0)
    {
        if (gridMoveDirection != Direction.Down && Time.time - lastDirectionChangeTime[(int)Direction.Up] > directionChangeDelay && previousDirection != Direction.Down && !isChangingDirection)
        {StartCoroutine(ChangeDirection(Direction.Up));}
    }
    else if (verticalInput < 0)
    {
        if (gridMoveDirection != Direction.Up && Time.time - lastDirectionChangeTime[(int)Direction.Down] > directionChangeDelay && previousDirection != Direction.Up && !isChangingDirection)
        {StartCoroutine(ChangeDirection(Direction.Down));}
    }
    previousDirection = gridMoveDirection;}

private IEnumerator ChangeDirection(Direction newDirection){
    isChangingDirection = true;
    yield return new WaitForEndOfFrame();
    if (Time.time - lastInputTime >= GetEffectiveInputCooldown())
    {   gridMoveDirection = newDirection;
        lastInputTime = Time.time;
        lastDirectionChangeTime[(int)newDirection] = Time.time;}
    isChangingDirection = false;}




    private void HandlerGridMovement() {
    Vector2Int gridMoveDirectionVector;
        switch(gridMoveDirection){
            default:
            case Direction.Right:   gridMoveDirectionVector = new Vector2Int(+1,0); break;
            case Direction.Left:    gridMoveDirectionVector = new Vector2Int(-1,0); break;
            case Direction.Up:      gridMoveDirectionVector = new Vector2Int(0,+1); break;
            case Direction.Down:    gridMoveDirectionVector = new Vector2Int(0,-1); break;
        }
    gridMoveTimer += Time.deltaTime * SnakeSpeed;
    if (gridMoveTimer >= gridMoveTimerMax) {
        if (nextGridMoveDirection != Vector2Int.zero) {
            gridMoveDirectionVector = nextGridMoveDirection;
            nextGridMoveDirection = Vector2Int.zero;
        }

        SnakeMovePosition previousSnakeMovePosition = null;
        if(snakeMovePositionList.Count>0){previousSnakeMovePosition=snakeMovePositionList[0];}
        SnakeMovePosition snakeMovePosition = new SnakeMovePosition(previousSnakeMovePosition,gridPosition,gridMoveDirection);
        snakeMovePositionList.Insert(0, snakeMovePosition);
        
        gridPosition += gridMoveDirectionVector;
        gridPosition = levelGrid.ValidateGridPosition(gridPosition);
        gridMoveTimer -= gridMoveTimerMax;

        if (levelGrid != null) {
           bool snakeAteFood = levelGrid.SnakeMoved(gridPosition);
           if (snakeAteFood){snakeBodySize++;CreateSnakeBodyPart();Powiekszenie.Play();addScore();}
           
        }

        if (snakeMovePositionList.Count >= snakeBodySize +1){
            snakeMovePositionList.RemoveAt(snakeMovePositionList.Count -1);
            }
        UpdateSnakeBodyParts();

        foreach (SnakeBodyPart snakeBodyPart in snakeBodyPartList){
        Vector2Int snakeBodyPartGridPosition = snakeBodyPart.GetGridPosition();
        if (gridPosition == snakeBodyPartGridPosition){
        SnakeSpeed = 0f;
        GameOverPanel.SetActive(!GameOverPanel.activeSelf);
        Dead.Play();
        canChangeSpeed = false;
        }}

       Vector3 headRotation = new Vector3(0, 0, GetAngleFromVector(gridMoveDirectionVector));
        if (gridMoveDirectionVector == Vector2.left)
            {headRotation.y = 180;
                transform.localScale = new Vector3(0.65f, 0.65f, 0.65f);}
        else
            {transform.localScale = new Vector3(0.65f, 0.65f, 0.65f);}

        transform.eulerAngles = headRotation;
        transform.position = new Vector3(gridPosition.x, gridPosition.y);}}

    private void CreateSnakeBodyPart(){
        snakeBodyPartList.Add(new SnakeBodyPart(snakeBodyPartList.Count));
    }
    private void UpdateSnakeBodyParts(){
        for (int i=0;i<snakeBodyPartList.Count;i++){
            snakeBodyPartList[i].SetSnakeMovePosition(snakeMovePositionList[i]);
        }
    }

        private float GetAngleFromVector(Vector2Int dir){
            float n=Mathf.Atan2(dir.y,dir.x)*Mathf.Rad2Deg;
            if(n<0)n+=360;
            if(n==180){n=0;}
            return n;
        }
    public Vector2Int GetGridPosition(){
        return gridPosition;
    }
    public List<Vector2Int> GetFullSnakeGridPositionList(){
        List<Vector2Int> gridPositionList = new List<Vector2Int>(){gridPosition};
        foreach (SnakeMovePosition snakeMovePosition in snakeMovePositionList){
            gridPositionList.Add(snakeMovePosition.GetGridPosition());
        }
        return gridPositionList;
    }




    private class SnakeBodyPart{
        
        private SnakeMovePosition snakeMovePosition;
        private Transform transform;
        private int bodyIndex;
        
        public SnakeBodyPart(int bodyIndex){
            this.bodyIndex = bodyIndex;
            GameObject snakeBodyGameObject = new GameObject("SnakeBody",typeof(SpriteRenderer));
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite=GameAssets.i.snakeBodySprite;
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sortingOrder = -bodyIndex;
            transform = snakeBodyGameObject.transform;
        }

        public void SetSnakeMovePosition(SnakeMovePosition snakeMovePosition){
            this.snakeMovePosition = snakeMovePosition;
            transform.position = new Vector3(snakeMovePosition.GetGridPosition().x, snakeMovePosition.GetGridPosition().y);
            float angle;
            switch(snakeMovePosition.GetDirection()){
            default:
            case Direction.Up:
                switch(snakeMovePosition.GetPreviousDirection()){
                    default:
                    angle=-90;break;
                    case Direction.Left:
                    angle=135;break;
                    case Direction.Right:
                    angle=-135;break;
                }
                break;
            case Direction.Down:
                switch(snakeMovePosition.GetPreviousDirection()){
                    default:
                    angle=90;break;
                    case Direction.Left:
                    angle=-135;break;
                    case Direction.Right:
                    angle=135;break;
                }
                break;
            case Direction.Left:
                switch(snakeMovePosition.GetPreviousDirection()){
                    default:
                    angle=180;break;
                    case Direction.Down:
                    angle=-135;break;
                    case Direction.Up:
                    angle=135;break;
                }
                break;
            case Direction.Right:
                switch(snakeMovePosition.GetPreviousDirection()){
                    default:
                    angle=180;break;
                    case Direction.Down:
                    angle=135;break;
                    case Direction.Up:
                    angle=225;break;
                }
                break;
            }
            transform.eulerAngles = new Vector3(0,0,angle);
        }

        public Vector2Int GetGridPosition(){
            return snakeMovePosition.GetGridPosition();
        }

        public int BodyIndex {
            get { return bodyIndex; }
        }
    }

    private class SnakeMovePosition{

    private SnakeMovePosition previousSnakeMovePosition;
    private Vector2Int gridPosition;
    private Direction direction;

    public SnakeMovePosition (SnakeMovePosition previousSnakeMovePosition, Vector2Int gridPosition, Direction direction){
        this.previousSnakeMovePosition=previousSnakeMovePosition;
        this.gridPosition=gridPosition;
        this.direction=direction;
        }
        public Vector2Int GetGridPosition(){
            return gridPosition;
        }

        public Direction GetDirection(){
            return direction;
        }
        public Direction GetPreviousDirection(){
            if(previousSnakeMovePosition == null) {return Direction.Right;} else {
            return previousSnakeMovePosition.direction;
        }

    }
}}

    
