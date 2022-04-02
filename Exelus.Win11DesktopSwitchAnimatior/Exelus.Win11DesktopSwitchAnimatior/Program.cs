using TCD.System.TouchInjection;
using System.Runtime.InteropServices;
using System;

[DllImport("user32")]
static extern int GetAsyncKeyState(int i);
TouchInjector.InitializeTouchInjection(4, TouchFeedback.NONE);

bool FourFingerSwipe(int distance)
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
    Thread.Sleep(100);

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
    return true;
}

while (true)
{
    Thread.Sleep(5);
    var ctrlState = GetAsyncKeyState(17);
    var alstState = GetAsyncKeyState(18);
    var leftArrowState = GetAsyncKeyState(37);
    var rightArrowState = GetAsyncKeyState(39);
    if (ctrlState != 0 && alstState != 0 && leftArrowState != 0)
    {
        FourFingerSwipe(50);
        Thread.Sleep(300);
    }
    if (ctrlState != 0 && alstState != 0 && rightArrowState != 0)
    {
        FourFingerSwipe(-50);
        Thread.Sleep(300);
    }
}