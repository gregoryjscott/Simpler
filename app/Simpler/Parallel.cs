namespace Simpler
{
    public static class Parallel
    {
        public static void Execute(params Task[] tasks)
        {
            System.Threading.Tasks.Parallel.ForEach(tasks, task => task.Execute());
        }
    }
}
