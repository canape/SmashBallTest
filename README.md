# SmashBallTest

**Smash Ball!**–inspired prototype created as part of **Rovio’s hiring process**.  
Built in **Unity**, the project focuses on gameplay structure, camera work, and clean architectural separation under a limited time constraint.

---

## ⏱ Time Spent

**Total:** ~21 hours  
**Period:** December 18–24

### December 18 (3h)
- Game analysis and initial design decisions (camera setup, overall structure)
- Installed core packages:
  - DoTween  
  - Virtual Joystick  
  - TextMesh Pro
- Created a test court

### December 19 (3h)
- Installed Cinemachine
- Began camera setup and experimentation
- Implemented a basic input system

### December 20 (3h)
- Migrated the project to URP (not part of the original plan)
- Installed Zenject
- Completed the main camera setup

### December 21 (3h)
- Implemented core managers:
  - `HeroManager`
  - `CourtManager`
  - `PlayerManager`

### December 22 (3h)
- Created DI installers
- Connected gameplay logic with managers

### December 23 (3h)
- Implemented UI

### December 24 (3h)
- Added `DialogsManager`
- Finished UI
- Implemented dialog-driven game flow
- Final polish

---

## 🧠 Code & Architecture Overview

The project was designed with **decoupling and extensibility** in mind. I intentionally leaned toward common architectural patterns despite the short time frame.

### Key Principles & Patterns
- **Dependency Injection (Zenject)**
- **MVP-style UI**
- **Factory**
- **Observer / Signals**
- **Separation of Concerns**

### High-Level Structure

- **SmashBallTest** is structured around a lightweight **MVP-style UI layer** and a centralized gameplay controller.
- **Zenject** handles:
  - Dependency injection
  - Lifecycle management
  - Signal-based communication
- **Presenters** react to View events and update the UI based on game state changes.
- The **GamePlayController** drives the match loop:
  - Court setup
  - Hero and opponent state
  - Input handling
  - Camera switching
  - Match phases (via a simple state enum)
- **Heroes** manage:
  - Movement
  - Swing actions
  - Lives
  - Emitting signals on life changes to keep UI synchronized

Overall, the architecture favors **loosely coupled components** (managers, presenters, signals) and **explicit game-state transitions**, making gameplay flow and UI updates easier to reason about and extend.

---

## ⚠ Known Issues & Technical Debt

- **GamePlayController has too many responsibilities**, including:
  - Input handling
  - AI logic
  - Camera control
  - Match flow
  - State transitions

Ideally, this should be split into smaller, focused services such as:
- Input Handler
- AI Controller
- Match State Manager
- Camera Coordinator

I originally planned to introduce a **Finite State Machine (FSM)** to formalize game states, but ran out of time. As a result, the current solution works but is not as clean or scalable as I would prefer.

---

## 📌 Notes

Given more time, my next steps would be:
- Introduce a proper FSM for match flow
- Reduce coupling in `GamePlayController`
- Add automated tests around game state transitions
- Improve AI behavior separation
