using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Reflection;
using Exelus.Win11DesktopSwitchAnimatior;
using Microsoft.Win32.TaskScheduler;
using System.Security.Principal;
using System.Diagnostics;

TaskService ts = new TaskService();
TaskDefinition td = ts.NewTask();

string appPath = AppContext.BaseDirectory;
string exeName = Assembly.GetExecutingAssembly().GetName().Name;
string exePath = appPath + exeName + ".exe";
string taskName = "Windows 11 Virtual desktop animation";

bool isSwitching = false;

Assembly assembly = Assembly.GetExecutingAssembly();
string version = assembly.GetName().Version.ToString();

Trace.WriteLine("This is a log message." + version);

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

[DllImport("user32")]
static extern int GetAsyncKeyState(int i);
TouchInjector.InitializeTouchInjection(4, TouchFeedback.NONE);

bool FourFingerSwipe(int distance, int time)
{
    isSwitching = true;
    var touches = new PointerTouchInfo[4];
    for (int i = 0; i < 4; i++)
    {
        var touch = new PointerTouchInfo()
        {
            PointerInfo = new PointerInfo()
            {
                PtPixelLocation = new TouchPoint()
                {
                    X = 200,
                    Y = (i * 100) + 100,
                },
                pointerType = PointerInputType.TOUCH,
                PointerFlags = PointerFlags.DOWN | PointerFlags.INRANGE | PointerFlags.INCONTACT,
                PointerId = (uint)i,
            },
            Orientation = 90,
            Pressure = 32000,
            TouchMasks = TouchMask.CONTACTAREA | TouchMask.ORIENTATION | TouchMask.PRESSURE
        };
        touches[i] = (touch);
    }

    TouchInjector.InjectTouchInput(4, touches);
    for (int i = 0; i < 4; i++)
    {
        touches[i].PointerInfo.PtPixelLocation.X += distance;
        touches[i].PointerInfo.PointerFlags = PointerFlags.UPDATE | PointerFlags.INRANGE | PointerFlags.INCONTACT;
    }
    TouchInjector.InjectTouchInput(4, touches);

    for (int i = 0; i < 4; i++)
    {
        touches[i].PointerInfo.PtPixelLocation.X += distance;
    }
    TouchInjector.InjectTouchInput(4, touches);

    for (int i = 0; i < 4; i++)
    {
        touches[i].PointerInfo.PtPixelLocation.X += distance;
        touches[i].PointerInfo.PointerFlags = PointerFlags.UP;
    }
    TouchInjector.InjectTouchInput(4, touches);
    isSwitching = false;
    return true;
}

while (true)
{
    var ctrlState = GetAsyncKeyState(17);
    var winState = GetAsyncKeyState(91);
    var upArrowState = GetAsyncKeyState(38);
    var downArrowState = GetAsyncKeyState(40);
    var altKeyState = GetAsyncKeyState(18);
    Thread.Sleep(5);
    /*
        * * Get Key State https://www.amibroker.com/guide/afl/getasynckeystate.html
    */

    Trace.WriteLine("This is a log message." + isSwitching);

    if (ctrlState != 0 && winState != 0 && upArrowState != 0 && !isSwitching)
    {
        FourFingerSwipe(100, 200);
        Thread.Sleep(200);
    }
    if (ctrlState != 0 && winState != 0 && downArrowState != 0 && !isSwitching)
    {
        FourFingerSwipe(-100, 200);
        Thread.Sleep(200);
    }
    if (altKeyState != 0 && winState != 0)
    {
        Hold(-200, 500);
    }
}

void Hold(int distance, int time)
{
    var touches = new PointerTouchInfo[4];
    for (int i = 0; i < 4; i++)
    {
        var touch = new PointerTouchInfo()
        {
            PointerInfo = new PointerInfo()
            {
                PtPixelLocation = new TouchPoint()
                {
                    X = 200,
                    Y = (i * 100) + 100,
                },
                pointerType = PointerInputType.TOUCH,
                PointerFlags = PointerFlags.DOWN | PointerFlags.INRANGE | PointerFlags.INCONTACT,
                PointerId = (uint)i,
            },
            Orientation = 90,
            Pressure = 100,
            TouchMasks = TouchMask.CONTACTAREA | TouchMask.ORIENTATION | TouchMask.PRESSURE
        };
        touches[i] = (touch);
    }

    TouchInjector.InjectTouchInput(4, touches);
    for (int i = 0; i < 4; i++)
    {
        touches[i].PointerInfo.PtPixelLocation.X += distance;
        touches[i].PointerInfo.PointerFlags = PointerFlags.UPDATE | PointerFlags.INRANGE | PointerFlags.INCONTACT;
    }
    while (true)
    {
        Thread.Sleep(5);
        var altKeyState = GetAsyncKeyState(18);
        var ctrlState = GetAsyncKeyState(17);
        var winState = GetAsyncKeyState(91);
        var upArrowState = GetAsyncKeyState(38);

        if (!(altKeyState != 0 && winState != 0))
        {
            for (int i = 0; i < 4; i++)
            {
                touches[i].PointerInfo.PointerFlags = PointerFlags.UP;
            }
            TouchInjector.InjectTouchInput(4, touches);
            break;
        }
        else
        {
            TouchInjector.InjectTouchInput(4, touches);
        }
    }
}