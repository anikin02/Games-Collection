using UnityEngine;

public class GameBoardMohjong : MonoBehaviour
{
    [Header("GameBoard properties")]
    public float TileSize;
    public float SpacingX;
    public float SpacingY;
    public int BoardWidth = 8;
    public int BoardHeight = 12;

    // -1 - This is when nothing is selected.
    public int[,] SelectedTwoTiles = 
    {
        {-1, -1},
        {-1, -1}
    };

    [Space(10)]
    [SerializeField]
    private TileMohjong tilePref;
    [SerializeField]
    private RectTransform rTransform;

    private TileMohjong[,] board;
    
    private void Start()
    {
        GenerateGameBoard();
    }

    private void Update()
    {   
        TryDestroy();
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
                board[x, y].SetValue(x, y, number, true);
                board[x, y].SetImage(4);
            }
        }
    }

    private void CreateGameBoard()
    {
        board = new TileMohjong[BoardWidth, BoardHeight];

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
                tile.SetValue(x, y, 4, true);
            }
        }
    }

    private void TryDestroy()
    {
        if ((SelectedTwoTiles[0,0] > -1) && (SelectedTwoTiles[1, 1] > -1))
        {   
            int firstX = SelectedTwoTiles[0, 0];
            int firstY = SelectedTwoTiles[0, 1];
            int secondX = SelectedTwoTiles[1, 0];
            int secondY = SelectedTwoTiles[1, 1];

            if (board[firstX, firstY].NumberTile == board[secondX, secondY].NumberTile)
            {
                board[firstX, firstY].DestroyItSelf(0.1f);
                board[secondX, secondY].DestroyItSelf(0.1f);
            }
            else
            {   
                board[firstX, firstY].CLoseTileWithDelay(0.7f);
                board[secondX, secondY].CLoseTileWithDelay(0.7f);
            }

            for (int i = 0; i<2; i++)
            {
                for (int j = 0; j<2; j++)
                {
                    SelectedTwoTiles[i, j] = -1;
                }
            }
        }
    }
}
