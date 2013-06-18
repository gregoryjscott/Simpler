namespace Simpler
{
    public class Parallel
    {
        public static bool Enabled = true;

        public static void Execute(params Task[] tasks)
        {
            if (Enabled)
            {
                System.Threading.Tasks.Parallel.ForEach(tasks, task => task.Execute());
            }
            else
            {
                foreach (var task in tasks)
                {
                    task.Execute();
                }
            }
        }
    }
}
