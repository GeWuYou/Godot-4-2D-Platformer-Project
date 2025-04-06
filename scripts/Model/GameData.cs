
using System.Collections.Generic;

namespace Godot42DPlatformerProject.scripts.model;

/// <summary>
/// GameData
/// </summary>
public class GameData
{
    /// <summary>
    /// 分数
    /// </summary>
    public int Score { get; set; }
    /// <summary>
    /// 已解锁的关卡
    /// </summary>
    public List<int> UnlockedLevels { get; set; }
}