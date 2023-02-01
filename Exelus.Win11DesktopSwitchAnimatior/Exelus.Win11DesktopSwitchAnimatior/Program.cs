using System.Runtime.InteropServices;
using Exelus.Win11DesktopSwitchAnimatior;
using Windows_11_Virtual_desktop_animation;

[DllImport("user32")]
static extern int GetAsyncKeyState(int i);
TouchInjector.InitializeTouchInjection(4, TouchFeedback.NONE);

bool isSwitching = false;

// handles the task scheduler
TaskSchedulerM ts = new();

bool FourFingerSwipe(int distance, int time)
{
    isSwitching = true;
    var touches = createTouchPoints();

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
    var touches = createTouchPoints();
        
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

PointerTouchInfo[] createTouchPoints()
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
    return touches;
}