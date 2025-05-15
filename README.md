# 🎮 Adaptive Enemy NPC System (Unity - Final Year Project)

This Unity prototype demonstrates an **intelligent, adaptive Enemy NPC** capable of perceiving, reacting to, and learning from player behavior. Designed for academic purposes, the system includes realistic perception mechanics, a finite state machine (FSM), and foundational groundwork for adaptive AI behavior and memory.

---

## 📌 Features Implemented

### ✅ Phase 1: Core Setup & Scene Design
- Created a structured Unity 3D project with organized folders (`Scripts`, `Prefabs`, `UI`, `Audio`, etc.).
- Built an enclosed test arena (corridor/arena) with obstacles and lighting.
- Added placeholder Player and Enemy objects for prototyping.
- Configured Unity NavMesh:
  - Baked surfaces.
  - Added `NavMeshAgent` to the enemy for navigation.

### ✅ Phase 2: Enemy Perception System
- Implemented **Field of View (FoV)** using raycasting and angle detection.
- Integrated **obstacle blocking** using `Physics.Raycast`.
- Created **awareness states**: `Unaware`, `Suspicious`, `Alerted`, `Engaged`.
- Visual feedback for awareness using debug colors and logs.
- Developed **auditory detection** via `SphereCollider` trigger for detecting loud actions (e.g., sprinting).

### ✅ Phase 3: Enemy Finite State Machine (FSM)
- Defined states: `Patrol`, `Chase`, `Attack`, `Search`, `Retreat`.
- Created `EnemyFSM` script using `enum`-based logic for transitions.
- Added patrol system via waypoints.
- Implemented chase logic using NavMeshAgent.
- Built a simple attack mechanic with console damage debug messages.
- Integrated basic **retreat behavior** based on low health threshold.

### ✅ Health Management System
- Reusable `HealthSystem` script for both player and enemy.
- Tracks health, damage, and death logic.
- Prints debug logs on health changes (to be connected to UI).

---

## 🛠 Technologies Used
- **Unity 2021+**
- **C#**
- **NavMesh System**
- **Raycasting**
- **Trigger Colliders**
- **FSM Architecture**

---

## 🚧 Work In Progress

| System                     | Status      | Notes |
|----------------------------|-------------|-------|
| 🎮 Player Combat & Controls | ⚙️ In Progress | Movement, animations, combat input |
| 🧠 Adaptive Behavior        | ⏳ Planned   | Aggression/stealth profiling |
| 🧠 Grudge Memory System     | ⏳ Planned   | Track past tactics and influence behavior |
| ❤️ Health Bar UI            | ⏳ Planned   | UI integration and health feedback |
| 🔊 Voice Feedback           | ⏳ Planned   | Audio lines tied to states/actions |
| 🧾 Game Menu & End Screen   | ⏳ Planned   | Start/Restart/Result/Quit options |


---

## 🎯 Upcoming Milestones
- [ ] Add player movement and attack input.
- [ ] Tie player attacks to enemy health system.
- [ ] Display health bars for both player and enemy.
- [ ] Build adaptive AI behavior and memory profiling.
- [ ] Implement audio cues and voice reactions.
- [ ] Complete menu system with game flow logic.

