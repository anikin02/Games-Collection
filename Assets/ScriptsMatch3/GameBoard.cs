using UnityEngine;
using UnityEngine.UI;

public class GameBoard : MonoBehaviour
{   
    [Header("GameBoard properties")]
    public float TileSize;
    public float SpacingX;
    public float SpacingY;
    public int BoardWidth = 8;
    public int BoardHeight = 12;

    [Header("UI")]
    public Text ScoreText;
    public Text RecordText;
    public int Score = 0;
    public int Record;

    // -1 - This is when nothing is selected.
    public int[,] SelectedTwoTiles = 
    {
        {-1, -1},
        {-1, -1}
    };

    [Space(10)]
    [SerializeField]
    private Tile tilePref;
    [SerializeField]
    private RectTransform rTransform;

    private Tile[,] board;
    
    private void Start()
    {
        GenerateGameBoard();
    }

    private void Update()
    {   
        TrySwap();
        FallTiles();
        string MethodTilesRestoration = "TilesRestoration";
        Invoke(MethodTilesRestoration, 0.5f);

        UpdateRecord();
        UIManager();
    }

    public void GenerateGameBoard()
    {
        if (board == null)
        {
            CreateGameBoard();
        }

        for (int x = 0; x < BoardWidth; x++)
        {
            for (int y = 0; y < BoardHeight; y++)
            {   
                int number = Random.Range(0, 4);
                number = GetNewNumber(x, y, number);
                board[x, y].SetValue(x, y, number, true);
            }
        }
    }

    private void CreateGameBoard()
    {
        board = new Tile[BoardWidth, BoardHeight];

        float Widht = BoardWidth * (TileSize + SpacingX) + SpacingX;
        float Height = BoardHeight * (TileSize + SpacingY) + SpacingY;
        rTransform.sizeDelta = new Vector2(Widht, Height);

        float startX = -(Widht / 2) + (TileSize / 2) + SpacingX;
        float startY = (Height / 2) - (TileSize / 2) - SpacingY; 

        for (int x = 0; x < BoardWidth; x++)
        {
            for (int y = 0; y < BoardHeight; y++)
            {
                var tile = Instantiate(tilePref, rTransform, false);
                var position = new Vector2(startX + (x * (TileSize + SpacingX)), (startY - (y * (TileSize + SpacingY) )));

                tile.transform.localPosition = position;

                board[x, y] = tile;
                tile.SetValue(x, y, 0, true);
            }
        }
    }

    private int GetNewNumber(int x, int y, int number)
    {   
        if ((x > 1) && (y > 1))
        {
            if ((board[x - 1, y].NumberTile == number) && (board[x, y - 1].NumberTile == number))
            {
                if ((board[x - 2, y].NumberTile == number) && (board[x, y - 2].NumberTile == number))
                {
                    int tmpNumber = number;
                    while (number == tmpNumber)
                    {   
                        number = Random.Range(0, 4);
                    }
                    return number;
                }
            }
        }

        if (x > 1)
        {
            if (board[x - 1, y].NumberTile == number)
            {
                if (board[x - 2, y].NumberTile == number)
                {
                    int tmpNumber = number;
                    while (number == tmpNumber)
                    {
                        number = Random.Range(0, 4);
                    }
                    return number;
                }
            }
        }
        
        if (y > 1)
        {   
            if (board[x, y - 1].NumberTile == number)
            {
                if (board[x, y - 2].NumberTile == number)
                {
                    int tmpNumber = number;
                    while (number == tmpNumber)
                    {
                        number = Random.Range(0, 4);
                    }
                    return number;
                }
            }
        }
        return number;
    }

    private void TrySwap()
    {   
        if ((SelectedTwoTiles[0, 0] > -1) && (SelectedTwoTiles[1, 0] > -1))
        {   
            if ( (SelectedTwoTiles[0, 0] == SelectedTwoTiles[1, 0]) && (((SelectedTwoTiles[0, 1] - SelectedTwoTiles[1, 1]) == 1) || ((SelectedTwoTiles[0, 1] - SelectedTwoTiles[1, 1]) == -1)))
            {
                int firstX = SelectedTwoTiles[0, 0];
                int firstY = SelectedTwoTiles[0, 1];
                int secondX = SelectedTwoTiles[1, 0];
                int secondY = SelectedTwoTiles[1, 1];
                
                if ((СheckingСombinations(firstX, firstY, secondX, secondY)) || (СheckingСombinations(secondX, secondY, firstX, firstY)))
                {   
                    Swap(firstX, firstY, secondX, secondY);
                    board[SelectedTwoTiles[0, 0], SelectedTwoTiles[0, 1]].ChangeColor(255.0f, 255.0f, 255.0f, 100);
                    board[SelectedTwoTiles[1, 0], SelectedTwoTiles[1, 1]].ChangeColor(255.0f, 255.0f, 255.0f, 100);
                    SelectedTwoTiles[0, 0] = - 1;
                    SelectedTwoTiles[0, 1] = - 1;
                    SelectedTwoTiles[1, 0] = - 1;
                    SelectedTwoTiles[1, 1] = - 1;
                    DestroyСombinations(firstX, firstY);
                    DestroyСombinations(secondX, secondY);
                }
                else
                {
                    board[firstX, firstY].ChangeColor(255.0f, 255.0f, 255.0f, 100);
                    board[secondX, secondY].ChangeColor(255.0f, 255.0f, 255.0f, 100);
                    SelectedTwoTiles[0, 0] = - 1;
                    SelectedTwoTiles[0, 1] = - 1;
                    SelectedTwoTiles[1, 0] = - 1;
                    SelectedTwoTiles[1, 1] = - 1;
                } 
            }

            else if ( (SelectedTwoTiles[0, 1] == SelectedTwoTiles[1, 1]) && (((SelectedTwoTiles[0, 0] - SelectedTwoTiles[1, 0]) == 1) || ((SelectedTwoTiles[0, 0] - SelectedTwoTiles[1, 0]) == -1)))
            {   
                int firstX = SelectedTwoTiles[0, 0];
                int firstY = SelectedTwoTiles[0, 1];
                int secondX = SelectedTwoTiles[1, 0];
                int secondY = SelectedTwoTiles[1, 1];

                if ((СheckingСombinations(firstX, firstY, secondX, secondY)) || (СheckingСombinations(secondX, secondY, firstX, firstY)))
                {   
                    Swap(firstX, firstY, secondX, secondY);
                    board[SelectedTwoTiles[0, 0], SelectedTwoTiles[0, 1]].ChangeColor(255.0f, 255.0f, 255.0f, 100);
                    board[SelectedTwoTiles[1, 0], SelectedTwoTiles[1, 1]].ChangeColor(255.0f, 255.0f, 255.0f, 100);
                    SelectedTwoTiles[0, 0] = - 1;
                    SelectedTwoTiles[0, 1] = - 1;
                    SelectedTwoTiles[1, 0] = - 1;
                    SelectedTwoTiles[1, 1] = - 1;
                    DestroyСombinations(firstX, firstY);
                    DestroyСombinations(secondX, secondY);
                }
                else
                {
                    board[firstX, firstY].ChangeColor(255.0f, 255.0f, 255.0f, 100);
                    board[secondX, secondY].ChangeColor(255.0f, 255.0f, 255.0f, 100);
                    SelectedTwoTiles[0, 0] = - 1;
                    SelectedTwoTiles[0, 1] = - 1;
                    SelectedTwoTiles[1, 0] = - 1;
                    SelectedTwoTiles[1, 1] = - 1;
                }
            }

            else
            {   
                board[SelectedTwoTiles[0, 0], SelectedTwoTiles[0, 1]].ChangeColor(255.0f, 255.0f, 255.0f, 100);
                board[SelectedTwoTiles[1, 0], SelectedTwoTiles[1, 1]].ChangeColor(255.0f, 255.0f, 255.0f, 100);
                SelectedTwoTiles[0, 0] = - 1;
                SelectedTwoTiles[0, 1] = - 1;
                SelectedTwoTiles[1, 0] = - 1;
                SelectedTwoTiles[1, 1] = - 1;
            }
        }
    }

    private void Swap(int firstX, int firstY, int secondX, int secondY)
    {   
        int numberOne = board[firstX, firstY].NumberTile;
        int numberTwo = board[secondX, secondY].NumberTile;

        bool boolOne = board[firstX, firstY].CanSelect;
        bool boolTwo = board[secondX, secondY].CanSelect;

        board[firstX, firstY].SetValue(firstX, firstY, numberTwo, boolTwo);
        board[secondX, secondY].SetValue(secondX, secondY, numberOne, boolOne);
    }

    private bool СheckingСombinations(int firstX, int firstY, int secondX, int secondY)
    {
        if (firstX == 0)
        {   
            if ((board[firstX, firstY].NumberTile == board[firstX + 1, secondY].NumberTile) && (board[firstX, firstY].NumberTile == board[firstX + 2, secondY].NumberTile) )
            {
                return true;
            }

            if ((board[firstX, firstY].NumberTile == board[secondX + 1, secondY].NumberTile) && (board[firstX, firstY].NumberTile == board[secondX + 2, secondY].NumberTile) )
            {
                return true;
            }
        }

        if (firstX == 1)
        {   
            if ((board[firstX, firstY].NumberTile == board[firstX + 1, secondY].NumberTile) && (board[firstX, firstY].NumberTile == board[firstX + 2, secondY].NumberTile) )
            {
                return true;
            }
            
            if ((board[firstX, firstY].NumberTile == board[firstX + 1, secondY].NumberTile) && (board[firstX, firstY].NumberTile == board[firstX - 1, secondY].NumberTile) )
            {
                return true;
            }

            if ((board[firstX, firstY].NumberTile == board[secondX + 1, secondY].NumberTile) && (board[firstX, firstY].NumberTile == board[secondX + 2, secondY].NumberTile) )
            {
                return true;
            }
        }

        if ((firstX > 1) && (firstX <= (BoardWidth - 3)))
        {   
            if ((board[firstX, firstY].NumberTile == board[firstX + 1, secondY].NumberTile) && (board[firstX, firstY].NumberTile == board[firstX +2, secondY].NumberTile) )
            {
                return true;
            }

            if ((board[firstX, firstY].NumberTile == board[firstX - 1, secondY].NumberTile) && (board[firstX, firstY].NumberTile == board[firstX - 2, secondY].NumberTile) )
            {
                return true;
            }
            
            if ((board[firstX, firstY].NumberTile == board[firstX + 1, secondY].NumberTile) && (board[firstX, firstY].NumberTile == board[firstX - 1, secondY].NumberTile) )
            {
                return true;
            }

            if ((board[firstX, firstY].NumberTile == board[secondX + 1, secondY].NumberTile) && (board[firstX, firstY].NumberTile == board[secondX + 2, secondY].NumberTile) )
            {
                return true;
            }

            if ((board[firstX, firstY].NumberTile == board[secondX - 1, secondY].NumberTile) && (board[firstX, firstY].NumberTile == board[secondX - 2, secondY].NumberTile) )
            {
                return true;
            }
        }

        if (firstX == (BoardWidth - 2))
        {   
            if ((board[firstX, firstY].NumberTile == board[firstX - 1, secondY].NumberTile) && (board[firstX, firstY].NumberTile == board[firstX - 2, secondY].NumberTile) )
            {
                return true;
            }

            if ((board[firstX, firstY].NumberTile == board[firstX + 1, secondY].NumberTile) && (board[firstX, firstY].NumberTile == board[firstX - 1, secondY].NumberTile) )
            {
                return true;
            }

            if ((board[firstX, firstY].NumberTile == board[secondX - 1, secondY].NumberTile) && (board[firstX, firstY].NumberTile == board[secondX - 2, secondY].NumberTile) )
            {
                return true;
            }
        }

        if (firstX == (BoardWidth - 1))
        {   
            if ((board[firstX, firstY].NumberTile == board[firstX - 1, secondY].NumberTile) && (board[firstX, firstY].NumberTile == board[firstX - 2, secondY].NumberTile) )
            {
                return true;
            }

            if ((board[firstX, firstY].NumberTile == board[secondX - 1, firstY].NumberTile) && (board[firstX, firstY].NumberTile == board[secondX - 2, firstY].NumberTile) )
            {
                return true;
            }
        }

        if (firstY == 0)
        {   
            if (firstY < secondY)
            { 
                if ((board[firstX, firstY].NumberTile == board[firstX, secondY + 1].NumberTile) && (board[firstX, firstY].NumberTile == board[firstX, secondY + 2].NumberTile))
                {
                    return true;
                }
            }
            else
            {
                if ((board[firstX, firstY].NumberTile == board[secondX, secondY + 1].NumberTile) && (board[firstX, firstY].NumberTile == board[secondX, secondY + 2].NumberTile))
                {
                    return true;
                }
            } 
        }

        if (firstY == 1)
        {   
            if (firstY < secondY)
            { 
                if ((board[firstX, firstY].NumberTile == board[firstX, secondY + 1].NumberTile) && (board[firstX, firstY].NumberTile == board[firstX, secondY + 2].NumberTile))
                {
                    return true;
                }
            }
            else
            {
                if ((board[firstX, firstY].NumberTile == board[secondX, secondY + 1].NumberTile) && (board[firstX, firstY].NumberTile == board[secondX, secondY + 2].NumberTile))
                {
                    return true;
                }

                if ((board[firstX, firstY].NumberTile == board[firstX, secondY + 1].NumberTile) && (board[firstX, firstY].NumberTile == board[firstX, secondY - 1].NumberTile))
                {
                    return true;
                }
            }
        }

        if ((firstY > 1) && (firstY <= (BoardHeight - 3)))
        {   
            if (firstY < secondY)
            { 
                if ((board[firstX, firstY].NumberTile == board[firstX, secondY + 1].NumberTile) && (board[firstX, firstY].NumberTile == board[firstX, secondY + 2].NumberTile))
                {
                    return true;                    
                }
                else if (secondY > 1)
                {
                    if ((board[firstX, firstY].NumberTile == board[firstX, secondY - 1].NumberTile) && (board[firstX, firstY].NumberTile == board[firstX, secondY - 2].NumberTile))
                    {
                        return true;
                    }
                }
            }
            else
            {
                if (firstY < secondY)
                { 
                    if ((board[firstX, firstY].NumberTile == board[secondX, secondY + 1].NumberTile) && (board[firstX, firstY].NumberTile == board[secondX, secondY + 2].NumberTile))
                    {
                        return true;
                    }
                }
                else if (firstY > secondY)
                {
                    if ((board[firstX, firstY].NumberTile == board[secondX, secondY - 1].NumberTile) && (board[firstX, firstY].NumberTile == board[secondX, secondY - 2].NumberTile))
                    {
                        return true;
                    }
                }
                else
                {
                    if ((board[firstX, firstY].NumberTile == board[secondX, secondY - 1].NumberTile) && (board[firstX, firstY].NumberTile == board[secondX, secondY - 2].NumberTile))
                    {
                        return true;
                    }
                    if ((board[firstX, firstY].NumberTile == board[secondX, secondY + 1].NumberTile) && (board[firstX, firstY].NumberTile == board[secondX, secondY + 2].NumberTile))
                    {
                        return true;
                    }
                    if ((board[firstX, firstY].NumberTile == board[secondX, secondY + 1].NumberTile) && (board[firstX, firstY].NumberTile == board[secondX, secondY - 1].NumberTile))
                    {
                    return true;
                    }
                }
            }

        }

        if (firstY == (BoardHeight - 2))
        {   
            if (firstY > secondY)
            { 
                if ((board[firstX, firstY].NumberTile == board[firstX, secondY - 1].NumberTile) && (board[firstX, firstY].NumberTile == board[firstX, secondY - 2].NumberTile))
                {
                    return true;
                }
            }
            else
            {
                if ((board[firstX, firstY].NumberTile == board[secondX, secondY - 1].NumberTile) && (board[firstX, firstY].NumberTile == board[secondX, secondY - 2].NumberTile))
                {
                     return true;
                }

                if ((board[firstX, firstY].NumberTile == board[firstX, secondY + 1].NumberTile) && (board[firstX, firstY].NumberTile == board[firstX, secondY - 1].NumberTile))
                {
                    return true;
                } 
            }
        }

        if (firstY == (BoardHeight - 1))
        {
            if (firstY > secondY)
            { 
                if ((board[firstX, firstY].NumberTile == board[firstX, secondY - 1].NumberTile) && (board[firstX, firstY].NumberTile == board[firstX, secondY - 2].NumberTile))
                {
                    return true;
                }
            }
            else
            {
                if ((board[firstX, firstY].NumberTile == board[secondX, secondY - 1].NumberTile) && (board[firstX, firstY].NumberTile == board[secondX, secondY - 2].NumberTile))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void DestroyСombinations(int firstX, int firstY)
    {   
        if (firstX == 0)
        {
            if ((board[firstX, firstY].NumberTile == board[firstX + 1, firstY].NumberTile) && (board[firstX, firstY].NumberTile == board[firstX +2, firstY].NumberTile) )
            {   
                board[firstX, firstY].DestroyItSelf(1);
                board[firstX + 1, firstY].DestroyItSelf(1);
                board[firstX + 2, firstY].DestroyItSelf(1);
            }
        }

        if (firstX == 1)
        {
            if ((board[firstX, firstY].NumberTile == board[firstX + 1, firstY].NumberTile) && (board[firstX, firstY].NumberTile == board[firstX +2, firstY].NumberTile) )
            {   
                board[firstX, firstY].DestroyItSelf(1);
                board[firstX + 1, firstY].DestroyItSelf(1);
                board[firstX + 2, firstY].DestroyItSelf(1);
            }
            
            if ((board[firstX, firstY].NumberTile == board[firstX + 1, firstY].NumberTile) && (board[firstX, firstY].NumberTile == board[firstX - 1, firstY].NumberTile) )
            {   
                board[firstX, firstY].DestroyItSelf(1);
                board[firstX + 1, firstY].DestroyItSelf(1);
                board[firstX - 1, firstY].DestroyItSelf(1);
            }
        }

        if ((firstX > 1) && (firstX <= (BoardWidth - 3)))
        {
            if ((board[firstX, firstY].NumberTile == board[firstX + 1, firstY].NumberTile) && (board[firstX, firstY].NumberTile == board[firstX + 2, firstY].NumberTile) )
            {   
                board[firstX, firstY].DestroyItSelf(1);
                board[firstX + 1, firstY].DestroyItSelf(1);
                board[firstX + 2, firstY].DestroyItSelf(1);
            }

            if ((board[firstX, firstY].NumberTile == board[firstX - 1, firstY].NumberTile) && (board[firstX, firstY].NumberTile == board[firstX - 2, firstY].NumberTile) )
            {   
                board[firstX, firstY].DestroyItSelf(1);
                board[firstX - 1, firstY].DestroyItSelf(1);
                board[firstX - 2, firstY].DestroyItSelf(1);
            }
            
            if ((board[firstX, firstY].NumberTile == board[firstX + 1, firstY].NumberTile) && (board[firstX, firstY].NumberTile == board[firstX - 1, firstY].NumberTile) )
            {   
                board[firstX, firstY].DestroyItSelf(1);
                board[firstX + 1, firstY].DestroyItSelf(1);
                board[firstX - 1, firstY].DestroyItSelf(1);
            }
        }

        if (firstX == (BoardWidth - 2))
        {    
            if ((board[firstX, firstY].NumberTile == board[firstX - 1, firstY].NumberTile) && (board[firstX, firstY].NumberTile == board[firstX - 2, firstY].NumberTile) )
            {   
                board[firstX, firstY].DestroyItSelf(1);
                board[firstX - 1, firstY].DestroyItSelf(1);
                board[firstX - 2, firstY].DestroyItSelf(1);
            }

            if ((board[firstX, firstY].NumberTile == board[firstX + 1, firstY].NumberTile) && (board[firstX, firstY].NumberTile == board[firstX - 1, firstY].NumberTile) )
            {   
                board[firstX, firstY].DestroyItSelf(1);
                board[firstX + 1, firstY].DestroyItSelf(1);
                board[firstX - 1, firstY].DestroyItSelf(1);
            }
        }

        if (firstX == (BoardWidth - 1))
        {    
            if ((board[firstX, firstY].NumberTile == board[firstX - 1, firstY].NumberTile) && (board[firstX, firstY].NumberTile == board[firstX - 2, firstY].NumberTile) )
            {   
                board[firstX, firstY].DestroyItSelf(1);
                board[firstX - 1, firstY].DestroyItSelf(1);
                board[firstX - 2, firstY].DestroyItSelf(1);
            }
        }

        if (firstY == 0)
        {
            if ((board[firstX, firstY].NumberTile == board[firstX, firstY + 1].NumberTile) && (board[firstX, firstY].NumberTile == board[firstX, firstY + 2].NumberTile))
            {   
                board[firstX, firstY].DestroyItSelf(1);
                board[firstX, firstY + 1].DestroyItSelf(1);
                board[firstX, firstY + 2].DestroyItSelf(1);
            }
        }

        if (firstY == 1)
        {
            if ((board[firstX, firstY].NumberTile == board[firstX, firstY + 1].NumberTile) && (board[firstX, firstY].NumberTile == board[firstX, firstY +2].NumberTile))
            {   
                board[firstX, firstY].DestroyItSelf(1);
                board[firstX, firstY + 1].DestroyItSelf(1);
                board[firstX, firstY + 2].DestroyItSelf(1);
            }
            
            if ((board[firstX, firstY].NumberTile == board[firstX, firstY + 1].NumberTile) && (board[firstX, firstY].NumberTile == board[firstX, firstY - 1].NumberTile))
            {   
                board[firstX, firstY].DestroyItSelf(1);
                board[firstX, firstY + 1].DestroyItSelf(1);
                board[firstX, firstY - 1].DestroyItSelf(1);
            }
        }

        if ((firstY > 1) && (firstY <= (BoardHeight - 3)))
        {
            if ((board[firstX, firstY].NumberTile == board[firstX, firstY + 1].NumberTile) && (board[firstX, firstY].NumberTile == board[firstX, firstY + 2].NumberTile))
            {   
                board[firstX, firstY].DestroyItSelf(1);
                board[firstX, firstY + 1].DestroyItSelf(1);
                board[firstX, firstY + 2].DestroyItSelf(1);
            }

            if ((board[firstX, firstY].NumberTile == board[firstX, firstY - 1].NumberTile) && (board[firstX, firstY].NumberTile == board[firstX, firstY - 2].NumberTile))
            {   
                board[firstX, firstY].DestroyItSelf(1);
                board[firstX, firstY - 2].DestroyItSelf(1);
                board[firstX, firstY - 1].DestroyItSelf(1);
            }
            
            if ((board[firstX, firstY].NumberTile == board[firstX, firstY + 1].NumberTile) && (board[firstX, firstY].NumberTile == board[firstX, firstY - 1].NumberTile))
            {   
                board[firstX, firstY].DestroyItSelf(1);
                board[firstX, firstY + 1].DestroyItSelf(1);
                board[firstX, firstY - 1].DestroyItSelf(1);
            }
        }

        if (firstY == (BoardHeight - 2))
        {    
            if ((board[firstX, firstY].NumberTile == board[firstX, firstY - 1].NumberTile) && (board[firstX, firstY].NumberTile == board[firstX, firstY - 2].NumberTile))
            {   
                board[firstX, firstY].DestroyItSelf(1);
                board[firstX, firstY - 2].DestroyItSelf(1);
                board[firstX, firstY - 1].DestroyItSelf(1);
            }

            if ((board[firstX, firstY].NumberTile == board[firstX, firstY + 1].NumberTile) && (board[firstX, firstY].NumberTile == board[firstX, firstY - 1].NumberTile))
            {   
                board[firstX, firstY].DestroyItSelf(1);
                board[firstX, firstY + 1].DestroyItSelf(1);
                board[firstX, firstY - 1].DestroyItSelf(1);
            }
        }

        if (firstY == (BoardHeight - 1))
        {    
            if ((board[firstX, firstY].NumberTile == board[firstX, firstY - 1].NumberTile) && (board[firstX, firstY].NumberTile == board[firstX, firstY - 2].NumberTile))
            {   
                board[firstX, firstY].DestroyItSelf(1);
                board[firstX, firstY - 2].DestroyItSelf(1);
                board[firstX, firstY - 1].DestroyItSelf(1);
            }
        }
    }

    private void TilesRestoration()
    {
        for (int x = 0; x < 8; x++)
        {
            if (board[x, 0].NumberTile == 4)
            {
                int number = Random.Range(0, 4);
                number = GetNewNumber(x, 0, number);
                board[x, 0].SetValue(x, 0, number, true);
            }
        }
    }

    private void FallTiles()
    {
        for (int y = 1; y < 12; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                if (board[x, y].NumberTile == 4)
                {
                    Swap(x, y, x, y - 1);
                }
            }
        }
    }

    public void AddPoints(int points)
    {
        Score += points;
    }

    private void UpdateRecord()
    {
        if (Score > Record)
        {
            Record = Score;
        }
    }

    private void UIManager()
    {
        ScoreText.text = Score.ToString();
        RecordText.text = Record.ToString();
    }
}