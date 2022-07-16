using UnityEngine.EventSystems;
using UnityEngine;

public class StateManager : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    public Color aliveColor = Color.green;
    public Color deadColor = Color.black;

    private SpriteRenderer rendererComponent;

    bool _isAlive;
    public bool isAlive
    {
        get
        {
            return _isAlive;
        }
        set
        {
            rendererComponent.color = (value) ? aliveColor : deadColor;
            _isAlive = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rendererComponent = GetComponent<SpriteRenderer>();
        isAlive = Random.value < 0.5;
    }

    public void OnPointerClick(PointerEventData data)
    {
        toggleState();
    }

    public void OnPointerEnter(PointerEventData data)
    {
        if (data.dragging)
        {
            toggleState();
        }
    }

    public void toggleState()
    {
        isAlive = !isAlive;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
