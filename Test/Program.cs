using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Task> taskList = new List<Task>();
            for (int i = 0; i < 50; i++)
            {
                taskList.Add(Task.Run(() =>
                {
                    HttpClient httpClient = new HttpClient();
                    _ = httpClient.GetAsync("http://localhost:5000/");
                }));
                if (taskList.Count > 10 && taskList.Any(a => a.IsCompleted))
                {
                    taskList.Remove(taskList.Where(a => a.IsCompleted).FirstOrDefault());
                }
            }
        }
    }
}
