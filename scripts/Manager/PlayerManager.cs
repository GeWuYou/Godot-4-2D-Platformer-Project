using System.Collections.Generic;
using Godot;
using Godot42DPlatformerProject.scripts.Extension;

namespace Godot42DPlatformerProject.scripts.Manager;

public partial class PlayerManager : Node, IRegisterAbleManager
{
    private readonly Dictionary<string, Node2D> _characters = new();
    private Node2D _current;
    public const string CharacterDefault = "NinjaFrog";

    public void Initialize()
    {
        var currentScene = GetTree().CurrentScene;
        var playerRoot = currentScene.GetNode<Node>("PlayerRoot");
        // 自动注册所有子角色
        foreach (var child in playerRoot.GetChildren())
        {
            if (child is not CharacterBody2D node)
            {
                continue;
            }

            _characters[node.Name] = node;
            InitializePlayer(node);
        }

        if (_characters.TryGetValue(CharacterDefault, out var defaultChar))
        {
            _current = defaultChar;
        }

        GD.Print($"[PlayerManager] 注册角色数量: {_characters.Count}");
    }

    /// <summary>
    /// 激活角色，并将其移动到指定出生点
    /// </summary>
    public static void ActivateCharacter(Node2D character, Vector2 spawnPosition)
    {
        if (character == null)
        {
            GD.PrintErr("[PlayerManager] 当前角色为空，无法激活！");
            return;
        }

        character.GlobalPosition = spawnPosition;
        if (!character.IsActive())
        {
            character.SetActive(true);
        }

        AttachCamera(character);
        GD.Print($"[PlayerManager] 激活角色: {character.Name} @ {spawnPosition}");
    }

    /// <summary>
    /// 禁用指定的角色节点，并将其全局位置设为零向量，同时释放与其关联的相机节点。
    /// </summary>
    /// <param name="character">要禁用的角色节点。</param>
    public static void DeactivateCharacter(Node2D character)
    {
        character.SetActive(false);
        character.GlobalPosition = Vector2.Zero;
        character.GetNode<Camera2D>("PlayerCamera")?.QueueFree();
        GD.Print($"[PlayerManager] 禁用角色: {character.Name}");
    }


    /// <summary>
    /// 将角色的相机挂载到角色身上
    /// </summary>
    /// <param name="target"> 角色节点 </param>
    private static void AttachCamera(Node2D target)
    {
        if (target.GetNodeOrNull<Camera2D>("PlayerCamera") != null) return;

        var camera = new Camera2D
        {
            Name = "PlayerCamera",
            PositionSmoothingEnabled = true,
            PositionSmoothingSpeed = 5,
            AnchorMode = Camera2D.AnchorModeEnum.DragCenter,
        };

        target.AddChild(camera);
    }

    private static void InitializePlayer(Node2D player)
    {
        // 禁用
        player.SetActive(false);
        
        if (player is not CharacterBody2D body)
        {
            return;
        }

        body.Velocity = Vector2.Zero;
    }


    public void SetActiveCharacter(string name = CharacterDefault)
    {
        _current?.SetActive(false);
        if (_characters.TryGetValue(name, out var newChar))
        {
            newChar.SetActive(true);
            _current = newChar;

            GD.Print($"[PlayerManager] 激活角色: {name}");
        }
        else
        {
            GD.PrintErr($"[PlayerManager] 未找到角色: {name}");
        }
    }

    /// <summary>
    /// 切换角色（会禁用之前的角色并启用新角色）
    /// </summary>
    public void SetActiveCharacter(string name, Vector2 spawnPosition)
    {
        _current?.SetActive(false);

        if (_characters.TryGetValue(name, out var newChar))
        {
            _current = newChar;
            ActivateCharacter(_current, spawnPosition);
        }
        else
        {
            GD.PrintErr($"[PlayerManager] 未找到角色: {name}");
        }
    }

    public Node2D GetCurrentCharacter() => _current;
}