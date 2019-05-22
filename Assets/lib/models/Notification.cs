using Sesim.Models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sesim.Models
{
    public class InGameNotification
    {
        public string title;
        public string content;
        public IReadOnlyList<NotificationAction> actions;
        INotificationProvider provider;
    }

    public class NotificationAction
    {
        public string caption;
        public string tooltip;
        public KeyCode shortcutKey;
        public int key;
    }

    public interface INotificationProvider
    {
        IReadOnlyList<NotificationAction> Actions { get; }
        void Resolve(int selectedKey, out string resolvedContent);
    }
}
