using Microsoft.Win32.TaskScheduler;
using System.Reflection;
using System.Security.Principal;

namespace Windows_11_Virtual_desktop_animation
{
    internal class TaskSchedulerM
    {
        TaskDefinition td;
        string exePath;
        string version;
        string taskName;
        TaskService ts;

        public TaskSchedulerM()
        {
            ts = new TaskService();
            td = ts.NewTask();

            string appPath = AppContext.BaseDirectory;
            string exeName = Assembly.GetExecutingAssembly().GetName().Name;
            exePath = appPath + exeName + ".exe";
            taskName = "Windows 11 Virtual desktop animation";

            Assembly assembly = Assembly.GetExecutingAssembly();
            version = assembly.GetName().Version?.ToString();

            Microsoft.Win32.TaskScheduler.Task task = ts.FindTask(taskName);
            string des = task.Definition.RegistrationInfo.Description;
            if (task == null)
            {
                createTask();
            }
            else
            {
                if (des != version)
                {
                    ts.RootFolder.DeleteTask(taskName);
                    createTask();
                }
            }
        }


        void createTask()
        {
            td.Actions.Add(new ExecAction("\"" + exePath + "\"", "", null));

            td.Principal.RunLevel = TaskRunLevel.Highest;
            // get default userid
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            td.RegistrationInfo.Description = version;
            // Get the user name
            string userName = identity.Name;
            td.Triggers.Add(new LogonTrigger());
            ts.RootFolder.RegisterTaskDefinition(taskName, td, TaskCreation.CreateOrUpdate, userName, null, TaskLogonType.InteractiveToken);
        }

    }
}
