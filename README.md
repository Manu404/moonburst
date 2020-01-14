# What is MoonBurst ?

![screenshot](https://github.com/Manu404/moonburst/blob/master/scrns/screen.png)

Moonburst is an arduino based solution for musician that aim to provide a generic midi open source interface that accept as much "generic" controller as possible, keeping the cost factor really low, yet versatile and open as much as possible. Some solutions already exists, but not as I envision mine, or at high costs and sometimes long learning curve.

The software is simple : you define functoid, that are binded to a footswitch, than can create multiple actions on that functoid based on footswtich triggers and/or states that will then send midi messages or trigger other functoid, etc,... To add buttons, just add new footswtichs. The idea is to be modular and open as possible.

# Supported Controllers
Full support: Those controlles have been tested and working
Haf support: Those are assumed to be comptabible based on documentation, but not tested by the dev.
If you have those, it would be amazing to send us a message about your expérience and confirm the support opr any issues !

## Currently Supported Controller
### Full Support
 - Digitech FS3X
 
## Coming Supported Controller
### Full Support
 - Generic Switch/Tap on mono interface
 - Generic 2 Switch/Tap on stereo interface
 - Generic Pot (expression pedal) on mono interface (this will enable support of most volume pedal on the market from known manufacturer)
### Half Support  
 - Boss FS6
 - Boss FS5L
 - Boss FS5U
 - Boss FS7

##Planned Supported Controllers (Q2 2020) : 
 - Vox VFS55
 
# About the project

<img align="left" src="https://github.com/Manu404/moonburst/blob/master/scrns/header_half.jpg">

I've been playing with looping and loopers for years, mostly for fun or to help when practicing. Have also experienced a lot of looping in a band context with drummer etc (all the issues that comes with the package). But still mostly for experimenting and fun.

Now, I have a tangible project and personal challenge in a piece I want to play totally alone, for whom I have a solution using software I already know (2 mobius instances hosted into reaper) and that I hope will be able to perform on stage. As an additional challenge, I want no automations or pre-recorded material, etc, ...

Being a guitarist, I need to control most of the software with footswtich and I plan on using a lot of different footswitch (around 10 for the moment). I had a FCB1010 years ago, but despite being not that bad programming midi, it's just too cumbersome for me to use. Configuration is a pain, I always needed to have the manual with me, etc... I sold it, and I don't plan on buying a new one.

I have a FS3X footswitch from digitech (3 momentary swtich box), it's 25€, works with a standard stereo jack interface, I know a bit of electronic and worked as a software developer in the past.

After posting the previous message and asking for review on several facebook music nerd/tech geek related groups, the reaction I had motivated me to make this project public. And here I'm, having to refactor that code I though nobody would read :p
