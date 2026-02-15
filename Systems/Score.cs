using System;
using System.IO;
using System.Linq;

namespace AntsShooter.Systems;

class Score
{
    private static readonly string savingsFilePath;

    static Score()
    {
        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string gameFolderPath = Path.Combine(appDataPath, "Icee");

        if (!Directory.Exists(gameFolderPath))
        {
            Directory.CreateDirectory(gameFolderPath);
        }

        savingsFilePath = Path.Combine(gameFolderPath, "AntsShooter.txt");

        if (!File.Exists(savingsFilePath))
        {
            File.WriteAllText(savingsFilePath, "");
        }
    }

    public static void Save(int score)
    {
        using (StreamWriter writer = File.AppendText(savingsFilePath))
        {
            writer.WriteLine(score);
        }
    }

    public static int[] LoadHighestScores()
    {
        string[] lines = File.ReadAllLines(savingsFilePath);

        if (lines.Length == 0)
            return new int[0];

        int[] scores = new int[lines.Length];

        for (int i = 0; i < lines.Length; i++)
        {
            scores[i] = Convert.ToInt32(lines[i]);
        }

        int[] uniqueScores = scores.Distinct().ToArray();

        for (int i = 0; i < uniqueScores.Length - 1; i++)
        {
            for (int j = i + 1; j < uniqueScores.Length; j++)
            {
                if (uniqueScores[j] > uniqueScores[i])
                {
                    int temp = uniqueScores[i];
                    uniqueScores[i] = uniqueScores[j];
                    uniqueScores[j] = temp;
                }
            }
        }

        int topCount = uniqueScores.Length >= 3 ? 3 : uniqueScores.Length;
        int[] top3 = new int[topCount];

        for (int i = 0; i < topCount; i++)
        {
            top3[i] = uniqueScores[i];
        }

        return top3;
    }
}
