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
    public void ScheduleNotification(DateTime energyIncreased, DateTime energyFullDate)
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
            FireTime = energyIncreased,
            

        };

        AndroidNotification androidNotificationEnergyFull = new AndroidNotification
        {
            Title = "Energy Full!",
            Text = "Not around eh? You have full energy now! Another round maybe?",
            SmallIcon = "default",
            LargeIcon = "default",
            FireTime = energyFullDate,

        };

        AndroidNotificationCenter.SendNotification(androidNotificationEnergyIncreased, ChannelID);

        AndroidNotificationCenter.SendNotification(androidNotificationEnergyFull, ChannelID);

    }
#endif
}
