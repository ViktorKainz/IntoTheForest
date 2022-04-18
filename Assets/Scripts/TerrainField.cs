using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TerrainField : MonoBehaviour
{
    public TerrainType type;
    public LevelGeneration level;
    public GameObject figure;
    public int x;
    public int y;

    public static float speed = 250f;
    public static int round = 1; //odd number player green even player red

    private Color[][] _startColors;
    private Animator animator;
    private static TerrainField selected;
    private static List<TerrainField> moveSelected = new List<TerrainField>();

    private IDictionary<FigureType, int> range = new Dictionary<FigureType, int>()
    {
        { FigureType.King, 1 },
        { FigureType.Pawn, 1 },
        { FigureType.Knight, 2 },
        { FigureType.Queen, 3 },
    };

    void Start()
    {
        if (figure != null)
            figure.transform.parent = transform;
    }

    private void Update()
    {
        if (figure != null)
        {
            figure.transform.parent = transform;
            // Animation
            if (animator == null)
            {
                GameObject child = figure.transform.GetChild(0).gameObject;
                animator = child.GetComponent<Animator>();
            }

            if (figure.transform.position != new Vector3(x * level.size.x, 0, y * level.size.z))
            {
                animator.SetBool("Walk", true);
            }
            else
            {
                animator.SetBool("Walk", false);
            }
        }

        if (figure != null)
            figure.transform.position = Vector3.MoveTowards(figure.transform.position,
                new Vector3(x * level.size.x, 0, y * level.size.z), Time.deltaTime * speed);
    }

    private void OnMouseUp()
    {
        if (round == -1) return;
        if (EventSystem.current.IsPointerOverGameObject())
        {
            if (selected == this || type == TerrainType.Castle)
            {
                if (selected != null)
                    selected.UnselectField();
                selected = null;
                return;
            }

            ClearSelection();

            if (selected != null && selected.figure != null && IsMovable(selected))
            {
                var r = range[selected.figure.GetComponent<GameFigure>().type];
                if ((Math.Abs(selected.x - x) <= r && selected.y == y) ||
                    (selected.x == x && Math.Abs(selected.y - y) <= r))
                {
                    var pos = new Vector2(x, y);
                    // Move to empty field
                    if (LevelGeneration.IsFieldEmpty(pos, level.getLevel()))
                    {
                        Debug.Log("Move");
                        MoveFigure(selected);
                        NextRound();
                    }
                    // Attack enemy field
                    else if ((round % 2 != 0 && level.IsFieldEnemy(pos)) ||
                             (round % 2 == 0 && !LevelGeneration.IsFieldEmpty(pos, level.getLevel())))
                    {
                        Debug.Log("Attack");
                        figure.GetComponent<GameFigure>().Kill();
                        MoveFigure(selected);
                        NextRound();
                    }
                    // Can not move to ally field
                    else
                    {
                        Debug.Log("Ally");
                    }
                }
            }

            if (figure != null && IsMovable(this))
            {
                var r = range[figure.GetComponent<GameFigure>().type];
                for (var i = -r; i <= r; i++)
                {
                    if (i == 0) continue;
                    level.GetField(new Vector2(x + i, y))?.SelectMove();
                    level.GetField(new Vector2(x, y + i))?.SelectMove();
                }
            }

            selected = this;
            SelectSuccess();
        }
    }

    private void MoveFigure(TerrainField field)
    {
        figure = field.figure;
        figure.transform.parent = transform;
        if (field.x < x)
            figure.transform.rotation = Quaternion.Euler(0, 90, 0);
        else if (field.x > x)
            figure.transform.rotation = Quaternion.Euler(0, 270, 0);
        else if (field.y < y)
            figure.transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (field.y > y)
            figure.transform.rotation = Quaternion.Euler(0, 180, 0);
        field.figure = null;
    }

    private void NextRound()
    {
        Text t = GameObject.FindWithTag("PlayerText").GetComponent<Text>();
        if (round == -1)
        {
            t.text = "";
            return;
        }

        if (round % 2 == 0)
        {
            t.color = Color.green;
            t.text = "Player green";
        }
        else
        {
            t.color = Color.red;
            t.text = "Player red";
        }

        round++;
    }


    private void SelectSuccess()
    {
        SelectField(Color.yellow);
    }

    private void SelectError()
    {
        SelectField(Color.red);
    }

    public void SelectMove()
    {
        if (type == TerrainType.Castle)
        {
            SelectError();
        }
        else
        {
            SelectField(Color.cyan);
        }

        moveSelected.Add(this);
    }

    private void SelectField(Color color)
    {
        var children = GetComponentsInChildren<Renderer>();
        _startColors = new Color[children.Length][];
        for (var i = 0; i < children.Length; i++)
        {
            _startColors[i] = new Color[children[i].materials.Length];
            for (var j = 0; j < children[i].materials.Length; j++)
            {
                _startColors[i][j] = children[i].materials[j].color;
                children[i].materials[j].color = color;
            }
        }
    }

    private void UnselectField()
    {
        if (_startColors == null) return;
        var children = GetComponentsInChildren<Renderer>();
        for (var i = 0; i < children.Length; i++)
        {
            if (i >= _startColors.Length)
                break;
            for (var j = 0; j < children[i].materials.Length; j++)
                children[i].materials[j].color = _startColors[i][j];
        }
    }

    private bool IsMovable(TerrainField f)
    {
        return f != null && f.figure != null &&
               !((f.figure.GetComponent<GameFigure>().enemy && round % 2 != 0) ||
                 (!f.figure.GetComponent<GameFigure>().enemy && round % 2 == 0));
    }

    public void closeAllCastleMenus()
    {
        Field[,] lvl = level.getLevel();
        foreach (Field field in lvl)
        {
            if (field.plan == level.getTerrain().castle)
            {
                field.obj.GetComponent<SpawnFigure>().setInactive();
            }
        }
    }

    private void ClearSelection()
    {
        if (selected != null)
        {
            selected.UnselectField();
        }

        foreach (var f in moveSelected)
        {
            f.UnselectField();
        }

        moveSelected.Clear();
    }
}

public enum TerrainType
{
    Castle,
    Desert,
    Forest,
    Grass,
    Lake,
    Mountain
}