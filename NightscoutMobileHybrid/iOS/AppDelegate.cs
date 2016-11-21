﻿using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using HockeyApp.iOS;
using UIKit;
using WindowsAzure.Messaging;
using Newtonsoft.Json.Linq;

namespace NightscoutMobileHybrid.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		private SBNotificationHub Hub { get; set; }

		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init();

			// Code for starting up the Xamarin Test Cloud Agent
#if ENABLE_TEST_CLOUD
			Xamarin.Calabash.Start();
#endif

			LoadApplication(new App());

			//HockeyApp
			var manager = BITHockeyManager.SharedHockeyManager;
			manager.Configure("d8783c34856046d9bc081c47708843f6");
			manager.StartManager();





			return base.FinishedLaunching(app, options);
		}

		public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
		{
			Hub = new SBNotificationHub(Constants.ConnectionString, Constants.NotificationHubPath);

			Hub.UnregisterAllAsync(deviceToken, (error) =>
			{
				if (error != null)
				{
					Console.WriteLine("Error calling Unregister: {0}", error.ToString());
					return;
				}
			});


				//adds a tag for the current Nightscout URL in the App Settings
				//TODO get the azuretag from status.json
				NSSet tags = new NSSet("921a2ea1643ac807adc27026c2eb351e25c53ff2"); 

				//const string template = "{\"aps\":{\"alert\":\"$(message)\"},\"request\":\"$(requestid)\"}";

				const string templateBodyAPNS = "{\"aps\":{\"alert\":\"$(message)\"},\"eventName\":\"$(eventName)\",\"group\":\"$(group)\",\"key\":\"$(key)\",\"level\":\"$(level)\",\"sound\":\"$(sound)\",\"title\":\"$(title)\"}";

				//var alert = new JObject(
				//new JProperty("aps", new JObject(new JProperty("alert", notificationText))),
				//new JProperty("inAppMessage", notificationText))
				//.ToString(Newtonsoft.Json.Formatting.None);
				
				//JObject templates = new JObject();
				//templates["genericMessage"] = new JObject
				//{
				//	{"body", templateBodyAPNS}
				//};

				var expiryDate = DateTime.Now.AddDays(90).ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));

				Hub.RegisterTemplateAsync(deviceToken,"nightscout",templateBodyAPNS,
                      expiryDate,tags,(errorCallback) =>
				{
					if (errorCallback != null)
						Console.WriteLine("RegisterNativeAsync error: " + errorCallback.ToString());
				});

		}

		public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
		{
			//base.FailedToRegisterForRemoteNotifications(application, error);
			Console.WriteLine("RegisterNativeAsync error: " + error.ToString());
		}

		public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
		{
			ProcessNotification(userInfo, false);
		}

		//public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
		//{
		//	CrossAzurePushNotifications.Platform.ProcessNotification(userInfo);
		//}

		//public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
		//{
		//	CrossAzurePushNotifications.Platform.RegisteredForRemoteNotifications(deviceToken);
		//}

		void ProcessNotification(NSDictionary options, bool fromFinishedLaunching)
		{
			// Check to see if the dictionary has the aps key.  This is the notification payload you would have sent
			if (null != options && options.ContainsKey(new NSString("aps")))
			{
				//Get the aps dictionary
				NSDictionary aps = options.ObjectForKey(new NSString("aps")) as NSDictionary;

				string alert = string.Empty;

				//Extract the alert text
				// NOTE: If you're using the simple alert by just specifying
				// "  aps:{alert:"alert msg here"}  ", this will work fine.
				// But if you're using a complex alert with Localization keys, etc.,
				// your "alert" object from the aps dictionary will be another NSDictionary.
				// Basically the JSON gets dumped right into a NSDictionary,
				// so keep that in mind.
				if (aps.ContainsKey(new NSString("alert")))
					alert = (aps[new NSString("alert")] as NSString).ToString();

				//If this came from the ReceivedRemoteNotification while the app was running,
				// we of course need to manually process things like the sound, badge, and alert.
				if (!fromFinishedLaunching)
				{
					//Manually show an alert
					if (!string.IsNullOrEmpty(alert))
					{
						UIAlertView avAlert = new UIAlertView("Notification", alert, null, "OK", null);
						avAlert.Show();
					}
				}
			}
		}
	}
}
