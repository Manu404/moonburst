[![Build status](https://ci.appveyor.com/api/projects/status/2ohdsl0j6kgifhov?svg=true)](https://ci.appveyor.com/project/Manu404/moonburst)
# What is MoonBurst ?

Moonburst is an arduino based solution for musician that aim to provide a generic midi open source interface that accept as many "generic" controller as possible, keeping the cost factor really low, yet versatile and open. Some solutions already exists, but not as I envision mine, or at high costs and sometimes long learning curve.

The software is simple : you define functoid, that are binded to a footswitch, than can create multiple actions on that functoid based on footswtich triggers and/or states that will then send midi messages or trigger other functoid, etc,... To add buttons, just add new footswtichs. The idea is to be modular and open.

![screenshot](https://github.com/Manu404/moonburst/blob/master/scrns/screen.png)

# Features

 - Tranform your regular footswitch pedals into full midi controllers
 - Use existing midi interface to send midi or use [loopMidi](https://www.tobias-erichsen.de/software/loopmidi.html) to create your own virtual interfaces
 - Host as a 32/64bit VST outputing midi (in dev)
 - Arduino hot-plug support
 - React to variety of triggers : 
   - Event
     - Press
     - Sustain/Pressed
     - Release
     - Released
   - State: (treat your switch as a toggle button)
     - On
     - Off
 - Custom midi message builder to define precisely what's send by each action.
 - Send multiple midi messages from the same trigger
 - Manual trigger of each action/channel
 - Real time delay per action
 - Midi Monitor
 - Piano roll for midi message builder
 - Western notation for pitch and dynamics
 
# Build MK1
Here's some pics and buylist for the mk1 prototype. The enclosure was one lying around, the tape was to fill some holes made for a previous project. Inputs are not well aligned and not planned well (analog/digital), the mk2 will improve on those points. But this one is working as a charm and is the one used for developement.

### Buylist

- Enclosure
- 4 dual TRS female / [thomann (2.88€/u)](https://www.thomann.de/be/neutrik_nsj12_hh_1.htm)
- TRS connector mounting screws / [thomann (0.98€/u)](https://www.thomann.de/be/neutrik_nrj_nut_b.htm)
- Arduino Uno (cheaper clone mostly works) / [amazon (25€/u)](https://www.amazon.fr/Arduino-A000066-M%C3%A9moire-flash-32/dp/B008GRTSV6/)
- Dupont connectors sets / [amazon (10€/set)](https://www.amazon.fr/ARCELI-Connecteur-Goupilles-Adaptateur-Assortiment/dp/B07PVVD26W/)
- Signal wire set, 9 color, 60m / [amazon(15€)](https://www.amazon.fr/Kit-fil-fixation-10-couleurs-60-m/dp/B001IRQRRO/)

### Pics

![mk1a](https://github.com/Manu404/moonburst/blob/master/scrns/out.png)
![mk1b](https://github.com/Manu404/moonburst/blob/master/scrns/in.png)

 
# Supported Controllers
*Full: Those controlles have been tested and working*

*Half: Those are assumed to be comptabible based on documentation, but not tested by the dev.
If you have those, it would be amazing to send us a message about your expérience and confirm the support opr any issues !*

### Implemented
*Full*
 - Digitech FS3X
 
### In Developement
*Full*
 - Generic Switch/Tap on mono interface
 - Generic 2 Switch/Tap on stereo interface
 - Generic Pot (expression pedal) on mono interface (this will enable support of most volume pedal on the market from known manufacturer)
 
*Half*
 - Boss FS6
 - Boss FS5L
 - Boss FS5U
 - Boss FS7

### Planned 
*Full*
 - Vox VFS55
 - 1 ENGL, 1 Peavy and 1 Marshall amp footswitchs (precise model coming soon)
 
# Supported Systems
Moonburst was designed for Microsoft Windows, the UI wouldn't be portable easily to linux, even tho there's plan to migrate as much as possible to .Net Core over time. The software was designed for a personal need first, so a lot of decision were made toward speed of developement regarding already known technologies.

Each release is tested against a clean default windows 10 install with latest updates.

# Roadmap
*For supported controllers, check the related section.*

 - Write documentation for arduino wiring
 - Arduino auto-config
 - Scene management for layouts
 - Midi-clocked delay
 - Implement value triggers to support expression pedal
 
It would be tempting to add a lot of midi features and tools, from remaping to routing, but I want to reduce the scope of the solution as much as possible, so those concerns can be managed at your will on your side. There's a lot of free midi (http://www.vst4free.com/index.php?m=midiVST) utility that I think are better option than trying to implement them (badly ?) myself.

# About the project

<img align="left" src="https://github.com/Manu404/moonburst/blob/master/scrns/header_half.jpg">

I've been playing with looping and loopers for years, mostly for fun or to help when practicing. Have also experienced a lot of looping in a band context with drummer etc (all the issues that comes with the package). But still mostly for experimenting and fun.

Now, I have a tangible project and personal challenge in a piece I want to play totally alone, for whom I have a solution using software I already know (2 mobius instances hosted into reaper) and that I hope will be able to perform on stage. As an additional challenge, I want no automations or pre-recorded material, etc, ...

Being a guitarist, I need to control most of the software with footswtich and I plan on using a lot of different footswitch (around 10 for the moment). I had a FCB1010 years ago, but despite being not that bad programming midi, it's just too cumbersome for me to use. Configuration is a pain, I always needed to have the manual with me, etc... I sold it, and I don't plan on buying a new one.

I have a FS3X footswitch from digitech (3 momentary swtich box), it's 25€, works with a standard stereo jack interface, I know a bit of electronic and worked as a software developer in the past.

After posting the previous message and asking for review on several facebook music nerd/tech geek related groups, the reaction I had motivated me to make this project public. And here I'm, having to refactor that code I though nobody would read :p

# b33rz

- [LoopersDelight](https://www.facebook.com/groups/LoopersDelight/) group members for their interest that led me to release this.
- [Tobias Erichsen](https://www.tobias-erichsen.de/) author of loopmidi
- [Marc Jacobi](https://github.com/obiwanjacobi/vst.net) author of VST.net, allowing the project to be hosted as a VST
- [Romain Caudron](https://www.facebook.com/RCAguitar/) for the borrowed footswitches
