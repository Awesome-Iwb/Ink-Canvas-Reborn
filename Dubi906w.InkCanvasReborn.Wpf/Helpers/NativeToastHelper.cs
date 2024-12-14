using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Notifications;

namespace Dubi906w.InkCanvasReborn.Wpf.Helpers {
    public static class NativeToastHelper {
        public static void ShowToast(string message) {
            var builder = new ToastContentBuilder()
                    .AddArgument("action", "viewConversation")
                    .AddArgument("conversationId", 9813)
                    .AddText("Some text")
                    .AddButton(new ToastButton()
                        .SetContent("Archive")
                        .AddArgument("action", "archive"))
                    .AddButton(new ToastButton()
                        .SetContent("Show")
                        .AddArgument("action", "archive"))
                ;
            builder.Show();
        }
    }
}
