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
    private static readonly int cpsLimit = 150;

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
        await Task.Delay(1000);
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

    int ValidInput(string input)
    {
        int total = 0;

        for(int i = 0; i < input.Length; ++i)
        {
            if (Input.GetKeyDown(input[i].ToString()))
                total++;
        }

        return total;
    }

    void Update()
    {
        int pressCount = ValidInput(Input.inputString);

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
        animationDelay = (int)(1000/(0.05f*DataHub.runningClicks + 0.5));
    }

    void FixedUpdate()
    {
        //Calculate CPS and determine if player is cheating
        if (DataHub.runningClicks > cpsLimit)
            Application.Quit(69);

        //Flip when character can flip
        if (flippable && DataHub.runningClicks > 0)
        {
            CalculateAnimationDelay(DataHub.runningClicks);
            Flip();
        }
    }
}
