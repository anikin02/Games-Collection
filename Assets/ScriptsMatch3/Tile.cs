using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public int X {get; private set;}
    public int Y {get; private set;}
    public bool CanSelect = true;

    public Sprite SpriteFirstTile;
    public Sprite SpriteSecondTile;
    public Sprite SpriteThirdTile;
    public Sprite SpriteFourthTile;
    
    // 0 - Dog; 1 - Cat; 2 - Cow; 3 - Pig; 4 - Nothing;
    public int NumberTile = 0;

    private GameObject Board;
    private string nameBoard = "GameBoard";

    private void Start()
    {
        Board = GameObject.Find(nameBoard);
        GetComponent<Button>().onClick.AddListener(Select);
    }

    public void SetValue(int x, int y, int number, bool canSelect)
    {
        X = x;
        Y = y;
        CanSelect = canSelect;
        NumberTile = number;

        SetImage(number);
    }

    public void SetImage(int number)
    {   
        switch(number)
        {
            case (0):
                GetComponent<Image>().sprite = SpriteFirstTile;
                ChangeColor(255, 255, 255, 100);
                break;
            case (1):
                GetComponent<Image>().sprite = SpriteSecondTile;
                ChangeColor(255, 255, 255, 100);
                break;
            case (2):
                GetComponent<Image>().sprite = SpriteThirdTile;
                ChangeColor(255, 255, 255, 100);
                break;
            case (3):
                GetComponent<Image>().sprite = SpriteFourthTile;
                ChangeColor(255, 255, 255, 100);
                break;
            case (4):
                ChangeColor(0, 0, 0, 0);
                break;
            default:
                break;
        }
    }

    public void Select()
    {   
        if(CanSelect)
        {   
            if (Board.GetComponent<GameBoard>().SelectedTwoTiles[0,0] == -1)
            {
                Board.GetComponent<GameBoard>().SelectedTwoTiles[0,0] = X;
                Board.GetComponent<GameBoard>().SelectedTwoTiles[0,1] = Y;
                ChangeColor(0, 255, 255, 100);
            }
            else if (Board.GetComponent<GameBoard>().SelectedTwoTiles[1,0] == -1)
            {
                Board.GetComponent<GameBoard>().SelectedTwoTiles[1,0] = X;
                Board.GetComponent<GameBoard>().SelectedTwoTiles[1,1] = Y;
                ChangeColor(0, 255, 255, 100);
            }
        }
    }

    public void ChangeColor(float red, float green, float blue, float alpha)
    {
        GetComponent<Image>().color = new Color(red, green, blue, alpha);    
    }
    
    public void DestroyItSelf(float waitTime)
    {   
        string nameMethod = "DestroyObject";
        Invoke(nameMethod, waitTime);
    }

    public void DestroyObject()
    {
        SetValue(X, Y, 4, false);
        Board.GetComponent<GameBoard>().AddPoints(1);
    }
}

