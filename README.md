# What is MoonBurst ?

Moonburst is an arduino based solution for musician that aim to provide a generic midi open source interface that accept as many "generic" controller as possible, keeping the cost factor really low, yet versatile and open. Some solutions already exists, but not as I envision mine, or at high costs and sometimes long learning curve.

The software is simple : you define functoid, that are binded to a footswitch, than can create multiple actions on that functoid based on footswtich triggers and/or states that will then send midi messages or trigger other functoid, etc,... To add buttons, just add new footswtichs. The idea is to be modular and open as possible.

![screenshot](https://github.com/Manu404/moonburst/blob/master/scrns/screen.png)

# Features

 - Use Virtual Midi Loopback or existing midi interface to send midi
 - Arduino hot-plug support and auto config
 - React to multiple triggers : 
   - Event
     - Press
     - Sustain/Pressed
     - Release
     - Released
   - State: (treat your switch as a toggle button)
     - On
     - Off
 - Custom midi message builder to define precisely what's send.
 - Trigger multiple midi messages from the same event
 - Manual trigger of each functoid/channel
 - Time-domain delay
 - Midi Monitor
 
# Roadmap
 - Write documentation for arduino wiring
 - For controllers compatibility, check the related section.
 - Scene management for layouts
 - Piano roll for midi message builder
 - Midi-clocked delay

# Compatible Controllers
Full compatibility: Those controlles have been tested and working

Haf compatibility: Those are assumed to be comptabible based on documentation, but not tested by the dev.
If you have those, it would be amazing to send us a message about your expérience and confirm the support opr any issues !

### Implemented
Full: 
 - Digitech FS3X
 
### In Developement
Full:
 - Generic Switch/Tap on mono interface
 - Generic 2 Switch/Tap on stereo interface
 - Generic Pot (expression pedal) on mono interface (this will enable support of most volume pedal on the market from known manufacturer)
 
Half: 
 - Boss FS6
 - Boss FS5L
 - Boss FS5U
 - Boss FS7

### Planned (Q2 2020) : 
Full:
 - Vox VFS55
 
# About the project

<img align="left" src="https://github.com/Manu404/moonburst/blob/master/scrns/header_half.jpg">

I've been playing with looping and loopers for years, mostly for fun or to help when practicing. Have also experienced a lot of looping in a band context with drummer etc (all the issues that comes with the package). But still mostly for experimenting and fun.

Now, I have a tangible project and personal challenge in a piece I want to play totally alone, for whom I have a solution using software I already know (2 mobius instances hosted into reaper) and that I hope will be able to perform on stage. As an additional challenge, I want no automations or pre-recorded material, etc, ...

Being a guitarist, I need to control most of the software with footswtich and I plan on using a lot of different footswitch (around 10 for the moment). I had a FCB1010 years ago, but despite being not that bad programming midi, it's just too cumbersome for me to use. Configuration is a pain, I always needed to have the manual with me, etc... I sold it, and I don't plan on buying a new one.

I have a FS3X footswitch from digitech (3 momentary swtich box), it's 25€, works with a standard stereo jack interface, I know a bit of electronic and worked as a software developer in the past.

After posting the previous message and asking for review on several facebook music nerd/tech geek related groups, the reaction I had motivated me to make this project public. And here I'm, having to refactor that code I though nobody would read :p
