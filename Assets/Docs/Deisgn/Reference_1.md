## 1. Board 与 Tile
### 1.1 GridManager类
- 负责初始化棋盘网格
- 负责网格的动态管理（重制网格）
- 考虑通过依赖注入（`Grid`、`Tile`）减少依赖关系
- 使用`Grid`提供的接口完成具体操作，同时协调其他系统的交互
- 提供基于游戏规则驱动的高层逻辑（比如规则驱动的批量操作）
- 使用事件总线或观察者模式，以解耦与其他组件的耦合。
### 1.2 Grid类
- 网格数据存储
   - 维护`Tile`的集合
   - 存储网格尺寸、位置映射等信息
   - 边界检测：确保方法传入的坐标在网格范围内
   - 低层次操作（增删、查询Tile、调整网格大小）
   - 提供通用的增删查改接口
   - 不处理复杂的全局逻辑
- [x] 提供`GetTilesInRange(string shape, Vector2Int position)`方法，用于获取指定范围内的所有Tile
   - Point
   - Line
      - Row
      - Column
      - Cross
   - Area
      - Square
      - Diamond
      - Circle
      - Cone
   - Irregular Shapes
      - Can be defined by a 2D array
   - Global
   - Random
### 1.3 Tile类
- Tile的逻辑层：包含Tile的逻辑状态和接口，用于拓展不同功能
- 持有一个实现`TileContent`接口的内容（我们先定义为`Element`）
### 1.4 TileContent接口
- 定义Tile上的内容（可以在之后拓展为`BasicElement`、`SpecialElement`等）
- 各类内容通过实现接口添加新的行为
### 1.5 TileView类
- Tile的表现层：负责Tile的可视化渲染
- 与Tile逻辑层解耦
- `TileView`能够根据`Tile`的逻辑变化，动态地更新。（观察者模式）

## 2. User Interaction
### 1.1 Interaction by Drag Tile
- 玩家可以通过拖曳`Tile`来完成一些操作（比如移动、交换位置）
- 需要处理`Tile`的拖曳、放置、交换等操作
- 需要处理`Tile`的边界检测
- 需要处理`Tile`的交换逻辑
- 需要处理`Tile`的放置逻辑

- [ ] 使用命令模式重构InputManager

---
- [x] 改动：
- `BoardInitializer` 
新的策略模式：
   - 保持随机初始化的基本逻辑，但不再填满整个 Grid。
      - 根据指定的 Tile 数量，随机选择空闲位置，以特定概率生成某种类型的 Tile。
   - 保持随机初始化的基本逻辑，但不再填满整个 Grid。
      - 根据指定的 Tile 数量，随机选择空闲位置，以特定概率生成某种类型的 Tile。
- [x] 问题：
- 初始状态：
   - 在`BoardInitializer`初始化过程中，Grid中的未填满部分不应为`null`，而是应该使用`ItileContent`的`Empty`实例填充，确保逻辑一致性。
- Grid类的逻辑更新
   - 修改`swapTile`方法的逻辑，使其操作对象从`Tile`改为`TileContent`，即`swapTileContents`，专注操作内容，保留`Tile`的物理位置不变
   - 确保交换逻辑针对`ITileContent`实例执行，需要确保`OnContentChanged`同步触发，以保持状态正确性和一致性
- 同步TileView更新
   - 当Grid中的`Content`发生变化时，必须同步更新对应的`TileView`，以确保视觉表现和逻辑状态一致
   - 可以通过观察者模式或事件机制通知`TileView`进行刷新 
- `ItleContent`接口中明确提供一个`IsEmpty`方法，用于判断Tile是否为空
- 确保`GridManager`的操作与`Tile`和`TileView`之间的事件流畅衔接，比如，`GridManager`通知`Tile`内容变更，`Tile`自动通知绑定的`TileView`进行更新
- [x] 改动：
   - 新增`EmptyBoardInitializer`策略，用于初始化空棋盘
   - `BoardInitializer`在Inspector中新增策略选择
- [x] 改动：`content`的内容应该要修改，不应该再是`BasicTileContent`，而是`ElementTileContent`
   - 是不是可以考虑使用`ElementTileContentFactory`来实现？
---
## 2. Dice System
- **目标**： 建立基础的骰子系统，包括骰子的创建，掷骰子以及在棋盘上生成基本与特殊元素
- **设计思路**：
   - 玩家可以有多个骰子，可以选择掷出某个骰子，骰子投出后，根据投出的结果，在棋盘上生成相应的元素
   - 生成的元素数量和类型与两个因素有关
      - 骰子的类型
      - 骰子投出的结果（骰子面）
      - 比如，投掷一颗【火焰】类型的骰子，投掷的结果是这颗骰子的其中一面【火球】，棋盘上对应生成了3个【火焰】元素与1个【火球】元素
      - 火球是特殊元素，火焰是普通元素，因为这颗骰子的等级是1级，所以它只能生成3个火焰元素。


### 2.1 DiceManager类
- 负责骰子的全局管理
- 提供骰子的创建、升级和销毁逻辑
- 提供骰子投掷的接口
- 与其它模块的关系
   - 管理`Dice`对象
   - 调用`DiceRoller`进行投掷
   - 与其它模块协作。
### 2.2 Dice类
- 具体的骰子类，定义骰子的核心属性和方法
- 类型：与基本元素（比如火元素）有关
- 等级：影响生成内容的数量和质量
- 骰子面集合（默认是6个面，但支持未来的拓展）：通过`DiceFace`实现。
- 支持骰子面的动态替换（比如升级或者游戏中的变化）
- 与其他模块的关系：
   - 包含`DiceFace`对象列表
### 2.3 DiceFace类
- 定义骰子单面的信息和行为：
   - 内容：与特殊元素（比如火球）有关
   - 权重：控制随机结果的概率
- 提供工薪方法，支持游戏过程中动态调整骰子面内容
- TODO:未来拓展方向：
   - 支持面与面之间的关联组合效果
   - 加入冷却时间或者动态属性
### 2.4 DiceRoller类
- 专注于投骰逻辑，返回`DiceFace`
- 支持加权随机或其他复杂的随机逻辑
- 与其它模块的关系
   - 依赖`RandomManager`提供随机数
   - 返回的投掷结果供`DiceOutcomeProcessor`处理

### 2.5.1 DiceOutcomeProcessor : IDiceOutcomeRule
- 是DiceOutcome的管理器
   - 负责注册和管理规则 `IDiceOutcomeRule`
   - 并根据骰子类型、骰子等级和`DiceFace`生成元素
      - 注意
      - 这里生成的有两种，它们都会生成，一种是普通元素，一种是特殊元素
      - 普通元素：根据Dice的等级和类型生成
      - 特殊元素：根据Dice的等级、类型和`DiceFace`生成
- 与其它模块的关系：
   - 接受`DiceRoller`的结果
   - 与`TileUpdater`交互
### 2.5.2 IDiceOutcomeRule
- 定义生成普通元素和特殊元素的规则
- 将生成的普通元素和特殊元素一起作为`TileContent`返回。
### 2.6 TileUpdater
- 操作棋盘模块`GridManager`
- 将DiceEffectGenerator提供的内容，更新对应的`Tile`
- 与其它模块的关系
   - 依赖`GridManager`和`Tile`

## 3. Element System
- 将`Dice`中的`ElementTileContent`独立出来，创建一个新的`Element`模块，为后续的拓展做准备，
因为后续的`MatchingSystem`需要根据不同的`Element`来进行匹配处理
   - `IElement`
      - string ID {get;}
      - string Type {get;}
      - string Description {get;}
      - bool Match(IElement other)
   - 设计`IBasicElement : IElement`接口
      - int value {get;}
   - 设计`ISpecialElement : IElement`接口
      - int level {get;}
      - void ActivateSpecialEffect()
   - 设计`IActiveElement`接口
      - void Activate(Context context)
   - 设计`IPassiveElement`接口
      - void Passive(Context context)
   - `Context`类
      - public GameState CurrentState {get; set;}
- 考虑：
   - Active/ Passive是否需要用Event来实现？(专门的`EventBus`?)
- [x] 改动：`ElementTileContentFactory`
   - 加入一个新的方法，既能产生`BasicElementTileContent`，也能产生`SpecialElementTileContent`
   - `BoardInitializer`中的`ContentProbility`的选择依赖于`ElementTileContentFactory`（使用Dropdown）
   - `ElementTileContentFactory`中的`ElementConfig`需要修改
      - 要么是`Basic Prefab`，要么是`Special Prefab`，因为一个`Type`同时不可能既是`Basic`又是`Special`

## 4. Matching System
- 流程: 
- [x] 修正！（不应该是`Tile`，而是`TileContent`）
   - 玩家移动`Tile` 
   - [ ] 问题：如果未来想要拓展，
      - 支持连续撤销
      - 使用`EventQueue`来支持记录？
      - 是不是可以在玩家进行完一系列的操作之后，统一使用按钮来触发`EventQueue`？
   - 首先以移动后的`Tile`为中心，调用TileRangeCalculator得到相邻的`TileContent`，使用`FindConnectedGroups(neighborTilesContent)`方法
      - Connected的定义是，两个`TileContent`相邻，同时类型形同
      - `FindConnectedGroups`方法返回一个`List<List<TileContent>>`，表示所有连接的`TileContent`组
   - 然后`Tile`与得到的`ConnectedGroups`进行匹配（只需要匹配每个组中的第一个元素即可），在这里调用`MatchingDetector : IMatchingRule`的方法
      - 判断`Tile`是否可以匹配
      - 引入`IMatchingRule`接口，用于定义匹配规则
   - 匹配成功后，调用`ResolveHandler : IResolveRule`的方法来处理匹配后的逻辑
      - 结果可以是
         - 消除
         - 生成
         - 激活
         - 移动
- [x] 改动：
   - `MatchingSystem`的Inspector中，加入debug.log的开关
   - 同时加入在`FindConnectedGroups`方法中，加入debug.log（这个也需要加入开关的功能），方便调试，知道每个ConnectedGroup的组成
   - 这个是另一个项目了...以后再说
- [ ] 改动：
   - 
