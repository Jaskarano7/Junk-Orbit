🪐 Junk Orbit – Space Junk Collector
A Unity-based casual arcade game where players pilot a rocket and collect floating space junk while managing their limited cargo capacity. The game blends resource management with fun, fluid controls and a dynamic space environment.

✨ Features
1) Player Movement - Joystick-driven rocket controls with smooth acceleration, speed limits, and rotation.
                     Thruster particle effects and rocket sound that fade in/out dynamically based on input.
2) Space Junk Collection - Floating and rotating junk objects with random motion.
                           Collectible only if:
                             a) Player has enough capacity.
                             b) Player’s level is high enough.
3) Player Stats - Level, Speed, Acceleration, Capacity, Points managed in a centralized PlayerData script.
                  Capacity and speed dynamically upgradable in-game.
4) Camera System - Smooth follow camera with positional offset.
                   Automatic wall fade-out when the camera line of sight is blocked.
5) Environment - MeteoridMotion : Floating and rotating obstacles to decorate and challenge the player.
                 BoundaryScript : Procedurally spawns asteroid walls as both visual barriers and physical colliders, keeping the player within orbit.
