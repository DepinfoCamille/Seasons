using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ButtonNavigation : MonoBehaviour
{

    int count = 1;
    public Button Button1;
    public Button Button2;
    public Button Button3;
    public Button Button4;
    private bool canMoveUp = true;
    private bool canMoveDown = true;
    ColorBlock selectColor;
    ColorBlock nonSelectColor;

    // Use this for initialization
    void Start()
    {

        selectColor = Button1.colors;
        nonSelectColor = Button1.colors;
        selectColor.normalColor = Color.blue;
        nonSelectColor.normalColor = Color.white;
        Button1.colors = selectColor;
    }

    void Update()
    {
        var inputY = Input.GetAxis("MenuVertical");

        if (inputY > 0.1)
        {
            MoveUp();
        }
        else
        {
            canMoveUp = true;
        }

        if (inputY < -0.1)
        {
            MoveDown();
        }
        else
        {
            canMoveDown = true;
        }

        if (Input.GetButtonDown("Enter"))
        {
            Debug.Log("Entrée ! ");
            GetComponent<SetNumberofPlayers>().Setup(count);
            GetComponent<Launcher>().StartGame();

        }
    }

    void MoveUp()
    {
        Debug.Log("move up");

        if (canMoveUp == true)
        {
            if (count == 4)
            {
                count = 3;
                Button4.colors = nonSelectColor;
                Button3.colors = selectColor;
            }
            else if (count == 3)
            {
                count = 2;
                Button3.colors = nonSelectColor;
                Button2.colors = selectColor;
            }
            else if (count == 2)
            {
                count = 1;
                Button2.colors = nonSelectColor;
                Button1.colors = selectColor;
            }

            else if (count == 1)
            {
                count = 4;
                Button1.colors = nonSelectColor;
                Button4.colors = selectColor;
            }
            canMoveUp = false;

        }
    }

    void MoveDown()
    {
        Debug.Log("move down");
        if (canMoveDown == true)
        {
            if (count == 1)
            {
                count = 2;
                Button1.colors = nonSelectColor;
                Button2.colors = selectColor;
            }
            else if (count == 2)
            {
                count = 3;
                Button2.colors = nonSelectColor;
                Button3.colors = selectColor;
            }

            else if (count == 3)
            {
                count = 4;
                Button3.colors = nonSelectColor;
                Button4.colors = selectColor;
            }
            else if (count == 4)
            {
                count = 1;
                Button4.colors = nonSelectColor;
                Button1.colors = selectColor;
            }
            canMoveDown = false;

        }
    }

}
