# MoonBurst
<p align="center">
  <img src="https://github.com/Manu404/moonburst/blob/master/scrns/header_half.jpg">
</p>

Moonburst is an arduino based solution for musician that aim to provide a generic midi open source interface that accept as much controller as possible, keeping the cost factor really low, yet versatile and open as much as possible. Some solutions already exists, but not as I envision mine, or at high costs and sometimes long learning curve.

The software is simple : you define functoid, that are binded to a footswitch, than can create multiple actions on that functoid based on footswtich triggers and/or states that will then send midi messages or trigger other functoid, etc,... To add buttons, just add new footswtichs. The idea is to be modular and open as possible.

![screenshot](https://github.com/Manu404/moonburst/blob/master/scrns/screen.png)

# About the project

I've been playing with looping and loopers for years, mostly for fun or to help when practicing. Have also experienced a lot of looping in a band context with drummer etc (all the issues that comes with the package). But still mostly for experimenting and fun.

Now, I have a tangible project and personal challenge in a piece I want to play totally alone, for whom I have a solution using software I already know (2 mobius instances hosted into reaper) and that I hope will be able to perform on stage. As an additional challenge, I want no automations or pre-recorded material, etc, ...

Being a guitarist, I need to control most of the software with footswtich and I plan on using a lot of different footswitch (around 10 for the moment). I had a FCB1010 years ago, but despite being not that bad programming midi, it's just too cumbersome for me to use. Configuration is a pain, I always needed to have the manual with me, etc... I sold it, and I don't plan on buying a new one.

I have a FS3X footswitch from digitech (3 momentary swtich box), it's 25€, works with a standard stereo jack interface, I know a bit of electronic and worked as a software developer in the past.

Few days later, here I am, with a generic midi gateway software using an arduino to interface multiple FS3X. I also want the UI to be touch-compatible (so button size etc is important) as I use android client that act as additional monitor, so this client would be displayed on a tablet during live performance. Also need to lock/secure some parts for live performance to avoid some disaster.
