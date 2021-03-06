﻿using Gcm.Client;
using Xamarin.Forms;
using NightscoutMobileHybrid.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(PushNotificationImplementation))]
namespace NightscoutMobileHybrid.Droid
{
	public class PushNotificationImplementation : Java.Lang.Object, IPushNotifications
	{
		public static RegisterRequest registerRequest;

		public PushNotificationImplementation()
		{
		}

		public void Register(RegisterRequest request)
		{
            var ctx = Forms.Context;
			registerRequest = request;

            // Check to ensure everything's set up right
            GcmClient.CheckDevice(ctx);
            GcmClient.CheckManifest(ctx);

            // Register for push notifications
            GcmClient.Register(ctx, Constants.SenderID);
        }

		public void Unregister()
		{
            var ctx = Forms.Context;

            GcmClient.UnRegister(ctx);
		}


	}
}
