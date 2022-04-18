using UnityEngine;
using UnityEngine.UI;

public class TerrainField : MonoBehaviour
{
    public TerrainType type;
    public LevelGeneration level;
    public GameObject figure;
    public int x;
    public int y;

    public static float speed = 250f;
    public static int round = 1;        //odd number player green even player red

    private Color[][] _startColors;
    private Animator animator;

    void Start()
    {
        if (figure != null)
            figure.transform.parent = transform;
    }

    private void Update()
    {
        if(figure != null)
        {
            figure.transform.parent = transform;
            // Animation
            if(animator == null) 
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
        if(round == -1) return;
        var selected = level.selected;
        if (selected == this || type == TerrainType.Castle)
        {
            if (selected != null)
                selected.UnselectField();
            level.selected = null;
            return;
        }

        if (selected != null)
        {
            selected.UnselectField();
            if (selected.figure != null && IsMovable(selected) &&
                ((selected.x == x && (selected.y - 1 == y || selected.y + 1 == y)) ||
                 (selected.y == y && (selected.x - 1 == x || selected.x + 1 == x))))
            {
                var pos = new Vector2(x, y);
                // Move to empty field
                if (level.IsFieldEmpty(pos))
                {
                    Debug.Log("Move");
                    MoveFigure(selected);
                    NextRound();
                }
                // Attack enemy field
                else if((round%2 != 0  && level.IsFieldEnemy(pos)) || (round%2 == 0 && !level.IsFieldEmpty(pos)))
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
        level.selected = this;
        if (figure != null && IsMovable(selected))
            SelectSuccess();
        else
            SelectError();
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
            t.color = Color.red;
            t.text = "Player red";
        }
        else
        {
            t.color = Color.green;
            t.text = "Player green";
        }
        round++;
    }

    private void SelectSuccess()
    {
        SelectField(Color.red);
    }

    private void SelectError()
    {
        SelectField(Color.yellow);
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
        for (var j = 0; j < children[i].materials.Length; j++)
            children[i].materials[j].color = _startColors[i][j];
    }

    private bool IsMovable(TerrainField f)
    {
        return f != null && f.figure != null &&
               !((f.figure.GetComponent<GameFigure>().enemy && round % 2 != 0) ||
                 (!f.figure.GetComponent<GameFigure>().enemy && round % 2 == 0));
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