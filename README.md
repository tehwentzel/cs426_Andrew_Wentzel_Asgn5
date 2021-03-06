# cs426_assignment5
### Cat Cache 2
### Andrew Wentzel

#### Description

#### Design Changes
Cat Cache originally was a game in which each player needed to hit different cubes in a certain order dictated by the "program" instructions on a screne.  This update adds additional background features and 2 new features: obstacles (capacitors), and a "garbage collector", the chases users to disrupt them if they haven't hit a block

#### Physics Contructs
 * Garbage Collector:  A sphere in the middle of the map.  Follows game gravity.  If a player has not hit a target cube in at least 3 seconds, it will "turn on", turning red and activating a spot light, and follow the player with the longest period of inactivity.  On collision with a player, a sound plays, the player is thrown away, and their inactivity timer is reset.

  * Capacitors: Blue cylinders in the game.  Follows game gravity but do not move in the x-z plane.  On a collision the cat is zapped: a "zap" sound plays and the cat is thrown up and away from the point of collision.

#### Billboard
Signs with camera tracking
 * A background image in one side with a scene of cool cyberspace like imagery (obtained from shutterstock.com)
 * Some cpu fans (made by Freepik from flaticon.com)

Static Signs
 * A sign warning of the dangers of a touching a capacity

 #### Sound
 * A zap sound is played on a collision with a capacitor cylinder
 * A laser sound is played on a collision between a player and the garbage collector sphere
 * A buzzer sound plays when the player hits an incorrect target cube
 * Polite applause plays on a game win

 #### Lighting
  * The screen with instructions has a new yellow light source 
  * All billboards mentioned have their own collored spotlight for illumination
  * The garbage collector has a red light that shines on the player when it is actively chasing the player.

## Original Writeup

### Executive Summary
Description: Cat Cache is a game where 2-4 players play as cats stuck in a giant computer.  All 4 cats are given instructions about where in the computers memory they need to go in order to follow the program run by the computer.  When the wrong memory is accessed, the computer "glitches" and gravity switches directions.  Cat Cache is run on a computer using unity and is played with just a keyboard.

Audience Analysis: Cat Cache is a short, fun game designed for casual players, and players ages 8-14 who are interested in learning about computers in school.

### Environmental Storytelling
The environment is styles to look like a computer, and instructions look like a computer screen printing out instructions.  Addresses are formatted like real computer memory addresses, and the order is hinted at, but not explicity stated on a billboard that uses "goto " statements to mimic assembly code.  A picture of a cat astronaut hints at the gravity inversion function.

### Formal Elements
Player Interaction Pattern: Multilateral Competition

Objective: Race/Solution

Procedures: 
* Who: Minimum 2 players 
* What: All players are cats moving to different locations in computer memory.
* Where: In a computer (cube)
* How: Players must touch the obstacles in a specific order.  The player will use W and D to accelerate and decelerate, A and D to change direction.

Rules: 4 addresses are listed on a screen that show the targets the player must hit. A second screen shows the order that the targets must be hit in, and the player needs to figure out the correct target by connecting the information on these two screens.  The first player to hit the targets in the correct order wins.  When a correct target is hit, the cube will become green for the player and the player will gain a point. When an incorrest target is hit, it will flash red, and gravity will change directions for all players.

Resources: Player characters and Target cubes.

Conflict: Players compete with each other to hit the targets first.  Players can move the targets and can invert gravity by hitting the wrong target, impeding other's progress.

Boundaries: The game is within a 3-d “cube” that contains random obstacles.

Outcome: The first player to hit the targets in the correct order wins and all targets are deactivated. The winning player then wins and the targets are turned into fish.

Unusual Procedure: Gravity will change directions when a player hits an incorrect target and the targets may respawn in different coordinates


