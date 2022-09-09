# KeyBoard Shortcuts

To Left workspace

<kbd>Win</kbd>+<kbd>Ctrl</kbd>+<kbd>↑</kbd>


To Right workspace

<kbd>Win</kbd>+<kbd>Ctrl</kbd>+<kbd>↓</kbd>

> :warning: **Auto-startup is broken as of now**: watch [this video](https://youtu.be/_bTTdKi7Lvg?t=369) to setup auto-start manually 

# Windows 11 desktop switch animations
In windows 11, animations for switching between virtual desktops have been disabled, this can be bothersome because without the sliding desktop animations its hard to tell which desktop you're on. This small application fixes that by adding the animations back.

# How it works
Windows 11 still supports slide animations when swiping on a touchscreen, we make use of this by sending touch inputs to the desktop that simulate a 4 finger swipe to left or right. This triggers the switch animation to respective left or right desktop.

# How to use
Simply build and run the application, it will start a background process. while it's running, press win + ctrl + up or down arrow to switch between desktops.


You can also run the `script.ps1` file in Powershell to toggle the program on and off
