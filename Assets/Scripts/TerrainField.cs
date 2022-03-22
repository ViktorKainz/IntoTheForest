using UnityEngine;

public class TerrainField : MonoBehaviour
{
    public TerrainType type;
    public LevelGeneration level;
    public GameObject figure;
    public int x;
    public int y;
    public float speed = 100f;

    private Color[][] _startColors;

    void Start()
    {
        if (figure != null)
        {
            figure.transform.parent = transform;
        }
    }

    private void Update()
    {
        if (figure != null)
        {
            figure.transform.position = Vector3.MoveTowards(figure.transform.position,
                new Vector3(x * level.size.x, 0, y * level.size.z), Time.deltaTime * 1000);
        }
    }

    private void OnMouseUp()
    {
        //Deactivate FigureCanvas from every castle
        if (level.selected == null)
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
        if (type == TerrainType.Castle)
        {
            return;
        }
        var s = level.selected;
        if (s == this)
        {
            UnselectField();
            level.selected = null;
        }
        else
        {
            if (s != null)
            {
                s.UnselectField();
                if (s.figure != null)
                {
                    if (s.figure.GetComponent<GameFigure>().enemy)
                    {
                        if ((s.x == x && (s.y - 1 == y || s.y + 1 == y)) ||
                            (s.y == y && (s.x - 1 == x || s.x + 1 == x)))
                        {
                            figure = s.figure;
                            figure.transform.parent = transform;
                            if (s.x < x)
                            {
                                figure.transform.rotation = Quaternion.Euler(0, 90, 0);
                            }
                            else if (s.x > x)
                            {
                                figure.transform.rotation = Quaternion.Euler(0, 270, 0);
                            }
                            else if (s.y < y)
                            {
                                figure.transform.rotation = Quaternion.Euler(0, 0, 0);
                            }
                            else if (s.y > y)
                            {
                                figure.transform.rotation = Quaternion.Euler(0, 180, 0);
                            }

                            s.figure = null;
                        }
                    }
                }
            }
            level.selected = this;

            if (figure != null && !figure.GetComponent<GameFigure>().enemy)
            {
                SelectSuccess();
            }
            else
            {
                SelectError();
            }
        }
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
        var children = GetComponentsInChildren<Renderer>();
        for (var i = 0; i < children.Length; i++)
        {
            for (var j = 0; j < children[i].materials.Length; j++)
            {
                children[i].materials[j].color = _startColors[i][j];
            }
        }
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