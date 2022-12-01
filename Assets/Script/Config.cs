using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class Config : MonoBehaviour
{
#if UNITY_ANDROID && !UNITY_EDITOR
    private static string dir = Application.persistentDataPath;
#else
    private static string dir = Directory.GetCurrentDirectory();
#endif

    private static string file = @"/board_data.ini";
    private static string path = dir + file;

    public static void DeleteDataFile()
    {
        File.Delete(path);
    }

    public static void SaveBoardData(SudokuData.SudokuBoardData board_data, string level, int board_index,
        int error_number, Dictionary<string, List<string>> grid_notes)
    {
        File.WriteAllText(path, string.Empty);
        StreamWriter writer = new StreamWriter(path, false);
        string current_time_string = $"#time:{Clock.GetCurretTime()}";
        string level_string = $"#level:{level}";
        string error_number_string = $"#errors:{error_number}";
        string board_index_string = $"#board_index:{board_index}";
        string unsolved_string = $"#unsolved:";
        string solved_string = $"#solved:";

        board_data.unsolved_data.ForEach((unsolved) =>
        {
            unsolved_string = unsolved_string.AddString(unsolved.ToString()).AddString(",");
        });

        board_data.solved_data.ForEach((solved) =>
        {
            solved_string = solved_string.AddString(solved.ToString()).AddString(",");
        });

        writer.WriteLine(current_time_string);
        writer.WriteLine(level_string);
        writer.WriteLine(error_number_string);
        writer.WriteLine(board_index_string);
        writer.WriteLine(unsolved_string);
        writer.WriteLine(solved_string);

        // bài 14 15 có vấn đề
        grid_notes.ForEach((square) =>
        {
            string square_string = $"#{square.Key}:";
            bool save = false;
            square.Value.ForEach((note) =>
            {
                if(note != " ")
                {
                    square_string = square_string.AddString(note).AddString(",");
                    save = true;
                }
            });

            if (save)
                writer.WriteLine(square_string);
        });

        writer.Close();

        LogSystem.LogSuccess($"Save data path: {path}");
    }

    public static Dictionary<int, List<int>> GetGridNotes()
    {
        Dictionary<int, List<int>> gird_notes = new Dictionary<int, List<int>>();
        string line;
        StreamReader file = new StreamReader(path);

        while((line = file.ReadLine()) != null)
        {
            string[] word = line.Split(':');
            if(word[0] == "#square_note")
            {
                int square_index = -1;
                List<int> notes = new List<int>();
                int.TryParse(word[1], out square_index);

                string[] substring = Regex.Split(word[2], ",");

                substring.ForEach((note) =>
                {
                    int note_number = -1;
                    int.TryParse(note, out note_number);
                    if (note_number > 0)
                        notes.Add(note_number);
                });

                gird_notes.Add(square_index, notes);
            }
        }

        file.Close();

        return gird_notes;
    }

    public static string ReadBoardLevel()
    {
        string line;
        string level = "";
        StreamReader file = new StreamReader(path);

        while ((line = file.ReadLine()) != null)
        {
            string[] word = line.Split(':');
            if (word[0] == "#level")
            {
                level = word[1];
            }
        }

        file.Close();
        return level;
    }

    public static SudokuData.SudokuBoardData ReadGridData()
    {
        string line;
        StreamReader file = new StreamReader(path);

        int[] unsolverd_data = new int[81];
        int[] solved_data = new int[81];

        int unsolved_index = 0;
        int solved_index = 0;

        while((line = file.ReadLine()) != null)
        {
            string[] word = line.Split(':');
            if(word[0] == "#unsolved")
            {
                string[] substrings = Regex.Split(word[1], ",");

                substrings.ForEach((_value) =>
                {
                    int square_number = -1;
                    if(int.TryParse(_value, out square_number))
                    {
                        unsolverd_data[unsolved_index] = square_number;
                        unsolved_index++;
                    }
                });
            }

            if (word[0] == "#solved")
            {
                string[] substrings = Regex.Split(word[1], ",");

                substrings.ForEach((_value) =>
                {
                    int square_number = -1;
                    if (int.TryParse(_value, out square_number))
                    {
                        solved_data[solved_index] = square_number;
                        solved_index++;
                    }
                });
            }
        }

        file.Close();
        return new SudokuData.SudokuBoardData(unsolverd_data, solved_data);
    }

    public static int ReadGameBoardLevel()
    {
        int level = -1;
        string line;
        StreamReader file = new StreamReader(path);

        while((line = file.ReadLine()) != null)
        {
            string[] word = line.Split(':');
            if(word[0] == "#board_index")
            {
                int.TryParse(word[1], out level);
            }
        }

        file.Close();
        return level;
    }

    public static float ReadGameTime()
    {
        float time = -1;
        string line;
        StreamReader file = new StreamReader(path);

        while ((line = file.ReadLine()) != null)
        {
            string[] word = line.Split(':');
            if (word[0] == "#time")
            {
                float.TryParse(word[1], out time);
            }
        }

        file.Close();
        return time;
    }

    public static int ErrorNumber()
    {
        int error = -1;
        string line;
        StreamReader file = new StreamReader(path);

        while ((line = file.ReadLine()) != null)
        {
            string[] word = line.Split(':');
            if (word[0] == "#errors")
            {
                int.TryParse(word[1], out error);
            }
        }

        file.Close();
        return error;
    }

    public static bool GameDataFileExit()
    {
        return File.Exists(path);
    }
}
