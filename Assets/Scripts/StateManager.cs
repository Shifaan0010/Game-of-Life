using UnityEngine.EventSystems;
using UnityEngine;

public class StateManager : MonoBehaviour, IPointerClickHandler
{
    public Color aliveColor = Color.green;
    public Color deadColor = Color.black;

    private SpriteRenderer rendererComponent;

    public bool isAlive
    {
        get
        {
            return grid[row, col] == 1;
        }
        // set
        // {
            
        //     _isAlive = value;
        // }
    }

    public int[,] grid;
    public int row;
    public int col;

    // Start is called before the first frame update
    void Start()
    {
        // rendererComponent = GetComponent<SpriteRenderer>();
    }

    public void OnPointerClick(PointerEventData data)
    {
        Debug.Log($"{data.position.x} {data.position.y}");
        // toggleState();
    }

    // public void toggleState()
    // {
    //     isAlive = !isAlive;
    // }

    // Update is called once per frame
    void Update()
    {
        // rendererComponent.color = (isAlive) ? aliveColor : deadColor;
    }
}
