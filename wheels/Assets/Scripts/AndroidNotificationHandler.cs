using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_ANDROID
using Unity.Notifications.Android;
#endif
using UnityEngine;

public class AndroidNotificationHandler : MonoBehaviour
{
#if UNITY_ANDROID
    private const string ChannelID = "notification_channel";
    public void ScheduleNotification(DateTime timeEnergyIncreased, int notificationIndex)
    {
        AndroidNotificationChannel notificationChannel = new AndroidNotificationChannel()
        {
            Id = ChannelID,
            Name = "Notification Channel",
            Description = "Android Push Notification channel",
            Importance = Importance.Default,
        };

        AndroidNotificationCenter.RegisterNotificationChannel(notificationChannel);

        AndroidNotification androidNotificationEnergyIncreased = new AndroidNotification
        {
            Title = "Energy Increased!",
            Text = "Dare to try again? You have energy now!",
            SmallIcon = "default",
            LargeIcon = "default",
            FireTime = timeEnergyIncreased,
            

        };

        AndroidNotification androidNotificationEnergyFull = new AndroidNotification
        {
            Title = "Energy Full!",
            Text = "Not around eh? You have full energy now! Another round maybe?",
            SmallIcon = "default",
            LargeIcon = "default",
            FireTime = timeEnergyIncreased,

        };
        AndroidNotification androidNotificationTry = new AndroidNotification
        {
            Title = "Energy z0000000rrttt",
            Text = "Not around z0rt?",
            SmallIcon = "default",
            LargeIcon = "default",
            FireTime = timeEnergyIncreased,

        };
        AndroidNotificationCenter.SendNotification(androidNotificationTry, ChannelID);

        switch (notificationIndex)
        {
            case 0:
                AndroidNotificationCenter.SendNotification(androidNotificationEnergyIncreased, ChannelID);
                Debug.Log("androidNotificationEnergyIncreased will fired at: " + timeEnergyIncreased);
                break;
            case 1:
                AndroidNotificationCenter.SendNotification(androidNotificationEnergyFull, ChannelID);
                Debug.Log("androidNotificationEnergyFull will fired at: " + timeEnergyIncreased);
                break;
            default:
                Debug.Log("no default notification");
                break;
        }
        //AndroidNotificationCenter.SendNotification(androidNotificationEnergyIncreased, ChannelID);

        //AndroidNotificationCenter.SendNotification(androidNotificationEnergyFull, ChannelID);

    }
#endif
}
