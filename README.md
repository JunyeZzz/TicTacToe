# 🎮 井字棋 - Unity 项目

这是一个使用 **Unity 引擎** 制作的单人井字棋游戏，玩家可以和内置的 AI 对战。项目代码结构清晰，使用了模块化的控制器拆分方式，便于维护与扩展。

---

## 🧩 项目结构说明

- `GameController.cs`：负责整体游戏流程控制、玩家输入、UI 更新。
- `AIController.cs`：负责 AI 的策略选择与胜负判断（包含 MiniMax 与随机两种模式）。
- `PauseMenuController.cs`：负责 ESC 或菜单按钮触发的暂停菜单功能。
- `MainMenu.cs`：主菜单逻辑，包括开始游戏、难度选择、设置音效、退出。
- `Canvas UI`：包含主界面、设置界面、暂停菜单、难度选择、游戏结束界面等 UI 元素。

---

## ✨ 已实现功能

### ✅ 基础玩法
- 支持玩家与 AI 对战。
- 玩家可以选择先手（X）或后手（O）。
- 自动判定胜负和平局，并弹出结算面板。

### ✅ AI 难度选择
- 在主菜单点击「人机对战」后，可选择：
  - **简单模式**：AI 会随机落子，适合初学者。
  - **困难模式**：AI 使用 MiniMax 算法，具备较强策略性。
- 选择后自动保存设置，并进入游戏。

### ✅ 智能 AI
- 使用 **MiniMax 算法** 实现的 AI 对手。
- 首回合优先选择角或中心，提升开局体验。

### ✅ 游戏菜单
- 主菜单：开始游戏、选择难度、打开设置、退出游戏。
- 设置菜单：可调节 **SFX 音效音量**，并自动保存设置。
- 暂停菜单：点击 ESC 或菜单按钮弹出，支持继续游戏、重开、返回主菜单。

### ✅ 用户体验优化
- 所有 UI 支持按钮交互。
- 点击落子带有音效。
- 游戏结束后支持一键重开或退出。

---

## 🛠️ 运行方式

1. 使用 Unity 打开项目。
2. 运行主场景（MainMenu）。
3. 点击「人机对战」，选择 AI 难度。
4. 选择 X 或 O 后进入游戏。

---

## 📁 后续计划

- 增加动画与音效细节提升体验。
- 支持双人本地对战模式。
- 添加存档功能。
- 添加悔棋功能。
- 添加不同棋盘主题切换。

---

## 📁 网络资源
- 音效： Light Switch ON / OFF by FillSoko  
  https://freesound.org/s/257958/  
  License: Creative Commons 0
