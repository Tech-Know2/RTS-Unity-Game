using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Notification
{
    public string header;
    public string description;

    public Notification(string header, string description)
    {
        this.header = header;
        this.description = description;
    }
}

public class NotificationController : MonoBehaviour
{
    //Display Counts
    public int totalNum;
    public int currentNum;

    //Notifications
    public List<Notification> notifications = new List<Notification>();

    //Display Objects
    public TextMeshProUGUI headerText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI totalNumText;
    public TextMeshProUGUI currentNumText;
    public GameObject notificationObject;

    public void Update()
    {
        totalNum = notifications.Count;

        if(totalNum != 0)
        {
            int displayNum = currentNum + 1;

            notificationObject.SetActive(true);

            totalNumText.text = totalNum.ToString();
            currentNumText.text = displayNum.ToString();

            headerText.text = notifications[currentNum].header;
            descriptionText.text = notifications[currentNum].description;
        } else 
        {
            notificationObject.SetActive(false);
        }
    }

    public void CreateNotification(string passedHeader, string passedDescription)
    {
        Notification notification = new Notification(passedHeader, passedDescription);
        notifications.Add(notification);
    }

    public void Left()
    {
        if(currentNum > 0)
        {
            currentNum--;

        } else if (currentNum == 0)
        {
            currentNum = totalNum - 1;
        }
    }

    public void Right()
    {
        if(currentNum < totalNum - 1)
        {
            currentNum++;

        } else if (currentNum == totalNum)
        {
            currentNum = 0;
        }
    }

    public void DeleteNotification()
    {
        if (totalNum > 0)
        {
            notifications.RemoveAt(currentNum);
            currentNum = CalcLeft();
        }
    }

    private int CalcLeft()
    {
        if(currentNum > 0)
        {
            return currentNum--;

        } else if (currentNum == 0 && totalNum != 0)
        {
            return currentNum = totalNum - 1;
        } else if (totalNum == 0)
        {
            return 0;
        }

        return 0;
    }
}
