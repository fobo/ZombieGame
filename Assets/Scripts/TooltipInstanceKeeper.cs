using UnityEngine;


//this is bad practice. This script is meant to carry over the tooltip panel from scene to scene, because i forgot to make a proper container 
//for the tooltip panel. This is only being done for the playtesting and should not be in the final version of the game.
public class TooltipInstanceKeeper : MonoBehaviour
{
    public static TooltipInstanceKeeper Instance { get; private set; }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

    }
}
