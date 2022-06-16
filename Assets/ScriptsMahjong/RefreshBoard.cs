using UnityEngine;
using UnityEngine.UI;

public class RefreshBoard : MonoBehaviour
{   
    public GameObject Board;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(Refresh); 
    }

    private void Refresh()
    {   
        Board.GetComponent<GameBoardMohjong>().GenerateGameBoard();
    }
}