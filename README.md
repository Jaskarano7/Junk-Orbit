https://github.com/user-attachments/assets/2317f65c-9a55-493b-a77e-598b95d1ab5d
# 🪐 Junk Orbit – Space Junk Collector  

_A Unity-based casual arcade game where players pilot a rocket and collect floating space junk while managing their limited cargo capacity. The game blends resource management with fun, fluid controls and a dynamic space environment._  

## ✨ Features  

1. **Player Movement**  
   - Joystick-driven rocket controls with smooth acceleration, speed limits, and rotation.  
   - Thruster particle effects and rocket sound that fade in/out dynamically based on input.  

2. **Space Junk Collection**  
   - Floating and rotating junk objects with random motion.  
   - Junk can only be collected if:  
     - Player has enough **capacity**.  
     - Player’s **level** is high enough.  

3. **Player Stats**  
   - Level, Speed, Acceleration, Capacity, and Points managed in a centralized `PlayerData` system.  
   - Capacity and speed dynamically upgradable in-game.  

4. **Camera System**  
   - Smooth follow camera with positional offset.  
   - Automatic wall fade-out when the line of sight is blocked.  

5. **Environment**  
   - **MeteoridMotion:** Floating and rotating obstacles to decorate and challenge the player.  
   - **BoundaryScript:** Procedurally spawns asteroid walls as visual barriers and physical colliders to keep the player within orbit.  

## 🎮 Gameplay Loop  

1. Fly around the arena using joystick controls.  
2. Collect space junk to earn points and fill your cargo hold.  
3. Manage capacity – return or upgrade when full.  
4. Avoid boundaries and meteoroids while maximizing your haul.  

## 🚀 Next Steps  

- 🛒 **Upgrade System:** Add an in-game shop for capacity, speed, and level upgrades.  
- 🤖 **Enemy AI:** Introduce competing drones that collect junk and create urgency.  
- ⚡ **Power-Ups:** Temporary boosts like magnet (auto-pickup), shield (collision immunity), and turbo (speed boost).  

## 🏗️ Tech Stack  

- **Engine:** Unity  
- **Language:** C#  
- **Input:** Mobile joystick controls  
- **Platform:** Mobile (Android/iOS)  
