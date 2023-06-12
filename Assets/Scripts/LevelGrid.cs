using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class LevelGrid {

private Vector2Int foodGridPosition;
private GameObject foodGameObject;
private int width;
private int height;
private Snake snake;
bool isTeleporting = false;

public LevelGrid (int width, int height){
	this.width=width;
	this.height=height;

}

public void Setup(Snake snake){
	this.snake=snake;
	SpawnFood();
}

List<Vector2Int> forbiddenArea1Corners = new List<Vector2Int>() {
        new Vector2Int(5,8),
        new Vector2Int(16,8),
        new Vector2Int(16,1),
        new Vector2Int(20,1),
        new Vector2Int(20,13),
        new Vector2Int(27,13),
        new Vector2Int(27,-7),
        new Vector2Int(17,-7),
        new Vector2Int(17,-12),
        new Vector2Int(25,-12),
        new Vector2Int(25,-18),
        new Vector2Int(2,-18),
        new Vector2Int(2,-6),
        new Vector2Int(9,-6),
        new Vector2Int(9,1),
        new Vector2Int(5,1),
        new Vector2Int(5,8)        
        };
List<Vector2Int> forbiddenArea2Corners = new List<Vector2Int>() {
        new Vector2Int(-31,15),
        new Vector2Int(-20,15),
        new Vector2Int(-20,9),
        new Vector2Int(-6,9),
        new Vector2Int(-6,-6),
        new Vector2Int(-17,-6),
        new Vector2Int(-17,-1),
        new Vector2Int(-22,-1),
        new Vector2Int(-22,-11),
        new Vector2Int(-31,-11),
        new Vector2Int(-31,15)
        };

public bool IsInsideForbiddenArea(Vector2Int point){
    bool insideForbiddenArea1 = false;
    int i = 0, j = forbiddenArea1Corners.Count - 1;
    for (; i < forbiddenArea1Corners.Count; j = i++)
    {
        if ((forbiddenArea1Corners[i].y > point.y) != (forbiddenArea1Corners[j].y > point.y) &&
            (point.x < (forbiddenArea1Corners[j].x - forbiddenArea1Corners[i].x) * (point.y - forbiddenArea1Corners[i].y) / (forbiddenArea1Corners[j].y - forbiddenArea1Corners[i].y) + forbiddenArea1Corners[i].x))
            insideForbiddenArea1 = !insideForbiddenArea1;
    }
    bool insideForbiddenArea2 = false;
    i = 0; j = forbiddenArea2Corners.Count - 1;
    for (; i < forbiddenArea2Corners.Count; j = i++)
    {
        if ((forbiddenArea2Corners[i].y > point.y) != (forbiddenArea2Corners[j].y > point.y) &&
            (point.x < (forbiddenArea2Corners[j].x - forbiddenArea2Corners[i].x) * (point.y - forbiddenArea2Corners[i].y) / (forbiddenArea2Corners[j].y - forbiddenArea2Corners[i].y) + forbiddenArea2Corners[i].x))
            insideForbiddenArea2 = !insideForbiddenArea2;
    }
    return insideForbiddenArea1 || insideForbiddenArea2;
}


private void SpawnFood()
{
    Vector2Int headGridPosition = snake.GetGridPosition();
    
    foodGameObject = new GameObject("Food", typeof(SpriteRenderer), typeof(Animator));
    SpriteRenderer foodSpriteRenderer = foodGameObject.GetComponent<SpriteRenderer>();
    Animator foodAnimator = foodGameObject.GetComponent<Animator>();

    do {foodGridPosition = new Vector2Int(Random.Range(-33, width-1), Random.Range(-18, height-1));
        } while (snake.GetFullSnakeGridPositionList().IndexOf(foodGridPosition) != -1 || IsInsideForbiddenArea(foodGridPosition));

    foodSpriteRenderer.sprite = GameAssets.i.foodSpriteMiesko;
    foodAnimator.runtimeAnimatorController = GameAssets.i.foodAnimationController;
    foodGameObject.transform.position = new Vector3(foodGridPosition.x, foodGridPosition.y);
}

public Vector2Int ValidateGridPosition(Vector2Int gridPosition){
    if (gridPosition.x == -18 && gridPosition.y == 1){
        if (!isTeleporting) {
            
            isTeleporting = true;
            gridPosition.x = -18;
            gridPosition.y = 7;
        }
    } else if (gridPosition.x == -18 && gridPosition.y == 7){
        if (!isTeleporting) {
            
            isTeleporting = true;
            gridPosition.x = -18;
            gridPosition.y = 1;
        }
    } else {
        isTeleporting = false;
    }
    return gridPosition;
}
public bool SnakeMoved(Vector2Int snakeGridPosition){
	if (snakeGridPosition == foodGridPosition){
	Object.Destroy(foodGameObject);
	SpawnFood();
    return true;
    } else{
	return false;
	}
}
}