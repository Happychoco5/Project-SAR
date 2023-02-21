using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    public float currHour;
    public int currDay;
    public int currWeek;
    public int currMonth;
    public int currYear;


    public Text dayText;
    public Text weekText;
    public Text timeText;
    public Text monthText;
    public Text yearText;

    public int timeOfDay;

    public float changeTime;

    public Material animeDay;
    public Material animeNight;
    public Material animeSunset;

    public LightingSettings animeDaySettings;
    public LightingSettings animeNightSettings;
    public LightingSettings animeSunsetSettings;

    public bool countOnce;
    public bool night;
    void Start()
    {
        StartCoroutine(RotateObject(180, Vector3.right, 12));
    }

    private void Update()
    {
        currHour += Time.deltaTime;
        changeTime += Time.deltaTime;

        timeText.text = "Time: " + currHour.ToString("00") + ":00";

        if(currHour >= 23)
        {
            //go to the next day
            //reset the time
            currHour = 0;
            currDay++;
            if(currDay >= 7)
            {
                //reset the day count
                //go to next week
                currWeek++;
                if(currWeek >= 4)
                {
                    currMonth++;
                    if(currMonth >= 12)
                    {
                        currYear++;
                        yearText.text = "Year: " + currYear.ToString();
                        currMonth = 0;
                    }
                    foreach(GameObject room in GameManager.instance.Rooms)
                    {
                        room.GetComponent<RoomScript>().PayRent();
                    }
                    monthText.text = "Month: " + currMonth.ToString();
                    currWeek = 0;
                }
                weekText.text = "Week: " + currWeek.ToString();
            }
            dayText.text = "Day: " + currDay.ToString();
        }
    }

    IEnumerator RotateObject(float angle, Vector3 axis, float inTime)
    {
        // calculate rotation speed
        float rotationSpeed = angle / inTime;

        while (true)
        {
            // save starting rotation position
            Quaternion startRotation = transform.rotation;

            float deltaAngle = 0;

            // rotate until reaching angle
            while (deltaAngle < angle)
            {
                deltaAngle += rotationSpeed * Time.deltaTime;
                deltaAngle = Mathf.Min(deltaAngle, angle);

                transform.rotation = startRotation * Quaternion.AngleAxis(deltaAngle, axis);

                yield return null;
            }

            // delay here
            yield return new WaitForSeconds(1);
        }
    }

    public void ChangeTimeofDay(int time)
    {
        if(time == 1)
        {
            //change to day
            RenderSettings.skybox = animeDay;
            //Lightmapping.lightingSettings = animeDaySettings;
        }
        else if(time == 2)
        {
            //change to night
            RenderSettings.skybox = animeNight;
            //Lightmapping.lightingSettings = animeNightSettings;
        }
        else if(time == 3)
        {
            //change to sunset
            RenderSettings.skybox = animeSunset;
            //Lightmapping.lightingSettings = animeSunsetSettings;
        }
    }
}
