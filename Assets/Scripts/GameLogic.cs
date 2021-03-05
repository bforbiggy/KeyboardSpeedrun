using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public SwitchScene switcher;

    //Animations
    public bool flippable = false;
    public bool toggle = true;
    public GameObject pLeft;
    public GameObject pRight;
    private static int animationDelay = 1000;

    //Gameplay numbers
    private static readonly float sensitivity = 0.03f;
    private static readonly float winPosition = 6.5f;
    private static readonly int cpsLimit = 175;

    void Start()
    {
        DataHub.totalTime = 0;
        PrepareFlip();
    }

    async Task PrepareFlip()
    {
        await Task.Delay(animationDelay);
        flippable = true;
    }

    async Task Decrement(int pressCount)
    {
        await Task.Delay((int)(DataHub.timeFrame * 1000));
        DataHub.runningClicks -= pressCount;
    }

    void Move(int pressCount)
    {
        float displacement = sensitivity * pressCount;
        transform.position += new Vector3(displacement, 0, 0);
    }

    void Flip()
    {
        flippable = false;
        toggle = !toggle;
        _ = PrepareFlip();
        pLeft.SetActive(toggle);
        pRight.SetActive(!toggle);
    }

    void HasWon()
    {
        if (transform.position.x > winPosition)
        {
            switcher.SwitchOver();
        }
    }

    void Anticheat(float avgCPS)
    {
        if (avgCPS > cpsLimit)
            Application.Quit(69);
    }

    void Update()
    {
        int pressCount = Input.inputString.Length;

        //Moves character based on player input
        Move(pressCount);

        //Keep track of game time and cps
        DataHub.runningClicks += pressCount;
        DataHub.totalClicks += pressCount;
        DataHub.totalTime += Time.deltaTime;

        //Set clicks to expire for running total
        _ = Decrement(pressCount);

        //Checks if game has been won
        HasWon();
    }

    void CalculateAnimationDelay(int clickSpeed)
    {
        animationDelay = (int)(1000/(0.05f*DataHub.averageCPS + 0.5));
    }

    void FixedUpdate()
    {
        //Calculate CPS and determine if player is cheating
        DataHub.averageCPS = DataHub.runningClicks / DataHub.timeFrame;
        Anticheat(DataHub.averageCPS);

        //Flip is cps limit has been reached and character can flip
        if (flippable && DataHub.averageCPS > 0)
        {
            CalculateAnimationDelay((int)DataHub.averageCPS);
            Flip();
        }
    }
}
