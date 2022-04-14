using UnityEngine;
using UnityEngine.EventSystems;

public class TerrainField : MonoBehaviour
{
    public TerrainType type;
    public LevelGeneration level;
    public GameObject figure;
    public int x;
    public int y;

    public static float speed = 100f;
    public static int round = 1;

    private Color[][] _startColors;

    void Start()
    {
        if (figure != null)
            figure.transform.parent = transform;
    }

    private void Update()
    {
        if (figure != null)
            figure.transform.position = Vector3.MoveTowards(figure.transform.position,
                new Vector3(x * level.size.x, 0, y * level.size.z), Time.deltaTime * speed);
    }

    private void OnMouseUp()
    {
        var selected = level.selected;
        //Deactivate FigureCanvas from every castle
        if (selected == null)
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
            if (selected.figure != null)
            {
                if (IsMovable(selected))
                {
                    if ((selected.x == x && (selected.y - 1 == y || selected.y + 1 == y)) ||
                        (selected.y == y && (selected.x - 1 == x || selected.x + 1 == x)))
                    {
                        figure = selected.figure;
                        figure.transform.parent = transform;
                        if (selected.x < x)
                            figure.transform.rotation = Quaternion.Euler(0, 90, 0);
                        else if (selected.x > x)
                            figure.transform.rotation = Quaternion.Euler(0, 270, 0);
                        else if (selected.y < y)
                            figure.transform.rotation = Quaternion.Euler(0, 0, 0);
                        else if (selected.y > y)
                            figure.transform.rotation = Quaternion.Euler(0, 180, 0);

                        selected.figure = null;
                        round++;
                    }
                }
            }
        }

        level.selected = this;

        if (figure != null && IsMovable(selected))
            SelectSuccess();
        else
            SelectError();
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