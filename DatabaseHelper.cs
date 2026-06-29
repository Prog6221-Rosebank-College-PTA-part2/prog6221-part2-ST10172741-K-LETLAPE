using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CyberSecurityBotGUI
{
    public class DatabaseHelper
    {
        private string connectionString =
            "server=localhost;database=cyberbot;uid=root;pwd=;";

        public void AddTask(TaskItem task)
        {
            using (MySqlConnection conn =
                   new MySqlConnection(connectionString))
            {
                conn.Open();

                string query =
                    @"INSERT INTO tasks
                    (Title,Description,ReminderDate,IsCompleted)
                    VALUES
                    (@title,@desc,@reminder,@complete)";

                MySqlCommand cmd =
                    new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@title", task.Title);
                cmd.Parameters.AddWithValue("@desc", task.Description);
                cmd.Parameters.AddWithValue("@reminder",
                    task.ReminderDate);

                cmd.Parameters.AddWithValue("@complete",
                    task.IsCompleted);

                cmd.ExecuteNonQuery();
            }
        }

        public List<TaskItem> GetTasks()
        {
            List<TaskItem> tasks = new List<TaskItem>();

            using (MySqlConnection conn =
                new MySqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT * FROM tasks";

                MySqlCommand cmd =
                    new MySqlCommand(query, conn);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    tasks.Add(new TaskItem()
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Title = reader["Title"].ToString(),
                        Description = reader["Description"].ToString(),
                        ReminderDate =
                            reader["ReminderDate"] == DBNull.Value
                            ? null
                            : (DateTime?)reader["ReminderDate"],

                        IsCompleted =
                            Convert.ToBoolean(reader["IsCompleted"])
                    });
                }
            }

            return tasks;
        }
    }
}