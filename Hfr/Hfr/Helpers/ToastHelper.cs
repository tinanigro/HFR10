using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using NotificationsExtensions.Toasts;

namespace Hfr.Helpers
{
    public static class ToastHelper
    {
        public static void Simple(string textline1, string title = "HFR10")
        {
            var content = new ToastContent();
            content.Visual = new ToastVisual()
            {
                TitleText = new ToastText()
                {
                    Text = title,
                },
                BodyTextLine1 = new ToastText()
                {
                    Text = textline1,
                }
            };
            content.Audio = new ToastAudio()
            {
                Src = new Uri("ms-winsoundevent:Notification.IM")
            };

            XmlDocument doc = content.GetXml();

            // Generate WinRT notification
            var toast = new ToastNotification(doc);
            toast.ExpirationTime = DateTimeOffset.Now.AddSeconds(30);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }
    }
}
