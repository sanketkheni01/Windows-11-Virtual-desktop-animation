# Windows 11 desktop switch animations
In windows 11, animations for switching between virtual desktops have been disabled, this can be bothersome because without the sliding desktop animations its hard to tell which desktop you're on. This small application fixes that by adding the animations back.

# How it works
Windows 11 still supports slide animations when swiping on a touchscreen, we make use of this by sending touch inputs to the desktop that simulate a 4 finger swipe to left or right. This triggers the switch animation to respective left or right desktop.

# How to use
Simply build and run the application, it will start a background process. while it's running, press ctrl + alt + left or right arrow to switch between desktops.

You can also run the `script.ps1` file in Powershell to toggle the program on and off


