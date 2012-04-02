namespace Simpler
{
    public abstract class Task
    {
        public abstract void Execute();

        //public static T Create<T>()
        //{
        //    var createTask = new CreateTask { TaskType = typeof(T) };
        //    createTask.Execute();
        //    return (T)createTask.TaskInstance;
        //}
    }
}