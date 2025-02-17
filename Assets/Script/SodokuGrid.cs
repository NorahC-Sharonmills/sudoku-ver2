﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SodokuGrid : MonoBehaviour
{
    public int colums = 0;
    public int rows = 0;
    public float square_offset = 0.0f;
    public GameObject grid_square;
    public Vector2 start_position = new Vector2(0.0f, 0.0f);
    public float square_scale = 1.0f;
    public Color line_highlight_color = Color.red;

    private List<GameObject> grid_squares = new List<GameObject>();
    private int selected_grid_data = 1;

    public float square_gap = 0.1f;

    void Start()
    {
        if (grid_square.GetComponent<GridSquare>() == null)
            Debug.Log("this game object need to have GridSquare script attached !");

        CreateGrid();
        if (GameSettings.Instance.GetContinutePreviousGame())
            SetGirdFromFile();
        else
            SetGridNumber(GameSettings.Instance.GetGameMode());

        AdsManager.Instance.ShowBanner();
    }

    private void SetGirdFromFile()
    {

        string level = GameSettings.Instance.GetGameMode();
        selected_grid_data = Config.ReadGameBoardLevel();
        var data = Config.ReadGridData();

        setGridSquareData(data);
        SetGridNotes(Config.GetGridNotes());
    }

    private void SetGridNotes(Dictionary<int, List<int>> notes)
    {
        notes.ForEach((note) =>
        {
            grid_squares[note.Key].GetComponent<GridSquare>().SetGridNotes(note.Value);
        });
    }

    public void CreateGrid()
    {
        SpawnGridSquare();
        SetSquarePosition();
    }

    private void SpawnGridSquare()
    {
        //0, 1, 2, 3, 4, 5, 6
        //7, 8, 9, 10, 11, 12, 13

        int square_index = 0;

        for(int row = 0; row < rows; row++)
        {
            for(int column = 0; column < colums; column++)
            {
                grid_squares.Add(Instantiate(grid_square) as GameObject);
                grid_squares[grid_squares.Count - 1].GetComponent<GridSquare>().SetSquareIndex(square_index);
                grid_squares[grid_squares.Count - 1].transform.parent = this.transform;
                grid_squares[grid_squares.Count - 1].transform.localScale = new Vector3(square_scale, square_scale, square_scale);
                square_index++;
            }
        }
    }

    private void SetSquarePosition()
    {
        var square_rect = grid_squares[0].GetComponent<RectTransform>();
        Vector2 offset = new Vector2();
        Vector2 square_gap_number = new Vector2(0.0f, 0.0f);
        bool row_moved = false;
        offset.x = square_rect.rect.width * square_rect.transform.localScale.x + square_offset;
        offset.y = square_rect.rect.height * square_rect.transform.localScale.y + square_offset;

        int column_number = 0;
        int row_number = 0;

        grid_squares.ForEach((_square) =>
        {
            if (column_number + 1 > colums)
            {
                row_number++;
                column_number = 0;
                square_gap_number.x = 0;
                row_moved = false;
            }

            var pos_x_offset = offset.x * column_number + (square_gap_number.x * square_gap);
            var pos_y_offset = offset.y * row_number + (square_gap_number.y * square_gap);

            if (column_number > 0 && column_number % 3 == 0)
            {
                square_gap_number.x++;
                pos_x_offset += square_gap;
            }

            if (row_number > 0 && row_number % 3 == 0 && row_moved == false)
            {
                row_moved = true;
                square_gap_number.y++;
                pos_y_offset += square_gap;
            }

            _square.GetComponent<RectTransform>().anchoredPosition = new Vector2(start_position.x + pos_x_offset, start_position.y - pos_y_offset);
            column_number++;
        });
    }

    private void SetGridNumber(string level)
    {
        selected_grid_data = Random.Range(0, SudokuData.Instance.sudoku_game[level].Count);
        var data = SudokuData.Instance.sudoku_game[level][selected_grid_data];
        setGridSquareData(data);
    }

    private void setGridSquareData(SudokuData.SudokuBoardData data)
    {
        for(int i = 0; i < grid_squares.Count; i++)
        {
            grid_squares[i].GetComponent<GridSquare>().SetHasDefaultValue(data.unsolved_data[i] != 0 && data.unsolved_data[i] == data.solved_data[i]);
            grid_squares[i].GetComponent<GridSquare>().SetNumber(data.unsolved_data[i]);
            grid_squares[i].GetComponent<GridSquare>().SetCorrectNumber(data.solved_data[i]);
            //grid_squares[i].GetComponent<GridSquare>().SetHasDefaultValue(data.unsolved_data[i] != 0 && data.unsolved_data[i] == data.solved_data[i]);
        }
    }

    private void OnEnable()
    {
        GameEvents.OnSquareSelected += OnSquareSelected;
        GameEvents.OnUpdateSquareNumber += CheckBoardComplete;
        GameEvents.OnSaveBoadData += OnSaveBoard;
        GameEvents.OnGiveAHint += OnGiveAHint;
    }

    private void OnDisable()
    {
        GameEvents.OnSquareSelected -= OnSquareSelected;
        GameEvents.OnUpdateSquareNumber -= CheckBoardComplete;
        GameEvents.OnSaveBoadData -= OnSaveBoard;
        GameEvents.OnGiveAHint -= OnGiveAHint;
    }

    private void OnGiveAHint()
    {
        var square_indexers = new List<int>();

        for(int i = 0; i < grid_squares.Count; i++)
        {
            var comp = grid_squares[i].GetComponent<GridSquare>();
            if(comp.GetSquareNumber() == 0 && comp.GetHasDefaultValue() == false)
            {
                square_indexers.Add(i);
            }
        }

        //we dont not have any empty square
        if (square_indexers.Count == 0)
            return;

        var random_index = UnityEngine.Random.Range(0, square_indexers.Count);
        var square_index = square_indexers[random_index];
        grid_squares[square_index].GetComponent<GridSquare>().SetCorrectValueOnHint();
        GameEvents.UpdateSquareNumberMethod(grid_squares[square_index].GetComponent<GridSquare>().GetCorrectNumber());
    }

    private void OnSaveBoard()
    {
        //**********************
        var solved_data = SudokuData.Instance.sudoku_game[GameSettings.Instance.GetGameMode()][selected_grid_data].solved_data;
        int[] unsolved_data = new int[81];
        Dictionary<string, List<string>> grid_notes = new Dictionary<string, List<string>>();

        for (int i = 0; i < grid_squares.Count; i++)
        {
            var comp = grid_squares[i].GetComponent<GridSquare>();
            unsolved_data[i] = comp.GetSquareNumber();

            string key = $"square_note:{i}";
            grid_notes.Add(key, comp.GetSquareNote());
        }

        SudokuData.SudokuBoardData curent_game_data = new SudokuData.SudokuBoardData(unsolved_data, solved_data);
        if (GameSettings.Instance.GetExitAfterWon() == false) //do not save data when exit after level complete data
        {
            Config.SaveBoardData(curent_game_data, GameSettings.Instance.GetGameMode(), selected_grid_data,
                Lives.Instance.GetErrorNumber(), grid_notes);
        }
        else
        {
            Config.DeleteDataFile();
        }

        AdsManager.Instance.HideBanner();
        GameSettings.Instance.SetExitAfterWon(false);
    }

    private void SetSquareColor(int[] data, Color color)
    {
        data.ForEach((index) =>
        {
            var comp = grid_squares[index].GetComponent<GridSquare>();
            // cái này check những số nào đã nhập đúng rồi thì không sáng
            //if (comp.HasWrongValue() == false && comp.IsSelected() == false)
            //{
            //    comp.SetSquareColor(color);
            //}
            comp.SetSquareColor(color);
        });
    }

    public void OnSquareSelected(int square_index)
    {
        var horizontal_line = LineIndicator.Instance.GetHorizontalLine(square_index);
        var vertical_line = LineIndicator.Instance.GetVerticalLine(square_index);
        var square = LineIndicator.Instance.GetSquare(square_index);

        if(grid_squares[square_index].GetComponent<GridSquare>().GetHasDefaultValue() == false)
        {
            SetSquareColor(LineIndicator.Instance.GetAllSquareIndexers(), Color.white);
            SetSquareColor(horizontal_line, line_highlight_color);
            SetSquareColor(vertical_line, line_highlight_color);
            SetSquareColor(square, line_highlight_color);
        }
        else
        {
            grid_squares.ForEach((square) =>
            {
                var comp = square.GetComponent<GridSquare>();
                if(comp.HasWrongValue() == false && comp.IsSelected() == false)
                {
                    comp.SetSquareColor(Color.white);
                }
            });
        }
    }

    private void CheckBoardComplete(int number)
    {
        int done = -1;
        for(int i = 0; i < grid_squares.Count; i++)
        {
            done = 1;
            var comp = grid_squares[i].GetComponent<GridSquare>();
            if (comp.IsCorrectNumberSet() == false)
            {
                done = 0;
                break;
            }
        }
        if (done == 1)
            GameEvents.OnBoardCompleteMethod();
    }

    public void SolveSudoku()
    {
        grid_squares.ForEach((square) =>
        {
            var comp = square.GetComponent<GridSquare>();
            comp.SetCorrectNumber();
        });

        CheckBoardComplete(0);
    }
}
