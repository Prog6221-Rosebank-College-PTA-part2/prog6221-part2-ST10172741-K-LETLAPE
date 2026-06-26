using System;
using System.Collections.Generic;
using System.Drawing;

public class ActivityLogger
{
    // Create list to store activities
    private List<string> activityLog = new List<string>();

    // Add activity to the log
    private void AddActivityLog(string activity)
    {
        activityLog.Add(
            $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}: {activity}");
    }

    // Display recent activities
    private void ShowActivityLog()
    {
        AppendBotResponse(
            "Here's a summary of recent actions:");

        // Show last 10 activities
        int start = Math.Max(activityLog.Count - 10, 0);

        for (int i = start; i < activityLog.Count; i++)
        {
            AppendText(
                $"{i + 1}. {activityLog[i]}\n",
                Color.Yellow);
        }
    }
}