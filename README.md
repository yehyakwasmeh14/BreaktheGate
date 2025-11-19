# Break the Gate - Unity Third-Person Game

A third-person action game where the player must destroy a gate by collecting explosive fuel while defending against enemy robots.

## 📋 Project Information

- **Unity Version:** 6000.2 (Unity 6)
- **Render Pipeline:** Universal Render Pipeline (URP)
- **Platform:** PC (Windows/Mac/Linux)
- **Input System:** New Input System (com.unity.inputsystem 1.14.2)

## 🎮 How to Run the Project

### Prerequisites
- Unity Hub installed
- Unity 6000.2 or later

### Steps
1. Open Unity Hub

2. Click **"Add"** and navigate to the project folder

3. Select Unity version **6000.2** (or compatible version)

4. Once the project opens, navigate to:
   ```
   Assets/Scenes/SampleScene.unity
   ```

5. Press the **Play** button in the Unity Editor

### Controls
- **WASD** - Move
- **Mouse** - Look around
- **Right Mouse Button** - Aim
- **Left Mouse Button** - Shoot (while aiming) / Click keypad buttons
- **R Key** - Pick up / Drop objects
- **ESC** - Unlock cursor (if needed)

## ✅ Task 1: Player Objective & Level Progression (Third-Person)

### Features Implemented

#### ✅ Player Objective System
- **Objective:** Unlock the warehouse by entering the password in the keypad, collect explosive fuel, and destroy the gate to progress
- **Third-Person Camera:** Cinemachine camera follows player with smooth transitions
- **Objective Tracking:** UI displays current objective and countdown timer (2:20 minutes)

#### ✅ Level Progression
- Gate acts as barrier to next area
- **Puzzle Element:** Warehouse door is locked and requires keypad password
- Player must:
  1. Find and approach the warehouse keypad
  2. Enter the correct password using the keypad interface
  3. Unlock the warehouse door
  4. Enter the warehouse and locate the explosive fuel canister
  5. Pick up the fuel using the pickup system
  6. Carry it out of the warehouse to the designated drop zone
  7. Drop it in the zone to arm the explosive
  8. Shoot the armed explosive to trigger detonation and destroy the gate
  9. Gate destruction unlocks access to the next area

#### ✅ Feedback Systems
- **UI Feedback:**
  - Objective panel showing current task
  - Countdown timer (turns yellow at 1:00, red at 0:30)
  - Keypad interface for password entry
  - Game Over screen on failure
  
- **Audio Feedback:**
  - Keypad button press sounds
  - Door unlock sound
  - Explosion sound when gate is destroyed
  - Pickup/drop sound effects
  - Victory/defeat audio cues

- **Visual Feedback:**
  - Interactive keypad buttons
  - Warehouse door opening animation
  - Explosion VFX when gate is destroyed
  - Gate disappears upon destruction
  - Enemies become hostile after gate destruction

### Scripts
- `GameManager.cs` - Tracks gate destruction and game state
- `UIManager.cs` - Manages UI panels, timer, and game flow
- `ExplosiveFuel.cs` - Handles explosive arming and detonation
- `DropZone.cs` - Detects fuel placement
- `PlayerPickupSystem.cs` - Enables object interaction
- `KeypadInteract.cs` - Handles keypad puzzle interaction (warehouse unlock)

---

## ✅ Task 2: Enemy AI & Player Interaction (Third-Person)

### Features Implemented

#### ✅ Enemy Patrol System
- Enemies wander within a defined radius when player not detected
- NavMesh-based pathfinding for realistic movement
- Smooth transitions between patrol points
- Idle states between waypoints

#### ✅ Detection & Chase
- **Detection Radius:** 15 units (configurable)
- Enemies detect player within radius
- Smooth chase behavior using NavMeshAgent
- Enemies face and track player position
- Returns to patrol when player exits detection range

#### ✅ Attack System
- **Attack Range:** 5 units stopping distance
- Enemies shoot projectiles at player
- Projectile-based damage system
- Line-of-sight checking for realistic behavior

#### ✅ Player Health System
- **UI Health Display:**
  - Floating health bar above player
  - Health bar in HUD
  
- **Health Mechanics:**
  - Takes damage from enemy projectiles
  - Health regeneration starts after 3 seconds of avoiding damage
  - Death triggers Game Over screen
  - Visual and audio feedback on damage

#### ✅ Third-Person Camera
- Cinemachine virtual camera system
- Smooth following with damping
- Adjustable FOV for aiming
- Camera offset for shoulder view when aiming
- Continuous tracking during all actions

### Scripts
- `Robot.cs` - Enemy AI, patrol, detection, and chase
- `RobotShooter.cs` - Enemy shooting behavior
- `EnemyBullet.cs` - Projectile damage and collision
- `EnemyHealth.cs` - Enemy damage and destruction
- `PlayerHealth.cs` - Player health, regeneration, and death
- `PlayerAiming.cs` - Aiming system with camera adjustments

---

## 🎯 Additional Features

### Weapon & Ammo System
- **Shooting Mechanics:**
  - Raycast-based shooting system
  - Muzzle flash VFX
  - Recoil and shooting audio
  - UI ammo counter
  
- **Ammo Pickups:**
  - Collectible ammo boxes scattered in the level
  - Respawn after 30 seconds when collected
  - Audio feedback on pickup

### Additional Scripts
- `WeaponController.cs` - Shooting system
- `PlayerAiming.cs` - Aiming mechanics
- `AmmoPickup.cs` - Ammo pickup and respawn system

---

## ⏱️ Work Time Table

**Total Development Time: 5 days (approximately 60 hours)**

### Day 1: Project Setup & Core Player Systems (12 hours)

| Subtask | Time Spent | Notes |
|---------|-----------|-------|
| Project setup & Unity configuration | 1 hour | URP setup, package installation, project structure |
| Third-person camera system (Cinemachine) | 2 hours | Virtual camera setup, follow behavior, damping |
| Character Controller & movement | 3 hours | Movement system, input handling, camera-relative controls |
| Player animation integration | 2 hours | Animator setup, blend trees, parameter smoothing |
| Aiming system implementation | 2 hours | FOV zoom, shoulder camera offset, aim toggle |
| Initial testing & debugging | 2 hours | Movement tuning, camera adjustments |
| **Day 1 Total** | **12 hours** | |

### Day 2: Combat Systems & Enemy AI Foundation (12 hours)

| Subtask | Time Spent | Notes |
|---------|-----------|-------|
| Weapon shooting system | 2.5 hours | Raycast shooting, muzzle flash, weapon controller |
| Ammo system & pickups | 1.5 hours | Ammo tracking, UI counter, pickup collectibles |
| Player health system | 2 hours | Health tracking, damage handling, regeneration |
| Health UI implementation | 1.5 hours | Health bars, HUD elements, color transitions |
| NavMesh setup & enemy basics | 2 hours | NavMesh baking, enemy GameObject setup |
| Enemy patrol/wander system | 2 hours | Wandering AI, random movement, idle states |
| Testing & debugging | 0.5 hours | Combat testing, balance adjustments |
| **Day 2 Total** | **12 hours** | |

### Day 3: Enemy AI, Detection & Attack Systems (13 hours)

| Subtask | Time Spent | Notes |
|---------|-----------|-------|
| Enemy detection & chase system | 3 hours | Detection radius, player tracking, chase behavior |
| Enemy shooting mechanics | 2.5 hours | Projectile spawning, aiming logic, attack cooldown |
| Enemy bullet system | 2 hours | Bullet movement, collision, damage dealing |
| Enemy health & destruction | 1.5 hours | Health system, death VFX, explosion effects |
| AI state management & polish | 2 hours | Return to patrol, state transitions, behavior tuning |
| Audio integration (combat) | 1 hour | Shooting sounds, explosion audio, spatial sound |
| Testing & debugging AI | 1 hour | AI behavior testing, pathfinding fixes |
| **Day 3 Total** | **13 hours** | |

### Day 4: Objective Systems & UI Implementation (12 hours)

| Subtask | Time Spent | Notes |
|---------|-----------|-------|
| Keypad puzzle system | 2.5 hours | Interactive keypad, password entry, door mechanics |
| Pickup/drop system | 3 hours | Object interaction, holding system, physics handling |
| Explosive fuel mechanics | 2 hours | Arming system, detonation, gate destruction |
| Drop zone implementation | 1 hour | Trigger detection, fuel placement validation |
| Game Manager implementation | 1.5 hours | Singleton pattern, game state tracking, win conditions |
| UI panels (Objective, Timer) | 1 hour | Canvas setup, TextMeshPro, UI layout |
| Testing objective flow | 1 hour | End-to-end gameplay testing |
| **Day 4 Total** | **12 hours** | |

### Day 5: UI/UX Polish, Bug Fixes & Final Testing (11 hours)

| Subtask | Time Spent | Notes |
|---------|-----------|-------|
| Victory/Game Over screens | 1.5 hours | End game panels, transitions, visual design |
| Restart system implementation | 1 hour | Scene reload, button functionality |
| UI/Input system bug fixes | 3 hours | Fixed button click issues, raycast blocking, Input System conflicts |
| Player death & Game Over flow | 1 hour | Death detection, UI triggers, control disabling |
| Cursor management system | 0.5 hours | Lock/unlock cursor, pause menu handling |
| Audio polish & VFX | 1 hour | Footsteps, UI sounds, particle effects |
| Code cleanup & optimization | 1.5 hours | Removing debug logs, code organization, refactoring |
| Final playtesting & balancing | 1.5 hours | Difficulty tuning, timer adjustments, testing all features |
| **Day 5 Total** | **11 hours** | |

---

**GRAND TOTAL: 60 HOURS**
