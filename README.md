# Ghost Intern

> **신입 귀신이 되어 인간들을 놀래키고 공포 에너지를 모아 퇴근하는 3D 캐주얼 게임**

<p align="center">
  <img src="docs/gifs/gameplay.gif" width="750">
</p>

---

## Project Overview

**Ghost Intern(귀신인턴)**은 Unity로 제작한 3D 미션형 캐주얼 게임입니다.

플레이어는 귀신 회사의 신입 인턴이 되어 도시를 탐색하고 NPC와 상호작용하며 다양한 미션을 수행합니다. 각 미션을 완료하면 **Fear Energy**를 획득하며, 최종적으로 **300 Energy**를 모아 회사로 복귀하면 하루의 업무가 종료됩니다.

단순한 개별 미니게임 구현에 그치지 않고, **탐색 → NPC 상호작용 → 미니게임 → 보상 → 이벤트 발생 → 엔딩**으로 이어지는 하나의 게임 플레이 흐름을 구현했습니다.

### Project Information

| Category        | Description           |
| --------------- | --------------------- |
| Development     | Individual Project    |
| Genre           | 3D Casual / Mini Game |
| Engine          | Unity                 |
| Language        | C#                    |
| Platform        | PC                    |
| Version Control | Git / GitHub          |

---

## Game Objective

플레이어의 목표는 도시를 돌아다니며 인간들을 놀라게 하고 **Fear Energy 300**을 수집하는 것입니다.

각 NPC와 상호작용하면 서로 다른 미니게임이 시작됩니다.

| Target  | Mission         | Reward |
| ------- | --------------- | -----: |
| Student | Jump Scare      |   +100 |
| Worker  | Power Off       |   +100 |
| Guard   | Hide from Guard |   +100 |

모든 업무를 완료하고 회사로 돌아오면 게임이 종료됩니다.

---

## Gameplay

### Jump Scare

학생 NPC와 상호작용하면 시작되는 **타이밍 기반 미니게임**입니다.

움직이는 포인터를 성공 영역에 맞춰 학생을 놀라게 하면 미션에 성공합니다.

<p align="center">
  <img src="docs/gifs/jump-scare.gif" width="650">
</p>

**Key Features**

* 타이밍 기반 성공/실패 판정
* 제한 시간
* 성공 시 NPC 상태 변화
* 미션 완료 후 Fear Energy 반영

---

### Power Off

직장인 NPC와 상호작용하면 시작되는 **기억력 기반 미니게임**입니다.

화면에 제시되는 전선의 순서를 기억한 뒤 제한 시간 안에 동일한 순서로 입력해야 합니다. 총 3개의 라운드를 통과하면 미션에 성공합니다.

<p align="center">
  <img src="docs/gifs/wire-game.gif" width="650">
</p>

**Key Features**

* 전선 순서 암기 및 입력
* 제한 시간
* 3 Round 진행
* 전선 및 Spark Effect 연출
* 성공 시 Blackout 연출

---

### Guard Mission

학생과 직장인 미션을 완료하면 경비원이 등장하며 새로운 미션이 활성화됩니다.

경비원의 행동을 확인하면서 제한 시간 안에 귀신을 숨기는 **반응형 미니게임**입니다. Round가 진행될수록 허용되는 반응 시간이 감소하여 난이도가 상승합니다.

<p align="center">
  <img src="docs/gifs/guard-game.gif" width="650">
</p>

**Key Features**

* 경비원 상태에 따른 반응형 플레이
* 제한 시간 내 숨기
* Round별 반응 시간 감소
* 단계적인 난이도 상승

---

## Game Flow

```text id="8p9r61"
START
  │
  ▼
Ghost Company
  │
  ▼
Explore City
  │
  ├───────────────┐
  ▼               ▼
Student          Worker
Mission          Mission
 +100             +100
  │               │
  └───────┬───────┘
          ▼
   Guard Appears
          │
          ▼
    Guard Mission
        +100
          │
          ▼
  Fear Energy 300
          │
          ▼
Return to Company
          │
          ▼
         END
```

---

## Key Features

### Mission & Progress System

NPC별로 서로 다른 미션을 구성하고, 각 미션의 완료 결과가 전체 게임 진행도인 **Fear Energy**에 반영되도록 구현했습니다.

각각 독립적으로 동작하는 미니게임을 하나의 게임 진행 시스템으로 연결했습니다.

### Scene Progress Management

메인 월드와 각 미니게임을 별도의 Scene으로 구성했습니다.

Scene 이동 후에도 획득한 Fear Energy와 미션 완료 상태가 유지되도록 게임 진행 데이터를 관리했습니다.

### NPC Interaction

플레이어가 NPC에게 접근하면 상호작용 안내가 나타나고, 입력을 통해 해당 NPC의 미니게임으로 이동할 수 있도록 구현했습니다.

### Progress-based Event

학생과 직장인 미션을 모두 완료하면 경비원 이벤트가 발생하도록 구성했습니다.

플레이어의 진행 상태에 따라 새로운 NPC와 미션이 활성화되는 조건 기반 게임 진행 구조를 구현했습니다.

### Quest UI

현재 수행해야 할 업무와 완료한 업무를 UI에서 확인할 수 있도록 구현했습니다.

---

## Tech Stack

| Category        | Technology            |
| --------------- | --------------------- |
| Game Engine     | Unity                 |
| Language        | C#                    |
| UI              | Unity UI, TextMeshPro |
| Data            | PlayerPrefs           |
| Version Control | Git, GitHub           |

**Unity Features**

* Scene Management
* Rigidbody
* Collider / Trigger
* Coroutine
* PlayerPrefs

---

## Project Structure

```text id="6uq9d8"
Assets/
├── Scripts/
│   ├── Manager/
│   ├── MiniGame/
│   ├── NPC/
│   ├── Player/
│   ├── Spawn/
│   └── UI/
│
├── Scenes/
├── Prefabs/
├── Materials/
├── Audio/
├── Sprites/
└── Model/
```

스크립트를 역할별로 분리하여 게임 진행, 미니게임, NPC, 플레이어, 스폰 및 UI 로직을 관리했습니다.

---

## What I Learned

### Game Flow Design

캐릭터 이동이나 개별 미니게임 구현에서 끝나는 것이 아니라,

**Exploration → NPC Interaction → Mini Game → Reward → Event → Ending**

으로 이어지는 전체 플레이 흐름을 설계하고 구현했습니다.

### Scene Data Management

각 미니게임을 독립적인 Scene으로 구성하면서도 미션 완료 여부와 Fear Energy가 유지되도록 구현하며 **Scene 간 게임 진행 데이터 관리 방식**을 경험했습니다.

### Progress-based Events

특정 미션의 완료 여부에 따라 새로운 NPC와 미션이 활성화되도록 구성하며 **게임 상태를 기반으로 콘텐츠를 제어하는 방식**을 구현했습니다.

---

## Future Improvements

* NPC 이동 및 행동 패턴 다양화
* Guard AI 고도화
* 미니게임 콘텐츠 추가
* UI/UX 개선
* Sound Effect 및 게임 연출 강화
* 게임 진행 데이터 관리 구조 개선

---

## Developer

**yahjwin**

Unity / C# Game Development
