# NightscoutHybridMobile
A cross platform mobile app that shows your Nightscout website in a WebView.

For more information about Nightscout and remote continuous glucose monitoring in the cloud, visit [nightscout.info](http://nightscout.info)

### Features:

1. Your full Nightscout website displayed inside the app
2. The ability to prevent the screen from locking so you can enjoy alarms throughout the night :)
3. Volume control in case you enjoy alarms throughout the day.
4. NEW: Push notifications for Alerts, Announcements, and Info events (customizable per device) so you can get alarms when the app is not running.  This is an alternative (FREE!) to Pushover, and works very similarly. 
5. NEW: Support for iOS and Android 
6. NEW: A flashlight so you can sneak into your child's room in the middle of the night for blood sugar checks and treatments.

We welcome your feedback and feature requests.  Please add them as [issues](https://github.com/aditmer/NightscoutHybridMobile/issues)

#Installing the mobile App 

If you would like to Beta Test this app, please join our [HockeyApp Team](https://rink.hockeyapp.net/recruit/460522d7157b4881a8e64adea9e15c74).  We are using HockyApp to distribute the beta.  If you have issues or questions on how to install this app, feel free to ask in our [Facebook Group](https://www.facebook.com/groups/347752172258608/).

# Updating your Nightscout for Push Notifications 
## Don't do this yet!  We will update these instructions and post in the Facebook group when this is ready.

Your Nightscout website will generate push notifications and send them to this new mobile app for any events (Highs, Lows, Announcements, etc) that happen.  You can customize which types of notifications you want to receive when in the mobile application.  However, you need  to update your Azure website from this Work In Progress (WIP) branch of the [Nightscout repo](https://github.com/nightscout/cgm-remote-monitor/tree/wip/azurepush).

The steps to do this are:

1. Update to the wip/azurepush branch via the [Updater Tool](http://nightscout.github.io/pages/test-beta/?branch=wip%2Fazurepush).  
2. Add the `azurepush` variable to your Enable string in application settings.
3. Ensure the `BASE_URL` is set in your App settings as described [here](https://github.com/srmoss/cgm-remote-monitor#required).  It should look something like this: `https://<your-website>.azurewebsties.net`.
