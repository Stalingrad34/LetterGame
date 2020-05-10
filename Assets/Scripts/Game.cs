using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    [SerializeField] private GameObject[] _allChars;
    [SerializeField] private Text _heightField;
    [SerializeField] private Text _widthField;

    private GameObject[,] _chars;
    private Vector2[,] _charPositions;
    private Rect _gameField;
    private int _height = 0;
    private int _width = 0;
    private float _timer = 3.0f;

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer < 3.0f)
        {
            MoveChars();
        }
    }

    public void MixButton()
    {
        Shuffle(_charPositions);
        _timer = 0;
    }

    private void MoveChars()
    {
        for (int i = 0; i < _height; i++)
        {
            for (int j = 0; j < _width; j++)
            {
                _chars[j, i].transform.position = Vector2.Lerp(_chars[j, i].transform.position, _charPositions[j, i], 2.0f * Time.deltaTime);
            }
        }
    }

    private void Shuffle(Vector2[,] _charPositions)
    {
        for (int i = 0; i < _height; i++)
        {
            for (int j = 0; j < _width; j++)
            {
                var randomW = UnityEngine.Random.Range(0, _width);
                var randomH = UnityEngine.Random.Range(0, _height);

                var temp = _charPositions[j, i];
                _charPositions[j, i] = _charPositions[randomW, randomH];
                _charPositions[randomW, randomH] = temp;
            }
        }
    }

        public void GenerateButton()
    {
        Clear();

        _height = Check(_heightField.text);
        _width = Check(_widthField.text);

        if (_height > 1 && _width > 1)
        {
            _charPositions = GeneratePositions(_height, _width);
            GenerateChars(_height, _width, _charPositions);            
        }   
    }   

    private void GenerateChars(int height, int width, Vector2[,] _charPositions)
    {      
        _chars = new GameObject[width , height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                _chars[x, y] = Instantiate(_allChars[UnityEngine.Random.Range(0, _allChars.Length)], _charPositions[x, y], Quaternion.identity);

                if (height > 5 || width > 5)
                {
                    float scale = 0;
                    if (height > width)
                    {
                        scale = _gameField.height / height;
                    }
                    else
                    {
                        scale = _gameField.width / width;
                    }

                    _chars[x, y].transform.localScale = new Vector2(scale, scale);
                }
            }
        }
    }

    private Vector2[,] GeneratePositions(int height, int width)
    {
        int h = 0;
        int w = 0;

        if (height > 5 )
        {
            h = 4;
        }
        else
        {
            h = height - 1;
        }

        if (width > 5)
        {
            w = 4;
        }
        else
        {
            w = width - 1;
        }
      
        _gameField = new Rect(0, 0, w, h);
        _gameField.center = Vector2.zero;

        float posY = _gameField.yMax;
        float posX = _gameField.xMin;
        float stepX = _gameField.width / (width - 1);
        float stepY = _gameField.height / (height - 1);

        float posXreset = posX;
        Vector2[,] pos = new Vector2[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                pos[x, y] = new Vector2(posX, posY);
                posX += stepX;
            }
            posX = posXreset;
            posY -= stepY;
        }

        return pos;
    }

    private int Check(string text)
    {
        bool valid = int.TryParse(text, out int value);

        if (valid)
        {
            if (value > 20)
            {
                value = 20;
            }
            return value;
        }
        else
        {         
            return 0;
            Error();
        }
    }

    private void Clear()
    {        
        if (_chars != null)
        {
            foreach (var item in _chars)
            {
                Destroy(item);
            }
        }
    }

    private void Error()
    {
        //Вывод ошибки при неправильном вводе.
    }
}
