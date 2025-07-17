<h1 align="center">波音 737-800 驾驶舱仪表三维仿真系统</h1>
<p align="center"><b>Boeing 737-800 Cockpit Instrument 3D Simulation System</b></p>
<p align="center">南昌航空大学 · 软件学院 · 凌云班 · 拼好机组 </p>

<p align="center">
  <img src="images/image3.png" height="30" alt="凌云班">
  <img src="https://img.shields.io/badge/Unity-2022.3_LTS-222222?style=flat-square&logo=unity&logoColor=white" alt="Unity">
  <img src="https://img.shields.io/badge/X--Plane-11-0066CC?style=flat-square" alt="X-Plane">
  <img src="https://img.shields.io/badge/C%23-10.0-239129?style=flat-square&logo=csharp&logoColor=white" alt="C#">
</p>
<p align="center">
基于 Unity + X-Plane 的波音 737-800 驾驶舱和仪表仿真系统<br>
实时仪表 · 物理操控 · 仿真场景 · 天气系统 · 飞行回放
</p>



---

## 项目简介

本项目是南昌航空大学软件学院凌云班大二年级的航空仪表仿真项目，采用 **Unity 2022.3 LTS** 作为 3D 渲染和交互平台，通过 UDP 协议与 **X-Plane 11** 飞行模拟软件实时通信，实现完整的波音 737-800 驾驶舱仿真。

### 项目背景

- **航空产业数字化转型**：以数字孪生、模拟仿真为核心，推动航空产业数字化转型
- **沉浸式航空科普**：通过游戏化形式让公众了解航空知识，提升对航空航天的兴趣
- **国产飞机数字孪生**：为国产飞机运营训练提供模拟器支撑平台

### 技术选型

| 技术 | 用途 |
|------|------|
| **Unity** | 实时 3D 渲染引擎，支持多平台，丰富的交互和物理系统 |
| **X-Plane 11** | 专业级飞行模拟器，高精度气动模型 |
| **C#** | Unity 开发语言，面向对象编程 |
| **UDP 通信** | 实现 Unity 与 X-Plane 的实时数据同步 |

---

## 效果展示

### 驾驶舱全景

<div align="center">
<img src="images/image9.png" width="80%">
<p><i>波音 737-800 驾驶舱全景视图</i></p>
</div>

---

### 仪表显示系统

<table>
<tr>
<td align="center"><img src="images/image12.png" width="300"><br><b>PFD 主飞行显示</b><br>空速 · 高度 · 姿态 · 航向</td>
<td align="center"><img src="images/image13.png" width="300"><br><b>ND 导航显示</b><br>航路 · 距离 · 方位</td>
<td align="center"><img src="images/image14.png" width="300"><br><b>EICAS 发动机指示</b><br>N1 · EGT · FF · 燃油量</td>
</tr>
</table>


---

### 场景与天气系统

<table>
<tr>
<td align="center"><img src="images/image29.png" width="400"><br><b>白天 · 晴朗</b></td>
<td align="center"><img src="images/image30.png" width="400"><br><b>黄昏 · 日落</b></td>
</tr>
</table>

---

### 动态演示

<table>
<tr>
<td align="center">
<img src="images/media8.gif" width="300"><br>
<b>地面场景展示</b>
</td>
<td align="center">
<img src="images/media9.gif" width="300"><br>
<b>飞行、场景与粒子系统展示</b>
</td>
<td align="center">
<img src="images/media4.gif" width="300"><br>
<b>座舱交互与仪表盘展示</b>
</td>
</tr>
<tr>
<td align="center">
<img src="images/media3.gif" width="300"><br>
<b>外部机翼组件动画展示，襟翼扰流板等</b>
</td>
<td align="center">
<img src="images/media11.gif" width="300"><br>
<b>机身外部组件动画</b>
</td>
<td align="center">
<img src="images/media10.gif" width="300"><br>
<b>发动机启动</b>
</td>
</tr>
</table>


---

### 开发环境

<div align="center">
<img src="images/image10.png" width="20%">
<p><i>Unity 编辑器开发环境</i></p>
</div>


---

## 技术架构

```
┌─────────────────────────────────────────────────────────────────┐
│                        X-Plane 11 (飞行物理仿真)                  │
│   • 高精度气动模型    • 天气系统    • 地形渲染    • 飞行物理引擎     │
└───────────────────────────────┬─────────────────────────────────┘
                                │
                          UDP 通信
                       (49001/49009)
                                │
┌───────────────────────────────▼─────────────────────────────────┐
│                        Unity 3D 引擎                            │
│  ┌─────────────┐  ┌─────────────┐  ┌─────────────┐             │
│  │  数据接收层   │  │  数据处理层   │  │  数据发送层   │             │
│  │ DataReceive  │→│ DataCenter  │→│  DataSend    │             │
│  └─────────────┘  └──────┬──────┘  └─────────────┘             │
│                          │                                      │
│  ┌───────────────────────▼───────────────────────┐              │
│  │              核心业务模块                       │              │
│  │  ┌─────────┐  ┌─────────┐  ┌─────────┐       │              │
│  │  │飞机控制  │  │仪表显示  │  │环境渲染  │       │              │
│  │  │Airplane │  │ PFD/ND  │  │ Weather │       │              │
│  │  │Controller│  │ EICAS   │  │  System │       │              │
│  │  └─────────┘  └─────────┘  └─────────┘       │              │
│  └───────────────────────────────────────────────┘              │
└─────────────────────────────────────────────────────────────────┘
```

### 数据流向

1. **X-Plane → Unity**：每帧接收 28 组 × 9 个 float 的飞行数据（空速、高度、姿态、发动机参数等）
2. **Unity 内部处理**：DataCenter 解析分发 → 各模块读取更新
3. **Unity → X-Plane**：将操纵杆/油门输入写入 X-Plane 数据引用

---

## 功能模块

###  飞行仪表系统

| 仪表 | 缩写 | 功能 |
|------|------|------|
| 主飞行显示 | PFD | 空速、高度、姿态、航向、垂直速度、飞行指引 |
| 导航显示 | ND | 航路、距离、方位、气象雷达、航点信息 |
| 发动机指示 | EICAS上 | N1转速、EGT排温、燃油流量 |
| 发动机指示 | EICAS下 | N2转速、滑油压力/温度/油量、振动值 |
| 备用仪表 | Standby | 备用空速、高度、姿态指示 |

###  座舱操控系统

| 操控件 | 交互方式 | 说明 |
|--------|----------|------|
| 操纵杆 | 鼠标拖拽 / 硬件摇杆 | 控制俯仰和滚转 |
| 油门杆 | 鼠标拖拽 / 键盘1/2 | 控制发动机推力 |
| 起落架手柄 | 鼠标点击 / 语音 | 收放起落架 |
| 襟翼手柄 | 鼠标拖拽 | 控制襟翼角度 |
| 配平手轮 | 鼠标拖拽 | 调整配平 |

### 语音控制系统

支持中文语音指令，长按录音按钮说出指令：

| 指令 | 动作 |
|------|------|
| "收起起落架" | 收起起落架 |
| "放下起落架" | 放下起落架 |
| "开门" | 打开最近的舱门 |
| "关门" | 关闭最近的舱门 |

###  环境系统

- **天气变化**：晴天、阴天、雨天、雪天
- **时间系统**：白天、黄昏、夜晚
- **座舱灯光**：可调节亮度（L/K键）

###  视角系统

按 `C` 键切换 4 种预设视角：

1. **驾驶舱视角** - 座舱内第一人称
2. **轨道视角** - 环绕飞机外部观察
3. **远距视角** - 固定位置观察
4. **自由视角** - 自由漫游

---

## 项目结构

```
Assets/
├── AirplaneController.cs      # 飞机位置/旋转控制（WGS84坐标转换）
├── DataCenter.cs              # 全局数据中枢（单例模式）
├── DataReceive.cs             # X-Plane UDP 数据接收
├── DataSend.cs                # 操控指令发送到 X-Plane
├── WorldManager.cs            # 浮动原点系统（解决浮点精度问题）
├── FloatingOrigin.cs          # 浮动原点（备用方案）
├── XPlaneConnectNative.cs     # xplaneConnect DLL 封装
│
├── Scripts/
│   ├── startgame.cs           # 启动流程控制
│   ├── JoystickController.cs  # 操纵杆控制（支持硬件摇杆）
│   ├── First Person Controller.cs  # 第一人称相机控制
│   ├── CameraSwitcher.cs      # 多视角切换
│   ├── CameraOrbit.cs         # 轨道相机
│   ├── VoiceCommand.cs        # 语音指令执行
│   ├── SpeechScript.cs        # 百度语音识别
│   ├── ConnectMenu.cs         # IP连接对话框
│   ├── GrobalMenu.cs          # ESC全局菜单
│   ├── FlyTimeSliderController.cs  # 飞行回放滑块
│   ├── MapController.cs       # 小地图控制
│   ├── Accelerator.cs         # 油门杆拖拽交互
│   │
│   ├── Engine/                # 发动机旋转与烟雾特效
│   ├── FLAPS/                 # 襟翼控制
│   ├── LandingGear/           # 起落架收放
│   ├── CockpitDoor/           # 座舱门控制
│   ├── FrontWheelDoor/        # 前轮舱门
│   ├── FrontWheelRoot/        # 前轮转向
│   ├── Fuselage/              # 机身相关
│   ├── Lighting/              # 座舱灯光控制
│   ├── knob/                  # 旋钮交互
│   ├── leftwheel/             # 左主起落架
│   └── rightwheel/            # 右主起落架
│
├── DoorCode/                  # 乘客门/货舱门控制
├── Panels/                    # 仪表面板预制体（PFD/ND/EICAS/Standby）
├── Cockpit/                   # 座舱3D模型与贴图
├── Models/                    # 飞机3D模型
├── Materials/                 # 材质
├── textures/                  # 贴图
├── Scenario/                  # 机场场景
├── Audio/                     # 音效资源
├── icon/                      # UI图标
├── Plugins/                   # 原生DLL（xplaneConnect.dll）
│
├── UniStorm Weather System/   # 天气系统插件
├── UGUIMiniMap/               # 小地图插件
├── PostProcessing/            # 后处理效果
├── TextMesh Pro/              # 文本渲染
└── Standard Assets/           # Unity标准资源
```

---

## 快速开始

### 环境要求

| 软件 | 版本要求 |
|------|----------|
| Unity | 2022.3.60f1 LTS 或更高 |
| X-Plane | 11（需要正版授权） |
| 操作系统 | Windows 10/11 |

### 安装步骤

**1. 克隆项目**

```bash
git clone https://github.com/cdz-hy/nchu_unity_cockpit.git
```

**2. 打开 Unity 项目**

- 启动 Unity Hub
- 点击 "Open" → 选择项目文件夹
- 等待 Unity 导入资源（首次打开可能需要较长时间）

**3. 配置 X-Plane 连接**

1. 启动 X-Plane 11
2. 进入 `Settings → Net Connections → Generic`
3. 启用 UDP 输入/输出
4. 设置端口为 `49009`
5. 填入连接 X-Plane 的电脑 IP 地址

**4. 运行项目**

1. 在 Unity 中打开 `Assets/Scenes/SampleScene.unity`
2. 点击 Play 运行
3. 等待 60 秒启动动画完成
4. 开始飞行模拟！

---

## 操控说明

### 基础操控

| 按键 | 功能 |
|------|------|
| `鼠标左键` | 拖拽操纵杆/油门杆 |
| `C` | 切换视角（4种预设） |
| `ESC` | 打开/关闭菜单 |
| `R` | 切换地图旋转模式 |
| `L` | 增加座舱灯光亮度 |
| `K` | 减少座舱灯光亮度 |
| `1` | 增加油门 |
| `2` | 减少油门 |

### 自由视角操控

| 按键 | 功能 |
|------|------|
| `W/A/S/D` | 前后左右移动 |
| `空格` | 上升 |
| `Left Shift` | 下降 |
| `鼠标右键双击` | 切换固定视角模式 |
| `鼠标滚轮` | 调整焦距（缩放） |

### 硬件摇杆

支持 Windows 标准游戏手柄，即插即用：
- 摇杆前后 → 控制俯仰（Pitch）
- 摇杆左右 → 控制滚转（Roll）

---

## 关键技术

### 1. 浮动原点系统 (Floating Origin)

解决 Unity 浮点数精度问题，支持超远距离飞行：

```csharp
// 当飞机水平距离超过阈值时，重置世界原点
float distance = new Vector2(player.x, player.z).magnitude;
if (distance > threshold) {
    // 反向移动所有场景物体，使飞机回到原点附近
    ShiftWorldOrigin();
}
```

### 2. WGS84 坐标转换

将 X-Plane 的经纬度数据转换为 Unity 世界坐标：

```csharp
// 基于 WGS84 椭球模型的精确转换
float latRad = centerLat * Mathf.Deg2Rad;
metersPerDegreeLat = 111132.92f - 559.82f * Mathf.Cos(2 * latRad);
metersPerDegreeLon = 111412.84f * Mathf.Cos(latRad);

float dx = -(lon - centerLon) * metersPerDegreeLon;
float dz = -(lat - centerLat) * metersPerDegreeLat;
float dy = (alt - initialAltitude) * feetToMeters;
```

### 3. UDP 实时通信

与 X-Plane 建立双向 UDP 通信：

- **接收**：每帧读取 28 组 × 9 个 float 的飞行数据
- **发送**：将操纵输入写入 X-Plane 数据引用（DREF）

### 4. 事件驱动架构

使用 C# 事件实现模块间解耦：

```csharp
// 数据接收事件
public static event Action<float[]> OnDataReceived;

// 操纵杆输入事件
public static event Action<float[]> joystickControllerRotation;
```
