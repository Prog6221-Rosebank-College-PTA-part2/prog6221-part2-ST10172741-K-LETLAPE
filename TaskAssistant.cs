using MySql.Data.MySqlClient;
using System;
using System.Data;

public class NewBaseType
{
    private string connectionString =
        "server=localhost;database=CyberBotDB;uid=root;pwd=;";

    // Add Task
    private void AddTask(string title, string description,
                         DateTime? reminderDate)
    {
        using (MySqlConnection conn =
               new MySqlConnection(connectionString))
        {
            conn.Open();

            string query = @"INSERT INTO Tasks
                            (Title, Description, ReminderDate)
                            VALUES(@title, @desc, @reminder)";

            MySqlCommand cmd = new MySqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@title", title);
            cmd.Parameters.AddWithValue("@desc", description);
            cmd.Parameters.AddWithValue("@reminder", reminderDate);

            cmd.ExecuteNonQuery();

            AddActivityLog($"Task Added: {title}");
            AppendBotResponse($"Task '{title}' added successfully.");
        }
    }

    // Complete Task
    private void CompleteTask(int taskID)
    {
        using (MySqlConnection conn =
               new MySqlConnection(connectionString))
        {
            conn.Open();

            string query =
                "UPDATE Tasks SET Status='Completed' WHERE TaskID=@id";

            MySqlCommand cmd =
                new MySqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@id", taskID);

            cmd.ExecuteNonQuery();

            AddActivityLog($"Task {taskID} marked completed");
        }
    }

    // Delete Task
    private void DeleteTask(int taskID)
    {
        using (MySqlConnection conn =
               new MySqlConnection(connectionString))
        {
            conn.Open();

            string query =
                "DELETE FROM Tasks WHERE TaskID=@id";

            MySqlCommand cmd =
                new MySqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@id", taskID);

            cmd.ExecuteNonQuery();

            AddActivityLog($"Task {taskID} deleted");
        }
    }

    // View all Tasks
    private void ViewTasks()
    {
        using (MySqlConnection conn =
               new MySqlConnection(connectionString))
        {
            conn.Open();

            string query = "SELECT * FROM Tasks";

            MySqlDataAdapter adapter =
                new MySqlDataAdapter(query, conn);

            DataTable dt = new DataTable();
            adapter.Fill(dt);

            dataGridView1.DataSource = dt;
        }
    }
}

public class TaskAssistant : NewBaseType
{
}