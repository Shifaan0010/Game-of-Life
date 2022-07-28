using UnityEngine;
using UnityEngine.UI;

public class PlayPause : MonoBehaviour
{
    public Sprite playImage;
    public Sprite pauseImage;

    private Image buttonImage;

    private GridManager gridManager;

    public void togglePlay()
    {
        gridManager.paused = !gridManager.paused;
        setButtonSprite();
    }

    private void setButtonSprite()
    {
        buttonImage.sprite = (gridManager.paused) ? pauseImage : playImage;
    }

    // Start is called before the first frame update
    void Start()
    {
        buttonImage = GameObject.FindGameObjectWithTag("PlayButtonImage").GetComponent<Image>();

        gridManager = GameObject.FindGameObjectWithTag("GridManager").GetComponent<GridManager>();

        setButtonSprite();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
