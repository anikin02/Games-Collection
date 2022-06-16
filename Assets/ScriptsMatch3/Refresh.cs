using UnityEngine;
using UnityEngine.UI;

public class Refresh : MonoBehaviour
{   
    public GameObject Board;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(RefreshBoard); 
    }

    private void RefreshBoard()
    {   
        Board.GetComponent<GameBoard>().GenerateGameBoard();
    }
}
